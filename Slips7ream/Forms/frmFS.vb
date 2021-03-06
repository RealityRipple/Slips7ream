﻿Public Class frmFS
  Public Sub New(OverFiles As List(Of String), SourceDir As String, SelectedFS As Byte)
    InitializeComponent()
    Dim sFiles As String = Nothing
    For Each sFile In OverFiles
      If String.IsNullOrEmpty(sFile) Then Continue For
      If sFile.Contains(sFile.Replace(String.Concat(SourceDir, IO.Path.DirectorySeparatorChar), "")) Then
        sFiles = String.Concat(sFiles, sFile.Replace(String.Concat(SourceDir, IO.Path.DirectorySeparatorChar), ""), vbNewLine)
      Else
        sFiles = String.Concat(sFiles, sFile.Replace(SourceDir, ""), vbNewLine)
      End If
    Next
    If String.IsNullOrEmpty(sFiles) Then
      txtLargeFiles.Text = String.Concat("Unable to load the file list!", vbNewLine, "You should never see this message, but if you are, a large file probably got deleted after it was detected.", vbNewLine, "You should probably stop the Slipstream process.")
    Else
      txtLargeFiles.Text = sFiles.Trim
    End If
    Select Case SelectedFS
      Case 0
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, String.Concat("Use ISO 9960 standard format, which has limited file name and size options.", vbNewLine, vbNewLine, "This option will corrupt large files."))
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = String.Concat("The ISO 9960 File System can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings.", vbNewLine, vbNewLine, "Slips7ream suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:")
      Case 1
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use Joliet"
        ttFS.SetToolTip(cmdUseCurrent, String.Concat("Use Joliet extension of ISO 9960, which has improved file names.", vbNewLine, vbNewLine, "This option will corrupt large files."))
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = String.Concat("File Systems based on ISO 9960 (like Joliet) can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings.", vbNewLine, vbNewLine, "Slips7ream suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:")
      Case 2
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use Joliet and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, String.Concat("Use Joliet extension of ISO 9960 with ISO 9960 as a backup if Joliet can't be read by the drive.", vbNewLine, vbNewLine, "Both the standard and backup formats will corrupt large files."))
        Me.AcceptButton = cmdUseUDF
        lblDescription.Text = String.Concat("File Systems based on ISO 9960 (like Joliet) can not handle files 4 GB or larger, and creating an ISO image may result in file corruption using your current settings.", vbNewLine, vbNewLine, "Slips7ream suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:")
      Case 4
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use UDF and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, String.Concat("Use UDF with ISO 9960 as a backup if UDF can't be read by the drive.", vbNewLine, vbNewLine, "The ISO 9960 backup will not read large files."))
        Me.AcceptButton = cmdUseCurrent
        lblDescription.Text = String.Concat("The ISO 9960 File System can not handle files 4 GB or larger, and the ISO 9960 backup of the UDF File System will result in file read problems if relied upon.", vbNewLine, vbNewLine, "Slips7ream suggests that you skip the ISO 9960 backup for this ISO, because the following files are greater than 4095 MB:")
      Case 6
        cmdUseCurrent.Visible = True
        cmdUseCurrent.Text = "Use UDF 1.02 and ISO 9960"
        ttFS.SetToolTip(cmdUseCurrent, String.Concat("Use UDF 1.02 with ISO 9960 as a backup if UDF 1.02 can't be read by the drive.", vbNewLine, vbNewLine, "The ISO 9960 backup will not read large files."))
        Me.AcceptButton = cmdUseCurrent
        lblDescription.Text = String.Concat("The ISO 9960 File System can not handle files 4 GB or larger, and the ISO 9960 backup of the UDF 1.02 File System will result in file read problems if relied upon.", vbNewLine, vbNewLine, "Slips7ream suggests that you skip the ISO 9960 backup for this ISO, because the following files are greater than 4095 MB:")
      Case Else
        cmdUseCurrent.Text = "Use Nothing"
        ttFS.SetToolTip(cmdUseCurrent, "This option is not enabled because UDF is already selected.")
        cmdUseCurrent.Visible = False
        If SelectedFS = 5 Then
          Me.AcceptButton = cmdUseUDF102
        Else
          Me.AcceptButton = cmdUseUDF
        End If
        lblDescription.Text = String.Concat("The UDF File System can handle files 4 GB and larger, and everything should be fine. I'm not sure why you're seeing this message. Please tell me how you got here.", vbNewLine, vbNewLine, "Slips7ream suggests you use a UDF-based file system for this ISO, because the following files are greater than 4095 MB:")
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
