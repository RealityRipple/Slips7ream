Imports System.Runtime.InteropServices
Imports System.IO
Namespace Extraction.COM
  <ComImport(), Guid("23170F69-40C1-278A-0000-000600200000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IArchiveExtractCallback
    Sub SetTotal(total As UInt64)
    Sub SetCompleted(<[In]()> ByRef completeValue As UInt64)
    Sub GetStream(index As UInt32, <MarshalAs(UnmanagedType.[Interface])> ByRef outStream As ISequentialOutStream, askExtractMode As ExtractMode)
    Sub PrepareOperation(extractMode As ExtractMode)
    Sub SetOperationResult(result As OperationResult)
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000600100000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IArchiveOpenCallback
    Sub SetTotal(files As IntPtr, bytes As IntPtr)
    Sub SetCompleted(files As IntPtr, bytes As IntPtr)
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000600300000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IArchiveOpenVolumeCallback
    Sub GetProperty(propID As ItemPropId, ByRef rv As PropVariant)
    <PreserveSig()>
    Function GetStream(<MarshalAs(UnmanagedType.LPWStr)> name As String, ByRef stream As IInStream) As <MarshalAs(UnmanagedType.I4)> Integer
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000600600000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IInArchive
    Sub Open(stream As IInStream, ByRef maxCheckStartPosition As UInt64, <MarshalAs(UnmanagedType.[Interface])> openArchiveCallback As IArchiveOpenCallback)
    Sub Close()
    Function GetNumberOfItems() As UInt32
    Sub GetProperty(index As UInt32, propID As ItemPropId, ByRef value As PropVariant)
    Sub Extract(<MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> indices As UInt32(), numItems As UInt32, testMode As ExtractMode, <MarshalAs(UnmanagedType.[Interface])> extractCallback As IArchiveExtractCallback)
    Sub GetArchiveProperty(propID As ItemPropId, ByRef value As PropVariant)
    Function GetNumberOfProperties() As UInteger
    Sub GetPropertyInfo(index As UInt32, <MarshalAs(UnmanagedType.BStr)> ByRef name As String, ByRef propID As ItemPropId, ByRef varType As UShort)
    Function GetNumberOfArchiveProperties() As UInteger
    Sub GetArchivePropertyInfo(index As UInt32, <MarshalAs(UnmanagedType.BStr)> name As String, ByRef propID As ItemPropId, ByRef varType As UShort)
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000600400000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IInArchiveGetStream
    Function GetStream(index As UInteger) As <MarshalAs(UnmanagedType.[Interface])> ISequentialInStream
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000300030000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IInStream
    Function Read(<Out(), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger) As UInteger
    Function Seek(offset As Long, seekOrigin As UInteger) As ULong
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000000050000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface IProgress
    Sub SetTotal(total As ULong)
    Sub SetCompleted(<[In]()> ByRef completeValue As ULong)
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000300010000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface ISequentialInStream
    Function Read(<Out(), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger) As UInteger
  End Interface
  <ComImport(), Guid("23170F69-40C1-278A-0000-000300020000"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
  Friend Interface ISequentialOutStream
    Function Write(<MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger) As UInteger
  End Interface
  Friend Interface ISevenZipStream
    Inherits IInStream
    Inherits ISequentialInStream
    Inherits ISequentialOutStream
    Inherits ISevenZipCleanupStream
    Inherits IDisposable
  End Interface
  Friend Interface ISevenZipCleanupStream
    Sub SetOK()
  End Interface
  Public Interface IArchiveEntry
    ReadOnly Property CompressedSize() As ULong
    ReadOnly Property Checksum() As UInteger
    ReadOnly Property DateTime() As DateTime
    Property Destination() As System.IO.FileInfo
    ReadOnly Property IsEncrypted() As Boolean
    ReadOnly Property Name() As String
    ReadOnly Property Size() As ULong
    ReadOnly Property Version() As UInteger
    ReadOnly Property Index() As UInteger
  End Interface
  Public Interface IArchiveFile
    Inherits IDisposable
    Inherits IEnumerable(Of IArchiveEntry)
    ReadOnly Property Archive() As System.IO.FileInfo
    ReadOnly Property ItemCount() As Integer
    Sub Extract()
    Sub Open()
    Event ExtractFile As EventHandler(Of ExtractFileEventArgs)
    Event ExtractProgress As EventHandler(Of ExtractProgressEventArgs)
  End Interface
  Public Class ExtractProgressEventArgs
    Inherits EventArgs
    Friend Sub New(aFile As FileInfo, aWritten As Long, aTotal As Long)
      ContinueOperation = True
      File = aFile
      Written = aWritten
      Total = aTotal
    End Sub
    Public Property ContinueOperation() As Boolean
      Get
        Return m_ContinueOperation
      End Get
      Set(value As Boolean)
        m_ContinueOperation = value
      End Set
    End Property
    Private m_ContinueOperation As Boolean
    Public Property File() As FileInfo
      Get
        Return m_File
      End Get
      Private Set(value As FileInfo)
        m_File = value
      End Set
    End Property
    Private m_File As FileInfo
    Public Property Total() As Long
      Get
        Return m_Total
      End Get
      Private Set(value As Long)
        m_Total = value
      End Set
    End Property
    Private m_Total As Long
    Public Property Written() As Long
      Get
        Return m_Written
      End Get
      Private Set(value As Long)
        m_Written = value
      End Set
    End Property
    Private m_Written As Long
  End Class
  Public Class ExtractFileEventArgs
    Inherits EventArgs
    Friend Sub New(aArchive As FileInfo, aItem As IArchiveEntry, aStage As ExtractionStage)
      ContinueOperation = True
      Archive = aArchive
      Item = aItem
      Stage = aStage
    End Sub
    Public Property Archive() As FileInfo
      Get
        Return m_Archive
      End Get
      Private Set(value As FileInfo)
        m_Archive = value
      End Set
    End Property
    Private m_Archive As FileInfo
    Public Property ContinueOperation() As Boolean
      Get
        Return m_ContinueOperation
      End Get
      Set(value As Boolean)
        m_ContinueOperation = value
      End Set
    End Property
    Private m_ContinueOperation As Boolean
    Public Property Item() As IArchiveEntry
      Get
        Return m_Item
      End Get
      Private Set(value As IArchiveEntry)
        m_Item = value
      End Set
    End Property
    Private m_Item As IArchiveEntry
    Public Property Stage() As ExtractionStage
      Get
        Return m_Stage
      End Get
      Private Set(value As ExtractionStage)
        m_Stage = value
      End Set
    End Property
    Private m_Stage As ExtractionStage
  End Class
End Namespace
