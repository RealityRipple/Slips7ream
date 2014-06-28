Imports System.IO
Imports Slips7ream.Extraction.COM
Namespace Extraction
  Friend Class SevenZipFileStream
    Implements ISevenZipStream
    Protected Const BUFFER_SIZE As Integer = 8192
    Protected file As FileInfo
    Protected stream As FileStream
    Protected Sub New(file As FileInfo, mode As FileMode, access As FileAccess, buffer_size As Integer)
      Init(file, mode, access, buffer_size)
    End Sub
    Public Sub New(file As FileInfo, mode As FileMode, access As FileAccess)
      Init(file, mode, access, BUFFER_SIZE)
    End Sub
    Private Sub Init(aFile As FileInfo, mode As FileMode, access As FileAccess, buffering As Integer)
      Dim opts = FileOptions.SequentialScan
      If access <> FileAccess.Read Then
        opts = opts Or FileOptions.WriteThrough
      End If
      If Not aFile.Directory.Exists AndAlso access <> FileAccess.Read Then
        aFile.Directory.Create()
      End If
      stream = New FileStream(aFile.FullName, mode, access, FileShare.Read, buffering, opts)
      file = aFile
    End Sub
    Public Overridable Sub Dispose() Implements IDisposable.Dispose
      If stream IsNot Nothing Then
        stream.Close()
        stream.Dispose()
        stream = Nothing
      End If
    End Sub
    Public Function Read(data As Byte(), size As UInteger) As UInteger Implements IInStream.Read, ISequentialInStream.Read
      Return CUInt(stream.Read(data, 0, CInt(size)))
    End Function
    Public Function Seek(offset As Long, seekOrigin As UInteger) As ULong Implements IInStream.Seek
      Return CULng(stream.Seek(offset, CType(seekOrigin, SeekOrigin)))
    End Function
    Public Overridable Sub SetOK() Implements ISevenZipStream.SetOK
    End Sub
    Public Overridable Function Write(data As Byte(), size As UInteger) As UInteger Implements ISequentialOutStream.Write
      stream.Write(data, 0, CInt(size))
      Return size
    End Function
  End Class
  Friend Class SevenZipOutFileStream
    Inherits SevenZipFileStream
    Protected Const FLUSH_SIZE As UInteger = 1 << 24
    Protected Const WRITE_BUFFER_SIZE As Integer = 1 << 20
    Private ReadOnly fileSize As Long
    Private uFlush As UInteger = FLUSH_SIZE
    Private ok As Boolean = False
    Private progress As UInteger = BUFFER_SIZE
    Public Sub New(aFile As FileInfo, aSize As Long)
      MyBase.New(aFile, FileMode.Create, FileAccess.ReadWrite, WRITE_BUFFER_SIZE)
      file = aFile
      fileSize = aSize
      If fileSize > 0 Then
        stream.SetLength(fileSize)
        stream.Flush()
      End If
    End Sub
    Public Event ProgressHandler As EventHandler(Of ExtractProgressEventArgs)
    Public Property Written() As Long
      Get
        Return m_Written
      End Get
      Private Set(value As Long)
        m_Written = value
      End Set
    End Property
    Private m_Written As Long
    Private Sub Flush(written As UInteger)
      uFlush -= Math.Min(written, uFlush)
      If uFlush = 0 Then
        stream.Flush()
        uFlush = FLUSH_SIZE
      End If
    End Sub
    Public Overrides Sub Dispose()
      MyBase.Dispose()
      If Not ok Then
        Try
          file.Delete()
        Catch generatedExceptionName As Exception
        End Try
      End If
    End Sub
    Public Overrides Sub SetOK()
      ok = True
    End Sub
    Public Overrides Function Write(data As Byte(), size As UInteger) As UInteger
      Dim rv = MyBase.Write(data, size)
      Flush(rv)
      Written += rv
      progress -= Math.Min(rv, progress)
      If progress = 0 Then
        Dim eventArgs = New ExtractProgressEventArgs(file, Written, fileSize)
        RaiseEvent ProgressHandler(Me, eventArgs)
        If Not eventArgs.ContinueOperation Then
          Throw New IOException("User canceled")
        End If
        progress = BUFFER_SIZE
      End If
      Return rv
    End Function
  End Class
End Namespace
