Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports System.Security
Imports System.Runtime.ConstrainedExecution
Imports System.ComponentModel
Namespace Extraction.COM
  Friend NotInheritable Class SafeNativeMethods
    Implements IDisposable
    Private Const Kernel32Dll As String = "kernel32.dll"
    Private NotInheritable Class SafeLibraryHandle
      Inherits SafeHandleZeroOrMinusOneIsInvalid
      Public Sub New()
        MyBase.New(True)
      End Sub
      <SuppressUnmanagedCodeSecurity()>
      <DllImport(Kernel32Dll)>
      Private Shared Function FreeLibrary(hModule As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
      End Function
      <ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)> _
      Protected Overrides Function ReleaseHandle() As Boolean
        Return FreeLibrary(handle)
      End Function
    End Class
    <DllImport(Kernel32Dll, CharSet:=CharSet.Auto, SetLastError:=True)> _
    Private Shared Function LoadLibrary(<MarshalAs(UnmanagedType.LPTStr)> lpFileName As String) As SafeLibraryHandle
    End Function
    <DllImport(Kernel32Dll, CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Private Shared Function GetProcAddress(hModule As SafeLibraryHandle, <MarshalAs(UnmanagedType.LPStr)> procName As String) As IntPtr
    End Function
    Private LibHandle As SafeLibraryHandle
    Public Sub New(sevenZipLibPath As String)
      LibHandle = LoadLibrary(sevenZipLibPath)
      If LibHandle.IsInvalid Then
        Throw New Win32Exception()
      End If
      Dim FunctionPtr As IntPtr = GetProcAddress(LibHandle, "GetHandlerProperty")
      If FunctionPtr = IntPtr.Zero Then
        LibHandle.Close()
        Throw New ArgumentException()
      End If
    End Sub
    Protected Overrides Sub Finalize()
      Try
        Dispose(False)
      Finally
        MyBase.Finalize()
      End Try
    End Sub
    Protected Sub Dispose(disposing As Boolean)
      If (LibHandle IsNot Nothing) AndAlso Not LibHandle.IsClosed Then
        LibHandle.Close()
      End If
      LibHandle = Nothing
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
    <UnmanagedFunctionPointer(CallingConvention.StdCall)>
    Public Delegate Function CreateObjectDelegate(<[In]()> ByRef classID As Guid, <[In]()> ByRef interfaceID As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef outObject As Object) As Integer
    Private Function CreateInterface(Of T As Class)(classId As Guid) As T
      If LibHandle Is Nothing Then
        Throw New ObjectDisposedException("SevenZipFormat")
      End If
      Dim CreateObject As CreateObjectDelegate = DirectCast(Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibHandle, "CreateObject"), GetType(CreateObjectDelegate)), CreateObjectDelegate)
      If CreateObject IsNot Nothing Then
        Dim Result As Object = Nothing
        Dim InterfaceId As Guid = GetType(T).GUID
        CreateObject(classId, InterfaceId, Result)
        Return TryCast(Result, T)
      End If
      Return Nothing
    End Function
    Public Function CreateInArchive(classId As Guid) As IInArchive
      Return CreateInterface(Of IInArchive)(classId)
    End Function
    <DllImport("ole32.dll")>
    Friend Shared Function PropVariantClear(ByRef pvar As PropVariant) As Integer
    End Function
  End Class
End Namespace
