Public Class frmConfig
  Private mySettings As MySettings
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    If String.IsNullOrEmpty(txtTemp.Text) Then txtTemp.Text = My.Computer.FileSystem.SpecialDirectories.Temp & "\Slips7ream"
    If Not txtTemp.Text.EndsWith("\") Then txtTemp.Text &= "\"
    Dim sDir As String = IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(txtTemp.Text))

    If My.Computer.FileSystem.DirectoryExists(sDir) Then
      mySettings.TempDir = txtTemp.Text
    Else
      txtTemp.Focus()
      MsgBox("Temp Parent Directory Not Found!" & vbNewLine & "Please choose a directory that exists to put the Temp directory in.", MsgBoxStyle.Exclamation, "Missing Temp Parent Directory")
    End If
    mySettings.x86WhiteList = Split(txtWhitelist.Text, vbNewLine)
    mySettings.Timeout = txtTimeout.Value * 1000 * 60
    mySettings.Save()
    Me.Close()
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
  Private Sub frmConfig_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    mySettings = New MySettings
    txtTemp.Text = mySettings.TempDir
    If String.IsNullOrEmpty(txtTemp.Text) Then txtTemp.Text = My.Computer.FileSystem.SpecialDirectories.Temp & "\Slips7ream"
    txtWhitelist.Text = Join(mySettings.x86WhiteList, vbNewLine)
    txtTimeout.Value = mySettings.Timeout / 1000 / 60
    If txtTimeout.Value = 0 Then
      lblTimeoutS.Text = " (Never)"
    ElseIf txtTimeout.Value = 1 Then
      lblTimeoutS.Text = " minute"
    Else
      lblTimeoutS.Text = " minutes"
    End If
  End Sub
  Private Sub cmdTemp_Click(sender As System.Object, e As System.EventArgs) Handles cmdTemp.Click
    Using dirDlg As New SaveFileDialog With
      {
        .AddExtension = False,
        .CheckPathExists = True,
        .CheckFileExists = False,
        .CreatePrompt = False,
        .FileName = "Slipstream",
        .Filter = "Directories|",
        .OverwritePrompt = False,
        .ShowHelp = False,
        .InitialDirectory = IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(IIf(txtTemp.Text.EndsWith("\"), txtTemp.Text, txtTemp.Text & "\"))),
        .Title = "Select a Directory for SLIPS7REAM to work in...",
        .ValidateNames = True
      }
      If dirDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txtTemp.Text = IIf(dirDlg.FileName.EndsWith("\"), dirDlg.FileName, dirDlg.FileName & "\")
    End Using
  End Sub
  Private Sub txtTimeout_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtTimeout.KeyUp
    If txtTimeout.Value = 0 Then
      lblTimeoutS.Text = " (Never)"
    ElseIf txtTimeout.Value = 1 Then
      lblTimeoutS.Text = " minute"
    Else
      lblTimeoutS.Text = " minutes"
    End If
  End Sub
  Private Sub txtTimeout_ValueChanged(sender As System.Object, e As System.EventArgs) Handles txtTimeout.ValueChanged
    If txtTimeout.Value = 0 Then
      lblTimeoutS.Text = " (Never)"
    ElseIf txtTimeout.Value = 1 Then
      lblTimeoutS.Text = " minute"
    Else
      lblTimeoutS.Text = " minutes"
    End If
  End Sub
  Private Sub lblDonate_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblDonate.LinkClicked
    Process.Start("http://realityripple.com/donate.php?itm=SLIPS7REAM")
  End Sub
End Class