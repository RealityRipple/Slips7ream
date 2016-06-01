Imports Microsoft.WindowsAPICodePack.Dialogs
Imports System.Runtime.InteropServices
Public Module modFunctions
  Public Declare Function WindowFromPoint Lib "user32" (pt As Point) As IntPtr
  Public Declare Function SendMessageA Lib "user32" (hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
  Private Declare Function SetupDiGetClassImageList Lib "setupapi" (ByRef classImageListData As SP_CLASSIMAGELIST_DATA) As Boolean
  Private Declare Function SetupDiGetClassImageIndex Lib "setupapi" (classImageListData As SP_CLASSIMAGELIST_DATA, classGUID As Guid, ByRef imageIndex As Int16) As Boolean
  Private Declare Function SetupDiDestroyClassImageList Lib "setupapi" (ByRef classImageListData As SP_CLASSIMAGELIST_DATA) As Boolean
  Private Declare Function DestroyIcon Lib "user32" (handle As IntPtr) As Boolean
  Private Declare Function ImageList_GetIcon Lib "comctl32" (hIML As IntPtr, index As Integer, flags As Integer) As IntPtr
  Private Declare Function ExtractIconEx Lib "shell32" (lpszFile As String, nIconIndex As Int16, phiconLarge() As IntPtr, phiconSmall() As IntPtr, nIcons As UInt16) As UInt16
  Private Declare Auto Function GetShortPathName Lib "kernel32.dll" (ByVal lpszLongPath As String, ByVal lpszShortPath As String, ByVal cchBuffer As Int32) As Int32
  Public Enum ArchitectureList
    x86
    amd64
    ia64
  End Enum
  Public Enum WIMGroup
    WIM
    Merge
    All
  End Enum
  <StructLayout(LayoutKind.Sequential)>
  Private Structure SP_CLASSIMAGELIST_DATA
    Public cbSize As UInteger
    Public ImageList As IntPtr
    Public Reserved As UInteger
  End Structure
  Public Function GetUpdateInfo(sPath As String) As Update_File()
    If IO.File.Exists(sPath) Then
      Select Case GetUpdateType(sPath)
        Case UpdateType.EXE
          Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(sPath)
          If fInfo.OriginalFilename = "iesetup.exe" And fInfo.ProductMajorPart > 9 Then
            Dim EXEPath As String = Update_File.WorkDir & "UpdateEXE_Extract" & IO.Path.DirectorySeparatorChar
            If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            IO.Directory.CreateDirectory(EXEPath)
            Dim iExtract As New Process With {.StartInfo = New ProcessStartInfo(sPath, "/x:" & EXEPath)}
            If iExtract.Start Then
              iExtract.WaitForExit()
              Dim updateList As New List(Of Update_File)
              For Each sFile In IO.Directory.GetFiles(EXEPath)
                If IO.Path.GetFileName(sFile).ToLower = "ie-win7.cab" Then
                  updateList.Add(New Update_File(sFile) With {.Path = sPath})
                ElseIf IO.Path.GetFileName(sFile).ToLower.Contains("ie-spelling") Then
                  updateList.Add(New Update_File(sFile) With {.Path = sPath})
                ElseIf IO.Path.GetFileName(sFile).ToLower.Contains("ie-hyphenation") Then
                  updateList.Add(New Update_File(sFile) With {.Path = sPath})
                ElseIf IO.Path.GetFileName(sFile).ToLower.Contains("ielangpack") Then
                  updateList.Add(New Update_File(sFile) With {.Path = sPath})
                End If
              Next
              If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              Return updateList.ToArray
            Else
              If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              Return {New Update_File(sPath)}
            End If
          Else
            Return {New Update_File(sPath)}
          End If
        Case Else
          Return {New Update_File(sPath)}
      End Select
    Else
      Return Nothing
    End If
  End Function
  Public Function GetUpdateName(Ident As Update_Identity) As String
    Select Case Ident.Name
      Case "WUClient-SelfUpdate-Core-TopLevel"
        Return "Windows Update Agent " & Ident.Version.Substring(0, Ident.Version.IndexOf(".", Ident.Version.IndexOf(".") + 1))
      Case "Microsoft-Windows-InternetExplorer-LanguagePack"
        Dim ieVer As String = Ident.Version.Substring(0, Ident.Version.IndexOf("."))
        Return Ident.Language & " IE" & ieVer & " Language Pack"
      Case "Microsoft-Windows-InternetExplorer-LIPPack"
        Dim ieVer As String = Ident.Version.Substring(0, Ident.Version.IndexOf("."))
        Return Ident.Language & " IE" & ieVer & " Language Interface Pack"
      Case "Microsoft-Windows-InternetExplorer-Package-TopLevel"
        Dim ieVer As String = Ident.Version.Substring(0, Ident.Version.IndexOf("."))
        Return "Internet Explorer " & ieVer
      Case "Microsoft-Windows-PlatformUpdate-Win7-SRV08R2-Package-TopLevel" : Return "Platform Update for Windows 7"
      Case "Microsoft-Windows-Client-LanguagePack-Package" : Return Ident.Language & " Multilingual User Interface Pack"
      Case "Microsoft-Windows-Client-Refresh-LanguagePack-Package" : Return Ident.Language & " Multilingual User Interface Pack"
      Case "Microsoft-Windows-LIP-LanguagePack-Package" : Return Ident.Language & " Language Interface Pack"
      Case "Microsoft-Windows-RDP-WinIP-Package-TopLevel" : Return "Remote Desktop Protocol Update"
      Case "Microsoft-Windows-RDP-BlueIP-Package-TopLevel" : Return "Remote App and Desktop Connections Update"
      Case "Microsoft-Windows-Security-WindowsActivationTechnologies-Package" : Return "Windows Activation Update"
      Case Else
        If Ident.Name.StartsWith("Package_for_") Then
          Return Ident.Name.Substring(12) & " Update"
        ElseIf Ident.Name.StartsWith("Microsoft-Windows-IE-Spelling-Parent-Package") Then
          Return "IE Spelling Package for " & Ident.Name.Substring(Ident.Name.LastIndexOf("-") + 1)
        ElseIf Ident.Name.StartsWith("Microsoft-Windows-IE-Hyphenation-Parent-Package") Then
          Return "IE Hyphenation Package for " & Ident.Name.Substring(Ident.Name.LastIndexOf("-") + 1)
        Else
          Return Ident.Name
        End If
    End Select
  End Function
  Public Function GetUpdateCompany(Provider As String, ReferenceList() As String) As String
    If String.IsNullOrEmpty(Provider) Then Return "Unknown"
    Do While Provider.EndsWith(".")
      Provider = Provider.Substring(0, Provider.Length - 1)
      If String.IsNullOrEmpty(Provider) Then Return "Unknown"
    Loop
    Do While Provider.EndsWith(" Inc")
      Provider = Provider.Substring(0, Provider.Length - 4)
      If String.IsNullOrEmpty(Provider) Then Return "Unknown"
    Loop
    Do While Provider.EndsWith(" GmbH")
      Provider = Provider.Substring(0, Provider.Length - 5)
      If String.IsNullOrEmpty(Provider) Then Return "Unknown"
    Loop
    Do While Provider.EndsWith(",")
      Provider = Provider.Substring(0, Provider.Length - 1)
      If String.IsNullOrEmpty(Provider) Then Return "Unknown"
    Loop
    For Each refItem In ReferenceList
      If Provider.StartsWith(refItem & " ") Then Provider = refItem
    Next
    Select Case Provider.ToLower
      Case "%msft%", "msft" : Return "Microsoft"
      Case "amd", "ati technologies" : Return "Advanced Micro Devices"
      Case "intel corporation" : Return "Intel"
      Case "nvidia corporation" : Return "NVIDIA"
      Case "hp", "hewlett-packard company", "compaq" : Return "Hewlett-Packard"
      Case "okidata" : Return "Oki"
      Case "philips semiconductors" : Return "NXP Semiconductors"
      Case "gestetner", "infotec", "lanier", "nrg" : Return "Ricoh"
      Case "atheros", "qualcomm" : Return "Qualcomm Atheros"
      Case "hanns-g" : Return "Hannspree"
      Case "realtek semiconductor corp" : Return "RealTek"
      Case "elantech" : Return "Elan"
    End Select
    Return Provider
  End Function
  Private Function ConvertPathVars(Path As String, OriginalINF As String, arch As ArchitectureList) As String
    Do While Path.Contains("%")
      Dim sA As String = Path.Substring(0, Path.IndexOf("%"))
      Dim sB As String = Path.Substring(Path.IndexOf("%", Path.IndexOf("%") + 1) + 1)
      Dim sE As String = Path.Substring(Path.IndexOf("%") + 1)
      sE = sE.Substring(0, sE.IndexOf("%"))
      Path = sA & GetEnvPath(sE, OriginalINF, arch) & sB
    Loop
    If IsNumeric(Path) Then Path = GetEnvPath(Path, OriginalINF, arch)
    Return Path.Trim
  End Function
  Private Function GetEnvPath(sInput As String, OriginalINF As String, arch As ArchitectureList) As String
    Dim sEnv As String = Nothing
    If IsNumeric(sInput) Then
      Dim pathWin As String = IO.Path.GetDirectoryName(OriginalINF)
      If pathWin.ToLower.Contains("windows") Then
        pathWin = pathWin.Substring(0, pathWin.ToLower.LastIndexOf("windows"))
      Else
        pathWin = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
      End If
      Select Case Int32.Parse(sInput)
        Case 1 : sEnv = IO.Path.GetDirectoryName(OriginalINF)
        Case &HA : sEnv = IO.Path.Combine(pathWin, "Windows")
        Case &HB : sEnv = IO.Path.Combine(pathWin, "Windows", "System32")
        Case &HC : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "drivers")
        Case &H11 : sEnv = IO.Path.Combine(pathWin, "Windows", "inf")
        Case &H12 : sEnv = IO.Path.Combine(pathWin, "Windows", "help")
        Case &H14 : sEnv = IO.Path.Combine(pathWin, "Windows", "fonts")
        Case &H15 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "viewers")
        Case &H17 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers", "color")
        Case &H18 : sEnv = pathWin
        Case &H19 : sEnv = IO.Path.Combine(pathWin, "Windows")
        Case &H1E : sEnv = pathWin
        Case &H32 : sEnv = IO.Path.Combine(pathWin, "Windows", "System")
        Case &H33 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool")
        Case &H34 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers")
        Case &H35 : sEnv = IO.Path.Combine(pathWin, "Users")
        Case &H36 : sEnv = pathWin
        Case &H37
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "W32X86")
            Case ArchitectureList.amd64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "x64")
            Case ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "IA64")
          End Select
        Case &H4016 : sEnv = IO.Path.Combine(pathWin, "ProgramData", "Microsoft", "Windows", "Start Menu")
        Case &H4017 : sEnv = IO.Path.Combine(pathWin, "ProgramData", "Microsoft", "Windows", "Start Menu", "Programs")
        Case &H4018 : sEnv = IO.Path.Combine(pathWin, "ProgramData", "Microsoft", "Windows", "Start Menu", "Programs", "Startup")
        Case &H4019 : sEnv = IO.Path.Combine(pathWin, "Users", "Public", "Desktop")
        Case &H401F : sEnv = IO.Path.Combine(pathWin, "Users", "Public", "Favorites")
        Case &H4023 : sEnv = IO.Path.Combine(pathWin, "ProgramData")
        Case &H4026 : sEnv = IO.Path.Combine(pathWin, "Program Files")
        Case &H4029
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32")
            Case ArchitectureList.amd64, ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Windows", "SysWOW64")
          End Select
        Case &H402A
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Program Files")
            Case ArchitectureList.amd64, ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Program Files (x86)")
          End Select
        Case &H402B : sEnv = IO.Path.Combine(pathWin, "Program Files", "Common Files")
        Case &H402C
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Program Files", "Common Files")
            Case ArchitectureList.amd64, ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Program Files (x86)", "Common Files")
          End Select
        Case &H402D : sEnv = IO.Path.Combine(pathWin, "ProgramData", "Microsoft", "Windows", "Templates")
        Case &H402E : sEnv = IO.Path.Combine(pathWin, "Users", "Public", "Documents")
        Case &H101D0
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers", "W32X86")
            Case ArchitectureList.amd64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers", "x64")
            Case ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers", "IA64")
          End Select
        Case &H101D1
          Select Case arch
            Case ArchitectureList.x86 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "W32X86")
            Case ArchitectureList.amd64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "x64")
            Case ArchitectureList.ia64 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "prtprocs", "IA64")
          End Select
        Case &H101D2 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32")
        Case &H101D3 : sEnv = IO.Path.Combine(pathWin, "Windows", "System32", "spool", "drivers", "color")
      End Select
    Else
      sEnv = Environ(sInput)
    End If
    If Not String.IsNullOrEmpty(sEnv) Then Return sEnv
    Dim sINF() As String = IO.File.ReadAllLines(OriginalINF)
    For Each line In sINF
      If line.TrimStart.ToLower.StartsWith(sInput.ToLower) And line.Contains("=") Then
        Dim sVal As String = line.Substring(line.IndexOf("=") + 1)
        sVal = CleanupINFString(sVal)
        Return GetEnvPath(sVal, OriginalINF, arch)
      End If
    Next
    Return sInput
  End Function
  Private Function CleanupINFString(Value As String) As String
    Dim sVal As String = Value.TrimStart
    Do While sVal.StartsWith("""")
      sVal = sVal.Substring(1)
    Loop
    If sVal.Contains(";") Then sVal = sVal.Substring(0, sVal.IndexOf(";"))
    sVal = sVal.TrimEnd
    Do While sVal.EndsWith("""")
      sVal = sVal.Substring(0, sVal.Length - 1)
    Loop
    sVal.Trim()
    Return sVal
  End Function
  Private Function GuessPath(FileName As String, DefaultPaths() As String, INFPath As String) As String
    Do While FileName.StartsWith("\")
      FileName = FileName.Substring(1)
    Loop
    Dim pathLocal As String = IO.Path.Combine(IO.Path.GetDirectoryName(INFPath), FileName)
    If IO.File.Exists(pathLocal) Then Return pathLocal
    If DefaultPaths IsNot Nothing Then
      For Each path In DefaultPaths
        Dim pathDefault As String = IO.Path.Combine(path, FileName)
        If IO.File.Exists(pathDefault) Then Return pathDefault
      Next
    End If
    Dim pathSys As String = IO.Path.GetDirectoryName(INFPath)
    If pathSys.ToLower.Contains("system32") Then
      pathSys = pathSys.Substring(0, pathSys.ToLower.LastIndexOf("system32") + 8)
      pathSys = IO.Path.Combine(pathSys, FileName)
      If IO.File.Exists(pathSys) Then Return pathSys
    End If
    Dim pathWin As String = IO.Path.GetDirectoryName(INFPath)
    If pathWin.ToLower.Contains("windows") Then
      pathWin = pathWin.Substring(0, pathWin.ToLower.LastIndexOf("windows") + 7)
      pathWin = IO.Path.Combine(pathWin, FileName)
      If IO.File.Exists(pathWin) Then Return pathWin
    End If
    If FileName.Contains("\") Then
      Return GuessPath(FileName.Substring(FileName.IndexOf("\") + 1), DefaultPaths, INFPath)
    Else
      Return FileName
    End If
  End Function
  Public Function GetDriverIcon(DriverINFPath As String, architecture As ArchitectureList) As Icon
    Dim sINF() As String = IO.File.ReadAllLines(DriverINFPath)
    Dim sResPath As String = Nothing
    Dim sDefPaths As New List(Of String)
    For Each sLine In sINF
      If sLine.ToLower.Contains("defaultdestdir") And sLine.Contains("=") And sDefPaths.Count = 0 Then
        Dim sParts() As String = Split(sLine, "=", 2)
        If sParts(1).Contains(",") Then
          Dim sPaths() As String = Split(sParts(1), ",")
          For Each sPath In sPaths
            sPath = CleanupINFString(sPath)
            sDefPaths.Add(ConvertPathVars(sPath, DriverINFPath, architecture))
          Next
        Else
          Dim sPath As String = CleanupINFString(sParts(1))
          sDefPaths.Add(ConvertPathVars(sPath, DriverINFPath, architecture))
        End If
      End If
      If sLine.ToLower.Contains("hkr") And sLine.Contains(",") And (sLine.ToLower.Contains("installer32") Or sLine.ToLower.Contains("enumproppages32")) And String.IsNullOrEmpty(sResPath) Then
        Dim sParts() As String = Split(sLine, ",", 5)
        If sParts.Length = 5 Then
          If sParts(2).ToLower = "installer32" Or sParts(2).ToLower = "enumproppages32" Then
            sResPath = CleanupINFString(sParts(4))
            If sResPath.Contains(",") Then sResPath = sResPath.Substring(0, sResPath.IndexOf(","))
            sResPath = ConvertPathVars(sResPath, DriverINFPath, architecture)
          End If
        End If
      End If
    Next
    For Each sLine In sINF
      If (sLine.ToLower.Contains("deviceicon") Or sLine.ToLower.Contains("devicebrandingicon")) And sLine.Contains(",") Then
        Dim sParts() As String = Split(sLine, ",", 5)
        If sParts.Length = 5 Then
          If sParts(0).ToLower = "deviceicon" Or sParts(0).ToLower = "devicebrandingicon" Then
            Dim sIconPath As String = CleanupINFString(sParts(4))
            Dim sIconNum As String = "0"
            If sIconPath.Contains(",") Then
              sIconNum = sIconPath.Substring(sIconPath.IndexOf(",") + 1)
              sIconPath = sIconPath.Substring(0, sIconPath.IndexOf(","))
            End If
            sIconPath = ConvertPathVars(CleanupINFString(sIconPath), DriverINFPath, architecture)
            sIconNum = CleanupINFString(sIconNum)
            If Not sIconPath.Contains(":\") Then sIconPath = GuessPath(sIconPath, sDefPaths.ToArray, DriverINFPath)
            If IO.File.Exists(sIconPath) Then
              Dim icoPtr(0) As IntPtr
              Dim siNumber As Int16 = Int16.Parse(sIconNum)
              If ExtractIconEx(sIconPath, siNumber, Nothing, icoPtr, 1) > 0 Then
                Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                DestroyIcon(icoPtr(0))
                Return ico
              End If
            End If
          End If
        End If
      End If
      If sLine.ToLower.Contains("hkr") And sLine.Contains(",") And sLine.ToLower.Contains("icons") Then
        Dim sParts() As String = Split(sLine, ",", 5)
        If sParts.Length = 5 Then
          If sParts(2).ToLower = "icons" Then
            Dim sIconPath As String = CleanupINFString(sParts(4))
            Dim sIconParts() As String = Split(sIconPath, ",", 2)
            Dim siFilePath As String = ConvertPathVars(sIconParts(0), DriverINFPath, architecture)
            If Not siFilePath.Contains(":\") Then siFilePath = GuessPath(siFilePath, sDefPaths.ToArray, DriverINFPath)
            If IO.File.Exists(siFilePath) Then
              Dim siNumber As Int16 = Int16.Parse(sIconParts(1))
              Dim icoPtr(0) As IntPtr
              If ExtractIconEx(siFilePath, siNumber, Nothing, icoPtr, 1) > 0 Then
                Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                DestroyIcon(icoPtr(0))
                Return ico
              End If
            End If
          End If
        End If
      End If
      If sLine.ToLower.Contains("hkr") And sLine.Contains(",") And sLine.ToLower.Contains("icon") Then
        Dim sParts() As String = Split(sLine, ",", 5)
        If sParts.Length = 5 Then
          If sParts(2).ToLower = "icon" Then
            Dim sIcon As String = CleanupINFString(sParts(4))
            If IsNumeric(sIcon) Then
              If sIcon.StartsWith("-") Then
                Dim icoPtr(0) As IntPtr
                Dim siNumber As Int16 = Int16.Parse(sIcon)
                If siNumber = -1 Then siNumber = 0
                Dim sSetupAPI As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "setupapi.dll")
                If ExtractIconEx(sSetupAPI, siNumber, Nothing, icoPtr, 1) > 0 Then
                  Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                  DestroyIcon(icoPtr(0))
                  Return ico
                End If
              ElseIf Not String.IsNullOrEmpty(sResPath) Then
                sIcon = "-" & sIcon
                If Not sResPath.Contains(":\") Then sResPath = GuessPath(sResPath, sDefPaths.ToArray, DriverINFPath)
                If IO.File.Exists(sResPath) Then
                  Dim icoPtr(0) As IntPtr
                  If ExtractIconEx(sResPath, Int16.Parse(sIcon), Nothing, icoPtr, 1) > 0 Then
                    Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                    DestroyIcon(icoPtr(0))
                    Return ico
                  End If
                End If
              End If
            End If
          End If
        End If
      End If
    Next
    Return Nothing
  End Function
  Public Function GetDriverCompanyIcon(Company As String) As Icon
    Select Case Company.ToLower
      Case "acer" : Return My.Resources.company_acer
      Case "adaptec" : Return My.Resources.company_adaptec
      Case "ads technologies" : Return My.Resources.company_ads_technologies
      Case "advanced micro devices" : Return My.Resources.company_advanced_micro_devices
      Case "afatech" : Return My.Resources.company_afatech
      Case "asustek computer" : Return My.Resources.company_asustek_computer
      Case "avermedia technologies" : Return My.Resources.company_avermedia_technologies
      Case "bitland" : Return My.Resources.company_bitland
      Case "brother" : Return My.Resources.company_brother
      Case "canon" : Return My.Resources.company_canon
      Case "compro technology" : Return My.Resources.company_compro_technology
      Case "conexant" : Return My.Resources.company_conexant
      Case "creatix" : Return My.Resources.company_creatix
      Case "dell" : Return My.Resources.company_dell
      Case "elan" : Return My.Resources.company_elan
      Case "emulex" : Return My.Resources.company_emulex
      Case "epson" : Return My.Resources.company_epson
      Case "fuji xerox" : Return My.Resources.company_fuji_xerox
      Case "gigabyte" : Return My.Resources.company_gigabyte
      Case "hannspree" : Return My.Resources.company_hannspree
      Case "hauppauge" : Return My.Resources.company_hauppauge
      Case "hewlett-packard" : Return My.Resources.company_hewlett_packard
      Case "ibm corporation" : Return My.Resources.company_ibm_corporation
      Case "icp vortex" : Return My.Resources.company_icp_vortex
      Case "intel" : Return My.Resources.company_intel
      Case "knc one" : Return My.Resources.company_knc_one
      Case "konica minolta" : Return My.Resources.company_konica_minolta
      Case "kworld" : Return My.Resources.company_kworld
      Case "kyocera" : Return My.Resources.company_kyocera
      Case "lexmark international" : Return My.Resources.company_lexmark_international
      Case "logitech" : Return My.Resources.company_logitech
      Case "lsi" : Return My.Resources.company_lsi
      Case "lumanate" : Return My.Resources.company_lumanate
      Case "microsoft" : Return My.Resources.company_microsoft
      Case "nvidia" : Return My.Resources.company_nvidia
      Case "nxp semiconductors" : Return My.Resources.company_nxp_semiconductors
      Case "oki" : Return My.Resources.company_oki
      Case "panasonic" : Return My.Resources.company_panasonic
      Case "pinnacle systems" : Return My.Resources.company_pinnacle_systems
      Case "promise technology" : Return My.Resources.company_promise_technology
      Case "qlogic" : Return My.Resources.company_qlogic
      Case "qualcomm atheros" : Return My.Resources.company_qualcomm_atheros
      Case "ralink" : Return My.Resources.company_ralink
      Case "realtek" : Return My.Resources.company_realtek
      Case "sceptre" : Return My.Resources.company_sceptre
      Case "ricoh" : Return My.Resources.company_ricoh
      Case "samsung" : Return My.Resources.company_samsung
      Case "savin" : Return My.Resources.company_savin
      Case "silicon integrated systems corp" : Return My.Resources.company_silicon_integrated_systems_corp
      Case "sharp" : Return My.Resources.company_sharp
      Case "sony" : Return My.Resources.company_sony
      Case "synaptics" : Return My.Resources.company_synaptics
      Case "terratec electronic" : Return My.Resources.company_terratec_electronic
      Case "toshiba" : Return My.Resources.company_toshiba
      Case "via technologies" : Return My.Resources.company_via_technologies
      Case "vidzmedia pte ltd" : Return My.Resources.company_vidzmedia_pte_ltd
      Case "vixs systems" : Return My.Resources.company_vixs_systems
      Case "xerox" : Return My.Resources.company_xerox
      Case Else : Return My.Resources.company_oem
    End Select
  End Function
  Public Function GetDriverClassIcon(ClassGUID As String, DriverINFPath As String, architecture As ArchitectureList) As Icon
    If Not String.IsNullOrEmpty(DriverINFPath) AndAlso IO.File.Exists(DriverINFPath) Then
      Dim sINF() As String = IO.File.ReadAllLines(DriverINFPath)
      Dim sResPath As String = Nothing
      Dim sDefPaths As New List(Of String)
      For Each sLine In sINF
        If sLine.ToLower.Contains("defaultdestdir") And sLine.Contains("=") And sDefPaths.Count = 0 Then
          Dim sParts() As String = Split(sLine, "=", 2)
          If sParts(1).Contains(",") Then
            Dim sPaths() As String = Split(sParts(1), ",")
            For Each sPath In sPaths
              sPath = CleanupINFString(sPath)
              sDefPaths.Add(ConvertPathVars(sPath, DriverINFPath, architecture))
            Next
          Else
            Dim sPath As String = CleanupINFString(sParts(1))
            sDefPaths.Add(ConvertPathVars(sPath, DriverINFPath, architecture))
          End If
        End If
        If sLine.ToLower.Contains("hkr") And sLine.Contains(",") And (sLine.ToLower.Contains("installer32") Or sLine.ToLower.Contains("enumproppages32")) And String.IsNullOrEmpty(sResPath) Then
          Dim sParts() As String = Split(sLine, ",", 5)
          If sParts.Length = 5 Then
            If sParts(2).ToLower = "installer32" Or sParts(2).ToLower = "enumproppages32" Then
              sResPath = CleanupINFString(sParts(4))
              If sResPath.Contains(",") Then sResPath = sResPath.Substring(0, sResPath.IndexOf(","))
              sResPath = ConvertPathVars(sResPath, DriverINFPath, architecture)
            End If
          End If
        End If
      Next
      For Each sLine In sINF
        If sLine.ToLower.Contains("devicebrandingicon") And sLine.Contains(",") Then
          Dim sParts() As String = Split(sLine, ",", 5)
          If sParts.Length = 5 Then
            If sParts(0).ToLower = "devicebrandingicon" Then
              Dim sIconPath As String = CleanupINFString(sParts(4))
              Dim sIconNum As String = "0"
              If sIconPath.Contains(",") Then
                sIconNum = sIconPath.Substring(sIconPath.IndexOf(",") + 1)
                sIconPath = sIconPath.Substring(0, sIconPath.IndexOf(","))
              End If
              sIconPath = ConvertPathVars(CleanupINFString(sIconPath), DriverINFPath, architecture)
              sIconNum = CleanupINFString(sIconNum)
              If Not sIconPath.Contains(":\") Then sIconPath = GuessPath(sIconPath, sDefPaths.ToArray, DriverINFPath)
              If IO.File.Exists(sIconPath) Then
                Dim icoPtr(0) As IntPtr
                Dim siNumber As Int16 = Int16.Parse(sIconNum)
                If ExtractIconEx(sIconPath, siNumber, Nothing, icoPtr, 1) > 0 Then
                  Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                  DestroyIcon(icoPtr(0))
                  Return ico
                End If
              End If
            End If
          End If
        End If
        If sLine.ToLower.Contains("hkr") And sLine.Contains(",") And sLine.ToLower.Contains("icon") Then
          Dim sParts() As String = Split(sLine, ",", 5)
          If sParts.Length = 5 Then
            If sParts(2).ToLower = "icon" Then
              Dim sIcon As String = CleanupINFString(sParts(4))
              If IsNumeric(sIcon) Then
                If sIcon.StartsWith("-") Then
                  Dim icoPtr(0) As IntPtr
                  Dim siNumber As Int16 = Int16.Parse(sIcon)
                  If siNumber = -1 Then siNumber = 0
                  Dim sSetupAPI As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "setupapi.dll")
                  If ExtractIconEx(sSetupAPI, siNumber, Nothing, icoPtr, 1) > 0 Then
                    Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                    DestroyIcon(icoPtr(0))
                    Return ico
                  End If
                ElseIf Not String.IsNullOrEmpty(sResPath) Then
                  sIcon = "-" & sIcon
                  If Not sResPath.Contains(":\") Then sResPath = GuessPath(sResPath, sDefPaths.ToArray, DriverINFPath)
                  If IO.File.Exists(sResPath) Then
                    Dim icoPtr(0) As IntPtr
                    If ExtractIconEx(sResPath, Int16.Parse(sIcon), Nothing, icoPtr, 1) > 0 Then
                      Dim ico As Icon = Icon.FromHandle(icoPtr(0)).Clone
                      DestroyIcon(icoPtr(0))
                      Return ico
                    End If
                  End If
                End If
              End If
            End If
          End If
        End If
      Next
    End If
    Dim imageListData As New SP_CLASSIMAGELIST_DATA
    imageListData.cbSize = Marshal.SizeOf(imageListData)
    SetupDiGetClassImageList(imageListData)
    Try
      Dim idxIcon As Integer
      If SetupDiGetClassImageIndex(imageListData, New Guid(ClassGUID), idxIcon) Then
        Dim ptrIcon As IntPtr = ImageList_GetIcon(imageListData.ImageList, idxIcon, 0)
        If Not ptrIcon = IntPtr.Zero Then
          Dim icoClass As Icon = Icon.FromHandle(ptrIcon).Clone
          DestroyIcon(ptrIcon)
          If icoClass.Width > 0 Then Return icoClass
        End If
      End If
      Return My.Resources.inf
    Finally
      SetupDiDestroyClassImageList(imageListData)
    End Try
  End Function
  Public Function ByteSize(InBytes As UInt64) As String
    If InBytes > 1024 Then
      If InBytes / 1024 > 1024 Then
        If InBytes / 1024 / 1024 > 1024 Then
          If InBytes / 1024 / 1024 / 1024 > 1024 Then
            Return Format((InBytes) / 1024 / 1024 / 1024 / 1024, "0.0#") & " TB"
          Else
            Return Format((InBytes) / 1024 / 1024 / 1024, "0.0#") & " GB"
          End If
        Else
          Return Format((InBytes) / 1024 / 1024, "0.0#") & " MB"
        End If
      Else
        Return Format((InBytes) / 1024, "0.0#") & " KB"
      End If
    Else
      Return (InBytes) & " B"
    End If
  End Function
  Private Function getTotalSize(ioDir As IO.DirectoryInfo) As UInt64
    Dim uSize As UInt64 = 0
    For Each ioFile As IO.FileInfo In ioDir.EnumerateFiles
      uSize += ioFile.Length
    Next
    Return uSize
  End Function
  Public ReadOnly Property AIKDir As String
    Get
      Dim toolsDir As String = Application.StartupPath & IO.Path.DirectorySeparatorChar & "tools" & IO.Path.DirectorySeparatorChar
      If Environment.Is64BitProcess Then
        toolsDir &= "amd64" & IO.Path.DirectorySeparatorChar
      Else
        toolsDir &= "x86" & IO.Path.DirectorySeparatorChar
      End If
      Return toolsDir
    End Get
  End Property
  Public ReadOnly Property DismPath As String
    Get
      Dim localDir As String = AIKDir
      localDir &= "Dism.exe"
      Dim systemDir As String = Environment.SystemDirectory & IO.Path.DirectorySeparatorChar & "Dism.exe"
      If IO.File.Exists(systemDir) Then
        Dim sysInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(systemDir)
        Dim locInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(localDir)
        If CompareMSVersions(sysInfo.ProductVersion, locInfo.ProductVersion) > -1 Then
          Return systemDir
        End If
      End If
      Return localDir
    End Get
  End Property
  Public Function ShortenPath(Path As String) As String
    Dim count As Integer = GetShortPathName(Path, Nothing, 0)
    Dim strShortPath As String = Space(count)
    Dim ret As Integer = GetShortPathName(Path, strShortPath, count)
    If ret > 0 Then
      Return strShortPath.Substring(0, ret)
    Else
      Return Path
    End If
  End Function
  Public Function NumericVal(value As String) As Long
    Dim sSize As String = value
    For J As Integer = sSize.Length - 1 To 0 Step -1
      If Not IsNumeric(sSize(J)) Then sSize = sSize.Substring(0, J) & sSize.Substring(J + 1)
    Next
    If String.IsNullOrEmpty(sSize) Then Return 0
    Dim lRet As Long
    If Long.TryParse(sSize, lRet) Then Return lRet
    Return 0
  End Function
  Public Sub SlowDeleteDirectory(Directory As String, OnDirectoryNotEmpty As FileIO.DeleteDirectoryOption)
    If Not IO.Directory.Exists(Directory) Then Return
    If OnDirectoryNotEmpty = FileIO.DeleteDirectoryOption.ThrowIfDirectoryNonEmpty Then
      If IO.Directory.GetFileSystemEntries(Directory).Count > 0 Then Return
      IO.Directory.Delete(Directory, False)
    Else
      Dim doProg As Boolean = True
      If Not frmMain.pbTotal.Visible Then doProg = False
      If frmMain.pbTotal.Value > 0 And frmMain.pbTotal.Maximum > 1 Then doProg = False
      Dim sDirs() As String = IO.Directory.GetDirectories(Directory)
      Dim sFiles() As String = IO.Directory.GetFiles(Directory)
      If sDirs.Count > 1 And doProg Then frmMain.SetTotal(0, sDirs.Count - 1)
      For I As Integer = 0 To sDirs.Count - 1
        If sDirs.Count > 1 And doProg Then frmMain.SetTotal(I, sDirs.Count - 1)
        SlowDeleteDirectory(sDirs(I), OnDirectoryNotEmpty)
        If I Mod 25 = 0 Then
          If frmMain.StopRun Then Return
          Application.DoEvents()
        End If
      Next
      If sFiles.Count > 0 Then
        If sFiles.Count > 1 And doProg Then frmMain.SetProgress(0, sFiles.Count - 1)
        For I As Integer = 0 To sFiles.Count - 1
          If sFiles.Count > 1 And doProg Then frmMain.SetProgress(I, sFiles.Count - 1)
          Try
            IO.File.Delete(sFiles(I))
          Catch ex As Exception
          End Try
          If I Mod 25 = 0 Then
            If frmMain.StopRun Then Return
            Application.DoEvents()
          End If
        Next
      End If
      Try
        IO.Directory.Delete(Directory)
      Catch ex As Exception
      End Try
    End If
  End Sub
  Private c_SlowCopyRet As New List(Of String)
  Public Function SlowCopyFile(File As String, Destination As String, Optional Move As Boolean = False) As Boolean
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncSlowCopyFile))
    Dim cIndex As Integer = c_SlowCopyRet.Count
    c_SlowCopyRet.Add(0)
    tRunWithReturn.Start({File, Destination, Move, cIndex})
    Do While IsNumeric(c_SlowCopyRet(cIndex))
      If frmMain.pbIndividual.Value = 0 And frmMain.pbIndividual.Maximum = 1 Then
        Dim iPercent As Integer
        If Integer.TryParse(c_SlowCopyRet(cIndex), iPercent) Then
          If iPercent > 990 Then
            frmMain.SetProgress(0, 0)
          Else
            frmMain.SetProgress(iPercent, 1000)
          End If
        End If
      End If
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If frmMain.StopRun Then Return False
    Loop
    Dim sRet As String = c_SlowCopyRet(cIndex)
    c_SlowCopyRet(cIndex) = Nothing
    If sRet = "OK" Then
      Return True
    Else
      MsgDlg(frmMain, sRet, "Unable to " & IIf(Move, "move ", "copy ") & IO.Path.GetFileNameWithoutExtension(File) & ".", "File Transfer Failure", MessageBoxButtons.OK, TaskDialogIcon.HardDrive, "Slow Copy File Transfer Failure")
      Return False
    End If
  End Function
  Private Sub AsyncSlowCopyFile(Obj As Object)
    Dim File As String = Obj(0)
    Dim Destination As String = Obj(1)
    Dim Move As Boolean = Obj(2)
    Dim cIndex As UInteger = Obj(3)
    If IO.File.Exists(File) Then
      If Not IO.Directory.Exists(IO.Path.GetDirectoryName(Destination)) Then IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(Destination))
      If IO.File.Exists(Destination) Then IO.File.Delete(Destination)
      If Move AndAlso File(0) = Destination(0) Then
        My.Computer.FileSystem.MoveFile(File, Destination, True)
      Else
        Dim destDrive As New IO.DriveInfo(Destination(0))
        Dim destFreeSpace As Long = destDrive.AvailableFreeSpace
        Const iChunkSize As Integer = 16 * 1024
        Using ioSource As New IO.FileStream(File, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read, iChunkSize * 4, True)
          If ioSource.Length > destFreeSpace Then
            c_SlowCopyRet(cIndex) = "Not Enough Space on Drive " & Destination(0) & ":\"
            Return
          End If
          Using ioReader As New IO.BinaryReader(ioSource)
            Using ioDest As New IO.FileStream(Destination, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None, iChunkSize * 4, True)
              Using ioWriter As New IO.BinaryWriter(ioDest)
                Dim lastPercent As Integer = 0
                Do Until ioReader.BaseStream.Position >= ioReader.BaseStream.Length
                  Dim lReadLeft As Long = ioReader.BaseStream.Length - ioReader.BaseStream.Position
                  If lReadLeft < iChunkSize Then
                    Dim iReadLeft As Integer = 0
                    If lReadLeft > Integer.MaxValue Then
                      iReadLeft = Integer.MaxValue
                    Else
                      iReadLeft = Convert.ToInt32(lReadLeft)
                    End If
                    Dim bChunk() As Byte = ioReader.ReadBytes(iReadLeft)
                    Try
                      ioWriter.Write(bChunk)
                    Catch ex As Exception
                      c_SlowCopyRet(cIndex) = ex.Message
                    End Try
                  Else
                    Dim bChunk() As Byte = ioReader.ReadBytes(iChunkSize)
                    Try
                      ioWriter.Write(bChunk)
                    Catch ex As Exception
                      c_SlowCopyRet(cIndex) = ex.Message
                    End Try
                  End If
                  Dim iPercent As Integer = Math.Floor((ioReader.BaseStream.Position / ioReader.BaseStream.Length) * 1000)
                  If Not iPercent = lastPercent Then
                    c_SlowCopyRet(cIndex) = iPercent
                    lastPercent = iPercent
                  End If
                Loop
                c_SlowCopyRet(cIndex) = "1000"
                ioWriter.Flush()
                ioWriter.Close()
              End Using
            End Using
            ioReader.Close()
          End Using
        End Using
        If Move Then IO.File.Delete(File)
      End If
      c_SlowCopyRet(cIndex) = "OK"
    Else
      c_SlowCopyRet(cIndex) = "File Not Found"
    End If
  End Sub
  Public Enum UpdateType
    MSU
    CAB
    LIP
    LP
    EXE
    MSI
    INF
    Other
  End Enum
  Public Function TypeToString(updateType As UpdateType) As String
    Select Case updateType
      Case modFunctions.UpdateType.MSU : Return "MSU"
      Case modFunctions.UpdateType.CAB : Return "CAB"
      Case modFunctions.UpdateType.LIP : Return "LIP"
      Case modFunctions.UpdateType.LP : Return "LP"
      Case modFunctions.UpdateType.EXE : Return "EXE"
      Case modFunctions.UpdateType.MSI : Return "MSI"
    End Select
    Return Nothing
  End Function
  Public Function GetUpdateType(Path As String) As UpdateType
    If IO.Path.GetFileName(Path).ToLower = "lp.cab" Then Return UpdateType.LP
    Select Case IO.Path.GetExtension(Path).ToLower
      Case ".cab" : Return UpdateType.CAB
      Case ".mlc" : Return UpdateType.LIP
      Case ".exe" : Return UpdateType.EXE
      Case ".msi" : Return UpdateType.MSI
      Case ".msu" : Return UpdateType.MSU
      Case ".inf" : Return UpdateType.INF
    End Select
    Return UpdateType.Other
  End Function
  Public Function GetUpdateCompression(Path As String) As Extraction.COM.ArchiveFormat.KnownSevenZipFormat
    Select Case IO.Path.GetExtension(Path).ToLower
      Case ".cab", ".mlc", ".msu", ".exe" : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Cab
      Case ".msi" : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Compound
      Case ".iso" : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Udf
      Case Else : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Unknown
    End Select
  End Function
  Public Function GetUpdateCompression(Type As UpdateType) As Extraction.COM.ArchiveFormat.KnownSevenZipFormat
    Select Case Type
      Case UpdateType.CAB, UpdateType.LIP, UpdateType.LP, UpdateType.MSU, UpdateType.EXE : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Cab
      Case UpdateType.MSI : Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Compound
    End Select
    Return Extraction.COM.ArchiveFormat.KnownSevenZipFormat.Unknown
  End Function
  Public Sub SortFiles(ByRef FileList() As String)
    Dim FilesOutOfOrder() As String = FileList
    For a As Integer = 1 To FilesOutOfOrder.Count - 1
      For b As Integer = 0 To FilesOutOfOrder.Count - 2
        If IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b + 1)).ToLower = "install" OrElse NumericVal(IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b + 1))) < NumericVal(IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b))) Then
          Dim tmp As String = FilesOutOfOrder(b)
          FilesOutOfOrder(b) = FilesOutOfOrder(b + 1)
          FilesOutOfOrder(b + 1) = tmp
        End If
      Next
    Next
    FileList = FilesOutOfOrder
  End Sub
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  Public Function ConvertTime(ByVal lngMS As UInt64, Optional ByVal Abbreviated As Boolean = False, Optional ByVal Trimmed As Boolean = True) As String
    Dim lngSeconds As UInt64 = lngMS \ 1000
    Dim lngWeeks As UInt64 = lngSeconds \ (60 * 60 * 24 * 7)
    lngSeconds = lngSeconds Mod (60 * 60 * 24 * 7)
    Dim lngDays As UInt64 = lngSeconds \ (60 * 60 * 24)
    lngSeconds = lngSeconds Mod (60 * 60 * 24)
    Dim lngHours As UInt64 = lngSeconds \ (60 * 60)
    lngSeconds = lngSeconds Mod (60 * 60)
    Dim lngMins As UInt64 = lngSeconds \ 60
    lngSeconds = lngSeconds Mod 60
    If Abbreviated Then
      If Trimmed Then
        If lngWeeks > 0 Then
          Return lngWeeks & "w " & lngDays & "d"
        ElseIf lngDays > 0 Then
          If lngHours > 20 Then
            If lngDays >= 6 Then
              Return "1 w"
            Else
              Return lngDays + 1 & " d"
            End If
          Else
            Return lngDays & IIf(lngHours > 14, "¾ d", IIf(lngHours > 8, "½ d", IIf(lngHours > 2, "¼ d", " d")))
          End If
        ElseIf lngHours > 0 Then
          If lngHours >= 22 Or (lngHours = 21 And lngMins > 50) Then
            Return "1 d"
          ElseIf lngMins > 50 Then
            Return lngHours + 1 & " h"
          Else
            Return lngHours & IIf(lngMins > 35, "¾ h", IIf(lngMins > 20, "½ h", IIf(lngMins > 5, "¼ h", " h")))
          End If
        ElseIf lngMins > 0 Then
          If lngMins >= 55 Or (lngMins = 54 And lngSeconds > 50) Then
            Return "1 h"
          ElseIf lngSeconds > 50 Then
            Return lngMins + 1 & " m"
          Else
            Return lngMins & IIf(lngSeconds > 35, "¾ m", IIf(lngSeconds > 20, "½ m", IIf(lngSeconds > 5, "¼ m", " m")))
          End If
        Else
          If lngSeconds > 55 Then
            Return "1 m"
          Else
            Return lngSeconds & "s"
          End If
        End If
      Else
        If lngWeeks > 0 Then
          Return lngWeeks & "w " & lngDays & "d " & lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngDays > 0 Then
          Return lngDays & "d " & lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngHours > 0 Then
          Return lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngMins > 0 Then
          Return lngMins & "m " & lngSeconds & "s"
        Else
          Return lngSeconds & "s"
        End If
      End If
    Else
      Dim strWeeks As String = IIf(lngWeeks = 1, vbNullString, "s")
      Dim strDays As String = IIf(lngDays = 1, vbNullString, "s")
      Dim strHours As String = IIf(lngHours = 1, vbNullString, "s")
      Dim strMins As String = IIf(lngMins = 1, vbNullString, "s")
      Dim strSeconds As String = IIf(lngSeconds = 1, vbNullString, "s")
      If Trimmed Then
        If lngWeeks > 0 Then
          Return lngWeeks & " Week" & strWeeks & " and " & lngDays & " Day" & strDays
        ElseIf lngDays > 0 Then
          If lngHours > 20 Then
            If lngDays >= 6 Then
              Return "1 Week"
            Else
              Return lngDays + 1 & " Days"
            End If
          Else
            Return lngDays & IIf(lngHours > 14, " and Three Quarter Days", IIf(lngHours > 8, " and a Half Days", IIf(lngHours > 2, " and a Quarter Days", " Day" & strDays)))
          End If
        ElseIf lngHours > 0 Then
          If lngHours >= 22 Or (lngHours = 21 And lngMins > 50) Then
            Return "1 Day"
          ElseIf lngMins > 50 Then
            Return lngHours + 1 & " Hours"
          Else
            Return lngHours & IIf(lngMins > 35, " and Three Quarter Hours", IIf(lngMins > 20, " and a Half Hours", IIf(lngMins > 5, " and a Quarter Hours", " Hour" & strHours)))
          End If
        ElseIf lngMins > 0 Then
          If lngMins >= 55 Or (lngMins = 54 And lngSeconds > 55) Then
            Return "1 Hour"
          ElseIf lngSeconds > 50 Then
            Return lngMins + 1 & " Minutes"
          Else
            Return lngMins & IIf(lngSeconds > 35, " and Three Quarter Minutes", IIf(lngSeconds > 20, " and a Half Minutes", IIf(lngSeconds > 5, " and a Quarter Minutes", " Minute" & strMins)))
          End If
        Else
          If lngSeconds > 55 Then
            Return "1 Minute"
          Else
            Return lngSeconds & " Second" & strSeconds
          End If
        End If
      Else
        If lngWeeks > 0 Then
          Return lngWeeks & " Week" & strWeeks & ", " & lngDays & " Day" & strDays & ", " & lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngDays > 0 Then
          Return lngDays & " Day" & strDays & ", " & lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngHours > 0 Then
          Return lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngMins > 0 Then
          Return lngMins & " Minute" & strMins & " and " & lngSeconds & " Second" & strSeconds
        Else
          Return lngSeconds & " Second" & strSeconds
        End If
      End If
    End If
  End Function
  Public Function CompareArchitectures(archA As String, typeB As ArchitectureList, neutralEqual As Boolean) As Boolean
    Dim typeA As ArchitectureList
    If archA.ToLower = "ia64" Then
      typeA = ArchitectureList.ia64
    ElseIf archA.Contains("64") Then
      typeA = ArchitectureList.amd64
    ElseIf archA.Contains("86") Or archA.Contains("32") Then
      typeA = ArchitectureList.x86
    ElseIf archA.ToLower = "neutral" Then
      Return neutralEqual
    Else
      Debug.Print("Unknown A Architecture: " & archA)
      typeA = ArchitectureList.x86
    End If
    Return typeA = typeB
  End Function
  Public Function CompareArchitectures(archA As String, archB As String, neutralEqual As Boolean) As Boolean
    Dim typeA As ArchitectureList
    If archA.ToLower = "ia64" Then
      typeA = ArchitectureList.ia64
    ElseIf archA.Contains("64") Then
      typeA = ArchitectureList.amd64
    ElseIf archA.Contains("86") Or archA.Contains("32") Then
      typeA = ArchitectureList.x86
    ElseIf archA.ToLower = "neutral" Then
      If neutralEqual Then Return True
      If archB.ToLower = "neutral" Then Return True
      Return False
    Else
      Debug.Print("Unknown A Architecture: " & archA)
      typeA = ArchitectureList.x86
    End If
    Dim typeB As ArchitectureList
    If archB.ToLower = "ia64" Then
      typeB = ArchitectureList.ia64
    ElseIf archB.Contains("64") Then
      typeB = ArchitectureList.amd64
    ElseIf archB.Contains("86") Or archB.Contains("32") Then
      typeB = ArchitectureList.x86
    ElseIf archB.ToLower = "neutral" Then
      Return neutralEqual
    Else
      Debug.Print("Unknown B Architecture: " & archB)
      typeB = ArchitectureList.x86
    End If
    Return typeA = typeB
  End Function
  Public Function CompareMSVersions(Ver1 As String, Ver2 As String) As Integer
    Dim v1Ver(3) As String
    If Ver1.Contains(".") Then
      For I As Integer = 1 To 3
        v1Ver(0) = Ver1.Split(".")(0)
        If Ver1.Split(".").Count > I Then
          v1Ver(I) = Ver1.Split(".")(I).Trim
        Else
          v1Ver(I) = "0"
        End If
      Next
    ElseIf Ver1.Contains(",") Then
      v1Ver(0) = Ver1.Split(".")(0)
      For I As Integer = 1 To 3
        If Ver1.Split(",").Count > I Then
          v1Ver(I) = Ver1.Split(",")(I).Trim
        Else
          v1Ver(I) = "0"
        End If
      Next
    End If
    Dim v2Ver(3) As String
    If Ver2.Contains(".") Then
      v2Ver(0) = Ver2.Split(".")(0)
      For I As Integer = 1 To 3
        If Ver2.Split(".").Count > I Then
          v2Ver(I) = Ver2.Split(".")(I).Trim
        Else
          v2Ver(I) = "0"
        End If
      Next
    ElseIf Ver2.Contains(",") Then
      v2Ver(0) = Ver2.Split(",")(0)
      For I As Integer = 1 To 3
        If Ver2.Split(",").Count > I Then
          v2Ver(I) = Ver2.Split(",")(I).Trim
        Else
          v2Ver(I) = "0"
        End If
      Next
    End If
    If Val(v1Ver(0)) > Val(v2Ver(0)) Then
      Return 1
    ElseIf Val(v1Ver(0)) = Val(v2Ver(0)) Then
      If Val(v1Ver(1)) > Val(v2Ver(1)) Then
        Return 1
      ElseIf Val(v1Ver(1)) = Val(v2Ver(1)) Then
        If Val(v1Ver(2)) > Val(v2Ver(2)) Then
          Return 1
        ElseIf Val(v1Ver(2)) = Val(v2Ver(2)) Then
          If Val(v1Ver(3)) > Val(v2Ver(3)) Then
            Return 1
          ElseIf Val(v1Ver(3)) = Val(v2Ver(3)) Then
            Return 0
          Else
            Return -1
          End If
        Else
          Return -1
        End If
      Else
        Return -1
      End If
    Else
      Return -1
    End If
  End Function
