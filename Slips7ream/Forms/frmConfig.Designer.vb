<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.txtTimeout = New System.Windows.Forms.NumericUpDown()
    Me.lblTemp = New System.Windows.Forms.Label()
    Me.txtTemp = New System.Windows.Forms.TextBox()
    Me.cmdTemp = New System.Windows.Forms.Button()
    Me.lblWhitelist = New System.Windows.Forms.Label()
    Me.txtWhitelist = New System.Windows.Forms.TextBox()
    Me.lblTimeoutS = New System.Windows.Forms.Label()
    Me.chkAlert = New System.Windows.Forms.CheckBox()
    Me.pnlAlert = New System.Windows.Forms.TableLayoutPanel()
    Me.txtAlertPath = New System.Windows.Forms.TextBox()
    Me.cmdAlertBrowse = New System.Windows.Forms.Button()
    Me.cmdPlay = New System.Windows.Forms.Button()
    Me.chkDefault = New System.Windows.Forms.CheckBox()
    Me.lblDonate = New Slips7ream.LinkLabel()
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
    Me.pnlConfig.Controls.Add(Me.lblTimeout, 0, 2)
    Me.pnlConfig.Controls.Add(Me.cmdClose, 3, 4)
    Me.pnlConfig.Controls.Add(Me.cmdSave, 2, 4)
    Me.pnlConfig.Controls.Add(Me.txtTimeout, 1, 2)
    Me.pnlConfig.Controls.Add(Me.lblTemp, 0, 0)
    Me.pnlConfig.Controls.Add(Me.txtTemp, 1, 0)
    Me.pnlConfig.Controls.Add(Me.cmdTemp, 3, 0)
    Me.pnlConfig.Controls.Add(Me.lblWhitelist, 0, 1)
    Me.pnlConfig.Controls.Add(Me.txtWhitelist, 1, 1)
    Me.pnlConfig.Controls.Add(Me.lblDonate, 0, 4)
    Me.pnlConfig.Controls.Add(Me.lblTimeoutS, 2, 2)
    Me.pnlConfig.Controls.Add(Me.chkAlert, 0, 3)
    Me.pnlConfig.Controls.Add(Me.pnlAlert, 1, 3)
    Me.pnlConfig.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlConfig.Location = New System.Drawing.Point(0, 0)
    Me.pnlConfig.Name = "pnlConfig"
    Me.pnlConfig.RowCount = 5
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.Size = New System.Drawing.Size(461, 232)
    Me.pnlConfig.TabIndex = 0
    '
    'lblTimeout
    '
    Me.lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout.AutoSize = True
    Me.lblTimeout.Location = New System.Drawing.Point(3, 153)
    Me.lblTimeout.Name = "lblTimeout"
    Me.lblTimeout.Size = New System.Drawing.Size(98, 13)
    Me.lblTimeout.TabIndex = 6
    Me.lblTimeout.Text = "Command Time&out:"
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(383, 206)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 9
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(302, 206)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 23)
    Me.cmdSave.TabIndex = 8
    Me.cmdSave.Text = "&Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'txtTimeout
    '
    Me.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.txtTimeout.Location = New System.Drawing.Point(127, 150)
    Me.txtTimeout.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
    Me.txtTimeout.Name = "txtTimeout"
    Me.txtTimeout.Size = New System.Drawing.Size(60, 20)
    Me.txtTimeout.TabIndex = 7
    '
    'lblTemp
    '
    Me.lblTemp.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTemp.AutoSize = True
    Me.lblTemp.Location = New System.Drawing.Point(3, 8)
    Me.lblTemp.Name = "lblTemp"
    Me.lblTemp.Size = New System.Drawing.Size(107, 13)
    Me.lblTemp.TabIndex = 3
    Me.lblTemp.Text = "&Temp Directory Path:"
    '
    'txtTemp
    '
    Me.txtTemp.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlConfig.SetColumnSpan(Me.txtTemp, 2)
    Me.txtTemp.Location = New System.Drawing.Point(127, 4)
    Me.txtTemp.Name = "txtTemp"
    Me.txtTemp.Size = New System.Drawing.Size(250, 20)
    Me.txtTemp.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.txtTemp, "Empty directory for the program to work in.")
    '
    'cmdTemp
    '
    Me.cmdTemp.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdTemp.Location = New System.Drawing.Point(383, 3)
    Me.cmdTemp.Name = "cmdTemp"
    Me.cmdTemp.Size = New System.Drawing.Size(75, 23)
    Me.cmdTemp.TabIndex = 5
    Me.cmdTemp.Text = "B&rowse..."
    Me.cmdTemp.UseVisualStyleBackColor = True
    '
    'lblWhitelist
    '
    Me.lblWhitelist.AutoSize = True
    Me.lblWhitelist.Location = New System.Drawing.Point(3, 35)
    Me.lblWhitelist.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
    Me.lblWhitelist.Name = "lblWhitelist"
    Me.lblWhitelist.Size = New System.Drawing.Size(108, 13)
    Me.lblWhitelist.TabIndex = 11
    Me.lblWhitelist.Text = "x86 Update &Whitelist:"
    '
    'txtWhitelist
    '
    Me.pnlConfig.SetColumnSpan(Me.txtWhitelist, 3)
    Me.txtWhitelist.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtWhitelist.Location = New System.Drawing.Point(127, 32)
    Me.txtWhitelist.Multiline = True
    Me.txtWhitelist.Name = "txtWhitelist"
    Me.txtWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtWhitelist.Size = New System.Drawing.Size(331, 112)
    Me.txtWhitelist.TabIndex = 12
    Me.ttInfo.SetTooltip(Me.txtWhitelist, resources.GetString("txtWhitelist.ToolTip"))
    '
    'lblTimeoutS
    '
    Me.lblTimeoutS.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeoutS.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblTimeoutS, 2)
    Me.lblTimeoutS.Location = New System.Drawing.Point(193, 153)
    Me.lblTimeoutS.Name = "lblTimeoutS"
    Me.lblTimeoutS.Size = New System.Drawing.Size(43, 13)
    Me.lblTimeoutS.TabIndex = 10
    Me.lblTimeoutS.Text = "minutes"
    '
    'chkAlert
    '
    Me.chkAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkAlert.AutoSize = True
    Me.chkAlert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkAlert.Location = New System.Drawing.Point(3, 179)
    Me.chkAlert.Name = "chkAlert"
    Me.chkAlert.Size = New System.Drawing.Size(118, 18)
    Me.chkAlert.TabIndex = 14
    Me.chkAlert.Text = "&Alert on Complete:"
    Me.ttInfo.SetTooltip(Me.chkAlert, "Play an auditory alert when the Slipstream process has completed.")
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
    Me.pnlAlert.Location = New System.Drawing.Point(124, 173)
    Me.pnlAlert.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAlert.Name = "pnlAlert"
    Me.pnlAlert.RowCount = 1
    Me.pnlAlert.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAlert.Size = New System.Drawing.Size(337, 30)
    Me.pnlAlert.TabIndex = 15
    '
    'txtAlertPath
    '
    Me.txtAlertPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAlertPath.Enabled = False
    Me.txtAlertPath.Location = New System.Drawing.Point(3, 5)
    Me.txtAlertPath.Name = "txtAlertPath"
    Me.txtAlertPath.Size = New System.Drawing.Size(238, 20)
    Me.txtAlertPath.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtAlertPath, "Path to the WAV file to play when the Slipstream process has completed.")
    '
    'cmdAlertBrowse
    '
    Me.cmdAlertBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdAlertBrowse.Enabled = False
    Me.cmdAlertBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAlertBrowse.Location = New System.Drawing.Point(247, 3)
    Me.cmdAlertBrowse.Name = "cmdAlertBrowse"
    Me.cmdAlertBrowse.Size = New System.Drawing.Size(29, 23)
    Me.cmdAlertBrowse.TabIndex = 1
    Me.cmdAlertBrowse.Text = ". . ."
    Me.cmdAlertBrowse.UseVisualStyleBackColor = True
    '
    'cmdPlay
    '
    Me.cmdPlay.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdPlay.Enabled = False
    Me.cmdPlay.Image = Global.Slips7ream.My.Resources.Resources.play
    Me.cmdPlay.Location = New System.Drawing.Point(311, 3)
    Me.cmdPlay.Name = "cmdPlay"
    Me.cmdPlay.Size = New System.Drawing.Size(23, 23)
    Me.cmdPlay.TabIndex = 3
    Me.ttInfo.SetTooltip(Me.cmdPlay, "Play the Alert sound.")
    Me.cmdPlay.UseVisualStyleBackColor = True
    '
    'chkDefault
    '
    Me.chkDefault.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.chkDefault.Appearance = System.Windows.Forms.Appearance.Button
    Me.chkDefault.Enabled = False
    Me.chkDefault.Image = Global.Slips7ream.My.Resources.Resources.win
    Me.chkDefault.Location = New System.Drawing.Point(282, 3)
    Me.chkDefault.Name = "chkDefault"
    Me.chkDefault.Size = New System.Drawing.Size(23, 23)
    Me.chkDefault.TabIndex = 4
    Me.chkDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttInfo.SetTooltip(Me.chkDefault, "Use the System Default Alert sound.")
    Me.chkDefault.UseVisualStyleBackColor = True
    '
    'lblDonate
    '
    Me.lblDonate.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblDonate.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblDonate, 2)
    Me.lblDonate.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblDonate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblDonate.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblDonate.Location = New System.Drawing.Point(50, 211)
    Me.lblDonate.Name = "lblDonate"
    Me.lblDonate.Size = New System.Drawing.Size(89, 13)
    Me.lblDonate.TabIndex = 13
    Me.lblDonate.Text = "Make a &Donation"
    '
    'frmConfig
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(461, 232)
    Me.Controls.Add(Me.pnlConfig)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmConfig"
    Me.ShowInTaskbar = False
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
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
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
End Class
