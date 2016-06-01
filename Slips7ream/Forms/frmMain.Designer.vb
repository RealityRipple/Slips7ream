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
    Me.lblImages = New System.Windows.Forms.Label()
    Me.lvImages = New Slips7ream.ListViewEx()
    Me.colIndex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.colSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.pnlServicePacks = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlSP64 = New System.Windows.Forms.TableLayoutPanel()
    Me.txtSP64 = New System.Windows.Forms.TextBox()
    Me.cmdSP64 = New System.Windows.Forms.Button()
    Me.chkSP = New System.Windows.Forms.CheckBox()
    Me.pnlSP = New System.Windows.Forms.TableLayoutPanel()
    Me.txtSP = New System.Windows.Forms.TextBox()
    Me.cmdSP = New System.Windows.Forms.Button()
    Me.lblSP64 = New System.Windows.Forms.Label()
    Me.brPackages = New Slips7ream.LineBreak()
    Me.pnlLoadPackageData = New System.Windows.Forms.TableLayoutPanel()
    Me.chkLoadDrivers = New System.Windows.Forms.CheckBox()
    Me.chkLoadUpdates = New System.Windows.Forms.CheckBox()
    Me.cmdLoadPackages = New System.Windows.Forms.Button()
    Me.chkLoadFeatures = New System.Windows.Forms.CheckBox()
    Me.pnlUpdates = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlMSU = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdAddMSU = New System.Windows.Forms.Button()
    Me.cmdClearMSU = New System.Windows.Forms.Button()
    Me.cmdRemMSU = New System.Windows.Forms.Button()
    Me.lblMSU = New System.Windows.Forms.Label()
    Me.lvMSU = New Slips7ream.ListViewEx()
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
    Me.pctOutputTear = New System.Windows.Forms.PictureBox()
    Me.lblISOLabel = New System.Windows.Forms.Label()
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
    Me.lblISOFeatures = New System.Windows.Forms.Label()
    Me.brUpdates = New Slips7ream.LineBreak()
    Me.brISO = New Slips7ream.LineBreak()
    Me.pnlISOLabel = New System.Windows.Forms.TableLayoutPanel()
    Me.txtISOLabel = New System.Windows.Forms.TextBox()
    Me.chkAutoLabel = New System.Windows.Forms.CheckBox()
    Me.tmrUpdateCheck = New System.Windows.Forms.Timer(Me.components)
    Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
    Me.mnuOutput = New System.Windows.Forms.ContextMenu()
    Me.mnuCopy = New System.Windows.Forms.MenuItem()
    Me.mnuCopyCommands = New System.Windows.Forms.MenuItem()
    Me.mnuClear = New System.Windows.Forms.MenuItem()
    Me.mnuSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuSelectAll = New System.Windows.Forms.MenuItem()
    Me.mnuImages = New System.Windows.Forms.ContextMenu()
    Me.mnuPackageInclude = New System.Windows.Forms.MenuItem()
    Me.mnuPackageRename = New System.Windows.Forms.MenuItem()
    Me.mnuPackageSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuPackageLocation = New System.Windows.Forms.MenuItem()
    Me.mnuPackageProperties = New System.Windows.Forms.MenuItem()
    Me.mnuMSU = New System.Windows.Forms.ContextMenu()
    Me.mnuUpdateTop = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateUp = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateDown = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateBottom = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateRemove = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateSpacer2 = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateLocation = New System.Windows.Forms.MenuItem()
    Me.mnuUpdateProperties = New System.Windows.Forms.MenuItem()
    Me.mnuISOLabel = New System.Windows.Forms.ContextMenu()
    Me.mnuLabel7x86 = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCST = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHB = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHP = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPR = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCUL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCEN = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCENVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCENCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabel7x86Space = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMU = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCAL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabel7x64 = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHBXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCHPXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCPRXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCULXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCENX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCENXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCENXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabel7x64Space = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALX = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALXFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALXFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALXFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALXVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCALXCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabel7AIO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUU = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUUFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUUFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUUFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUUVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCMUUCHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTA = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTAFRE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTAFRER = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTAFREO = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTAVOL = New System.Windows.Forms.MenuItem()
    Me.mnuLabelGRMCSTACHE = New System.Windows.Forms.MenuItem()
    Me.mnuLabelSpace = New System.Windows.Forms.MenuItem()
    Me.mnuLabelAuto = New System.Windows.Forms.MenuItem()
    Me.helpS7M = New System.Windows.Forms.HelpProvider()
    Me.ttInfo = New Slips7ream.ToolTip(Me.components)
    Me.ttActivity = New Slips7ream.ToolTip(Me.components)
    Me.pnlSlips7ream.SuspendLayout()
    CType(Me.spltSlips7ream, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.spltSlips7ream.Panel1.SuspendLayout()
    Me.spltSlips7ream.Panel2.SuspendLayout()
    Me.spltSlips7ream.SuspendLayout()
    Me.pnlPackages.SuspendLayout()
    Me.pnlServicePacks.SuspendLayout()
    Me.pnlSP64.SuspendLayout()
    Me.pnlSP.SuspendLayout()
    Me.pnlLoadPackageData.SuspendLayout()
    Me.pnlUpdates.SuspendLayout()
    Me.pnlMSU.SuspendLayout()
    Me.pnlWIM.SuspendLayout()
    Me.pnlBottom.SuspendLayout()
    Me.pnlISO.SuspendLayout()
    Me.pnlProgress.SuspendLayout()
    CType(Me.pctOutputTear, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlControl.SuspendLayout()
    Me.pnlISOOptions.SuspendLayout()
    Me.pnlMerge.SuspendLayout()
    CType(Me.pctTitle, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlISOLabel.SuspendLayout()
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
    Me.pnlSlips7ream.Controls.Add(Me.pnlBottom, 0, 11)
    Me.pnlSlips7ream.Controls.Add(Me.chkISO, 0, 6)
    Me.pnlSlips7ream.Controls.Add(Me.pnlISO, 1, 6)
    Me.pnlSlips7ream.Controls.Add(Me.pnlProgress, 0, 12)
    Me.pnlSlips7ream.Controls.Add(Me.lblISOLabel, 0, 7)
    Me.pnlSlips7ream.Controls.Add(Me.pnlControl, 0, 10)
    Me.pnlSlips7ream.Controls.Add(Me.pnlISOOptions, 1, 8)
    Me.pnlSlips7ream.Controls.Add(Me.chkMerge, 0, 2)
    Me.pnlSlips7ream.Controls.Add(Me.pnlMerge, 1, 2)
    Me.pnlSlips7ream.Controls.Add(Me.pctTitle, 0, 0)
    Me.pnlSlips7ream.Controls.Add(Me.lblISOFeatures, 0, 8)
    Me.pnlSlips7ream.Controls.Add(Me.brUpdates, 0, 4)
    Me.pnlSlips7ream.Controls.Add(Me.brISO, 0, 9)
    Me.pnlSlips7ream.Controls.Add(Me.pnlISOLabel, 1, 7)
    Me.pnlSlips7ream.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlSlips7ream.Location = New System.Drawing.Point(0, 0)
    Me.pnlSlips7ream.Name = "pnlSlips7ream"
    Me.pnlSlips7ream.RowCount = 13
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
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSlips7ream.Size = New System.Drawing.Size(424, 754)
    Me.pnlSlips7ream.TabIndex = 0
    '
    'spltSlips7ream
    '
    Me.pnlSlips7ream.SetColumnSpan(Me.spltSlips7ream, 2)
    Me.spltSlips7ream.Cursor = System.Windows.Forms.Cursors.HSplit
    Me.spltSlips7ream.Dock = System.Windows.Forms.DockStyle.Fill
    Me.spltSlips7ream.DrawGrabHandle = True
    Me.spltSlips7ream.Location = New System.Drawing.Point(0, 100)
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
    Me.spltSlips7ream.Size = New System.Drawing.Size(424, 309)
    Me.spltSlips7ream.SplitterDistance = 167
    Me.spltSlips7ream.SplitterIncrement = 16
    Me.spltSlips7ream.TabIndex = 4
    Me.ttInfo.SetToolTip(Me.spltSlips7ream, "Click and Drag to resize the Image Packages and Updates boxes.")
    '
    'pnlPackages
    '
    Me.pnlPackages.ColumnCount = 2
    Me.pnlPackages.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
    Me.pnlPackages.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPackages.Controls.Add(Me.lblImages, 0, 0)
    Me.pnlPackages.Controls.Add(Me.lvImages, 1, 0)
    Me.pnlPackages.Controls.Add(Me.pnlServicePacks, 0, 3)
    Me.pnlPackages.Controls.Add(Me.brPackages, 0, 2)
    Me.pnlPackages.Controls.Add(Me.pnlLoadPackageData, 0, 1)
    Me.pnlPackages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlPackages.Location = New System.Drawing.Point(0, 0)
    Me.pnlPackages.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPackages.Name = "pnlPackages"
    Me.pnlPackages.RowCount = 4
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPackages.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPackages.Size = New System.Drawing.Size(424, 167)
    Me.pnlPackages.TabIndex = 0
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
    'lvImages
    '
    Me.lvImages.BackColor = System.Drawing.SystemColors.Window
    Me.lvImages.CheckBoxes = True
    Me.lvImages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIndex, Me.colName, Me.colSize})
    Me.lvImages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvImages.FullRowSelect = True
    Me.lvImages.FullRowTooltip = True
    Me.helpS7M.SetHelpKeyword(Me.lvImages, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.0_Image_Packages.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvImages, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvImages.HideSelection = False
    Me.lvImages.Location = New System.Drawing.Point(103, 3)
    Me.lvImages.MultiSelect = False
    Me.lvImages.Name = "lvImages"
    Me.lvImages.ReadOnly = False
    Me.pnlPackages.SetRowSpan(Me.lvImages, 2)
    Me.helpS7M.SetShowHelp(Me.lvImages, True)
    Me.lvImages.Size = New System.Drawing.Size(318, 86)
    Me.lvImages.TabIndex = 1
    Me.lvImages.TooltipTitles = True
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
    'pnlServicePacks
    '
    Me.pnlServicePacks.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlServicePacks.AutoSize = True
    Me.pnlServicePacks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlServicePacks.ColumnCount = 2
    Me.pnlPackages.SetColumnSpan(Me.pnlServicePacks, 2)
    Me.pnlServicePacks.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlServicePacks.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlServicePacks.Controls.Add(Me.pnlSP64, 1, 1)
    Me.pnlServicePacks.Controls.Add(Me.chkSP, 0, 0)
    Me.pnlServicePacks.Controls.Add(Me.pnlSP, 1, 0)
    Me.pnlServicePacks.Controls.Add(Me.lblSP64, 0, 1)
    Me.pnlServicePacks.Location = New System.Drawing.Point(0, 107)
    Me.pnlServicePacks.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlServicePacks.Name = "pnlServicePacks"
    Me.pnlServicePacks.RowCount = 2
    Me.pnlServicePacks.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlServicePacks.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlServicePacks.Size = New System.Drawing.Size(424, 60)
    Me.pnlServicePacks.TabIndex = 2
    '
    'pnlSP64
    '
    Me.pnlSP64.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlSP64.AutoSize = True
    Me.pnlSP64.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlSP64.ColumnCount = 2
    Me.pnlSP64.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP64.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlSP64.Controls.Add(Me.txtSP64, 0, 0)
    Me.pnlSP64.Controls.Add(Me.cmdSP64, 1, 0)
    Me.pnlSP64.Location = New System.Drawing.Point(105, 30)
    Me.pnlSP64.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSP64.Name = "pnlSP64"
    Me.pnlSP64.RowCount = 1
    Me.pnlSP64.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSP64.Size = New System.Drawing.Size(319, 30)
    Me.pnlSP64.TabIndex = 3
    Me.pnlSP64.Visible = False
    '
    'txtSP64
    '
    Me.txtSP64.AllowDrop = True
    Me.txtSP64.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtSP64, "/1_SLIPS7REAM_Interface/1.4_Service_Pack.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtSP64, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtSP64.Location = New System.Drawing.Point(3, 5)
    Me.txtSP64.Name = "txtSP64"
    Me.helpS7M.SetShowHelp(Me.txtSP64, True)
    Me.txtSP64.Size = New System.Drawing.Size(232, 20)
    Me.txtSP64.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtSP64, "Windows 7 x64 Service Pack 1 installer EXE." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Note that x64 Integration requires " & _
        "a 64-bit Operating System.)")
    '
    'cmdSP64
    '
    Me.cmdSP64.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSP64.AutoSize = True
    Me.cmdSP64.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdSP64.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdSP64, "/1_SLIPS7REAM_Interface/1.4_Service_Pack.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdSP64, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdSP64.Location = New System.Drawing.Point(241, 3)
    Me.cmdSP64.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdSP64.Name = "cmdSP64"
    Me.cmdSP64.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdSP64, True)
    Me.cmdSP64.Size = New System.Drawing.Size(75, 24)
    Me.cmdSP64.TabIndex = 1
    Me.cmdSP64.Text = "Browse..."
    Me.ttInfo.SetToolTip(Me.cmdSP64, "Choose an x64 Service Pack EXE.")
    Me.cmdSP64.UseVisualStyleBackColor = True
    '
    'chkSP
    '
    Me.chkSP.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkSP.AutoSize = True
    Me.chkSP.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkSP, "/1_SLIPS7REAM_Interface/1.4_Service_Pack.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkSP, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkSP.Location = New System.Drawing.Point(3, 6)
    Me.chkSP.Name = "chkSP"
    Me.helpS7M.SetShowHelp(Me.chkSP, True)
    Me.chkSP.Size = New System.Drawing.Size(99, 18)
    Me.chkSP.TabIndex = 0
    Me.chkSP.Text = "&Service Pack:"
    Me.ttInfo.SetToolTip(Me.chkSP, "Integrate Windows 7 Service Pack 1 into an RTM Image.")
    Me.chkSP.UseVisualStyleBackColor = True
    '
    'pnlSP
    '
    Me.pnlSP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlSP.AutoSize = True
    Me.pnlSP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlSP.ColumnCount = 2
    Me.pnlSP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlSP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlSP.Controls.Add(Me.txtSP, 0, 0)
    Me.pnlSP.Controls.Add(Me.cmdSP, 1, 0)
    Me.pnlSP.Location = New System.Drawing.Point(105, 0)
    Me.pnlSP.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSP.Name = "pnlSP"
    Me.pnlSP.RowCount = 1
    Me.pnlSP.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSP.Size = New System.Drawing.Size(319, 30)
    Me.pnlSP.TabIndex = 1
    '
    'txtSP
    '
    Me.txtSP.AllowDrop = True
    Me.txtSP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtSP.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.txtSP, "/1_SLIPS7REAM_Interface/1.4_Service_Pack.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtSP, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtSP.Location = New System.Drawing.Point(3, 5)
    Me.txtSP.Name = "txtSP"
    Me.helpS7M.SetShowHelp(Me.txtSP, True)
    Me.txtSP.Size = New System.Drawing.Size(232, 20)
    Me.txtSP.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtSP, "Windows 7 Service Pack 1 installer EXE." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Note that x64 Integration requires a 64" & _
        "-bit Operating System.)")
    '
    'cmdSP
    '
    Me.cmdSP.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSP.AutoSize = True
    Me.cmdSP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdSP.Enabled = False
    Me.cmdSP.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdSP, "/1_SLIPS7REAM_Interface/1.4_Service_Pack.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdSP, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdSP.Location = New System.Drawing.Point(241, 3)
    Me.cmdSP.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdSP.Name = "cmdSP"
    Me.cmdSP.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdSP, True)
    Me.cmdSP.Size = New System.Drawing.Size(75, 24)
    Me.cmdSP.TabIndex = 1
    Me.cmdSP.Text = "Browse..."
    Me.ttInfo.SetToolTip(Me.cmdSP, "Choose a Service Pack EXE.")
    Me.cmdSP.UseVisualStyleBackColor = True
    '
    'lblSP64
    '
    Me.lblSP64.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblSP64.AutoSize = True
    Me.lblSP64.Location = New System.Drawing.Point(3, 38)
    Me.lblSP64.Name = "lblSP64"
    Me.lblSP64.Size = New System.Drawing.Size(94, 13)
    Me.lblSP64.TabIndex = 2
    Me.lblSP64.Text = "x64 Ser&vice Pack:"
    Me.lblSP64.Visible = False
    '
    'brPackages
    '
    Me.brPackages.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.brPackages.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.brPackages.CausesValidation = False
    Me.pnlPackages.SetColumnSpan(Me.brPackages, 2)
    Me.brPackages.Location = New System.Drawing.Point(3, 98)
    Me.brPackages.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.brPackages.Name = "brPackages"
    Me.brPackages.Padding = New System.Windows.Forms.Padding(1)
    Me.brPackages.Size = New System.Drawing.Size(418, 3)
    Me.brPackages.TabIndex = 3
    Me.brPackages.TabStop = False
    '
    'pnlLoadPackageData
    '
    Me.pnlLoadPackageData.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlLoadPackageData.AutoSize = True
    Me.pnlLoadPackageData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlLoadPackageData.ColumnCount = 4
    Me.pnlLoadPackageData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlLoadPackageData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlLoadPackageData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlLoadPackageData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlLoadPackageData.Controls.Add(Me.chkLoadDrivers, 0, 0)
    Me.pnlLoadPackageData.Controls.Add(Me.chkLoadUpdates, 0, 0)
    Me.pnlLoadPackageData.Controls.Add(Me.cmdLoadPackages, 3, 0)
    Me.pnlLoadPackageData.Controls.Add(Me.chkLoadFeatures, 0, 0)
    Me.pnlLoadPackageData.Location = New System.Drawing.Point(1, 66)
    Me.pnlLoadPackageData.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlLoadPackageData.Name = "pnlLoadPackageData"
    Me.pnlLoadPackageData.RowCount = 1
    Me.pnlLoadPackageData.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlLoadPackageData.Size = New System.Drawing.Size(99, 26)
    Me.pnlLoadPackageData.TabIndex = 4
    '
    'chkLoadDrivers
    '
    Me.chkLoadDrivers.Appearance = System.Windows.Forms.Appearance.Button
    Me.helpS7M.SetHelpKeyword(Me.chkLoadDrivers, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.1_Parse_Packages.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkLoadDrivers, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkLoadDrivers.Image = Global.Slips7ream.My.Resources.Resources.load_driver
    Me.chkLoadDrivers.Location = New System.Drawing.Point(48, 1)
    Me.chkLoadDrivers.Margin = New System.Windows.Forms.Padding(0, 1, 1, 1)
    Me.chkLoadDrivers.Name = "chkLoadDrivers"
    Me.helpS7M.SetShowHelp(Me.chkLoadDrivers, True)
    Me.chkLoadDrivers.Size = New System.Drawing.Size(24, 24)
    Me.chkLoadDrivers.TabIndex = 8
    Me.ttInfo.SetToolTip(Me.chkLoadDrivers, "Parse information about Integrated Drivers in Image Packages to prevent duplicate" & _
        " drivers.")
    Me.chkLoadDrivers.UseVisualStyleBackColor = True
    '
    'chkLoadUpdates
    '
    Me.chkLoadUpdates.Appearance = System.Windows.Forms.Appearance.Button
    Me.helpS7M.SetHelpKeyword(Me.chkLoadUpdates, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.1_Parse_Packages.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkLoadUpdates, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkLoadUpdates.Image = Global.Slips7ream.My.Resources.Resources.load_update
    Me.chkLoadUpdates.Location = New System.Drawing.Point(24, 1)
    Me.chkLoadUpdates.Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.chkLoadUpdates.Name = "chkLoadUpdates"
    Me.helpS7M.SetShowHelp(Me.chkLoadUpdates, True)
    Me.chkLoadUpdates.Size = New System.Drawing.Size(24, 24)
    Me.chkLoadUpdates.TabIndex = 7
    Me.ttInfo.SetToolTip(Me.chkLoadUpdates, "Parse information about Integrated Updates in all Image Packages to prevent dupli" & _
        "cate updates.")
    Me.chkLoadUpdates.UseVisualStyleBackColor = True
    '
    'cmdLoadPackages
    '
    Me.cmdLoadPackages.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdLoadPackages.AutoSize = True
    Me.cmdLoadPackages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.helpS7M.SetHelpKeyword(Me.cmdLoadPackages, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.1_Parse_Packages.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdLoadPackages, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdLoadPackages.Image = Global.Slips7ream.My.Resources.Resources.u_n
    Me.cmdLoadPackages.Location = New System.Drawing.Point(74, 1)
    Me.cmdLoadPackages.Margin = New System.Windows.Forms.Padding(1)
    Me.cmdLoadPackages.MinimumSize = New System.Drawing.Size(24, 24)
    Me.cmdLoadPackages.Name = "cmdLoadPackages"
    Me.cmdLoadPackages.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdLoadPackages, True)
    Me.cmdLoadPackages.Size = New System.Drawing.Size(24, 24)
    Me.cmdLoadPackages.TabIndex = 5
    Me.ttInfo.SetToolTip(Me.cmdLoadPackages, resources.GetString("cmdLoadPackages.ToolTip"))
    Me.cmdLoadPackages.UseVisualStyleBackColor = True
    '
    'chkLoadFeatures
    '
    Me.chkLoadFeatures.Appearance = System.Windows.Forms.Appearance.Button
    Me.helpS7M.SetHelpKeyword(Me.chkLoadFeatures, "/1_SLIPS7REAM_Interface/1.3_Image_Packages/1.3.1_Parse_Packages.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkLoadFeatures, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkLoadFeatures.Image = Global.Slips7ream.My.Resources.Resources.load_feature
    Me.chkLoadFeatures.Location = New System.Drawing.Point(0, 1)
    Me.chkLoadFeatures.Margin = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.chkLoadFeatures.Name = "chkLoadFeatures"
    Me.helpS7M.SetShowHelp(Me.chkLoadFeatures, True)
    Me.chkLoadFeatures.Size = New System.Drawing.Size(24, 24)
    Me.chkLoadFeatures.TabIndex = 6
    Me.ttInfo.SetToolTip(Me.chkLoadFeatures, "Parse information about Windows Features in all Image Packages.")
    Me.chkLoadFeatures.UseVisualStyleBackColor = True
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
    Me.pnlUpdates.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdates.Size = New System.Drawing.Size(424, 138)
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
    Me.pnlMSU.Location = New System.Drawing.Point(100, 108)
    Me.pnlMSU.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlMSU.Name = "pnlMSU"
    Me.pnlMSU.RowCount = 1
    Me.pnlMSU.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlMSU.Size = New System.Drawing.Size(324, 30)
    Me.pnlMSU.TabIndex = 2
    '
    'cmdAddMSU
    '
    Me.cmdAddMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdAddMSU.AutoSize = True
    Me.cmdAddMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdAddMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdAddMSU, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.1_Add_Updates.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdAddMSU, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdAddMSU.Location = New System.Drawing.Point(3, 3)
    Me.cmdAddMSU.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdAddMSU.Name = "cmdAddMSU"
    Me.cmdAddMSU.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdAddMSU, True)
    Me.cmdAddMSU.Size = New System.Drawing.Size(92, 24)
    Me.cmdAddMSU.TabIndex = 0
    Me.cmdAddMSU.Text = "&Add Updates..."
    Me.ttInfo.SetToolTip(Me.cmdAddMSU, "Add MSU, CAB, MLC, or Language Pack EXE updates.")
    Me.cmdAddMSU.UseVisualStyleBackColor = True
    '
    'cmdClearMSU
    '
    Me.cmdClearMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdClearMSU.AutoSize = True
    Me.cmdClearMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdClearMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClearMSU.Location = New System.Drawing.Point(233, 3)
    Me.cmdClearMSU.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdClearMSU.Name = "cmdClearMSU"
    Me.cmdClearMSU.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdClearMSU.Size = New System.Drawing.Size(88, 24)
    Me.cmdClearMSU.TabIndex = 3
    Me.cmdClearMSU.Text = "Clear Updates"
    Me.ttInfo.SetToolTip(Me.cmdClearMSU, "Clear the list of Windows Updates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Shortcut: Shift+Delete)")
    Me.cmdClearMSU.UseVisualStyleBackColor = True
    '
    'cmdRemMSU
    '
    Me.cmdRemMSU.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdRemMSU.AutoSize = True
    Me.cmdRemMSU.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdRemMSU.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRemMSU.Location = New System.Drawing.Point(123, 3)
    Me.cmdRemMSU.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdRemMSU.Name = "cmdRemMSU"
    Me.cmdRemMSU.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdRemMSU.Size = New System.Drawing.Size(104, 24)
    Me.cmdRemMSU.TabIndex = 2
    Me.cmdRemMSU.Text = "Remove Updates"
    Me.ttInfo.SetToolTip(Me.cmdRemMSU, "Remove the selected items from the list of Windows Updates." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Shortcut: Delete)" & _
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
    Me.lvMSU.BackColor = System.Drawing.SystemColors.Window
    Me.lvMSU.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colUpdate, Me.colType})
    Me.lvMSU.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lvMSU.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
    Me.helpS7M.SetHelpKeyword(Me.lvMSU, "/1_SLIPS7REAM_Interface/1.5_Updates/1.5.0_Updates.htm")
    Me.helpS7M.SetHelpNavigator(Me.lvMSU, System.Windows.Forms.HelpNavigator.Topic)
    Me.lvMSU.HideSelection = False
    Me.lvMSU.LargeImageList = Me.imlUpdates
    Me.lvMSU.Location = New System.Drawing.Point(103, 3)
    Me.lvMSU.Name = "lvMSU"
    Me.lvMSU.ReadOnly = False
    Me.helpS7M.SetShowHelp(Me.lvMSU, True)
    Me.lvMSU.Size = New System.Drawing.Size(318, 102)
    Me.lvMSU.SmallImageList = Me.imlUpdates
    Me.lvMSU.TabIndex = 1
    Me.lvMSU.TooltipTitles = True
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
    Me.pnlWIM.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlWIM.Size = New System.Drawing.Size(324, 30)
    Me.pnlWIM.TabIndex = 1
    '
    'txtWIM
    '
    Me.txtWIM.AllowDrop = True
    Me.txtWIM.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.helpS7M.SetHelpKeyword(Me.txtWIM, "/1_SLIPS7REAM_Interface/1.1_INSTALL.WIM.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtWIM, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtWIM.Location = New System.Drawing.Point(3, 5)
    Me.txtWIM.Name = "txtWIM"
    Me.helpS7M.SetShowHelp(Me.txtWIM, True)
    Me.txtWIM.Size = New System.Drawing.Size(237, 20)
    Me.txtWIM.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtWIM, "Source WIM or ISO to create image from.")
    '
    'cmdWIM
    '
    Me.cmdWIM.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdWIM.AutoSize = True
    Me.cmdWIM.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdWIM.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdWIM, "/1_SLIPS7REAM_Interface/1.1_INSTALL.WIM.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdWIM, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdWIM.Location = New System.Drawing.Point(246, 3)
    Me.cmdWIM.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdWIM.Name = "cmdWIM"
    Me.cmdWIM.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdWIM, True)
    Me.cmdWIM.Size = New System.Drawing.Size(75, 24)
    Me.cmdWIM.TabIndex = 1
    Me.cmdWIM.Text = "Browse..."
    Me.ttInfo.SetToolTip(Me.cmdWIM, "Choose a WIM or ISO file.")
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
    Me.pnlBottom.Location = New System.Drawing.Point(0, 576)
    Me.pnlBottom.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlBottom.Name = "pnlBottom"
    Me.pnlBottom.RowCount = 1
    Me.pnlBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlBottom.Size = New System.Drawing.Size(424, 30)
    Me.pnlBottom.TabIndex = 11
    '
    'cmdBegin
    '
    Me.cmdBegin.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdBegin.AutoSize = True
    Me.cmdBegin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdBegin.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdBegin.Location = New System.Drawing.Point(265, 3)
    Me.cmdBegin.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdBegin.Name = "cmdBegin"
    Me.cmdBegin.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdBegin.Size = New System.Drawing.Size(75, 24)
    Me.cmdBegin.TabIndex = 1
    Me.cmdBegin.Text = "&Begin"
    Me.ttInfo.SetToolTip(Me.cmdBegin, "Start the Slipstream procedure.")
    Me.cmdBegin.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.AutoSize = True
    Me.cmdClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(346, 3)
    Me.cmdClose.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdClose.Size = New System.Drawing.Size(75, 24)
    Me.cmdClose.TabIndex = 3
    Me.cmdClose.Text = "&Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'lblActivity
    '
    Me.lblActivity.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblActivity.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblActivity.Location = New System.Drawing.Point(28, 7)
    Me.lblActivity.Margin = New System.Windows.Forms.Padding(3)
    Me.lblActivity.Name = "lblActivity"
    Me.lblActivity.Size = New System.Drawing.Size(57, 15)
    Me.lblActivity.TabIndex = 0
    Me.lblActivity.Text = "Idle"
    Me.lblActivity.UseMnemonic = False
    '
    'cmdConfig
    '
    Me.cmdConfig.AutoSize = True
    Me.cmdConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdConfig.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdConfig.Location = New System.Drawing.Point(176, 3)
    Me.cmdConfig.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdConfig.Name = "cmdConfig"
    Me.cmdConfig.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdConfig.Size = New System.Drawing.Size(83, 24)
    Me.cmdConfig.TabIndex = 2
    Me.cmdConfig.Text = "Confi&guration"
    Me.ttInfo.SetToolTip(Me.cmdConfig, "Change SLIPS7REAM settings.")
    Me.cmdConfig.UseVisualStyleBackColor = True
    '
    'expOutput
    '
    Me.expOutput.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.expOutput.AutoSize = True
    Me.expOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.expOutput.Location = New System.Drawing.Point(3, 5)
    Me.expOutput.Name = "expOutput"
    Me.expOutput.Size = New System.Drawing.Size(19, 19)
    Me.expOutput.TabIndex = 4
    Me.expOutput.Text = Nothing
    Me.ttInfo.SetToolTip(Me.expOutput, "Show Output console.")
    '
    'cmdOpenFolder
    '
    Me.cmdOpenFolder.AutoSize = True
    Me.cmdOpenFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdOpenFolder.Location = New System.Drawing.Point(91, 3)
    Me.cmdOpenFolder.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdOpenFolder.Name = "cmdOpenFolder"
    Me.cmdOpenFolder.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.cmdOpenFolder.Size = New System.Drawing.Size(79, 24)
    Me.cmdOpenFolder.TabIndex = 5
    Me.cmdOpenFolder.Text = "Open &Folder"
    Me.ttInfo.SetToolTip(Me.cmdOpenFolder, "Open the folder containing the complete ISO or WIM file.")
    Me.cmdOpenFolder.UseVisualStyleBackColor = True
    Me.cmdOpenFolder.Visible = False
    '
    'chkISO
    '
    Me.chkISO.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkISO.AutoSize = True
    Me.chkISO.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkISO, "/1_SLIPS7REAM_Interface/1.6_Save_to_ISO.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkISO, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkISO.Location = New System.Drawing.Point(3, 430)
    Me.chkISO.Name = "chkISO"
    Me.helpS7M.SetShowHelp(Me.chkISO, True)
    Me.chkISO.Size = New System.Drawing.Size(93, 18)
    Me.chkISO.TabIndex = 5
    Me.chkISO.Text = "Save to &ISO:"
    Me.ttInfo.SetToolTip(Me.chkISO, "Insert the Image into a Windows 7 ISO.")
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
    Me.pnlISO.Location = New System.Drawing.Point(100, 424)
    Me.pnlISO.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlISO.Name = "pnlISO"
    Me.pnlISO.RowCount = 1
    Me.pnlISO.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlISO.Size = New System.Drawing.Size(324, 30)
    Me.pnlISO.TabIndex = 6
    '
    'txtISO
    '
    Me.txtISO.AllowDrop = True
    Me.txtISO.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtISO.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.txtISO, "/1_SLIPS7REAM_Interface/1.6_Save_to_ISO.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtISO, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtISO.Location = New System.Drawing.Point(3, 5)
    Me.txtISO.Name = "txtISO"
    Me.helpS7M.SetShowHelp(Me.txtISO, True)
    Me.txtISO.Size = New System.Drawing.Size(237, 20)
    Me.txtISO.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtISO, "ISO to update with the new image.")
    '
    'cmdISO
    '
    Me.cmdISO.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdISO.AutoSize = True
    Me.cmdISO.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdISO.Enabled = False
    Me.cmdISO.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdISO, "/1_SLIPS7REAM_Interface/1.6_Save_to_ISO.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdISO, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdISO.Location = New System.Drawing.Point(246, 3)
    Me.cmdISO.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdISO.Name = "cmdISO"
    Me.cmdISO.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdISO, True)
    Me.cmdISO.Size = New System.Drawing.Size(75, 24)
    Me.cmdISO.TabIndex = 1
    Me.cmdISO.Text = "Browse..."
    Me.ttInfo.SetToolTip(Me.cmdISO, "Choose a Windows 7 ISO." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Use an x86 ISO if you're merging x86 and x64.)")
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
    Me.pnlProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlProgress.Controls.Add(Me.pbTotal, 0, 0)
    Me.pnlProgress.Controls.Add(Me.pbIndividual, 1, 0)
    Me.pnlProgress.Controls.Add(Me.txtOutput, 0, 2)
    Me.pnlProgress.Controls.Add(Me.pctOutputTear, 0, 1)
    Me.pnlProgress.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProgress.Location = New System.Drawing.Point(0, 606)
    Me.pnlProgress.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlProgress.Name = "pnlProgress"
    Me.pnlProgress.RowCount = 3
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProgress.Size = New System.Drawing.Size(424, 148)
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
    Me.txtOutput.Location = New System.Drawing.Point(3, 45)
    Me.txtOutput.Multiline = True
    Me.txtOutput.Name = "txtOutput"
    Me.txtOutput.ReadOnly = True
    Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtOutput.Size = New System.Drawing.Size(418, 100)
    Me.txtOutput.TabIndex = 2
    Me.txtOutput.Visible = False
    '
    'pctOutputTear
    '
    Me.pctOutputTear.BackColor = System.Drawing.SystemColors.InactiveCaption
    Me.pnlProgress.SetColumnSpan(Me.pctOutputTear, 2)
    Me.pctOutputTear.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctOutputTear.Location = New System.Drawing.Point(0, 22)
    Me.pctOutputTear.Margin = New System.Windows.Forms.Padding(0)
    Me.pctOutputTear.Name = "pctOutputTear"
    Me.pctOutputTear.Size = New System.Drawing.Size(424, 20)
    Me.pctOutputTear.TabIndex = 6
    Me.pctOutputTear.TabStop = False
    Me.ttInfo.SetToolTip(Me.pctOutputTear, "Click and Drag to tear the Output Console into its own window.")
    Me.pctOutputTear.Visible = False
    '
    'lblISOLabel
    '
    Me.lblISOLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblISOLabel.AutoSize = True
    Me.lblISOLabel.Enabled = False
    Me.lblISOLabel.Location = New System.Drawing.Point(3, 460)
    Me.lblISOLabel.Name = "lblISOLabel"
    Me.lblISOLabel.Size = New System.Drawing.Size(57, 13)
    Me.lblISOLabel.TabIndex = 7
    Me.lblISOLabel.Text = "ISO &Label:"
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
    Me.pnlControl.Location = New System.Drawing.Point(0, 549)
    Me.pnlControl.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlControl.Name = "pnlControl"
    Me.pnlControl.RowCount = 1
    Me.pnlControl.RowStyles.Add(New System.Windows.Forms.RowStyle())
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
    Me.helpS7M.SetHelpKeyword(Me.cmbPriority, "/1_SLIPS7REAM_Interface/1.8_Process_Priority.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmbPriority, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmbPriority.Items.AddRange(New Object() {"Realtime", "High", "Above Normal", "Normal", "Below Normal", "Low"})
    Me.cmbPriority.Location = New System.Drawing.Point(103, 3)
    Me.cmbPriority.MaximumSize = New System.Drawing.Size(115, 0)
    Me.cmbPriority.MinimumSize = New System.Drawing.Size(115, 0)
    Me.cmbPriority.Name = "cmbPriority"
    Me.helpS7M.SetShowHelp(Me.cmbPriority, True)
    Me.cmbPriority.Size = New System.Drawing.Size(115, 21)
    Me.cmbPriority.TabIndex = 1
    Me.ttInfo.SetToolTip(Me.cmbPriority, resources.GetString("cmbPriority.ToolTip"))
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
    Me.helpS7M.SetHelpKeyword(Me.cmbCompletion, "/1_SLIPS7REAM_Interface/1.9_On_Completion.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmbCompletion, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmbCompletion.Items.AddRange(New Object() {"Do Nothing", "Play Alert Noise", "Close Program", "Shut Down", "Restart", "Sleep"})
    Me.cmbCompletion.Location = New System.Drawing.Point(307, 3)
    Me.cmbCompletion.MaximumSize = New System.Drawing.Size(115, 0)
    Me.cmbCompletion.MinimumSize = New System.Drawing.Size(115, 0)
    Me.cmbCompletion.Name = "cmbCompletion"
    Me.helpS7M.SetShowHelp(Me.cmbCompletion, True)
    Me.cmbCompletion.Size = New System.Drawing.Size(115, 21)
    Me.cmbCompletion.TabIndex = 4
    Me.ttInfo.SetToolTip(Me.cmbCompletion, "Event to run after Slipstream is complete.")
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
    Me.helpS7M.SetHelpKeyword(Me.pnlISOOptions, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.0_ISO_Features.htm")
    Me.helpS7M.SetHelpNavigator(Me.pnlISOOptions, System.Windows.Forms.HelpNavigator.Topic)
    Me.pnlISOOptions.Location = New System.Drawing.Point(100, 480)
    Me.pnlISOOptions.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlISOOptions.Name = "pnlISOOptions"
    Me.pnlISOOptions.RowCount = 2
    Me.pnlISOOptions.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlISOOptions.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.helpS7M.SetShowHelp(Me.pnlISOOptions, True)
    Me.pnlISOOptions.Size = New System.Drawing.Size(324, 54)
    Me.pnlISOOptions.TabIndex = 9
    '
    'cmbLimitType
    '
    Me.cmbLimitType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbLimitType.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbLimitType.FormattingEnabled = True
    Me.helpS7M.SetHelpKeyword(Me.cmbLimitType, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.5_Split_Method.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmbLimitType, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmbLimitType.Items.AddRange(New Object() {"Single File", "Split WIM", "Split ISO"})
    Me.cmbLimitType.Location = New System.Drawing.Point(124, 30)
    Me.cmbLimitType.Name = "cmbLimitType"
    Me.helpS7M.SetShowHelp(Me.cmbLimitType, True)
    Me.cmbLimitType.Size = New System.Drawing.Size(75, 21)
    Me.cmbLimitType.TabIndex = 4
    Me.ttInfo.SetToolTip(Me.cmbLimitType, resources.GetString("cmbLimitType.ToolTip"))
    '
    'cmbISOFormat
    '
    Me.cmbISOFormat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbISOFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbISOFormat.Enabled = False
    Me.cmbISOFormat.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbISOFormat.FormattingEnabled = True
    Me.helpS7M.SetHelpKeyword(Me.cmbISOFormat, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.4_File_System.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmbISOFormat, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmbISOFormat.Items.AddRange(New Object() {"ISO 9960", "Joliet", "Joliet / ISO 9960", "UDF", "UDF / ISO 9960", "UDF 1.02", "UDF 1.02 / ISO 9960"})
    Me.cmbISOFormat.Location = New System.Drawing.Point(205, 3)
    Me.cmbISOFormat.Name = "cmbISOFormat"
    Me.helpS7M.SetShowHelp(Me.cmbISOFormat, True)
    Me.cmbISOFormat.Size = New System.Drawing.Size(116, 21)
    Me.cmbISOFormat.TabIndex = 2
    Me.ttInfo.SetToolTip(Me.cmbISOFormat, resources.GetString("cmbISOFormat.ToolTip"))
    '
    'chkUnlock
    '
    Me.chkUnlock.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkUnlock.AutoSize = True
    Me.chkUnlock.Enabled = False
    Me.chkUnlock.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkUnlock, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.2_Unlock_All_Editions.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkUnlock, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkUnlock.Location = New System.Drawing.Point(3, 4)
    Me.chkUnlock.Name = "chkUnlock"
    Me.helpS7M.SetShowHelp(Me.chkUnlock, True)
    Me.chkUnlock.Size = New System.Drawing.Size(115, 18)
    Me.chkUnlock.TabIndex = 0
    Me.chkUnlock.Text = "Unlock All &Editions"
    Me.ttInfo.SetToolTip(Me.chkUnlock, "Remove ""ei.cfg"" and the install catalogs from the ISO to allow installation of al" & _
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
    Me.helpS7M.SetHelpKeyword(Me.chkUEFI, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.3_UEFI_Boot.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkUEFI, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkUEFI.Location = New System.Drawing.Point(3, 31)
    Me.chkUEFI.Name = "chkUEFI"
    Me.helpS7M.SetShowHelp(Me.chkUEFI, True)
    Me.chkUEFI.Size = New System.Drawing.Size(81, 18)
    Me.chkUEFI.TabIndex = 3
    Me.chkUEFI.Text = "UEFI B&oot"
    Me.ttInfo.SetToolTip(Me.chkUEFI, resources.GetString("chkUEFI.ToolTip"))
    Me.chkUEFI.UseVisualStyleBackColor = True
    '
    'cmbLimit
    '
    Me.cmbLimit.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbLimit.DropDownWidth = 140
    Me.cmbLimit.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbLimit.FormattingEnabled = True
    Me.helpS7M.SetHelpKeyword(Me.cmbLimit, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.5_Split_Method.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmbLimit, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmbLimit.Location = New System.Drawing.Point(205, 30)
    Me.cmbLimit.Name = "cmbLimit"
    Me.helpS7M.SetShowHelp(Me.cmbLimit, True)
    Me.cmbLimit.Size = New System.Drawing.Size(116, 21)
    Me.cmbLimit.TabIndex = 5
    Me.ttInfo.SetToolTip(Me.cmbLimit, resources.GetString("cmbLimit.ToolTip"))
    '
    'chkMerge
    '
    Me.chkMerge.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkMerge.AutoSize = True
    Me.chkMerge.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkMerge, "/1_SLIPS7REAM_Interface/1.2_Merge_WIM.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkMerge, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkMerge.Location = New System.Drawing.Point(3, 76)
    Me.chkMerge.Name = "chkMerge"
    Me.helpS7M.SetShowHelp(Me.chkMerge, True)
    Me.chkMerge.Size = New System.Drawing.Size(91, 18)
    Me.chkMerge.TabIndex = 2
    Me.chkMerge.Text = "&Merge WIM:"
    Me.ttInfo.SetToolTip(Me.chkMerge, "Merge the Packages of another Image with this one.")
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
    Me.pnlMerge.Location = New System.Drawing.Point(100, 70)
    Me.pnlMerge.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlMerge.Name = "pnlMerge"
    Me.pnlMerge.RowCount = 1
    Me.pnlMerge.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlMerge.Size = New System.Drawing.Size(324, 30)
    Me.pnlMerge.TabIndex = 3
    '
    'txtMerge
    '
    Me.txtMerge.AllowDrop = True
    Me.txtMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtMerge.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.txtMerge, "/1_SLIPS7REAM_Interface/1.2_Merge_WIM.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtMerge, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtMerge.Location = New System.Drawing.Point(3, 5)
    Me.txtMerge.Name = "txtMerge"
    Me.helpS7M.SetShowHelp(Me.txtMerge, True)
    Me.txtMerge.Size = New System.Drawing.Size(237, 20)
    Me.txtMerge.TabIndex = 0
    Me.ttInfo.SetToolTip(Me.txtMerge, "WIM or ISO to Merge with.")
    '
    'cmdMerge
    '
    Me.cmdMerge.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdMerge.AutoSize = True
    Me.cmdMerge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdMerge.Enabled = False
    Me.cmdMerge.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.cmdMerge, "/1_SLIPS7REAM_Interface/1.2_Merge_WIM.htm")
    Me.helpS7M.SetHelpNavigator(Me.cmdMerge, System.Windows.Forms.HelpNavigator.Topic)
    Me.cmdMerge.Location = New System.Drawing.Point(246, 3)
    Me.cmdMerge.MinimumSize = New System.Drawing.Size(75, 24)
    Me.cmdMerge.Name = "cmdMerge"
    Me.cmdMerge.Padding = New System.Windows.Forms.Padding(0, 1, 0, 1)
    Me.helpS7M.SetShowHelp(Me.cmdMerge, True)
    Me.cmdMerge.Size = New System.Drawing.Size(75, 24)
    Me.cmdMerge.TabIndex = 1
    Me.cmdMerge.Text = "Browse..."
    Me.ttInfo.SetToolTip(Me.cmdMerge, "Choose a WIM or ISO file.")
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
    'lblISOFeatures
    '
    Me.lblISOFeatures.AutoSize = True
    Me.lblISOFeatures.Enabled = False
    Me.lblISOFeatures.Location = New System.Drawing.Point(3, 486)
    Me.lblISOFeatures.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
    Me.lblISOFeatures.Name = "lblISOFeatures"
    Me.lblISOFeatures.Size = New System.Drawing.Size(72, 13)
    Me.lblISOFeatures.TabIndex = 23
    Me.lblISOFeatures.Text = "ISO Features:"
    '
    'brUpdates
    '
    Me.brUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.brUpdates.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.brUpdates.CausesValidation = False
    Me.pnlSlips7ream.SetColumnSpan(Me.brUpdates, 2)
    Me.brUpdates.Location = New System.Drawing.Point(3, 415)
    Me.brUpdates.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.brUpdates.Name = "brUpdates"
    Me.brUpdates.Padding = New System.Windows.Forms.Padding(1)
    Me.brUpdates.Size = New System.Drawing.Size(418, 3)
    Me.brUpdates.TabIndex = 24
    Me.brUpdates.TabStop = False
    '
    'brISO
    '
    Me.brISO.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.brISO.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.brISO.CausesValidation = False
    Me.pnlSlips7ream.SetColumnSpan(Me.brISO, 2)
    Me.brISO.Location = New System.Drawing.Point(3, 540)
    Me.brISO.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.brISO.Name = "brISO"
    Me.brISO.Padding = New System.Windows.Forms.Padding(1)
    Me.brISO.Size = New System.Drawing.Size(418, 3)
    Me.brISO.TabIndex = 25
    Me.brISO.TabStop = False
    '
    'pnlISOLabel
    '
    Me.pnlISOLabel.AutoSize = True
    Me.pnlISOLabel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlISOLabel.ColumnCount = 2
    Me.pnlISOLabel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlISOLabel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlISOLabel.Controls.Add(Me.txtISOLabel, 1, 0)
    Me.pnlISOLabel.Controls.Add(Me.chkAutoLabel, 0, 0)
    Me.pnlISOLabel.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlISOLabel.Location = New System.Drawing.Point(100, 454)
    Me.pnlISOLabel.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlISOLabel.Name = "pnlISOLabel"
    Me.pnlISOLabel.RowCount = 1
    Me.pnlISOLabel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlISOLabel.Size = New System.Drawing.Size(324, 26)
    Me.pnlISOLabel.TabIndex = 8
    '
    'txtISOLabel
    '
    Me.txtISOLabel.AllowDrop = True
    Me.txtISOLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtISOLabel.Enabled = False
    Me.helpS7M.SetHelpKeyword(Me.txtISOLabel, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.1_ISO_Label.htm")
    Me.helpS7M.SetHelpNavigator(Me.txtISOLabel, System.Windows.Forms.HelpNavigator.Topic)
    Me.txtISOLabel.Location = New System.Drawing.Point(88, 3)
    Me.txtISOLabel.MaxLength = 32
    Me.txtISOLabel.Name = "txtISOLabel"
    Me.helpS7M.SetShowHelp(Me.txtISOLabel, True)
    Me.txtISOLabel.Size = New System.Drawing.Size(233, 20)
    Me.txtISOLabel.TabIndex = 1
    Me.ttInfo.SetToolTip(Me.txtISOLabel, "Disc Label for ISO.")
    '
    'chkAutoLabel
    '
    Me.chkAutoLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkAutoLabel.AutoSize = True
    Me.chkAutoLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.helpS7M.SetHelpKeyword(Me.chkAutoLabel, "/1_SLIPS7REAM_Interface/1.7_ISO_Features/1.7.1_ISO_Label.htm")
    Me.helpS7M.SetHelpNavigator(Me.chkAutoLabel, System.Windows.Forms.HelpNavigator.Topic)
    Me.chkAutoLabel.Location = New System.Drawing.Point(3, 4)
    Me.chkAutoLabel.Name = "chkAutoLabel"
    Me.helpS7M.SetShowHelp(Me.chkAutoLabel, True)
    Me.chkAutoLabel.Size = New System.Drawing.Size(79, 18)
    Me.chkAutoLabel.TabIndex = 0
    Me.chkAutoLabel.Text = "Read ISO"
    Me.ttInfo.SetToolTip(Me.chkAutoLabel, "Automatically load the Label of the ISO selected in the Save to ISO field above.")
    Me.chkAutoLabel.UseVisualStyleBackColor = True
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
    Me.mnuOutput.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCopy, Me.mnuCopyCommands, Me.mnuClear, Me.mnuSpacer, Me.mnuSelectAll})
    '
    'mnuCopy
    '
    Me.mnuCopy.Index = 0
    Me.mnuCopy.Text = "&Copy"
    '
    'mnuCopyCommands
    '
    Me.mnuCopyCommands.Index = 1
    Me.mnuCopyCommands.Text = "Copy Co&mmands"
    '
    'mnuClear
    '
    Me.mnuClear.Index = 2
    Me.mnuClear.Text = "C&lear"
    '
    'mnuSpacer
    '
    Me.mnuSpacer.Index = 3
    Me.mnuSpacer.Text = "-"
    '
    'mnuSelectAll
    '
    Me.mnuSelectAll.Index = 4
    Me.mnuSelectAll.Text = "Select &All"
    '
    'mnuImages
    '
    Me.mnuImages.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuPackageInclude, Me.mnuPackageRename, Me.mnuPackageSpacer, Me.mnuPackageLocation, Me.mnuPackageProperties})
    '
    'mnuPackageInclude
    '
    Me.mnuPackageInclude.Index = 0
    Me.mnuPackageInclude.Text = "Include in Image"
    '
    'mnuPackageRename
    '
    Me.mnuPackageRename.Index = 1
    Me.mnuPackageRename.Text = "Rename Package"
    '
    'mnuPackageSpacer
    '
    Me.mnuPackageSpacer.Index = 2
    Me.mnuPackageSpacer.Text = "-"
    '
    'mnuPackageLocation
    '
    Me.mnuPackageLocation.Index = 3
    Me.mnuPackageLocation.Text = "Open Image Location"
    '
    'mnuPackageProperties
    '
    Me.mnuPackageProperties.DefaultItem = True
    Me.mnuPackageProperties.Index = 4
    Me.mnuPackageProperties.Text = "Properties"
    '
    'mnuMSU
    '
    Me.mnuMSU.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuUpdateTop, Me.mnuUpdateUp, Me.mnuUpdateDown, Me.mnuUpdateBottom, Me.mnuUpdateSpacer, Me.mnuUpdateRemove, Me.mnuUpdateSpacer2, Me.mnuUpdateLocation, Me.mnuUpdateProperties})
    '
    'mnuUpdateTop
    '
    Me.mnuUpdateTop.Index = 0
    Me.mnuUpdateTop.Text = "Move Updates to Top"
    '
    'mnuUpdateUp
    '
    Me.mnuUpdateUp.Index = 1
    Me.mnuUpdateUp.Text = "Move Updates Up"
    '
    'mnuUpdateDown
    '
    Me.mnuUpdateDown.Index = 2
    Me.mnuUpdateDown.Text = "Move Updates Down"
    '
    'mnuUpdateBottom
    '
    Me.mnuUpdateBottom.Index = 3
    Me.mnuUpdateBottom.Text = "Move Updates to Bottom"
    '
    'mnuUpdateSpacer
    '
    Me.mnuUpdateSpacer.Index = 4
    Me.mnuUpdateSpacer.Text = "-"
    '
    'mnuUpdateRemove
    '
    Me.mnuUpdateRemove.Index = 5
    Me.mnuUpdateRemove.Text = "Remove Updates"
    '
    'mnuUpdateSpacer2
    '
    Me.mnuUpdateSpacer2.Index = 6
    Me.mnuUpdateSpacer2.Text = "-"
    '
    'mnuUpdateLocation
    '
    Me.mnuUpdateLocation.Index = 7
    Me.mnuUpdateLocation.Text = "Open Updates Location"
    '
    'mnuUpdateProperties
    '
    Me.mnuUpdateProperties.DefaultItem = True
    Me.mnuUpdateProperties.Index = 8
    Me.mnuUpdateProperties.Text = "Properties"
    '
    'mnuISOLabel
    '
    Me.mnuISOLabel.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabel7x86, Me.mnuLabel7x64, Me.mnuLabel7AIO, Me.mnuLabelSpace, Me.mnuLabelAuto})
    '
    'mnuLabel7x86
    '
    Me.mnuLabel7x86.Index = 0
    Me.mnuLabel7x86.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCST, Me.mnuLabelGRMCHB, Me.mnuLabelGRMCHP, Me.mnuLabelGRMCPR, Me.mnuLabelGRMCUL, Me.mnuLabelGRMCEN, Me.mnuLabel7x86Space, Me.mnuLabelGRMCMU, Me.mnuLabelGRMCAL})
    Me.mnuLabel7x86.Text = "Windows 7 x86"
    '
    'mnuLabelGRMCST
    '
    Me.mnuLabelGRMCST.Index = 0
    Me.mnuLabelGRMCST.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCSTFRE, Me.mnuLabelGRMCSTCHE})
    Me.mnuLabelGRMCST.Text = "Starter"
    '
    'mnuLabelGRMCSTFRE
    '
    Me.mnuLabelGRMCSTFRE.Index = 0
    Me.mnuLabelGRMCSTFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCSTFRER, Me.mnuLabelGRMCSTFREO, Me.mnuLabelGRMCSTVOL})
    Me.mnuLabelGRMCSTFRE.Text = "Release"
    '
    'mnuLabelGRMCSTFRER
    '
    Me.mnuLabelGRMCSTFRER.Index = 0
    Me.mnuLabelGRMCSTFRER.Text = "Retail"
    '
    'mnuLabelGRMCSTFREO
    '
    Me.mnuLabelGRMCSTFREO.Index = 1
    Me.mnuLabelGRMCSTFREO.Text = "OEM"
    '
    'mnuLabelGRMCSTVOL
    '
    Me.mnuLabelGRMCSTVOL.Index = 2
    Me.mnuLabelGRMCSTVOL.Text = "Volume License"
    '
    'mnuLabelGRMCSTCHE
    '
    Me.mnuLabelGRMCSTCHE.Index = 1
    Me.mnuLabelGRMCSTCHE.Text = "Debug"
    '
    'mnuLabelGRMCHB
    '
    Me.mnuLabelGRMCHB.Index = 1
    Me.mnuLabelGRMCHB.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHBFRE, Me.mnuLabelGRMCHBCHE})
    Me.mnuLabelGRMCHB.Text = "Home Basic"
    '
    'mnuLabelGRMCHBFRE
    '
    Me.mnuLabelGRMCHBFRE.Index = 0
    Me.mnuLabelGRMCHBFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHBFRER, Me.mnuLabelGRMCHBFREO, Me.mnuLabelGRMCHBVOL})
    Me.mnuLabelGRMCHBFRE.Text = "Release"
    '
    'mnuLabelGRMCHBFRER
    '
    Me.mnuLabelGRMCHBFRER.Index = 0
    Me.mnuLabelGRMCHBFRER.Text = "Retail"
    '
    'mnuLabelGRMCHBFREO
    '
    Me.mnuLabelGRMCHBFREO.Index = 1
    Me.mnuLabelGRMCHBFREO.Text = "OEM"
    '
    'mnuLabelGRMCHBVOL
    '
    Me.mnuLabelGRMCHBVOL.Index = 2
    Me.mnuLabelGRMCHBVOL.Text = "Volume License"
    '
    'mnuLabelGRMCHBCHE
    '
    Me.mnuLabelGRMCHBCHE.Index = 1
    Me.mnuLabelGRMCHBCHE.Text = "Debug"
    '
    'mnuLabelGRMCHP
    '
    Me.mnuLabelGRMCHP.Index = 2
    Me.mnuLabelGRMCHP.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHPFRE, Me.mnuLabelGRMCHPCHE})
    Me.mnuLabelGRMCHP.Text = "Home Premium"
    '
    'mnuLabelGRMCHPFRE
    '
    Me.mnuLabelGRMCHPFRE.Index = 0
    Me.mnuLabelGRMCHPFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHPFRER, Me.mnuLabelGRMCHPFREO, Me.mnuLabelGRMCHPVOL})
    Me.mnuLabelGRMCHPFRE.Text = "Release"
    '
    'mnuLabelGRMCHPFRER
    '
    Me.mnuLabelGRMCHPFRER.Index = 0
    Me.mnuLabelGRMCHPFRER.Text = "Retail"
    '
    'mnuLabelGRMCHPFREO
    '
    Me.mnuLabelGRMCHPFREO.Index = 1
    Me.mnuLabelGRMCHPFREO.Text = "OEM"
    '
    'mnuLabelGRMCHPVOL
    '
    Me.mnuLabelGRMCHPVOL.Index = 2
    Me.mnuLabelGRMCHPVOL.Text = "Volume License"
    '
    'mnuLabelGRMCHPCHE
    '
    Me.mnuLabelGRMCHPCHE.Index = 1
    Me.mnuLabelGRMCHPCHE.Text = "Debug"
    '
    'mnuLabelGRMCPR
    '
    Me.mnuLabelGRMCPR.Index = 3
    Me.mnuLabelGRMCPR.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCPRFRE, Me.mnuLabelGRMCPRCHE})
    Me.mnuLabelGRMCPR.Text = "Professional"
    '
    'mnuLabelGRMCPRFRE
    '
    Me.mnuLabelGRMCPRFRE.Index = 0
    Me.mnuLabelGRMCPRFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCPRFRER, Me.mnuLabelGRMCPRFREO, Me.mnuLabelGRMCPRVOL})
    Me.mnuLabelGRMCPRFRE.Text = "Release"
    '
    'mnuLabelGRMCPRFRER
    '
    Me.mnuLabelGRMCPRFRER.Index = 0
    Me.mnuLabelGRMCPRFRER.Text = "Retail"
    '
    'mnuLabelGRMCPRFREO
    '
    Me.mnuLabelGRMCPRFREO.Index = 1
    Me.mnuLabelGRMCPRFREO.Text = "OEM"
    '
    'mnuLabelGRMCPRVOL
    '
    Me.mnuLabelGRMCPRVOL.Index = 2
    Me.mnuLabelGRMCPRVOL.Text = "Volume License"
    '
    'mnuLabelGRMCPRCHE
    '
    Me.mnuLabelGRMCPRCHE.Index = 1
    Me.mnuLabelGRMCPRCHE.Text = "Debug"
    '
    'mnuLabelGRMCUL
    '
    Me.mnuLabelGRMCUL.Index = 4
    Me.mnuLabelGRMCUL.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCULFRE, Me.mnuLabelGRMCULCHE})
    Me.mnuLabelGRMCUL.Text = "Ultimate"
    '
    'mnuLabelGRMCULFRE
    '
    Me.mnuLabelGRMCULFRE.Index = 0
    Me.mnuLabelGRMCULFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCULFRER, Me.mnuLabelGRMCULFREO, Me.mnuLabelGRMCULVOL})
    Me.mnuLabelGRMCULFRE.Text = "Release"
    '
    'mnuLabelGRMCULFRER
    '
    Me.mnuLabelGRMCULFRER.Index = 0
    Me.mnuLabelGRMCULFRER.Text = "Retail"
    '
    'mnuLabelGRMCULFREO
    '
    Me.mnuLabelGRMCULFREO.Index = 1
    Me.mnuLabelGRMCULFREO.Text = "OEM"
    '
    'mnuLabelGRMCULVOL
    '
    Me.mnuLabelGRMCULVOL.Index = 2
    Me.mnuLabelGRMCULVOL.Text = "Volume License"
    '
    'mnuLabelGRMCULCHE
    '
    Me.mnuLabelGRMCULCHE.Index = 1
    Me.mnuLabelGRMCULCHE.Text = "Debug"
    '
    'mnuLabelGRMCEN
    '
    Me.mnuLabelGRMCEN.Index = 5
    Me.mnuLabelGRMCEN.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCENVOL, Me.mnuLabelGRMCENCHE})
    Me.mnuLabelGRMCEN.Text = "Enterprise"
    '
    'mnuLabelGRMCENVOL
    '
    Me.mnuLabelGRMCENVOL.Index = 0
    Me.mnuLabelGRMCENVOL.Text = "Volume License"
    '
    'mnuLabelGRMCENCHE
    '
    Me.mnuLabelGRMCENCHE.Index = 1
    Me.mnuLabelGRMCENCHE.Text = "Debug"
    '
    'mnuLabel7x86Space
    '
    Me.mnuLabel7x86Space.Index = 6
    Me.mnuLabel7x86Space.Text = "-"
    '
    'mnuLabelGRMCMU
    '
    Me.mnuLabelGRMCMU.Index = 7
    Me.mnuLabelGRMCMU.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUFRE, Me.mnuLabelGRMCMUCHE})
    Me.mnuLabelGRMCMU.Text = "Multiple"
    '
    'mnuLabelGRMCMUFRE
    '
    Me.mnuLabelGRMCMUFRE.Index = 0
    Me.mnuLabelGRMCMUFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUFRER, Me.mnuLabelGRMCMUFREO, Me.mnuLabelGRMCMUVOL})
    Me.mnuLabelGRMCMUFRE.Text = "Release"
    '
    'mnuLabelGRMCMUFRER
    '
    Me.mnuLabelGRMCMUFRER.Index = 0
    Me.mnuLabelGRMCMUFRER.Text = "Retail"
    '
    'mnuLabelGRMCMUFREO
    '
    Me.mnuLabelGRMCMUFREO.Index = 1
    Me.mnuLabelGRMCMUFREO.Text = "OEM"
    '
    'mnuLabelGRMCMUVOL
    '
    Me.mnuLabelGRMCMUVOL.Index = 2
    Me.mnuLabelGRMCMUVOL.Text = "Volume License"
    '
    'mnuLabelGRMCMUCHE
    '
    Me.mnuLabelGRMCMUCHE.Index = 1
    Me.mnuLabelGRMCMUCHE.Text = "Debug"
    '
    'mnuLabelGRMCAL
    '
    Me.mnuLabelGRMCAL.Index = 8
    Me.mnuLabelGRMCAL.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCALFRE, Me.mnuLabelGRMCALCHE})
    Me.mnuLabelGRMCAL.Text = "All-in-One"
    '
    'mnuLabelGRMCALFRE
    '
    Me.mnuLabelGRMCALFRE.Index = 0
    Me.mnuLabelGRMCALFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCALFRER, Me.mnuLabelGRMCALFREO, Me.mnuLabelGRMCALVOL})
    Me.mnuLabelGRMCALFRE.Text = "Release"
    '
    'mnuLabelGRMCALFRER
    '
    Me.mnuLabelGRMCALFRER.Index = 0
    Me.mnuLabelGRMCALFRER.Text = "Retail"
    '
    'mnuLabelGRMCALFREO
    '
    Me.mnuLabelGRMCALFREO.Index = 1
    Me.mnuLabelGRMCALFREO.Text = "OEM"
    '
    'mnuLabelGRMCALVOL
    '
    Me.mnuLabelGRMCALVOL.Index = 2
    Me.mnuLabelGRMCALVOL.Text = "Volume License"
    '
    'mnuLabelGRMCALCHE
    '
    Me.mnuLabelGRMCALCHE.Index = 1
    Me.mnuLabelGRMCALCHE.Text = "Debug"
    '
    'mnuLabel7x64
    '
    Me.mnuLabel7x64.Index = 1
    Me.mnuLabel7x64.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHBX, Me.mnuLabelGRMCHPX, Me.mnuLabelGRMCPRX, Me.mnuLabelGRMCULX, Me.mnuLabelGRMCENX, Me.mnuLabel7x64Space, Me.mnuLabelGRMCMUX, Me.mnuLabelGRMCALX})
    Me.mnuLabel7x64.Text = "Windows 7 x64"
    '
    'mnuLabelGRMCHBX
    '
    Me.mnuLabelGRMCHBX.Index = 0
    Me.mnuLabelGRMCHBX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHBXFRE, Me.mnuLabelGRMCHBXCHE})
    Me.mnuLabelGRMCHBX.Text = "Home Basic"
    '
    'mnuLabelGRMCHBXFRE
    '
    Me.mnuLabelGRMCHBXFRE.Index = 0
    Me.mnuLabelGRMCHBXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHBXFRER, Me.mnuLabelGRMCHBXFREO, Me.mnuLabelGRMCHBXVOL})
    Me.mnuLabelGRMCHBXFRE.Text = "Release"
    '
    'mnuLabelGRMCHBXFRER
    '
    Me.mnuLabelGRMCHBXFRER.Index = 0
    Me.mnuLabelGRMCHBXFRER.Text = "Retail"
    '
    'mnuLabelGRMCHBXFREO
    '
    Me.mnuLabelGRMCHBXFREO.Index = 1
    Me.mnuLabelGRMCHBXFREO.Text = "OEM"
    '
    'mnuLabelGRMCHBXVOL
    '
    Me.mnuLabelGRMCHBXVOL.Index = 2
    Me.mnuLabelGRMCHBXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCHBXCHE
    '
    Me.mnuLabelGRMCHBXCHE.Index = 1
    Me.mnuLabelGRMCHBXCHE.Text = "Debug"
    '
    'mnuLabelGRMCHPX
    '
    Me.mnuLabelGRMCHPX.Index = 1
    Me.mnuLabelGRMCHPX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHPXFRE, Me.mnuLabelGRMCHPXCHE})
    Me.mnuLabelGRMCHPX.Text = "Home Premium"
    '
    'mnuLabelGRMCHPXFRE
    '
    Me.mnuLabelGRMCHPXFRE.Index = 0
    Me.mnuLabelGRMCHPXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCHPXFRER, Me.mnuLabelGRMCHPXFREO, Me.mnuLabelGRMCHPXVOL})
    Me.mnuLabelGRMCHPXFRE.Text = "Release"
    '
    'mnuLabelGRMCHPXFRER
    '
    Me.mnuLabelGRMCHPXFRER.Index = 0
    Me.mnuLabelGRMCHPXFRER.Text = "Retail"
    '
    'mnuLabelGRMCHPXFREO
    '
    Me.mnuLabelGRMCHPXFREO.Index = 1
    Me.mnuLabelGRMCHPXFREO.Text = "OEM"
    '
    'mnuLabelGRMCHPXVOL
    '
    Me.mnuLabelGRMCHPXVOL.Index = 2
    Me.mnuLabelGRMCHPXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCHPXCHE
    '
    Me.mnuLabelGRMCHPXCHE.Index = 1
    Me.mnuLabelGRMCHPXCHE.Text = "Debug"
    '
    'mnuLabelGRMCPRX
    '
    Me.mnuLabelGRMCPRX.Index = 2
    Me.mnuLabelGRMCPRX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCPRXFRE, Me.mnuLabelGRMCPRXCHE})
    Me.mnuLabelGRMCPRX.Text = "Professional"
    '
    'mnuLabelGRMCPRXFRE
    '
    Me.mnuLabelGRMCPRXFRE.Index = 0
    Me.mnuLabelGRMCPRXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCPRXFRER, Me.mnuLabelGRMCPRXFREO, Me.mnuLabelGRMCPRXVOL})
    Me.mnuLabelGRMCPRXFRE.Text = "Release"
    '
    'mnuLabelGRMCPRXFRER
    '
    Me.mnuLabelGRMCPRXFRER.Index = 0
    Me.mnuLabelGRMCPRXFRER.Text = "Retail"
    '
    'mnuLabelGRMCPRXFREO
    '
    Me.mnuLabelGRMCPRXFREO.Index = 1
    Me.mnuLabelGRMCPRXFREO.Text = "OEM"
    '
    'mnuLabelGRMCPRXVOL
    '
    Me.mnuLabelGRMCPRXVOL.Index = 2
    Me.mnuLabelGRMCPRXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCPRXCHE
    '
    Me.mnuLabelGRMCPRXCHE.Index = 1
    Me.mnuLabelGRMCPRXCHE.Text = "Debug"
    '
    'mnuLabelGRMCULX
    '
    Me.mnuLabelGRMCULX.Index = 3
    Me.mnuLabelGRMCULX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCULXFRE, Me.mnuLabelGRMCULXCHE})
    Me.mnuLabelGRMCULX.Text = "Ultimate"
    '
    'mnuLabelGRMCULXFRE
    '
    Me.mnuLabelGRMCULXFRE.Index = 0
    Me.mnuLabelGRMCULXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCULXFRER, Me.mnuLabelGRMCULXFREO, Me.mnuLabelGRMCULXVOL})
    Me.mnuLabelGRMCULXFRE.Text = "Release"
    '
    'mnuLabelGRMCULXFRER
    '
    Me.mnuLabelGRMCULXFRER.Index = 0
    Me.mnuLabelGRMCULXFRER.Text = "Retail"
    '
    'mnuLabelGRMCULXFREO
    '
    Me.mnuLabelGRMCULXFREO.Index = 1
    Me.mnuLabelGRMCULXFREO.Text = "OEM"
    '
    'mnuLabelGRMCULXVOL
    '
    Me.mnuLabelGRMCULXVOL.Index = 2
    Me.mnuLabelGRMCULXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCULXCHE
    '
    Me.mnuLabelGRMCULXCHE.Index = 1
    Me.mnuLabelGRMCULXCHE.Text = "Debug"
    '
    'mnuLabelGRMCENX
    '
    Me.mnuLabelGRMCENX.Index = 4
    Me.mnuLabelGRMCENX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCENXVOL, Me.mnuLabelGRMCENXCHE})
    Me.mnuLabelGRMCENX.Text = "Enterprise"
    '
    'mnuLabelGRMCENXVOL
    '
    Me.mnuLabelGRMCENXVOL.Index = 0
    Me.mnuLabelGRMCENXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCENXCHE
    '
    Me.mnuLabelGRMCENXCHE.Index = 1
    Me.mnuLabelGRMCENXCHE.Text = "Debug"
    '
    'mnuLabel7x64Space
    '
    Me.mnuLabel7x64Space.Index = 5
    Me.mnuLabel7x64Space.Text = "-"
    '
    'mnuLabelGRMCMUX
    '
    Me.mnuLabelGRMCMUX.Index = 6
    Me.mnuLabelGRMCMUX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUXFRE, Me.mnuLabelGRMCMUXCHE})
    Me.mnuLabelGRMCMUX.Text = "Multiple"
    '
    'mnuLabelGRMCMUXFRE
    '
    Me.mnuLabelGRMCMUXFRE.Index = 0
    Me.mnuLabelGRMCMUXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUXFRER, Me.mnuLabelGRMCMUXFREO, Me.mnuLabelGRMCMUXVOL})
    Me.mnuLabelGRMCMUXFRE.Text = "Release"
    '
    'mnuLabelGRMCMUXFRER
    '
    Me.mnuLabelGRMCMUXFRER.Index = 0
    Me.mnuLabelGRMCMUXFRER.Text = "Retail"
    '
    'mnuLabelGRMCMUXFREO
    '
    Me.mnuLabelGRMCMUXFREO.Index = 1
    Me.mnuLabelGRMCMUXFREO.Text = "OEM"
    '
    'mnuLabelGRMCMUXVOL
    '
    Me.mnuLabelGRMCMUXVOL.Index = 2
    Me.mnuLabelGRMCMUXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCMUXCHE
    '
    Me.mnuLabelGRMCMUXCHE.Index = 1
    Me.mnuLabelGRMCMUXCHE.Text = "Debug"
    '
    'mnuLabelGRMCALX
    '
    Me.mnuLabelGRMCALX.Index = 7
    Me.mnuLabelGRMCALX.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCALXFRE, Me.mnuLabelGRMCALXCHE})
    Me.mnuLabelGRMCALX.Text = "All-in-One"
    '
    'mnuLabelGRMCALXFRE
    '
    Me.mnuLabelGRMCALXFRE.Index = 0
    Me.mnuLabelGRMCALXFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCALXFRER, Me.mnuLabelGRMCALXFREO, Me.mnuLabelGRMCALXVOL})
    Me.mnuLabelGRMCALXFRE.Text = "Release"
    '
    'mnuLabelGRMCALXFRER
    '
    Me.mnuLabelGRMCALXFRER.Index = 0
    Me.mnuLabelGRMCALXFRER.Text = "Retail"
    '
    'mnuLabelGRMCALXFREO
    '
    Me.mnuLabelGRMCALXFREO.Index = 1
    Me.mnuLabelGRMCALXFREO.Text = "OEM"
    '
    'mnuLabelGRMCALXVOL
    '
    Me.mnuLabelGRMCALXVOL.Index = 2
    Me.mnuLabelGRMCALXVOL.Text = "Volume License"
    '
    'mnuLabelGRMCALXCHE
    '
    Me.mnuLabelGRMCALXCHE.Index = 1
    Me.mnuLabelGRMCALXCHE.Text = "Debug"
    '
    'mnuLabel7AIO
    '
    Me.mnuLabel7AIO.Index = 2
    Me.mnuLabel7AIO.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUU, Me.mnuLabelGRMCSTA})
    Me.mnuLabel7AIO.Text = "Windows 7 AIO"
    '
    'mnuLabelGRMCMUU
    '
    Me.mnuLabelGRMCMUU.Index = 0
    Me.mnuLabelGRMCMUU.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUUFRE, Me.mnuLabelGRMCMUUCHE})
    Me.mnuLabelGRMCMUU.Text = "Mutiple"
    '
    'mnuLabelGRMCMUUFRE
    '
    Me.mnuLabelGRMCMUUFRE.Index = 0
    Me.mnuLabelGRMCMUUFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCMUUFRER, Me.mnuLabelGRMCMUUFREO, Me.mnuLabelGRMCMUUVOL})
    Me.mnuLabelGRMCMUUFRE.Text = "Release"
    '
    'mnuLabelGRMCMUUFRER
    '
    Me.mnuLabelGRMCMUUFRER.Index = 0
    Me.mnuLabelGRMCMUUFRER.Text = "Retail"
    '
    'mnuLabelGRMCMUUFREO
    '
    Me.mnuLabelGRMCMUUFREO.Index = 1
    Me.mnuLabelGRMCMUUFREO.Text = "OEM"
    '
    'mnuLabelGRMCMUUVOL
    '
    Me.mnuLabelGRMCMUUVOL.Index = 2
    Me.mnuLabelGRMCMUUVOL.Text = "Volume License"
    '
    'mnuLabelGRMCMUUCHE
    '
    Me.mnuLabelGRMCMUUCHE.Index = 1
    Me.mnuLabelGRMCMUUCHE.Text = "Debug"
    '
    'mnuLabelGRMCSTA
    '
    Me.mnuLabelGRMCSTA.Index = 1
    Me.mnuLabelGRMCSTA.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCSTAFRE, Me.mnuLabelGRMCSTACHE})
    Me.mnuLabelGRMCSTA.Text = "All-in-One"
    '
    'mnuLabelGRMCSTAFRE
    '
    Me.mnuLabelGRMCSTAFRE.Index = 0
    Me.mnuLabelGRMCSTAFRE.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLabelGRMCSTAFRER, Me.mnuLabelGRMCSTAFREO, Me.mnuLabelGRMCSTAVOL})
    Me.mnuLabelGRMCSTAFRE.Text = "Release"
    '
    'mnuLabelGRMCSTAFRER
    '
    Me.mnuLabelGRMCSTAFRER.Index = 0
    Me.mnuLabelGRMCSTAFRER.Text = "Retail"
    '
    'mnuLabelGRMCSTAFREO
    '
    Me.mnuLabelGRMCSTAFREO.Index = 1
    Me.mnuLabelGRMCSTAFREO.Text = "OEM"
    '
    'mnuLabelGRMCSTAVOL
    '
    Me.mnuLabelGRMCSTAVOL.Index = 2
    Me.mnuLabelGRMCSTAVOL.Text = "Volume License"
    '
    'mnuLabelGRMCSTACHE
    '
    Me.mnuLabelGRMCSTACHE.Index = 1
    Me.mnuLabelGRMCSTACHE.Text = "Debug"
    '
    'mnuLabelSpace
    '
    Me.mnuLabelSpace.Index = 3
    Me.mnuLabelSpace.Text = "-"
    '
    'mnuLabelAuto
    '
    Me.mnuLabelAuto.Index = 4
    Me.mnuLabelAuto.Text = "Auto-Detect"
    '
    'helpS7M
    '
    Me.helpS7M.HelpNamespace = "S7M.chm"
    '
    'ttInfo
    '
    Me.ttInfo.AutoPopDelay = 30000
    Me.ttInfo.InitialDelay = 500
    Me.ttInfo.ReshowDelay = 100
    '
    'ttActivity
    '
    Me.ttActivity.HideOnHover = False
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(424, 754)
    Me.Controls.Add(Me.pnlSlips7ream)
    Me.helpS7M.SetHelpKeyword(Me, "/1_SLIPS7REAM_Interface/1.0_SLIPS7REAM_Interface.htm")
    Me.helpS7M.SetHelpNavigator(Me, System.Windows.Forms.HelpNavigator.Topic)
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MinimumSize = New System.Drawing.Size(440, 556)
    Me.Name = "frmMain"
    Me.helpS7M.SetShowHelp(Me, True)
    Me.Text = "SLIPS7REAM - Windows 7 Image Slipstream Utility"
    Me.pnlSlips7ream.ResumeLayout(False)
    Me.pnlSlips7ream.PerformLayout()
    Me.spltSlips7ream.Panel1.ResumeLayout(False)
    Me.spltSlips7ream.Panel2.ResumeLayout(False)
    CType(Me.spltSlips7ream, System.ComponentModel.ISupportInitialize).EndInit()
    Me.spltSlips7ream.ResumeLayout(False)
    Me.pnlPackages.ResumeLayout(False)
    Me.pnlPackages.PerformLayout()
    Me.pnlServicePacks.ResumeLayout(False)
    Me.pnlServicePacks.PerformLayout()
    Me.pnlSP64.ResumeLayout(False)
    Me.pnlSP64.PerformLayout()
    Me.pnlSP.ResumeLayout(False)
    Me.pnlSP.PerformLayout()
    Me.pnlLoadPackageData.ResumeLayout(False)
    Me.pnlLoadPackageData.PerformLayout()
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
    CType(Me.pctOutputTear, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlControl.ResumeLayout(False)
    Me.pnlControl.PerformLayout()
    Me.pnlISOOptions.ResumeLayout(False)
    Me.pnlISOOptions.PerformLayout()
    Me.pnlMerge.ResumeLayout(False)
    Me.pnlMerge.PerformLayout()
    CType(Me.pctTitle, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlISOLabel.ResumeLayout(False)
    Me.pnlISOLabel.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlSlips7ream As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblWIM As System.Windows.Forms.Label
  Friend WithEvents txtWIM As System.Windows.Forms.TextBox
  Friend WithEvents cmdWIM As System.Windows.Forms.Button
  Friend WithEvents txtSP As System.Windows.Forms.TextBox
  Friend WithEvents cmdSP As System.Windows.Forms.Button
  Friend WithEvents lblMSU As System.Windows.Forms.Label
  Friend WithEvents lvMSU As ListViewEx
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
  Friend WithEvents pnlControl As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPriority As System.Windows.Forms.Label
  Friend WithEvents cmbPriority As System.Windows.Forms.ComboBox
  Friend WithEvents lblCompletion As System.Windows.Forms.Label
  Friend WithEvents pnlISOOptions As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblISOFS As System.Windows.Forms.Label
  Friend WithEvents cmbISOFormat As System.Windows.Forms.ComboBox
  Friend WithEvents txtOutput As System.Windows.Forms.TextBox
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
  Friend WithEvents pnlServicePacks As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblISOFeatures As System.Windows.Forms.Label
  Friend WithEvents brPackages As Slips7ream.LineBreak
  Friend WithEvents brUpdates As Slips7ream.LineBreak
  Friend WithEvents brISO As Slips7ream.LineBreak
  Friend WithEvents pctOutputTear As System.Windows.Forms.PictureBox
  Friend WithEvents mnuImages As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuPackageInclude As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPackageRename As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPackageSpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPackageLocation As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPackageProperties As System.Windows.Forms.MenuItem
  Friend WithEvents mnuMSU As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuUpdateLocation As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateProperties As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateTop As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateUp As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateDown As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateBottom As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateSpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateRemove As System.Windows.Forms.MenuItem
  Friend WithEvents mnuUpdateSpacer2 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCST As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHB As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHP As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPR As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCUL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCEN As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabel7x64 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCENVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCENCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMU As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCAL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabel7AIO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUU As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTA As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUUFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUUFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUUFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUUVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUUCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabel7x86Space As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelSpace As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelAuto As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHBXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTAFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTAFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTAFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTAVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCSTACHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCHPXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCULXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCENX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCENXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCENXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabel7x64Space As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCMUXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALX As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALXFRE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALXFRER As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALXVOL As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCALXCHE As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabelGRMCPRXFREO As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLabel7x86 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuISOLabel As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuLabelGRMCSTFRER As System.Windows.Forms.MenuItem
  Friend WithEvents pnlLoadPackageData As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdLoadPackages As System.Windows.Forms.Button
  Friend WithEvents chkLoadFeatures As System.Windows.Forms.CheckBox
  Friend WithEvents chkLoadDrivers As System.Windows.Forms.CheckBox
  Friend WithEvents chkLoadUpdates As System.Windows.Forms.CheckBox
  Friend WithEvents lvImages As Slips7ream.ListViewEx
  Friend WithEvents ttActivity As Slips7ream.ToolTip
  Friend WithEvents helpS7M As System.Windows.Forms.HelpProvider
  Friend WithEvents pnlISOLabel As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtISOLabel As System.Windows.Forms.TextBox
  Friend WithEvents chkAutoLabel As System.Windows.Forms.CheckBox
  Friend WithEvents mnuCopyCommands As System.Windows.Forms.MenuItem

End Class
