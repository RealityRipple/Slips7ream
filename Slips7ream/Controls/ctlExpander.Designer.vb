<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Expander
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
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
    Me.pctExpander = New System.Windows.Forms.PictureBox()
    Me.pnlExpander = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExpander = New System.Windows.Forms.Label()
    CType(Me.pctExpander, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlExpander.SuspendLayout()
    Me.SuspendLayout()
    '
    'pctExpander
    '
    Me.pctExpander.BackColor = System.Drawing.Color.Transparent
    Me.pctExpander.Location = New System.Drawing.Point(0, 0)
    Me.pctExpander.Margin = New System.Windows.Forms.Padding(0)
    Me.pctExpander.Name = "pctExpander"
    Me.pctExpander.Size = New System.Drawing.Size(19, 19)
    Me.pctExpander.TabIndex = 0
    Me.pctExpander.TabStop = False
    '
    'pnlExpander
    '
    Me.pnlExpander.AutoSize = True
    Me.pnlExpander.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExpander.BackColor = System.Drawing.Color.Transparent
    Me.pnlExpander.ColumnCount = 2
    Me.pnlExpander.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExpander.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExpander.Controls.Add(Me.pctExpander, 0, 0)
    Me.pnlExpander.Controls.Add(Me.lblExpander, 1, 0)
    Me.pnlExpander.Location = New System.Drawing.Point(0, 0)
    Me.pnlExpander.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExpander.Name = "pnlExpander"
    Me.pnlExpander.RowCount = 1
    Me.pnlExpander.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExpander.Size = New System.Drawing.Size(71, 19)
    Me.pnlExpander.TabIndex = 1
    '
    'lblExpander
    '
    Me.lblExpander.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExpander.AutoSize = True
    Me.lblExpander.BackColor = System.Drawing.Color.Transparent
    Me.lblExpander.Location = New System.Drawing.Point(19, 3)
    Me.lblExpander.Margin = New System.Windows.Forms.Padding(0)
    Me.lblExpander.Name = "lblExpander"
    Me.lblExpander.Size = New System.Drawing.Size(52, 13)
    Me.lblExpander.TabIndex = 1
    Me.lblExpander.Text = "Expander"
    '
    'ctlExpander
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.Controls.Add(Me.pnlExpander)
    Me.Name = "ctlExpander"
    Me.Size = New System.Drawing.Size(71, 19)
    CType(Me.pctExpander, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlExpander.ResumeLayout(False)
    Me.pnlExpander.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pctExpander As System.Windows.Forms.PictureBox
  Friend WithEvents pnlExpander As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblExpander As System.Windows.Forms.Label

End Class
