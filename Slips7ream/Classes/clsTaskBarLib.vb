Imports System.Runtime.InteropServices
Namespace TaskbarLib
  <ComImport(), Guid("56FDF344-FD6D-11D0-958A-006097C9A090"), ClassInterface(CShort(0)), TypeLibType(CShort(2))>
  Public Class TaskbarListClass
    Implements ITaskbarList3, ITaskbarList2, ITaskbarList
    Public Sub New()
    End Sub
    Public Overridable Sub HrInit() Implements ITaskbarList3.HrInit, ITaskbarList2.HrInit, ITaskbarList.HrInit
    End Sub
    Public Overridable Sub ActivateTab(<[In]()> ByVal hwnd As Integer) Implements ITaskbarList3.ActivateTab, ITaskbarList2.ActivateTab, ITaskbarList.ActivateTab
    End Sub
    Public Overridable Sub AddTab(<[In]()> ByVal hwnd As Integer) Implements ITaskbarList3.AddTab, ITaskbarList2.AddTab, ITaskbarList.AddTab
    End Sub
    Public Overridable Sub DeleteTab(<[In]()> ByVal hwnd As Integer) Implements ITaskbarList3.DeleteTab, ITaskbarList2.DeleteTab, ITaskbarList.DeleteTab
    End Sub
    Public Overridable Sub MarkFullscreenWindow(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal fFullscreen As Integer) Implements ITaskbarList3.MarkFullscreenWindow, ITaskbarList2.MarkFullscreenWindow
    End Sub
    Public Overridable Sub RegisterTab(<[In]()> ByVal hwndTab As Integer, <[In](), ComAliasName("TaskbarLib.wireHWND")> ByRef hwndMDI As RemotableHandle) Implements ITaskbarList3.RegisterTab
    End Sub
    Public Overridable Sub SetActivateAlt(<[In]()> ByVal hwnd As Integer) Implements ITaskbarList3.SetActivateAlt, ITaskbarList2.SetActivateAlt, ITaskbarList.SetActivateAlt
    End Sub
    Public Overridable Sub SetOverlayIcon(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.IUnknown)> ByVal hIcon As Object, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszDescription As String) Implements ITaskbarList3.SetOverlayIcon
    End Sub
    Public Overridable Sub SetProgressState(<[In]()> ByVal hwnd As IntPtr, <[In]()> ByVal tbpFlags As TBPFLAG) Implements ITaskbarList3.SetProgressState
    End Sub
    Public Overridable Sub SetProgressValue(<[In]()> ByVal hwnd As IntPtr, <[In]()> ByVal ullCompleted As UInt64, <[In]()> ByVal ullTotal As UInt64) Implements ITaskbarList3.SetProgressValue
    End Sub
    Public Overridable Sub SetTabActive(<[In]()> ByVal hwndTab As Integer, <[In]()> ByVal hwndMDI As Integer, <[In]()> ByVal tbatFlags As TBATFLAG) Implements ITaskbarList3.SetTabActive
    End Sub
    Public Overridable Sub SetTabOrder(<[In]()> ByVal hwndTab As Integer, <[In]()> ByVal hwndInsertBefore As Integer) Implements ITaskbarList3.SetTabOrder
    End Sub
    Public Overridable Sub SetThumbnailClip(<[In]()> ByVal hwnd As Integer, <[In]()> ByRef prcClip As tagRECT) Implements ITaskbarList3.SetThumbnailClip
    End Sub
    Public Overridable Sub SetThumbnailTooltip(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszTip As String) Implements ITaskbarList3.SetThumbnailTooltip
    End Sub
    Public Overridable Sub ThumbBarAddButtons(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal cButtons As UInt32, <[In]()> ByRef pButton As tagTHUMBBUTTON) Implements ITaskbarList3.ThumbBarAddButtons
    End Sub
    Public Overridable Sub ThumbBarSetImageList(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.IUnknown)> ByVal himl As Object) Implements ITaskbarList3.ThumbBarSetImageList
    End Sub
    Public Overridable Sub ThumbBarUpdateButtons(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal cButtons As UInt32, <[In]()> ByRef pButton As tagTHUMBBUTTON) Implements ITaskbarList3.ThumbBarUpdateButtons
    End Sub
    Public Overridable Sub UnregisterTab(<[In]()> ByVal hwndTab As Integer) Implements ITaskbarList3.UnregisterTab
    End Sub
  End Class
  <ComImport(), InterfaceType(CShort(1)), Guid("56FDF342-FD6D-11D0-958A-006097C9A090")> Public Interface ITaskbarList
    Sub HrInit()
    Sub AddTab(<[In]()> ByVal hwnd As Integer)
    Sub DeleteTab(<[In]()> ByVal hwnd As Integer)
    Sub ActivateTab(<[In]()> ByVal hwnd As Integer)
    Sub SetActivateAlt(<[In]()> ByVal hwnd As Integer)
  End Interface
  <ComImport(), Guid("602D4995-B13A-429B-A66E-1935E44F4317"), InterfaceType(CShort(1))> Public Interface ITaskbarList2
    Inherits ITaskbarList
    Overloads Sub HrInit()
    Overloads Sub AddTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub DeleteTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub ActivateTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub SetActivateAlt(<[In]()> ByVal hwnd As Integer)
    Sub MarkFullscreenWindow(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal fFullscreen As Integer)
  End Interface
  <ComImport(), InterfaceType(CShort(1)), Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")> Public Interface ITaskbarList3
    Inherits ITaskbarList2
    Overloads Sub HrInit()
    Overloads Sub AddTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub DeleteTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub ActivateTab(<[In]()> ByVal hwnd As Integer)
    Overloads Sub SetActivateAlt(<[In]()> ByVal hwnd As Integer)
    Overloads Sub MarkFullscreenWindow(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal fFullscreen As Integer)
    Sub SetProgressValue(<[In]()> ByVal hwnd As IntPtr, <[In]()> ByVal ullCompleted As UInt64, <[In]()> ByVal ullTotal As UInt64)
    Sub SetProgressState(<[In]()> ByVal hwnd As IntPtr, <[In]()> ByVal tbpFlags As TBPFLAG)
    Sub RegisterTab(<[In]()> ByVal hwndTab As Integer, <[In](), ComAliasName("TaskbarLib.wireHWND")> ByRef hwndMDI As RemotableHandle)
    Sub UnregisterTab(<[In]()> ByVal hwndTab As Integer)
    Sub SetTabOrder(<[In]()> ByVal hwndTab As Integer, <[In]()> ByVal hwndInsertBefore As Integer)
    Sub SetTabActive(<[In]()> ByVal hwndTab As Integer, <[In]()> ByVal hwndMDI As Integer, <[In]()> ByVal tbatFlags As TBATFLAG)
    Sub ThumbBarAddButtons(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal cButtons As UInt32, <[In]()> ByRef pButton As tagTHUMBBUTTON)
    Sub ThumbBarUpdateButtons(<[In]()> ByVal hwnd As Integer, <[In]()> ByVal cButtons As UInt32, <[In]()> ByRef pButton As tagTHUMBBUTTON)
    Sub ThumbBarSetImageList(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.IUnknown)> ByVal himl As Object)
    Sub SetOverlayIcon(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.IUnknown)> ByVal hIcon As Object, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszDescription As String)
    Sub SetThumbnailTooltip(<[In]()> ByVal hwnd As Integer, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal pszTip As String)
    Sub SetThumbnailClip(<[In]()> ByVal hwnd As Integer, <[In]()> ByRef prcClip As tagRECT)
  End Interface
  <ComImport(), CoClass(GetType(TaskbarListClass)), Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")> Public Interface TaskbarList
    Inherits ITaskbarList3
  End Interface
  <StructLayout(LayoutKind.Sequential, Pack:=4)> Public Structure tagRECT
    Public left As Integer
    Public top As Integer
    Public right As Integer
    Public bottom As Integer
  End Structure
  <StructLayout(LayoutKind.Sequential, Pack:=4)> Public Structure RemotableHandle
    Public fContext As Integer
    Public u As IWinTypes
  End Structure
  <StructLayout(LayoutKind.Explicit, Pack:=4)> Public Structure IWinTypes
    <FieldOffset(0)> Public hInproc As Integer
    <FieldOffset(0)> Public hRemote As Integer
  End Structure
  <StructLayout(LayoutKind.Sequential, Pack:=4)> Public Structure tagTHUMBBUTTON
    Public dwMask As UInt32
    Public iId As UInt32
    Public iBitmap As UInt32
    <MarshalAs(UnmanagedType.IUnknown)> Public hIcon As Object
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=260)> Public szTip As UInt16()
    Public dwFlags As UInt32
  End Structure
  <Flags()> Public Enum TBATFLAG
    TBATF_USEMDILIVEPREVIEW = 2
    TBATF_USEMDITHUMBNAIL = 1
  End Enum
  <Flags()> Public Enum TBPFLAG
    TBPF_ERROR = 4
    TBPF_INDETERMINATE = 1
    TBPF_NOPROGRESS = 0
    TBPF_NORMAL = 2
    TBPF_PAUSED = 8
  End Enum
  Friend Class TaskbarFinder
    <DllImport("user32.dll", CharSet:=CharSet.Unicode)> _
    Private Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)> _
    Public Shared Function FindWindowEx(parentHandle As IntPtr, childAfter As IntPtr, lclassName As String, windowTitle As String) As IntPtr
    End Function
    Friend Shared Function TaskbarVisible() As Boolean
      Dim shellTrayWnd As IntPtr = FindWindow("Shell_TrayWnd", String.Empty)
      If shellTrayWnd.Equals(IntPtr.Zero) Then
        Return False
      Else
        Dim trayNotifyWnd As IntPtr = FindWindowEx(shellTrayWnd, IntPtr.Zero, "TrayNotifyWnd", String.Empty)
        If trayNotifyWnd.Equals(IntPtr.Zero) Then
          Return False
        Else
          Dim sysPagerWnd As IntPtr = FindWindowEx(trayNotifyWnd, IntPtr.Zero, "SysPager", String.Empty)
          If sysPagerWnd.Equals(IntPtr.Zero) Then
            Return False
          Else
            Return True
          End If
        End If
      End If
    End Function
  End Class
End Namespace
