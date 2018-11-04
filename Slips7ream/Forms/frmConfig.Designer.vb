<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
    Me.pnlConfig = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTimeout = New System.Windows.Forms.Label()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.cmdOK = New System.Windows.Forms.Button()
    Me.txtTimeout = New System.Windows.Forms.NumericUpDown()
    Me.lblTemp = New System.Windows.Forms.Label()
    Me.txtTemp = New System.Windows.Forms.TextBox()
    Me.cmdTemp = New System.Windows.Forms.Button()
    Me.lblWhitelist = New System.Windows.Forms.Label()
    Me.txtWhitelist = New System.Windows.Forms.TextBox()
    Me.lblDonate = New Slips7ream.LinkLabel()
    Me.lblTimeoutS = New System.Windows.Forms.Label()
    Me.chkAlert = New System.Windows.Forms.CheckBox()
    Me.pnlAlert = New System.Windows.Forms.TableLayoutPanel()
    Me.txtAlertPath = New System.Windows.Forms.TextBox()
    Me.cmdAlertBrowse = New System.Windows.Forms.Button()
    Me.cmdPlay = New System.Windows.Forms.Button()
    Me.chkDefault = New System.Windows.Forms.CheckBox()
    Me.chkHideDriverOutput = New System.Windows.Forms.CheckBox()
    Me.lblUpdateDB = New System.Windows.Forms.Label()
    Me.lblUpdateDBVer = New System.Windows.Forms.Label()
    Me.cmdUpdateDBCheck = New System.Windows.Forms.Button()
    Me.helpS7M = New System.Windows.Forms.HelpProvider()
    Me.ttInfo = New Slips7ream.ToolTip(Me.components)
    Me.pnlConfig.SuspendLayout()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAlert.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlConfig
    '
    Me.pnlConfig.ColumnCount = 4
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.Controls.Add(Me.lblTimeout, 0, 3)
    Me.pnlConfig.Controls.Add(Me.cmdCancel, 3, 6)
    Me.pnlConfig.Controls.Add(Me.cmdOK, 2, 6)
    Me.pnlConfig.Controls.Add(Me.txtTimeout, 1, 3)
    Me.pnlConfig.Controls.Add(Me.lblTemp, 0, 0)
    Me.pnlConfig.Controls.Add(Me.txtTemp, 1, 0)
    Me.pnlConfig.Controls.Add(Me.cmdTemp, 3, 0)
    Me.pnlConfig.Controls.Add(Me.lblWhitelist, 0, 2)
    Me.pnlConfig.Controls.Add(Me.txtWhitelist, 1, 2)
    Me.pnlConfig.Controls.Add(Me.lblDonate, 0, 6)
    Me.pnlConfig.Controls.Add(Me.lblTimeoutS, 2, 3)
    Me.pnlConfig.Controls.Add(Me.chkAlert, 0, 5)
    Me.pnlConfig.Controls.Add(Me.pnlAlert, 1, 5)
    Me.pnlConfig.Controls.Add(Me.chkHideDriverOutput, 1, 4)
    Me.pnlConfig.Controls.Add(Me.lblUpdateDB, 0, 1)
    Me.pnlConfig.Controls.Add(Me.lblUpdateDBVer, 1, 1)
    Me.pnlConfig.Controls.Add(Me.cmdUpdateDBCheck, 2, 1)
    Me.pnlConfig.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlConfig.Location = New System.Drawing.Point(0, 0)
    Me.pnlConfig.Name = "pnlConfig"
    Me.pnlConfig.RowCount = 7
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.Size = New System.Drawing.Size(461, 257)
    Me.pnlConfig.TabIndex = 0
    '
    'lblTimeout
    '
    Me.lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout.AutoSize = True
    Me.helpS7M.SetHelpKeyword(Me.lblTimeout, "/2_Configuration/2.3_Command_Timeout.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblTimeout, System.Windows.Forms.HelpNavigator.Topic)
    Me.lblTimeout.Location = New System.Drawing.Point(3, 154)
    Me.lblTimeout.Name = "lblTimeout"
    Me.helpS7M.SetShowHelp(Me.lblTimeout, True)
    Me.lblTimeout.Size = New System.Drawing.Size(98, 13)
    Me.lblTimeout.TabIndex = 8
    Me.lblTimeout.Text = "Command Time&out:"
    '
    'cmdCancel
    '
    Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdCancel, "/2_Configuration/2.0_Configuration.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdCancel, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.cmdCancel, "Close the SLIPS7REAM Configuration window.")
    Me.cmdCancel.Location = New System.Drawing.Point(383, 231)
    Me.cmdCancel.Name = "cmdCancel"
    Me.helpS7M.SetShowHelp(Me.cmdCancel, True)
    Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
    Me.cmdCancel.TabIndex = 15
    Me.cmdCancel.Text = "Cancel"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'cmdOK
    '
    Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdOK, "/2_Configuration/2.0_Configuration.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdOK, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.cmdOK, "Save changes to your settings and close the SLIPS7REAM Configuration window.")
    Me.cmdOK.Location = New System.Drawing.Point(302, 231)
    Me.cmdOK.Name = "cmdOK"
    Me.helpS7M.SetShowHelp(Me.cmdOK, True)
    Me.cmdOK.Size = New System.Drawing.Size(75, 23)
    Me.cmdOK.TabIndex = 14
    Me.cmdOK.Text = "OK"
    Me.cmdOK.UseVisualStyleBackColor = True
    '
    'txtTimeout
    '
    Me.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.helpS7M.SetHelpKeyword(Me.txtTimeout, "/2_Configuration/2.3_Command_Timeout.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtTimeout, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtTimeout, resources.GetString("txtTimeout.HelpString"))
    Me.txtTimeout.Location = New System.Drawing.Point(127, 151)
    Me.txtTimeout.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
    Me.txtTimeout.Name = "txtTimeout"
    Me.helpS7M.SetShowHelp(Me.txtTimeout, True)
    Me.txtTimeout.Size = New System.Drawing.Size(60, 20)
    Me.txtTimeout.TabIndex = 9
    '
    'lblTemp
    '
    Me.lblTemp.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTemp.AutoSize = True
    Me.helpS7M.SetHelpKeyword(Me.lblTemp, "/2_Configuration/2.1_Temp_Directory_Path.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblTemp, System.Windows.Forms.HelpNavigator.Topic)
    Me.lblTemp.Location = New System.Drawing.Point(3, 8)
    Me.lblTemp.Name = "lblTemp"
    Me.helpS7M.SetShowHelp(Me.lblTemp, True)
    Me.lblTemp.Size = New System.Drawing.Size(107, 13)
    Me.lblTemp.TabIndex = 0
    Me.lblTemp.Text = "&Temp Directory Path:"
    '
    'txtTemp
    '
    Me.txtTemp.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlConfig.SetColumnSpan(Me.txtTemp, 2)
    Me.helpS7M.SetHelpKeyword(Me.txtTemp, "/2_Configuration/2.1_Temp_Directory_Path.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtTemp, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtTemp, resources.GetString("txtTemp.HelpString"))
    Me.txtTemp.Location = New System.Drawing.Point(127, 4)
    Me.txtTemp.Name = "txtTemp"
    Me.helpS7M.SetShowHelp(Me.txtTemp, True)
    Me.txtTemp.Size = New System.Drawing.Size(250, 20)
    Me.txtTemp.TabIndex = 1
    Me.ttInfo.SetToolTip(Me.txtTemp, "Empty directory for the program to work in.")
    '
    'cmdTemp
    '
    Me.cmdTemp.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.helpS7M.SetHelpKeyword(Me.cmdTemp, "/2_Configuration/2.1_Temp_Directory_Path.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdTemp, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdTemp.Location = New System.Drawing.Point(383, 3)
    Me.cmdTemp.Name = "cmdTemp"
    Me.helpS7M.SetShowHelp(Me.cmdTemp, True)
    Me.cmdTemp.Size = New System.Drawing.Size(75, 23)
    Me.cmdTemp.TabIndex = 2
    Me.cmdTemp.Text = "B&rowse..."
    Me.cmdTemp.UseVisualStyleBackColor = True
    '
    'lblWhitelist
    '
    Me.lblWhitelist.AutoSize = True
    Me.helpS7M.SetHelpKeyword(Me.lblWhitelist, "/2_Configuration/2.2_Update_Whitelist.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblWhitelist, System.Windows.Forms.HelpNavigator.Topic)
    Me.lblWhitelist.Location = New System.Drawing.Point(3, 64)
    Me.lblWhitelist.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
    Me.lblWhitelist.Name = "lblWhitelist"
    Me.helpS7M.SetShowHelp(Me.lblWhitelist, True)
    Me.lblWhitelist.Size = New System.Drawing.Size(108, 13)
    Me.lblWhitelist.TabIndex = 6
    Me.lblWhitelist.Text = "x86 Update &Whitelist:"
    '
    'txtWhitelist
    '
    Me.txtWhitelist.AcceptsReturn = True
    Me.pnlConfig.SetColumnSpan(Me.txtWhitelist, 3)
    Me.txtWhitelist.Dock = System.Windows.Forms.DockStyle.Fill
    Me.helpS7M.SetHelpKeyword(Me.txtWhitelist, "/2_Configuration/2.2_Update_Whitelist.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtWhitelist, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtWhitelist, resources.GetString("txtWhitelist.HelpString"))
    Me.txtWhitelist.Location = New System.Drawing.Point(127, 61)
    Me.txtWhitelist.Multiline = True
    Me.txtWhitelist.Name = "txtWhitelist"
    Me.txtWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.helpS7M.SetShowHelp(Me.txtWhitelist, True)
    Me.txtWhitelist.Size = New System.Drawing.Size(331, 84)
    Me.txtWhitelist.TabIndex = 7
    Me.ttInfo.SetToolTip(Me.txtWhitelist, resources.GetString("txtWhitelist.ToolTip"))
    '
    'lblDonate
    '
    Me.lblDonate.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblDonate.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblDonate, 2)
    Me.lblDonate.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblDonate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline)
    Me.lblDonate.ForeColor = System.Drawing.Color.MediumBlue
    Me.helpS7M.SetHelpString(Me.lblDonate, "Click this link to visit realityripple.com and make a donation to support develop" & _
        "ment of SLIPS7REAM.")
    Me.lblDonate.Location = New System.Drawing.Point(50, 236)
    Me.lblDonate.Name = "lblDonate"
    Me.helpS7M.SetShowHelp(Me.lblDonate, True)
    Me.lblDonate.Size = New System.Drawing.Size(89, 13)
    Me.lblDonate.TabIndex = 16
    Me.lblDonate.Text = "Make a Donation"
    '
    'lblTimeoutS
    '
    Me.lblTimeoutS.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeoutS.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblTimeoutS, 2)
    Me.helpS7M.SetHelpKeyword(Me.lblTimeoutS, "/2_Configuration/2.3_Command_Timeout.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblTimeoutS, System.Windows.Forms.HelpNavigator.Topic)
    Me.lblTimeoutS.Location = New System.Drawing.Point(193, 154)
    Me.lblTimeoutS.Name = "lblTimeoutS"
    Me.helpS7M.SetShowHelp(Me.lblTimeoutS, True)
    Me.lblTimeoutS.Size = New System.Drawing.Size(43, 13)
    Me.lblTimeoutS.TabIndex = 10
    Me.lblTimeoutS.Text = "minutes"
    '
    'chkAlert
    '
    Me.chkAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkAlert.AutoSize = True
    Me.chkAlert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkAlert, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkAlert, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.chkAlert, "The Alert on Complete feature lets you play a sound effect when the Slipstream Pr" & _
        "ocess completes.")
    Me.chkAlert.Location = New System.Drawing.Point(3, 204)
    Me.chkAlert.Name = "chkAlert"
    Me.helpS7M.SetShowHelp(Me.chkAlert, True)
    Me.chkAlert.Size = New System.Drawing.Size(118, 18)
    Me.chkAlert.TabIndex = 12
    Me.chkAlert.Text = "&Alert on Complete:"
    Me.ttInfo.SetToolTip(Me.chkAlert, "Play an auditory alert when the Slipstream process has completed.")
    Me.chkAlert.UseVisualStyleBackColor = True
    '
    'pnlAlert
    '
    Me.pnlAlert.ColumnCount = 4
    Me.pnlConfig.SetColumnSpan(Me.pnlAlert, 3)
    Me.pnlAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAlert.Controls.Add(Me.txtAlertPath, 0, 0)
    Me.pnlAlert.Controls.Add(Me.cmdAlertBrowse, 1, 0)
    Me.pnlAlert.Controls.Add(Me.cmdPlay, 3, 0)
    Me.pnlAlert.Controls.Add(Me.chkDefault, 2, 0)
    Me.pnlAlert.Dock = System.Windows.Forms.DockStyle.Fill
    Me.helpS7M.SetHelpKeyword(Me.pnlAlert, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.pnlAlert, System.Windows.Forms.HelpNavigator.Topic)
    Me.pnlAlert.Location = New System.Drawing.Point(124, 198)
    Me.pnlAlert.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAlert.Name = "pnlAlert"
    Me.pnlAlert.RowCount = 1
    Me.pnlAlert.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.helpS7M.SetShowHelp(Me.pnlAlert, True)
    Me.pnlAlert.Size = New System.Drawing.Size(337, 30)
    Me.pnlAlert.TabIndex = 13
    '
    'txtAlertPath
    '
    Me.txtAlertPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAlertPath.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.txtAlertPath, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtAlertPath, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtAlertPath, "The Alert on Complete feature lets you play a sound effect when the Slipstream Pr" & _
        "ocess completes." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This file should be a WAV audio file, although any audio file " & _
        "may be selected.")
    Me.txtAlertPath.Location = New System.Drawing.Point(3, 5)
    Me.txtAlertPath.Name = "txtAlertPath"
    Me.helpS7M.SetShowHelp(Me.txtAlertPath, True)
    Me.txtAlertPath.Size = New System.Drawing.Size(238, 20)
    Me.txtAlertPath.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtAlertPath, "Path to the WAV file to play when the Slipstream process has completed.")
    '
    'cmdAlertBrowse
    '
    Me.cmdAlertBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdAlertBrowse.Enabled = False
    Me.cmdAlertBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdAlertBrowse, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdAlertBrowse, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.cmdAlertBrowse, "Click this button to browse for an audio file to use as the Alert.")
    Me.cmdAlertBrowse.Location = New System.Drawing.Point(247, 3)
    Me.cmdAlertBrowse.Name = "cmdAlertBrowse"
    Me.helpS7M.SetShowHelp(Me.cmdAlertBrowse, True)
    Me.cmdAlertBrowse.Size = New System.Drawing.Size(29, 23)
    Me.cmdAlertBrowse.TabIndex = 1
    Me.cmdAlertBrowse.Text = ". . ."
    Me.cmdAlertBrowse.UseVisualStyleBackColor = True
    '
    'cmdPlay
    '
    Me.cmdPlay.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdPlay.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.cmdPlay, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdPlay, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.cmdPlay, "This button will test the playback of the selected Alert audio file.")
    Me.cmdPlay.Image = CType(resources.GetObject("cmdPlay.Image"), System.Drawing.Image)
    Me.cmdPlay.Location = New System.Drawing.Point(311, 3)
    Me.cmdPlay.Name = "cmdPlay"
    Me.helpS7M.SetShowHelp(Me.cmdPlay, True)
    Me.cmdPlay.Size = New System.Drawing.Size(23, 23)
    Me.cmdPlay.TabIndex = 3
    Me.ttInfo.SetToolTip(Me.cmdPlay, "Play the Alert sound.")
    Me.cmdPlay.UseVisualStyleBackColor = True
    '
    'chkDefault
    '
    Me.chkDefault.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.chkDefault.Appearance = System.Windows.Forms.Appearance.Button
    Me.chkDefault.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.chkDefault, "/2_Configuration/2.5_Alert_on_Complete.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkDefault, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.chkDefault, "Instead of using a custom sound file, you can use the built-in Windows ""tada.wav""" & _
        " sound file or default alert sound.")
    Me.chkDefault.Image = CType(resources.GetObject("chkDefault.Image"), System.Drawing.Image)
    Me.chkDefault.Location = New System.Drawing.Point(282, 3)
    Me.chkDefault.Name = "chkDefault"
    Me.helpS7M.SetShowHelp(Me.chkDefault, True)
    Me.chkDefault.Size = New System.Drawing.Size(23, 23)
    Me.chkDefault.TabIndex = 2
    Me.chkDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttInfo.SetToolTip(Me.chkDefault, "Use the System Default Alert sound.")
    Me.chkDefault.UseVisualStyleBackColor = True
    '
    'chkHideDriverOutput
    '
    Me.chkHideDriverOutput.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.chkHideDriverOutput, 2)
    Me.chkHideDriverOutput.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkHideDriverOutput, "/2_Configuration/2.4_Hide_Driver_Console_Output.htm")
    Me.helpS7M.SetHelpString(Me.chkHideDriverOutput, resources.GetString("chkHideDriverOutput.HelpString"))
    Me.chkHideDriverOutput.Location = New System.Drawing.Point(127, 177)
    Me.chkHideDriverOutput.Name = "chkHideDriverOutput"
    Me.helpS7M.SetShowHelp(Me.chkHideDriverOutput, True)
    Me.chkHideDriverOutput.Size = New System.Drawing.Size(161, 18)
    Me.chkHideDriverOutput.TabIndex = 11
    Me.chkHideDriverOutput.Text = "Hide &Driver Console Output"
    Me.chkHideDriverOutput.UseVisualStyleBackColor = True
    '
    'lblUpdateDB
    '
    Me.lblUpdateDB.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUpdateDB.AutoSize = True
    Me.helpS7M.SetHelpKeyword(Me.lblUpdateDB, "/2_Configuration/2.2_KB_Article_Databases.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblUpdateDB, System.Windows.Forms.HelpNavigator.Topic)
    Me.lblUpdateDB.Location = New System.Drawing.Point(3, 37)
    Me.lblUpdateDB.Name = "lblUpdateDB"
    Me.helpS7M.SetShowHelp(Me.lblUpdateDB, True)
    Me.lblUpdateDB.Size = New System.Drawing.Size(110, 13)
    Me.lblUpdateDB.TabIndex = 3
    Me.lblUpdateDB.Text = "&KB Article Databases:"
    '
    'lblUpdateDBVer
    '
    Me.lblUpdateDBVer.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUpdateDBVer.AutoSize = True
    Me.helpS7M.SetHelpKeyword(Me.lblUpdateDBVer, "/2_Configuration/2.2_KB_Article_Databases.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblUpdateDBVer, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.lblUpdateDBVer, resources.GetString("lblUpdateDBVer.HelpString"))
    Me.lblUpdateDBVer.Location = New System.Drawing.Point(141, 30)
    Me.lblUpdateDBVer.Name = "lblUpdateDBVer"
    Me.helpS7M.SetShowHelp(Me.lblUpdateDBVer, True)
    Me.lblUpdateDBVer.Size = New System.Drawing.Size(46, 26)
    Me.lblUpdateDBVer.TabIndex = 4
    Me.lblUpdateDBVer.Text = "2009.01" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2009.01"
    '
    'cmdUpdateDBCheck
    '
    Me.cmdUpdateDBCheck.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.helpS7M.SetHelpKeyword(Me.cmdUpdateDBCheck, "/2_Configuration/2.2_KB_Article_Databases.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdUpdateDBCheck, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdUpdateDBCheck.Location = New System.Drawing.Point(193, 32)
    Me.cmdUpdateDBCheck.Name = "cmdUpdateDBCheck"
    Me.helpS7M.SetShowHelp(Me.cmdUpdateDBCheck, True)
    Me.cmdUpdateDBCheck.Size = New System.Drawing.Size(110, 23)
    Me.cmdUpdateDBCheck.TabIndex = 5
    Me.cmdUpdateDBCheck.Text = "Update Databases"
    Me.cmdUpdateDBCheck.UseVisualStyleBackColor = True
    '
    'helpS7M
    '
    Me.helpS7M.HelpNamespace = "S7M.chm"
    '
    'frmConfig
    '
    Me.AcceptButton = Me.cmdOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(461, 257)
    Me.Controls.Add(Me.pnlConfig)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.HelpButton = True
    Me.helpS7M.SetHelpKeyword(Me, "/2_Configuration/2.0_Configuration.htm")
    Me.helpS7M.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmConfig"
    Me.helpS7M.SetShowHelp(Me, True)
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "SLIPS7REAM Configuration"
    Me.pnlConfig.ResumeLayout(False)
    Me.pnlConfig.PerformLayout()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAlert.ResumeLayout(False)
    Me.pnlAlert.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlConfig As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblTemp As System.Windows.Forms.Label
  Friend WithEvents txtTemp As System.Windows.Forms.TextBox
  Friend WithEvents cmdTemp As System.Windows.Forms.Button
  Friend WithEvents lblTimeout As System.Windows.Forms.Label
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents cmdOK As System.Windows.Forms.Button
  Friend WithEvents txtTimeout As System.Windows.Forms.NumericUpDown
  Friend WithEvents lblTimeoutS As System.Windows.Forms.Label
  Friend WithEvents ttInfo As Slips7ream.ToolTip
  Friend WithEvents lblWhitelist As System.Windows.Forms.Label
  Friend WithEvents txtWhitelist As System.Windows.Forms.TextBox
  Friend WithEvents lblDonate As Slips7ream.LinkLabel
  Friend WithEvents chkAlert As System.Windows.Forms.CheckBox
  Friend WithEvents pnlAlert As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtAlertPath As System.Windows.Forms.TextBox
  Friend WithEvents cmdAlertBrowse As System.Windows.Forms.Button
  Friend WithEvents cmdPlay As System.Windows.Forms.Button
  Friend WithEvents chkDefault As System.Windows.Forms.CheckBox
  Friend WithEvents helpS7M As System.Windows.Forms.HelpProvider
  Friend WithEvents chkHideDriverOutput As System.Windows.Forms.CheckBox
  Friend WithEvents lblUpdateDB As System.Windows.Forms.Label
  Friend WithEvents lblUpdateDBVer As System.Windows.Forms.Label
  Friend WithEvents cmdUpdateDBCheck As System.Windows.Forms.Button
End Class
