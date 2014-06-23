Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices
Public Enum KnownSevenZipFormat
  SevenZip
  Arj
  BZip2
  Cab
  Chm
  Compound
  Cpio
  Deb
  GZip
  Iso
  Lzh
  Lzma
  Nsis
  Rar
  Rpm
  Split
  Tar
  Wim
  Lzw
  Udf
  Xar
  Mub
  Hfs
  Dmg
  XZ
  Mslz
  PE
  Elf
  Swf
  Vhd
  Z
  Zip
End Enum
Public Class SevenZipFormat
  Implements IDisposable
#Region "Win32 API"
  Private Const Kernel32Dll As String = "kernel32.dll"
  Private NotInheritable Class SafeLibraryHandle
    Inherits SafeHandleZeroOrMinusOneIsInvalid
    Public Sub New()
      MyBase.New(True)
    End Sub
    <Security.SuppressUnmanagedCodeSecurity()>
    <DllImport(Kernel32Dll)>
    Private Shared Function FreeLibrary(hModule As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <Runtime.ConstrainedExecution.ReliabilityContract(Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, Runtime.ConstrainedExecution.Cer.Success)>
    Protected Overrides Function ReleaseHandle() As Boolean
      Return FreeLibrary(handle)
    End Function
  End Class
  <DllImport(Kernel32Dll, CharSet:=CharSet.Auto, SetLastError:=True)>
  Private Shared Function LoadLibrary(<MarshalAs(UnmanagedType.LPTStr)> lpFileName As String) As SafeLibraryHandle
  End Function
  <DllImport(Kernel32Dll, CharSet:=CharSet.Ansi, SetLastError:=True)>
  Private Shared Function GetProcAddress(hModule As SafeLibraryHandle, <MarshalAs(UnmanagedType.LPStr)> procName As String) As IntPtr
  End Function
#End Region
  Private LibHandle As SafeLibraryHandle
  Public Sub New(sevenZipLibPath As String)
    LibHandle = LoadLibrary(sevenZipLibPath)
    If LibHandle.IsInvalid Then
      Throw New System.ComponentModel.Win32Exception(sevenZipLibPath)
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
  Public Function CreateInArchive(classId As Guid) As IInArchive
    If LibHandle Is Nothing Then
      Throw New ObjectDisposedException("SevenZipFormat")
    End If
    Dim CreateObject As CreateObjectDelegate = DirectCast(Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibHandle, "CreateObject"), GetType(CreateObjectDelegate)), CreateObjectDelegate)
    If CreateObject IsNot Nothing Then
      Dim Result As Object = Nothing
      Dim InterfaceId As Guid = GetType(IInArchive).GUID
      CreateObject(classId, InterfaceId, Result)
      Return TryCast(Result, IInArchive)
    End If
    Return Nothing
  End Function
  Private Shared FFormatClassMap As Dictionary(Of KnownSevenZipFormat, Guid)
  Private Shared ReadOnly Property FormatClassMap() As Dictionary(Of KnownSevenZipFormat, Guid)
    Get
      If FFormatClassMap Is Nothing Then
        FFormatClassMap = New Dictionary(Of KnownSevenZipFormat, Guid)()
        FFormatClassMap.Add(KnownSevenZipFormat.SevenZip, New Guid("23170f69-40c1-278a-1000-000110070000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Arj, New Guid("23170f69-40c1-278a-1000-000110040000"))
        FFormatClassMap.Add(KnownSevenZipFormat.BZip2, New Guid("23170f69-40c1-278a-1000-000110020000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Cab, New Guid("23170f69-40c1-278a-1000-000110080000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Chm, New Guid("23170f69-40c1-278a-1000-000110e90000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Compound, New Guid("23170f69-40c1-278a-1000-000110e50000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Cpio, New Guid("23170f69-40c1-278a-1000-000110ed0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Deb, New Guid("23170f69-40c1-278a-1000-000110ec0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.GZip, New Guid("23170f69-40c1-278a-1000-000110ef0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Iso, New Guid("23170f69-40c1-278a-1000-000110e70000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Lzh, New Guid("23170f69-40c1-278a-1000-000110060000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Lzma, New Guid("23170f69-40c1-278a-1000-0001100a0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Nsis, New Guid("23170f69-40c1-278a-1000-000110090000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Rar, New Guid("23170f69-40c1-278a-1000-000110030000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Rpm, New Guid("23170f69-40c1-278a-1000-000110eb0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Split, New Guid("23170f69-40c1-278a-1000-000110ea0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Tar, New Guid("23170f69-40c1-278a-1000-000110ee0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Wim, New Guid("23170f69-40c1-278a-1000-000110e60000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Lzw, New Guid("23170f69-40c1-278a-1000-000110050000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Zip, New Guid("23170f69-40c1-278a-1000-000110010000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Udf, New Guid("23170f69-40c1-278a-1000-000110E00000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Xar, New Guid("23170f69-40c1-278a-1000-000110E10000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Mub, New Guid("23170f69-40c1-278a-1000-000110E20000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Hfs, New Guid("23170f69-40c1-278a-1000-000110E30000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Dmg, New Guid("23170f69-40c1-278a-1000-000110E40000"))
        FFormatClassMap.Add(KnownSevenZipFormat.XZ, New Guid("23170f69-40c1-278a-1000-0001100C0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Mslz, New Guid("23170f69-40c1-278a-1000-000110D50000"))
        FFormatClassMap.Add(KnownSevenZipFormat.PE, New Guid("23170f69-40c1-278a-1000-000110DD0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Elf, New Guid("23170f69-40c1-278a-1000-000110DE0000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Swf, New Guid("23170f69-40c1-278a-1000-000110D70000"))
        FFormatClassMap.Add(KnownSevenZipFormat.Vhd, New Guid("23170f69-40c1-278a-1000-000110DC0000"))
      End If
      Return FFormatClassMap
    End Get
  End Property
  Public Shared Function GetClassIdFromKnownFormat(format As KnownSevenZipFormat) As Guid
    Dim Result As Guid
    If FormatClassMap.TryGetValue(format, Result) Then
      Return Result
    End If
    Return Guid.Empty
  End Function
End Class
