Public Class frmConfig
  Private mySettings As MySettings
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    If String.IsNullOrEmpty(txtTemp.Text) Then txtTemp.Text = My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "Slips7ream"
    If Not txtTemp.Text.EndsWith(IO.Path.DirectorySeparatorChar) Then txtTemp.Text &= IO.Path.DirectorySeparatorChar
    Dim sDir As String = IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(txtTemp.Text))

    If IO.Directory.Exists(sDir) Then
      mySettings.TempDir = txtTemp.Text
    Else
      txtTemp.Focus()
      MsgDlg(Me, "Please choose a directory that exists to put the Temp directory in.", "Unable to find parent directory.", "Folder Not Found", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , """" & sDir & """ does not exist.")
      Return
    End If
    mySettings.x86WhiteList = Split(txtWhitelist.Text, vbNewLine)
    mySettings.Timeout = txtTimeout.Value * 1000 * 60
    If Not mySettings.PlayAlertNoise = chkAlert.Checked Then
      frmMain.cmbCompletion.SelectedIndex = IIf(chkAlert.Checked, 1, 0)
    End If
    mySettings.PlayAlertNoise = chkAlert.Checked
    mySettings.AlertNoisePath = IIf(chkDefault.Checked, String.Empty, txtAlertPath.Text)
    mySettings.Save()
    Me.Close()
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
  Private Sub frmConfig_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    mySettings = New MySettings
    txtTemp.Text = mySettings.TempDir
    If String.IsNullOrEmpty(txtTemp.Text) Then txtTemp.Text = My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "Slips7ream"
    txtWhitelist.Text = Join(mySettings.x86WhiteList, vbNewLine)
    txtTimeout.Value = mySettings.Timeout / 1000 / 60
    If txtTimeout.Value = 0 Then
      lblTimeoutS.Text = " (Never)"
    ElseIf txtTimeout.Value = 1 Then
      lblTimeoutS.Text = " minute"
    Else
      lblTimeoutS.Text = " minutes"
    End If
    chkAlert.Checked = mySettings.PlayAlertNoise
    txtAlertPath.Text = mySettings.AlertNoisePath
  End Sub
  Private Sub cmdTemp_Click(sender As System.Object, e As System.EventArgs) Handles cmdTemp.Click
    Using dirDlg As New SaveFileDialog With
      {
        .AddExtension = False,
        .CheckPathExists = True,
        .CheckFileExists = False,
        .CreatePrompt = False,
        .FileName = "Slips7ream",
        .Filter = "Directories|",
        .OverwritePrompt = False,
        .ShowHelp = False,
        .InitialDirectory = IIf(String.IsNullOrEmpty(txtTemp.Text), IO.Path.GetTempPath, IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(IIf(txtTemp.Text.EndsWith(IO.Path.DirectorySeparatorChar), txtTemp.Text, txtTemp.Text & IO.Path.DirectorySeparatorChar)))),
        .Title = "Select a Directory for SLIPS7REAM to work in...",
        .ValidateNames = True
      }
      If dirDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txtTemp.Text = IIf(dirDlg.FileName.EndsWith(IO.Path.DirectorySeparatorChar), dirDlg.FileName, dirDlg.FileName & IO.Path.DirectorySeparatorChar)
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

  Private Sub cmdAlertBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdAlertBrowse.Click
    Dim defDir As String
    If String.IsNullOrEmpty(txtAlertPath.Text) Then
      defDir = My.Computer.FileSystem.SpecialDirectories.MyDocuments & IO.Path.DirectorySeparatorChar
    Else
      defDir = IO.Path.GetDirectoryName(txtAlertPath.Text)
    End If
    Using alertDlg As New OpenFileDialog With
      {
        .AddExtension = False,
        .CheckPathExists = True,
        .CheckFileExists = True,
        .FileName = "Slips7ream",
        .Filter = "Sound Files|*.wav|All Files|*.*",
        .ShowHelp = False,
        .InitialDirectory = defDir,
        .Title = "Select an Alert sound to play when SLIPS7REAM is done...",
        .ValidateNames = True
      }
      If alertDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txtAlertPath.Text = alertDlg.FileName
    End Using
  End Sub

  Private Sub chkAlert_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAlert.CheckedChanged
    txtAlertPath.Enabled = chkAlert.Checked And Not chkDefault.Checked
    cmdAlertBrowse.Enabled = chkAlert.Checked And Not chkDefault.Checked
    chkDefault.Enabled = chkAlert.Checked
    cmdPlay.Enabled = chkAlert.Checked
  End Sub

  Private Sub chkDefault_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkDefault.CheckedChanged
    txtAlertPath.Enabled = chkAlert.Checked And Not chkDefault.Checked
    cmdAlertBrowse.Enabled = chkAlert.Checked And Not chkDefault.Checked
  End Sub

  Private Sub cmdPlay_Click(sender As System.Object, e As System.EventArgs) Handles cmdPlay.Click
    If chkDefault.Checked Then
      My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
    ElseIf IO.File.Exists(txtAlertPath.Text) Then
      Try
        My.Computer.Audio.Play(txtAlertPath.Text, AudioPlayMode.Background)
      Catch ex As Exception
        MsgDlg(Me, "The Alert sound file failed to play!", "File could not be played.", "Alert File Error", MessageBoxButtons.OK, TaskDialogIcon.MusicFile, , ex.Message)
      End Try
    Else
      MsgDlg(Me, "The Alert sound file does not exist!", "File could not be played.", "Alert File Error", MessageBoxButtons.OK, TaskDialogIcon.MusicFile)
    End If
  End Sub
End Class