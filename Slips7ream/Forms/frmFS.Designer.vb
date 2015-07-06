<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFS
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFS))
    Me.pnlFS = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDescription = New System.Windows.Forms.Label()
    Me.txtLargeFiles = New System.Windows.Forms.TextBox()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdUseUDF = New System.Windows.Forms.Button()
    Me.cmdUseUDF102 = New System.Windows.Forms.Button()
    Me.cmdUseCurrent = New System.Windows.Forms.Button()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.ttFS = New System.Windows.Forms.ToolTip(Me.components)
    Me.pnlFS.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlFS
    '
    Me.pnlFS.ColumnCount = 1
    Me.pnlFS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlFS.Controls.Add(Me.lblDescription, 0, 0)
    Me.pnlFS.Controls.Add(Me.txtLargeFiles, 0, 1)
    Me.pnlFS.Controls.Add(Me.pnlButtons, 0, 2)
    Me.pnlFS.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlFS.Location = New System.Drawing.Point(0, 0)
    Me.pnlFS.Name = "pnlFS"
    Me.pnlFS.RowCount = 3
    Me.pnlFS.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlFS.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlFS.Size = New System.Drawing.Size(478, 180)
    Me.pnlFS.TabIndex = 0
    '
    'lblDescription
    '
    Me.lblDescription.AutoSize = True
    Me.lblDescription.Location = New System.Drawing.Point(3, 0)
    Me.lblDescription.Name = "lblDescription"
    Me.lblDescription.Size = New System.Drawing.Size(464, 65)
    Me.lblDescription.TabIndex = 1
    Me.lblDescription.Text = resources.GetString("lblDescription.Text")
    '
    'txtLargeFiles
    '
    Me.txtLargeFiles.BackColor = System.Drawing.SystemColors.Window
    Me.txtLargeFiles.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtLargeFiles.Location = New System.Drawing.Point(3, 68)
    Me.txtLargeFiles.Multiline = True
    Me.txtLargeFiles.Name = "txtLargeFiles"
    Me.txtLargeFiles.ReadOnly = True
    Me.txtLargeFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtLargeFiles.Size = New System.Drawing.Size(472, 78)
    Me.txtLargeFiles.TabIndex = 2
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 4
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdUseUDF, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdUseUDF102, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdUseCurrent, 2, 0)
    Me.pnlButtons.Controls.Add(Me.cmdCancel, 3, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(80, 149)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(398, 31)
    Me.pnlButtons.TabIndex = 0
    '
    'cmdUseUDF
    '
    Me.cmdUseUDF.AutoSize = True
    Me.cmdUseUDF.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdUseUDF.Location = New System.Drawing.Point(3, 3)
    Me.cmdUseUDF.Name = "cmdUseUDF"
    Me.cmdUseUDF.Size = New System.Drawing.Size(75, 23)
    Me.cmdUseUDF.TabIndex = 0
    Me.cmdUseUDF.Text = "Use U&DF"
    Me.ttFS.SetToolTip(Me.cmdUseUDF, "Use Universal Disk Format, or ISO/IEC 13346. Superior standard, but less compatib" & _
        "le with older hardware." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This is the best option for a modern system.")
    Me.cmdUseUDF.UseVisualStyleBackColor = True
    '
    'cmdUseUDF102
    '
    Me.cmdUseUDF102.AutoSize = True
    Me.cmdUseUDF102.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdUseUDF102.Location = New System.Drawing.Point(84, 3)
    Me.cmdUseUDF102.Name = "cmdUseUDF102"
    Me.cmdUseUDF102.Size = New System.Drawing.Size(89, 23)
    Me.cmdUseUDF102.TabIndex = 1
    Me.cmdUseUDF102.Text = "Use &UDF 1.02"
    Me.ttFS.SetToolTip(Me.cmdUseUDF102, "Use legacy UDF, which is understood by most drives. Compatible with old hardware." & _
        "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This is a good option if your disc drive is old and limited.")
    Me.cmdUseUDF102.UseVisualStyleBackColor = True
    '
    'cmdUseCurrent
    '
    Me.cmdUseCurrent.AutoSize = True
    Me.cmdUseCurrent.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdUseCurrent.Location = New System.Drawing.Point(179, 3)
    Me.cmdUseCurrent.Name = "cmdUseCurrent"
    Me.cmdUseCurrent.Size = New System.Drawing.Size(93, 23)
    Me.cmdUseCurrent.TabIndex = 2
    Me.cmdUseCurrent.Text = "Use Current &FS"
    Me.ttFS.SetToolTip(Me.cmdUseCurrent, "Use the File System you already selected, damn the consequences.")
    Me.cmdUseCurrent.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.AutoSize = True
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(278, 3)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(117, 25)
    Me.cmdCancel.TabIndex = 3
    Me.cmdCancel.Text = "&Cancel ISO Creation"
    Me.ttFS.SetToolTip(Me.cmdCancel, "Don't make an ISO, cancel any further action.")
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'frmFS
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(478, 180)
    Me.Controls.Add(Me.pnlFS)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmFS"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "SLIPS7REAM File System Size Conflict"
    Me.pnlFS.ResumeLayout(False)
    Me.pnlFS.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlFS As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDescription As System.Windows.Forms.Label
  Friend WithEvents txtLargeFiles As System.Windows.Forms.TextBox
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdUseUDF As System.Windows.Forms.Button
  Friend WithEvents cmdUseUDF102 As System.Windows.Forms.Button
  Friend WithEvents cmdUseCurrent As System.Windows.Forms.Button
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents ttFS As System.Windows.Forms.ToolTip
End Class
