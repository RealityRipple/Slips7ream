<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDriverProps
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
    Me.pnlDriver = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlDriverData = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDriverStorePath = New System.Windows.Forms.Label()
    Me.txtDriverStorePath = New System.Windows.Forms.TextBox()
    Me.lblPublishedName = New System.Windows.Forms.Label()
    Me.txtPublishedName = New System.Windows.Forms.TextBox()
    Me.lblInbox = New System.Windows.Forms.Label()
    Me.txtInbox = New System.Windows.Forms.TextBox()
    Me.lblProviderName = New System.Windows.Forms.Label()
    Me.lblOriginalFileName = New System.Windows.Forms.Label()
    Me.txtOriginalFileName = New System.Windows.Forms.TextBox()
    Me.txtProviderName = New System.Windows.Forms.TextBox()
    Me.lblDate = New System.Windows.Forms.Label()
    Me.txtDate = New System.Windows.Forms.TextBox()
    Me.lblVersion = New System.Windows.Forms.Label()
    Me.txtVersion = New System.Windows.Forms.TextBox()
    Me.lblBootCritical = New System.Windows.Forms.Label()
    Me.txtBootCritical = New System.Windows.Forms.TextBox()
    Me.grpClass = New System.Windows.Forms.GroupBox()
    Me.pnlClass = New System.Windows.Forms.TableLayoutPanel()
    Me.lblClassName = New System.Windows.Forms.Label()
    Me.lblClassDescription = New System.Windows.Forms.Label()
    Me.lblClassGUID = New System.Windows.Forms.Label()
    Me.txtClassName = New System.Windows.Forms.TextBox()
    Me.txtClassDescription = New System.Windows.Forms.TextBox()
    Me.txtClassGUID = New System.Windows.Forms.TextBox()
    Me.pctClassIcon = New System.Windows.Forms.PictureBox()
    Me.pctDriverIcon = New System.Windows.Forms.PictureBox()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.pnlDriverInfo = New System.Windows.Forms.TableLayoutPanel()
    Me.lblArchitecture = New System.Windows.Forms.Label()
    Me.cmbArchitecture = New System.Windows.Forms.ComboBox()
    Me.lblHardware = New System.Windows.Forms.Label()
    Me.cmbHardware = New System.Windows.Forms.ComboBox()
    Me.grpHardware = New System.Windows.Forms.GroupBox()
    Me.pnlHardware = New System.Windows.Forms.TableLayoutPanel()
    Me.lblHWID = New System.Windows.Forms.Label()
    Me.lblHWCompatibleIDs = New System.Windows.Forms.Label()
    Me.lblHWExcludeIDs = New System.Windows.Forms.Label()
    Me.lstHWCompatibleIDs = New System.Windows.Forms.ListBox()
    Me.lstHWExcludeIDs = New System.Windows.Forms.ListBox()
    Me.lstHWIDs = New System.Windows.Forms.ListBox()
    Me.lblHWServiceName = New System.Windows.Forms.Label()
    Me.txtHWServiceName = New System.Windows.Forms.TextBox()
    Me.lblHWDescription = New System.Windows.Forms.Label()
    Me.txtHWDescription = New System.Windows.Forms.TextBox()
    Me.lblHWManufacturer = New System.Windows.Forms.Label()
    Me.lblHWArchitecture = New System.Windows.Forms.Label()
    Me.txtHWManufacturer = New System.Windows.Forms.TextBox()
    Me.txtHWArchitecture = New System.Windows.Forms.TextBox()
    Me.pnlDriver.SuspendLayout()
    Me.pnlDriverData.SuspendLayout()
    Me.grpClass.SuspendLayout()
    Me.pnlClass.SuspendLayout()
    CType(Me.pctClassIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pctDriverIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlDriverInfo.SuspendLayout()
    Me.grpHardware.SuspendLayout()
    Me.pnlHardware.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlDriver
    '
    Me.pnlDriver.ColumnCount = 2
    Me.pnlDriver.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlDriver.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlDriver.Controls.Add(Me.pnlDriverData, 0, 0)
    Me.pnlDriver.Controls.Add(Me.cmdClose, 0, 1)
    Me.pnlDriver.Controls.Add(Me.pnlDriverInfo, 1, 0)
    Me.pnlDriver.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDriver.Location = New System.Drawing.Point(0, 0)
    Me.pnlDriver.Name = "pnlDriver"
    Me.pnlDriver.RowCount = 2
    Me.pnlDriver.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDriver.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriver.Size = New System.Drawing.Size(534, 341)
    Me.pnlDriver.TabIndex = 0
    '
    'pnlDriverData
    '
    Me.pnlDriverData.AutoSize = True
    Me.pnlDriverData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDriverData.ColumnCount = 3
    Me.pnlDriverData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDriverData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDriverData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDriverData.Controls.Add(Me.lblDriverStorePath, 0, 2)
    Me.pnlDriverData.Controls.Add(Me.txtDriverStorePath, 2, 2)
    Me.pnlDriverData.Controls.Add(Me.lblPublishedName, 0, 1)
    Me.pnlDriverData.Controls.Add(Me.txtPublishedName, 2, 1)
    Me.pnlDriverData.Controls.Add(Me.lblInbox, 0, 3)
    Me.pnlDriverData.Controls.Add(Me.txtInbox, 2, 3)
    Me.pnlDriverData.Controls.Add(Me.lblProviderName, 0, 4)
    Me.pnlDriverData.Controls.Add(Me.lblOriginalFileName, 0, 0)
    Me.pnlDriverData.Controls.Add(Me.txtOriginalFileName, 2, 0)
    Me.pnlDriverData.Controls.Add(Me.txtProviderName, 2, 4)
    Me.pnlDriverData.Controls.Add(Me.lblDate, 0, 5)
    Me.pnlDriverData.Controls.Add(Me.txtDate, 2, 5)
    Me.pnlDriverData.Controls.Add(Me.lblVersion, 0, 6)
    Me.pnlDriverData.Controls.Add(Me.txtVersion, 2, 6)
    Me.pnlDriverData.Controls.Add(Me.lblBootCritical, 0, 7)
    Me.pnlDriverData.Controls.Add(Me.txtBootCritical, 2, 7)
    Me.pnlDriverData.Controls.Add(Me.grpClass, 0, 8)
    Me.pnlDriverData.Controls.Add(Me.pctDriverIcon, 1, 0)
    Me.pnlDriverData.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDriverData.Location = New System.Drawing.Point(0, 0)
    Me.pnlDriverData.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDriverData.Name = "pnlDriverData"
    Me.pnlDriverData.RowCount = 9
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDriverData.Size = New System.Drawing.Size(267, 310)
    Me.pnlDriverData.TabIndex = 0
    '
    'lblDriverStorePath
    '
    Me.lblDriverStorePath.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDriverStorePath.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblDriverStorePath, 2)
    Me.lblDriverStorePath.Location = New System.Drawing.Point(3, 58)
    Me.lblDriverStorePath.Name = "lblDriverStorePath"
    Me.lblDriverStorePath.Size = New System.Drawing.Size(91, 13)
    Me.lblDriverStorePath.TabIndex = 4
    Me.lblDriverStorePath.Text = "Driver Store Path:"
    '
    'txtDriverStorePath
    '
    Me.txtDriverStorePath.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDriverStorePath.Location = New System.Drawing.Point(101, 55)
    Me.txtDriverStorePath.Name = "txtDriverStorePath"
    Me.txtDriverStorePath.ReadOnly = True
    Me.txtDriverStorePath.Size = New System.Drawing.Size(163, 20)
    Me.txtDriverStorePath.TabIndex = 5
    '
    'lblPublishedName
    '
    Me.lblPublishedName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPublishedName.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblPublishedName, 2)
    Me.lblPublishedName.Location = New System.Drawing.Point(3, 32)
    Me.lblPublishedName.Name = "lblPublishedName"
    Me.lblPublishedName.Size = New System.Drawing.Size(87, 13)
    Me.lblPublishedName.TabIndex = 2
    Me.lblPublishedName.Text = "Published Name:"
    '
    'txtPublishedName
    '
    Me.txtPublishedName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPublishedName.Location = New System.Drawing.Point(101, 29)
    Me.txtPublishedName.Name = "txtPublishedName"
    Me.txtPublishedName.ReadOnly = True
    Me.txtPublishedName.Size = New System.Drawing.Size(163, 20)
    Me.txtPublishedName.TabIndex = 3
    '
    'lblInbox
    '
    Me.lblInbox.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInbox.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblInbox, 2)
    Me.lblInbox.Location = New System.Drawing.Point(3, 84)
    Me.lblInbox.Name = "lblInbox"
    Me.lblInbox.Size = New System.Drawing.Size(36, 13)
    Me.lblInbox.TabIndex = 6
    Me.lblInbox.Text = "Inbox:"
    '
    'txtInbox
    '
    Me.txtInbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInbox.Location = New System.Drawing.Point(101, 81)
    Me.txtInbox.Name = "txtInbox"
    Me.txtInbox.ReadOnly = True
    Me.txtInbox.Size = New System.Drawing.Size(163, 20)
    Me.txtInbox.TabIndex = 7
    '
    'lblProviderName
    '
    Me.lblProviderName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProviderName.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblProviderName, 2)
    Me.lblProviderName.Location = New System.Drawing.Point(3, 110)
    Me.lblProviderName.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblProviderName.Name = "lblProviderName"
    Me.lblProviderName.Size = New System.Drawing.Size(80, 13)
    Me.lblProviderName.TabIndex = 8
    Me.lblProviderName.Text = "Provider Name:"
    '
    'lblOriginalFileName
    '
    Me.lblOriginalFileName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOriginalFileName.AutoSize = True
    Me.lblOriginalFileName.Location = New System.Drawing.Point(3, 6)
    Me.lblOriginalFileName.Name = "lblOriginalFileName"
    Me.lblOriginalFileName.Size = New System.Drawing.Size(76, 13)
    Me.lblOriginalFileName.TabIndex = 0
    Me.lblOriginalFileName.Text = "Original Name:"
    '
    'txtOriginalFileName
    '
    Me.txtOriginalFileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOriginalFileName.Location = New System.Drawing.Point(101, 3)
    Me.txtOriginalFileName.Name = "txtOriginalFileName"
    Me.txtOriginalFileName.ReadOnly = True
    Me.txtOriginalFileName.Size = New System.Drawing.Size(163, 20)
    Me.txtOriginalFileName.TabIndex = 1
    '
    'txtProviderName
    '
    Me.txtProviderName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProviderName.Location = New System.Drawing.Point(101, 107)
    Me.txtProviderName.Name = "txtProviderName"
    Me.txtProviderName.ReadOnly = True
    Me.txtProviderName.Size = New System.Drawing.Size(163, 20)
    Me.txtProviderName.TabIndex = 9
    '
    'lblDate
    '
    Me.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDate.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblDate, 2)
    Me.lblDate.Location = New System.Drawing.Point(3, 136)
    Me.lblDate.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblDate.Name = "lblDate"
    Me.lblDate.Size = New System.Drawing.Size(33, 13)
    Me.lblDate.TabIndex = 10
    Me.lblDate.Text = "Date:"
    '
    'txtDate
    '
    Me.txtDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDate.Location = New System.Drawing.Point(101, 133)
    Me.txtDate.Name = "txtDate"
    Me.txtDate.ReadOnly = True
    Me.txtDate.Size = New System.Drawing.Size(163, 20)
    Me.txtDate.TabIndex = 11
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblVersion.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblVersion, 2)
    Me.lblVersion.Location = New System.Drawing.Point(3, 162)
    Me.lblVersion.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblVersion.Name = "lblVersion"
    Me.lblVersion.Size = New System.Drawing.Size(45, 13)
    Me.lblVersion.TabIndex = 12
    Me.lblVersion.Text = "Version:"
    '
    'txtVersion
    '
    Me.txtVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVersion.Location = New System.Drawing.Point(101, 159)
    Me.txtVersion.Name = "txtVersion"
    Me.txtVersion.ReadOnly = True
    Me.txtVersion.Size = New System.Drawing.Size(163, 20)
    Me.txtVersion.TabIndex = 13
    '
    'lblBootCritical
    '
    Me.lblBootCritical.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblBootCritical.AutoSize = True
    Me.pnlDriverData.SetColumnSpan(Me.lblBootCritical, 2)
    Me.lblBootCritical.Location = New System.Drawing.Point(3, 188)
    Me.lblBootCritical.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblBootCritical.Name = "lblBootCritical"
    Me.lblBootCritical.Size = New System.Drawing.Size(66, 13)
    Me.lblBootCritical.TabIndex = 14
    Me.lblBootCritical.Text = "Boot Critical:"
    '
    'txtBootCritical
    '
    Me.txtBootCritical.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtBootCritical.Location = New System.Drawing.Point(101, 185)
    Me.txtBootCritical.Name = "txtBootCritical"
    Me.txtBootCritical.ReadOnly = True
    Me.txtBootCritical.Size = New System.Drawing.Size(163, 20)
    Me.txtBootCritical.TabIndex = 15
    '
    'grpClass
    '
    Me.pnlDriverData.SetColumnSpan(Me.grpClass, 3)
    Me.grpClass.Controls.Add(Me.pnlClass)
    Me.grpClass.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpClass.Location = New System.Drawing.Point(3, 211)
    Me.grpClass.Name = "grpClass"
    Me.grpClass.Size = New System.Drawing.Size(261, 96)
    Me.grpClass.TabIndex = 16
    Me.grpClass.TabStop = False
    Me.grpClass.Text = "Driver Class"
    '
    'pnlClass
    '
    Me.pnlClass.ColumnCount = 3
    Me.pnlClass.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlClass.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlClass.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlClass.Controls.Add(Me.lblClassName, 0, 0)
    Me.pnlClass.Controls.Add(Me.lblClassDescription, 0, 1)
    Me.pnlClass.Controls.Add(Me.lblClassGUID, 0, 2)
    Me.pnlClass.Controls.Add(Me.txtClassName, 2, 0)
    Me.pnlClass.Controls.Add(Me.txtClassDescription, 2, 1)
    Me.pnlClass.Controls.Add(Me.txtClassGUID, 2, 2)
    Me.pnlClass.Controls.Add(Me.pctClassIcon, 1, 0)
    Me.pnlClass.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlClass.Location = New System.Drawing.Point(3, 16)
    Me.pnlClass.Name = "pnlClass"
    Me.pnlClass.RowCount = 3
    Me.pnlClass.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlClass.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlClass.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlClass.Size = New System.Drawing.Size(255, 77)
    Me.pnlClass.TabIndex = 0
    '
    'lblClassName
    '
    Me.lblClassName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblClassName.AutoSize = True
    Me.lblClassName.Location = New System.Drawing.Point(3, 6)
    Me.lblClassName.Name = "lblClassName"
    Me.lblClassName.Size = New System.Drawing.Size(38, 13)
    Me.lblClassName.TabIndex = 0
    Me.lblClassName.Text = "Name:"
    '
    'lblClassDescription
    '
    Me.lblClassDescription.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblClassDescription.AutoSize = True
    Me.pnlClass.SetColumnSpan(Me.lblClassDescription, 2)
    Me.lblClassDescription.Location = New System.Drawing.Point(3, 31)
    Me.lblClassDescription.Name = "lblClassDescription"
    Me.lblClassDescription.Size = New System.Drawing.Size(63, 13)
    Me.lblClassDescription.TabIndex = 2
    Me.lblClassDescription.Text = "Description:"
    '
    'lblClassGUID
    '
    Me.lblClassGUID.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblClassGUID.AutoSize = True
    Me.pnlClass.SetColumnSpan(Me.lblClassGUID, 2)
    Me.lblClassGUID.Location = New System.Drawing.Point(3, 57)
    Me.lblClassGUID.Name = "lblClassGUID"
    Me.lblClassGUID.Size = New System.Drawing.Size(37, 13)
    Me.lblClassGUID.TabIndex = 4
    Me.lblClassGUID.Text = "GUID:"
    '
    'txtClassName
    '
    Me.txtClassName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtClassName.Location = New System.Drawing.Point(72, 3)
    Me.txtClassName.Name = "txtClassName"
    Me.txtClassName.ReadOnly = True
    Me.txtClassName.Size = New System.Drawing.Size(180, 20)
    Me.txtClassName.TabIndex = 1
    '
    'txtClassDescription
    '
    Me.txtClassDescription.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtClassDescription.Location = New System.Drawing.Point(72, 28)
    Me.txtClassDescription.Name = "txtClassDescription"
    Me.txtClassDescription.ReadOnly = True
    Me.txtClassDescription.Size = New System.Drawing.Size(180, 20)
    Me.txtClassDescription.TabIndex = 3
    '
    'txtClassGUID
    '
    Me.txtClassGUID.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtClassGUID.Location = New System.Drawing.Point(72, 53)
    Me.txtClassGUID.Name = "txtClassGUID"
    Me.txtClassGUID.ReadOnly = True
    Me.txtClassGUID.Size = New System.Drawing.Size(180, 20)
    Me.txtClassGUID.TabIndex = 5
    '
    'pctClassIcon
    '
    Me.pctClassIcon.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pctClassIcon.Location = New System.Drawing.Point(53, 4)
    Me.pctClassIcon.Margin = New System.Windows.Forms.Padding(0)
    Me.pctClassIcon.Name = "pctClassIcon"
    Me.pctClassIcon.Size = New System.Drawing.Size(16, 16)
    Me.pctClassIcon.TabIndex = 6
    Me.pctClassIcon.TabStop = False
    '
    'pctDriverIcon
    '
    Me.pctDriverIcon.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pctDriverIcon.Location = New System.Drawing.Point(82, 5)
    Me.pctDriverIcon.Margin = New System.Windows.Forms.Padding(0)
    Me.pctDriverIcon.Name = "pctDriverIcon"
    Me.pctDriverIcon.Size = New System.Drawing.Size(16, 16)
    Me.pctDriverIcon.TabIndex = 17
    Me.pctDriverIcon.TabStop = False
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdClose.AutoSize = True
    Me.cmdClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDriver.SetColumnSpan(Me.cmdClose, 2)
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(451, 313)
    Me.cmdClose.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Padding = New System.Windows.Forms.Padding(1)
    Me.cmdClose.Size = New System.Drawing.Size(80, 25)
    Me.cmdClose.TabIndex = 2
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'pnlDriverInfo
    '
    Me.pnlDriverInfo.ColumnCount = 2
    Me.pnlDriverInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDriverInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDriverInfo.Controls.Add(Me.lblArchitecture, 0, 0)
    Me.pnlDriverInfo.Controls.Add(Me.cmbArchitecture, 1, 0)
    Me.pnlDriverInfo.Controls.Add(Me.lblHardware, 0, 1)
    Me.pnlDriverInfo.Controls.Add(Me.cmbHardware, 1, 1)
    Me.pnlDriverInfo.Controls.Add(Me.grpHardware, 0, 2)
    Me.pnlDriverInfo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDriverInfo.Location = New System.Drawing.Point(267, 0)
    Me.pnlDriverInfo.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDriverInfo.Name = "pnlDriverInfo"
    Me.pnlDriverInfo.RowCount = 3
    Me.pnlDriverInfo.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverInfo.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDriverInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDriverInfo.Size = New System.Drawing.Size(267, 310)
    Me.pnlDriverInfo.TabIndex = 1
    '
    'lblArchitecture
    '
    Me.lblArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblArchitecture.AutoSize = True
    Me.lblArchitecture.Location = New System.Drawing.Point(3, 7)
    Me.lblArchitecture.Name = "lblArchitecture"
    Me.lblArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblArchitecture.TabIndex = 0
    Me.lblArchitecture.Text = "Architecture:"
    '
    'cmbArchitecture
    '
    Me.cmbArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbArchitecture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbArchitecture.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbArchitecture.FormattingEnabled = True
    Me.cmbArchitecture.Location = New System.Drawing.Point(76, 3)
    Me.cmbArchitecture.Name = "cmbArchitecture"
    Me.cmbArchitecture.Size = New System.Drawing.Size(188, 21)
    Me.cmbArchitecture.TabIndex = 1
    '
    'lblHardware
    '
    Me.lblHardware.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHardware.AutoSize = True
    Me.lblHardware.Location = New System.Drawing.Point(3, 34)
    Me.lblHardware.Name = "lblHardware"
    Me.lblHardware.Size = New System.Drawing.Size(56, 13)
    Me.lblHardware.TabIndex = 2
    Me.lblHardware.Text = "Hardware:"
    '
    'cmbHardware
    '
    Me.cmbHardware.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbHardware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbHardware.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbHardware.FormattingEnabled = True
    Me.cmbHardware.Location = New System.Drawing.Point(76, 30)
    Me.cmbHardware.Name = "cmbHardware"
    Me.cmbHardware.Size = New System.Drawing.Size(188, 21)
    Me.cmbHardware.TabIndex = 3
    '
    'grpHardware
    '
    Me.pnlDriverInfo.SetColumnSpan(Me.grpHardware, 2)
    Me.grpHardware.Controls.Add(Me.pnlHardware)
    Me.grpHardware.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grpHardware.Location = New System.Drawing.Point(3, 57)
    Me.grpHardware.Name = "grpHardware"
    Me.grpHardware.Size = New System.Drawing.Size(261, 250)
    Me.grpHardware.TabIndex = 4
    Me.grpHardware.TabStop = False
    Me.grpHardware.Text = "Hardware Description"
    '
    'pnlHardware
    '
    Me.pnlHardware.ColumnCount = 2
    Me.pnlHardware.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlHardware.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlHardware.Controls.Add(Me.lblHWID, 0, 4)
    Me.pnlHardware.Controls.Add(Me.lblHWCompatibleIDs, 0, 6)
    Me.pnlHardware.Controls.Add(Me.lblHWExcludeIDs, 0, 8)
    Me.pnlHardware.Controls.Add(Me.lstHWCompatibleIDs, 1, 6)
    Me.pnlHardware.Controls.Add(Me.lstHWExcludeIDs, 1, 8)
    Me.pnlHardware.Controls.Add(Me.lstHWIDs, 1, 4)
    Me.pnlHardware.Controls.Add(Me.lblHWServiceName, 0, 0)
    Me.pnlHardware.Controls.Add(Me.txtHWServiceName, 1, 0)
    Me.pnlHardware.Controls.Add(Me.lblHWDescription, 0, 1)
    Me.pnlHardware.Controls.Add(Me.txtHWDescription, 1, 1)
    Me.pnlHardware.Controls.Add(Me.lblHWManufacturer, 0, 3)
    Me.pnlHardware.Controls.Add(Me.lblHWArchitecture, 0, 2)
    Me.pnlHardware.Controls.Add(Me.txtHWManufacturer, 1, 3)
    Me.pnlHardware.Controls.Add(Me.txtHWArchitecture, 1, 2)
    Me.pnlHardware.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlHardware.Location = New System.Drawing.Point(3, 16)
    Me.pnlHardware.Name = "pnlHardware"
    Me.pnlHardware.RowCount = 10
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33112!))
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33445!))
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33445!))
    Me.pnlHardware.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlHardware.Size = New System.Drawing.Size(255, 231)
    Me.pnlHardware.TabIndex = 0
    '
    'lblHWID
    '
    Me.lblHWID.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWID.AutoSize = True
    Me.lblHWID.Location = New System.Drawing.Point(3, 110)
    Me.lblHWID.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWID.Name = "lblHWID"
    Me.lblHWID.Size = New System.Drawing.Size(70, 13)
    Me.lblHWID.TabIndex = 8
    Me.lblHWID.Text = "Hardware ID:"
    '
    'lblHWCompatibleIDs
    '
    Me.lblHWCompatibleIDs.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWCompatibleIDs.AutoSize = True
    Me.lblHWCompatibleIDs.Location = New System.Drawing.Point(3, 152)
    Me.lblHWCompatibleIDs.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWCompatibleIDs.Name = "lblHWCompatibleIDs"
    Me.lblHWCompatibleIDs.Size = New System.Drawing.Size(81, 13)
    Me.lblHWCompatibleIDs.TabIndex = 10
    Me.lblHWCompatibleIDs.Text = "Compatible IDs:"
    '
    'lblHWExcludeIDs
    '
    Me.lblHWExcludeIDs.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWExcludeIDs.AutoSize = True
    Me.lblHWExcludeIDs.Location = New System.Drawing.Point(3, 194)
    Me.lblHWExcludeIDs.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWExcludeIDs.Name = "lblHWExcludeIDs"
    Me.lblHWExcludeIDs.Size = New System.Drawing.Size(67, 13)
    Me.lblHWExcludeIDs.TabIndex = 12
    Me.lblHWExcludeIDs.Text = "Exclude IDs:"
    '
    'lstHWCompatibleIDs
    '
    Me.lstHWCompatibleIDs.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstHWCompatibleIDs.FormattingEnabled = True
    Me.lstHWCompatibleIDs.IntegralHeight = False
    Me.lstHWCompatibleIDs.Location = New System.Drawing.Point(90, 149)
    Me.lstHWCompatibleIDs.Name = "lstHWCompatibleIDs"
    Me.pnlHardware.SetRowSpan(Me.lstHWCompatibleIDs, 2)
    Me.lstHWCompatibleIDs.ScrollAlwaysVisible = True
    Me.lstHWCompatibleIDs.Size = New System.Drawing.Size(162, 36)
    Me.lstHWCompatibleIDs.TabIndex = 11
    '
    'lstHWExcludeIDs
    '
    Me.lstHWExcludeIDs.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstHWExcludeIDs.FormattingEnabled = True
    Me.lstHWExcludeIDs.IntegralHeight = False
    Me.lstHWExcludeIDs.Location = New System.Drawing.Point(90, 191)
    Me.lstHWExcludeIDs.Name = "lstHWExcludeIDs"
    Me.pnlHardware.SetRowSpan(Me.lstHWExcludeIDs, 2)
    Me.lstHWExcludeIDs.ScrollAlwaysVisible = True
    Me.lstHWExcludeIDs.Size = New System.Drawing.Size(162, 37)
    Me.lstHWExcludeIDs.TabIndex = 13
    '
    'lstHWIDs
    '
    Me.lstHWIDs.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstHWIDs.FormattingEnabled = True
    Me.lstHWIDs.IntegralHeight = False
    Me.lstHWIDs.Location = New System.Drawing.Point(90, 107)
    Me.lstHWIDs.Name = "lstHWIDs"
    Me.pnlHardware.SetRowSpan(Me.lstHWIDs, 2)
    Me.lstHWIDs.ScrollAlwaysVisible = True
    Me.lstHWIDs.Size = New System.Drawing.Size(162, 36)
    Me.lstHWIDs.TabIndex = 9
    '
    'lblHWServiceName
    '
    Me.lblHWServiceName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWServiceName.AutoSize = True
    Me.lblHWServiceName.Location = New System.Drawing.Point(3, 6)
    Me.lblHWServiceName.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWServiceName.Name = "lblHWServiceName"
    Me.lblHWServiceName.Size = New System.Drawing.Size(77, 13)
    Me.lblHWServiceName.TabIndex = 0
    Me.lblHWServiceName.Text = "Service Name:"
    '
    'txtHWServiceName
    '
    Me.txtHWServiceName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHWServiceName.Location = New System.Drawing.Point(90, 3)
    Me.txtHWServiceName.Name = "txtHWServiceName"
    Me.txtHWServiceName.ReadOnly = True
    Me.txtHWServiceName.Size = New System.Drawing.Size(162, 20)
    Me.txtHWServiceName.TabIndex = 1
    '
    'lblHWDescription
    '
    Me.lblHWDescription.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWDescription.AutoSize = True
    Me.lblHWDescription.Location = New System.Drawing.Point(3, 32)
    Me.lblHWDescription.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWDescription.Name = "lblHWDescription"
    Me.lblHWDescription.Size = New System.Drawing.Size(63, 13)
    Me.lblHWDescription.TabIndex = 2
    Me.lblHWDescription.Text = "Description:"
    '
    'txtHWDescription
    '
    Me.txtHWDescription.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHWDescription.Location = New System.Drawing.Point(90, 29)
    Me.txtHWDescription.Name = "txtHWDescription"
    Me.txtHWDescription.ReadOnly = True
    Me.txtHWDescription.Size = New System.Drawing.Size(162, 20)
    Me.txtHWDescription.TabIndex = 3
    '
    'lblHWManufacturer
    '
    Me.lblHWManufacturer.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWManufacturer.AutoSize = True
    Me.lblHWManufacturer.Location = New System.Drawing.Point(3, 84)
    Me.lblHWManufacturer.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWManufacturer.Name = "lblHWManufacturer"
    Me.lblHWManufacturer.Size = New System.Drawing.Size(73, 13)
    Me.lblHWManufacturer.TabIndex = 6
    Me.lblHWManufacturer.Text = "Manufacturer:"
    '
    'lblHWArchitecture
    '
    Me.lblHWArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHWArchitecture.AutoSize = True
    Me.lblHWArchitecture.Location = New System.Drawing.Point(3, 58)
    Me.lblHWArchitecture.Margin = New System.Windows.Forms.Padding(3, 6, 3, 7)
    Me.lblHWArchitecture.Name = "lblHWArchitecture"
    Me.lblHWArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblHWArchitecture.TabIndex = 4
    Me.lblHWArchitecture.Text = "Architecture:"
    '
    'txtHWManufacturer
    '
    Me.txtHWManufacturer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHWManufacturer.Location = New System.Drawing.Point(90, 81)
    Me.txtHWManufacturer.Name = "txtHWManufacturer"
    Me.txtHWManufacturer.ReadOnly = True
    Me.txtHWManufacturer.Size = New System.Drawing.Size(162, 20)
    Me.txtHWManufacturer.TabIndex = 7
    '
    'txtHWArchitecture
    '
    Me.txtHWArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHWArchitecture.Location = New System.Drawing.Point(90, 55)
    Me.txtHWArchitecture.Name = "txtHWArchitecture"
    Me.txtHWArchitecture.ReadOnly = True
    Me.txtHWArchitecture.Size = New System.Drawing.Size(162, 20)
    Me.txtHWArchitecture.TabIndex = 5
    '
    'frmDriverProps
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(534, 341)
    Me.Controls.Add(Me.pnlDriver)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.MinimumSize = New System.Drawing.Size(550, 375)
    Me.Name = "frmDriverProps"
    Me.ShowInTaskbar = False
    Me.Text = "Driver INF Properties"
    Me.pnlDriver.ResumeLayout(False)
    Me.pnlDriver.PerformLayout()
    Me.pnlDriverData.ResumeLayout(False)
    Me.pnlDriverData.PerformLayout()
    Me.grpClass.ResumeLayout(False)
    Me.pnlClass.ResumeLayout(False)
    Me.pnlClass.PerformLayout()
    CType(Me.pctClassIcon, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pctDriverIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlDriverInfo.ResumeLayout(False)
    Me.pnlDriverInfo.PerformLayout()
    Me.grpHardware.ResumeLayout(False)
    Me.pnlHardware.ResumeLayout(False)
    Me.pnlHardware.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlDriver As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlDriverData As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDriverStorePath As System.Windows.Forms.Label
  Friend WithEvents txtDriverStorePath As System.Windows.Forms.TextBox
  Friend WithEvents lblPublishedName As System.Windows.Forms.Label
  Friend WithEvents txtPublishedName As System.Windows.Forms.TextBox
  Friend WithEvents lblInbox As System.Windows.Forms.Label
  Friend WithEvents txtInbox As System.Windows.Forms.TextBox
  Friend WithEvents txtClassName As System.Windows.Forms.TextBox
  Friend WithEvents lblClassDescription As System.Windows.Forms.Label
  Friend WithEvents txtClassDescription As System.Windows.Forms.TextBox
  Friend WithEvents lblProviderName As System.Windows.Forms.Label
  Friend WithEvents txtOriginalFileName As System.Windows.Forms.TextBox
  Friend WithEvents lblClassGUID As System.Windows.Forms.Label
  Friend WithEvents txtClassGUID As System.Windows.Forms.TextBox
  Friend WithEvents txtProviderName As System.Windows.Forms.TextBox
  Friend WithEvents lblDate As System.Windows.Forms.Label
  Friend WithEvents txtDate As System.Windows.Forms.TextBox
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents txtVersion As System.Windows.Forms.TextBox
  Friend WithEvents lblBootCritical As System.Windows.Forms.Label
  Friend WithEvents txtBootCritical As System.Windows.Forms.TextBox
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents pnlDriverInfo As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblArchitecture As System.Windows.Forms.Label
  Friend WithEvents cmbArchitecture As System.Windows.Forms.ComboBox
  Friend WithEvents lblHardware As System.Windows.Forms.Label
  Friend WithEvents cmbHardware As System.Windows.Forms.ComboBox
  Friend WithEvents grpHardware As System.Windows.Forms.GroupBox
  Friend WithEvents pnlHardware As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblHWManufacturer As System.Windows.Forms.Label
  Friend WithEvents txtHWManufacturer As System.Windows.Forms.TextBox
  Friend WithEvents lblHWDescription As System.Windows.Forms.Label
  Friend WithEvents lblHWArchitecture As System.Windows.Forms.Label
  Friend WithEvents lblHWID As System.Windows.Forms.Label
  Friend WithEvents lblHWServiceName As System.Windows.Forms.Label
  Friend WithEvents lblHWExcludeIDs As System.Windows.Forms.Label
  Friend WithEvents txtHWDescription As System.Windows.Forms.TextBox
  Friend WithEvents txtHWArchitecture As System.Windows.Forms.TextBox
  Friend WithEvents txtHWServiceName As System.Windows.Forms.TextBox
  Friend WithEvents lstHWExcludeIDs As System.Windows.Forms.ListBox
  Friend WithEvents lstHWIDs As System.Windows.Forms.ListBox
  Friend WithEvents lblHWCompatibleIDs As System.Windows.Forms.Label
  Friend WithEvents lstHWCompatibleIDs As System.Windows.Forms.ListBox
  Friend WithEvents grpClass As System.Windows.Forms.GroupBox
  Friend WithEvents pnlClass As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblOriginalFileName As System.Windows.Forms.Label
  Friend WithEvents lblClassName As System.Windows.Forms.Label
  Friend WithEvents pctClassIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pctDriverIcon As System.Windows.Forms.PictureBox
End Class
