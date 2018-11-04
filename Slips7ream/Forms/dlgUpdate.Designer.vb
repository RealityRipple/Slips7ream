<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgUpdate
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgUpdate))
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdUpdate = New System.Windows.Forms.Button()
    Me.lblInfo = New System.Windows.Forms.Label()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.lblChangeLog = New Slips7ream.LinkLabel()
    Me.pctUpdate = New System.Windows.Forms.PictureBox()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.TableLayoutPanel2.SuspendLayout()
    CType(Me.pctUpdate, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.AutoSize = True
    Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.TableLayoutPanel1.ColumnCount = 2
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.pctUpdate, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.lblTitle, 1, 0)
    Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 2
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(440, 151)
    Me.TableLayoutPanel1.TabIndex = 0
    '
    'TableLayoutPanel2
    '
    Me.TableLayoutPanel2.AutoSize = True
    Me.TableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.TableLayoutPanel2.ColumnCount = 3
    Me.TableLayoutPanel1.SetColumnSpan(Me.TableLayoutPanel2, 2)
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel2.Controls.Add(Me.cmdUpdate, 1, 1)
    Me.TableLayoutPanel2.Controls.Add(Me.cmdCancel, 2, 1)
    Me.TableLayoutPanel2.Controls.Add(Me.lblChangeLog, 0, 1)
    Me.TableLayoutPanel2.Controls.Add(Me.lblInfo, 0, 0)
    Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 54)
    Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
    Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
    Me.TableLayoutPanel2.RowCount = 2
    Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel2.Size = New System.Drawing.Size(440, 98)
    Me.TableLayoutPanel2.TabIndex = 0
    '
    'cmdUpdate
    '
    Me.cmdUpdate.AutoSize = True
    Me.cmdUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdUpdate.Location = New System.Drawing.Point(274, 67)
    Me.cmdUpdate.MinimumSize = New System.Drawing.Size(75, 25)
    Me.cmdUpdate.Name = "cmdUpdate"
    Me.cmdUpdate.Padding = New System.Windows.Forms.Padding(6, 3, 3, 3)
    Me.cmdUpdate.Size = New System.Drawing.Size(82, 28)
    Me.cmdUpdate.TabIndex = 0
    Me.cmdUpdate.Text = "Install Now"
    Me.cmdUpdate.UseVisualStyleBackColor = True
    '
    'lblInfo
    '
    Me.lblInfo.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblInfo.AutoSize = True
    Me.TableLayoutPanel2.SetColumnSpan(Me.lblInfo, 3)
    Me.lblInfo.Location = New System.Drawing.Point(80, 6)
    Me.lblInfo.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblInfo.Name = "lblInfo"
    Me.lblInfo.Size = New System.Drawing.Size(280, 52)
    Me.lblInfo.TabIndex = 0
    Me.lblInfo.Text = "A new version of %P is ready to be installed." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click ""Install Now"" to update to V" & _
    "ersion %1." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Download and installation should take less than a minute. "
    '
    'cmdCancel
    '
    Me.cmdCancel.AutoSize = True
    Me.cmdCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(362, 67)
    Me.cmdCancel.MinimumSize = New System.Drawing.Size(75, 25)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Padding = New System.Windows.Forms.Padding(3)
    Me.cmdCancel.Size = New System.Drawing.Size(75, 28)
    Me.cmdCancel.TabIndex = 1
    Me.cmdCancel.Text = "Later"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'lblChangeLog
    '
    Me.lblChangeLog.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblChangeLog.AutoSize = True
    Me.lblChangeLog.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblChangeLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline)
    Me.lblChangeLog.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblChangeLog.Location = New System.Drawing.Point(3, 74)
    Me.lblChangeLog.Name = "lblChangeLog"
    Me.lblChangeLog.Size = New System.Drawing.Size(124, 13)
    Me.lblChangeLog.TabIndex = 2
    Me.lblChangeLog.Text = "Read the Release Notes"
    '
    'pctUpdate
    '
    Me.pctUpdate.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pctUpdate.Image = Global.Slips7ream.My.Resources.Resources.update
    Me.pctUpdate.Location = New System.Drawing.Point(3, 3)
    Me.pctUpdate.Name = "pctUpdate"
    Me.pctUpdate.Size = New System.Drawing.Size(48, 48)
    Me.pctUpdate.TabIndex = 1
    Me.pctUpdate.TabStop = False
    '
    'lblTitle
    '
    Me.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblTitle.AutoSize = True
    Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTitle.Location = New System.Drawing.Point(159, 15)
    Me.lblTitle.Name = "lblTitle"
    Me.lblTitle.Size = New System.Drawing.Size(175, 24)
    Me.lblTitle.TabIndex = 2
    Me.lblTitle.Text = "%P v%1 is Available"
    '
    'dlgUpdate
    '
    Me.AcceptButton = Me.cmdUpdate
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(440, 151)
    Me.Controls.Add(Me.TableLayoutPanel1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "dlgUpdate"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Update for Slips7ream"
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.TableLayoutPanel1.PerformLayout()
    Me.TableLayoutPanel2.ResumeLayout(False)
    Me.TableLayoutPanel2.PerformLayout()
    CType(Me.pctUpdate, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblInfo As System.Windows.Forms.Label
  Friend WithEvents cmdUpdate As System.Windows.Forms.Button
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents pctUpdate As System.Windows.Forms.PictureBox
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents lblChangeLog As Slips7ream.LinkLabel

End Class
