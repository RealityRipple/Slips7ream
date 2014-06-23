Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading
<StructLayout(LayoutKind.Explicit, Size:=16)>
Public Structure PropVariant
  <DllImport("ole32.dll")>
  Private Shared Function PropVariantClear(ByRef pvar As PropVariant) As Integer
  End Function
  <FieldOffset(0)>
  Public vt As UShort
  <FieldOffset(8)>
  Public pointerValue As IntPtr
  <FieldOffset(8)>
  Public byteValue As Byte
  <FieldOffset(8)>
  Public longValue As Long
  <FieldOffset(8)>
  Public filetime As System.Runtime.InteropServices.ComTypes.FILETIME
  Public ReadOnly Property VarType() As VarEnum
    Get
      Return CType(vt, VarEnum)
    End Get
  End Property
  Public Sub Clear()
    Select Case VarType
      Case VarEnum.VT_EMPTY
        Exit Select
      Case VarEnum.VT_NULL, VarEnum.VT_I2, VarEnum.VT_I4, VarEnum.VT_R4, VarEnum.VT_R8, VarEnum.VT_CY, _
       VarEnum.VT_DATE, VarEnum.VT_ERROR, VarEnum.VT_BOOL, VarEnum.VT_I1, VarEnum.VT_UI1, VarEnum.VT_UI2, _
       VarEnum.VT_UI4, VarEnum.VT_I8, VarEnum.VT_UI8, VarEnum.VT_INT, VarEnum.VT_UINT, VarEnum.VT_HRESULT, _
       VarEnum.VT_FILETIME
        vt = 0
        Exit Select
      Case Else
        PropVariantClear(Me)
        Exit Select
    End Select
  End Sub
  Public Function GetObject() As Object
    Select Case VarType
      Case VarEnum.VT_EMPTY
        Return Nothing
      Case VarEnum.VT_FILETIME
        Return DateTime.FromFileTime(longValue)
      Case Else
        Dim PropHandle As GCHandle = GCHandle.Alloc(Me, GCHandleType.Pinned)
        Try
          Return Marshal.GetObjectForNativeVariant(PropHandle.AddrOfPinnedObject())
        Finally
          PropHandle.Free()
        End Try
    End Select
  End Function
End Structure
<ComImport()> _
<Guid("23170F69-40C1-278A-0000-000000050000")> _
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IProgress
  Sub SetTotal(total As ULong)
  Sub SetCompleted(<[In]()> ByRef completeValue As ULong)
End Interface
<ComImport()> _
<Guid("23170F69-40C1-278A-0000-000600100000")> _
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IArchiveOpenCallback
  Sub SetTotal(files As IntPtr, bytes As IntPtr)
  Sub SetCompleted(files As IntPtr, bytes As IntPtr)
End Interface
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000500100000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface ICryptoGetTextPassword
  <PreserveSig()>
  Function CryptoGetTextPassword(<MarshalAs(UnmanagedType.BStr)> ByRef password As String) As Integer
End Interface
Public Enum AskMode As Integer
  kExtract = 0
  kTest
  kSkip
End Enum
Public Enum OperationResult As Integer
  kOK = 0
  kUnSupportedMethod
  kDataError
  kCRCError
End Enum
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000600200000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IArchiveExtractCallback
  Sub SetTotal(total As ULong)
  Sub SetCompleted(<[In]()> ByRef completeValue As ULong)
  <PreserveSig()>
  Function GetStream(index As UInteger, <MarshalAs(UnmanagedType.[Interface])> ByRef outStream As ISequentialOutStream, askExtractMode As AskMode) As Integer
  Sub PrepareOperation(askExtractMode As AskMode)
  Sub SetOperationResult(resultEOperationResult As OperationResult)
End Interface
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000600300000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IArchiveOpenVolumeCallback
  Sub GetProperty(propID As ItemPropId, value As IntPtr)
  <PreserveSig()>
  Function GetStream(<MarshalAs(UnmanagedType.LPWStr)> name As String, <MarshalAs(UnmanagedType.[Interface])> ByRef inStream As IInStream) As Integer
End Interface
<ComImport()> _
<Guid("23170F69-40C1-278A-0000-000600400000")> _
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IInArchiveGetStream
  Function GetStream(index As UInteger) As <MarshalAs(UnmanagedType.[Interface])> ISequentialInStream
End Interface
<ComImport()> _
<Guid("23170F69-40C1-278A-0000-000300010000")> _
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface ISequentialInStream
  Function Read(<Out(), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger) As UInteger
End Interface
<ComImport()> _
<Guid("23170F69-40C1-278A-0000-000300020000")> _
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface ISequentialOutStream
  <PreserveSig()> _
  Function Write(<[In](), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger, processedSize As IntPtr) As Integer
End Interface
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000300030000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IInStream
  Function Read(<Out(), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger) As UInteger
  Sub Seek(offset As Long, seekOrigin As UInteger, newPosition As IntPtr)
End Interface
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000300040000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IOutStream
  <PreserveSig()>
  Function Write(<[In](), MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> data As Byte(), size As UInteger, processedSize As IntPtr) As Integer
  Sub Seek(offset As Long, seekOrigin As UInteger, newPosition As IntPtr)
  <PreserveSig()>
  Function SetSize(newSize As Long) As Integer