#Region "Task Dialogs"
  Public Function UpdateSelectionBox(owner As Form, newData As Update_File, oldData As Update_File, ByRef Always As Boolean) As TaskDialogResult
    If TaskDialog.IsPlatformSupported Then
      Dim UpdateName As String = Nothing
      If String.IsNullOrEmpty(oldData.Failure) Then
        If oldData.Name.Contains("Internet Explorer") Then
          UpdateName = "Internet Explorer"
        Else
          UpdateName = oldData.Name
        End If
      End If
      If String.IsNullOrEmpty(UpdateName) Then
        If String.IsNullOrEmpty(newData.Failure) Then
          If newData.Name.Contains("Internet Explorer") Then
            UpdateName = "Internet Explorer"
          Else
            UpdateName = newData.Name
          End If
        End If
      End If
      If String.IsNullOrEmpty(UpdateName) Then
        UpdateName = IO.Path.GetFileNameWithoutExtension(oldData.Path)
      End If
      If CompareMSVersions(newData.KBVersion, oldData.KBVersion) > 0 Then
        Using dlgUpdate As New TaskDialog
          dlgUpdate.Cancelable = False
          dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
          dlgUpdate.Caption = "Replace Older Version?"
          dlgUpdate.InstructionText = "There is already an older version of " & UpdateName & " in the Update List."
          dlgUpdate.StandardButtons = TaskDialogStandardButtons.None
          dlgUpdate.Text = "Click the version you want to keep"
          dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
          dlgUpdate.FooterCheckBoxChecked = False
          dlgUpdate.FooterCheckBoxText = "&Do this for all new versions"
          Dim en As String = ChrW(&H2003)
          Dim em As String = en & en
          Dim sYes As String
          Dim newFInfo As New IO.FileInfo(newData.Path)
          If String.IsNullOrEmpty(newData.Failure) Then
            Dim newDate As String = newData.BuildDate
            If String.IsNullOrEmpty(newDate) Then newDate = newFInfo.LastWriteTime.ToShortDateString
            sYes = "Replace the update with this new version:" & vbNewLine &
              em & newData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newDate
          Else
            sYes = "Replace the update with this new version:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(newData.Path) & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdYes As New CommandLink(
            "cmdYes",
            "Use Newer Version " & newData.KBVersion,
            sYes)
          cmdYes.Default = True
          AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdYes)
          Dim sNo As String
          Dim oldFInfo As New IO.FileInfo(oldData.Path)
          If String.IsNullOrEmpty(oldData.Failure) Then
            Dim oldDate As String = oldData.BuildDate
            If String.IsNullOrEmpty(oldDate) Then oldDate = oldFInfo.LastWriteTime.ToShortDateString
            sNo = "This update will not be replaced:" & vbNewLine &
              em & oldData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldDate
          Else
            sNo = "This update will not be replaced:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(oldData.Path) & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdNo As New CommandLink(
            "cmdNo",
            "Use Older Version " & oldData.KBVersion,
            sNo)
          AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdNo)
          dlgUpdate.OwnerWindowHandle = owner.Handle
          AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
          Dim ret As TaskDialogResult
          Try
            ret = dlgUpdate.Show
          Catch ex As Exception
            Return UpdateSelectionBoxLegacy(owner, newData, oldData)
          End Try
          Always = dlgUpdate.FooterCheckBoxChecked
          Return ret
        End Using
      Else
        Using dlgUpdate As New TaskDialog
          dlgUpdate.Cancelable = False
          dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
          dlgUpdate.Caption = "Replace Newer Version?"
          dlgUpdate.InstructionText = "There is already a newer version of " & UpdateName & " in the Update List."
          dlgUpdate.StandardButtons = TaskDialogStandardButtons.None
          dlgUpdate.Text = "Click the version you want to keep"
          dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
          dlgUpdate.FooterCheckBoxChecked = False
          dlgUpdate.FooterCheckBoxText = "&Do this for all old versions"
          Dim en As String = ChrW(&H2003)
          Dim em As String = en & en
          Dim sYes As String
          Dim newFInfo As New IO.FileInfo(newData.Path)
          If String.IsNullOrEmpty(newData.Failure) Then
            Dim newDate As String = newData.BuildDate
            If String.IsNullOrEmpty(newDate) Then newDate = newFInfo.LastWriteTime.ToShortDateString
            sYes = "Replace the update with this old version:" & vbNewLine &
              em & newData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newDate
          Else
            sYes = "Replace the update with this old version:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(newData.Path) & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdYes As New CommandLink(
            "cmdYes",
            "Use Older Version " & newData.KBVersion,
            sYes)
          AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
          Dim sNo As String
          Dim oldFInfo As New IO.FileInfo(oldData.Path)
          If String.IsNullOrEmpty(oldData.Failure) Then
            Dim oldDate As String = oldData.BuildDate
            If String.IsNullOrEmpty(oldDate) Then oldDate = oldFInfo.LastWriteTime.ToShortDateString
            sNo = "This update will not be replaced:" & vbNewLine &
              em & oldData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldDate
          Else
            sNo = "This update will not be replaced:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(oldData.Path) & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdNo As New CommandLink(
            "cmdNo",
            "Use Newer Version " & oldData.KBVersion,
            sNo)
          cmdNo.Default = True
          AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdNo)
          dlgUpdate.Controls.Add(cmdYes)
          dlgUpdate.OwnerWindowHandle = owner.Handle
          AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
          Dim ret As TaskDialogResult
          Try
            ret = dlgUpdate.Show
          Catch ex As Exception
            Return UpdateSelectionBoxLegacy(owner, newData, oldData)
          End Try
          Always = dlgUpdate.FooterCheckBoxChecked
          Return ret
        End Using
      End If
    Else
      Return UpdateSelectionBoxLegacy(owner, newData, oldData)
    End If
  End Function
  Private Function UpdateSelectionBoxLegacy(owner As Form, newData As Update_File, oldData As Update_File) As TaskDialogResult
    Dim UpdateName As String = Nothing
    If String.IsNullOrEmpty(oldData.Failure) Then
      If oldData.Name.Contains("Internet Explorer") Then
        UpdateName = "Internet Explorer"
      Else
        UpdateName = oldData.Name
      End If
    End If
    If String.IsNullOrEmpty(UpdateName) Then
      If String.IsNullOrEmpty(newData.Failure) Then
        If newData.Name.Contains("Internet Explorer") Then
          UpdateName = "Internet Explorer"
        Else
          UpdateName = newData.Name
        End If
      End If
    End If
    If String.IsNullOrEmpty(UpdateName) Then
      UpdateName = IO.Path.GetFileNameWithoutExtension(oldData.Path)
    End If
    If CompareMSVersions(newData.KBVersion, oldData.KBVersion) > 0 Then
      Dim ret As DialogResult = MessageBox.Show(owner, "There is already an older version of this update in the list." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Older Version?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
      If ret = DialogResult.Yes Then Return TaskDialogResult.Yes
      If ret = DialogResult.No Then Return TaskDialogResult.No
      Return TaskDialogResult.Cancel
    Else
      Dim ret As DialogResult = MessageBox.Show(owner, "There is already a newer version of this update in the list." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Newer Version?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
      If ret = DialogResult.Yes Then Return TaskDialogResult.Yes
      If ret = DialogResult.No Then Return TaskDialogResult.No
      Return TaskDialogResult.Cancel
    End If
  End Function
  Public Enum Comparison
    Newer
    Mixed
    Older
  End Enum
  Public Function IntegratedUpdateSelectionBox(owner As Form, newData As Update_File, oldPData() As Update_Integrated, PList() As ImagePackage, ByRef Always As Boolean, newCompared As Comparison) As TaskDialogResult
    If TaskDialog.IsPlatformSupported Then
      Dim UpdateName As String = Nothing
      If String.IsNullOrEmpty(newData.Failure) Then
        If newData.Name.Contains("Internet Explorer") Then
          UpdateName = "Internet Explorer"
        Else
          UpdateName = newData.Name
        End If
      End If
      If String.IsNullOrEmpty(UpdateName) Then
        UpdateName = IO.Path.GetFileNameWithoutExtension(newData.Path)
      End If
      Select Case newCompared
        Case Comparison.Newer
          Using dlgUpdate As New TaskDialog
            dlgUpdate.Cancelable = True
            dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
            dlgUpdate.Caption = "Replace Older Versions?"
            dlgUpdate.InstructionText = "There are already older versions of " & UpdateName & " integrated into Image Packages."
            dlgUpdate.StandardButtons = TaskDialogStandardButtons.Cancel
            dlgUpdate.Text = "Click the version you want to keep"
            dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
            dlgUpdate.FooterCheckBoxChecked = False
            dlgUpdate.FooterCheckBoxText = "&Do this for all new versions"
            Dim en As String = ChrW(&H2003)
            Dim em As String = en & en
            Dim sYes As String
            Dim newFInfo As New IO.FileInfo(newData.Path)
            If String.IsNullOrEmpty(newData.Failure) Then
              Dim newDate As String = newData.BuildDate
              If String.IsNullOrEmpty(newDate) Then newDate = newFInfo.LastWriteTime.ToShortDateString
              sYes = "Replace the updates with this new version:" & vbNewLine &
                em & newData.DisplayName & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newDate
            Else
              sYes = "Replace the updates with this new version:" & vbNewLine &
                em & IO.Path.GetFileNameWithoutExtension(newData.Path) & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
            End If
            Dim cmdYes As New CommandLink(
              "cmdYes",
              "Use Newer Version " & newData.KBVersion,
              sYes)
            cmdYes.Default = True
            AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
            dlgUpdate.Controls.Add(cmdYes)
            Dim sNo As String
            sNo = "These updates will not be replaced:"
            Dim sOther As String = Nothing
            For I As Integer = 0 To oldPData.Length - 1
              If String.IsNullOrEmpty(oldPData(I).Identity) Then
                If String.IsNullOrEmpty(sOther) Then sOther = "These updates could not be determined:"
                sOther &= vbNewLine & en & PList(I).Name
              Else
                If CompareMSVersions(newData.Ident.Version, oldPData(I).Ident.Version) > 0 Then
                  Dim oldV As String = oldPData(I).Ident.Version
                  If oldV.StartsWith("6.1.") Then oldV = oldV.Substring(4)
                  sNo &= vbNewLine &
                         en & oldPData(I).Ident.Name & " (" & oldV & ")" & vbNewLine &
                         em & "Installed in " & PList(I).Name
                End If
              End If
            Next
            Dim cmdNo As New CommandLink(
              "cmdNo",
              "Use Older Integrated Versions",
              sNo)
            AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
            dlgUpdate.Controls.Add(cmdNo)
            If Not String.IsNullOrEmpty(sOther) Then
              dlgUpdate.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter
              dlgUpdate.DetailsCollapsedLabel = "Show Unknown Versions"
              dlgUpdate.DetailsExpandedLabel = "Hide Unknown Versions"
              dlgUpdate.DetailsExpandedText = sOther & vbNewLine & "The list of Integrated Windows Updates for these Image Packages may not have been loaded or these Editions of Windows may not be supported by this Update."
            End If
            dlgUpdate.OwnerWindowHandle = owner.Handle
            AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
            Dim ret As TaskDialogResult
            Try
              ret = dlgUpdate.Show
            Catch ex As Exception
              Return IntegratedUpdateSelectionBoxLegacy(owner, newData, oldPData, PList, Always)
            End Try
            Always = dlgUpdate.FooterCheckBoxChecked
            Return ret
          End Using
        Case Comparison.Older
          Using dlgUpdate As New TaskDialog
            dlgUpdate.Cancelable = True
            dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
            dlgUpdate.Caption = "Replace Newer Versions?"
            dlgUpdate.InstructionText = "There are already newer versions of " & UpdateName & " integrated into Image Packages."
            dlgUpdate.StandardButtons = TaskDialogStandardButtons.Cancel
            dlgUpdate.Text = "Click the version you want to keep"
            dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
            dlgUpdate.FooterCheckBoxChecked = False
            dlgUpdate.FooterCheckBoxText = "&Do this for all old versions"
            Dim en As String = ChrW(&H2003)
            Dim em As String = en & en
            Dim sYes As String
            Dim newFInfo As New IO.FileInfo(newData.Path)
            If String.IsNullOrEmpty(newData.Failure) Then
              Dim newDate As String = newData.BuildDate
              If String.IsNullOrEmpty(newDate) Then newDate = newFInfo.LastWriteTime.ToShortDateString
              sYes = "Replace the updates with this old version:" & vbNewLine &
                em & newData.DisplayName & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newDate
            Else
              sYes = "Replace the updates with this old version:" & vbNewLine &
                em & IO.Path.GetFileNameWithoutExtension(newData.Path) & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
            End If
            Dim cmdYes As New CommandLink(
              "cmdYes",
              "Use Older Version " & newData.KBVersion,
              sYes)
            AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
            Dim sNo As String
            sNo = "These updates will not be replaced:"
            Dim sOther As String = Nothing
            For I As Integer = 0 To oldPData.Length - 1
              If String.IsNullOrEmpty(oldPData(I).Identity) Then
                If String.IsNullOrEmpty(sOther) Then sOther = "These updates could not be determined:"
                sOther &= vbNewLine & en & PList(I).Name
              Else
                If CompareMSVersions(newData.Ident.Version, oldPData(I).Ident.Version) > 0 Then
                  Dim oldV As String = oldPData(I).Ident.Version
                  If oldV.StartsWith("6.1.") Then oldV = oldV.Substring(4)
                  sNo &= vbNewLine &
                         en & oldPData(I).Ident.Name & " (" & oldV & ")" & vbNewLine &
                         em & "Installed in " & PList(I).Name
                End If
              End If
            Next
            Dim cmdNo As New CommandLink(
              "cmdNo",
              "Use Newer Integrated Versions",
              sNo)
            cmdNo.Default = True
            AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
            dlgUpdate.Controls.Add(cmdNo)
            dlgUpdate.Controls.Add(cmdYes)
            If Not String.IsNullOrEmpty(sOther) Then
              dlgUpdate.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter
              dlgUpdate.DetailsCollapsedLabel = "Show Unknown Versions"
              dlgUpdate.DetailsExpandedLabel = "Hide Unknown Versions"
              dlgUpdate.DetailsExpandedText = sOther & vbNewLine & "The list of Integrated Windows Updates for these Image Packages may not have been loaded or these Editions of Windows may not be supported by this Update."
            End If
            dlgUpdate.OwnerWindowHandle = owner.Handle
            AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
            Dim ret As TaskDialogResult
            Try
              ret = dlgUpdate.Show
            Catch ex As Exception
              Return IntegratedUpdateSelectionBoxLegacy(owner, newData, oldPData, PList, Always)
            End Try
            Always = dlgUpdate.FooterCheckBoxChecked
            Return ret
          End Using
        Case Comparison.Mixed
          Using dlgUpdate As New TaskDialog
            dlgUpdate.Cancelable = True
            dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
            dlgUpdate.Caption = "Replace Other Versions?"
            dlgUpdate.InstructionText = "There are already other versions of " & UpdateName & " integrated into Image Packages."
            dlgUpdate.StandardButtons = TaskDialogStandardButtons.Cancel
            dlgUpdate.Text = "Click the versions you want to keep"
            dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
            Dim en As String = ChrW(&H2003)
            Dim em As String = en & en
            Dim sAll As String
            Dim newFInfo As New IO.FileInfo(newData.Path)
            If String.IsNullOrEmpty(newData.Failure) Then
              Dim newDate As String = newData.BuildDate
              If String.IsNullOrEmpty(newDate) Then newDate = newFInfo.LastWriteTime.ToShortDateString
              sAll = "Replace all updates with this version:" & vbNewLine &
                em & newData.DisplayName & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newDate
            Else
              sAll = "Replace all updates with this version:" & vbNewLine &
                em & IO.Path.GetFileNameWithoutExtension(newData.Path) & vbNewLine &
                em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
                em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
            End If
            Dim cmdAll As New CommandLink(
              "cmdAll",
              "Use Version " & newData.KBVersion,
              sAll)
            AddHandler cmdAll.Click, AddressOf SelectionDialogCommandLink_Click
            Dim sYes As String
            sYes = "These updates will be upgraded:"
            Dim sOther As String = Nothing
            For I As Integer = 0 To oldPData.Length - 1
              If String.IsNullOrEmpty(oldPData(I).Identity) Then
                If String.IsNullOrEmpty(sOther) Then sOther = "These updates could not be determined:"
                sOther &= vbNewLine & en & PList(I).Name
              Else
                If CompareMSVersions(newData.Ident.Version, oldPData(I).Ident.Version) > 0 Then
                  Dim oldV As String = oldPData(I).Ident.Version
                  If oldV.StartsWith("6.1.") Then oldV = oldV.Substring(4)
                  sYes &= vbNewLine &
                          en & oldPData(I).Ident.Name & " (" & oldV & ")" & vbNewLine &
                          em & "Installed in " & PList(I).Name
                End If
              End If
            Next
            If Not sYes.Contains(vbNewLine) Then sYes &= vbNewLine & en & "Unable to determine! See Unknown Versions below"
            Dim cmdYes As New CommandLink(
              "cmdYes",
              "Use Newer Version " & newData.KBVersion & " on Older Updates",
              sYes)
            AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
            cmdYes.Default = True
            Dim sNo As String
            sNo = "These updates will be downgraded:"
            For I As Integer = 0 To oldPData.Length - 1
              If String.IsNullOrEmpty(oldPData(I).Identity) Then Continue For
              If CompareMSVersions(newData.Ident.Version, oldPData(I).Ident.Version) < 0 Then
                Dim oldV As String = oldPData(I).Ident.Version
                If oldV.StartsWith("6.1.") Then oldV = oldV.Substring(4)
                sNo &= vbNewLine &
                       en & oldPData(I).Ident.Name & " (" & oldV & ")" & vbNewLine &
                       em & "Installed in " & PList(I).Name
              End If
            Next
            If Not sNo.Contains(vbNewLine) Then sNo &= vbNewLine & en & "Unable to determine! See Unknown Versions below"
            Dim cmdNo As New CommandLink(
              "cmdNo",
              "Use Older Version " & newData.KBVersion & " on Newer Updates",
              sNo)
            AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
            Dim cmdNone As New CommandLink(
              "cmdNone",
              "Don't Replace Integrated Versions", "Only Image Packages that don't have this update will receive it")
            AddHandler cmdNone.Click, AddressOf SelectionDialogCommandLink_Click
            dlgUpdate.Controls.Add(cmdAll)
            dlgUpdate.Controls.Add(cmdYes)
            dlgUpdate.Controls.Add(cmdNo)
            dlgUpdate.Controls.Add(cmdNone)
            If Not String.IsNullOrEmpty(sOther) Then sOther &= vbNewLine & "The list of Integrated Windows Updates for these Image Packages may not have been loaded or these Editions of Windows may not be supported by this Update."
            For I As Integer = 0 To oldPData.Length - 1
              If String.IsNullOrEmpty(oldPData(I).Identity) Then Continue For
              If CompareMSVersions(newData.Ident.Version, oldPData(I).Ident.Version) = 0 Then
                If String.IsNullOrEmpty(sOther) Then
                  sOther = "These updates are already the same version:"
                Else
                  sOther &= vbNewLine & vbNewLine & "These updates are already the same version:"
                End If
                Dim oldV As String = oldPData(I).Ident.Version
                If oldV.StartsWith("6.1.") Then oldV = oldV.Substring(4)
                sOther &= vbNewLine &
                       en & oldPData(I).Ident.Name & " (" & oldV & ")" & vbNewLine &
                       em & "Installed in " & PList(I).Name
              End If
            Next
            If Not String.IsNullOrEmpty(sOther) Then
              dlgUpdate.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter
              dlgUpdate.DetailsCollapsedLabel = "Show Unknown Versions"
              dlgUpdate.DetailsExpandedLabel = "Hide Unknown Versions"
              dlgUpdate.DetailsExpandedText = sOther
            End If
            dlgUpdate.OwnerWindowHandle = owner.Handle
            AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
            Try
              Return dlgUpdate.Show
            Catch ex As Exception
              Return IntegratedUpdateSelectionBoxLegacy(owner, newData, oldPData, PList, newCompared)
            End Try
          End Using
        Case Else
          Return TaskDialogResult.Cancel
      End Select
    Else
      Return IntegratedUpdateSelectionBoxLegacy(owner, newData, oldPData, PList, newCompared)
    End If
  End Function
  Private Function IntegratedUpdateSelectionBoxLegacy(owner As Form, newData As Update_File, oldPData() As Update_Integrated, PList() As ImagePackage, newCompared As Comparison) As TaskDialogResult
    Dim UpdateName As String = Nothing
    If String.IsNullOrEmpty(newData.Failure) Then
      If newData.Name.Contains("Internet Explorer") Then
        UpdateName = "Internet Explorer"
      Else
        UpdateName = newData.Name
      End If
    End If
    If String.IsNullOrEmpty(UpdateName) Then
      UpdateName = IO.Path.GetFileNameWithoutExtension(newData.Path)
    End If
    Select Case newCompared
      Case Comparison.Newer
        Dim ret As DialogResult = MessageBox.Show(owner, "There are already older versions of this update integrated in the Image Package." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Older Versions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If ret = DialogResult.Yes Then Return TaskDialogResult.Yes
        If ret = DialogResult.No Then Return TaskDialogResult.No
      Case Comparison.Older
        Dim ret As DialogResult = MessageBox.Show(owner, "There are already newer versions of this update integrated in the Image Package." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Newer Versions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If ret = DialogResult.Yes Then Return TaskDialogResult.Yes
        If ret = DialogResult.No Then Return TaskDialogResult.No
      Case Comparison.Mixed
        Dim oldRet As DialogResult = MessageBox.Show(owner, "There are already older versions of this update integrated in the Image Package." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Older Versions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If oldRet = DialogResult.Cancel Then Return TaskDialogResult.Cancel
        Dim newRet As DialogResult = MessageBox.Show(owner, "There are already newer versions of this update integrated in the Image Package." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newData.KBVersion & "?", "Replace Newer Versions?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If newRet = DialogResult.Cancel Then Return TaskDialogResult.Cancel
        If oldRet = DialogResult.Yes And newRet = DialogResult.Yes Then
          Return TaskDialogResult.Ok
        ElseIf oldRet = DialogResult.Yes And newRet = DialogResult.No Then
          Return TaskDialogResult.Yes
        ElseIf oldRet = DialogResult.No And newRet = DialogResult.Yes Then
          Return TaskDialogResult.No
        ElseIf oldRet = DialogResult.No And newRet = DialogResult.No Then
          Return TaskDialogResult.No
        End If
    End Select
    Return TaskDialogResult.Cancel
  End Function
  Private Sub SelectionDialogButton_Click(sender As TaskDialogButton, e As EventArgs)
    Select Case sender.Name
      Case "cmdYes" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Yes)
      Case "cmdNo" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.No)
      Case "cmdCancel" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Cancel)
      Case "cmdClose" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Close)
      Case "cmdAbort" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.CustomButtonClicked)
      Case "cmdOK" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Ok)
      Case "cmdRetry" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Retry)
    End Select
  End Sub
  Private Sub SelectionDialogCommandLink_Click(sender As TaskDialogCommandLink, e As EventArgs)
    Select Case sender.Name
      Case "cmdYes" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Yes)
      Case "cmdNo" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.No)
      Case "cmdAll" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Ok)
      Case "cmdNone" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Close)
    End Select
  End Sub
  Private Class CommandLink
    Inherits TaskDialogCommandLink
    Public Sub New(name As String, text As String)
      MyBase.New(name, text)
    End Sub
    Public Sub New(name As String, text As String, instruction As String)
      MyBase.New(name, text, instruction)
    End Sub
    Public Overrides Function ToString() As String
      Dim str As String
      Dim noLabel As Boolean = String.IsNullOrEmpty(Me.Text)
      Dim noInstruction As Boolean = String.IsNullOrEmpty(Me.Instruction)
      If noLabel And noInstruction Then
        str = String.Empty
      ElseIf Not noLabel And noInstruction Then
        str = Me.Text
      ElseIf noLabel And Not noInstruction Then
        str = Me.Instruction
      Else
        str = Me.Text & vbLf & Me.Instruction
      End If
      Return str
    End Function
  End Class
  Public Enum TaskDialogIcon
    None = 0
    Space = &H1
    File = &H2
    OpenFolder = &H3
    OpenFolder2 = &H4
    ClosedFolder = &H5
    FolderOpening = &H6
    SearchCatalog = &H8
    CatalogFolder = &H9
    CatalogFolderOpening = &HA
    Games = &HE
    EXE = &HF
    SearchFolder = &H12
    AlignedTextFile = &H13
    Envelope = &H14
    PictureViewer = &H15
    MusicSheet = &H16
    VideoClip = &H17
    DefaultPrograms = &H18
    Internet = &H19
    PrinterFolder = &H1A
    ControlPanel = &H1B
    FloppyDrive35 = &H1C
    FloppyDrive525 = &H1D
    CDDrive = &H1E
    NetworkDriveDisconnected = &H1F
    HardDrive = &H20
    NetworkDrive = &H21
    MemoryChip = &H22
    RemovableDrive = &H23
    WindowsDrive = &H24
    DVDDrive = &H25
    DVDR = &H26
    DVDRAM = &H27
    DVDROM = &H28
    DVDRW = &H29
    ZipDrive = &H2A
    TapeDrive = &H2B
    BluDrive = &H2C
    PrinterInternet = &H2D
    Camcorder = &H2E
    Phone = &H2F
    PrinterDefaultFloppy = &H30
    PrinterDefault = &H31
    PrinterDefaultNetwork = &H32
    Printer = &H33
    PrinterFloppy = &H34
    PrinterNetwork = &H35
    TrashFull = &H36
    TrashEmpty = &H37
    DVD = &H38
    Camera = &H39
    CardReader = &H3A
    Padlock = &H3B
    SDCard = &H3C
    CD = &H3D
    CDR = &H3E
    CDROM = &H3F
    CDRW = &H40
    JewelCase = &H41
    MP3Player = &H42
    DLL = &H43
    Batch = &H44
    INI = &H45
    GIF = &H46
    BMP = &H47
    JPEG = &H48
    InternetFolder = &H49
    InternetFolderOpen = &H4A
    UnknownDrive = &H4B
    Fax = &H4C
    Fonts = &H4D
    ShieldUAC = &H4E
    UserAccount = &H4F
    StartMenu = &H50
    Information = &H51
    Key = &H52
    PNG = &H53
    Warning = &H54
    CDMusic = &H55
    Accessibility = &H56
    WindowsUpdate = &H57
    UserAccount2 = &H58
    Delete = &H59
    RichText = &H5A
    WhiteFloppyDrive = &H5B
    SilverScreen = &H5C
    PDA = &H5D
    TextSelection = &H5E
    Scanner = &H5F
    ExternalChip = &H60
    Disabled = &H61
    [Error] = &H62
    Question = &H63
    Run = &H64
    Sleep = &H65
    BlankTextFile = &H66
    Projector = &H67
    ShieldQuestion = &H68
    ShieldError = &H69
    ShieldOK = &H6A
    ShieldWarning = &H6B
    MusicFolder = &H6C
    Computer = &H6D
    Desktop = &H6E
    Defrag = &H6F
    DocumentsFolder = &H70
    PicturesFolder = &H71
    Options = &H72
    FavoritesFolder = &H73
    TaskDialog = &H74
    Recent = &H75
    InternetNetwork = &H78
    UserFolder = &H7B
    FontBigA = &H7C
    FontTTCert = &H7D
    FontTCCert = &H7E
    FontOCert = &H7F
    FontLittleA = &H80
    Fonts2 = &H81
    GuestUser = &H82
    MusicFile = &H83
    PictureFile = &H84
    VideoFile = &H85
    MediaFile = &H86
    DVDMusic = &H87
    DVDVideo = &H88
    CDMusic2 = &H89
    DVDVideoClip = &H8A
    HDDVDVideoClip = &H8B
    BluRayVideoClip = &H8C
    VCDVideoClip = &H8D
    SVCDVideoClip = &H8E
    NetworkFolder = &H8F
    InternetTime = &H90
    SearchComputer = &H91
    CDUnknown = &H92
    ComputerTransfer = &H93
    LibraryStack = &H94
    SystemProperties = &H95
    ResourceMonitor = &H96
    Personalize = &H97
    Network = &H98
    CDBurn = &H9B
    [Default] = &H9D
    FontTT = &H9E
    FontTC = &H9F
    FontO = &HA0
    WindowsUpdate2 = &HA1
    FolderFull = &HA2
    Shortcut = &HA3
    [Shared] = &HA4
    Preferences = &HA5
    FolderPreferences = &HA6
    ZipDriveDisconnected = &HA7
    WindowsPhotoFile = &HA8
    Download = &HA9
    Bad = &HAA
    MoveToNetwork = &HAB
    DVDPlusR = &HAC
    DVDPlusRW = &HAD
    ZipFile = &HAE
    CompressedFile = &HAF
    Sound = &HB0
    Search = &HB1
    UserAccountsFolder = &HB2
    InternetRJ45 = &HB3
    CDMusicPlus = &HB4
    ContactsFolder = &HB5
    Keyboard = &HB6
    DesktopFolder = &HB7
    DownloadsFolder = &HB8
    LinksFolder = &HB9
    GamesFolder = &HBA
    VideoFolder = &HBD
    GreenFolder = &HBE
    EmptyBox = &HBF
    BorderedBox = &HC0
    VideoStandard = &HC1
    VideoWide = &HC2
    ShieldUpdateTime = &HC3
    PrinterSound = &HC4
    PersonalizeComputer = &HC5
    Library = &H3E9
    LibraryDocuments = &H3EA
    LibraryPictures = &H3EB
    LibraryMusic = &H3EC
    LibraryVideos = &H3ED
    TV = &H3F0
    Users = &H3F2
    Homegroup = &H3F5
    Library2 = &H3F6
    FavoritesFolder2 = &H3FC
    ComputerDialog = &H3FD
    ComputerTasks = &H3FE
    Libraries = &H3FF
    Favorites = &H400
    SearchesFolder = &H401
    Album = &H402
    No = &H403
    User = &H405
    DriveUnlocked = &H406
    DriveLocked = &H407
    DriveUnlockedError = &H408
    DriveUnlockedWindows = &H409
    DriveUnlcokedErrorWindows = &H40A
    DriveUnlockedRemovable = &H40B
    DriveLockedRemovable = &H40C
    DriveUnlockedErrorRemovable = &H40D
    TaskDialog2 = &HFF9C
    ShieldUAC2 = &HFFF7
    ShieldOK2 = &HFFF8
    ShieldError2 = &HFFF9
    ShieldWarning2 = &HFFFA
    ShieldUAC3 = &HFFFB
    ShieldUAC4 = &HFFFC
    Information2 = &HFFFD
    Error2 = &HFFFE
    Warning2 = &HFFFF
  End Enum
  Public Function MsgDlg(owner As Form, Text As String, Optional Title As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As TaskDialogIcon = TaskDialogIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing, Optional HelpTopic As String = Nothing) As DialogResult
    If owner.Name = "frmMain" Then
      Dim main As frmMain = owner
      If main.pbTotal.Visible AndAlso main.taskBar IsNot Nothing Then main.taskBar.SetProgressState(main.Handle, TaskbarLib.TBPFLAG.TBPF_ERROR)
    End If
    Try
      If TaskDialog.IsPlatformSupported Then
        Using dlgMessage As New TaskDialog
          dlgMessage.Cancelable = True
          dlgMessage.Caption = "SLIPS7REAM - " & Caption
          dlgMessage.InstructionText = Title
          dlgMessage.Text = Text
          dlgMessage.Icon = Icon
          dlgMessage.HyperlinksEnabled = True
          AddHandler dlgMessage.HyperlinkClick, AddressOf dlg_HyperlinkClick
          Select Case Buttons
            Case MessageBoxButtons.OK
              Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdOK.Default = True
              Else
                cmdOK.Default = False
              End If
              AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdOK)
            Case MessageBoxButtons.YesNo
              Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
              Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdYes.Default = True
                cmdNo.Default = False
              ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
                cmdYes.Default = False
                cmdNo.Default = True
              Else
                cmdYes.Default = False
                cmdNo.Default = False
              End If
              AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdYes)
              dlgMessage.Controls.Add(cmdNo)
            Case MessageBoxButtons.YesNoCancel
              Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
              Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
              Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdYes.Default = True
                cmdNo.Default = False
                cmdCancel.Default = False
              ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
                cmdYes.Default = False
                cmdNo.Default = True
                cmdCancel.Default = False
              Else
                cmdYes.Default = False
                cmdNo.Default = False
                cmdCancel.Default = True
              End If
              AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdYes)
              dlgMessage.Controls.Add(cmdNo)
              dlgMessage.Controls.Add(cmdCancel)
            Case MessageBoxButtons.OKCancel
              Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
              Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdOK.Default = True
                cmdCancel.Default = False
              ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
                cmdOK.Default = False
                cmdCancel.Default = True
              Else
                cmdOK.Default = False
                cmdCancel.Default = False
              End If
              AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdOK)
              dlgMessage.Controls.Add(cmdCancel)
            Case MessageBoxButtons.RetryCancel
              Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
              Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdRetry.Default = True
                cmdCancel.Default = False
              ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
                cmdRetry.Default = False
                cmdCancel.Default = True
              Else
                cmdRetry.Default = False
                cmdCancel.Default = False
              End If
              AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdRetry)
              dlgMessage.Controls.Add(cmdCancel)
            Case MessageBoxButtons.AbortRetryIgnore
              Dim cmdAbort As New TaskDialogButton("cmdNo", "&Abort")
              Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
              Dim cmdIgnore As New TaskDialogButton("cmdClose", "&Ignore")
              If DefaultButton = MessageBoxDefaultButton.Button1 Then
                cmdAbort.Default = True
                cmdRetry.Default = False
                cmdIgnore.Default = False
              ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
                cmdAbort.Default = False
                cmdRetry.Default = True
                cmdIgnore.Default = False
              Else
                cmdAbort.Default = False
                cmdRetry.Default = False
                cmdIgnore.Default = True
              End If
              AddHandler cmdAbort.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
              AddHandler cmdIgnore.Click, AddressOf SelectionDialogButton_Click
              dlgMessage.Controls.Add(cmdAbort)
              dlgMessage.Controls.Add(cmdRetry)
              dlgMessage.Controls.Add(cmdIgnore)
          End Select
          If Not String.IsNullOrEmpty(HelpTopic) Then
            Dim cmdHelp As New TaskDialogButton("cmdHelp", "&Help")
            cmdHelp.Default = False
            AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, HelpTopicPath(HelpTopic))
            dlgMessage.Controls.Add(cmdHelp)
          End If
          If Not String.IsNullOrEmpty(Details) Then
            dlgMessage.DetailsExpanded = False
            dlgMessage.DetailsCollapsedLabel = "View Details"
            dlgMessage.DetailsExpandedLabel = "Hide Details"
            dlgMessage.DetailsExpandedText = Details
            dlgMessage.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandContent
          End If
          If owner IsNot Nothing Then dlgMessage.OwnerWindowHandle = owner.Handle
          AddHandler dlgMessage.Opened, AddressOf RefreshDlg
          Dim ret As TaskDialogResult
          Try
            ret = dlgMessage.Show()
          Catch ex As Exception
            Return MsgDlgLegacy(owner, Text, Title, Caption, Buttons, Icon, DefaultButton, Details, HelpTopic)
          End Try
          Select Case ret
            Case TaskDialogResult.Yes : Return DialogResult.Yes
            Case TaskDialogResult.No : Return IIf(Buttons = MessageBoxButtons.AbortRetryIgnore, DialogResult.Abort, DialogResult.No)
            Case TaskDialogResult.Ok : Return DialogResult.OK
            Case TaskDialogResult.Cancel : Return DialogResult.Cancel
            Case TaskDialogResult.Close : Return DialogResult.Ignore
            Case TaskDialogResult.Retry : Return DialogResult.Retry
          End Select
          Return DialogResult.None
        End Using
      Else
        Return MsgDlgLegacy(owner, Text, Title, Caption, Buttons, Icon, DefaultButton, Details, HelpTopic)
      End If
    Finally
      If owner.Name = "frmMain" Then
        Dim main As frmMain = owner
        If main.pbTotal.Visible AndAlso main.taskBar IsNot Nothing Then main.taskBar.SetProgressState(main.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
      End If
    End Try
  End Function
  Private Sub RefreshDlg(sender As Object, e As EventArgs)
    Dim dlg As TaskDialog = sender
    dlg.Icon = dlg.Icon
    dlg.InstructionText = dlg.InstructionText
  End Sub
  Private Sub dlg_HyperlinkClick(sender As Object, e As TaskDialogHyperlinkClickedEventArgs)
    Try
      Process.Start(e.LinkText)
    Catch ex As Exception
    End Try
  End Sub
  Private Function MsgDlgLegacy(owner As Form, Text As String, Optional Title As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As TaskDialogIcon = TaskDialogIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing, Optional HelpTopic As String = Nothing) As DialogResult
    Dim Content As String
    If String.IsNullOrEmpty(Title) And String.IsNullOrEmpty(Text) Then
      Content = String.Empty
    ElseIf String.IsNullOrEmpty(Title) Then
      Content = Text
    ElseIf String.IsNullOrEmpty(Text) Then
      Content = Title
    Else
      Content = Title & vbNewLine & vbNewLine & Text
    End If
    Dim msgIcon As MessageBoxIcon = MessageBoxIcon.None
    Select Case Icon
      Case TaskDialogIcon.None : msgIcon = MessageBoxIcon.None
      Case TaskDialogIcon.Error, TaskDialogIcon.Error2, TaskDialogIcon.ShieldError, TaskDialogIcon.ShieldError2 : msgIcon = MessageBoxIcon.Error
      Case TaskDialogIcon.Question, TaskDialogIcon.ShieldQuestion : msgIcon = MessageBoxIcon.Question
      Case TaskDialogIcon.Warning, TaskDialogIcon.Warning2, TaskDialogIcon.ShieldWarning, TaskDialogIcon.ShieldWarning2 : msgIcon = MessageBoxIcon.Warning
      Case TaskDialogIcon.Information, TaskDialogIcon.Information2 : msgIcon = MessageBoxIcon.Information
      Case Else
        Select Case Buttons
          Case MessageBoxButtons.YesNo, MessageBoxButtons.YesNoCancel : msgIcon = MessageBoxIcon.Question
          Case MessageBoxButtons.OKCancel : msgIcon = MessageBoxIcon.Information
          Case MessageBoxButtons.RetryCancel : msgIcon = MessageBoxIcon.Error
          Case MessageBoxButtons.AbortRetryIgnore : msgIcon = MessageBoxIcon.Warning
        End Select
    End Select
    If Not String.IsNullOrEmpty(HelpTopic) Then
      Return MessageBox.Show(owner, Content, Caption, Buttons, msgIcon, DefaultButton, 0, "S7M.chm", HelpNavigator.Topic, HelpTopicPath(HelpTopic))
    Else
      Return MessageBox.Show(owner, Content, Caption, Buttons, msgIcon, DefaultButton)
    End If
  End Function
  Private Function HelpTopicPath(helpTopic As String) As String
    Select Case helpTopic
      Case "Error Adding Updates" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.1_Error_Adding_Updates.htm"
      Case "Remove All Updates" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.2_Remove_All_Updates.htm"
      Case "No Parse Data Selected" : Return "1_SLIPS7REAM_Interface\1.3_Image_Packages\1.3.1_Parse_Packages.htm"
      Case "Open Folder File Missing" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.3_Open_Folder_Missing.htm#file"
      Case "Open Folder Folder Missing" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.3_Open_Folder_Missing.htm#folder"
      Case "Extraction CRC Error" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.4_Extraction_Error.htm#crc_error"
      Case "Extraction Data Error" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.4_Extraction_Error.htm#data_error"
      Case "Extraction Unsupported Method" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.4_Extraction_Error.htm#unsupported_method"
      Case "Extraction File Not Found" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.4_Extraction_Error.htm#file_not_found"
      Case "Extraction Error" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.4_Extraction_Error.htm#other_error"
      Case "Active Mount" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.5_Active_Mount.htm"
      Case "EFI File Not Found" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.6_EFI_Missing.htm#file_not_found"
      Case "EFI Folder Not Found" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.6_EFI_Missing.htm#folder_not_found"
      Case "Stop Integrating" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#integration"
      Case "Stop Integrating and Close" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#integration_close"
      Case "Stop Loading Package Data" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#packages"
      Case "Stop Loading Package Data and Close" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#packages_close"
      Case "Stop Loading Updates" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#updates"
      Case "Stop Loading Updates and Close" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.7_Interrupt.htm#updates_close"
      Case "Alert File Failure" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.8_Alert_Problem.htm#failure"
      Case "Alert File Missing" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.8_Alert_Problem.htm#missing"
      Case "Temp Folder Not Found" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.9_Temp_Folder_Missing.htm"
      Case "Dependent Features" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.10_Dependent_Features.htm"
      Case "Missing Feature" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.10_Dependent_Features.htm#missing"
      Case "No Features" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.11_Package_Failure.htm#features"
      Case "No Updates" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.11_Package_Failure.htm#updates"
      Case "No Drivers" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.11_Package_Failure.htm#drivers"
      Case "Slow Copy File Transfer Failure" : Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.12_File_Transfer_Failure.htm"
      Case Else
        Debug.Print("No Help Page for """ & helpTopic & """")
        Return "1_SLIPS7REAM_Interface\1.10_Dialogs\1.10.0_Dialogs.htm"
    End Select
  End Function
#End Region
End Module
