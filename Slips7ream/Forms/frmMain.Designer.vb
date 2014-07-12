<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
    Me.pnlSlips7ream = New System.Windows.Forms.TableLayoutPanel()
    Me.spltSlips7ream = New Slips7ream.SplitContainerEx()
    Me.pnlPackages = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlSP64 = New System.Windows.Forms.TableLayoutPanel()
    Me.txtSP64 = New System.Windows.Forms.TextBox()
    Me.cmdSP64 = New System.Windows.Forms.Button()
    Me.lblImages = New System.Windows.Forms.Label()
    Me.chkSP = New System.Windows.Forms.CheckBox()
    Me.lblSP64 = New System.Windows.Forms.Label()
    Me.lvImages = New System.Windows.Forms.ListView()
    Me.colIndex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.pnlSP = New System.Windows.Forms.TableLayoutPanel()
    Me.txtSP = New System.Windows.Forms.TextBox()
    Me.cmdSP = New System.Windows.Forms.Button()
    Me.pnlUpdates = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlMSU = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdAddMSU = New System.Windows.Forms.Button()
    Me.cmdClearMSU = New System.Windows.Forms.Button()
    Me.cmdRemMSU = New System.Windows.Forms.Button()
    Me.lblMSU = New System.Windows.Forms.Label()
    Me.lvMSU = New System.Windows.Forms.ListView()
    Me.colUpdate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.imlUpdates = New System.Windows.Forms.ImageList(Me.components)
    Me.lblWIM = New System.Windows.Forms.Label()
    Me.pnlWIM = New System.Windows.Forms.TableLayoutPanel()
    Me.txtWIM = New System.Windows.Forms.TextBox()
    Me.cmdWIM = New System.Windows.Forms.Button()
    Me.pnlBottom = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdBegin = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.lblActivity = New System.Windows.Forms.Label()
    Me.cmdConfig = New System.Windows.Forms.Button()
    Me.expOutput = New Slips7ream.Expander()
    Me.cmdOpenFolder = New System.Windows.Forms.Button()
    Me.chkISO = New System.Windows.Forms.CheckBox()
    Me.pnlISO = New System.Windows.Forms.TableLayoutPanel()
    Me.txtISO = New System.Windows.Forms.TextBox()
    Me.cmdISO = New System.Windows.Forms.Button()
    Me.pnlProgress = New System.Windows.Forms.TableLayoutPanel()
    Me.pbTotal = New System.Windows.Forms.ProgressBar()
    Me.pbIndividual = New System.Windows.Forms.ProgressBar()
    Me.txtOutput = New System.Windows.Forms.TextBox()
    Me.txtOutputError = New System.Windows.Forms.TextBox()
    Me.lblISOLabel = New System.Windows.Forms.Label()
    Me.txtISOLabel = New System.Windows.Forms.TextBox()
    Me.pnlControl = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPriority = New System.Windows.Forms.Label()
    Me.cmbPriority = New System.Windows.Forms.ComboBox()
    Me.lblCompletion = New System.Windows.Forms.Label()
    Me.cmbCompletion = New System.Windows.Forms.ComboBox()
    Me.pnlISOOptions = New System.Windows.Forms.TableLayoutPanel()
    Me.cmbLimitType = New System.Windows.Forms.ComboBox()
    Me.cmbISOFormat = New System.Windows.Forms.ComboBox()
    Me.chkUnlock = New System.Windows.Forms.CheckBox()
    Me.lblISOFS = New System.Windows.Forms.Label()
    Me.chkUEFI = New System.Windows.Forms.CheckBox()
    Me.cmbLimit = New System.Windows.Forms.ComboBox()
    Me.chkMerge = New System.Windows.Forms.CheckBox()
    Me.pnlMerge = New System.Windows.Forms.TableLayoutPanel()
    Me.txtMerge = New System.Windows.Forms.TextBox()
    Me.cmdMerge = New System.Windows.Forms.Button()
    Me.pctTitle = New System.Windows.Forms.PictureBox()
    Me.tmrUpdateCheck = New System.Windows.Forms.Timer(Me.components)
    Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
    Me.mnuOutput = New System.Windows.Forms.ContextMenu()
    Me.mnuCopy = New System.Windows.Forms.MenuItem()
    Me.mnuClear = New System.Windows.Forms.MenuItem()
    Me.mnuSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuSelectAll = New System.Windows.Forms.MenuItem()
    Me.ttInfo = New Slips7ream.ToolTip(Me.components)
    Me.pnlSlips7ream.SuspendLayout()
    CType(Me.spltSlips7ream, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.spltSlips7ream.Panel1.SuspendLayout()
    Me.spltSlips7ream.Panel2.SuspendLayout()
    Me.spltSlips7ream.SuspendLayout()
    Me.pnlPackages.SuspendLayout()
    Me.pnlSP64.SuspendLayout()
    Me.pnlSP.SuspendLayout()
    Me.pnlUpdates.SuspendLayout()
    Me.pnlMSU.SuspendLayout()
    Me.pnlWIM.SuspendLayout()
    Me.pnlBottom.SuspendLayout()
    Me.pnlISO.SuspendLayout()
    Me.pnlProgress.SuspendLayout()
    Me.pnlControl.SuspendLayout()
    Me.pnlISOOptions.SuspendLayout()
    Me.pnlMerge.SuspendLayout()
    CType(Me.pctTitle, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlSlips7ream
    '
    Me.pnlSlips7ream.ColumnCount = 2
    Me.pnlSlips7ream.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
    Me.pnlSlips7ream.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSlips7ream.Controls.Add(Me.spltSlips7ream, 0, 3)
    Me.pnlSlips7ream.Controls.Add(Me.lblWIM, 0, 1)
    Me.pnlSlips7ream.Controls.Add(Me.pnlWIM, 1, 1)
    Me.pnlSlips7ream.Controls.Add(Me.pnlBottom, 0, 9)
    Me.pnlSlips7ream.Controls.Add(Me.chkISO, 0, 5)
    Me.pnlSlips7ream.Controls.Add(Me.pnlISO, 1, 5)
    Me.pnlSlips7ream.Controls.Add(Me.pnlProgress, 0, 10)
    Me.pnlSlips7ream.Controls.Add(Me.lblISOLabel, 0, 6)
    Me.pnlSlips7ream.Controls.Add(Me.txtISOLabel, 1, 6)
    Me.pnlSlips7ream.Controls.Add(Me.pnlControl, 0, 8)
    Me.pnlSlips7ream.Controls.Add(Me.pnlISOOptions, 1, 7)
    Me.pnlSlips7ream.Controls.Add(Me.chkMerge, 0, 2)
    Me.pnlSlips7ream.Controls.Add(Me.pnlMerge, 1, 2)
    Me.pnlSlips7ream.Controls.Add(Me.pctTitle, 0, 0)
    Me.pnlSlips7ream.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSlips7ream.Location = New System.Drawing.Point(0, 0)
    Me.pnlSlips7ream.Name = "pnlSlips7ream"
    Me.pnlSlips7ream.RowCount = 11
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.Size = New System.Drawing.Size(424, 757)
    Me.pnlSlips7ream.TabIndex = 0
    '
    'spltSlips7ream
    '
    Me.pnlSlips7ream.SetColumnSpan(Me.spltSlips7ream, 2)
    Me.spltSlips7ream.Cursor = System.Windows.Forms.Cursors.Default
    Me.spltSlips7ream.Dock = System.Windows.Forms.DockStyle.Fill
    Me.spltSlips7ream.DrawGrabHandle = True
    Me.spltSlips7ream.Location = New System.Drawing.Point(0, 98)
    Me.spltSlips7ream.Margin = New System.Windows.Forms.Padding(0)
    Me.spltSlips7ream.Name = "spltSlips7ream"
    Me.spltSlips7ream.Orientation = System.Windows.Forms.Orientation.Horizontal
    '
    'spltSlips7ream.Panel1
    '
    Me.spltSlips7ream.Panel1.Controls.Add(Me.pnlPackages)
    Me.spltSlips7ream.Panel1MinSize = 97
    '
    'spltSlips7ream.Panel2
    '
    Me.spltSlips7ream.Panel2.Controls.Add(Me.pnlUpdates)
    Me.spltSlips7ream.Panel2MinSize = 90
    Me.spltSlips7ream.ResizeRectangle = False
    Me.spltSlips7ream.Size = New System.Drawing.Size(424, 285)
    Me.spltSlips7ream.SplitterDistance = 181
    Me.spltSlips7ream.SplitterIncrement = 16
    Me.spltSlips7ream.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.spltSlips7ream, "Click and Drag to resize the Image Packages and Updates boxes.")
    '
    'pnlPackages
    '
    Me.pnlPackages.ColumnCount = 2
    Me.pnlPackages.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
    Me.pnlPackages.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPackages.Controls.Add(Me.pnlSP64, 1, 2)
    Me.pnlPackages.Controls.Add(Me.lblImages, 0, 0)
    Me.pnlPackages.Controls.Add(Me.chkSP, 0, 1)
    Me.pnlPackages.Controls.Add(Me.lblSP64, 0, 2)
    Me.pnlPackages.Controls.Add(Me.lvImages, 1, 0)
    Me.pnlPackages.Controls.Add(Me.pnlSP, 1, 1)
    Me.pnlPackages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlPackages.Location = New System.Drawing.Point(0, 0)
    Me.pnlPackages.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPackages.Name = "pnlPackages"
    Me.pnlPackages.RowCount = 3
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPackages.Size = New System.Drawing.Size(424, 181)
    Me.pnlPackages.TabIndex = 0
    '
    'pnlSP64
    '
    Me.pnlSP64.AutoSize = True
    Me.pnlSP64.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlSP64.ColumnCount = 2
    Me.pnlSP64.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP64.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlSP64.Controls.Add(Me.txtSP64, 0, 0)
    Me.pnlSP64.Controls.Add(Me.cmdSP64, 1, 0)
    Me.pnlSP64.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSP64.Location = New System.Drawing.Point(100, 152)
    Me.pnlSP64.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSP64.Name = "pnlSP64"
    Me.pnlSP64.RowCount = 1
    Me.pnlSP64.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP64.Size = New System.Drawing.Size(324, 29)
    Me.pnlSP64.TabIndex = 5
    Me.pnlSP64.Visible = False
    '
    'txtSP64
    '
    Me.txtSP64.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSP64.Location = New System.Drawing.Point(3, 4)
    Me.txtSP64.Name = "txtSP64"
    Me.txtSP64.Size = New System.Drawing.Size(237, 20)
    Me.txtSP64.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtSP64, "Windows 7 x64 Service Pack 1 installer EXE." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Note that x64 Integration requires " & _
        "a 64-bit Operating System.)")
    '
    'cmdSP64
    '
    Me.cmdSP64.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSP64.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSP64.Location = New System.Drawing.Point(246, 3)
    Me.cmdSP64.Name = "cmdSP64"
    Me.cmdSP64.Size = New System.Drawing.Size(75, 23)
    Me.cmdSP64.TabIndex = 1
    Me.cmdSP64.Text = "Browse..."
    Me.ttInfo.SetTooltip(Me.cmdSP64, "Choose an x64 Service Pack EXE.")
    Me.cmdSP64.UseVisualStyleBackColor = True
    '
    'lblImages
    '
    Me.lblImages.AutoSize = True
    Me.lblImages.Location = New System.Drawing.Point(3, 6)
    Me.lblImages.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
    Me.lblImages.Name = "lblImages"
    Me.lblImages.Size = New System.Drawing.Size(90, 13)
    Me.lblImages.TabIndex = 0
    Me.lblImages.Text = "Image &Packages:"
    '
    'chkSP
    '
    Me.chkSP.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkSP.AutoSize = True
    Me.chkSP.Location = New System.Drawing.Point(3, 129)
    Me.chkSP.Name = "chkSP"
    Me.chkSP.Size = New System.Drawing.Size(93, 17)
    Me.chkSP.TabIndex = 2
    Me.chkSP.Text = "&Service Pack:"
    Me.ttInfo.SetTooltip(Me.chkSP, "Integrate Windows 7 Service Pack 1 into an RTM Image.")
    Me.chkSP.UseVisualStyleBackColor = True
    '
    'lblSP64
    '
    Me.lblSP64.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSP64.AutoSize = True
    Me.lblSP64.Location = New System.Drawing.Point(3, 160)
    Me.lblSP64.Name = "lblSP64"
    Me.lblSP64.Size = New System.Drawing.Size(94, 13)
    Me.lblSP64.TabIndex = 4
    Me.lblSP64.Text = "x64 Ser&vice Pack:"
    Me.lblSP64.Visible = False
    '
    'lvImages
    '
    Me.lvImages.CheckBoxes = True
    Me.lvImages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIndex, Me.colName, Me.colSize})
    Me.lvImages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvImages.FullRowSelect = True
    Me.lvImages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.lvImages.Location = New System.Drawing.Point(103, 3)
    Me.lvImages.MultiSelect = False
    Me.lvImages.Name = "lvImages"
    Me.lvImages.ShowItemToolTips = True
    Me.lvImages.Size = New System.Drawing.Size(318, 117)
    Me.lvImages.TabIndex = 1
    Me.lvImages.UseCompatibleStateImageBehavior = False
    Me.lvImages.View = System.Windows.Forms.View.Details
    '
    'colIndex
    '
    Me.colIndex.Text = "Index"
    '
    'colName
    '
    Me.colName.Text = "Name"
    Me.colName.Width = 120
    '
    'colSize
    '
    Me.colSize.Text = "Size"
    Me.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'pnlSP
    '
    Me.pnlSP.AutoSize = True
    Me.pnlSP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlSP.ColumnCount = 2
    Me.pnlSP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlSP.Controls.Add(Me.txtSP, 0, 0)
    Me.pnlSP.Controls.Add(Me.cmdSP, 1, 0)
    Me.pnlSP.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSP.Location = New System.Drawing.Point(100, 123)
    Me.pnlSP.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSP.Name = "pnlSP"
    Me.pnlSP.RowCount = 1
    Me.pnlSP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP.Size = New System.Drawing.Size(324, 29)
    Me.pnlSP.TabIndex = 3
    '
    'txtSP
    '
    Me.txtSP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSP.Enabled = False
    Me.txtSP.Location = New System.Drawing.Point(3, 4)
    Me.txtSP.Name = "txtSP"
    Me.txtSP.Size = New System.Drawing.Size(237, 20)
    Me.txtSP.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtSP, "Windows 7 Service Pack 1 installer EXE." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Note that x64 Integration requires a 64" & _
        "-bit Operating System.)")
    '
    'cmdSP
    '
    Me.cmdSP.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSP.Enabled = False
    Me.cmdSP.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSP.Location = New System.Drawing.Point(246, 3)
    Me.cmdSP.Name = "cmdSP"
    Me.cmdSP.Size = New System.Drawing.Size(75, 23)
    Me.cmdSP.TabIndex = 1
    Me.cmdSP.Text = "Browse..."
    Me.ttInfo.SetTooltip(Me.cmdSP, "Choose a Service Pack EXE.")
    Me.cmdSP.UseVisualStyleBackColor = True
    '
    'pnlUpdates
    '
    Me.pnlUpdates.ColumnCount = 2
    Me.pnlUpdates.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
    Me.pnlUpdates.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdates.Controls.Add(Me.pnlMSU, 1, 1)
    Me.pnlUpdates.Controls.Add(Me.lblMSU, 0, 0)
    Me.pnlUpdates.Controls.Add(Me.lvMSU, 1, 0)
    Me.pnlUpdates.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUpdates.Location = New System.Drawing.Point(0, 0)
    Me.pnlUpdates.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUpdates.Name = "pnlUpdates"
    Me.pnlUpdates.RowCount = 2
    Me.pnlUpdates.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdates.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
    Me.pnlUpdates.Size = New System.Drawing.Size(424, 100)
    Me.pnlUpdates.TabIndex = 0
    '
    'pnlMSU
    '
    Me.pnlMSU.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlMSU.AutoSize = True
    Me.pnlMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlMSU.ColumnCount = 4
    Me.pnlMSU.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMSU.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlMSU.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMSU.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMSU.Controls.Add(Me.cmdAddMSU, 0, 0)
    Me.pnlMSU.Controls.Add(Me.cmdClearMSU, 3, 0)
    Me.pnlMSU.Controls.Add(Me.cmdRemMSU, 2, 0)
    Me.pnlMSU.Location = New System.Drawing.Point(100, 70)
    Me.pnlMSU.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlMSU.Name = "pnlMSU"
    Me.pnlMSU.RowCount = 1
    Me.pnlMSU.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlMSU.Size = New System.Drawing.Size(324, 29)
    Me.pnlMSU.TabIndex = 2
    '
    'cmdAddMSU
    '
    Me.cmdAddMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdAddMSU.AutoSize = True
    Me.cmdAddMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdAddMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAddMSU.Location = New System.Drawing.Point(3, 3)
    Me.cmdAddMSU.Name = "cmdAddMSU"
    Me.cmdAddMSU.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
    Me.cmdAddMSU.Size = New System.Drawing.Size(92, 23)
    Me.cmdAddMSU.TabIndex = 0
    Me.cmdAddMSU.Text = "&Add Updates..."
    Me.ttInfo.SetTooltip(Me.cmdAddMSU, "Add MSU, CAB, MLC, or Language Pack EXE updates.")
    Me.cmdAddMSU.UseVisualStyleBackColor = True
    '
    'cmdClearMSU
    '
    Me.cmdClearMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdClearMSU.AutoSize = True
    Me.cmdClearMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdClearMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClearMSU.Location = New System.Drawing.Point(233, 3)
    Me.cmdClearMSU.Name = "cmdClearMSU"
    Me.cmdClearMSU.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
    Me.cmdClearMSU.Size = New System.Drawing.Size(88, 23)
    Me.cmdClearMSU.TabIndex = 3
    Me.cmdClearMSU.Text = "Clear Updates"
    Me.ttInfo.SetTooltip(Me.cmdClearMSU, "Clear the list of Windows Updates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Shortcut: Shift+Delete)")
    Me.cmdClearMSU.UseVisualStyleBackColor = True
    '
    'cmdRemMSU
    '
    Me.cmdRemMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdRemMSU.AutoSize = True
    Me.cmdRemMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdRemMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRemMSU.Location = New System.Drawing.Point(123, 3)
    Me.cmdRemMSU.Name = "cmdRemMSU"
    Me.cmdRemMSU.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
    Me.cmdRemMSU.Size = New System.Drawing.Size(104, 23)
    Me.cmdRemMSU.TabIndex = 2
    Me.cmdRemMSU.Text = "Remove Updates"
    Me.ttInfo.SetTooltip(Me.cmdRemMSU, "Remove the selected items from the list of Windows Updates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Shortcut: Delete)" & _
        "")
    Me.cmdRemMSU.UseVisualStyleBackColor = True
    '
    'lblMSU
    '
    Me.lblMSU.AutoSize = True
    Me.lblMSU.Location = New System.Drawing.Point(3, 6)
    Me.lblMSU.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
    Me.lblMSU.Name = "lblMSU"
    Me.lblMSU.Size = New System.Drawing.Size(53, 13)
    Me.lblMSU.TabIndex = 0
    Me.lblMSU.Text = "&Updates: "
    '
    'lvMSU
    '
    Me.lvMSU.AllowDrop = True
    Me.lvMSU.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colUpdate, Me.colType})
    Me.lvMSU.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvMSU.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.lvMSU.HideSelection = False
    Me.lvMSU.LargeImageList = Me.imlUpdates
    Me.lvMSU.Location = New System.Drawing.Point(103, 3)
    Me.lvMSU.Name = "lvMSU"
    Me.lvMSU.ShowItemToolTips = True
    Me.lvMSU.Size = New System.Drawing.Size(318, 64)
    Me.lvMSU.SmallImageList = Me.imlUpdates
    Me.lvMSU.TabIndex = 1
    Me.lvMSU.UseCompatibleStateImageBehavior = False
    Me.lvMSU.View = System.Windows.Forms.View.Details
    '
    'colUpdate
    '
    Me.colUpdate.Text = "Windows Update"
    Me.colUpdate.Width = 229
    '
    'colType
    '
    Me.colType.Text = "Type"
    Me.colType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.colType.Width = 75
    '
    'imlUpdates
    '
    Me.imlUpdates.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
    Me.imlUpdates.ImageSize = New System.Drawing.Size(16, 16)
    Me.imlUpdates.TransparentColor = System.Drawing.Color.Transparent
    '
    'lblWIM
    '
    Me.lblWIM.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblWIM.AutoSize = True
    Me.lblWIM.Location = New System.Drawing.Point(3, 48)
    Me.lblWIM.Name = "lblWIM"
    Me.lblWIM.Size = New System.Drawing.Size(83, 13)
    Me.lblWIM.TabIndex = 0
    Me.lblWIM.Text = "INSTALL.&WIM: "
    '
    'pnlWIM
    '
    Me.pnlWIM.AutoSize = True
    Me.pnlWIM.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlWIM.ColumnCount = 2
    Me.pnlWIM.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlWIM.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlWIM.Controls.Add(Me.txtWIM, 0, 0)
    Me.pnlWIM.Controls.Add(Me.cmdWIM, 1, 0)
    Me.pnlWIM.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlWIM.Location = New System.Drawing.Point(100, 40)
    Me.pnlWIM.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlWIM.Name = "pnlWIM"
    Me.pnlWIM.RowCount = 1
    Me.pnlWIM.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlWIM.Size = New System.Drawing.Size(324, 29)
    Me.pnlWIM.TabIndex = 1
    '
    'txtWIM
    '
    Me.txtWIM.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtWIM.Location = New System.Drawing.Point(3, 4)
    Me.txtWIM.Name = "txtWIM"
    Me.txtWIM.Size = New System.Drawing.Size(237, 20)
    Me.txtWIM.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtWIM, "Source WIM or ISO to create image from.")
    '
    'cmdWIM
    '
    Me.cmdWIM.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdWIM.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdWIM.Location = New System.Drawing.Point(246, 3)
    Me.cmdWIM.Name = "cmdWIM"
    Me.cmdWIM.Size = New System.Drawing.Size(75, 23)
    Me.cmdWIM.TabIndex = 1
    Me.cmdWIM.Text = "Browse..."
    Me.ttInfo.SetTooltip(Me.cmdWIM, "Choose a WIM or ISO file.")
    Me.cmdWIM.UseVisualStyleBackColor = True
    '
    'pnlBottom
    '
    Me.pnlBottom.AutoSize = True
    Me.pnlBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlBottom.ColumnCount = 6
    Me.pnlSlips7ream.SetColumnSpan(Me.pnlBottom, 2)
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlBottom.Controls.Add(Me.cmdBegin, 4, 0)
    Me.pnlBottom.Controls.Add(Me.cmdClose, 5, 0)
    Me.pnlBottom.Controls.Add(Me.lblActivity, 1, 0)
    Me.pnlBottom.Controls.Add(Me.cmdConfig, 3, 0)
    Me.pnlBottom.Controls.Add(Me.expOutput, 0, 0)
    Me.pnlBottom.Controls.Add(Me.cmdOpenFolder, 2, 0)
    Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlBottom.Location = New System.Drawing.Point(0, 519)
    Me.pnlBottom.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlBottom.Name = "pnlBottom"
    Me.pnlBottom.RowCount = 1
    Me.pnlBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlBottom.Size = New System.Drawing.Size(424, 29)
    Me.pnlBottom.TabIndex = 11
    '
    'cmdBegin
    '
    Me.cmdBegin.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdBegin.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdBegin.Location = New System.Drawing.Point(265, 3)
    Me.cmdBegin.Name = "cmdBegin"
    Me.cmdBegin.Size = New System.Drawing.Size(75, 23)
    Me.cmdBegin.TabIndex = 1
    Me.cmdBegin.Text = "&Begin"
    Me.ttInfo.SetTooltip(Me.cmdBegin, "Start the Slipstream procedure.")
    Me.cmdBegin.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(346, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 3
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'lblActivity
    '
    Me.lblActivity.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblActivity.AutoEllipsis = True
    Me.lblActivity.Location = New System.Drawing.Point(28, 8)
    Me.lblActivity.Margin = New System.Windows.Forms.Padding(3)
    Me.lblActivity.Name = "lblActivity"
    Me.lblActivity.Size = New System.Drawing.Size(64, 13)
    Me.lblActivity.TabIndex = 0
    Me.lblActivity.Text = "Idle"
    Me.lblActivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.lblActivity.UseMnemonic = False
    '
    'cmdConfig
    '
    Me.cmdConfig.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdConfig.Location = New System.Drawing.Point(179, 3)
    Me.cmdConfig.Name = "cmdConfig"
    Me.cmdConfig.Size = New System.Drawing.Size(80, 23)
    Me.cmdConfig.TabIndex = 2
    Me.cmdConfig.Text = "Confi&guration"
    Me.ttInfo.SetTooltip(Me.cmdConfig, "Change SLIPS7REAM settings.")
    Me.cmdConfig.UseVisualStyleBackColor = True
    '
    'expOutput
    '
    Me.expOutput.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.expOutput.AutoSize = True
    Me.expOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.expOutput.Location = New System.Drawing.Point(3, 5)
    Me.expOutput.Name = "expOutput"
    Me.expOutput.Open = False
    Me.expOutput.Size = New System.Drawing.Size(19, 19)
    Me.expOutput.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.expOutput, "Show Output consoles.")
    '
    'cmdOpenFolder
    '
    Me.cmdOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdOpenFolder.Location = New System.Drawing.Point(98, 3)
    Me.cmdOpenFolder.Name = "cmdOpenFolder"
    Me.cmdOpenFolder.Size = New System.Drawing.Size(75, 23)
    Me.cmdOpenFolder.TabIndex = 5
    Me.cmdOpenFolder.Text = "Open &Folder"
    Me.ttInfo.SetTooltip(Me.cmdOpenFolder, "Open the folder containing the complete ISO or WIM file.")
    Me.cmdOpenFolder.UseVisualStyleBackColor = True
    Me.cmdOpenFolder.Visible = False
    '
    'chkISO
    '
    Me.chkISO.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkISO.AutoSize = True
    Me.chkISO.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkISO.Location = New System.Drawing.Point(3, 388)
    Me.chkISO.Name = "chkISO"
    Me.chkISO.Size = New System.Drawing.Size(93, 18)
    Me.chkISO.TabIndex = 5
    Me.chkISO.Text = "Save to &ISO:"
    Me.ttInfo.SetTooltip(Me.chkISO, "Insert the Image into a Windows 7 ISO.")
    Me.chkISO.UseVisualStyleBackColor = True
    '
    'pnlISO
    '
    Me.pnlISO.AutoSize = True
    Me.pnlISO.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlISO.ColumnCount = 2
    Me.pnlISO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlISO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlISO.Controls.Add(Me.txtISO, 0, 0)
    Me.pnlISO.Controls.Add(Me.cmdISO, 1, 0)
    Me.pnlISO.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlISO.Location = New System.Drawing.Point(100, 383)
    Me.pnlISO.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlISO.Name = "pnlISO"
    Me.pnlISO.RowCount = 1
    Me.pnlISO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlISO.Size = New System.Drawing.Size(324, 29)
    Me.pnlISO.TabIndex = 6
    '
    'txtISO
    '
    Me.txtISO.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtISO.Enabled = False
    Me.txtISO.Location = New System.Drawing.Point(3, 4)
    Me.txtISO.Name = "txtISO"
    Me.txtISO.Size = New System.Drawing.Size(237, 20)
    Me.txtISO.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtISO, "ISO to update with the new image.")
    '
    'cmdISO
    '
    Me.cmdISO.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdISO.Enabled = False
    Me.cmdISO.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdISO.Location = New System.Drawing.Point(246, 3)
    Me.cmdISO.Name = "cmdISO"
    Me.cmdISO.Size = New System.Drawing.Size(75, 23)
    Me.cmdISO.TabIndex = 1
    Me.cmdISO.Text = "Browse..."
    Me.ttInfo.SetTooltip(Me.cmdISO, "Choose a Windows 7 ISO." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Use an x86 ISO if you're merging x86 and x64.)")
    Me.cmdISO.UseVisualStyleBackColor = True
    '
    'pnlProgress
    '
    Me.pnlProgress.AutoSize = True
    Me.pnlProgress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlProgress.ColumnCount = 2
    Me.pnlSlips7ream.SetColumnSpan(Me.pnlProgress, 2)
    Me.pnlProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlProgress.Controls.Add(Me.pbTotal, 0, 0)
    Me.pnlProgress.Controls.Add(Me.pbIndividual, 1, 0)
    Me.pnlProgress.Controls.Add(Me.txtOutput, 0, 1)
    Me.pnlProgress.Controls.Add(Me.txtOutputError, 0, 2)
    Me.pnlProgress.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProgress.Location = New System.Drawing.Point(0, 548)
    Me.pnlProgress.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlProgress.Name = "pnlProgress"
    Me.pnlProgress.RowCount = 3
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.Size = New System.Drawing.Size(424, 209)
    Me.pnlProgress.TabIndex = 12
    '
    'pbTotal
    '
    Me.pbTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pbTotal.Location = New System.Drawing.Point(3, 3)
    Me.pbTotal.Name = "pbTotal"
    Me.pbTotal.Size = New System.Drawing.Size(206, 16)
    Me.pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous
    Me.pbTotal.TabIndex = 0
    Me.pbTotal.Visible = False
    '
    'pbIndividual
    '
    Me.pbIndividual.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pbIndividual.Location = New System.Drawing.Point(215, 3)
    Me.pbIndividual.Name = "pbIndividual"
    Me.pbIndividual.Size = New System.Drawing.Size(206, 16)
    Me.pbIndividual.Style = System.Windows.Forms.ProgressBarStyle.Continuous
    Me.pbIndividual.TabIndex = 1
    Me.pbIndividual.Visible = False
    '
    'txtOutput
    '
    Me.pnlProgress.SetColumnSpan(Me.txtOutput, 2)
    Me.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtOutput.Location = New System.Drawing.Point(3, 25)
    Me.txtOutput.Multiline = True
    Me.txtOutput.Name = "txtOutput"
    Me.txtOutput.ReadOnly = True
    Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtOutput.Size = New System.Drawing.Size(418, 100)
    Me.txtOutput.TabIndex = 2
    Me.txtOutput.Visible = False
    '
    'txtOutputError
    '
    Me.txtOutputError.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlProgress.SetColumnSpan(Me.txtOutputError, 2)
    Me.txtOutputError.Location = New System.Drawing.Point(3, 131)
    Me.txtOutputError.Multiline = True
    Me.txtOutputError.Name = "txtOutputError"
    Me.txtOutputError.ReadOnly = True
    Me.txtOutputError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtOutputError.Size = New System.Drawing.Size(418, 75)
    Me.txtOutputError.TabIndex = 3
    Me.txtOutputError.Visible = False
    '
    'lblISOLabel
    '
    Me.lblISOLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblISOLabel.AutoSize = True
    Me.lblISOLabel.Enabled = False
    Me.lblISOLabel.Location = New System.Drawing.Point(3, 418)
    Me.lblISOLabel.Name = "lblISOLabel"
    Me.lblISOLabel.Size = New System.Drawing.Size(57, 13)
    Me.lblISOLabel.TabIndex = 7
    Me.lblISOLabel.Text = "ISO &Label:"
    '
    'txtISOLabel
    '
    Me.txtISOLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtISOLabel.Enabled = False
    Me.txtISOLabel.Location = New System.Drawing.Point(103, 415)
    Me.txtISOLabel.MaxLength = 32
    Me.txtISOLabel.Name = "txtISOLabel"
    Me.txtISOLabel.Size = New System.Drawing.Size(318, 20)
    Me.txtISOLabel.TabIndex = 8
    Me.ttInfo.SetTooltip(Me.txtISOLabel, "Disc Label for ISO.")
    '
    'pnlControl
    '
    Me.pnlControl.AutoSize = True
    Me.pnlControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlControl.ColumnCount = 4
    Me.pnlSlips7ream.SetColumnSpan(Me.pnlControl, 2)
    Me.pnlControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
    Me.pnlControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlControl.Controls.Add(Me.lblPriority, 0, 0)
    Me.pnlControl.Controls.Add(Me.cmbPriority, 1, 0)
    Me.pnlControl.Controls.Add(Me.lblCompletion, 2, 0)
    Me.pnlControl.Controls.Add(Me.cmbCompletion, 3, 0)
    Me.pnlControl.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlControl.Location = New System.Drawing.Point(0, 492)
    Me.pnlControl.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlControl.Name = "pnlControl"
    Me.pnlControl.RowCount = 1
    Me.pnlControl.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlControl.Size = New System.Drawing.Size(424, 27)
    Me.pnlControl.TabIndex = 10
    '
    'lblPriority
    '
    Me.lblPriority.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPriority.AutoSize = True
    Me.lblPriority.Location = New System.Drawing.Point(3, 7)
    Me.lblPriority.Name = "lblPriority"
    Me.lblPriority.Size = New System.Drawing.Size(82, 13)
    Me.lblPriority.TabIndex = 0
    Me.lblPriority.Text = "Process P&riority:"
    '
    'cmbPriority
    '
    Me.cmbPriority.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbPriority.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbPriority.FormattingEnabled = True
    Me.cmbPriority.Items.AddRange(New Object() {"Realtime", "High", "Above Normal", "Normal", "Below Normal", "Low"})
    Me.cmbPriority.Location = New System.Drawing.Point(103, 3)
    Me.cmbPriority.MaximumSize = New System.Drawing.Size(115, 0)
    Me.cmbPriority.MinimumSize = New System.Drawing.Size(115, 0)
    Me.cmbPriority.Name = "cmbPriority"
    Me.cmbPriority.Size = New System.Drawing.Size(115, 21)
    Me.cmbPriority.TabIndex = 1
    Me.ttInfo.SetTooltip(Me.cmbPriority, resources.GetString("cmbPriority.ToolTip"))
    '
    'lblCompletion
    '
    Me.lblCompletion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblCompletion.AutoSize = True
    Me.lblCompletion.Location = New System.Drawing.Point(222, 7)
    Me.lblCompletion.Name = "lblCompletion"
    Me.lblCompletion.Size = New System.Drawing.Size(79, 13)
    Me.lblCompletion.TabIndex = 2
    Me.lblCompletion.Text = "O&n Completion:"
    '
    'cmbCompletion
    '
    Me.cmbCompletion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbCompletion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbCompletion.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbCompletion.FormattingEnabled = True
    Me.cmbCompletion.Items.AddRange(New Object() {"Do Nothing", "Close Program", "Shut Down", "Restart", "Sleep"})
    Me.cmbCompletion.Location = New System.Drawing.Point(307, 3)
    Me.cmbCompletion.MaximumSize = New System.Drawing.Size(115, 0)
    Me.cmbCompletion.MinimumSize = New System.Drawing.Size(115, 0)
    Me.cmbCompletion.Name = "cmbCompletion"
    Me.cmbCompletion.Size = New System.Drawing.Size(115, 21)
    Me.cmbCompletion.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.cmbCompletion, "Event to run after Slipstream is complete.")
    '
    'pnlISOOptions
    '
    Me.pnlISOOptions.AutoSize = True
    Me.pnlISOOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlISOOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
    Me.pnlISOOptions.ColumnCount = 3
    Me.pnlISOOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlISOOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlISOOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlISOOptions.Controls.Add(Me.cmbLimitType, 1, 1)
    Me.pnlISOOptions.Controls.Add(Me.cmbISOFormat, 2, 0)
    Me.pnlISOOptions.Controls.Add(Me.chkUnlock, 0, 0)
    Me.pnlISOOptions.Controls.Add(Me.lblISOFS, 1, 0)
    Me.pnlISOOptions.Controls.Add(Me.chkUEFI, 0, 1)
    Me.pnlISOOptions.Controls.Add(Me.cmbLimit, 2, 1)
    Me.pnlISOOptions.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlISOOptions.Location = New System.Drawing.Point(100, 438)
    Me.pnlISOOptions.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlISOOptions.Name = "pnlISOOptions"
    Me.pnlISOOptions.RowCount = 2
    Me.pnlISOOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlISOOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlISOOptions.Size = New System.Drawing.Size(324, 54)
    Me.pnlISOOptions.TabIndex = 9
    '
    'cmbLimitType
    '
    Me.cmbLimitType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbLimitType.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbLimitType.FormattingEnabled = True
    Me.cmbLimitType.Items.AddRange(New Object() {"Single File", "Split WIM", "Split ISO"})
    Me.cmbLimitType.Location = New System.Drawing.Point(124, 30)
    Me.cmbLimitType.Name = "cmbLimitType"
    Me.cmbLimitType.Size = New System.Drawing.Size(75, 21)
    Me.cmbLimitType.TabIndex = 4
    Me.ttInfo.SetTooltip(Me.cmbLimitType, resources.GetString("cmbLimitType.ToolTip"))
    '
    'cmbISOFormat
    '
    Me.cmbISOFormat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbISOFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbISOFormat.Enabled = False
    Me.cmbISOFormat.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbISOFormat.FormattingEnabled = True
    Me.cmbISOFormat.Items.AddRange(New Object() {"ISO 9960", "Joliet", "Joliet / ISO 9960", "UDF", "UDF / ISO 9960", "UDF 1.02", "UDF 1.02 / ISO 9960"})
    Me.cmbISOFormat.Location = New System.Drawing.Point(205, 3)
    Me.cmbISOFormat.Name = "cmbISOFormat"
    Me.cmbISOFormat.Size = New System.Drawing.Size(116, 21)
    Me.cmbISOFormat.TabIndex = 2
    Me.ttInfo.SetTooltip(Me.cmbISOFormat, resources.GetString("cmbISOFormat.ToolTip"))
    '
    'chkUnlock
    '
    Me.chkUnlock.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkUnlock.AutoSize = True
    Me.chkUnlock.Enabled = False
    Me.chkUnlock.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUnlock.Location = New System.Drawing.Point(3, 4)
    Me.chkUnlock.Name = "chkUnlock"
    Me.chkUnlock.Size = New System.Drawing.Size(115, 18)
    Me.chkUnlock.TabIndex = 0
    Me.chkUnlock.Text = "Unlock All &Editions"
    Me.ttInfo.SetTooltip(Me.chkUnlock, "Remove ""ei.cfg"" and the install catalogs from the ISO to allow installation of al" & _
        "l editions.")
    Me.chkUnlock.UseVisualStyleBackColor = True
    '
    'lblISOFS
    '
    Me.lblISOFS.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblISOFS.AutoSize = True
    Me.lblISOFS.Enabled = False
    Me.lblISOFS.Location = New System.Drawing.Point(124, 7)
    Me.lblISOFS.Name = "lblISOFS"
    Me.lblISOFS.Size = New System.Drawing.Size(63, 13)
    Me.lblISOFS.TabIndex = 1
    Me.lblISOFS.Text = "File Sys&tem:"
    '
    'chkUEFI
    '
    Me.chkUEFI.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkUEFI.AutoSize = True
    Me.chkUEFI.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUEFI.Location = New System.Drawing.Point(3, 31)
    Me.chkUEFI.Name = "chkUEFI"
    Me.chkUEFI.Size = New System.Drawing.Size(81, 18)
    Me.chkUEFI.TabIndex = 3
    Me.chkUEFI.Text = "UEFI B&oot"
    Me.ttInfo.SetTooltip(Me.chkUEFI, resources.GetString("chkUEFI.ToolTip"))
    Me.chkUEFI.UseVisualStyleBackColor = True
    '
    'cmbLimit
    '
    Me.cmbLimit.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbLimit.DropDownWidth = 140
    Me.cmbLimit.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbLimit.FormattingEnabled = True
    Me.cmbLimit.Location = New System.Drawing.Point(205, 30)
    Me.cmbLimit.Name = "cmbLimit"
    Me.cmbLimit.Size = New System.Drawing.Size(116, 21)
    Me.cmbLimit.TabIndex = 5
    Me.ttInfo.SetTooltip(Me.cmbLimit, resources.GetString("cmbLimit.ToolTip"))
    '
    'chkMerge
    '
    Me.chkMerge.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkMerge.AutoSize = True
    Me.chkMerge.Location = New System.Drawing.Point(3, 75)
    Me.chkMerge.Name = "chkMerge"
    Me.chkMerge.Size = New System.Drawing.Size(85, 17)
    Me.chkMerge.TabIndex = 2
    Me.chkMerge.Text = "&Merge WIM:"
    Me.ttInfo.SetTooltip(Me.chkMerge, "Merge the Packages of another Image with this one.")
    Me.chkMerge.UseVisualStyleBackColor = True
    '
    'pnlMerge
    '
    Me.pnlMerge.AutoSize = True
    Me.pnlMerge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlMerge.ColumnCount = 2
    Me.pnlMerge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlMerge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlMerge.Controls.Add(Me.txtMerge, 0, 0)
    Me.pnlMerge.Controls.Add(Me.cmdMerge, 1, 0)
    Me.pnlMerge.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlMerge.Location = New System.Drawing.Point(100, 69)
    Me.pnlMerge.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlMerge.Name = "pnlMerge"
    Me.pnlMerge.RowCount = 1
    Me.pnlMerge.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlMerge.Size = New System.Drawing.Size(324, 29)
    Me.pnlMerge.TabIndex = 3
    '
    'txtMerge
    '
    Me.txtMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtMerge.Enabled = False
    Me.txtMerge.Location = New System.Drawing.Point(3, 4)
    Me.txtMerge.Name = "txtMerge"
    Me.txtMerge.Size = New System.Drawing.Size(237, 20)
    Me.txtMerge.TabIndex = 0
    Me.ttInfo.SetTooltip(Me.txtMerge, "WIM or ISO to Merge with.")
    '
    'cmdMerge
    '
    Me.cmdMerge.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdMerge.Enabled = False
    Me.cmdMerge.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdMerge.Location = New System.Drawing.Point(246, 3)
    Me.cmdMerge.Name = "cmdMerge"
    Me.cmdMerge.Size = New System.Drawing.Size(75, 23)
    Me.cmdMerge.TabIndex = 1
    Me.cmdMerge.Text = "Browse..."
    Me.ttInfo.SetTooltip(Me.cmdMerge, "Choose a WIM or ISO file.")
    Me.cmdMerge.UseVisualStyleBackColor = True
    '
    'pctTitle
    '
    Me.pnlSlips7ream.SetColumnSpan(Me.pctTitle, 2)
    Me.pctTitle.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctTitle.Location = New System.Drawing.Point(0, 0)
    Me.pctTitle.Margin = New System.Windows.Forms.Padding(0)
    Me.pctTitle.Name = "pctTitle"
    Me.pctTitle.Size = New System.Drawing.Size(424, 40)
    Me.pctTitle.TabIndex = 22
    Me.pctTitle.TabStop = False
    '
    'tmrUpdateCheck
    '
    Me.tmrUpdateCheck.Enabled = True
    Me.tmrUpdateCheck.Interval = 2000
    '
    'tmrAnimation
    '
    Me.tmrAnimation.Interval = 10
    '
    'mnuOutput
    '
    Me.mnuOutput.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCopy, Me.mnuClear, Me.mnuSpacer, Me.mnuSelectAll})
    '
    'mnuCopy
    '
    Me.mnuCopy.Index = 0
    Me.mnuCopy.Text = "&Copy"
    '
    'mnuClear
    '
    Me.mnuClear.Index = 1
    Me.mnuClear.Text = "C&lear"
    '
    'mnuSpacer
    '
    Me.mnuSpacer.Index = 2
    Me.mnuSpacer.Text = "-"
    '
    'mnuSelectAll
    '
    Me.mnuSelectAll.Index = 3
    Me.mnuSelectAll.Text = "Select &All"
    '
    'ttInfo
    '
    Me.ttInfo.AutoPopDelay = 30000
    Me.ttInfo.InitialDelay = 500
    Me.ttInfo.ReshowDelay = 100
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(424, 757)
    Me.Controls.Add(Me.pnlSlips7ream)
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MinimumSize = New System.Drawing.Size(440, 520)
    Me.Name = "frmMain"
    Me.Text = "SLIPS7REAM - Windows 7 Image Slipstream Utility"
    Me.pnlSlips7ream.ResumeLayout(False)
    Me.pnlSlips7ream.PerformLayout()
    Me.spltSlips7ream.Panel1.ResumeLayout(False)
    Me.spltSlips7ream.Panel2.ResumeLayout(False)
    CType(Me.spltSlips7ream, System.ComponentModel.ISupportInitialize).EndInit()
    Me.spltSlips7ream.ResumeLayout(False)
    Me.pnlPackages.ResumeLayout(False)
    Me.pnlPackages.PerformLayout()
    Me.pnlSP64.ResumeLayout(False)
    Me.pnlSP64.PerformLayout()
    Me.pnlSP.ResumeLayout(False)
    Me.pnlSP.PerformLayout()
    Me.pnlUpdates.ResumeLayout(False)
    Me.pnlUpdates.PerformLayout()
    Me.pnlMSU.ResumeLayout(False)
    Me.pnlMSU.PerformLayout()
    Me.pnlWIM.ResumeLayout(False)
    Me.pnlWIM.PerformLayout()
    Me.pnlBottom.ResumeLayout(False)
    Me.pnlBottom.PerformLayout()
    Me.pnlISO.ResumeLayout(False)
    Me.pnlISO.PerformLayout()
    Me.pnlProgress.ResumeLayout(False)
    Me.pnlProgress.PerformLayout()
    Me.pnlControl.ResumeLayout(False)
    Me.pnlControl.PerformLayout()
    Me.pnlISOOptions.ResumeLayout(False)
    Me.pnlISOOptions.PerformLayout()
    Me.pnlMerge.ResumeLayout(False)
    Me.pnlMerge.PerformLayout()
    CType(Me.pctTitle, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlSlips7ream As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblWIM As System.Windows.Forms.Label
  Friend WithEvents txtWIM As System.Windows.Forms.TextBox
  Friend WithEvents cmdWIM As System.Windows.Forms.Button
  Friend WithEvents txtSP As System.Windows.Forms.TextBox
  Friend WithEvents cmdSP As System.Windows.Forms.Button
  Friend WithEvents lblMSU As System.Windows.Forms.Label
  Friend WithEvents lvMSU As System.Windows.Forms.ListView
  Friend WithEvents pnlMSU As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdAddMSU As System.Windows.Forms.Button
  Friend WithEvents cmdClearMSU As System.Windows.Forms.Button
  Friend WithEvents cmdRemMSU As System.Windows.Forms.Button
  Friend WithEvents colUpdate As System.Windows.Forms.ColumnHeader
  Friend WithEvents colType As System.Windows.Forms.ColumnHeader
  Friend WithEvents cmdBegin As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents imlUpdates As System.Windows.Forms.ImageList
  Friend WithEvents pnlWIM As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlSP As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlBottom As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblActivity As System.Windows.Forms.Label
  Friend WithEvents chkSP As System.Windows.Forms.CheckBox
  Friend WithEvents chkISO As System.Windows.Forms.CheckBox
  Friend WithEvents pnlISO As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtISO As System.Windows.Forms.TextBox
  Friend WithEvents cmdISO As System.Windows.Forms.Button
  Friend WithEvents chkUnlock As System.Windows.Forms.CheckBox
  Friend WithEvents pnlMerge As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtMerge As System.Windows.Forms.TextBox
  Friend WithEvents cmdMerge As System.Windows.Forms.Button
  Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
  Friend WithEvents lvImages As System.Windows.Forms.ListView
  Friend WithEvents lblImages As System.Windows.Forms.Label
  Friend WithEvents colIndex As System.Windows.Forms.ColumnHeader
  Friend WithEvents colName As System.Windows.Forms.ColumnHeader
  Friend WithEvents colSize As System.Windows.Forms.ColumnHeader
  Friend WithEvents ttInfo As Slips7ream.ToolTip
  Friend WithEvents pnlProgress As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pbTotal As System.Windows.Forms.ProgressBar
  Friend WithEvents pbIndividual As System.Windows.Forms.ProgressBar
  Friend WithEvents cmdConfig As System.Windows.Forms.Button
  Friend WithEvents lblISOLabel As System.Windows.Forms.Label
  Friend WithEvents txtISOLabel As System.Windows.Forms.TextBox
  Friend WithEvents pnlControl As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPriority As System.Windows.Forms.Label
  Friend WithEvents cmbPriority As System.Windows.Forms.ComboBox
  Friend WithEvents lblCompletion As System.Windows.Forms.Label
  Friend WithEvents pnlISOOptions As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblISOFS As System.Windows.Forms.Label
  Friend WithEvents cmbISOFormat As System.Windows.Forms.ComboBox
  Friend WithEvents txtOutput As System.Windows.Forms.TextBox
  Friend WithEvents txtOutputError As System.Windows.Forms.TextBox
  Friend WithEvents pnlSP64 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtSP64 As System.Windows.Forms.TextBox
  Friend WithEvents cmdSP64 As System.Windows.Forms.Button
  Friend WithEvents lblSP64 As System.Windows.Forms.Label
  Friend WithEvents cmbCompletion As System.Windows.Forms.ComboBox
  Friend WithEvents tmrUpdateCheck As System.Windows.Forms.Timer
  Friend WithEvents chkUEFI As System.Windows.Forms.CheckBox
  Friend WithEvents cmbLimit As System.Windows.Forms.ComboBox
  Friend WithEvents cmbLimitType As System.Windows.Forms.ComboBox
  Friend WithEvents expOutput As Slips7ream.Expander
  Friend WithEvents tmrAnimation As System.Windows.Forms.Timer
  Friend WithEvents pctTitle As System.Windows.Forms.PictureBox
  Friend WithEvents mnuOutput As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuClear As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
  Friend WithEvents cmdOpenFolder As System.Windows.Forms.Button
  Friend WithEvents spltSlips7ream As SplitContainerEx
  Friend WithEvents pnlPackages As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUpdates As System.Windows.Forms.TableLayoutPanel

End Class
