Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports Slips7ream.Extraction.COM
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization

Namespace Extraction
  Public NotInheritable Class ArchiveFile
    Implements IArchiveFile
    Implements IArchiveOpenCallback
    Implements IArchiveOpenVolumeCallback
    Implements IProgress
    Implements IDisposable
    Private ReadOnly m_archive As FileInfo
    Private current As FileInfo
    Private c_format As Guid
    Private comment As String
    Public Shared ReadOnly AllFormats As New ReadOnlyCollection(Of ArchiveFormat.KnownSevenZipFormat)(New List(Of ArchiveFormat.KnownSevenZipFormat)() From {
      ArchiveFormat.KnownSevenZipFormat.Udf,
      ArchiveFormat.KnownSevenZipFormat.Cab,
      ArchiveFormat.KnownSevenZipFormat.Zip,
      ArchiveFormat.KnownSevenZipFormat.PE,
      ArchiveFormat.KnownSevenZipFormat.Arj,
      ArchiveFormat.KnownSevenZipFormat.GZip,
      ArchiveFormat.KnownSevenZipFormat.BZip2,
      ArchiveFormat.KnownSevenZipFormat.Chm,
      ArchiveFormat.KnownSevenZipFormat.Compound,
      ArchiveFormat.KnownSevenZipFormat.Cpio,
      ArchiveFormat.KnownSevenZipFormat.Deb,
      ArchiveFormat.KnownSevenZipFormat.Dmg,
      ArchiveFormat.KnownSevenZipFormat.Elf,
      ArchiveFormat.KnownSevenZipFormat.Hfs,
      ArchiveFormat.KnownSevenZipFormat.Iso,
      ArchiveFormat.KnownSevenZipFormat.Lzh,
      ArchiveFormat.KnownSevenZipFormat.Lzma,
      ArchiveFormat.KnownSevenZipFormat.Lzw,
      ArchiveFormat.KnownSevenZipFormat.Mub,
      ArchiveFormat.KnownSevenZipFormat.Mslz,
      ArchiveFormat.KnownSevenZipFormat.Nsis,
      ArchiveFormat.KnownSevenZipFormat.Rar,
      ArchiveFormat.KnownSevenZipFormat.Rpm,
      ArchiveFormat.KnownSevenZipFormat.Split,
      ArchiveFormat.KnownSevenZipFormat.Swf,
      ArchiveFormat.KnownSevenZipFormat.Tar,
      ArchiveFormat.KnownSevenZipFormat.Vhd,
      ArchiveFormat.KnownSevenZipFormat.Wim,
      ArchiveFormat.KnownSevenZipFormat.Xar,
      ArchiveFormat.KnownSevenZipFormat.XZ,
      ArchiveFormat.KnownSevenZipFormat.Z
    })
    Private ReadOnly items As New Dictionary(Of String, IArchiveEntry)
    Private ReadOnly fileStreams As New Dictionary(Of String, SevenZipFileStream)
    Public Sub New(archive As FileInfo)
      If archive Is Nothing Then Throw New ArgumentNullException("archive")
      Me.m_archive = archive
      Me.c_format = Guid.Empty
      If Not archive.Exists Then Throw New FileNotFoundException("Empty List supplied")
      current = archive
    End Sub
    Public Sub New(archive As FileInfo, format As ArchiveFormat.KnownSevenZipFormat)
      If archive Is Nothing Then Throw New ArgumentNullException("archive")
      Me.m_archive = archive
      Me.c_format = ArchiveFormat.GetClassIdFromKnownFormat(format)
      If Not archive.Exists Then Throw New FileNotFoundException("archive")
      current = archive
    End Sub
    Public Event ExtractFile As EventHandler(Of ExtractFileEventArgs) Implements IArchiveFile.ExtractFile
    Public Sub ExtractFileRaiser(owner As Object, args As ExtractFileEventArgs)
      RaiseEvent ExtractFile(owner, args)
    End Sub
    Public Event ExtractProgress As EventHandler(Of ExtractProgressEventArgs) Implements IArchiveFile.ExtractProgress
    Public Sub ExtractProgressRaiser(owner As Object, args As ExtractProgressEventArgs)
      RaiseEvent ExtractProgress(owner, args)
    End Sub
    Public ReadOnly Property Archive() As FileInfo Implements IArchiveFile.Archive
      Get
        Return m_archive
      End Get
    End Property
    Public ReadOnly Property ArchiveComment As String
      Get
        Return comment
      End Get
    End Property
    Public ReadOnly Property ItemCount() As Integer Implements IArchiveFile.ItemCount
      Get
        Return items.Count
      End Get
    End Property
    Public Property Format As ArchiveFormat.KnownSevenZipFormat
      Get
        For Each fmt As ArchiveFormat.KnownSevenZipFormat In AllFormats
          If ArchiveFormat.GetClassIdFromKnownFormat(fmt) = c_format Then Return fmt
        Next
        Return ArchiveFormat.KnownSevenZipFormat.Unknown
      End Get
      Set(value As ArchiveFormat.KnownSevenZipFormat)
        c_format = ArchiveFormat.GetClassIdFromKnownFormat(value)
      End Set
    End Property

    Private Sub CloseStreams()
      For Each s As SevenZipFileStream In fileStreams.Values
        s.Dispose()
      Next
      fileStreams.Clear()
    End Sub
    Private Sub IArchiveOpenVolumeCallback_GetProperty(propID As ItemPropId, ByRef rv As PropVariant) Implements IArchiveOpenVolumeCallback.GetProperty
      Select Case propID
        Case ItemPropId.Name
          rv.type = VarEnum.VT_BSTR
          rv.union.bstrValue = Marshal.StringToBSTR(m_archive.FullName)
          Return
        Case ItemPropId.Size
          rv.type = VarEnum.VT_UI8
          rv.union.ui8Value = CULng(current.Length)
          Return
        Case Else
          Throw New NotImplementedException()
      End Select
    End Sub
    Private Function IArchiveOpenVolumeCallback_GetStream(name As String, ByRef stream As IInStream) As Integer Implements IArchiveOpenVolumeCallback.GetStream
      Dim c = New FileInfo(name)
      If Not c.Exists Then
        stream = Nothing
        Return 1
      End If
      current = c
      If fileStreams.ContainsKey(name) Then
        stream = fileStreams(name)
        stream.Seek(0, 0)
        Return 0
      End If
      Dim fileStream = New SevenZipFileStream(current, FileMode.Open, FileAccess.Read)
      fileStreams(name) = fileStream
      stream = fileStream
      Return 0
    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
      CloseStreams()
    End Sub
    Public Sub Extract() Implements IArchiveFile.Extract
      Try
        Using ar = New Archive(m_archive, Me, c_format)
          Dim indices = New List(Of UInteger)()
          Dim files = New Dictionary(Of UInteger, IArchiveEntry)()
          Dim e = ar.GetNumberOfItems()
          If e > 0 Then
            For i As UInteger = 0 To e - 1
              Dim name = ar.GetProperty(i, ItemPropId.Path).GetString()
              If String.IsNullOrEmpty(name) Then Continue For
              If c_format = ArchiveFormat.GetClassIdFromKnownFormat(ArchiveFormat.KnownSevenZipFormat.Split) Then
                name = Path.GetFileName(name)
              End If
              If Not items.ContainsKey(name) Then
                Continue For
              End If
              Dim entry As IArchiveEntry = items(name)
              If entry.Destination Is Nothing Then
                Continue For
              End If
              indices.Add(i)
              files(i) = entry
            Next
            If indices.Count > 0 Then
              Using callback = New ExtractCallback(Me, files)
                ar.Extract(indices.ToArray(), CUInt(indices.Count), ExtractMode.Extract, callback)
              End Using
              If files IsNot Nothing Then
                For Each File In files
                  Dim newInfo As New IO.FileInfo(File.Value.Destination.FullName)
                  newInfo.CreationTime = File.Value.DateTime
                  newInfo.LastAccessTime = Now
                  newInfo.LastWriteTime = File.Value.DateTime
                Next
              End If
            Else
              Throw New FileNotFoundException("No matched files to extract.")
            End If
          Else
            Throw New FileNotFoundException("No files to extract.")
          End If
          ar.Close()
        End Using
      Finally
        CloseStreams()
      End Try
    End Sub
    Private Function GetIArchiveFileEnumerator() As IEnumerator(Of IArchiveEntry) Implements IArchiveFile.GetEnumerator
      Return items.Values.GetEnumerator()
    End Function
    Private Function GetIEnumerableEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
      Return items.Values.GetEnumerator()
    End Function
    Public Sub Open() Implements IArchiveFile.Open
      Dim opened As Boolean = False
      Try
        Dim formats As New List(Of Guid)
        If Not c_format = Guid.Empty Then
          formats.Add(c_format)
        Else
          For Each f As ArchiveFormat.KnownSevenZipFormat In AllFormats
            formats.Add(ArchiveFormat.GetClassIdFromKnownFormat(f))
          Next
        End If
        For Each f As Guid In formats
          Try
            Using ar = New Archive(m_archive, Me, f)
              Dim minCrypted As IArchiveEntry = Nothing
              Dim minIndex As UInteger = 0
              Dim e = ar.GetNumberOfItems()
              If e < 1 Then Continue For
              For I As UInteger = 0 To e - 1
                If ar.GetProperty(I, ItemPropId.IsDir).GetBool() Then Continue For
                Dim name As String
                If f = ArchiveFormat.GetClassIdFromKnownFormat(ArchiveFormat.KnownSevenZipFormat.Split) Then
                  name = ar.GetProperty(I, ItemPropId.Path).GetString()
                  If String.IsNullOrEmpty(name) Then Continue For
                  name = Path.GetFileName(name)
                Else
                  name = ar.GetProperty(I, ItemPropId.Path).GetString()
                  If String.IsNullOrEmpty(name) Then Continue For
                End If
                Dim size = ar.GetProperty(I, ItemPropId.Size).GetUlong()
                Dim packedSize = ar.GetProperty(I, ItemPropId.PackedSize).GetUlong()
                If packedSize = 0 Then packedSize = size
                Dim isCrypted = ar.GetProperty(I, ItemPropId.Encrypted).GetBool()
                Dim crc = ar.GetProperty(I, ItemPropId.CRC).GetUint()
                Dim tModified = ar.GetProperty(I, ItemPropId.ModificationTime).GetDate
                Dim entry As IArchiveEntry = New ItemInfo(name, crc, isCrypted, size, packedSize, tModified, I)
                items.Add(name, entry)
                If isCrypted AndAlso (minCrypted Is Nothing OrElse minCrypted.CompressedSize > packedSize) Then
                  minCrypted = entry
                  minIndex = I
                End If
              Next
              comment = ar.GetArchiveProperty(ItemPropId.Comment).GetString
              c_format = f
              opened = True
            End Using
            Exit For
          Catch generatedExceptionName As IndexOutOfRangeException
            Throw New ArchiveException("Failed to open archive.")
          End Try
        Next
        If Not opened Then
          If formats.Count > 1 Then
            Throw New ArchiveException("Invalid archive!")
          Else
            Throw New ArchiveException("Failed to open archive as " & Format.ToString & ".")
          End If
        End If
      Finally
        CloseStreams()
      End Try

    End Sub
    Public Sub SetCompleted(ByRef completeValue As ULong) Implements IProgress.SetCompleted
    End Sub
    Public Sub SetCompleted(files As IntPtr, bytes As IntPtr) Implements IArchiveOpenCallback.SetCompleted
    End Sub
    Public Sub SetTotal(total As ULong) Implements IProgress.SetTotal
    End Sub
    Public Sub SetTotal(files As IntPtr, bytes As IntPtr) Implements IArchiveOpenCallback.SetTotal
    End Sub
    Private c_ecount As UInteger = 0
    Public ReadOnly Property ExtractionCount As Integer
      Get
        If c_ecount > 0 Then Return c_ecount
        c_ecount = 0
        For Each item In Me
          If item.Destination IsNot Nothing Then c_ecount += 1
        Next
        Return c_ecount
      End Get
    End Property
    Private Class ExtractCallback
      Implements IArchiveExtractCallback
      Implements IDisposable
      Implements IProgress
      Implements IArchiveOpenVolumeCallback
      Private current As UInteger = 0
      Private ReadOnly files As Dictionary(Of UInteger, IArchiveEntry)
      Private mode As ExtractMode = ExtractMode.Skip
      Private ReadOnly owner As ArchiveFile
      Private stream As ISevenZipStream
      Friend Sub New(aOwner As ArchiveFile, aFiles As Dictionary(Of UInteger, IArchiveEntry))
        owner = aOwner
        files = aFiles
      End Sub
      Private Sub IArchiveOpenVolumeCallback_GetProperty(propID As ItemPropId, ByRef rv As PropVariant) Implements IArchiveOpenVolumeCallback.GetProperty
        TryCast(owner, IArchiveOpenVolumeCallback).GetProperty(propID, rv)
      End Sub
      Private Function IArchiveOpenVolumeCallback_GetStream(name As String, ByRef inStream As IInStream) As Integer Implements IArchiveOpenVolumeCallback.GetStream
        Return TryCast(owner, IArchiveOpenVolumeCallback).GetStream(name, inStream)
      End Function
      Public Sub Dispose() Implements IDisposable.Dispose
        If stream IsNot Nothing Then
          stream.Dispose()
          stream = Nothing
        End If
      End Sub
      Public Sub GetStream(index As UInteger, ByRef outStream As ISequentialOutStream, extractMode__1 As ExtractMode) Implements IArchiveExtractCallback.GetStream
        If Not files.ContainsKey(index) Then
          outStream = Nothing
          mode = ExtractMode.Skip
          Return
        End If
        If extractMode__1 = ExtractMode.Extract Then
          If stream IsNot Nothing Then
            stream.Dispose()
          End If
          current = index
          Dim args = New ExtractFileEventArgs(owner.Archive, files(current), ExtractionStage.Extracting)
          owner.ExtractFileRaiser(owner, args)
          If Not args.ContinueOperation Then
            Throw New IOException("User aborted!")
          End If
          Dim ostream = New SevenZipOutFileStream(files(index).Destination, CLng(files(index).Size))
          AddHandler ostream.ProgressHandler, AddressOf ostream_ProgressHandler
          stream = ostream
        Else
          If stream IsNot Nothing Then
            stream.Dispose()
          End If
          stream = New NullStream()
        End If
        outStream = stream
        mode = extractMode__1
      End Sub
      Private Sub ostream_ProgressHandler(sender As Object, e As ExtractProgressEventArgs)
        owner.ExtractProgressRaiser(sender, e)
      End Sub
      Public Sub PrepareOperation(askExtractMode As ExtractMode) Implements IArchiveExtractCallback.PrepareOperation
      End Sub
      Public Sub SetCompleted(ByRef completeValue As ULong) Implements IArchiveExtractCallback.SetCompleted, IProgress.SetCompleted
      End Sub
      Public Sub SetOperationResult(resultEOperationResult As OperationResult) Implements IArchiveExtractCallback.SetOperationResult
        If mode <> ExtractMode.Skip AndAlso resultEOperationResult <> OperationResult.OK Then
          Throw New IOException(resultEOperationResult.ToString())
        End If
        If stream IsNot Nothing Then
          stream.SetOK()
          stream.Dispose()
          stream = Nothing
        End If
        If mode = ExtractMode.Extract Then
          Dim args = New ExtractFileEventArgs(owner.Archive, files(current), ExtractionStage.Done)
          owner.ExtractFileRaiser(owner, args)
          If Not args.ContinueOperation Then
            Throw New IOException("User aborted!")
          End If
        End If
      End Sub
      Public Sub SetTotal(total As ULong) Implements IArchiveExtractCallback.SetTotal, IProgress.SetTotal
      End Sub
      Friend Class NullStream
        Implements ISevenZipStream
        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
        Public Function Read(data As Byte(), size As UInteger) As UInteger Implements IInStream.Read, ISequentialInStream.Read
          Return 0
        End Function
        Public Function Seek(offset As Long, seekOrigin As UInteger) As ULong Implements ISevenZipStream.Seek
          Return 0
        End Function
        Public Sub SetOK() Implements ISevenZipCleanupStream.SetOK
        End Sub
        Public Function Write(data As Byte(), size As UInteger) As UInteger Implements ISevenZipStream.Write
          Return size
        End Function
      End Class
    End Class
    Friend Class ItemInfo
      Implements IArchiveEntry
      Private ReadOnly m_compressedSize As ULong
      Private ReadOnly crc As UInteger
      Private m_destination As FileInfo = Nothing
      Private ReadOnly isCrypted As Boolean
      Private ReadOnly m_name As String
      Private Shared ReadOnly regPreClean As New Regex("^((?:\\|/)?(?:images|bilder|DH|set|cd(?:a|b|\d+))(?:\\|/))", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
      Private ReadOnly m_size As ULong
      Private ReadOnly m_index As UInteger
      Private ReadOnly m_modified As Date
      Friend Sub New(aName As String, aCrc As UInteger, aIsCrypted As Boolean, aSize As ULong, aCompressedSize As ULong, aModified As Date, index As UInteger)
        m_name = aName
        While True
          Dim m = regPreClean.Match(m_name)
          If Not m.Success Then
            Exit While
          End If
          m_name = m_name.Substring(m.Value.Length)
        End While
        While m_name.Length <> 0 AndAlso m_name(0) = Path.DirectorySeparatorChar OrElse m_name(0) = Path.AltDirectorySeparatorChar
          m_name = m_name.Substring(1)
        End While
        m_name = CleanFileName(m_name)
        crc = aCrc
        isCrypted = aIsCrypted
        m_size = aSize
        m_compressedSize = aCompressedSize
        m_index = index
        m_modified = aModified 
      End Sub
      Public ReadOnly Property CompressedSize() As ULong Implements IArchiveEntry.CompressedSize
        Get
          Return m_compressedSize
        End Get
      End Property
      Public ReadOnly Property Checksum() As UInteger Implements IArchiveEntry.Checksum
        Get
          Return crc
        End Get
      End Property
      Public ReadOnly Property DateTime() As DateTime Implements IArchiveEntry.DateTime
        Get
          If m_modified.Year = 1970 Then
            Return New Date(1970, 1, 1)
          Else
            Return m_modified
          End If
        End Get
      End Property
      Public Property Destination() As FileInfo Implements IArchiveEntry.Destination
        Get
          Return m_destination
        End Get
        Set(value As FileInfo)
          m_destination = value
        End Set
      End Property
      Public ReadOnly Property IsEncrypted() As Boolean Implements IArchiveEntry.IsEncrypted
        Get
          Return isCrypted
        End Get
      End Property
      Public ReadOnly Property Name() As String Implements IArchiveEntry.Name
        Get
          Return m_name
        End Get
      End Property
      Public ReadOnly Property Size() As ULong Implements IArchiveEntry.Size
        Get
          Return m_size
        End Get
      End Property
      Public ReadOnly Property Version() As UInteger Implements IArchiveEntry.Version
        Get
          Throw New NotImplementedException()
        End Get
      End Property
      Public ReadOnly Property Index As UInteger Implements IArchiveEntry.Index
        Get
          Return m_index
        End Get
      End Property
      Private Function CleanFileName(fileName As String) As String
        If fileName Is Nothing Then
          Throw New ArgumentNullException("fileName")
        End If
        Dim result = New StringBuilder()
        Dim i As Integer = 0, e As Integer = fileName.Length
        While i < e
          Dim ch = fileName(i)
          Dim ah = Asc(ch)
          If ah < 32 OrElse ah = 34 OrElse ah = 60 OrElse ah = 62 OrElse ah = 63 OrElse ah = 127 OrElse ah = &H2A OrElse ah = &H3F Then
            If i = e - 1 Then
              Exit While
            End If
            result.Append("_"c)
            Continue While
          End If
          result.Append(ch)
          i += 1
        End While
        fileName = result.ToString().Trim()
        Return fileName.Replace("/"c, "\"c)
      End Function
    End Class
  End Class
  <Serializable()>
  Public Class ArchiveException
    Inherits Exception
    Protected Sub New(info As SerializationInfo, context As StreamingContext)
      MyBase.New(info, context)
    End Sub
    Public Sub New()
      MyBase.New("Archive cannot be opened")
    End Sub
    Public Sub New(message As String)
      MyBase.New(message)
    End Sub
    Public Sub New(message As String, innerException As Exception)
      MyBase.New(message, innerException)
    End Sub
  End Class
End Namespace
