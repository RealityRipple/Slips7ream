Imports Microsoft.WindowsAPICodePack.Dialogs
Public Class frmMain
  Implements IMessageFilter
  Public StopRun As Boolean = False
  Friend taskBar As TaskbarLib.TaskbarList
  Private LangChange As Boolean = False
  Private RunComplete As Boolean = False
  Private RunActivity As Byte = 0
  Private tLister As Threading.Thread
  Private tLister2 As Threading.Thread
  Private tWIMDrag As Threading.Thread
  Private tListUp As Threading.Thread
  Private Const HeightDifferentialA As Integer = 126
  Private Const HeightDifferentialB As Integer = 22
  Private Const FrameInterval As UInteger = 3
  Private WithEvents cUpdate As clsUpdate
  Private FrameNumber As UInteger
  Private FrameCount As UInteger
  Private mngDisp As MNG
  Private fTitleFont As New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular)
  Private sTitleText As String
  Private windowChangedSize As Boolean
  Private mySettings As MySettings
  Private outputWindow As Boolean = False
  Private PrerequisiteList() As Prerequisite = {New Prerequisite("2592687:2574819"),
                                                New Prerequisite("2830477:2574819,2857650"),
                                                New Prerequisite("2718695:2533623,2639308,2670838,2729094,2731771,2786081"),
                                                New Prerequisite("2841134:2533623,2639308,2670838,2729094,2731771,2786081,2834140,2882822,2888049,2849696,2849697"),
                                                New Prerequisite("2923545:2830477"),
                                                New Prerequisite("2965788:2830477"),
                                                New Prerequisite("2984976:2830477,2984972,2592687"),
                                                New Prerequisite("3020388:2830477"),
                                                New Prerequisite("3042058:3020369"),
                                                New Prerequisite("3075226:2830477"),
                                                New Prerequisite("3126446:2830477")}
  Private Enum MNGList
    Move
    Copy
    Delete
  End Enum
  Private Enum ReleaseType
    Starter
    HomeBasic
    HomePremium
    Professional
    Ultimate
    Enterprise
    Multiple
    All
  End Enum
  Private Enum Architecture
    x86
    x64
    Universal
  End Enum
  Private Enum BuildType
    Release
    Debug
    Volume
  End Enum
  Private Enum PurposeType
    Retail
    OEM
  End Enum
  Private Structure Prerequisite
    Public KBWithRequirement As String
    Public Requirement() As String
    Public Sub New(Input As String)
      If Not Input.Contains(":") Then Return
      Dim splitInput() As String = Split(Input, ":", 2)
      KBWithRequirement = splitInput(0)
      If Not splitInput(1).Contains(",") Then
        Requirement = {splitInput(1)}
      Else
        Requirement = Split(splitInput(1), ",")
      End If
    End Sub
  End Structure
