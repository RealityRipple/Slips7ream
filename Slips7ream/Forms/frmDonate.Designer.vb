<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDonate
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDonate))
    Me.pnlDonate = New System.Windows.Forms.TableLayoutPanel()
    Me.pctStar = New System.Windows.Forms.PictureBox()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdDonate = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.lblPlea = New System.Windows.Forms.Label()
    Me.ttButtons = New System.Windows.Forms.ToolTip(Me.components)
    Me.line = New Slips7ream.LineBreak()
    Me.pnlDonate.SuspendLayout()
    CType(Me.pctStar, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlDonate
    '
    Me.pnlDonate.AutoSize = True
    Me.pnlDonate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDonate.ColumnCount = 2
    Me.pnlDonate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDonate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDonate.Controls.Add(Me.pctStar, 0, 0)
    Me.pnlDonate.Controls.Add(Me.pnlButtons, 1, 3)
    Me.pnlDonate.Controls.Add(Me.lblTitle, 1, 0)
    Me.pnlDonate.Controls.Add(Me.lblPlea, 1, 2)
    Me.pnlDonate.Controls.Add(Me.line, 1, 1)
    Me.pnlDonate.Location = New System.Drawing.Point(0, 0)
    Me.pnlDonate.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDonate.Name = "pnlDonate"
    Me.pnlDonate.RowCount = 4
    Me.pnlDonate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDonate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlDonate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDonate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDonate.Size = New System.Drawing.Size(638, 255)
    Me.pnlDonate.TabIndex = 0
    '
    'pctStar
    '
    Me.pctStar.Image = Global.Slips7ream.My.Resources.Resources.star
    Me.pctStar.Location = New System.Drawing.Point(0, 0)
    Me.pctStar.Margin = New System.Windows.Forms.Padding(0)
    Me.pctStar.Name = "pctStar"
    Me.pnlDonate.SetRowSpan(Me.pctStar, 4)
    Me.pctStar.Size = New System.Drawing.Size(150, 255)
    Me.pctStar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
    Me.pctStar.TabIndex = 0
    Me.pctStar.TabStop = False
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlButtons.Controls.Add(Me.cmdDonate, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(326, 220)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlButtons.Size = New System.Drawing.Size(312, 35)
    Me.pnlButtons.TabIndex = 0
    '
    'cmdDonate
    '
    Me.cmdDonate.AutoSize = True
    Me.cmdDonate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDonate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdDonate.Location = New System.Drawing.Point(3, 3)
    Me.cmdDonate.Name = "cmdDonate"
    Me.cmdDonate.Size = New System.Drawing.Size(164, 29)
    Me.cmdDonate.TabIndex = 0
    Me.cmdDonate.Text = "Make a Donation!"
    Me.ttButtons.SetToolTip(Me.cmdDonate, "Contribute $5 or $10 to RealityRipple Software to keep SLIPS7REAM alive!")
    Me.cmdDonate.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.AutoSize = True
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdClose.Location = New System.Drawing.Point(173, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(136, 29)
    Me.cmdClose.TabIndex = 2
    Me.cmdClose.Text = "No Thank You"
    Me.ttButtons.SetToolTip(Me.cmdClose, "Close this nag screen and see it again in a little over half a year.")
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'lblTitle
    '
    Me.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblTitle.AutoSize = True
    Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTitle.Location = New System.Drawing.Point(247, 3)
    Me.lblTitle.Margin = New System.Windows.Forms.Padding(3, 3, 3, 6)
    Me.lblTitle.Name = "lblTitle"
    Me.lblTitle.Size = New System.Drawing.Size(294, 31)
    Me.lblTitle.TabIndex = 1
    Me.lblTitle.Text = "Support SLIPS7REAM!"
    '
    'lblPlea
    '
    Me.lblPlea.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPlea.AutoSize = True
    Me.lblPlea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPlea.Location = New System.Drawing.Point(153, 60)
    Me.lblPlea.Name = "lblPlea"
    Me.lblPlea.Size = New System.Drawing.Size(475, 160)
    Me.lblPlea.TabIndex = 2
    Me.lblPlea.Text = resources.GetString("lblPlea.Text")
    Me.lblPlea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'line
    '
    Me.line.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.line.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.line.CausesValidation = False
    Me.line.Location = New System.Drawing.Point(153, 48)
    Me.line.Name = "line"
    Me.line.Padding = New System.Windows.Forms.Padding(1)
    Me.line.Size = New System.Drawing.Size(482, 4)
    Me.line.TabIndex = 3
    Me.line.TabStop = False
    '
    'frmDonate
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.ClientSize = New System.Drawing.Size(642, 258)
    Me.Controls.Add(Me.pnlDonate)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.Name = "frmDonate"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Please Support SLIPS7REAM!"
    Me.TopMost = True
    Me.pnlDonate.ResumeLayout(False)
    Me.pnlDonate.PerformLayout()
    CType(Me.pctStar, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlDonate As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctStar As System.Windows.Forms.PictureBox
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdDonate As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents lblPlea As System.Windows.Forms.Label
  Friend WithEvents line As Slips7ream.LineBreak
  Friend WithEvents ttButtons As System.Windows.Forms.ToolTip
End Class
