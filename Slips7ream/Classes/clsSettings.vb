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
        Exit Sub
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

Public Class SpeedStats
  Public Shared Property WIM_ExtractFromISO(Size As String) As Long
    Get
      Return ReadField("WIM_ExtractFromISO_" & Size)
    End Get
    Set(value As Long)
      WriteField("WIM_ExtractFromISO_" & Size, value)
    End Set
  End Property
  Public Shared Property NOWIM_ExtractFromISO(Size As String) As Long
    Get
      Return ReadField("NOWIM_ExtractFromISO_" & Size)
    End Get
    Set(value As Long)
      WriteField("NOWIM_ExtractFromISO_" & Size, value)
    End Set
  End Property
  Public Shared Property WIM_ParseImage(Size As String) As Long
    Get
      Return ReadField("WIM_ParseImage_" & Size)
    End Get
    Set(value As Long)
      WriteField("WIM_ParseImage_" & Size, value)
    End Set
  End Property
  Public Shared Property WIM_MountImage(Architecture As String) As Long
    Get
      Return ReadField("WIM_MountImage_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("WIM_MountImage_x" & Architecture, value)
    End Set
  End Property
  Public Shared Property WIM_SaveImage(Architecture As String) As Long
    Get
      Return ReadField("WIM_SaveImage_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("WIM_SaveImage_x" & Architecture, value)
    End Set
  End Property
  Public Shared Property WIM_UnmountImage(Architecture As String) As Long
    Get
      Return ReadField("WIM_UnmountImage_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("WIM_UnmountImage_x" & Architecture, value)
    End Set
  End Property
  Public Shared Property WIM_MergeImage(First As String) As Long
    Get
      Return ReadField("WIM_MergeImage_" & First)
    End Get
    Set(value As Long)
      WriteField("WIM_MergeImage_" & First, value)
    End Set
  End Property
  Public Shared Property WIM_MergeAndCompressImage(First As String) As Long
    Get
      Return ReadField("WIM_MergeAndCompressImage_" & First)
    End Get
    Set(value As Long)
      WriteField("WIM_MergeAndCompressImage_" & First, value)
    End Set
  End Property
  Public Shared Property WIM_MoveImage(Size As String) As Long
    Get
      Return ReadField("WIM_MoveImage_" & Size)
    End Get
    Set(value As Long)
      WriteField("WIM_MoveImage_" & Size, value)
    End Set
  End Property
  Public Shared Property WIM_SplitImage(Size As String, ChunkSize As String) As Long
    Get
      Return ReadField("WIM_SplitImage_" & Size & "_to_" & ChunkSize)
    End Get
    Set(value As Long)
      WriteField("WIM_SplitImage_" & Size & "_to_" & ChunkSize, value)
    End Set
  End Property

  Public Shared Property SP_Extract(Architecture As String) As Long
    Get
      Return ReadField("SP_Extract_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("SP_Extract_x" & Architecture, value)
    End Set
  End Property
  Public Shared Property SP_Integrate(Architecture As String) As Long
    Get
      Return ReadField("SP_Integrate_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("SP_Integrate_x" & Architecture, value)
    End Set
  End Property

  Public Shared Property Update_Parse([Type] As String, Architecture As String) As Long
    Get
      Return ReadField(Type & "_Parse_" & Architecture)
    End Get
    Set(value As Long)
      WriteField(Type & "_Parse_" & Architecture, value)
    End Set
  End Property
  Public Shared Property Update_Extract([Type] As String, Architecture As String) As Long
    Get
      Return ReadField(Type & "_Extract_" & Architecture)
    End Get
    Set(value As Long)
      WriteField(Type & "_Extract_" & Architecture, value)
    End Set
  End Property
  Public Shared Property Update_Integrate([Type] As String, Architecture As String, Size As String) As Long
    Get
      Return ReadField(Type & "_Integrate_x" & Architecture & "_" & Size)
    End Get
    Set(value As Long)
      WriteField(Type & "_Integrate_x" & Architecture & "_" & Size, value)
    End Set
  End Property

  Public Shared Property ISO_Make() As Long
    Get
      Return ReadField("ISO_Make")
    End Get
    Set(value As Long)
      WriteField("ISO_Make", value)
    End Set
  End Property

  Public Shared Property Clean_Temp As Long
    Get
      Return ReadField("Clean_Temp")
    End Get
    Set(value As Long)
      WriteField("Clean_Temp", value)
    End Set
  End Property
  Public Shared Property Clean_MOUNT As Long
    Get
      Return ReadField("Clean_MOUNT")
    End Get
    Set(value As Long)
      WriteField("Clean_MOUNT", value)
    End Set
  End Property
  Public Shared Property Clean_WORK As Long
    Get
      Return ReadField("Clean_WORK")
    End Get
    Set(value As Long)
      WriteField("Clean_WORK", value)
    End Set
  End Property
  Public Shared Property Clean_SP1(Architecture As String) As Long
    Get
      Return ReadField("Clean_SP1_x" & Architecture)
    End Get
    Set(value As Long)
      WriteField("Clean_SP1_x" & Architecture, value)
    End Set
  End Property

  Private Shared Sub WriteField(FieldName As String, NewValue As Long)
    Dim newStr As String = Convert.ToString(NewValue, 16)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey(Application.CompanyName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName, True).CreateSubKey(Application.ProductName)
    If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName).GetSubKeyNames.Contains("SpeedStats") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName, True).CreateSubKey("SpeedStats")
    Dim ActiveHive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName & "\SpeedStats", True)
    If ActiveHive.GetValueNames.Contains(FieldName) Then
      Dim OldValues() As String = ActiveHive.GetValue(FieldName, {""})
      If OldValues.Count = 1 AndAlso String.IsNullOrEmpty(OldValues(0)) Then
        ActiveHive.SetValue(FieldName, {newStr}, Microsoft.Win32.RegistryValueKind.MultiString)
      ElseIf OldValues.Count = 0 Then
        ActiveHive.SetValue(FieldName, {newStr}, Microsoft.Win32.RegistryValueKind.MultiString)
      ElseIf OldValues.Count = 10 Then
        Dim NewValues(OldValues.Count - 1) As String
        For I As Integer = 1 To OldValues.Count - 1
          NewValues(I) = OldValues(I - 1)
        Next
        NewValues(0) = newStr
        ActiveHive.SetValue(FieldName, NewValues, Microsoft.Win32.RegistryValueKind.MultiString)
      Else
        Dim NewValues(OldValues.Count) As String
        For I As Integer = 0 To OldValues.Count - 1
          NewValues(I + 1) = OldValues(I)
        Next
        NewValues(0) = newStr
        ActiveHive.SetValue(FieldName, NewValues, Microsoft.Win32.RegistryValueKind.MultiString)
      End If
    Else
      ActiveHive.SetValue(FieldName, {newStr}, Microsoft.Win32.RegistryValueKind.MultiString)
    End If
  End Sub

  Private Shared Function ReadField(FieldName As String) As Long
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains(Application.CompanyName) Then
      If My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName).GetSubKeyNames.Contains(Application.ProductName) Then
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName).GetSubKeyNames.Contains("SpeedStats") Then
          Dim Hive As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\" & Application.CompanyName & "\" & Application.ProductName & "\SpeedStats")
          If Hive.GetValueNames.Contains(FieldName) Then
            Dim Values() As String = Hive.GetValue(FieldName, {""})
            If Values.Count = 1 AndAlso String.IsNullOrEmpty(Values(0)) Then
              Return 0
            ElseIf Values.Count = 0 Then
              Return 0
            Else
              Dim total As Long = 0
              For I As Integer = 0 To Values.Count - 1
                total += Convert.ToInt64(Values(I), 16)
              Next
              total = total / Values.Count
              Return total
            End If
          Else
            Return 0
          End If
        Else
          Return 0
        End If
      Else
        Return 0
      End If
    Else
      Return 0
    End If
  End Function
End Class