#Region "GUI"
  Public Sub New()
    InitializeComponent()
    Application.AddMessageFilter(Me)
  End Sub
  Private Sub frmMain_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
    redrawCaption()
  End Sub
  Private Sub frmMain_Deactivate(sender As Object, e As System.EventArgs) Handles Me.Deactivate
    redrawCaption()
  End Sub
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Me.Tag = "LOADING"
    mySettings = New MySettings
    imlUpdates.Images.Add("MSU", My.Resources.update_wusa)
    imlUpdates.Images.Add("DIR", My.Resources.update_folder)
    imlUpdates.Images.Add("CAB", My.Resources.update_cab)
    imlUpdates.Images.Add("MLC", My.Resources.update_mlc)
    imlUpdates.Images.Add("INF", My.Resources.inf)
    ttInfo.SetToolTip(expOutput.pctExpander, "Show Output console.")
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
      SetLimitValues(cmbLimitType.SelectedIndex)
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
      Me.Location = New Point(Screen.PrimaryScreen.Bounds.Left + (Screen.PrimaryScreen.Bounds.Width / 2) - (Me.Width / 2), Screen.PrimaryScreen.Bounds.Top + (Screen.PrimaryScreen.Bounds.Height / 2) - (Me.Height / 2))
    ElseIf Screen.PrimaryScreen.Bounds.Contains(mySettings.Position) Then
      Me.Location = mySettings.Position
    Else
      Me.Location = New Point(Screen.PrimaryScreen.Bounds.Left + (Screen.PrimaryScreen.Bounds.Width / 2) - (Me.Width / 2), Screen.PrimaryScreen.Bounds.Top + (Screen.PrimaryScreen.Bounds.Height / 2) - (Me.Height / 2))
    End If
    If VisualStyles.VisualStyleInformation.DisplayName = "Aero style" And TaskbarLib.TaskbarFinder.TaskbarVisible Then
      If taskBar Is Nothing Then taskBar = New TaskbarLib.TaskbarList
    Else
      taskBar = Nothing
    End If
    chkLoadFeatures.Checked = mySettings.LoadFeatures
    chkLoadUpdates.Checked = mySettings.LoadUpdates
    chkLoadDrivers.Checked = mySettings.LoadDrivers
    ToggleInputs(True)
    SetDisp(MNGList.Move)
    FreshDraw()
    Me.Tag = Nothing
  End Sub
  Private Sub frmMain_LocationChanged(sender As Object, e As System.EventArgs) Handles Me.LocationChanged
    If outputWindow Then frmOutput.RePosition()
  End Sub
  Private Sub frmMain_ResizeBegin(sender As Object, e As System.EventArgs) Handles Me.ResizeBegin
    windowChangedSize = False
  End Sub
  Private windowStateSaved As FormWindowState
  Private Sub frmMain_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    windowChangedSize = True
    RedoColumns()
    If Me.Tag Is Nothing And Me.Visible Then
      mySettings.Position = Me.Location
      If pbTotal.Visible Then
        If txtOutput.Visible Then
          mySettings.Size = New Size(Me.Width, Me.Height - (HeightDifferentialA + HeightDifferentialB))
        Else
          mySettings.Size = New Size(Me.Width, Me.Height - HeightDifferentialB)
        End If
      Else
        mySettings.Size = Me.Size
      End If
    End If
    If Not tmrAnimation.Enabled Then
      FreshDraw()
    End If
    If Not windowStateSaved = Me.WindowState Then
      windowStateSaved = Me.WindowState
      If pnlSP64.Visible And Me.windowStateSaved = FormWindowState.Normal Then
        Me.Height += 1
        Me.Height -= 1
      End If
    End If
    If outputWindow Then frmOutput.RePosition()
  End Sub
  Private Sub frmMain_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
    If Not windowStateSaved = Me.WindowState Then windowChangedSize = True
    If pnlSP64.Visible And windowChangedSize Then
      Me.Height += 1
      Me.Height -= 1
    End If
    windowChangedSize = False
    RedoColumns()
  End Sub
  Private Sub spltSlips7ream_SplitterMoved(sender As System.Object, e As System.Windows.Forms.SplitterEventArgs) Handles spltSlips7ream.SplitterMoved
    RedoColumns()
  End Sub
  Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.WindowsShutDown Then
      StopRun = True
      Return
    End If
    If RunActivity > 0 Then
      Dim sActivity As String = "doing work"
      Dim sProc As String = "current"
      Dim sTitle As String = "Working"
      Select Case RunActivity
        Case 1
          sActivity = "slipstreaming updates and packages"
          sProc = "update integration"
          sTitle = "Integrating"
        Case 2
          sActivity = "extracting and reading Image Package data"
          sProc = "extraction"
          sTitle = "Loading Package Data"
        Case 3
          sActivity = "extracting and reading Update data"
          sProc = "update parsing"
          sTitle = "Loading Updates"
      End Select
      If MsgDlg(Me, "Do you want to cancel the " & sProc & " proceess and close SLIPS7REAM?", "SLIPS7REAM is busy " & sActivity & ".", "Stop " & sTitle & " and Close?", MessageBoxButtons.YesNo, TaskDialogIcon.Question, MessageBoxDefaultButton.Button2, , "Stop " & sTitle & " and Close") = Windows.Forms.DialogResult.No Then
        e.Cancel = True
        Return
      End If
    End If
    StopRun = True
    CloseCleanup()
  End Sub
  Private Sub CloseCleanup()
    If IO.Directory.Exists(WorkDir) Then
      SetTitle("Cleaning Up Files", "Cleaning up mounts, work, and temporary directories...")
      SetDisp(MNGList.Delete)
      ToggleInputs(False)
      cmdClose.Enabled = False
      StopRun = False
      CleanMounts()
      SetStatus("Clearing Temp Directory...")
      WriteToOutput("Deleting """ & WorkDir & """...")
      Try
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Catch ex As Exception
      End Try
      StopRun = True
      pctTitle.Image = Nothing
      mngDisp = Nothing
    End If
  End Sub
  Private Sub tmrAnimation_Tick(sender As System.Object, e As System.EventArgs) Handles tmrAnimation.Tick
    If mngDisp IsNot Nothing Then
      Dim bmpFrame As Bitmap = mngDisp.ToBitmap(FrameNumber)
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
          g.DrawString(sTitleText, fTitleFont, Brushes.Black, New Point(16, 8))
          g.DrawString(sTitleText, fTitleFont, Brushes.White, New Point(15, 7))
        End Using
        pctTitle.Image = bmpDisplay.Clone
      End Using
    Else
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
          g.DrawString(sTitleText, fTitleFont, Brushes.Black, New Point(16, 8))
          g.DrawString(sTitleText, fTitleFont, Brushes.White, New Point(15, 7))
        End Using
        pctTitle.Image = bmpDisplay.Clone
      End Using
    End If
    FrameNumber += FrameInterval
    If FrameNumber > FrameCount Then FrameNumber = 1
    tmrAnimation.Interval = FrameInterval * 10
  End Sub
  Private Sub FreshDraw()
    Dim digits As Integer = 4
    If My.Application.Info.Version.Revision = 0 Then
      digits = 3
      If My.Application.Info.Version.Build = 0 Then
        digits = 2
      End If
    End If
    SetTitle(My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString(digits), My.Application.Info.ProductName & " - Windows 7 Image Slipstream Utility by " & My.Application.Info.CompanyName)
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
        g.DrawString(sTitleText, fTitleFont, Brushes.Black, New Point(16, 8))
        g.DrawString(sTitleText, fTitleFont, Brushes.White, New Point(15, 7))
      End Using
      pctTitle.Image = bmpDisplay.Clone
    End Using
  End Sub
  Private Sub SetDisp(DispType As MNGList)
    If mngDisp IsNot Nothing Then mngDisp = Nothing
    mngDisp = New MNG
    Select Case DispType
      Case MNGList.Move
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng", My.Resources.move, False)
      Case MNGList.Copy
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng", My.Resources.copy, False)
      Case MNGList.Delete
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng", My.Resources.delete, False)
    End Select
    If IO.File.Exists(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng") Then
      If mngDisp.Load(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng") Then
        FrameNumber = 0
        FrameCount = mngDisp.NumEmbeddedPNG - 1
      Else
        mngDisp = Nothing
      End If
      IO.File.Delete(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng")
    End If
  End Sub
  Private Sub RedoColumns()
    If Not lvMSU.Columns.Count = 0 Then
      lvMSU.BeginUpdate()
      lvMSU.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvMSU.Columns(1).Width < 75 Then lvMSU.Columns(1).Width = 75
      Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 2
      If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
      lvMSU.EndUpdate()
    End If
    If Not lvImages.Columns.Count = 0 Then
      lvImages.BeginUpdate()
      Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width) - 2
      If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
      lvImages.EndUpdate()
    End If
    If cmdAddMSU.Enabled Then
      If Not cmdRemMSU.Enabled = (lvMSU.SelectedItems.Count > 0) Then cmdRemMSU.Enabled = (lvMSU.SelectedItems.Count > 0)
      If Not cmdClearMSU.Enabled = (lvMSU.Items.Count > 0) Then cmdClearMSU.Enabled = (lvMSU.Items.Count > 0)
    End If
    If pctOutputTear.Visible Then redrawCaption()
  End Sub
  Private Sub redrawCaption()
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
        g.DrawString("SLIPS7REAM Output Console", SystemFonts.SmallCaptionFont, SystemBrushes.ActiveCaptionText, New RectangleF(3, 1, pctOutputTear.Width - 6, pctOutputTear.Height - 2), New StringFormat(StringFormatFlags.NoWrap Or StringFormatFlags.NoClip))
      End Using
      pctOutputTear.Image = bTitle.Clone
    End Using
  End Sub
  Private Delegate Sub ToggleInputsInvoker(bEnabled As Boolean, sStatus As String)
  Private msuBGList As New Dictionary(Of String, Color)
  Private Sub ToggleInputs(bEnabled As Boolean, Optional sStatus As String = Nothing)
    If Me.InvokeRequired Then
      Me.Invoke(New ToggleInputsInvoker(AddressOf ToggleInputs), bEnabled, sStatus)
      Return
    End If
    If bEnabled Then
      RunActivity = 0
      Me.Cursor = Cursors.Default
      tmrAnimation.Stop()
      FreshDraw()
    Else
      Me.Cursor = Cursors.AppStarting
      tmrAnimation.Start()
    End If
    pnlSlips7ream.SuspendLayout()
    lblWIM.Enabled = bEnabled
    txtWIM.Enabled = bEnabled
    cmdWIM.Enabled = bEnabled
    chkSP.Enabled = bEnabled
    txtSP.Enabled = IIf(bEnabled, chkSP.Checked, bEnabled)
    cmdSP.Enabled = IIf(bEnabled, chkSP.Checked, bEnabled)
    lblSP64.Enabled = IIf(bEnabled, chkSP.Checked, bEnabled)
    txtSP64.Enabled = IIf(bEnabled, chkSP.Checked, bEnabled)
    cmdSP64.Enabled = IIf(bEnabled, chkSP.Checked, bEnabled)
    lblMSU.Enabled = bEnabled
    If Not bEnabled And msuBGList.Count = 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        msuBGList.Add(CType(lvItem.Tag(0), Update_File).Identity, lvItem.BackColor)
      Next
    End If
    lvMSU.ReadOnly = Not bEnabled
    If bEnabled And msuBGList.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        If msuBGList.ContainsKey(CType(lvItem.Tag(0), Update_File).Identity) Then lvItem.BackColor = msuBGList(CType(lvItem.Tag(0), Update_File).Identity)
      Next
      msuBGList.Clear()
    End If
    cmdAddMSU.Enabled = bEnabled
    cmdRemMSU.Enabled = IIf(bEnabled, lvMSU.SelectedItems.Count > 0, bEnabled)
    cmdClearMSU.Enabled = IIf(bEnabled, lvMSU.Items.Count > 0, bEnabled)
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
    txtISO.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    cmdISO.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    lblISOFeatures.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    chkUnlock.Enabled = IIf(bEnabled, chkISO.Checked And (chkUnlock.Tag Is Nothing), bEnabled)
    chkUEFI.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    lblISOLabel.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    chkAutoLabel.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    txtISOLabel.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    lblISOFS.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    cmbISOFormat.Enabled = IIf(bEnabled, chkISO.Checked, bEnabled)
    cmbLimitType.Enabled = bEnabled
    If chkISO.Checked Then
      If Not cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Add("Split ISO")
    Else
      If cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Remove("Split ISO")
    End If
    If cmbLimitType.SelectedIndex = -1 Then cmbLimitType.SelectedIndex = 0
    cmbLimit.Enabled = IIf(bEnabled, cmbLimitType.SelectedIndex > 0, bEnabled)
    chkMerge.Enabled = bEnabled
    txtMerge.Enabled = IIf(bEnabled, chkMerge.Checked, bEnabled)
    cmdMerge.Enabled = IIf(bEnabled, chkMerge.Checked, bEnabled)
    lblImages.Enabled = bEnabled
    chkLoadFeatures.Enabled = IIf(bEnabled, Not String.IsNullOrEmpty(txtWIM.Text), False)
    chkLoadUpdates.Enabled = IIf(bEnabled, Not String.IsNullOrEmpty(txtWIM.Text), False)
    chkLoadDrivers.Enabled = IIf(bEnabled, Not String.IsNullOrEmpty(txtWIM.Text), False)
    cmdLoadPackages.Enabled = IIf(bEnabled, Not String.IsNullOrEmpty(txtWIM.Text) And (chkLoadFeatures.Checked Or chkLoadUpdates.Checked Or chkLoadDrivers.Checked), False)
    lvImages.ReadOnly = Not bEnabled
    If bEnabled Then
      cmdClose.Text = "&Close"
      Me.CancelButton = Nothing
      If String.IsNullOrEmpty(sStatus) Then
        SetStatus("Idle")
      Else
        SetStatus(sStatus)
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
  Private Sub AskForDonations()
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
  Public Sub ClickedDonate()
    Try
      mySettings.LastNag = DateAdd(DateInterval.Month, 24, Today)
    Catch ex As Exception
    End Try
  End Sub
#Region "Menus"
  Private Sub mnuOutput_Popup(sender As System.Object, e As System.EventArgs) Handles mnuOutput.Popup
    Dim mParent As ContextMenu = sender
    Dim txtSel As TextBox = mParent.SourceControl
    mnuSelectAll.Enabled = Not txtSel.TextLength = 0
    mnuClear.Enabled = Not txtSel.TextLength = 0
    mnuCopy.Enabled = Not txtSel.SelectedText.Length = 0
  End Sub
  Private Sub mnuCopy_Click(sender As System.Object, e As System.EventArgs) Handles mnuCopy.Click
    Dim mSender As MenuItem = sender
    Dim mParent As ContextMenu = mSender.Parent
    Dim txtSel As TextBox = mParent.SourceControl
    If Not txtSel.SelectedText.Length = 0 Then Clipboard.SetText(txtSel.SelectedText)
  End Sub
  Private Sub mnuClear_Click(sender As System.Object, e As System.EventArgs) Handles mnuClear.Click
    Dim mSender As MenuItem = sender
    Dim mParent As ContextMenu = mSender.Parent
    Dim txtSel As TextBox = mParent.SourceControl
    txtSel.Text = ""
  End Sub
  Private Sub mnuSelectAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuSelectAll.Click
    Dim mSender As MenuItem = sender
    Dim mParent As ContextMenu = mSender.Parent
    Dim txtSel As TextBox = mParent.SourceControl
    If Not txtSel.TextLength = 0 Then
      txtSel.SelectionStart = 0
      txtSel.SelectionLength = txtSel.TextLength
      txtSel.Focus()
    End If
  End Sub
#End Region
#Region "Textbox Drag/Drop"
  Private Sub TextBoxDragDropEvent(sender As TextBox, e As System.Windows.Forms.DragEventArgs)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If Data.Length = 1 Then
        sender.Text = Data(0)
      End If
    End If
  End Sub
  Public Sub TextBoxDragEnterEvent(sender As TextBox, e As DragEventArgs)
    e.Effect = DragDropEffects.Copy
  End Sub
  Public Sub TextBoxDragOverEvent(sender As TextBox, e As DragEventArgs, AllowedTypes() As String)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If Data.Length = 1 Then
        Dim hasImage As Boolean = False
        For Each aType As String In AllowedTypes
          If IO.Path.GetExtension(Data(0)).ToLower = aType Then
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
  End Sub
#End Region
#Region "WIM"
  Private Sub txtWIM_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtWIM.DragDrop
    TextBoxDragDropEvent(sender, e)
  End Sub
  Private Sub SetISOtoWIM()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf SetISOtoWIM))
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
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtWIM_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtWIM.DragOver
    TextBoxDragOverEvent(sender, e, {".wim", ".iso"})
  End Sub
  Private Sub txtWIM_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtWIM.TextChanged
    If Not IO.File.Exists(txtWIM.Text) Then Return
    RunComplete = False
    StopRun = False
    RunActivity = 2
    cmdBegin.Text = "&Begin"
    cmdLoadPackages.Image = My.Resources.u_n
    cmdOpenFolder.Visible = False
    If tLister Is Nothing Then
      tLister = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ParseImageList))
      tLister.Start("WIM")
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
    txtSP.Enabled = IIf(chkSP.Enabled, chkSP.Checked, False)
    cmdSP.Enabled = IIf(chkSP.Enabled, chkSP.Checked, False)
    If chkSP.Checked Then
      Dim has86 As Boolean = False
      Dim has64 As Boolean = False
      For Each lvItem As ListViewItem In lvImages.Items
        Dim propData As ImagePackage = lvItem.Tag(1)
        If propData.Architecture.Contains("64") Then has64 = True
        If propData.Architecture.Contains("86") Then has86 = True
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
    RedoColumns()
  End Sub
  Private Sub txtSP_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragDrop
    TextBoxDragDropEvent(sender, e)
  End Sub
  Private Sub txtSP_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragEnter
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtSP_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP.DragOver
    TextBoxDragOverEvent(sender, e, {".exe"})
  End Sub
  Private Sub txtSP_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSP.TextChanged
    SetStatus("Idle")
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
    TextBoxDragDropEvent(sender, e)
  End Sub
  Private Sub txtSP64_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP64.DragEnter
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtSP64_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtSP64.DragOver
    TextBoxDragOverEvent(sender, e, {".exe"})
  End Sub
  Private Sub txtSP64_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSP64.TextChanged
    SetStatus("Idle")
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
  Private Sub lvMSU_DoubleClick(sender As Object, e As System.EventArgs) Handles lvMSU.DoubleClick
    If lvMSU.SelectedItems IsNot Nothing AndAlso lvMSU.SelectedItems.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.SelectedItems
        Dim msuData As Update_File = lvItem.Tag(0)
        If Not String.IsNullOrEmpty(msuData.Failure) Then If ExtractFailureAlert(msuData.Failure) Then Continue For
        If Not String.IsNullOrEmpty(msuData.Name) Then
          If msuData.Name = "DRIVER" Then
            Dim props As New frmDriverProps(msuData.DriverData)
            props.Show(Me)
          Else
            For Each Form In OwnedForms
              If Form.Tag = msuData.Name Then
                Form.Focus()
                Return
              End If
            Next
            Dim props As New frmUpdateProps
            props.Tag = msuData.Name
            props.Text = GetUpdateName(msuData.Ident) & " Properties"
            props.txtName.Text = msuData.Name
            props.txtDisplayName.Text = msuData.DisplayName
            props.txtIdentity.Text = msuData.Identity
            props.txtAppliesTo.Text = msuData.AppliesTo
            props.txtArchitecture.Text = msuData.Architecture
            props.txtVersion.Text = msuData.KBVersion
            props.txtBuildDate.Text = msuData.BuildDate
            If String.IsNullOrEmpty(msuData.SupportLink) Then
              props.lblKBLink.Link = False
              props.lblKBLink.Tag = Nothing
              If String.IsNullOrEmpty(msuData.KBArticle) Then
                props.lblKBLink.Text = "Details"
                props.lblKBLink.Visible = False
              Else
                props.lblKBLink.Text = "Article KB" & msuData.KBArticle
                props.lblKBLink.Visible = True
              End If
            Else
              props.lblKBLink.Link = True
              props.lblKBLink.Tag = msuData.SupportLink
              If String.IsNullOrEmpty(msuData.KBArticle) Then
                props.lblKBLink.Text = "Details"
                props.lblKBLink.Visible = True
              Else
                props.lblKBLink.Text = "Article KB" & msuData.KBArticle
                props.lblKBLink.Visible = True
              End If
            End If
            props.ttLink.SetToolTip(props.lblKBLink, msuData.SupportLink)
            props.Show(Me)
          End If
        End If
      Next
    End If
  End Sub
#Region "Drag/Drop"
  Private Function GetFileCountFromDir(Path As String) As Integer
    Dim iCount As Integer = 0
    For Each sFile As String In IO.Directory.EnumerateFiles(Path)
      Select Case IO.Path.GetExtension(sFile).ToLower
        Case ".msu", ".cab", ".mlc", ".exe", ".msi", ".inf"
          iCount += 1
      End Select
    Next
    For Each sDirectory As String In IO.Directory.EnumerateDirectories(Path)
      iCount += GetFileCountFromDir(sDirectory)
    Next
    Return iCount
  End Function
  Private Function GetFilesFromDir(Path As String) As String()
    Dim sFiles As New Collections.Generic.List(Of String)
    For Each sFile As String In IO.Directory.EnumerateFiles(Path)
      sFiles.Add(sFile)
    Next
    For Each sDirectory As String In IO.Directory.EnumerateDirectories(Path)
      Dim sChildren() As String = GetFilesFromDir(sDirectory)
      If Not sChildren Is Nothing AndAlso sChildren.Count > 0 Then sFiles.AddRange(sChildren)
    Next
    Return sFiles.ToArray
  End Function
  Public Sub DragDropEvent(sender As Object, e As DragEventArgs)
    ReplaceAllOldUpdates = TriState.UseDefault
    ReplaceAllNewUpdates = TriState.UseDefault
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data() As String = e.Data.GetData("FileDrop")
      Dim FileCount As Integer
      Dim ReplaceData As Boolean = False
      For Each Path In Data
        If IO.File.Exists(Path) Then
          FileCount += 1
        ElseIf IO.Directory.Exists(Path) Then
          FileCount += GetFileCountFromDir(Path)
          ReplaceData = True
        End If
      Next
      If ReplaceData Then
        Dim newData As New Collections.Generic.List(Of String)
        For Each Path In Data
          If IO.File.Exists(Path) Then
            newData.Add(Path)
          ElseIf IO.Directory.Exists(Path) Then
            Dim sChildren() As String = GetFilesFromDir(Path)
            If Not sChildren Is Nothing AndAlso sChildren.Count > 0 Then newData.AddRange(sChildren)
          End If
        Next
        Data = newData.ToArray
      End If
      If FileCount > 2 Then
        RunActivity = 3
        StopRun = False
        SetDisp(MNGList.Delete)
        SetTitle("Parsing Update Information", "Reading data from update files...")
        ToggleInputs(False)
        SetTotal(0, FileCount)
        SetProgress(0, 0)
        SetStatus("Reading Update Information...")
      End If
      Dim FailCollection As New Collections.Generic.List(Of String)
      Dim iProg As Integer = 0
      lvMSU.BeginUpdate()
      For Each Item In Data
        If FileCount > 2 Then
          iProg += 1
          SetTotal(iProg, FileCount)
          Application.DoEvents()
          If StopRun Then
            SetProgress(0, 1)
            ToggleInputs(True)
            lvMSU.EndUpdate()
            Return
          End If
        End If
        Dim Cancelled As Boolean = False
        Dim msuList As Update_File() = GetUpdateInfo(Item)
        If msuList IsNot Nothing AndAlso msuList.Count > 0 Then
          For Each msuData As Update_File In msuList
            Dim addRet As AddResult = AddToUpdates(msuData)
            If addRet.Cancel Then
              Cancelled = True
              Exit For
            End If
            If Not addRet.Success Then FailCollection.Add(IIf(String.IsNullOrEmpty(msuData.Name), IO.Path.GetFileNameWithoutExtension(Item), IIf(msuData.Name = "DRIVER", IO.Path.GetFileNameWithoutExtension(msuData.DriverData.DriverStorePath), msuData.Name)) & ": " & addRet.FailReason)
            If Not lvMSU.Columns.Count = 0 Then
              'lvMSU.BeginUpdate()
              lvMSU.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
              If lvMSU.Columns(1).Width < 75 Then lvMSU.Columns(1).Width = 75
              Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 1
              If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
              'lvMSU.EndUpdate()
            End If
          Next
        Else
          FailCollection.Add(IO.Path.GetFileNameWithoutExtension(Item) & ": Update not found.")
        End If
        If Cancelled Then Exit For
      Next
      If FileCount > 2 Then
        SetProgress(0, 1)
        ToggleInputs(True)
      End If
      lvMSU.EndUpdate()
      RedoColumns()
      If FailCollection.Count > 0 Then
        MsgDlg(Me, "Some files could not be added to the Update List." & vbNewLine & "Click View Details to see a complete list.", "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, TaskDialogIcon.WindowsUpdate, , CleanupFailures(FailCollection), "Error Adding Updates")
      End If
      SetStatus("Idle")
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Public Sub DragEnterEvent(sender As Object, e As DragEventArgs)
    e.Effect = DragDropEffects.All
  End Sub
  Public Sub DragOverEvent(sender As Object, e As DragEventArgs)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data() As String = e.Data.GetData("FileDrop")
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
  Private sourceRows As New Collections.Generic.List(Of Integer)
  Private Sub lvMSU_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragDrop
    If sourceRows.Count > 0 Then
      sourceRows.Clear()
    Else
      DragDropEvent(sender, e)
    End If
  End Sub
  Private Sub lvMSU_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragEnter
    DragEnterEvent(sender, e)
  End Sub
  Private Sub lvMSU_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lvMSU.DragOver
    If sourceRows.Count > 0 Then
      Dim ptRet As Drawing.Point = lvMSU.PointToClient(New Drawing.Point(e.X, e.Y))
      Dim ht = lvMSU.HitTest(ptRet.X, ptRet.Y)
      Dim TargetRowIndex As Integer = -1
      If ht.Item IsNot Nothing Then
        TargetRowIndex = ht.Item.Index
      End If
      If TargetRowIndex > -1 Then
        If Not sourceRows(0) = TargetRowIndex Then
          Dim targetI As Integer = TargetRowIndex
          For Each item As ListViewItem In lvMSU.SelectedItems
            Dim tmpItem = item.Clone
            item.Remove()
            If lvMSU.Items.Count <= targetI Then
              targetI = CType(lvMSU.Items.Add(tmpItem), ListViewItem).Index
            Else
              lvMSU.Items.Insert(targetI, tmpItem)
            End If
            lvMSU.Items(targetI).Selected = True
            targetI += 1
          Next
          sourceRows.Clear()
          For Each item As ListViewItem In lvMSU.SelectedItems
            sourceRows.Add(item.Index)
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
  Private LastClick As Long = 0
  Private Sub lvMSU_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseDown
    sourceRows.Clear()
    If e.Button = Windows.Forms.MouseButtons.Left And lvMSU.SelectedItems.Count > 0 Then
      For Each item As ListViewItem In lvMSU.SelectedItems
        sourceRows.Add(item.Index)
      Next
    End If
  End Sub
  Private Sub lvMSU_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseMove
    Static lastLoc As Drawing.Point
    If e.Button = Windows.Forms.MouseButtons.Left And lvMSU.SelectedItems.Count > 0 And sourceRows.Count > 0 Then
      If Math.Abs(lastLoc.X - e.Location.X) > 3 Or Math.Abs(lastLoc.Y - e.Location.Y) > 3 Then
        lvMSU.DoDragDrop(lvMSU.SelectedItems, DragDropEffects.Move)
      End If
    End If
  End Sub
  Private Sub lvMSU_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvMSU.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If LastClick > 0 And (TickCount() - LastClick) < 250 Then
        lvMSU_DoubleClick(lvMSU, New EventArgs)
      End If
      sourceRows.Clear()
      LastClick = TickCount()
    ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
      If lvMSU.SelectedItems.Count > 0 Then
        mnuMSU.Tag = lvMSU.SelectedItems
        If lvMSU.SelectedItems.Count > 1 Then
          mnuUpdateTop.Text = "Move Updates to Top"
          mnuUpdateUp.Text = "Move Updates Up"
          mnuUpdateDown.Text = "Move Updates Down"
          mnuUpdateBottom.Text = "Move Updates to Bottom"
          mnuUpdateRemove.Text = "Remove Updates"
          mnuUpdateLocation.Text = "Open Updates Location"
        Else
          mnuUpdateTop.Text = "Move Update to Top"
          mnuUpdateUp.Text = "Move Update Up"
          mnuUpdateDown.Text = "Move Update Down"
          mnuUpdateBottom.Text = "Move Update to Bottom"
          mnuUpdateRemove.Text = "Remove Update"
          mnuUpdateLocation.Text = "Open Update Location"
        End If
        mnuMSU.Show(lvMSU, e.Location)
      End If
    End If
  End Sub
  Private Sub lvMSU_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvMSU.SelectedIndexChanged
    If sourceRows.Count > 0 Then Return
    If lvMSU.SelectedItems.Count > 0 Then
      cmdRemMSU.Enabled = True
      If lvMSU.SelectedItems.Count > 1 Then
        cmdRemMSU.Text = "Remove Updates"
      Else
        cmdRemMSU.Text = "Remove Update"
      End If
    Else
      cmdRemMSU.Enabled = False
      cmdRemMSU.Text = "Remove Updates"
    End If
  End Sub
#End Region
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
#Region "Buttons"
  Private Sub cmdAddMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddMSU.Click
    ReplaceAllOldUpdates = TriState.UseDefault
    ReplaceAllNewUpdates = TriState.UseDefault
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
        If FileCount > 2 Then
          RunActivity = 3
          StopRun = False
          SetDisp(MNGList.Delete)
          SetTitle("Parsing Update Information", "Reading data from update files...")
          ToggleInputs(False)
          SetTotal(0, FileCount)
          SetProgress(0, 0)
          SetStatus("Reading Update Information...")
        End If
        For I As Integer = 0 To cdlBrowse.FileNames.Count - 1
          Dim sUpdate As String = cdlBrowse.FileNames(I)
          If FileCount > 2 Then
            SetTotal(I + 1, FileCount)
            Application.DoEvents()
            If StopRun Then
              SetProgress(0, 1)
              ToggleInputs(True)
              lvMSU.EndUpdate()
              Return
            End If
          End If
          Dim Cancelled As Boolean = False
          Dim msuList As Update_File() = GetUpdateInfo(sUpdate)
          If msuList IsNot Nothing AndAlso msuList.Count > 0 Then
            For Each msuData As Update_File In msuList
              Dim addRet As AddResult = AddToUpdates(msuData)
              If addRet.Cancel Then
                Cancelled = True
                Exit For
              End If
              If Not addRet.Success Then FailCollection.Add(IIf(String.IsNullOrEmpty(msuData.Name), IO.Path.GetFileNameWithoutExtension(sUpdate), IIf(msuData.Name = "DRIVER", IO.Path.GetFileNameWithoutExtension(msuData.DriverData.DriverStorePath), msuData.Name)) & ": " & addRet.FailReason)
              If Not lvMSU.Columns.Count = 0 Then
                lvMSU.BeginUpdate()
                lvMSU.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                If lvMSU.Columns(1).Width < 75 Then lvMSU.Columns(1).Width = 75
                Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 1
                If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
                lvMSU.EndUpdate()
              End If
            Next
          Else
            FailCollection.Add(IO.Path.GetFileNameWithoutExtension(sUpdate) & ": Update not found.")
          End If
          If Cancelled Then Exit For
        Next
        If FileCount > 2 Then
          SetProgress(0, 1)
          ToggleInputs(True)
        End If
        RedoColumns()
        If FailCollection.Count > 0 Then
          MsgDlg(Me, "Some files could not be added to the Update List." & vbNewLine & "Click View Details to see a complete list.", "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, TaskDialogIcon.WindowsUpdate, , CleanupFailures(FailCollection), "Error Adding Updates")
        End If
      End If
    End Using
    SetStatus("Idle")
  End Sub
  Private Class AddResult
    Public Success As Boolean
    Public FailReason As String
    Public Cancel As Boolean
    Public Sub New(b As Boolean, Optional s As String = "")
      Success = b
      If Not b Then
        If String.IsNullOrEmpty(s) Then
          Cancel = True
        Else
          FailReason = s
        End If
      End If
    End Sub
  End Class
  Dim ReplaceAllOldUpdates As TriState = TriState.UseDefault
  Dim ReplaceAllNewUpdates As TriState = TriState.UseDefault
  Private Function AddToUpdates(msuData As Update_File) As AddResult
    If Not String.IsNullOrEmpty(msuData.Failure) Then Return New AddResult(False, msuData.Failure)
    If String.IsNullOrEmpty(msuData.Name) Then Return New AddResult(False, "Unable to parse information.")
    If GetUpdateType(msuData.Path) = UpdateType.Other Then
      If String.IsNullOrEmpty(IO.Path.GetExtension(msuData.Path)) Then
        Return New AddResult(False, "Unknown Update Type.")
      Else
        Return New AddResult(False, "Unknown Update Type: """ & IO.Path.GetExtension(msuData.Path).Substring(1).ToUpper & """.")
      End If
    End If
    Dim PList(lvImages.Items.Count - 1) As ImagePackage
    If msuData.Name = "DRIVER" Then
      Dim drvData As Driver = msuData.DriverData
      If String.IsNullOrEmpty(drvData.DriverStorePath) Then Return New AddResult(False)
      Dim InImage(lvImages.Items.Count - 1) As Driver
      If lvImages.Items.Count > 0 Then
        For I As Integer = 0 To lvImages.Items.Count - 1
          If lvImages.Items(I).Checked Then
            PList(I) = lvImages.Items(I).Tag(1)
            Dim dInfo As List(Of Driver) = lvImages.Items(I).Tag(3)
            If dInfo IsNot Nothing Then
              For Each driver As Driver In dInfo
                If drvData = driver Then Return New AddResult(False, "Driver already integrated.")
              Next
            End If
          End If
        Next
      End If
      For Each item As ListViewItem In lvMSU.Items
        Dim itemData As Update_File = item.Tag(0)
        If itemData.Name = "DRIVER" AndAlso itemData.DriverData = drvData Then
          Return New AddResult(False, "Driver already added.")
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
      Dim en As String = ChrW(&H2003)
      Dim ttPublishedName As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.PublishedName) Then
        ttPublishedName = IO.Path.GetFileName(msuData.DriverData.PublishedName)
        If Not String.IsNullOrEmpty(msuData.DriverData.Version) Then ttPublishedName &= " v" & msuData.DriverData.Version
      End If
      Dim ttOriginalFileName As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.OriginalFileName) Then ttOriginalFileName = en & "Original File Name: " & msuData.DriverData.OriginalFileName
      Dim ttDriverStorePath As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.DriverStorePath) Then ttDriverStorePath = en & "Driver Store Path: " & msuData.DriverData.DriverStorePath
      Dim ttInBox As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.InBox) Then ttInBox = en & "In-Box: " & msuData.DriverData.InBox
      Dim ttClassName As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.ClassName) Then
        ttClassName = en & "Class Name: " & msuData.DriverData.ClassName
        If Not String.IsNullOrEmpty(msuData.DriverData.ClassDescription) Then ttClassName &= " (" & msuData.DriverData.ClassDescription & ")"
      ElseIf Not String.IsNullOrEmpty(msuData.DriverData.ClassDescription) Then
        ttClassName = en & "Class Description: " & msuData.DriverData.ClassDescription
      End If
      Dim ttClassGUID As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.ClassGUID) Then ttClassGUID = en & "Class GUID: " & msuData.DriverData.ClassGUID
      Dim ttProviderName As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.ProviderName) Then ttProviderName = en & "Provider: " & msuData.DriverData.ProviderName
      Dim ttDate As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.Date) Then ttDate = en & "Date: " & msuData.DriverData.Date
      Dim ttArch As String = Nothing
      If msuData.DriverData.Architectures IsNot Nothing AndAlso msuData.DriverData.Architectures.Count > 0 Then ttArch = en & "Supported Architectures: " & Join(msuData.DriverData.Architectures.ToArray, ", ")
      Dim ttBootCritical As String = Nothing
      If Not String.IsNullOrEmpty(msuData.DriverData.BootCritical) Then ttBootCritical = en & "Boot Critical: " & msuData.DriverData.BootCritical
      Dim myTag(1) As Object
      myTag(0) = msuData
      Dim ttNotice As String = Nothing
      lvItem.ToolTipText = IIf(String.IsNullOrEmpty(ttPublishedName), "", ttPublishedName & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttOriginalFileName), "", ttOriginalFileName & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttDriverStorePath), "", ttDriverStorePath & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttInBox), "", ttInBox & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttClassName), "", ttClassName & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttClassGUID), "", ttClassGUID & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttProviderName), "", ttProviderName & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttDate), "", ttDate & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttArch), "", ttArch & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttBootCritical), "", ttBootCritical & vbNewLine) &
                           IIf(String.IsNullOrEmpty(ttNotice), "", ttNotice & vbNewLine)
      lvItem.Tag = myTag
      lvItem.ForeColor = lvMSU.ForeColor
      lvItem.ImageKey = "INF"
      If msuData.DriverData.Architectures Is Nothing Then
        lvItem.SubItems.Add("Driver")
      Else
        If msuData.DriverData.Architectures.Contains("x86") Then
          If msuData.DriverData.Architectures.Contains("amd64") Then
            lvItem.SubItems.Add("Universal Driver")
          Else
            lvItem.SubItems.Add("x86 Driver")
          End If
        ElseIf msuData.DriverData.Architectures.Contains("amd64") Then
          lvItem.SubItems.Add("amd64 Driver")
        ElseIf msuData.DriverData.Architectures.Contains("ia64") Then
          Return New AddResult(False, "Driver is for Itanium processors.")
        Else
          Return New AddResult(False, "Driver is for " & Join(msuData.DriverData.Architectures.ToArray, ", ") & " processors.")
        End If
      End If
      lvMSU.Items.Add(lvItem)
      Return New AddResult(True)
    Else
      Dim InOne As Boolean = False
      Dim OnlyUp As Boolean = False
      Dim OnlyDown As Boolean = False
      Dim InImage(lvImages.Items.Count - 1) As Update_Integrated
      If lvImages.Items.Count > 0 Then
        For I As Integer = 0 To lvImages.Items.Count - 1
          If lvImages.Items(I).Checked Then
            PList(I) = lvImages.Items(I).Tag(1)
            If PList(I).IntegratedUpdateList IsNot Nothing Then
              For Each package As Update_Integrated In PList(I).IntegratedUpdateList
                If msuData.Ident.Name = package.Ident.Name Then
                  If msuData.Ident.Architecture = package.Ident.Architecture Then
                    If msuData.Ident.Language = package.Ident.Language Then
                      InImage(I) = package
                      InImage(I).Parent = PList(I)
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
          For I As Integer = 0 To InImage.Length - 1
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
            Return New AddResult(False, "Update already integrated.")
          ElseIf allLess Then
            Dim always As Boolean = False
            If ReplaceAllOldUpdates = TriState.True Then
              OnlyUp = True
            ElseIf ReplaceAllOldUpdates = TriState.False Then
              Return New AddResult(True)
            Else
              Dim sbRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage, PList.ToArray, always, Comparison.Newer)
              If sbRet = TaskDialogResult.Yes Then
                If always Then ReplaceAllOldUpdates = TriState.True
                OnlyUp = True
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then ReplaceAllOldUpdates = TriState.False
                Return New AddResult(True)
              Else
                Return New AddResult(False)
              End If
            End If
          ElseIf allGreater Then
            Dim always As Boolean = False
            If ReplaceAllNewUpdates = TriState.True Then
              OnlyDown = True
            ElseIf ReplaceAllNewUpdates = TriState.False Then
              Return New AddResult(True)
            Else
              Dim sbRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage, PList.ToArray, always, Comparison.Older)
              If sbRet = TaskDialogResult.Yes Then
                If always Then ReplaceAllNewUpdates = TriState.True
                OnlyDown = True
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then ReplaceAllNewUpdates = TriState.False
                Return New AddResult(True)
              Else
                Return New AddResult(False)
              End If
            End If
          Else
            Dim sRet As TaskDialogResult = IntegratedUpdateSelectionBox(Me, msuData, InImage, PList.ToArray, False, Comparison.Mixed)
            If sRet = TaskDialogResult.Ok Then
            ElseIf sRet = TaskDialogResult.Yes Then
              OnlyUp = True
            ElseIf sRet = TaskDialogResult.No Then
              OnlyDown = True
            ElseIf sRet = TaskDialogResult.Close Then
              Return New AddResult(True)
            ElseIf sRet = TaskDialogResult.Cancel Then
              Return New AddResult(False)
            End If
          End If
        End If
      End If
      For Each item As ListViewItem In lvMSU.Items
        Dim itemData As Update_File = item.Tag(0)
        If itemData.Identity = msuData.Identity Then
          Return New AddResult(False, "Update already added.")
        ElseIf Replace(itemData.Ident.Name.ToLower, "-refresh-", "-") = Replace(msuData.Ident.Name.ToLower, "-refresh-", "-") And itemData.Ident.Language.ToLower = msuData.Ident.Language.ToLower And itemData.Architecture.ToLower = msuData.Architecture.ToLower Then
          Dim always As Boolean = False
          Dim CompareRet As Integer = CompareMSVersions(itemData.Ident.Version, msuData.Ident.Version)
          If CompareRet = 0 Then
            Return New AddResult(False, "Update already added.")
          ElseIf CompareRet > 0 Then
            If ReplaceAllOldUpdates = TriState.True Then
              item.Remove()
            ElseIf ReplaceAllOldUpdates = TriState.False Then
              Return New AddResult(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then ReplaceAllOldUpdates = TriState.True
                item.Remove()
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then ReplaceAllOldUpdates = TriState.False
                Return New AddResult(True)
              Else
                Return New AddResult(False)
              End If
            End If
          ElseIf CompareRet < 0 Then
            If ReplaceAllNewUpdates = TriState.True Then
              item.Remove()
            ElseIf ReplaceAllNewUpdates = TriState.False Then
              Return New AddResult(True)
            Else
              Dim sbRet As TaskDialogResult = UpdateSelectionBox(Me, msuData, itemData, always)
              If sbRet = TaskDialogResult.Yes Then
                If always Then ReplaceAllNewUpdates = TriState.True
                item.Remove()
              ElseIf sbRet = TaskDialogResult.No Then
                If always Then ReplaceAllNewUpdates = TriState.False
                Return New AddResult(True)
              Else
                Return New AddResult(False)
              End If
            End If
          End If
        End If
      Next
      If msuData.KBArticle = "2533552" Then Return New AddResult(False, "Update can't be integrated.")
      If msuData.KBArticle = "947821" Then Return New AddResult(False, "Update can't be integrated.")
      If msuData.KBArticle = "3035583" Then Return New AddResult(False, "Update can't be integrated.")
      Dim lvItem As New ListViewItem(msuData.Name)
      Dim myTag(1) As Object
      myTag(0) = msuData
      lvItem.Tag = myTag
      Dim bWhitelist As Boolean = msuData.Architecture = "x86" AndAlso CheckWhitelist(msuData.DisplayName)
      Dim en As String = ChrW(&H2003)
      Dim ttItem As String = IIf(String.IsNullOrEmpty(msuData.KBArticle), msuData.Name, "KB" & msuData.KBArticle)
      ttItem &= vbNewLine & en & msuData.AppliesTo & " " & msuData.Architecture & IIf(bWhitelist, " [Whitelisted for 64-bit]", "")
      If Not String.IsNullOrEmpty(msuData.BuildDate) Then ttItem &= vbNewLine & en & "Built: " & msuData.BuildDate
      ttItem &= vbNewLine & en & ShortenPath(msuData.Path)
      lvItem.BackColor = IIf(bWhitelist, SystemColors.GradientInactiveCaption, lvMSU.BackColor)
      If msuData.KBArticle = "2647753" And msuData.Ident.Version = "6.1.2.0" Then
        lvItem.ForeColor = Color.Red
        ttItem &= vbNewLine & en & "Version 2 may not integrate correctly. SLIPS7REAM suggests using Version 4 from the Microsoft Update Catalog."
      End If
      If PrerequisiteList.Length > 0 Then
        For Each prereq As Prerequisite In PrerequisiteList
          If msuData.KBArticle = prereq.KBWithRequirement And prereq.Requirement.Length > 0 Then
            Dim Req(prereq.Requirement.Length - 1) As Boolean
            For I As Integer = 0 To Req.Length - 1
              Req(I) = False
            Next
            If lvImages.Items.Count > 0 Then
              For Each searchImage As ListViewItem In lvImages.Items
                Dim searchItem As ImagePackage = searchImage.Tag(1)
                If searchItem.IntegratedUpdateList IsNot Nothing AndAlso searchItem.IntegratedUpdateList.Count > 0 Then
                  For Each searchPackage As Update_Integrated In searchItem.IntegratedUpdateList
                    For I As Integer = 0 To prereq.Requirement.Length - 1
                      If searchPackage.Ident.Name.Contains(prereq.Requirement(I)) Then Req(I) = True
                    Next
                  Next
                End If
              Next
            End If
            If lvMSU.Items.Count > 0 Then
              For Each searchItem As ListViewItem In lvMSU.Items
                Dim searchData As Update_File = searchItem.Tag(0)
                For I As Integer = 0 To prereq.Requirement.Length - 1
                  If searchData.KBArticle = prereq.Requirement(I) Then Req(I) = True
                Next
                If Not Req.Contains(False) Then Exit For
              Next
            End If
            If Req.Contains(False) Then
              lvItem.ForeColor = Color.Orange
              If prereq.Requirement.Length = 1 Then
                ttItem &= vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " is also integrated."
              ElseIf prereq.Requirement.Length = 2 Then
                ttItem &= vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " and KB" & prereq.Requirement(1) & " are also integrated."
              Else
                Dim sKBList As String = Nothing
                For I As Integer = 0 To prereq.Requirement.Length - 2
                  sKBList &= "KB" & prereq.Requirement(I) & ", "
                Next
                sKBList &= "and KB" & prereq.Requirement(prereq.Requirement.Length - 1)
                ttItem &= vbNewLine & en & "Please make sure " & sKBList & " are also integrated."
              End If
            End If
          End If
          For Each sReq As String In prereq.Requirement
            If msuData.KBArticle = sReq Then
              Dim Req As Boolean = False
              If prereq.Requirement.Length < 2 Then
                Req = True
              Else
                Dim Reqs(prereq.Requirement.Length - 1) As Boolean
                For I As Integer = 0 To Reqs.Length - 1
                  Reqs(I) = False
                Next
                For I As Integer = 0 To prereq.Requirement.Length - 1
                  If prereq.Requirement(I) = sReq Then
                    Reqs(I) = True
                    Continue For
                  End If
                  For Each searchItem As ListViewItem In lvMSU.Items
                    Dim searchData As Update_File = searchItem.Tag(0)
                    If searchData.KBArticle = prereq.Requirement(I) Then
                      Reqs(I) = True
                      Exit For
                    End If
                  Next
                Next
                Req = Not Reqs.Contains(False)
              End If
              If Req Then
                For Each searchItem As ListViewItem In lvMSU.Items
                  Dim searchData As Update_File = searchItem.Tag(0)
                  If searchData.KBArticle = prereq.KBWithRequirement Then
                    searchItem.ForeColor = lvMSU.ForeColor
                    If searchItem.ToolTipText.Contains("Please make sure") Then searchItem.ToolTipText = searchItem.ToolTipText.Substring(0, searchItem.ToolTipText.LastIndexOf(vbNewLine))
                    Exit For
                  End If
                Next
              End If
            End If
          Next
        Next
      End If
      If msuData.Ident.Name = "Microsoft-Windows-LIP-LanguagePack-Package" Then
        Dim langList As New List(Of String)
        Dim hasSP As Boolean = False
        If lvImages.Items.Count > 0 Then
          For Each lvImage As ListViewItem In lvImages.Items
            Dim imageInfo As ImagePackage = lvImage.Tag(1)
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
            Dim updateInfo As Update_File = lvUpdate.Tag(0)
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
        If langList.Count > 0 Then
          Select Case msuData.Ident.Language.ToLower
            Case "af-za", "sq-al", "am-et", "hy-am", "as-in", "bn-bd", "bn-in", "prs-af",
                 "fil-ph", "ka-ge", "gu-in", "ha-latn-ng", "hi-in", "is-is", "ig-ng", "id-id",
                 "iu-latn-ca", "ga-ie", "kn-in", "km-kh", "sw-ke", "kok-in", "mk-mk", "ms-my",
                 "ms-bn", "ml-in", "mt-mt", "mi-nz", "mr-in", "ne-np", "or-in", "fa-ir",
                 "pa-in", "nso-za", "tn-za", "si-lk", "ta-in", "te-in", "ur-pk",
                 "vi-vn", "cy-gb", "xh-za", "yo-ng", "zu-za"
              If Not langList.Contains("en") Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure the English Language Pack is also integrated."
              End If
            Case "gl-es", "quz-pe"
              If Not langList.Contains("es") Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure the Spanish Language Pack is also integrated."
              End If
            Case "nn-no"
              If Not langList.Contains("nb") Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure the Norwegian (Bokmål) Language Pack is also integrated."
              End If
            Case "ky-kg", "tt-ru"
              If Not langList.Contains("ru") Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure the Russian Language Pack is also integrated."
              End If
            Case "eu-es", "ca-es"
              If Not (langList.Contains("es") Or langList.Contains("fr")) Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure either the French or the Spanish Language Pack is also integrated."
              End If
            Case "bs-cyrl-ba", "bs-latn-ba"
              If Not (langList.Contains("en") Or langList.Contains("hr") Or langList.Contains("sr-latn")) Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure either the English, the Croatian, or the Serbian (Latin) Language Pack is also integrated."
              End If
            Case "sr-cyrl-cs"
              If Not (langList.Contains("en") Or langList.Contains("sr-latn")) Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure either the English or the Serbian (Latin) Language Pack is also integrated."
              End If
            Case "lb-lu"
              If Not (langList.Contains("en") Or langList.Contains("fr")) Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure either the English or the French Language Pack is also integrated."
              End If
            Case "az-latn-az", "kk-kz", "mn-mn", "tk-tm", "uz-latn-uz"
              If Not (langList.Contains("en") Or langList.Contains("ru")) Then
                lvItem.ForeColor = Color.Orange
                ttItem &= vbNewLine & en & "Please make sure either the English or the Russian Language Pack is also integrated."
              End If
          End Select
        End If
        If chkSP.Checked Or hasSP Then
          If msuData.Ident.Version = "6.1.7600.16385" Then
            Select Case msuData.Ident.Language.ToLower
              Case "ca-es", "cy-gb", "hi-in", "is-is", "sr-cyrl-cs"
                lvItem.ForeColor = Color.Red
                ttItem &= vbNewLine & en & "This Language Interface Pack has been superseded by Service Pack 1 and may not integrate correctly."
            End Select
          End If
        End If
      End If
      If InOne Then
        If lvItem.ForeColor = lvMSU.ForeColor Then lvItem.ForeColor = Color.Orange
        If OnlyUp Then
          ttItem &= vbNewLine & en & "This update will upgrade only integrated older versions."
        ElseIf OnlyDown Then
          ttItem &= vbNewLine & en & "This update will downgrade only integrated newer versions."
          myTag(1) = InImage
          lvItem.Tag = myTag
        Else
          ttItem &= vbNewLine & en & "This update will replace both newer and older integrated versions."
          myTag(1) = InImage
          lvItem.Tag = myTag
        End If
      End If
      lvItem.ToolTipText = ttItem
      Select Case GetUpdateType(msuData.Path)
        Case UpdateType.MSU
          lvItem.ImageKey = "MSU"
          lvItem.SubItems.Add(msuData.Architecture & " MSU")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
        Case UpdateType.CAB
          lvItem.ImageKey = "CAB"
          lvItem.SubItems.Add(msuData.Architecture & " CAB")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
        Case UpdateType.LP
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(msuData.Architecture & " MUI")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
        Case UpdateType.LIP, UpdateType.MSI
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(msuData.Architecture & " LIP")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
        Case UpdateType.EXE
          If msuData.Name.Contains("Windows Update Agent") Then
            lvItem.ImageKey = "CAB"
            lvItem.SubItems.Add(msuData.Architecture & " CAB")
            lvMSU.Items.Add(lvItem)
          ElseIf msuData.DisplayName.Contains("Internet Explorer") Then
            lvItem.ImageKey = "CAB"
            lvItem.SubItems.Add(msuData.Architecture & " CAB")
            lvMSU.Items.Add(lvItem)
          Else
            lvItem.ImageKey = "MLC"
            lvItem.SubItems.Add(msuData.Architecture & " MUI")
            lvMSU.Items.Add(lvItem)
          End If
          Return New AddResult(True)
      End Select
    End If
    If String.IsNullOrEmpty(IO.Path.GetExtension(msuData.Path)) Then
      Return New AddResult(False, "Unknown Update Type.")
    Else
      Return New AddResult(False, "Unknown Update Type: """ & IO.Path.GetExtension(msuData.Path).Substring(1).ToUpper & """.")
    End If
  End Function
  Private Function CleanupFailures(FailCollection As List(Of String)) As String
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
          sFailList &= failGroup.Item(0) & " (" & (failGroup.Count - 1).ToString & " updates)" & vbNewLine
        Else
          sFailList &= failGroup.Item(1) & vbNewLine
        End If
      Next
      If Not String.IsNullOrEmpty(sFailList) AndAlso sFailList.Length > 1 AndAlso sFailList.EndsWith(vbNewLine) Then sFailList = sFailList.Substring(0, sFailList.Length - 2)
      Return sFailList
    Else
      Return Join(FailCollection.ToArray, vbNewLine)
    End If
  End Function
  Private Sub SetRequirements()
    Dim en As String = ChrW(&H2003)
    If PrerequisiteList.Length > 0 Then
      For U As Integer = 0 To lvMSU.Items.Count - 1
        Dim lvItem As ListViewItem = lvMSU.Items(U)
        Dim msuData As Update_File = lvItem.Tag(0)
        If msuData.Name = "DRIVER" Then Continue For
        For Each prereq As Prerequisite In PrerequisiteList
          If msuData.KBArticle = prereq.KBWithRequirement And prereq.Requirement.Length > 0 Then
            Dim Req(prereq.Requirement.Length - 1) As Boolean
            For I As Integer = 0 To Req.Length - 1
              Req(I) = False
            Next
            If lvImages.Items.Count > 0 Then
              For Each searchImage As ListViewItem In lvImages.Items
                Dim searchItem As ImagePackage = searchImage.Tag(1)
                If searchItem.IntegratedUpdateList IsNot Nothing AndAlso searchItem.IntegratedUpdateList.Count > 0 Then
                  For Each searchPackage As Update_Integrated In searchItem.IntegratedUpdateList
                    For I As Integer = 0 To prereq.Requirement.Length - 1
                      If searchPackage.Ident.Name.Contains(prereq.Requirement(I)) Then Req(I) = True
                      If Not String.IsNullOrEmpty(searchPackage.UpdateInfo.Name) AndAlso searchPackage.UpdateInfo.Name.Contains(prereq.Requirement(I)) Then Req(I) = True
                    Next
                  Next
                End If
              Next
            End If
            If lvMSU.Items.Count > 0 Then
              For Each searchItem As ListViewItem In lvMSU.Items
                Dim searchData As Update_File = searchItem.Tag(0)
                For I As Integer = 0 To prereq.Requirement.Length - 1
                  If searchData.KBArticle = prereq.Requirement(I) Then Req(I) = True
                Next
                If Not Req.Contains(False) Then Exit For
              Next
            End If
            If Req.Contains(False) Then
              lvItem.ForeColor = Color.Orange
              If prereq.Requirement.Length = 1 Then
                If Not lvItem.ToolTipText.Contains(vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " is also integrated.") Then lvItem.ToolTipText &= vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " is also integrated."
              ElseIf prereq.Requirement.Length = 2 Then
                If Not lvItem.ToolTipText.Contains(vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " and KB" & prereq.Requirement(1) & " are also integrated.") Then lvItem.ToolTipText &= vbNewLine & en & "Please make sure KB" & prereq.Requirement(0) & " and KB" & prereq.Requirement(1) & " are also integrated."
              Else
                Dim sKBList As String = Nothing
                For I As Integer = 0 To prereq.Requirement.Length - 2
                  sKBList &= "KB" & prereq.Requirement(I) & ", "
                Next
                sKBList &= "and KB" & prereq.Requirement(prereq.Requirement.Length - 1)
                If Not lvItem.ToolTipText.Contains(vbNewLine & en & "Please make sure " & sKBList & " are also integrated.") Then lvItem.ToolTipText &= vbNewLine & en & "Please make sure " & sKBList & " are also integrated."
              End If
            ElseIf lvItem.ForeColor = Color.Orange And Not msuData.Ident.Name = "Microsoft-Windows-LIP-LanguagePack-Package" Then
              lvItem.ForeColor = lvMSU.ForeColor
              If lvItem.ToolTipText.Contains(vbNewLine & en & "Please make sure ") Then
                Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure "))
                Dim SecondHalf As String = Nothing
                If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure ") + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure ") + 2))
                lvItem.ToolTipText = FirstHalf & SecondHalf
              End If
            End If
          End If
          For Each sReq As String In prereq.Requirement
            If msuData.KBArticle = sReq Then
              Dim Req As Boolean = False
              If prereq.Requirement.Length < 2 Then
                Req = True
              Else
                Dim Reqs(prereq.Requirement.Length - 1) As Boolean
                For I As Integer = 0 To Reqs.Length - 1
                  Reqs(I) = False
                Next
                For I As Integer = 0 To prereq.Requirement.Length - 1
                  If prereq.Requirement(I) = sReq Then
                    Reqs(I) = True
                    Continue For
                  End If
                  For Each searchItem As ListViewItem In lvMSU.Items
                    Dim searchData As Update_File = searchItem.Tag(0)
                    If searchData.KBArticle = prereq.Requirement(I) Then
                      Reqs(I) = True
                      Exit For
                    End If
                  Next
                Next
                Req = Not Reqs.Contains(False)
              End If
              If Req Then
                For Each searchItem As ListViewItem In lvMSU.Items
                  Dim searchData As Update_File = searchItem.Tag(0)
                  If searchData.KBArticle = prereq.KBWithRequirement Then
                    searchItem.ForeColor = lvMSU.ForeColor
                    If searchItem.ToolTipText.Contains("Please make sure") Then searchItem.ToolTipText = searchItem.ToolTipText.Substring(0, searchItem.ToolTipText.LastIndexOf(vbNewLine))
                    Exit For
                  End If
                Next
              ElseIf lvItem.ForeColor = Color.Orange And Not msuData.Ident.Name = "Microsoft-Windows-LIP-LanguagePack-Package" Then
                lvItem.ForeColor = lvMSU.ForeColor
                If lvItem.ToolTipText.Contains(vbNewLine & en & "Please make sure ") Then
                  Dim FirstHalf As String = lvItem.ToolTipText.Substring(0, lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure "))
                  Dim SecondHalf As String = Nothing
                  If lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure ") + 2).Contains(vbNewLine) Then SecondHalf = lvItem.ToolTipText.Substring(lvItem.ToolTipText.IndexOf(vbNewLine, lvItem.ToolTipText.IndexOf(vbNewLine & en & "Please make sure ") + 2))
                  lvItem.ToolTipText = FirstHalf & SecondHalf
                End If
              End If
            End If
          Next
        Next
      Next
    End If
  End Sub
  Private Sub cmdRemMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdRemMSU.Click
    If lvMSU.Items.Count > 0 Then
      Dim lIndex As Integer = -1
      For Each lvItem As ListViewItem In lvMSU.SelectedItems
        lIndex = lvItem.Index
        lvItem.Remove()
      Next
      If lIndex >= 0 And lvMSU.Items.Count > 0 Then
        If lIndex >= lvMSU.Items.Count Then lIndex = lvMSU.Items.Count - 1
        lvMSU.Items(lIndex).Selected = True
      End If
      SetRequirements()
      SetStatus("Idle")
      RedoColumns()
      cmdRemMSU.Focus()
    Else
      Beep()
    End If
  End Sub
  Private Sub cmdClearMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdClearMSU.Click
    If lvMSU.Items.Count > 0 Then
      If MsgDlg(Me, IIf(lvMSU.Items.Count > 2, "All " & lvMSU.Items.Count & " updates", "All updates") & " will be removed from the list.", "Do you want to clear the Update List?", "Remove All Updates", MessageBoxButtons.YesNo, TaskDialogIcon.Delete, MessageBoxDefaultButton.Button2, , "Remove All Updates") = Windows.Forms.DialogResult.Yes Then
        lvMSU.Items.Clear()
        SetStatus("Idle")
        RedoColumns()
      End If
    Else
      Beep()
    End If
  End Sub
#End Region
#Region "Context Menu"
  Private Sub mnuUpdateTop_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateTop.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    Dim targetI As Integer = 0
    For Each item As ListViewItem In msuItems
      Dim tmpItem = item.Clone
      item.Remove()
      lvMSU.Items.Insert(targetI, tmpItem)
      lvMSU.Items(targetI).Selected = True
      targetI += 1
    Next
  End Sub
  Private Sub mnuUpdateUp_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateUp.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    Dim targetI As Integer = Integer.MaxValue
    For Each item As ListViewItem In msuItems
      If item.Index < targetI Then targetI = item.Index
    Next
    If targetI = Integer.MaxValue Or targetI = 0 Then Return
    targetI -= 1
    For Each item As ListViewItem In msuItems
      Dim tmpItem = item.Clone
      item.Remove()
      lvMSU.Items.Insert(targetI, tmpItem)
      lvMSU.Items(targetI).Selected = True
      targetI += 1
    Next
  End Sub
  Private Sub mnuUpdateDown_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateDown.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    Dim targetI As Integer = Integer.MaxValue
    For Each item As ListViewItem In msuItems
      If item.Index < targetI Then targetI = item.Index
    Next
    If targetI = Integer.MaxValue Or targetI >= lvMSU.Items.Count - msuItems.Count Then Return
    targetI += 1
    For Each item As ListViewItem In msuItems
      Dim tmpItem = item.Clone
      item.Remove()
      If lvMSU.Items.Count <= targetI Then
        targetI = CType(lvMSU.Items.Add(tmpItem), ListViewItem).Index
      Else
        lvMSU.Items.Insert(targetI, tmpItem)
      End If
      lvMSU.Items(targetI).Selected = True
      targetI += 1
    Next
  End Sub
  Private Sub mnuUpdateBottom_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateBottom.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    For Each item As ListViewItem In msuItems
      Dim tmpItem As ListViewItem = item.Clone
      item.Remove()
      lvMSU.Items.Add(tmpItem)
      tmpItem.Selected = True
    Next
  End Sub
  Private Sub mnuUpdateRemove_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateRemove.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    Dim lIndex As Integer = -1
    For Each lvItem As ListViewItem In msuItems
      lIndex = lvItem.Index
      lvItem.Remove()
    Next
    If lIndex >= 0 And lvMSU.Items.Count > 0 Then
      If lIndex >= lvMSU.Items.Count Then lIndex = lvMSU.Items.Count - 1
      lvMSU.Items(lIndex).Selected = True
    End If
    SetStatus("Idle")
    RedoColumns()
  End Sub
  Private Sub mnuUpdateLocation_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateLocation.Click
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    Dim itemDirs As New SortedList(Of String, List(Of String))()
    For Each lvItem As ListViewItem In msuItems
      Dim itemDir As String = IO.Path.GetDirectoryName(CType(lvItem.Tag(0), Update_File).Path)
      If itemDirs.Count > 0 Then
        If Not itemDirs.ContainsKey(itemDir) Then
          itemDirs.Add(itemDir, New List(Of String))
        End If
      Else
        itemDirs.Add(itemDir, New List(Of String))
      End If
      itemDirs(itemDir).Add(CType(lvItem.Tag(0), Update_File).Path)
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
    Dim msuItems As ListView.SelectedListViewItemCollection = CType(mnuMSU.Tag, ListView.SelectedListViewItemCollection)
    For Each lvItem As ListViewItem In msuItems
      Dim msuData As Update_File = lvItem.Tag(0)
      If Not String.IsNullOrEmpty(msuData.Failure) Then If ExtractFailureAlert(msuData.Failure) Then Continue For
      If Not String.IsNullOrEmpty(msuData.Name) Then
        If msuData.Name = "DRIVER" Then
          Dim props As New frmDriverProps(msuData.DriverData)
          props.Show(Me)
        Else
          For Each Form In OwnedForms
            If Form.Tag = msuData.Name Then
              Form.Focus()
              Return
            End If
          Next
          Dim props As New frmUpdateProps
          props.Tag = msuData.Name
          props.Text = GetUpdateName(msuData.Ident) & " Properties"
          props.txtName.Text = msuData.Name
          props.txtDisplayName.Text = msuData.DisplayName
          props.txtIdentity.Text = msuData.Identity
          props.txtAppliesTo.Text = msuData.AppliesTo
          props.txtArchitecture.Text = msuData.Architecture
          props.txtVersion.Text = msuData.KBVersion
          props.txtBuildDate.Text = msuData.BuildDate
          If String.IsNullOrEmpty(msuData.SupportLink) Then
            props.lblKBLink.Link = False
            props.lblKBLink.Tag = Nothing
            If String.IsNullOrEmpty(msuData.KBArticle) Then
              props.lblKBLink.Text = "Details"
              props.lblKBLink.Visible = False
            Else
              props.lblKBLink.Text = "Article KB" & msuData.KBArticle
              props.lblKBLink.Visible = True
            End If
          Else
            props.lblKBLink.Link = True
            props.lblKBLink.Tag = msuData.SupportLink
            If String.IsNullOrEmpty(msuData.KBArticle) Then
              props.lblKBLink.Text = "Details"
              props.lblKBLink.Visible = True
            Else
              props.lblKBLink.Text = "Article KB" & msuData.KBArticle
              props.lblKBLink.Visible = True
            End If
          End If
          props.ttLink.SetToolTip(props.lblKBLink, msuData.SupportLink)
          props.Show(Me)
        End If
      End If
    Next
  End Sub
#End Region
#End Region
#Region "ISO"
  Private Sub chkISO_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkISO.CheckedChanged
    RunComplete = False
    cmdBegin.Text = "&Begin"
    cmdOpenFolder.Visible = False
    txtISO.Enabled = chkISO.Checked
    cmdISO.Enabled = chkISO.Checked
    chkUEFI.Enabled = chkISO.Checked
    lblISOFeatures.Enabled = chkISO.Checked
    chkUnlock.Enabled = chkISO.Checked And (chkUnlock.Tag Is Nothing)
    lblISOLabel.Enabled = chkISO.Checked
    chkAutoLabel.Enabled = chkISO.Checked
    txtISOLabel.Enabled = chkISO.Checked
    lblISOFS.Enabled = chkISO.Checked
    cmbISOFormat.Enabled = chkISO.Checked
    If chkISO.Checked Then
      If Not cmbLimitType.Items.Contains("Split ISO") Then
        cmbLimitType.Items.Add("Split ISO")
        If cmbLimitType.SelectedIndex = 1 Then cmbLimitType.SelectedIndex = 2
      End If
    Else
      If cmbLimitType.Items.Contains("Split ISO") Then
        If cmbLimitType.SelectedIndex = 2 Then cmbLimitType.SelectedIndex = 1
        cmbLimitType.Items.Remove("Split ISO")
      End If
    End If
  End Sub
  Private Sub txtISO_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragDrop
    TextBoxDragDropEvent(sender, e)
  End Sub
  Private Sub txtISO_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragEnter
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtISO_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISO.DragOver
    TextBoxDragOverEvent(sender, e, {".iso"})
  End Sub
  Private Sub txtISO_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISO.TextChanged
    RunComplete = False
    cmdBegin.Text = "&Begin"
    cmdOpenFolder.Visible = False
    Dim foundEI As Boolean = False
    Dim foundCLG As Boolean = False
    If Not String.IsNullOrEmpty(txtISO.Text) AndAlso IO.File.Exists(txtISO.Text) Then
      WriteToOutput("Extracting File List from """ & txtISO.Text & """...")
      Dim sFiles() As String = ExtractFilesList(txtISO.Text)
      If sFiles IsNot Nothing Then
        For Each sFile In sFiles
          If sFile.ToLower.Contains("ei.cfg") Then foundEI = True
          If sFile.ToLower.Contains(".clg") Then foundCLG = True
          If foundEI And foundCLG Then Exit For
        Next
      End If
      If foundEI Or foundCLG Then
        If chkUnlock.Tag IsNot Nothing Then
          If chkUnlock.Tag = "Y" Then
            chkUnlock.Checked = True
          ElseIf chkUnlock.Tag = "N" Then
            chkUnlock.Checked = False
          End If
          chkUnlock.Tag = Nothing
        End If
        chkUnlock.Enabled = True
      Else
        If chkUnlock.Tag Is Nothing Then
          If chkUnlock.Checked Then
            chkUnlock.Tag = "Y"
          Else
            chkUnlock.Tag = "N"
          End If
        End If
        chkUnlock.Checked = True
        chkUnlock.Enabled = False
      End If
      If chkAutoLabel.Checked Then
        WriteToOutput("Extracting Comment from """ & txtISO.Text & """...")
        Dim sComment As String = ExtractComment(txtISO.Text)
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
      SetStatus("Idle")
    End If
  End Sub
  Private Sub chkAutoLabel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutoLabel.CheckedChanged
    If chkAutoLabel.Checked And (Not String.IsNullOrEmpty(txtISO.Text) AndAlso IO.File.Exists(txtISO.Text)) Then
      WriteToOutput("Extracting Comment from """ & txtISO.Text & """...")
      Dim sComment As String = ExtractComment(txtISO.Text)
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
      SetStatus("Idle")
    End If
    mySettings.AutoISOLabel = chkAutoLabel.Checked
  End Sub
  Private Sub txtISOLabel_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragDrop
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If Data.Length = 1 Then
        Dim sPath As String = Data(0)
        WriteToOutput("Extracting Comment from """ & sPath & """...")
        Dim sComment As String = ExtractComment(sPath)
        If Not String.IsNullOrEmpty(sComment) Then txtISOLabel.Text = sComment
      End If
    End If
  End Sub
  Private Sub txtISOLabel_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragEnter
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtISOLabel_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtISOLabel.DragOver
    TextBoxDragOverEvent(sender, e, {".iso"})
  End Sub
  Private Sub txtISOLabel_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISOLabel.TextChanged
    If Me.Tag Is Nothing Then
      If Not String.IsNullOrEmpty(txtISOLabel.Text) Then
        mySettings.DefaultISOLabel = txtISOLabel.Text
      End If
    End If
  End Sub
#Region "ISO Label Context Menu"
#Region "x86"
#Region "Starter"
  Private Sub mnuLabelGRMCSTFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Starter, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Starter, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCSTVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Starter, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Starter, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Home Basic"
  Private Sub mnuLabelGRMCHBFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHBVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGRMCHPFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHPVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGRMCPRFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCPRVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGRMCULFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCULVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGRMCENVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Enterprise, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCENCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Enterprise, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGRMCMUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCALFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x86, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x86, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCALVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x86, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x86, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#End Region
#Region "x64"
#Region "Home Basic"
  Private Sub mnuLabelGRMCHBXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHBXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHBXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHBXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomeBasic, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Home Premium"
  Private Sub mnuLabelGRMCHPXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCHPXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCHPXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCHPXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.HomePremium, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Professional"
  Private Sub mnuLabelGRMCPRXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCPRXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCPRXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCPRXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Professional, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Ultimate"
  Private Sub mnuLabelGRMCULXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCULXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCULXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCULXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Ultimate, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Enterprise"
  Private Sub mnuLabelGRMCENXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Enterprise, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCENXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCENXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Enterprise, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "Multiple"
  Private Sub mnuLabelGRMCMUXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCALXFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x64, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALXFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x64, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCALXVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x64, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCALXCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCALXCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.x64, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#End Region
#Region "AIO"
#Region "Multiple"
  Private Sub mnuLabelGRMCMUUFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.Universal, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUUFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.Universal, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCMUUVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.Universal, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCMUUCHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCMUUCHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.Multiple, Architecture.Universal, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#Region "All"
  Private Sub mnuLabelGRMCSTAFRER_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTAFRER.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.Universal, BuildType.Release, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTAFREO_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTAFREO.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.Universal, BuildType.Release, PurposeType.OEM)
  End Sub
  Private Sub mnuLabelGRMCSTAVOL_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTAVOL.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.Universal, BuildType.Volume, PurposeType.Retail)
  End Sub
  Private Sub mnuLabelGRMCSTACHE_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelGRMCSTACHE.Click
    txtISOLabel.Text = MakeISOName(ReleaseType.All, Architecture.Universal, BuildType.Debug, PurposeType.Retail)
  End Sub
#End Region
#End Region
  Private Sub mnuLabelAuto_Click(sender As System.Object, e As System.EventArgs) Handles mnuLabelAuto.Click
    Dim rType As ReleaseType = ReleaseType.All
    Dim rArc As Architecture = Architecture.Universal
    Dim rBui As BuildType = BuildType.Release
    Dim rPur As PurposeType = PurposeType.Retail
    Dim has86, has64 As Boolean : has86 = False : has64 = False
    Dim stCount, hbCount, hpCount, prCount, ulCount, enCount As Integer
    If lvImages.Items.Count > 0 Then
      For Each Image As ListViewItem In lvImages.Items
        If Image.Checked Then
          Dim imInfo As ImagePackage = Image.Tag(1)
          Select Case imInfo.Edition
            Case "Starter" : stCount += 1
            Case "HomeBasic" : hbCount += 1
            Case "HomePremium" : hpCount += 1
            Case "Professional" : prCount += 1
            Case "Ultimate" : ulCount += 1
            Case "Enterprise" : enCount += 1
          End Select
          If imInfo.Architecture = "x86" Then
            has86 = True
          ElseIf imInfo.Architecture = "x64" Then
            has64 = True
          End If
        End If
      Next
    End If
    If has86 And has64 Then
      rArc = Architecture.Universal
      If stCount >= 1 And hbCount >= 2 And hpCount >= 2 And prCount >= 2 And ulCount >= 2 Then
        rType = ReleaseType.All
      ElseIf (IIf(stCount > 0, 1, 0) + IIf(hbCount > 0, 1, 0) + IIf(hpCount > 0, 1, 0) + IIf(prCount > 0, 1, 0) + IIf(ulCount > 0, 1, 0) + IIf(enCount > 0, 1, 0)) > 1 Then
        rType = ReleaseType.Multiple
      ElseIf stCount > 0 Then
        rType = ReleaseType.Starter
      ElseIf hbCount > 0 Then
        rType = ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ReleaseType.Enterprise
        rBui = BuildType.Volume
      End If
    ElseIf has64 Then
      rArc = Architecture.x64
      If hbCount >= 1 And hpCount >= 1 And prCount >= 1 And ulCount >= 1 Then
        rType = ReleaseType.All
      ElseIf (IIf(hbCount > 0, 1, 0) + IIf(hpCount > 0, 1, 0) + IIf(prCount > 0, 1, 0) + IIf(ulCount > 0, 1, 0) + IIf(enCount > 0, 1, 0)) > 1 Then
        rType = ReleaseType.Multiple
      ElseIf hbCount > 0 Then
        rType = ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ReleaseType.Enterprise
        rBui = BuildType.Volume
      End If
    ElseIf has86 Then
      rArc = Architecture.x86
      If stCount >= 1 And hbCount >= 1 And hpCount >= 1 And prCount >= 1 And ulCount >= 1 Then
        rType = ReleaseType.All
      ElseIf (IIf(stCount > 0, 1, 0) + IIf(hbCount > 0, 1, 0) + IIf(hpCount > 0, 1, 0) + IIf(prCount > 0, 1, 0) + IIf(ulCount > 0, 1, 0) + IIf(enCount > 0, 1, 0)) > 1 Then
        rType = ReleaseType.Multiple
      ElseIf stCount > 0 Then
        rType = ReleaseType.Starter
      ElseIf hbCount > 0 Then
        rType = ReleaseType.HomeBasic
      ElseIf hpCount > 0 Then
        rType = ReleaseType.HomePremium
      ElseIf prCount > 0 Then
        rType = ReleaseType.Professional
      ElseIf ulCount > 0 Then
        rType = ReleaseType.Ultimate
      ElseIf enCount > 0 Then
        rType = ReleaseType.Enterprise
        rBui = BuildType.Volume
      End If
    End If
    txtISOLabel.Text = MakeISOName(rType, rArc, rBui, rPur)
  End Sub
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
    If Me.Tag Is Nothing Then
      mySettings.DefaultFS = cmbISOFormat.Text
    End If
  End Sub
  Private Sub LoadLabelVals(ByRef langVal As String, ByRef medVal As String)
    Dim langList As New List(Of String)
    If lvImages.Items.Count > 0 Then
      For Each item As ListViewItem In lvImages.Items
        Dim imageInfo As ImagePackage = item.Tag(1)
        For Each language As String In imageInfo.LangList
          Dim sLang As String = language.Substring(0, language.IndexOf("-")).ToUpper
          If Not langList.Contains(sLang) Then langList.Add(sLang)
        Next
      Next
    End If
    If lvMSU.Items.Count > 0 Then
      For Each item As ListViewItem In lvMSU.Items
        Dim updateInfo As Update_File = item.Tag(0)
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
    If Not String.IsNullOrEmpty(langVal) Then langVal = "_" & langVal
    If Not String.IsNullOrEmpty(medVal) Then medVal = "_" & medVal
  End Sub
  Private Function MakeISOName(Release As ReleaseType, Arch As Architecture, Build As BuildType, Purpose As PurposeType) As String
    Dim rel As String = "UL"
    Dim arc As String = ""
    Dim bui As String = "FRE"
    Dim pur As String = "R"
    Dim langVal As String = "EN"
    Dim medVal As String = "DVD"
    LoadLabelVals(langVal, medVal)
    Select Case Release
      Case ReleaseType.Starter : rel = "ST"
      Case ReleaseType.HomeBasic : rel = "HB"
      Case ReleaseType.HomePremium : rel = "HP"
      Case ReleaseType.Professional : rel = "PR"
      Case ReleaseType.Ultimate : rel = "UL"
      Case ReleaseType.Enterprise : rel = "EN"
      Case ReleaseType.Multiple : rel = "MU"
      Case ReleaseType.All : rel = "AL"
    End Select
    Select Case Arch
      Case Architecture.x86 : arc = ""
      Case Architecture.x64 : arc = "X"
      Case Architecture.Universal : arc = "U"
    End Select
    Select Case Build
      Case BuildType.Release : bui = "FRE"
      Case BuildType.Debug : bui = "CHE"
      Case BuildType.Volume : bui = "VOL"
    End Select
    If Build = BuildType.Debug Then
      pur = ""
    Else
      If Build = BuildType.Volume Then
        pur = ""
      Else
        Select Case Purpose
          Case PurposeType.Retail : pur = "R"
          Case PurposeType.OEM : pur = "O"
        End Select
      End If
    End If
    Return "GRMC" & rel & arc & bui & pur & langVal & medVal
  End Function
#End Region
#Region "Merge"
  Private Sub chkMerge_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMerge.CheckedChanged
    txtMerge.Enabled = chkMerge.Checked
    cmdMerge.Enabled = chkMerge.Checked
    If chkMerge.Checked Then
      txtMerge_TextChanged(txtMerge, New EventArgs)
    Else
      ClearImageList("MERGE")
    End If
    SortImageList()
    RedoColumns()
  End Sub
  Private Sub txtMerge_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragDrop
    TextBoxDragDropEvent(sender, e)
  End Sub
  Private Sub txtMerge_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragEnter
    TextBoxDragEnterEvent(sender, e)
  End Sub
  Private Sub txtMerge_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtMerge.DragOver
    TextBoxDragOverEvent(sender, e, {".wim", ".iso"})
  End Sub
  Private Sub txtMerge_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMerge.TextChanged
    If Not IO.File.Exists(txtMerge.Text) Then Return
    StopRun = False
    RunActivity = 2
    If tLister2 Is Nothing Then
      tLister2 = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ParseImageList))
      tLister2.Start("MERGE")
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
#Region "Packages"
  Private imageOrderCol As Integer
  Private imageOrderSorting As SortOrder
  Private Sub lvImages_ColumnClick(sender As Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lvImages.ColumnClick
    If e.Column = imageOrderCol Then
      If imageOrderSorting = SortOrder.Ascending Then
        imageOrderSorting = SortOrder.Descending
      Else
        imageOrderSorting = SortOrder.Ascending
      End If
    Else
      imageOrderCol = e.Column
      imageOrderSorting = SortOrder.Descending
    End If
    SortImageList()
    RedoColumns()
  End Sub
  Private Sub lvImages_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvImages.KeyUp
    If lvImages.SelectedItems IsNot Nothing AndAlso lvImages.SelectedItems.Count > 0 Then
      If e.KeyCode = Keys.F2 Then
        RenamePackage(CType(lvImages.SelectedItems(0), ListViewItem))
      End If
    End If
  End Sub
  Private Sub lvImages_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvImages.MouseDoubleClick
    If lvImages.SelectedItems IsNot Nothing AndAlso lvImages.SelectedItems.Count > 0 Then
      If lvImages.SelectedItems(0).SubItems(1).Bounds.Contains(e.Location) Then
        RenamePackage(CType(lvImages.SelectedItems(0), ListViewItem))
      Else
        ShowPackageProperties()
      End If
    End If
  End Sub
  Private Sub ShowPackageProperties()
    Dim wimID As String = lvImages.SelectedItems(0).Tag(0)
    Dim propData As ImagePackage = lvImages.SelectedItems(0).Tag(1)
    Dim featureData As List(Of Feature) = lvImages.SelectedItems(0).Tag(2)
    Dim driverData As List(Of Driver) = lvImages.SelectedItems(0).Tag(3)
    Dim props As New frmPackageProps(wimID, propData, featureData, driverData)
    props.txtName.Text = lvImages.SelectedItems(0).SubItems(1).Text
    AddHandler props.Response, AddressOf PackageProperties_Response
    props.Show(Me)
  End Sub
  Private Sub PackageProperties_Response(sender As Object, e As frmPackageProps.PackagePropertiesEventArgs)
    For Each lvItem As ListViewItem In lvImages.Items
      If lvItem.Text = e.ImageIndex And lvItem.SubItems(1).Text = e.OldImageName Then
        If Not String.IsNullOrEmpty(e.NewImageName) Then lvItem.SubItems(1).Text = e.NewImageName
        Dim tag0 As String = Nothing
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If LBound(lvItem.Tag) = 0 Then tag0 = lvItem.Tag(0)
          End If
        End If
        Dim tag1 As ImagePackage = Nothing
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 0 Then tag1 = lvItem.Tag(1)
          End If
        End If
        Dim tag2 As List(Of Feature) = Nothing
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 1 Then tag2 = lvItem.Tag(2)
          End If
        End If
        Dim tag3 As List(Of Driver) = Nothing
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 2 Then tag3 = lvItem.Tag(3)
          End If
        End If
        If e.UpdateList IsNot Nothing AndAlso e.UpdateList.Length > 0 Then
          For Each item In e.UpdateList
            For I As Integer = 0 To tag1.IntegratedUpdateList.Count - 1
              If tag1.IntegratedUpdateList(I).Identity = item.Identity Then
                Dim newTag1Update As Update_Integrated = tag1.IntegratedUpdateList(I)
                newTag1Update.Remove = item.Remove
                tag1.IntegratedUpdateList(I) = newTag1Update
                Exit For
              End If
            Next
          Next
        End If
        If e.FeatureList IsNot Nothing AndAlso e.FeatureList.Length > 0 Then
          For Each item In e.FeatureList
            For I As Integer = 0 To tag2.Count - 1
              If tag2(I).FeatureName = item.FeatureName Then
                Dim newTag2Feature As Feature = tag2(I)
                newTag2Feature.Enable = item.Enable
                tag2(I) = newTag2Feature
                Exit For
              End If
            Next
          Next
        End If
        If e.DriverList IsNot Nothing AndAlso e.DriverList.Length > 0 Then
          For Each item In e.DriverList
            For I As Integer = 0 To tag3.Count - 1
              If tag3(I).PublishedName = item.PublishedName Then
                Dim newTag3Driver As Driver = tag3(I)
                newTag3Driver.Remove = item.Remove
                tag3(I) = newTag3Driver
                Exit For
              End If
            Next
          Next
        End If
        lvItem.Tag = {tag0, tag1, tag2, tag3}
        Exit For
      End If
    Next
    SetRequirements()
  End Sub
  Private Sub lvImages_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvImages.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      Dim selItem As ListViewItem = lvImages.GetItemAt(e.X, e.Y)
      If selItem IsNot Nothing Then
        mnuImages.Tag = selItem
        mnuPackageInclude.Checked = selItem.Checked
        mnuImages.Show(lvImages, e.Location)
      End If
    End If
  End Sub
  Private Delegate Sub ClearImageListInvoker(ToClear As String)
  Private Sub ClearImageList(ToClear As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ClearImageListInvoker(AddressOf ClearImageList), ToClear)
      Return
    End If
    If String.IsNullOrEmpty(ToClear) Then
      lvImages.Items.Clear()
    Else
      For Each lvItem As ListViewItem In lvImages.Items
        If lvItem.Tag(0) = ToClear Then lvItem.Remove()
      Next
    End If
    RedoColumns()
  End Sub
  Private Delegate Sub ImageListInvoker(lvItem As ListViewItem)
  Private Sub AddToImageList(lvItem As ListViewItem)
    If Me.InvokeRequired Then
      Me.Invoke(New ImageListInvoker(AddressOf AddToImageList), lvItem)
      Return
    End If
    lvImages.Items.Add(lvItem)
    SortImageList()
    RedoColumns()
    chkSP_CheckedChanged(chkSP, New EventArgs)
  End Sub
  Private Sub SortImageList()
    Dim sortOrder As LVItemSorter.OrderBy = LVItemSorter.OrderBy.Display
    If imageOrderCol = 1 Then
      sortOrder = LVItemSorter.OrderBy.OS
    ElseIf imageOrderCol = 2 Then
      sortOrder = LVItemSorter.OrderBy.Size
    End If

    If chkMerge.Checked And sortOrder = LVItemSorter.OrderBy.Display Then
      lvImages.ShowGroups = True
      lvImages.Groups.Clear()
      If imageOrderSorting = Windows.Forms.SortOrder.Ascending Then
        lvImages.Groups.Add("MERGE", "Merge Image")
        lvImages.Groups.Add("WIM", "Source Image")
      Else
        lvImages.Groups.Add("WIM", "Source Image")
        lvImages.Groups.Add("MERGE", "Merge Image")
      End If
      For Each lvItem As ListViewItem In lvImages.Items
        lvItem.Group = lvImages.Groups(lvItem.Tag(0))
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
    lvImages.ListViewItemSorter = New LVItemSorter(sortOrder, imageOrderSorting)
    lvImages.Sort()
  End Sub
  Private Class LVItemSorter
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
    Private Function MakeComparison(x As ListViewItem, y As ListViewItem, s As OrderBy)
      Select Case s
        Case OrderBy.Display
          If x.Tag(0) = "WIM" And Not y.Tag(0) = "WIM" Then Return -1
          If Not x.Tag(0) = "WIM" And y.Tag(0) = "WIM" Then Return 1
          If Val(x.SubItems(0).Text) > Val(y.SubItems(0).Text) Then Return 1
          If Val(x.SubItems(0).Text) < Val(y.SubItems(0).Text) Then Return -1
          Return 0
        Case OrderBy.OS
          Dim packageX As ImagePackage = x.Tag(1)
          Dim packageY As ImagePackage = y.Tag(1)
          If packageX.Architecture = "x64" And Not packageY.Architecture = "x64" Then Return 1
          If Not packageX.Architecture = "x64" And packageY.Architecture = "x64" Then Return -1
          If packageX.SPLevel > packageY.SPLevel Then Return 1
          If packageX.SPLevel < packageY.SPLevel Then Return -1
          Dim iX As Integer = 0
          Dim iY As Integer = 0
          If packageX.Desc = "Windows 7 STARTER" Then
            iX = 1
          ElseIf packageX.Desc = "Windows 7 HOMEBASIC" Then
            iX = 2
          ElseIf packageX.Desc = "Windows 7 HOMEPREMIUM" Then
            iX = 3
          ElseIf packageX.Desc = "Windows 7 PROFESSIONAL" Then
            iX = 4
          ElseIf packageX.Desc = "Windows 7 ULTIMATE" Then
            iX = 5
          ElseIf packageX.Desc = "Windows 7 ENTERPRISE" Then
            iX = 6
          Else
            iX = 7
          End If
          If packageY.Desc = "Windows 7 STARTER" Then
            iY = 1
          ElseIf packageY.Desc = "Windows 7 HOMEBASIC" Then
            iY = 2
          ElseIf packageY.Desc = "Windows 7 HOMEPREMIUM" Then
            iY = 3
          ElseIf packageY.Desc = "Windows 7 PROFESSIONAL" Then
            iY = 4
          ElseIf packageY.Desc = "Windows 7 ULTIMATE" Then
            iY = 5
          ElseIf packageY.Desc = "Windows 7 ENTERPRISE" Then
            iY = 6
          Else
            iY = 7
          End If
          If iX > iY Then
            Return 1
          ElseIf iX < iY Then
            Return -1
          End If
          Return Date.Compare(Date.Parse(packageX.Modified.Replace(" - ", " ")), Date.Parse(packageY.Modified.Replace(" - ", " ")))
        Case OrderBy.Size
          Dim packageX As ImagePackage = x.Tag(1)
          Dim packageY As ImagePackage = y.Tag(1)
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
  Private Sub ClearLister(ToClear As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ClearImageListInvoker(AddressOf ClearLister), ToClear)
      Return
    End If
    If ToClear = "WIM" Then
      tLister = Nothing
    ElseIf ToClear = "MERGE" Then
      tLister2 = Nothing
    Else
      tLister = Nothing
      tLister2 = Nothing
    End If
  End Sub
  Private Delegate Sub ParseImageListInvoker(ToRun As String)
  Private Sub ParseImageList(ToRun As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ParseImageListInvoker(AddressOf ParseImageList), ToRun)
      Return
    End If
    SetDisp(MNGList.Delete)
    SetTitle("Parsing WIM Packages", "Reading data from Windows Image package descriptor...")
    ToggleInputs(False)
    If ToRun = "WIM" Then
      ParseMainWIM()
      If tWIMDrag Is Nothing Then
        tWIMDrag = New Threading.Thread(New Threading.ThreadStart(AddressOf SetISOtoWIM))
        tWIMDrag.Start()
      End If
    ElseIf ToRun = "MERGE" Then
      ParseMergeWIM()
    Else
      ParseMainWIM()
      ParseMergeWIM()
    End If
    ClearLister(ToRun)
    ToggleInputs(True)
    FreshDraw()
  End Sub
  Private Sub RenamePackage(selItem As ListViewItem)
    Dim sName As String = selItem.SubItems(1).Text
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
          If Not String.IsNullOrEmpty(txtInput.Text) Then selItem.SubItems(1).Text = txtInput.Text
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
          If Not String.IsNullOrEmpty(txtInput.Text) Then selItem.SubItems(1).Text = txtInput.Text
          lvImages.Controls.Remove(txtInput)
          txtInput = Nothing
        End If
      End Sub)
  End Sub
  Private Sub chkLoadPackageData_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkLoadFeatures.CheckedChanged, chkLoadUpdates.CheckedChanged, chkLoadDrivers.CheckedChanged
    cmdLoadPackages.Enabled = chkLoadFeatures.Checked Or chkLoadUpdates.Checked Or chkLoadDrivers.Checked
    Dim sList As New List(Of String)
    If chkLoadFeatures.Checked Then sList.Add("Features")
    If chkLoadUpdates.Checked Then sList.Add("Updates")
    If chkLoadDrivers.Checked Then sList.Add("Drivers")
    Dim sListText As String
    If sList.Count = 0 Then
      sListText = "Features, Updates, and Drivers"
    ElseIf sList.Count = 1 Then
      sListText = sList(0)
    ElseIf sList.Count = 2 Then
      sListText = sList(0) & " and " & sList(1)
    Else
      sListText = sList(0) & ", " & sList(1) & ", and " & sList(2)
    End If
    ttInfo.SetToolTip(cmdLoadPackages, "Begin Image Package Parsing procedure to gather information about " & sListText & "." & vbNewLine & vbNewLine &
                                       "(This is optional - if you know which files are already integrated, you can save time by skipping this.)")
    If Me.Tag Is Nothing Then
      If Not chkLoadFeatures.Checked = mySettings.LoadFeatures Then mySettings.LoadFeatures = chkLoadFeatures.Checked
      If Not chkLoadUpdates.Checked = mySettings.LoadUpdates Then mySettings.LoadUpdates = chkLoadUpdates.Checked
      If Not chkLoadDrivers.Checked = mySettings.LoadDrivers Then mySettings.LoadDrivers = chkLoadDrivers.Checked
    End If
  End Sub
  Private Sub cmdLoadPackages_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoadPackages.Click
    If (Not chkLoadFeatures.Checked) And (Not chkLoadUpdates.Checked) And (Not chkLoadDrivers.Checked) Then
      MsgDlg(Me, "You must select which data you wish to load from the Image Packages before beginning the procedure.", "No Data Selected", "Parse Image Packages", MessageBoxButtons.OK, TaskDialogIcon.ControlPanel, , , "No Parse Data Selected")
      Return
    End If
    cmdLoadPackages.Image = My.Resources.u_i
    Dim doFeatures As Boolean = chkLoadFeatures.Checked
    Dim doUpdates As Boolean = chkLoadUpdates.Checked
    Dim doDrivers As Boolean = chkLoadDrivers.Checked
    If doFeatures Then
      LoadPackageFeatures("WIM")
      LoadPackageFeatures("Merge")
      chkLoadFeatures.Checked = False
    End If
    If doUpdates Then
      LoadPackageUpdates("WIM")
      LoadPackageUpdates("Merge")
      chkLoadUpdates.Checked = False
    End If
    If doDrivers Then
      LoadPackageDrivers("WIM")
      LoadPackageDrivers("Merge")
      chkLoadDrivers.Checked = False
    End If
    chkLoadFeatures.Checked = doFeatures
    chkLoadUpdates.Checked = doUpdates
    chkLoadDrivers.Checked = doDrivers
    cmdLoadPackages.Image = My.Resources.u_a
  End Sub
#Region "Package Feature List"
  Private LoadFeatureComplete As Boolean
  Public Sub LoadPackageFeatures(WIMID As String, Optional SelectedIndex As Integer = -1)
    ToggleInputs(False)
    CleanMounts()
    LoadFeatureComplete = False
    StopRun = False
    If SelectedIndex = -1 Then
      SetTitle("Parsing Features", "Populating the features list for each image package...")
    Else
      SetTitle("Parsing Features", "Populating the features list for the selected image package...")
    End If
    RunActivity = 3
    tListUp = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf LoadFeatureList))
    tListUp.Start({WIMID, SelectedIndex})
    Do Until LoadFeatureComplete
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then
        SetTotal(0, 1)
        SetProgress(0, 1)
        ToggleInputs(True)
        Return
      End If
    Loop
    tListUp = Nothing
    SetTotal(0, 1)
    SetProgress(0, 1)
    ToggleInputs(True)
  End Sub
  Private Delegate Sub LoadFeatureListInvoker(Param As Object)
  Private Sub LoadFeatureList(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New LoadFeatureListInvoker(AddressOf LoadFeatureList), Param)
      Return
    End If
    Dim ListType As String = Param(0)
    Dim SelIndex As Integer = Param(1)
    Dim sWIM As String = Nothing
    If ListType = "WIM" Then
      sWIM = txtWIM.Text
    ElseIf ListType = "Merge" Then
      sWIM = txtMerge.Text
    End If
    If String.IsNullOrEmpty(sWIM) Then
      LoadFeatureComplete = True
      Return
    End If
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkWIM As String = ParseWork & ListType & IO.Path.DirectorySeparatorChar
    If IO.File.Exists(ParseWork) Then
      WriteToOutput("Deleting """ & ParseWork & """...")
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End If
    If StopRun Then Return
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    Dim totalVal As Integer = 0
    Dim totalMax As Integer = 2
    SetTotal(totalVal, totalMax)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      totalMax += 1
      totalVal += 1
      SetTotal(totalVal, totalMax)
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      If StopRun Then Return
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetStatus("Reading Image Packages...")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      If StopRun Then Return
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetProgress(0, 1)
      SetStatus("Populating Image Package List...")
      For I As Integer = 1 To PackageCount
        If SelIndex > -1 Then
          If Not I = SelIndex Then Continue For
        End If
        Dim progVal As Integer = (I - 1) * 4
        Dim progMax As Integer = (PackageCount) * 4
        If SelIndex > -1 Then
          progVal = 0
          progMax = 4
        End If
        SetProgress(progVal, progMax)
        Dim Package As ImagePackage = GetDISMPackageData(WIMFile, I)
        If Package = New ImagePackage Then
          LoadFeatureComplete = True
          Return
        End If
        If Package.Architecture.ToLower = "x64" Then
          If Not Package.Name.Contains("64") Then Package.Name &= " x64"
        End If
        Dim lvItem As ListViewItem = Nothing
        For Each item As ListViewItem In lvImages.Items
          Dim iPackage As ImagePackage = item.Tag(1)
          If item.Tag(0) = ListType.ToUpper And Package = iPackage Then
            lvItem = item
            Exit For
          End If
        Next
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 1 Then
              Dim tFL As List(Of Feature) = lvItem.Tag(2)
              If tFL IsNot Nothing AndAlso tFL.Count > 0 Then Continue For
            End If
          End If
        End If
        progVal += 1
        SetProgress(progVal, progMax)
        SetStatus("Mounting " & Package.Name & " Package...")
        If InitDISM(WIMFile, I, Mount) Then
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          SetStatus("Populating " & Package.Name & " Package Feature List...")
          Dim FeatureNames() As String = GetDISMFeatures(Mount)
          Dim FeatureCount As Integer = FeatureNames.Length
          If StopRun Then Return
          Dim Features As New List(Of Feature)
          For J As Integer = 0 To FeatureCount - 1
            SetProgress(progVal * FeatureCount + (J + 1), progMax * FeatureCount)
            SetStatus("Parsing " & Package.Name & " Package Feature " & (J + 1) & " of " & FeatureCount & ": " & FeatureNames(J) & "...")
            Features.Add(GetDISMFeatureData(Mount, FeatureNames(J)))
            If StopRun Then Return
          Next
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          Dim tag0 As String = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If LBound(lvItem.Tag) = 0 Then tag0 = lvItem.Tag(0)
            End If
          End If
          If String.IsNullOrEmpty(tag0) Then tag0 = ListType.ToUpper
          Dim tag1 As ImagePackage = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If UBound(lvItem.Tag) > 0 Then tag1 = lvItem.Tag(1)
            End If
          End If
          If tag1.Index = 0 Then tag1 = Package
          Dim tag2 As List(Of Feature) = Features
          Dim tag3 As List(Of Driver) = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If LBound(lvItem.Tag) > 2 Then tag3 = lvItem.Tag(3)
            End If
          End If
          lvItem.Tag = {tag0, tag1, tag2, tag3}
        Else
          SetStatus("Failed to Mount Package """ & Package.Name & """!")
          LoadFeatureComplete = True
          Return
        End If
        progVal += 1
        SetProgress(progVal, progMax)
        SetStatus("Dismounting " & Package.Name & " Package...")
        DiscardDISM(Mount)
      Next
      SetStatus("Idle")
    Else
      SetStatus("Could not Extract Image from ISO!")
    End If
    LoadFeatureComplete = True
  End Sub
#End Region
#Region "Package Updates List"
  Private LoadUpdateComplete As Boolean
  Public Sub LoadPackageUpdates(WIMID As String, Optional SelectedIndex As Integer = -1)
    ToggleInputs(False)
    CleanMounts()
    LoadUpdateComplete = False
    StopRun = False
    If SelectedIndex = -1 Then
      SetTitle("Parsing Updates", "Populating the integrated update list for each image package...")
    Else
      SetTitle("Parsing Updates", "Populating the integrated update list for the selected image package...")
    End If
    RunActivity = 3
    tListUp = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf LoadUpdateList))
    tListUp.Start({WIMID, SelectedIndex})
    Do Until LoadUpdateComplete
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then
        SetTotal(0, 1)
        SetProgress(0, 1)
        ToggleInputs(True)
        Return
      End If
    Loop
    SetRequirements()
    tListUp = Nothing
    SetTotal(0, 1)
    SetProgress(0, 1)
    ToggleInputs(True)
  End Sub
  Private Delegate Sub LoadUpdateListInvoker(Param As Object)
  Private Sub LoadUpdateList(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New LoadUpdateListInvoker(AddressOf LoadUpdateList), Param)
      Return
    End If
    Dim ListType As String = Param(0)
    Dim selIndex As Integer = Param(1)
    Dim sWIM As String = Nothing
    If ListType = "WIM" Then
      sWIM = txtWIM.Text
    ElseIf ListType = "Merge" Then
      sWIM = txtMerge.Text
    End If
    If String.IsNullOrEmpty(sWIM) Then
      LoadUpdateComplete = True
      Return
    End If
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkWIM As String = ParseWork & ListType & IO.Path.DirectorySeparatorChar
    If IO.File.Exists(ParseWork) Then
      WriteToOutput("Deleting """ & ParseWork & """...")
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End If
    If StopRun Then Return
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    Dim totalVal As Integer = 0
    Dim totalMax As Integer = 2
    SetTotal(totalVal, totalMax)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      totalMax += 1
      totalVal += 1
      SetTotal(totalVal, totalMax)
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      If StopRun Then Return
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetStatus("Reading Image Packages...")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      If StopRun Then Return
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetProgress(0, 1)
      SetStatus("Populating Image Package List...")
      For I As Integer = 1 To PackageCount
        If selIndex > -1 Then
          If Not I = selIndex Then Continue For
        End If
        Dim progVal As Integer = I * 3
        Dim progMax As Integer = (PackageCount + 1) * 3
        If selIndex > -1 Then
          progVal = 0
          progMax = 3
        End If
        SetProgress(progVal, progMax)
        Dim Package As ImagePackage = GetDISMPackageData(WIMFile, I)
        If Package = New ImagePackage Then
          LoadUpdateComplete = True
          Return
        End If
        If Package.Architecture.ToLower = "x64" Then
          If Not Package.Name.Contains("64") Then Package.Name &= " x64"
        End If
        Dim lvItem As ListViewItem = Nothing
        For Each item As ListViewItem In lvImages.Items
          Dim iPackage As ImagePackage = item.Tag(1)
          If item.Tag(0) = ListType.ToUpper And Package = iPackage Then
            lvItem = item
            Exit For
          End If
        Next
        If lvItem Is Nothing Then
          LoadUpdateComplete = True
          SetStatus("Failed to Match Package """ & Package.Name & """ to an Image Package!")
          Return
        End If
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 0 Then
              Dim tPI As ImagePackage = lvItem.Tag(1)
              If tPI.IntegratedUpdateList IsNot Nothing AndAlso tPI.IntegratedUpdateList.Count > 0 Then Continue For
            End If
          End If
        End If
        SetStatus("Mounting " & Package.Name & " Package...")
        If InitDISM(WIMFile, I, Mount) Then
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          SetStatus("Populating " & Package.Name & " Package Update List...")
          Dim upList As List(Of Update_Integrated) = GetDISMPackageItems(Mount, Package)
          For J As Integer = 0 To upList.Count - 1
            SetProgress(progVal * upList.Count + (J + 1), progMax * upList.Count)
            SetStatus("Parsing " & Package.Name & " Package Update " & (J + 1) & " of " & upList.Count & ": " & upList(J).Ident.Name & "...")
            GetDISMPackageItemData(Mount, upList(J))
            If StopRun Then Return
          Next
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          Package.PopulateUpdateList(upList)
          If StopRun Then Return
          Dim tag0 As String = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If LBound(lvItem.Tag) = 0 Then tag0 = lvItem.Tag(0)
            End If
          End If
          If String.IsNullOrEmpty(tag0) Then tag0 = ListType.ToUpper
          Dim tag1 As ImagePackage = Package
          Dim tag2 As List(Of Feature) = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If UBound(lvItem.Tag) > 1 Then tag2 = lvItem.Tag(2)
            End If
          End If
          Dim tag3 As List(Of Driver) = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If UBound(lvItem.Tag) > 2 Then tag3 = lvItem.Tag(3)
            End If
          End If
          lvItem.Tag = {tag0, tag1, tag2, tag3}
        Else
          SetStatus("Failed to Mount Package """ & Package.Name & """!")
          LoadUpdateComplete = True
          Return
        End If
        progVal += 1
        SetProgress(progVal, progMax)
        SetStatus("Dismounting " & Package.Name & " Package...")
        DiscardDISM(Mount)
      Next
      SetStatus("Idle")
    Else
      SetStatus("Could not Extract Image from ISO!")
    End If
    LoadUpdateComplete = True
  End Sub
#End Region
#Region "Package Drivers List"
  Private LoadDriverComplete As Boolean
  Public Sub LoadPackageDrivers(WIMID As String, Optional SelectedIndex As Integer = -1)
    ToggleInputs(False)
    CleanMounts()
    LoadDriverComplete = False
    StopRun = False
    If SelectedIndex = -1 Then
      SetTitle("Parsing Drivers", "Populating the Driver list for each image package...")
    Else
      SetTitle("Parsing Drivers", "Populating the Driver list for the selected image package...")
    End If
    RunActivity = 3
    tListUp = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf LoadDriverList))
    tListUp.Start({WIMID, SelectedIndex})
    Do Until LoadDriverComplete
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun Then
        SetTotal(0, 1)
        SetProgress(0, 1)
        ToggleInputs(True)
        Return
      End If
    Loop
    tListUp = Nothing
    SetTotal(0, 1)
    SetProgress(0, 1)
    ToggleInputs(True)
  End Sub
  Private Delegate Sub LoadDriverListInvoker(Param As Object)
  Private Sub LoadDriverList(Param As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New LoadDriverListInvoker(AddressOf LoadDriverList), Param)
      Return
    End If
    Dim ListType As String = Param(0)
    Dim selIndex As Integer = Param(1)
    Dim sWIM As String = Nothing
    If ListType = "WIM" Then
      sWIM = txtWIM.Text
    ElseIf ListType = "Merge" Then
      sWIM = txtMerge.Text
    End If
    If String.IsNullOrEmpty(sWIM) Then
      LoadDriverComplete = True
      Return
    End If
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkWIM As String = ParseWork & ListType & IO.Path.DirectorySeparatorChar
    If IO.File.Exists(ParseWork) Then
      WriteToOutput("Deleting """ & ParseWork & """...")
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End If
    If StopRun Then Return
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    Dim totalVal As Integer = 0
    Dim totalMax As Integer = 2
    SetTotal(totalVal, totalMax)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      totalMax += 1
      totalVal += 1
      SetTotal(totalVal, totalMax)
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      If StopRun Then Return
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetStatus("Reading Image Packages...")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      If StopRun Then Return
      totalVal += 1
      SetTotal(totalVal, totalMax)
      SetProgress(0, 1)
      SetStatus("Populating Image Package List...")
      For I As Integer = 1 To PackageCount
        If selIndex > -1 Then
          If Not I = selIndex Then Continue For
        End If
        Dim progVal As Integer = I * 3
        Dim progMax As Integer = (PackageCount + 1) * 3
        If selIndex > -1 Then
          progVal = 0
          progMax = 3
        End If
        SetProgress(progVal, progMax)
        Dim Package As ImagePackage = GetDISMPackageData(WIMFile, I)
        If Package = New ImagePackage Then
          LoadDriverComplete = True
          Return
        End If
        Dim arch As ArchitectureList = ArchitectureList.x86
        If Package.Architecture.ToLower = "x64" Then
          If Not Package.Name.Contains("64") Then Package.Name &= " x64"
          arch = ArchitectureList.amd64
        End If
        Dim lvItem As ListViewItem = Nothing
        For Each item As ListViewItem In lvImages.Items
          Dim iPackage As ImagePackage = item.Tag(1)
          If item.Tag(0) = ListType.ToUpper And Package = iPackage Then
            lvItem = item
            Exit For
          End If
        Next
        If lvItem Is Nothing Then
          LoadDriverComplete = True
          SetStatus("Failed to Match Package """ & Package.Name & """ to an Image Package!")
          Return
        End If
        If lvItem.Tag IsNot Nothing Then
          If IsArray(lvItem.Tag) Then
            If UBound(lvItem.Tag) > 0 Then
              Dim tDL As List(Of Driver) = lvItem.Tag(3)
              If tDL IsNot Nothing AndAlso tDL.Count > 0 Then Continue For
            End If
          End If
        End If
        SetStatus("Mounting " & Package.Name & " Package...")
        If InitDISM(WIMFile, I, Mount) Then
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          SetStatus("Populating " & Package.Name & " Package Driver List...")
          Dim driverList As List(Of Driver) = GetDISMDriverItems(Mount, True)
          For J As Integer = 0 To driverList.Count - 1
            SetProgress(progVal * driverList.Count + (J + 1), progMax * driverList.Count)
            SetStatus("Parsing " & Package.Name & " Package Driver " & (J + 1) & " of " & driverList.Count & ": " & driverList(J).OriginalFileName & "...")
            GetDISMDriverItemData(Mount, driverList(J), arch)
            If StopRun Then Return
          Next
          If StopRun Then Return
          progVal += 1
          SetProgress(progVal, progMax)
          If StopRun Then Return
          Dim tag0 As String = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If LBound(lvItem.Tag) = 0 Then tag0 = lvItem.Tag(0)
            End If
          End If
          If String.IsNullOrEmpty(tag0) Then tag0 = ListType.ToUpper
          Dim tag1 As ImagePackage = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If UBound(lvItem.Tag) > 0 Then tag1 = lvItem.Tag(1)
            End If
          End If
          If tag1.Index = 0 Then tag1 = Package
          Dim tag2 As List(Of Feature) = Nothing
          If lvItem.Tag IsNot Nothing Then
            If IsArray(lvItem.Tag) Then
              If UBound(lvItem.Tag) > 1 Then tag2 = lvItem.Tag(2)
            End If
          End If
          Dim tag3 As List(Of Driver) = driverList
          lvItem.Tag = {tag0, tag1, tag2, tag3}
        Else
          SetStatus("Failed to Mount Package """ & Package.Name & """!")
          LoadDriverComplete = True
          Return
        End If
        progVal += 1
        SetProgress(progVal, progMax)
        SetStatus("Dismounting " & Package.Name & " Package...")
        DiscardDISM(Mount)
      Next
      SetStatus("Idle")
    Else
      SetStatus("Could not Extract Image from ISO!")
    End If
    LoadDriverComplete = True
  End Sub
#End Region
#Region "Context Menu"
  Private Sub mnuPackageInclude_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageInclude.Click
    Dim selItem As ListViewItem = CType(mnuImages.Tag, ListViewItem)
    mnuPackageInclude.Checked = Not mnuPackageInclude.Checked
    selItem.Checked = mnuPackageInclude.Checked
  End Sub
  Private Sub mnuPackageRename_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageRename.Click
    RenamePackage(CType(mnuImages.Tag, ListViewItem))
  End Sub
  Private Sub mnuPackageLocation_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageLocation.Click
    Dim selItem As ListViewItem = CType(mnuImages.Tag, ListViewItem)
    If chkMerge.Checked Then
      If selItem.Tag(0) = "MERGE" Then
        Process.Start("explorer", "/select,""" & txtMerge.Text & """")
      Else
        Process.Start("explorer", "/select,""" & txtWIM.Text & """")
      End If
    Else
      Process.Start("explorer", "/select,""" & txtWIM.Text & """")
    End If
  End Sub
  Private Sub mnuPackageProperties_Click(sender As System.Object, e As System.EventArgs) Handles mnuPackageProperties.Click
    ShowPackageProperties()
  End Sub
#End Region
#End Region
#Region "UEFI"
  Private Sub chkUEFI_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUEFI.CheckedChanged
    If chkUEFI.Checked Then
      cmbLimitType.SelectedIndex = 1
    End If
  End Sub
#End Region
#Region "Limit"
  Private Sub cmbLimitType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbLimitType.SelectedIndexChanged
    If Me.Tag Is Nothing Then
      Dim sDefault As String = cmbLimit.Text
      SetLimitValues(cmbLimitType.SelectedIndex)
      If Not String.IsNullOrEmpty(sDefault) Then cmbLimit.Text = sDefault
      If cmbLimitType.SelectedIndex > 0 Then
        If String.IsNullOrEmpty(cmbLimit.Text) Then
          If chkUEFI.Checked Then
            cmbLimit.SelectedIndex = 1
          Else
            cmbLimit.SelectedIndex = 2
          End If
        End If
      Else
        cmbLimit.Text = Nothing
      End If
      mySettings.DefaultSplit = cmbLimitType.SelectedIndex
    End If
  End Sub
  Private Sub SetLimitValues(Index As Integer)
    Select Case Index
      Case 0
        cmbLimit.Text = Nothing
        cmbLimit.Enabled = False
      Case 1
        cmbLimit.Enabled = True
        cmbLimit.Items.Clear()
        cmbLimit.Items.AddRange({"700 MB (CD)", "4095 MB (32-Bit Limit)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)", "25000 MB (BD)", "50000 MB (Dual-Layer BD)"})
        If chkUEFI.Checked Then
          cmbLimit.SelectedIndex = 1
        Else
          cmbLimit.SelectedIndex = 2
        End If
      Case 2
        cmbLimit.Enabled = True
        cmbLimit.Items.Clear()
        cmbLimit.Items.AddRange({"700 MB (CD)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)", "25000 MB (BD)", "50000 MB (Dual-Layer BD)"})
        cmbLimit.SelectedIndex = 1
      Case Else
        cmbLimit.Text = Nothing
        cmbLimit.Enabled = False
    End Select
  End Sub
  Private Sub cmbLimit_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cmbLimit.KeyDown
    If e.KeyCode = Keys.OemPeriod Or e.KeyCode = Keys.Decimal Or e.KeyCode = Keys.Subtract Or e.KeyCode = Keys.OemMinus Then e.SuppressKeyPress = True
  End Sub
  Private Sub cmbLimit_LostFocus(sender As Object, e As System.EventArgs) Handles cmbLimit.LostFocus
    If Me.Tag Is Nothing Then
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
          If iLimit > 0 And cmbLimit.Text.ToLower.Contains(iLimit & " mb") Then
            mySettings.SplitVal = cmbLimit.Text
            Return
          End If
        ElseIf cmbLimit.Text.ToLower.Contains("kb") Then
          iLimit = Math.Ceiling(CSng(NumericVal(cmbLimit.Text)) / 1024.0F)
        ElseIf cmbLimit.Text.ToLower.Contains("b") Then
          iLimit = Math.Ceiling(CSng(NumericVal(cmbLimit.Text)) / 1024.0F / 1024.0F)
        Else
          iLimit = NumericVal(cmbLimit.Text)
        End If
      Catch ex As Exception
        iLimit = 1
      End Try
      If iLimit < 1 Then iLimit = 1
      If cmbLimitType.SelectedIndex = 2 And iLimit < 351 Then iLimit = 351
      cmbLimit.Text = iLimit & " MB"
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
    If Me.Tag Is Nothing Then mySettings.Priority = cmbPriority.Text
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
          Process.Start("explorer.exe", "/select," & sPath)
        Catch ex As Exception
          MsgDlg(Me, "Unable to open the folder for """ & sPath & """!", "Unable to open folder.", "Folder was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , ex.Message, "Open Folder Folder Missing")
        End Try
      Else
        MsgDlg(Me, "Unable to find the file """ & sPath & """!", "Unable to find completed Image.", "File was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , , "Open Folder File Missing")
      End If
    End If
  End Sub
  Private Sub cmdConfig_Click(sender As System.Object, e As System.EventArgs) Handles cmdConfig.Click
    frmConfig.ShowDialog(Me)
    mySettings = New MySettings
  End Sub
  Private Sub cmdBegin_Click(sender As System.Object, e As System.EventArgs) Handles cmdBegin.Click
    Slipstream()
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    If cmdClose.Text = "&Close" Then
      Me.Close()
    Else
      If RunActivity > 0 Then
        Dim sActivity As String = "doing work"
        Dim sProc As String = "current"
        Dim sTitle As String = "Working"
        Select Case RunActivity
          Case 1
            sActivity = "slipstreaming updates and packages"
            sProc = "update integration"
            sTitle = "Integrating"
          Case 2
            sActivity = "extracting and reading Image Package data"
            sProc = "extraction"
            sTitle = "Loading Package Data"
          Case 3
            sActivity = "extracting and reading Update data"
            sProc = "update parsing"
            sTitle = "Loading Updates"
        End Select
        If MsgDlg(Me, "Do you want to cancel the " & sProc & " proceess?", "SLIPS7REAM is busy " & sActivity & ".", "Stop " & sTitle & "?", MessageBoxButtons.YesNo, TaskDialogIcon.Question, MessageBoxDefaultButton.Button2, , "Stop " & sTitle) = Windows.Forms.DialogResult.No Then
          Return
        End If
      End If
      StopRun = True
      cmdClose.Enabled = False
      Application.DoEvents()
    End If
  End Sub
#End Region
#Region "Status"
  Private Delegate Function GetStatusInvoker() As String
  Private Function GetStatus() As String
    If Me.InvokeRequired Then
      Return Me.Invoke(New GetStatusInvoker(AddressOf GetStatus))
    Else
      Return lblActivity.Tag
    End If
  End Function
  Private ActivityMouse As Boolean
  Private Delegate Sub SetStatusInvoker(Message As String)
  Private Sub SetStatus(Message As String)
    If Me.InvokeRequired Then
      Me.Invoke(New SetStatusInvoker(AddressOf SetStatus), Message)
      Return
    End If
    SetActivityText(Message)
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
    If ActivityMouse Then
      HideActivityTT()
      ShowActivityTT()
    End If
    Application.DoEvents()
  End Sub
  Private Sub lblActivity_SizeChanged(sender As Object, e As System.EventArgs) Handles lblActivity.SizeChanged
    SetActivityText(GetStatus)
  End Sub
  Private Sub SetActivityText(Message As String)
    lblActivity.Tag = Message
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
      expectedW = g.MeasureString(Message, lblActivity.Font, lblActivity.Size, New StringFormat(StringFormatFlags.NoWrap), chars, lines).Width - 8
    End Using
    If chars < Message.Length Then
      lblActivity.Text = RTrim(Message.Substring(0, chars - 3)) & "..."
    Else
      lblActivity.Text = Message
    End If
  End Sub
  Private Sub lblActivity_MouseEnter(sender As Object, e As System.EventArgs) Handles lblActivity.MouseEnter
    ActivityMouse = True
    ShowActivityTT()
  End Sub
  Private Sub lblActivity_MouseLeave(sender As Object, e As System.EventArgs) Handles lblActivity.MouseLeave
    HideActivityTT()
    ActivityMouse = False
  End Sub
  Private Sub ShowActivityTT()
    If String.IsNullOrEmpty(GetStatus) Then Return
    If Not GetStatus() = lblActivity.Text Then
      ttActivity.Show(GetStatus, lblActivity, -3, -2, Integer.MaxValue)
    End If
  End Sub
  Private Sub HideActivityTT()
    ttActivity.Hide(lblActivity)
  End Sub
  Private Delegate Sub SetTitleInvoker(Title As String, Tooltip As String)
  Private Sub SetTitle(Title As String, Tooltip As String)
    If Me.InvokeRequired Then
      Me.Invoke(New SetTitleInvoker(AddressOf SetTitle), Title, Tooltip)
      Return
    End If
    sTitleText = Title
    If String.IsNullOrWhiteSpace(Tooltip) Then
      ttInfo.SetToolTip(pctTitle, Nothing)
    Else
      ttInfo.SetToolTip(pctTitle, Tooltip)
    End If
  End Sub
  Private realProgVal, realProgMax As Integer
  Private realTotalVal, realTotalMax As Integer
  Private Delegate Sub SetProgressInvoker(Value As Integer, Maximum As Integer, GonnaSub As Boolean)
  Public Sub SetProgress(Value As Integer, Maximum As Integer, Optional GonnaSub As Boolean = False)
    If Me.InvokeRequired Then
      Me.Invoke(New SetProgressInvoker(AddressOf SetProgress), Value, Maximum, GonnaSub)
      Return
    End If
    If Value = 0 And Maximum = 0 Then
      pbIndividual.Style = ProgressBarStyle.Marquee
      realProgVal = 0
      realProgMax = 1
      SetRawProgress(realProgVal, realProgMax)
    ElseIf Value <= Maximum Then
      pbIndividual.Style = ProgressBarStyle.Continuous
      realProgVal = Value
      realProgMax = Maximum
      If GonnaSub Then
        SetRawProgress(realProgVal - 1, realProgMax)
      Else
        SetRawProgress(realProgVal, realProgMax)
      End If
    Else
      pbIndividual.Style = ProgressBarStyle.Continuous
      realProgVal = 0
      realProgMax = Maximum
      SetRawProgress(realProgVal, realProgMax)
    End If
    If Not GonnaSub And realTotalVal > 0 And realProgVal > 0 Then
      Dim denominator As Integer = realTotalMax * realProgMax
      Dim numerator As Integer = ((realTotalVal - 1) * realProgMax) + realProgVal
      SetRawTotal(numerator, denominator)
    End If
  End Sub
  Private Delegate Sub SetSubProgressInvoker(Value As Integer, Maximum As Integer)
  Public Sub SetSubProgress(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.Invoke(New SetSubProgressInvoker(AddressOf SetSubProgress), Value, Maximum)
      Return
    End If
    If realProgVal > 0 And realProgMax > 1 Then
      Dim denominator As Integer = realProgMax * Maximum
      Dim numerator As Integer = ((realProgVal - 1) * Maximum) + Value
      SetRawProgress(numerator, denominator)
      If realTotalVal > 0 And numerator > 0 Then
        Dim tDenominator As Integer = realTotalMax * denominator
        Dim tNumerator As Integer = ((realTotalVal - 1) * denominator) + numerator
        SetRawTotal(tNumerator, tDenominator)
      End If
    Else
      Dim tmpProgVal As Integer = 0
      Dim tmpProgMax As Integer = 1
      If Value = 0 And Maximum = 0 Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        tmpProgVal = 0
        tmpProgMax = 1
        SetRawProgress(tmpProgVal, tmpProgMax)
      ElseIf Value <= Maximum Then
        pbIndividual.Style = ProgressBarStyle.Continuous
        tmpProgVal = Value
        tmpProgMax = Maximum
        SetRawProgress(tmpProgVal, tmpProgMax)
      Else
        pbIndividual.Style = ProgressBarStyle.Continuous
        tmpProgVal = 0
        tmpProgMax = Maximum
        SetRawProgress(tmpProgVal, tmpProgMax)
      End If
      If realTotalVal > 0 And tmpProgVal > 0 Then
        Dim denominator As Integer = realTotalMax * tmpProgMax
        Dim numerator As Integer = ((realTotalVal - 1) * tmpProgMax) + tmpProgVal
        SetRawTotal(numerator, denominator)
      End If
    End If
  End Sub
  Private Sub SetRawProgress(Value As Integer, Maximum As Integer)
    pbIndividual.Maximum = 10000
    pbIndividual.Value = (Value / Maximum) * 10000
    If Value = 0 And Maximum = 1 And pbIndividual.Style = ProgressBarStyle.Marquee Then
      ttInfo.SetToolTip(pbIndividual, "Progress: Indeterminate")
    Else
      ttInfo.SetToolTip(pbIndividual, "Progress: " & FormatPercent(Value / Maximum, 2, TriState.True, TriState.False, TriState.False))
    End If
  End Sub
  Public Sub SetTotal(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.Invoke(New SetSubProgressInvoker(AddressOf SetTotal), Value, Maximum)
      Return
    End If
    If Value = 0 Then
      realTotalVal = 0
      If Maximum < 1 Then Maximum = 1
      realTotalMax = Maximum
      SetRawTotal(realTotalVal, realTotalMax)
      Return
    End If
    If Value > Maximum Then
      realTotalVal = Maximum
      realTotalMax = Maximum
      SetRawTotal(realTotalVal - 1, realTotalMax)
      Return
    End If
    If (pbIndividual.Value = 0) Or (pbIndividual.Style = ProgressBarStyle.Marquee) Or (pbIndividual.Value >= pbIndividual.Maximum) Then
      realTotalVal = Value
      realTotalMax = Maximum
      SetRawTotal(realTotalVal - 1, realTotalMax)
      Return
    End If
    If realTotalVal > 0 Then
      Dim denominator As Integer = realTotalMax * pbIndividual.Maximum
      Dim numerator As Integer = ((realTotalVal - 1) * pbIndividual.Maximum) + pbIndividual.Value
      SetRawTotal(numerator, denominator)
    Else
      If Maximum < 1 Then Maximum = 1
      SetRawTotal(Value, Maximum)
    End If
  End Sub
  Private Sub SetRawTotal(Value As Integer, Maximum As Integer)
    pbTotal.Maximum = 10000
    pbTotal.Value = (Value / Maximum) * 10000
    ttInfo.SetToolTip(pbTotal, "Total Progress: " & FormatPercent(Value / Maximum, 2, TriState.True, TriState.False, TriState.False))
    Try
      If taskBar IsNot Nothing Then
        If Value > 0 Then
          taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
          taskBar.SetProgressValue(Me.Handle, pbTotal.Value, pbTotal.Maximum)
        Else
          taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
        End If
      End If
    Catch ex As Exception
    End Try
    Application.DoEvents()
  End Sub
  Private Sub expOutput_Closed(sender As Object, e As System.EventArgs) Handles expOutput.Closed
    ttInfo.Hide(expOutput)
    ttInfo.Hide(expOutput.pctExpander)
    ttInfo.SetToolTip(expOutput, "Show Output console.")
    ttInfo.SetToolTip(expOutput.pctExpander, "Show Output console.")
    If outputWindow Then
      txtOutput.Text = frmOutput.txtOutput.Text
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
      frmOutput.Hide()
      outputWindow = False
    Else
      pnlSlips7ream.SuspendLayout()
      pctOutputTear.Visible = False
      txtOutput.Visible = False
      Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
      Dim newHeight As Integer = Me.Height - HeightDifferentialA
      Do Until Me.Height <= newHeight + 4
        Me.Height -= 4
      Loop
      Me.Height = newHeight
      pnlSlips7ream.ResumeLayout(True)
    End If
  End Sub
  Private Sub expOutput_Opened(sender As System.Object, e As System.EventArgs) Handles expOutput.Opened
    ttInfo.Hide(expOutput)
    ttInfo.Hide(expOutput.pctExpander)
    ttInfo.SetToolTip(expOutput, "Hide Output console.")
    ttInfo.SetToolTip(expOutput.pctExpander, "Hide Output console.")
    If outputWindow Then
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
        Me.Height += 4
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
      redrawCaption()
    End If
  End Sub
  Private Delegate Sub WriteToOutputCallBack(Message As String)
  Private Sub WriteToOutput(Message As String)
    If Me.InvokeRequired Then
      Me.Invoke(New WriteToOutputCallBack(AddressOf WriteToOutput), Message)
      Return
    End If
    If outputWindow Then
      frmOutput.txtOutput.AppendText(Message & vbNewLine)
    Else
      txtOutput.AppendText(Message & vbNewLine)
    End If
  End Sub
  Private tearFrom As Point = Point.Empty
  Private moving As Boolean = False
  Private Sub pctOutputTear_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctOutputTear.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Left Then
      tearFrom = e.Location
    End If
  End Sub
  Private Sub pctOutputTear_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctOutputTear.MouseMove
    If moving Then
      Dim newX As Integer = MousePosition.X - tearFrom.X
      Dim newY As Integer = MousePosition.Y - tearFrom.Y
      frmOutput.Location = New Point(newX, newY)
    End If
    If outputWindow Then Return
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Not pctOutputTear.DisplayRectangle.Contains(e.Location) Then
        outputWindow = True
        moving = True
        pnlSlips7ream.SuspendLayout()
        pctOutputTear.Visible = False
        txtOutput.Visible = False
        Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
        Me.Height -= HeightDifferentialA
        pnlSlips7ream.ResumeLayout(True)
        frmOutput.Show(Me)
        Dim newX As Integer = MousePosition.X - tearFrom.X
        Dim newY As Integer = MousePosition.Y - tearFrom.Y
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
    If Not tearFrom.IsEmpty Then
      moving = False
      tearFrom = Point.Empty
      frmOutput.DoResize()
    End If
  End Sub
  Public Sub ReturnOutput()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ReturnOutput))
      Return
    End If
    moving = False
    tearFrom = Point.Empty
    If Not outputWindow Then Return
    outputWindow = False
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
#End Region
#End Region
#Region "Command Calls"
  Private Function CleanMounts() As Boolean
    Try
      Dim Args As String = "/Get-MountedWimInfo"
      WriteToOutput("DISM " & Args)
      Dim DISMInfo As String = RunWithReturn(DismPath, Args & " /English", True)
      Dim mFindA As String = WorkDir.Substring(0, WorkDir.Length - 1).ToLower
      Dim mFindB As String = ShortenPath(mFindA).ToLower
      If Not DISMInfo.Contains("No mounted images found.") Then
        SetStatus("Cleaning Old DISM Mounts...")
        Dim sLines() As String = Split(DISMInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Dir : ") Then
            Dim tmpPath As String = line.Substring(line.IndexOf(":") + 2)
            If line.ToLower.Contains(mFindA) Or line.ToLower.Contains(mFindB) Then DiscardDISM(tmpPath)
          End If
        Next
      End If
      Args = "/Cleanup-Wim"
      WriteToOutput("DISM " & Args)
      RunHidden(DismPath, Args)
      Args = "/unmount"
      WriteToOutput("IMAGEX " & Args)
      Dim ImageXInfo As String = RunWithReturn(AIKDir & "imagex", Args, True)
      If Not ImageXInfo.Contains("Number of Mounted Images: 0") Then
        SetStatus("Cleaning Old ImageX Mounts...")
        Dim sLines() As String = Split(ImageXInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Path") Then
            Dim tmpPath As String = line.Substring(line.IndexOf("[") + 1)
            tmpPath = tmpPath.Substring(0, tmpPath.IndexOf("]"))
            If tmpPath.ToLower.Contains(mFindA) Or tmpPath.ToLower.Contains(mFindB) Then
              Args = "/unmount " & ShortenPath(tmpPath)
              WriteToOutput("IMAGEX " & Args)
              RunHidden(AIKDir & "imagex", Args)
            End If
          End If
        Next
      End If
      Args = "/cleanup"
      WriteToOutput("IMAGEX " & Args)
      RunHidden(AIKDir & "imagex", Args)
      WriteToOutput("Deleting """ & WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar & """...")
      SlowDeleteDirectory(WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar, FileIO.DeleteDirectoryOption.DeleteAllContents)
      If IO.Directory.Exists(WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar) Then Return False
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
#Region "DISM"
  Private Function InitDISM(WIMFile As String, WIMIndex As Integer, MountPath As String) As Boolean
    Dim Args As String = "/Mount-Wim /WimFile:" & ShortenPath(WIMFile) & " /index:" & WIMIndex.ToString.Trim & " /MountDir:" & ShortenPath(MountPath)
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMPackages(WIMFile As String) As Integer
    Dim Args As String = "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile)
    WriteToOutput("DISM " & Args)
    Dim PackageList As String = RunWithReturn(DismPath, Args & " /English")
    Dim PackageRows() As String = Split(PackageList, vbNewLine)
    Dim Indexes As Integer = 0
    For Each row In PackageRows
      If row.StartsWith("Index : ") Then
        Indexes += 1
      End If
    Next
    Return Indexes
  End Function
  Private Function GetDISMPackageData(WIMFile As String, Index As Integer) As ImagePackage
    Dim Args As String = "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile) & " /index:" & Index
    WriteToOutput("DISM " & Args)
    Dim PackageList As String = RunWithReturn(DismPath, Args & " /English")
    Dim Info As New ImagePackage(PackageList)
    Return Info
  End Function
  Private Function AddPackageItemToDISM(MountPath As String, AddPath As String) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Add-Package /PackagePath:" & ShortenPath(AddPath)
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function RemovePackageItemFromDISM(MountPath As String, PackageName As String) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Remove-Package /PackageName:" & PackageName
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMPackageItems(MountPath As String, Parent As ImagePackage) As List(Of Update_Integrated)
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-Packages"
    WriteToOutput("DISM " & Args)
    Dim PackageList As String = RunWithReturn(DismPath, Args & " /English")
    If PackageList.Contains("Packages listing:") Then
      If PackageList.Contains("The operation completed successfully.") Then
        PackageList = PackageList.Substring(PackageList.IndexOf("Packages listing:") + 21)
        PackageList = PackageList.Substring(0, PackageList.IndexOf("The operation completed successfully.") - 4)
        Dim PackageItems() As String = Split(PackageList, vbNewLine & vbNewLine)
        Dim pList As New List(Of Update_Integrated)
        For Each item As String In PackageItems
          pList.Add(New Update_Integrated(Parent, item))
        Next
        Return pList
      End If
    End If
    Return Nothing
  End Function
  Private Sub GetDISMPackageItemData(MountPath As String, ByRef Package As Update_Integrated)
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-PackageInfo /PackageName:" & Package.Identity
    WriteToOutput("DISM " & Args)
    Dim PackageData As String = RunWithReturn(DismPath, Args & " /English")
    If PackageData.Contains("Package information:") Then
      If PackageData.Contains("The operation completed successfully.") Then
        PackageData = PackageData.Substring(PackageData.IndexOf("Packages information:") + 24)
        PackageData = PackageData.Substring(0, PackageData.IndexOf("The operation completed successfully.") - 4)
        Package.ParseInfo(PackageData)
      End If
    End If
    Return
  End Sub
  Private Function EnableDISMFeature(MountPath As String, FeatureName As String) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Enable-Feature /FeatureName:"
    If FeatureName.Contains(" ") Then
      Args &= """" & FeatureName & """"
    Else
      Args &= FeatureName
    End If
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DisableDISMFeature(MountPath As String, FeatureName As String) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Disable-Feature /FeatureName:"
    If FeatureName.Contains(" ") Then
      Args &= """" & FeatureName & """"
    Else
      Args &= FeatureName
    End If
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMFeatures(MountPath As String) As String()
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-Features /Format:Table"
    WriteToOutput("DISM " & Args)
    Dim FeatureList As String = RunWithReturn(DismPath, Args & " /English")
    If FeatureList.Contains("Features listing for package :") Then
      If FeatureList.Contains("The operation completed successfully.") Then
        FeatureList = FeatureList.Substring(FeatureList.LastIndexOf("| --"))
        FeatureList = FeatureList.Substring(FeatureList.IndexOf(vbNewLine) + 2)
        FeatureList = FeatureList.Substring(0, FeatureList.IndexOf("The operation completed successfully.") - 4)
        Dim FeatureItems() As String = Split(FeatureList, vbNewLine)
        For I As Integer = 0 To FeatureItems.Length - 1
          If FeatureItems(I).Contains("|") Then FeatureItems(I) = FeatureItems(I).Substring(0, FeatureItems(I).LastIndexOf("|"))
          FeatureItems(I) = Trim(FeatureItems(I))
        Next
        Return FeatureItems
      End If
    End If
    Return Nothing
  End Function
  Private Function GetDISMFeatureData(MountPath As String, FeatureName As String) As Feature
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-FeatureInfo /FeatureName:"
    If FeatureName.Contains(" ") Then
      Args &= """" & FeatureName & """"
    Else
      Args &= FeatureName
    End If
    WriteToOutput("DISM " & Args)
    Dim FeatureData As String = RunWithReturn(DismPath, Args & " /English")
    Dim Info As New Feature(FeatureData)
    If Not Info.FeatureName = FeatureName Then Stop
    Return Info
  End Function
  Private Function AddDriverToDISM(MountPath As String, AddPath As String, Recurse As Boolean, ForceUnsigned As Boolean) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Add-Driver /Driver:" & ShortenPath(AddPath)
    If Recurse Then Args &= " /Recurse"
    If ForceUnsigned Then Args &= " /ForceUnsigned"
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function RemoveDriverFromDISM(MountPath As String, DriverName As String) As Boolean
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Remove-Driver /Driver:" & DriverName
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMDriverItems(MountPath As String, All As Boolean) As List(Of Driver)
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-Drivers" & IIf(All, " /All", "") & " /Format:Table"
    WriteToOutput("DISM " & Args)
    Dim DriverList As String = RunWithReturn(DismPath, Args & " /English")
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
  Private Sub GetDISMDriverItemData(MountPath As String, ByRef Driver As Driver, arch As ArchitectureList)
    Dim Args As String = "/Image:" & ShortenPath(MountPath) & " /Get-DriverInfo /Driver:" & Driver.PublishedName
    WriteToOutput("DISM " & Args)
    Dim DriverData As String = RunWithReturn(DismPath, Args & " /English")
    If DriverData.Contains("Driver package information:") Then
      If DriverData.Contains("The operation completed successfully.") Then
        DriverData = DriverData.Substring(DriverData.IndexOf("Driver package information:") + 31)
        DriverData = DriverData.Substring(0, DriverData.IndexOf("The operation completed successfully.") - 4)
        Driver.ReadExtraData(DriverData, ShortenPath(MountPath), arch)
      End If
    End If
  End Sub
  Public Function GetDISMDriverItemData(DriverINFPath As String) As Driver
    Dim Args As String = "/Online /Get-DriverInfo /Driver:" & ShortenPath(DriverINFPath)
    WriteToOutput("DISM " & Args)
    Dim DriverData As String = RunWithReturn(DismPath, Args & " /English")
    If DriverData.Contains("Driver package information:") Then
      If DriverData.Contains("The operation completed successfully.") Then
        DriverData = DriverData.Substring(DriverData.IndexOf("Driver package information:") + 31)
        DriverData = DriverData.Substring(0, DriverData.IndexOf("The operation completed successfully.") - 4)
        Dim driver As New Driver("")
        driver.ReadExtraData(DriverData, "", IIf(Environment.Is64BitOperatingSystem, ArchitectureList.amd64, ArchitectureList.x86))
        Return driver
      End If
    End If
    Return Nothing
  End Function
  Private Function SaveDISM(MountPath As String) As Boolean
    Dim Args As String = "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /commit"
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DiscardDISM(MountPath As String) As Boolean
    Dim Args As String = "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /discard"
    WriteToOutput("DISM " & Args)
    Dim sRet As String = RunWithReturn(DismPath, Args & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
#End Region
#Region "IMAGEX"
  Private Function SplitWIM(SourceWIM As String, DestSWM As String, Size As Integer) As Boolean
    Dim Args As String = "/split " & SourceWIM & " " & DestSWM & " " & Size
    WriteToOutput("IMAGEX " & Args)
    Dim sRet As String = RunWithReturn(AIKDir & "imagex", Args)
    Return sRet.Contains("Successfully split")
  End Function
  Private Function ExportWIM(SourceWIM As String, SourceIndex As Integer, DestWIM As String, DestName As String) As Boolean
    ReturnProgress = True
    Dim Args As String = "/export """ & SourceWIM & """ " & SourceIndex & " """ & DestWIM & """ """ & DestName & """ /compress maximum"
    WriteToOutput("IMAGEX " & Args)
    Dim sRet As String = RunWithReturn(AIKDir & "imagex", Args)
    ReturnProgress = False
    Return sRet.Contains("Successfully exported image")
  End Function
  Private Function UpdateLang(SourcePath As String) As Boolean
    Dim Args As String = "-genlangini -dist:" & ShortenPath(SourcePath) & " -image:" & ShortenPath(Mount) & " -f"
    WriteToOutput("IntlCFG " & Args)
    Dim sRet As String = RunWithReturn(AIKDir & "intlcfg", Args)
    If Not sRet.Contains("A new Lang.ini file has been generated") Then
      Return False
    End If
    Dim MountPath As String = WorkDir & "BOOT" & IO.Path.DirectorySeparatorChar
    If Not IO.Directory.Exists(MountPath) Then IO.Directory.CreateDirectory(MountPath)
    ReturnProgress = True
    Args = "/mountrw " & ShortenPath(SourcePath & "sources" & IO.Path.DirectorySeparatorChar & "boot.wim") & " 2 " & ShortenPath(MountPath)
    WriteToOutput("IMAGEX " & Args)
    sRet = RunWithReturn(AIKDir & "imagex", Args)
    ReturnProgress = False
    If Not sRet.Contains("Successfully mounted image.") Then
      SetStatus("Failed to mount boot.wim!")
      Return False
    End If
    If IO.File.Exists(SourcePath & "sources" & IO.Path.DirectorySeparatorChar & "lang.ini") Then
      WriteToOutput("Copying """ & SourcePath & "sources" & IO.Path.DirectorySeparatorChar & "lang.ini"" to """ & MountPath & "sources" & IO.Path.DirectorySeparatorChar & "lang.ini""...")
      My.Computer.FileSystem.CopyFile(SourcePath & "sources" & IO.Path.DirectorySeparatorChar & "lang.ini", MountPath & "sources" & IO.Path.DirectorySeparatorChar & "lang.ini", True)
    End If
    ReturnProgress = True
    Args = "/unmount /commit " & ShortenPath(MountPath)
    WriteToOutput("IMAGEX " & Args)
    sRet = RunWithReturn(AIKDir & "imagex", Args)
    ReturnProgress = False
    If Not sRet.Contains("Successfully unmounted image.") Then
      SetStatus("Failed to unmount boot.wim!")
      Return False
    End If
    WriteToOutput("Deleting """ & MountPath & """...")
    SlowDeleteDirectory(MountPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
    Return True
  End Function
#End Region
#Region "OSCDIMG"
  Private Function MakeISO(FromDir As String, Label As String, BootOrderFile As String, FileSystem As String, DestISO As String) As Boolean
    ReturnProgress = True
    Dim Args As String
    If String.IsNullOrEmpty(BootOrderFile) Then
      Args = "-m " & FileSystem & " " & ShortenPath(FromDir.Substring(0, FromDir.Length - 1)) & " " & ShortenPath(DestISO) & " -l" & Label
    Else
      Args = "-m " & FileSystem & " -yo" & ShortenPath(BootOrderFile) & " -b" & ShortenPath(FromDir & "boot" & IO.Path.DirectorySeparatorChar & "etfsboot.com") & " " & ShortenPath(FromDir.Substring(0, FromDir.Length - 1)) & " " & ShortenPath(DestISO) & " -l" & Label
    End If
    WriteToOutput("OSCDIMG " & Args)
    Dim sRet As String = RunWithReturn(AIKDir & "oscdimg", Args)
    ReturnProgress = False
    Return sRet.Contains("Done.")
  End Function
#End Region
#Region "7-Zip"
  Private WithEvents Extractor As Extraction.ArchiveFile
  Private c_ExtractRet As New Collections.Generic.List(Of String)
  Private Delegate Sub ExtractAllFilesInvoker(Source As String, Destination As String)
  Private Sub ExtractAllFiles(Source As String, Destination As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ExtractAllFilesInvoker(AddressOf ExtractAllFiles), Source, Destination)
      Return
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncExtractAllFiles))
    Dim cIndex As Integer = c_ExtractRet.Count
    c_ExtractRet.Add(Nothing)
    tRunWithReturn.Start({Source, Destination, cIndex})
    Do While String.IsNullOrEmpty(c_ExtractRet(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = c_ExtractRet(cIndex)
    c_ExtractRet(cIndex) = Nothing
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Data Error"
        MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Unsupported Method"
        MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "File Not Found"
        MsgDlg(Me, "Unable to find files in " & IO.Path.GetFileName(Source) & "!", "The files were not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case Else
        MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Error")
    End Select
  End Sub
  Private Sub AsyncExtractAllFiles(Obj As Object)
    Dim Source, Destination As String, cIndex As UInteger
    Source = Obj(0)
    Destination = Obj(1)
    cIndex = Obj(2)
    If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extractor.Open()
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extractor.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      file.Destination = New IO.FileInfo(Destination & file.Name)
    Next
    Try
      Extractor.Extract()
      Extractor.Dispose()
      Extractor = Nothing
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Extracting: " & ex.Message
      Return
    End Try
    SetSubProgress(100, 100)
    c_ExtractRet(cIndex) = "OK"
  End Sub
  Private Delegate Sub ExtractFilesInvoker(Source As String, Destination As String, Except As String)
  Private Sub ExtractFiles(Source As String, Destination As String, Except As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ExtractFilesInvoker(AddressOf ExtractFiles), Source, Destination, Except)
      Return
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncExtractFiles))
    Dim cIndex As Integer = c_ExtractRet.Count
    c_ExtractRet.Add(Nothing)
    tRunWithReturn.Start({Source, Destination, Except, cIndex})
    Do While String.IsNullOrEmpty(c_ExtractRet(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = c_ExtractRet(cIndex)
    c_ExtractRet(cIndex) = Nothing
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Data Error"
        MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Unsupported Method"
        MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "File Not Found"
        MsgDlg(Me, "Unable to find files in " & IO.Path.GetFileName(Source) & "!", "The files were not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case Else
        MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Error")
    End Select
  End Sub
  Private Sub AsyncExtractFiles(Obj As Object)
    Dim Source, Destination, Except As String, cIndex As UInteger
    Source = Obj(0)
    Destination = Obj(1)
    Except = Obj(2)
    cIndex = Obj(3)
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extractor.Open()
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extractor.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      If Not file.Name.ToLower.EndsWith(Except.ToLower) Then
        file.Destination = New IO.FileInfo(Destination & file.Name)
      End If
    Next
    Try
      Extractor.Extract()
      Extractor.Dispose()
      Extractor = Nothing
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Extracting: " & ex.Message
      Return
    End Try
    SetSubProgress(100, 100)
    c_ExtractRet(cIndex) = "OK"
  End Sub
  Private Delegate Sub ExtractAFileInvoker(Source As String, Destination As String, File As String)
  Private Sub ExtractAFile(Source As String, Destination As String, File As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ExtractAFileInvoker(AddressOf ExtractAFile), Source, Destination, File)
      Return
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncExtractAFile))
    Dim cIndex As Integer = c_ExtractRet.Count
    c_ExtractRet.Add(Nothing)
    tRunWithReturn.Start({Source, Destination, File, cIndex})
    Do While String.IsNullOrEmpty(c_ExtractRet(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = c_ExtractRet(cIndex)
    c_ExtractRet(cIndex) = Nothing
    If StopRun Then Return
    Select Case sRet
      Case "OK"
      Case "CRC Error"
        MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Data Error"
        MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "Unsupported Method"
        MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case "File Not Found"
        MsgDlg(Me, "Unable to find """ & File & """ in " & IO.Path.GetFileName(Source) & "!", "The file was not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
      Case Else
        MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Error")
    End Select
  End Sub
  Private Sub AsyncExtractAFile(Obj As Object)
    Dim Source, Destination, Find As String
    Source = Obj(0)
    Destination = Obj(1)
    Find = Obj(2)
    Dim cIndex As UInteger = Obj(3)
    Dim bFound As Boolean = False
    If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extractor.Open()
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
      Return
    End Try
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extractor.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      If file.Name.ToLower.EndsWith(Find.ToLower) Then
        file.Destination = New IO.FileInfo(Destination & IO.Path.GetFileName(file.Name))
        bFound = True
        Exit For
      End If
    Next
    Try
      Extractor.Extract()
      Extractor.Dispose()
      Extractor = Nothing
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Extracting: " & ex.Message
      Return
    End Try
    SetSubProgress(100, 100)
    If bFound Then
      c_ExtractRet(cIndex) = "OK"
    Else
      c_ExtractRet(cIndex) = "File Not Found"
    End If
  End Sub
  Private Delegate Function ExtractFilesListInvoker(Source As String) As String()
  Private Function ExtractFilesList(Source As String) As String()
    If Me.InvokeRequired Then
      Return Me.Invoke(New ExtractFilesListInvoker(AddressOf ExtractFilesList), Source)
    End If
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncExtractFilesList))
    Dim cIndex As Integer = c_ExtractRet.Count
    c_ExtractRet.Add(Nothing)
    tRunWithReturn.Start({Source, cIndex})
    Do While String.IsNullOrEmpty(c_ExtractRet(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = c_ExtractRet(cIndex)
    c_ExtractRet(cIndex) = Nothing
    If sRet.Contains("|") Then
      Return Split(sRet, "|")
    Else
      Select Case sRet
        Case "CRC Error"
          MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to read the file list!", "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
        Case "Data Error"
          MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to read the file list!", "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
        Case "Unsupported Method"
          MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to read the file list!", "There was an error while extracting.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
        Case "File Not Found"
          MsgDlg(Me, "Unable to read any files in " & IO.Path.GetFileName(Source) & "!", "Files were not found.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
        Case "File List Busy"
          MsgDlg(Me, "Unable to read the file list in " & IO.Path.GetFileName(Source) & "!", "File list was busy.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction " & sRet)
        Case Else
          MsgDlg(Me, sRet, "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Error")
      End Select
      Return Nothing
    End If
  End Function
  Private Sub AsyncExtractFilesList(Obj As Object)
    Dim Source As String
    Source = Obj(0)
    Dim cIndex As UInteger = Obj(1)
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extractor.Open()
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
      Return
    End Try
    Dim sList As New List(Of String)
    For Each File In Extractor
      sList.Add(File.Name)
    Next
    If sList.Count > 0 Then
      c_ExtractRet(cIndex) = Join(sList.ToArray, "|")
    Else
      c_ExtractRet(cIndex) = "File Not Found"
    End If
  End Sub
  Private Function ExtractComment(Source As String) As String
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
    Try
      Extractor.Open()
      Return Extractor.ArchiveComment
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      Return Nothing
    End Try
  End Function
  Private Sub Extractor_ExtractFile(sender As Object, e As Extraction.COM.ExtractFileEventArgs) Handles Extractor.ExtractFile
    If StopRun Then e.ContinueOperation = False
    If Extractor.ExtractionCount() > 1 Then
      If e.Stage = Extraction.COM.ExtractionStage.Done Then
        SetSubProgress(e.Item.Index, Extractor.ItemCount)
        Application.DoEvents()
      End If
    End If
  End Sub
  Private Sub Extractor_ExtractProgress(sender As Object, e As Extraction.COM.ExtractProgressEventArgs) Handles Extractor.ExtractProgress
    If Extractor.ExtractionCount = 1 AndAlso e.Total > 1048576 * 64 Then
      If StopRun Then e.ContinueOperation = False
      Dim iMax As Integer = 10000
      Dim iVal As Integer = (e.Written / e.Total) * iMax
      If pbIndividual.Value = iVal AndAlso pbIndividual.Maximum = iMax Then Return
      SetSubProgress(iVal, iMax)
      Application.DoEvents()
    End If
  End Sub
  Private Function ExtractFailureAlert(Message As String) As Boolean
    If String.IsNullOrEmpty(Message) Then Return False
    If Message = "OK" Then
      Return False
    ElseIf Message.StartsWith("CRC Error") Then
      MsgDlg(Me, "CRC Error in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction CRC Error")
    ElseIf Message.StartsWith("Data Error") Then
      MsgDlg(Me, "Data Error in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Data Error")
    ElseIf Message.StartsWith("Unsupported Method") Then
      MsgDlg(Me, "Unsupported Method in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Unsupported Method")
    ElseIf Message.StartsWith("File Not Found") Then
      MsgDlg(Me, "Unable to find expected file in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "The file was not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction File Not Found")
    Else
      MsgDlg(Me, Message, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error, , , "Extraction Error")
    End If
    Return True
  End Function
#End Region
#Region "EXE2CAB"
  Private Function EXE2CAB(Source As String, Destination As String) As Boolean
    Try
      Using eRead As New IO.FileStream(Source, IO.FileMode.Open, IO.FileAccess.Read)
        Dim bFound As Boolean = False
        Do Until eRead.Position >= eRead.Length
          Dim bRead As Byte = eRead.ReadByte
          If bRead = &H4D Then
            If eRead.ReadByte = &H53 Then
              If eRead.ReadByte = &H43 Then
                If eRead.ReadByte = &H46 Then
                  bFound = True
                  eRead.Position -= 4
                  Exit Do
                Else
                  eRead.Position -= 3
                End If
              Else
                eRead.Position -= 2
              End If
            Else
              eRead.Position -= 1
            End If
          End If
        Loop
        If bFound Then
          Using cWrite As New IO.FileStream(Destination, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            cWrite.Position = 0
            Dim lSize As Long = eRead.Length - eRead.Position
            Dim maxSize As Long = Integer.MaxValue / 4
            If lSize > maxSize Then
              Dim lChunks As Long = Math.Floor(lSize / maxSize)
              Dim lLastSize As Long = lSize Mod maxSize
              For L As Long = 0 To lChunks - 1
                Dim bData(maxSize - 1) As Byte
                eRead.Read(bData, 0, maxSize)
                cWrite.Write(bData, 0, maxSize)
                Erase bData
              Next
              If lLastSize > 0 Then
                Dim bLast(lLastSize - 1) As Byte
                eRead.Read(bLast, 0, lLastSize)
                cWrite.Write(bLast, 0, lLastSize)
                Erase bLast
              End If
            Else
              Dim bData(lSize - 1) As Byte
              eRead.Read(bData, 0, lSize)
              cWrite.Write(bData, 0, lSize)
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
  Private ReturnProgress As Boolean
#Region "Run With Return"
  Private c_RunWithReturnRet As New Collections.Generic.List(Of String)
  Private Function RunWithReturn(Filename As String, Arguments As String, Optional IgnoreStopRun As Boolean = False) As String
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncRunWithReturn))
    Dim cIndex As Integer = c_RunWithReturnRet.Count
    c_RunWithReturnRet.Add(Nothing)
    c_RunWithReturnAccumulation.Add(Nothing)
    c_RunWithReturnErrorAccumulation.Add(Nothing)
    tRunWithReturn.Start({Filename, Arguments, cIndex, Process.GetCurrentProcess.PriorityClass})
    Do While c_RunWithReturnRet(cIndex) Is Nothing
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If Not IgnoreStopRun AndAlso StopRun Then
        Try
          tRunWithReturn.Abort()
        Catch ex As Exception
        End Try
        Return ""
      End If
    Loop
    Dim sRet As String = c_RunWithReturnRet(cIndex)
    c_RunWithReturnRet(cIndex) = Nothing
    Return sRet
  End Function
  Private Delegate Sub RunWithReturnRetCallBack(Index As Integer, Output As String)
  Private Sub RunWithReturnRet(Index As Integer, Output As String)
    If Me.InvokeRequired Then
      Me.Invoke(New RunWithReturnRetCallBack(AddressOf RunWithReturnRet), Index, Output)
      Return
    End If
    If Output.StartsWith("!" & vbNewLine) Then
      Output = Output.Substring(3)
    Else
      WriteToOutput(Output)
    End If
    c_RunWithReturnRet(Index) = Output
  End Sub
  Private Sub AsyncRunWithReturn(Obj As Object)
    Dim Filename As String = Obj(0)
    Dim Arguments As String = Obj(1)
    Dim Index As Integer = Obj(2)
    Dim Priority As Diagnostics.ProcessPriorityClass = Obj(3)
    Dim PkgList As New Process
    PkgList.StartInfo.FileName = Filename
    PkgList.StartInfo.Arguments = Arguments
    PkgList.StartInfo.UseShellExecute = False
    PkgList.StartInfo.RedirectStandardInput = True
    PkgList.StartInfo.RedirectStandardOutput = True
    PkgList.StartInfo.RedirectStandardError = True
    PkgList.StartInfo.CreateNoWindow = True
    PkgList.StartInfo.EnvironmentVariables.Add("RUNNING_INDEX", Index)
    AddHandler PkgList.OutputDataReceived, AddressOf RunWithReturnOutputHandler
    AddHandler PkgList.ErrorDataReceived, AddressOf RunWithReturnErrorHandler
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
    Catch ex As Exception
      RunWithReturnRet(Index, ex.Message)
      Err.Clear()
    End Try
  End Sub
  Private c_RunWithReturnAccumulation As New Collections.Generic.List(Of String)
  Private c_RunWithReturnErrorAccumulation As New Collections.Generic.List(Of String)
  Private Sub AsyncRunWithReturnRet(Index As Integer, Output As String)
    If Me.InvokeRequired Then
      Me.Invoke(New RunWithReturnRetCallBack(AddressOf AsyncRunWithReturnRet), Index, Output)
      Return
    End If
    If Output Is Nothing Then
      c_RunWithReturnRet(Index) = "!" & vbNewLine & c_RunWithReturnAccumulation(Index)
      c_RunWithReturnAccumulation(Index) = Nothing
    Else
      If ReturnProgress Then
        If Output.Contains(" ] Exporting progress") Or Output.Contains(" ] Mounting progress") Or Output.Contains(" ] Committing Image progress") Or Output.Contains(" ] Unmounting progress") Or Output.Contains(" ] Mount cleanup progress") Then
          Dim ProgI As String = Output.Substring(2)
          ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
          SetSubProgress(Val(ProgI), 100)
        End If
      End If
      c_RunWithReturnAccumulation(Index) &= Output & vbNewLine
      WriteToOutput(Output)
    End If
  End Sub
  Private Sub AsyncRunWithReturnErrorRet(Index As Integer, Output As String)
    If Me.InvokeRequired Then
      Me.Invoke(New RunWithReturnRetCallBack(AddressOf AsyncRunWithReturnErrorRet), Index, Output)
      Return
    End If
    If String.IsNullOrEmpty(Output) Then
      WriteToOutput(Nothing)
      Return
    End If
    If ReturnProgress Then
      If Output.Contains("% complete") Then
        Dim ProgI As String = Output.Substring(0, Output.IndexOf("%"))
        SetSubProgress(Val(ProgI), 100)
      End If
      WriteToOutput(Output)
    Else
      If Output.Contains("% complete") Then
        WriteToOutput(Output)
      Else
        WriteToOutput("<ERROR> " & Output)
      End If
    End If
  End Sub
  Private Sub RunWithReturnOutputHandler(sender As Object, e As DataReceivedEventArgs)
    Dim tmpData As String = e.Data
    Dim pkgData As Process = sender
    Dim Index As Integer = pkgData.StartInfo.EnvironmentVariables("RUNNING_INDEX")
    AsyncRunWithReturnRet(Index, tmpData)
  End Sub
  Private Sub RunWithReturnErrorHandler(sender As Object, e As DataReceivedEventArgs)
    Dim tmpData As String = e.Data
    If Not tmpData Is Nothing Then
      Dim pkgData As Process = sender
      Dim Index As Integer = pkgData.StartInfo.EnvironmentVariables("RUNNING_INDEX")
      AsyncRunWithReturnErrorRet(Index, tmpData)
    End If
  End Sub
#End Region
#Region "Run Hidden"
  Private c_RunHiddenRet As New Collections.Generic.List(Of Boolean)
  Private Sub RunHidden(Filename As String, Arguments As String)
    Dim tRunHidden As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncRunHidden))
    Dim cIndex As Integer = c_RunHiddenRet.Count
    c_RunHiddenRet.Add(False)
    tRunHidden.Start({Filename, Arguments, cIndex, Process.GetCurrentProcess.PriorityClass})
    Do Until c_RunHiddenRet(cIndex)
      Application.DoEvents()
      Threading.Thread.Sleep(1)
      If StopRun And Not Arguments.ToLower.Contains("unmount") Then
        Try
          tRunHidden.Abort()
        Catch ex As Exception
        End Try
        Return
      End If
    Loop
    c_RunHiddenRet(cIndex) = False
  End Sub
  Private Delegate Sub RunHiddenCallBack(Index As Integer)
  Private Sub RunHiddenRet(Index As Integer)
    If Me.InvokeRequired Then
      Me.Invoke(New RunHiddenCallBack(AddressOf RunHiddenRet), Index)
      Return
    End If
    c_RunHiddenRet(Index) = True
  End Sub
  Private Sub AsyncRunHidden(Obj As Object)
    Dim Filename As String = Obj(0)
    Dim Arguments As String = Obj(1)
    Dim Index As Integer = Obj(2)
    Dim Priority As Diagnostics.ProcessPriorityClass = Obj(3)
    Dim PkgList As New Process
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
      RunHiddenRet(Index)
    Catch ex As Exception
      Err.Clear()
      RunHiddenRet(Index)
    End Try
  End Sub
#End Region
  Private Sub ListCleanup(ByRef List As Collections.IList)
    Dim isEmpty As Boolean = True
    For Each item In List
      If item.GetType = GetType(String) AndAlso item Is Nothing Then
        REM ok
      ElseIf item.GetType = GetType(Boolean) AndAlso item = False Then
        REM ok
      Else
        isEmpty = False
        Exit For
      End If
    Next
    If isEmpty Then
      List.Clear()
    End If
  End Sub
#End Region
#End Region
#Region "Integration"
  Private Sub Slipstream()
    RunComplete = False
    StopRun = False
    LangChange = False
    If txtISOLabel.Enabled And txtISOLabel.Text.Contains(" ") Then
      SetStatus("Spaces are not allowed in the ISO Label!")
      txtISOLabel.Text = Replace(txtISOLabel.Text, " ", "_")
      txtISOLabel.Focus()
      Return
    End If
    SetTitle("Preparing Images", "Cleaning up mounts and extracting WIM from ISO if necessary...")
    RunActivity = 1
    ToggleInputs(False)
    Dim WIMFile As String = Nothing
    If IO.Directory.Exists(WorkDir) Then
      SetProgress(0, 1)
      If Not CleanMounts() Then
        ToggleInputs(True, "Active Mount Detected!")
        cmdConfig.Focus()
        Beep()
        MsgDlg(Me, "The selected Temp directory has an active mount that could not be removed. Please restart your computer or change your Temp directory before continuing.", "Active mount has been detected.", "Unable to Begin Slipstream Process", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked, , , "Active Mount")
        Return
      End If
      SetStatus("Clearing Old Data...")
      Try
        WriteToOutput("Deleting """ & WorkDir & """...")
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Application.DoEvents()
      Catch ex As Exception
        Application.DoEvents()
      End Try
    End If
    Dim iTotalVal As Integer = 0
    Dim iTotalMax As Integer = 3
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then iTotalMax += 1
    If chkMerge.Checked AndAlso IO.Path.GetExtension(txtMerge.Text).ToLower = ".iso" Then iTotalMax += 1
    If lvMSU.Items.Count > 0 Then iTotalMax += 2
    If Not String.IsNullOrEmpty(txtSP.Text) Then
      iTotalMax += 1
      If Not String.IsNullOrEmpty(txtSP64.Text) Then iTotalMax += 1
    End If
    If Not String.IsNullOrEmpty(txtISO.Text) Then
      iTotalMax += 2
      If cmbLimitType.SelectedIndex > 0 Then iTotalMax += 1
    End If
    Dim DoIntegratedUpdates As Boolean = False
    Dim DoFeatures As Boolean = False
    Dim DoIntegratedDrivers As Boolean = False
    For Each lvRow As ListViewItem In lvImages.Items
      If lvRow.Checked Then
        If Not DoIntegratedUpdates Then
          Dim TestIntegratedUpdates As List(Of Update_Integrated) = CType(lvRow.Tag(1), ImagePackage).IntegratedUpdateList
          If TestIntegratedUpdates IsNot Nothing AndAlso TestIntegratedUpdates.Count > 0 Then DoIntegratedUpdates = True
        End If
        If Not DoFeatures Then
          Dim TestFeatures As List(Of Feature) = lvRow.Tag(2)
          If TestFeatures IsNot Nothing AndAlso TestFeatures.Count > 0 Then DoFeatures = True
        End If
        If Not DoIntegratedDrivers Then
          Dim TestIntegratedDrivers As List(Of Driver) = lvRow.Tag(3)
          If TestIntegratedDrivers IsNot Nothing AndAlso TestIntegratedDrivers.Count > 0 Then DoIntegratedDrivers = True
        End If
      End If
    Next
    If DoIntegratedUpdates Then iTotalMax += 1
    If DoFeatures Then iTotalMax += 1
    If DoIntegratedUpdates Then iTotalMax += 1
    SetProgress(0, 1)
    SetTotal(iTotalVal, iTotalMax)
    pbTotal.Style = ProgressBarStyle.Continuous
    SetDisp(MNGList.Copy)
    SetStatus("Checking WIM...")
    If String.IsNullOrEmpty(txtWIM.Text) OrElse Not IO.File.Exists(txtWIM.Text) Then
      ToggleInputs(True, "Missing WIM File!")
      txtWIM.Focus()
      Beep()
      Return
    End If
    If chkMerge.Checked Then
      If String.IsNullOrEmpty(txtMerge.Text) OrElse Not IO.File.Exists(txtMerge.Text) Then
        ToggleInputs(True, "Missing Merge File!")
        txtMerge.Focus()
        Beep()
        Return
      End If
    End If
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetStatus("Extracting Image from ISO...")
      WriteToOutput("Extracting ""INSTALL.WIM"" from """ & txtWIM.Text & """ to """ & Work & """...")
      ExtractAFile(txtWIM.Text, Work, "INSTALL.WIM")
      WIMFile = Work & "INSTALL.WIM"
    Else
      WIMFile = txtWIM.Text
    End If
    If StopRun Then
      ToggleInputs(True)
      Return
    End If
    SetProgress(0, 1)
    SetDisp(MNGList.Delete)
    SetTitle("Merging Image Packages", "Merging all WIM packages into single WIM...")
    SetStatus("Checking Merge...")
    Dim MergeFile As String = Nothing
    If chkMerge.Checked Then MergeFile = txtMerge.Text
    Dim MergeWIM As String = Nothing
    Dim MergeWork As String = Work & "Merge" & IO.Path.DirectorySeparatorChar
    If Not String.IsNullOrEmpty(MergeFile) Then
      If IO.Directory.Exists(MergeWork) Then
        WriteToOutput("Deleting """ & MergeWork & """...")
        Try
          SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
        End Try
      End If
      IO.Directory.CreateDirectory(MergeWork)
      Dim MergeWorkExtract As String = MergeWork & "Extract" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(MergeWorkExtract) Then IO.Directory.CreateDirectory(MergeWorkExtract)
      If IO.Path.GetExtension(MergeFile).ToLower = ".iso" Then
        SetProgress(0, 1)
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        SetStatus("Extracting Merge Image from ISO...")
        WriteToOutput("Extracting ""INSTALL.WIM"" from """ & MergeFile & """ to """ & MergeWorkExtract & """...")
        ExtractAFile(MergeFile, MergeWorkExtract, "INSTALL.WIM")
        Application.DoEvents()
        MergeWIM = MergeWorkExtract & "INSTALL.WIM"
      Else
        MergeWIM = MergeFile
      End If
    End If
    If StopRun Then
      ToggleInputs(True)
      Return
    End If
    SetProgress(0, 1)
    iTotalVal += 1
    SetTotal(iTotalVal, iTotalMax)
    Dim iProgVal As Integer = 0
    Dim iProgMax As Integer = lvImages.CheckedItems.Count
    SetProgress(iProgVal, iProgMax)
    Dim NewWIM As String = Work & "newINSTALL.wim"
    SetStatus("Generating Image...")
    Dim ImageIntegratedUpdates() As List(Of Update_Integrated) = Nothing
    Dim ImageFeatures() As List(Of Feature) = Nothing
    Dim ImageDrivers() As List(Of Driver) = Nothing
    For Each lvRow As ListViewItem In lvImages.Items
      If lvRow.Checked Then
        ReDim Preserve ImageIntegratedUpdates(iProgVal)
        ReDim Preserve ImageFeatures(iProgVal)
        ReDim Preserve ImageDrivers(iProgVal)
        iProgVal += 1
        SetProgress(iProgVal, iProgMax, True)
        Dim RowIndex As String = lvRow.Text
        Dim RowName As String = lvRow.SubItems(1).Text
        Dim RowImage As String
        If lvRow.Tag(0) = "WIM" Then
          RowImage = WIMFile
        ElseIf lvRow.Tag(0) = "MERGE" Then
          RowImage = MergeWIM
        Else
          Continue For
        End If
        ImageIntegratedUpdates(iProgVal - 1) = CType(lvRow.Tag(1), ImagePackage).IntegratedUpdateList
        ImageFeatures(iProgVal - 1) = lvRow.Tag(2)
        ImageDrivers(iProgVal - 1) = lvRow.Tag(3)
        SetStatus("Merging WIM """ & RowName & """...")
        If ExportWIM(RowImage, RowIndex, NewWIM, RowName) Then
          Continue For
        Else
          ToggleInputs(True, "Failed to Merge WIM """ & RowName & """")
          Return
        End If
      End If
      If StopRun Then
        ToggleInputs(True)
        Return
      End If
    Next
    If IO.Directory.Exists(MergeWork) Then
      WriteToOutput("Deleting """ & MergeWork & """...")
      Try
        SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Catch ex As Exception
      End Try
    End If
    SetProgress(0, 1)
    iTotalVal += 1
    SetTotal(iTotalVal, iTotalMax)
    SetStatus("Making Backup of Old WIM...")
    Dim BakWIM As String = WorkDir & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(WIMFile & "_BAK.WIM")
    WriteToOutput("Copying """ & WIMFile & """ to """ & BakWIM & """...")
    If Not SlowCopyFile(WIMFile, BakWIM, True) Then
      ToggleInputs(True, "Failed to back up Install WIM!")
      Return
    End If
    SetStatus("Moving Generated WIM...")
    WriteToOutput("Copying """ & NewWIM & """ to """ & WIMFile & """...")
    If Not SlowCopyFile(NewWIM, WIMFile, True) Then
      SetStatus("Generated WIM Move Failed! Reverting to Old WIM...")
      WriteToOutput("Copying """ & BakWIM & """ to """ & WIMFile & """...")
      If Not SlowCopyFile(BakWIM, WIMFile, True) Then
        ToggleInputs(True, "Generated WIM Move Failed! Original WIM Restore Failed!")
      Else
        ToggleInputs(True, "Generated WIM Move Failed! Original WIM Restored.")
      End If
      Return
    End If
    If IO.File.Exists(BakWIM) Then
      SetStatus("Cleaning Up Backup WIM...")
      WriteToOutput("Deleting """ & BakWIM & """...")
      IO.File.Delete(BakWIM)
    End If
    SetProgress(0, 1)
    Application.DoEvents()
    If StopRun Then
      ToggleInputs(True)
      Return
    End If
    SetDisp(MNGList.Move)
    SetTitle("Preparing Files", "Checking updates and Service Packs to integrate...")
    SetStatus("Checking Service Pack...")
    Dim SPFile As String = Nothing
    If chkSP.Checked Then
      If String.IsNullOrEmpty(txtSP.Text) OrElse Not IO.File.Exists(txtSP.Text) Then
        ToggleInputs(True, "Missing Service Pack File!")
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
        ToggleInputs(True, "Missing x64 Service Pack File!")
        txtSP64.Focus()
        Beep()
        Return
      Else
        SP64File = txtSP64.Text
      End If
    Else
      SP64File = Nothing
    End If
    SetStatus("Checking ISO...")
    Dim ISOFile As String = Nothing
    If chkISO.Checked Then
      If String.IsNullOrEmpty(txtISO.Text) OrElse Not IO.File.Exists(txtISO.Text) Then
        ToggleInputs(True, "Missing ISO File!")
        txtISO.Focus()
        Beep()
        Return
      Else
        ISOFile = txtISO.Text
      End If
    Else
      ISOFile = Nothing
    End If
    SetStatus("Collecting Update List...")
    Dim UpdateFiles As New Collections.Generic.List(Of Update_File)
    Dim RemoveFiles As New Collections.Generic.List(Of Update_Integrated())
    If lvMSU.Items.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        UpdateFiles.Add(CType(lvItem.Tag(0), Update_File))
        If lvItem.Tag(1) IsNot Nothing Then
          RemoveFiles.Add(CType(lvItem.Tag(1), Update_Integrated()))
        End If
      Next
    End If
    If StopRun Then
      ToggleInputs(True)
      Return
    End If
    SetDisp(MNGList.Copy)
    SetTitle("Adding Service Packs", "Integrating Service Pack data into WIM packages...")
    If Not String.IsNullOrEmpty(SPFile) Then
      If String.IsNullOrEmpty(SP64File) Then
        SetProgress(0, 1)
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        SetStatus("Integrating Service Pack...")
        If IntegrateSP(WIMFile, SPFile) Then
          SetStatus("Service Pack Integrated!")
        Else
          Dim sMsg As String = GetStatus()
          ToggleInputs(True, sMsg)
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      Else
        SetProgress(0, 1)
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        SetStatus("Integrating x86 Service Pack...")
        If IntegrateSP(WIMFile, SPFile, "86") Then
          SetStatus("x86 Service Pack Integrated!")
        Else
          Dim sMsg As String = GetStatus()
          ToggleInputs(True, sMsg)
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
        SetProgress(0, 1)
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        SetStatus("Integrating x64 Service Pack...")
        If IntegrateSP(WIMFile, SP64File, "64") Then
          SetStatus("x64 Service Pack Integrated!")
        Else
          Dim sMsg As String = GetStatus()
          ToggleInputs(True, sMsg)
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      End If
    End If
    If DoFeatures Then
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetDisp(MNGList.Copy)
      SetTitle("Toggling Windows Features", "Enabling and disabling selected Features in WIM packages...")
      SetStatus("Toggling Features...")
      If IntegratedFeatures(WIMFile, ImageFeatures.ToArray) Then
        SetStatus("Features Toggled!")
      Else
        Dim sMsg As String = GetStatus()
        ToggleInputs(True, sMsg)
        Return
      End If
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return
      End If
    End If
    If DoIntegratedUpdates Then
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetDisp(MNGList.Copy)
      SetTitle("Removing Integrated Updates", "Removing selected integrated Windows Updates from WIM packages...")
      SetStatus("Removing Updates...")
      If IntegratedUpdates(WIMFile, ImageIntegratedUpdates.ToArray) Then
        SetStatus("Updates Removed!")
      Else
        Dim sMsg As String = GetStatus()
        ToggleInputs(True, sMsg)
        Return
      End If
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return
      End If
    End If
    If DoIntegratedDrivers Then
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetDisp(MNGList.Copy)
      SetTitle("Removing Integrated Drivers", "Removing selected integrated Drivers from WIM packages...")
      SetStatus("Removing Drivers...")
      If IntegratedDrivers(WIMFile, ImageDrivers) Then
        SetStatus("Drivers Removed!")
      Else
        Dim sMsg As String = GetStatus()
        ToggleInputs(True, sMsg)
        Return
      End If
    End If
    SetDisp(MNGList.Copy)
    SetTitle("Adding Windows Updates", "Integrating update data into WIM packages...")
    Dim NoMount As Boolean = True
    If UpdateFiles.Count > 0 Then
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetStatus("Integrating Updates...")
      If IntegrateFiles(WIMFile, UpdateFiles.ToArray, RemoveFiles.ToArray, iTotalVal, iTotalMax) Then
        SetStatus("Updates Integrated!")
        NoMount = False
      Else
        Dim sMsg As String = GetStatus()
        ToggleInputs(True, sMsg)
        Return
      End If
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return
      End If
    End If
    If chkUEFI.Checked And NoMount Then
      SetStatus("Mounting Final WIM for UEFI boot data...")
      InitDISM(WIMFile, GetDISMPackages(WIMFile), Mount)
      NoMount = False
    End If
    SetProgress(0, 1)
    iTotalVal += 1
    SetTotal(iTotalVal, iTotalMax)
    SetDisp(MNGList.Move)
    If Not String.IsNullOrEmpty(ISOFile) Then
      SetTitle("Generating ISO", "Preparing WIMs and file structures, and writing ISO data...")
      Dim ISODir As String = Work & "ISO" & IO.Path.DirectorySeparatorChar
      Dim ISODDir As String = Work & "ISOD%n" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(ISODir) Then IO.Directory.CreateDirectory(ISODir)
      SetProgress(0, 1)
      SetStatus("Extracting ISO contents...")
      WriteToOutput("Extracting Setup Disc files from """ & ISOFile & """ to """ & ISODir & """...")
      ExtractFiles(ISOFile, ISODir, "install.wim")
      If chkUnlock.Checked Then
        SetStatus("Unlocking All Editions...")
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "ei.cfg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "ei.cfg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "ei.cfg")
        End If
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 STARTER.clg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 STARTER.clg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 STARTER.clg")
        End If
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEBASIC.clg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEBASIC.clg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEBASIC.clg")
        End If
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEPREMIUM.clg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEPREMIUM.clg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 HOMEPREMIUM.clg")
        End If
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 PROFESSIONAL.clg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 PROFESSIONAL.clg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 PROFESSIONAL.clg")
        End If
        If IO.File.Exists(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 ULTIMATE.clg") Then
          WriteToOutput("Deleting """ & ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 ULTIMATE.clg""...")
          IO.File.Delete(ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install_Windows 7 ULTIMATE.clg")
        End If
      End If
      If IO.Directory.Exists(ISODir & "[BOOT]") Then
        WriteToOutput("Deleting """ & ISODir & "[BOOT]""...")
        Try
          SlowDeleteDirectory(ISODir & "[BOOT]", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
          WriteToOutput("Failed to delete """ & ISODir & "[BOOT]""!")
        End Try
      End If
      If LangChange Then
        SetProgress(0, 0)
        SetStatus("Updating Language INIs...")
        SetProgress(0, 1)
        If Not UpdateLang(ISODir) Then
          Dim sMsg As String = GetStatus()
          If Not NoMount Then DiscardDISM(Mount)
          ToggleInputs(True, sMsg)
          Return
        End If
        SetStatus("Updated Language INIs!")
        If StopRun Then
          If Not NoMount Then DiscardDISM(Mount)
          ToggleInputs(True)
          Return
        End If
      End If
      If chkUEFI.Checked Then
        SetStatus("Enabling UEFI Boot...")
        Dim sEFIBootDir As String = ISODir & IO.Path.DirectorySeparatorChar & "efi" & IO.Path.DirectorySeparatorChar & "microsoft" & IO.Path.DirectorySeparatorChar & "boot" & IO.Path.DirectorySeparatorChar
        Dim sUEFIBootDir As String = ISODir & IO.Path.DirectorySeparatorChar & "efi" & IO.Path.DirectorySeparatorChar & "boot" & IO.Path.DirectorySeparatorChar
        Dim sEFIFile As String = Mount & IO.Path.DirectorySeparatorChar & "Windows" & IO.Path.DirectorySeparatorChar & "Boot" & IO.Path.DirectorySeparatorChar & "EFI" & IO.Path.DirectorySeparatorChar & "bootmgfw.efi"
        Dim sUEFIFile As String = sUEFIBootDir & "bootx64.efi"
        If IO.Directory.Exists(sEFIBootDir) Then
          If Not IO.Directory.Exists(sUEFIBootDir) Then
            WriteToOutput("Copying """ & sEFIBootDir & """ to """ & sUEFIBootDir & """...")
            My.Computer.FileSystem.CopyDirectory(sEFIBootDir, sUEFIBootDir, True)
            If IO.File.Exists(sEFIFile) Then
              WriteToOutput("Copying """ & sEFIFile & """ to """ & sUEFIFile & """...")
              If SlowCopyFile(sEFIFile, sUEFIFile, False) Then
                SetStatus("UEFI Boot Enabled!")
              Else
                SetStatus("Failed to copy UEFI file!")
              End If
            Else
              WriteToOutput("Deleting """ & sUEFIBootDir & """...")
              SlowDeleteDirectory(sUEFIBootDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
              chkUEFI.Checked = False
              MsgDlg(Me, "Unable to find the file """ & IO.Path.DirectorySeparatorChar & "Windows" & IO.Path.DirectorySeparatorChar & "Boot" & IO.Path.DirectorySeparatorChar & "EFI" & IO.Path.DirectorySeparatorChar & "bootmgfw.efi"" in Image!", "Unable to enable UEFI Boot.", "File was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , , "EFI File Not Found")
            End If
          Else
            chkUEFI.Checked = False
            SetStatus("UEFI Boot already enabled!")
          End If
        Else
          chkUEFI.Checked = False
          MsgDlg(Me, "Unable to find the folder """ & IO.Path.DirectorySeparatorChar & "efi" & IO.Path.DirectorySeparatorChar & "microsoft" & IO.Path.DirectorySeparatorChar & "boot" & IO.Path.DirectorySeparatorChar & """ in ISO!", "Unable to enable UEFI Boot.", "Folder was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , , "EFI Folder Not Found")
        End If
      End If
      If Not NoMount Then
        SetProgress(0, 1)
        SetStatus("Saving Final Image Package...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Final Image Package!")
          Return
        End If
      End If
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetStatus("Integrating and Compressing INSTALL.WIM...")
      Dim ISOWIMFile As String = ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install.wim"
      If IO.File.Exists(ISOWIMFile) Then
        WriteToOutput("Deleting """ & ISOWIMFile & """...")
        IO.File.Delete(ISOWIMFile)
      End If
      Dim NewWIMPackageCount As Integer = GetDISMPackages(WIMFile)
      SetProgress(0, NewWIMPackageCount)
      For I As Integer = 1 To NewWIMPackageCount
        Dim NewWIMPackageInfo = GetDISMPackageData(WIMFile, I)
        Dim RowIndex As String = NewWIMPackageInfo.Index
        Dim RowName As String = NewWIMPackageInfo.Name
        SetProgress(I, NewWIMPackageCount, True)
        SetStatus("Integrating and Compressing INSTALL.WIM Package """ & RowName & """...")
        If ExportWIM(WIMFile, RowIndex, ISOWIMFile, RowName) Then
          Continue For
        Else
          ToggleInputs(True, "Failed to Integrate WIM Package """ & RowName & """")
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      Next
      If cmbLimitType.SelectedIndex > 0 Then
        SetProgress(0, 1)
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        Dim splUEFI As Boolean = chkUEFI.Checked
        Dim limVal As String = cmbLimit.Text
        If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
        Dim splFileSize As Long = NumericVal(limVal)
        Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(ISOWIMFile).Length
        Dim splISOSize As Long = splWIMSize + 351
        If cmbLimitType.SelectedIndex = 2 Then
          If splISOSize > splFileSize Then
            Dim ISOSplit As Long = splFileSize
            Dim WIMSplit As Long = ISOSplit - 351
            If splUEFI And (WIMSplit > 4095) Then WIMSplit = 4095
            If Math.Floor(ISOSplit / WIMSplit) > 1 Then
              REM Split WIM to WIMSplit size, put 1 WIM on first, and Math.Floor(ISOSplit / WIMSplit) per ISO afterward
              SetProgress(0, 0)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                If IO.File.Exists(ISOWIMFile) Then
                  WriteToOutput("Deleting """ & ISOWIMFile & """...")
                  IO.File.Delete(ISOWIMFile)
                End If
                SetProgress(0, 1)
                SetStatus("Generating Data ISOs...")
                Dim DiscNumber As Integer = 1
                Dim WIMNumber As Integer = 0
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources" & IO.Path.DirectorySeparatorChar, FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                For I As Integer = 0 To FilesInOrder.Count - 1
                  Dim File As String = FilesInOrder(I)
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Continue For
                  ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                    WIMNumber += 1
                    Dim sIDir As String = ISODDir.Replace("%n", DiscNumber)
                    If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                    If Not IO.Directory.Exists(sIDir & "sources" & IO.Path.DirectorySeparatorChar) Then IO.Directory.CreateDirectory(sIDir & "sources" & IO.Path.DirectorySeparatorChar)
                    SetProgress(0, 1)
                    Dim sNewFile As String = sIDir & IO.Path.DirectorySeparatorChar & "sources" & IO.Path.DirectorySeparatorChar & IO.Path.GetFileName(File)
                    WriteToOutput("Copying """ & File & """ to """ & sNewFile & """...")
                    If Not SlowCopyFile(File, sNewFile, True) Then
                      ToggleInputs(True, "Failed to move Install WIM #" & WIMNumber & "!")
                      Return
                    End If
                    If WIMNumber = Math.Floor(ISOSplit / WIMSplit) Or I = FilesInOrder.Count - 1 Then
                      WriteToOutput("Extracting Files from """ & Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip"" to """ & sIDir & """...")
                      ExtractAllFiles(Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip", sIDir)
                      If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                        SetStatus("Looking for oversized files...")
                        Dim BigList = FindBigFiles(sIDir)
                        If BigList.Count > 0 Then
                          Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
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
                                ToggleInputs(True, "ISO creation cancelled by user!")
                                Return
                            End Select
                          End Using
                        End If
                      End If
                      Dim DiscIDNo As String = DiscNumber
                      Dim DiscFiles() As String = My.Computer.FileSystem.GetFiles(sIDir & IO.Path.DirectorySeparatorChar & "sources" & IO.Path.DirectorySeparatorChar, FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                      SortFiles(DiscFiles)
                      DiscIDNo = NumericVal(IO.Path.GetFileNameWithoutExtension(DiscFiles(0)))
                      Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & DiscIDNo & IO.Path.GetExtension(ISOFile)
                      SetStatus("Building Data ISO " & DiscNumber & " (Labeled as Installation disc " & DiscIDNo & ")...")
                      Try
                        My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                        Dim isoLabel As String = txtISOLabel.Text
                        If isoLabel.Length > 32 - (5 + DiscIDNo.ToString.Length) Then
                          isoLabel = isoLabel.Substring(0, 32 - (5 + DiscIDNo.ToString.Length)) & "_DISC" & DiscIDNo
                        Else
                          isoLabel &= "_DISC" & DiscIDNo
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
                        MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                      Catch ex As Exception
                      End Try
                      DiscNumber += 1
                      WIMNumber = 0
                    End If
                  End If
                Next
              Else
                ToggleInputs(True, "Failed to Split WIM!")
                Return
              End If
            ElseIf Math.Floor(ISOSplit / WIMSplit) = 1 Then
              REM Split WIM to WIMSplit size, put 1 WIM on each ISO including first
              SetProgress(0, 0)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                If IO.File.Exists(ISOWIMFile) Then
                  WriteToOutput("Deleting """ & ISOWIMFile & """...")
                  IO.File.Delete(ISOWIMFile)
                End If
                SetProgress(0, 1)
                SetStatus("Generating Data ISOs...")
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources" & IO.Path.DirectorySeparatorChar, FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
                For Each File As String In FilesInOrder
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Continue For
                  ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                    Dim discNo As String = IO.Path.GetFileNameWithoutExtension(File).Substring(7)
                    Dim sIDir As String = ISODDir.Replace("%n", discNo)
                    If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                    If Not IO.Directory.Exists(sIDir & "sources" & IO.Path.DirectorySeparatorChar) Then IO.Directory.CreateDirectory(sIDir & "sources" & IO.Path.DirectorySeparatorChar)
                    SetProgress(0, 1)
                    Dim sNewFile As String = sIDir & IO.Path.DirectorySeparatorChar & "sources" & IO.Path.DirectorySeparatorChar & IO.Path.GetFileName(File)
                    WriteToOutput("Copying """ & File & """ to """ & sNewFile & """...")
                    If Not SlowCopyFile(File, sNewFile, True) Then
                      ToggleInputs(True, "Failed to move Install WIM #" & discNo & "!")
                      Return
                    End If
                    WriteToOutput("Extracting Files from """ & Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip"" to """ & sIDir & """...")
                    ExtractAllFiles(Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
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
                              ToggleInputs(True, "ISO creation cancelled by user!")
                              Return
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & discNo & IO.Path.GetExtension(ISOFile)
                    SetStatus("Building Data ISO " & discNo & "...")
                    Try
                      My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                      Dim isoLabel As String = txtISOLabel.Text
                      If isoLabel.Length > 32 - (5 + discNo.ToString.Length) Then
                        isoLabel = isoLabel.Substring(0, 32 - (5 + discNo.ToString.Length)) & "_DISC" & discNo
                      Else
                        isoLabel &= "_DISC" & discNo
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
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                    Catch ex As Exception
                    End Try
                  End If
                Next
              Else
                ToggleInputs(True, "Failed to Split WIM!")
                Return
              End If
            Else
              REM Split WIM to WIMSplit size, put no WIMs on first ISO, one per ISO afterward
              SetProgress(0, 0)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                If IO.File.Exists(ISOWIMFile) Then
                  WriteToOutput("Deleting """ & ISOWIMFile & """...")
                  IO.File.Delete(ISOWIMFile)
                End If
                SetProgress(0, 1)
                SetStatus("Generating Data ISOs...")
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources" & IO.Path.DirectorySeparatorChar, FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
                For Each File As String In FilesInOrder
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Dim sIDir As String = ISODDir.Replace("%n", "1")
                    If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                    If Not IO.Directory.Exists(sIDir & "sources" & IO.Path.DirectorySeparatorChar) Then IO.Directory.CreateDirectory(sIDir & "sources" & IO.Path.DirectorySeparatorChar)
                    SetProgress(0, 1)
                    Dim sNewFile As String = sIDir & IO.Path.DirectorySeparatorChar & "sources" & IO.Path.DirectorySeparatorChar & "install.swm"
                    WriteToOutput("Copying """ & File & """ to """ & sNewFile & """...")
                    If Not SlowCopyFile(File, sNewFile, True) Then
                      ToggleInputs(True, "Failed to move Primary Install WIM!")
                      Return
                    End If
                    WriteToOutput("Extracting Files from """ & Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip"" to """ & sIDir & """...")
                    ExtractAllFiles(Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
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
                              ToggleInputs(True, "ISO creation cancelled by user!")
                              Return
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D1" & IO.Path.GetExtension(ISOFile)
                    SetStatus("Building Data ISO 1...")
                    Try
                      My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                      Dim isoLabel As String = txtISOLabel.Text
                      If isoLabel.Length > 32 - 6 Then
                        isoLabel = isoLabel.Substring(0, 32 - 6) & "_DISC1"
                      Else
                        isoLabel &= "_DISC1"
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
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                    Catch ex As Exception
                    End Try
                  ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                    Dim discNo As String = IO.Path.GetFileNameWithoutExtension(File).Substring(7)
                    Dim sIDir As String = ISODDir.Replace("%n", discNo)
                    If Not IO.Directory.Exists(sIDir) Then IO.Directory.CreateDirectory(sIDir)
                    If Not IO.Directory.Exists(sIDir & "sources" & IO.Path.DirectorySeparatorChar) Then IO.Directory.CreateDirectory(sIDir & "sources" & IO.Path.DirectorySeparatorChar)
                    SetProgress(0, 1)
                    Dim sNewFile As String = sIDir & IO.Path.DirectorySeparatorChar & "sources" & IO.Path.DirectorySeparatorChar & IO.Path.GetFileName(File)
                    WriteToOutput("Copying """ & File & """ to """ & sNewFile & """...")
                    If Not SlowCopyFile(File, sNewFile, True) Then
                      ToggleInputs(True, "Failed to move Install WIM #" & discNo & "!")
                      Return
                    End If
                    WriteToOutput("Extracting Files from """ & Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip"" to """ & sIDir & """...")
                    ExtractAllFiles(Application.StartupPath & IO.Path.DirectorySeparatorChar & "AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
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
                              ToggleInputs(True, "ISO creation cancelled by user!")
                              Return
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & discNo & IO.Path.GetExtension(ISOFile)
                    SetStatus("Building Data ISO " & discNo & "...")
                    Try
                      My.Computer.FileSystem.WriteAllBytes(ISODFile, {0, 0}, False)
                      Dim isoLabel As String = txtISOLabel.Text
                      If isoLabel.Length > 32 - (5 + discNo.Length) Then
                        isoLabel = isoLabel.Substring(0, 32 - (5 + discNo.Length)) & "_DISC" & discNo
                      Else
                        isoLabel &= "_DISC" & discNo
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
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                    Catch ex As Exception
                    End Try
                  End If
                Next
              Else
                ToggleInputs(True, "Failed to Split WIM!")
                Return
              End If
            End If
          ElseIf splUEFI And splWIMSize > 4095 Then
            REM Split WIM to 4095 MB size
            SetProgress(0, 0)
            SetStatus("Splitting WIM into 4 GB Chunks...")
            If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), 4095) Then
              If IO.File.Exists(ISOWIMFile) Then
                WriteToOutput("Deleting """ & ISOWIMFile & """...")
                IO.File.Delete(ISOWIMFile)
              End If
            Else
              ToggleInputs(True, "Failed to Split WIM!")
              Return
            End If
          End If
        Else
          If splWIMSize > splFileSize Then
            REM Split WIM to FileSize size
            SetProgress(0, 0)
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), splFileSize) Then
              If IO.File.Exists(ISOWIMFile) Then
                WriteToOutput("Deleting """ & ISOWIMFile & """...")
                IO.File.Delete(ISOWIMFile)
              End If
            Else
              ToggleInputs(True, "Failed to Split WIM!")
              Return
            End If
          End If
        End If
      Else
        If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
          SetStatus("Looking for oversized files...")
          Dim BigList = FindBigFiles(ISODir)
          If BigList.Count > 0 Then
            Using fsAlert As New frmFS(BigList, ISODir, cmbISOFormat.SelectedIndex)
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
                  ToggleInputs(True, "ISO creation cancelled by user!")
                  Return
              End Select
            End Using
          End If
        End If
      End If
      Dim BootOrder As String = Work & "bootorder.txt"
      My.Computer.FileSystem.WriteAllText(BootOrder, "boot" & IO.Path.DirectorySeparatorChar & "bcd" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "boot.sdi" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "bootfix.bin" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "bootsect.exe" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "etfsboot.com" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "memtest.efi" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "memtest.exe" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "en-us" & IO.Path.DirectorySeparatorChar & "bootsect.exe.mui" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "fonts" & IO.Path.DirectorySeparatorChar & "chs_boot.ttf" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "fonts" & IO.Path.DirectorySeparatorChar & "cht_boot.ttf" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "fonts" & IO.Path.DirectorySeparatorChar & "jpn_boot.ttf" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "fonts" & IO.Path.DirectorySeparatorChar & "kor_boot.ttf" & vbNewLine &
                                          "boot" & IO.Path.DirectorySeparatorChar & "fonts" & IO.Path.DirectorySeparatorChar & "wgl4_boot.ttf" & vbNewLine &
                                          "sources" & IO.Path.DirectorySeparatorChar & "boot.wim" & vbNewLine, False, System.Text.Encoding.GetEncoding(1252))
      SetProgress(0, 1)
      SetStatus("Making Backup of Old ISO...")
      WriteToOutput("Copying """ & ISOFile & """ to """ & ISOFile & ".del""...")
      If Not SlowCopyFile(ISOFile, ISOFile & ".del", True) Then
        ToggleInputs(True, "Failed to back up ISO!")
        Return
      End If
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)
      SetProgress(0, 0)
      SetStatus("Building New ISO...")
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
        SetProgress(0, 1)
        Saved = MakeISO(ISODir, isoLabel, BootOrder, isoFormat, ISOFile)
      Catch ex As Exception
        Saved = False
      End Try
      SetProgress(0, 1)
      If Saved Then
        If IO.File.Exists(ISOFile & ".del") Then
          WriteToOutput("Deleting """ & ISOFile & ".del""...")
          IO.File.Delete(ISOFile & ".del")
        End If
      Else
        SetStatus("ISO Build Failed! Restoring Old ISO...")
        WriteToOutput("Copying """ & ISOFile & ".del"" to """ & ISOFile & """...")
        If Not SlowCopyFile(ISOFile & ".del", ISOFile, True) Then
          ToggleInputs(True, "ISO Build and backup ISO restore failed!")
        End If
        Return
      End If
    Else
      SetTitle("Generating WIM", "Preparing WIMs and file structure...")
      If Not NoMount Then
        SetProgress(0, 1)
        SetStatus("Saving Final Image Package...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Final Image Package!")
          Return
        End If
      End If
      SetStatus("Compressing INSTALL.WIM...")
      Dim OldWIM As String = IO.Path.GetDirectoryName(WIMFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(WIMFile & "_OLD.WIM")
      SetProgress(0, 1)
      WriteToOutput("Copying """ & WIMFile & """ to """ & OldWIM & """...")
      If Not SlowCopyFile(WIMFile, OldWIM, True) Then
        ToggleInputs(True, "Failed to back up Install WIM!")
        Return
      End If
      Dim NewWIMPackageCount As Integer = GetDISMPackages(OldWIM)
      SetProgress(0, NewWIMPackageCount)
      For I As Integer = 1 To NewWIMPackageCount
        Dim NewWIMPackageInfo As ImagePackage = GetDISMPackageData(OldWIM, I)
        Dim RowIndex As String = NewWIMPackageInfo.Index
        Dim RowName As String = NewWIMPackageInfo.Name
        SetProgress(I, NewWIMPackageCount, True)
        If ExportWIM(OldWIM, RowIndex, WIMFile, RowName) Then
          Continue For
        Else
          ToggleInputs(True, "Failed to Compress WIM """ & RowName & """")
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      Next
      WriteToOutput("Deleting """ & OldWIM & """...")
      IO.File.Delete(OldWIM)
      If cmbLimitType.SelectedIndex > 0 Then
        If IO.File.Exists(WIMFile) Then
          Dim limVal As String = cmbLimit.Text
          If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
          Dim splFileSize As Long = NumericVal(limVal)
          Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(WIMFile).Length
          If splWIMSize > splFileSize Then
            REM Split WIMs to FileSize size
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            If SplitWIM(WIMFile, IO.Path.ChangeExtension(WIMFile, ".swm"), splFileSize) Then
              WriteToOutput("Deleting """ & WIMFile & """...")
              IO.File.Delete(WIMFile)
            Else
              ToggleInputs(True, "Failed to Split WIM!")
              Return
            End If
          End If
        End If
      End If
    End If
    SetProgress(0, 100)
    SetTotal(0, 100)
    RunComplete = True
    ToggleInputs(True, "Complete!")
    Select Case cmbCompletion.SelectedIndex
      Case 0
        REM done
      Case 1
        REM play alert noise
        If String.IsNullOrEmpty(mySettings.AlertNoisePath) Then
          Dim tada As String = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Media\tada.wav"
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
            Dim tada As String = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Media\tada.wav"
            If IO.File.Exists(tada) Then
              My.Computer.Audio.Play(tada, AudioPlayMode.Background)
            Else
              My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
            End If
          End Try
        Else
          mySettings.AlertNoisePath = String.Empty
          Dim tada As String = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Media\tada.wav"
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
        CloseCleanup()
        Process.Start("shutdown", "/s /t 0 /d p:0:0 /f")
      Case 4
        REM restart
        CloseCleanup()
        Process.Start("shutdown", "/r /t 0 /d p:0:0 /f")
      Case 5
        REM sleep
        CloseCleanup()
        Application.SetSuspendState(PowerState.Suspend, False, False)
        Me.Close()
    End Select
  End Sub
  Private Function IntegrateFiles(WIMPath As String, MSUData() As Update_File, Remove()() As Update_Integrated, ByRef iTotalVal As Integer, iTotalMax As Integer) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount < 1 Then
      ToggleInputs(True, "No packages in WIM!")
      Return False
    End If
    If MSUData.Length < 1 Then Return True
    Dim DISM_32 As New List(Of ImagePackage)
    Dim DISM_64 As New List(Of ImagePackage)
    Dim MSU_32 As New List(Of Update_File)
    Dim MSU_64 As New List(Of Update_File)
    Dim RMV_32 As New List(Of Update_Integrated)
    Dim RMV_64 As New List(Of Update_Integrated)
    Dim pbMax As Integer = PackageCount + MSUData.Length
    Dim pbVal As Integer = 0
    SetProgress(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      SetProgress(pbVal, pbMax)
      SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return False
      End If
      Dim tmpDISM As ImagePackage = GetDISMPackageData(WIMPath, I)
      If tmpDISM.Architecture = "x86" Then
        DISM_32.Add(tmpDISM)
      ElseIf tmpDISM.Architecture = "x64" Then
        DISM_64.Add(tmpDISM)
      End If
      If Remove.Length > 0 Then
        For R As Integer = 0 To Remove.Length - 1
          If Not String.IsNullOrEmpty(Remove(R)(I - 1).Identity) Then
            If tmpDISM.Architecture = "x86" Then
              RMV_32.Add(Remove(R)(I - 1))
            ElseIf tmpDISM.Architecture = "x64" Then
              RMV_64.Add(Remove(R)(I - 1))
            End If
          End If
        Next
      End If
    Next
    For I As Integer = 0 To MSUData.Length - 1
      pbVal += 1
      SetProgress(pbVal, pbMax)
      SetStatus("Loading Update """ & IO.Path.GetFileNameWithoutExtension(MSUData(I).Path) & """ Data...")
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return False
      End If
      If MSUData(I).Name = "DRIVER" Then
        If MSUData(I).DriverData.Architectures Is Nothing OrElse MSUData(I).DriverData.Architectures.Count = 0 Then Continue For
        If DISM_32.Count > 0 And MSUData(I).DriverData.Architectures.Contains("x86") Then
          MSU_32.Add(MSUData(I))
        End If
        If DISM_64.Count > 0 And MSUData(I).DriverData.Architectures.Contains("amd64") Then
          MSU_64.Add(MSUData(I))
        End If
      Else
        If Not String.IsNullOrEmpty(MSUData(I).Failure) Then Continue For
        If String.IsNullOrEmpty(MSUData(I).Architecture) Then Continue For
        If DISM_32.Count > 0 And MSUData(I).Architecture.ToLower = "x86" Then
          MSU_32.Add(MSUData(I))
        End If
        If DISM_64.Count > 0 Then
          If MSUData(I).Architecture.ToLower = "amd64" Then
            MSU_64.Add(MSUData(I))
          Else
            If CheckWhitelist(MSUData(I).DisplayName) Then MSU_64.Add(MSUData(I))
          End If
        End If
      End If
    Next
    SortForRequirement(MSU_32)
    SortForRequirement(MSU_64)
    SetProgress(0, 0)
    pbMax = 1
    pbVal = 0
    If MSU_32.Count > 0 Then
      For Each tmpDISM In DISM_32
        pbMax += MSU_32.Count + 2
        pbMax += RMV_32.Count
      Next
    End If
    If MSU_64.Count > 0 Then
      For Each tmpDISM In DISM_64
        pbMax += MSU_64.Count + 2
        pbMax += RMV_64.Count
      Next
    End If
    SetProgress(0, pbMax)
    iTotalVal += 1
    SetTotal(iTotalVal, iTotalMax)
    If MSU_32.Count > 0 Then
      For D As Integer = 0 To DISM_32.Count - 1
        Dim tmpDISM As ImagePackage = DISM_32(D)
        pbVal += 1
        SetProgress(pbVal, pbMax)
        SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
        If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        If RMV_32.Count > 0 Then
          For I As Integer = 0 To RMV_32.Count - 1
            pbVal += 1
            SetProgress(pbVal, pbMax)
            If RMV_32(I).Parent.Index = tmpDISM.Index Then
              SetStatus((I + 1).ToString.Trim & "/" & RMV_32.Count.ToString & " - Removing " & GetUpdateName(RMV_32(I).Ident) & " from " & tmpDISM.Name & "...")
              If Not RemovePackageItemFromDISM(Mount, RMV_32(I).Identity) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to remove " & GetUpdateName(RMV_32(I).Ident) & " from " & tmpDISM.Name & "!")
                Return False
              End If
            End If
          Next
        End If
        For I As Integer = 0 To MSU_32.Count - 1
          Dim tmpMSU = MSU_32(I)
          pbVal += 1
          SetProgress(pbVal, pbMax)
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
            shownName = "KB" & tmpMSU.KBArticle
          End If
          SetStatus((I + 1).ToString.Trim & "/" & MSU_32.Count.ToString & " - Integrating " & shownName & " into " & tmpDISM.Name & "...")
          Dim upType = GetUpdateType(tmpMSU.Path)
          Select Case upType
            Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
              If Not AddPackageItemToDISM(Mount, tmpMSU.Path) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If upType = UpdateType.LP Then LangChange = True
            Case UpdateType.EXE
              Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tmpMSU.Path)
              If fInfo.OriginalFilename = "iesetup.exe" And fInfo.ProductMajorPart > 9 Then
                Dim EXEPath As String = WorkDir & "UpdateEXE_Extract" & IO.Path.DirectorySeparatorChar
                If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                IO.Directory.CreateDirectory(EXEPath)
                Dim iExtract As New Process With {.StartInfo = New ProcessStartInfo(tmpMSU.Path, "/x:" & EXEPath)}
                If iExtract.Start Then
                  iExtract.WaitForExit()
                  Dim bFound As Boolean = False
                  Dim Extracted() As String = IO.Directory.GetFiles(EXEPath)
                  For J As Integer = 0 To Extracted.Count - 1
                    If tmpMSU.Identity = New Update_File(Extracted(J)).Identity Then
                      bFound = True
                      If Not AddPackageItemToDISM(Mount, Extracted(J)) Then
                        DiscardDISM(Mount)
                        ToggleInputs(True, "Failed to integrate " & shownName & " (" & IO.Path.GetFileName(Extracted(J)) & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      Exit For
                    End If
                  Next
                  If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                  If Not bFound Then
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                Else
                  If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                  Return False
                End If
              ElseIf fInfo.OriginalFilename = "mergedwusetup.exe" Then
                Dim tmpCAB As String = WorkDir & "wusetup.cab"
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to extract " & shownName & " from EXE to CAB!")
                  Return False
                End If
                Dim cabList() As String = ExtractFilesList(tmpCAB)
                If cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                  Dim useEXE As String = "WUA-Downlevel.exe"
                  If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                  ExtractAFile(tmpCAB, WorkDir, useEXE)
                  If Not IO.File.Exists(WorkDir & useEXE) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  Else
                    Dim useCab1 As String = "WUClient-SelfUpdate-ActiveX.cab"
                    Dim useCab2 As String = "WUClient-SelfUpdate-Aux-TopLevel.cab"
                    Dim useCab3 As String = "WUClient-SelfUpdate-Core-TopLevel.cab"
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab1)
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab2)
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab3)
                    If Not IO.File.Exists(WorkDir & useCab1) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not IO.File.Exists(WorkDir & useCab2) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not IO.File.Exists(WorkDir & useCab3) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab1) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab2) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab3) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                End If
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
              Else
                Dim tmpCAB As String = WorkDir & "lp.cab"
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to extract " & shownName & " from EXE to CAB!")
                  Return False
                End If
                Dim cabList() As String = ExtractFilesList(tmpCAB)
                If cabList.Contains("update.mum") Then
                  If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                End If
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                LangChange = True
              End If
            Case UpdateType.LIP
              Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
              If IO.File.Exists(tmpCAB) Then
                WriteToOutput("Deleting """ & tmpCAB & """...")
                IO.File.Delete(tmpCAB)
              End If
              WriteToOutput("Copying """ & tmpMSU.Path & """ to """ & tmpCAB & """...")
              My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
              If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If IO.File.Exists(tmpCAB) Then
                WriteToOutput("Deleting """ & tmpCAB & """...")
                IO.File.Delete(tmpCAB)
              End If
              LangChange = True
            Case UpdateType.MSI
              Dim MSIPath As String = WorkDir & "UpdateMSI_Extract" & IO.Path.DirectorySeparatorChar
              If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              IO.Directory.CreateDirectory(MSIPath)
              Dim tmpCAB As String = MSIPath & "LIP.cab"
              Dim tmpMLC As String = MSIPath & "LIP.mlc"
              ExtractAFile(tmpMSU.Path, MSIPath, "LIP.cab")
              If IO.File.Exists(tmpCAB) Then
                ExtractAFile(tmpCAB, MSIPath, "LIP.mlc")
                If IO.File.Exists(tmpMLC) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                  WriteToOutput("Renaming """ & tmpMLC & """ as """ & tmpCAB & """...")
                  Rename(tmpMLC, tmpCAB)
                  If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
                Else
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                  Return False
                End If
              Else
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              LangChange = True
            Case UpdateType.INF
              If Not AddDriverToDISM(Mount, tmpMSU.Path, False, True) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
          End Select
          SetProgress(pbVal, pbMax)
          If StopRun Then
            DiscardDISM(Mount)
            ToggleInputs(True)
            Return False
          End If
        Next
        pbVal += 1
        SetProgress(pbVal, pbMax)
        Dim DoSave As Boolean = True
        If DISM_64.Count = 0 Then
          If D = DISM_32.Count - 1 Then
            DoSave = False
          End If
        End If
        If DoSave Then
          SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
          If Not SaveDISM(Mount) Then
            DiscardDISM(Mount)
            ToggleInputs(True, "Failed to Save Image Package """ & tmpDISM.Name & """!")
            Return False
          End If
        End If
        If StopRun Then
          ToggleInputs(True)
          Return False
        End If
      Next
    End If
    If MSU_64.Count > 0 Then
      For D As Integer = 0 To DISM_64.Count - 1
        Dim tmpDISM As ImagePackage = DISM_64(D)
        pbVal += 1
        SetProgress(pbVal, pbMax)
        SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
        If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        If RMV_64.Count > 0 Then
          For I As Integer = 0 To RMV_64.Count - 1
            pbVal += 1
            SetProgress(pbVal, pbMax)
            If RMV_64(I).Parent.Index = tmpDISM.Index Then
              SetStatus((I + 1).ToString.Trim & "/" & RMV_64.Count.ToString & " - Removing " & RMV_64(I).Ident.Name & " from " & tmpDISM.Name & "...")
              If Not RemovePackageItemFromDISM(Mount, RMV_64(I).Identity) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to remove " & RMV_64(I).Ident.Name & " from " & tmpDISM.Name & "!")
                Return False
              End If
            End If
          Next
        End If
        For I As Integer = 0 To MSU_64.Count - 1
          Dim tmpMSU = MSU_64(I)
          pbVal += 1
          SetProgress(pbVal, pbMax)
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
            shownName = "KB" & tmpMSU.KBArticle
          End If
          SetStatus((I + 1).ToString.Trim & "/" & MSU_64.Count.ToString & " - Integrating " & shownName & " into " & tmpDISM.Name & "...")
          Dim upType = GetUpdateType(tmpMSU.Path)
          Select Case upType
            Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
              If Not AddPackageItemToDISM(Mount, tmpMSU.Path) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If upType = UpdateType.LP Then LangChange = True
            Case UpdateType.EXE
              Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(tmpMSU.Path)
              If fInfo.OriginalFilename = "iesetup.exe" And fInfo.ProductMajorPart > 9 Then
                Dim EXEPath As String = WorkDir & "UpdateEXE_Extract" & IO.Path.DirectorySeparatorChar
                If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                IO.Directory.CreateDirectory(EXEPath)
                Dim iExtract As New Process With {.StartInfo = New ProcessStartInfo(tmpMSU.Path, "/x:" & EXEPath)}
                If iExtract.Start Then
                  iExtract.WaitForExit()
                  Dim bFound As Boolean = False
                  Dim Extracted() As String = IO.Directory.GetFiles(EXEPath)
                  For J As Integer = 0 To Extracted.Count - 1
                    If tmpMSU.Identity = New Update_File(Extracted(J)).Identity Then
                      bFound = True
                      If Not AddPackageItemToDISM(Mount, Extracted(J)) Then
                        DiscardDISM(Mount)
                        ToggleInputs(True, "Failed to integrate " & shownName & " (" & IO.Path.GetFileName(Extracted(J)) & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      Exit For
                    End If
                  Next
                  If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                  If Not bFound Then
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                Else
                  If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                  Return False
                End If
              ElseIf fInfo.OriginalFilename = "mergedwusetup.exe" Then
                Dim tmpCAB As String = WorkDir & "wusetup.cab"
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to extract " & shownName & " from EXE to CAB!")
                  Return False
                End If
                Dim cabList() As String = ExtractFilesList(tmpCAB)
                If cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                  Dim useEXE As String = "WUA-Downlevel.exe"
                  If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                  ExtractAFile(tmpCAB, WorkDir, useEXE)
                  If Not IO.File.Exists(WorkDir & useEXE) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  Else
                    Dim useCab1 As String = "WUClient-SelfUpdate-ActiveX.cab"
                    Dim useCab2 As String = "WUClient-SelfUpdate-Aux-TopLevel.cab"
                    Dim useCab3 As String = "WUClient-SelfUpdate-Core-TopLevel.cab"
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab1)
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab2)
                    ExtractAFile(WorkDir & useEXE, WorkDir, useCab3)
                    If Not IO.File.Exists(WorkDir & useCab1) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not IO.File.Exists(WorkDir & useCab2) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not IO.File.Exists(WorkDir & useCab3) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab1) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab2) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                    If Not AddPackageItemToDISM(Mount, WorkDir & useCab3) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      If IO.File.Exists(WorkDir & useEXE) Then
                        WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                        IO.File.Delete(WorkDir & useEXE)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True, "Failed to integrate " & shownName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                      Return False
                    End If
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                End If
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
              Else
                Dim tmpCAB As String = WorkDir & "lp.cab"
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to extract " & shownName & " from EXE to CAB!")
                  Return False
                End If
                Dim cabList() As String = ExtractFilesList(tmpCAB)
                If cabList.Contains("update.mum") Then
                  If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                Else
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                End If
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                LangChange = True
              End If
            Case UpdateType.LIP
              Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
              If IO.File.Exists(tmpCAB) Then
                WriteToOutput("Deleting """ & tmpCAB & """...")
                IO.File.Delete(tmpCAB)
              End If
              WriteToOutput("Copying """ & tmpMSU.Path & """ to """ & tmpCAB & """...")
              My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
              If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                If IO.File.Exists(tmpCAB) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                End If
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If IO.File.Exists(tmpCAB) Then
                WriteToOutput("Deleting """ & tmpCAB & """...")
                IO.File.Delete(tmpCAB)
              End If
              LangChange = True
            Case UpdateType.MSI
              Dim MSIPath As String = WorkDir & "UpdateMSI_Extract" & IO.Path.DirectorySeparatorChar
              If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              IO.Directory.CreateDirectory(MSIPath)
              Dim tmpCAB As String = MSIPath & "LIP.cab"
              Dim tmpMLC As String = MSIPath & "LIP.mlc"
              ExtractAFile(tmpMSU.Path, MSIPath, "LIP.cab")
              If IO.File.Exists(tmpCAB) Then
                ExtractAFile(tmpCAB, MSIPath, "LIP.mlc")
                If IO.File.Exists(tmpMLC) Then
                  WriteToOutput("Deleting """ & tmpCAB & """...")
                  IO.File.Delete(tmpCAB)
                  WriteToOutput("Renaming """ & tmpMLC & """ as """ & tmpCAB & """...")
                  Rename(tmpMLC, tmpCAB)
                  If Not AddPackageItemToDISM(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
                Else
                  DiscardDISM(Mount)
                  ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                  Return False
                End If
              Else
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
              If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
              LangChange = True
            Case UpdateType.INF
              If Not AddDriverToDISM(Mount, tmpMSU.Path, False, True) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to integrate " & shownName & " into " & tmpDISM.Name & "!")
                Return False
              End If
          End Select
          SetProgress(pbVal, pbMax)
          If StopRun Then
            DiscardDISM(Mount)
            ToggleInputs(True)
            Return False
          End If
        Next
        pbVal += 1
        SetProgress(pbVal, pbMax)
        Dim DoSave As Boolean = True
        If D = DISM_64.Count - 1 Then
          DoSave = False
        End If
        If DoSave Then
          SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
          If Not SaveDISM(Mount) Then
            DiscardDISM(Mount)
            ToggleInputs(True, "Failed to Save Image Package """ & tmpDISM.Name & """!")
            Return False
          End If
        End If
        If StopRun Then
          ToggleInputs(True)
          Return False
        End If
      Next
    End If
    Return True
  End Function
  Private Function IntegrateSP(WIMPath As String, SPPath As String, Optional Architecture As String = Nothing) As Boolean
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    SetProgress(0, 1)
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    Dim ActivePackages As Integer = 0
    Dim activeArch As String = Architecture
    If PackageCount > 0 Then
      For I As Integer = 1 To PackageCount
        Dim dismData As ImagePackage = GetDISMPackageData(WIMPath, I)
        Try
          If dismData.SPLevel > 0 Then Continue For
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not dismData.Architecture.Contains(Architecture) Then Continue For
        Catch ex As Exception
          Continue For
        End Try
        If String.IsNullOrEmpty(activeArch) Then activeArch = dismData.Architecture.Substring(1)
        ActivePackages += 1
      Next
    End If
    If ActivePackages = 0 Then Return True
    Dim pbVal As Integer = 0
    Dim pbMax As Integer = (ActivePackages * 3) + 14
    SetProgress(pbVal, pbMax)
    SetStatus("Extracting Service Pack...")
    RunHidden(SPPath, "/x:""" & Work & "SP1""")
    pbVal += 1
    SetProgress(pbVal, pbMax, True)
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    Dim Extract86 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "windows6.1-KB976932-X86.cab"
    Dim Extract64 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "windows6.1-KB976932-X64.cab"
    If IO.File.Exists(Extract86) Then
      SetStatus("Preparing Service Pack (Extracting KB976932.cab)...")
      WriteToOutput("Extracting """ & Extract86 & """ to """ & Work & "SP1""...")
      ExtractAllFiles(Extract86, Work & "SP1")
      WriteToOutput("Deleting """ & Extract86 & """...")
      IO.File.Delete(Extract86)
    ElseIf IO.File.Exists(Extract64) Then
      SetStatus("Preparing Service Pack (Extracting KB976932.cab)...")
      WriteToOutput("Extracting """ & Extract64 & """ to """ & Work & "SP1""...")
      ExtractAllFiles(Extract64, Work & "SP1")
      WriteToOutput("Deleting """ & Extract64 & """...")
      IO.File.Delete(Extract64)
    Else
      ToggleInputs(True, "No KB976932.cab to extract!")
      Return False
    End If
    pbVal += 1
    SetProgress(pbVal, pbMax, True)
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    Dim Extract As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "NestedMPPcontent.cab"
    If IO.File.Exists(Extract) Then
      SetStatus("Preparing Service Pack (Extracting NestedMPPcontent.cab)...")
      WriteToOutput("Extracting """ & Extract & """ to """ & Work & "SP1""...")
      ExtractAllFiles(Extract, Work & "SP1")
      WriteToOutput("Deleting """ & Extract & """...")
      IO.File.Delete(Extract)
    Else
      ToggleInputs(True, "No NestedMPPcontent.cab to extract!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    pbVal += 1
    SetProgress(pbVal, pbMax)
    Dim Update As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "update.ses"
    If IO.File.Exists(Update) Then
      SetStatus("Preparing Service Pack (Modifying update.ses)...")
      UpdateSES(Update)
    Else
      ToggleInputs(True, "No update.ses to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    pbVal += 1
    SetProgress(pbVal, pbMax)
    Update = Work & "SP1" & IO.Path.DirectorySeparatorChar & "update.mum"
    If IO.File.Exists(Update) Then
      SetStatus("Preparing Service Pack (Modifying update.mum)...")
      UpdateMUM(Update)
    Else
      ToggleInputs(True, "No update.mum to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    pbVal += 1
    SetProgress(pbVal, pbMax)
    Dim Update86 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "Windows7SP1-KB976933~31bf3856ad364e35~x86~~6.1.1.17514.mum"
    Dim Update64 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "Windows7SP1-KB976933~31bf3856ad364e35~amd64~~6.1.1.17514.mum"
    If IO.File.Exists(Update86) Then
      SetStatus("Preparing Service Pack (Modifying KB976933.mum)...")
      UpdateMUM(Update86)
    ElseIf IO.File.Exists(Update64) Then
      SetStatus("Preparing Service Pack (Modifying KB976933.mum)...")
      UpdateMUM(Update64)
    Else
      ToggleInputs(True, "No KB976933.mum to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    pbVal += 1
    SetProgress(pbVal, pbMax)
    Dim CABList As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "cabinet.cablist.ini"
    If IO.File.Exists(CABList) Then
      WriteToOutput("Deleting """ & CABList & """...")
      IO.File.Delete(CABList)
    End If
    CABList = Work & "SP1" & IO.Path.DirectorySeparatorChar & "old_cabinet.cablist.ini"
    If IO.File.Exists(CABList) Then
      WriteToOutput("Deleting """ & CABList & """...")
      IO.File.Delete(CABList)
    End If
    For I As Integer = 0 To 6
      pbVal += 1
      SetProgress(pbVal, pbMax, True)
      Extract = Work & "SP1" & IO.Path.DirectorySeparatorChar & "KB976933-LangsCab" & I.ToString.Trim & ".cab"
      If IO.File.Exists(Extract) Then
        SetStatus("Preparing Service Pack (Extracting Language CAB " & (I + 1).ToString.Trim & " of 7)...")
        WriteToOutput("Extracting """ & Extract & """ to """ & Work & "SP1""...")
        ExtractAllFiles(Extract, Work & "SP1")
        WriteToOutput("Deleting """ & Extract & """...")
        IO.File.Delete(Extract)
      Else
        ToggleInputs(True, "No KB976933-LangsCab" & I.ToString.Trim & ".cab to extract!")
        Return False
      End If
      If StopRun Then
        ToggleInputs(True)
        Return False
      End If
    Next
    pbVal += 1
    SetProgress(pbVal, pbMax)
    If PackageCount > 0 Then
      Dim iRunCount As Integer = 0
      For I As Integer = 1 To PackageCount
        Dim dismData As ImagePackage = GetDISMPackageData(WIMPath, I)
        Try
          If dismData.SPLevel > 0 Then Continue For
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not dismData.Architecture.Contains(Architecture) Then
            Continue For
          End If
        Catch ex As Exception
        End Try
        pbVal += 1
        SetProgress(pbVal, pbMax)
        SetStatus("Integrating Service Pack (Loading " & dismData.Name & ")...")
        If Not InitDISM(WIMPath, I, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        pbVal += 1
        SetProgress(pbVal, pbMax)
        SetStatus("Integrating Service Pack into " & dismData.Name & "...")
        If Not AddPackageItemToDISM(Mount, Work & "SP1") Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Add Service Pack to Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        pbVal += 1
        SetProgress(pbVal, pbMax)
        SetStatus("Integrating Service Pack (Saving " & dismData.Name & ")...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          ToggleInputs(True)
          Return False
        End If
      Next
    Else
      ToggleInputs(True, "No packages in WIM!")
      Return False
    End If
    SetProgress(0, pbMax)
    SetStatus("Clearing Temp Files...")
    WriteToOutput("Deleting """ & Work & "SP1""...")
    SlowDeleteDirectory(Work & "SP1", FileIO.DeleteDirectoryOption.DeleteAllContents)
    Return True
  End Function
  Private Function IntegratedFeatures(WIMPath As String, FeatureData() As List(Of Feature)) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount < 1 Then
      ToggleInputs(True, "No packages in WIM!")
      Return False
    End If
    If FeatureData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each FeatureList As List(Of Feature) In FeatureData
      If FeatureList IsNot Nothing Then pbMax += FeatureList.Count + 1
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    SetProgress(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      SetProgress(pbVal, pbMax)
      If FeatureData(I - 1) IsNot Nothing AndAlso FeatureData(I - 1).Count > 0 Then
        SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = GetDISMPackageData(WIMPath, I)
        If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        For J As Integer = 0 To FeatureData(I - 1).Count - 1
          pbVal += 1
          SetProgress(pbVal, pbMax)
          If FeatureData(I - 1)(J).Enable Then
            If Not (FeatureData(I - 1)(J).State = "Enabled" Or FeatureData(I - 1)(J).State = "Enable Pending") Then
              SetStatus((J + 1).ToString.Trim & "/" & FeatureData(I - 1).Count.ToString & " - Enabling " & FeatureData(I - 1)(J).DisplayName & " in " & tmpDISM.Name & "...")
              If Not EnableDISMFeature(Mount, FeatureData(I - 1)(J).FeatureName) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to eneable " & FeatureData(I - 1)(J).DisplayName & " in " & tmpDISM.Name & "!")
                Return False
              End If
            End If
          Else
            If (FeatureData(I - 1)(J).State = "Enabled" Or FeatureData(I - 1)(J).State = "Enable Pending") Then
              SetStatus((J + 1).ToString.Trim & "/" & FeatureData(I - 1).Count.ToString & " - Disabling " & FeatureData(I - 1)(J).DisplayName & " in " & tmpDISM.Name & "...")
              If Not DisableDISMFeature(Mount, FeatureData(I - 1)(J).FeatureName) Then
                DiscardDISM(Mount)
                ToggleInputs(True, "Failed to disable " & FeatureData(I - 1)(J).DisplayName & " in " & tmpDISM.Name & "!")
                Return False
              End If
            End If
          End If
        Next
        SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
  Private Function IntegratedUpdates(WIMPath As String, UpdateData() As List(Of Update_Integrated)) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount < 1 Then
      ToggleInputs(True, "No packages in WIM!")
      Return False
    End If
    If UpdateData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each UpdateList As List(Of Update_Integrated) In UpdateData
      If UpdateList IsNot Nothing Then pbMax += UpdateList.Count + 1
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    SetProgress(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      SetProgress(pbVal, pbMax)
      If UpdateData(I - 1) IsNot Nothing AndAlso UpdateData(I - 1).Count > 0 Then
        SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = GetDISMPackageData(WIMPath, I)
        SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
        If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        For J As Integer = 0 To UpdateData(I - 1).Count - 1
          pbVal += 1
          SetProgress(pbVal, pbMax)
          If UpdateData(I - 1)(J).Remove And Not (UpdateData(I - 1)(J).State = "Uninstall Pending" Or UpdateData(I - 1)(J).State = "Superseded") Then
            SetStatus((J + 1).ToString.Trim & "/" & UpdateData(I - 1).Count.ToString & " - Removing " & UpdateData(I - 1)(J).Ident.Name & " from " & tmpDISM.Name & "...")
            If Not RemovePackageItemFromDISM(Mount, UpdateData(I - 1)(J).Identity) Then
              DiscardDISM(Mount)
              ToggleInputs(True, "Failed to remove " & UpdateData(I - 1)(J).Ident.Name & " from " & tmpDISM.Name & "!")
              Return False
            End If
          End If
          If StopRun Then
            DiscardDISM(Mount)
            ToggleInputs(True)
            Return False
          End If
        Next
        SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
  Private Function IntegratedDrivers(WIMPath As String, DriverData() As List(Of Driver)) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount < 1 Then
      ToggleInputs(True, "No packages in WIM!")
      Return False
    End If
    If DriverData.Length < 1 Then Return True
    Dim pbMax As Integer = 0
    For Each DriverList As List(Of Driver) In DriverData
      If DriverList IsNot Nothing Then pbMax += DriverList.Count + 1
    Next
    If pbMax = 0 Then Return True
    Dim pbVal As Integer = 0
    SetProgress(pbVal, pbMax)
    For I As Integer = 1 To PackageCount
      pbVal += 1
      SetProgress(pbVal, pbMax)
      If DriverData(I - 1) IsNot Nothing AndAlso DriverData(I - 1).Count > 0 Then
        SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        Dim tmpDISM As ImagePackage = GetDISMPackageData(WIMPath, I)
        If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Load Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        For J As Integer = 0 To DriverData(I - 1).Count - 1
          pbVal += 1
          SetProgress(pbVal, pbMax)
          If DriverData(I - 1)(J).Remove Then
            SetStatus((J + 1).ToString.Trim & "/" & DriverData(I - 1).Count.ToString & " - Removing " & DriverData(I - 1)(J).PublishedName & " from " & tmpDISM.Name & "...")
            If Not RemoveDriverFromDISM(Mount, DriverData(I - 1)(J).PublishedName) Then
              DiscardDISM(Mount)
              ToggleInputs(True, "Failed to remove " & DriverData(I - 1)(J).PublishedName & " from " & tmpDISM.Name & "!")
              Return False
            End If
          End If
          If StopRun Then
            DiscardDISM(Mount)
            ToggleInputs(True)
            Return False
          End If
        Next
        SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True, "Failed to Save Image Package """ & tmpDISM.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
      End If
    Next
    Return True
  End Function
#Region "SP1 Extras"
  Private Sub UpdateSES(Path As String)
    Dim XUpdate As XElement = XElement.Load(Path)
    For Each node As XElement In XUpdate.Elements("Tasks")
      If node.Attribute("operationMode") = "OfflineInstall" Then
        node.Element("Phase").Element("package").SetAttributeValue("targetState", "Installed")
      ElseIf node.Attribute("operationMode") = "OfflineUninstall" Then
        node.Element("Phase").Element("package").SetAttributeValue("targetState", "Installed")
      End If
    Next
    XUpdate.Save(Path)
  End Sub
  Private Sub UpdateMUM(Path As String)
    Dim XUpdate As XElement = XElement.Load(Path)
    Dim xPackage = XUpdate.Element("{urn:schemas-microsoft-com:asm.v3}package")
    Dim xExtended = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}packageExtended")
    xExtended.SetAttributeValue("allowedOffline", "true")
    XUpdate.Save(Path)
  End Sub
#End Region
#End Region
#Region "Useful Functions"
  Private Sub ParseMainWIM()
    Dim sWIM As String = txtWIM.Text
    If String.IsNullOrEmpty(sWIM) Then Return
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkWIM As String = ParseWork & "WIM" & IO.Path.DirectorySeparatorChar
    If IO.File.Exists(ParseWork) Then
      WriteToOutput("Deleting """ & ParseWork & """...")
      SlowDeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End If
    IO.Directory.CreateDirectory(ParseWork)
    IO.Directory.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    Dim iTotalCount As Integer = 2
    Dim iTotalVal As Integer = 0
    SetTotal(0, iTotalCount)
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      iTotalCount += 1
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      SetStatus("Reading Image Packages...")
      ClearImageList("WIM")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      SetProgress(0, PackageCount)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      SetStatus("Populating Image Package List...")
      For I As Integer = 1 To PackageCount
        SetProgress(I, PackageCount)
        Dim Package As ImagePackage = GetDISMPackageData(WIMFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
        If Package = New ImagePackage Then Return
        If Package.Architecture.ToLower = "x64" Then
          If Not Package.Name.Contains("64") Then Package.Name &= " x64"
        End If
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        lvItem.Tag = {"WIM", Package, Nothing, Nothing}
        Dim en As String = ChrW(&H2003)
        Dim ttItem As String = Package.Desc & IIf(Package.SPLevel > 0, " Service Pack " & Package.SPLevel, "") & vbNewLine &
                               en & Package.ProductType & " " & Package.Version & "." & Package.SPBuild & " (" & Package.Edition & ") " & Package.Architecture & vbNewLine &
                               en & FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True) & " files, " & FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True) & " folders" & vbNewLine &
                               en & Package.Modified & vbNewLine &
                               en & ShortenPath(WIMFile)
        lvItem.ToolTipText = ttItem
        AddToImageList(lvItem)
        If Not lvImages.Columns.Count = 0 Then
          lvImages.BeginUpdate()
          Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width) - 1
          If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
          lvImages.EndUpdate()
        End If
      Next
      If tLister2 Is Nothing Then
        SetStatus("Idle")
      Else
        SetStatus("Extracting Merge Image from ISO...")
      End If
    Else
      SetStatus("Could not Extract Image from ISO!")
    End If
    SetProgress(0, 10)
    SetTotal(0, iTotalCount)
  End Sub
  Private Sub ParseMergeWIM()
    Dim sMerge As String = txtMerge.Text
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkMerge As String = ParseWork & "Merge" & IO.Path.DirectorySeparatorChar
    IO.Directory.CreateDirectory(ParseWorkMerge)
    If String.IsNullOrEmpty(sMerge) Then Return
    Dim MergeFile As String = String.Empty
    Dim iTotalCount As Integer = 2
    Dim iTotalVal As Integer = 0
    SetTotal(iTotalVal, iTotalCount)
    If IO.Path.GetExtension(sMerge).ToLower = ".iso" Then
      SetStatus("Extracting Merge Image from ISO...")
      iTotalCount += 1
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      ExtractAFile(sMerge, ParseWorkMerge, "INSTALL.WIM")
      MergeFile = ParseWorkMerge & "INSTALL.WIM"
    Else
      MergeFile = sMerge
    End If
    If IO.File.Exists(MergeFile) Then
      SetStatus("Reading Merge Image Packages...")
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      ClearImageList("MERGE")
      Dim PackageCount As Integer = GetDISMPackages(MergeFile)
      SetProgress(0, PackageCount)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalCount)
      SetStatus("Populating Merge Image Package List...")
      For I As Integer = 1 To PackageCount
        SetProgress(I, PackageCount)
        Dim Package As ImagePackage = GetDISMPackageData(MergeFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
        If Package = New ImagePackage Then Return
        If Package.Architecture.ToLower = "x64" Then
          If Not Package.Name.Contains("64") Then Package.Name &= " x64"
        End If
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        lvItem.Tag = {"MERGE", Package, Nothing, Nothing}
        Dim en As String = ChrW(&H2003)
        Dim ttItem As String = Package.Desc & IIf(Package.SPLevel > 0, " Service Pack " & Package.SPLevel, "") & vbNewLine &
                               en & Package.ProductType & " " & Package.Version & "." & Package.SPBuild & " (" & Package.Edition & ") " & Package.Architecture & vbNewLine &
                               en & FormatNumber(Package.Files, 0, TriState.False, TriState.False, TriState.True) & " files, " & FormatNumber(Package.Directories, 0, TriState.False, TriState.False, TriState.True) & " folders" & vbNewLine &
                               en & Package.Modified & vbNewLine &
                               en & ShortenPath(MergeFile)
        lvItem.ToolTipText = ttItem
        AddToImageList(lvItem)
        If Not lvImages.Columns.Count = 0 Then
          lvImages.BeginUpdate()
          Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width) - 1
          If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
          lvImages.EndUpdate()
        End If
      Next
      If tLister Is Nothing Then
        SetStatus("Idle")
      Else
        SetStatus("Extracting Image from ISO...")
      End If
    Else
      SetStatus("Could not Extract Merge Image from ISO!")
    End If
    SetProgress(0, 10)
    SetTotal(0, iTotalCount)
  End Sub
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
  Private Sub SortForRequirement(ByRef msuList As List(Of Update_File))
    For I As Integer = 0 To msuList.Count - 1
      CheckForRequirement(msuList, I)
    Next
  End Sub
  Private Sub CheckForRequirement(ByRef msuList As List(Of Update_File), Index As Integer)
    For I As Integer = 0 To PrerequisiteList.Length - 1
      If msuList(Index).KBArticle = PrerequisiteList(I).KBWithRequirement Then
        For J As Integer = Index + 1 To msuList.Count - 1
          If PrerequisiteList(I).Requirement.Contains(msuList(J).KBArticle) Then
            Dim moveItem As Update_File = msuList(J)
            msuList.RemoveAt(J)
            msuList.Insert(Index, moveItem)
            CheckForRequirement(msuList, Index)
          End If
        Next
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
  Private ReadOnly Property WorkDir As String
    Get
      Dim tempDir As String = mySettings.TempDir
      If String.IsNullOrEmpty(tempDir) Then tempDir = My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "Slips7ream" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(tempDir) Then IO.Directory.CreateDirectory(tempDir)
      Return tempDir
    End Get
  End Property
  Private ReadOnly Property Work As String
    Get
      Dim sDir As String = WorkDir & "WORK" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(sDir) Then IO.Directory.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
  Private ReadOnly Property Mount As String
    Get
      Dim sDir As String = WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(sDir) Then IO.Directory.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
  Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
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
  Private Sub tmrUpdateCheck_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdateCheck.Tick
    tmrUpdateCheck.Stop()
    If mySettings.LastUpdate.Year = 1970 Then mySettings.LastUpdate = Today
    If DateDiff(DateInterval.Day, mySettings.LastUpdate, Today) > 13 Then
      mySettings.LastUpdate = Today
      SetStatus("Beginning Update Check...")
      cUpdate = New clsUpdate
      Dim updateCaller As New MethodInvoker(AddressOf cUpdate.CheckVersion)
      updateCaller.BeginInvoke(Nothing, Nothing)
    End If
  End Sub
  Private Sub cUpdate_CheckingVersion(sender As Object, e As System.EventArgs) Handles cUpdate.CheckingVersion
    If GetStatus() = "Beginning Update Check..." Or GetStatus() = "Checking for new Version..." Then SetStatus("Checking for new Version...")
  End Sub
  Private Sub cUpdate_CheckProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdate.CheckProgressChanged
    If GetStatus() = "Beginning Update Check..." Or GetStatus() = "Checking for new Version..." Then SetStatus("Checking for new Version... (" & e.ProgressPercentage & "%)")
  End Sub
  Private Sub cUpdate_CheckResult(sender As Object, e As clsUpdate.CheckEventArgs) Handles cUpdate.CheckResult
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(Of clsUpdate.CheckEventArgs)(AddressOf cUpdate_CheckResult), sender, e)
    Else
      If e.Cancelled Then
        SetStatus("Update Check Cancelled!")
      ElseIf e.Error IsNot Nothing Then
        SetStatus("Update Check Failed: " & e.Error.Message & "!")
      Else
        If e.Result = clsUpdate.CheckEventArgs.ResultType.NewUpdate Then
          SetStatus("New Version Available!")
          If MsgDlg(Me, "Would you like to update now?", "SLIPS7REAM v" & e.Version & " is available!", "Application Update", MessageBoxButtons.YesNo, TaskDialogIcon.InternetRJ45) = Windows.Forms.DialogResult.Yes Then
            cUpdate.DownloadUpdate(WorkDir & "Setup.exe")
          End If
        Else
          SetStatus("Idle")
          AskForDonations()
        End If
      End If
    End If
  End Sub
  Private Sub cUpdate_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles cUpdate.DownloadingUpdate
    SetStatus("Downloading New Version - Waiting for Response...")
  End Sub
  Private Sub cUpdate_DownloadResult(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles cUpdate.DownloadResult
    If Me.InvokeRequired Then
      Me.Invoke(New System.ComponentModel.AsyncCompletedEventHandler(AddressOf cUpdate_CheckResult), sender, e)
    Else
      If e.Cancelled Then
        SetStatus("Update Download Cancelled!")
      ElseIf e.Error IsNot Nothing Then
        SetStatus("Update Download Failed: " & e.Error.Message & "!")
      Else
        cUpdate.Dispose()
        SetStatus("Download Complete!")
        Application.DoEvents()
        If My.Computer.FileSystem.FileExists(WorkDir & "Setup.exe") Then
          Do
            Try
              Shell(WorkDir & "Setup.exe /silent", AppWinStyle.NormalFocus, False)
              Exit Do
            Catch ex As Exception
              If MsgDlg(Me, "If you have User Account Control enabled, please allow the SLIPS7REAM Installer to run." & vbNewLine & "Would you like to attempt to run the update again?", "There was an error starting the update.", "Application Update Failure", MessageBoxButtons.YesNo, TaskDialogIcon.ShieldUAC, , ex.Message) = Windows.Forms.DialogResult.No Then Exit Do
            End Try
          Loop
          Application.Exit()
        Else
          SetStatus("Update Failure!")
        End If
      End If
    End If
  End Sub
  Private Sub cUpdate_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdate.UpdateProgressChanged
    SetStatus("Downloading New Version - " & ByteSize(e.BytesReceived) & " of " & ByteSize(e.TotalBytesToReceive) & "... (" & e.ProgressPercentage & "%)")
  End Sub
#End Region
End Class
