Imports Microsoft.WindowsAPICodePack.Dialogs
Public Class frmMain
  Implements IMessageFilter
#Region "Friend Variables"
  Friend RunActivity As ActivityType = ActivityType.Nothing
  Friend StopRun As Boolean = False
  Friend taskBar As TaskbarLib.TaskbarListClass
#End Region
#Region "Private Variables"
  Private mySettings As MySettings
  Private isStarting As Boolean = False
  Private RunComplete As Boolean = False
  Private windowChangedSize As Boolean = False
#End Region
#Region "Paths"
  Private ReadOnly Property WorkDir As String
    Get
      Dim tempDir As String = mySettings.TempDir
      If String.IsNullOrEmpty(tempDir) Then tempDir = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "Slips7ream")
      If Not IO.Directory.Exists(tempDir) Then IO.Directory.CreateDirectory(tempDir)
      Return tempDir
    End Get
  End Property
  Private ReadOnly Property Work As String
    Get
      Dim sDir As String = IO.Path.Combine(WorkDir, "WORK")
      If Not IO.Directory.Exists(sDir) Then IO.Directory.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
  Private ReadOnly Property Mount(Optional ForBoot As Boolean = False) As String
    Get
      Dim sDir As String = IO.Path.Combine(WorkDir, "MOUNT")
      If ForBoot Then sDir = IO.Path.Combine(WorkDir, "MBOOT")
      If Not IO.Directory.Exists(sDir) Then IO.Directory.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
#End Region
#Region "GUI"
  Private GUI_Resize_WindowState As FormWindowState
  Private Delegate Sub GUI_ToggleEnabledInvoker(bEnabled As Boolean, sStatus As String)
  Public Sub New()
    InitializeComponent()
    Application.AddMessageFilter(Me)
  End Sub
  Private Sub frmMain_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
    ConsoleOutput_Title_Redraw()
  End Sub
  Private Sub frmMain_Deactivate(sender As Object, e As System.EventArgs) Handles Me.Deactivate
    ConsoleOutput_Title_Redraw()
  End Sub
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    tmrTitleMNG = New ThreadedTimer(Me, 1000)
    isStarting = True
    mySettings = New MySettings
    imlUpdates.Images.Add("MSU", My.Resources.update_wusa)
    imlUpdates.Images.Add("DIR", My.Resources.update_folder)
    imlUpdates.Images.Add("CAB", My.Resources.update_cab)
    imlUpdates.Images.Add("MLC", My.Resources.update_mlc)
    imlUpdates.Images.Add("INF", My.Resources.inf)
    ttInfo.SetToolTip(expOutput.pctExpander, "Show Output Console.")
    If String.IsNullOrEmpty(mySettings.DefaultFS) Then
      cmbISOFormat.SelectedIndex = 0
    Else
      Try
        cmbISOFormat.Text = mySettings.DefaultFS
      Catch ex As Exception
        cmbISOFormat.SelectedIndex = 0
      End Try
    End If
    If mySettings.DefaultSplit > 0 Then
      Dim SplitVal As String = Nothing
      If Not String.IsNullOrEmpty(mySettings.SplitVal) Then
        Try
          SplitVal = mySettings.SplitVal
        Catch ex As Exception
          SplitVal = Nothing
        End Try
      End If
      If cmbLimitType.Items.Count >= mySettings.DefaultSplit Then
        cmbLimitType.SelectedIndex = mySettings.DefaultSplit
      Else
        cmbLimitType.SelectedIndex = 0
      End If
      Limit_SetValues(cmbLimitType.SelectedIndex)
      If Not String.IsNullOrEmpty(SplitVal) Then cmbLimit.Text = SplitVal
    Else
      cmbLimitType.SelectedIndex = 0
    End If
    If String.IsNullOrEmpty(mySettings.Priority) Then
      Select Case Process.GetCurrentProcess.PriorityClass
        Case ProcessPriorityClass.RealTime : cmbPriority.SelectedIndex = 0
        Case ProcessPriorityClass.High : cmbPriority.SelectedIndex = 1
        Case ProcessPriorityClass.AboveNormal : cmbPriority.SelectedIndex = 2
        Case ProcessPriorityClass.Normal : cmbPriority.SelectedIndex = 3
        Case ProcessPriorityClass.BelowNormal : cmbPriority.SelectedIndex = 4
        Case ProcessPriorityClass.Idle : cmbPriority.SelectedIndex = 5
        Case Else : cmbPriority.SelectedIndex = 3
      End Select
    Else
      Try
        cmbPriority.Text = mySettings.Priority
      Catch ex As Exception
        Select Case Process.GetCurrentProcess.PriorityClass
          Case ProcessPriorityClass.RealTime : cmbPriority.SelectedIndex = 0
          Case ProcessPriorityClass.High : cmbPriority.SelectedIndex = 1
          Case ProcessPriorityClass.AboveNormal : cmbPriority.SelectedIndex = 2
          Case ProcessPriorityClass.Normal : cmbPriority.SelectedIndex = 3
          Case ProcessPriorityClass.BelowNormal : cmbPriority.SelectedIndex = 4
          Case ProcessPriorityClass.Idle : cmbPriority.SelectedIndex = 5
          Case Else : cmbPriority.SelectedIndex = 3
        End Select
      End Try
    End If
    If mySettings.PlayAlertNoise Then
      cmbCompletion.Text = "Play Alert Noise"
    Else
      cmbCompletion.Text = "Do Nothing"
    End If
    chkAutoLabel.Checked = mySettings.AutoISOLabel
    If String.IsNullOrEmpty(mySettings.DefaultISOLabel) Then
      txtISOLabel.Text = "GRMCULFRER_EN_DVD"
    Else
      txtISOLabel.Text = mySettings.DefaultISOLabel
    End If
    txtISOLabel.ContextMenu = mnuISOLabel
    If mySettings.Size.IsEmpty Then
      Me.Size = Me.MinimumSize
    ElseIf mySettings.Size.Width < Screen.PrimaryScreen.Bounds.Width And mySettings.Size.Height < Screen.PrimaryScreen.Bounds.Height Then
      Me.Size = mySettings.Size
    Else
      Me.Size = Me.MinimumSize
    End If
    txtOutput.ContextMenu = mnuOutput
    If mySettings.Position.IsEmpty Then
      Me.Location = New Point(Screen.PrimaryScreen.Bounds.Left + CInt(Screen.PrimaryScreen.Bounds.Width / 2) - CInt(Me.Width / 2), Screen.PrimaryScreen.Bounds.Top + CInt(Screen.PrimaryScreen.Bounds.Height / 2) - CInt(Me.Height / 2))
    ElseIf Screen.PrimaryScreen.Bounds.Contains(mySettings.Position) Then
      Me.Location = mySettings.Position
    Else
      Me.Location = New Point(Screen.PrimaryScreen.Bounds.Left + CInt(Screen.PrimaryScreen.Bounds.Width / 2) - CInt(Me.Width / 2), Screen.PrimaryScreen.Bounds.Top + CInt(Screen.PrimaryScreen.Bounds.Height / 2) - CInt(Me.Height / 2))
    End If
    If VisualStyles.VisualStyleInformation.DisplayName = "Aero style" And TaskbarLib.TaskbarFinder.TaskbarVisible Then
      If taskBar Is Nothing Then taskBar = New TaskbarLib.TaskbarList
    Else
      taskBar = Nothing
    End If
    chkLoadFeatures.Checked = mySettings.LoadFeatures
    chkLoadUpdates.Checked = mySettings.LoadUpdates
    chkLoadDrivers.Checked = mySettings.LoadDrivers
    GUI_ToggleEnabled(True)
    TitleMNG_DrawTitle()
    isStarting = False
  End Sub
  Private Sub frmMain_LocationChanged(sender As Object, e As System.EventArgs) Handles Me.LocationChanged
    If ConsoleOutput_WindowMode Then frmOutput.RePosition()
  End Sub
  Private Sub frmMain_ResizeBegin(sender As Object, e As System.EventArgs) Handles Me.ResizeBegin
    windowChangedSize = False
  End Sub
  Private Sub frmMain_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    windowChangedSize = True
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.All)
    If Not isStarting And Me.Visible Then
      mySettings.Position = Me.Location
      If pbTotal.Visible Then
        If txtOutput.Visible Then
          mySettings.Size = New Size(Me.Width, Me.Height - (HeightDifferentialA + HeightDifferentialB))
        Else
          mySettings.Size = New Size(Me.Width, Me.Height - HeightDifferentialB)
        End If
      Else
        If txtOutput.Visible Then
          mySettings.Size = New Size(Me.Width, Me.Height - HeightDifferentialA)
        Else
          mySettings.Size = Me.Size
        End If
      End If
    End If
    TitleMNG_DrawTitle(True)
    If Not GUI_Resize_WindowState = Me.WindowState Then
      GUI_Resize_WindowState = Me.WindowState
      If pnlSP64.Visible And Me.GUI_Resize_WindowState = FormWindowState.Normal Then
        Me.Height += 1
        Me.Height -= 1
      End If
    End If
    If ConsoleOutput_WindowMode Then frmOutput.RePosition()
  End Sub
  Private Sub frmMain_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
    If Not GUI_Resize_WindowState = Me.WindowState Then windowChangedSize = True
    If pnlSP64.Visible And windowChangedSize Then
      Me.Height += 1
      Me.Height -= 1
    End If
    windowChangedSize = False
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.All)
    TitleMNG_DrawTitle(True)
  End Sub
  Private Sub spltSlips7ream_SplitterMoved(sender As System.Object, e As System.Windows.Forms.SplitterEventArgs) Handles spltSlips7ream.SplitterMoved
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.All)
  End Sub
  Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.WindowsShutDown Then
      StopRun = True
      Return
    End If
    If Not RunActivity = ActivityType.Nothing Then
      Dim Activity As ActivityRet = ActivityParser(RunActivity)
      If MsgDlg(Me, String.Format("Do you want to cancel the {0} proceess and close {1}?", Activity.Process, Application.ProductName), String.Format("{1} is busy {0}.", Activity.Activity, Application.ProductName), String.Format("Stop {0} and Close?", Activity.Title), MessageBoxButtons.YesNo, _TaskDialogIcon.Question, MessageBoxDefaultButton.Button2, , String.Format("Stop {0} and Close", Activity.Title)) = Windows.Forms.DialogResult.No Then
        e.Cancel = True
        Return
      End If
    End If
    StopRun = True
    GUI_Cloing()
    If tmrTitleMNG.IsActive Then tmrTitleMNG.Stop()
  End Sub
  Private Sub GUI_Cloing()
    If IO.Directory.Exists(WorkDir) Then
      TitleMNG_SetTitle("Cleaning Up Files", "Cleaning up mounts, work, and temporary directories...")
      TitleMNG_SetDisplay(TitleMNG_List.Delete)
      GUI_ToggleEnabled(False)
      cmdClose.Enabled = False
      StopRun = False
      CleanMounts(True)
      Status_SetText("Clearing Temporary Files...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", WorkDir))
      Try
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents, True)
      Catch ex As Exception
      End Try
      StopRun = True
      pctTitle.BackgroundImage = Nothing
      TitleMNG_Image = Nothing
    End If
  End Sub
  Private Enum GUI_ListView_Columns
    MSU = 1
    Images = 2
    OutputCaption = 4
    All = MSU Or Images Or OutputCaption
  End Enum
  Private Sub GUI_ListView_ResizeColumns(WhichLists As GUI_ListView_Columns)
    If (WhichLists And GUI_ListView_Columns.MSU) = GUI_ListView_Columns.MSU Then
      If Not lvMSU.Columns.Count = 0 Then
        lvMSU.BeginUpdate()
        lvMSU.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        If lvMSU.Columns(1).Width < 55 Then lvMSU.Columns(1).Width = 55
        Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 2
        If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
        lvMSU.EndUpdate()
      End If
      If cmdAddMSU.Enabled Then
        If Not cmdRemMSU.Enabled = (lvMSU.SelectedItems.Count > 0) Then cmdRemMSU.Enabled = (lvMSU.SelectedItems.Count > 0)
        If Not cmdClearMSU.Enabled = (lvMSU.Items.Count > 0) Then cmdClearMSU.Enabled = (lvMSU.Items.Count > 0)
      End If
    End If
    If (WhichLists And GUI_ListView_Columns.Images) = GUI_ListView_Columns.Images Then
      If Not lvImages.Columns.Count = 0 Then
        lvImages.BeginUpdate()
        lvImages.Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        If lvImages.Columns(0).Width < 50 Then lvImages.Columns(0).Width = 50
        lvImages.Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        If lvImages.Columns(2).Width < 43 Then lvImages.Columns(2).Width = 43
        lvImages.Columns(3).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        If lvImages.Columns(3).Width < 60 Then lvImages.Columns(3).Width = 60
        Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width + lvImages.Columns(3).Width) - 2
        If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
        lvImages.EndUpdate()
      End If
    End If
    If (WhichLists And GUI_ListView_Columns.OutputCaption) = GUI_ListView_Columns.OutputCaption Then If pctOutputTear.Visible Then ConsoleOutput_Title_Redraw()
  End Sub
  Private Sub GUI_ToggleEnabled(bEnabled As Boolean, Optional sStatus As String = Nothing)
    If Me.InvokeRequired Then
      Me.Invoke(New GUI_ToggleEnabledInvoker(AddressOf GUI_ToggleEnabled), bEnabled, sStatus)
      Return
    End If
    If bEnabled Then
      RunActivity = ActivityType.Nothing
      Me.Cursor = Cursors.Default
      tmrTitleMNG.Stop()
      TitleMNG_Image = Nothing
      TitleMNG_DrawTitle()
    Else
      Me.Cursor = Cursors.AppStarting
      tmrTitleMNG.Start()
    End If
    pnlSlips7ream.SuspendLayout()
    lblWIM.Enabled = bEnabled
    txtWIM.Enabled = bEnabled
    cmdWIM.Enabled = bEnabled
    chkSP.Enabled = bEnabled
    If bEnabled Then
      txtSP.Enabled = chkSP.Checked
      cmdSP.Enabled = chkSP.Checked
      lblSP64.Enabled = chkSP.Checked
      txtSP64.Enabled = chkSP.Checked
      cmdSP64.Enabled = chkSP.Checked
    Else
      txtSP.Enabled = False
      cmdSP.Enabled = False
      lblSP64.Enabled = False
      txtSP64.Enabled = False
      cmdSP64.Enabled = False
    End If
    lblMSU.Enabled = bEnabled
    If Not bEnabled And Updates_BackgroundColors.Count = 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        Updates_BackgroundColors.Add(lvItem.Name, lvItem.BackColor)
      Next
    End If
    lvMSU.ReadOnly = Not bEnabled
    If bEnabled And Updates_BackgroundColors.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        If Updates_BackgroundColors.ContainsKey(lvItem.Name) Then lvItem.BackColor = Updates_BackgroundColors(lvItem.Name)
      Next
      Updates_BackgroundColors.Clear()
    End If
    cmdAddMSU.Enabled = bEnabled
    If bEnabled Then
      cmdRemMSU.Enabled = lvMSU.SelectedItems.Count > 0
      cmdClearMSU.Enabled = lvMSU.Items.Count > 0
    Else
      cmdRemMSU.Enabled = False
      cmdClearMSU.Enabled = False
    End If
    If bEnabled Then
      If RunComplete Then
        cmdBegin.Text = "Rerun"
        cmdOpenFolder.Visible = True
      Else
        cmdBegin.Text = "&Begin"
        cmdOpenFolder.Visible = False
      End If
    Else
      cmdBegin.Text = "&Begin"
      cmdOpenFolder.Visible = False
    End If
    cmdBegin.Visible = bEnabled
    chkISO.Enabled = bEnabled
    If bEnabled Then
      txtISO.Enabled = chkISO.Checked
      cmdISO.Enabled = chkISO.Checked
      lblISOFeatures.Enabled = chkISO.Checked
      chkUnlock.Enabled = chkISO.Checked And ISO_UnlockEditionsCheckbox = TriState.UseDefault
      chkUEFI.Enabled = chkISO.Checked And ISO_UEFICheckbox = TriState.UseDefault
      lblISOLabel.Enabled = chkISO.Checked
      chkAutoLabel.Enabled = chkISO.Checked
      txtISOLabel.Enabled = chkISO.Checked
      lblISOFS.Enabled = chkISO.Checked
      cmbISOFormat.Enabled = chkISO.Checked
    Else
      txtISO.Enabled = False
      cmdISO.Enabled = False
      lblISOFeatures.Enabled = False
      chkUnlock.Enabled = False
      chkUEFI.Enabled = False
      lblISOLabel.Enabled = False
      chkAutoLabel.Enabled = False
      txtISOLabel.Enabled = False
      lblISOFS.Enabled = False
      cmbISOFormat.Enabled = False
    End If
    cmbLimitType.Enabled = bEnabled
    If chkISO.Checked Then
      If Not cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Add("Split ISO")
    Else
      If cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Remove("Split ISO")
    End If
    If cmbLimitType.SelectedIndex = -1 Then cmbLimitType.SelectedIndex = 0
    If bEnabled Then
      cmbLimit.Enabled = cmbLimitType.SelectedIndex > 0
    Else
      cmbLimit.Enabled = False
    End If
    chkMerge.Enabled = bEnabled
    If bEnabled Then
      txtMerge.Enabled = chkMerge.Checked
      cmdMerge.Enabled = chkMerge.Checked
    Else
      txtMerge.Enabled = False
      cmdMerge.Enabled = False
    End If
    lblImages.Enabled = bEnabled
    If bEnabled Then
      chkLoadFeatures.Enabled = Not String.IsNullOrEmpty(txtWIM.Text)
      chkLoadUpdates.Enabled = Not String.IsNullOrEmpty(txtWIM.Text)
      chkLoadDrivers.Enabled = Not String.IsNullOrEmpty(txtWIM.Text)
      cmdLoadPackages.Enabled = Not String.IsNullOrEmpty(txtWIM.Text) And (chkLoadFeatures.Checked Or chkLoadUpdates.Checked Or chkLoadDrivers.Checked)
    Else
      chkLoadFeatures.Enabled = False
      chkLoadUpdates.Enabled = False
      chkLoadDrivers.Enabled = False
      cmdLoadPackages.Enabled = False
    End If
    lvImages.ReadOnly = Not bEnabled
    If bEnabled Then
      cmdClose.Text = "&Close"
      Me.CancelButton = Nothing
      If String.IsNullOrEmpty(sStatus) Then
        Status_SetText("Idle")
      Else
        Status_SetText(sStatus)
      End If
    Else
      cmdClose.Text = "&Cancel"
      Me.CancelButton = cmdClose
    End If
    cmdConfig.Visible = bEnabled
    cmdClose.Enabled = True
    If pbTotal.Visible = bEnabled Then
      pbTotal.Visible = Not bEnabled
      pbIndividual.Visible = Not bEnabled
      If Me.WindowState = FormWindowState.Normal Then
        If pbTotal.Visible Then
          Me.Height += HeightDifferentialB
          Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height + HeightDifferentialB)
        Else
          Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialB)
          Me.Height -= HeightDifferentialB
        End If
      End If
    End If
    pbTotal.Visible = Not bEnabled
    pbIndividual.Visible = Not bEnabled
    If Not pbTotal.Visible Then pbTotal.Value = 0
    If Not pbIndividual.Visible Then pbIndividual.Value = 0
    pnlSlips7ream.ResumeLayout(True)
    Try
      If pbTotal.Visible Then
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
      Else
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
      End If
    Catch ex As Exception
    End Try
  End Sub
#Region "Textbox Drag/Drop"
  Private Sub TextBoxDragDropEvent(sender As TextBox, e As System.Windows.Forms.DragEventArgs)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If IsArray(Data) Then
        Dim sData() As String = CType(Data, String())
        If sData.Length = 1 Then sender.Text = sData(0)
      End If
    End If
  End Sub
  Friend Sub TextBoxDragEnterEvent(sender As TextBox, e As DragEventArgs)
    e.Effect = DragDropEffects.Copy
  End Sub
  Friend Sub TextBoxDragOverEvent(sender As TextBox, e As DragEventArgs, AllowedTypes() As String)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If IsArray(Data) Then
        Dim sData() As String = CType(Data, String())
        If sData.Length = 1 Then
          Dim hasImage As Boolean = False
          For Each aType As String In AllowedTypes
            If IO.Path.GetExtension(sData(0)).ToLower = aType Then
              hasImage = True
              Exit For
            End If
          Next
          If hasImage Then
            e.Effect = DragDropEffects.Copy
          Else
            e.Effect = DragDropEffects.None
          End If
        Else
          e.Effect = DragDropEffects.None
        End If
      Else
        e.Effect = DragDropEffects.None
      End If
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
#End Region
#Region "WIM"
  Private thdWIMInput As Threading.Thread
  Private thdWIMDragDrop As Threading.Thread
  Private WIM_LastValue As String
  Private Sub txtWIM_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtWIM.DragDrop
    TextBoxDragDropEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub WIM_PopulateISOData()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf WIM_PopulateISOData))
      Return
    End If
    If Not chkISO.Checked Or String.IsNullOrEmpty(txtISO.Text) Then
      If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
        chkISO.Checked = True
        txtISO.Text = txtWIM.Text
      Else
        chkISO.Checked = False
        txtISO.Text = String.Empty
      End If
    End If
  End Sub
  Private Sub txtWIM_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtWIM.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtWIM_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtWIM.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".wim", ".iso"})
  End Sub
  Private Sub txtWIM_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtWIM.TextChanged
    If txtWIM.Text = WIM_LastValue Then Return
    WIM_LastValue = txtWIM.Text
    If String.IsNullOrEmpty(txtWIM.Text) OrElse Not IO.File.Exists(txtWIM.Text) Then
      ImagePackage_ClearList(WIMGroup.WIM)
      Return
    End If
    If Not IO.File.Exists(txtWIM.Text) Then Return
    RunComplete = False
    StopRun = False
    RunActivity = ActivityType.LoadingPackageData
    cmdBegin.Text = "&Begin"
    cmdLoadPackages.Image = My.Resources.u_n
    cmdOpenFolder.Visible = False
    If thdWIMInput Is Nothing Then
      thdWIMInput = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_ParseList))
      thdWIMInput.Start(WIMGroup.WIM)
    End If
  End Sub
  Private Sub cmdWIM_Click(sender As System.Object, e As System.EventArgs) Handles cmdWIM.Click
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AllowNonFileSystemItems = False
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000001")
      cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("INSTALL.WIM Sources", "WIM,ISO"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("INSTALL.WIM", "WIM"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Windows 7 ISO", "ISO"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = False
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Choose INSTALL.WIM Image..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.1_INSTALL.WIM.htm")
      If cdlBrowse.ShowDialog(Me.Handle) = CommonFileDialogResult.Ok Then txtWIM.Text = cdlBrowse.FileName
    End Using
  End Sub
#End Region
#Region "Service Pack"
  Private Sub chkSP_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSP.CheckedChanged
    If chkSP.Enabled Then
      txtSP.Enabled = chkSP.Checked
      cmdSP.Enabled = chkSP.Checked
    Else
      txtSP.Enabled = False
      cmdSP.Enabled = False
    End If
    If chkSP.Checked Then
      Dim has86 As Boolean = False
      Dim has64 As Boolean = False
      For Each lvItem As ListViewItem In lvImages.Items
        Dim lvIndex As Integer = CInt(lvItem.Tag)
        Dim propData As ImagePackage = ImagePackage_ListData(lvIndex).Package
        If CompareArchitectures(propData.Architecture, ArchitectureList.amd64, False) Then has64 = True
        If CompareArchitectures(propData.Architecture, ArchitectureList.x86, False) Then has86 = True
        If has86 And has64 Then Exit For
      Next
      If has86 And has64 Then
        chkSP.Text = "x86 &Service Pack:"
        lblSP64.Visible = True
        pnlSP64.Visible = True
        lblSP64.Enabled = chkSP.Enabled
        txtSP64.Enabled = chkSP.Enabled
        cmdSP64.Enabled = chkSP.Enabled
        If spltSlips7ream.Panel1MinSize = 97 Then
          spltSlips7ream.SplitterDistance += 29
          spltSlips7ream.Panel1MinSize = 126
        End If
      Else
        chkSP.Text = "&Service Pack:"
        lblSP64.Visible = False
        pnlSP64.Visible = False
        lblSP64.Enabled = False
        txtSP64.Enabled = False
        cmdSP64.Enabled = False
        If spltSlips7ream.Panel1MinSize = 126 Then
          spltSlips7ream.Panel1MinSize = 97
          spltSlips7ream.SplitterDistance -= 29
        End If
      End If
    Else
      chkSP.Text = "&Service Pack:"
      lblSP64.Visible = False
      pnlSP64.Visible = False
      lblSP64.Enabled = False
      txtSP64.Enabled = False
      cmdSP64.Enabled = False
      If spltSlips7ream.Panel1MinSize = 126 Then
        spltSlips7ream.Panel1MinSize = 97
        spltSlips7ream.SplitterDistance -= 29
      End If
    End If
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.All)
  End Sub
  Private Sub txtSP_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragDrop
    TextBoxDragDropEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtSP_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtSP_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".exe"})
  End Sub
  Private Sub txtSP_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSP.TextChanged
    Status_SetText("Idle")
  End Sub
  Private Sub cmdSP_Click(sender As System.Object, e As System.EventArgs) Handles cmdSP.Click
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AllowNonFileSystemItems = False
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000003")
      If Not String.IsNullOrEmpty(txtWIM.Text) AndAlso IO.File.Exists(txtWIM.Text) Then
        cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtWIM.Text)
      Else
        cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      End If
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Service Pack EXE", "EXE"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = False
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Choose Windows 7 Service Pack 1 EXE..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.4_Service_Pack.htm")
      If Not String.IsNullOrEmpty(txtSP.Text) Then cdlBrowse.InitialDirectory = txtSP.Text
      If cdlBrowse.ShowDialog(Me.Handle) = CommonFileDialogResult.Ok Then txtSP.Text = cdlBrowse.FileName
    End Using
  End Sub
#Region "64-Bit"
  Private Sub txtSP64_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP64.DragDrop
    TextBoxDragDropEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtSP64_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP64.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtSP64_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP64.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".exe"})
  End Sub
  Private Sub txtSP64_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSP64.TextChanged
    Status_SetText("Idle")
  End Sub
  Private Sub cmdSP64_Click(sender As System.Object, e As System.EventArgs) Handles cmdSP64.Click
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AllowNonFileSystemItems = False
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000004")
      If Not String.IsNullOrEmpty(txtSP.Text) AndAlso IO.File.Exists(txtSP.Text) Then
        cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtSP.Text)
      Else
        If Not String.IsNullOrEmpty(txtWIM.Text) AndAlso IO.File.Exists(txtWIM.Text) Then
          cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtWIM.Text)
        Else
          cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
      End If
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Service Pack EXE", "EXE"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = False
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Choose Windows 7 x64 Service Pack 1 EXE..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.4_Service_Pack.htm")
      If Not String.IsNullOrEmpty(txtSP64.Text) Then cdlBrowse.InitialDirectory = txtSP64.Text
      If cdlBrowse.ShowDialog(Me.Handle) = CommonFileDialogResult.Ok Then txtSP64.Text = cdlBrowse.FileName
    End Using
  End Sub
#End Region
#End Region
#Region "Updates"
  Private Updates_ListBusy As Boolean
  Private Shared Updates_ListData As New SortedList(Of Integer, Updates_Data)
  Private Updates_ReplaceAllOld As TriState = TriState.UseDefault
  Private Updates_ReplaceAllNew As TriState = TriState.UseDefault
  Private Updates_BackgroundColors As New Dictionary(Of String, Color)
  Private Updates_ListSelection As ListView.SelectedListViewItemCollection
  Private PrerequisiteList() As Updates_Prerequisite = {New Updates_Prerequisite("2592687:2574819"),
                                                        New Updates_Prerequisite("2830477:2574819,2857650"),
                                                        New Updates_Prerequisite("2718695:2533623,2639308,2670838,2729094,2731771,2786081/3125574,2639308,2670838,2729094"),
                                                        New Updates_Prerequisite("2841134:2533623,2639308,2670838,2729094,2731771,2786081,2834140,2882822,2888049,2849696,2849697/3125574,2639308,2670838,2729094,2834140,2849696,2849697"),
                                                        New Updates_Prerequisite("2923545:2830477"),
                                                        New Updates_Prerequisite("2965788:2830477"),
                                                        New Updates_Prerequisite("2984976:2830477,2984972,2592687"),
                                                        New Updates_Prerequisite("3020388:2830477"),
                                                        New Updates_Prerequisite("3042058:3020369"),
                                                        New Updates_Prerequisite("3075226:2830477"),
                                                        New Updates_Prerequisite("3125574:3020369"),
                                                        New Updates_Prerequisite("3126446:2830477")}
  Private Sub lvMSU_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvMSU.ColumnWidthChanged
    If Updates_ListBusy Then Return
    Updates_ListBusy = True
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
    lvMSU.BeginUpdate()
    If e.ColumnIndex = 0 Then
      Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 2
      If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
    End If
    If e.ColumnIndex = 1 Then
      lvMSU.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvMSU.Columns(1).Width < 55 Then lvMSU.Columns(1).Width = 55
    End If
    lvMSU.EndUpdate()
    Updates_ListBusy = False
  End Sub
  Private Sub lvMSU_ColumnWidthChanging(sender As Object, e As System.Windows.Forms.ColumnWidthChangingEventArgs) Handles lvMSU.ColumnWidthChanging
    e.Cancel = True
  End Sub
  Private Sub lvMSU_DoubleClick(sender As Object, e As System.EventArgs) Handles lvMSU.DoubleClick
    If lvMSU.SelectedItems IsNot Nothing AndAlso lvMSU.SelectedItems.Count > 0 Then Updates_Properties(lvMSU.SelectedItems)
  End Sub
  Private Sub lvMSU_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvMSU.KeyDown
    If e.KeyCode = Keys.Delete Then
      If e.Shift Then
        cmdClearMSU_Click(cmdClearMSU, New EventArgs)
      Else
        If lvMSU.SelectedItems.Count > 0 Then
          cmdRemMSU_Click(cmdRemMSU, New EventArgs)
        End If
      End If
    End If
  End Sub
#Region "Drag/Drop"
  Private Updates_SelectedRows As New List(Of Integer)
  Private Updates_LastMouseClick As Long = 0
  Private Updates_LastMousePoint As Point = Point.Empty
  Private Function Updates_GetFilesList(Path As String) As String()
    Dim sFiles As New List(Of String)
    For Each sFile As String In IO.Directory.EnumerateFiles(Path)
      sFiles.Add(sFile)
    Next
    For Each sDirectory As String In IO.Directory.EnumerateDirectories(Path)
      Dim sChildren() As String = Updates_GetFilesList(sDirectory)
      If Not sChildren Is Nothing AndAlso sChildren.Count > 0 Then sFiles.AddRange(sChildren)
    Next
    Return sFiles.ToArray
  End Function
  Friend Sub DragDropCallback(obj As Object)
    Dim sender As Object = CType(obj, Object())(0)
    Dim e As DragEventArgs = CType(CType(obj, Object())(1), DragEventArgs)
    Me.Invoke(New EventHandler(Of DragEventArgs)(AddressOf DragDropEvent), sender, e)
  End Sub
  Friend Sub DragDropEvent(sender As Object, e As DragEventArgs)
    Updates_ReplaceAllOld = TriState.UseDefault
    Updates_ReplaceAllNew = TriState.UseDefault
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data() As String = CType(e.Data.GetData("FileDrop"), String())
      Dim ReplaceData As Boolean = False
      For Each Path In Data
        If IO.Directory.Exists(Path) Then
          ReplaceData = True
          Exit For
        End If
      Next
      If ReplaceData Then
        Dim newData As New List(Of String)
        For Each Path In Data
          If IO.File.Exists(Path) Then
            newData.Add(Path)
          ElseIf IO.Directory.Exists(Path) Then
            Dim sChildren() As String = Updates_GetFilesList(Path)
            If Not sChildren Is Nothing AndAlso sChildren.Count > 0 Then newData.AddRange(sChildren)
          End If
        Next
        Data = newData.ToArray
      End If
      Dim FileCount As Integer = Data.Length
      RunActivity = ActivityType.LoadingUpdates
      StopRun = False
      TitleMNG_SetDisplay(TitleMNG_List.Delete)
      TitleMNG_SetTitle("Parsing Update Information", "Reading data from update files...")
      GUI_ToggleEnabled(False)
      Progress_Total(0, FileCount)
      Progress_Normal(0, 0)
      Status_SetText("Reading Update Information...")
      Dim FailCollection As New List(Of String)
      Dim iProg As Integer = 0
      Dim msuCollection As New List(Of ListViewItem)
      For Each Item In Data
        iProg += 1
        Progress_Total(iProg, FileCount)
        Progress_Normal(0, 1)
        Application.DoEvents()
        If StopRun Then
          Progress_Normal(0, 1)
          GUI_ToggleEnabled(True)
          lvMSU.EndUpdate()
          Return
        End If
        Dim Cancelled As Boolean = False
        Dim newUpdateList As Update_File() = GetUpdateInfo(Item)
        Dim UpdateCount As Integer = 0
        If newUpdateList IsNot Nothing Then UpdateCount = newUpdateList.Count
        Dim iProg2 As Integer = 0
        Progress_Normal(0, UpdateCount)
        If UpdateCount > 0 Then
          For Each msuData As Update_File In newUpdateList
            Dim addRet As Updates_AddFile_Result = Updates_AddFile(msuData, msuCollection)
            iProg2 += 1
            Progress_Normal(iProg2, UpdateCount)
            If addRet.Cancel Then
              Cancelled = True
              Exit For
            End If
            If Not addRet.Success Then
              Dim sUpdName As String = msuData.Name
              If String.IsNullOrEmpty(msuData.Name) Then
                sUpdName = IO.Path.GetFileNameWithoutExtension(Item)
              ElseIf msuData.Name = "DRIVER" Then
                sUpdName = IO.Path.GetFileNameWithoutExtension(msuData.DriverData.DriverStorePath)
              End If
              FailCollection.Add(String.Format("{0}: {1}", sUpdName, addRet.FailReason))
            End If
          Next
        Else
          FailCollection.Add(String.Format("{0}: Update not found.", IO.Path.GetFileNameWithoutExtension(Item)))
        End If
        If Cancelled Then Exit For
      Next
      If msuCollection.Count > 0 Then
        lvMSU.Items.AddRange(msuCollection.ToArray)
        Updates_Requirements()
        SortMSUsForIntegration(lvMSU)
      End If
      Progress_Normal(0, 1)
      GUI_ToggleEnabled(True)
      GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
      If FailCollection.Count > 0 Then
        MsgDlg(Me, String.Concat("Some files could not be added to the Update List.", vbNewLine, "Click View Details to see a complete list."), "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, _TaskDialogIcon.WindowsUpdate, , Updates_FailureCleanup(FailCollection), "Error Adding Updates")
      End If
      Status_SetText("Idle")
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Friend Sub DragEnterEvent(sender As Object, e As DragEventArgs)
    e.Effect = DragDropEffects.All
  End Sub
  Friend Sub DragOverEvent(sender As Object, e As DragEventArgs)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data() As String = CType(e.Data.GetData("FileDrop"), String())
      Dim hasUpdate As Boolean = False
      For Each file In Data
        If IO.File.Exists(file) Then
          Select Case IO.Path.GetExtension(file).ToLower
            Case ".msu", ".cab", ".mlc", ".exe", ".msi", ".inf"
              hasUpdate = True
              Exit For
          End Select
        ElseIf IO.Directory.Exists(file) Then
          hasUpdate = True
          Exit For
        End If
      Next
      If hasUpdate Then
        e.Effect = DragDropEffects.Copy
      Else
        e.Effect = DragDropEffects.None
      End If
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Private Sub lvMSU_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragDrop
    If Updates_SelectedRows.Count > 0 Then
      Updates_SelectedRows.Clear()
    Else
      Dim tDragDrop As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DragDropCallback))
      tDragDrop.Start(CType({sender, e}, Object()))
    End If
  End Sub
  Private Sub lvMSU_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragEnter
    DragEnterEvent(sender, e)
  End Sub
  Private Sub lvMSU_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragOver
    If Updates_SelectedRows.Count > 0 Then
      Dim ptRet As Drawing.Point = lvMSU.PointToClient(New Drawing.Point(e.X, e.Y))
      Dim ht = lvMSU.HitTest(ptRet.X, ptRet.Y)
      Dim TargetRowIndex As Integer = -1
      If ht.Item IsNot Nothing Then
        TargetRowIndex = ht.Item.Index
      End If
      If TargetRowIndex > -1 Then
        If Not Updates_SelectedRows(0) = TargetRowIndex Then
          Dim targetI As Integer = TargetRowIndex
          For Each item As ListViewItem In lvMSU.SelectedItems
            Dim sItemName As String = item.Name
            Dim tmpItem As ListViewItem = CType(item.Clone, ListViewItem)
            item.Remove()
            If lvMSU.Items.Count <= targetI Then
              targetI = CType(lvMSU.Items.Add(tmpItem), ListViewItem).Index
            Else
              lvMSU.Items.Insert(targetI, tmpItem)
            End If
            lvMSU.Items(targetI).Name = sItemName
            lvMSU.Items(targetI).Selected = True
            targetI += 1
          Next
          Updates_SelectedRows.Clear()
          For Each item As ListViewItem In lvMSU.SelectedItems
            Updates_SelectedRows.Add(item.Index)
          Next
        End If
        lvMSU.EnsureVisible(TargetRowIndex)
        e.Effect = DragDropEffects.Move Or DragDropEffects.Scroll
      Else
        e.Effect = DragDropEffects.None
      End If
    Else
      DragOverEvent(sender, e)
    End If
  End Sub
  Private Sub lvMSU_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseDown
    If Updates_SelectedRows.Count > 0 Then Updates_SelectedRows.Clear()
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If lvMSU.SelectedItems.Count > 0 Then
        For Each item As ListViewItem In lvMSU.SelectedItems
          Updates_SelectedRows.Add(item.Index)
        Next
      Else
        Dim selItem As ListViewItem = lvMSU.GetItemAt(e.X, e.Y)
        If selItem IsNot Nothing Then Updates_SelectedRows.Add(selItem.Index)
      End If
    End If
  End Sub
  Private Sub lvMSU_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseMove
    Static lastLoc As Drawing.Point
    If e.Button = Windows.Forms.MouseButtons.Left And lvMSU.SelectedItems.Count > 0 And Updates_SelectedRows.Count > 0 Then
      If Math.Abs(lastLoc.X - e.Location.X) > 3 Or Math.Abs(lastLoc.Y - e.Location.Y) > 3 Then
        lvMSU.DoDragDrop(lvMSU.SelectedItems, DragDropEffects.Move)
      End If
    End If
  End Sub
  Private Sub lvMSU_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Updates_LastMouseClick > 0 And Not Updates_LastMousePoint.IsEmpty Then
        If (TickCount() - Updates_LastMouseClick) <= SystemInformation.DoubleClickTime Then
          Dim allowedRect As New Rectangle(New Point(Updates_LastMousePoint.X - CInt(SystemInformation.DoubleClickSize.Width / 2), Updates_LastMousePoint.Y - CInt(SystemInformation.DoubleClickSize.Height / 2)), SystemInformation.DoubleClickSize)
          If allowedRect.Contains(e.Location) Then lvMSU_DoubleClick(lvMSU, New EventArgs)
        End If
      End If
      Updates_SelectedRows.Clear()
      Updates_LastMouseClick = TickCount()
      Updates_LastMousePoint = e.Location
    ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
      If lvMSU.SelectedItems.Count > 0 Then
        Updates_ListSelection = lvMSU.SelectedItems
        Dim allUpdates As Boolean = True
        Dim allDrivers As Boolean = True
        Dim hasDriver As Boolean = False
        Dim driverBootSetup As Boolean = True
        Dim driverBootPE As Boolean = True
        For I As Integer = 0 To lvMSU.SelectedItems.Count - 1
          Dim lvIndex As Integer = CInt(lvMSU.SelectedItems(I).Tag)
          Dim itemData As Update_File = Updates_ListData(lvIndex).Update
          If itemData.Name = "DRIVER" Then
            hasDriver = True
            allUpdates = False
            If Not itemData.DriverData.IntegrateIntoBootSetup Then driverBootSetup = False
            If Not itemData.DriverData.IntegrateIntoBootPE Then driverBootPE = False
          Else
            allDrivers = False
          End If
        Next
        If allUpdates Then
          If lvMSU.SelectedItems.Count > 1 Then
            mnuUpdateTop.Text = "Move Updates to &Top"
            mnuUpdateUp.Text = "Move Updates &Up"
            mnuUpdateDown.Text = "Move Updates &Down"
            mnuUpdateBottom.Text = "Move Updates to &Bottom"
            mnuUpdateRemove.Text = "&Remove Updates"
            mnuUpdateLocation.Text = "Open Update &Locations"
          Else
            mnuUpdateTop.Text = "Move Update to &Top"
            mnuUpdateUp.Text = "Move Update &Up"
            mnuUpdateDown.Text = "Move Update &Down"
            mnuUpdateBottom.Text = "Move Update to &Bottom"
            mnuUpdateRemove.Text = "&Remove Update"
            mnuUpdateLocation.Text = "Open Update &Location"
          End If
        ElseIf allDrivers Then
          If lvMSU.SelectedItems.Count > 1 Then
            mnuUpdateTop.Text = "Move Drivers to &Top"
            mnuUpdateUp.Text = "Move Drivers &Up"
            mnuUpdateDown.Text = "Move Drivers &Down"
            mnuUpdateBottom.Text = "Move Drivers to &Bottom"
            mnuUpdateRemove.Text = "&Remove Drivers"
            mnuUpdateBootPEDriver.Text = "Include Drivers in &PE"
            mnuUpdateBootSetupDriver.Text = "Include Drivers in &Setup"
            mnuUpdateLocation.Text = "Open Driver &Locations"
          Else
            mnuUpdateTop.Text = "Move Driver to &Top"
            mnuUpdateUp.Text = "Move Driver &Up"
            mnuUpdateDown.Text = "Move Driver &Down"
            mnuUpdateBottom.Text = "Move Driver to &Bottom"
            mnuUpdateRemove.Text = "&Remove Driver"
            mnuUpdateBootPEDriver.Text = "Include Driver in &PE"
            mnuUpdateBootSetupDriver.Text = "Include Driver in &Setup"
            mnuUpdateLocation.Text = "Open Driver &Location"
          End If
        Else
          mnuUpdateTop.Text = "Move Updates and Drivers to &Top"
          mnuUpdateUp.Text = "Move Updates and Drivers &Up"
          mnuUpdateDown.Text = "Move Updates and Drivers &Down"
          mnuUpdateBottom.Text = "Move Updates and Drivers to &Bottom"
          mnuUpdateRemove.Text = "&Remove Updates and Drivers"
          mnuUpdateBootPEDriver.Text = "Include Drivers in &PE"
          mnuUpdateBootSetupDriver.Text = "Include Drivers in &Setup"
          mnuUpdateLocation.Text = "Open Update and Driver &Locations"
        End If
        If Not chkISO.Checked Or String.IsNullOrEmpty(txtISO.Text) Then
          hasDriver = False
        ElseIf Not IO.File.Exists(txtISO.Text) Then
          hasDriver = False
        End If
        mnuUpdateBootSetupDriver.Visible = hasDriver
        mnuUpdateBootSetupDriver.Checked = driverBootSetup
        mnuUpdateBootPEDriver.Visible = hasDriver
        mnuUpdateBootPEDriver.Checked = driverBootPE
        mnuMSU.Show(lvMSU, e.Location)
      End If
    End If
  End Sub
  Private Sub lvMSU_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvMSU.SelectedIndexChanged
    If lvMSU.SelectedItems.Count > 0 Then
      cmdRemMSU.Enabled = True
      If lvMSU.SelectedItems.Count > 1 Then
        cmdRemMSU.Text = "Remove Up&dates"
      Else
        cmdRemMSU.Text = "Remove Up&date"
      End If
    Else
      cmdRemMSU.Enabled = False
      cmdRemMSU.Text = "Remove Up&dates"
    End If
  End Sub
#End Region
#Region "Buttons"
  Private Sub cmdAddMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddMSU.Click
    Updates_ReplaceAllOld = TriState.UseDefault
    Updates_ReplaceAllNew = TriState.UseDefault
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000005")
      If Not String.IsNullOrEmpty(txtSP.Text) AndAlso IO.File.Exists(txtSP.Text) Then
        cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtSP.Text)
      Else
        If Not String.IsNullOrEmpty(txtWIM.Text) AndAlso IO.File.Exists(txtWIM.Text) Then
          cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtWIM.Text)
        Else
          cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
      End If
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Packages", "MSU,CAB,MLC,EXE,MSI,INF"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Windows Updates", "MSU,CAB"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Language Packs", "MLC,EXE,MSI,CAB"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Drivers", "INF"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = True
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Add Windows Updates..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.5_Updates\1.5.1_Add_Updates.htm")
      If cdlBrowse.ShowDialog(Me.Handle) = Windows.Forms.DialogResult.OK Then
        Dim FailCollection As New List(Of String)
        Dim FileCount As Integer = cdlBrowse.FileNames.Count
        RunActivity = ActivityType.LoadingUpdates
        StopRun = False
        TitleMNG_SetDisplay(TitleMNG_List.Delete)
        TitleMNG_SetTitle("Parsing Update Information", "Reading data from update files...")
        GUI_ToggleEnabled(False)
        Progress_Total(0, FileCount)
        Progress_Normal(0, 0)
        Status_SetText("Reading Update Information...")
        Dim msuCollection As New List(Of ListViewItem)
        For I As Integer = 0 To cdlBrowse.FileNames.Count - 1
          Dim sUpdate As String = cdlBrowse.FileNames(I)
          Progress_Total(I + 1, FileCount)
          Progress_Normal(0, 1)
          Application.DoEvents()
          If StopRun Then
            Progress_Normal(0, 1)
            GUI_ToggleEnabled(True)
            Return
          End If
          Dim Cancelled As Boolean = False
          Dim newUpdateList As Update_File() = GetUpdateInfo(sUpdate)
          Dim UpdateCount As Integer = 0
          If newUpdateList IsNot Nothing Then UpdateCount = newUpdateList.Count
          Dim iProg2 As Integer = 0
          Progress_Normal(0, UpdateCount)
          If UpdateCount > 0 Then
            For Each msuData As Update_File In newUpdateList
              Dim addRet As Updates_AddFile_Result = Updates_AddFile(msuData, msuCollection)
              iProg2 += 1
              Progress_Normal(iProg2, UpdateCount)
              If addRet.Cancel Then
                Cancelled = True
                Exit For
              End If
              If Not addRet.Success Then
                Dim sUpdName As String = msuData.Name
                If String.IsNullOrEmpty(msuData.Name) Then
                  sUpdName = IO.Path.GetFileNameWithoutExtension(sUpdate)
                ElseIf msuData.Name = "DRIVER" Then
                  sUpdName = IO.Path.GetFileNameWithoutExtension(msuData.DriverData.DriverStorePath)
                End If
                FailCollection.Add(String.Format("{0}: {1}", sUpdName, addRet.FailReason))
              End If
            Next
          Else
            FailCollection.Add(String.Format("{0}: Update not found.", IO.Path.GetFileNameWithoutExtension(sUpdate)))
          End If
          If Cancelled Then Exit For
        Next
        If msuCollection.Count > 0 Then
          lvMSU.Items.AddRange(msuCollection.ToArray)
          Updates_Requirements()
          SortMSUsForIntegration(lvMSU)
        End If
        Progress_Normal(0, 1)
        GUI_ToggleEnabled(True)
        GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
        If FailCollection.Count > 0 Then
          MsgDlg(Me, String.Concat("Some files could not be added to the Update List.", vbNewLine, "Click View Details to see a complete list."), "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, _TaskDialogIcon.WindowsUpdate, , Updates_FailureCleanup(FailCollection), "Error Adding Updates")
        End If
      End If
    End Using
    Status_SetText("Idle")
  End Sub
  Private Sub cmdRemMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdRemMSU.Click
    If lvMSU.Items.Count > 0 Then
      Dim lIndex As Integer = -1
      For Each lvItem As ListViewItem In lvMSU.SelectedItems
        Dim lvIndex As Integer = CInt(lvItem.Tag)
        lIndex = lvItem.Index
        If Updates_ListData.ContainsKey(lvIndex) Then Updates_ListData.Remove(lvIndex)
        lvItem.Remove()
      Next
      If lIndex >= 0 And lvMSU.Items.Count > 0 Then
        If lIndex >= lvMSU.Items.Count Then lIndex = lvMSU.Items.Count - 1
        lvMSU.Items(lIndex).Selected = True
      End If
      Updates_Requirements()
      Status_SetText("Idle")
      GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
      cmdRemMSU.Focus()
    Else
      Beep()
    End If
  End Sub
  Private Sub cmdClearMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdClearMSU.Click
    If lvMSU.Items.Count > 0 Then
      Dim sMsg As String = "All updates will be removed from the list."
      If lvMSU.Items.Count > 2 Then sMsg = String.Format("All {0} updates will be removed from the list.", lvMSU.Items.Count)
      If MsgDlg(Me, sMsg, "Do you want to clear the Update List?", "Remove All Updates", MessageBoxButtons.YesNo, _TaskDialogIcon.Delete, MessageBoxDefaultButton.Button2, , "Remove All Updates") = Windows.Forms.DialogResult.Yes Then
        Updates_ListData.Clear()
        lvMSU.Items.Clear()
        Status_SetText("Idle")
        GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
      End If
    Else
      Beep()
    End If
  End Sub
#End Region
#Region "Context Menu"
  Private Sub mnuUpdateTop_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateTop.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim targetI As Integer = 0
    For Each item As ListViewItem In Updates_ListSelection
      Dim sItemName As String = item.Name
      Dim tmpItem As ListViewItem = CType(item.Clone, ListViewItem)
      item.Remove()
      lvMSU.Items.Insert(targetI, tmpItem)
      lvMSU.Items(targetI).Name = sItemName
      lvMSU.Items(targetI).Selected = True
      targetI += 1
    Next
  End Sub
  Private Sub mnuUpdateUp_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateUp.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim targetI As Integer = Integer.MaxValue
    For Each item As ListViewItem In Updates_ListSelection
      If item.Index < targetI Then targetI = item.Index
    Next
    If targetI = Integer.MaxValue Or targetI < 0 Then Return
    If targetI > 0 Then targetI -= 1
    For Each item As ListViewItem In Updates_ListSelection
      Dim sItemName As String = item.Name
      Dim tmpItem As ListViewItem = CType(item.Clone, ListViewItem)
      item.Remove()
      lvMSU.Items.Insert(targetI, tmpItem)
      lvMSU.Items(targetI).Name = sItemName
      lvMSU.Items(targetI).Selected = True
      targetI += 1
    Next
  End Sub
  Private Sub mnuUpdateDown_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateDown.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim targetI As Integer = Integer.MinValue
    For Each item As ListViewItem In Updates_ListSelection
      If item.Index > targetI Then targetI = item.Index
    Next
    If targetI = Integer.MinValue Or targetI > lvMSU.Items.Count - 1 Then Return
    targetI += 1
    For Each item As ListViewItem In Updates_ListSelection
      Dim sItemName As String = item.Name
      Dim tmpItem As ListViewItem = CType(item.Clone, ListViewItem)
      item.Remove()
      If lvMSU.Items.Count <= targetI Then
        targetI = CType(lvMSU.Items.Add(tmpItem), ListViewItem).Index
      Else
        lvMSU.Items.Insert(targetI, tmpItem)
      End If
      lvMSU.Items(targetI).Name = sItemName
      lvMSU.Items(targetI).Selected = True
    Next
  End Sub
  Private Sub mnuUpdateBottom_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateBottom.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    For Each item As ListViewItem In Updates_ListSelection
      Dim sItemName As String = item.Name
      Dim tmpItem As ListViewItem = CType(item.Clone, ListViewItem)
      item.Remove()
      lvMSU.Items.Add(tmpItem)
      tmpItem.Name = sItemName
      tmpItem.Selected = True
    Next
  End Sub
  Private Sub mnuUpdateRemove_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateRemove.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim lIndex As Integer = -1
    For Each lvItem As ListViewItem In Updates_ListSelection
      lIndex = lvItem.Index
      lvItem.Remove()
    Next
    If lIndex >= 0 And lvMSU.Items.Count > 0 Then
      If lIndex >= lvMSU.Items.Count Then lIndex = lvMSU.Items.Count - 1
      lvMSU.Items(lIndex).Selected = True
    End If
    Status_SetText("Idle")
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.MSU)
  End Sub
  Private Sub mnuUpdatePEDriver_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateBootPEDriver.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim enable As Boolean = Not mnuUpdateBootPEDriver.Checked
    For Each lvItem As ListViewItem In Updates_ListSelection
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      Dim itemData As Updates_Data = Updates_ListData(lvIndex)
      If itemData.Update.Name = "DRIVER" Then
        itemData.Update.DriverData.IntegrateIntoBootPE = enable
        Updates_ListData(lvIndex) = itemData
      End If
    Next
    Updates_Requirements()
  End Sub
  Private Sub mnuUpdateBootDriver_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateBootSetupDriver.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim enable As Boolean = Not mnuUpdateBootSetupDriver.Checked
    For Each lvItem As ListViewItem In Updates_ListSelection
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      Dim itemData As Updates_Data = Updates_ListData(lvIndex)
      If itemData.Update.Name = "DRIVER" Then
        itemData.Update.DriverData.IntegrateIntoBootSetup = enable
        Updates_ListData(lvIndex) = itemData
      End If
    Next
    Updates_Requirements()
  End Sub
  Private Sub mnuUpdateLocation_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateLocation.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Dim itemDirs As New SortedList(Of String, List(Of String))()
    For Each lvItem As ListViewItem In Updates_ListSelection
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      Dim itemDir As String = IO.Path.GetDirectoryName(Updates_ListData(lvIndex).Update.Path)
      If itemDirs.Count > 0 Then
        If Not itemDirs.ContainsKey(itemDir) Then
          itemDirs.Add(itemDir, New List(Of String))
        End If
      Else
        itemDirs.Add(itemDir, New List(Of String))
      End If
      itemDirs(itemDir).Add(Updates_ListData(lvIndex).Update.Path)
    Next
    If itemDirs.Count > 0 Then
      If itemDirs.Count > 1 Then
        For Each sItem In itemDirs
          clsExplorer.OpenFolderAndSelectFiles(sItem.Key, sItem.Value.ToArray)
        Next
      Else
        clsExplorer.OpenFolderAndSelectFiles(itemDirs.First.Key, itemDirs.First.Value.ToArray)
      End If
    End If
  End Sub
  Private Sub mnuUpdateProperties_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateProperties.Click
    If Updates_ListSelection Is Nothing OrElse Updates_ListSelection.Count = 0 Then Return
    Updates_Properties(Updates_ListSelection)
  End Sub
#End Region
  Private Sub Updates_Requirements()
    If PrerequisiteList.Length > 0 Then
      For U As Integer = 0 To lvMSU.Items.Count - 1
        Dim lvIndex As Integer = CInt(lvMSU.Items(U).Tag)
        Dim lvItem As ListViewItem = lvMSU.Items(U)
        Dim msuData As Update_File = Updates_ListData(lvIndex).Update
        If msuData.Name = "DRIVER" Then
          If msuData.DriverData.IntegrateIntoBootPE And msuData.DriverData.IntegrateIntoBootSetup Then
            If Not lvItem.ForeColor = mySettings.DriverColorBootAndPE Then
              lvItem.ForeColor = mySettings.DriverColorBootAndPE
              lvItem.ToolTipText = msuData.DriverData.ToString
            End If
          ElseIf msuData.DriverData.IntegrateIntoBootPE Then
            If Not lvItem.ForeColor = mySettings.DriverColorPE Then
              lvItem.ForeColor = mySettings.DriverColorPE
              lvItem.ToolTipText = msuData.DriverData.ToString
            End If
          ElseIf msuData.DriverData.IntegrateIntoBootSetup Then
            If Not lvItem.ForeColor = mySettings.DriverColorBoot Then
              lvItem.ToolTipText = msuData.DriverData.ToString
              lvItem.ForeColor = mySettings.DriverColorBoot
            End If
          Else
            If Not lvItem.ForeColor = lvMSU.ForeColor Then
              lvItem.ForeColor = lvMSU.ForeColor
              lvItem.ToolTipText = msuData.DriverData.ToString
            End If
          End If
          Continue For
        End If
        If msuData.Ident.Name = "Microsoft-Windows-LIP-LanguagePack-Package" Then
          Dim langList As New List(Of String)
          Dim hasSP As Boolean = False
          If lvImages.Items.Count > 0 Then
            For Each lvImage As ListViewItem In lvImages.Items
              Dim lvIndex2 As Integer = CInt(lvImage.Tag)
              Dim imageInfo As ImagePackage = ImagePackage_ListData(lvIndex2).Package
              If imageInfo.SPLevel > 0 Then hasSP = True
              For Each langReg In imageInfo.LangList
                If langReg.Contains("-") Then
                  Dim lang As String = langReg.Substring(0, langReg.LastIndexOf("-"))
                  If Not langList.Contains(lang) Then langList.Add(lang)
                Else
                  If Not langList.Contains(langReg) Then langList.Add(langReg)
                End If
              Next
            Next
          End If
          If lvMSU.Items.Count > 0 Then
            For Each lvUpdate As ListViewItem In lvMSU.Items
              Dim lvIndex2 As Integer = CInt(lvUpdate.Tag)
              Dim updateInfo As Update_File = Updates_ListData(lvIndex2).Update
              If updateInfo.Ident.Name = "Microsoft-Windows-Client-Refresh-LanguagePack-Package" Or updateInfo.Ident.Name = "Microsoft-Windows-Client-LanguagePack-Package" Then
                If updateInfo.Ident.Language.Contains("-") Then
                  Dim lang As String = updateInfo.Ident.Language.Substring(0, updateInfo.Ident.Language.LastIndexOf("-"))
                  If Not langList.Contains(lang) Then langList.Add(lang)
                Else
                  If Not langList.Contains(updateInfo.Ident.Language) Then langList.Add(updateInfo.Ident.Language)
                End If
              End If
            Next
          End If
          Dim cItem As Color = lvMSU.ForeColor
          Dim sReqLine As String = Nothing
          If langList.Count > 0 Then
            Select Case msuData.Ident.Language.ToLower
              Case "af-za", "sq-al", "am-et", "hy-am", "as-in", "bn-bd", "bn-in", "prs-af",
                   "fil-ph", "ka-ge", "gu-in", "ha-latn-ng", "hi-in", "is-is", "ig-ng", "id-id",
                   "iu-latn-ca", "ga-ie", "kn-in", "km-kh", "sw-ke", "kok-in", "mk-mk", "ms-my",
                   "ms-bn", "ml-in", "mt-mt", "mi-nz", "mr-in", "ne-np", "or-in", "fa-ir",
                   "pa-in", "nso-za", "tn-za", "si-lk", "ta-in", "te-in", "ur-pk",
                   "vi-vn", "cy-gb", "xh-za", "yo-ng", "zu-za"
                If Not langList.Contains("en") Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure the English Language Pack is also integrated.")
                End If
              Case "gl-es", "quz-pe"
                If Not langList.Contains("es") Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure the Spanish Language Pack is also integrated.")
                End If
              Case "nn-no"
                If Not langList.Contains("nb") Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure the Norwegian (Bokmål) Language Pack is also integrated.")
                End If
              Case "ky-kg", "tt-ru"
                If Not langList.Contains("ru") Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure the Russian Language Pack is also integrated.")
                End If
              Case "eu-es", "ca-es"
                If Not (langList.Contains("es") Or langList.Contains("fr")) Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure either the French or the Spanish Language Pack is also integrated.")
                End If
              Case "bs-cyrl-ba", "bs-latn-ba"
                If Not (langList.Contains("en") Or langList.Contains("hr") Or langList.Contains("sr-latn")) Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English, the Croatian, or the Serbian (Latin) Language Pack is also integrated.")
                End If
              Case "sr-cyrl-cs"
                If Not (langList.Contains("en") Or langList.Contains("sr-latn")) Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the Serbian (Latin) Language Pack is also integrated.")
                End If
              Case "lb-lu"
                If Not (langList.Contains("en") Or langList.Contains("fr")) Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the French Language Pack is also integrated.")
                End If
              Case "az-latn-az", "kk-kz", "mn-mn", "tk-tm", "uz-latn-uz"
                If Not (langList.Contains("en") Or langList.Contains("ru")) Then
                  cItem = mySettings.UpdateColorRequirement
                  sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the Russian Language Pack is also integrated.")
                End If
            End Select
          Else
            Select Case msuData.Ident.Language.ToLower
              Case "af-za", "sq-al", "am-et", "hy-am", "as-in", "bn-bd", "bn-in", "prs-af",
                   "fil-ph", "ka-ge", "gu-in", "ha-latn-ng", "hi-in", "is-is", "ig-ng", "id-id",
                   "iu-latn-ca", "ga-ie", "kn-in", "km-kh", "sw-ke", "kok-in", "mk-mk", "ms-my",
                   "ms-bn", "ml-in", "mt-mt", "mi-nz", "mr-in", "ne-np", "or-in", "fa-ir",
                   "pa-in", "nso-za", "tn-za", "si-lk", "ta-in", "te-in", "ur-pk",
                   "vi-vn", "cy-gb", "xh-za", "yo-ng", "zu-za"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure the English Language Pack is also integrated.")
              Case "gl-es", "quz-pe"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure the Spanish Language Pack is also integrated.")
              Case "nn-no"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure the Norwegian (Bokmål) Language Pack is also integrated.")
              Case "ky-kg", "tt-ru"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure the Russian Language Pack is also integrated.")
              Case "eu-es", "ca-es"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure either the French or the Spanish Language Pack is also integrated.")
              Case "bs-cyrl-ba", "bs-latn-ba"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English, the Croatian, or the Serbian (Latin) Language Pack is also integrated.")
              Case "sr-cyrl-cs"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the Serbian (Latin) Language Pack is also integrated.")
              Case "lb-lu"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the French Language Pack is also integrated.")
              Case "az-latn-az", "kk-kz", "mn-mn", "tk-tm", "uz-latn-uz"
                cItem = mySettings.UpdateColorRequirement
                sReqLine = String.Concat(vbNewLine, en, "Please make sure either the English or the Russian Language Pack is also integrated.")
            End Select
          End If
          If chkSP.Checked Or hasSP Then
            If msuData.Ident.Version = "6.1.7600.16385" Then
              Select Case msuData.Ident.Language.ToLower
                Case "ca-es", "cy-gb", "hi-in", "is-is", "sr-cyrl-cs"
                  cItem = Color.Red
                  sReqLine = String.Concat(vbNewLine, en, "This Language Interface Pack has been superseded by Service Pack 1 and may not integrate correctly.")
              End Select
            End If
          End If
          If Not lvItem.ForeColor = cItem Then lvItem.ForeColor = cItem
          If Not String.IsNullOrEmpty(sReqLine) Then
            If Not lvItem.ToolTipText.Contains(sReqLine) Then lvItem.ToolTipText = String.Concat(lvItem.ToolTipText, sReqLine)
          Else
            Dim sOldLine As String = String.Concat(vbNewLine, en, "Please make sure")
            Dim sOldLine2 As String = String.Concat(vbNewLine, en, "This Language Interface Pack has been superseded by Service Pack 1 and may not integrate correctly.")
            If lvItem.ToolTipText.Contains(sOldLine) Then
              Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(sOldLine))
              Dim SecondHalf As String = Nothing
              If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(sOldLine) + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(sOldLine) + 2))
              lvItem.ToolTipText = String.Concat(FirstHalf, SecondHalf)
            ElseIf lvItem.ToolTipText.Contains(sOldLine2) Then
              Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(sOldLine2))
              Dim SecondHalf As String = Nothing
              If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(sOldLine2) + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(sOldLine2) + 2))
              lvItem.ToolTipText = String.Concat(FirstHalf, SecondHalf)
            End If
            lvItem.ToolTipText = String.Concat(lvItem.ToolTipText, sReqLine)
          End If
          Continue For
        End If

        For Each prereq As Updates_Prerequisite In PrerequisiteList
          If msuData.KBArticle = prereq.KBWithRequirement And prereq.Requirement.Length > 0 Then
            If prereq.Requirement.Length = 1 Then
              If prereq.Requirement(0).Length > 0 Then
                Dim Req(prereq.Requirement(0).Length - 1) As Boolean
                For I As Integer = 0 To Req.Length - 1
                  Req(I) = False
                Next
                If lvMSU.Items.Count > 0 Then
                  For Each searchItem As ListViewItem In lvMSU.Items
                    Dim lvIndex2 As Integer = CInt(searchItem.Tag)
                    Dim searchData As Update_File = Updates_ListData(lvIndex2).Update
                    For I As Integer = 0 To prereq.Requirement(0).Length - 1
                      If searchData.KBArticle = prereq.Requirement(0)(I) Then Req(I) = True
                    Next
                    If Not Req.Contains(False) Then Exit For
                  Next
                End If
                If Req.Contains(False) Then
                  If lvImages.Items.Count > 0 Then
                    For Each searchImage As ListViewItem In lvImages.Items
                      Dim lvIndex2 As Integer = CInt(searchImage.Tag)
                      Dim searchItem As ImagePackage = ImagePackage_ListData(lvIndex2).Package
                      If Not CompareArchitectures(searchItem.Architecture, msuData.Architecture, True) Then Continue For
                      If searchItem.IntegratedUpdateList IsNot Nothing AndAlso searchItem.IntegratedUpdateList.Count > 0 Then
                        For Each searchPackage As Update_Integrated In searchItem.IntegratedUpdateList
                          For I As Integer = 0 To prereq.Requirement(0).Length - 1
                            If searchPackage.Ident.Name.Contains(prereq.Requirement(0)(I)) Then Req(I) = True
                            If Not String.IsNullOrEmpty(searchPackage.UpdateInfo.Identity) AndAlso searchPackage.UpdateInfo.Name.Contains(prereq.Requirement(0)(I)) Then Req(I) = True
                          Next
                        Next
                      End If
                      If Not Req.Contains(False) Then Exit For
                    Next
                  End If
                End If
                If Req.Contains(False) Then
                  lvItem.ForeColor = mySettings.UpdateColorRequirement
                  Dim myReqs As New List(Of String)
                  For I As Integer = 0 To prereq.Requirement(0).Length - 1
                    If Not Req(I) Then myReqs.Add(prereq.Requirement(0)(I))
                  Next
                  Dim sReqLine As String = Nothing
                  If myReqs.Count = 1 Then
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure KB{0} is also integrated.", myReqs(0)))
                  ElseIf myReqs.Count = 2 Then
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure KB{0} and KB{1} are also integrated.", myReqs(0), myReqs(1)))
                  ElseIf myReqs.Count > 2 Then
                    Dim sKBList As String = Nothing
                    For I As Integer = 0 To myReqs.Count - 2
                      sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(I)), ", ")
                    Next
                    sKBList = String.Concat(sKBList, String.Format("and KB{0}", myReqs(myReqs.Count - 1)))
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure {0} are also integrated.", sKBList))
                  End If
                  If Not String.IsNullOrEmpty(sReqLine) Then
                    If Not lvItem.ToolTipText.Contains(sReqLine) Then
                      Dim sOldLine As String = String.Concat(vbNewLine, en, "Please make sure")
                      If lvItem.ToolTipText.Contains(sOldLine) Then
                        Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(sOldLine))
                        Dim SecondHalf As String = Nothing
                        If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(sOldLine) + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(sOldLine) + 2))
                        lvItem.ToolTipText = String.Concat(FirstHalf, SecondHalf)
                      End If
                      lvItem.ToolTipText = String.Concat(lvItem.ToolTipText, sReqLine)
                    End If
                  End If
                End If
              End If
            Else
              Dim ReqA(prereq.Requirement.Length - 1) As Boolean
              For A As Integer = 0 To ReqA.Length - 1
                ReqA(A) = False
              Next
              Dim Req(prereq.Requirement.Length - 1)() As Boolean
              For I As Integer = 0 To Req.Length - 1
                ReDim Req(I)(prereq.Requirement(I).Length - 1)
              Next
              If lvImages.Items.Count > 0 Then
                For Each searchImage As ListViewItem In lvImages.Items
                  Dim lvIndex2 As Integer = CInt(searchImage.Tag)
                  Dim searchItem As ImagePackage = ImagePackage_ListData(lvIndex2).Package
                  If Not CompareArchitectures(searchItem.Architecture, msuData.Architecture, True) Then Continue For
                  If searchItem.IntegratedUpdateList IsNot Nothing AndAlso searchItem.IntegratedUpdateList.Count > 0 Then
                    For Each searchPackage As Update_Integrated In searchItem.IntegratedUpdateList
                      For A As Integer = 0 To prereq.Requirement.Length - 1
                        For I As Integer = 0 To prereq.Requirement(A).Length - 1
                          If searchPackage.Ident.Name.Contains(prereq.Requirement(A)(I)) Then
                            ReqA(A) = True
                            Req(A)(I) = True
                          End If
                        Next
                      Next
                    Next
                  End If
                Next
              End If
              If lvMSU.Items.Count > 0 Then
                For Each searchItem As ListViewItem In lvMSU.Items
                  Dim lvIndex2 As Integer = CInt(searchItem.Tag)
                  Dim searchData As Update_File = Updates_ListData(lvIndex2).Update
                  For A As Integer = 0 To prereq.Requirement.Length - 1
                    For I As Integer = 0 To prereq.Requirement(A).Length - 1
                      If searchData.KBArticle = prereq.Requirement(A)(I) Then
                        ReqA(A) = True
                        Req(A)(I) = True
                      End If
                    Next
                  Next
                Next
              End If
              Dim ReqB(ReqA.Length - 1) As Boolean
              For B As Integer = 0 To ReqB.Length - 1
                If ReqA(B) Then
                  ReqB(B) = Not Req(B).Contains(False)
                Else
                  ReqB(B) = False
                End If
              Next
              If Not ReqB.Contains(True) Then
                lvItem.ForeColor = mySettings.UpdateColorRequirement
                Dim partialCount As Integer = ReqA.Count(Function(a As Boolean) As Boolean
                                                           Return a
                                                         End Function)
                Dim sReqLine As String = Nothing
                If partialCount = 0 Then
                  Dim sKBList As String = Nothing
                  For A As Integer = 0 To Req.Length - 1
                    Dim myReqs As New List(Of String)
                    For I As Integer = 0 To prereq.Requirement(A).Length - 1
                      If Not Req(A)(I) Then myReqs.Add(prereq.Requirement(A)(I))
                    Next
                    If myReqs.Count = 1 Then
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(0)))
                    ElseIf myReqs.Count = 2 Then
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      sKBList = String.Concat(sKBList, String.Format("KB{0} and KB{1}", myReqs(0), myReqs(1)))
                    Else
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      For I As Integer = 0 To myReqs.Count - 2
                        sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(I)), ", ")
                      Next
                      sKBList = String.Concat(sKBList, String.Format("and KB{0}", myReqs(myReqs.Count - 1).ToString))
                    End If
                  Next
                  If sKBList.Contains(" and ") Then
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure either  {0}  are also integrated.", sKBList))
                  Else
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure either  {0}  is also integrated.", sKBList))
                  End If
                ElseIf partialCount = 1 Then
                  Dim validReq As Integer = Array.IndexOf(Of Boolean)(ReqA, True)
                  Dim myReqs As New List(Of String)
                  For I As Integer = 0 To prereq.Requirement(validReq).Length - 1
                    If Not Req(validReq)(I) Then myReqs.Add(prereq.Requirement(validReq)(I))
                  Next
                  If myReqs.Count = 1 Then
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure KB{0} is also integrated.", myReqs(0)))
                  ElseIf myReqs.Count = 2 Then
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure KB{0} and KB{1} are also integrated.", myReqs(0), myReqs(1)))
                  Else
                    Dim sKBList As String = Nothing
                    For I As Integer = 0 To myReqs.Count - 2
                      sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(I)), ", ")
                    Next
                    sKBList = String.Concat(sKBList, String.Format("and KB{0}", myReqs(myReqs.Count - 1)))
                    sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure {0} are also integrated.", sKBList))
                  End If
                Else
                  Dim validReqs As New List(Of Integer)
                  Dim highVal As Double = 0
                  For A As Integer = 0 To ReqA.Length - 1
                    If Not ReqA(A) Then Continue For
                    Dim aCount As Integer = Req(A).Count(Function(c As Boolean) As Boolean
                                                           Return c
                                                         End Function)
                    If (aCount / Req(A).Length) > highVal Then highVal = (aCount / Req(A).Length)
                  Next
                  For A As Integer = 0 To ReqA.Length - 1
                    If Not ReqA(A) Then Continue For
                    Dim aCount As Integer = Req(A).Count(Function(c As Boolean) As Boolean
                                                           Return c
                                                         End Function)
                    If (aCount / Req(A).Length) >= highVal - 0.1 Then validReqs.Add(A)
                  Next
                  Dim sKBList As String = Nothing
                  For Each A As Integer In validReqs
                    If Not ReqA(A) Then Continue For
                    Dim myReqs As New List(Of String)
                    For I As Integer = 0 To prereq.Requirement(A).Length - 1
                      If Not Req(A)(I) Then myReqs.Add(prereq.Requirement(A)(I))
                    Next
                    If myReqs.Count = 1 Then
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(0)))
                    ElseIf myReqs.Count = 2 Then
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      sKBList = String.Concat(sKBList, String.Format("KB{0} and KB{1}", myReqs(0), myReqs(1)))
                    Else
                      If Not String.IsNullOrEmpty(sKBList) Then sKBList = String.Concat(sKBList, "  or  ")
                      For I As Integer = 0 To myReqs.Count - 2
                        sKBList = String.Concat(sKBList, String.Format("KB{0}", myReqs(I)), ", ")
                      Next
                      sKBList = String.Concat(sKBList, String.Format("and KB{0}", myReqs(myReqs.Count - 1).ToString))
                    End If
                  Next
                  If validReqs.Count > 1 Then
                    If sKBList.Contains(" and ") Then
                      sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure either  {0}  are also integrated.", sKBList))
                    Else
                      sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure either  {0}  is also integrated.", sKBList))
                    End If
                  Else
                    If sKBList.Contains(" and ") Then
                      sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure {0} are also integrated.", sKBList))
                    Else
                      sReqLine = String.Concat(vbNewLine, en, String.Format("Please make sure {0} is also integrated.", sKBList))
                    End If
                  End If
                End If
                If Not String.IsNullOrEmpty(sReqLine) Then
                  If Not lvItem.ToolTipText.Contains(sReqLine) Then
                    Dim sOldLine As String = String.Concat(vbNewLine, en, "Please make sure")
                    If lvItem.ToolTipText.Contains(sOldLine) Then
                      Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(sOldLine))
                      Dim SecondHalf As String = Nothing
                      If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(sOldLine) + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(sOldLine) + 2))
                      lvItem.ToolTipText = String.Concat(FirstHalf, SecondHalf)
                    End If
                    lvItem.ToolTipText = String.Concat(lvItem.ToolTipText, sReqLine)
                  End If
                End If
              End If
            End If
          End If
          For A As Integer = 0 To prereq.Requirement.Length - 1
            For Each sReq As String In prereq.Requirement(A)
              If msuData.KBArticle = sReq Then
                Dim Req As Boolean = False
                If prereq.Requirement(A).Length < 2 Then
                  Req = True
                Else
                  Dim Reqs(prereq.Requirement(A).Length - 1) As Boolean
                  For I As Integer = 0 To Reqs.Length - 1
                    Reqs(I) = False
                  Next
                  For I As Integer = 0 To prereq.Requirement(A).Length - 1
                    If prereq.Requirement(A)(I) = sReq Then
                      Reqs(I) = True
                      Continue For
                    End If
                    For Each searchItem As ListViewItem In lvMSU.Items
                      Dim lvIndex2 As Integer = CInt(searchItem.Tag)
                      Dim searchData As Update_File = Updates_ListData(lvIndex2).Update
                      If searchData.KBArticle = prereq.Requirement(A)(I) Then
                        Reqs(I) = True
                        Exit For
                      End If
                    Next
                    If lvImages.Items.Count > 0 Then
                      For Each searchImage As ListViewItem In lvImages.Items
                        Dim lvIndex2 As Integer = CInt(searchImage.Tag)
                        Dim searchItem As ImagePackage = ImagePackage_ListData(lvIndex2).Package
                        If Not CompareArchitectures(searchItem.Architecture, msuData.Architecture, True) Then Continue For
                        If searchItem.IntegratedUpdateList IsNot Nothing AndAlso searchItem.IntegratedUpdateList.Count > 0 Then
                          For Each searchPackage As Update_Integrated In searchItem.IntegratedUpdateList
                            If searchPackage.Ident.Name.Contains(prereq.Requirement(A)(I)) Then
                              Reqs(I) = True
                              Exit For
                            End If
                          Next
                        End If
                      Next
                    End If
                  Next
                  Req = Not Reqs.Contains(False)
                End If
                If Req Then
                  For Each searchItem As ListViewItem In lvMSU.Items
                    Dim lvIndex2 As Integer = CInt(searchItem.Tag)
                    Dim searchData As Update_File = Updates_ListData(lvIndex2).Update
                    If searchData.KBArticle = prereq.KBWithRequirement Then
                      searchItem.ForeColor = lvMSU.ForeColor
                      If searchItem.ToolTipText.Contains("Please make sure") Then searchItem.ToolTipText = searchItem.ToolTipText.Substring(0, searchItem.ToolTipText.LastIndexOf(vbNewLine))
                      Exit For
                    End If
                  Next
                ElseIf lvItem.ForeColor = mySettings.UpdateColorRequirement And Not msuData.Ident.Name = "Microsoft-Windows-LIP-LanguagePack-Package" Then
                  lvItem.ForeColor = lvMSU.ForeColor
                  Dim sOldLine As String = String.Concat(vbNewLine, en, "Please make sure")
                  If lvItem.ToolTipText.Contains(sOldLine) Then
                    Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(sOldLine))
                    Dim SecondHalf As String = Nothing
                    If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(sOldLine) + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(sOldLine) + 2))
                    lvItem.ToolTipText = String.Concat(FirstHalf, SecondHalf)
                  End If
                End If
              End If
            Next
          Next
        Next
      Next
    End If
  End Sub
  Private Sub Updates_Properties(items As ListView.SelectedListViewItemCollection)
    For Each lvItem As ListViewItem In items
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      Dim msuData As Update_File = Updates_ListData(lvIndex).Update
      If Not String.IsNullOrEmpty(msuData.Failure) Then If Extract_ShowFailure(msuData.Failure) Then Continue For
      If Not String.IsNullOrEmpty(msuData.Name) Then
        If msuData.Name = "DRIVER" Then
          Dim props As New frmDriverProps(msuData.DriverData)
          props.Show(Me)
        Else
          Dim bOpened As Boolean = False
          For Each Form In OwnedForms
            If Form.Name = String.Format("frmUpdate{0}Props", msuData.Ident.GetHashCode.ToString("X")) Then
              Form.Focus()
              bOpened = True
              Exit For
            End If
          Next
          If bOpened Then Continue For
          Dim props As New frmUpdateProps
          props.Name = String.Format("frmUpdate{0}Props", msuData.Ident.GetHashCode.ToString("X"))
          props.Text = String.Format("{0} Properties", GetUpdateName(msuData.Ident, msuData.ReleaseType))
          props.txtName.Text = msuData.Name
          props.txtDisplayName.Text = msuData.DisplayName
          props.txtIdentity.Text = msuData.Identity
          props.txtReleaseType.Text = msuData.ReleaseType
          props.txtAppliesTo.Text = msuData.AppliesTo
          props.txtArchitecture.Text = msuData.Architecture
          props.txtVersion.Text = msuData.KBVersion
          props.txtBuildDate.Text = msuData.BuildDate
          If String.IsNullOrEmpty(msuData.SupportLink) Then
            props.lblKBLink.Link = False
            props.KBLink = Nothing
            If String.IsNullOrEmpty(msuData.KBArticle) Then
              props.lblKBLink.Text = "Details"
              props.lblKBLink.Visible = False
            Else
              props.lblKBLink.Text = String.Format("Article KB{0}", msuData.KBArticle)
              props.lblKBLink.Visible = True
            End If
          Else
            props.lblKBLink.Link = True
            props.KBLink = msuData.SupportLink
            If String.IsNullOrEmpty(msuData.KBArticle) Then
              props.lblKBLink.Text = "Details"
              props.lblKBLink.Visible = True
            Else
              props.lblKBLink.Text = String.Format("Article KB{0}", msuData.KBArticle)
              props.lblKBLink.Visible = True
            End If
          End If
          props.ttLink.SetToolTip(props.lblKBLink, msuData.SupportLink)
          props.Show(Me)
        End If
      End If
    Next
  End Sub
  Private Function Updates_AddFile(msuData As Update_File, ByRef msuCollection As List(Of ListViewItem)) As Updates_AddFile_Result
    If Not String.IsNullOrEmpty(msuData.Failure) Then Return New Updates_AddFile_Result(msuData.Failure)
    If String.IsNullOrEmpty(msuData.Name) Then Return New Updates_AddFile_Result("Unable to parse information.")
    If GetUpdateType(msuData.Path) = UpdateType.Other Then
      If String.IsNullOrEmpty(IO.Path.GetExtension(msuData.Path)) Then
        Return New Updates_AddFile_Result("Unknown Update Type.")
      Else
        Return New Updates_AddFile_Result(String.Format("Unknown Update Type: ""{0}"".", IO.Path.GetExtension(msuData.Path).Substring(1).ToUpper))
      End If
    End If
    If msuData.Name = "DRIVER" Then
      Dim drvData As Driver = msuData.DriverData
      If String.IsNullOrEmpty(drvData.DriverStorePath) Then Return New Updates_AddFile_Result(False)
      If lvImages.Items.Count > 0 Then
        For I As Integer = 0 To lvImages.Items.Count - 1
          Dim lvIndex As Integer = CInt(lvImages.Items(I).Tag)
          If lvImages.Items(I).Checked Then
            Dim dInfo As List(Of Driver) = ImagePackage_ListData(lvIndex).DriverList
            If dInfo IsNot Nothing Then
              For Each driver As Driver In dInfo
                If drvData = driver Then Return New Updates_AddFile_Result("Driver already integrated.")
              Next
            End If
          End If
        Next
      End If
      For Each item As ListViewItem In lvMSU.Items
        Dim lvIndex As Integer = CInt(item.Tag)
        If Updates_ListData(lvIndex).Update.Name = "DRIVER" AndAlso Updates_ListData(lvIndex).Update.DriverData = drvData Then
          Return New Updates_AddFile_Result("Driver already added.")
        End If
      Next
      For Each item As ListViewItem In msuCollection
        Dim lvIndex As Integer = CInt(item.Tag)
        If Updates_ListData(lvIndex).Update.Name = "DRIVER" AndAlso Updates_ListData(lvIndex).Update.DriverData = drvData Then
          Return New Updates_AddFile_Result("Driver already added.")
        End If
      Next
      Dim sDriverDisplayName As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.OriginalFileName) Then
        sDriverDisplayName = IO.Path.GetFileNameWithoutExtension(msuData.DriverData.OriginalFileName)
      ElseIf Not String.IsNullOrEmpty(msuData.DriverData.PublishedName) Then
        sDriverDisplayName = IO.Path.GetFileNameWithoutExtension(msuData.DriverData.PublishedName)
      Else
        sDriverDisplayName = IO.Path.GetFileNameWithoutExtension(msuData.Path)
      End If
      Dim lvItem As New ListViewItem(sDriverDisplayName)
      lvItem.Name = String.Format("lviDriver{0}", AlphanumericVal(msuData.DriverData.PublishedName))
      lvItem.ToolTipText = msuData.DriverData.ToString
      Dim lTag As Integer = 0
      Do
        lTag += 1
        If lTag >= Integer.MaxValue Then Exit Do
      Loop While Updates_ListData.ContainsKey(lTag)
      Updates_ListData.Add(lTag, New Updates_Data(msuData, frmMain.Updates_Data.Update_Replace.OnlyMissing))
      lvItem.Tag = lTag
      lvItem.ForeColor = lvMSU.ForeColor
      lvItem.BackColor = SystemColors.Window
      If lvMSU.ReadOnly Then
        Updates_BackgroundColors.Add(lvItem.Name, lvItem.BackColor)
        lvItem.BackColor = lvMSU.BackColor
      End If
      lvItem.ImageKey = "INF"
      If msuData.DriverData.Architectures Is Nothing Then
        lvItem.SubItems.Add("Driver")
      Else
        Dim isx86 As Boolean = msuData.DriverData.Architectures.Exists(New Predicate(Of String)(Function(arch As String) As Boolean
                                                                                                  Return CompareArchitectures(arch, ArchitectureList.x86, True)
                                                                                                End Function))
        Dim isx64 As Boolean = msuData.DriverData.Architectures.Exists(New Predicate(Of String)(Function(arch As String) As Boolean
                                                                                                  Return CompareArchitectures(arch, ArchitectureList.amd64, True)
                                                                                                End Function))
        Dim isIA As Boolean = msuData.DriverData.Architectures.Exists(New Predicate(Of String)(Function(arch As String) As Boolean
                                                                                                 Return CompareArchitectures(arch, ArchitectureList.ia64, True)
                                                                                               End Function))
        If isx86 Then
          If isx64 Then
            lvItem.SubItems.Add("Universal Driver")
          Else
            lvItem.SubItems.Add("x86 Driver")
          End If
        ElseIf isx64 Then
          lvItem.SubItems.Add("amd64 Driver")
        ElseIf isIA Then
          Return New Updates_AddFile_Result("Driver is for Itanium processors.")
        Else
          Return New Updates_AddFile_Result(String.Format("Driver is for {0} processors.", Join(msuData.DriverData.Architectures.ToArray, ", ")))
        End If
      End If
      msuCollection.Add(lvItem)
      Return New Updates_AddFile_Result(True)
    Else
      Dim PList As New SortedList(Of Integer, ImagePackage)
      Dim InOne As Boolean = False
      Dim UpdateAction As Updates_Data.Update_Replace
      Dim InImage As New SortedList(Of Integer, Update_Integrated)
      If lvImages.Items.Count > 0 Then
        Dim n As Integer = -1
        For I As Integer = 0 To lvImages.Items.Count - 1
          Dim lvIndex As Integer = CInt(lvImages.Items(I).Tag)
          If lvImages.Items(I).Checked Then
            n += 1
            PList.Add(n, ImagePackage_ListData(lvIndex).Package)
            If Not CheckWhitelist(msuData.DisplayName) AndAlso Not CompareArchitectures(PList(n).Architecture, msuData.Architecture, False) Then Continue For
            If PList(n).IntegratedUpdateList IsNot Nothing Then
              For Each package As Update_Integrated In PList(n).IntegratedUpdateList
                If msuData.Ident.Name = package.Ident.Name Then
                  If CompareArchitectures(msuData.Ident.Architecture, package.Ident.Architecture, False) Then
                    If msuData.Ident.Language = package.Ident.Language Then
                      package.Parent = PList(n)
                      InImage.Add(n, package)
                      InOne = True
                      Exit For
                    End If
                  End If
                End If
              Next
            End If
          End If
        Next
        If InOne Then
          Dim allEqual As Boolean = True
          Dim allGreater As Boolean = True
          Dim allLess As Boolean = True
          For Each I As Integer In InImage.Keys
            If Not CheckWhitelist(msuData.DisplayName) AndAlso Not CompareArchitectures(InImage(I).Parent.Architecture, msuData.Architecture, False) Then Continue For
            If String.IsNullOrEmpty(InImage(I).Identity) Then
              allEqual = False
              Continue For
            End If
            Dim CompareRet As Integer = CompareMSVersions(InImage(I).Ident.Version, msuData.Ident.Version)
            If CompareRet = 0 Then
              allGreater = False
              allLess = False
            ElseIf CompareRet < 0 Then
              allGreater = False
              allEqual = False
            Else
              allLess = False
              allEqual = False
            End If
            If Not allGreater And Not allLess And Not allEqual Then Exit For
          Next
          If allEqual Then
            Return New Updates_AddFile_Result("Update already integrated.")
          ElseIf allLess Then
            Dim always As Boolean = False
            If Updates_ReplaceAllOld = TriState.True Then
              UpdateAction = Updates_Data.Update_Replace.OnlyOlder
            ElseIf Updates_ReplaceAllOld = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage.Values.ToArray, PList.Values.ToArray, always, Comparison.Newer)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllOld = TriState.True
                UpdateAction = Updates_Data.Update_Replace.OnlyOlder
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllOld = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          ElseIf allGreater Then
            Dim always As Boolean = False
            If Updates_ReplaceAllNew = TriState.True Then
              UpdateAction = Updates_Data.Update_Replace.OnlyNewer
            ElseIf Updates_ReplaceAllNew = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage.Values.ToArray, PList.Values.ToArray, always, Comparison.Older)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllNew = TriState.True
                UpdateAction = Updates_Data.Update_Replace.OnlyMissing
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllNew = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          Else
            Dim sRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage.Values.ToArray, PList.Values.ToArray, False, Comparison.Mixed)
            If sRet = TaskDialogResult.Ok Then
              UpdateAction = Updates_Data.Update_Replace.All
            ElseIf sRet = TaskDialogResult.Yes Then
              UpdateAction = Updates_Data.Update_Replace.OnlyOlder
            ElseIf sRet = TaskDialogResult.No Then
              UpdateAction = Updates_Data.Update_Replace.OnlyNewer
            ElseIf sRet = TaskDialogResult.Close Then
              UpdateAction = Updates_Data.Update_Replace.OnlyMissing
            ElseIf sRet = TaskDialogResult.Cancel Then
              Return New Updates_AddFile_Result(False)
            End If
          End If
        End If
      End If
      For Each item As ListViewItem In lvMSU.Items
        Dim lvIndex As Integer = CInt(item.Tag)
        Dim itemData As Update_File = Updates_ListData(lvIndex).Update
        If itemData.Name = "DRIVER" Then Continue For
        If itemData.Identity = msuData.Identity Then
          Return New Updates_AddFile_Result("Update already added.")
        ElseIf Replace(itemData.Ident.Name.ToLower, "-refresh-", "-") = Replace(msuData.Ident.Name.ToLower, "-refresh-", "-") And itemData.Ident.Language.ToLower = msuData.Ident.Language.ToLower And CompareArchitectures(itemData.Architecture, msuData.Architecture, True) Then
          Dim always As Boolean = False
          Dim CompareRet As Integer = CompareMSVersions(itemData.Ident.Version, msuData.Ident.Version)
          If CompareRet = 0 Then
            Return New Updates_AddFile_Result("Update already added.")
          ElseIf CompareRet > 0 Then
            If Updates_ReplaceAllOld = TriState.True Then
              item.Remove()
            ElseIf Updates_ReplaceAllOld = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllOld = TriState.True
                item.Remove()
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllOld = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          ElseIf CompareRet < 0 Then
            If Updates_ReplaceAllNew = TriState.True Then
              item.Remove()
            ElseIf Updates_ReplaceAllNew = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllNew = TriState.True
                item.Remove()
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllNew = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          End If
        End If
      Next
      For I As Integer = msuCollection.Count - 1 To 0 Step -1
        Dim lvIndex As Integer = CInt(msuCollection(I).Tag)
        Dim itemData As Update_File = Updates_ListData(lvIndex).Update
        If itemData.Name = "DRIVER" Then Continue For
        If itemData.Identity = msuData.Identity Then
          Return New Updates_AddFile_Result("Update already added.")
        ElseIf Replace(itemData.Ident.Name.ToLower, "-refresh-", "-") = Replace(msuData.Ident.Name.ToLower, "-refresh-", "-") And itemData.Ident.Language.ToLower = msuData.Ident.Language.ToLower And CompareArchitectures(itemData.Architecture, msuData.Architecture, True) Then
          Dim always As Boolean = False
          Dim CompareRet As Integer = CompareMSVersions(itemData.Ident.Version, msuData.Ident.Version)
          If CompareRet = 0 Then
            Return New Updates_AddFile_Result("Update already added.")
          ElseIf CompareRet > 0 Then
            If Updates_ReplaceAllOld = TriState.True Then
              msuCollection.RemoveAt(I)
            ElseIf Updates_ReplaceAllOld = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllOld = TriState.True
                msuCollection.RemoveAt(I)
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllOld = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          ElseIf CompareRet < 0 Then
            If Updates_ReplaceAllNew = TriState.True Then
              msuCollection.RemoveAt(I)
            ElseIf Updates_ReplaceAllNew = TriState.False Then
              Return New Updates_AddFile_Result(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then Updates_ReplaceAllNew = TriState.True
                msuCollection.RemoveAt(I)
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then Updates_ReplaceAllNew = TriState.False
                Return New Updates_AddFile_Result(True)
              Else
                Return New Updates_AddFile_Result(False)
              End If
            End If
          End If
        End If
      Next
      If Not msuData.AllowedOffline Then Return New Updates_AddFile_Result("Update can't be integrated.")
      If msuData.KBArticle = "947821" Then Return New Updates_AddFile_Result("Update can't be integrated.")
      If msuData.KBArticle = "3035583" Then Return New Updates_AddFile_Result("Update can't be integrated.")
      Dim lvItem As New ListViewItem(msuData.Name)
      lvItem.Name = String.Format("lviUpdate{0}", AlphanumericVal(msuData.Identity))
      Dim lTag As Integer = 0
      Do
        lTag += 1
        If lTag >= Integer.MaxValue Then Exit Do
      Loop While Updates_ListData.ContainsKey(lTag)
      Updates_ListData.Add(lTag, New Updates_Data(msuData, UpdateAction))
      lvItem.Tag = lTag
      Dim bWhitelist As Boolean = CompareArchitectures(msuData.Architecture, ArchitectureList.x86, True) AndAlso CheckWhitelist(msuData.DisplayName)
      Dim ttItem As String = msuData.Name
      If Not String.IsNullOrEmpty(msuData.KBArticle) Then
        ttItem = String.Format("KB{0}", msuData.KBArticle)
      ElseIf Not msuData.Ident.IsEmpty Then
        ttItem = msuData.Ident.Name
      End If
      If Not String.IsNullOrEmpty(msuData.KBVersion) Then ttItem = String.Concat(ttItem, String.Format(" v{0}", msuData.KBVersion))
      ttItem = String.Concat(ttItem, vbNewLine, en, String.Format("For {0} {1}{2}", msuData.AppliesTo, msuData.Architecture, IIf(bWhitelist, " [Whitelisted for 64-Bit]", "")))
      If Not String.IsNullOrEmpty(msuData.BuildDate) Then ttItem = String.Concat(ttItem, vbNewLine, en, String.Format("Built on {0}", msuData.BuildDate))
      ttItem = String.Concat(ttItem, vbNewLine, en, String.Format("Stored at {0}", ShortenPath(msuData.Path)))
      lvItem.BackColor = SystemColors.Window
      If bWhitelist Then lvItem.BackColor = SystemColors.GradientInactiveCaption
      If lvMSU.ReadOnly Then
        Updates_BackgroundColors.Add(lvItem.Name, lvItem.BackColor)
        lvItem.BackColor = lvMSU.BackColor
      End If
      If msuData.KBArticle = "2647753" And msuData.Ident.Version = "6.1.2.0" Then
        lvItem.ForeColor = mySettings.UpdateColorSuperseded
        ttItem = String.Concat(ttItem, vbNewLine, en, String.Format("Version 2 may not integrate correctly. {0} suggests using Version 4 from the Microsoft Update Catalog.", Application.ProductName))
      End If
      If InOne Then
        If lvItem.ForeColor = lvMSU.ForeColor Then lvItem.ForeColor = mySettings.UpdateColorUpgrade
        If UpdateAction = Updates_Data.Update_Replace.OnlyOlder Then
          ttItem = String.Concat(ttItem, vbNewLine, en, "This update will replace integrated versions if they are older")
        ElseIf UpdateAction = Updates_Data.Update_Replace.OnlyNewer Then
          ttItem = String.Concat(ttItem, vbNewLine, en, "This update will replace integrated versions if they are newer")
        ElseIf UpdateAction = Updates_Data.Update_Replace.OnlyMissing Then
          ttItem = String.Concat(ttItem, vbNewLine, en, "This update will not replace any integrated versions")
        Else
          ttItem = String.Concat(ttItem, vbNewLine, en, "This update will replace all integrated versions")
        End If
      End If
      lvItem.ToolTipText = ttItem
      Select Case GetUpdateType(msuData.Path)
        Case UpdateType.MSU
          lvItem.ImageKey = "MSU"
          lvItem.SubItems.Add(String.Format("{0} MSU", msuData.Architecture))
          msuCollection.Add(lvItem)
          Return New Updates_AddFile_Result(True)
        Case UpdateType.CAB
          lvItem.ImageKey = "CAB"
          lvItem.SubItems.Add(String.Format("{0} CAB", msuData.Architecture))
          msuCollection.Add(lvItem)
          Return New Updates_AddFile_Result(True)
        Case UpdateType.LP
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(String.Format("{0} MUI", msuData.Architecture))
          msuCollection.Add(lvItem)
          Return New Updates_AddFile_Result(True)
        Case UpdateType.LIP, UpdateType.MSI
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(String.Format("{0} LIP", msuData.Architecture))
          msuCollection.Add(lvItem)
          Return New Updates_AddFile_Result(True)
        Case UpdateType.EXE
          If msuData.Name.Contains("Windows Update Agent") Then
            lvItem.ImageKey = "CAB"
            lvItem.SubItems.Add(String.Format("{0} CAB", msuData.Architecture))
            msuCollection.Add(lvItem)
          ElseIf msuData.DisplayName.Contains("Internet Explorer") Then
            lvItem.ImageKey = "CAB"
            lvItem.SubItems.Add(String.Format("{0} CAB", msuData.Architecture))
            msuCollection.Add(lvItem)
          Else
            lvItem.ImageKey = "MLC"
            lvItem.SubItems.Add(String.Format("{0} MUI", msuData.Architecture))
            msuCollection.Add(lvItem)
          End If
          Return New Updates_AddFile_Result(True)
      End Select
    End If
    If String.IsNullOrEmpty(IO.Path.GetExtension(msuData.Path)) Then
      Return New Updates_AddFile_Result("Unknown Update Type.")
    Else
      Return New Updates_AddFile_Result(String.Format("Unknown Update Type: ""{0}"".", IO.Path.GetExtension(msuData.Path).Substring(1).ToUpper))
    End If
  End Function
  Private Function Updates_FailureCleanup(FailCollection As List(Of String)) As String
    If FailCollection.Count > 10 Then
      Dim FailCollectionGroups As New List(Of List(Of String))
      For Each Failure In FailCollection
        Dim FailMsg As String = Failure.Substring(Failure.IndexOf(":") + 2)
        If FailCollectionGroups.Count = 0 Then
          Dim FirstGroup As New List(Of String)
          FirstGroup.Add(FailMsg)
          FirstGroup.Add(Failure)
          FailCollectionGroups.Add(FirstGroup)
        Else
          Dim Added As Boolean = False
          For Each FailGroup In FailCollectionGroups
            If FailGroup.Item(0) = FailMsg Then
              FailGroup.Add(Failure)
              Added = True
              Exit For
            End If
          Next
          If Not Added Then
            Dim FirstOf As New List(Of String)
            FirstOf.Add(FailMsg)
            FirstOf.Add(Failure)
            FailCollectionGroups.Add(FirstOf)
          End If
        End If
      Next
      Dim sFailList As String = Nothing
      For Each failGroup In FailCollectionGroups
        If failGroup.Count > 2 Then
          sFailList = String.Concat(sFailList, String.Format("{0} ({1} updates)", failGroup.Item(0), (failGroup.Count - 1).ToString), vbNewLine)
        Else
          sFailList = String.Concat(sFailList, failGroup.Item(1), vbNewLine)
        End If
      Next
      If Not String.IsNullOrEmpty(sFailList) AndAlso sFailList.Length > 1 AndAlso sFailList.EndsWith(vbNewLine) Then sFailList = sFailList.Substring(0, sFailList.Length - 2)
      Return sFailList
    Else
      Return Join(FailCollection.ToArray, vbNewLine)
    End If
  End Function
  Private Class Updates_AddFile_Result
    Public Success As Boolean
    Public FailReason As String
    Public Cancel As Boolean
    Public Sub New(Failure As String)
      Success = False
      If String.IsNullOrEmpty(Failure) Then
        Cancel = True
      Else
        FailReason = Failure
      End If
    End Sub
    Public Sub New(NoFail As Boolean)
      Success = NoFail
      If Not Success Then Cancel = True
    End Sub
  End Class
  Private Structure Updates_Prerequisite
    Public KBWithRequirement As String
    Public Requirement()() As String
    Public Sub New(Input As String)
      If Not Input.Contains(":") Then Return
      Dim splitInput() As String = Split(Input, ":", 2)
      KBWithRequirement = splitInput(0)
      If splitInput(1).Contains("/") Then
        Dim splitRequirement() As String = Split(splitInput(1), "/", 2)
        Dim reqCount As Integer = 0
        For Each reqList In splitRequirement
          If String.IsNullOrEmpty(reqList) Then Continue For
          ReDim Preserve Requirement(reqCount)
          If Not reqList.Contains(",") Then
            Requirement(reqCount) = {reqList}
          Else
            Requirement(reqCount) = Split(reqList, ",")
          End If
          reqCount += 1
        Next
      Else
        ReDim Requirement(0)
        If Not splitInput(1).Contains(",") Then
          Requirement(0) = {splitInput(1)}
        Else
          Requirement(0) = Split(splitInput(1), ",")
        End If
      End If
    End Sub
  End Structure
  Private Structure Updates_Data
    Public Update As Update_File
    Public ReplaceStyle As Update_Replace
    Public Enum Update_Replace
      All
      OnlyNewer
      OnlyOlder
      OnlyMissing
    End Enum
    Public Sub New(updateFile As Update_File, updateReplace As Update_Replace)
      Update = updateFile
      ReplaceStyle = updateReplace
    End Sub
  End Structure
#End Region
#Region "ISO"
  Private ISO_UnlockEditionsCheckbox As TriState = TriState.UseDefault
  Private ISO_UEFICheckbox As TriState = TriState.UseDefault
  Private Sub chkISO_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkISO.CheckedChanged
    RunComplete = False
    cmdBegin.Text = "&Begin"
    cmdOpenFolder.Visible = False
    txtISO.Enabled = chkISO.Checked
    cmdISO.Enabled = chkISO.Checked
    chkUEFI.Enabled = chkISO.Checked And ISO_UEFICheckbox = TriState.UseDefault
    lblISOFeatures.Enabled = chkISO.Checked
    chkUnlock.Enabled = chkISO.Checked And ISO_UnlockEditionsCheckbox = TriState.UseDefault
    lblISOLabel.Enabled = chkISO.Checked
    chkAutoLabel.Enabled = chkISO.Checked
    txtISOLabel.Enabled = chkISO.Checked
    lblISOFS.Enabled = chkISO.Checked
    cmbISOFormat.Enabled = chkISO.Checked
    If chkISO.Checked Then
      If Not cmbLimitType.Items.Contains("Split ISO") Then
        cmbLimitType.Items.Add("Split ISO")
        'If cmbLimitType.SelectedIndex = 1 Then cmbLimitType.SelectedIndex = 2
      End If
    Else
      If cmbLimitType.Items.Contains("Split ISO") Then
        If cmbLimitType.SelectedIndex = 2 Then cmbLimitType.SelectedIndex = 1
        cmbLimitType.Items.Remove("Split ISO")
      End If
    End If
  End Sub
  Private Sub txtISO_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragDrop
    TextBoxDragDropEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtISO_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtISO_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".iso"})
  End Sub
  Private Sub txtISO_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISO.TextChanged
    StopRun = False
    RunComplete = False
    cmdBegin.Text = "&Begin"
    cmdOpenFolder.Visible = False
    Dim foundEI As Boolean = False
    Dim foundUEFI As Boolean = False
    If String.IsNullOrEmpty(txtISO.Text) OrElse Not IO.File.Exists(txtISO.Text) Then
      ISO_UnlockEditionsCheckbox = TriState.UseDefault
      ISO_UEFICheckbox = TriState.UseDefault
      UEFI_IsEnabled = False
      Return
    End If
    Dim sFiles() As String = Extract_FileList(txtISO.Text)
    If sFiles IsNot Nothing Then
      For Each sFile In sFiles
        If sFile.ToLower.Contains("ei.cfg") Then foundEI = True
        If sFile.ToLower = "efi\boot\bootx64.efi" Then foundUEFI = True
        If foundEI And foundUEFI Then Exit For
      Next
    End If
    If foundEI Then
      If Not ISO_UnlockEditionsCheckbox = TriState.UseDefault Then
        If ISO_UnlockEditionsCheckbox = TriState.True Then
          chkUnlock.Checked = True
        ElseIf ISO_UnlockEditionsCheckbox = TriState.False Then
          chkUnlock.Checked = False
        End If
        ISO_UnlockEditionsCheckbox = TriState.UseDefault
      End If
      chkUnlock.Enabled = True
    Else
      If ISO_UnlockEditionsCheckbox = TriState.UseDefault Then
        If chkUnlock.Checked Then
          ISO_UnlockEditionsCheckbox = TriState.True
        Else
          ISO_UnlockEditionsCheckbox = TriState.False
        End If
      End If
      chkUnlock.Checked = True
      chkUnlock.Enabled = False
    End If
    If foundUEFI Then
      UEFI_IsEnabled = True
      If ISO_UEFICheckbox = TriState.UseDefault Then
        If chkUEFI.Checked Then
          ISO_UEFICheckbox = TriState.True
        Else
          ISO_UEFICheckbox = TriState.False
        End If
      End If
      chkUEFI.Checked = True
      chkUEFI.Enabled = False
    Else
      UEFI_IsEnabled = False
      If Not ISO_UEFICheckbox = TriState.UseDefault Then
        If ISO_UEFICheckbox = TriState.True Then
          chkUEFI.Checked = True
        ElseIf ISO_UEFICheckbox = TriState.False Then
          chkUEFI.Checked = False
        End If
        ISO_UEFICheckbox = TriState.UseDefault
      End If
      chkUEFI.Enabled = True
    End If
    If chkAutoLabel.Checked Then
      Dim sComment As String = Extract_Comment(txtISO.Text)
      If Not String.IsNullOrEmpty(sComment) Then
        If sComment.Contains(vbLf) Then
          Dim sParts() As String = Split(sComment, vbLf)
          For Each part In sParts
            If Not String.IsNullOrEmpty(part) Then
              If part.StartsWith("Volume: ") Then
                sComment = part.Substring(part.IndexOf(": ") + 2)
                Exit For
              End If
            End If
          Next
        End If
        txtISOLabel.Text = sComment
      End If
    End If
    Status_SetText("Idle")
  End Sub
  Private Sub chkAutoLabel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutoLabel.CheckedChanged
    If chkAutoLabel.Checked And (Not String.IsNullOrEmpty(txtISO.Text) AndAlso IO.File.Exists(txtISO.Text)) Then
      Dim sComment As String = Extract_Comment(txtISO.Text)
      If Not String.IsNullOrEmpty(sComment) Then
        If sComment.Contains(vbLf) Then
          Dim sParts() As String = Split(sComment, vbLf)
          For Each part In sParts
            If Not String.IsNullOrEmpty(part) Then
              If part.StartsWith("Volume: ") Then
                sComment = part.Substring(part.IndexOf(": ") + 2)
                Exit For
              End If
            End If
          Next
        End If
        txtISOLabel.Text = sComment
      End If
      Status_SetText("Idle")
    End If
    mySettings.AutoISOLabel = chkAutoLabel.Checked
  End Sub
  Private Sub txtISOLabel_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragDrop
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If IsArray(Data) Then
        Dim aData() As String = CType(Data, String())
        If aData.Length = 1 Then
          Dim sPath As String = aData(0)
          Dim sComment As String = Extract_Comment(sPath)
          If Not String.IsNullOrEmpty(sComment) Then txtISOLabel.Text = sComment
        End If
      End If
    End If
  End Sub
  Private Sub txtISOLabel_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtISOLabel_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".iso"})
  End Sub
  Private Sub txtISOLabel_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISOLabel.TextChanged
    If Not isStarting Then
      If Not String.IsNullOrEmpty(txtISOLabel.Text) Then
        mySettings.DefaultISOLabel = txtISOLabel.Text
      End If
    End If
  End Sub
#Region "ISO Label Context Menu"
  Private Enum ISOLabel_ReleaseType
    Starter
    HomeBasic
    HomePremium
    Professional
    Ultimate
    Enterprise
    Multiple
    All
  End Enum
  Private Enum ISOLabel_Architecture
    x86
    x64
    Universal
  End Enum
  Private Enum ISOLabel_BuildType
    Release
    Debug
    Volume
  End Enum
  Private Enum ISOLabel_PurposeType
    Retail
    OEM
  End Enum
#Region "x86"
#Region "RTM"
#Region "Starter"
  Private Sub mnuLabelGRMCSTFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCSTVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Basic"
  Private Sub mnuLabelGRMCHBFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHBVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGRMCHPFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHPVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGRMCPRFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCPRVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGRMCULFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCULVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGRMCENVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCENCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGRMCMUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCALFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCALVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#Region "SP1"
#Region "Starter"
  Private Sub mnuLabelGSP1RMCSTFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCSTFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCSTFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCSTFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCSTVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCSTVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCSTCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCSTCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Starter, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Basic"
  Private Sub mnuLabelGSP1RMCHBFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHBFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCHBVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHBCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGSP1RMCHPFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHPFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCHPVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHPCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGSP1RMCPRFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCPRFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCPRVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCPRCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGSP1RMCULFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCULFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCULVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCULCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGSP1RMCENVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCENVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCENCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCENCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGSP1RMCMUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCMUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGSP1RMCALFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCALFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCALVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCALCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x86, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#End Region
#Region "x64"
#Region "RTM"
#Region "Home Basic"
  Private Sub mnuLabelGRMCHBXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHBXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGRMCHPXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHPXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGRMCPRXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCPRXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGRMCULXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCULXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGRMCENXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCENXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGRMCMUXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCALXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCALXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#Region "SP1"
#Region "Home Basic"
  Private Sub mnuLabelGSP1RMCHBXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHBXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCHBXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHBXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHBXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomeBasic, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGSP1RMCHPXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHPXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCHPXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCHPXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCHPXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.HomePremium, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGSP1RMCPRXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCPRXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCPRXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCPRXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCPRXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Professional, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGSP1RMCULXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCULXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCULXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCULXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCULXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Ultimate, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGSP1RMCENXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCENXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCENXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCENXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Enterprise, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGSP1RMCMUXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCMUXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGSP1RMCALXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALXFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCALXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALXFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCALXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALXVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCALXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALXCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.x64, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#End Region
#Region "AIO"
#Region "RTM"
#Region "Multiple"
  Private Sub mnuLabelGRMCMUUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCSTAFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTAFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCSTAVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTACHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(0, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#Region "SP1"
#Region "Multiple"
  Private Sub mnuLabelGSP1RMCMUUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCMUUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCMUUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCMUUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.Multiple, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGSP1RMCSTAFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALUFRER.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCSTAFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALUFREO.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Release, ISOLabel_PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGSP1RMCSTAVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALUVOL.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Volume, ISOLabel_PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGSP1RMCSTACHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGSP1RMCALUCHE.Click
    txtISOLabel.Text = ISOLabel_MakeName(1, ISOLabel_ReleaseType.All, ISOLabel_Architecture.Universal, ISOLabel_BuildType.Debug, ISOLabel_PurposeType.Retail)
  End Sub
#End Region
#End Region
#End Region
  Private Sub mnuLabelAuto_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelAuto.Click
    Dim rSP As Integer = 0
    Dim rType As ISOLabel_ReleaseType = ISOLabel_ReleaseType.All
    Dim rArc As ISOLabel_Architecture = ISOLabel_Architecture.Universal
    Dim rBui As ISOLabel_BuildType = ISOLabel_BuildType.Release
    Dim rPur As ISOLabel_PurposeType = ISOLabel_PurposeType.Retail
    Dim has86, has64 As Boolean : has86 = False : has64 = False
    Dim hasST, hasHB, hasHP, hasPR, hasUL, hasEN As Integer
    Dim stCount, hbCount, hpCount, prCount, ulCount, enCount As Integer
    If chkSP.Checked AndAlso Not String.IsNullOrEmpty(txtSP.Text) Then rSP = 1
    If lvImages.Items.Count > 0 Then
      For Each Image As ListViewItem In lvImages.Items
        Dim lvIndex As Integer = CInt(Image.Tag)
        If Image.Checked Then
          Dim imInfo As ImagePackage = ImagePackage_ListData(lvIndex).Package
          If imInfo.SPLevel > rSP Then rSP = imInfo.SPLevel
          If imInfo.SPBuild >= 23403 And rSP = 1 Then rSP = 2
          Select Case imInfo.Edition
            Case "Starter" : stCount += 1
            Case "HomeBasic" : hbCount += 1
            Case "HomePremium" : hpCount += 1
            Case "Professional" : prCount += 1
            Case "Ultimate" : ulCount += 1
            Case "Enterprise" : enCount += 1
          End Select
          If CompareArchitectures(imInfo.Architecture, ArchitectureList.x86, False) Then has86 = True
          If CompareArchitectures(imInfo.Architecture, ArchitectureList.amd64, False) Then has64 = True
        End If
      Next
    End If
    If lvMSU.Items.Count > 0 Then
      For Each Update As ListViewItem In lvMSU.Items
        Dim lvIndex As Integer = CInt(Update.Tag)
        Dim updateInfo As Update_File = Updates_ListData(lvIndex).Update
        If updateInfo.KBArticle = "3125574" And rSP = 1 Then rSP = 2
      Next
    End If
    If stCount > 0 Then hasST = 1
    If hbCount > 0 Then hasHB = 1
    If hpCount > 0 Then hasHP = 1
    If prCount > 0 Then hasPR = 1
    If ulCount > 0 Then hasUL = 1
    If enCount > 0 Then hasEN = 1
    If has86 And has64 Then
      rArc = ISOLabel_Architecture.Universal
      If stCount >= 1 And hbCount >= 2 And hpCount >= 2 And prCount >= 2 And ulCount >= 2 Then
        rType = ISOLabel_ReleaseType.All
      ElseIf (hasST + hasHB + hasHP + hasPR + hasUL + hasEN) > 1 Then
        rType = ISOLabel_ReleaseType.Multiple
      ElseIf stCount > 0 Then
        rType = ISOLabel_ReleaseType.Starter
      ElseIf hbCount > 0 Then
        rType = ISOLabel_ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ISOLabel_ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ISOLabel_ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ISOLabel_ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ISOLabel_ReleaseType.Enterprise
        rBui = ISOLabel_BuildType.Volume
      End If
    ElseIf has64 Then
      rArc = ISOLabel_Architecture.x64
      If hbCount >= 1 And hpCount >= 1 And prCount >= 1 And ulCount >= 1 Then
        rType = ISOLabel_ReleaseType.All
      ElseIf (hasHB + hasHP + hasPR + hasUL + hasEN) > 1 Then
        rType = ISOLabel_ReleaseType.Multiple
      ElseIf hbCount > 0 Then
        rType = ISOLabel_ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ISOLabel_ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ISOLabel_ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ISOLabel_ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ISOLabel_ReleaseType.Enterprise
        rBui = ISOLabel_BuildType.Volume
      End If
    ElseIf has86 Then
      rArc = ISOLabel_Architecture.x86
      If stCount >= 1 And hbCount >= 1 And hpCount >= 1 And prCount >= 1 And ulCount >= 1 Then
        rType = ISOLabel_ReleaseType.All
      ElseIf (hasST + hasHB + hasHP + hasPR + hasUL + hasEN) > 1 Then
        rType = ISOLabel_ReleaseType.Multiple
      ElseIf stCount > 0 Then
        rType = ISOLabel_ReleaseType.Starter
      ElseIf hbCount > 0 Then
        rType = ISOLabel_ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ISOLabel_ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ISOLabel_ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ISOLabel_ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ISOLabel_ReleaseType.Enterprise
        rBui = ISOLabel_BuildType.Volume
      End If
    End If
    txtISOLabel.Text = ISOLabel_MakeName(rSP, rType, rArc, rBui, rPur)
  End Sub
  Private Sub ISOLabel_LoadValues(ByRef langVal As String, ByRef medVal As String)
    Dim langList As New List(Of String)
    If lvImages.Items.Count > 0 Then
      For Each item As ListViewItem In lvImages.Items
        Dim lvIndex As Integer = CInt(item.Tag)
        Dim imageInfo As ImagePackage = ImagePackage_ListData(lvIndex).Package
        For Each language As String In imageInfo.LangList
          Dim sLang As String = language.Substring(0, language.IndexOf("-")).ToUpper
          If Not langList.Contains(sLang) Then langList.Add(sLang)
        Next
      Next
    End If
    If lvMSU.Items.Count > 0 Then
      For Each item As ListViewItem In lvMSU.Items
        Dim lvIndex As Integer = CInt(item.Tag)
        Dim updateInfo As Update_File = Updates_ListData(lvIndex).Update
        If updateInfo.Name.Contains("Interface Pack") Then
          If updateInfo.Name.Contains("-") Then
            Dim sLang As String = updateInfo.Name.Substring(0, updateInfo.Name.IndexOf("-")).ToUpper
            If Not langList.Contains(sLang) Then langList.Add(sLang)
          Else
            Dim sLang As String = updateInfo.Name.Substring(0, updateInfo.Name.IndexOf(" ")).ToUpper
            If Not langList.Contains(sLang) Then langList.Add(sLang)
          End If
        End If
      Next
    End If
    If langList.Count = 0 Then
      langVal = "EN"
    ElseIf langList.Count = 1 Then
      If langList(0).Contains("-") Then
        langVal = langList(0).Substring(0, langList(0).IndexOf("-")).ToUpper
      Else
        langVal = langList(0).ToUpper
      End If
    ElseIf langList.Count >= 90 Then
      langVal = "UNI"
    ElseIf langList.Count > 1 Then
      langVal = "MUL"
    End If
    If cmbLimitType.SelectedIndex = 0 Then
      medVal = "DVD"
    ElseIf cmbLimitType.SelectedIndex = 1 Then
      medVal = "SWM_DVD"
    Else
      Dim limVal As String = cmbLimit.Text
      If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
      Dim splFileSize As Long = NumericVal(limVal)
      If splFileSize <= 700 Then
        medVal = "CD"
      ElseIf splFileSize <= 4700 Then
        medVal = "DV5"
      ElseIf splFileSize <= 8500 Then
        medVal = "DV9"
      ElseIf splFileSize <= 25000 Then
        medVal = "BD"
      ElseIf splFileSize <= 50000 Then
        medVal = "BD_DL"
      Else
        medVal = "USB"
      End If
    End If
    If Not String.IsNullOrEmpty(langVal) Then langVal = String.Concat("_", langVal)
    If Not String.IsNullOrEmpty(medVal) Then medVal = String.Concat("_", medVal)
  End Sub
  Private Function ISOLabel_MakeName(ServicePack As Integer, Release As ISOLabel_ReleaseType, Arch As ISOLabel_Architecture, Build As ISOLabel_BuildType, Purpose As ISOLabel_PurposeType) As String
    Dim sp As String = ""
    Dim rel As String = "UL"
    Dim arc As String = ""
    Dim bui As String = "FRE"
    Dim pur As String = "R"
    Dim langVal As String = "EN"
    Dim medVal As String = "DVD"
    ISOLabel_LoadValues(langVal, medVal)
    If ServicePack > 0 Then sp = "SP" & ServicePack
    Select Case Release
      Case ISOLabel_ReleaseType.Starter : rel = "ST"
      Case ISOLabel_ReleaseType.HomeBasic : rel = "HB"
      Case ISOLabel_ReleaseType.HomePremium : rel = "HP"
      Case ISOLabel_ReleaseType.Professional : rel = "PR"
      Case ISOLabel_ReleaseType.Ultimate : rel = "UL"
      Case ISOLabel_ReleaseType.Enterprise : rel = "EN"
      Case ISOLabel_ReleaseType.Multiple : rel = "MU"
      Case ISOLabel_ReleaseType.All : rel = "AL"
    End Select
    Select Case Arch
      Case ISOLabel_Architecture.x86 : arc = ""
      Case ISOLabel_Architecture.x64 : arc = "X"
      Case ISOLabel_Architecture.Universal : arc = "U"
    End Select
    Select Case Build
      Case ISOLabel_BuildType.Release : bui = "FRE"
      Case ISOLabel_BuildType.Debug : bui = "CHE"
      Case ISOLabel_BuildType.Volume : bui = "VOL"
    End Select
    If Build = ISOLabel_BuildType.Debug Then
      pur = ""
    Else
      If Build = ISOLabel_BuildType.Volume Then
        pur = ""
      Else
        Select Case Purpose
          Case ISOLabel_PurposeType.Retail : pur = "R"
          Case ISOLabel_PurposeType.OEM : pur = "O"
        End Select
      End If
    End If
    Return String.Concat("G", sp, "RMC", rel, arc, bui, pur, langVal, medVal)
  End Function
#End Region
  Private Sub cmdISO_Click(sender As System.Object, e As System.EventArgs) Handles cmdISO.Click
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AllowNonFileSystemItems = False
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000006")
      If Not String.IsNullOrEmpty(txtWIM.Text) AndAlso IO.File.Exists(txtWIM.Text) Then
        cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtWIM.Text)
      Else
        cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      End If
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Windows 7 ISO", "ISO"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = False
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Choose ISO to Save Image To..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.6_Save_to_ISO.htm")
      If Not String.IsNullOrEmpty(txtISO.Text) Then cdlBrowse.InitialDirectory = txtISO.Text
      If cdlBrowse.ShowDialog(Me.Handle) = CommonFileDialogResult.Ok Then txtISO.Text = cdlBrowse.FileName
    End Using
  End Sub
  Private Sub cmbISOFormat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbISOFormat.SelectedIndexChanged
    If Not isStarting Then
      mySettings.DefaultFS = cmbISOFormat.Text
    End If
  End Sub
#End Region
#Region "Merge"
  Private thdMergeInput As Threading.Thread
  Private Merge_LastValue As String
  Private Sub chkMerge_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMerge.CheckedChanged
    txtMerge.Enabled = chkMerge.Checked
    cmdMerge.Enabled = chkMerge.Checked
    If chkMerge.Checked Then
      txtMerge_TextChanged(txtMerge, New EventArgs)
    Else
      ImagePackage_ClearList(WIMGroup.Merge)
    End If
    ImagePackage_SortList()
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.Images)
  End Sub
  Private Sub txtMerge_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragDrop
    TextBoxDragDropEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtMerge_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragEnter
    TextBoxDragEnterEvent(CType(sender, TextBox), e)
  End Sub
  Private Sub txtMerge_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragOver
    TextBoxDragOverEvent(CType(sender, TextBox), e, {".wim", ".iso"})
  End Sub
  Private Sub txtMerge_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMerge.TextChanged
    If txtMerge.Text = Merge_LastValue Then Return
    Merge_LastValue = txtMerge.Text
    If String.IsNullOrEmpty(txtMerge.Text) OrElse Not IO.File.Exists(txtMerge.Text) Then
      ImagePackage_ClearList(WIMGroup.Merge)
      Return
    End If
    RunComplete = False
    StopRun = False
    RunActivity = ActivityType.LoadingPackageData
    cmdBegin.Text = "&Begin"
    cmdLoadPackages.Image = My.Resources.u_n
    cmdOpenFolder.Visible = False
    If thdMergeInput Is Nothing Then
      thdMergeInput = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_ParseList))
      thdMergeInput.Start(WIMGroup.Merge)
    End If
  End Sub
  Private Sub cmdMerge_Click(sender As System.Object, e As System.EventArgs) Handles cmdMerge.Click
    Using cdlBrowse As New CommonOpenFileDialog
      cdlBrowse.AllowNonFileSystemItems = False
      cdlBrowse.AddToMostRecentlyUsedList = True
      cdlBrowse.CookieIdentifier = New Guid("00000000-0000-0000-0000-000000000002")
      If Not String.IsNullOrEmpty(txtWIM.Text) AndAlso IO.File.Exists(txtWIM.Text) Then
        cdlBrowse.DefaultDirectory = IO.Path.GetDirectoryName(txtWIM.Text)
      Else
        cdlBrowse.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      End If
      cdlBrowse.EnsureFileExists = True
      cdlBrowse.EnsurePathExists = True
      cdlBrowse.EnsureReadOnly = True
      cdlBrowse.EnsureValidNames = True
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("INSTALL.WIM Sources", "WIM,ISO"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("INSTALL.WIM", "WIM"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("Windows 7 ISO", "ISO"))
      cdlBrowse.Filters.Add(New CommonFileDialogFilter("All Files", "*"))
      cdlBrowse.Multiselect = False
      cdlBrowse.NavigateToShortcut = True
      cdlBrowse.RestoreDirectory = False
      cdlBrowse.ShowPlacesList = True
      cdlBrowse.Title = "Choose INSTALL.WIM Image to Merge..."
      Dim cmdHelp As New Controls.CommonFileDialogButton("cmdHelp", "&Help")
      cdlBrowse.Controls.Add(cmdHelp)
      AddHandler cmdHelp.Click, Sub(sender2 As Object, e2 As EventArgs) Help.ShowHelp(Nothing, "S7M.chm", HelpNavigator.Topic, "1_SLIPS7REAM_Interface\1.2_Merge_WIM.htm")
      If Not String.IsNullOrEmpty(txtMerge.Text) Then cdlBrowse.InitialDirectory = txtMerge.Text
      If cdlBrowse.ShowDialog(Me.Handle) = CommonFileDialogResult.Ok Then txtMerge.Text = cdlBrowse.FileName
    End Using
  End Sub
#End Region
#Region "Image Packages"
  Friend Shared ImagePackage_ListData As New SortedList(Of Integer, ImagePackage_PackageData)
  Private thdPackageListUpdate As Threading.Thread
  Private ImagePackage_SortColumn As Integer
  Private ImagePackage_SortOrder As SortOrder
  Private ImagePackage_ListSelection As ListViewItem
  Private ImagePackageList_LastLeft As Long
  Private Delegate Sub ImagePackage_AddToListInvoker(lvItem() As ListViewItem)
  Private Delegate Sub ImagePackage_ListInvoker(PassedWIMGroup As WIMGroup)
  Private Delegate Sub ImagePackage_ParseInvoker(Param As Object)
  Private Sub lvImages_ColumnClick(sender As Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lvImages.ColumnClick
    If e.Column = ImagePackage_SortColumn Then
      If ImagePackage_SortOrder = SortOrder.Ascending Then
        ImagePackage_SortOrder = SortOrder.Descending
      Else
        ImagePackage_SortOrder = SortOrder.Ascending
      End If
    Else
      ImagePackage_SortColumn = e.Column
      ImagePackage_SortOrder = SortOrder.Descending
    End If
    ImagePackage_SortList()
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.Images)
  End Sub
  Private Sub lvImages_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvImages.ColumnWidthChanged
    Static busy As Boolean
    If busy Then Return
    busy = True
    lvImages.BeginUpdate()
    If e.ColumnIndex = 0 Then
      lvImages.Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvImages.Columns(0).Width < 50 Then lvImages.Columns(0).Width = 50
    End If
    If e.ColumnIndex = 1 Then
      Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width + lvImages.Columns(3).Width) - 2
      If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
    End If
    If e.ColumnIndex = 2 Then
      lvImages.Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvImages.Columns(2).Width < 43 Then lvImages.Columns(2).Width = 43
    End If
    If e.ColumnIndex = 3 Then
      lvImages.Columns(3).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvImages.Columns(3).Width < 60 Then lvImages.Columns(3).Width = 60
    End If
    lvImages.EndUpdate()
    busy = False
  End Sub
  Private Sub lvImages_ColumnWidthChanging(sender As Object, e As System.Windows.Forms.ColumnWidthChangingEventArgs) Handles lvImages.ColumnWidthChanging
    e.Cancel = True
  End Sub
  Private Sub lvImages_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvImages.KeyUp
    If lvImages.SelectedItems IsNot Nothing AndAlso lvImages.SelectedItems.Count > 0 Then
      If e.KeyCode = Keys.F2 Then
        ImagePackage_Rename(CType(lvImages.SelectedItems(0), ListViewItem))
      End If
    End If
  End Sub
  Private Sub lvImages_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvImages.MouseDoubleClick
    If lvImages.SelectedItems IsNot Nothing AndAlso lvImages.SelectedItems.Count > 0 Then
      If lvImages.SelectedItems(0).SubItems(1).Bounds.Contains(e.Location) AndAlso (TickCount() - ImagePackageList_LastLeft) > 100 Then
        ImagePackage_Rename(CType(lvImages.SelectedItems(0), ListViewItem))
      Else
        ImagePackage_ShowProperties()
      End If
    End If
  End Sub
  Private Sub lvImages_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvImages.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      Dim selItem As ListViewItem = lvImages.GetItemAt(e.X, e.Y)
      If selItem IsNot Nothing Then
        ImagePackage_ListSelection = selItem
        mnuPackageInclude.Checked = selItem.Checked
        mnuImages.Show(lvImages, e.Location)
      End If
    ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
      ImagePackageList_LastLeft = TickCount()
    End If
  End Sub
  Private Sub ImagePackage_ShowProperties()
    Dim selIndex As Integer = CInt(lvImages.SelectedItems(0).Tag)
    Dim propData As ImagePackage_PackageData = ImagePackage_ListData(selIndex)
    Dim packageName As String = String.Format("frmPackage{0}Props", propData.Package.ToString)
    For Each Form In OwnedForms
      If Form.Name = packageName Then
        Form.Focus()
        Return
      End If
    Next
    Dim props As New frmPackageProps(propData.Group, propData.Package, propData.FeatureList, propData.DriverList)
    props.Name = packageName
    props.txtName.Text = propData.NewName
    props.txtDesc.Text = propData.NewDesc
    AddHandler props.Response, AddressOf ImagePackages_ShowPropertiesResponse
    props.Show(Me)
  End Sub
  Private Sub ImagePackages_ShowPropertiesResponse(sender As Object, e As frmPackageProps.PackagePropertiesEventArgs)
    For Each lvItem As ListViewItem In lvImages.Items
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      If ImagePackage_ListData(lvIndex).Package.ToString = e.ImageID Then
        Dim imageDataItem As ImagePackage_PackageData = ImagePackage_ListData(lvIndex)
        If Not String.IsNullOrEmpty(e.NewImageName) Then
          lvItem.SubItems(1).Text = e.NewImageName
          Dim oldName As String = imageDataItem.NewName
          imageDataItem.NewName = e.NewImageName
          Dim ttItem As String = lvItem.ToolTipText
          If ttItem.StartsWith(String.Concat(oldName, vbNewLine)) Then
            ttItem = String.Concat(e.NewImageName, vbNewLine, ttItem.Substring(String.Concat(oldName, vbNewLine).Length))
          End If
          If Not lvItem.ToolTipText = ttItem Then lvItem.ToolTipText = ttItem
        End If
        If Not String.IsNullOrEmpty(e.NewImageDesc) Then
          Dim oldDesc As String = imageDataItem.NewDesc
          imageDataItem.NewDesc = e.NewImageDesc
          Dim ttItem As String = lvItem.ToolTipText
          If ttItem.Contains(String.Concat(vbNewLine, en, oldDesc, vbNewLine)) Then
            ttItem = ttItem.Replace(String.Concat(vbNewLine, en, oldDesc, vbNewLine), String.Concat(vbNewLine, en, e.NewImageDesc, vbNewLine))
          End If
          If Not lvItem.ToolTipText = ttItem Then lvItem.ToolTipText = ttItem
        End If
        If e.UpdateList IsNot Nothing AndAlso e.UpdateList.Length > 0 Then
          For Each item In e.UpdateList
            For I As Integer = 0 To imageDataItem.Package.IntegratedUpdateList.Count - 1
              If imageDataItem.Package.IntegratedUpdateList(I).Identity = item.Identity Then
                Dim newUpdate As Update_Integrated = imageDataItem.Package.IntegratedUpdateList(I)
                newUpdate.Remove = item.Remove
                imageDataItem.Package.IntegratedUpdateList(I) = newUpdate
                Exit For
              End If
            Next
          Next
        End If
        If e.FeatureList IsNot Nothing AndAlso e.FeatureList.Length > 0 Then
          For Each item In e.FeatureList
            For I As Integer = 0 To imageDataItem.FeatureList.Count - 1
              If imageDataItem.FeatureList(I).FeatureName = item.FeatureName Then
                Dim newFeature As Feature = imageDataItem.FeatureList(I)
                newFeature.Enable = item.Enable
                imageDataItem.FeatureList(I) = newFeature
                Exit For
              End If
            Next
          Next
        End If
        If e.DriverList IsNot Nothing AndAlso e.DriverList.Length > 0 Then
          For Each item In e.DriverList
            For I As Integer = 0 To imageDataItem.DriverList.Count - 1
              If imageDataItem.DriverList(I).PublishedName = item.PublishedName Then
                Dim newDriver As Driver = imageDataItem.DriverList(I)
                newDriver.Remove = item.Remove
                imageDataItem.DriverList(I) = newDriver
                Exit For
              End If
            Next
          Next
        End If
        ImagePackage_ListData(lvIndex) = imageDataItem
        Exit For
      End If
    Next
    Updates_Requirements()
  End Sub
  Private Sub ImagePackage_AddToList(lvItems() As ListViewItem)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_AddToListInvoker(AddressOf ImagePackage_AddToList), lvItems)
      Return
    End If
    lvImages.Items.AddRange(lvItems)
    ImagePackage_SortList()
    chkSP_CheckedChanged(chkSP, New EventArgs)
  End Sub
  Private Sub ImagePackage_SortList()
    Dim sortOrder As ImagePackage_Sorter.OrderBy = ImagePackage_Sorter.OrderBy.Display
    If ImagePackage_SortColumn = 1 Then
      sortOrder = ImagePackage_Sorter.OrderBy.OS
    ElseIf ImagePackage_SortColumn = 2 Then
      sortOrder = ImagePackage_Sorter.OrderBy.Architecture
    ElseIf ImagePackage_SortColumn = 3 Then
      sortOrder = ImagePackage_Sorter.OrderBy.Size
    End If
    If chkMerge.Checked And sortOrder = ImagePackage_Sorter.OrderBy.Display Then
      lvImages.ShowGroups = True
      lvImages.Groups.Clear()
      If ImagePackage_SortOrder = Windows.Forms.SortOrder.Ascending Then
        lvImages.Groups.Add(WIMGroup.Merge.ToString.ToUpper, "Merge Image")
        lvImages.Groups.Add(WIMGroup.WIM.ToString.ToUpper, "Source Image")
      Else
        lvImages.Groups.Add(WIMGroup.WIM.ToString.ToUpper, "Source Image")
        lvImages.Groups.Add(WIMGroup.Merge.ToString.ToUpper, "Merge Image")
      End If
      For Each lvItem As ListViewItem In lvImages.Items
        Dim lvIndex As Integer = CInt(lvItem.Tag)
        lvItem.Group = lvImages.Groups(ImagePackage_ListData(lvIndex).Group.ToString.ToUpper)
        lvItem.BackColor = lvImages.BackColor
      Next
    Else
      If lvImages.Groups.Count > 0 Then
        lvImages.ShowGroups = True
        lvImages.Groups.Clear()
        For Each lvItem As ListViewItem In lvImages.Items
          lvItem.Group = Nothing
          lvItem.BackColor = lvImages.BackColor
        Next
        lvImages.ShowGroups = False
      End If
    End If
    lvImages.ListViewItemSorter = New ImagePackage_Sorter(sortOrder, ImagePackage_SortOrder)
    lvImages.Sort()
  End Sub
  Private Sub ImagePackage_ClearList(ToClear As WIMGroup)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ListInvoker(AddressOf ImagePackage_ClearList), ToClear)
      Return
    End If
    If ToClear = WIMGroup.All Then
      ImagePackage_ListData.Clear()
      lvImages.Items.Clear()
    Else
      For Each lvItem As ListViewItem In lvImages.Items
        Dim lvIndex As Integer = CInt(lvItem.Tag)
        If ImagePackage_ListData(lvIndex).Group = ToClear Then
          ImagePackage_ListData.Remove(lvIndex)
          lvItem.Remove()
        End If
      Next
    End If
    GUI_ListView_ResizeColumns(GUI_ListView_Columns.Images)
  End Sub
  Private Sub ImagePackage_ClearInputThreads(ToClear As WIMGroup)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ListInvoker(AddressOf ImagePackage_ClearInputThreads), ToClear)
      Return
    End If
    If ToClear = WIMGroup.WIM Then
      thdWIMInput = Nothing
    ElseIf ToClear = WIMGroup.Merge Then
      thdMergeInput = Nothing
    Else
      thdWIMInput = Nothing
      thdMergeInput = Nothing
    End If
  End Sub
  Private Sub ImagePackage_ParseList(ToRun As Object)
    ImagePackage_ParseList(CType(ToRun, WIMGroup))
  End Sub
  Private Sub ImagePackage_ParseList(ToRun As WIMGroup)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ListInvoker(AddressOf ImagePackage_ParseList), ToRun)
      Return
    End If
    GUI_ToggleEnabled(False)
    TitleMNG_SetDisplay(TitleMNG_List.Delete)
    TitleMNG_SetTitle("Parsing WIM Packages", "Reading data from Windows Image package descriptor...")
    If ToRun = WIMGroup.WIM Then
      ImagePackage_ParseList_WIM()
      If thdWIMDragDrop Is Nothing Then
        thdWIMDragDrop = New Threading.Thread(New Threading.ThreadStart(AddressOf WIM_PopulateISOData))
        thdWIMDragDrop.Start()
      End If
    ElseIf ToRun = WIMGroup.Merge Then
      ImagePackage_ParseList_Merge()
    Else
      ImagePackage_ParseList_WIM()
      ImagePackage_ParseList_Merge()
    End If
    ImagePackage_ClearInputThreads(ToRun)
    If lvMSU.Items.Count > 0 Then Updates_Requirements()
    GUI_ToggleEnabled(True)
    TitleMNG_DrawTitle()
  End Sub
  Private Sub ImagePackage_ParseList_WIM()
    Dim sWIM As String = txtWIM.Text
    If String.IsNullOrEmpty(sWIM) Then Return
    Dim ParseWork As String = IO.Path.Combine(Work, "PARSE")
    Dim ParseWorkWIM As String = IO.Path.Combine(ParseWork, WIMGroup.WIM.ToString)
    If IO.File.Exists(ParseWork) Then
      Status_SetText("Clearing Temporary Files...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ParseWork))
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
    End If
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    Dim iTotalCount As Integer = 2
    Dim iTotalVal As Integer = 0
    Progress_Total(0, 1)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      Status_SetText("Extracting Image from ISO...")
      iTotalCount += 1
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      Extract_File(sWIM, ParseWorkWIM, "INSTALL.WIM")
      WIMFile = IO.Path.Combine(ParseWorkWIM, "INSTALL.WIM")
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      Status_SetText("Reading Image Packages...")
      ImagePackage_ClearList(WIMGroup.WIM)
      Progress_Normal(0, 1)
      Dim PackageCount As Integer = DISM_Image_GetCount(WIMFile)
      Progress_Normal(0, PackageCount)
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      Status_SetText("Populating Image Package List...")
      Dim imageCollection As New List(Of ListViewItem)
      For I As Integer = 1 To PackageCount
        Progress_Normal(I, PackageCount, True)
        Dim Package As ImagePackage = DISM_Image_GetData(WIMFile, I)
        Dim lvItem As New ListViewItem(CStr(Package.Index))
        lvItem.Name = String.Format("lviPackage{0}", AlphanumericVal(Package.ToString))
        If Package.IsEmpty Then Return
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(Package.Architecture)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        Dim lTag As Integer = 0
        Do
          lTag += 1
          If lTag >= Integer.MaxValue Then Exit Do
        Loop While ImagePackage_ListData.ContainsKey(lTag)
        ImagePackage_ListData.Add(lTag, New ImagePackage_PackageData(WIMGroup.WIM, Package, Nothing, Nothing))
        lvItem.Tag = lTag
        Dim ttItem As String = Nothing
        If Package.SPLevel > 0 Then
          ttItem = String.Concat(String.Format("{0} {1}.{2} ({3} Service Pack {4}) {5}", Package.ProductType, Package.Version, Package.SPBuild, Package.Edition, Package.SPLevel, Package.Architecture), vbNewLine,
                                 en, Package.Desc, vbNewLine,
                                 en, String.Format("{0} files, {1} folders", FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True), FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True)), vbNewLine,
                                 en, Package.Modified, vbNewLine,
                                 en, ShortenPath(WIMFile))
        Else
          ttItem = String.Concat(String.Format("{0} {1}.{2} ({3}) {4}", Package.ProductType, Package.Version, Package.SPBuild, Package.Edition, Package.Architecture), vbNewLine,
                                 en, Package.Desc, vbNewLine,
                                 en, String.Format("{0} files, {0} folders", FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True), FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True)), vbNewLine,
                                 en, Package.Modified, vbNewLine,
                                 en, ShortenPath(WIMFile))
        End If
        lvItem.ToolTipText = ttItem
        imageCollection.Add(lvItem)
      Next
      ImagePackage_AddToList(imageCollection.ToArray)
      If thdMergeInput Is Nothing Then
        Status_SetText("Idle")
      Else
        Status_SetText("Extracting Merge Image from ISO...")
      End If
    Else
      Status_SetText("Could not Extract Image from ISO!")
    End If
    Progress_Normal(0, 1)
    Progress_Total(0, 1)
  End Sub
  Private Sub ImagePackage_ParseList_Merge()
    Dim sMerge As String = txtMerge.Text
    Dim ParseWork As String = IO.Path.Combine(Work, "PARSE")
    Dim ParseWorkMerge As String = IO.Path.Combine(ParseWork, WIMGroup.Merge.ToString)
    IO.Directory.CreateDirectory(ParseWorkMerge)
    If String.IsNullOrEmpty(sMerge) Then Return
    Dim MergeFile As String = String.Empty
    Dim iTotalCount As Integer = 2
    Dim iTotalVal As Integer = 0
    Progress_Total(0, 1)
    If IO.Path.GetExtension(sMerge).ToLower = ".iso" Then
      Status_SetText("Extracting Merge Image from ISO...")
      iTotalCount += 1
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      Extract_File(sMerge, ParseWorkMerge, "INSTALL.WIM")
      MergeFile = IO.Path.Combine(ParseWorkMerge, "INSTALL.WIM")
    Else
      MergeFile = sMerge
    End If
    If IO.File.Exists(MergeFile) Then
      Status_SetText("Reading Merge Image Packages...")
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      ImagePackage_ClearList(WIMGroup.Merge)
      Progress_Normal(0, 1)
      Dim PackageCount As Integer = DISM_Image_GetCount(MergeFile)
      Progress_Normal(0, PackageCount)
      iTotalVal += 1
      Progress_Total(iTotalVal, iTotalCount)
      Status_SetText("Populating Merge Image Package List...")
      Dim imageCollection As New List(Of ListViewItem)
      For I As Integer = 1 To PackageCount
        Progress_Normal(I, PackageCount, True)
        Dim Package As ImagePackage = DISM_Image_GetData(MergeFile, I)
        Dim lvItem As New ListViewItem(CStr(Package.Index))
        lvItem.Name = String.Format("lviPackage{0}", AlphanumericVal(Package.ToString))
        If Package.IsEmpty Then Return
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(Package.Architecture)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        Dim lTag As Integer = 0
        Do
          lTag += 1
          If lTag >= Integer.MaxValue Then Exit Do
        Loop While ImagePackage_ListData.ContainsKey(lTag)
        ImagePackage_ListData.Add(lTag, New ImagePackage_PackageData(WIMGroup.Merge, Package, Nothing, Nothing))
        lvItem.Tag = lTag
        Dim ttItem As String = Nothing
        If Package.SPLevel > 0 Then
          ttItem = String.Concat(String.Format("{0} {1}.{2} ({3} Service Pack {4}) {5}", Package.ProductType, Package.Version, Package.SPBuild, Package.Edition, Package.SPLevel, Package.Architecture), vbNewLine,
                                 en, Package.Desc, vbNewLine,
                                 en, String.Format("{0} files, {1} folders", FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True), FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True)), vbNewLine,
                                 en, Package.Modified, vbNewLine,
                                 en, ShortenPath(MergeFile))
        Else
          ttItem = String.Concat(String.Format("{0} {1}.{2} ({3}) {4}", Package.ProductType, Package.Version, Package.SPBuild, Package.Edition, Package.Architecture), vbNewLine,
                                 en, Package.Desc, vbNewLine,
                                 en, String.Format("{0} files, {0} folders", FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True), FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True)), vbNewLine,
                                 en, Package.Modified, vbNewLine,
                                 en, ShortenPath(MergeFile))
        End If
        lvItem.ToolTipText = ttItem
        imageCollection.Add(lvItem)
      Next
      ImagePackage_AddToList(imageCollection.ToArray)
      If thdWIMInput Is Nothing Then
        Status_SetText("Idle")
      Else
        Status_SetText("Extracting Image from ISO...")
      End If
    Else
      Status_SetText("Could not Extract Merge Image from ISO!")
    End If
    Progress_Normal(0, 1)
    Progress_Total(0, 1)
  End Sub
  Private Sub ImagePackage_Rename(selItem As ListViewItem)
    Dim lvIndex As Integer = CInt(selItem.Tag)
    Dim ItemData As ImagePackage_PackageData = ImagePackage_ListData(lvIndex)
    Dim sName As String = ItemData.NewName
    If String.IsNullOrEmpty(sName) Then sName = selItem.SubItems(1).Text
    Dim txtInput As New TextBox
    lvImages.Controls.Add(txtInput)
    txtInput.Location = New Point(selItem.SubItems(1).Bounds.X + 6, selItem.SubItems(1).Bounds.Y + 2)
    txtInput.Size = New Size(selItem.SubItems(1).Bounds.Width - 8, selItem.SubItems(1).Bounds.Height - 1)
    txtInput.BackColor = selItem.SubItems(1).BackColor
    txtInput.ForeColor = selItem.SubItems(1).ForeColor
    txtInput.BorderStyle = BorderStyle.None
    txtInput.Text = sName
    txtInput.Focus()
    txtInput.SelectAll()
    AddHandler txtInput.KeyPress, New EventHandler(Of KeyPressEventArgs)(
      Sub(s As Object, ea As KeyPressEventArgs)
        If AscW(ea.KeyChar) = Keys.Return Then
          If Not String.IsNullOrEmpty(txtInput.Text) Then
            ItemData.NewName = txtInput.Text
            ImagePackage_ListData(lvIndex) = ItemData
            selItem.SubItems(1).Text = txtInput.Text
          End If
          lvImages.Controls.Remove(txtInput)
          txtInput = Nothing
          ea.Handled = True
        ElseIf AscW(ea.KeyChar) = Keys.Escape Then
          txtInput.Text = sName
          lvImages.Controls.Remove(txtInput)
          txtInput = Nothing
          ea.Handled = True
        End If
      End Sub)
    AddHandler txtInput.LostFocus, New EventHandler(
      Sub(s As Object, ea As EventArgs)
        If lvImages.Controls.Contains(txtInput) Then
          If Not String.IsNullOrEmpty(txtInput.Text) Then
            ItemData.NewName = txtInput.Text
            ImagePackage_ListData(lvIndex) = ItemData
            selItem.SubItems(1).Text = txtInput.Text
          End If
          lvImages.Controls.Remove(txtInput)
          txtInput = Nothing
        End If
      End Sub)
  End Sub
  Private Sub chkLoadPackageData_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkLoadFeatures.CheckedChanged, chkLoadUpdates.CheckedChanged, chkLoadDrivers.CheckedChanged
    If Not chkLoadFeatures.Enabled And Not chkLoadUpdates.Enabled And Not chkLoadDrivers.Enabled Then Return
    cmdLoadPackages.Enabled = chkLoadFeatures.Checked Or chkLoadUpdates.Checked Or chkLoadDrivers.Checked
    Dim sList As New List(Of String)
    If chkLoadFeatures.Checked Then sList.Add("Features")
    If chkLoadUpdates.Checked Then sList.Add("Updates")
    If chkLoadDrivers.Checked Then sList.Add("Drivers")
    Dim sListText As String
    If sList.Count = 0 Then
      sListText = "the components you select"
    ElseIf sList.Count = 1 Then
      sListText = sList(0)
    ElseIf sList.Count = 2 Then
      sListText = String.Format("{0} and {1}", sList(0), sList(1))
    Else
      sListText = String.Format("{0}, {1}, and {2}", sList(0), sList(1), sList(2))
    End If
    ttInfo.SetToolTip(cmdLoadPackages, String.Concat(String.Format("Begin Image Package Parsing procedure to gather information about {0}.", sListText), vbNewLine, vbNewLine,
                                       "(This is optional - if you know which items are already integrated, you can save time by skipping this.)"))
    If Not isStarting Then
      If Not chkLoadFeatures.Checked = mySettings.LoadFeatures Then mySettings.LoadFeatures = chkLoadFeatures.Checked
      If Not chkLoadUpdates.Checked = mySettings.LoadUpdates Then mySettings.LoadUpdates = chkLoadUpdates.Checked
      If Not chkLoadDrivers.Checked = mySettings.LoadDrivers Then mySettings.LoadDrivers = chkLoadDrivers.Checked
    End If
  End Sub
  Private Sub cmdLoadPackages_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoadPackages.Click
    If (Not chkLoadFeatures.Checked) And (Not chkLoadUpdates.Checked) And (Not chkLoadDrivers.Checked) Then
      MsgDlg(Me, "You must select which data you wish to load from the Image Packages before beginning the procedure.", "No Data Selected", "Parse Image Packages", MessageBoxButtons.OK, _TaskDialogIcon.ControlPanel, , , "No Parse Data Selected")
      Return
    End If
    StopRun = False
    cmdLoadPackages.Image = My.Resources.u_i
    Dim doFeatures As Boolean = chkLoadFeatures.Checked
    Dim doUpdates As Boolean = chkLoadUpdates.Checked
    Dim doDrivers As Boolean = chkLoadDrivers.Checked
    Dim doMerge As Boolean = chkMerge.Checked AndAlso Not String.IsNullOrEmpty(txtMerge.Text)
    ImagePackage_Init(doFeatures, doUpdates, doDrivers, lvImages.Items.Count > 1)
    Dim pbVal As Integer = 0
    Dim pbMax As Integer = 3
    ' +1 Ensure the location of the WIM file
    ' +1 Mount Package
    ' +1 Unmount Package
    If doFeatures Then pbMax += 1 ' +1 Parse Features
    If doUpdates Then pbMax += 1 ' +1 Parse Updates
    If doDrivers Then pbMax += 1 ' +1 Parse Drivers
    If doMerge Then pbMax *= 2 ' *2 Do all the same things for the Merge WIM
    Progress_Normal(0, 1)
    pbVal += 1 ' +1 Ensure the location of the Main WIM file
    Progress_Total(pbVal, pbMax)
    cmdLoadPackages.Image = My.Resources.u_p
    chkLoadFeatures.Checked = False
    chkLoadUpdates.Checked = False
    chkLoadDrivers.Checked = False
    Dim WIMData As KeyValuePair(Of String, Integer) = ImagePackage_Prepare(WIMGroup.WIM)
    If String.IsNullOrEmpty(WIMData.Key) Then
      ImagePackage_Complete(Not StopRun)
      chkLoadFeatures.Checked = doFeatures
      chkLoadUpdates.Checked = doUpdates
      chkLoadDrivers.Checked = doDrivers
      cmdLoadPackages.Image = My.Resources.u_u
      Return
    End If
    Dim pbValSub As Integer = pbVal * WIMData.Value
    Dim pbMaxSub As Integer = pbMax * WIMData.Value
    For I As Integer = 1 To WIMData.Value
      Progress_Normal(0, 1)
      pbValSub += 1 ' +1/count of packages in main wim Mount Package
      Progress_Total(pbValSub, pbMaxSub)
      cmdLoadPackages.Image = My.Resources.u_p
      chkLoadFeatures.Checked = False
      chkLoadUpdates.Checked = False
      chkLoadDrivers.Checked = False
      Dim MountPath As ImagePackage_ImageData = ImagePackage_Mount(WIMData.Key, WIMGroup.WIM, I)
      If MountPath.IsEmpty Then
        ImagePackage_Complete(Not StopRun)
        chkLoadFeatures.Checked = doFeatures
        chkLoadUpdates.Checked = doUpdates
        chkLoadDrivers.Checked = doDrivers
        cmdLoadPackages.Image = My.Resources.u_u
        Return
      End If
      If MountPath.Name = "SKIP" Then
        pbVal += 5
        If doFeatures Then pbValSub += 1
        If doUpdates Then pbValSub += 1
        If doDrivers Then pbValSub += 1
        pbValSub += 1
        Continue For
      End If
      If StopRun Then
        ImagePackage_Complete(False)
        chkLoadFeatures.Checked = doFeatures
        chkLoadUpdates.Checked = doUpdates
        chkLoadDrivers.Checked = doDrivers
        cmdLoadPackages.Image = My.Resources.u_n
        Return
      End If
      cmdLoadPackages.Image = My.Resources.u_i
      If doFeatures Then
        Progress_Normal(0, 1)
        pbValSub += 1 ' +1/count of packages in main wim Parse Features
        Progress_Total(pbValSub, pbMaxSub)
        chkLoadFeatures.Checked = True
        chkLoadUpdates.Checked = False
        chkLoadDrivers.Checked = False
        If Not ImagePackage_Features_Load(MountPath) Then
          Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
          ImagePackage_Discard(MountPath)
          ImagePackage_Complete(Not StopRun, failStatus)
          chkLoadFeatures.Checked = doFeatures
          chkLoadUpdates.Checked = doUpdates
          chkLoadDrivers.Checked = doDrivers
          cmdLoadPackages.Image = My.Resources.u_u
          Return
        End If
      End If
      If doUpdates Then
        Progress_Normal(0, 1)
        pbValSub += 1 ' +1/count of packages in main wim Parse Updates
        Progress_Total(pbValSub, pbMaxSub)
        chkLoadFeatures.Checked = False
        chkLoadUpdates.Checked = True
        chkLoadDrivers.Checked = False
        If Not ImagePackage_Updates_Load(MountPath) Then
          Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
          ImagePackage_Discard(MountPath)
          ImagePackage_Complete(Not StopRun, failStatus)
          chkLoadFeatures.Checked = doFeatures
          chkLoadUpdates.Checked = doUpdates
          chkLoadDrivers.Checked = doDrivers
          cmdLoadPackages.Image = My.Resources.u_u
          Return
        End If
      End If
      If doDrivers Then
        Progress_Normal(0, 1)
        pbValSub += 1 ' +1/count of packages in main wim Parse Drivers
        Progress_Total(pbValSub, pbMaxSub)
        chkLoadFeatures.Checked = False
        chkLoadUpdates.Checked = False
        chkLoadDrivers.Checked = True
        If Not ImagePackage_Drivers_Load(MountPath) Then
          Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
          ImagePackage_Discard(MountPath)
          ImagePackage_Complete(Not StopRun, failStatus)
          chkLoadFeatures.Checked = doFeatures
          chkLoadUpdates.Checked = doUpdates
          chkLoadDrivers.Checked = doDrivers
          cmdLoadPackages.Image = My.Resources.u_u
          Return
        End If
      End If
      Progress_Normal(0, 1)
      pbValSub += 1 ' +1/count of packages in main wim Unmount Package
      Progress_Total(pbValSub, pbMaxSub)
      cmdLoadPackages.Image = My.Resources.u_a
      chkLoadFeatures.Checked = False
      chkLoadUpdates.Checked = False
      chkLoadDrivers.Checked = False
      ImagePackage_Discard(MountPath)
      pbVal += 5
    Next
    If doMerge Then
      Progress_Normal(0, 1)
      pbVal += 1 ' +1 Ensure the location of the Merge WIM file
      Progress_Total(pbVal, pbMax)
      cmdLoadPackages.Image = My.Resources.u_p
      chkLoadFeatures.Checked = False
      chkLoadUpdates.Checked = False
      chkLoadDrivers.Checked = False
      Dim MergeData As KeyValuePair(Of String, Integer) = ImagePackage_Prepare(WIMGroup.Merge)
      If String.IsNullOrEmpty(MergeData.Key) Then
        ImagePackage_Complete(Not StopRun)
        chkLoadFeatures.Checked = doFeatures
        chkLoadUpdates.Checked = doUpdates
        chkLoadDrivers.Checked = doDrivers
        cmdLoadPackages.Image = My.Resources.u_u
        Return
      End If
      pbValSub = pbVal * MergeData.Value
      pbMaxSub = pbMax * MergeData.Value
      For I As Integer = 1 To MergeData.Value
        Progress_Normal(0, 1)
        pbValSub += 1 ' +1/count of packages in main wim Mount Package
        Progress_Total(pbValSub, pbMaxSub)
        cmdLoadPackages.Image = My.Resources.u_p
        chkLoadFeatures.Checked = False
        chkLoadUpdates.Checked = False
        chkLoadDrivers.Checked = False
        Dim MountPath As ImagePackage_ImageData = ImagePackage_Mount(MergeData.Key, WIMGroup.Merge, I)
        If MountPath.IsEmpty Then
          ImagePackage_Complete(Not StopRun)
          chkLoadFeatures.Checked = doFeatures
          chkLoadUpdates.Checked = doUpdates
          chkLoadDrivers.Checked = doDrivers
          cmdLoadPackages.Image = My.Resources.u_u
          Return
        End If
        If MountPath.Name = "SKIP" Then
          pbVal += 5
          If doFeatures Then pbValSub += 1
          If doUpdates Then pbValSub += 1
          If doDrivers Then pbValSub += 1
          pbValSub += 1
          Continue For
        End If
        If StopRun Then
          ImagePackage_Complete(False)
          chkLoadFeatures.Checked = doFeatures
          chkLoadUpdates.Checked = doUpdates
          chkLoadDrivers.Checked = doDrivers
          cmdLoadPackages.Image = My.Resources.u_n
          Return
        End If
        cmdLoadPackages.Image = My.Resources.u_i
        If doFeatures Then
          Progress_Normal(0, 1)
          pbValSub += 1 ' +1/count of packages in main wim Parse Features
          Progress_Total(pbValSub, pbMaxSub)
          chkLoadFeatures.Checked = True
          chkLoadUpdates.Checked = False
          chkLoadDrivers.Checked = False
          If Not ImagePackage_Features_Load(MountPath) Then
            Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
            ImagePackage_Discard(MountPath)
            ImagePackage_Complete(Not StopRun, failStatus)
            chkLoadFeatures.Checked = doFeatures
            chkLoadUpdates.Checked = doUpdates
            chkLoadDrivers.Checked = doDrivers
            cmdLoadPackages.Image = My.Resources.u_u
            Return
          End If
        End If
        If doUpdates Then
          Progress_Normal(0, 1)
          pbValSub += 1 ' +1/count of packages in main wim Parse Updates
          Progress_Total(pbValSub, pbMaxSub)
          chkLoadFeatures.Checked = False
          chkLoadUpdates.Checked = True
          chkLoadDrivers.Checked = False
          If Not ImagePackage_Updates_Load(MountPath) Then
            Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
            ImagePackage_Discard(MountPath)
            ImagePackage_Complete(Not StopRun, failStatus)
            chkLoadFeatures.Checked = doFeatures
            chkLoadUpdates.Checked = doUpdates
            chkLoadDrivers.Checked = doDrivers
            cmdLoadPackages.Image = My.Resources.u_u
            Return
          End If
        End If
        If doDrivers Then
          Progress_Normal(0, 1)
          pbValSub += 1 ' +1/count of packages in main wim Parse Drivers
          Progress_Total(pbValSub, pbMaxSub)
          chkLoadFeatures.Checked = False
          chkLoadUpdates.Checked = False
          chkLoadDrivers.Checked = True
          If Not ImagePackage_Drivers_Load(MountPath) Then
            Dim failStatus As String = CStr(IIf(StopRun, Nothing, Status_GetText()))
            ImagePackage_Discard(MountPath)
            ImagePackage_Complete(Not StopRun, failStatus)
            chkLoadFeatures.Checked = doFeatures
            chkLoadUpdates.Checked = doUpdates
            chkLoadDrivers.Checked = doDrivers
            cmdLoadPackages.Image = My.Resources.u_u
            Return
          End If
        End If
        Progress_Normal(0, 1)
        pbValSub += 1 ' +1/count of packages in main wim Unmount Package
        Progress_Total(pbValSub, pbMaxSub)
        cmdLoadPackages.Image = My.Resources.u_a
        chkLoadFeatures.Checked = False
        chkLoadUpdates.Checked = False
        chkLoadDrivers.Checked = False
        ImagePackage_Discard(MountPath)
        pbVal += 5
      Next
    End If
    ImagePackage_Complete(False)
    chkLoadFeatures.Checked = doFeatures
    chkLoadUpdates.Checked = doUpdates
    chkLoadDrivers.Checked = doDrivers
    cmdLoadPackages.Image = My.Resources.u_a
  End Sub
  Friend Sub ImagePackage_Init(doingFeatures As Boolean, doingUpdates As Boolean, doingDrivers As Boolean, doingMultiplePackages As Boolean)
    GUI_ToggleEnabled(False)
    StopRun = False
    TitleMNG_SetDisplay(TitleMNG_List.Delete)
    Dim sList As New List(Of String)
    If doingFeatures Then sList.Add("Features")
    If doingUpdates Then sList.Add("Updates")
    If doingDrivers Then sList.Add("Drivers")
    Dim sListText As String
    If sList.Count = 0 Then
      sListText = "the components you select"
    ElseIf sList.Count = 1 Then
      sListText = sList(0)
    ElseIf sList.Count = 2 Then
      sListText = String.Format("{0} and {1}", sList(0), sList(1))
    Else
      sListText = String.Format("{0}, {1}, and {2}", sList(0), sList(1), sList(2))
    End If
    If doingMultiplePackages Then
      TitleMNG_SetTitle("Parsing Image Packages", String.Format("Populating information about {0} from each image package...", sListText))
    Else
      TitleMNG_SetTitle("Parsing Image Package", String.Format("Populating information about {0} from the selected image package...", sListText))
    End If
    CleanMounts(True)
  End Sub
  Private Shared ImagePackage_Prepare_Return As KeyValuePair(Of String, Integer)
  Friend Function ImagePackage_Prepare(ImageGroup As WIMGroup) As KeyValuePair(Of String, Integer)
    RunActivity = ActivityType.LoadingPackageData
    ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Prepare_Task))
    thdPackageListUpdate.Start(CObj(ImageGroup))
    Do While String.IsNullOrEmpty(ImagePackage_Prepare_Return.Key)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return Nothing
    Loop
    thdPackageListUpdate = Nothing
    If ImagePackage_Prepare_Return.Key.StartsWith("ERROR: ") Then
      Status_SetText(ImagePackage_Prepare_Return.Key.Substring(7))
      Return Nothing
    End If
    Return ImagePackage_Prepare_Return
  End Function
  Private Sub ImagePackage_Prepare_Task(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Prepare_Task), Param)
      Return
    End If
    Dim ImageGroup As WIMGroup = CType(Param, WIMGroup)
    Dim sWIM As String = Nothing
    If ImageGroup = WIMGroup.WIM Then
      sWIM = txtWIM.Text
    ElseIf ImageGroup = WIMGroup.Merge Then
      sWIM = txtMerge.Text
    End If
    If String.IsNullOrEmpty(sWIM) Then
      ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)("ERROR: No WIM file selected!", 0)
      Return
    End If
    If Not IO.File.Exists(sWIM) Then
      ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)("ERROR: Could not Find Selected WIM!", 0)
      Return
    End If
    Progress_Normal(0, 1)
    Dim ParseWork As String = IO.Path.Combine(Work, "PARSE")
    Dim ParseWorkWIM As String = IO.Path.Combine(ParseWork, ImageGroup.ToString)
    Dim progVal As Integer = 0
    Dim progMax As Integer = 3
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    If IO.File.Exists(ParseWork) Then
      Status_SetText("Clearing Temporary Files...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ParseWork))
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents, True)
    End If
    If StopRun Then Return
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      Status_SetText("Extracting Image from ISO...")
      Extract_File(sWIM, ParseWorkWIM, "INSTALL.WIM")
      If StopRun Then Return
      WIMFile = IO.Path.Combine(ParseWorkWIM, "INSTALL.WIM")
      If Not IO.File.Exists(WIMFile) Then
        ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)("ERROR: Could not Extract Image from ISO!", 0)
        Return
      End If
    Else
      WIMFile = sWIM
    End If
    If Not IO.File.Exists(WIMFile) Then
      ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)("ERROR: Could not Find Selected WIM!", 0)
      Return
    End If
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    Status_SetText("Reading Image Packages...")
    Dim PackageCount As Integer = DISM_Image_GetCount(WIMFile)
    ImagePackage_Prepare_Return = New KeyValuePair(Of String, Integer)(WIMFile, PackageCount)
  End Sub
  Private Shared ImagePackage_Mount_Return As ImagePackage_ImageData
  Friend Function ImagePackage_Mount(WIMFile As String, ImageGroup As WIMGroup, SelectedIndex As Integer) As ImagePackage_ImageData
    RunActivity = ActivityType.LoadingPackageData
    ImagePackage_Mount_Return = Nothing
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Mount_Task))
    thdPackageListUpdate.Start(CType({WIMFile, ImageGroup, SelectedIndex}, Object()))
    Do While ImagePackage_Mount_Return.IsEmpty
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return Nothing
    Loop
    thdPackageListUpdate = Nothing
    If ImagePackage_Mount_Return.IsEmpty Then Return Nothing
    If ImagePackage_Mount_Return.Name.StartsWith("ERROR: ") Then
      Status_SetText(ImagePackage_Mount_Return.Name.Substring(7))
      Return Nothing
    End If
    Return ImagePackage_Mount_Return
  End Function
  Private Sub ImagePackage_Mount_Task(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Mount_Task), Param)
      Return
    End If
    Dim WIMFile As String = CType(CType(Param, Object())(0), String)
    Dim ImageGroup As WIMGroup = CType(CType(Param, Object())(1), WIMGroup)
    Dim SelIndex As Integer = CInt(CType(Param, Object())(2))
    If String.IsNullOrEmpty(WIMFile) Then
      ImagePackage_Mount_Return = New ImagePackage_ImageData("ERROR: No WIM file selected!", 0)
      Return
    End If
    If Not IO.File.Exists(WIMFile) Then
      ImagePackage_Mount_Return = New ImagePackage_ImageData("ERROR: Could not Find Selected WIM!", 0)
      Return
    End If
    Progress_Normal(0, 1)
    Dim progVal As Integer = 0
    Dim progMax As Integer = 2
    Status_SetText(String.Format("Loading Image Package #{0} Data...", SelIndex))
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    Dim Package As ImagePackage = DISM_Image_GetData(WIMFile, SelIndex)
    If Package.IsEmpty Then
      ImagePackage_Mount_Return = New ImagePackage_ImageData(String.Format("ERROR: Could not Parse Image Package #{0} Data!", SelIndex), 0)
      Return
    End If
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In lvImages.Items
      Dim lvIndex As Integer = CInt(item.Tag)
      If ImagePackage_ListData(lvIndex).Group = ImageGroup AndAlso ImagePackage_ListData(lvIndex).Package = Package Then
        lvItem = item
        Exit For
      End If
    Next
    If lvItem Is Nothing Then
      ImagePackage_Mount_Return = New ImagePackage_ImageData(String.Format("ERROR: Failed to Match Package ""{0}"" to an Image Package!", Package.Name), 0)
      Return
    End If
    Dim lvIndex2 As Integer = 0
    If lvItem.Tag IsNot Nothing Then
      lvIndex2 = CInt(lvItem.Tag)
      Dim tFI As List(Of Feature) = ImagePackage_ListData(lvIndex2).FeatureList
      Dim tDI As List(Of Driver) = ImagePackage_ListData(lvIndex2).DriverList
      Dim tUI As List(Of Update_Integrated) = ImagePackage_ListData(lvIndex2).Package.IntegratedUpdateList
      If (tFI IsNot Nothing AndAlso tFI.Count > 0) And (tDI IsNot Nothing AndAlso tDI.Count > 0) And (tUI IsNot Nothing AndAlso tUI.Count > 0) Then
        ImagePackage_Mount_Return = New ImagePackage_ImageData("SKIP", 0)
        Return
      End If
    End If
    Status_SetText(String.Format("Mounting {0} Package...", Package.Name))
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    If Not DISM_Image_Load(WIMFile, SelIndex, Mount) Then
      ImagePackage_Mount_Return = New ImagePackage_ImageData(String.Format("ERROR: Failed to Mount Package ""{0}""!", Package.Name), 0)
      Return
    End If
    ImagePackage_Mount_Return = New ImagePackage_ImageData(Package.Name, lvIndex2)
  End Sub
  Private ImagePackage_Discard_Loaded As Boolean
  Friend Sub ImagePackage_Discard(Path As ImagePackage_ImageData)
    ImagePackage_Discard_Loaded = False
    TitleMNG_SetDisplay(TitleMNG_List.Delete)
    RunActivity = ActivityType.LoadingPackageFeatures
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Discard_Task))
    thdPackageListUpdate.Start(CObj(Path))
    Do Until ImagePackage_Discard_Loaded
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return
    Loop
    thdPackageListUpdate = Nothing
  End Sub
  Private Sub ImagePackage_Discard_Task(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Discard_Task), Param)
      Return
    End If
    Dim ImagePath As ImagePackage_ImageData = CType(Param, ImagePackage_ImageData)
    Progress_Normal(0, 1)
    Status_SetText(String.Format("Dismounting {0} Package...", ImagePath.Name))
    DISM_Image_Discard(Mount)
    ImagePackage_Discard_Loaded = True
  End Sub
  Friend Sub ImagePackage_Complete(Fail As Boolean, Optional oldStatus As String = Nothing)
    If Fail Then
      If String.IsNullOrEmpty(oldStatus) Then oldStatus = Status_GetText()
    Else
      oldStatus = "Idle"
    End If
    Progress_Normal(0, 1)
    Progress_Total(0, 1)
    GUI_ToggleEnabled(True, oldStatus)
  End Sub
#Region "Package Feature List"
  Private ImagePackage_Features_Load_Response As String
  Friend Function ImagePackage_Features_Load(ImagePath As ImagePackage_ImageData) As Boolean
    ImagePackage_Features_Load_Response = Nothing
    RunActivity = ActivityType.LoadingPackageFeatures
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Features_Parse))
    thdPackageListUpdate.Start(CObj(ImagePath))
    Do While String.IsNullOrEmpty(ImagePackage_Features_Load_Response)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return False
    Loop
    thdPackageListUpdate = Nothing
    If ImagePackage_Features_Load_Response = "OK" Then Return True
    If ImagePackage_Features_Load_Response = "SKIP" Then Return True
    Status_SetText(ImagePackage_Features_Load_Response)
    Return False
  End Function
  Private Sub ImagePackage_Features_Parse(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Features_Parse), Param)
      Return
    End If
    Dim ImagePath As ImagePackage_ImageData = CType(Param, ImagePackage_ImageData)
    Dim progVal As Integer = 0
    Dim progMax As Integer = 2
    Progress_Normal(0, 1)
    Dim tFL As List(Of Feature) = ImagePackage_ListData(ImagePath.Index).FeatureList
    If tFL IsNot Nothing AndAlso tFL.Count > 0 Then
      ImagePackage_Features_Load_Response = "SKIP"
      Return
    End If
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    Status_SetText(String.Format("Populating {0} Package Feature List...", ImagePath.Name))
    Dim pData As ImagePackage_PackageData = ImagePackage_ListData(ImagePath.Index)
    Dim FeatureNames() As String = DISM_Feature_GetList(Mount)
    If FeatureNames Is Nothing Then
      ImagePackage_Features_Load_Response = String.Format("Failed to read {0} Package Feature List!", ImagePath.Name)
      Return
    ElseIf FeatureNames.Length < 1 Then
      ImagePackage_Features_Load_Response = String.Format("{0} Package has no Features!", ImagePath.Name)
      Return
    End If
    Dim FeatureCount As Integer = FeatureNames.Length
    If StopRun Then Return
    Dim Features As New List(Of Feature)
    progVal += 1
    For I As Integer = 0 To FeatureCount - 1
      Progress_Normal((progVal - 1) * FeatureCount + (I + 1), progMax * FeatureCount, True)
      Status_SetText(String.Format("Parsing {0} Package Feature {2} of {3}: {1}...", ImagePath.Name, FeatureNames(I), (I + 1), FeatureCount))
      Dim loadedFeature As Feature = DISM_Feature_GetData(Mount, FeatureNames(I))
      If Not loadedFeature.IsEmpty Then Features.Add(loadedFeature)
      If StopRun Then Return
    Next
    If StopRun Then Return
    'progVal += 1
    'Progress_Normal(progVal, progMax, True)
    pData.FeatureList = Features
    ImagePackage_ListData(ImagePath.Index) = pData
    ImagePackage_Features_Load_Response = "OK"
  End Sub
#End Region
#Region "Package Updates List"
  Private ImagePackage_Updates_Load_Response As String
  Friend Function ImagePackage_Updates_Load(ImagePath As ImagePackage_ImageData) As Boolean
    ImagePackage_Updates_Load_Response = Nothing
    RunActivity = ActivityType.LoadingPackageUpdates
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Updates_Parse))
    thdPackageListUpdate.Start(CObj(ImagePath))
    Do While String.IsNullOrEmpty(ImagePackage_Updates_Load_Response)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return False
    Loop
    Updates_Requirements()
    thdPackageListUpdate = Nothing
    If ImagePackage_Updates_Load_Response = "OK" Then Return True
    If ImagePackage_Updates_Load_Response = "SKIP" Then Return True
    Status_SetText(ImagePackage_Updates_Load_Response)
    Return False
  End Function
  Private Sub ImagePackage_Updates_Parse(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Updates_Parse), Param)
      Return
    End If
    Dim ImagePath As ImagePackage_ImageData = CType(Param, ImagePackage_ImageData)
    Dim progVal As Integer = 0
    Dim progMax As Integer = 2
    Progress_Normal(0, 1)
    Dim tUL As List(Of Update_Integrated) = ImagePackage_ListData(ImagePath.Index).Package.IntegratedUpdateList
    If tUL IsNot Nothing AndAlso tUL.Count > 0 Then
      ImagePackage_Updates_Load_Response = "SKIP"
      Return
    End If
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    Status_SetText(String.Format("Populating {0} Package Update List...", ImagePath.Name))
    Dim pData As ImagePackage_PackageData = ImagePackage_ListData(ImagePath.Index)
    Dim Package As ImagePackage = pData.Package
    Dim upList As List(Of Update_Integrated) = DISM_Update_GetList(Mount, Package)
    If upList Is Nothing Then
      ImagePackage_Updates_Load_Response = String.Format("Failed to read {0} Package Update List!", ImagePath.Name)
      Return
    ElseIf upList.Count < 1 Then
      ImagePackage_Updates_Load_Response = String.Format("{0} Package has no Updates!", ImagePath.Name)
      Return
    End If
    Dim UpdateCount As Integer = upList.Count
    If StopRun Then Return
    progVal += 1
    For I As Integer = 0 To UpdateCount - 1
      Progress_Normal((progVal - 1) * UpdateCount + (I + 1), progMax * UpdateCount, True)
      Status_SetText(String.Format("Parsing {0} Package Update {2} of {3}: {1}...", ImagePath.Name, upList(I).Ident.Name, (I + 1), UpdateCount))
      DISM_Update_GetData(Mount, upList(I))
      If StopRun Then Return
    Next
    If StopRun Then Return
    'progVal += 1
    'Progress_Normal(progVal, progMax, True)
    Package.PopulateUpdateList(upList)
    If StopRun Then Return
    pData.Package = Package
    ImagePackage_ListData(ImagePath.Index) = pData
    ImagePackage_Updates_Load_Response = "OK"
  End Sub
#End Region
#Region "Package Drivers List"
  Private ImagePackage_Drivers_Load_Response As String
  Friend Function ImagePackage_Drivers_Load(ImagePath As ImagePackage_ImageData) As Boolean
    ImagePackage_Drivers_Load_Response = Nothing
    RunActivity = ActivityType.LoadingPackageDrivers
    thdPackageListUpdate = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ImagePackage_Drivers_Parse))
    thdPackageListUpdate.Start(CObj(ImagePath))
    Do While String.IsNullOrEmpty(ImagePackage_Drivers_Load_Response)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then Return False
    Loop
    thdPackageListUpdate = Nothing
    If ImagePackage_Drivers_Load_Response = "OK" Then Return True
    If ImagePackage_Drivers_Load_Response = "SKIP" Then Return True
    Status_SetText(ImagePackage_Drivers_Load_Response)
    Return False
  End Function
  Private Sub ImagePackage_Drivers_Parse(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New ImagePackage_ParseInvoker(AddressOf ImagePackage_Drivers_Parse), Param)
      Return
    End If
    Dim ImagePath As ImagePackage_ImageData = CType(Param, ImagePackage_ImageData)
    Dim progVal As Integer = 0
    Dim progMax As Integer = 2
    Progress_Normal(0, 1)
    If StopRun Then Return
    Dim tDL As List(Of Driver) = ImagePackage_ListData(ImagePath.Index).DriverList
    If tDL IsNot Nothing AndAlso tDL.Count > 0 Then
      ImagePackage_Drivers_Load_Response = "SKIP"
      Return
    End If
    progVal += 1
    Progress_Normal(progVal, progMax, True)
    Status_SetText(String.Format("Populating {0} Package Driver List...", ImagePath.Name))
    Dim pData As ImagePackage_PackageData = ImagePackage_ListData(ImagePath.Index)
    Dim Package As ImagePackage = pData.Package
    Dim arch As ArchitectureList = ArchitectureList.x86
    If CompareArchitectures(Package.Architecture, ArchitectureList.amd64, False) Then
      arch = ArchitectureList.amd64
    End If
    Dim driverList As List(Of Driver) = DISM_Driver_GetList(Mount, True)
    If driverList Is Nothing Then
      ImagePackage_Drivers_Load_Response = String.Format("Failed to read {0} Package Driver List!", ImagePath.Name)
      Return
    ElseIf driverList.Count < 1 Then
      ImagePackage_Drivers_Load_Response = String.Format("{0} Package has no Drivers!", ImagePath.Name)
      Return
    End If
    Dim DriverCount As Integer = driverList.Count
    If StopRun Then Return
    progVal += 1
    For I As Integer = 0 To DriverCount - 1
      Progress_Normal((progVal - 1) * DriverCount + (I + 1), progMax * DriverCount, True)
      Status_SetText(String.Format("Parsing {0} Package Driver {2} of {3}: {1}...", ImagePath.Name, driverList(I).OriginalFileName, (I + 1), DriverCount))
      DISM_Driver_GetData(Mount, driverList(I), arch)
      If StopRun Then Return
    Next
    If StopRun Then Return
    'progVal += 1
    'Progress_Normal(progVal, progMax, True)
    pData.DriverList = driverList
    ImagePackage_ListData(ImagePath.Index) = pData
    ImagePackage_Drivers_Load_Response = "OK"
  End Sub
#End Region
#Region "Context Menu"
  Private Sub mnuPackageInclude_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageInclude.Click
    mnuPackageInclude.Checked = Not mnuPackageInclude.Checked
    ImagePackage_ListSelection.Checked = mnuPackageInclude.Checked
  End Sub
  Private Sub mnuPackageRename_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageRename.Click
    ImagePackage_Rename(ImagePackage_ListSelection)
  End Sub
  Private Sub mnuPackageLocation_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageLocation.Click
    Dim sPath As String = Nothing
    If ImagePackage_ListData(CInt(ImagePackage_ListSelection.Tag)).Group = WIMGroup.Merge Then
      sPath = txtMerge.Text
    Else
      sPath = txtWIM.Text
    End If
    If Not String.IsNullOrEmpty(sPath) Then
      If IO.Directory.Exists(sPath) Or IO.File.Exists(sPath) Then
        Try
          Process.Start("explorer", String.Format("/select,""{0}""", sPath))
        Catch ex As Exception
          MsgDlg(Me, String.Format("Unable to open the folder for ""{0}""!", sPath), "Unable to open folder.", "Folder was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , ex.Message, "Open Folder Folder Missing")
        End Try
      Else
        MsgDlg(Me, String.Format("Unable to find the file ""{0}""!", sPath), "Unable to find Image.", "File was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , , "Open Folder File Missing")
      End If
    End If
  End Sub
  Private Sub mnuPackageProperties_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageProperties.Click
    ImagePackage_ShowProperties()
  End Sub
#End Region
  Friend Structure ImagePackage_ImageData
    Public [Name] As String
    Public Index As Integer
    Public Sub New(ImageName As String, ImageIndex As Integer)
      [Name] = ImageName
      Index = ImageIndex
    End Sub
    Public ReadOnly Property IsEmpty As Boolean
      Get
        If Not String.IsNullOrEmpty(Name) Then Return False
        If Not Index = 0 Then Return False
        Return True
      End Get
    End Property
  End Structure
  Friend Structure ImagePackage_PackageData
    Public Group As WIMGroup
    Public Package As ImagePackage
    Public FeatureList As List(Of Feature)
    Public DriverList As List(Of Driver)
    Public NewName As String
    Public NewDesc As String
    Public Sub New(ImageGroup As WIMGroup, pkgImage As ImagePackage, features As List(Of Feature), drivers As List(Of Driver))
      Group = ImageGroup
      Package = pkgImage
      FeatureList = features
      DriverList = drivers
      NewName = pkgImage.Name
      NewDesc = pkgImage.Desc
    End Sub
  End Structure
  Private Class ImagePackage_Sorter
    Implements IComparer
    Private sortBy As OrderBy
    Private sortOrdering As SortOrder
    Public Sub New()
      Me.New(OrderBy.OS, SortOrder.Ascending)
    End Sub
    Public Sub New(Sort As OrderBy, Sorting As SortOrder)
      sortBy = Sort
      sortOrdering = Sorting
    End Sub
    Public Enum OrderBy
      Display
      OS
      Size
      Architecture
    End Enum
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
      Dim ret As Integer = MakeComparison(CType(x, ListViewItem), CType(y, ListViewItem), sortBy)
      If sortOrdering = SortOrder.Ascending Then
        If ret = -1 Then Return 1
        If ret = 1 Then Return -1
        Return 0
      Else
        Return ret
      End If
    End Function
    Private Function MakeComparison(x As ListViewItem, y As ListViewItem, s As OrderBy) As Integer
      Dim xT As Integer = CInt(x.Tag)
      Dim yT As Integer = CInt(y.Tag)
      Select Case s
        Case OrderBy.Display
          If ImagePackage_ListData(xT).Group = WIMGroup.WIM And Not ImagePackage_ListData(yT).Group = WIMGroup.WIM Then Return -1
          If Not ImagePackage_ListData(xT).Group = WIMGroup.WIM And ImagePackage_ListData(yT).Group = WIMGroup.WIM Then Return 1
          If Val(x.SubItems(0).Text) > Val(y.SubItems(0).Text) Then Return 1
          If Val(x.SubItems(0).Text) < Val(y.SubItems(0).Text) Then Return -1
          Return 0
        Case OrderBy.OS
          Dim packageX As ImagePackage = ImagePackage_ListData(xT).Package
          Dim packageY As ImagePackage = ImagePackage_ListData(yT).Package
          Dim iX As Integer = 0
          Dim iY As Integer = 0
          If packageX.Edition = "Starter" Then
            iX = 1
          ElseIf packageX.Edition = "HomeBasic" Then
            iX = 2
          ElseIf packageX.Edition = "HomePremium" Then
            iX = 3
          ElseIf packageX.Edition = "Professional" Then
            iX = 4
          ElseIf packageX.Edition = "Ultimate" Then
            iX = 5
          ElseIf packageX.Edition = "Enterprise" Then
            iX = 6
          Else
            iX = 7
          End If
          If packageY.Edition = "Starter" Then
            iY = 1
          ElseIf packageY.Edition = "HomeBasic" Then
            iY = 2
          ElseIf packageY.Edition = "HomePremium" Then
            iY = 3
          ElseIf packageY.Edition = "Professional" Then
            iY = 4
          ElseIf packageY.Edition = "Ultimate" Then
            iY = 5
          ElseIf packageY.Edition = "Enterprise" Then
            iY = 6
          Else
            iY = 7
          End If
          If iX > iY Then
            Return 1
          ElseIf iX < iY Then
            Return -1
          End If
          If CompareArchitectures(packageX.Architecture, ArchitectureList.ia64, False) Then
            If CompareArchitectures(packageY.Architecture, ArchitectureList.amd64, False) Then Return 1
            If CompareArchitectures(packageY.Architecture, ArchitectureList.x86, False) Then Return 1
          End If
          If CompareArchitectures(packageX.Architecture, ArchitectureList.amd64, False) Then
            If CompareArchitectures(packageY.Architecture, ArchitectureList.ia64, False) Then Return -1
            If CompareArchitectures(packageY.Architecture, ArchitectureList.x86, False) Then Return 1
          End If
          If CompareArchitectures(packageX.Architecture, ArchitectureList.x86, False) Then
            If CompareArchitectures(packageY.Architecture, ArchitectureList.ia64, False) Then Return -1
            If CompareArchitectures(packageY.Architecture, ArchitectureList.amd64, False) Then Return -1
          End If
          If packageX.SPLevel > packageY.SPLevel Then Return 1
          If packageX.SPLevel < packageY.SPLevel Then Return -1
          Return Date.Compare(Date.Parse(packageX.Modified.Replace(" - ", " ")), Date.Parse(packageY.Modified.Replace(" - ", " ")))
        Case OrderBy.Architecture
          Dim packageX As ImagePackage = ImagePackage_ListData(xT).Package
          Dim packageY As ImagePackage = ImagePackage_ListData(yT).Package
          Return CompareArchitecturesVal(packageX.Architecture, packageY.Architecture, False)
        Case OrderBy.Size
          Dim packageX As ImagePackage = ImagePackage_ListData(xT).Package
          Dim packageY As ImagePackage = ImagePackage_ListData(yT).Package
          If packageX.Size > packageY.Size Then
            Return 1
          ElseIf packageX.Size < packageY.Size Then
            Return -1
          End If
          Return 0
        Case Else
          Return 0
      End Select
    End Function
  End Class
#End Region
#Region "UEFI"
  Private UEFI_IsEnabled As Boolean
  Private Sub chkUEFI_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUEFI.CheckedChanged
    If isStarting Then Return
    'If UEFI_IsEnabled Then Return
    'If cmbLimitType.SelectedIndex = 0 Then Return
    'Limit_SetValues(cmbLimitType.SelectedIndex)
  End Sub
#End Region
#Region "Limit"
  Private Sub cmbLimitType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbLimitType.SelectedIndexChanged
    If isStarting Then Return
    Limit_SetValues(cmbLimitType.SelectedIndex)
    mySettings.DefaultSplit = cmbLimitType.SelectedIndex
  End Sub
  Private Sub Limit_SetValues(Index As Integer)
    Select Case Index
      Case 0
        cmbLimit.Text = Nothing
        cmbLimit.Enabled = False
      Case 1
        Dim selWas As String = cmbLimit.Text
        cmbLimit.Enabled = True
        cmbLimit.Items.Clear()
        cmbLimit.Items.AddRange(CType({"700 MB (CD)", "4095 MB (32-Bit Limit)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)", "25000 MB (BD)", "50000 MB (Dual-Layer BD)"}, Object()))
        If Not String.IsNullOrEmpty(selWas) Then
          cmbLimit.Text = selWas
        Else
          cmbLimit.SelectedIndex = 2
        End If
      Case 2
        Dim selWas As String = cmbLimit.Text
        cmbLimit.Enabled = True
        cmbLimit.Items.Clear()
        cmbLimit.Items.AddRange(CType({"700 MB (CD)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)", "25000 MB (BD)", "50000 MB (Dual-Layer BD)"}, Object()))
        If selWas = "4095 MB (32-Bit Limit)" Then selWas = Nothing
        If Not String.IsNullOrEmpty(selWas) Then
          cmbLimit.Text = selWas
        Else
          cmbLimit.SelectedIndex = 1
        End If
      Case Else
        cmbLimit.Text = Nothing
        cmbLimit.Enabled = False
    End Select
  End Sub
  Private Sub cmbLimit_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cmbLimit.KeyDown
    If e.KeyCode = Keys.OemPeriod Or e.KeyCode = Keys.Decimal Or e.KeyCode = Keys.Subtract Or e.KeyCode = Keys.OemMinus Then e.SuppressKeyPress = True
  End Sub
  Private Sub cmbLimit_LostFocus(sender As Object, e As System.EventArgs) Handles cmbLimit.LostFocus
    If Not isStarting Then
      Dim iLimit As Long = 1
      Try
        If String.IsNullOrEmpty(cmbLimit.Text) Then
          iLimit = 1
        ElseIf cmbLimit.Text.ToLower.Contains("pb") Then
          iLimit = NumericVal(cmbLimit.Text) * 1024 * 1024 * 1024
        ElseIf cmbLimit.Text.ToLower.Contains("tb") Then
          iLimit = NumericVal(cmbLimit.Text) * 1024 * 1024
        ElseIf cmbLimit.Text.ToLower.Contains("gb") Then
          iLimit = NumericVal(cmbLimit.Text) * 1024
        ElseIf cmbLimit.Text.ToLower.Contains("mb") Then
          Dim limVal As String = cmbLimit.Text
          If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
          iLimit = NumericVal(limVal)
          If iLimit > 0 And cmbLimit.Text.ToLower.Contains(String.Format("{0} mb", iLimit)) Then
            mySettings.SplitVal = cmbLimit.Text
            Return
          End If
        ElseIf cmbLimit.Text.ToLower.Contains("kb") Then
          iLimit = CLng(Math.Ceiling(CSng(NumericVal(cmbLimit.Text)) / 1024.0F))
        ElseIf cmbLimit.Text.ToLower.Contains("b") Then
          iLimit = CLng(Math.Ceiling(CSng(NumericVal(cmbLimit.Text)) / 1024.0F / 1024.0F))
        Else
          iLimit = NumericVal(cmbLimit.Text)
        End If
      Catch ex As Exception
        iLimit = 1
      End Try
      If iLimit < 1 Then iLimit = 1
      If cmbLimitType.SelectedIndex = 2 And iLimit < 351 Then iLimit = 351
      cmbLimit.Text = String.Format("{0} MB", iLimit)
      mySettings.SplitVal = cmbLimit.Text
    End If
  End Sub
#End Region
#Region "Control"
  Private Sub cmbPriority_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbPriority.SelectedIndexChanged
    Select Case cmbPriority.SelectedIndex
      Case 0 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.RealTime
      Case 1 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
      Case 2 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
      Case 3 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Normal
      Case 4 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.BelowNormal
      Case 5 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Idle
      Case Else : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Normal
    End Select
    If Not isStarting Then mySettings.Priority = cmbPriority.Text
  End Sub
  Private Sub cmbCompletion_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbCompletion.SelectedIndexChanged
    If cmbCompletion.SelectedIndex = 0 Then
      mySettings.PlayAlertNoise = False
    ElseIf cmbCompletion.SelectedIndex = 1 Then
      mySettings.PlayAlertNoise = True
    End If
  End Sub
  Private Sub cmdOpenFolder_Click(sender As System.Object, e As System.EventArgs) Handles cmdOpenFolder.Click
    Dim sPath As String = Nothing
    If chkISO.Checked Then
      If Not String.IsNullOrEmpty(txtISO.Text) Then sPath = txtISO.Text
    Else
      If Not String.IsNullOrEmpty(txtWIM.Text) Then sPath = txtWIM.Text
    End If
    If Not String.IsNullOrEmpty(sPath) Then
      If IO.Directory.Exists(sPath) Or IO.File.Exists(sPath) Then
        Try
          Process.Start("explorer", String.Format("/select,""{0}""", sPath))
        Catch ex As Exception
          MsgDlg(Me, String.Format("Unable to open the folder for ""{0}""!", sPath), "Unable to open folder.", "Folder was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , ex.Message, "Open Folder Folder Missing")
        End Try
      Else
        MsgDlg(Me, String.Format("Unable to find the file ""{0}""!", sPath), "Unable to find completed Image.", "File was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , , "Open Folder File Missing")
      End If
    End If
  End Sub
  Private Sub cmdConfig_Click(sender As System.Object, e As System.EventArgs) Handles cmdConfig.Click
    Dim fConfig As New frmConfig
    fConfig.ShowDialog(Me)
    fConfig.Dispose()
    mySettings = New MySettings
  End Sub
  Private Sub cmdBegin_Click(sender As System.Object, e As System.EventArgs) Handles cmdBegin.Click
    Slipstream()
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    If cmdClose.Text = "&Close" Then
      Me.Close()
    Else
      If Not RunActivity = ActivityType.Nothing Then
        Dim Activity As ActivityRet = ActivityParser(RunActivity)
        If MsgDlg(Me, String.Format("Do you want to cancel the {0} proceess?", Activity.Process), String.Format("{1} is busy {0}.", Activity.Activity, Application.ProductName), String.Format("Stop {0}?", Activity.Title), MessageBoxButtons.YesNo, _TaskDialogIcon.Question, MessageBoxDefaultButton.Button2, , String.Format("Stop {0}", Activity.Title)) = Windows.Forms.DialogResult.No Then Return
      End If
      StopRun = True
      cmdClose.Enabled = False
      Application.DoEvents()
    End If
  End Sub
#End Region
#Region "Title MNG"
  Private TitleMNG_Image As MNG
  Private TitleMNG_Text As String
  Private TitleMNG_Font As New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular)
  Private TitleMNG_LastDraw As Long
  Private TitleMNG_FrameNumber As UInteger
  Private TitleMNG_FrameCount As UInteger
  Private WithEvents tmrTitleMNG As ThreadedTimer
  Private Delegate Sub TitleMNG_SetTitleInvoker(Title As String, Tooltip As String)
  Private Enum TitleMNG_List
    Move
    Copy
    Delete
  End Enum
  Private Sub tmrTitleMNG_Tick(sender As System.Object, e As System.EventArgs) Handles tmrTitleMNG.Tick
    tmrTitleMNG.Interval = 1000
    TitleMNG_DrawTitle(True)
  End Sub
  Private Function TitleMNG_DefaultTitle() As String
    Dim digits As Integer = 4
    If My.Application.Info.Version.Revision = 0 Then
      digits = 3
      If My.Application.Info.Version.Build = 0 Then
        digits = 2
      End If
    End If
    Return String.Format("{0} {1}", My.Application.Info.ProductName, My.Application.Info.Version.ToString(digits))
  End Function
  Private Sub TitleMNG_SetTitle(Title As String, Tooltip As String)
    If Me.InvokeRequired Then
      Me.Invoke(New TitleMNG_SetTitleInvoker(AddressOf TitleMNG_SetTitle), Title, Tooltip)
      Return
    End If
    TitleMNG_Text = Title
    If String.IsNullOrWhiteSpace(Tooltip) Then
      ttInfo.SetToolTip(pctTitle, Nothing)
    Else
      ttInfo.SetToolTip(pctTitle, Tooltip)
    End If
  End Sub
  Private Sub TitleMNG_SetDisplay(DispType As TitleMNG_List)
    If TitleMNG_Image IsNot Nothing Then TitleMNG_Image = Nothing
    TitleMNG_Image = New MNG
    Dim mngPath As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "slips7ream_anim.mng")
    Select Case DispType
      Case TitleMNG_List.Move
        My.Computer.FileSystem.WriteAllBytes(mngPath, My.Resources.move, False)
      Case TitleMNG_List.Copy
        My.Computer.FileSystem.WriteAllBytes(mngPath, My.Resources.copy, False)
      Case TitleMNG_List.Delete
        My.Computer.FileSystem.WriteAllBytes(mngPath, My.Resources.delete, False)
    End Select
    If IO.File.Exists(mngPath) Then
      If TitleMNG_Image.Load(mngPath) Then
        TitleMNG_FrameNumber = 0
        If TitleMNG_Image.NumEmbeddedPNG > 0 Then
          TitleMNG_FrameCount = CUInt(TitleMNG_Image.NumEmbeddedPNG - 1)
        Else
          TitleMNG_FrameCount = 0
        End If
      Else
        TitleMNG_Image = Nothing
        TitleMNG_FrameNumber = 0
        TitleMNG_FrameCount = 0
      End If
      IO.File.Delete(mngPath)
    End If
  End Sub
  Private Sub TitleMNG_DrawTitle(Optional SkipTitle As Boolean = False)
    If Not SkipTitle Then TitleMNG_SetTitle(TitleMNG_DefaultTitle, String.Format("{0} - Windows 7 Image Slipstream Utility by {1}", My.Application.Info.ProductName, My.Application.Info.CompanyName))
    If TitleMNG_Image Is Nothing Then
      Dim DefaultSize As New Size(270, 40)
      Using bmpDisplay As New Bitmap(pctTitle.Width, DefaultSize.Height)
        Using g As Graphics = Graphics.FromImage(bmpDisplay)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
          Dim FirstRect As New Rectangle(0, 0, pctTitle.Width - DefaultSize.Width, DefaultSize.Height)
          Using firstBrush As New Drawing2D.LinearGradientBrush(FirstRect, Color.FromArgb(15, 95, 180), Color.FromArgb(32, 121, 201), 0)
            g.FillRectangle(firstBrush, FirstRect)
          End Using
          Dim SecondRect As New Rectangle(pctTitle.Width - DefaultSize.Width, 0, DefaultSize.Width, DefaultSize.Height)
          Using secondBrush As New Drawing2D.LinearGradientBrush(SecondRect, Color.FromArgb(32, 121, 201), Color.FromArgb(45, 166, 209), 0)
            g.FillRectangle(secondBrush, SecondRect)
          End Using
          g.DrawString(TitleMNG_Text, TitleMNG_Font, Brushes.Black, New Point(16, 8))
          g.DrawString(TitleMNG_Text, TitleMNG_Font, Brushes.White, New Point(15, 7))
        End Using
        pctTitle.BackgroundImage = CType(bmpDisplay.Clone, Image)
      End Using
      Return
    End If
    If TickCount() - TitleMNG_LastDraw >= 9 Then
      TitleMNG_FrameNumber += 1UI
      If TitleMNG_FrameNumber > TitleMNG_FrameCount Then TitleMNG_FrameNumber = 0
      TitleMNG_LastDraw = TickCount()
    End If
    If TitleMNG_FrameNumber > Integer.MaxValue Then TitleMNG_FrameNumber = 0
    Dim bmpFrame As Bitmap = TitleMNG_Image.ToBitmap(CInt(TitleMNG_FrameNumber))
    If bmpFrame Is Nothing Then Return
    Using bmpDisplay As New Bitmap(pctTitle.Width, bmpFrame.Height)
      Using g As Graphics = Graphics.FromImage(bmpDisplay)
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        Dim FirstRect As New Rectangle(0, 0, pctTitle.Width - bmpFrame.Width - 48, bmpFrame.Height)
        Using firstBrush As New Drawing2D.LinearGradientBrush(FirstRect, Color.FromArgb(15, 95, 180), Color.FromArgb(32, 121, 201), 0)
          g.FillRectangle(firstBrush, FirstRect)
        End Using
        Dim SecondRect As New Rectangle(pctTitle.Width - bmpFrame.Width - 48, 0, 48, bmpFrame.Height)
        Dim EdgeColor As Color = bmpFrame.GetPixel(0, 20)
        Using secondBrush As New Drawing2D.LinearGradientBrush(SecondRect, Color.FromArgb(32, 121, 201), EdgeColor, 0)
          g.FillRectangle(secondBrush, SecondRect)
        End Using
        g.DrawImage(bmpFrame, New Point(pctTitle.Width - bmpFrame.Width, 0))
        g.DrawString(TitleMNG_Text, TitleMNG_Font, Brushes.Black, New Point(16, 8))
        g.DrawString(TitleMNG_Text, TitleMNG_Font, Brushes.White, New Point(15, 7))
      End Using
      pctTitle.BackgroundImage = CType(bmpDisplay.Clone, Image)
    End Using
  End Sub
#End Region
#Region "Status"
  Private Status_Text As String
  Private Status_ShowingToolTip As Boolean
  Private Delegate Sub Status_SetTextInvoker(Message As String)
  Private Function Status_GetText() As String
    If Me.InvokeRequired Then
      Return CStr(Me.Invoke(New MethodInvoker(AddressOf Status_GetText)))
    Else
      Return Status_Text
    End If
  End Function
  Private Sub Status_SetText(Message As String)
    If Me.InvokeRequired Then
      Me.Invoke(New Status_SetTextInvoker(AddressOf Status_SetText), Message)
      Return
    End If
    Status_SetActivityText(Message)
    If Message.Contains("...") And cmdBegin.Visible Then
      lblWIM.Enabled = False
      txtWIM.Enabled = False
      cmdWIM.Enabled = False
      cmdBegin.Visible = False
      chkMerge.Enabled = False
      txtMerge.Enabled = False
      cmdMerge.Enabled = False
    ElseIf Not Message.Contains("...") And Not cmdBegin.Visible Then
      lblWIM.Enabled = True
      txtWIM.Enabled = True
      cmdWIM.Enabled = True
      cmdBegin.Visible = True
      chkMerge.Enabled = True
      txtMerge.Enabled = chkMerge.Checked
      cmdMerge.Enabled = chkMerge.Checked
    End If
    If Status_ShowingToolTip Then
      Status_HideToolTip()
      Status_ShowToolTip()
    End If
    Application.DoEvents()
  End Sub
  Private Sub Status_SetActivityText(Message As String)
    Status_Text = Message
    If String.IsNullOrEmpty(Message) Then
      lblActivity.Text = Nothing
      Return
    End If
    If lblActivity.Width < 16 Then
      lblActivity.Text = Message
      Return
    End If
    Dim expectedW As Integer = 0
    Dim chars As Integer = 0
    Dim lines As Integer = 0
    Using g As Graphics = Graphics.FromImage(New Bitmap(1, 1))
      expectedW = CInt(g.MeasureString(Message, lblActivity.Font, lblActivity.Size, New StringFormat(StringFormatFlags.NoWrap), chars, lines).Width) - 8
    End Using
    If chars < Message.Length Then
      lblActivity.Text = String.Concat(Message.Substring(0, chars - 3).TrimEnd, "...")
    Else
      lblActivity.Text = Message
    End If
  End Sub
  Private Sub Status_ShowToolTip()
    If String.IsNullOrEmpty(Status_GetText) Then Return
    If Not Status_GetText() = lblActivity.Text Then
      ttActivity.Show(Status_GetText, lblActivity, -3, -2, Integer.MaxValue)
    End If
  End Sub
  Private Sub Status_HideToolTip()
    ttActivity.Hide(lblActivity)
  End Sub
  Private Sub lblActivity_SizeChanged(sender As Object, e As System.EventArgs) Handles lblActivity.SizeChanged
    Status_SetActivityText(Status_GetText)
  End Sub
  Private Sub lblActivity_MouseEnter(sender As Object, e As System.EventArgs) Handles lblActivity.MouseEnter
    Status_ShowingToolTip = True
    Status_ShowToolTip()
  End Sub
  Private Sub lblActivity_MouseLeave(sender As Object, e As System.EventArgs) Handles lblActivity.MouseLeave
    Status_HideToolTip()
    Status_ShowingToolTip = False
  End Sub
#End Region
#Region "Progress Bars"
#Region "Total"
  Private Progress_Total_Value, Progress_Total_Maximum As Integer
  Private Delegate Sub Progress_TotalInvoker(Value As Integer, Maximum As Integer)
  Friend Sub Progress_Total(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.Invoke(New Progress_TotalInvoker(AddressOf Progress_Total), Value, Maximum)
      Return
    End If
    If Value = 0 Then
      Progress_Total_Value = 0
      If Maximum < 1 Then Maximum = 1
      Progress_Total_Maximum = Maximum
      Progress_Total_Set(Progress_Total_Value, Progress_Total_Maximum)
      Return
    End If
    If Value > Maximum Then
      Progress_Total_Value = Maximum
      Progress_Total_Maximum = Maximum
      Progress_Total_Set(Progress_Total_Value - 1, Progress_Total_Maximum)
      Return
    End If
    If (pbIndividual.Value = 0) Or (pbIndividual.Style = ProgressBarStyle.Marquee) Or (pbIndividual.Value >= pbIndividual.Maximum) Then
      Progress_Total_Value = Value
      Progress_Total_Maximum = Maximum
      Progress_Total_Set(Progress_Total_Value - 1, Progress_Total_Maximum)
      Return
    End If
    If Progress_Total_Value > 0 Then
      Dim denominator As Integer = Progress_Total_Maximum * pbIndividual.Maximum
      Dim numerator As Integer = ((Progress_Total_Value - 1) * pbIndividual.Maximum) + pbIndividual.Value
      Progress_Total_Set(numerator, denominator)
    Else
      If Maximum < 1 Then Maximum = 1
      Progress_Total_Set(Value, Maximum)
    End If
  End Sub
  Private Sub Progress_Total_Set(Value As Integer, Maximum As Integer)
    pbTotal.Maximum = 10000
    ' TODO: Remove this line
    If Value > 0 AndAlso pbTotal.Value > CInt(Value / Maximum * 10000) Then Debug.Print("Total Progress Going DOWN!")
    pbTotal.Value = CInt(Value / Maximum * 10000)
    Dim sTT As String = String.Format("Total Progress: {0}", FormatPercent(Value / Maximum, 1, TriState.True, TriState.False, TriState.False))
    If Not ttInfo.GetToolTip(pbTotal) = sTT Then
      ' TODO: Remove from here to Debug.Print
      'Dim myTrace As New System.Diagnostics.StackTrace
      'Dim sTrace As String = Nothing
      'For I As Integer = 1 To myTrace.FrameCount - 1
      '  If myTrace.GetFrame(I).GetMethod.Module.Name.ToLower = IO.Path.ChangeExtension(My.Application.Info.AssemblyName, "exe").ToLower Then
      '    If String.IsNullOrEmpty(sTrace) Then
      '      sTrace = myTrace.GetFrame(I).GetMethod.Name
      '    Else
      '      sTrace = String.Concat(myTrace.GetFrame(I).GetMethod.Name, ">", sTrace)
      '    End If
      '  End If
      'Next
      'Debug.Print(sTT & " called from " & sTrace)
      ttInfo.SetToolTip(pbTotal, sTT)
    End If
    Try
      If taskBar IsNot Nothing Then
        If Value > 0 Then
          taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
          taskBar.SetProgressValue(Me.Handle, CULng(pbTotal.Value), CULng(pbTotal.Maximum))
        Else
          taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
        End If
      End If
    Catch ex As Exception
    End Try
    Application.DoEvents()
  End Sub
#End Region
#Region "Progress"
  Private Progress_Normal_Value, Progress_Normal_Maximum As Integer
  Private Delegate Sub Progress_NormalInvoker(Value As Integer, Maximum As Integer, GonnaSub As Boolean)
  Private Delegate Sub Progress_Normal_SubInvoker(Value As Integer, Maximum As Integer, ActionTitle As String)
  Friend Sub Progress_Normal(Value As Integer, Maximum As Integer, Optional GonnaSub As Boolean = False)
    If Me.InvokeRequired Then
      Me.Invoke(New Progress_NormalInvoker(AddressOf Progress_Normal), Value, Maximum, GonnaSub)
      Return
    End If
    If Value = 0 And Maximum = 0 Then
      pbIndividual.Style = ProgressBarStyle.Marquee
      Progress_Normal_Value = 0
      Progress_Normal_Maximum = 1
      Progress_Normal_Set(Progress_Normal_Value, Progress_Normal_Maximum)
    ElseIf Value <= Maximum Then
      pbIndividual.Style = ProgressBarStyle.Continuous
      Progress_Normal_Value = Value
      Progress_Normal_Maximum = Maximum
      If GonnaSub Then
        Progress_Normal_Set(Progress_Normal_Value - 1, Progress_Normal_Maximum)
      Else
        Progress_Normal_Set(Progress_Normal_Value, Progress_Normal_Maximum)
      End If
    Else
      pbIndividual.Style = ProgressBarStyle.Continuous
      Progress_Normal_Value = 0
      Progress_Normal_Maximum = Maximum
      Progress_Normal_Set(Progress_Normal_Value, Progress_Normal_Maximum)
    End If
    If Not GonnaSub And Progress_Total_Value > 0 And Progress_Normal_Value > 0 Then
      Dim denominator As Integer = Progress_Total_Maximum * Progress_Normal_Maximum
      Dim numerator As Integer = ((Progress_Total_Value - 1) * Progress_Normal_Maximum) + Progress_Normal_Value
      Progress_Total_Set(numerator, denominator)
    End If
  End Sub
  Friend Sub Progress_Normal_Sub(Value As Integer, Maximum As Integer, ActionTitle As String)
    If Me.InvokeRequired Then
      If Me.Disposing Or Me.IsDisposed Then Return
      Me.Invoke(New Progress_Normal_SubInvoker(AddressOf Progress_Normal_Sub), Value, Maximum, ActionTitle)
      Return
    End If
    If Progress_Normal_Value > 0 And Progress_Normal_Maximum > 1 Then
      Dim ActionPercent As String = "Indeterminate"
      If Maximum > 0 Then ActionPercent = FormatPercent(Value / Maximum, 1, TriState.True, TriState.False, TriState.False)
      Dim denominator As Integer = Progress_Normal_Maximum * Maximum
      Dim numerator As Integer = ((Progress_Normal_Value - 1) * Maximum) + Value
      If denominator < 1 Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        numerator = 0
        denominator = 1
      Else
        pbIndividual.Style = ProgressBarStyle.Continuous
      End If
      Progress_Normal_Set(numerator, denominator, ActionTitle, ActionPercent)
      If Progress_Total_Value > 0 And numerator > 0 Then
        Dim tDenominator As Integer = Progress_Total_Maximum * denominator
        Dim tNumerator As Integer = ((Progress_Total_Value - 1) * denominator) + numerator
        Progress_Total_Set(tNumerator, tDenominator)
      End If
    Else
      Dim tmpProgVal As Integer = 0
      Dim tmpProgMax As Integer = 1
      If Value = 0 And Maximum = 0 Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        tmpProgVal = 0
        tmpProgMax = 1
        Progress_Normal_Set(tmpProgVal, tmpProgMax)
      ElseIf Value <= Maximum Then
        pbIndividual.Style = ProgressBarStyle.Continuous
        tmpProgVal = Value
        tmpProgMax = Maximum
        Progress_Normal_Set(tmpProgVal, tmpProgMax)
      Else
        pbIndividual.Style = ProgressBarStyle.Continuous
        tmpProgVal = 0
        tmpProgMax = Maximum
        Progress_Normal_Set(tmpProgVal, tmpProgMax)
      End If
      If Progress_Total_Value > 0 And tmpProgVal > 0 Then
        Dim denominator As Integer = Progress_Total_Maximum * tmpProgMax
        Dim numerator As Integer = ((Progress_Total_Value - 1) * tmpProgMax) + tmpProgVal
        Progress_Total_Set(numerator, denominator)
      End If
    End If
  End Sub
  Private Sub Progress_Normal_Set(Value As Integer, Maximum As Integer, Optional SubTitle As String = Nothing, Optional SubPercent As String = Nothing)
    pbIndividual.Maximum = 10000
    ' TODO: Remove this line
    If Value > 0 AndAlso pbIndividual.Value > CInt(Value / Maximum * 10000) Then Debug.Print("      Progress Going DOWN!")
    pbIndividual.Value = CInt(Value / Maximum * 10000)
    Dim sProgress As String = "Indeterminate"
    If Not (Value = 0 And Maximum = 1 And pbIndividual.Style = ProgressBarStyle.Marquee) Then sProgress = FormatPercent(Value / Maximum, 1, TriState.True, TriState.False, TriState.False)
    Dim sTT As String = String.Format("Progress: {0}", sProgress)
    If Not String.IsNullOrEmpty(SubTitle) And Not String.IsNullOrEmpty(SubPercent) Then sTT = String.Concat(sTT, vbNewLine, String.Format("{0}: {1}", SubTitle, SubPercent))
    If Not ttInfo.GetToolTip(pbIndividual) = sTT Then
      ' TODO: Remove from here to Debug.Print
      'Dim myTrace As New System.Diagnostics.StackTrace
      'Dim sTrace As String = Nothing
      'For I As Integer = 1 To myTrace.FrameCount - 1
      '  If myTrace.GetFrame(I).GetMethod.Module.Name.ToLower = IO.Path.ChangeExtension(My.Application.Info.AssemblyName, "exe").ToLower Then
      '    If String.IsNullOrEmpty(sTrace) Then
      '      sTrace = myTrace.GetFrame(I).GetMethod.Name
      '    Else
      '      sTrace = String.Concat(myTrace.GetFrame(I).GetMethod.Name, ">", sTrace)
      '    End If
      '  End If
      'Next
      'Debug.Print("      " & sTT.Replace(vbNewLine, " - ") & " called from " & sTrace)
      ttInfo.SetToolTip(pbIndividual, sTT)
    End If
  End Sub
#End Region
#End Region
#Region "Console Output"
  Private Const HeightDifferentialA As Integer = 126
  Private Const HeightDifferentialB As Integer = 22
  Private ConsoleOutput_WindowMode As Boolean = False
  Private ConsoleOutput_WindowTearFrom As Point = Point.Empty
  Private ConsoleOutput_WindowMoving As Boolean = False
  Private Delegate Sub ConsoleOutput_WriteInvoker(Message As String, MsgType As ConsoleOutput_MessageType)
  Private Enum ConsoleOutput_MessageType
    Command
    Output
  End Enum
  Private Sub ConsoleOutput_Write(Message As String, Optional MsgType As ConsoleOutput_MessageType = ConsoleOutput_MessageType.Command)
    If Me.InvokeRequired Then
      Me.Invoke(New ConsoleOutput_WriteInvoker(AddressOf ConsoleOutput_Write), Message, MsgType)
      Return
    End If
    Try
      Dim tOutput As TextBox = txtOutput
      If ConsoleOutput_WindowMode Then tOutput = frmOutput.txtOutput
      If String.IsNullOrEmpty(Message) Then
        tOutput.AppendText(vbNewLine)
      Else
        If tOutput.TextLength > 1000000 Then
          Dim sClipped As String = tOutput.Text
          sClipped = sClipped.Substring(sClipped.IndexOf(vbNewLine & vbNewLine, 150000) + 4)
          tOutput.Text = sClipped
        End If
        If MsgType = ConsoleOutput_MessageType.Command Then
          If tOutput.Text.Contains(vbNewLine) Then
            Dim outputSplit() As String = Split(tOutput.Text, vbNewLine)
            If outputSplit.Length > 1 Then
              If outputSplit(outputSplit.Length - 2).Length > 2 AndAlso outputSplit(outputSplit.Length - 2).Substring(0, 3) = "   " Then
                tOutput.AppendText(String.Concat(vbNewLine, vbNewLine, Message, vbNewLine))
              Else
                tOutput.AppendText(String.Concat(vbNewLine, Message, vbNewLine))
              End If
            Else
              tOutput.AppendText(String.Concat(vbNewLine, Message, vbNewLine))
            End If
          Else
            tOutput.AppendText(String.Concat(Message, vbNewLine))
          End If
        Else
          tOutput.AppendText(String.Concat("   ", Message, vbNewLine))
        End If
      End If
    Catch ex As Exception
    End Try
  End Sub
  Friend Sub ConsoleOutput_ReturnFromWindow()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ConsoleOutput_ReturnFromWindow))
      Return
    End If
    ConsoleOutput_WindowMoving = False
    ConsoleOutput_WindowTearFrom = Point.Empty
    If Not ConsoleOutput_WindowMode Then Return
    ConsoleOutput_WindowMode = False
    pnlSlips7ream.SuspendLayout()
    Me.Height += HeightDifferentialA
    Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height + HeightDifferentialA)
    pctOutputTear.Visible = True
    txtOutput.Visible = True
    txtOutput.Text = frmOutput.txtOutput.Text
    If txtOutput.Text.Length > 0 Then
      txtOutput.SelectionStart = txtOutput.Text.Length - 1
      txtOutput.SelectionLength = 0
      txtOutput.ScrollToCaret()
    End If
    pnlSlips7ream.ResumeLayout(True)
    frmOutput.Hide()
  End Sub
#Region "Expander Button"
  Private Sub expOutput_Closed(sender As Object, e As System.EventArgs) Handles expOutput.Closed
    ttInfo.Hide(expOutput)
    ttInfo.Hide(expOutput.pctExpander)
    ttInfo.SetToolTip(expOutput, "Show Output Console.")
    ttInfo.SetToolTip(expOutput.pctExpander, "Show Output Console.")
    If ConsoleOutput_WindowMode Then
      txtOutput.Text = frmOutput.txtOutput.Text
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
      frmOutput.Hide()
      ConsoleOutput_WindowMode = False
    Else
      pnlSlips7ream.SuspendLayout()
      pctOutputTear.Visible = False
      txtOutput.Visible = False
      Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
      Dim newHeight As Integer = Me.Height - HeightDifferentialA
      Do Until Me.Height <= newHeight + 4
        Dim expectedHeight As Integer = Me.Height - 4
        Me.Height -= 4
        If Not Me.Height = expectedHeight Then Exit Do
      Loop
      Me.Height = newHeight
      pnlSlips7ream.ResumeLayout(True)
      frmMain_Resize(Me, New EventArgs)
    End If
  End Sub
  Private Sub expOutput_Opened(sender As System.Object, e As System.EventArgs) Handles expOutput.Opened
    ttInfo.Hide(expOutput)
    ttInfo.Hide(expOutput.pctExpander)
    ttInfo.SetToolTip(expOutput, "Hide Output Console.")
    ttInfo.SetToolTip(expOutput.pctExpander, "Hide Output Console.")
    If ConsoleOutput_WindowMode Then
      frmOutput.Show(Me)
      frmOutput.Location = New Point(Me.Left, Me.Bottom)
      frmOutput.txtOutput.Text = txtOutput.Text
      If frmOutput.txtOutput.Text.Length > 0 Then
        frmOutput.txtOutput.SelectionStart = frmOutput.txtOutput.Text.Length - 1
        frmOutput.txtOutput.SelectionLength = 0
        frmOutput.txtOutput.ScrollToCaret()
      End If
    Else
      pnlSlips7ream.SuspendLayout()
      Dim newHeight As Integer = Me.Height + HeightDifferentialA
      Do Until Me.Height >= newHeight - 4
        Dim expectedHeight As Integer = Me.Height + 4
        Me.Height += 4
        If Not Me.Height = expectedHeight Then Exit Do
      Loop
      Me.Height = newHeight
      Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height + HeightDifferentialA)
      pctOutputTear.Visible = True
      txtOutput.Visible = True
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
      pnlSlips7ream.ResumeLayout(True)
      ConsoleOutput_Title_Redraw()
      frmMain_Resize(Me, New EventArgs)
    End If
  End Sub
#End Region
#Region "Title Bar"
  Private Sub ConsoleOutput_Title_Redraw()
    Using bTitle As New Bitmap(pctOutputTear.Width, pctOutputTear.Height)
      Using g As Graphics = Graphics.FromImage(bTitle)
        g.Clear(Color.Transparent)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        If ActiveForm Is Me Then
          g.FillRectangle(New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(pctOutputTear.Width, 0), SystemColors.ActiveCaption, SystemColors.GradientActiveCaption), pctOutputTear.DisplayRectangle)
        Else
          g.FillRectangle(New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(pctOutputTear.Width, 0), SystemColors.InactiveCaption, SystemColors.GradientInactiveCaption), pctOutputTear.DisplayRectangle)
        End If
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        g.DrawString(String.Format("{0} Output Console", Application.ProductName), SystemFonts.SmallCaptionFont, SystemBrushes.ActiveCaptionText, New RectangleF(3, 1, pctOutputTear.Width - 6, pctOutputTear.Height - 2), New StringFormat(StringFormatFlags.NoWrap Or StringFormatFlags.NoClip))
      End Using
      pctOutputTear.Image = CType(bTitle.Clone, Image)
    End Using
  End Sub
  Private Sub pctOutputTear_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctOutputTear.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Left Then
      ConsoleOutput_WindowTearFrom = e.Location
    End If
  End Sub
  Private Sub pctOutputTear_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctOutputTear.MouseMove
    If ConsoleOutput_WindowMoving Then
      Dim newX As Integer = MousePosition.X - ConsoleOutput_WindowTearFrom.X
      Dim newY As Integer = MousePosition.Y - ConsoleOutput_WindowTearFrom.Y
      frmOutput.Location = New Point(newX, newY)
    End If
    If ConsoleOutput_WindowMode Then Return
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Not pctOutputTear.DisplayRectangle.Contains(e.Location) Then
        ConsoleOutput_WindowMode = True
        ConsoleOutput_WindowMoving = True
        pnlSlips7ream.SuspendLayout()
        pctOutputTear.Visible = False
        txtOutput.Visible = False
        Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
        Me.Height -= HeightDifferentialA
        pnlSlips7ream.ResumeLayout(True)
        frmOutput.Show(Me)
        Dim newX As Integer = MousePosition.X - ConsoleOutput_WindowTearFrom.X
        Dim newY As Integer = MousePosition.Y - ConsoleOutput_WindowTearFrom.Y
        frmOutput.Location = New Point(newX, newY)
        frmOutput.Activate()
        frmOutput.txtOutput.Text = txtOutput.Text
        If frmOutput.txtOutput.Text.Length > 0 Then
          frmOutput.txtOutput.SelectionStart = frmOutput.txtOutput.Text.Length - 1
          frmOutput.txtOutput.SelectionLength = 0
          frmOutput.txtOutput.ScrollToCaret()
        End If
      End If
    End If
  End Sub
  Private Sub pctOutputTear_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctOutputTear.MouseUp
    If Not ConsoleOutput_WindowTearFrom.IsEmpty Then
      ConsoleOutput_WindowMoving = False
      ConsoleOutput_WindowTearFrom = Point.Empty
      frmOutput.DoResize()
    End If
  End Sub
#End Region
#Region "Menus"
  Private Sub mnuOutput_Popup(sender As System.Object, e As System.EventArgs) Handles mnuOutput.Popup
    Dim mParent As ContextMenu = CType(sender, ContextMenu)
    Dim txtSel As TextBox = CType(mParent.SourceControl, TextBox)
    mnuSelectAll.Enabled = Not txtSel.TextLength = 0
    mnuClear.Enabled = Not txtSel.TextLength = 0
    mnuCopy.Enabled = Not txtSel.SelectedText.Length = 0
    If Not txtSel.SelectedText.Length = 0 Then
      Dim sSelFrom As Integer = txtSel.SelectionStart
      Dim StartLastNewLine As Integer = -1
      If txtSel.Text.Substring(txtSel.SelectionStart).Contains(vbNewLine) Then
        StartLastNewLine = txtSel.Text.IndexOf(vbNewLine, txtSel.SelectionStart)
      Else
        StartLastNewLine = txtSel.Text.Length
      End If
      Dim StartNewLine As Integer = -1
      If txtSel.Text.Substring(0, StartLastNewLine).Contains(vbNewLine) Then
        StartNewLine = txtSel.Text.Substring(0, StartLastNewLine).LastIndexOf(vbNewLine)
      Else
        StartNewLine = 0
      End If
      sSelFrom = StartNewLine
      Dim sSelTo As Integer = txtSel.SelectionStart + txtSel.SelectionLength
      If txtSel.Text.Substring(txtSel.SelectionStart + txtSel.SelectionLength).Contains(vbNewLine) Then
        sSelTo = txtSel.Text.IndexOf(vbNewLine, txtSel.SelectionStart + txtSel.SelectionLength)
      ElseIf txtSel.Text.Substring(0, txtSel.SelectionStart + txtSel.SelectionLength).Contains(vbNewLine) Then
        If txtSel.Text.Length >= txtSel.SelectionStart + txtSel.SelectionLength Then
          sSelTo = txtSel.Text.Length
        Else
          sSelTo = txtSel.Text.Substring(0, txtSel.SelectionStart + txtSel.SelectionLength).LastIndexOf(vbNewLine)
        End If
      Else
        sSelTo = txtSel.Text.Length
      End If
      Dim sText As String = txtSel.Text.Substring(sSelFrom, sSelTo - sSelFrom)
      Do While sText.Contains(String.Concat(vbNewLine, vbNewLine))
        sText = sText.Replace(String.Concat(vbNewLine, vbNewLine), vbNewLine)
      Loop
      sText = sText.Trim
      Dim sLines() As String = Split(sText, vbNewLine)
      sText = Nothing
      For I As Integer = 0 To sLines.Length - 1
        If String.IsNullOrEmpty(sLines(I)) Then Continue For
        If sLines(I).StartsWith("   ") Then Continue For
        sText = String.Concat(sText, sLines(I), vbNewLine)
      Next
      If String.IsNullOrEmpty(sText) Then
        mnuCopyCommands.Enabled = False
      Else
        mnuCopyCommands.Enabled = True
      End If
    Else
      mnuCopyCommands.Enabled = False
    End If
  End Sub
  Private Sub mnuCopy_Click(sender As System.Object, e As System.EventArgs) Handles mnuCopy.Click
    Dim mSender As MenuItem = CType(sender, MenuItem)
    Dim mParent As ContextMenu = CType(mSender.Parent, ContextMenu)
    Dim txtSel As TextBox = CType(mParent.SourceControl, TextBox)
    If Not txtSel.SelectedText.Length = 0 Then Clipboard.SetText(txtSel.SelectedText)
  End Sub
  Private Sub mnuCopyCommands_Click(sender As System.Object, e As System.EventArgs) Handles mnuCopyCommands.Click
    Dim mSender As MenuItem = CType(sender, MenuItem)
    Dim mParent As ContextMenu = CType(mSender.Parent, ContextMenu)
    Dim txtSel As TextBox = CType(mParent.SourceControl, TextBox)
    If Not txtSel.SelectedText.Length = 0 Then
      Dim sSelFrom As Integer = txtSel.SelectionStart
      Dim StartLastNewLine As Integer = -1
      If txtSel.Text.Substring(txtSel.SelectionStart).Contains(vbNewLine) Then
        StartLastNewLine = txtSel.Text.IndexOf(vbNewLine, txtSel.SelectionStart)
      Else
        StartLastNewLine = txtSel.Text.Length
      End If
      Dim StartNewLine As Integer = -1
      If txtSel.Text.Substring(0, StartLastNewLine).Contains(vbNewLine) Then
        StartNewLine = txtSel.Text.Substring(0, StartLastNewLine).LastIndexOf(vbNewLine)
      Else
        StartNewLine = 0
      End If
      sSelFrom = StartNewLine
      Dim sSelTo As Integer = txtSel.SelectionStart + txtSel.SelectionLength
      If txtSel.Text.Substring(txtSel.SelectionStart + txtSel.SelectionLength).Contains(vbNewLine) Then
        sSelTo = txtSel.Text.IndexOf(vbNewLine, txtSel.SelectionStart + txtSel.SelectionLength)
      ElseIf txtSel.Text.Substring(0, txtSel.SelectionStart + txtSel.SelectionLength).Contains(vbNewLine) Then
        If txtSel.Text.Length >= txtSel.SelectionStart + txtSel.SelectionLength Then
          sSelTo = txtSel.Text.Length
        Else
          sSelTo = txtSel.Text.Substring(0, txtSel.SelectionStart + txtSel.SelectionLength).LastIndexOf(vbNewLine)
        End If
      Else
        sSelTo = txtSel.Text.Length
      End If
      Dim sText As String = txtSel.Text.Substring(sSelFrom, sSelTo - sSelFrom)
      Do While sText.Contains(String.Concat(vbNewLine, vbNewLine))
        sText = sText.Replace(String.Concat(vbNewLine, vbNewLine), vbNewLine)
      Loop
      sText = sText.Trim
      Dim sLines() As String = Split(sText, vbNewLine)
      sText = Nothing
      For I As Integer = 0 To sLines.Length - 1
        If String.IsNullOrEmpty(sLines(I)) Then Continue For
        If sLines(I).StartsWith("   ") Then Continue For
        sText = String.Concat(sText, sLines(I), vbNewLine)
      Next
      If String.IsNullOrEmpty(sText) Then Return
      Clipboard.SetText(sText.TrimEnd)
    End If
  End Sub
  Private Sub mnuClear_Click(sender As System.Object, e As System.EventArgs) Handles mnuClear.Click
    Dim mSender As MenuItem = CType(sender, MenuItem)
    Dim mParent As ContextMenu = CType(mSender.Parent, ContextMenu)
    Dim txtSel As TextBox = CType(mParent.SourceControl, TextBox)
    txtSel.Text = ""
  End Sub
  Private Sub mnuSelectAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuSelectAll.Click
    Dim mSender As MenuItem = CType(sender, MenuItem)
    Dim mParent As ContextMenu = CType(mSender.Parent, ContextMenu)
    Dim txtSel As TextBox = CType(mParent.SourceControl, TextBox)
    If Not txtSel.TextLength = 0 Then
      txtSel.SelectionStart = 0
      txtSel.SelectionLength = txtSel.TextLength
      txtSel.Focus()
    End If
  End Sub
#End Region
#End Region
#Region "Donations"
  Private Sub Donate_Show()
    Try
      If Math.Abs(DateDiff(DateInterval.Minute, Process.GetCurrentProcess.StartTime, Now)) > 30 Then
        If Now.Month = 5 Or Now.Month = 9 Or Now.Month = 12 Then
          If Now.DayOfWeek = DayOfWeek.Saturday Or Now.DayOfWeek = DayOfWeek.Sunday Then
            Dim lastAsk As Long = DateDiff(DateInterval.Month, mySettings.LastNag, Now)
            If lastAsk > 6 Or lastAsk < -24 Then
              mySettings.LastNag = Today
              frmDonate.Show()
            End If
          End If
        End If
      End If
    Catch
    End Try
  End Sub
  Friend Sub Donate_Clicked()
    Try
      mySettings.LastNag = DateAdd(DateInterval.Month, 24, Today)
    Catch ex As Exception
    End Try
  End Sub
#End Region
#End Region
#Region "Command Calls"
  Private Function CleanMounts(IndependentProgress As Boolean) As Boolean
    Try
      Dim pbVal As Integer = 0
      Dim pbMax As Integer = 8
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      If My.Computer.Registry.LocalMachine.GetSubKeyNames.Contains(My.Application.Info.ProductName) Then
        Dim hkPath As String = IO.Path.Combine("HKLM", My.Application.Info.ProductName)
        ConsoleOutput_Write(String.Concat("REG unload ", hkPath))
        Run_WithReturn("reg", String.Concat("unload ", hkPath), True)
      End If
      Status_SetText("Getting DISM Mount List...")
      Dim Args As String = "/Get-MountedWimInfo"
      ConsoleOutput_Write(String.Format("DISM {0}", Args))
      Dim DISMInfo As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args), True)
      Dim mFindA As String = WorkDir.ToLower
      Dim mFindB As String = ShortenPath(mFindA).ToLower
      Dim sFind As New List(Of String)
      If Not DISMInfo.Contains("No mounted images found.") Then
        Dim sLines() As String = Split(DISMInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Dir : ") Then
            Dim tmpPath As String = line.Substring(line.IndexOf(":") + 2)
            If line.ToLower.Contains(mFindA) Or line.ToLower.Contains(mFindB) Then sFind.Add(tmpPath)
          End If
        Next
      End If
      pbVal += 1
      If sFind.Count > 0 Then
        Status_SetText("Cleaning old DISM Mounts...")
        For I As Integer = 0 To sFind.Count - 1
          If IndependentProgress Then
            Progress_Normal(I, sFind.Count - 1, True)
            Progress_Total(pbVal, pbMax)
          Else
            Progress_Normal((pbVal - 1) * sFind.Count, pbMax * sFind.Count, True)
          End If
          DISM_Image_Discard(sFind(I))
        Next
      End If
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      Status_SetText("Cleaning any corrupted DISM Mounts...")
      Args = "/Cleanup-Wim"
      ConsoleOutput_Write(String.Format("DISM {0}", Args))
      Run_Hidden(DismPath, Args)
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      Status_SetText("Getting ImageX Mount List...")
      Args = "/unmount"
      ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
      Dim ImageXInfo As String = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args, True)
      sFind = New List(Of String)
      If Not ImageXInfo.Contains("Number of Mounted Images: 0") Then
        Dim sLines() As String = Split(ImageXInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Path") Then
            Dim tmpPath As String = line.Substring(line.IndexOf("[") + 1)
            tmpPath = tmpPath.Substring(0, tmpPath.IndexOf("]"))
            If tmpPath.ToLower.Contains(mFindA) Or tmpPath.ToLower.Contains(mFindB) Then
              sFind.Add(tmpPath)
            End If
          End If
        Next
      End If
      pbVal += 1
      If sFind.Count > 0 Then
        Status_SetText("Cleaning Old ImageX Mounts...")
        For I As Integer = 0 To sFind.Count - 1
          If IndependentProgress Then
            Progress_Normal(I, sFind.Count - 1, True)
            Progress_Total(pbVal, pbMax)
          Else
            Progress_Normal((pbVal - 1) * sFind.Count, pbMax * sFind.Count, True)
          End If
          Args = String.Format("/unmount {0}", ShortenPath(sFind(I)))
          ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
          Run_Hidden(IO.Path.Combine(AIKDir, "imagex"), Args)
        Next
      End If
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      Status_SetText("Cleaning any corrupted ImageX Mounts...")
      Args = "/cleanup"
      ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
      Run_Hidden(IO.Path.Combine(AIKDir, "imagex"), Args)
      Dim stillExists As Boolean = False
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      Status_SetText("Deleting Leftover Files...")
      If IO.Directory.Exists(IO.Path.Combine(WorkDir, "MBOOT")) Then
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, "MBOOT")))
        SlowDeleteDirectory(IO.Path.Combine(WorkDir, "MBOOT"), FileIO.DeleteDirectoryOption.DeleteAllContents, True)
        If IO.Directory.Exists(IO.Path.Combine(WorkDir, "MBOOT")) Then stillExists = True
      End If
      pbVal += 1
      If IndependentProgress Then
        Progress_Normal(0, 1)
        Progress_Total(pbVal, pbMax)
      Else
        Progress_Normal(pbVal, pbMax, True)
      End If
      If IO.Directory.Exists(IO.Path.Combine(WorkDir, "MOUNT")) Then
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, "MOUNT")))
        SlowDeleteDirectory(IO.Path.Combine(WorkDir, "MOUNT"), FileIO.DeleteDirectoryOption.DeleteAllContents, True)
        If IO.Directory.Exists(IO.Path.Combine(WorkDir, "MOUNT")) Then stillExists = True
      End If
      Progress_Normal(0, 1)
      Progress_Total(0, 1)
      Return Not stillExists
    Catch ex As Exception
      Progress_Normal(0, 1)
      Progress_Total(0, 1)
      Return False
    End Try
  End Function
#Region "DISM"
  Private DISM_Return_ProgressValue As Integer
  Private DISM_Return_ProgressTotal As Integer
  Private DISM_Return_ProgressStatus As String
  Private Sub DISM_Return_Clean()
    Run_WithReturn_ShowProgress = False
    DISM_Return_ProgressStatus = Nothing
    DISM_Return_ProgressValue = -1
    DISM_Return_ProgressTotal = 0
  End Sub
#Region "Image"
  Private Function DISM_Image_Load(WIMFile As String, WIMIndex As Integer, MountPath As String) As Boolean
    Dim Args As String = String.Format("/Mount-Wim /WimFile:{0} /index:{1} /MountDir:{2}", ShortenPath(WIMFile), WIMIndex, ShortenPath(MountPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Mounting Image"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Image_GetCount(WIMFile As String) As Integer
    Dim Args As String = String.Format("/Get-WimInfo /WimFile:{0}", ShortenPath(WIMFile))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Image"
    DISM_Return_ProgressTotal = 2
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim PackageList As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Dim PackageRows() As String = Split(PackageList, vbNewLine)
    Dim Indexes As Integer = 0
    For Each row In PackageRows
      If row.StartsWith("Index : ") Then
        Indexes += 1
      End If
    Next
    Return Indexes
  End Function
  Private Function DISM_Image_GetData(WIMFile As String, Index As Integer) As ImagePackage
    Dim Args As String = String.Format("/Get-WimInfo /WimFile:{0} /index:{1}", ShortenPath(WIMFile), Index)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Image Package"
    DISM_Return_ProgressTotal = 4
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim PackageList As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    Run_WithReturn_ShowProgress = False
    Dim Info As New ImagePackage(PackageList)
    Args = String.Format("/info ""{0}"" {1}", ShortenPath(WIMFile), Index)
    DISM_Return_ProgressValue += 1
    Progress_Normal_Sub(DISM_Return_ProgressValue, DISM_Return_ProgressTotal, DISM_Return_ProgressStatus)
    ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
    Dim ExtraList As String = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args)
    DISM_Return_Clean()
    Dim extraLines() As String = Split(ExtraList, vbNewLine)
    Dim FoundDName As Boolean = False
    Dim FoundDDesc As Boolean = False
    For Each extraLine In extraLines
      If extraLine.Contains("<DISPLAYNAME>") Then
        Dim sText As String = extraLine
        sText = sText.Substring(sText.IndexOf("<DISPLAYNAME>") + 13)
        If sText.Contains("</") Then sText = sText.Substring(0, sText.IndexOf("</"))
        If Not String.IsNullOrEmpty(sText) Then
          Info.Name = sText
          FoundDName = True
        End If
      End If
      If extraLine.Contains("<DISPLAYDESCRIPTION>") Then
        Dim sText As String = extraLine
        sText = sText.Substring(sText.IndexOf("<DISPLAYDESCRIPTION>") + 20)
        If sText.Contains("</") Then sText = sText.Substring(0, sText.IndexOf("</"))
        If Not String.IsNullOrEmpty(sText) Then
          Info.Desc = sText
          FoundDDesc = True
        End If
      End If
    Next
    If FoundDName And FoundDDesc Then Return Info
    For Each extraLine In extraLines
      If Not FoundDName AndAlso extraLine.Contains("<NAME>") Then
        Dim sText As String = extraLine
        sText = sText.Substring(sText.IndexOf("<NAME>") + 6)
        If sText.Contains("</") Then sText = sText.Substring(0, sText.IndexOf("</"))
        If Not String.IsNullOrEmpty(sText) Then Info.Name = sText
      End If
      If Not FoundDDesc AndAlso extraLine.Contains("<DESCRIPTION>") Then
        Dim sText As String = extraLine
        sText = sText.Substring(sText.IndexOf("<DESCRIPTION>") + 13)
        If sText.Contains("</") Then sText = sText.Substring(0, sText.IndexOf("</"))
        If Not String.IsNullOrEmpty(sText) Then Info.Desc = sText
      End If
    Next
    Return Info
  End Function
  Private Function DISM_Image_Save(MountPath As String) As Boolean
    Dim Args As String = String.Format("/Unmount-Wim /MountDir:{0} /commit", ShortenPath(MountPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Saving Image"
    DISM_Return_ProgressTotal = 4
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Image_Discard(MountPath As String) As Boolean
    Dim Args As String = String.Format("/Unmount-Wim /MountDir:{0} /discard", ShortenPath(MountPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Discarding Image"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    If DISM_Return_ProgressValue > 3 Then Debug.Print("Discard returned " & DISM_Return_ProgressValue)
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
#End Region
#Region "Updates"
  Private Function DISM_Update_Add(MountPath As String, AddPath As String, Optional TryExclusiveFix As Boolean = True) As Boolean
    Dim Args As String = String.Format("/Image:{0} /Add-Package /PackagePath:{1}", ShortenPath(MountPath), ShortenPath(AddPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Integrating Update"
    DISM_Return_ProgressTotal = 4
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    If sRet.ToLower.Contains("0x800f082f") And TryExclusiveFix Then
      Dim hkPath As String = IO.Path.Combine("HKLM", My.Application.Info.ProductName)
      Dim msPath As String = IO.Path.Combine(MountPath, "windows", "system32", "config", "software")
      ConsoleOutput_Write(String.Format("REG load {0} {1} ", hkPath, msPath))
      Dim sLoad As String = Run_WithReturn("reg", String.Format("load {0} {1} ", hkPath, msPath))
      If Not sLoad.Contains("The operation completed successfully.") Then Return False
      ConsoleOutput_Write("Making modifications to virtual registry...")
      Dim sMod As String = clsRegistry.ModifySessionsPending(hkPath)
      If String.IsNullOrEmpty(sMod) Then
        ConsoleOutput_Write("Complete!", ConsoleOutput_MessageType.Output)
      Else
        ConsoleOutput_Write(sMod, ConsoleOutput_MessageType.Output)
      End If
      ConsoleOutput_Write(String.Concat("REG unload ", hkPath))
      Dim sUnload As String = Run_WithReturn("reg", String.Concat("unload ", hkPath))
      If Not sUnload.Contains("The operation completed successfully.") Then Return False
      If Not String.IsNullOrEmpty(sMod) Then Return False
      Return DISM_Update_Add(MountPath, AddPath, False)
    End If
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Update_Remove(MountPath As String, PackageName As String) As Boolean
    Dim Args As String = String.Format("/Image:{0} /Remove-Package /PackageName:{1}", ShortenPath(MountPath), PackageName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Removing Update"
    DISM_Return_ProgressTotal = 4
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))

    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Update_GetList(MountPath As String, Parent As ImagePackage) As List(Of Update_Integrated)
    Dim Args As String = String.Format("/Image:{0} /Get-Packages", ShortenPath(MountPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Update List"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim PackageList As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    If PackageList.Contains("Packages listing:") Then
      If PackageList.Contains("The operation completed successfully.") Then
        PackageList = PackageList.Substring(PackageList.IndexOf("Packages listing:") + 21)
        PackageList = PackageList.Substring(0, PackageList.IndexOf("The operation completed successfully.") - 4)
        Dim PackageItems() As String = Split(PackageList, String.Concat(vbNewLine, vbNewLine))
        Dim pList As New List(Of Update_Integrated)
        For Each item As String In PackageItems
          pList.Add(New Update_Integrated(Parent, item))
        Next
        Return pList
      End If
    End If
    Return Nothing
  End Function
  Private Sub DISM_Update_GetData(MountPath As String, ByRef Package As Update_Integrated)
    Dim Args As String = String.Format("/Image:{0} /Get-PackageInfo /PackageName:{1}", ShortenPath(MountPath), Package.Identity)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Parsing Update"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim PackageData As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    If PackageData.Contains("Package information:") Then
      If PackageData.Contains("The operation completed successfully.") Then
        PackageData = PackageData.Substring(PackageData.IndexOf("Packages information:") + 24)
        PackageData = PackageData.Substring(0, PackageData.IndexOf("The operation completed successfully.") - 4)
        Package.ParseInfo(PackageData)
      End If
    End If
    Return
  End Sub
#End Region
#Region "Features"
  Private Function DISM_Feature_Enable(MountPath As String, FeatureName As String) As Boolean
    If FeatureName.Contains(" ") Then FeatureName = String.Format("""{0}""", FeatureName)
    Dim Args As String = String.Format("/Image:{0} /Enable-Feature /FeatureName:{1}", ShortenPath(MountPath), FeatureName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Enabling Windows Feature"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Feature_Disable(MountPath As String, FeatureName As String) As Boolean
    If FeatureName.Contains(" ") Then FeatureName = String.Format("""{0}""", FeatureName)
    Dim Args As String = String.Format("/Image:{0} /Disable-Feature /FeatureName:{1}", ShortenPath(MountPath), FeatureName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Disabling Windows Feature"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Feature_GetList(MountPath As String) As String()
    Dim Args As String = String.Format("/Image:{0} /Get-Features /Format:Table", ShortenPath(MountPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Windows Feature List"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim FeatureList As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    If FeatureList.Contains("Features listing for package :") Then
      If FeatureList.Contains("The operation completed successfully.") Then
        FeatureList = FeatureList.Substring(FeatureList.LastIndexOf("| --"))
        FeatureList = FeatureList.Substring(FeatureList.IndexOf(vbNewLine) + 2)
        FeatureList = FeatureList.Substring(0, FeatureList.IndexOf("The operation completed successfully.") - 4)
        Dim FeatureItems() As String = Split(FeatureList, vbNewLine)
        For I As Integer = 0 To FeatureItems.Length - 1
          If FeatureItems(I).Contains("|") Then FeatureItems(I) = FeatureItems(I).Substring(0, FeatureItems(I).LastIndexOf("|"))
          FeatureItems(I) = FeatureItems(I).Trim
        Next
        Return FeatureItems
      End If
    End If
    Return Nothing
  End Function
  Private Function DISM_Feature_GetData(MountPath As String, FeatureName As String) As Feature
    Dim passedName As String = FeatureName
    If passedName.Contains(" ") Then passedName = String.Format("""{0}""", passedName)
    Dim Args As String = String.Format("/Image:{0} /Get-FeatureInfo /FeatureName:{1}", ShortenPath(MountPath), passedName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Parsing Feature"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim FeatureData As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Dim Info As New Feature(FeatureData)
    If Not Info.FeatureName = FeatureName Then
      If Info.IsEmpty Then Return Nothing
      ConsoleOutput_Write(String.Format("Feature Name Mismatch: Expected ""{0}"", got ""{1}""!", FeatureName, Info.FeatureName))
    End If
    Return Info
  End Function
#End Region
#Region "Drivers"
  Private Function DISM_Driver_Add(MountPath As String, AddPath As String, Recurse As Boolean, ForceUnsigned As Boolean) As Boolean
    Dim Args As String = String.Format("/Image:{0} /Add-Driver /Driver:{1}", ShortenPath(MountPath), ShortenPath(AddPath))
    If Recurse Then Args = String.Concat(Args, " /Recurse")
    If ForceUnsigned Then Args = String.Concat(Args, " /ForceUnsigned")
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Integrating Driver"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Driver_Remove(MountPath As String, DriverName As String) As Boolean
    Dim Args As String = String.Format("/Image:{0} /Remove-Driver /Driver:{1}", ShortenPath(MountPath), DriverName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Removing Driver"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim sRet As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DISM_Driver_GetList(MountPath As String, All As Boolean) As List(Of Driver)
    Dim Args As String = Nothing
    If All Then
      Args = String.Format("/Image:{0} /Get-Drivers /All /Format:Table", ShortenPath(MountPath))
    Else
      Args = String.Format("/Image:{0} /Get-Drivers /Format:Table", ShortenPath(MountPath))
    End If
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Driver List"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim DriverList As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args))
    DISM_Return_Clean()
    If DriverList.Contains("Driver packages listing:") Then
      If DriverList.Contains("The operation completed successfully.") Then
        DriverList = DriverList.Substring(DriverList.LastIndexOf("| --"))
        DriverList = DriverList.Substring(DriverList.IndexOf(vbNewLine) + 2)
        DriverList = DriverList.Substring(0, DriverList.IndexOf("The operation completed successfully.") - 4)
        Dim DriverItems() As String = Split(DriverList, vbNewLine)
        Dim dList As New List(Of Driver)
        For Each item As String In DriverItems
          dList.Add(New Driver(item))
        Next
        Return dList
      End If
    End If
    Return Nothing
  End Function
  Private Sub DISM_Driver_GetData(MountPath As String, ByRef Driver As Driver, arch As ArchitectureList)
    Dim Args As String = String.Format("/Image:{0} /Get-DriverInfo /Driver:{1}", ShortenPath(MountPath), Driver.PublishedName)
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Parsing Driver"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim DriverData As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args), , Not mySettings.HideDriverOutput)
    DISM_Return_Clean()
    If DriverData.Contains("Driver package information:") Then
      If DriverData.Contains("The operation completed successfully.") Then
        DriverData = DriverData.Substring(DriverData.IndexOf("Driver package information:") + 31)
        DriverData = DriverData.Substring(0, DriverData.IndexOf("The operation completed successfully.") - 4)
        Driver.ReadExtraData(DriverData, ShortenPath(MountPath), arch)
      End If
    End If
  End Sub
  Friend Function DISM_DriverINF_GetData(DriverINFPath As String) As Driver
    Dim Args As String = String.Format("/Online /Get-DriverInfo /Driver:{0}", ShortenPath(DriverINFPath))
    DISM_Return_Clean()
    Run_WithReturn_ShowProgress = True
    DISM_Return_ProgressStatus = "Reading Driver Information"
    DISM_Return_ProgressTotal = 3
    ConsoleOutput_Write(String.Format("DISM {0}", Args))
    Dim DriverData As String = Run_WithReturn(DismPath, String.Format("{0} /English", Args), , Not mySettings.HideDriverOutput)
    DISM_Return_Clean()
    If DriverData.Contains("Driver package information:") Then
      If DriverData.Contains("The operation completed successfully.") Then
        DriverData = DriverData.Substring(DriverData.IndexOf("Driver package information:") + 31)
        DriverData = DriverData.Substring(0, DriverData.IndexOf("The operation completed successfully.") - 4)
        Dim driver As New Driver("")
        If Environment.Is64BitOperatingSystem Then
          driver.ReadExtraData(DriverData, "", ArchitectureList.amd64)
        Else
          driver.ReadExtraData(DriverData, "", ArchitectureList.x86)
        End If
        Return driver
      End If
    End If
    Return Nothing
  End Function
#End Region
#End Region
#Region "ImageX"
  Private Function ImageX_Split(SourceWIM As String, DestSWM As String, Size As Long) As Boolean
    Dim Args As String = String.Format("/split {0} {1} {2}", SourceWIM, DestSWM, Size)
    ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
    Dim sRet As String = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args)
    If sRet.Contains("Successfully split") Then Return True
    Return False
  End Function
  Private Function ImageX_Export(SourceWIM As String, SourceIndex As Integer, DestWIM As String, DestName As String, DestDescr As String) As Boolean
    Dim RNArgs As String = String.Format("/info ""{0}"" {1} ""{2}"" ""{3}""", SourceWIM, SourceIndex, DestName, DestDescr)
    ConsoleOutput_Write(String.Format("IMAGEX {0}", RNArgs))
    Dim RNRet As String = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), RNArgs)
    Dim Args As String = String.Format("/export ""{0}"" {1} ""{2}"" ""{3}"" /compress maximum", SourceWIM, SourceIndex, DestWIM, DestName)
    Run_WithReturn_ShowProgress = True
    ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
    Dim sRet As String = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args)
    Run_WithReturn_ShowProgress = False
    Return sRet.Contains("Successfully exported image")
  End Function
  Private Function ImageX_UpdateLanguage(SourcePath As String) As Boolean
    Dim Args As String = String.Format("-genlangini -dist:{0} -image:{1} -f", ShortenPath(SourcePath), ShortenPath(Mount))
    ConsoleOutput_Write(String.Format("IntlCFG {0}", Args))
    Dim sRet As String = Run_WithReturn(IO.Path.Combine(AIKDir, "intlcfg"), Args)
    If Not sRet.Contains("A new Lang.ini file has been generated") Then
      Return False
    End If
    Dim MountPath As String = IO.Path.Combine(WorkDir, "BOOT")
    If Not IO.Directory.Exists(MountPath) Then IO.Directory.CreateDirectory(MountPath)
    Args = String.Format("/mountrw {0} {1} {2}", ShortenPath(IO.Path.Combine(SourcePath, "sources", "boot.wim")), 2, ShortenPath(MountPath))
    Run_WithReturn_ShowProgress = True
    ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
    sRet = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args)
    Run_WithReturn_ShowProgress = False
    If Not sRet.Contains("Successfully mounted image.") Then
      Status_SetText("Failed to mount boot.wim!")
      Return False
    End If
    If IO.File.Exists(IO.Path.Combine(SourcePath, "sources", "lang.ini")) Then
      ConsoleOutput_Write(String.Format("ConsoleOutput_WindowMoving ""{0}"" to ""{1}""...", IO.Path.Combine(SourcePath, "sources", "lang.ini"), IO.Path.Combine(MountPath, "sources", "lang.ini")))
      My.Computer.FileSystem.CopyFile(IO.Path.Combine(SourcePath, "sources", "lang.ini"), IO.Path.Combine(MountPath, "sources", "lang.ini"), True)
    End If
    Args = String.Format("/unmount /commit {0}", ShortenPath(MountPath))
    Run_WithReturn_ShowProgress = True
    ConsoleOutput_Write(String.Format("IMAGEX {0}", Args))
    sRet = Run_WithReturn(IO.Path.Combine(AIKDir, "imagex"), Args)
    Run_WithReturn_ShowProgress = False
    If Not sRet.Contains("Successfully unmounted image.") Then
      Status_SetText("Failed to unmount boot.wim!")
      Return False
    End If
    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MountPath))
    SlowDeleteDirectory(MountPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
    Return True
  End Function
#End Region
#Region "OSCDIMG"
  Private Function OSCDIMG_MakeISO(FromDir As String, DestISO As String, Label As String, FileSystem As String, Optional BootOrderFile As String = Nothing, Optional UEFI As Boolean = False) As Boolean
    Dim Args As String
    If String.IsNullOrEmpty(BootOrderFile) Then
      Args = String.Format("-m {0} {1} {2} -l{3}", FileSystem, ShortenPath(FromDir), ShortenPath(DestISO), Label)
    Else
      If UEFI Then
        Args = String.Format("-m {0} -yo{1} -bootdata:2#p0,e,b{2}#pEF,e,b{3} {4} {5} -l{6}", FileSystem, ShortenPath(BootOrderFile), ShortenPath(IO.Path.Combine(FromDir, "boot", "etfsboot.com")), ShortenPath(IO.Path.Combine(FromDir, "efi", "boot", "efisys.bin")), ShortenPath(FromDir), ShortenPath(DestISO), Label)
      Else
        Args = String.Format("-m {0} -yo{1} -b{2} {3} {4} -l{5}", FileSystem, ShortenPath(BootOrderFile), ShortenPath(IO.Path.Combine(FromDir, "boot", "etfsboot.com")), ShortenPath(FromDir), ShortenPath(DestISO), Label)
      End If
    End If
    Run_WithReturn_ShowProgress = True
    ConsoleOutput_Write(String.Format("OSCDIMG {0}", Args))
    Dim sRet As String = Run_WithReturn(IO.Path.Combine(AIKDir, "oscdimg"), Args)
    Run_WithReturn_ShowProgress = False
    Return sRet.Contains("Done.")
  End Function
#End Region
#Region "7-Zip"
  Private WithEvents Extract_Archive As Extraction.ArchiveFile
  Private Extract_ReturnList As New List(Of String)
  Private Sub Extract_ReturnList_Clean()
    Dim isEmpty As Boolean = True
    For I As Integer = 0 To Extract_ReturnList.Count - 1
      If Not String.IsNullOrEmpty(Extract_ReturnList(I)) Then
        isEmpty = False
        Exit For
      End If
    Next
    If isEmpty Then Extract_ReturnList.Clear()
  End Sub
#Region "All Files"
  Private Delegate Sub Extract_AllFilesInvoker(Source As String, Destination As String, SourceName As String)
  Private Sub Extract_AllFiles(Source As String, Destination As String, Optional SourceName As String = Nothing)
    If Me.InvokeRequired Then
      Me.Invoke(New Extract_AllFilesInvoker(AddressOf Extract_AllFiles), Source, Destination, SourceName)
      Return
    End If
    PreventSleep()
    If String.IsNullOrEmpty(SourceName) Then
      ConsoleOutput_Write(String.Format("Extracting all files from ""{0}"" to ""{1}""...", Source, Destination))
    Else
      ConsoleOutput_Write(String.Format("Extracting all {1} files from ""{0}"" to ""{2}""...", Source, SourceName, Destination))
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Extract_AllFiles_Task))
    Dim cIndex As Integer = Extract_ReturnList.Count
    Extract_ReturnList.Add(Nothing)
    tRunWithReturn.Start(CType({Source, Destination, cIndex}, Object()))
    Do While String.IsNullOrEmpty(Extract_ReturnList(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = Extract_ReturnList(cIndex)
    Extract_ReturnList(cIndex) = Nothing
    Extract_ReturnList_Clean()
    If sRet = "OK" Then
      ConsoleOutput_Write("Extraction Complete!", ConsoleOutput_MessageType.Output)
    Else
      ConsoleOutput_Write(sRet, ConsoleOutput_MessageType.Output)
    End If
    If StopRun Then
      AllowSleep()
      Return
    End If
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, String.Format("CRC Error in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Data Error"
        MsgDlg(Me, String.Format("Data Error in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Unsupported Method"
        MsgDlg(Me, String.Format("Unsupported Method in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "File Not Found"
        MsgDlg(Me, String.Format("Unable to find files in {0}!", IO.Path.GetFileName(Source)), "The files were not found.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case Else
        MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Error")
    End Select
    AllowSleep()
  End Sub
  Private Sub Extract_AllFiles_Task(Obj As Object)
    Dim Source, Destination As String, cIndex As Integer
    Source = CStr(CType(Obj, Object())(0))
    Destination = CStr(CType(Obj, Object())(1))
    cIndex = CInt(CType(Obj, Object())(2))
    Extract_Archive = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extract_Archive.Open()
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Opening: {0}", ex.Message)
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extract_Archive.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      file.Destination = New IO.FileInfo(IO.Path.Combine(Destination, file.Name))
    Next
    Try
      Extract_Archive.Extract()
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Extracting: {0}", ex.Message)
      Return
    End Try
    Progress_Normal_Sub(10000, 10000, "Extracting Files")
    Extract_ReturnList(cIndex) = "OK"
  End Sub
#End Region
#Region "All Except"
  Private Delegate Sub Extract_AllFiles_ExceptInvoker(Source As String, Destination As String, Except As String, SourceName As String)
  Private Sub Extract_AllFiles_Except(Source As String, Destination As String, Except As String, Optional SourceName As String = Nothing)
    If Me.InvokeRequired Then
      Me.Invoke(New Extract_AllFiles_ExceptInvoker(AddressOf Extract_AllFiles_Except), Source, Destination, Except, SourceName)
      Return
    End If
    PreventSleep()
    If String.IsNullOrEmpty(SourceName) Then
      ConsoleOutput_Write(String.Format("Extracting files except ""*{2}"" from ""{0}"" to ""{1}""...", Source, Destination, Except))
    Else
      ConsoleOutput_Write(String.Format("Extracting {1} files except ""*{3}"" from ""{0}"" to ""{2}""...", Source, SourceName, Destination, Except))
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Extract_AllFiles_Except_Task))
    Dim cIndex As Integer = Extract_ReturnList.Count
    Extract_ReturnList.Add(Nothing)
    tRunWithReturn.Start(CType({Source, Destination, Except, cIndex}, Object()))
    Do While String.IsNullOrEmpty(Extract_ReturnList(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = Extract_ReturnList(cIndex)
    Extract_ReturnList(cIndex) = Nothing
    Extract_ReturnList_Clean()
    If sRet = "OK" Then
      ConsoleOutput_Write("Extraction Complete!", ConsoleOutput_MessageType.Output)
    Else
      ConsoleOutput_Write(sRet, ConsoleOutput_MessageType.Output)
    End If
    If StopRun Then
      AllowSleep()
      Return
    End If
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, String.Format("CRC Error in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Data Error"
        MsgDlg(Me, String.Format("Data Error in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Unsupported Method"
        MsgDlg(Me, String.Format("Unsupported Method in {0} while attempting to extract files!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "File Not Found"
        MsgDlg(Me, String.Format("Unable to find files in {0}!", IO.Path.GetFileName(Source)), "The files were not found.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case Else
        MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Error")
    End Select
    AllowSleep()
  End Sub
  Private Sub Extract_AllFiles_Except_Task(Obj As Object)
    Dim Source, Destination, Except As String, cIndex As Integer
    Source = CStr(CType(Obj, Object())(0))
    Destination = CStr(CType(Obj, Object())(1))
    Except = CStr(CType(Obj, Object())(2))
    cIndex = CInt(CType(Obj, Object())(3))
    Extract_Archive = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extract_Archive.Open()
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Opening: {0}", ex.Message)
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extract_Archive.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      If Not file.Name.ToLower.EndsWith(Except.ToLower) Then
        file.Destination = New IO.FileInfo(IO.Path.Combine(Destination, file.Name))
      End If
    Next
    Try
      Extract_Archive.Extract()
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Extracting: {0}", ex.Message)
      Return
    End Try
    Progress_Normal_Sub(10000, 10000, "Extracting Files")
    Extract_ReturnList(cIndex) = "OK"
  End Sub
#End Region
#Region "Specific File"
  Private Delegate Sub Extract_FileInvoker(Source As String, Destination As String, File As String, SourceName As String)
  Private Sub Extract_File(Source As String, Destination As String, File As String, Optional SourceName As String = Nothing)
    If Me.InvokeRequired Then
      Me.Invoke(New Extract_FileInvoker(AddressOf Extract_File), Source, Destination, File, SourceName)
      Return
    End If
    PreventSleep()
    If String.IsNullOrEmpty(SourceName) Then
      ConsoleOutput_Write(String.Format("Extracting ""{2}"" from ""{0}"" to ""{1}""...", Source, Destination, File))
    Else
      ConsoleOutput_Write(String.Format("Extracting {1} file ""{3}"" from ""{0}"" to ""{2}""...", Source, SourceName, Destination, File))
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Extract_File_Task))
    Dim cIndex As Integer = Extract_ReturnList.Count
    Extract_ReturnList.Add(Nothing)
    tRunWithReturn.Start(CType({Source, Destination, File, cIndex}, Object()))
    Do While String.IsNullOrEmpty(Extract_ReturnList(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = Extract_ReturnList(cIndex)
    Extract_ReturnList(cIndex) = Nothing
    Extract_ReturnList_Clean()
    If sRet = "OK" Then
      ConsoleOutput_Write("Extraction Complete!", ConsoleOutput_MessageType.Output)
    Else
      ConsoleOutput_Write(sRet, ConsoleOutput_MessageType.Output)
    End If
    If StopRun Then
      AllowSleep()
      Return
    End If
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, String.Format("CRC Error in {0} while attempting to extract ""{1}""!", IO.Path.GetFileName(Source), File), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Data Error"
        MsgDlg(Me, String.Format("Data Error in {0} while attempting to extract ""{1}""!", IO.Path.GetFileName(Source), File), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "Unsupported Method"
        MsgDlg(Me, String.Format("Unsupported Method in {0} while attempting to extract ""{1}""!", IO.Path.GetFileName(Source), File), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case "File Not Found"
        MsgDlg(Me, String.Format("Unable to find ""{1}"" in {0}!", IO.Path.GetFileName(Source), File), "The file was not found.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
      Case Else
        If sRet = "Error Extracting: No matched files to extract." And File = "INSTALL.WIM" Then
          Dim PackageList() As String = Extract_FileList(Source)
          Dim containsSWM As Boolean = False
          For I As Integer = 0 To PackageList.Length - 1
            If PackageList(I).ToLower.EndsWith(".swm") Then
              containsSWM = True
              Exit For
            End If
          Next
          If containsSWM Then
            MsgDlg(Me, "This ISO's INSTALL.WIM file has been split up into SWM files. WIM files can't be modified once they've been split.", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction INSTALLSWM")
          Else
            MsgDlg(Me, "This ISO does not have an INSTALL.WIM file. It may not be a Windows 7 Installation ISO or there may be an issue with the file system.", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction INSTALLWIM")
          End If
        Else
          MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Error")
        End If
    End Select
    AllowSleep()
  End Sub
  Private Sub Extract_File_Task(Obj As Object)
    Dim Source, Destination, Find As String
    Source = CStr(CType(Obj, Object())(0))
    Destination = CStr(CType(Obj, Object())(1))
    Find = CStr(CType(Obj, Object())(2))
    Dim cIndex As Integer = CInt(CType(Obj, Object())(3))
    Dim bFound As Boolean = False
    Extract_Archive = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extract_Archive.Open()
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Opening: {0}", ex.Message)
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extract_Archive.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      If file.Name.ToLower.EndsWith(Find.ToLower) Then
        file.Destination = New IO.FileInfo(IO.Path.Combine(Destination, IO.Path.GetFileName(file.Name)))
        bFound = True
        Exit For
      End If
    Next
    Try
      Extract_Archive.Extract()
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Extracting: {0}", ex.Message)
      Return
    End Try
    Progress_Normal_Sub(10000, 10000, "Extracting File")
    If bFound Then
      Extract_ReturnList(cIndex) = "OK"
    Else
      Extract_ReturnList(cIndex) = "File Not Found"
    End If
  End Sub
#End Region
#Region "File List"
  Private Delegate Function Extract_FileListInvoker(Source As String) As String()
  Private Function Extract_FileList(Source As String) As String()
    If Me.InvokeRequired Then
      Return CType(Me.Invoke(New Extract_FileListInvoker(AddressOf Extract_FileList), Source), String())
    End If
    PreventSleep()
    ConsoleOutput_Write(String.Format("Extracting File List from ""{0}""...", Source))
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Extract_FileList_Task))
    Dim cIndex As Integer = Extract_ReturnList.Count
    Extract_ReturnList.Add(Nothing)
    tRunWithReturn.Start(CType({Source, cIndex}, Object()))
    Do While String.IsNullOrEmpty(Extract_ReturnList(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = Extract_ReturnList(cIndex)
    Extract_ReturnList(cIndex) = Nothing
    Extract_ReturnList_Clean()
    If sRet.Contains("|") Then
      ConsoleOutput_Write("Extraction Complete!", ConsoleOutput_MessageType.Output)
    Else
      ConsoleOutput_Write(sRet, ConsoleOutput_MessageType.Output)
    End If
    If StopRun Then
      AllowSleep()
      Return Nothing
    End If
    If sRet.Contains("|") Then
      AllowSleep()
      Return Split(sRet, "|")
    Else
      Select Case sRet
        Case "CRC Error"
          MsgDlg(Me, String.Format("CRC Error in {0} while attempting to read the file list!", IO.Path.GetFileName(Source)), "There was an error while reading.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
        Case "Data Error"
          MsgDlg(Me, String.Format("Data Error in {0} while attempting to read the file list!", IO.Path.GetFileName(Source)), "There was an error while reading.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
        Case "Unsupported Method"
          MsgDlg(Me, String.Format("Unsupported Method in {0} while attempting to read the file list!", IO.Path.GetFileName(Source)), "There was an error while extracting.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
        Case "File Not Found"
          MsgDlg(Me, String.Format("Unable to read any files in {0}!", IO.Path.GetFileName(Source)), "Files were not found.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
        Case "File List Busy"
          MsgDlg(Me, String.Format("Unable to read the file list in {0}!", IO.Path.GetFileName(Source)), "File list was busy.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , String.Format("Extraction {0}", sRet))
        Case Else
          MsgDlg(Me, sRet, "There was an error while reading.", "File read error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Error")
      End Select
      AllowSleep()
      Return Nothing
    End If
  End Function
  Private Sub Extract_FileList_Task(Obj As Object)
    Dim Source As String = CStr(CType(Obj, Object())(0))
    Dim cIndex As Integer = CInt(CType(Obj, Object())(1))
    Extract_Archive = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extract_Archive.Open()
    Catch ex As Exception
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      Extract_ReturnList(cIndex) = String.Format("Error Opening: {0}", ex.Message)
      Return
    End Try
    Dim sList As New List(Of String)
    For Each File In Extract_Archive
      sList.Add(File.Name)
    Next
    If sList.Count > 0 Then
      Extract_ReturnList(cIndex) = Join(sList.ToArray, "|")
    Else
      Extract_ReturnList(cIndex) = "File Not Found"
    End If
  End Sub
#End Region
#Region "Comment"
  Private Function Extract_Comment(Source As String) As String
    PreventSleep()
    ConsoleOutput_Write(String.Format("Extracting Comment from ""{0}""...", Source))
    Extract_Archive = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extract_Archive.Open()
      Dim sComment = Extract_Archive.ArchiveComment
      ConsoleOutput_Write("Extraction Complete!", ConsoleOutput_MessageType.Output)
      AllowSleep()
      Return sComment
    Catch ex As Exception
      ConsoleOutput_Write(String.Format("Error Reading: {0}", ex.Message), ConsoleOutput_MessageType.Output)
      Extract_Archive.Dispose()
      Extract_Archive = Nothing
      AllowSleep()
      Return Nothing
    End Try
  End Function
#End Region
  Private Sub Extract_Archive_ExtractFile(sender As Object, e As Extraction.COM.ExtractFileEventArgs) Handles Extract_Archive.ExtractFile
    If StopRun Then e.ContinueOperation = False
    If Extract_Archive.ExtractionCount() > 1 Then
      If e.Stage = Extraction.COM.ExtractionStage.Done Then
        If e.Item.Index > Integer.MaxValue Then
          Progress_Normal_Sub(0, 0, "Extracting Files")
        Else
          Progress_Normal_Sub(CInt(e.Item.Index), Extract_Archive.ItemCount, "Extracting Files")
        End If
        Application.DoEvents()
      End If
    End If
  End Sub
  Private Sub Extract_Archive_ExtractProgress(sender As Object, e As Extraction.COM.ExtractProgressEventArgs) Handles Extract_Archive.ExtractProgress
    If Extract_Archive.ExtractionCount = 1 AndAlso e.Total > 1048576 * 64 Then
      If StopRun Then e.ContinueOperation = False
      Dim iMax As Integer = 10000
      Dim iVal As Integer = CInt(e.Written / e.Total * iMax)
      If pbIndividual.Value = iVal AndAlso pbIndividual.Maximum = iMax Then Return
      Progress_Normal_Sub(iVal, iMax, "Extracting File")
      Application.DoEvents()
    End If
  End Sub
  Private Function Extract_ShowFailure(Message As String) As Boolean
    If String.IsNullOrEmpty(Message) Then Return False
    ConsoleOutput_Write(Message, ConsoleOutput_MessageType.Output)
    If StopRun Then Return True
    If Message = "OK" Then
      Return False
    ElseIf Message.StartsWith("CRC Error") Then
      MsgDlg(Me, String.Format("CRC Error in compressed file!", vbNewLine, Message.Substring(Message.IndexOf("|") + 1)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction CRC Error")
    ElseIf Message.StartsWith("Data Error") Then
      MsgDlg(Me, String.Format("Data Error in compressed file!", vbNewLine, Message.Substring(Message.IndexOf("|") + 1)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Data Error")
    ElseIf Message.StartsWith("Unsupported Method") Then
      MsgDlg(Me, String.Format("Unsupported Method in compressed file!", vbNewLine, Message.Substring(Message.IndexOf("|") + 1)), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Unsupported Method")
    ElseIf Message.StartsWith("File Not Found") Then
      MsgDlg(Me, String.Format("Unable to find expected file in compressed file!", vbNewLine, Message.Substring(Message.IndexOf("|") + 1)), "The file was not found.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction File Not Found")
    Else
      MsgDlg(Me, Message, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, _TaskDialogIcon.Error, , , "Extraction Error")
    End If
    Return True
  End Function
#End Region
#Region "EXE2CAB"
  Private Function EXE2CAB_Convert(Source As String, Destination As String) As Boolean
    Try
      Using eRead As New IO.FileStream(Source, IO.FileMode.Open, IO.FileAccess.Read)
        Dim bFound As Boolean = False
        Do Until eRead.Position >= eRead.Length
          Dim bRead As Integer = eRead.ReadByte
          If Not bRead = &H4D Then Continue Do
          If Not eRead.ReadByte = &H53 Then eRead.Position -= 1 : Continue Do
          If Not eRead.ReadByte = &H43 Then eRead.Position -= 2 : Continue Do
          If Not eRead.ReadByte = &H46 Then eRead.Position -= 3 : Continue Do
          bFound = True
          eRead.Position -= 4
          Exit Do
        Loop
        If bFound Then
          Using cWrite As New IO.FileStream(Destination, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            cWrite.Position = 0
            Dim lSize As Long = eRead.Length - eRead.Position
            Dim maxSize As Integer = CLng(Integer.MaxValue / 4)
            If lSize > maxSize Then
              Dim lChunks As Long = CLng(Math.Floor(lSize / maxSize))
              Dim iLastSize As Integer = CInt(lSize Mod maxSize)
              For L As Long = 0 To lChunks - 1
                Dim bData(maxSize - 1) As Byte
                eRead.Read(bData, 0, maxSize)
                cWrite.Write(bData, 0, maxSize)
                Erase bData
              Next
              If iLastSize > 0 Then
                Dim bLast(iLastSize - 1) As Byte
                eRead.Read(bLast, 0, iLastSize)
                cWrite.Write(bLast, 0, iLastSize)
                Erase bLast
              End If
            Else
              If lSize > Integer.MaxValue Then Return False
              Dim bData(CInt(lSize) - 1) As Byte
              eRead.Read(bData, 0, CInt(lSize))
              cWrite.Write(bData, 0, CInt(lSize))
              Erase bData
            End If
          End Using
          Return True
        Else
          Return False
        End If
      End Using
    Catch ex As Exception
      Return False
    End Try
  End Function
#End Region
#Region "Caller Functions"
  Private Shared Run_WithReturn_ShowProgress As Boolean
#Region "Run With Return"
  Private Shared Run_WithReturn_ReturnList As New List(Of String)
  Private Shared Run_WithReturn_FinishedList As New List(Of Boolean)
  Private Shared Run_WithReturn_OutputAccumulation As New List(Of String)
  Private Delegate Sub Run_WithReturn_PassReturnInvoker(Index As Integer, Output As String, WriteOutput As Boolean)
  Private Function Run_WithReturn(Filename As String, Arguments As String, Optional IgnoreStopRun As Boolean = False, Optional DisplayOutput As Boolean = True) As String
    PreventSleep()
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Run_WithReturn_Task))
    Dim cIndex As Integer = Run_WithReturn_ReturnList.Count
    Run_WithReturn_ReturnList.Add(Nothing)
    Run_WithReturn_FinishedList.Add(False)
    Run_WithReturn_OutputAccumulation.Add(Nothing)
    tRunWithReturn.Start(CType({Filename, Arguments, cIndex, DisplayOutput, Process.GetCurrentProcess.PriorityClass}, Object()))
    Dim iCompletedAt As Long = 0
    Do While Run_WithReturn_ReturnList(cIndex) Is Nothing
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If Not IgnoreStopRun AndAlso StopRun Then
        Try
          tRunWithReturn.Abort()
        Catch ex As Exception
        End Try
        AllowSleep()
        Return ""
      End If
      If iCompletedAt > 0 Then
        If String.IsNullOrEmpty(Run_WithReturn_OutputAccumulation(cIndex)) Then
          If TickCount() - iCompletedAt > 5000 Then Run_WithReturn_ReturnList(cIndex) = "Process has terminated"
        Else
          Run_WithReturn_ReturnList(cIndex) = Run_WithReturn_OutputAccumulation(cIndex)
        End If
      End If
        If Run_WithReturn_FinishedList(cIndex) And iCompletedAt = 0 Then iCompletedAt = TickCount()
    Loop
    Dim sRet As String = Run_WithReturn_ReturnList(cIndex)
    If Not String.IsNullOrEmpty(sRet) AndAlso sRet.StartsWith(String.Concat("!", vbNewLine)) Then sRet = sRet.Substring(3)
    Run_WithReturn_ReturnList(cIndex) = Nothing
    Run_WithReturn_FinishedList(cIndex) = False
    Run_WithReturn_OutputAccumulation(cIndex) = Nothing
    If DISM_Return_ProgressTotal > 0 Then
      DISM_Return_ProgressValue += 1
      If DISM_Return_ProgressValue > DISM_Return_ProgressTotal Then DISM_Return_ProgressValue = DISM_Return_ProgressTotal
      If DISM_Return_ProgressValue < 0 Then DISM_Return_ProgressValue = 0
      Progress_Normal_Sub(DISM_Return_ProgressValue, DISM_Return_ProgressTotal, DISM_Return_ProgressStatus)
    End If
    AllowSleep()
    Return sRet
  End Function
  Private Sub Run_WithReturn_PassReturn(Index As Integer, Output As String, WriteOutput As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New Run_WithReturn_PassReturnInvoker(AddressOf Run_WithReturn_PassReturn), Index, Output, WriteOutput)
      Return
    End If
    If Output.StartsWith(String.Concat("!", vbNewLine)) Then
      Output = Output.Substring(3)
    ElseIf WriteOutput Then
      ConsoleOutput_Write(Output, ConsoleOutput_MessageType.Output)
    End If
    Run_WithReturn_ReturnList(Index) = Output
  End Sub
  Private Sub Run_WithReturn_Task(Obj As Object)
    Dim Filename As String = CStr(CType(Obj, Object())(0))
    Dim Arguments As String = CStr(CType(Obj, Object())(1))
    Dim Index As Integer = CInt(CType(Obj, Object())(2))
    Dim DisplayOutput As Boolean = CBool(CType(Obj, Object())(3))
    Dim Priority As Diagnostics.ProcessPriorityClass = CType(CType(Obj, Object())(4), Diagnostics.ProcessPriorityClass)
    Using PkgList As New Process
      PkgList.StartInfo.FileName = Filename
      PkgList.StartInfo.Arguments = Arguments
      PkgList.StartInfo.UseShellExecute = False
      PkgList.StartInfo.RedirectStandardInput = True
      PkgList.StartInfo.RedirectStandardOutput = True
      PkgList.StartInfo.RedirectStandardError = True
      PkgList.StartInfo.CreateNoWindow = True
      PkgList.StartInfo.EnvironmentVariables.Add("RUNNING_INDEX", CStr(Index))
      If DisplayOutput Then
        PkgList.StartInfo.EnvironmentVariables.Add("WRITE_OUTPUT", "Y")
      Else
        PkgList.StartInfo.EnvironmentVariables.Add("WRITE_OUTPUT", "N")
      End If
      PkgList.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8
      PkgList.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8
      AddHandler PkgList.OutputDataReceived, AddressOf Run_WithReturn_HandleOutput
      AddHandler PkgList.ErrorDataReceived, AddressOf Run_WithReturn_HandleError
      PkgList.Start()
      Try
        PkgList.BeginErrorReadLine()
        PkgList.BeginOutputReadLine()
        PkgList.PriorityClass = Priority
        If mySettings.Timeout > 0 Then
          PkgList.WaitForExit(mySettings.Timeout)
        Else
          PkgList.WaitForExit()
        End If
        Run_WithReturn_FinishedList(Index) = True
      Catch ex As Exception
        Run_WithReturn_PassReturn(Index, ex.Message, True)
        Err.Clear()
      End Try
    End Using
  End Sub
  Private Sub Run_WithReturn_PassOutput(Index As Integer, Output As String, WriteOutput As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New Run_WithReturn_PassReturnInvoker(AddressOf Run_WithReturn_PassOutput), Index, Output, WriteOutput)
      Return
    End If
    If Output Is Nothing Then
      If String.IsNullOrEmpty(Run_WithReturn_OutputAccumulation(Index)) Then
        Run_WithReturn_ReturnList(Index) = "Process has terminated"
      Else
        Run_WithReturn_ReturnList(Index) = String.Concat("!", vbNewLine, Run_WithReturn_OutputAccumulation(Index))
      End If
      Run_WithReturn_OutputAccumulation(Index) = Nothing
    ElseIf String.IsNullOrEmpty(Output) Then
      Run_WithReturn_OutputAccumulation(Index) = String.Concat(Run_WithReturn_OutputAccumulation(Index), vbNewLine)
      If WriteOutput Then ConsoleOutput_Write(Nothing, ConsoleOutput_MessageType.Output)
      Return
    Else
      If Run_WithReturn_ShowProgress Then
        If Output.Contains(" ] Exporting progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          Progress_Normal_Sub(CInt(ProgI), 100, "Exporting")
        ElseIf Output.Contains(" ] Mounting progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          Progress_Normal_Sub(CInt(ProgI), 100, "Mounting")
        ElseIf Output.Contains(" ] Committing Image progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          Progress_Normal_Sub(CInt(ProgI), 100, "Committing")
        ElseIf Output.Contains(" ] Unmounting progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          Progress_Normal_Sub(CInt(ProgI), 100, "Unmounting")
        ElseIf Output.Contains(" ] Mount cleanup progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          Progress_Normal_Sub(CInt(ProgI), 100, "Cleaning")
        ElseIf Output.Contains("Version: ") Or
          Output.Contains("Mounting image") Or
          Output.Contains("Image Version: ") Or
          Output.Contains("Processing 1 of 1 - ") Or
          Output.Contains("Saving image") Or
          Output.Contains("Unmounting image") Or
          Output.Contains("The operation completed successfully.") Then
          If DISM_Return_ProgressTotal > 0 Then
            DISM_Return_ProgressValue += 1
            If DISM_Return_ProgressValue > DISM_Return_ProgressTotal Then DISM_Return_ProgressValue = DISM_Return_ProgressTotal
            If DISM_Return_ProgressValue < 0 Then DISM_Return_ProgressValue = 0
            Progress_Normal_Sub(DISM_Return_ProgressValue, DISM_Return_ProgressTotal, DISM_Return_ProgressStatus)
          End If
        End If
      End If
      Run_WithReturn_OutputAccumulation(Index) = String.Concat(Run_WithReturn_OutputAccumulation(Index), Output, vbNewLine)
      If WriteOutput Then ConsoleOutput_Write(Output, ConsoleOutput_MessageType.Output)
    End If
  End Sub
  Private Sub Run_WithReturn_PassError(Index As Integer, Output As String, WriteOutput As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New Run_WithReturn_PassReturnInvoker(AddressOf Run_WithReturn_PassError), Index, Output, WriteOutput)
      Return
    End If
    If String.IsNullOrEmpty(Output) Then
      If WriteOutput Then ConsoleOutput_Write(Nothing, ConsoleOutput_MessageType.Output)
      Return
    End If
    If Run_WithReturn_ShowProgress Then
      If Output.Contains("% complete") Then
        Dim ProgI As String = Output.Substring(0, Output.IndexOf("%"))
        If ProgI.Contains(" ") Then ProgI = ProgI.Substring(ProgI.LastIndexOf(" ") + 1)
        Progress_Normal_Sub(CInt(ProgI), 100, "Writing ISO")
      End If
      If WriteOutput Then ConsoleOutput_Write(Output, ConsoleOutput_MessageType.Output)
    ElseIf WriteOutput Then
      If Output.Contains("% complete") Then
        ConsoleOutput_Write(Output, ConsoleOutput_MessageType.Output)
      Else
        ConsoleOutput_Write(String.Format("<ERROR> {0}", Output), ConsoleOutput_MessageType.Output)
      End If
    End If
  End Sub
  Private Sub Run_WithReturn_HandleOutput(sender As Object, e As DataReceivedEventArgs)
    If StopRun Then Return
    Dim tmpData As String = e.Data
    Dim pkgData As Process = CType(sender, Process)
    Dim Index As Integer = CInt(pkgData.StartInfo.EnvironmentVariables("RUNNING_INDEX"))
    Run_WithReturn_PassOutput(Index, tmpData, pkgData.StartInfo.EnvironmentVariables("WRITE_OUTPUT") = "Y")
  End Sub
  Private Sub Run_WithReturn_HandleError(sender As Object, e As DataReceivedEventArgs)
    If StopRun Then Return
    Dim tmpData As String = e.Data
    If Not tmpData Is Nothing Then
      Dim pkgData As Process = CType(sender, Process)
      Dim Index As Integer = CInt(pkgData.StartInfo.EnvironmentVariables("RUNNING_INDEX"))
      Run_WithReturn_PassError(Index, tmpData, pkgData.StartInfo.EnvironmentVariables("WRITE_OUTPUT") = "Y")
    End If
  End Sub
#End Region
#Region "Run Hidden"
  Private Run_Hidden_FinishedList As New List(Of Boolean)
  Private Delegate Sub Run_Hidden_PassReturnInvoker(Index As Integer)
  Private Sub Run_Hidden(Filename As String, Arguments As String)
    PreventSleep()
    Dim tRunHidden As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Run_Hidden_Task))
    Dim cIndex As Integer = Run_Hidden_FinishedList.Count
    Run_Hidden_FinishedList.Add(False)
    tRunHidden.Start(CType({Filename, Arguments, cIndex, Process.GetCurrentProcess.PriorityClass}, Object()))
    Do Until Run_Hidden_FinishedList(cIndex)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun And Not Arguments.ToLower.Contains("unmount") Then
        Try
          tRunHidden.Abort()
        Catch ex As Exception
        End Try
        AllowSleep()
        Return
      End If
    Loop
    Run_Hidden_FinishedList(cIndex) = False
    AllowSleep()
  End Sub
  Private Sub Run_Hidden_PassReturn(Index As Integer)
    If Me.InvokeRequired Then
      Me.Invoke(New Run_Hidden_PassReturnInvoker(AddressOf Run_Hidden_PassReturn), Index)
      Return
    End If
    Run_Hidden_FinishedList(Index) = True
  End Sub
  Private Sub Run_Hidden_Task(Obj As Object)
    Dim Filename As String = CStr(CType(Obj, Object())(0))
    Dim Arguments As String = CStr(CType(Obj, Object())(1))
    Dim Index As Integer = CInt(CType(Obj, Object())(2))
    Dim Priority As Diagnostics.ProcessPriorityClass = CType(CType(Obj, Object())(3), Diagnostics.ProcessPriorityClass)
    Using PkgList As New Process
      PkgList.StartInfo.FileName = Filename
      PkgList.StartInfo.Arguments = Arguments
      PkgList.StartInfo.UseShellExecute = True
      PkgList.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
      PkgList.Start()
      Try
        PkgList.PriorityClass = Priority
        If mySettings.Timeout > 0 Then
          PkgList.WaitForExit(mySettings.Timeout)
        Else
          PkgList.WaitForExit()
        End If
        Run_Hidden_PassReturn(Index)
      Catch ex As Exception
        Err.Clear()
        Run_Hidden_PassReturn(Index)
      End Try
    End Using
  End Sub
#End Region
#End Region
#End Region
#Region "Integration"
  Private Integrate_LanguageChanged As Boolean = False
  Private Sub Slipstream()
    If lvImages.Items.Count = 0 OrElse lvImages.CheckedItems.Count = 0 Then
      Status_SetText("No Image Package Selected. ")
      lvImages.Focus()
      Beep()
      Return
    End If
    Dim IdenticalNames As New List(Of String)
    For I As Integer = 0 To lvImages.Items.Count - 1
      If Not lvImages.Items(I).Checked Then Continue For
      Dim lvIndex As Integer = CInt(lvImages.Items(I).Tag)
      Dim selName As String = ImagePackage_ListData(lvIndex).NewName
      For J As Integer = 0 To lvImages.Items.Count - 1
        If J = I Then Continue For
        If Not lvImages.Items(J).Checked Then Continue For
        Dim lvIndex2 As Integer = CInt(lvImages.Items(J).Tag)
        If ImagePackage_ListData(lvIndex2).NewName = selName Then
          If Not IdenticalNames.Contains(selName) Then IdenticalNames.Add(selName)
        End If
      Next
    Next
    If IdenticalNames.Count > 0 Then
      Status_SetText("All Image Packages must have different names!")
      Dim no64 As Boolean = True
      Dim all86 As Boolean = True
      Dim all64 As Boolean = True
      Dim allIA As Boolean = True
      For I As Integer = 0 To lvImages.Items.Count - 1
        Dim lvIndex As Integer = CInt(lvImages.Items(I).Tag)
        If Not IdenticalNames.Contains(ImagePackage_ListData(lvIndex).NewName) Then Continue For
        If ImagePackage_ListData(lvIndex).NewName.Contains("64") Then no64 = False
        If CompareArchitectures(ImagePackage_ListData(lvIndex).Package.Architecture, ArchitectureList.x86, False) Then all64 = False : allIA = False
        If CompareArchitectures(ImagePackage_ListData(lvIndex).Package.Architecture, ArchitectureList.amd64, False) Then all86 = False : allIA = False
        If CompareArchitectures(ImagePackage_ListData(lvIndex).Package.Architecture, ArchitectureList.ia64, False) Then all86 = False : all64 = False
      Next
      If all86 Or all64 Or allIA Then no64 = False
      If no64 Then
        If MsgDlg(Me, "Would you like to add ""x64"" (or ""ia64"") to 64-Bit image names automatically? This may resolve the issue. Please note that this will change the Display Name of the affected images.", "All Image Packages must have different names!", "Unable to Begin Slipstream Process", MessageBoxButtons.YesNo, _TaskDialogIcon.FontBigA, MessageBoxDefaultButton.Button2, String.Concat("The following Image Package Names have been used more than once: ", vbNewLine, Join(IdenticalNames.ToArray, vbNewLine)), "Unique Names") = Windows.Forms.DialogResult.Yes Then
          For I As Integer = 0 To lvImages.Items.Count - 1
            Dim lvIndex As Integer = CInt(lvImages.Items(I).Tag)
            Dim imgD As ImagePackage_PackageData = ImagePackage_ListData(lvIndex)
            If CompareArchitectures(imgD.Package.Architecture, ArchitectureList.amd64, False) Then
              If Not imgD.NewName.Contains("64") Then
                Dim oldName As String = imgD.NewName
                imgD.NewName = String.Concat(imgD.NewName, " x64")
                lvImages.Items(I).SubItems(1).Text = imgD.NewName
                ImagePackage_ListData(lvIndex) = imgD
              End If
            ElseIf CompareArchitectures(imgD.Package.Architecture, ArchitectureList.ia64, False) Then
              If Not imgD.NewName.Contains("64") Then
                Dim oldName As String = imgD.NewName
                imgD.NewName = String.Concat(imgD.NewName, " ia64")
                lvImages.Items(I).SubItems(1).Text = imgD.NewName
                ImagePackage_ListData(lvIndex) = imgD
              End If
            End If
          Next
          Slipstream()
        Else
          lvImages.Focus()
        End If
      Else
        MsgDlg(Me, "Please change any image package names which are identical.", "All Image Packages must have different names!", "Unable to Begin Slipstream Process", MessageBoxButtons.OK, _TaskDialogIcon.FontBigA, , Join(IdenticalNames.ToArray, vbNewLine), "Unique Names")
        lvImages.Focus()
        Beep()
      End If
      Return
    End If
    RunComplete = False
    StopRun = False
    Integrate_LanguageChanged = False
    If txtISOLabel.Enabled And txtISOLabel.Text.Contains(" ") Then
      Status_SetText("Spaces are not allowed in the ISO Label!")
      txtISOLabel.Text = Replace(txtISOLabel.Text, " ", "_")
      txtISOLabel.Focus()
      Return
    End If
    TitleMNG_SetDisplay(TitleMNG_List.Delete)
    TitleMNG_SetTitle("Preparing Images", "Cleaning up mounts and extracting WIM from ISO if necessary...")
    RunActivity = ActivityType.Integrating
    GUI_ToggleEnabled(False)
    Dim WIMFile As String = Nothing
    If IO.Directory.Exists(WorkDir) Then
      Progress_Normal(0, 1)
      If Not CleanMounts(True) Then
        GUI_ToggleEnabled(True, "Active Mount Detected!")
        cmdConfig.Focus()
        Beep()
        MsgDlg(Me, "The selected Temp directory has an active mount that could not be removed. Please restart your computer or change your Temp directory before continuing.", "Active mount has been detected.", "Unable to Begin Slipstream Process", MessageBoxButtons.OK, _TaskDialogIcon.DriveLocked, , , "Active Mount")
        Return
      End If
      Status_SetText("Clearing Temporary Files...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", WorkDir))
      Try
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents, True)
        Application.DoEvents()
      Catch ex As Exception
        Application.DoEvents()
      End Try
    End If
    Dim iTotalVal As Integer = 0
    Dim iTotalMax As Integer = 6
    ' +1 Put all the checked images together into one package
    ' +1 Back up the original INSTALL.WIM
    ' +1 Read the number of image packages in the new INSTALL.WIM
    ' IF Just a WIM File
    '  +1 Finalize WIM
    '  +1 Back up the old WIM
    '  +1 Compress Finalized WIM to the old WIM's location
    ' ELSE IF Full ISO Deal
    '  +1 Prepare Language Data and UEFI
    '  +1 Finalize WIM
    '  +1 Compress Finalized WIM
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then iTotalMax += 1 ' +1 Extract INSTALL.WIM from Main ISO
    If chkMerge.Checked AndAlso IO.Path.GetExtension(txtMerge.Text).ToLower = ".iso" Then iTotalMax += 1 ' +1 Extract INSTALL.WIM from Merge ISO
    If lvMSU.Items.Count > 0 Then iTotalMax += 2 ' +1 Read Update Information, +1 Integrate Windows Updates
    Dim modifyBoot As Boolean = False
    Dim modifyPE As Boolean = False
    For I As Integer = 0 To lvMSU.Items.Count - 1
      Dim dIndex As Integer = CInt(lvMSU.Items(I).Tag)
      If Updates_ListData(dIndex).Update.Name = "DRIVER" Then
        If Updates_ListData(dIndex).Update.DriverData.IntegrateIntoBootPE Then modifyPE = True
        If Updates_ListData(dIndex).Update.DriverData.IntegrateIntoBootSetup Then modifyBoot = True
        If modifyBoot And modifyPE Then Exit For
      End If
    Next
    If modifyBoot And modifyPE Then
      iTotalMax += 6
      ' +1 Extract BOOT.WIM
      ' +1 Read image package count of BOOT.WIM
      ' +1 Read Driver Information for PE
      ' +1 Integrate PE Drivers
      ' +1 Read Driver Information for Installer
      ' +1 Integrate Installer Drivers
    ElseIf modifyBoot Or modifyPE Then
      iTotalMax += 4
      ' +1 Extract BOOT.WIM
      ' +1 Read image package count of BOOT.WIM
      ' +1 Read Driver Information (for PE or Installer)
      ' +1 Read Driver Information (for PE or Installer)
    End If
    If Not String.IsNullOrEmpty(txtSP.Text) Then
      iTotalMax += 1 ' +1 Integrate the (32-bit or only) Service Pack 1
      If Not String.IsNullOrEmpty(txtSP64.Text) Then iTotalMax += 1 ' +1 Integrate the 64-bit Service Pack 1
    End If
    If Not String.IsNullOrEmpty(txtISO.Text) Then
      iTotalMax += 1 ' +1 Build ISO
      If cmbLimitType.SelectedIndex > 0 Then iTotalMax += 1 ' +1 Split WIM
    End If
    If (Not UEFI_IsEnabled AndAlso chkUEFI.Checked) AndAlso lvMSU.Items.Count = 0 Then iTotalMax += 1 ' +1 Mount image for UEFI purposes if the image isn't mounted already
    Dim DoFeatures As Boolean = False
    Dim DoIntegratedUpdates As Boolean = False
    Dim DoIntegratedDrivers As Boolean = False
    For Each lvRow As ListViewItem In lvImages.Items
      Dim lvIndex As Integer = CInt(lvRow.Tag)
      If lvRow.Checked Then
        If Not DoFeatures Then
          Dim TestFeatures As List(Of Feature) = ImagePackage_ListData(lvIndex).FeatureList
          If TestFeatures IsNot Nothing AndAlso TestFeatures.Count > 0 Then
            For I As Integer = 0 To TestFeatures.Count - 1
              If TestFeatures(I).Enable Then
                If Not (TestFeatures(I).State = "Enabled" Or TestFeatures(I).State = "Enable Pending") Then
                  DoFeatures = True
                  Exit For
                End If
              Else
                If TestFeatures(I).State = "Enabled" Or TestFeatures(I).State = "Enable Pending" Then
                  DoFeatures = True
                  Exit For
                End If
              End If
            Next
          End If
        End If
        If Not DoIntegratedUpdates Then
          Dim TestIntegratedUpdates As List(Of Update_Integrated) = ImagePackage_ListData(lvIndex).Package.IntegratedUpdateList
          If TestIntegratedUpdates IsNot Nothing AndAlso TestIntegratedUpdates.Count > 0 Then
            For I As Integer = 0 To TestIntegratedUpdates.Count - 1
              If TestIntegratedUpdates(I).Remove And Not (TestIntegratedUpdates(I).State = "Uninstall Pending" Or TestIntegratedUpdates(I).State = "Superseded") Then
                DoIntegratedUpdates = True
                Exit For
              End If
            Next
          End If
        End If
        If Not DoIntegratedDrivers Then
          Dim TestIntegratedDrivers As List(Of Driver) = ImagePackage_ListData(lvIndex).DriverList
          If TestIntegratedDrivers IsNot Nothing AndAlso TestIntegratedDrivers.Count > 0 Then
            For I As Integer = 0 To TestIntegratedDrivers.Count - 1
              If TestIntegratedDrivers(I).Remove Then
                DoIntegratedDrivers = True
                Exit For
              End If
            Next
          End If
        End If
      End If
    Next
    If DoIntegratedUpdates Then iTotalMax += 1 ' +1 Remove Integrated Updates
    If DoFeatures Then iTotalMax += 1 ' +1 Change Integrated Features
    If DoIntegratedDrivers Then iTotalMax += 1 ' +1 Remove Integrated Drivers
    'All Totals Calculated
    Progress_Normal(0, 1)
    Progress_Total(iTotalVal, iTotalMax)
    pbTotal.Style = ProgressBarStyle.Continuous
    TitleMNG_SetDisplay(TitleMNG_List.Copy)
    Status_SetText("Checking WIM...")
    If String.IsNullOrEmpty(txtWIM.Text) OrElse Not IO.File.Exists(txtWIM.Text) Then
      GUI_ToggleEnabled(True, "Missing WIM File!")
      txtWIM.Focus()
      Beep()
      Return
    End If
    If chkMerge.Checked Then
      Status_SetText("Checking Merge WIM...")
      If String.IsNullOrEmpty(txtMerge.Text) OrElse Not IO.File.Exists(txtMerge.Text) Then
        GUI_ToggleEnabled(True, "Missing Merge File!")
        txtMerge.Focus()
        Beep()
        Return
      End If
    End If
    Dim ISOFile As String = Nothing
    If chkISO.Checked Then
      Status_SetText("Checking ISO...")
      If String.IsNullOrEmpty(txtISO.Text) OrElse Not IO.File.Exists(txtISO.Text) Then
        GUI_ToggleEnabled(True, "Missing ISO File!")
        txtISO.Focus()
        Beep()
        Return
      End If
      ISOFile = txtISO.Text
    Else
      ISOFile = Nothing
    End If
    Dim BOOTWIMFile As String = Nothing
    Dim bootPackageCount As Integer = 0
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Extract INSTALL.WIM from Main ISO
      Progress_Total(iTotalVal, iTotalMax)
      Status_SetText("Extracting Image from ISO...")
      Extract_File(txtWIM.Text, Work, "INSTALL.WIM")
      WIMFile = IO.Path.Combine(Work, "INSTALL.WIM")
    Else
      WIMFile = txtWIM.Text
    End If
    If Not String.IsNullOrEmpty(ISOFile) Then
      If modifyBoot Or modifyPE Then
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Extract BOOT.WIM
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Extracting Boot Image from ISO...")
        Extract_File(ISOFile, Work, "BOOT.WIM")
        BOOTWIMFile = IO.Path.Combine(Work, "BOOT.WIM")
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Read image package count of BOOT.WIM
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Reading Boot Image Package Count...")
        bootPackageCount = DISM_Image_GetCount(BOOTWIMFile)
      End If
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return
    End If
    Progress_Normal(0, 1)
    TitleMNG_SetDisplay(TitleMNG_List.Delete)
    TitleMNG_SetTitle("Merging Image Packages", "Merging all WIM packages into single WIM...")
    Status_SetText("Checking Merge...")
    Dim MergeFile As String = Nothing
    If chkMerge.Checked Then MergeFile = txtMerge.Text
    Dim MergeWIM As String = Nothing
    Dim MergeWork As String = IO.Path.Combine(Work, WIMGroup.Merge.ToString)
    If Not String.IsNullOrEmpty(MergeFile) Then
      If IO.Directory.Exists(MergeWork) Then
        Status_SetText("Clearing Temporary Files...")
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MergeWork))
        Try
          SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          Application.DoEvents()
        Catch ex As Exception
          Application.DoEvents()
        End Try
      End If
      IO.Directory.CreateDirectory(MergeWork)
      Dim MergeWorkExtract As String = IO.Path.Combine(MergeWork, "Extract")
      If Not IO.Directory.Exists(MergeWorkExtract) Then IO.Directory.CreateDirectory(MergeWorkExtract)
      If IO.Path.GetExtension(MergeFile).ToLower = ".iso" Then
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Extract INSTALL.WIM from Merge ISO
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Extracting Merge Image from ISO...")
        Extract_File(MergeFile, MergeWorkExtract, "INSTALL.WIM")
        Application.DoEvents()
        MergeWIM = IO.Path.Combine(MergeWorkExtract, "INSTALL.WIM")
      Else
        MergeWIM = MergeFile
      End If
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return
    End If
    Progress_Normal(0, 1)
    iTotalVal += 1 ' +1 Put all the checked images together into one package
    Progress_Total(iTotalVal, iTotalMax)
    Dim iProgVal As Integer = 0
    Dim iProgMax As Integer = lvImages.CheckedItems.Count
    Progress_Normal(iProgVal, iProgMax)
    Dim NewWIM As String = IO.Path.Combine(Work, "newINSTALL.wim")
    Status_SetText("Generating Image...")
    Dim ImageIntegratedUpdates() As List(Of Update_Integrated) = Nothing
    Dim ImageFeatures() As List(Of Feature) = Nothing
    Dim ImageDrivers() As List(Of Driver) = Nothing
    For Each lvRow As ListViewItem In lvImages.Items
      Dim lvIndex As Integer = CInt(lvRow.Tag)
      If lvRow.Checked Then
        ReDim Preserve ImageIntegratedUpdates(iProgVal)
        ReDim Preserve ImageFeatures(iProgVal)
        ReDim Preserve ImageDrivers(iProgVal)
        iProgVal += 1
        Progress_Normal(iProgVal, iProgMax, True)
        Dim RowIndex As Integer = CInt(lvRow.Text)
        Dim RowName As String = ImagePackage_ListData(lvIndex).NewName
        If String.IsNullOrEmpty(RowName) Then RowName = lvRow.SubItems(1).Text
        Dim RowDesc As String = ImagePackage_ListData(lvIndex).NewDesc
        If String.IsNullOrEmpty(RowDesc) Then RowDesc = ImagePackage_ListData(lvIndex).Package.Desc
        Dim RowImage As String
        If ImagePackage_ListData(lvIndex).Group = WIMGroup.WIM Then
          RowImage = WIMFile
        ElseIf ImagePackage_ListData(lvIndex).Group = WIMGroup.Merge Then
          RowImage = MergeWIM
        Else
          Continue For
        End If
        ImageIntegratedUpdates(iProgVal - 1) = ImagePackage_ListData(lvIndex).Package.IntegratedUpdateList
        ImageFeatures(iProgVal - 1) = ImagePackage_ListData(lvIndex).FeatureList
        ImageDrivers(iProgVal - 1) = ImagePackage_ListData(lvIndex).DriverList
        Status_SetText(String.Format("Merging WIM ""{0}""...", RowName))
        If Not ImageX_Export(RowImage, RowIndex, NewWIM, RowName, RowDesc) Then
          GUI_ToggleEnabled(True, String.Format("Failed to Merge WIM ""{0}""", RowName))
          Return
        End If
      End If
      If StopRun Then
        GUI_ToggleEnabled(True)
        Return
      End If
    Next
    If IO.Directory.Exists(MergeWork) Then
      Status_SetText("Clearing Temporary Files...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MergeWork))
      Try
        SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
      Catch ex As Exception
      End Try
    End If
    Progress_Normal(0, 1)
    iTotalVal += 1 ' +1 Back up the original INSTALL.WIM
    Progress_Total(iTotalVal, iTotalMax)
    iProgMax = 5
    iProgVal = 1
    Progress_Normal(iProgVal, iProgMax, True)
    Status_SetText("Making Backup of Old WIM...")
    Dim BakWIM As String = IO.Path.Combine(WorkDir, String.Concat(IO.Path.GetFileNameWithoutExtension(WIMFile), "_BAK.WIM"))
    ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", WIMFile, BakWIM))
    If Not SlowCopyFile(WIMFile, BakWIM, AddressOf Progress_Normal_Sub, True) Then
      GUI_ToggleEnabled(True, "Failed to back up Install WIM!")
      Return
    End If
    iProgVal += 1
    Progress_Normal(iProgVal, iProgMax, True)
    Status_SetText("Moving Generated WIM...")
    ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", NewWIM, WIMFile))
    If Not SlowCopyFile(NewWIM, WIMFile, AddressOf Progress_Normal_Sub, True) Then
      Progress_Total(0, 100)
      Progress_Normal(0, 1)
      Status_SetText("Generated WIM Move Failed! Reverting to Old WIM...")
      ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", BakWIM, WIMFile))
      If Not SlowCopyFile(BakWIM, WIMFile, AddressOf Progress_Normal, True) Then
        GUI_ToggleEnabled(True, "Generated WIM Move Failed! Original WIM Restore Failed!")
      Else
        GUI_ToggleEnabled(True, "Generated WIM Move Failed! Original WIM Restored.")
      End If
      Return
    End If
    iProgVal += 1
    Progress_Normal(iProgVal, iProgMax, True)
    If IO.File.Exists(BakWIM) Then
      Status_SetText("Cleaning Up Backup WIM...")
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", BakWIM))
      IO.File.Delete(BakWIM)
    End If
    iProgVal += 1
    Progress_Normal(iProgVal, iProgMax, True)
    Application.DoEvents()
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return
    End If
    TitleMNG_SetDisplay(TitleMNG_List.Move)
    TitleMNG_SetTitle("Preparing Files", "Checking updates and Service Packs to integrate...")
    Status_SetText("Checking Service Pack...")
    Dim SPFile As String = Nothing
    If chkSP.Checked Then
      If String.IsNullOrEmpty(txtSP.Text) OrElse Not IO.File.Exists(txtSP.Text) Then
        GUI_ToggleEnabled(True, "Missing Service Pack File!")
        txtSP.Focus()
        Beep()
        Return
      Else
        SPFile = txtSP.Text
      End If
    Else
      SPFile = Nothing
    End If
    Dim SP64File As String = Nothing
    If pnlSP64.Visible Then
      If String.IsNullOrEmpty(txtSP64.Text) Then
        SP64File = Nothing
      ElseIf Not IO.File.Exists(txtSP64.Text) Then
        GUI_ToggleEnabled(True, "Missing x64 Service Pack File!")
        txtSP64.Focus()
        Beep()
        Return
      Else
        SP64File = txtSP64.Text
      End If
    Else
      SP64File = Nothing
    End If
    iProgVal += 1
    Progress_Normal(iProgVal, iProgMax, True)
    Status_SetText("Collecting Update List...")
    Dim UpdateFiles As New Dictionary(Of Update_File, Dictionary(Of String, Boolean))
    Dim BootDriverFiles As New Dictionary(Of Update_File, Dictionary(Of String, Boolean))
    Dim PEDriverFiles As New Dictionary(Of Update_File, Dictionary(Of String, Boolean))
    Dim iSubMax As Integer = 0
    For Each lvItem As ListViewItem In lvMSU.Items
      For I As Integer = 0 To lvImages.Items.Count - 1
        iSubMax += lvMSU.Items.Count
        Dim lvIndex2 As Integer = CInt(lvImages.Items(I).Tag)
        Dim imageID As String = ImagePackage_ListData(lvIndex2).Package.ToString
        Dim MSUList As List(Of Update_Integrated) = ImagePackage_ListData(lvIndex2).Package.IntegratedUpdateList
        If MSUList IsNot Nothing Then
          iSubMax += MSUList.Count
        End If
      Next
    Next
    Dim iSubVal As Integer = -1
    If lvMSU.Items.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        Dim lvIndex As Integer = CInt(lvItem.Tag)
        Dim doReplace As New Dictionary(Of String, Boolean)
        For I As Integer = 0 To lvImages.Items.Count - 1
          Dim lvIndex2 As Integer = CInt(lvImages.Items(I).Tag)
          Dim imageID As String = ImagePackage_ListData(lvIndex2).Package.ToString
          Dim MSUList As List(Of Update_Integrated) = ImagePackage_ListData(lvIndex2).Package.IntegratedUpdateList
          If MSUList IsNot Nothing Then
            Dim bFound As Boolean = False
            For J As Integer = 0 To MSUList.Count - 1
              iSubVal += 1
              Progress_Normal_Sub(iSubVal, iSubMax, "Comparing Updates")
              If MSUList(J).Ident = Updates_ListData(lvIndex).Update.Ident Then
                bFound = True
                Select Case Updates_ListData(lvIndex).ReplaceStyle
                  Case Updates_Data.Update_Replace.All
                    If CompareMSVersions(MSUList(J).Ident.Version, Updates_ListData(lvIndex).Update.Ident.Version) = 0 Then
                      doReplace.Add(imageID, True)
                    Else
                      doReplace.Add(imageID, False)
                    End If
                  Case Updates_Data.Update_Replace.OnlyNewer
                    If CompareMSVersions(MSUList(J).Ident.Version, Updates_ListData(lvIndex).Update.Ident.Version) > 0 Then
                      doReplace.Add(imageID, True)
                    Else
                      doReplace.Add(imageID, False)
                    End If
                  Case Updates_Data.Update_Replace.OnlyOlder
                    If CompareMSVersions(MSUList(J).Ident.Version, Updates_ListData(lvIndex).Update.Ident.Version) < 0 Then
                      doReplace.Add(imageID, True)
                    Else
                      doReplace.Add(imageID, False)
                    End If
                  Case Updates_Data.Update_Replace.OnlyMissing
                    doReplace.Add(imageID, False)
                End Select
                Exit For
              End If
            Next
            If Not bFound Then
              If Updates_ListData(lvIndex).ReplaceStyle = Updates_Data.Update_Replace.OnlyMissing Then
                doReplace.Add(imageID, True)
              Else
                doReplace.Add(imageID, False)
              End If
            End If
          Else
            If Updates_ListData(lvIndex).ReplaceStyle = Updates_Data.Update_Replace.OnlyMissing Then
              doReplace.Add(imageID, True)
            Else
              doReplace.Add(imageID, False)
            End If
          End If
        Next
        UpdateFiles.Add(Updates_ListData(lvIndex).Update, doReplace)
        If Updates_ListData(lvIndex).Update.Name = "DRIVER" Then
          If Updates_ListData(lvIndex).Update.DriverData.IntegrateIntoBootSetup Then BootDriverFiles.Add(Updates_ListData(lvIndex).Update, doReplace)
          If Updates_ListData(lvIndex).Update.DriverData.IntegrateIntoBootPE Then PEDriverFiles.Add(Updates_ListData(lvIndex).Update, doReplace)
        End If
      Next
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return
    End If
    Progress_Normal(0, 1)
    iTotalVal += 1 ' +1 Read the number of image packages in the new INSTALL.WIM
    Progress_Total(iTotalVal, iTotalMax)
    Status_SetText("Reading Image Package Count...")
    Dim wimPackageCount As Integer = DISM_Image_GetCount(WIMFile)
    Progress_Normal(0, 1)
    TitleMNG_SetDisplay(TitleMNG_List.Copy)
    TitleMNG_SetTitle("Adding Service Packs", "Integrating Service Pack data into WIM packages...")
    If Not String.IsNullOrEmpty(SPFile) Then
      If String.IsNullOrEmpty(SP64File) Then
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Integrate the (only) Service Pack 1
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Integrating Service Pack...")
        If Integrate_ServicePack(WIMFile, wimPackageCount, SPFile) Then
          Status_SetText("Service Pack Integrated!")
        Else
          Dim sMsg As String = Status_GetText()
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return
        End If
      Else
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Integrate the (32-bit) Service Pack 1
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Integrating x86 Service Pack...")
        If Integrate_ServicePack(WIMFile, wimPackageCount, SPFile, "86") Then
          Status_SetText("x86 Service Pack Integrated!")
        Else
          Dim sMsg As String = Status_GetText()
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return
        End If
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Integrate the 64-bit Service Pack 1
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Integrating x64 Service Pack...")
        If Integrate_ServicePack(WIMFile, wimPackageCount, SP64File, "64") Then
          Status_SetText("x64 Service Pack Integrated!")
        Else
          Dim sMsg As String = Status_GetText()
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return
        End If
      End If
    End If
    If DoFeatures Then
      Progress_Normal(0, 1)
      iTotalVal += 1 '+1 Change Integrated Features
      Progress_Total(iTotalVal, iTotalMax)
      TitleMNG_SetDisplay(TitleMNG_List.Copy)
      TitleMNG_SetTitle("Toggling Windows Features", "Enabling and disabling selected Features in WIM packages...")
      Status_SetText("Toggling Features...")
      If Integrate_ToggleFeatures(WIMFile, wimPackageCount, ImageFeatures.ToArray) Then
        Status_SetText("Features Toggled!")
      Else
        Dim sMsg As String = Status_GetText()
        GUI_ToggleEnabled(True, sMsg)
        Return
      End If
      If StopRun Then
        If IO.Directory.Exists(Mount) Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount)
        End If
        GUI_ToggleEnabled(True)
        Return
      End If
    End If
    If DoIntegratedUpdates Then
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Remove Integrated Updates
      Progress_Total(iTotalVal, iTotalMax)
      TitleMNG_SetDisplay(TitleMNG_List.Copy)
      TitleMNG_SetTitle("Removing Integrated Updates", "Removing selected integrated Windows Updates from WIM packages...")
      Status_SetText("Removing Updates...")
      If Integrate_RemoveUpdates(WIMFile, wimPackageCount, ImageIntegratedUpdates.ToArray) Then
        Status_SetText("Updates Removed!")
      Else
        Dim sMsg As String = Status_GetText()
        GUI_ToggleEnabled(True, sMsg)
        Return
      End If
      If StopRun Then
        If IO.Directory.Exists(Mount) Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount)
        End If
        GUI_ToggleEnabled(True)
        Return
      End If
    End If
    If DoIntegratedDrivers Then
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Remove Integrated Drivers
      Progress_Total(iTotalVal, iTotalMax)
      TitleMNG_SetDisplay(TitleMNG_List.Copy)
      TitleMNG_SetTitle("Removing Drivers", "Removing selected integrated Drivers from WIM packages...")
      Status_SetText("Removing Drivers...")
      If Integrate_RemoveDrivers(WIMFile, wimPackageCount, ImageDrivers) Then
        Status_SetText("Drivers Removing!")
      Else
        Dim sMsg As String = Status_GetText()
        GUI_ToggleEnabled(True, sMsg)
        Return
      End If
    End If
    TitleMNG_SetDisplay(TitleMNG_List.Copy)
    TitleMNG_SetTitle("Adding Windows Updates", "Integrating update data into WIM packages...")
    Dim NoMount As Boolean = True
    If UpdateFiles.Count > 0 Then
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Read Update Information
      Progress_Total(iTotalVal, iTotalMax)
      Status_SetText("Integrating Updates...")
      If Integrate_Files(WIMFile, wimPackageCount, UpdateFiles.ToArray, iTotalVal, iTotalMax, Integrate_Files_WIM.INSTALL) Then ' +1 Integrate Updates
        Status_SetText("Updates Integrated!")
        NoMount = False
      Else
        Dim sMsg As String = Status_GetText()
        GUI_ToggleEnabled(True, sMsg)
        Return
      End If
      If StopRun Then
        If IO.Directory.Exists(Mount) Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount)
        End If
        GUI_ToggleEnabled(True)
        Return
      End If
      If modifyPE Then
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Read Driver Information for PE
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Integrating PE Drivers...")
        If Integrate_Files(BOOTWIMFile, bootPackageCount, PEDriverFiles.ToArray, iTotalVal, iTotalMax, Integrate_Files_WIM.BOOT_PE) Then ' +1 Integrate PE Drivers
          Status_SetText("PE Drivers Integrated!")
        Else
          Dim sMsg As String = Status_GetText()
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        If StopRun Then
          If IO.Directory.Exists(Mount) Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount)
          End If
          GUI_ToggleEnabled(True)
          Return
        End If
      End If
      If modifyBoot Then
        Progress_Normal(0, 1)
        iTotalVal += 1 ' +1 Read Driver Information for Installer
        Progress_Total(iTotalVal, iTotalMax)
        Status_SetText("Integrating Boot Drivers...")
        If Integrate_Files(BOOTWIMFile, bootPackageCount, BootDriverFiles.ToArray, iTotalVal, iTotalMax, Integrate_Files_WIM.BOOT_INSTALLER) Then ' +1 Integrate Installer Drivers
          Status_SetText("Boot Drivers Integrated!")
        Else
          Dim sMsg As String = Status_GetText()
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        If StopRun Then
          If IO.Directory.Exists(Mount) Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount)
          End If
          GUI_ToggleEnabled(True)
          Return
        End If
      End If
    End If
    If NoMount AndAlso (Not UEFI_IsEnabled AndAlso chkUEFI.Checked) Then
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Mount image for UEFI purposes if the image isn't mounted already
      Progress_Total(iTotalVal, iTotalMax)
      'todo: progress needs more progress
      Status_SetText("Mounting Final WIM for UEFI boot data...")
      DISM_Image_Load(WIMFile, wimPackageCount, Mount)
      NoMount = False
    End If
    Progress_Normal(0, 1)
    iTotalVal += 1 ' +1 IF Just a WIM File, Finalize WIM; ELSE IF Full ISO Deal, Prepare Language Data and UEFI
    Progress_Total(iTotalVal, iTotalMax)
    TitleMNG_SetDisplay(TitleMNG_List.Move)
    If String.IsNullOrEmpty(ISOFile) Then
      REM Just a WIM File
      TitleMNG_SetTitle("Generating WIM", "Preparing WIMs and file structure...")
      If Not NoMount Then
        Progress_Normal(0, 1)
        Status_SetText("Saving Final Image Package...")
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", "Failed to Save Final Image Package!")
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, "Failed to Save Final Image Package!")
          Return
        End If
      End If
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Back up the old WIM
      Progress_Total(iTotalVal, iTotalMax)
      Status_SetText("Making Backup of Old WIM...")
      Dim OldWIM As String = IO.Path.Combine(IO.Path.GetDirectoryName(WIMFile), IO.Path.ChangeExtension(String.Format("{0}_OLD", IO.Path.GetFileNameWithoutExtension(WIMFile)), IO.Path.GetExtension(WIMFile)))
      Progress_Normal(0, 1)
      ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", WIMFile, OldWIM))
      If Not SlowCopyFile(WIMFile, OldWIM, AddressOf Progress_Normal, True) Then
        GUI_ToggleEnabled(True, "Failed to back up Install WIM!")
        Return
      End If
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Compress Finalized WIM to the old WIM's location
      Progress_Total(iTotalVal, iTotalMax)
      Progress_Normal(0, 0)
      Status_SetText("Reading INSTALL.WIM Package Count...")
      Dim NewWIMPackageCount As Integer = DISM_Image_GetCount(OldWIM)
      Progress_Normal(0, NewWIMPackageCount * 2)
      For I As Integer = 1 To NewWIMPackageCount
        Progress_Normal(I * 2 - 1, wimPackageCount * 2, True)
        Dim NewWIMPackageInfo As ImagePackage = DISM_Image_GetData(OldWIM, I)
        Dim RowIndex As Integer = NewWIMPackageInfo.Index
        Dim RowName As String = NewWIMPackageInfo.Name
        Dim RowDesc As String = NewWIMPackageInfo.Desc
        Progress_Normal(I * 2, wimPackageCount * 2, True)
        Status_SetText(String.Format("Integrating and Compressing INSTALL.WIM Package ""{0}""...", RowName))
        If Not ImageX_Export(OldWIM, RowIndex, WIMFile, RowName, RowDesc) Then
          GUI_ToggleEnabled(True, String.Format("Failed to Compress WIM ""{0}""", RowName))
          Return
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return
        End If
      Next
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", OldWIM))
      IO.File.Delete(OldWIM)
      If cmbLimitType.SelectedIndex = 1 Then
        If IO.File.Exists(WIMFile) Then
          Dim limVal As String = cmbLimit.Text
          If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
          Dim splFileSize As Long = NumericVal(limVal)
          Dim splByteSize As Long = splFileSize * 1024 * 1024
          Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(WIMFile).Length
          If splWIMSize > splByteSize Then
            REM Split WIMs to FileSize size
            Status_SetText(String.Format("Splitting WIM into {0} Chunks...", ByteSize(splByteSize)))
            If Not ImageX_Split(WIMFile, IO.Path.ChangeExtension(WIMFile, ".swm"), splFileSize) Then
              GUI_ToggleEnabled(True, "Failed to Split WIM!")
              Return
            End If
            ConsoleOutput_Write(String.Format("Deleting ""{0}""...", WIMFile))
            IO.File.Delete(WIMFile)
          Else
            ConsoleOutput_Write(String.Format("The finalized INSTALL.WIM file is already smaller than the split size of {0}!", ByteSize(splByteSize)))
          End If
        End If
      End If
    Else
      REM Full ISO Deal
      TitleMNG_SetTitle("Generating ISO", "Preparing WIMs and file structures, and writing ISO data...")
      Dim ISODir As String = IO.Path.Combine(Work, "ISO")
      Dim ISODDir As String = IO.Path.Combine(Work, "ISOD%n")
      If Not IO.Directory.Exists(ISODir) Then IO.Directory.CreateDirectory(ISODir)
      Progress_Normal(0, 1)
      Status_SetText("Extracting ISO contents...")
      Extract_AllFiles_Except(ISOFile, ISODir, "install.wim", "Setup Disc")
      Status_SetText("Erasing Catalog Files...")
      Try
        For Each sourceFileName As String In IO.Directory.EnumerateFiles(IO.Path.Combine(ISODir, "sources"))
          If Not IO.Path.GetExtension(sourceFileName).ToLower = ".clg" Then Continue For
          ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(ISODir, "sources", sourceFileName)))
          Try
            IO.File.Delete(IO.Path.Combine(ISODir, "sources", sourceFileName))
          Catch dex As Exception
            ConsoleOutput_Write(String.Format("Error deleting ""{0}""!", IO.Path.Combine(ISODir, "sources")))
          End Try
        Next
      Catch ex As Exception
        ConsoleOutput_Write(String.Format("Error iterating through ""{0}""!", IO.Path.Combine(ISODir, "sources")))
      End Try
      If chkUnlock.Checked Then
        Status_SetText("Unlocking All Editions...")
        If IO.File.Exists(IO.Path.Combine(ISODir, "sources", "ei.cfg")) Then
          ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(ISODir, "sources", "ei.cfg")))
          Try
            IO.File.Delete(IO.Path.Combine(ISODir, "sources", "ei.cfg"))
          Catch ex As Exception
            ConsoleOutput_Write(String.Format("Error deleting ""{0}""!", IO.Path.Combine(ISODir, "sources", "ei.cfg")))
            chkUnlock.Checked = False
          End Try
        End If
      End If
      If IO.Directory.Exists(IO.Path.Combine(ISODir, "[BOOT]")) Then
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(ISODir, "[BOOT]")))
        Try
          SlowDeleteDirectory(IO.Path.Combine(ISODir, "[BOOT]"), FileIO.DeleteDirectoryOption.DeleteAllContents, False)
        Catch ex As Exception
          ConsoleOutput_Write(String.Format("Failed to delete ""{0}""!", IO.Path.Combine(ISODir, "[BOOT]")))
        End Try
      End If
      If Integrate_LanguageChanged Then
        Progress_Normal(0, 0)
        Status_SetText("Updating Language INIs...")
        Progress_Normal(0, 1)
        If Not ImageX_UpdateLanguage(ISODir) Then
          Dim sMsg As String = Status_GetText()
          If Not NoMount Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount)
          End If
          GUI_ToggleEnabled(True, sMsg)
          Return
        End If
        Status_SetText("Updated Language INIs!")
        If StopRun Then
          If Not NoMount Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount)
          End If
          GUI_ToggleEnabled(True)
          Return
        End If
      End If
      If modifyBoot Or modifyPE Then
        Status_SetText("Moving Boot Image...")
        Dim ISOBOOTWIMFile As String = IO.Path.Combine(ISODir, "sources", "boot.wim")
        ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", BOOTWIMFile, ISOBOOTWIMFile))
        If SlowCopyFile(BOOTWIMFile, ISOBOOTWIMFile, True) Then
          Status_SetText("Boot Image Updated!")
        Else
          Status_SetText("Failed to copy Boot Image!")
        End If
      End If
      If Not UEFI_IsEnabled AndAlso chkUEFI.Checked Then
        Status_SetText("Enabling UEFI Boot...")
        Dim sEFIBootDir As String = IO.Path.Combine(ISODir, "efi", "microsoft", "boot")
        Dim sUEFIBootDir As String = IO.Path.Combine(ISODir, "efi", "boot")
        Dim sEFIFile As String = IO.Path.Combine(Mount, "Windows", "Boot", "EFI", "bootmgfw.efi")
        Dim sUEFIFile As String = IO.Path.Combine(sUEFIBootDir, "bootx64.efi")
        If IO.Directory.Exists(sEFIBootDir) Then
          If Not IO.Directory.Exists(sUEFIBootDir) Then
            ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", sEFIBootDir, sUEFIBootDir))
            My.Computer.FileSystem.CopyDirectory(sEFIBootDir, sUEFIBootDir, True)
            If IO.File.Exists(sEFIFile) Then
              ConsoleOutput_Write(String.Format("Copying ""{0}"" to ""{1}""...", sEFIFile, sUEFIFile))
              If SlowCopyFile(sEFIFile, sUEFIFile, False) Then
                Status_SetText("UEFI Boot Enabled!")
              Else
                Status_SetText("Failed to copy UEFI file!")
              End If
            Else
              ConsoleOutput_Write(String.Format("Deleting ""{0}""...", sUEFIBootDir))
              SlowDeleteDirectory(sUEFIBootDir, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
              chkUEFI.Checked = False
              MsgDlg(Me, String.Format("Unable to find the file ""{0}"" in Image Package!", String.Concat(IO.Path.DirectorySeparatorChar, IO.Path.Combine("Windows", "Boot", "EFI", "bootmgfw.efi"))), "Unable to enable UEFI Boot.", "File was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , , "EFI File Not Found")
            End If
          Else
            chkUEFI.Checked = False
            Status_SetText("UEFI Boot already enabled!")
          End If
        Else
          chkUEFI.Checked = False
          MsgDlg(Me, String.Format("Unable to find the folder ""{0}"" in ISO!", String.Concat(IO.Path.DirectorySeparatorChar, IO.Path.Combine("efi", "microsoft", "boot"))), "Unable to enable UEFI Boot.", "Folder was not found.", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, , , "EFI Folder Not Found")
        End If
      End If
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Finalize WIM
      Progress_Total(iTotalVal, iTotalMax)
      If Not NoMount Then
        Progress_Normal(0, 1)
        Status_SetText("Saving Final Image Package...")
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", "Failed to Save Final Image Package!")
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, "Failed to Save Final Image Package!")
          Return
        End If
      End If
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Compress Finalized WIM
      Progress_Total(iTotalVal, iTotalMax)
      Status_SetText("Integrating and Compressing INSTALL.WIM...")
      Dim ISOWIMFile As String = IO.Path.Combine(ISODir, "sources", "install.wim")
      If IO.File.Exists(ISOWIMFile) Then
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOWIMFile))
        IO.File.Delete(ISOWIMFile)
      End If
      Dim isoUsedSpace As ULong = GetDirSize(ISODir)
      Dim isoUsedMB As Long = CLng(isoUsedSpace / 1024 / 1024)
      Progress_Normal(0, wimPackageCount * 2)
      For I As Integer = 1 To wimPackageCount
        Progress_Normal(I * 2 - 1, wimPackageCount * 2, True)
        Dim NewWIMPackageInfo As ImagePackage = DISM_Image_GetData(WIMFile, I)
        Dim RowIndex As Integer = NewWIMPackageInfo.Index
        If RowIndex = 0 Then
          GUI_ToggleEnabled(True, String.Format("Failed to Parse WIM File ""{0}"" item #{1}", WIMFile, I))
          Return
        End If
        If Not RowIndex = I Then
          Debug.Print("Index Mismatch or something?")
        End If
        Dim RowName As String = NewWIMPackageInfo.Name
        Dim RowDesc As String = NewWIMPackageInfo.Desc
        Progress_Normal(I * 2, wimPackageCount * 2, True)
        Status_SetText(String.Format("Integrating and Compressing INSTALL.WIM Package ""{0}""...", RowName))
        If Not ImageX_Export(WIMFile, RowIndex, ISOWIMFile, RowName, RowDesc) Then
          GUI_ToggleEnabled(True, String.Format("Failed to Integrate WIM Package ""{0}""", RowName))
          Return
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return
        End If
      Next

      If cmbLimitType.SelectedIndex = 0 Then
        If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
          Status_SetText("Looking for oversized files...")
          Dim BigList = FindBigFiles(ISODir)
          If BigList.Count > 0 Then
            Using fsAlert As New frmFS(BigList, ISODir, CByte(cmbISOFormat.SelectedIndex))
              Select Case fsAlert.ShowDialog(Me)
                Case Windows.Forms.DialogResult.Yes
                  REM UDF
                  cmbISOFormat.SelectedIndex = 3
                Case Windows.Forms.DialogResult.OK
                  REM UDF102
                  cmbISOFormat.SelectedIndex = 5
                Case Windows.Forms.DialogResult.No
                  REM Default
                Case Windows.Forms.DialogResult.Cancel
                  REM STOP
                  GUI_ToggleEnabled(True, "ISO creation cancelled by user!")
                  Return
              End Select
            End Using
          End If
        End If
      ElseIf cmbLimitType.SelectedIndex = 1 Then
        Dim limVal As String = cmbLimit.Text
        If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
        Dim splFileSize As Long = NumericVal(limVal)
        Dim splByteSize As Long = splFileSize * 1024 * 1024
        Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(ISOWIMFile).Length
        If splWIMSize <= splByteSize Then
          ConsoleOutput_Write(String.Format("The finalized INSTALL.WIM file is already smaller than the split size of {0}!", ByteSize(splByteSize)))
        Else
          REM Split WIM to FileSize size
          Progress_Normal(0, 1)
          iTotalVal += 1 ' +1 Split WIM
          Progress_Total(iTotalVal, iTotalMax)
          Progress_Normal(0, 0)
          Status_SetText(String.Format("Splitting WIM into {0} Chunks...", ByteSize(splByteSize)))
          If Not ImageX_Split(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), splFileSize) Then
            GUI_ToggleEnabled(True, "Failed to Split WIM!")
            Return
          End If
          If IO.File.Exists(ISOWIMFile) Then
            ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOWIMFile))
            IO.File.Delete(ISOWIMFile)
          End If
        End If
      ElseIf cmbLimitType.SelectedIndex = 2 Then
        Dim limVal As String = cmbLimit.Text
        If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
        Dim splFileSize As Long = NumericVal(limVal)
        Dim splByteSize As Long = splFileSize * 1024 * 1024
        Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(ISOWIMFile).Length
        Dim splISOSize As Long = splWIMSize + isoUsedMB
        If splISOSize <= splByteSize Then
          ConsoleOutput_Write(String.Format("The expected ISO size with the finalized INSTALL.WIM file is already smaller than the split size of {0}!", ByteSize(splByteSize)))
        Else
          Progress_Normal(0, 1)
          iTotalVal += 1 ' +1 Split WIM
          Progress_Total(iTotalVal, iTotalMax)
          Dim ISOSplit As Long = splFileSize
          Dim WIMSplit As Long = ISOSplit - isoUsedMB
          If Math.Floor(ISOSplit / WIMSplit) > 1 Then
            REM Split WIM to WIMSplit size, put 1 WIM on first, and Math.Floor(ISOSplit / WIMSplit) per ISO afterward
            Progress_Normal(0, 0)
            Status_SetText(String.Format("Splitting WIM into {0} Chunks...", ByteSize(WIMSplit * 1024 * 1024)))
            If Not ImageX_Split(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
              GUI_ToggleEnabled(True, "Failed to Split WIM!")
              Return
            End If
            If IO.File.Exists(ISOWIMFile) Then
              ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOWIMFile))
              IO.File.Delete(ISOWIMFile)
            End If
            Progress_Normal(0, 1)
            Status_SetText("Generating Data ISOs...")
            Dim DiscNumber As Integer = 1
            Dim WIMNumber As Integer = 0
            Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(IO.Path.Combine(ISODir, "sources"), FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
            SortFiles(FilesInOrder)
            For I As Integer = 0 To FilesInOrder.Count - 1
              Dim File As String = FilesInOrder(I)
              If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                Continue For
              ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                WIMNumber += 1
                Dim sIDir As String = ISODDir.Replace("%n", CStr(DiscNumber))
                If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                If Not IO.Directory.Exists(IO.Path.Combine(sIDir, "sources")) Then IO.Directory.CreateDirectory(IO.Path.Combine(sIDir, "sources"))
                Progress_Normal(0, 1)
                Dim sNewFile As String = IO.Path.Combine(sIDir, "sources", IO.Path.GetFileName(File))
                ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", File, sNewFile))
                If Not SlowCopyFile(File, sNewFile, AddressOf Progress_Normal, True) Then
                  GUI_ToggleEnabled(True, String.Format("Failed to move Install WIM #{0}!", WIMNumber))
                  Return
                End If
                If WIMNumber = Math.Floor(ISOSplit / WIMSplit) Or I = FilesInOrder.Count - 1 Then
                  Extract_AllFiles(IO.Path.Combine(Application.StartupPath, "AR.zip"), sIDir)
                  If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                    Status_SetText("Looking for oversized files...")
                    Dim BigList = FindBigFiles(sIDir)
                    If BigList.Count > 0 Then
                      Using fsAlert As New frmFS(BigList, sIDir, CByte(cmbISOFormat.SelectedIndex))
                        Select Case fsAlert.ShowDialog(Me)
                          Case Windows.Forms.DialogResult.Yes
                            REM UDF
                            cmbISOFormat.SelectedIndex = 3
                          Case Windows.Forms.DialogResult.OK
                            REM UDF102
                            cmbISOFormat.SelectedIndex = 5
                          Case Windows.Forms.DialogResult.No
                            REM Default
                          Case Windows.Forms.DialogResult.Cancel
                            REM STOP
                            GUI_ToggleEnabled(True, "ISO creation cancelled by user!")
                            Return
                        End Select
                      End Using
                    End If
                  End If
                  Dim DiscIDNo As String = CStr(DiscNumber)
                  Dim DiscFiles() As String = My.Computer.FileSystem.GetFiles(IO.Path.Combine(sIDir, "sources"), FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                  SortFiles(DiscFiles)
                  DiscIDNo = CStr(NumericVal(IO.Path.GetFileNameWithoutExtension(DiscFiles(0))))
                  Dim ISODFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(ISOFile), IO.Path.ChangeExtension(String.Format("{0}_D{1}", IO.Path.GetFileNameWithoutExtension(ISOFile), DiscIDNo), IO.Path.GetExtension(ISOFile)))
                  Status_SetText(String.Format("Building Data ISO {0} (Labeled as Installation disc {1})...", DiscNumber, DiscIDNo))
                  Try
                    My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                    Dim isoLabel As String = txtISOLabel.Text
                    If isoLabel.Length > 32 - (5 + DiscIDNo.ToString.Length) Then
                      isoLabel = String.Format("{0}_DISC{1}", isoLabel.Substring(0, 32 - (5 + DiscIDNo.ToString.Length)), DiscIDNo)
                    Else
                      isoLabel = String.Format("{0}_DISC{1}", isoLabel, DiscIDNo)
                    End If
                    If isoLabel.Contains(" ") Then isoLabel = Replace(isoLabel, " ", "")
                    Dim isoFormat As String = "-n"
                    Select Case cmbISOFormat.SelectedIndex
                      Case 0 : isoFormat = "-n"
                      Case 2 : isoFormat = "-j1"
                      Case 1 : isoFormat = "-j2"
                      Case 4 : isoFormat = "-u1"
                      Case 3 : isoFormat = "-u2"
                      Case 6 : isoFormat = "-u1 -udfver102"
                      Case 5 : isoFormat = "-u2 -udfver102"
                    End Select
                    OSCDIMG_MakeISO(sIDir, ISODFile, isoLabel, isoFormat)
                  Catch ex As Exception
                  End Try
                  DiscNumber += 1
                  WIMNumber = 0
                End If
              End If
            Next
          ElseIf Math.Floor(ISOSplit / WIMSplit) = 1 Then
            REM Split WIM to WIMSplit size, put 1 WIM on each ISO including first
            Progress_Normal(0, 0)
            Status_SetText(String.Format("Splitting WIM into {0} Chunks...", ByteSize(WIMSplit * 1024 * 1024)))
            If Not ImageX_Split(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
              GUI_ToggleEnabled(True, "Failed to Split WIM!")
              Return
            End If
            If Not ImageX_Split(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
              GUI_ToggleEnabled(True, "Failed to Split WIM!")
              Return
            End If
            If IO.File.Exists(ISOWIMFile) Then
              ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOWIMFile))
              IO.File.Delete(ISOWIMFile)
            End If
            Progress_Normal(0, 1)
            Status_SetText("Generating Data ISOs...")
            Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(IO.Path.Combine(ISODir, "sources"), FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
            SortFiles(FilesInOrder)
            FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
            For Each File As String In FilesInOrder
              If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                Continue For
              ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                Dim discNo As String = IO.Path.GetFileNameWithoutExtension(File).Substring(7)
                Dim sIDir As String = ISODDir.Replace("%n", discNo)
                If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                If Not IO.Directory.Exists(IO.Path.Combine(sIDir, "sources")) Then IO.Directory.CreateDirectory(IO.Path.Combine(sIDir, "sources"))
                Progress_Normal(0, 1)
                Dim sNewFile As String = IO.Path.Combine(sIDir, "sources", IO.Path.GetFileName(File))
                ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", File, sNewFile))
                If Not SlowCopyFile(File, sNewFile, AddressOf Progress_Normal, True) Then
                  GUI_ToggleEnabled(True, String.Format("Failed to move Install WIM #{0}!", discNo))
                  Return
                End If
                Extract_AllFiles(IO.Path.Combine(Application.StartupPath, "AR.zip"), sIDir)
                If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                  Status_SetText("Looking for oversized files...")
                  Dim BigList = FindBigFiles(sIDir)
                  If BigList.Count > 0 Then
                    Using fsAlert As New frmFS(BigList, sIDir, CByte(cmbISOFormat.SelectedIndex))
                      Select Case fsAlert.ShowDialog(Me)
                        Case Windows.Forms.DialogResult.Yes
                          REM UDF
                          cmbISOFormat.SelectedIndex = 3
                        Case Windows.Forms.DialogResult.OK
                          REM UDF102
                          cmbISOFormat.SelectedIndex = 5
                        Case Windows.Forms.DialogResult.No
                          REM Default
                        Case Windows.Forms.DialogResult.Cancel
                          REM STOP
                          GUI_ToggleEnabled(True, "ISO creation cancelled by user!")
                          Return
                      End Select
                    End Using
                  End If
                End If
                Dim ISODFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(ISOFile), IO.Path.ChangeExtension(String.Format("{0}_D{1}", IO.Path.GetFileNameWithoutExtension(ISOFile), discNo), IO.Path.GetExtension(ISOFile)))
                Status_SetText(String.Format("Building Data ISO {0}...", discNo))
                Try
                  My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                  Dim isoLabel As String = txtISOLabel.Text
                  If isoLabel.Length > 32 - (5 + discNo.ToString.Length) Then
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel.Substring(0, 32 - (5 + discNo.ToString.Length)), discNo)
                  Else
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel, discNo)
                  End If
                  If isoLabel.Contains(" ") Then isoLabel = Replace(isoLabel, " ", "")
                  Dim isoFormat As String = "-n"
                  Select Case cmbISOFormat.SelectedIndex
                    Case 0 : isoFormat = "-n"
                    Case 2 : isoFormat = "-j1"
                    Case 1 : isoFormat = "-j2"
                    Case 4 : isoFormat = "-u1"
                    Case 3 : isoFormat = "-u2"
                    Case 6 : isoFormat = "-u1 -udfver102"
                    Case 5 : isoFormat = "-u2 -udfver102"
                  End Select
                  OSCDIMG_MakeISO(sIDir, ISODFile, isoLabel, isoFormat)
                Catch ex As Exception
                End Try
              End If
            Next
          Else
            REM Split WIM to WIMSplit size, put no WIMs on first ISO, one per ISO afterward
            Progress_Normal(0, 0)
            Status_SetText(String.Format("Splitting WIM into {0} Chunks...", ByteSize(WIMSplit * 1024 * 1024)))
            If Not ImageX_Split(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
              GUI_ToggleEnabled(True, "Failed to Split WIM!")
              Return
            End If
            If IO.File.Exists(ISOWIMFile) Then
              ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOWIMFile))
              IO.File.Delete(ISOWIMFile)
            End If
            Progress_Normal(0, 1)
            Status_SetText("Generating Data ISOs...")
            Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(IO.Path.Combine(ISODir, "sources"), FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
            SortFiles(FilesInOrder)
            FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
            For Each File As String In FilesInOrder
              If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                Dim sIDir As String = ISODDir.Replace("%n", "1")
                If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                If Not IO.Directory.Exists(IO.Path.Combine(sIDir, "sources")) Then IO.Directory.CreateDirectory(IO.Path.Combine(sIDir, "sources"))
                Progress_Normal(0, 1)
                Dim sNewFile As String = IO.Path.Combine(sIDir, "sources", "install.swm")
                ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", File, sNewFile))
                If Not SlowCopyFile(File, sNewFile, AddressOf Progress_Normal, True) Then
                  GUI_ToggleEnabled(True, "Failed to move Primary Install WIM!")
                  Return
                End If
                Extract_AllFiles(IO.Path.Combine(Application.StartupPath, "AR.zip"), sIDir)
                If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                  Status_SetText("Looking for oversized files...")
                  Dim BigList = FindBigFiles(sIDir)
                  If BigList.Count > 0 Then
                    Using fsAlert As New frmFS(BigList, sIDir, CByte(cmbISOFormat.SelectedIndex))
                      Select Case fsAlert.ShowDialog(Me)
                        Case Windows.Forms.DialogResult.Yes
                          REM UDF
                          cmbISOFormat.SelectedIndex = 3
                        Case Windows.Forms.DialogResult.OK
                          REM UDF102
                          cmbISOFormat.SelectedIndex = 5
                        Case Windows.Forms.DialogResult.No
                          REM Default
                        Case Windows.Forms.DialogResult.Cancel
                          REM STOP
                          GUI_ToggleEnabled(True, "ISO creation cancelled by user!")
                          Return
                      End Select
                    End Using
                  End If
                End If
                Dim ISODFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(ISOFile), IO.Path.ChangeExtension(String.Format("{0}_D{1}", IO.Path.GetFileNameWithoutExtension(ISOFile), 1), IO.Path.GetExtension(ISOFile)))
                Status_SetText("Building Data ISO 1...")
                Try
                  My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                  Dim isoLabel As String = txtISOLabel.Text
                  If isoLabel.Length > 32 - 6 Then
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel.Substring(0, 32 - 6), 1)
                  Else
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel, 1)
                  End If
                  If isoLabel.Contains(" ") Then isoLabel = Replace(isoLabel, " ", "")
                  Dim isoFormat As String = "-n"
                  Select Case cmbISOFormat.SelectedIndex
                    Case 0 : isoFormat = "-n"
                    Case 2 : isoFormat = "-j1"
                    Case 1 : isoFormat = "-j2"
                    Case 4 : isoFormat = "-u1"
                    Case 3 : isoFormat = "-u2"
                    Case 6 : isoFormat = "-u1 -udfver102"
                    Case 5 : isoFormat = "-u2 -udfver102"
                  End Select
                  OSCDIMG_MakeISO(sIDir, ISODFile, isoLabel, isoFormat)
                Catch ex As Exception
                End Try
              ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                Dim discNo As String = IO.Path.GetFileNameWithoutExtension(File).Substring(7)
                Dim sIDir As String = ISODDir.Replace("%n", discNo)
                If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                If Not IO.Directory.Exists(IO.Path.Combine(sIDir, "sources")) Then IO.Directory.CreateDirectory(IO.Path.Combine(sIDir, "sources"))
                Progress_Normal(0, 1)
                Dim sNewFile As String = IO.Path.Combine(sIDir, "sources", IO.Path.GetFileName(File))
                ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", File, sNewFile))
                If Not SlowCopyFile(File, sNewFile, AddressOf Progress_Normal, True) Then
                  GUI_ToggleEnabled(True, String.Format("Failed to move Install WIM #{0}!", discNo))
                  Return
                End If
                Extract_AllFiles(IO.Path.Combine(Application.StartupPath, "AR.zip"), sIDir)
                If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                  Status_SetText("Looking for oversized files...")
                  Dim BigList = FindBigFiles(sIDir)
                  If BigList.Count > 0 Then
                    Using fsAlert As New frmFS(BigList, sIDir, CByte(cmbISOFormat.SelectedIndex))
                      Select Case fsAlert.ShowDialog(Me)
                        Case Windows.Forms.DialogResult.Yes
                          REM UDF
                          cmbISOFormat.SelectedIndex = 3
                        Case Windows.Forms.DialogResult.OK
                          REM UDF102
                          cmbISOFormat.SelectedIndex = 5
                        Case Windows.Forms.DialogResult.No
                          REM Default
                        Case Windows.Forms.DialogResult.Cancel
                          REM STOP
                          GUI_ToggleEnabled(True, "ISO creation cancelled by user!")
                          Return
                      End Select
                    End Using
                  End If
                End If
                Dim ISODFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(ISOFile), IO.Path.ChangeExtension(String.Format("{0}_D{1}", IO.Path.GetFileNameWithoutExtension(ISOFile), discNo), IO.Path.GetExtension(ISOFile)))
                Status_SetText(String.Format("Building Data ISO {0}...", discNo))
                Try
                  My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                  Dim isoLabel As String = txtISOLabel.Text
                  If isoLabel.Length > 32 - (5 + discNo.Length) Then
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel.Substring(0, 32 - (5 + discNo.Length)), discNo)
                  Else
                    isoLabel = String.Format("{0}_DISC{1}", isoLabel, discNo)
                  End If
                  If isoLabel.Contains(" ") Then isoLabel = Replace(isoLabel, " ", "")
                  Dim isoFormat As String = "-n"
                  Select Case cmbISOFormat.SelectedIndex
                    Case 0 : isoFormat = "-n"
                    Case 2 : isoFormat = "-j1"
                    Case 1 : isoFormat = "-j2"
                    Case 4 : isoFormat = "-u1"
                    Case 3 : isoFormat = "-u2"
                    Case 6 : isoFormat = "-u1 -udfver102"
                    Case 5 : isoFormat = "-u2 -udfver102"
                  End Select
                  OSCDIMG_MakeISO(sIDir, ISODFile, isoLabel, isoFormat)
                Catch ex As Exception
                End Try
              End If
            Next
          End If
        End If
      End If
      Dim BootOrder As String = IO.Path.Combine(Work, "bootorder.txt")
      My.Computer.FileSystem.WriteAllText(BootOrder, String.Concat(IO.Path.Combine("boot", "bcd"), vbNewLine,
                                                                   IO.Path.Combine("boot", "boot.sdi"), vbNewLine,
                                                                   IO.Path.Combine("boot", "bootfix.bin"), vbNewLine,
                                                                   IO.Path.Combine("boot", "bootsect.exe"), vbNewLine,
                                                                   IO.Path.Combine("boot", "etfsboot.com"), vbNewLine,
                                                                   IO.Path.Combine("boot", "memtest.efi"), vbNewLine,
                                                                   IO.Path.Combine("boot", "memtest.exe"), vbNewLine,
                                                                   IO.Path.Combine("boot", "en-us", "bootsect.exe.mui"), vbNewLine,
                                                                   IO.Path.Combine("boot", "fonts", "chs_boot.ttf"), vbNewLine,
                                                                   IO.Path.Combine("boot", "fonts", "cht_boot.ttf"), vbNewLine,
                                                                   IO.Path.Combine("boot", "fonts", "jpn_boot.ttf"), vbNewLine,
                                                                   IO.Path.Combine("boot", "fonts", "kor_boot.ttf"), vbNewLine,
                                                                   IO.Path.Combine("boot", "fonts", "wgl4_boot.ttf"), vbNewLine,
                                                                   IO.Path.Combine("sources", "boot.wim"), vbNewLine), False, System.Text.Encoding.GetEncoding(1252))
      Progress_Normal(0, 1)
      Status_SetText("Making Backup of Old ISO...")
      Dim ISOBak As String = String.Concat(ISOFile, ".del")
      ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", ISOFile, ISOBak))
      If Not SlowCopyFile(ISOFile, ISOBak, AddressOf Progress_Normal, True) Then
        GUI_ToggleEnabled(True, "Failed to back up ISO!")
        Return
      End If
      Progress_Normal(0, 1)
      iTotalVal += 1 ' +1 Build ISO
      Progress_Total(iTotalVal, iTotalMax)
      Progress_Normal(0, 0)
      Status_SetText("Building New ISO...")
      Dim Saved As Boolean = False
      Try
        My.Computer.FileSystem.WriteAllBytes(ISOFile, {0, 0}, False)
        Dim isoLabel As String = txtISOLabel.Text
        If isoLabel.Contains(" ") Then isoLabel = Replace(isoLabel, " ", "")
        Dim isoFormat As String = "-n"
        Select Case cmbISOFormat.SelectedIndex
          Case 0 : isoFormat = "-n"
          Case 2 : isoFormat = "-j1"
          Case 1 : isoFormat = "-j2"
          Case 4 : isoFormat = "-u1"
          Case 3 : isoFormat = "-u2"
          Case 6 : isoFormat = "-u1 -udfver102"
          Case 5 : isoFormat = "-u2 -udfver102"
        End Select
        Progress_Normal(0, 1)
        Saved = OSCDIMG_MakeISO(ISODir, ISOFile, isoLabel, isoFormat, BootOrder, Not UEFI_IsEnabled AndAlso chkUEFI.Checked)
      Catch ex As Exception
        Saved = False
      End Try
      Progress_Normal(0, 1)
      If Saved Then
        If IO.File.Exists(ISOBak) Then
          ConsoleOutput_Write(String.Format("Deleting ""{0}""...", ISOBak))
          IO.File.Delete(ISOBak)
        End If
      Else
        Status_SetText("ISO Build Failed! Restoring Old ISO...")
        ConsoleOutput_Write(String.Format("Moving ""{0}"" to ""{1}""...", ISOBak, ISOFile))
        If Not SlowCopyFile(ISOBak, ISOFile, AddressOf Progress_Normal, True) Then
          GUI_ToggleEnabled(True, "ISO Build and backup ISO restore failed!")
        End If
        GUI_ToggleEnabled(True, "Failed to build ISO!")
        Return
      End If
    End If
    Progress_Normal(0, 100)
    Progress_Total(0, 100)
    RunComplete = True
    GUI_ToggleEnabled(True, "Complete!")
    Select Case cmbCompletion.SelectedIndex
      Case 0
        REM done
      Case 1
        REM play alert noise
        Dim tada As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", "tada.wav")
        If String.IsNullOrEmpty(mySettings.AlertNoisePath) Then
          If IO.File.Exists(tada) Then
            My.Computer.Audio.Play(tada, AudioPlayMode.Background)
          Else
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
          End If
        ElseIf IO.File.Exists(mySettings.AlertNoisePath) Then
          Try
            My.Computer.Audio.Play(mySettings.AlertNoisePath, AudioPlayMode.Background)
          Catch ex As Exception
            mySettings.AlertNoisePath = String.Empty
            If IO.File.Exists(tada) Then
              My.Computer.Audio.Play(tada, AudioPlayMode.Background)
            Else
              My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
            End If
          End Try
        Else
          mySettings.AlertNoisePath = String.Empty
          If IO.File.Exists(tada) Then
            My.Computer.Audio.Play(tada, AudioPlayMode.Background)
          Else
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
          End If
        End If
      Case 2
        REM close
        Me.Close()
      Case 3
        REM shut down
        GUI_Cloing()
        Process.Start("shutdown", "/s /t 0 /d p:0:0 /f")
      Case 4
        REM restart
        GUI_Cloing()
        Process.Start("shutdown", "/r /t 0 /d p:0:0 /f")
      Case 5
        REM sleep
        GUI_Cloing()
        Application.SetSuspendState(PowerState.Suspend, False, False)
        Me.Close()
    End Select
  End Sub
  Private Function Integrate_Files(WIMPath As String, PackageCount As Integer, UpdateData() As KeyValuePair(Of Update_File, Dictionary(Of String, Boolean)), ByRef iTotalVal As Integer, iTotalMax As Integer, WIMID As Integrate_Files_WIM) As Boolean
    If PackageCount < 1 Then
      GUI_ToggleEnabled(True, "No packages in WIM!")
      Return False
    End If
    Dim BootWIM As Boolean = (WIMID = Integrate_Files_WIM.BOOT_PE) Or (WIMID = Integrate_Files_WIM.BOOT_INSTALLER)
    If UpdateData.Length < 1 Then Return True
    Dim DISM_32 As New List(Of ImagePackage)
    Dim DISM_64 As New List(Of ImagePackage)
    Dim MSU_32 As New List(Of KeyValuePair(Of Update_File, Dictionary(Of String, Boolean)))
    Dim MSU_64 As New List(Of KeyValuePair(Of Update_File, Dictionary(Of String, Boolean)))
    Dim pbMax As Integer = PackageCount + UpdateData.Length
    Dim pbVal As Integer = 0
    Progress_Normal(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      Status_SetText(String.Format("Loading Image Package #{0} Data...", I))
      If StopRun Then
        ' TODO: Possibly set a title and status when stop is requested?
        Progress_Total(0, 100)
        Progress_Normal(0, 1)
        DISM_Image_Discard(Mount(BootWIM))
        GUI_ToggleEnabled(True)
        Return False
      End If
      Dim tmpDISM As ImagePackage = DISM_Image_GetData(WIMPath, I)
      Select Case WIMID
        Case Integrate_Files_WIM.BOOT_PE
          If Not tmpDISM.Edition = "WindowsPE" Then Continue For
          If String.IsNullOrEmpty(tmpDISM.Name) Then Continue For
          If tmpDISM.Name.StartsWith("Microsoft Windows PE") Then
            If CompareArchitectures(tmpDISM.Architecture, ArchitectureList.x86, True) Then
              DISM_32.Add(tmpDISM)
            ElseIf CompareArchitectures(tmpDISM.Architecture, ArchitectureList.amd64, True) Then
              DISM_64.Add(tmpDISM)
            End If
          End If
        Case Integrate_Files_WIM.BOOT_INSTALLER
          If Not tmpDISM.Edition = "WindowsPE" Then Continue For
          If String.IsNullOrEmpty(tmpDISM.Name) Then Continue For
          If tmpDISM.Name.StartsWith("Microsoft Windows Setup") Then
            If CompareArchitectures(tmpDISM.Architecture, ArchitectureList.x86, True) Then
              DISM_32.Add(tmpDISM)
            ElseIf CompareArchitectures(tmpDISM.Architecture, ArchitectureList.amd64, True) Then
              DISM_64.Add(tmpDISM)
            End If
          End If
        Case Else
          If CompareArchitectures(tmpDISM.Architecture, ArchitectureList.x86, True) Then
            DISM_32.Add(tmpDISM)
          ElseIf CompareArchitectures(tmpDISM.Architecture, ArchitectureList.amd64, True) Then
            DISM_64.Add(tmpDISM)
          End If
      End Select
    Next
    For I As Integer = 0 To UpdateData.Length - 1
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      Status_SetText(String.Format("Loading Update ""{0}"" Data...", IO.Path.GetFileNameWithoutExtension(UpdateData(I).Key.Path)))
      If StopRun Then
        ' TODO: Possibly set a title and status when stop is requested?
        Progress_Total(0, 100)
        Progress_Normal(0, 1)
        DISM_Image_Discard(Mount(BootWIM))
        GUI_ToggleEnabled(True)
        Return False
      End If
      If UpdateData(I).Key.Name = "DRIVER" Then
        If UpdateData(I).Key.DriverData.Architectures Is Nothing OrElse UpdateData(I).Key.DriverData.Architectures.Count = 0 Then Continue For
        If DISM_32.Count > 0 And UpdateData(I).Key.DriverData.Architectures.Exists(New Predicate(Of String)(Function(arch As String) As Boolean
                                                                                                              Return CompareArchitectures(arch, ArchitectureList.x86, True)
                                                                                                            End Function)) Then
          MSU_32.Add(UpdateData(I))
        End If
        If DISM_64.Count > 0 And UpdateData(I).Key.DriverData.Architectures.Exists(New Predicate(Of String)(Function(arch As String) As Boolean
                                                                                                              Return CompareArchitectures(arch, ArchitectureList.amd64, True)
                                                                                                            End Function)) Then
          MSU_64.Add(UpdateData(I))
        End If
      Else
        If Not String.IsNullOrEmpty(UpdateData(I).Key.Failure) Then Continue For
        If String.IsNullOrEmpty(UpdateData(I).Key.Architecture) Then Continue For
        If DISM_32.Count > 0 And CompareArchitectures(UpdateData(I).Key.Architecture, ArchitectureList.x86, True) Then
          MSU_32.Add(UpdateData(I))
        End If
        If DISM_64.Count > 0 Then
          If CompareArchitectures(UpdateData(I).Key.Architecture, ArchitectureList.amd64, True) Then
            MSU_64.Add(UpdateData(I))
          Else
            If CheckWhitelist(UpdateData(I).Key.DisplayName) Then MSU_64.Add(UpdateData(I))
          End If
        End If
      End If
    Next
    SortUpdatesForIntegration(MSU_32)
    SortUpdatesForIntegration(MSU_64)
    Progress_Normal(0, 0)
    pbMax = 1
    pbVal = 0
    If MSU_32.Count > 0 Then
      For Each tmpDISM In DISM_32
        pbMax += MSU_32.Count * 3 + 3
      Next
    End If
    If MSU_64.Count > 0 Then
      For Each tmpDISM In DISM_64
        pbMax += MSU_64.Count * 3 + 3
      Next
    End If
    Progress_Normal(0, pbMax)
    iTotalVal += 1
    Progress_Total(iTotalVal, iTotalMax)
    If MSU_32.Count > 0 Then
      For D As Integer = 0 To DISM_32.Count - 1
        Dim tmpDISM As ImagePackage = DISM_32(D)
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Loading Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Load(WIMPath, tmpDISM.Index, Mount(BootWIM)) Then
          DISM_Image_Discard(Mount(BootWIM))
          GUI_ToggleEnabled(True, String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount(BootWIM))
          GUI_ToggleEnabled(True)
          Return False
        End If
        For I As Integer = 0 To MSU_32.Count - 1
          Dim tmpMSU As Update_File = MSU_32(I).Key
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          Dim shownName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
          If Not String.IsNullOrEmpty(tmpMSU.Name) Then
            If tmpMSU.Name = "DRIVER" Then
              If Not String.IsNullOrEmpty(tmpMSU.DriverData.OriginalFileName) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.OriginalFileName)
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DriverData.PublishedName) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.PublishedName)
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DriverData.DriverStorePath) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.DriverStorePath)
              End If
            Else
              shownName = tmpMSU.Name
            End If
          ElseIf Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
            shownName = String.Format("KB{0}", tmpMSU.KBArticle)
          End If
          If MSU_32(I).Value().ContainsKey(tmpDISM.ToString) AndAlso MSU_32(I).Value()(tmpDISM.ToString) = False Then Continue For
          Status_SetText(String.Format("{1}/{2} - Integrating {0} into {3}...", shownName, I + 1, MSU_32.Count, tmpDISM.Name))
          Dim upType = GetUpdateType(tmpMSU.Path)
          Select Case upType
            Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Update_Add(Mount(BootWIM), tmpMSU.Path) Then
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If upType = UpdateType.LP Then Integrate_LanguageChanged = True
            Case UpdateType.EXE
              Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tmpMSU.Path)
              If fInfo.OriginalFilename = "iesetup.exe" And fInfo.ProductMajorPart > 9 Then
                Dim EXEPath As String = IO.Path.Combine(WorkDir, "UpdateEXE_Extract")
                If IO.Directory.Exists(EXEPath) Then
                  Status_SetText("Clearing Temporary Files...")
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                  SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                End If
                IO.Directory.CreateDirectory(EXEPath)
                Dim pbSubVal As Integer = pbVal * 3
                Dim pbSubMax As Integer = pbMax * 3
                pbVal += 1
                pbSubVal += 1
                Progress_Normal(pbSubVal, pbSubMax, True)
                Using iExtract As New Process With {.StartInfo = New ProcessStartInfo(tmpMSU.Path, String.Format("/x:{0}", EXEPath))}
                  If iExtract.Start Then
                    iExtract.WaitForExit()
                    pbSubVal += 1
                    Progress_Normal(pbSubVal, pbSubMax, True)
                    Dim bFound As Boolean = False
                    Dim Extracted() As String = IO.Directory.GetFiles(EXEPath)
                    For J As Integer = 0 To Extracted.Count - 1
                      If tmpMSU.Identity = New Update_File(Extracted(J)).Identity Then
                        bFound = True
                        pbSubVal += 1
                        Progress_Normal(pbSubVal, pbSubMax, True)
                        If Not DISM_Update_Add(Mount(BootWIM), Extracted(J)) Then
                          DISM_Image_Discard(Mount(BootWIM))
                          GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, IO.Path.GetFileName(Extracted(J)), tmpDISM.Name))
                          Return False
                        End If
                        Exit For
                      End If
                    Next
                    If IO.Directory.Exists(EXEPath) Then
                      Status_SetText("Clearing Temporary Files...")
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                      SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                    End If
                    If Not bFound Then
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                      Return False
                    End If
                  Else
                    If IO.Directory.Exists(EXEPath) Then
                      Status_SetText("Clearing Temporary Files...")
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                      SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                End Using
              ElseIf fInfo.OriginalFilename = "mergedwusetup.exe" Then
                Dim tmpCAB As String = IO.Path.Combine(WorkDir, "wusetup.cab")
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB_Convert(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to extract {0} from EXE to CAB!", shownName))
                  Return False
                End If
                Dim cabList() As String = Extract_FileList(tmpCAB)
                If cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                  Dim useEXE As String = "WUA-Downlevel.exe"
                  If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                  Extract_File(tmpCAB, WorkDir, useEXE)
                  If Not IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  Else
                    Dim exVal As Integer = pbVal * 6
                    Dim exMax As Integer = pbMax * 6
                    Progress_Normal(exVal, exMax, True)
                    Dim useCab1 As String = "WUClient-SelfUpdate-ActiveX.cab"
                    Dim useCab2 As String = "WUClient-SelfUpdate-Aux-TopLevel.cab"
                    Dim useCab3 As String = "WUClient-SelfUpdate-Core-TopLevel.cab"
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab1)
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab2)
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab3)
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab1)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab1, tmpDISM.Name))
                      Return False
                    End If
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab2)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab2, tmpDISM.Name))
                      Return False
                    End If
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab3)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab3, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab1)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab1, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab2)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab2, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab3)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab3, tmpDISM.Name))
                      Return False
                    End If
                    pbVal += 1
                    Progress_Normal(pbVal, pbMax, True)
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                End If
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
              Else
                Dim tmpCAB As String = IO.Path.Combine(WorkDir, "lp.cab")
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB_Convert(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to extract {0} from EXE to CAB!", shownName))
                  Return False
                End If
                Dim cabList() As String = Extract_FileList(tmpCAB)
                If cabList.Contains("update.mum") Then
                  pbVal += 1
                  Progress_Normal(pbVal, pbMax, True)
                  If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                End If
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                Integrate_LanguageChanged = True
              End If
            Case UpdateType.LIP
              Dim tmpCAB As String = IO.Path.Combine(WorkDir, String.Concat("tmp", IO.Path.ChangeExtension(tmpMSU.Path, ".cab")))
              If IO.File.Exists(tmpCAB) Then
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                IO.File.Delete(tmpCAB)
              End If
              ConsoleOutput_Write(String.Format("Copying ""{0}"" to ""{1}""...", tmpMSU.Path, tmpCAB))
              My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If IO.File.Exists(tmpCAB) Then
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                IO.File.Delete(tmpCAB)
              End If
              Integrate_LanguageChanged = True
            Case UpdateType.MSI
              Dim MSIPath As String = IO.Path.Combine(WorkDir, "UpdateMSI_Extract")
              If IO.Directory.Exists(MSIPath) Then
                Status_SetText("Clearing Temporary Files...")
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MSIPath))
                SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
              End If
              IO.Directory.CreateDirectory(MSIPath)
              Dim tmpCAB As String = IO.Path.Combine(MSIPath, "LIP.cab")
              Dim tmpMLC As String = IO.Path.Combine(MSIPath, "LIP.mlc")
              Extract_File(tmpMSU.Path, MSIPath, "LIP.cab")
              If IO.File.Exists(tmpCAB) Then
                Extract_File(tmpCAB, MSIPath, "LIP.mlc")
                If IO.File.Exists(tmpMLC) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                  ConsoleOutput_Write(String.Format("Renaming ""{0}"" as ""{1}""...", tmpMLC, tmpCAB))
                  Rename(tmpMLC, tmpCAB)
                  pbVal += 1
                  Progress_Normal(pbVal, pbMax, True)
                  If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  Integrate_LanguageChanged = True
                Else
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                  Return False
                End If
              Else
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If IO.Directory.Exists(MSIPath) Then
                Status_SetText("Clearing Temporary Files...")
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MSIPath))
                SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
              End If
              Integrate_LanguageChanged = True
            Case UpdateType.INF
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Driver_Add(Mount(BootWIM), tmpMSU.Path, False, True) Then
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
          End Select
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          If StopRun Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount(BootWIM))
            GUI_ToggleEnabled(True)
            Return False
          End If
        Next
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Dim DoSave As Boolean = True
        If DISM_64.Count = 0 Then
          If D = DISM_32.Count - 1 Then
            DoSave = False
          End If
        End If
        If DoSave Or BootWIM Then
          Status_SetText(String.Format("Saving Image Package ""{0}""...", tmpDISM.Name))
          If Not DISM_Image_Save(Mount(BootWIM)) Then
            TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            Status_SetText("Image Package Save Failed! Discarding Mount...")
            DISM_Image_Discard(Mount(BootWIM))
            GUI_ToggleEnabled(True, String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
            Return False
          End If
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return False
        End If
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
      Next
    End If
    If MSU_64.Count > 0 Then
      For D As Integer = 0 To DISM_64.Count - 1
        Dim tmpDISM As ImagePackage = DISM_64(D)
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Loading Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Load(WIMPath, tmpDISM.Index, Mount(BootWIM)) Then
          DISM_Image_Discard(Mount(BootWIM))
          GUI_ToggleEnabled(True, String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount(BootWIM))
          GUI_ToggleEnabled(True)
          Return False
        End If
        For I As Integer = 0 To MSU_64.Count - 1
          Dim tmpMSU As Update_File = MSU_64(I).Key
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          Dim shownName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
          If Not String.IsNullOrEmpty(tmpMSU.Name) Then
            If tmpMSU.Name = "DRIVER" Then
              If Not String.IsNullOrEmpty(tmpMSU.DriverData.OriginalFileName) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.OriginalFileName)
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DriverData.PublishedName) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.PublishedName)
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DriverData.DriverStorePath) Then
                shownName = IO.Path.GetFileNameWithoutExtension(tmpMSU.DriverData.DriverStorePath)
              End If
            Else
              shownName = tmpMSU.Name
            End If
          ElseIf Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
            shownName = String.Format("KB{0}", tmpMSU.KBArticle)
          End If
          If MSU_64(I).Value().ContainsKey(tmpDISM.ToString) AndAlso MSU_64(I).Value()(tmpDISM.ToString) = False Then Continue For
          Status_SetText(String.Format("{1}/{2} - Integrating {0} into {3}...", shownName, I + 1, MSU_64.Count, tmpDISM.Name))
          Dim upType = GetUpdateType(tmpMSU.Path)
          Select Case upType
            Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Update_Add(Mount(BootWIM), tmpMSU.Path) Then
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If upType = UpdateType.LP Then Integrate_LanguageChanged = True
            Case UpdateType.EXE
              Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tmpMSU.Path)
              If fInfo.OriginalFilename = "iesetup.exe" And fInfo.ProductMajorPart > 9 Then
                Dim EXEPath As String = IO.Path.Combine(WorkDir, "UpdateEXE_Extract")
                If IO.Directory.Exists(EXEPath) Then
                  Status_SetText("Clearing Temporary Files...")
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                  SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                End If
                IO.Directory.CreateDirectory(EXEPath)
                Dim pbSubVal As Integer = pbVal * 3
                Dim pbSubMax As Integer = pbMax * 3
                pbVal += 1
                pbSubVal += 1
                Progress_Normal(pbSubVal, pbSubMax, True)
                Using iExtract As New Process With {.StartInfo = New ProcessStartInfo(tmpMSU.Path, String.Format("/x:{0}", EXEPath))}
                  If iExtract.Start Then
                    iExtract.WaitForExit()
                    pbSubVal += 1
                    Progress_Normal(pbSubVal, pbSubMax, True)
                    Dim bFound As Boolean = False
                    Dim Extracted() As String = IO.Directory.GetFiles(EXEPath)
                    For J As Integer = 0 To Extracted.Count - 1
                      If tmpMSU.Identity = New Update_File(Extracted(J)).Identity Then
                        bFound = True
                        pbSubVal += 1
                        Progress_Normal(pbSubVal, pbSubMax, True)
                        If Not DISM_Update_Add(Mount(BootWIM), Extracted(J)) Then
                          DISM_Image_Discard(Mount(BootWIM))
                          GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, IO.Path.GetFileName(Extracted(J)), tmpDISM.Name))
                          Return False
                        End If
                        Exit For
                      End If
                    Next
                    If IO.Directory.Exists(EXEPath) Then
                      Status_SetText("Clearing Temporary Files...")
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                      SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                    End If
                    If Not bFound Then
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                      Return False
                    End If
                  Else
                    If IO.Directory.Exists(EXEPath) Then
                      Status_SetText("Clearing Temporary Files...")
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", EXEPath))
                      SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                End Using
              ElseIf fInfo.OriginalFilename = "mergedwusetup.exe" Then
                Dim tmpCAB As String = IO.Path.Combine(WorkDir, "wusetup.cab")
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB_Convert(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to extract {0} from EXE to CAB!", shownName))
                  Return False
                End If
                Dim cabList() As String = Extract_FileList(tmpCAB)
                If cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                  Dim useEXE As String = "WUA-Downlevel.exe"
                  If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                  Extract_File(tmpCAB, WorkDir, useEXE)
                  If Not IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  Else
                    Dim exVal As Integer = pbVal * 6
                    Dim exMax As Integer = pbMax * 6
                    Progress_Normal(exVal, exMax, True)
                    Dim useCab1 As String = "WUClient-SelfUpdate-ActiveX.cab"
                    Dim useCab2 As String = "WUClient-SelfUpdate-Aux-TopLevel.cab"
                    Dim useCab3 As String = "WUClient-SelfUpdate-Core-TopLevel.cab"
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab1)
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab2)
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    Extract_File(IO.Path.Combine(WorkDir, useEXE), WorkDir, useCab3)
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab1)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab1, tmpDISM.Name))
                      Return False
                    End If
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab2)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab2, tmpDISM.Name))
                      Return False
                    End If
                    If Not IO.File.Exists(IO.Path.Combine(WorkDir, useCab3)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab3, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab1)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab1, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab2)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab2, tmpDISM.Name))
                      Return False
                    End If
                    exVal += 1
                    Progress_Normal(exVal, exMax, True)
                    If Not DISM_Update_Add(Mount(BootWIM), IO.Path.Combine(WorkDir, useCab3)) Then
                      If IO.File.Exists(tmpCAB) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(IO.Path.Combine(WorkDir, useEXE)) Then
                        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(WorkDir, useEXE)))
                        IO.File.Delete(IO.Path.Combine(WorkDir, useEXE))
                      End If
                      DISM_Image_Discard(Mount(BootWIM))
                      GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} ({1}) into {2}!", shownName, useCab3, tmpDISM.Name))
                      Return False
                    End If
                    pbVal += 1
                    Progress_Normal(pbVal, pbMax, True)
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                End If
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
              Else
                Dim tmpCAB As String = IO.Path.Combine(WorkDir, "lp.cab")
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB_Convert(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to extract {0} from EXE to CAB!", shownName))
                  Return False
                End If
                Dim cabList() As String = Extract_FileList(tmpCAB)
                If cabList.Contains("update.mum") Then
                  pbVal += 1
                  Progress_Normal(pbVal, pbMax, True)
                  If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                End If
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                Integrate_LanguageChanged = True
              End If
            Case UpdateType.LIP
              Dim tmpCAB As String = IO.Path.Combine(WorkDir, String.Concat("tmp", IO.Path.ChangeExtension(tmpMSU.Path, ".cab")))
              If IO.File.Exists(tmpCAB) Then
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                IO.File.Delete(tmpCAB)
              End If
              ConsoleOutput_Write(String.Format("Copying ""{0}"" to ""{1}""...", tmpMSU.Path, tmpCAB))
              My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                If IO.File.Exists(tmpCAB) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                End If
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If IO.File.Exists(tmpCAB) Then
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                IO.File.Delete(tmpCAB)
              End If
              Integrate_LanguageChanged = True
            Case UpdateType.MSI
              Dim MSIPath As String = IO.Path.Combine(WorkDir, "UpdateMSI_Extract")
              If IO.Directory.Exists(MSIPath) Then
                Status_SetText("Clearing Temporary Files...")
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MSIPath))
                SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
              End If
              IO.Directory.CreateDirectory(MSIPath)
              Dim tmpCAB As String = IO.Path.Combine(MSIPath, "LIP.cab")
              Dim tmpMLC As String = IO.Path.Combine(MSIPath, "LIP.mlc")
              Extract_File(tmpMSU.Path, MSIPath, "LIP.cab")
              If IO.File.Exists(tmpCAB) Then
                Extract_File(tmpCAB, MSIPath, "LIP.mlc")
                If IO.File.Exists(tmpMLC) Then
                  ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                  IO.File.Delete(tmpCAB)
                  ConsoleOutput_Write(String.Format("Renaming ""{0}"" as ""{1}""...", tmpMLC, tmpCAB))
                  Rename(tmpMLC, tmpCAB)
                  pbVal += 1
                  Progress_Normal(pbVal, pbMax, True)
                  If Not DISM_Update_Add(Mount(BootWIM), tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                      IO.File.Delete(tmpCAB)
                    End If
                    DISM_Image_Discard(Mount(BootWIM))
                    GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", tmpCAB))
                    IO.File.Delete(tmpCAB)
                  End If
                  Integrate_LanguageChanged = True
                Else
                  DISM_Image_Discard(Mount(BootWIM))
                  GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                  Return False
                End If
              Else
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
              If IO.Directory.Exists(MSIPath) Then
                Status_SetText("Clearing Temporary Files...")
                ConsoleOutput_Write(String.Format("Deleting ""{0}""...", MSIPath))
                SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
              End If
              Integrate_LanguageChanged = True
            Case UpdateType.INF
              pbVal += 1
              Progress_Normal(pbVal, pbMax, True)
              If Not DISM_Driver_Add(Mount(BootWIM), tmpMSU.Path, False, True) Then
                DISM_Image_Discard(Mount(BootWIM))
                GUI_ToggleEnabled(True, String.Format("Failed to integrate {0} into {1}!", shownName, tmpDISM.Name))
                Return False
              End If
          End Select
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          If StopRun Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            DISM_Image_Discard(Mount(BootWIM))
            GUI_ToggleEnabled(True)
            Return False
          End If
        Next
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Dim DoSave As Boolean = True
        If D = DISM_64.Count - 1 Then DoSave = False
        If DoSave Or BootWIM Then
          Status_SetText(String.Format("Saving Image Package ""{0}""...", tmpDISM.Name))
          If Not DISM_Image_Save(Mount(BootWIM)) Then
            TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            Status_SetText("Image Package Save Failed! Discarding Mount...")
            DISM_Image_Discard(Mount(BootWIM))
            GUI_ToggleEnabled(True, String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
            Return False
          End If
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return False
        End If
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
      Next
    End If
    Return True
  End Function
  Private Function Integrate_ServicePack(WIMPath As String, PackageCount As Integer, SPPath As String, Optional Architecture As String = Nothing) As Boolean
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    Dim pbVal As Integer = 0
    Dim pbMax As Integer = (PackageCount * 5) + 15
    Progress_Normal(pbVal, pbMax)
    Dim ActivePackages As Integer = 0
    Dim activeArch As String = Architecture
    If PackageCount > 0 Then
      For I As Integer = 1 To PackageCount
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Dim dismData As ImagePackage = DISM_Image_GetData(WIMPath, I)
        Try
          If dismData.SPLevel > 0 Then Continue For
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not CompareArchitectures(dismData.Architecture, Architecture, True) Then Continue For
        Catch ex As Exception
          Continue For
        End Try
        If String.IsNullOrEmpty(activeArch) Then activeArch = dismData.Architecture.Substring(1)
        ActivePackages += 1
      Next
    End If
    If ActivePackages = 0 Then Return True
    pbVal += 1
    pbMax = (ActivePackages * 3) + 15 + (PackageCount * 2)
    Progress_Normal(pbVal, pbMax)
    Status_SetText("Extracting Service Pack...")
    Run_Hidden(SPPath, String.Format("/x:""{0}""", IO.Path.Combine(Work, "SP1")))
    pbVal += 1
    Progress_Normal(pbVal, pbMax, True)
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    Dim Extract86 As String = IO.Path.Combine(Work, "SP1", "windows6.1-KB976932-X86.cab")
    Dim Extract64 As String = IO.Path.Combine(Work, "SP1", "windows6.1-KB976932-X64.cab")
    Dim upArch As String = Nothing
    If IO.File.Exists(Extract86) Then
      Status_SetText("Preparing Service Pack (Extracting KB976932.cab)...")
      Extract_AllFiles(Extract86, IO.Path.Combine(Work, "SP1"))
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", Extract86))
      IO.File.Delete(Extract86)
      upArch = "x86"
    ElseIf IO.File.Exists(Extract64) Then
      Status_SetText("Preparing Service Pack (Extracting KB976932.cab)...")
      Extract_AllFiles(Extract64, IO.Path.Combine(Work, "SP1"))
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", Extract64))
      IO.File.Delete(Extract64)
      upArch = "amd64"
    Else
      GUI_ToggleEnabled(True, "No KB976932.cab to extract!")
      Return False
    End If
    pbVal += 1
    Progress_Normal(pbVal, pbMax, True)
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    Dim Extract As String = IO.Path.Combine(Work, "SP1", "NestedMPPcontent.cab")
    If IO.File.Exists(Extract) Then
      Status_SetText("Preparing Service Pack (Extracting NestedMPPcontent.cab)...")
      Extract_AllFiles(Extract, IO.Path.Combine(Work, "SP1"))
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", Extract))
      IO.File.Delete(Extract)
    Else
      GUI_ToggleEnabled(True, "No NestedMPPcontent.cab to extract!")
      Return False
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    pbVal += 1
    Progress_Normal(pbVal, pbMax)
    Dim KB976932mum_active As String = IO.Path.Combine(Work, "SP1", "update.mum")
    Dim KB976932ses_active As String = IO.Path.Combine(Work, "SP1", "update.ses")
    Dim KB976933mum_active As String = IO.Path.Combine(Work, "SP1", String.Format("Windows7SP1-KB976933~31bf3856ad364e35~{0}~~6.1.1.17514.mum", upArch))
    Dim KB976932mum_stored As String = IO.Path.Combine(Work, String.Format("Package_for_KB976932~31bf3856ad364e35~{0}~~6.1.1.17514.mum", upArch))
    Dim KB976932ses_stored As String = IO.Path.Combine(Work, String.Format("Package_for_KB976932~31bf3856ad364e35~{0}~~6.1.1.17514.ses", upArch))
    Dim KB976933mum_stored As String = IO.Path.Combine(Work, String.Format("Windows7SP1-KB976933~31bf3856ad364e35~{0}~~6.1.1.17514.mum", upArch))
    If IO.File.Exists(KB976932ses_active) Then
      Status_SetText("Preparing Service Pack (Modifying update.ses)...")
      Integrate_ServicePack_UpdateSES(KB976932ses_active, KB976932ses_stored)
    Else
      GUI_ToggleEnabled(True, "No update.ses to modify!")
      Return False
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    pbVal += 1
    Progress_Normal(pbVal, pbMax)
    If IO.File.Exists(KB976932mum_active) Then
      Status_SetText("Preparing Service Pack (Modifying update.mum)...")
      Integrate_ServicePack_UpdateMUM(KB976932mum_active, KB976932mum_stored)
    Else
      GUI_ToggleEnabled(True, "No update.mum to modify!")
      Return False
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    pbVal += 1
    Progress_Normal(pbVal, pbMax)
    If IO.File.Exists(KB976933mum_active) Then
      Status_SetText("Preparing Service Pack (Modifying KB976933.mum)...")
      Integrate_ServicePack_UpdateMUM(KB976933mum_active, KB976933mum_stored)
    Else
      GUI_ToggleEnabled(True, "No KB976933.mum to modify!")
      Return False
    End If
    If StopRun Then
      GUI_ToggleEnabled(True)
      Return False
    End If
    pbVal += 1
    Progress_Normal(pbVal, pbMax)
    Dim CABList As String = IO.Path.Combine(Work, "SP1", "cabinet.cablist.ini")
    If IO.File.Exists(CABList) Then
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", CABList))
      IO.File.Delete(CABList)
    End If
    CABList = IO.Path.Combine(Work, "SP1", "old_cabinet.cablist.ini")
    If IO.File.Exists(CABList) Then
      ConsoleOutput_Write(String.Format("Deleting ""{0}""...", CABList))
      IO.File.Delete(CABList)
    End If
    For I As Integer = 0 To 6
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      Extract = IO.Path.Combine(Work, "SP1", String.Format("KB976933-LangsCab{0}.cab", I))
      If IO.File.Exists(Extract) Then
        Status_SetText(String.Format("Preparing Service Pack (Extracting Language CAB {0} of 7)...", I + 1))
        Extract_AllFiles(Extract, IO.Path.Combine(Work, "SP1"))
        ConsoleOutput_Write(String.Format("Deleting ""{0}""...", Extract))
        IO.File.Delete(Extract)
      Else
        GUI_ToggleEnabled(True, String.Format("No KB976933-LangsCab{0}.cab to extract!", I))
        Return False
      End If
      If StopRun Then
        GUI_ToggleEnabled(True)
        Return False
      End If
    Next
    pbVal += 1
    Progress_Normal(pbVal, pbMax, True)
    If PackageCount > 0 Then
      Dim iRunCount As Integer = 0
      For I As Integer = 1 To PackageCount
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Dim dismData As ImagePackage = DISM_Image_GetData(WIMPath, I)
        Try
          If dismData.SPLevel > 0 Then Continue For
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not CompareArchitectures(dismData.Architecture, Architecture, True) Then
            Continue For
          End If
        Catch ex As Exception
        End Try
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Integrating Service Pack (Loading {0})...", dismData.Name))
        If Not DISM_Image_Load(WIMPath, I, Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Load Image Package ""{0}""!", dismData.Name))
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Image Package Load Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Load Image Package ""{0}""!", dismData.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Integrating Service Pack into {0}...", dismData.Name))
        If Not DISM_Update_Add(Mount, IO.Path.Combine(Work, "SP1")) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Add Service Pack to Image Package ""{0}""!", dismData.Name))
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Service Pack Integration Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Add Service Pack to Image Package ""{0}""!", dismData.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        If Not (IO.File.Exists(KB976932mum_stored) And IO.File.Exists(KB976932ses_stored) And IO.File.Exists(KB976933mum_stored)) Then
          Status_SetText(String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
          If MsgDlg(Me, String.Concat("The Image Package may require that the System Update Readiness Tool be applied after installation.", vbNewLine, "Would you like to stop the integration process?"), "Unable to locate backup MUM and SES files.", "Service Pack Integration Warning", MessageBoxButtons.YesNo, _TaskDialogIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            Status_SetText("Service Pack Integration Failed! Discarding Mount...")
            DISM_Image_Discard(Mount)
            GUI_ToggleEnabled(True, String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
            Return False
          End If
        End If
        Dim KB976932mum_mount As String = IO.Path.Combine(Mount, dismData.SystemRoot, "servicing", "Packages", String.Format("Package_for_KB976932~31bf3856ad364e35~{0}~~6.1.1.17514.mum", upArch))
        Dim KB976932ses_mount As String = IO.Path.Combine(Mount, dismData.SystemRoot, "servicing", "Packages", String.Format("Package_for_KB976932~31bf3856ad364e35~{0}~~6.1.1.17514.ses", upArch))
        Dim KB976933mum_mount As String = IO.Path.Combine(Mount, dismData.SystemRoot, "servicing", "Packages", String.Format("Windows7SP1-KB976933~31bf3856ad364e35~{0}~~6.1.1.17514.mum", upArch))
        If Not (IO.File.Exists(KB976932mum_mount) And IO.File.Exists(KB976932ses_mount) And IO.File.Exists(KB976933mum_mount)) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Service Pack Integration Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
          Return False
        End If
        Status_SetText(String.Format("Integrating Service Pack into {0} (Reverting MUM and SES Files)...", dismData.Name))
        Dim iFailed As Boolean = False
        If Not GrantFullControlToEveryone(KB976932mum_mount) Then iFailed = True
        If Not GrantFullControlToEveryone(KB976932ses_mount) Then iFailed = True
        If Not GrantFullControlToEveryone(KB976933mum_mount) Then iFailed = True
        If Not iFailed Then
          Try
            IO.File.Copy(KB976932mum_stored, KB976932mum_mount, True)
          Catch ex As Exception
            iFailed = True
          End Try
          Try
            IO.File.Copy(KB976932ses_stored, KB976932ses_mount, True)
          Catch ex As Exception
            iFailed = True
          End Try
          Try
            IO.File.Copy(KB976933mum_stored, KB976933mum_mount, True)
          Catch ex As Exception
            iFailed = True
          End Try
        End If
        RevokeFullControlForEveryone(KB976932mum_mount)
        RevokeFullControlForEveryone(KB976932ses_mount)
        RevokeFullControlForEveryone(KB976933mum_mount)
        If iFailed Then
          Status_SetText(String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
          If MsgDlg(Me, String.Concat("The Image Package may require that the System Update Readiness Tool be applied after installation.", vbNewLine, "Would you like to stop the integration process?"), "Unable to revert backup MUM and SES files.", "Service Pack Integration Warning", MessageBoxButtons.YesNo, _TaskDialogIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
            Progress_Total(0, 100)
            Progress_Normal(0, 1)
            Status_SetText("Service Pack Integration Failed! Discarding Mount...")
            DISM_Image_Discard(Mount)
            GUI_ToggleEnabled(True, String.Format("Failed to Revert MUM and SES Files for Image Package ""{0}""!", dismData.Name))
            Return False
          End If
        End If
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Integrating Service Pack (Saving {0})...", dismData.Name))
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", dismData.Name))
          Progress_Total(0, 100)
          Progress_Normal(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Save Image Package ""{0}""!", dismData.Name))
          Return False
        End If
        If StopRun Then
          GUI_ToggleEnabled(True)
          Return False
        End If
      Next
    Else
      GUI_ToggleEnabled(True, "No packages in WIM!")
      Return False
    End If
    Progress_Normal(0, pbMax)
    Status_SetText("Clearing Temporary Files...")
    ConsoleOutput_Write(String.Format("Deleting ""{0}""...", IO.Path.Combine(Work, "SP1")))
    SlowDeleteDirectory(IO.Path.Combine(Work, "SP1"), FileIO.DeleteDirectoryOption.DeleteAllContents, False)
    Return True
  End Function
  Private Function Integrate_ToggleFeatures(WIMPath As String, PackageCount As Integer, FeatureData() As List(Of Feature)) As Boolean
    If PackageCount < 1 Then
      GUI_ToggleEnabled(True, "No packages in WIM!")
      Return False
    End If
    If FeatureData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each FeatureList As List(Of Feature) In FeatureData
      If FeatureList IsNot Nothing Then pbMax += FeatureList.Count + 3
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    Progress_Normal(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      If FeatureData(I - 1) IsNot Nothing AndAlso FeatureData(I - 1).Count > 0 Then
        Status_SetText(String.Format("Loading Image Package #{0} Data...", I))
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = DISM_Image_GetData(WIMPath, I)
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Loading Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Load(WIMPath, tmpDISM.Index, Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Load Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to load Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        For J As Integer = 0 To FeatureData(I - 1).Count - 1
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          If FeatureData(I - 1)(J).Enable Then
            If Not (FeatureData(I - 1)(J).State = "Enabled" Or FeatureData(I - 1)(J).State = "Enable Pending") Then
              Status_SetText(String.Format("{1}/{2} - Enabling {0} in {3}...", FeatureData(I - 1)(J).DisplayName, J + 1, FeatureData(I - 1).Count, tmpDISM.Name))
              If Not DISM_Feature_Enable(Mount, FeatureData(I - 1)(J).FeatureName) Then
                TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Enable Feature in Image Package ""{0}""!", tmpDISM.Name))
                Progress_Normal(0, 1)
                Progress_Total(0, 1)
                Status_SetText("Feature Enable Failed! Discarding Mount...")
                DISM_Image_Discard(Mount)
                GUI_ToggleEnabled(True, String.Format("Failed to eneable {0} in {1}!", FeatureData(I - 1)(J).DisplayName, tmpDISM.Name))
                Return False
              End If
            End If
          Else
            If (FeatureData(I - 1)(J).State = "Enabled" Or FeatureData(I - 1)(J).State = "Enable Pending") Then
              Status_SetText(String.Format("{1}/{2} - Disabling {0} in {3}...", FeatureData(I - 1)(J).DisplayName, J + 1, FeatureData(I - 1).Count, tmpDISM.Name))
              If Not DISM_Feature_Disable(Mount, FeatureData(I - 1)(J).FeatureName) Then
                TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Disable Feature in Image Package ""{0}""!", tmpDISM.Name))
                Progress_Normal(0, 1)
                Progress_Total(0, 1)
                Status_SetText("Feature Disable Failed! Discarding Mount...")
                DISM_Image_Discard(Mount)
                GUI_ToggleEnabled(True, String.Format("Failed to disable {0} in {1}!", FeatureData(I - 1)(J).DisplayName, tmpDISM.Name))
                Return False
              End If
            End If
          End If
        Next
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Saving Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to save Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
  Private Function Integrate_RemoveUpdates(WIMPath As String, PackageCount As Integer, UpdateData() As List(Of Update_Integrated)) As Boolean
    If PackageCount < 1 Then
      GUI_ToggleEnabled(True, "No packages in WIM!")
      Return False
    End If
    If UpdateData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each UpdateList As List(Of Update_Integrated) In UpdateData
      If UpdateList IsNot Nothing Then pbMax += UpdateList.Count + 3
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    Progress_Normal(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      If UpdateData(I - 1) IsNot Nothing AndAlso UpdateData(I - 1).Count > 0 Then
        Status_SetText(String.Format("Loading Image Package #{0} Data...", I))
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = DISM_Image_GetData(WIMPath, I)
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Loading Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Load(WIMPath, tmpDISM.Index, Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Load Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        For J As Integer = 0 To UpdateData(I - 1).Count - 1
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          If UpdateData(I - 1)(J).Remove And Not (UpdateData(I - 1)(J).State = "Uninstall Pending" Or UpdateData(I - 1)(J).State = "Superseded") Then
            Status_SetText(String.Format("{1}/{2} - Removing {0} from {3}...", UpdateData(I - 1)(J).Ident.Name, J + 1, UpdateData(I - 1).Count, tmpDISM.Name))
            If Not DISM_Update_Remove(Mount, UpdateData(I - 1)(J).Identity) Then
              TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Remove Update from Image Package ""{0}""!", tmpDISM.Name))
              Progress_Normal(0, 1)
              Progress_Total(0, 1)
              Status_SetText("Update Removal Failed! Discarding Mount...")
              DISM_Image_Discard(Mount)
              GUI_ToggleEnabled(True, String.Format("Failed to remove {0} from {1}!", UpdateData(I - 1)(J).Ident.Name, tmpDISM.Name))
              Return False
            End If
          End If
          If StopRun Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Normal(0, 1)
            Progress_Total(0, 1)
            DISM_Image_Discard(Mount)
            GUI_ToggleEnabled(True)
            Return False
          End If
        Next
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Saving Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
  Private Function Integrate_RemoveDrivers(WIMPath As String, PackageCount As Integer, DriverData() As List(Of Driver)) As Boolean
    If PackageCount < 1 Then
      GUI_ToggleEnabled(True, "No packages in WIM!")
      Return False
    End If
    If DriverData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each DriverList As List(Of Driver) In DriverData
      If DriverList IsNot Nothing Then pbMax += DriverList.Count + 3
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    Progress_Normal(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      Progress_Normal(pbVal, pbMax, True)
      If DriverData(I - 1) IsNot Nothing AndAlso DriverData(I - 1).Count > 0 Then
        Status_SetText(String.Format("Loading Image Package #{0} Data...", I))
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = DISM_Image_GetData(WIMPath, I)
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Loading Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Load(WIMPath, tmpDISM.Index, Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Load Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Load Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
        For J As Integer = 0 To DriverData(I - 1).Count - 1
          pbVal += 1
          Progress_Normal(pbVal, pbMax, True)
          If DriverData(I - 1)(J).Remove Then
            Status_SetText(String.Format("{1}/{2} - Removing {0} from {3}...", DriverData(I - 1)(J).PublishedName, J + 1, DriverData(I - 1).Count, tmpDISM.Name))
            If Not DISM_Driver_Remove(Mount, DriverData(I - 1)(J).PublishedName) Then
              TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Remove Driver from Image Package ""{0}""!", tmpDISM.Name))
              Progress_Normal(0, 1)
              Progress_Total(0, 1)
              Status_SetText("Driver Removal Failed! Discarding Mount...")
              DISM_Image_Discard(Mount)
              GUI_ToggleEnabled(True, String.Format("Failed to remove {0} from {1}!", DriverData(I - 1)(J).PublishedName, tmpDISM.Name))
              Return False
            End If
          End If
          If StopRun Then
            ' TODO: Possibly set a title and status when stop is requested?
            Progress_Normal(0, 1)
            Progress_Total(0, 1)
            DISM_Image_Discard(Mount)
            GUI_ToggleEnabled(True)
            Return False
          End If
        Next
        pbVal += 1
        Progress_Normal(pbVal, pbMax, True)
        Status_SetText(String.Format("Saving Image Package ""{0}""...", tmpDISM.Name))
        If Not DISM_Image_Save(Mount) Then
          TitleMNG_SetTitle("Discarding Changes", String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          Status_SetText("Image Package Save Failed! Discarding Mount...")
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True, String.Format("Failed to Save Image Package ""{0}""!", tmpDISM.Name))
          Return False
        End If
        If StopRun Then
          ' TODO: Possibly set a title and status when stop is requested?
          Progress_Normal(0, 1)
          Progress_Total(0, 1)
          DISM_Image_Discard(Mount)
          GUI_ToggleEnabled(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
#Region "SP1 Extras"
  Private Sub Integrate_ServicePack_UpdateSES(Path As String, BackupPath As String)
    Try
      IO.File.Copy(Path, BackupPath, True)
    Catch ex As Exception
    End Try
    Dim XUpdate As XElement = XElement.Load(Path, LoadOptions.PreserveWhitespace)
    For Each node As XElement In XUpdate.Elements("Tasks")
      If CStr(node.Attribute("operationMode")) = "OfflineInstall" Then
        node.Element("Phase").Element("package").SetAttributeValue("targetState", "Installed")
      ElseIf CStr(node.Attribute("operationMode")) = "OfflineUninstall" Then
        node.Element("Phase").Element("package").SetAttributeValue("targetState", "Installed")
      End If
    Next
    XUpdate.Save(Path, SaveOptions.DisableFormatting)
  End Sub
  Private Sub Integrate_ServicePack_UpdateMUM(Path As String, BackupPath As String)
    Try
      IO.File.Copy(Path, BackupPath, True)
    Catch ex As Exception
    End Try
    Dim XUpdate As XElement = XElement.Load(Path, LoadOptions.PreserveWhitespace)
    Dim xPackage = XUpdate.Element("{urn:schemas-microsoft-com:asm.v3}package")
    Dim xExtended = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}packageExtended")
    xExtended.SetAttributeValue("allowedOffline", "true")
    XUpdate.Save(Path, SaveOptions.DisableFormatting)
  End Sub
#End Region
  Private Enum Integrate_Files_WIM
    INSTALL = 0
    BOOT_PE = 1
    BOOT_INSTALLER = 2
  End Enum
#End Region
#Region "Useful Functions"
  Private Function GrantFullControlToEveryone(FilePath As String) As Boolean
    Try
      Dim Security As System.Security.AccessControl.FileSecurity = IO.File.GetAccessControl(FilePath)
      Dim Sid As New System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, Nothing)
      Dim Account As System.Security.Principal.NTAccount = TryCast(Sid.Translate(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
      Dim Grant As New System.Security.AccessControl.FileSystemAccessRule(Account, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.None, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow)
      Security.AddAccessRule(Grant)
      IO.File.SetAccessControl(FilePath, Security)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
  Private Function RevokeFullControlForEveryone(FilePath As String) As Boolean
    Try
      Dim Security As System.Security.AccessControl.FileSecurity = IO.File.GetAccessControl(FilePath)
      Dim Sid As New System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, Nothing)
      Dim Account As System.Security.Principal.NTAccount = TryCast(Sid.Translate(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
      Dim Revoke As New System.Security.AccessControl.FileSystemAccessRule(Account, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.None, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow)
      Security.RemoveAccessRuleSpecific(Revoke)
      IO.File.SetAccessControl(FilePath, Security)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
  Private Function CheckWhitelist(msuDisplayName As String) As Boolean
    Dim UpdateWhitelist() As String = mySettings.x86WhiteList
    Dim isOK As Boolean = False
    If UpdateWhitelist IsNot Nothing Then
      For Each whitelistItem In UpdateWhitelist
        If Not String.IsNullOrEmpty(whitelistItem) Then
          If String.Compare(msuDisplayName, whitelistItem, True) = 0 Then
            Return True
          End If
        End If
      Next
    End If
    Return False
  End Function
  Private Sub SortMSUsForIntegration(ByRef msuList As ListViewEx)
    SortMSUsByKBArticle(msuList)
    For I As Integer = 0 To msuList.Items.Count - 1
      SortMSUsByRequirement(msuList, I)
    Next
  End Sub
  Private Sub SortMSUsByKBArticle(ByRef updateList As ListViewEx)
    Dim newList As New List(Of ListViewItem)
    For Each update As ListViewItem In updateList.Items
      newList.Add(update)
    Next
    Dim newArray() As ListViewItem = newList.ToArray
    Dim kSort As New MSUKBSorter
    Array.Sort(Of ListViewItem)(newArray, kSort)
    updateList.Items.Clear()
    updateList.Items.AddRange(newArray)
  End Sub
  Private Class MSUKBSorter
    Implements IComparer(Of ListViewItem)
    Public Function Compare(x As ListViewItem, y As ListViewItem) As Integer Implements System.Collections.Generic.IComparer(Of ListViewItem).Compare
      Dim xData As Updates_Data = Updates_ListData(CInt(x.Tag))
      Dim yData As Updates_Data = Updates_ListData(CInt(y.Tag))
      If xData.Update.Name = "DRIVER" Then
        If Not yData.Update.Name = "DRIVER" Then Return -1
        Dim xSortIndex As String = Nothing
        Dim ySortIndex As String = Nothing
        If String.IsNullOrEmpty(xData.Update.DriverData.DriverStorePath) Or String.IsNullOrEmpty(yData.Update.DriverData.DriverStorePath) Then
          If String.IsNullOrEmpty(xData.Update.DriverData.PublishedName) Or String.IsNullOrEmpty(yData.Update.DriverData.PublishedName) Then
            xSortIndex = xData.Update.DriverData.OriginalFileName
            ySortIndex = yData.Update.DriverData.OriginalFileName
          Else
            xSortIndex = xData.Update.DriverData.PublishedName
            ySortIndex = yData.Update.DriverData.PublishedName
          End If
        Else
          xSortIndex = xData.Update.DriverData.DriverStorePath
          ySortIndex = yData.Update.DriverData.DriverStorePath
        End If
        Return String.Compare(xSortIndex, ySortIndex, True)
      Else
        If yData.Update.Name = "DRIVER" Then Return 1
        If String.IsNullOrEmpty(xData.Update.KBArticle) Or String.IsNullOrEmpty(yData.Update.KBArticle) Then
          If String.IsNullOrEmpty(xData.Update.Identity) Or String.IsNullOrEmpty(yData.Update.Identity) Then
            If Not Date.Compare(Date.Parse(xData.Update.BuildDate), Date.Parse(yData.Update.BuildDate)) = 0 Then Return Date.Compare(Date.Parse(xData.Update.BuildDate), Date.Parse(yData.Update.BuildDate))
          Else
            If Not String.Compare(xData.Update.Identity, yData.Update.Identity, True) = 0 Then Return String.Compare(xData.Update.Identity, yData.Update.Identity, True)
          End If
        Else
          Dim xKB As Long = NumericVal(xData.Update.KBArticle)
          Dim yKB As Long = NumericVal(yData.Update.KBArticle)
          If xKB < yKB Then Return -1
          If xKB > yKB Then Return 1
        End If
        Return CompareArchitecturesVal(xData.Update.Architecture, yData.Update.Architecture, False)
      End If
    End Function
  End Class
  Private Sub SortMSUsByRequirement(ByRef updateList As ListViewEx, Index As Integer)
    Dim iLoc As Integer = Index
    For I As Integer = 0 To PrerequisiteList.Length - 1
      Dim iData As Updates_Data = Updates_ListData(CInt(updateList.Items(Index).Tag))
      If iData.Update.KBArticle = PrerequisiteList(I).KBWithRequirement Then
        Dim didMove As Boolean = False
        For J As Integer = Index + 1 To updateList.Items.Count - 1
          Dim jData As Updates_Data = Updates_ListData(CInt(updateList.Items(J).Tag))
          For R As Integer = 0 To PrerequisiteList(I).Requirement.Length - 1
            If PrerequisiteList(I).Requirement(R).Contains(jData.Update.KBArticle) Then
              Dim moveItem As ListViewItem = updateList.Items(J)
              updateList.Items.RemoveAt(J)
              updateList.Items.Insert(iLoc, moveItem)
              iLoc+=1
              didMove = True
              Exit For
            End If
          Next
        Next
        If didMove Then
          For L As Integer = Index To iLoc
            SortMSUsByRequirement(updateList, L)
          Next L
        End If
        Exit For
      End If
    Next
  End Sub
  Private Sub SortUpdatesForIntegration(ByRef updateList As List(Of KeyValuePair(Of Update_File, Dictionary(Of String, Boolean))))
    For I As Integer = 0 To updateList.Count - 1
      SortUpdatesByRequirement(updateList, I)
    Next
  End Sub
  Private Class UpdateKBSorter
    Implements IComparer(Of KeyValuePair(Of Update_File, Dictionary(Of String, Boolean)))
    Public Function Compare(x As System.Collections.Generic.KeyValuePair(Of Update_File, System.Collections.Generic.Dictionary(Of String, Boolean)), y As System.Collections.Generic.KeyValuePair(Of Update_File, System.Collections.Generic.Dictionary(Of String, Boolean))) As Integer Implements System.Collections.Generic.IComparer(Of System.Collections.Generic.KeyValuePair(Of Update_File, System.Collections.Generic.Dictionary(Of String, Boolean))).Compare
      If x.Key.Name = "DRIVER" Then
        If y.Key.Name = "DRIVER" Then
          Dim xSortIndex As String = Nothing
          Dim ySortIndex As String = Nothing
          If String.IsNullOrEmpty(x.Key.DriverData.DriverStorePath) Or String.IsNullOrEmpty(y.Key.DriverData.DriverStorePath) Then
            If String.IsNullOrEmpty(x.Key.DriverData.PublishedName) Or String.IsNullOrEmpty(y.Key.DriverData.PublishedName) Then
              xSortIndex = x.Key.DriverData.OriginalFileName
              ySortIndex = y.Key.DriverData.OriginalFileName
            Else
              xSortIndex = x.Key.DriverData.PublishedName
              ySortIndex = y.Key.DriverData.PublishedName
            End If
          Else
            xSortIndex = x.Key.DriverData.DriverStorePath
            ySortIndex = y.Key.DriverData.DriverStorePath
          End If
          Return String.Compare(xSortIndex, ySortIndex, True)
        End If
        Return -1
      Else
        If y.Key.Name = "DRIVER" Then Return 1
        If String.IsNullOrEmpty(x.Key.KBArticle) Or String.IsNullOrEmpty(y.Key.KBArticle) Then
          If String.IsNullOrEmpty(x.Key.Identity) Or String.IsNullOrEmpty(y.Key.Identity) Then
            Return Date.Compare(Date.Parse(x.Key.BuildDate), Date.Parse(y.Key.BuildDate))
          Else
            Return String.Compare(x.Key.Identity, y.Key.Identity, True)
          End If
        Else
          Dim xKB As Long = NumericVal(x.Key.KBArticle)
          Dim yKB As Long = NumericVal(y.Key.KBArticle)
          If xKB = yKB Then Return 0
          If xKB > yKB Then Return 1
          Return -1
        End If
      End If
    End Function
  End Class
  Private Sub SortUpdatesByRequirement(ByRef updateList As List(Of KeyValuePair(Of Update_File, Dictionary(Of String, Boolean))), Index As Integer)
    For I As Integer = 0 To PrerequisiteList.Length - 1
      If updateList(Index).Key.KBArticle = PrerequisiteList(I).KBWithRequirement Then
        Dim didMove As Boolean = False
        For J As Integer = Index + 1 To updateList.Count - 1
          For R As Integer = 0 To PrerequisiteList(I).Requirement.Length - 1
            If PrerequisiteList(I).Requirement(R).Contains(updateList(J).Key.KBArticle) Then
              Dim moveItem As KeyValuePair(Of Update_File, Dictionary(Of String, Boolean)) = updateList(J)
              updateList.RemoveAt(J)
              updateList.Insert(Index, moveItem)
              didMove = True
            End If
          Next
        Next
        If didMove Then SortUpdatesByRequirement(updateList, Index)
        Exit For
      End If
    Next
  End Sub
  Private Function FindBigFiles(directory As String) As List(Of String)
    Dim myFileList As New List(Of String)
    For Each sFile As String In My.Computer.FileSystem.GetFiles(directory)
      If My.Computer.FileSystem.GetFileInfo(sFile).Length >= &HFFF00000L Then
        myFileList.Add(sFile)
      End If
    Next
    For Each sDir As String In My.Computer.FileSystem.GetDirectories(directory)
      Application.DoEvents()
      Dim fileRet As List(Of String) = FindBigFiles(sDir)
      If fileRet.Count > 0 Then myFileList.AddRange(fileRet)
    Next
    Return myFileList
  End Function
  Friend Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
    If m.Msg = &H20A Then
      Dim pos As New Point(m.LParam.ToInt32 And &HFFFF, m.LParam.ToInt32 >> 16)
      Dim hWnd As IntPtr = WindowFromPoint(pos)
      If (Not hWnd = IntPtr.Zero) And (Not hWnd = m.HWnd) And (Not Control.FromHandle(hWnd) Is Nothing) Then
        SendMessageA(hWnd, m.Msg, m.WParam, m.LParam)
        Return True
      End If
    End If
    Return False
  End Function
#End Region
#Region "Update Check"
  Private WithEvents cUpdateCheck As clsUpdate
  Private Sub tmrUpdateCheck_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdateCheck.Tick
    tmrUpdateCheck.Stop()
    If mySettings.LastUpdate.Year = 1970 Then mySettings.LastUpdate = Today
    If DateDiff(DateInterval.Day, mySettings.LastUpdate, Today) > 13 Then
      mySettings.LastUpdate = Today
      Status_SetText("Beginning Update Check...")
      cUpdateCheck = New clsUpdate
      Dim updateCaller As New MethodInvoker(AddressOf cUpdateCheck.CheckVersion)
      updateCaller.BeginInvoke(Nothing, Nothing)
    End If
  End Sub
  Private Sub cUpdateCheck_CheckingVersion(sender As Object, e As System.EventArgs) Handles cUpdateCheck.CheckingVersion
    If Status_GetText() = "Beginning Update Check..." Or Status_GetText() = "Checking for new Version..." Then Status_SetText("Checking for new Version...")
  End Sub
  Private Sub cUpdateCheck_CheckProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdateCheck.CheckProgressChanged
    If Status_GetText() = "Beginning Update Check..." Or Status_GetText() = "Checking for new Version..." Then Status_SetText(String.Format("Checking for new Version... ({0}%)", e.ProgressPercentage))
  End Sub
  Private Sub cUpdateCheck_CheckResult(sender As Object, e As clsUpdate.CheckEventArgs) Handles cUpdateCheck.CheckResult
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(Of clsUpdate.CheckEventArgs)(AddressOf cUpdateCheck_CheckResult), sender, e)
      Return
    End If
    If e.Cancelled Then
      Status_SetText("Update Check Cancelled!")
    ElseIf e.Error IsNot Nothing Then
      Status_SetText(String.Format("Update Check Failed: {0}!", e.Error.Message))
    Else
      If e.Result = clsUpdate.CheckEventArgs.ResultType.NewUpdate Then
        Status_SetText("New Version Available!")
        If MsgDlg(Me, "Would you like to update now?", String.Format("{1} v{0} is available!", e.Version, Application.ProductName), "Application Update", MessageBoxButtons.YesNo, _TaskDialogIcon.InternetRJ45) = Windows.Forms.DialogResult.Yes Then
          cUpdateCheck.DownloadUpdate(IO.Path.Combine(WorkDir, "Setup.exe"))
        End If
      Else
        Status_SetText("Idle")
        Donate_Show()
      End If
    End If
  End Sub
  Private Sub cUpdateCheck_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles cUpdateCheck.DownloadingUpdate
    Status_SetText("Downloading New Version - Waiting for Response...")
  End Sub
  Private Sub cUpdateCheck_DownloadResult(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles cUpdateCheck.DownloadResult
    If Me.InvokeRequired Then
      Me.Invoke(New System.ComponentModel.AsyncCompletedEventHandler(AddressOf cUpdateCheck_DownloadResult), sender, e)
      Return
    End If
    If e.Cancelled Then
      Status_SetText("Update Download Cancelled!")
    ElseIf e.Error IsNot Nothing Then
      Status_SetText(String.Format("Update Download Failed: {0}!", e.Error.Message))
    Else
      cUpdateCheck.Dispose()
      Status_SetText("Download Complete!")
      Dim RecheckOnMissing As Boolean = False
      Do
        Application.DoEvents()
        Try
          If My.Computer.FileSystem.FileExists(IO.Path.Combine(WorkDir, "Setup.exe")) Then
            Shell(String.Format("{0} /silent", IO.Path.Combine(WorkDir, "Setup.exe")), AppWinStyle.NormalFocus, False)
            Application.Exit()
            Return
          ElseIf RecheckOnMissing Then
            Status_SetText("Update Failure!")
            If MsgDlg(Me, String.Concat("The update file no longer exists in the location it was downloaded to.", vbNewLine, String.Format("If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the {0} Installer.", My.Application.Info.ProductName)), "The update installer is missing.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning) = Windows.Forms.DialogResult.Cancel Then Exit Do
          Else
            Status_SetText("Update Failure!")
            MsgDlg(Me, String.Concat("The update file no longer exists in the location it was downloaded to.", vbNewLine, String.Format("If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the {0} Installer.", My.Application.Info.ProductName)), "The update installer is missing.", "Software Update Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldWarning)
            Exit Do
          End If
        Catch ex As Exception
          Status_SetText("Update Failure!")
          If MsgDlg(Me, String.Concat(String.Format("If you have User Account Control enabled, please allow the {1} Installer to run.", Application.ProductName), vbNewLine, "If you are running any Anti-Virus software, please make sure it hasn't quarantined or deleted the {0} Installer."), "There was an error starting the update.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning, , ex.Message) = Windows.Forms.DialogResult.Cancel Then Exit Do
          RecheckOnMissing = True
        End Try
      Loop
    End If
  End Sub
  Private Sub cUpdateCheck_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdateCheck.UpdateProgressChanged
    Status_SetText(String.Format("Downloading New Version - {0} of {1}... {2}%)", ByteSize(e.BytesReceived), ByteSize(e.TotalBytesToReceive), e.ProgressPercentage))
  End Sub
#End Region
End Class
