﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
    Me.lblIdentity = New System.Windows.Forms.Label()
    Me.txtIdentity = New System.Windows.Forms.TextBox()
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
    Me.lblName = New System.Windows.Forms.Label()
    Me.txtName = New System.Windows.Forms.TextBox()
    Me.lblVersion = New System.Windows.Forms.Label()
    Me.txtVersion = New System.Windows.Forms.TextBox()
    Me.ttLink = New Slips7ream.ToolTip(Me.components)
    Me.helpS7M = New System.Windows.Forms.HelpProvider()
    Me.pnlUpdateData.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlUpdateData
    '
    Me.pnlUpdateData.AutoSize = True
    Me.pnlUpdateData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUpdateData.ColumnCount = 2
    Me.pnlUpdateData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUpdateData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdateData.Controls.Add(Me.lblIdentity, 0, 4)
    Me.pnlUpdateData.Controls.Add(Me.txtIdentity, 1, 4)
    Me.pnlUpdateData.Controls.Add(Me.lblDisplayName, 0, 2)
    Me.pnlUpdateData.Controls.Add(Me.txtDisplayName, 1, 2)
    Me.pnlUpdateData.Controls.Add(Me.lblAppliesTo, 0, 6)
    Me.pnlUpdateData.Controls.Add(Me.txtAppliesTo, 1, 6)
    Me.pnlUpdateData.Controls.Add(Me.lblArchitecture, 0, 8)
    Me.pnlUpdateData.Controls.Add(Me.txtArchitecture, 1, 8)
    Me.pnlUpdateData.Controls.Add(Me.lblBuildDate, 0, 10)
    Me.pnlUpdateData.Controls.Add(Me.txtBuildDate, 1, 10)
    Me.pnlUpdateData.Controls.Add(Me.lblKBArticle, 0, 14)
    Me.pnlUpdateData.Controls.Add(Me.lblKBLink, 1, 14)
    Me.pnlUpdateData.Controls.Add(Me.cmdClose, 1, 16)
    Me.pnlUpdateData.Controls.Add(Me.lblName, 0, 0)
    Me.pnlUpdateData.Controls.Add(Me.txtName, 1, 0)
    Me.pnlUpdateData.Controls.Add(Me.lblVersion, 0, 12)
    Me.pnlUpdateData.Controls.Add(Me.txtVersion, 1, 12)
    Me.pnlUpdateData.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUpdateData.Location = New System.Drawing.Point(0, 0)
    Me.pnlUpdateData.Name = "pnlUpdateData"
    Me.pnlUpdateData.RowCount = 17
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlUpdateData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdateData.Size = New System.Drawing.Size(334, 241)
    Me.pnlUpdateData.TabIndex = 0
    '
    'lblIdentity
    '
    Me.lblIdentity.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblIdentity.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblIdentity, "The Identity string contains the following data, delimited by tildes (~):" & vbNewLine &
        "  Name: The Identity Name is the name of the update. It's how SLIPS7REAM tells one update from another, and how it knows to compare different versions of the same update." & vbNewLine &
        "  Token: The Token value is almost always 31bf3856ad364e35." & vbNewLine &
        "  Architecture: (Usually) redundant with the Architecture value below. If empty, then this update is ""AnyCPU"" or Architecture-neutral." & vbNewLine &
        "  Language: The Language this update applies to. If empty, then this update is Language-neutral." & vbNewLine &
        "  Version: (Usually) redundant with the Version value below.")
    Me.lblIdentity.Location = New System.Drawing.Point(3, 58)
    Me.lblIdentity.Name = "lblIdentity"
    Me.helpS7M.SetShowHelp(Me.lblIdentity, True)
    Me.lblIdentity.Size = New System.Drawing.Size(44, 13)
    Me.lblIdentity.TabIndex = 4
    Me.lblIdentity.Text = "Identity:"
    '
    'txtIdentity
    '
    Me.txtIdentity.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtIdentity, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtIdentity, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtIdentity, "The Identity string contains the following data, delimited by tildes (~):" & vbNewLine &
        "  Name: The Identity Name is the name of the update. It's how SLIPS7REAM tells one update from another, and how it knows to compare different versions of the same update." & vbNewLine &
        "  Token: The Token value is almost always 31bf3856ad364e35." & vbNewLine &
        "  Architecture: (Usually) redundant with the Architecture value below. If empty, then this update is ""AnyCPU"" or Architecture-neutral." & vbNewLine &
        "  Language: The Language this update applies to. If empty, then this update is Language-neutral." & vbNewLine &
        "  Version: (Usually) redundant with the Version value below.")
    Me.txtIdentity.Location = New System.Drawing.Point(99, 55)
    Me.txtIdentity.Name = "txtIdentity"
    Me.txtIdentity.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtIdentity, True)
    Me.txtIdentity.Size = New System.Drawing.Size(232, 20)
    Me.txtIdentity.TabIndex = 5
    '
    'lblDisplayName
    '
    Me.lblDisplayName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDisplayName.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblDisplayName, "Depends on the type of file, but is generally a common name of the update, often " & _
        "with a version number." & vbNewLine &
        "  For MSU Files, this is the * part of the ""*pkgProperties.txt"" file inside the update." & vbNewLine &
        "  For CAB files, this is the ""displayName"" value in the ""update.mum"" XML file." & vbNewLine &
        "  For LIP files, MSI and LP.CAB Language files, this is the ""identifier"" value in the ""update.mum"" XML file." & vbNewLine &
        "  EXE parsing uses either CAB or Language rules depending on what it is.")
    Me.lblDisplayName.Location = New System.Drawing.Point(3, 32)
    Me.lblDisplayName.Name = "lblDisplayName"
    Me.helpS7M.SetShowHelp(Me.lblDisplayName, True)
    Me.lblDisplayName.Size = New System.Drawing.Size(75, 13)
    Me.lblDisplayName.TabIndex = 2
    Me.lblDisplayName.Text = "Display Name:"
    '
    'txtDisplayName
    '
    Me.txtDisplayName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtDisplayName, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtDisplayName, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtDisplayName, "Depends on the type of file, but is generally a common name of the update, often " & _
        "with a version number." & vbNewLine &
        "  For MSU Files, this is the * part of the ""*pkgProperties.txt"" file inside the update." & vbNewLine &
        "  For CAB files, this is the ""displayName"" value in the ""update.mum"" XML file." & vbNewLine &
        "  For LIP files, MSI and LP.CAB Language files, this is the ""identifier"" value in the ""update.mum"" XML file." & vbNewLine &
        "  EXE parsing uses either CAB or Language rules depending on what it is.")
    Me.txtDisplayName.Location = New System.Drawing.Point(99, 29)
    Me.txtDisplayName.Name = "txtDisplayName"
    Me.txtDisplayName.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtDisplayName, True)
    Me.txtDisplayName.Size = New System.Drawing.Size(232, 20)
    Me.txtDisplayName.TabIndex = 3
    '
    'lblAppliesTo
    '
    Me.lblAppliesTo.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAppliesTo.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblAppliesTo, "The Operating System or Program which the Update applies to.")
    Me.lblAppliesTo.Location = New System.Drawing.Point(3, 84)
    Me.lblAppliesTo.Name = "lblAppliesTo"
    Me.helpS7M.SetShowHelp(Me.lblAppliesTo, True)
    Me.lblAppliesTo.Size = New System.Drawing.Size(56, 13)
    Me.lblAppliesTo.TabIndex = 6
    Me.lblAppliesTo.Text = "Applies to:"
    '
    'txtAppliesTo
    '
    Me.txtAppliesTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtAppliesTo, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtAppliesTo, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtAppliesTo, "The Operating System or Program which the Update applies to.")
    Me.txtAppliesTo.Location = New System.Drawing.Point(99, 81)
    Me.txtAppliesTo.Name = "txtAppliesTo"
    Me.txtAppliesTo.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtAppliesTo, True)
    Me.txtAppliesTo.Size = New System.Drawing.Size(232, 20)
    Me.txtAppliesTo.TabIndex = 7
    '
    'lblArchitecture
    '
    Me.lblArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblArchitecture.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblArchitecture, "The OS Architecture this update is designed for. SLIPS7REAM uses this value to de" & _
        "termine if it should be integrated into an Image Package.")
    Me.lblArchitecture.Location = New System.Drawing.Point(3, 110)
    Me.lblArchitecture.Name = "lblArchitecture"
    Me.helpS7M.SetShowHelp(Me.lblArchitecture, True)
    Me.lblArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblArchitecture.TabIndex = 8
    Me.lblArchitecture.Text = "Architecture:"
    '
    'txtArchitecture
    '
    Me.txtArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtArchitecture, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtArchitecture, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtArchitecture, "The OS Architecture this update is designed for. SLIPS7REAM uses this value to de" & _
        "termine if it should be integrated into an Image Package.")
    Me.txtArchitecture.Location = New System.Drawing.Point(99, 107)
    Me.txtArchitecture.Name = "txtArchitecture"
    Me.txtArchitecture.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtArchitecture, True)
    Me.txtArchitecture.Size = New System.Drawing.Size(232, 20)
    Me.txtArchitecture.TabIndex = 9
    '
    'lblBuildDate
    '
    Me.lblBuildDate.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblBuildDate.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblBuildDate, "This is usually in the format of YEAR/MM/DD.")
    Me.lblBuildDate.Location = New System.Drawing.Point(3, 136)
    Me.lblBuildDate.Name = "lblBuildDate"
    Me.helpS7M.SetShowHelp(Me.lblBuildDate, True)
    Me.lblBuildDate.Size = New System.Drawing.Size(59, 13)
    Me.lblBuildDate.TabIndex = 10
    Me.lblBuildDate.Text = "Build Date:"
    '
    'txtBuildDate
    '
    Me.txtBuildDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtBuildDate, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtBuildDate, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtBuildDate, "This is usually in the format of YEAR/MM/DD.")
    Me.txtBuildDate.Location = New System.Drawing.Point(99, 133)
    Me.txtBuildDate.Name = "txtBuildDate"
    Me.txtBuildDate.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtBuildDate, True)
    Me.txtBuildDate.Size = New System.Drawing.Size(232, 20)
    Me.txtBuildDate.TabIndex = 11
    '
    'lblKBArticle
    '
    Me.lblKBArticle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblKBArticle.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblKBArticle, "You can click the KB Article link to view details about this update, if this upda" & _
        "te has a KB Article Number, or Details if it's a Support Link.")
    Me.lblKBArticle.Location = New System.Drawing.Point(3, 188)
    Me.lblKBArticle.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblKBArticle.Name = "lblKBArticle"
    Me.helpS7M.SetShowHelp(Me.lblKBArticle, True)
    Me.lblKBArticle.Size = New System.Drawing.Size(90, 13)
    Me.lblKBArticle.TabIndex = 14
    Me.lblKBArticle.Text = "Knowledge Base:"
    '
    'lblKBLink
    '
    Me.lblKBLink.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblKBLink.AutoSize = True
    Me.lblKBLink.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblKBLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblKBLink.ForeColor = System.Drawing.Color.MediumBlue
    Me.helpS7M.SetHelpKeyword(Me.lblKBLink, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.lblKBLink, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.lblKBLink, "You can click the KB Article link to view details about this update, if this upda" & _
        "te has a KB Article Number, or Details if it's a Support Link.")
    Me.lblKBLink.Location = New System.Drawing.Point(99, 188)
    Me.lblKBLink.Name = "lblKBLink"
    Me.helpS7M.SetShowHelp(Me.lblKBLink, True)
    Me.lblKBLink.Size = New System.Drawing.Size(45, 13)
    Me.lblKBLink.TabIndex = 15
    Me.lblKBLink.TabStop = True
    Me.lblKBLink.Text = "Article 0"
    Me.lblKBLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdClose.AutoSize = True
    Me.cmdClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdClose, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdClose, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.cmdClose, "Close the Windows Update Properties window.")
    Me.cmdClose.Location = New System.Drawing.Point(251, 213)
    Me.cmdClose.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Padding = New System.Windows.Forms.Padding(1)
    Me.helpS7M.SetShowHelp(Me.cmdClose, True)
    Me.cmdClose.Size = New System.Drawing.Size(80, 25)
    Me.cmdClose.TabIndex = 16
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'lblName
    '
    Me.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblName.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblName, "The name SLIPS7REAM uses in the Update List.")
    Me.lblName.Location = New System.Drawing.Point(3, 6)
    Me.lblName.Name = "lblName"
    Me.helpS7M.SetShowHelp(Me.lblName, True)
    Me.lblName.Size = New System.Drawing.Size(38, 13)
    Me.lblName.TabIndex = 0
    Me.lblName.Text = "Name:"
    '
    'txtName
    '
    Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtName, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtName, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtName, "The name SLIPS7REAM uses in the Update List.")
    Me.txtName.Location = New System.Drawing.Point(99, 3)
    Me.txtName.Name = "txtName"
    Me.txtName.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtName, True)
    Me.txtName.Size = New System.Drawing.Size(232, 20)
    Me.txtName.TabIndex = 1
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblVersion.AutoSize = True
    Me.helpS7M.SetHelpString(Me.lblVersion, "The Version of most Windows Updates is 6.1.Update_Major.Update_Minor to identify " & _
        "them as updates for Windows 7 (or other numbers for other versions).")
    Me.lblVersion.Location = New System.Drawing.Point(3, 162)
    Me.lblVersion.Name = "lblVersion"
    Me.helpS7M.SetShowHelp(Me.lblVersion, True)
    Me.lblVersion.Size = New System.Drawing.Size(45, 13)
    Me.lblVersion.TabIndex = 12
    Me.lblVersion.Text = "Version:"
    '
    'txtVersion
    '
    Me.txtVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtVersion, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtVersion, System.Windows.Forms.HelpNavigator.Topic)
    Me.helpS7M.SetHelpString(Me.txtVersion, "The Version of most Windows Updates is 6.1.Update_Major.Update_Minor to identify " & _
        "them as updates for Windows 7 (or other numbers for other versions).")
    Me.txtVersion.Location = New System.Drawing.Point(99, 159)
    Me.txtVersion.Name = "txtVersion"
    Me.txtVersion.ReadOnly = True
    Me.helpS7M.SetShowHelp(Me.txtVersion, True)
    Me.txtVersion.Size = New System.Drawing.Size(232, 20)
    Me.txtVersion.TabIndex = 13
    '
    'helpS7M
    '
    Me.helpS7M.HelpNamespace = "S7M.chm"
    '
    'frmUpdateProps
    '
    Me.AcceptButton = Me.cmdClose
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(334, 241)
    Me.Controls.Add(Me.pnlUpdateData)
    Me.HelpButton = True
    Me.helpS7M.SetHelpKeyword(Me, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.2_Update_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.MinimumSize = New System.Drawing.Size(320, 275)
    Me.Name = "frmUpdateProps"
    Me.helpS7M.SetShowHelp(Me, True)
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Windows Update Properties"
    Me.pnlUpdateData.ResumeLayout(False)
    Me.pnlUpdateData.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

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
  Friend WithEvents lblIdentity As System.Windows.Forms.Label
  Friend WithEvents txtIdentity As System.Windows.Forms.TextBox
  Friend WithEvents lblName As System.Windows.Forms.Label
  Friend WithEvents txtName As System.Windows.Forms.TextBox
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents txtVersion As System.Windows.Forms.TextBox
  Friend WithEvents helpS7M As System.Windows.Forms.HelpProvider
End Class
