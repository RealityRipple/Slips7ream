Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Security.AccessControl

Public Class clsRegistry

#Region "Declares"
  <DllImport("advapi32.dll", EntryPoint:="AdjustTokenPrivileges", SetLastError:=True)> _
  Private Shared Function AdjustTokenPrivileges(TokenHandle As IntPtr, DisableAllPriv As Integer, ByRef NewState As TOKEN_PRIVILEGES, BufferLength As Integer, PreviousState As Integer, ReturnLength As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function

  Private Declare Function AllocateAndInitializeSid Lib "advapi32.dll" (ByVal pIdentifierAuthority As SID_IDENTIFIER_AUTHORITY, ByVal nSubAuthorityCount As Byte, ByVal dwSubAuthority0 As Integer, ByVal dwSubAuthority1 As Integer, ByVal dwSubAuthority2 As Integer, ByVal dwSubAuthority3 As Integer, ByVal dwSubAuthority4 As Integer, ByVal dwSubAuthority5 As Integer, ByVal dwSubAuthority6 As Integer, ByVal dwSubAuthority7 As Integer, ByRef pSid As IntPtr) As Boolean

  <DllImportAttribute("kernel32.dll", EntryPoint:="CloseHandle")> _
  Private Shared Function CloseHandle(<InAttribute()> hObject As IntPtr) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
  End Function

  <DllImportAttribute("advapi32.dll", EntryPoint:="FreeSid")> _
  Private Shared Function FreeSid(<InAttribute()> pSid As IntPtr) As IntPtr
  End Function

  <DllImportAttribute("kernel32.dll", EntryPoint:="GetCurrentProcess")> _
  Private Shared Function GetCurrentProcess() As IntPtr
  End Function

  <DllImportAttribute("kernel32.dll", EntryPoint:="LocalFree")> _
  Private Shared Function LocalFree(hMem As IntPtr) As IntPtr
  End Function

  <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, EntryPoint:="LookupPrivilegeValueA", SetLastError:=True)> _
  Private Shared Function LookupPrivilegeValueA(lpSystemName As Integer, <MarshalAs(UnmanagedType.LPStr)> lpName As String, ByRef lpLuid As LUID) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function

  <DllImport("advapi32.dll", EntryPoint:="OpenProcessToken", SetLastError:=True)> _
  Private Shared Function OpenProcessToken(ProcessHandle As IntPtr, DesiredAccess As UInteger, ByRef TokenHandle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function

  <DllImport("advapi32.dll", CharSet:=CharSet.Auto)> _
  Private Shared Function SetNamedSecurityInfo(<MarshalAs(UnmanagedType.LPTStr)> pObjectName As String, ObjectType As SE_OBJECT_TYPE, SecurityInfo As SECURITY_INFORMATION, psidOwner As IntPtr, psidGroup As IntPtr, pDacl As IntPtr, pSacl As IntPtr) As Integer
  End Function

  <DllImport("Advapi32.dll", EntryPoint:="SetEntriesInAclA", CallingConvention:=CallingConvention.Winapi, SetLastError:=True, CharSet:=CharSet.Ansi)> _
  Private Shared Function SetEntriesInAcl(CountofExplicitEntries As Integer, ByRef ea As EXPLICIT_ACCESS, OldAcl As IntPtr, ByRef NewAcl As IntPtr) As Integer
  End Function
#End Region

#Region "Enums"
  <Flags()>
  Private Enum ACCESS_MASK : Uint32
    DELETE = &H10000
    READ_CONTROL = &H20000
    WRITE_DAC = &H40000
    WRITE_OWNER = &H80000
    SYNCHRONIZE = &H100000

    STANDARD_RIGHTS_REQUIRED = &HF0000

    STANDARD_RIGHTS_READ = &H20000
    STANDARD_RIGHTS_WRITE = &H20000
    STANDARD_RIGHTS_EXECUTE = &H20000

    STANDARD_RIGHTS_ALL = &H1F0000

    SPECIFIC_RIGHTS_ALL = &HFFFF

    ACCESS_SYSTEM_SECURITY = &H1000000

    MAXIMUM_ALLOWED = &H2000000

    GENERIC_READ = &H80000000
    GENERIC_WRITE = &H40000000
    GENERIC_EXECUTE = &H20000000
    GENERIC_ALL = &H10000000

    DESKTOP_READOBJECTS = &H1
    DESKTOP_CREATEWINDOW = &H2
    DESKTOP_CREATEMENU = &H4
    DESKTOP_HOOKCONTROL = &H8
    DESKTOP_JOURNALRECORD = &H10
    DESKTOP_JOURNALPLAYBACK = &H20
    DESKTOP_ENUMERATE = &H40
    DESKTOP_WRITEOBJECTS = &H80
    DESKTOP_SWITCHDESKTOP = &H100

    WINSTA_ENUMDESKTOPS = &H1
    WINSTA_READATTRIBUTES = &H2
    WINSTA_ACCESSCLIPBOARD = &H4
    WINSTA_CREATEDESKTOP = &H8
    WINSTA_WRITEATTRIBUTES = &H10
    WINSTA_ACCESSGLOBALATOMS = &H20
    WINSTA_EXITWINDOWS = &H40
    WINSTA_ENUMERATE = &H100
    WINSTA_READSCREEN = &H200

    WINSTA_ALL_ACCESS = &H37F
  End Enum

  Private Enum ACCESS_MODE
    NOT_USED_ACCESS
    GRANT_ACCESS
    SET_ACCESS
    DENY_ACCESS
    REVOKE_ACCESS
    SET_AUDIT_SUCCESS
    SET_AUDIT_FAILURE
  End Enum

  Private Enum SE_OBJECT_TYPE
    SE_UNKNOWN_OBJECT_TYPE = 0
    SE_FILE_OBJECT
    SE_SERVICE
    SE_PRINTER
    SE_REGISTRY_KEY
    SE_LMSHARE
    SE_KERNEL_OBJECT
    SE_WINDOW_OBJECT
    SE_DS_OBJECT
    SE_DS_OBJECT_ALL
    SE_PROVIDER_DEFINED_OBJECT
    SE_WMIGUID_OBJECT
    SE_REGISTRY_WOW64_32KEY
  End Enum

  Private Enum SECURITY_INFORMATION As Integer
    OWNER_SECURITY_INFORMATION = &H1
    GROUP_SECURITY_INFORMATION = &H2
    DACL_SECURITY_INFORMATION = &H4
    SACL_SECURITY_INFORMATION = &H8
    UNPROTECTED_SACL_SECURITY_INFORMATION = &H10000000
    UNPROTECTED_DACL_SECURITY_INFORMATION = &H20000000
    PROTECTED_SACL_SECURITY_INFORMATION = &H40000000
    PROTECTED_DACL_SECURITY_INFORMATION = &H80000000
  End Enum

  Private Enum TOKEN_PRIVILEGES_ENUM As UInteger
    ASSIGN_PRIMARY = &H1
    TOKEN_DUPLICATE = &H2
    TOKEN_IMPERSONATE = &H4
    TOKEN_QUERY = &H8
    TOKEN_QUERY_SOURCE = &H10
    TOKEN_ADJUST_PRIVILEGES = &H20
    TOKEN_ADJUST_GROUPS = &H40
    TOKEN_ADJUST_DEFAULT = &H80
    TOKEN_ADJUST_SESSIONID = &H100
  End Enum

  Private Enum TRUSTEE_FORM
    TRUSTEE_IS_SID
    TRUSTEE_IS_NAME
    TRUSTEE_BAD_FORM
    TRUSTEE_IS_OBJECTS_AND_SID
    TRUSTEE_IS_OBJECTS_AND_NAME
  End Enum

  Private Enum TRUSTEE_TYPE
    TRUSTEE_IS_UNKNOWN
    TRUSTEE_IS_USER
    TRUSTEE_IS_GROUP
    TRUSTEE_IS_DOMAIN
    TRUSTEE_IS_ALIAS
    TRUSTEE_IS_WELL_KNOWN_GROUP
    TRUSTEE_IS_DELETED
    TRUSTEE_IS_INVALID
    TRUSTEE_IS_COMPUTER
  End Enum
#End Region

#Region "Structures"
  <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)>
  Private Structure EXPLICIT_ACCESS
    Dim grfAccessPermissions As Integer
    Dim grfAccessMode As Integer
    Dim grfInheritance As Integer
    Dim Trustee As TRUSTEE
  End Structure

  Private Structure LUID
    Public LowPart As UInt32
    Public HighPart As Integer
  End Structure

  Private Structure LUID_AND_ATTRIBUTES
    Public Luid As LUID
    Public Attributes As Integer
  End Structure

  Private Structure SID_IDENTIFIER_AUTHORITY
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)>
    Public Value() As Byte
    Public Sub New(ByVal val As Byte())
      Value = val
    End Sub
  End Structure

  <StructLayoutAttribute(LayoutKind.Sequential)>
  Private Structure TOKEN_PRIVILEGES
    Public PrivilegeCount As UInteger
    <MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst:=1, ArraySubType:=UnmanagedType.Struct)>
    Public Privileges As LUID_AND_ATTRIBUTES()
  End Structure

  <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)> _
  Private Structure TRUSTEE
    Dim pMultipleTrustee As IntPtr
    Dim MultipleTrusteeOperation As Integer
    Dim TrusteeForm As Integer
    Dim TrusteeType As Integer
    Dim ptstrName As IntPtr
  End Structure
