<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdateProps
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
    Me.pnlUpdateData = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDisplayName = New System.Windows.Forms.Label()
    Me.txtDisplayName = New System.Windows.Forms.TextBox()
    Me.lblAppliesTo = New System.Windows.Forms.Label()
    Me.txtAppliesTo = New System.Windows.Forms.TextBox()
    Me.lblArchitecture = New System.Windows.Forms.Label()
    Me.txtArchitecture = New System.Windows.Forms.TextBox()
    Me.lblBuildDate = New System.Windows.Forms.Label()
    Me.txtBuildDate = New System.Windows.Forms.TextBox()
    Me.lblKBArticle = New System.Windows.Forms.Label()
    Me.lblKBLink = New Slips7ream.LinkLabel()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.ttLink = New Slips7ream.ToolTip(Me.components)
    Me.pnlUpdateData.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlUpdateData
    '
    Me.pnlUpdateData.ColumnCount = 2
    Me.pnlUpdateData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUpdateData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdateData.Controls.Add(Me.lblDisplayName, 0, 0)
    Me.pnlUpdateData.Controls.Add(Me.txtDisplayName, 1, 0)
    Me.pnlUpdateData.Controls.Add(Me.lblAppliesTo, 0, 1)
    Me.pnlUpdateData.Controls.Add(Me.txtAppliesTo, 1, 1)
    Me.pnlUpdateData.Controls.Add(Me.lblArchitecture, 0, 2)
    Me.pnlUpdateData.Controls.Add(Me.txtArchitecture, 1, 2)
    Me.pnlUpdateData.Controls.Add(Me.lblBuildDate, 0, 3)
    Me.pnlUpdateData.Controls.Add(Me.txtBuildDate, 1, 3)
    Me.pnlUpdateData.Controls.Add(Me.lblKBArticle, 0, 4)
    Me.pnlUpdateData.Controls.Add(Me.lblKBLink, 1, 4)
    Me.pnlUpdateData.Controls.Add(Me.cmdClose, 1, 5)
    Me.pnlUpdateData.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUpdateData.Location = New System.Drawing.Point(0, 0)
    Me.pnlUpdateData.Name = "pnlUpdateData"
    Me.pnlUpdateData.RowCount = 6
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.Size = New System.Drawing.Size(284, 156)
    Me.pnlUpdateData.TabIndex = 0
    '
    'lblDisplayName
    '
    Me.lblDisplayName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDisplayName.AutoSize = True
    Me.lblDisplayName.Location = New System.Drawing.Point(3, 6)
    Me.lblDisplayName.Name = "lblDisplayName"
    Me.lblDisplayName.Size = New System.Drawing.Size(75, 13)
    Me.lblDisplayName.TabIndex = 0
    Me.lblDisplayName.Text = "Display Name:"
    '
    'txtDisplayName
    '
    Me.txtDisplayName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDisplayName.Location = New System.Drawing.Point(99, 3)
    Me.txtDisplayName.Name = "txtDisplayName"
    Me.txtDisplayName.ReadOnly = True
    Me.txtDisplayName.Size = New System.Drawing.Size(182, 20)
    Me.txtDisplayName.TabIndex = 1
    '
    'lblAppliesTo
    '
    Me.lblAppliesTo.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAppliesTo.AutoSize = True
    Me.lblAppliesTo.Location = New System.Drawing.Point(3, 32)
    Me.lblAppliesTo.Name = "lblAppliesTo"
    Me.lblAppliesTo.Size = New System.Drawing.Size(56, 13)
    Me.lblAppliesTo.TabIndex = 2
    Me.lblAppliesTo.Text = "Applies to:"
    '
    'txtAppliesTo
    '
    Me.txtAppliesTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAppliesTo.Location = New System.Drawing.Point(99, 29)
    Me.txtAppliesTo.Name = "txtAppliesTo"
    Me.txtAppliesTo.ReadOnly = True
    Me.txtAppliesTo.Size = New System.Drawing.Size(182, 20)
    Me.txtAppliesTo.TabIndex = 3
    '
    'lblArchitecture
    '
    Me.lblArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblArchitecture.AutoSize = True
    Me.lblArchitecture.Location = New System.Drawing.Point(3, 58)
    Me.lblArchitecture.Name = "lblArchitecture"
    Me.lblArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblArchitecture.TabIndex = 4
    Me.lblArchitecture.Text = "Architecture:"
    '
    'txtArchitecture
    '
    Me.txtArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtArchitecture.Location = New System.Drawing.Point(99, 55)
    Me.txtArchitecture.Name = "txtArchitecture"
    Me.txtArchitecture.ReadOnly = True
    Me.txtArchitecture.Size = New System.Drawing.Size(182, 20)
    Me.txtArchitecture.TabIndex = 5
    '
    'lblBuildDate
    '
    Me.lblBuildDate.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblBuildDate.AutoSize = True
    Me.lblBuildDate.Location = New System.Drawing.Point(3, 84)
    Me.lblBuildDate.Name = "lblBuildDate"
    Me.lblBuildDate.Size = New System.Drawing.Size(59, 13)
    Me.lblBuildDate.TabIndex = 6
    Me.lblBuildDate.Text = "Build Date:"
    '
    'txtBuildDate
    '
    Me.txtBuildDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtBuildDate.Location = New System.Drawing.Point(99, 81)
    Me.txtBuildDate.Name = "txtBuildDate"
    Me.txtBuildDate.ReadOnly = True
    Me.txtBuildDate.Size = New System.Drawing.Size(182, 20)
    Me.txtBuildDate.TabIndex = 7
    '
    'lblKBArticle
    '
    Me.lblKBArticle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblKBArticle.AutoSize = True
    Me.lblKBArticle.Location = New System.Drawing.Point(3, 109)
    Me.lblKBArticle.Name = "lblKBArticle"
    Me.lblKBArticle.Size = New System.Drawing.Size(90, 13)
    Me.lblKBArticle.TabIndex = 8
    Me.lblKBArticle.Text = "Knowledge Base:"
    '
    'lblKBLink
    '
    Me.lblKBLink.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblKBLink.AutoSize = True
    Me.lblKBLink.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblKBLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblKBLink.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblKBLink.Location = New System.Drawing.Point(99, 109)
    Me.lblKBLink.Name = "lblKBLink"
    Me.lblKBLink.Size = New System.Drawing.Size(45, 13)
    Me.lblKBLink.TabIndex = 9
    Me.lblKBLink.TabStop = True
    Me.lblKBLink.Text = "Article 0"
    Me.lblKBLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(206, 130)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 10
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'frmUpdateProps
    '
    Me.AcceptButton = Me.cmdClose
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(284, 156)
    Me.Controls.Add(Me.pnlUpdateData)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmUpdateProps"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Windows Update Properties"
    Me.pnlUpdateData.ResumeLayout(False)
    Me.pnlUpdateData.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlUpdateData As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDisplayName As System.Windows.Forms.Label
  Friend WithEvents txtDisplayName As System.Windows.Forms.TextBox
  Friend WithEvents lblAppliesTo As System.Windows.Forms.Label
  Friend WithEvents txtAppliesTo As System.Windows.Forms.TextBox
  Friend WithEvents lblArchitecture As System.Windows.Forms.Label
  Friend WithEvents txtArchitecture As System.Windows.Forms.TextBox
  Friend WithEvents lblBuildDate As System.Windows.Forms.Label
  Friend WithEvents txtBuildDate As System.Windows.Forms.TextBox
  Friend WithEvents lblKBArticle As System.Windows.Forms.Label
  Friend WithEvents lblKBLink As LinkLabel
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents ttLink As Slips7ream.ToolTip
End Class
