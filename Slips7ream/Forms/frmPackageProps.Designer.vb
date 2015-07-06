<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPackageProps
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPackageProps))
    Me.pnlProps = New System.Windows.Forms.TableLayoutPanel()
    Me.lblIndex = New System.Windows.Forms.Label()
    Me.txtIndex = New System.Windows.Forms.TextBox()
    Me.lblName = New System.Windows.Forms.Label()
    Me.txtName = New System.Windows.Forms.TextBox()
    Me.lblDesc = New System.Windows.Forms.Label()
    Me.txtDesc = New System.Windows.Forms.TextBox()
    Me.lblSize = New System.Windows.Forms.Label()
    Me.txtSize = New System.Windows.Forms.TextBox()
    Me.lblArchitecture = New System.Windows.Forms.Label()
    Me.txtArchitecture = New System.Windows.Forms.TextBox()
    Me.lblHAL = New System.Windows.Forms.Label()
    Me.txtHAL = New System.Windows.Forms.TextBox()
    Me.lblVersion = New System.Windows.Forms.Label()
    Me.txtVersion = New System.Windows.Forms.TextBox()
    Me.lblSPBuild = New System.Windows.Forms.Label()
    Me.txtSPBuild = New System.Windows.Forms.TextBox()
    Me.lblSPLevel = New System.Windows.Forms.Label()
    Me.txtSPLevel = New System.Windows.Forms.TextBox()
    Me.lblEdition = New System.Windows.Forms.Label()
    Me.txtEdition = New System.Windows.Forms.TextBox()
    Me.lblInstallation = New System.Windows.Forms.Label()
    Me.txtInstallation = New System.Windows.Forms.TextBox()
    Me.lblProductType = New System.Windows.Forms.Label()
    Me.txtProductType = New System.Windows.Forms.TextBox()
    Me.lblProductSuite = New System.Windows.Forms.Label()
    Me.txtProductSuite = New System.Windows.Forms.TextBox()
    Me.lblSystemRoot = New System.Windows.Forms.Label()
    Me.txtSystemRoot = New System.Windows.Forms.TextBox()
    Me.lblFiles = New System.Windows.Forms.Label()
    Me.txtFiles = New System.Windows.Forms.TextBox()
    Me.lblDirectories = New System.Windows.Forms.Label()
    Me.txtDirectories = New System.Windows.Forms.TextBox()
    Me.lblCreated = New System.Windows.Forms.Label()
    Me.txtCreated = New System.Windows.Forms.TextBox()
    Me.lblModified = New System.Windows.Forms.Label()
    Me.txtModified = New System.Windows.Forms.TextBox()
    Me.lblLanguages = New System.Windows.Forms.Label()
    Me.txtLanguages = New System.Windows.Forms.TextBox()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.lvUpdates = New System.Windows.Forms.ListView()
    Me.colPackage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colVer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.imlUpdates = New System.Windows.Forms.ImageList(Me.components)
    Me.lblUpdates = New System.Windows.Forms.Label()
    Me.pnlProps.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlProps
    '
    Me.pnlProps.ColumnCount = 3
    Me.pnlProps.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProps.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlProps.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350.0!))
    Me.pnlProps.Controls.Add(Me.lblIndex, 0, 0)
    Me.pnlProps.Controls.Add(Me.txtIndex, 1, 0)
    Me.pnlProps.Controls.Add(Me.lblName, 0, 1)
    Me.pnlProps.Controls.Add(Me.txtName, 1, 1)
    Me.pnlProps.Controls.Add(Me.lblDesc, 0, 2)
    Me.pnlProps.Controls.Add(Me.txtDesc, 1, 2)
    Me.pnlProps.Controls.Add(Me.lblSize, 0, 3)
    Me.pnlProps.Controls.Add(Me.txtSize, 1, 3)
    Me.pnlProps.Controls.Add(Me.lblArchitecture, 0, 4)
    Me.pnlProps.Controls.Add(Me.txtArchitecture, 1, 4)
    Me.pnlProps.Controls.Add(Me.lblHAL, 0, 5)
    Me.pnlProps.Controls.Add(Me.txtHAL, 1, 5)
    Me.pnlProps.Controls.Add(Me.lblVersion, 0, 6)
    Me.pnlProps.Controls.Add(Me.txtVersion, 1, 6)
    Me.pnlProps.Controls.Add(Me.lblSPBuild, 0, 7)
    Me.pnlProps.Controls.Add(Me.txtSPBuild, 1, 7)
    Me.pnlProps.Controls.Add(Me.lblSPLevel, 0, 8)
    Me.pnlProps.Controls.Add(Me.txtSPLevel, 1, 8)
    Me.pnlProps.Controls.Add(Me.lblEdition, 0, 9)
    Me.pnlProps.Controls.Add(Me.txtEdition, 1, 9)
    Me.pnlProps.Controls.Add(Me.lblInstallation, 0, 10)
    Me.pnlProps.Controls.Add(Me.txtInstallation, 1, 10)
    Me.pnlProps.Controls.Add(Me.lblProductType, 0, 11)
    Me.pnlProps.Controls.Add(Me.txtProductType, 1, 11)
    Me.pnlProps.Controls.Add(Me.lblProductSuite, 0, 12)
    Me.pnlProps.Controls.Add(Me.txtProductSuite, 1, 12)
    Me.pnlProps.Controls.Add(Me.lblSystemRoot, 0, 13)
    Me.pnlProps.Controls.Add(Me.txtSystemRoot, 1, 13)
    Me.pnlProps.Controls.Add(Me.lblFiles, 0, 14)
    Me.pnlProps.Controls.Add(Me.txtFiles, 1, 14)
    Me.pnlProps.Controls.Add(Me.lblDirectories, 0, 15)
    Me.pnlProps.Controls.Add(Me.txtDirectories, 1, 15)
    Me.pnlProps.Controls.Add(Me.lblCreated, 0, 16)
    Me.pnlProps.Controls.Add(Me.txtCreated, 1, 16)
    Me.pnlProps.Controls.Add(Me.lblModified, 0, 17)
    Me.pnlProps.Controls.Add(Me.txtModified, 1, 17)
    Me.pnlProps.Controls.Add(Me.lblLanguages, 0, 18)
    Me.pnlProps.Controls.Add(Me.txtLanguages, 1, 18)
    Me.pnlProps.Controls.Add(Me.pnlButtons, 0, 19)
    Me.pnlProps.Controls.Add(Me.lvUpdates, 2, 1)
    Me.pnlProps.Controls.Add(Me.lblUpdates, 2, 0)
    Me.pnlProps.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProps.Location = New System.Drawing.Point(0, 0)
    Me.pnlProps.Name = "pnlProps"
    Me.pnlProps.RowCount = 20
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlProps.Size = New System.Drawing.Size(631, 533)
    Me.pnlProps.TabIndex = 0
    '
    'lblIndex
    '
    Me.lblIndex.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblIndex.AutoSize = True
    Me.lblIndex.Location = New System.Drawing.Point(3, 6)
    Me.lblIndex.Name = "lblIndex"
    Me.lblIndex.Size = New System.Drawing.Size(36, 13)
    Me.lblIndex.TabIndex = 0
    Me.lblIndex.Text = "Index:"
    '
    'txtIndex
    '
    Me.txtIndex.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtIndex.Location = New System.Drawing.Point(111, 2)
    Me.txtIndex.Margin = New System.Windows.Forms.Padding(2)
    Me.txtIndex.Name = "txtIndex"
    Me.txtIndex.ReadOnly = True
    Me.txtIndex.Size = New System.Drawing.Size(168, 20)
    Me.txtIndex.TabIndex = 1
    '
    'lblName
    '
    Me.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblName.AutoSize = True
    Me.lblName.Location = New System.Drawing.Point(3, 30)
    Me.lblName.Name = "lblName"
    Me.lblName.Size = New System.Drawing.Size(38, 13)
    Me.lblName.TabIndex = 2
    Me.lblName.Text = "&Name:"
    '
    'txtName
    '
    Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.Location = New System.Drawing.Point(111, 27)
    Me.txtName.Margin = New System.Windows.Forms.Padding(2)
    Me.txtName.Name = "txtName"
    Me.txtName.Size = New System.Drawing.Size(168, 20)
    Me.txtName.TabIndex = 3
    '
    'lblDesc
    '
    Me.lblDesc.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDesc.AutoSize = True
    Me.lblDesc.Location = New System.Drawing.Point(3, 54)
    Me.lblDesc.Name = "lblDesc"
    Me.lblDesc.Size = New System.Drawing.Size(63, 13)
    Me.lblDesc.TabIndex = 4
    Me.lblDesc.Text = "Description:"
    '
    'txtDesc
    '
    Me.txtDesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDesc.Location = New System.Drawing.Point(111, 51)
    Me.txtDesc.Margin = New System.Windows.Forms.Padding(2)
    Me.txtDesc.Name = "txtDesc"
    Me.txtDesc.ReadOnly = True
    Me.txtDesc.Size = New System.Drawing.Size(168, 20)
    Me.txtDesc.TabIndex = 5
    '
    'lblSize
    '
    Me.lblSize.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSize.AutoSize = True
    Me.lblSize.Location = New System.Drawing.Point(3, 78)
    Me.lblSize.Name = "lblSize"
    Me.lblSize.Size = New System.Drawing.Size(30, 13)
    Me.lblSize.TabIndex = 6
    Me.lblSize.Text = "Size:"
    '
    'txtSize
    '
    Me.txtSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSize.Location = New System.Drawing.Point(111, 75)
    Me.txtSize.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSize.Name = "txtSize"
    Me.txtSize.ReadOnly = True
    Me.txtSize.Size = New System.Drawing.Size(168, 20)
    Me.txtSize.TabIndex = 7
    '
    'lblArchitecture
    '
    Me.lblArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblArchitecture.AutoSize = True
    Me.lblArchitecture.Location = New System.Drawing.Point(3, 102)
    Me.lblArchitecture.Name = "lblArchitecture"
    Me.lblArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblArchitecture.TabIndex = 8
    Me.lblArchitecture.Text = "Architecture:"
    '
    'txtArchitecture
    '
    Me.txtArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtArchitecture.Location = New System.Drawing.Point(111, 99)
    Me.txtArchitecture.Margin = New System.Windows.Forms.Padding(2)
    Me.txtArchitecture.Name = "txtArchitecture"
    Me.txtArchitecture.ReadOnly = True
    Me.txtArchitecture.Size = New System.Drawing.Size(168, 20)
    Me.txtArchitecture.TabIndex = 9
    '
    'lblHAL
    '
    Me.lblHAL.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHAL.AutoSize = True
    Me.lblHAL.Location = New System.Drawing.Point(3, 126)
    Me.lblHAL.Name = "lblHAL"
    Me.lblHAL.Size = New System.Drawing.Size(31, 13)
    Me.lblHAL.TabIndex = 10
    Me.lblHAL.Text = "HAL:"
    '
    'txtHAL
    '
    Me.txtHAL.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHAL.Location = New System.Drawing.Point(111, 123)
    Me.txtHAL.Margin = New System.Windows.Forms.Padding(2)
    Me.txtHAL.Name = "txtHAL"
    Me.txtHAL.ReadOnly = True
    Me.txtHAL.Size = New System.Drawing.Size(168, 20)
    Me.txtHAL.TabIndex = 11
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblVersion.AutoSize = True
    Me.lblVersion.Location = New System.Drawing.Point(3, 150)
    Me.lblVersion.Name = "lblVersion"
    Me.lblVersion.Size = New System.Drawing.Size(45, 13)
    Me.lblVersion.TabIndex = 12
    Me.lblVersion.Text = "Version:"
    '
    'txtVersion
    '
    Me.txtVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVersion.Location = New System.Drawing.Point(111, 147)
    Me.txtVersion.Margin = New System.Windows.Forms.Padding(2)
    Me.txtVersion.Name = "txtVersion"
    Me.txtVersion.ReadOnly = True
    Me.txtVersion.Size = New System.Drawing.Size(168, 20)
    Me.txtVersion.TabIndex = 13
    '
    'lblSPBuild
    '
    Me.lblSPBuild.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSPBuild.AutoSize = True
    Me.lblSPBuild.Location = New System.Drawing.Point(3, 174)
    Me.lblSPBuild.Name = "lblSPBuild"
    Me.lblSPBuild.Size = New System.Drawing.Size(100, 13)
    Me.lblSPBuild.TabIndex = 14
    Me.lblSPBuild.Text = "Service Pack Build:"
    '
    'txtSPBuild
    '
    Me.txtSPBuild.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSPBuild.Location = New System.Drawing.Point(111, 171)
    Me.txtSPBuild.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSPBuild.Name = "txtSPBuild"
    Me.txtSPBuild.ReadOnly = True
    Me.txtSPBuild.Size = New System.Drawing.Size(168, 20)
    Me.txtSPBuild.TabIndex = 15
    '
    'lblSPLevel
    '
    Me.lblSPLevel.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSPLevel.AutoSize = True
    Me.lblSPLevel.Location = New System.Drawing.Point(3, 198)
    Me.lblSPLevel.Name = "lblSPLevel"
    Me.lblSPLevel.Size = New System.Drawing.Size(103, 13)
    Me.lblSPLevel.TabIndex = 16
    Me.lblSPLevel.Text = "Service Pack Level:"
    '
    'txtSPLevel
    '
    Me.txtSPLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSPLevel.Location = New System.Drawing.Point(111, 195)
    Me.txtSPLevel.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSPLevel.Name = "txtSPLevel"
    Me.txtSPLevel.ReadOnly = True
    Me.txtSPLevel.Size = New System.Drawing.Size(168, 20)
    Me.txtSPLevel.TabIndex = 17
    '
    'lblEdition
    '
    Me.lblEdition.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblEdition.AutoSize = True
    Me.lblEdition.Location = New System.Drawing.Point(3, 222)
    Me.lblEdition.Name = "lblEdition"
    Me.lblEdition.Size = New System.Drawing.Size(42, 13)
    Me.lblEdition.TabIndex = 18
    Me.lblEdition.Text = "Edition:"
    '
    'txtEdition
    '
    Me.txtEdition.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtEdition.Location = New System.Drawing.Point(111, 219)
    Me.txtEdition.Margin = New System.Windows.Forms.Padding(2)
    Me.txtEdition.Name = "txtEdition"
    Me.txtEdition.ReadOnly = True
    Me.txtEdition.Size = New System.Drawing.Size(168, 20)
    Me.txtEdition.TabIndex = 19
    '
    'lblInstallation
    '
    Me.lblInstallation.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInstallation.AutoSize = True
    Me.lblInstallation.Location = New System.Drawing.Point(3, 246)
    Me.lblInstallation.Name = "lblInstallation"
    Me.lblInstallation.Size = New System.Drawing.Size(60, 13)
    Me.lblInstallation.TabIndex = 20
    Me.lblInstallation.Text = "Installation:"
    '
    'txtInstallation
    '
    Me.txtInstallation.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInstallation.Location = New System.Drawing.Point(111, 243)
    Me.txtInstallation.Margin = New System.Windows.Forms.Padding(2)
    Me.txtInstallation.Name = "txtInstallation"
    Me.txtInstallation.ReadOnly = True
    Me.txtInstallation.Size = New System.Drawing.Size(168, 20)
    Me.txtInstallation.TabIndex = 21
    '
    'lblProductType
    '
    Me.lblProductType.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProductType.AutoSize = True
    Me.lblProductType.Location = New System.Drawing.Point(3, 270)
    Me.lblProductType.Name = "lblProductType"
    Me.lblProductType.Size = New System.Drawing.Size(74, 13)
    Me.lblProductType.TabIndex = 22
    Me.lblProductType.Text = "Product Type:"
    '
    'txtProductType
    '
    Me.txtProductType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProductType.Location = New System.Drawing.Point(111, 267)
    Me.txtProductType.Margin = New System.Windows.Forms.Padding(2)
    Me.txtProductType.Name = "txtProductType"
    Me.txtProductType.ReadOnly = True
    Me.txtProductType.Size = New System.Drawing.Size(168, 20)
    Me.txtProductType.TabIndex = 23
    '
    'lblProductSuite
    '
    Me.lblProductSuite.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProductSuite.AutoSize = True
    Me.lblProductSuite.Location = New System.Drawing.Point(3, 294)
    Me.lblProductSuite.Name = "lblProductSuite"
    Me.lblProductSuite.Size = New System.Drawing.Size(74, 13)
    Me.lblProductSuite.TabIndex = 24
    Me.lblProductSuite.Text = "Product Suite:"
    '
    'txtProductSuite
    '
    Me.txtProductSuite.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProductSuite.Location = New System.Drawing.Point(111, 291)
    Me.txtProductSuite.Margin = New System.Windows.Forms.Padding(2)
    Me.txtProductSuite.Name = "txtProductSuite"
    Me.txtProductSuite.ReadOnly = True
    Me.txtProductSuite.Size = New System.Drawing.Size(168, 20)
    Me.txtProductSuite.TabIndex = 25
    '
    'lblSystemRoot
    '
    Me.lblSystemRoot.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSystemRoot.AutoSize = True
    Me.lblSystemRoot.Location = New System.Drawing.Point(3, 318)
    Me.lblSystemRoot.Name = "lblSystemRoot"
    Me.lblSystemRoot.Size = New System.Drawing.Size(70, 13)
    Me.lblSystemRoot.TabIndex = 26
    Me.lblSystemRoot.Text = "System Root:"
    '
    'txtSystemRoot
    '
    Me.txtSystemRoot.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSystemRoot.Location = New System.Drawing.Point(111, 315)
    Me.txtSystemRoot.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSystemRoot.Name = "txtSystemRoot"
    Me.txtSystemRoot.ReadOnly = True
    Me.txtSystemRoot.Size = New System.Drawing.Size(168, 20)
    Me.txtSystemRoot.TabIndex = 27
    '
    'lblFiles
    '
    Me.lblFiles.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblFiles.AutoSize = True
    Me.lblFiles.Location = New System.Drawing.Point(3, 342)
    Me.lblFiles.Name = "lblFiles"
    Me.lblFiles.Size = New System.Drawing.Size(31, 13)
    Me.lblFiles.TabIndex = 28
    Me.lblFiles.Text = "Files:"
    '
    'txtFiles
    '
    Me.txtFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFiles.Location = New System.Drawing.Point(111, 339)
    Me.txtFiles.Margin = New System.Windows.Forms.Padding(2)
    Me.txtFiles.Name = "txtFiles"
    Me.txtFiles.ReadOnly = True
    Me.txtFiles.Size = New System.Drawing.Size(168, 20)
    Me.txtFiles.TabIndex = 29
    '
    'lblDirectories
    '
    Me.lblDirectories.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDirectories.AutoSize = True
    Me.lblDirectories.Location = New System.Drawing.Point(3, 366)
    Me.lblDirectories.Name = "lblDirectories"
    Me.lblDirectories.Size = New System.Drawing.Size(60, 13)
    Me.lblDirectories.TabIndex = 30
    Me.lblDirectories.Text = "Directories:"
    '
    'txtDirectories
    '
    Me.txtDirectories.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDirectories.Location = New System.Drawing.Point(111, 363)
    Me.txtDirectories.Margin = New System.Windows.Forms.Padding(2)
    Me.txtDirectories.Name = "txtDirectories"
    Me.txtDirectories.ReadOnly = True
    Me.txtDirectories.Size = New System.Drawing.Size(168, 20)
    Me.txtDirectories.TabIndex = 31
    '
    'lblCreated
    '
    Me.lblCreated.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblCreated.AutoSize = True
    Me.lblCreated.Location = New System.Drawing.Point(3, 390)
    Me.lblCreated.Name = "lblCreated"
    Me.lblCreated.Size = New System.Drawing.Size(47, 13)
    Me.lblCreated.TabIndex = 32
    Me.lblCreated.Text = "Created:"
    '
    'txtCreated
    '
    Me.txtCreated.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtCreated.Location = New System.Drawing.Point(111, 387)
    Me.txtCreated.Margin = New System.Windows.Forms.Padding(2)
    Me.txtCreated.Name = "txtCreated"
    Me.txtCreated.ReadOnly = True
    Me.txtCreated.Size = New System.Drawing.Size(168, 20)
    Me.txtCreated.TabIndex = 33
    '
    'lblModified
    '
    Me.lblModified.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblModified.AutoSize = True
    Me.lblModified.Location = New System.Drawing.Point(3, 414)
    Me.lblModified.Name = "lblModified"
    Me.lblModified.Size = New System.Drawing.Size(53, 13)
    Me.lblModified.TabIndex = 34
    Me.lblModified.Text = "Modfified:"
    '
    'txtModified
    '
    Me.txtModified.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtModified.Location = New System.Drawing.Point(111, 411)
    Me.txtModified.Margin = New System.Windows.Forms.Padding(2)
    Me.txtModified.Name = "txtModified"
    Me.txtModified.ReadOnly = True
    Me.txtModified.Size = New System.Drawing.Size(168, 20)
    Me.txtModified.TabIndex = 35
    '
    'lblLanguages
    '
    Me.lblLanguages.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblLanguages.AutoSize = True
    Me.lblLanguages.Location = New System.Drawing.Point(3, 458)
    Me.lblLanguages.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblLanguages.Name = "lblLanguages"
    Me.lblLanguages.Size = New System.Drawing.Size(63, 13)
    Me.lblLanguages.TabIndex = 36
    Me.lblLanguages.Text = "&Languages:"
    '
    'txtLanguages
    '
    Me.txtLanguages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtLanguages.Location = New System.Drawing.Point(111, 435)
    Me.txtLanguages.Margin = New System.Windows.Forms.Padding(2)
    Me.txtLanguages.Multiline = True
    Me.txtLanguages.Name = "txtLanguages"
    Me.txtLanguages.ReadOnly = True
    Me.txtLanguages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtLanguages.Size = New System.Drawing.Size(168, 59)
    Me.txtLanguages.TabIndex = 37
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlProps.SetColumnSpan(Me.pnlButtons, 3)
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(469, 500)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(162, 29)
    Me.pnlButtons.TabIndex = 38
    '
    'cmdClose
    '
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(84, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 40
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 23)
    Me.cmdSave.TabIndex = 39
    Me.cmdSave.Text = "&Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'lvUpdates
    '
    Me.lvUpdates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPackage, Me.colVer})
    Me.lvUpdates.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvUpdates.FullRowSelect = True
    Me.lvUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.lvUpdates.Location = New System.Drawing.Point(284, 28)
    Me.lvUpdates.MultiSelect = False
    Me.lvUpdates.Name = "lvUpdates"
    Me.pnlProps.SetRowSpan(Me.lvUpdates, 18)
    Me.lvUpdates.ShowGroups = False
    Me.lvUpdates.ShowItemToolTips = True
    Me.lvUpdates.Size = New System.Drawing.Size(344, 465)
    Me.lvUpdates.SmallImageList = Me.imlUpdates
    Me.lvUpdates.TabIndex = 40
    Me.lvUpdates.UseCompatibleStateImageBehavior = False
    Me.lvUpdates.View = System.Windows.Forms.View.Details
    '
    'colPackage
    '
    Me.colPackage.Text = "Package Name"
    Me.colPackage.Width = 228
    '
    'colVer
    '
    Me.colVer.Text = "Version"
    Me.colVer.Width = 92
    '
    'imlUpdates
    '
    Me.imlUpdates.ImageStream = CType(resources.GetObject("imlUpdates.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.imlUpdates.TransparentColor = System.Drawing.Color.Transparent
    Me.imlUpdates.Images.SetKeyName(0, "DID")
    Me.imlUpdates.Images.SetKeyName(1, "DO")
    Me.imlUpdates.Images.SetKeyName(2, "UNDO")
    Me.imlUpdates.Images.SetKeyName(3, "NO")
    '
    'lblUpdates
    '
    Me.lblUpdates.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblUpdates.AutoSize = True
    Me.lblUpdates.Location = New System.Drawing.Point(405, 6)
    Me.lblUpdates.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblUpdates.Name = "lblUpdates"
    Me.lblUpdates.Size = New System.Drawing.Size(101, 13)
    Me.lblUpdates.TabIndex = 39
    Me.lblUpdates.Text = "Integrated &Updates:"
    '
    'frmPackageProps
    '
    Me.AcceptButton = Me.cmdSave
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(631, 533)
    Me.Controls.Add(Me.pnlProps)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmPackageProps"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Image Package Properties"
    Me.pnlProps.ResumeLayout(False)
    Me.pnlProps.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlProps As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblIndex As System.Windows.Forms.Label
  Friend WithEvents txtIndex As System.Windows.Forms.TextBox
  Friend WithEvents lblName As System.Windows.Forms.Label
  Friend WithEvents txtName As System.Windows.Forms.TextBox
  Friend WithEvents lblDesc As System.Windows.Forms.Label
  Friend WithEvents txtDesc As System.Windows.Forms.TextBox
  Friend WithEvents lblSize As System.Windows.Forms.Label
  Friend WithEvents txtSize As System.Windows.Forms.TextBox
  Friend WithEvents lblArchitecture As System.Windows.Forms.Label
  Friend WithEvents txtArchitecture As System.Windows.Forms.TextBox
  Friend WithEvents lblHAL As System.Windows.Forms.Label
  Friend WithEvents txtHAL As System.Windows.Forms.TextBox
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents txtVersion As System.Windows.Forms.TextBox
  Friend WithEvents lblSPBuild As System.Windows.Forms.Label
  Friend WithEvents txtSPBuild As System.Windows.Forms.TextBox
  Friend WithEvents lblSPLevel As System.Windows.Forms.Label
  Friend WithEvents txtSPLevel As System.Windows.Forms.TextBox
  Friend WithEvents lblEdition As System.Windows.Forms.Label
  Friend WithEvents txtEdition As System.Windows.Forms.TextBox
  Friend WithEvents lblInstallation As System.Windows.Forms.Label
  Friend WithEvents txtInstallation As System.Windows.Forms.TextBox
  Friend WithEvents lblProductType As System.Windows.Forms.Label
  Friend WithEvents txtProductType As System.Windows.Forms.TextBox
  Friend WithEvents lblProductSuite As System.Windows.Forms.Label
  Friend WithEvents txtProductSuite As System.Windows.Forms.TextBox
  Friend WithEvents lblSystemRoot As System.Windows.Forms.Label
  Friend WithEvents txtSystemRoot As System.Windows.Forms.TextBox
  Friend WithEvents lblFiles As System.Windows.Forms.Label
  Friend WithEvents txtFiles As System.Windows.Forms.TextBox
  Friend WithEvents lblDirectories As System.Windows.Forms.Label
  Friend WithEvents txtDirectories As System.Windows.Forms.TextBox
  Friend WithEvents lblCreated As System.Windows.Forms.Label
  Friend WithEvents txtCreated As System.Windows.Forms.TextBox
  Friend WithEvents lblModified As System.Windows.Forms.Label
  Friend WithEvents txtModified As System.Windows.Forms.TextBox
  Friend WithEvents lblLanguages As System.Windows.Forms.Label
  Friend WithEvents txtLanguages As System.Windows.Forms.TextBox
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents lblUpdates As System.Windows.Forms.Label
  Friend WithEvents lvUpdates As System.Windows.Forms.ListView
  Friend WithEvents colPackage As System.Windows.Forms.ColumnHeader
  Friend WithEvents colVer As System.Windows.Forms.ColumnHeader
  Friend WithEvents imlUpdates As System.Windows.Forms.ImageList
End Class
