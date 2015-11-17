Public Class MySettings
  Private c_TempDir As String
  Private c_Timeout As Integer = 0
  Private c_x86WhiteList() As String = {"Windows6.2-KB2764913-x86", "Windows6.2-KB2764916-x86", "Windows6.3-KB2849697-x86", "Windows6.3-KB2849696-x86"}
  Private c_Position As Point = New Point
  Private c_Size As Size = New Size(440, 480)
  Private c_DefaultISOLabel As String = "GRMCULFRER_EN_DVD"
  Private c_DefaultFS As String = "ISO 9960"
  Private c_Priority As String = "Normal"
  Private c_DefaultSplit As Integer = 0
  Private c_SplitVal As String
  Private c_LastUpdate As Date = New Date(1970, 1, 1)
  Private c_PlayAlertNoise As Boolean
  Private c_AlertNoisePath As String
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
  Public Sub New()
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
      If My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then
        ReadRegistry()
        Return
      End If
    End If
    ReadLegacy()
  End Sub
  Private Sub ReadRegistry()
    Dim Hive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName)
    If Hive.GetValueNames.Contains("Temp Directory") Then
      c_TempDir = Hive.GetValue("Temp Directory", My.Settings.TempDir)
    Else
      c_TempDir = My.Settings.TempDir
    End If
    If Hive.GetValueNames.Contains("Timeout") Then
      c_Timeout = Hive.GetValue("Timeout", My.Settings.Timeout)
    Else
      c_Timeout = My.Settings.Timeout
    End If
    If Hive.GetValueNames.Contains("x86 Whitelist") Then
      c_x86WhiteList = Hive.GetValue("x86 Whitelist", Split(My.Settings.x86WhiteList, vbNewLine))
    Else
      c_x86WhiteList = Split(My.Settings.x86WhiteList, vbNewLine)
    End If
    If Hive.GetSubKeyNames.Contains("Display") Then
      Dim disp As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Display")
      If disp.GetValueNames.Contains("Left") Then
        c_Position.X = Hive.OpenSubKey("Display").GetValue("Left", My.Settings.Position.X)
      Else
        c_Position.X = My.Settings.Position.X
      End If
      If disp.GetValueNames.Contains("Top") Then
        c_Position.Y = Hive.OpenSubKey("Display").GetValue("Top", My.Settings.Position.Y)
      Else
        c_Position.Y = My.Settings.Position.Y
      End If
      If disp.GetValueNames.Contains("Width") Then
        c_Size.Width = Hive.OpenSubKey("Display").GetValue("Width", My.Settings.Size.Width)
      Else
        c_Size.Width = My.Settings.Size.Width
      End If
      If disp.GetValueNames.Contains("Height") Then
        c_Size.Height = Hive.OpenSubKey("Display").GetValue("Height", My.Settings.Size.Height)
      Else
        c_Size.Height = My.Settings.Size.Height
      End If
    Else
      c_Position = My.Settings.Position
      c_Size = My.Settings.Size
    End If
    If Hive.GetValueNames.Contains("Default ISO Label") Then
      c_DefaultISOLabel = Hive.GetValue("Default ISO Label", My.Settings.DefaultISOLabel)
    Else
      c_DefaultISOLabel = My.Settings.DefaultISOLabel
    End If
    If Hive.GetValueNames.Contains("Default File System") Then
      c_DefaultFS = Hive.GetValue("Default File System", My.Settings.DefaultFS)
    Else
      c_DefaultFS = My.Settings.DefaultFS
    End If
    If Hive.GetValueNames.Contains("Priority") Then
      c_Priority = Hive.GetValue("Priority", My.Settings.Priority)
    Else
      c_Priority = My.Settings.Priority
    End If
    If Hive.GetValueNames.Contains("Default Split") Then
      c_DefaultSplit = Hive.GetValue("Default Split", My.Settings.DefaultSplit)
    Else
      c_DefaultSplit = My.Settings.DefaultSplit
    End If
    If Hive.GetValueNames.Contains("Split Value") Then
      c_SplitVal = Hive.GetValue("Split Value", My.Settings.SplitVal)
    Else
      c_SplitVal = My.Settings.SplitVal
    End If
    If Hive.GetValueNames.Contains("Last Update") Then
      c_LastUpdate = Date.FromBinary(Hive.GetValue("Last Update", My.Settings.LastUpdate.ToBinary))
    Else
      c_LastUpdate = My.Settings.LastUpdate
    End If
    If Hive.GetSubKeyNames.Contains("Alert") Then
      Dim alert As Microsoft.Win32.RegistryKey = Hive.OpenSubKey("Alert")
      c_PlayAlertNoise = (alert.GetValue(String.Empty, "N") = "Y")
      If alert.GetValueNames.Contains("Path") Then
        c_AlertNoisePath = alert.GetValue("Path", String.Empty)
      Else
        c_AlertNoisePath = String.Empty
      End If
    Else
      c_PlayAlertNoise = False
      c_AlertNoisePath = String.Empty
    End If
  End Sub
  Private Sub ReadLegacy()
    c_TempDir = My.Settings.TempDir
    c_Timeout = My.Settings.Timeout
    c_x86WhiteList = Split(My.Settings.x86WhiteList, vbNewLine)
    c_Position = My.Settings.Position
    c_Size = My.Settings.Size
    c_DefaultISOLabel = My.Settings.DefaultISOLabel
    c_DefaultFS = My.Settings.DefaultFS
    c_Priority = My.Settings.Priority
    c_DefaultSplit = My.Settings.DefaultSplit
    c_SplitVal = My.Settings.SplitVal
    c_LastUpdate = My.Settings.LastUpdate
    c_PlayAlertNoise = False
    c_AlertNoisePath = String.Empty
    Save()
  End Sub
  Public Sub Save()
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName, True).CreateSubKey(Application.ProductName)
    Dim ActiveHive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName, True)
    ActiveHive.SetValue("Temp Directory", c_TempDir, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Timeout", c_Timeout, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("x86 Whitelist", c_x86WhiteList, Microsoft.Win32.RegistryValueKind.MultiString)
    If Not ActiveHive.GetSubKeyNames.Contains("Display") Then ActiveHive.CreateSubKey("Display")
    ActiveHive.OpenSubKey("Display", True).SetValue("Left", c_Position.X, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Top", c_Position.Y, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Width", c_Size.Width, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.OpenSubKey("Display", True).SetValue("Height", c_Size.Height, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("Default ISO Label", c_DefaultISOLabel, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Default File System", c_DefaultFS, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Priority", c_Priority, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Default Split", c_DefaultSplit, Microsoft.Win32.RegistryValueKind.DWord)
    ActiveHive.SetValue("Split Value", c_SplitVal, Microsoft.Win32.RegistryValueKind.String)
    ActiveHive.SetValue("Last Update", c_LastUpdate.ToBinary, Microsoft.Win32.RegistryValueKind.QWord)
    If Not ActiveHive.GetSubKeyNames.Contains("Alert") Then ActiveHive.CreateSubKey("Alert")
    ActiveHive.OpenSubKey("Alert", True).SetValue(String.Empty, IIf(c_PlayAlertNoise, "Y", "N"))
    ActiveHive.OpenSubKey("Alert", True).SetValue("Path", c_AlertNoisePath)
  End Sub
End Class
