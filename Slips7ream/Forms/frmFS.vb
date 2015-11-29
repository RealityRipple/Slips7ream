Public Class frmFS
  Public Sub New(OverFiles As List(Of String), SourceDir As String, SelectedFS As Byte)
    InitializeComponent()
    Dim sFiles As String = ""
    For Each sFile In OverFiles
      sFiles &= sFile.Replace(SourceDir, "") & vbNewLine
    Next
    txtLargeFiles.Text = sFiles
    Select Case SelectedFS
      Case 0
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, "Use ISO 9960 standard format, which has limited file name and size options." & vbNewLine & vbNewLine & "This option will corrupt large files.")
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = "The ISO 9960 File System can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings." & vbNewLine & vbNewLine & "SLIPS7REAM suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:"
      Case 1
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use Joliet"
        ttFS.SetToolTip(cmdUseCurrent, "Use Joliet extension of ISO 9960, which has improved file names." & vbNewLine & vbNewLine & "This option will corrupt large files.")
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = "File Systems based on ISO 9960 (like Joliet) can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings." & vbNewLine & vbNewLine & "SLIPS7REAM suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:"
      Case 2
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use Joliet and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, "Use Joliet extension of ISO 9960 with ISO 9960 as a backup if Joliet can't be read by the drive." & vbNewLine & vbNewLine & "Both the standard and backup formats will corrupt large files.")
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = "File Systems based on ISO 9960 (like Joliet) can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings." & vbNewLine & vbNewLine & "SLIPS7REAM suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:"
      Case 4
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use UDF and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, "Use UDF with ISO 9960 as a backup if UDF can't be read by the drive." & vbNewLine & vbNewLine & "The ISO 9960 backup will not read large files.")
        Me.AcceptButton = cmdUseCurrent
        lblDescription.Text = "The ISO 9960 File System can not handle files 4 GB or larger, and the ISO 9960 backup of the UDF File System will result in file read problems if relied upon." & vbNewLine & vbNewLine & "SLIPS7REAM suggests that you skip the ISO 9960 backup for this ISO, because the following files are greater than 4095 MB:"
      Case 6
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use UDF 1.02 and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, "Use UDF 1.02 with ISO 9960 as a backup if UDF 1.02 can't be read by the drive." & vbNewLine & vbNewLine & "The ISO 9960 backup will not read large files.")
        Me.AcceptButton = cmdUseCurrent
        lblDescription.Text = "The ISO 9960 File System can not handle files 4 GB or larger, and the ISO 9960 backup of the UDF 1.02 File System will result in file read problems if relied upon." & vbNewLine & vbNewLine & "SLIPS7REAM suggests that you skip the ISO 9960 backup for this ISO, because the following files are greater than 4095 MB:"
      Case Else
        cmdUseCurrent.Text = "Use Nothing"
        ttFS.SetToolTip(cmdUseCurrent, "This option is not enabled because UDF is already selected.")
        cmdUseCurrent.Visible = False
        Me.AcceptButton = IIf(SelectedFS = 5, cmdUseUDF102, cmdUseUDF)
        lblDescription.Text = "The UDF File System can handle files 4 GB and larger, and everything should be fine. I'm not sure why you're seeing this message. Please tell me how you got here." & vbNewLine & vbNewLine & "SLIPS7REAM suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:"
    End Select
  End Sub
  Private Sub cmdUseUDF_Click(sender As System.Object, e As System.EventArgs) Handles cmdUseUDF.Click
    Me.DialogResult = Windows.Forms.DialogResult.Yes
    Me.Close()
  End Sub
  Private Sub cmdUseUDF102_Click(sender As System.Object, e As System.EventArgs) Handles cmdUseUDF102.Click
    Me.DialogResult = Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub
  Private Sub cmdUseCurrent_Click(sender As System.Object, e As System.EventArgs) Handles cmdUseCurrent.Click
    Me.DialogResult = Windows.Forms.DialogResult.No
    Me.Close()
  End Sub
  Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
    Me.DialogResult = Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub
End Class