#End Region

#Region "Constants"
  Private Const DOMAIN_ALIAS_RID_ADMINS As Integer = 544
  Private Const NO_INHERITANCE As Integer = 0
  Private Const SE_PRIVILEGE_ENABLED As Integer = 2
  Private Const SE_RESTORE_NAME As String = "SeRestorePrivilege"
  Private Const SE_TAKE_OWNERSHIP_NAME As String = "SeTakeOwnershipPrivilege"
  Private Const SECURITY_NT_AUTHORITY As Integer = 5
  Private Const SECURITY_LOCAL_SYSTEM_RID As Integer = 18
  Private Const SECURITY_BUILTIN_DOMAIN_RID As Integer = 32
#End Region

  Private Shared Function ChangeObjectOwnership(ObjectName As String, ObjectType As SE_OBJECT_TYPE, SYSTEM As Boolean) As Boolean
    Dim success As Boolean = False
    Dim pSidAdmin As IntPtr = IntPtr.Zero
    Dim pAcl As IntPtr = IntPtr.Zero
    Dim name As String = ObjectName

    Dim sidNTAuthority As SID_IDENTIFIER_AUTHORITY
    If SYSTEM Then
      sidNTAuthority = New SID_IDENTIFIER_AUTHORITY With {.Value = New Byte() {0, 0, 0, 0, 0, 5}}
      success = AllocateAndInitializeSid(sidNTAuthority, CByte(1), SECURITY_LOCAL_SYSTEM_RID, 0, 0, 0, 0, 0, 0, 0, pSidAdmin)
    Else
      sidNTAuthority = New SID_IDENTIFIER_AUTHORITY With {.Value = New Byte() {0, 0, 0, 0, 0, 5}}
      success = AllocateAndInitializeSid(sidNTAuthority, CByte(2), SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, pSidAdmin)
    End If

    If ObjectName.StartsWith("HKEY_CLASSES_ROOT") Then
      name = ObjectName.Replace("HKEY_CLASSES_ROOT", "CLASSES_ROOT")
    ElseIf ObjectName.StartsWith("HKEY_CURRENT_USER") Then
      name = ObjectName.Replace("HKEY_CURRENT_USER", "CURRENT_USER")
    ElseIf ObjectName.StartsWith("HKEY_LOCAL_MACHINE") Then
      name = ObjectName.Replace("HKEY_LOCAL_MACHINE", "MACHINE")
    ElseIf ObjectName.StartsWith("HKEY_USERS") Then
      name = ObjectName.Replace("HKEY_USERS", "USERS")
    End If

    If success Then
      Dim explicitAccesss As EXPLICIT_ACCESS() = New EXPLICIT_ACCESS(0) {}
      explicitAccesss(0).grfAccessPermissions = ACCESS_MASK.GENERIC_ALL
      explicitAccesss(0).grfAccessMode = ACCESS_MODE.SET_ACCESS
      explicitAccesss(0).grfInheritance = NO_INHERITANCE
      explicitAccesss(0).Trustee.TrusteeForm = TRUSTEE_FORM.TRUSTEE_IS_SID
      explicitAccesss(0).Trustee.TrusteeType = TRUSTEE_TYPE.TRUSTEE_IS_GROUP

      explicitAccesss(0).Trustee.ptstrName = pSidAdmin
      SetEntriesInAcl(1, explicitAccesss(0), IntPtr.Zero, pAcl)

      success = SetPrivilege(SE_TAKE_OWNERSHIP_NAME, True)
      success = SetPrivilege(SE_RESTORE_NAME, True)
      If success Then
        Dim p As Integer = SetNamedSecurityInfo(name, ObjectType, SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION, pSidAdmin, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
        success = SetPrivilege(SE_TAKE_OWNERSHIP_NAME, False)
        success = SetPrivilege(SE_RESTORE_NAME, False)
        If success Then
          SetNamedSecurityInfo(name, ObjectType, SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, IntPtr.Zero, IntPtr.Zero, pAcl, IntPtr.Zero)
        End If
      End If
    End If

    If pSidAdmin <> IntPtr.Zero Then
      FreeSid(pSidAdmin)
    End If
    If pAcl <> IntPtr.Zero Then
      LocalFree(pAcl)
    End If
    Return success
  End Function

  Private Shared Function SetPrivilege(privilege As String, allow As Boolean) As Boolean
    Dim success As Boolean = False
    Dim token As IntPtr = IntPtr.Zero
    Dim tokenPrivileges As New TOKEN_PRIVILEGES()

    success = OpenProcessToken(GetCurrentProcess(), CUInt(TOKEN_PRIVILEGES_ENUM.TOKEN_ADJUST_PRIVILEGES) Or CUInt(TOKEN_PRIVILEGES_ENUM.TOKEN_QUERY), token)
    If success Then
      If allow Then
        Dim luid As LUID
        LookupPrivilegeValueA(Nothing, privilege, luid)
        tokenPrivileges.PrivilegeCount = 1
        tokenPrivileges.Privileges = New LUID_AND_ATTRIBUTES(0) {}
        tokenPrivileges.Privileges(0).Luid = luid
        tokenPrivileges.Privileges(0).Attributes = SE_PRIVILEGE_ENABLED
      End If
      success = AdjustTokenPrivileges(token, 0, tokenPrivileges, 0, 0, 0)
    End If
    If token <> IntPtr.Zero Then
      CloseHandle(token)
    End If
    Return success
  End Function

  Public Shared Function ModifySessionsPending() As Boolean
    Try
      Dim subkey As String = "SLIPS7REAM\Microsoft\Windows\CurrentVersion\Component Based Servicing\SessionsPending"
      If Not ChangeObjectOwnership("MACHINE\" & subkey, SE_OBJECT_TYPE.SE_REGISTRY_KEY, False) Then Return False
      Using wk As RegistryKey = Registry.LocalMachine.OpenSubKey(subkey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey)
        Dim rs As RegistrySecurity = wk.GetAccessControl()
        rs.SetAccessRuleProtection(False, False)
        Dim cvrn As Security.AccessControl.AuthorizationRuleCollection = rs.GetAccessRules(True, True, GetType(System.Security.Principal.NTAccount))
        For I As Integer = 0 To cvrn.Count - 1
          If Not cvrn(I).GetType Is GetType(Security.AccessControl.RegistryAccessRule) Then Continue For
          Dim myOldRule = CType(cvrn(I), Security.AccessControl.RegistryAccessRule)
          rs.RemoveAccessRule(myOldRule)
        Next
        wk.SetAccessControl(rs)
        wk.SetValue("Exclusive", 0, Microsoft.Win32.RegistryValueKind.DWord)
        wk.SetValue("TotalSessionPhases", 0, Microsoft.Win32.RegistryValueKind.DWord)
        wk.Close()
      End Using
      If Not ChangeObjectOwnership("MACHINE\" & subkey, SE_OBJECT_TYPE.SE_REGISTRY_KEY, True) Then Return False
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
End Class
