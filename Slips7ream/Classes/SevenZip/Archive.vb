Imports System.IO
Imports System.Runtime.InteropServices
Imports Slips7ream.Extraction.COM
Namespace Extraction
  Friend Class Archive
    Implements IDisposable
    Implements IInArchive
    Private inArchive As IInArchive = Nothing
    Private stream As IInStream
    Private snM As SafeNativeMethods
    Friend Sub New(aFile As FileInfo, callback As IArchiveOpenCallback, format As Guid)
      stream = New SevenZipFileStream(aFile, FileMode.Open, FileAccess.Read)
      InternalOpen(callback, format)
    End Sub
    Private Sub InternalOpen(callback As IArchiveOpenCallback, format As Guid)
      Dim [Interface] = GetType(IInArchive).GUID
      Dim result As Object = Nothing
      snM = New SafeNativeMethods(AIKDir & "7z.dll")
      result = snM.CreateInArchive(format)
      If result Is Nothing Then
        Throw New COMException("Cannot create Archive")
      End If
      inArchive = TryCast(result, IInArchive)
      Dim sp = CULng(1 << 23)
      inArchive.Open(stream, sp, callback)
    End Sub
    Public Sub Close() Implements IInArchive.Close
      If inArchive IsNot Nothing Then
        inArchive.Close()
        Runtime.InteropServices.Marshal.ReleaseComObject(inArchive)
        inArchive = Nothing
      End If
      If stream IsNot Nothing AndAlso TypeOf stream Is IDisposable Then
        DirectCast(stream, IDisposable).Dispose()
      End If
      stream = Nothing
      If snM IsNot Nothing Then
        snM.Dispose()
        snM = Nothing
      End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Close()
    End Sub
    Public Sub Extract(indices As UInt32(), numItems As UInt32, testMode As ExtractMode, extractCallback As IArchiveExtractCallback) Implements IInArchive.Extract
      inArchive.Extract(indices, numItems, testMode, extractCallback)
    End Sub
    Public Function GetArchiveProperty(propId As ItemPropId) As [Variant]
      Return New [Variant](Me, propId)
    End Function
    Public Sub GetArchiveProperty(propID As ItemPropId, ByRef value As PropVariant) Implements IInArchive.GetArchiveProperty
      inArchive.GetArchiveProperty(propID, value)
    End Sub
    Public Sub GetArchivePropertyInfo(index As UInt32, name As String, ByRef propID As ItemPropId, ByRef varType As UShort) Implements IInArchive.GetArchivePropertyInfo
      inArchive.GetArchivePropertyInfo(index, name, propID, varType)
    End Sub
    Public Function GetNumberOfArchiveProperties() As UInteger Implements IInArchive.GetNumberOfArchiveProperties
      Return inArchive.GetNumberOfArchiveProperties()
    End Function
    Public Function GetNumberOfItems() As UInteger Implements IInArchive.GetNumberOfItems
      Return inArchive.GetNumberOfItems()
    End Function
    Public Function GetNumberOfProperties() As UInteger Implements IInArchive.GetNumberOfProperties
      Return inArchive.GetNumberOfProperties()
    End Function
    Public Function GetProperty(Index As UInt32, propId As ItemPropId) As [Variant]
      Return New [Variant](Me, Index, propId)
    End Function
    Public Sub GetProperty(index As UInt32, propID As ItemPropId, ByRef value As PropVariant) Implements IInArchive.GetProperty
      inArchive.GetProperty(index, propID, value)
    End Sub
    Public Sub GetPropertyInfo(index As UInt32, ByRef name As String, ByRef propID As ItemPropId, ByRef varType As UShort) Implements IInArchive.GetPropertyInfo
      inArchive.GetPropertyInfo(index, name, propID, varType)
    End Sub
    Public Function GetStream(index As UInt32) As ISequentialInStream
      Return TryCast(inArchive, IInArchiveGetStream).GetStream(index)
    End Function
    Public Sub Open(s As IInStream, ByRef maxCheckStartPosition As UInt64, openArchiveCallback As IArchiveOpenCallback) Implements IInArchive.Open
      Throw New NotImplementedException()
    End Sub
  End Class
End Namespace