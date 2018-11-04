Public Class MySettings
  Private c_TempDir As String
  Private c_Timeout As Integer = 0
  Private c_x86WhiteList() As String = {"Windows6.2-KB2764913-x86", "Windows6.2-KB2764916-x86", "Windows6.3-KB2849697-x86", "Windows6.3-KB2849696-x86"}
  Private c_Position As Point = New Point
  Private c_Size As Size = New Size(440, 480)
  Private c_AutoISOLabel As Boolean = True
  Private c_DefaultISOLabel As String = "GRMCULFRER_EN_DVD"
  Private c_DefaultFS As String = "ISO 9960"
  Private c_Priority As String = "Normal"
  Private c_DefaultSplit As Integer = 0
  Private c_SplitVal As String
  Private c_LastUpdate As Date = New Date(1970, 1, 1)
  Private c_LastNag As Date = New Date(1970, 1, 1)
  Private c_PlayAlertNoise As Boolean
  Private c_AlertNoisePath As String
  Private c_HideDriverOutput As Boolean
  Private c_LoadFeatures As Boolean
  Private c_LoadUpdates As Boolean
  Private c_LoadDrivers As Boolean
  Private c_UpdateColorSuperseded As Color
  Private c_UpdateColorUpgrade As Color
  Private c_UpdateColorRequirement As Color
  Private c_DriverColorPE As Color
  Private c_DriverColorBoot As Color
  Private c_DriverColorBootAndPE As Color
  Public Property TempDir As String
    Get
      Return c_TempDir
    End Get
    Set(value As String)
      c_TempDir = value
      Save()
    End Set
  End Property
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
      Save()
    End Set
  End Property
  Public Property x86WhiteList As String()
    Get
      Return c_x86WhiteList
    End Get
    Set(value As String())
      c_x86WhiteList = value
      Save()
    End Set
  End Property
  Public Property Position As Point
    Get
      Return c_Position
    End Get
    Set(value As Point)
      c_Position = value
      Save()
    End Set
  End Property
  Public Property Size As Size
    Get
      Return c_Size
    End Get
    Set(value As Size)
      c_Size = value
      Save()
    End Set
  End Property
  Public Property AutoISOLabel As Boolean
    Get
      Return c_AutoISOLabel
    End Get
    Set(value As Boolean)
      c_AutoISOLabel = value
    End Set
  End Property
  Public Property DefaultISOLabel As String
    Get
      Return c_DefaultISOLabel
    End Get
    Set(value As String)
      c_DefaultISOLabel = value
      Save()
    End Set
  End Property
  Public Property DefaultFS As String
    Get
      Return c_DefaultFS
    End Get
    Set(value As String)
      c_DefaultFS = value
      Save()
    End Set
  End Property
  Public Property Priority As String
    Get
      Return c_Priority
    End Get
    Set(value As String)
      c_Priority = value
      Save()
    End Set
  End Property
  Public Property DefaultSplit As Integer
    Get
      Return c_DefaultSplit
    End Get
    Set(value As Integer)
      c_DefaultSplit = value
      Save()
    End Set
  End Property
  Public Property SplitVal As String
    Get
      Return c_SplitVal
    End Get
    Set(value As String)
      c_SplitVal = value
      Save()
    End Set
  End Property
  Public Property LastUpdate As Date
    Get
      Return c_LastUpdate
    End Get
    Set(value As Date)
      c_LastUpdate = value
      Save()
    End Set
  End Property
  Public Property LastNag As Date
    Get
      Return c_LastNag
    End Get
    Set(value As Date)
      c_LastNag = value
      Save()
    End Set
  End Property
  Public Property PlayAlertNoise As Boolean
    Get
      Return c_PlayAlertNoise
    End Get
    Set(value As Boolean)
      c_PlayAlertNoise = value
      Save()
    End Set
  End Property
  Public Property AlertNoisePath As String
    Get
      Return c_AlertNoisePath
    End Get
    Set(value As String)
      c_AlertNoisePath = value
      Save()
    End Set
  End Property
  Public Property HideDriverOutput As Boolean
    Get
      Return c_HideDriverOutput
    End Get
    Set(value As Boolean)
      c_HideDriverOutput = value
      Save()
    End Set
  End Property
  Public Property LoadFeatures As Boolean
    Get
      Return c_LoadFeatures
    End Get
    Set(value As Boolean)
      c_LoadFeatures = value
      Save()
    End Set
  End Property
  Public Property LoadUpdates As Boolean
    Get
      Return c_LoadUpdates
    End Get
    Set(value As Boolean)
      c_LoadUpdates = value
      Save()
    End Set
  End Property
  Public Property LoadDrivers As Boolean
    Get
      Return c_LoadDrivers
    End Get
    Set(value As Boolean)
      c_LoadDrivers = value
      Save()
    End Set
  End Property
  Public Property UpdateColorSuperseded As Color
    Get
      Return c_UpdateColorSuperseded
    End Get
    Set(value As Color)
      c_UpdateColorSuperseded = value
      Save()
    End Set
  End Property
  Public Property UpdateColorUpgrade As Color
    Get
      Return c_UpdateColorUpgrade
    End Get
    Set(value As Color)
      c_UpdateColorUpgrade = value
      Save()
    End Set
  End Property
  Public Property UpdateColorRequirement As Color
    Get
      Return c_UpdateColorRequirement
    End Get
    Set(value As Color)
      c_UpdateColorRequirement = value
      Save()
    End Set
  End Property
  Public Property DriverColorPE As Color
    Get
      Return c_DriverColorPE
    End Get
    Set(value As Color)
      c_DriverColorPE = value
      Save()
    End Set
  End Property
  Public Property DriverColorBoot As Color
    Get
      Return c_DriverColorBoot
    End Get
    Set(value As Color)
      c_DriverColorBoot = value
      Save()
    End Set
  End Property
  Public Property DriverColorBootAndPE As Color
    Get
      Return c_DriverColorBootAndPE
    End Get
    Set(value As Color)
      c_DriverColorBootAndPE = value
      Save()
    End Set
  End Property
  Public Sub New()
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
      If My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then
        ReadRegistry()
        Return
      End If
    End If
    ReadLegacy()
  End Sub
  Private Sub ReadRegistry()
    Dim Hive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName))
    If Hive.GetValueNames.Contains("Temp Directory") Then
      c_TempDir = CStr(Hive.GetValue("Temp Directory", My.Settings.TempDir))
    Else
      c_TempDir = My.Settings.TempDir
    End If
    If Hive.GetValueNames.Contains("Timeout") Then
      c_Timeout = CInt(Hive.GetValue("Timeout", My.Settings.Timeout))
    Else
      c_Timeout = My.Settings.Timeout
    End If
    If Hive.GetValueNames.Contains("x86 Whitelist") Then
      c_x86WhiteList = CType(Hive.GetValue("x86 Whitelist", Split(My.Settings.x86WhiteList, vbNewLine)), String())
    Else
      c_x86WhiteList = Split(My.Settings.x86WhiteList, vbNewLine)
    End If
    If Hive.GetSubKeyNames.Contains("Display") Then
      Dim disp As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Display")
      If disp.GetValueNames.Contains("Left") Then
        c_Position.X = CInt(Hive.OpenSubKey("Display").GetValue("Left", My.Settings.Position.X))
      Else
        c_Position.X = My.Settings.Position.X
      End If
      If disp.GetValueNames.Contains("Top") Then
        c_Position.Y = CInt(Hive.OpenSubKey("Display").GetValue("Top", My.Settings.Position.Y))
      Else
        c_Position.Y = My.Settings.Position.Y
      End If
      If disp.GetValueNames.Contains("Width") Then
        c_Size.Width = CInt(Hive.OpenSubKey("Display").GetValue("Width", My.Settings.Size.Width))
      Else
        c_Size.Width = My.Settings.Size.Width
      End If
      If disp.GetValueNames.Contains("Height") Then
        c_Size.Height = CInt(Hive.OpenSubKey("Display").GetValue("Height", My.Settings.Size.Height))
      Else
        c_Size.Height = My.Settings.Size.Height
      End If
    Else
      c_Position = My.Settings.Position
      c_Size = My.Settings.Size
    End If
    If Hive.GetValueNames.Contains("Auto ISO Label") Then
      c_AutoISOLabel = (CStr(Hive.GetValue("Auto ISO Label", IIf(My.Settings.AutoISOLabel, "Y", "N"))) = "Y")
    Else
      c_AutoISOLabel = My.Settings.AutoISOLabel
    End If
    If Hive.GetValueNames.Contains("Default ISO Label") Then
      c_DefaultISOLabel = CStr(Hive.GetValue("Default ISO Label", My.Settings.DefaultISOLabel))
    Else
      c_DefaultISOLabel = My.Settings.DefaultISOLabel
    End If
    If Hive.GetValueNames.Contains("Default File System") Then
      c_DefaultFS = CStr(Hive.GetValue("Default File System", My.Settings.DefaultFS))
    Else
      c_DefaultFS = My.Settings.DefaultFS
    End If
    If Hive.GetValueNames.Contains("Priority") Then
      c_Priority = CStr(Hive.GetValue("Priority", My.Settings.Priority))
    Else
      c_Priority = My.Settings.Priority
    End If
    If Hive.GetValueNames.Contains("Default Split") Then
      c_DefaultSplit = CInt(Hive.GetValue("Default Split", My.Settings.DefaultSplit))
    Else
      c_DefaultSplit = My.Settings.DefaultSplit
    End If
    If Hive.GetValueNames.Contains("Split Value") Then
      c_SplitVal = CStr(Hive.GetValue("Split Value", My.Settings.SplitVal))
    Else
      c_SplitVal = My.Settings.SplitVal
    End If
    If Hive.GetValueNames.Contains("Last Update") Then
      c_LastUpdate = Date.FromBinary(CLng(Hive.GetValue("Last Update", My.Settings.LastUpdate.ToBinary)))
    Else
      c_LastUpdate = My.Settings.LastUpdate
    End If
    If Hive.GetValueNames.Contains("Last Nag") Then
      c_LastNag = Date.FromBinary(CLng(Hive.GetValue("Last Nag", New Date(1970, 1, 1))))
    Else
      c_LastNag = New Date(1970, 1, 1)
    End If
    If Hive.GetValueNames.Contains("Hide Driver Output") Then
      c_HideDriverOutput = (CStr(Hive.GetValue("Hide Driver Output", "Y")) = "Y")
    Else
      c_HideDriverOutput = True
    End If
    If Hive.GetSubKeyNames.Contains("Alert") Then
      Dim alert As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Alert")
      c_PlayAlertNoise = (CStr(alert.GetValue(String.Empty, "N")) = "Y")
      If alert.GetValueNames.Contains("Path") Then
        c_AlertNoisePath = CStr(alert.GetValue("Path", String.Empty))
      Else
        c_AlertNoisePath = String.Empty
      End If
    Else
      c_PlayAlertNoise = False
      c_AlertNoisePath = String.Empty
    End If
    If Hive.GetSubKeyNames.Contains("Load Package Data") Then
      Dim loadPackageData As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Load Package Data")
      If loadPackageData.GetValueNames.Contains("Features") Then
        c_LoadFeatures = (CStr(loadPackageData.GetValue("Features", "N")) = "Y")
      Else
        c_LoadFeatures = False
      End If
      If loadPackageData.GetValueNames.Contains("Updates") Then
        c_LoadUpdates = (CStr(loadPackageData.GetValue("Updates", "Y")) = "Y")
      Else
        c_LoadUpdates = True
      End If
      If loadPackageData.GetValueNames.Contains("Drivers") Then
        c_LoadDrivers = (CStr(loadPackageData.GetValue("Drivers", "N")) = "Y")
      Else
        c_LoadDrivers = False
      End If
    Else
      c_LoadFeatures = False
      c_LoadUpdates = True
      c_LoadDrivers = False
    End If
    If Hive.GetSubKeyNames.Contains("Update Colors") Then
      Dim updateColors As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Update Colors")
      If updateColors.GetValueNames.Contains("Requirement") Then
        c_UpdateColorRequirement = Color.FromArgb(CInt(updateColors.GetValue("Requirement", Color.Red.ToArgb)))
      Else
        c_UpdateColorRequirement = Color.Red
      End If
      If updateColors.GetValueNames.Contains("Upgrade") Then
        c_UpdateColorUpgrade = Color.FromArgb(CInt(updateColors.GetValue("Upgrade", Color.RoyalBlue.ToArgb)))
      Else
        c_UpdateColorUpgrade = Color.RoyalBlue
      End If
      If updateColors.GetValueNames.Contains("Superseded") Then
        c_UpdateColorSuperseded = Color.FromArgb(CInt(updateColors.GetValue("Superseded", Color.DarkOrange.ToArgb)))
      Else
        c_UpdateColorSuperseded = Color.DarkOrange
      End If
    Else
      c_UpdateColorRequirement = Color.Red
      c_UpdateColorUpgrade = Color.RoyalBlue
      c_UpdateColorSuperseded = Color.DarkOrange
    End If
    If Hive.GetSubKeyNames.Contains("Driver Colors") Then
      Dim driverColors As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Driver Colors")
      If driverColors.GetValueNames.Contains("PE") Then
        c_DriverColorPE = Color.FromArgb(CInt(driverColors.GetValue("PE", Color.Blue.ToArgb)))
      Else
        c_DriverColorPE = Color.Blue
      End If
      If driverColors.GetValueNames.Contains("Boot") Then
        c_DriverColorBoot = Color.FromArgb(CInt(driverColors.GetValue("Boot", Color.ForestGreen.ToArgb)))
      Else
        c_DriverColorBoot = Color.ForestGreen
      End If
      If driverColors.GetValueNames.Contains("BootAndPE") Then
        c_DriverColorBootAndPE = Color.FromArgb(CInt(driverColors.GetValue("BootAndPE", Color.Purple.ToArgb)))
      Else
        c_DriverColorBootAndPE = Color.Purple
      End If
    Else
      c_DriverColorPE = Color.Blue
      c_DriverColorBoot = Color.ForestGreen
      c_DriverColorBootAndPE = Color.Purple
    End If
  End Sub
  Private Sub ReadLegacy()
    c_TempDir = My.Settings.TempDir
    c_Timeout = My.Settings.Timeout
    c_x86WhiteList = Split(My.Settings.x86WhiteList, vbNewLine)
    c_Position = My.Settings.Position
    c_Size = My.Settings.Size
    c_AutoISOLabel = True
    c_DefaultISOLabel = My.Settings.DefaultISOLabel
    c_DefaultFS = My.Settings.DefaultFS
    c_Priority = My.Settings.Priority
    c_DefaultSplit = My.Settings.DefaultSplit
    c_SplitVal = My.Settings.SplitVal
    c_LastUpdate = My.Settings.LastUpdate
    c_LastNag = New Date(1970, 1, 1)
    c_PlayAlertNoise = False
    c_AlertNoisePath = String.Empty
    c_HideDriverOutput = True
    c_LoadFeatures = False
    c_LoadUpdates = True
    c_LoadDrivers = False
    c_UpdateColorRequirement = Color.Red
    c_UpdateColorUpgrade = Color.RoyalBlue
    c_UpdateColorSuperseded = Color.DarkOrange
    c_DriverColorPE = Color.Blue
    c_DriverColorBoot = Color.ForestGreen
    c_DriverColorBootAndPE = Color.Purple
    Save()
  End Sub
  Public Sub Save()
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName), True).CreateSubKey(Application.ProductName)
    Dim ActiveHive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName), True)
    ActiveHive.SetValue("Temp Directory", c_TempDir, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Timeout", c_Timeout, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("x86 Whitelist", c_x86WhiteList, Microsoft.Win32.RegistryValueKind.MultiString)
    If Not ActiveHive.GetSubKeyNames.Contains("Display") Then ActiveHive.CreateSubKey("Display")
    ActiveHive.OpenSubKey("Display", True).SetValue("Left", c_Position.X, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Top", c_Position.Y, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Width", c_Size.Width, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Height", c_Size.Height, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("Auto ISO Label", IIf(c_AutoISOLabel, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Default ISO Label", c_DefaultISOLabel, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Default File System", c_DefaultFS, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Priority", c_Priority, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Default Split", c_DefaultSplit, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("Split Value", c_SplitVal, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Last Update", c_LastUpdate.ToBinary, Microsoft.Win32.RegistryValueKind.QWord)
    ActiveHive.SetValue("Last Nag", c_LastNag.ToBinary, Microsoft.Win32.RegistryValueKind.QWord)
    ActiveHive.SetValue("Hide Driver Output", IIf(c_HideDriverOutput, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    If Not ActiveHive.GetSubKeyNames.Contains("Alert") Then ActiveHive.CreateSubKey("Alert")
    ActiveHive.OpenSubKey("Alert", True).SetValue(String.Empty, IIf(c_PlayAlertNoise, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    If String.IsNullOrEmpty(c_AlertNoisePath) Then
      If ActiveHive.OpenSubKey("Alert", True).GetValueNames.Contains("Path") Then ActiveHive.OpenSubKey("Alert", True).DeleteValue("Path")
    Else
      ActiveHive.OpenSubKey("Alert", True).SetValue("Path", c_AlertNoisePath, Microsoft.Win32.RegistryValueKind.String)
    End If
    If Not ActiveHive.GetSubKeyNames.Contains("Load Package Data") Then ActiveHive.CreateSubKey("Load Package Data")
    ActiveHive.OpenSubKey("Load Package Data", True).SetValue("Features", IIf(c_LoadFeatures, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.OpenSubKey("Load Package Data", True).SetValue("Updates", IIf(c_LoadUpdates, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.OpenSubKey("Load Package Data", True).SetValue("Drivers", IIf(c_LoadDrivers, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
    If Not ActiveHive.GetSubKeyNames.Contains("Update Colors") Then ActiveHive.CreateSubKey("Update Colors")
    ActiveHive.OpenSubKey("Update Colors", True).SetValue("Requirement", c_UpdateColorRequirement.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Update Colors", True).SetValue("Upgrade", c_UpdateColorUpgrade.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Update Colors", True).SetValue("Superseded", c_UpdateColorSuperseded.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
    If Not ActiveHive.GetSubKeyNames.Contains("Driver Colors") Then ActiveHive.CreateSubKey("Driver Colors")
    ActiveHive.OpenSubKey("Driver Colors", True).SetValue("PE", c_DriverColorPE.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Driver Colors", True).SetValue("Boot", c_DriverColorBoot.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Driver Colors", True).SetValue("BootAndPE", c_DriverColorBootAndPE.ToArgb, Microsoft.Win32.RegistryValueKind.DWord)
  End Sub
End Class
Public Class MyPrerequisites
  Public Structure Updates_Prerequisite
    Public KBWithRequirement As String
    Public Requirement()() As String
    Public Sub New(Input As String)
      If Not Input.Contains(":") Then Return
      Dim splitInput() As String = Split(Input, ":", 2)
      KBWithRequirement = splitInput(0)
      If splitInput(1).Contains("/") Then
        Dim splitRequirement() As String = Split(splitInput(1), "/", 2)
        Dim reqCount As Integer = 0
        For Each reqList In splitRequirement
          If String.IsNullOrEmpty(reqList) Then Continue For
          ReDim Preserve Requirement(reqCount)
          If Not reqList.Contains(",") Then
            Requirement(reqCount) = {reqList}
          Else
            Requirement(reqCount) = Split(reqList, ",")
          End If
          reqCount += 1
        Next
      Else
        ReDim Requirement(0)
        If Not splitInput(1).Contains(",") Then
          Requirement(0) = {splitInput(1)}
        Else
          Requirement(0) = Split(splitInput(1), ",")
        End If
      End If
    End Sub
  End Structure
  Private c_PrerequisiteList() As Updates_Prerequisite
  Private c_PrereqVer As Version
  Public ReadOnly Property DatabaseVersion As Version
    Get
      Return c_PrereqVer
    End Get
  End Property
  Public ReadOnly Property PrerequisiteList As Updates_Prerequisite()
    Get
      Return c_PrerequisiteList
    End Get
  End Property
  Public Sub New()
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
      If My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then
        If My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName)).GetSubKeyNames.Contains("Prerequisite.db") Then
          ReadRegistry()
          Return
        End If
      End If
    End If
    ReadLegacy()
  End Sub
  Private Sub ReadRegistry()
    Dim Hive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName, "Prerequisite.db"))
    Dim sVersion As String = CStr(Hive.GetValue(Nothing, "2009.01"))
    c_PrereqVer = New Version(sVersion)
    If Hive.ValueCount <= 1 Then
      ReadLegacy()
      Return
    End If
    ReDim c_PrerequisiteList(Hive.ValueCount - 2)
    Dim I As Integer = 0
    For Each sRequiree As String In Hive.GetValueNames
      If String.IsNullOrEmpty(sRequiree) Then Continue For
      Dim preReq As New Updates_Prerequisite(sRequiree & ":" & Join(CType(Hive.GetValue(sRequiree, ""), String()), "/"))
      c_PrerequisiteList(I) = preReq
      I += 1
    Next
  End Sub
  Private Sub ReadLegacy()
    c_PrereqVer = New Version(2009, 1)
    c_PrerequisiteList = {
      New Updates_Prerequisite("2592687:2574819/3125574"),
      New Updates_Prerequisite("2830477:2574819,2857650"),
      New Updates_Prerequisite("2718695:2533623,2639308,2670838,2729094,2731771,2786081/3125574,2639308,2670838,2729094"),
      New Updates_Prerequisite("2841134:2533623,2639308,2670838,2729094,2731771,2786081,2834140,2882822,2888049,2849696,2849697/3125574,2670838,2729094,2849696,2849697"),
      New Updates_Prerequisite("2923545:2830477"),
      New Updates_Prerequisite("2965788:2830477/3125574"),
      New Updates_Prerequisite("2984976:2830477,2984972,2592687/3125574,2984972,2592687"),
      New Updates_Prerequisite("3020388:2830477/3125574"),
      New Updates_Prerequisite("3042058:3020369/3177467"),
      New Updates_Prerequisite("3042839:2830477/3125574"),
      New Updates_Prerequisite("3075226:2830477/3125574"),
      New Updates_Prerequisite("3082849:3078071"),
      New Updates_Prerequisite("3095316:3087038"),
      New Updates_Prerequisite("3125574:3020369/3177467"),
      New Updates_Prerequisite("3126446:2830477/3125574")
    }
    Save()
  End Sub
  Public Function Import(inData As String) As Boolean
    Dim sData() As String = Split(inData, vbLf)
    Dim sDataVer As String = sData(0)
    Dim dNewVer As New Version(sDataVer)
    If (dNewVer.Major > c_PrereqVer.Major) OrElse ((dNewVer.Major = c_PrereqVer.Major) And (dNewVer.Minor > c_PrereqVer.Minor)) Then
      c_PrereqVer = dNewVer
      ReDim c_PrerequisiteList(sData.Length - 2)
      For I As Integer = 1 To sData.Length - 1
        c_PrerequisiteList(I - 1) = New Updates_Prerequisite(sData(I))
      Next
      Save()
      Return True
    End If
    Return False
  End Function
  Private Sub Save()
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName), True).CreateSubKey(Application.ProductName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName)).GetSubKeyNames.Contains("Prerequisite.db") Then My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName), True).CreateSubKey("Prerequisite.db")
    Dim ActiveHive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName, "Prerequisite.db"), True)
    ActiveHive.SetValue(Nothing, c_PrereqVer.ToString(2))
    For Each sOldRequiree As String In ActiveHive.GetValueNames
      If String.IsNullOrEmpty(sOldRequiree) Then Continue For
      ActiveHive.DeleteValue(sOldRequiree)
    Next
    For Each Requiree As Updates_Prerequisite In c_PrerequisiteList
      Dim sReqData As New List(Of String)
      For Each Prereq() As String In Requiree.Requirement
        sReqData.Add(Join(Prereq, ","))
      Next
      ActiveHive.SetValue(Requiree.KBWithRequirement, sReqData.ToArray, Microsoft.Win32.RegistryValueKind.MultiString)
    Next
  End Sub
End Class
Public Class MyReplacements
  Public Structure Updates_Replacement
    Public NewKB As String
    Public OldKBs() As String
    Public Sub New(Input As String)
      If Not Input.Contains(":") Then Return
      Dim splitInput() As String = Split(Input, ":", 2)
      NewKB = splitInput(0)
      If Not splitInput(1).Contains(",") Then
        OldKBs = {splitInput(1)}
      Else
        OldKBs = Split(splitInput(1), ",")
      End If
    End Sub
  End Structure
  Private c_ReplacementList() As Updates_Replacement
  Private c_ReplaceVer As Version
  Public ReadOnly Property DatabaseVersion As Version
    Get
      Return c_ReplaceVer
    End Get
  End Property
  Public ReadOnly Property ReplacementList As Updates_Replacement()
    Get
      Return c_ReplacementList
    End Get
  End Property
  Public Sub New()
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
      If My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then
        If My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName)).GetSubKeyNames.Contains("Replacement.db") Then
          ReadRegistry()
          Return
        End If
      End If
    End If
    ReadLegacy()
  End Sub
  Private Sub ReadRegistry()
    Dim Hive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName, "Replacement.db"))
    Dim sVersion As String = CStr(Hive.GetValue(Nothing, "2009.01"))
    c_ReplaceVer = New Version(sVersion)
    If Hive.ValueCount <= 1 Then
      ReadLegacy()
      Return
    End If
    ReDim c_ReplacementList(Hive.ValueCount - 2)
    Dim I As Integer = 0
    For Each sReplacer As String In Hive.GetValueNames
      If String.IsNullOrEmpty(sReplacer) Then Continue For
      Dim rePlace As New Updates_Replacement(sReplacer & ":" & Join(CType(Hive.GetValue(sReplacer, ""), String()), ","))
      c_ReplacementList(I) = rePlace
      I += 1
    Next
  End Sub
  Private Sub ReadLegacy()
    c_ReplaceVer = New Version(2009, 1)
    c_ReplacementList = {
      New Updates_Replacement("3125574:2574819,2603229,2607047,2607576,2633952,2639308,2640148,2647753,2660075,2661254,2677070,2679255,2699779,2709630,2709981,2719857,2726535,2731771,2732059,2732487,2732500,2735855,2739159,2741355,2749655,2756822,2760730,2762895,2763523,2773072,2779562,2786081,2786400,2791765,2794119,2798162,2799926,2800095,2808679,2813956,2829104,2830477,2834140,2835174,2836502,2843630,2846960,2846960,2847077,2852386,2853952,2863058,2868116,2882822,2888049,2890882,2891804,2893519,2904266,2905454,2908783,2913152,2913431,2913751,2918077,2919469,2922717,2923398,2923545,2928562,2929733,2929755,2966583,2970228,2973337,2977728,2978092,2980245,2981580,2985461,2994023,2998527,2999226,3000988,3001554,3004394,3005788,3006121,3006137,3006625,3008627,3009736,3013410,3013531,3014406,3020338,3020370,3040272,3045645,3048761,3049874,3054476,3065979,3068708,3075249,3077715,3078667,3080079,3080149,3081954,3092627,3095649,3102429,3107998,3112148,3118401,3121255,3133977,3137061,3138378,3138901,3147071,3148851")
    }
    Save()
  End Sub
  Public Function Import(inData As String) As Boolean
    Dim sData() As String = Split(inData, vbLf)
    Dim sDataVer As String = sData(0)
    Dim dNewVer As New Version(sDataVer)
    If (dNewVer.Major > c_ReplaceVer.Major) OrElse ((dNewVer.Major = c_ReplaceVer.Major) And (dNewVer.Minor > c_ReplaceVer.Minor)) Then
      c_ReplaceVer = dNewVer
      ReDim c_ReplacementList(sData.Length - 2)
      For I As Integer = 1 To sData.Length - 1
        c_ReplacementList(I - 1) = New Updates_Replacement(sData(I))
      Next
      Save()
      Return True
    End If
    Return False
  End Function
  Private Sub Save()
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName)).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName), True).CreateSubKey(Application.ProductName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName)).GetSubKeyNames.Contains("Replacement.db") Then My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName), True).CreateSubKey("Replacement.db")
    Dim ActiveHive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey(IO.Path.Combine("Software", Application.CompanyName, Application.ProductName, "Replacement.db"), True)
    ActiveHive.SetValue(Nothing, c_ReplaceVer.ToString(2))
    For Each sOldRequiree As String In ActiveHive.GetValueNames
      If String.IsNullOrEmpty(sOldRequiree) Then Continue For
      ActiveHive.DeleteValue(sOldRequiree)
    Next
    For Each Replacee As Updates_Replacement In c_ReplacementList
      ActiveHive.SetValue(Replacee.NewKB, Replacee.OldKBs, Microsoft.Win32.RegistryValueKind.MultiString)
    Next
  End Sub
End Class