End Interface
Public Enum ItemPropId As UInteger
  kpidNoProperty = 0
  kpidHandlerItemIndex = 2
  kpidPath
  kpidName
  kpidExtension
  kpidIsFolder
  kpidSize
  kpidPackedSize
  kpidAttributes
  kpidCreationTime
  kpidLastAccessTime
  kpidLastWriteTime
  kpidSolid
  kpidCommented
  kpidEncrypted
  kpidSplitBefore
  kpidSplitAfter
  kpidDictionarySize
  kpidCRC
  kpidType
  kpidIsAnti
  kpidMethod
  kpidHostOS
  kpidFileSystem
  kpidUser
  kpidGroup
  kpidBlock
  kpidComment
  kpidPosition
  kpidPrefix
  kpidTotalSize = &H1100
  kpidFreeSpace
  kpidClusterSize
  kpidVolumeName
  kpidLocalName = &H1200
  kpidProvider
  kpidUserDefined = &H10000
End Enum
<ComImport()>
<Guid("23170F69-40C1-278A-0000-000600600000")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IInArchive
  <PreserveSig()>
  Function Open(stream As IInStream, <[In]()> ByRef maxCheckStartPosition As ULong, <MarshalAs(UnmanagedType.[Interface])> openArchiveCallback As IArchiveOpenCallback) As Integer
  Sub Close()
  Function GetNumberOfItems() As UInteger
  Sub GetProperty(index As UInteger, propID As ItemPropId, ByRef value As PropVariant)
  <PreserveSig()>
  Function Extract(<MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=1)> indices As UInteger(), numItems As UInteger, testMode As Integer, <MarshalAs(UnmanagedType.[Interface])> extractCallback As IArchiveExtractCallback) As Integer
  Sub GetArchiveProperty(propID As UInteger, ByRef value As PropVariant)
  Function GetNumberOfProperties() As UInteger
  Sub GetPropertyInfo(index As UInteger, <MarshalAs(UnmanagedType.BStr)> ByRef name As String, ByRef propID As ItemPropId, ByRef varType As UShort)
  Function GetNumberOfArchiveProperties() As UInteger
  Sub GetArchivePropertyInfo(index As UInteger, <MarshalAs(UnmanagedType.BStr)> name As String, ByRef propID As UInteger, ByRef varType As UShort)
End Interface
Public Enum ArchivePropId As UInteger
  kName = 0
  kClassID
  kExtension
  kAddExtension
  kUpdate
  kKeepName
  kStartSignature
  kFinishSignature
  kAssociate
End Enum
<UnmanagedFunctionPointer(CallingConvention.StdCall)>
Public Delegate Function CreateObjectDelegate(<[In]()> ByRef classID As Guid, <[In]()> ByRef interfaceID As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef outObject As Object) As Integer
<UnmanagedFunctionPointer(CallingConvention.StdCall)>
Public Delegate Function GetHandlerPropertyDelegate(propID As ArchivePropId, ByRef value As PropVariant) As Integer
<UnmanagedFunctionPointer(CallingConvention.StdCall)>
Public Delegate Function GetNumberOfFormatsDelegate(ByRef numFormats As UInteger) As Integer
<UnmanagedFunctionPointer(CallingConvention.StdCall)>
Public Delegate Function GetHandlerProperty2Delegate(formatIndex As UInteger, propID As ArchivePropId, ByRef value As PropVariant) As Integer
Public Class StreamWrapper
  Implements IDisposable
  Protected BaseStream As Stream
  Protected Sub New(baseStream__1 As Stream)
    BaseStream = baseStream__1
  End Sub
  Public Sub Dispose() Implements IDisposable.Dispose
    BaseStream.Close()
  End Sub
  Public Overridable Sub Seek(offset As Long, seekOrigin As UInteger, newPosition As IntPtr)
    Dim Position As Long = CUInt(BaseStream.Seek(offset, CType(seekOrigin, SeekOrigin)))
    If newPosition <> IntPtr.Zero Then
      Marshal.WriteInt64(newPosition, Position)
    End If
  End Sub
End Class
Public Class InStreamWrapper
  Inherits StreamWrapper
  Implements ISequentialInStream
  Implements IInStream
  Public Sub New(baseStream As Stream)
    MyBase.New(baseStream)
  End Sub
  Public Function Read(data As Byte(), size As UInteger) As UInteger Implements ISequentialInStream.Read, IInStream.Read
    Return CUInt(BaseStream.Read(data, 0, CInt(size)))
  End Function
  Public Overrides Sub Seek(offset As Long, seekOrigin As UInteger, newPosition As System.IntPtr) Implements IInStream.Seek
    MyBase.Seek(offset, seekOrigin, newPosition)
  End Sub
End Class
Public Class OutStreamWrapper
  Inherits StreamWrapper
  Implements ISequentialOutStream
  Implements IOutStream
  Public Sub New(baseStream As Stream)
    MyBase.New(baseStream)
  End Sub
  Public Function SetSize(newSize As Long) As Integer Implements IOutStream.SetSize
    BaseStream.SetLength(newSize)
    Return 0
  End Function
  Public Function Write(data As Byte(), size As UInteger, processedSize As IntPtr) As Integer Implements ISequentialOutStream.Write, IOutStream.Write
    BaseStream.Write(data, 0, CInt(size))
    If processedSize <> IntPtr.Zero Then
      Marshal.WriteInt32(processedSize, CInt(size))
    End If
    Return 0
  End Function
  Public Overrides Sub Seek(offset As Long, seekOrigin As UInteger, newPosition As System.IntPtr) Implements IOutStream.Seek
    MyBase.Seek(offset, seekOrigin, newPosition)
  End Sub
End Class
