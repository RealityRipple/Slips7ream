Public Class frmDriverProps
  Private myDriver As Driver
  Public Sub New(Driver As Driver)
    InitializeComponent()
    myDriver = Driver
    Me.Text = String.Format("{0} Driver Properties", myDriver.OriginalFileName)
    pctDriverIcon.Image = myDriver.DriverIcon
    txtPublishedName.Text = myDriver.PublishedName
    txtOriginalFileName.Text = myDriver.OriginalFileName
    txtDriverStorePath.Text = myDriver.DriverStorePath
    txtInBox.Text = myDriver.InBox
    pctClassIcon.Image = myDriver.ClassIcon
    txtClassName.Text = myDriver.ClassName
    txtClassDescription.Text = myDriver.ClassDescription
    txtClassGUID.Text = myDriver.ClassGUID
    txtProviderName.Text = myDriver.ProviderName
    txtDate.Text = myDriver.Date
    txtVersion.Text = myDriver.Version
    txtBootCritical.Text = myDriver.BootCritical
    If myDriver.Architectures IsNot Nothing AndAlso myDriver.Architectures.Count > 0 Then
      cmbArchitecture.Items.Clear()
      cmbArchitecture.Enabled = True
      For Each arch As String In myDriver.Architectures
        If CompareArchitectures(arch, ArchitectureList.ia64, False) Then
          cmbArchitecture.Items.Add(String.Format("{0} (Not Used)", arch))
        Else
          cmbArchitecture.Items.Add(arch)
        End If
      Next
      cmbArchitecture.SelectedIndex = 0
    Else
      If myDriver.DriverHardware Is Nothing Then
        cmbArchitecture.Items.Clear()
        cmbArchitecture.Enabled = False
        cmbHardware.Items.Clear()
        cmbHardware.Enabled = False
        lstHWIDs.Items.Clear()
        'lstHWIDs.Enabled = False
        lstHWCompatibleIDs.Items.Clear()
        'lstHWCompatibleIDs.Enabled = False
        lstHWExcludeIDs.Items.Clear()
        'lstHWExcludeIDs.Enabled = False
        Return
      End If
      cmbArchitecture.Items.Clear()
      cmbArchitecture.Enabled = True
      For Each arch As String In myDriver.DriverHardware.Keys
        If CompareArchitectures(arch, ArchitectureList.ia64, False) Then
          cmbArchitecture.Items.Add(String.Format("{0} (Not Used)", arch))
        Else
          cmbArchitecture.Items.Add(arch)
        End If
      Next
      cmbArchitecture.SelectedIndex = 0
    End If
  End Sub
  Private Sub cmbArchitecture_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbArchitecture.SelectedIndexChanged
    If cmbArchitecture.SelectedIndex = -1 Then
      cmbHardware.Items.Clear()
      cmbHardware.Enabled = False
      txtHWServiceName.Text = Nothing
      txtHWArchitecture.Text = Nothing
      txtHWManufacturer.Text = Nothing
      lstHWIDs.Items.Clear()
      lstHWCompatibleIDs.Items.Clear()
      lstHWExcludeIDs.Items.Clear()
      Return
    End If
    cmbHardware.Items.Clear()
    cmbHardware.Enabled = True
    Dim sArch As String = cmbArchitecture.Text
    If sArch.Contains(" (Not Used)") Then sArch = sArch.Substring(0, sArch.Length - 11)
    For Each hw As Driver_Hardware In myDriver.DriverHardware(sArch)
      cmbHardware.Items.Add(hw.Description)
    Next
    txtHWServiceName.Text = Nothing
    txtHWArchitecture.Text = Nothing
    txtHWManufacturer.Text = Nothing
    lstHWIDs.Items.Clear()
    lstHWCompatibleIDs.Items.Clear()
    lstHWExcludeIDs.Items.Clear()
    If cmbHardware.Items.Count > 0 Then
      cmbHardware.SelectedIndex = 0
    Else
      cmbHardware.Enabled = False
    End If
  End Sub
  Private Sub cmbHardware_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbHardware.SelectedIndexChanged
    If cmbHardware.SelectedIndex = -1 Then
      txtHWServiceName.Text = Nothing
      txtHWArchitecture.Text = Nothing
      txtHWManufacturer.Text = Nothing
      lstHWIDs.Items.Clear()
      lstHWCompatibleIDs.Items.Clear()
      lstHWExcludeIDs.Items.Clear()
      Return
    End If
    Dim sArch As String = cmbArchitecture.Text
    If sArch.Contains(" (Not Used)") Then sArch = sArch.Substring(0, sArch.Length - 11)
    Dim myHW As Driver_Hardware = myDriver.DriverHardware(sArch)(cmbHardware.SelectedIndex)
    txtHWServiceName.Text = myHW.ServiceName
    txtHWArchitecture.Text = myHW.Architecture
    txtHWManufacturer.Text = myHW.Manufacturer
    lstHWIDs.Items.Clear()
    If myHW.HardwareIDs IsNot Nothing AndAlso myHW.HardwareIDs.Count > 0 Then
      lstHWIDs.Items.AddRange(myHW.HardwareIDs.Keys.ToArray())
    Else
      lstHWIDs.Items.Add("(None)")
    End If
    lstHWCompatibleIDs.Items.Clear()
    lstHWExcludeIDs.Items.Clear()
    lstHWIDs.SelectedIndex = 0
  End Sub
  Private Sub lstHWIDs_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstHWIDs.SelectedIndexChanged
    If lstHWIDs.SelectedIndex = -1 Then
      lstHWCompatibleIDs.Items.Clear()
      lstHWExcludeIDs.Items.Clear()
      Return
    End If
    Dim sArch As String = cmbArchitecture.Text
    If sArch.Contains(" (Not Used)") Then sArch = sArch.Substring(0, sArch.Length - 11)
    Dim myHW As Driver_Hardware_IDLists = myDriver.DriverHardware(sArch)(cmbHardware.SelectedIndex).HardwareIDs(CStr(lstHWIDs.SelectedItem))
    lstHWCompatibleIDs.Items.Clear()
    If myHW.CompatibleIDs IsNot Nothing AndAlso myHW.CompatibleIDs.Count > 0 Then
      lstHWCompatibleIDs.Items.AddRange(myHW.CompatibleIDs.ToArray)
    Else
      lstHWCompatibleIDs.Items.Add("(None)")
    End If
    lstHWExcludeIDs.Items.Clear()
    If myHW.ExcludeIDs IsNot Nothing AndAlso myHW.ExcludeIDs.Count > 0 Then
      lstHWExcludeIDs.Items.AddRange(myHW.ExcludeIDs.ToArray)
    Else
      lstHWExcludeIDs.Items.Add("(None)")
    End If
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
End Class
