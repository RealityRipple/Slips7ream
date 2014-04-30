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
    Me.lblDonate = New Slips7ream.LinkLabel()
    Me.lblTimeoutS = New System.Windows.Forms.Label()
    Me.ttInfo = New Slips7ream.ToolTip(Me.components)
    Me.pnlConfig.SuspendLayout()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.pnlConfig.Controls.Add(Me.cmdClose, 3, 3)
    Me.pnlConfig.Controls.Add(Me.cmdSave, 2, 3)
    Me.pnlConfig.Controls.Add(Me.txtTimeout, 1, 2)
    Me.pnlConfig.Controls.Add(Me.lblTemp, 0, 0)
    Me.pnlConfig.Controls.Add(Me.txtTemp, 1, 0)
    Me.pnlConfig.Controls.Add(Me.cmdTemp, 3, 0)
    Me.pnlConfig.Controls.Add(Me.lblWhitelist, 0, 1)
    Me.pnlConfig.Controls.Add(Me.txtWhitelist, 1, 1)
    Me.pnlConfig.Controls.Add(Me.lblDonate, 0, 3)
    Me.pnlConfig.Controls.Add(Me.lblTimeoutS, 2, 2)
    Me.pnlConfig.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlConfig.Location = New System.Drawing.Point(0, 0)
    Me.pnlConfig.Name = "pnlConfig"
    Me.pnlConfig.RowCount = 4
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlConfig.Size = New System.Drawing.Size(398, 180)
    Me.pnlConfig.TabIndex = 0
    '
    'lblTimeout
    '
    Me.lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout.AutoSize = True
    Me.lblTimeout.Location = New System.Drawing.Point(3, 131)
    Me.lblTimeout.Name = "lblTimeout"
    Me.lblTimeout.Size = New System.Drawing.Size(98, 13)
    Me.lblTimeout.TabIndex = 6
    Me.lblTimeout.Text = "Command Timeout:"
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.Location = New System.Drawing.Point(320, 154)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 9
    Me.cmdClose.Text = "Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSave.Location = New System.Drawing.Point(239, 154)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 23)
    Me.cmdSave.TabIndex = 8
    Me.cmdSave.Text = "Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'txtTimeout
    '
    Me.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.txtTimeout.Location = New System.Drawing.Point(117, 128)
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
    Me.lblTemp.Text = "Temp Directory Path:"
    '
    'txtTemp
    '
    Me.txtTemp.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlConfig.SetColumnSpan(Me.txtTemp, 2)
    Me.txtTemp.Location = New System.Drawing.Point(117, 4)
    Me.txtTemp.Name = "txtTemp"
    Me.txtTemp.Size = New System.Drawing.Size(197, 20)
    Me.txtTemp.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.txtTemp, "Empty directory for the program to work in.")
    '
    'cmdTemp
    '
    Me.cmdTemp.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdTemp.Location = New System.Drawing.Point(320, 3)
    Me.cmdTemp.Name = "cmdTemp"
    Me.cmdTemp.Size = New System.Drawing.Size(75, 23)
    Me.cmdTemp.TabIndex = 5
    Me.cmdTemp.Text = "Browse..."
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
    Me.lblWhitelist.Text = "x86 Update Whitelist:"
    '
    'txtWhitelist
    '
    Me.pnlConfig.SetColumnSpan(Me.txtWhitelist, 3)
    Me.txtWhitelist.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtWhitelist.Location = New System.Drawing.Point(117, 32)
    Me.txtWhitelist.Multiline = True
    Me.txtWhitelist.Name = "txtWhitelist"
    Me.txtWhitelist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtWhitelist.Size = New System.Drawing.Size(278, 90)
    Me.txtWhitelist.TabIndex = 12
    Me.ttInfo.SetTooltip(Me.txtWhitelist, resources.GetString("txtWhitelist.ToolTip"))
    '
    'lblDonate
    '
    Me.lblDonate.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblDonate.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblDonate, 2)
    Me.lblDonate.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblDonate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline)
    Me.lblDonate.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblDonate.Location = New System.Drawing.Point(45, 159)
    Me.lblDonate.Name = "lblDonate"
    Me.lblDonate.Size = New System.Drawing.Size(89, 13)
    Me.lblDonate.TabIndex = 13
    Me.lblDonate.Text = "Make a Donation"
    '
    'lblTimeoutS
    '
    Me.lblTimeoutS.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeoutS.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.lblTimeoutS, 2)
    Me.lblTimeoutS.Location = New System.Drawing.Point(183, 131)
    Me.lblTimeoutS.Name = "lblTimeoutS"
    Me.lblTimeoutS.Size = New System.Drawing.Size(43, 13)
    Me.lblTimeoutS.TabIndex = 10
    Me.lblTimeoutS.Text = "minutes"
    '
    'frmConfig
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(398, 180)
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
End Class
