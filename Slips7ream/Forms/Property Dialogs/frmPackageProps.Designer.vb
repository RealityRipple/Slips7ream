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
    Me.lblSPBuild = New System.Windows.Forms.Label()
    Me.lblSPLevel = New System.Windows.Forms.Label()
    Me.txtSPBuild = New System.Windows.Forms.TextBox()
    Me.txtSPLevel = New System.Windows.Forms.TextBox()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.imlUpdates = New System.Windows.Forms.ImageList(Me.components)
    Me.imlFeatures = New System.Windows.Forms.ImageList(Me.components)
    Me.cmdLoadFeatures = New System.Windows.Forms.Button()
    Me.cmdLoadUpdates = New System.Windows.Forms.Button()
    Me.colFeatureDisplayName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colFeatureDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colFeatureRestart = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.pnlMain = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlLists = New System.Windows.Forms.TableLayoutPanel()
    Me.expFeatures = New Slips7ream.Expander()
    Me.tvFeatures = New Slips7ream.TreeViewEx()
    Me.expUpdates = New Slips7ream.Expander()
    Me.lvUpdates = New Slips7ream.ListViewEx()
    Me.colPackage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colVer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.expDrivers = New Slips7ream.Expander()
    Me.cmdLoadDrivers = New System.Windows.Forms.Button()
    Me.pnlDrivers = New System.Windows.Forms.TableLayoutPanel()
    Me.lvDriverClass = New Slips7ream.ListViewEx()
    Me.lvcGroup = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.imlDriverClass = New System.Windows.Forms.ImageList(Me.components)
    Me.lvDriverProvider = New Slips7ream.ListViewEx()
    Me.lvcCompany = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.imlDriverCompany = New System.Windows.Forms.ImageList(Me.components)
    Me.lvDriverINF = New Slips7ream.ListViewEx()
    Me.lvcName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.lvcVer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.imlDriverINF = New System.Windows.Forms.ImageList(Me.components)
    Me.lblDriverClass = New System.Windows.Forms.Label()
    Me.lblDriverProvider = New System.Windows.Forms.Label()
    Me.lblDriverINF = New System.Windows.Forms.Label()
    Me.mnuFeatures = New System.Windows.Forms.ContextMenu()
    Me.mnuFeatureEnabled = New System.Windows.Forms.MenuItem()
    Me.mnuFeatureSpace = New System.Windows.Forms.MenuItem()
    Me.mnuFeatureExpandAll = New System.Windows.Forms.MenuItem()
    Me.mnuFeatureExpand = New System.Windows.Forms.MenuItem()
    Me.mnuFeatureCollapse = New System.Windows.Forms.MenuItem()
    Me.mnuFeatureCollapseAll = New System.Windows.Forms.MenuItem()
    Me.mnuUpdates = New System.Windows.Forms.ContextMenu()
    Me.mnuUpdateInclude = New System.Windows.Forms.MenuItem()
    Me.helpS7M = New System.Windows.Forms.HelpProvider()
    Me.pnlProps.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.pnlMain.SuspendLayout()
    Me.pnlLists.SuspendLayout()
    Me.pnlDrivers.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlProps
    '
    Me.pnlProps.AutoSize = True
    Me.pnlProps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlProps.ColumnCount = 2
    Me.pnlProps.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProps.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProps.Controls.Add(Me.lblIndex, 0, 0)
    Me.pnlProps.Controls.Add(Me.txtIndex, 1, 0)
    Me.pnlProps.Controls.Add(Me.lblName, 0, 2)
    Me.pnlProps.Controls.Add(Me.txtName, 1, 2)
    Me.pnlProps.Controls.Add(Me.lblDesc, 0, 4)
    Me.pnlProps.Controls.Add(Me.txtDesc, 1, 4)
    Me.pnlProps.Controls.Add(Me.lblSize, 0, 6)
    Me.pnlProps.Controls.Add(Me.txtSize, 1, 6)
    Me.pnlProps.Controls.Add(Me.lblArchitecture, 0, 8)
    Me.pnlProps.Controls.Add(Me.txtArchitecture, 1, 8)
    Me.pnlProps.Controls.Add(Me.lblHAL, 0, 10)
    Me.pnlProps.Controls.Add(Me.txtHAL, 1, 10)
    Me.pnlProps.Controls.Add(Me.lblVersion, 0, 12)
    Me.pnlProps.Controls.Add(Me.txtVersion, 1, 12)
    Me.pnlProps.Controls.Add(Me.lblEdition, 0, 18)
    Me.pnlProps.Controls.Add(Me.txtEdition, 1, 18)
    Me.pnlProps.Controls.Add(Me.lblInstallation, 0, 20)
    Me.pnlProps.Controls.Add(Me.txtInstallation, 1, 20)
    Me.pnlProps.Controls.Add(Me.lblProductType, 0, 22)
    Me.pnlProps.Controls.Add(Me.txtProductType, 1, 22)
    Me.pnlProps.Controls.Add(Me.lblProductSuite, 0, 24)
    Me.pnlProps.Controls.Add(Me.txtProductSuite, 1, 24)
    Me.pnlProps.Controls.Add(Me.lblSystemRoot, 0, 26)
    Me.pnlProps.Controls.Add(Me.txtSystemRoot, 1, 26)
    Me.pnlProps.Controls.Add(Me.lblFiles, 0, 28)
    Me.pnlProps.Controls.Add(Me.txtFiles, 1, 28)
    Me.pnlProps.Controls.Add(Me.lblDirectories, 0, 30)
    Me.pnlProps.Controls.Add(Me.txtDirectories, 1, 30)
    Me.pnlProps.Controls.Add(Me.lblCreated, 0, 32)
    Me.pnlProps.Controls.Add(Me.txtCreated, 1, 32)
    Me.pnlProps.Controls.Add(Me.lblModified, 0, 34)
    Me.pnlProps.Controls.Add(Me.txtModified, 1, 34)
    Me.pnlProps.Controls.Add(Me.lblLanguages, 0, 36)
    Me.pnlProps.Controls.Add(Me.txtLanguages, 1, 36)
    Me.pnlProps.Controls.Add(Me.lblSPBuild, 0, 16)
    Me.pnlProps.Controls.Add(Me.lblSPLevel, 0, 14)
    Me.pnlProps.Controls.Add(Me.txtSPBuild, 1, 16)
    Me.pnlProps.Controls.Add(Me.txtSPLevel, 1, 14)
    Me.pnlProps.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProps.Location = New System.Drawing.Point(0, 0)
    Me.pnlProps.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlProps.Name = "pnlProps"
    Me.pnlProps.RowCount = 38
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.777627!))
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProps.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00272!))
    Me.pnlProps.Size = New System.Drawing.Size(313, 491)
    Me.pnlProps.TabIndex = 0
    '
    'lblIndex
    '
    Me.lblIndex.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblIndex.AutoSize = True
    Me.lblIndex.Location = New System.Drawing.Point(3, 5)
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
    Me.txtIndex.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtIndex.Name = "txtIndex"
    Me.txtIndex.ReadOnly = True
    Me.txtIndex.Size = New System.Drawing.Size(200, 20)
    Me.txtIndex.TabIndex = 1
    '
    'lblName
    '
    Me.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblName.AutoSize = True
    Me.lblName.Location = New System.Drawing.Point(3, 29)
    Me.lblName.Name = "lblName"
    Me.lblName.Size = New System.Drawing.Size(38, 13)
    Me.lblName.TabIndex = 2
    Me.lblName.Text = "&Name:"
    '
    'txtName
    '
    Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtName.Location = New System.Drawing.Point(111, 26)
    Me.txtName.Margin = New System.Windows.Forms.Padding(2)
    Me.txtName.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtName.Name = "txtName"
    Me.txtName.Size = New System.Drawing.Size(200, 20)
    Me.txtName.TabIndex = 3
    '
    'lblDesc
    '
    Me.lblDesc.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDesc.AutoSize = True
    Me.lblDesc.Location = New System.Drawing.Point(3, 53)
    Me.lblDesc.Name = "lblDesc"
    Me.lblDesc.Size = New System.Drawing.Size(63, 13)
    Me.lblDesc.TabIndex = 4
    Me.lblDesc.Text = "Description:"
    '
    'txtDesc
    '
    Me.txtDesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDesc.Location = New System.Drawing.Point(111, 50)
    Me.txtDesc.Margin = New System.Windows.Forms.Padding(2)
    Me.txtDesc.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtDesc.Name = "txtDesc"
    Me.txtDesc.ReadOnly = True
    Me.txtDesc.Size = New System.Drawing.Size(200, 20)
    Me.txtDesc.TabIndex = 5
    '
    'lblSize
    '
    Me.lblSize.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSize.AutoSize = True
    Me.lblSize.Location = New System.Drawing.Point(3, 77)
    Me.lblSize.Name = "lblSize"
    Me.lblSize.Size = New System.Drawing.Size(30, 13)
    Me.lblSize.TabIndex = 6
    Me.lblSize.Text = "Size:"
    '
    'txtSize
    '
    Me.txtSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSize.Location = New System.Drawing.Point(111, 74)
    Me.txtSize.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSize.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtSize.Name = "txtSize"
    Me.txtSize.ReadOnly = True
    Me.txtSize.Size = New System.Drawing.Size(200, 20)
    Me.txtSize.TabIndex = 7
    '
    'lblArchitecture
    '
    Me.lblArchitecture.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblArchitecture.AutoSize = True
    Me.lblArchitecture.Location = New System.Drawing.Point(3, 101)
    Me.lblArchitecture.Name = "lblArchitecture"
    Me.lblArchitecture.Size = New System.Drawing.Size(67, 13)
    Me.lblArchitecture.TabIndex = 8
    Me.lblArchitecture.Text = "Architecture:"
    '
    'txtArchitecture
    '
    Me.txtArchitecture.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtArchitecture.Location = New System.Drawing.Point(111, 98)
    Me.txtArchitecture.Margin = New System.Windows.Forms.Padding(2)
    Me.txtArchitecture.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtArchitecture.Name = "txtArchitecture"
    Me.txtArchitecture.ReadOnly = True
    Me.txtArchitecture.Size = New System.Drawing.Size(200, 20)
    Me.txtArchitecture.TabIndex = 9
    '
    'lblHAL
    '
    Me.lblHAL.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHAL.AutoSize = True
    Me.lblHAL.Location = New System.Drawing.Point(3, 125)
    Me.lblHAL.Name = "lblHAL"
    Me.lblHAL.Size = New System.Drawing.Size(31, 13)
    Me.lblHAL.TabIndex = 10
    Me.lblHAL.Text = "HAL:"
    '
    'txtHAL
    '
    Me.txtHAL.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHAL.Location = New System.Drawing.Point(111, 122)
    Me.txtHAL.Margin = New System.Windows.Forms.Padding(2)
    Me.txtHAL.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtHAL.Name = "txtHAL"
    Me.txtHAL.ReadOnly = True
    Me.txtHAL.Size = New System.Drawing.Size(200, 20)
    Me.txtHAL.TabIndex = 11
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblVersion.AutoSize = True
    Me.lblVersion.Location = New System.Drawing.Point(3, 149)
    Me.lblVersion.Name = "lblVersion"
    Me.lblVersion.Size = New System.Drawing.Size(45, 13)
    Me.lblVersion.TabIndex = 12
    Me.lblVersion.Text = "Version:"
    '
    'txtVersion
    '
    Me.txtVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVersion.Location = New System.Drawing.Point(111, 146)
    Me.txtVersion.Margin = New System.Windows.Forms.Padding(2)
    Me.txtVersion.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtVersion.Name = "txtVersion"
    Me.txtVersion.ReadOnly = True
    Me.txtVersion.Size = New System.Drawing.Size(200, 20)
    Me.txtVersion.TabIndex = 13
    '
    'lblEdition
    '
    Me.lblEdition.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblEdition.AutoSize = True
    Me.lblEdition.Location = New System.Drawing.Point(3, 221)
    Me.lblEdition.Name = "lblEdition"
    Me.lblEdition.Size = New System.Drawing.Size(42, 13)
    Me.lblEdition.TabIndex = 18
    Me.lblEdition.Text = "Edition:"
    '
    'txtEdition
    '
    Me.txtEdition.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtEdition.Location = New System.Drawing.Point(111, 218)
    Me.txtEdition.Margin = New System.Windows.Forms.Padding(2)
    Me.txtEdition.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtEdition.Name = "txtEdition"
    Me.txtEdition.ReadOnly = True
    Me.txtEdition.Size = New System.Drawing.Size(200, 20)
    Me.txtEdition.TabIndex = 19
    '
    'lblInstallation
    '
    Me.lblInstallation.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInstallation.AutoSize = True
    Me.lblInstallation.Location = New System.Drawing.Point(3, 245)
    Me.lblInstallation.Name = "lblInstallation"
    Me.lblInstallation.Size = New System.Drawing.Size(60, 13)
    Me.lblInstallation.TabIndex = 20
    Me.lblInstallation.Text = "Installation:"
    '
    'txtInstallation
    '
    Me.txtInstallation.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInstallation.Location = New System.Drawing.Point(111, 242)
    Me.txtInstallation.Margin = New System.Windows.Forms.Padding(2)
    Me.txtInstallation.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtInstallation.Name = "txtInstallation"
    Me.txtInstallation.ReadOnly = True
    Me.txtInstallation.Size = New System.Drawing.Size(200, 20)
    Me.txtInstallation.TabIndex = 21
    '
    'lblProductType
    '
    Me.lblProductType.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProductType.AutoSize = True
    Me.lblProductType.Location = New System.Drawing.Point(3, 269)
    Me.lblProductType.Name = "lblProductType"
    Me.lblProductType.Size = New System.Drawing.Size(74, 13)
    Me.lblProductType.TabIndex = 22
    Me.lblProductType.Text = "Product Type:"
    '
    'txtProductType
    '
    Me.txtProductType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProductType.Location = New System.Drawing.Point(111, 266)
    Me.txtProductType.Margin = New System.Windows.Forms.Padding(2)
    Me.txtProductType.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtProductType.Name = "txtProductType"
    Me.txtProductType.ReadOnly = True
    Me.txtProductType.Size = New System.Drawing.Size(200, 20)
    Me.txtProductType.TabIndex = 23
    '
    'lblProductSuite
    '
    Me.lblProductSuite.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProductSuite.AutoSize = True
    Me.lblProductSuite.Location = New System.Drawing.Point(3, 293)
    Me.lblProductSuite.Name = "lblProductSuite"
    Me.lblProductSuite.Size = New System.Drawing.Size(74, 13)
    Me.lblProductSuite.TabIndex = 24
    Me.lblProductSuite.Text = "Product Suite:"
    '
    'txtProductSuite
    '
    Me.txtProductSuite.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProductSuite.Location = New System.Drawing.Point(111, 290)
    Me.txtProductSuite.Margin = New System.Windows.Forms.Padding(2)
    Me.txtProductSuite.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtProductSuite.Name = "txtProductSuite"
    Me.txtProductSuite.ReadOnly = True
    Me.txtProductSuite.Size = New System.Drawing.Size(200, 20)
    Me.txtProductSuite.TabIndex = 25
    '
    'lblSystemRoot
    '
    Me.lblSystemRoot.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSystemRoot.AutoSize = True
    Me.lblSystemRoot.Location = New System.Drawing.Point(3, 317)
    Me.lblSystemRoot.Name = "lblSystemRoot"
    Me.lblSystemRoot.Size = New System.Drawing.Size(70, 13)
    Me.lblSystemRoot.TabIndex = 26
    Me.lblSystemRoot.Text = "System Root:"
    '
    'txtSystemRoot
    '
    Me.txtSystemRoot.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSystemRoot.Location = New System.Drawing.Point(111, 314)
    Me.txtSystemRoot.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSystemRoot.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtSystemRoot.Name = "txtSystemRoot"
    Me.txtSystemRoot.ReadOnly = True
    Me.txtSystemRoot.Size = New System.Drawing.Size(200, 20)
    Me.txtSystemRoot.TabIndex = 27
    '
    'lblFiles
    '
    Me.lblFiles.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblFiles.AutoSize = True
    Me.lblFiles.Location = New System.Drawing.Point(3, 341)
    Me.lblFiles.Name = "lblFiles"
    Me.lblFiles.Size = New System.Drawing.Size(57, 13)
    Me.lblFiles.TabIndex = 28
    Me.lblFiles.Text = "File Count:"
    '
    'txtFiles
    '
    Me.txtFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFiles.Location = New System.Drawing.Point(111, 338)
    Me.txtFiles.Margin = New System.Windows.Forms.Padding(2)
    Me.txtFiles.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtFiles.Name = "txtFiles"
    Me.txtFiles.ReadOnly = True
    Me.txtFiles.Size = New System.Drawing.Size(200, 20)
    Me.txtFiles.TabIndex = 29
    '
    'lblDirectories
    '
    Me.lblDirectories.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDirectories.AutoSize = True
    Me.lblDirectories.Location = New System.Drawing.Point(3, 365)
    Me.lblDirectories.Name = "lblDirectories"
    Me.lblDirectories.Size = New System.Drawing.Size(83, 13)
    Me.lblDirectories.TabIndex = 30
    Me.lblDirectories.Text = "Directory Count:"
    '
    'txtDirectories
    '
    Me.txtDirectories.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtDirectories.Location = New System.Drawing.Point(111, 362)
    Me.txtDirectories.Margin = New System.Windows.Forms.Padding(2)
    Me.txtDirectories.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtDirectories.Name = "txtDirectories"
    Me.txtDirectories.ReadOnly = True
    Me.txtDirectories.Size = New System.Drawing.Size(200, 20)
    Me.txtDirectories.TabIndex = 31
    '
    'lblCreated
    '
    Me.lblCreated.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblCreated.AutoSize = True
    Me.lblCreated.Location = New System.Drawing.Point(3, 389)
    Me.lblCreated.Name = "lblCreated"
    Me.lblCreated.Size = New System.Drawing.Size(47, 13)
    Me.lblCreated.TabIndex = 32
    Me.lblCreated.Text = "Created:"
    '
    'txtCreated
    '
    Me.txtCreated.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtCreated.Location = New System.Drawing.Point(111, 386)
    Me.txtCreated.Margin = New System.Windows.Forms.Padding(2)
    Me.txtCreated.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtCreated.Name = "txtCreated"
    Me.txtCreated.ReadOnly = True
    Me.txtCreated.Size = New System.Drawing.Size(200, 20)
    Me.txtCreated.TabIndex = 33
    '
    'lblModified
    '
    Me.lblModified.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblModified.AutoSize = True
    Me.lblModified.Location = New System.Drawing.Point(3, 413)
    Me.lblModified.Name = "lblModified"
    Me.lblModified.Size = New System.Drawing.Size(50, 13)
    Me.lblModified.TabIndex = 34
    Me.lblModified.Text = "Modified:"
    '
    'txtModified
    '
    Me.txtModified.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtModified.Location = New System.Drawing.Point(111, 410)
    Me.txtModified.Margin = New System.Windows.Forms.Padding(2)
    Me.txtModified.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtModified.Name = "txtModified"
    Me.txtModified.ReadOnly = True
    Me.txtModified.Size = New System.Drawing.Size(200, 20)
    Me.txtModified.TabIndex = 35
    '
    'lblLanguages
    '
    Me.lblLanguages.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblLanguages.AutoSize = True
    Me.lblLanguages.Location = New System.Drawing.Point(3, 438)
    Me.lblLanguages.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblLanguages.Name = "lblLanguages"
    Me.lblLanguages.Size = New System.Drawing.Size(63, 13)
    Me.lblLanguages.TabIndex = 36
    Me.lblLanguages.Text = "&Languages:"
    '
    'txtLanguages
    '
    Me.txtLanguages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtLanguages.Location = New System.Drawing.Point(111, 434)
    Me.txtLanguages.Margin = New System.Windows.Forms.Padding(2)
    Me.txtLanguages.MinimumSize = New System.Drawing.Size(200, 55)
    Me.txtLanguages.Multiline = True
    Me.txtLanguages.Name = "txtLanguages"
    Me.txtLanguages.ReadOnly = True
    Me.pnlProps.SetRowSpan(Me.txtLanguages, 2)
    Me.txtLanguages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtLanguages.Size = New System.Drawing.Size(200, 55)
    Me.txtLanguages.TabIndex = 37
    '
    'lblSPBuild
    '
    Me.lblSPBuild.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSPBuild.AutoSize = True
    Me.lblSPBuild.Location = New System.Drawing.Point(3, 197)
    Me.lblSPBuild.Name = "lblSPBuild"
    Me.lblSPBuild.Size = New System.Drawing.Size(100, 13)
    Me.lblSPBuild.TabIndex = 16
    Me.lblSPBuild.Text = "Service Pack Build:"
    '
    'lblSPLevel
    '
    Me.lblSPLevel.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSPLevel.AutoSize = True
    Me.lblSPLevel.Location = New System.Drawing.Point(3, 173)
    Me.lblSPLevel.Name = "lblSPLevel"
    Me.lblSPLevel.Size = New System.Drawing.Size(103, 13)
    Me.lblSPLevel.TabIndex = 14
    Me.lblSPLevel.Text = "Service Pack Level:"
    '
    'txtSPBuild
    '
    Me.txtSPBuild.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSPBuild.Location = New System.Drawing.Point(111, 194)
    Me.txtSPBuild.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSPBuild.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtSPBuild.Name = "txtSPBuild"
    Me.txtSPBuild.ReadOnly = True
    Me.txtSPBuild.Size = New System.Drawing.Size(200, 20)
    Me.txtSPBuild.TabIndex = 17
    '
    'txtSPLevel
    '
    Me.txtSPLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSPLevel.Location = New System.Drawing.Point(111, 170)
    Me.txtSPLevel.Margin = New System.Windows.Forms.Padding(2)
    Me.txtSPLevel.MinimumSize = New System.Drawing.Size(200, 4)
    Me.txtSPLevel.Name = "txtSPLevel"
    Me.txtSPLevel.ReadOnly = True
    Me.txtSPLevel.Size = New System.Drawing.Size(200, 20)
    Me.txtSPLevel.TabIndex = 15
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(447, 491)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(172, 31)
    Me.pnlButtons.TabIndex = 2
    '
    'cmdClose
    '
    Me.cmdClose.AutoSize = True
    Me.cmdClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(89, 3)
    Me.cmdClose.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Padding = New System.Windows.Forms.Padding(1)
    Me.cmdClose.Size = New System.Drawing.Size(80, 25)
    Me.cmdClose.TabIndex = 1
    Me.cmdClose.Text = "&Cancel"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.AutoSize = True
    Me.cmdSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Padding = New System.Windows.Forms.Padding(1)
    Me.cmdSave.Size = New System.Drawing.Size(80, 25)
    Me.cmdSave.TabIndex = 0
    Me.cmdSave.Text = "OK"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'imlUpdates
    '
    Me.imlUpdates.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
    Me.imlUpdates.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlUpdates.TransparentColor = System.Drawing.Color.Transparent
    '
    'imlFeatures
    '
    Me.imlFeatures.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
    Me.imlFeatures.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlFeatures.TransparentColor = System.Drawing.Color.Transparent
    '
    'cmdLoadFeatures
    '
    Me.cmdLoadFeatures.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdLoadFeatures.AutoSize = True
    Me.cmdLoadFeatures.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.cmdLoadFeatures, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.1_Windo" & _
        "ws_Features.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdLoadFeatures, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdLoadFeatures.Image = Global.Slips7ream.My.Resources.Resources.load_feature
    Me.cmdLoadFeatures.Location = New System.Drawing.Point(68, 135)
    Me.cmdLoadFeatures.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdLoadFeatures.Name = "cmdLoadFeatures"
    Me.cmdLoadFeatures.Padding = New System.Windows.Forms.Padding(1)
    Me.helpS7M.SetShowHelp(Me.cmdLoadFeatures, True)
    Me.cmdLoadFeatures.Size = New System.Drawing.Size(169, 25)
    Me.cmdLoadFeatures.TabIndex = 2
    Me.cmdLoadFeatures.Text = "Load Windows Features List"
    Me.cmdLoadFeatures.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
    Me.cmdLoadFeatures.UseVisualStyleBackColor = True
    '
    'cmdLoadUpdates
    '
    Me.cmdLoadUpdates.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdLoadUpdates.AutoSize = True
    Me.cmdLoadUpdates.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.cmdLoadUpdates, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.2_Integ" & _
        "rated_Windows_Updates.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdLoadUpdates, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdLoadUpdates.Image = Global.Slips7ream.My.Resources.Resources.load_update
    Me.cmdLoadUpdates.Location = New System.Drawing.Point(43, 298)
    Me.cmdLoadUpdates.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdLoadUpdates.Name = "cmdLoadUpdates"
    Me.cmdLoadUpdates.Padding = New System.Windows.Forms.Padding(1)
    Me.helpS7M.SetShowHelp(Me.cmdLoadUpdates, True)
    Me.cmdLoadUpdates.Size = New System.Drawing.Size(219, 25)
    Me.cmdLoadUpdates.TabIndex = 5
    Me.cmdLoadUpdates.Text = "Load Integrated Windows Updates List"
    Me.cmdLoadUpdates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
    Me.cmdLoadUpdates.UseVisualStyleBackColor = True
    '
    'colFeatureDisplayName
    '
    Me.colFeatureDisplayName.Text = "Feature Name"
    Me.colFeatureDisplayName.Width = 110
    '
    'colFeatureDescription
    '
    Me.colFeatureDescription.Text = "Description"
    Me.colFeatureDescription.Width = 100
    '
    'colFeatureRestart
    '
    Me.colFeatureRestart.Text = "Restart Required"
    Me.colFeatureRestart.Width = 100
    '
    'pnlMain
    '
    Me.pnlMain.AutoSize = True
    Me.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlMain.ColumnCount = 2
    Me.pnlMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMain.Controls.Add(Me.pnlLists, 1, 0)
    Me.pnlMain.Controls.Add(Me.pnlProps, 0, 0)
    Me.pnlMain.Controls.Add(Me.pnlButtons, 1, 1)
    Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlMain.Location = New System.Drawing.Point(0, 0)
    Me.pnlMain.Name = "pnlMain"
    Me.pnlMain.RowCount = 2
    Me.pnlMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlMain.Size = New System.Drawing.Size(619, 522)
    Me.pnlMain.TabIndex = 0
    '
    'pnlLists
    '
    Me.pnlLists.AutoSize = True
    Me.pnlLists.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlLists.ColumnCount = 1
    Me.pnlLists.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlLists.Controls.Add(Me.expFeatures, 0, 0)
    Me.pnlLists.Controls.Add(Me.tvFeatures, 0, 1)
    Me.pnlLists.Controls.Add(Me.cmdLoadFeatures, 0, 2)
    Me.pnlLists.Controls.Add(Me.expUpdates, 0, 3)
    Me.pnlLists.Controls.Add(Me.lvUpdates, 0, 4)
    Me.pnlLists.Controls.Add(Me.cmdLoadUpdates, 0, 5)
    Me.pnlLists.Controls.Add(Me.expDrivers, 0, 6)
    Me.pnlLists.Controls.Add(Me.cmdLoadDrivers, 0, 8)
    Me.pnlLists.Controls.Add(Me.pnlDrivers, 0, 7)
    Me.pnlLists.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlLists.Location = New System.Drawing.Point(313, 0)
    Me.pnlLists.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlLists.Name = "pnlLists"
    Me.pnlLists.RowCount = 9
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLists.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlLists.Size = New System.Drawing.Size(306, 491)
    Me.pnlLists.TabIndex = 1
    '
    'expFeatures
    '
    Me.expFeatures.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.expFeatures.AutoSize = True
    Me.expFeatures.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.expFeatures, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.1_Windo" & _
        "ws_Features.htm")
    Me.helpS7M.SetHelpNavigator(Me.expFeatures, System.Windows.Forms.HelpNavigator.Topic)
    Me.expFeatures.Location = New System.Drawing.Point(3, 6)
    Me.expFeatures.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.expFeatures.Name = "expFeatures"
    Me.helpS7M.SetShowHelp(Me.expFeatures, True)
    Me.expFeatures.Size = New System.Drawing.Size(120, 19)
    Me.expFeatures.TabIndex = 0
    Me.expFeatures.Text = "Windows &Features"
    '
    'tvFeatures
    '
    Me.tvFeatures.BackColor = System.Drawing.SystemColors.Window
    Me.tvFeatures.CheckBoxes = True
    Me.tvFeatures.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tvFeatures.FullRowSelect = True
    Me.helpS7M.SetHelpKeyword(Me.tvFeatures, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.1_Windo" & _
        "ws_Features.htm")
    Me.helpS7M.SetHelpNavigator(Me.tvFeatures, System.Windows.Forms.HelpNavigator.Topic)
    Me.tvFeatures.HideSelection = False
    Me.tvFeatures.ImageIndex = 0
    Me.tvFeatures.ImageList = Me.imlFeatures
    Me.tvFeatures.Location = New System.Drawing.Point(3, 34)
    Me.tvFeatures.MinimumSize = New System.Drawing.Size(300, 4)
    Me.tvFeatures.Name = "tvFeatures"
    Me.tvFeatures.ReadOnly = False
    Me.tvFeatures.SelectedImageIndex = 0
    Me.helpS7M.SetShowHelp(Me.tvFeatures, True)
    Me.tvFeatures.Size = New System.Drawing.Size(300, 95)
    Me.tvFeatures.TabIndex = 1
    Me.tvFeatures.TooltipTitles = True
    '
    'expUpdates
    '
    Me.expUpdates.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.expUpdates.AutoSize = True
    Me.expUpdates.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.expUpdates, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.2_Integ" & _
        "rated_Windows_Updates.htm")
    Me.helpS7M.SetHelpNavigator(Me.expUpdates, System.Windows.Forms.HelpNavigator.Topic)
    Me.expUpdates.Location = New System.Drawing.Point(3, 169)
    Me.expUpdates.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.expUpdates.Name = "expUpdates"
    Me.helpS7M.SetShowHelp(Me.expUpdates, True)
    Me.expUpdates.Size = New System.Drawing.Size(170, 19)
    Me.expUpdates.TabIndex = 3
    Me.expUpdates.Text = "Integrated Windows &Updates"
    '
    'lvUpdates
    '
    Me.lvUpdates.BackColor = System.Drawing.SystemColors.Window
    Me.lvUpdates.CheckBoxes = True
    Me.lvUpdates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPackage, Me.colVer})
    Me.lvUpdates.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvUpdates.FullRowSelect = True
    Me.lvUpdates.FullRowTooltip = True
    Me.lvUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.helpS7M.SetHelpKeyword(Me.lvUpdates, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.2_Integ" & _
        "rated_Windows_Updates.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvUpdates, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvUpdates.HideSelection = False
    Me.lvUpdates.Location = New System.Drawing.Point(3, 197)
    Me.lvUpdates.MinimumSize = New System.Drawing.Size(300, 4)
    Me.lvUpdates.MultiSelect = False
    Me.lvUpdates.Name = "lvUpdates"
    Me.lvUpdates.ReadOnly = False
    Me.helpS7M.SetShowHelp(Me.lvUpdates, True)
    Me.lvUpdates.Size = New System.Drawing.Size(300, 95)
    Me.lvUpdates.SmallImageList = Me.imlUpdates
    Me.lvUpdates.TabIndex = 4
    Me.lvUpdates.TooltipTitles = True
    Me.lvUpdates.UseCompatibleStateImageBehavior = False
    Me.lvUpdates.View = System.Windows.Forms.View.Details
    '
    'colPackage
    '
    Me.colPackage.Text = "Package Name"
    Me.colPackage.Width = 200
    '
    'colVer
    '
    Me.colVer.Text = "Version"
    Me.colVer.Width = 92
    '
    'expDrivers
    '
    Me.expDrivers.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.expDrivers.AutoSize = True
    Me.expDrivers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.expDrivers, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.3_Integ" & _
        "rated_Drivers.htm")
    Me.helpS7M.SetHelpNavigator(Me.expDrivers, System.Windows.Forms.HelpNavigator.Topic)
    Me.expDrivers.Location = New System.Drawing.Point(3, 332)
    Me.expDrivers.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.expDrivers.Name = "expDrivers"
    Me.helpS7M.SetShowHelp(Me.expDrivers, True)
    Me.expDrivers.Size = New System.Drawing.Size(116, 19)
    Me.expDrivers.TabIndex = 6
    Me.expDrivers.Text = "Integrated Drivers"
    '
    'cmdLoadDrivers
    '
    Me.cmdLoadDrivers.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdLoadDrivers.AutoSize = True
    Me.cmdLoadDrivers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.cmdLoadDrivers, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.3_Integ" & _
        "rated_Drivers.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdLoadDrivers, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdLoadDrivers.Image = Global.Slips7ream.My.Resources.Resources.load_driver
    Me.cmdLoadDrivers.Location = New System.Drawing.Point(70, 462)
    Me.cmdLoadDrivers.MinimumSize = New System.Drawing.Size(80, 25)
    Me.cmdLoadDrivers.Name = "cmdLoadDrivers"
    Me.cmdLoadDrivers.Padding = New System.Windows.Forms.Padding(1)
    Me.helpS7M.SetShowHelp(Me.cmdLoadDrivers, True)
    Me.cmdLoadDrivers.Size = New System.Drawing.Size(165, 25)
    Me.cmdLoadDrivers.TabIndex = 8
    Me.cmdLoadDrivers.Text = "Load Integrated Drivers List"
    Me.cmdLoadDrivers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
    Me.cmdLoadDrivers.UseVisualStyleBackColor = True
    '
    'pnlDrivers
    '
    Me.pnlDrivers.ColumnCount = 3
    Me.pnlDrivers.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDrivers.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
    Me.pnlDrivers.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
    Me.pnlDrivers.Controls.Add(Me.lvDriverClass, 0, 1)
    Me.pnlDrivers.Controls.Add(Me.lvDriverProvider, 1, 1)
    Me.pnlDrivers.Controls.Add(Me.lvDriverINF, 2, 1)
    Me.pnlDrivers.Controls.Add(Me.lblDriverClass, 0, 0)
    Me.pnlDrivers.Controls.Add(Me.lblDriverProvider, 1, 0)
    Me.pnlDrivers.Controls.Add(Me.lblDriverINF, 2, 0)
    Me.pnlDrivers.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDrivers.Location = New System.Drawing.Point(3, 360)
    Me.pnlDrivers.Name = "pnlDrivers"
    Me.pnlDrivers.RowCount = 2
    Me.pnlDrivers.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDrivers.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDrivers.Size = New System.Drawing.Size(300, 95)
    Me.pnlDrivers.TabIndex = 7
    '
    'lvDriverClass
    '
    Me.lvDriverClass.BackColor = System.Drawing.SystemColors.Window
    Me.lvDriverClass.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvcGroup})
    Me.lvDriverClass.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvDriverClass.FullRowSelect = True
    Me.lvDriverClass.FullRowTooltip = True
    Me.lvDriverClass.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
    Me.helpS7M.SetHelpKeyword(Me.lvDriverClass, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.3_Integ" & _
        "rated_Drivers.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvDriverClass, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvDriverClass.HideSelection = False
    Me.lvDriverClass.Location = New System.Drawing.Point(3, 16)
    Me.lvDriverClass.MultiSelect = False
    Me.lvDriverClass.Name = "lvDriverClass"
    Me.lvDriverClass.ReadOnly = False
    Me.helpS7M.SetShowHelp(Me.lvDriverClass, True)
    Me.lvDriverClass.Size = New System.Drawing.Size(93, 76)
    Me.lvDriverClass.SmallImageList = Me.imlDriverClass
    Me.lvDriverClass.TabIndex = 1
    Me.lvDriverClass.TooltipTitles = True
    Me.lvDriverClass.UseCompatibleStateImageBehavior = False
    Me.lvDriverClass.View = System.Windows.Forms.View.Details
    '
    'lvcGroup
    '
    Me.lvcGroup.Text = "Group"
    '
    'imlDriverClass
    '
    Me.imlDriverClass.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
    Me.imlDriverClass.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlDriverClass.TransparentColor = System.Drawing.Color.Transparent
    '
    'lvDriverProvider
    '
    Me.lvDriverProvider.BackColor = System.Drawing.SystemColors.Window
    Me.lvDriverProvider.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvcCompany})
    Me.lvDriverProvider.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvDriverProvider.FullRowSelect = True
    Me.lvDriverProvider.FullRowTooltip = True
    Me.lvDriverProvider.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
    Me.helpS7M.SetHelpKeyword(Me.lvDriverProvider, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.3_Integ" & _
        "rated_Drivers.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvDriverProvider, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvDriverProvider.HideSelection = False
    Me.lvDriverProvider.Location = New System.Drawing.Point(102, 16)
    Me.lvDriverProvider.MultiSelect = False
    Me.lvDriverProvider.Name = "lvDriverProvider"
    Me.lvDriverProvider.ReadOnly = False
    Me.helpS7M.SetShowHelp(Me.lvDriverProvider, True)
    Me.lvDriverProvider.Size = New System.Drawing.Size(94, 76)
    Me.lvDriverProvider.SmallImageList = Me.imlDriverCompany
    Me.lvDriverProvider.TabIndex = 3
    Me.lvDriverProvider.TooltipTitles = True
    Me.lvDriverProvider.UseCompatibleStateImageBehavior = False
    Me.lvDriverProvider.View = System.Windows.Forms.View.Details
    '
    'lvcCompany
    '
    Me.lvcCompany.Text = "Company"
    '
    'imlDriverCompany
    '
    Me.imlDriverCompany.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
    Me.imlDriverCompany.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlDriverCompany.TransparentColor = System.Drawing.Color.Transparent
    '
    'lvDriverINF
    '
    Me.lvDriverINF.BackColor = System.Drawing.SystemColors.Window
    Me.lvDriverINF.CheckBoxes = True
    Me.lvDriverINF.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvcName, Me.lvcVer})
    Me.lvDriverINF.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvDriverINF.FullRowSelect = True
    Me.lvDriverINF.FullRowTooltip = True
    Me.lvDriverINF.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.helpS7M.SetHelpKeyword(Me.lvDriverINF, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.3_Integ" & _
        "rated_Drivers.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvDriverINF, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvDriverINF.HideSelection = False
    Me.lvDriverINF.Location = New System.Drawing.Point(202, 16)
    Me.lvDriverINF.MultiSelect = False
    Me.lvDriverINF.Name = "lvDriverINF"
    Me.lvDriverINF.ReadOnly = False
    Me.helpS7M.SetShowHelp(Me.lvDriverINF, True)
    Me.lvDriverINF.Size = New System.Drawing.Size(95, 76)
    Me.lvDriverINF.SmallImageList = Me.imlDriverINF
    Me.lvDriverINF.TabIndex = 5
    Me.lvDriverINF.TooltipTitles = True
    Me.lvDriverINF.UseCompatibleStateImageBehavior = False
    Me.lvDriverINF.View = System.Windows.Forms.View.Details
    '
    'lvcName
    '
    Me.lvcName.Text = "Name"
    Me.lvcName.Width = 65
    '
    'lvcVer
    '
    Me.lvcVer.Text = "Version"
    Me.lvcVer.Width = 25
    '
    'imlDriverINF
    '
    Me.imlDriverINF.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
    Me.imlDriverINF.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlDriverINF.TransparentColor = System.Drawing.Color.Transparent
    '
    'lblDriverClass
    '
    Me.lblDriverClass.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDriverClass.AutoSize = True
    Me.lblDriverClass.Location = New System.Drawing.Point(3, 0)
    Me.lblDriverClass.Name = "lblDriverClass"
    Me.lblDriverClass.Size = New System.Drawing.Size(66, 13)
    Me.lblDriverClass.TabIndex = 0
    Me.lblDriverClass.Text = "Driver Class:"
    '
    'lblDriverProvider
    '
    Me.lblDriverProvider.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDriverProvider.AutoSize = True
    Me.lblDriverProvider.Location = New System.Drawing.Point(102, 0)
    Me.lblDriverProvider.Name = "lblDriverProvider"
    Me.lblDriverProvider.Size = New System.Drawing.Size(49, 13)
    Me.lblDriverProvider.TabIndex = 2
    Me.lblDriverProvider.Text = "Provider:"
    '
    'lblDriverINF
    '
    Me.lblDriverINF.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDriverINF.AutoSize = True
    Me.lblDriverINF.Location = New System.Drawing.Point(202, 0)
    Me.lblDriverINF.Name = "lblDriverINF"
    Me.lblDriverINF.Size = New System.Drawing.Size(46, 13)
    Me.lblDriverINF.TabIndex = 4
    Me.lblDriverINF.Text = "INF File:"
    '
    'mnuFeatures
    '
    Me.mnuFeatures.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFeatureEnabled, Me.mnuFeatureSpace, Me.mnuFeatureExpandAll, Me.mnuFeatureExpand, Me.mnuFeatureCollapse, Me.mnuFeatureCollapseAll})
    '
    'mnuFeatureEnabled
    '
    Me.mnuFeatureEnabled.Index = 0
    Me.mnuFeatureEnabled.Text = "Enabled"
    '
    'mnuFeatureSpace
    '
    Me.mnuFeatureSpace.Index = 1
    Me.mnuFeatureSpace.Text = "-"
    '
    'mnuFeatureExpandAll
    '
    Me.mnuFeatureExpandAll.Index = 2
    Me.mnuFeatureExpandAll.Text = "Expand All Trees"
    '
    'mnuFeatureExpand
    '
    Me.mnuFeatureExpand.Index = 3
    Me.mnuFeatureExpand.Text = "Expand Tree"
    '
    'mnuFeatureCollapse
    '
    Me.mnuFeatureCollapse.Index = 4
    Me.mnuFeatureCollapse.Text = "Collapse Tree"
    '
    'mnuFeatureCollapseAll
    '
    Me.mnuFeatureCollapseAll.Index = 5
    Me.mnuFeatureCollapseAll.Text = "Collapse All Trees"
    '
    'mnuUpdates
    '
    Me.mnuUpdates.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuUpdateInclude})
    '
    'mnuUpdateInclude
    '
    Me.mnuUpdateInclude.Checked = True
    Me.mnuUpdateInclude.Index = 0
    Me.mnuUpdateInclude.Text = "Included in Image"
    '
    'helpS7M
    '
    Me.helpS7M.HelpNamespace = "S7M.chm"
    '
    'frmPackageProps
    '
    Me.AcceptButton = Me.cmdSave
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(619, 522)
    Me.Controls.Add(Me.pnlMain)
    Me.helpS7M.SetHelpKeyword(Me, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.2_Package_Properties/1.3.2.0_Packa" & _
        "ge_Properties.htm")
    Me.helpS7M.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MinimumSize = New System.Drawing.Size(635, 560)
    Me.Name = "frmPackageProps"
    Me.helpS7M.SetShowHelp(Me, True)
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Image Package Properties"
    Me.pnlProps.ResumeLayout(False)
    Me.pnlProps.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    Me.pnlMain.ResumeLayout(False)
    Me.pnlMain.PerformLayout()
    Me.pnlLists.ResumeLayout(False)
    Me.pnlLists.PerformLayout()
    Me.pnlDrivers.ResumeLayout(False)
    Me.pnlDrivers.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

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
  Friend WithEvents lvUpdates As ListViewEx
  Friend WithEvents colPackage As System.Windows.Forms.ColumnHeader
  Friend WithEvents colVer As System.Windows.Forms.ColumnHeader
  Friend WithEvents imlUpdates As System.Windows.Forms.ImageList
  Friend WithEvents tvFeatures As TreeViewEx
  Friend WithEvents colFeatureDisplayName As System.Windows.Forms.ColumnHeader
  Friend WithEvents colFeatureDescription As System.Windows.Forms.ColumnHeader
  Friend WithEvents colFeatureRestart As System.Windows.Forms.ColumnHeader
  Friend WithEvents expFeatures As Slips7ream.Expander
  Friend WithEvents expUpdates As Slips7ream.Expander
  Friend WithEvents cmdLoadFeatures As System.Windows.Forms.Button
  Friend WithEvents cmdLoadUpdates As System.Windows.Forms.Button
  Friend WithEvents imlFeatures As System.Windows.Forms.ImageList
  Friend WithEvents pnlMain As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlLists As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents mnuFeatures As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuUpdates As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuFeatureEnabled As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFeatureSpace As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFeatureExpand As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFeatureCollapse As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateInclude As System.Windows.Forms.MenuItem
  Friend WithEvents expDrivers As Slips7ream.Expander
  Friend WithEvents cmdLoadDrivers As System.Windows.Forms.Button
  Friend WithEvents pnlDrivers As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lvDriverClass As Slips7ream.ListViewEx
  Friend WithEvents lvDriverProvider As Slips7ream.ListViewEx
  Friend WithEvents lvDriverINF As Slips7ream.ListViewEx
  Friend WithEvents lblDriverClass As System.Windows.Forms.Label
  Friend WithEvents lblDriverProvider As System.Windows.Forms.Label
  Friend WithEvents lblDriverINF As System.Windows.Forms.Label
  Friend WithEvents lvcGroup As System.Windows.Forms.ColumnHeader
  Friend WithEvents lvcCompany As System.Windows.Forms.ColumnHeader
  Friend WithEvents lvcName As System.Windows.Forms.ColumnHeader
  Friend WithEvents lvcVer As System.Windows.Forms.ColumnHeader
  Friend WithEvents imlDriverClass As System.Windows.Forms.ImageList
  Friend WithEvents imlDriverINF As System.Windows.Forms.ImageList
  Friend WithEvents imlDriverCompany As System.Windows.Forms.ImageList
  Friend WithEvents mnuFeatureExpandAll As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFeatureCollapseAll As System.Windows.Forms.MenuItem
  Friend WithEvents helpS7M As System.Windows.Forms.HelpProvider
End Class
