﻿Public Class frmMain
  Public StopRun As Boolean = False
  Friend taskBar As TaskbarLib.TaskbarList
  Private LangChange As Boolean = False
  Private RunComplete As Boolean = False
  Private RunActivity As Byte = 0
  Private tLister As Threading.Thread
  Private tLister2 As Threading.Thread
  Private Const HeightDifferentialA As Integer = 207
  Private Const HeightDifferentialB As Integer = 22
  Private Const FrameInterval As UInteger = 3
  Private WithEvents cUpdate As clsUpdate
  Private FrameNumber As UInteger
  Private FrameCount As UInteger
  Private mngDisp As MNG
  Private sTitleText As String
  Private windowChangedSize As Boolean
  Private mySettings As MySettings
  Private outputWindow As Boolean = False
  Private Enum MNGList
    Move
    Copy
    Delete
  End Enum
#Region "GUI"
  Private Sub frmMain_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
    redrawCaption()
  End Sub
  Private Sub frmMain_Deactivate(sender As Object, e As System.EventArgs) Handles Me.Deactivate
    redrawCaption()
  End Sub
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Me.Tag = "LOADING"
    mySettings = New MySettings
    Me.imlUpdates.Images.Add("MSU", My.Resources.wusa)
    Me.imlUpdates.Images.Add("DIR", My.Resources.folder)
    Me.imlUpdates.Images.Add("CAB", My.Resources.cab)
    Me.imlUpdates.Images.Add("MLC", My.Resources.mlc)
    ttInfo.SetTooltip(expOutput.pctExpander, "Show Output consoles.")
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
    lvImages.ShowGroups = True
    lvImages.Groups.Add("WIM", "Source Image")
    lvImages.Groups.Add("MERGE", "Merge Image")
    lvImages.ShowGroups = False
    If String.IsNullOrEmpty(mySettings.DefaultISOLabel) Then
      txtISOLabel.Text = "GRMCULFRER_EN_DVD"
    Else
      txtISOLabel.Text = mySettings.DefaultISOLabel
    End If
    If mySettings.Size.IsEmpty Then
      Me.Size = Me.MinimumSize
    ElseIf mySettings.Size.Width < Screen.PrimaryScreen.Bounds.Width And mySettings.Size.Height < Screen.PrimaryScreen.Bounds.Height Then
      Me.Size = mySettings.Size
    Else
      Me.Size = Me.MinimumSize
    End If
    txtOutput.ContextMenu = mnuOutput
    txtOutputError.ContextMenu = mnuOutput
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
      If MsgDlg(Me, "Do you want to cancel the " & sProc & " proceess and close SLIPS7REAM?", "SLIPS7REAM is busy " & sActivity & ".", "Stop " & sTitle & " and Close?", MessageBoxButtons.YesNo, TaskDialogIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
        e.Cancel = True
        Return
      End If
    End If
    StopRun = True
    CloseCleanup()
  End Sub
  Private Sub CloseCleanup()
    If IO.Directory.Exists(WorkDir) Then
      SetTotalTime(SpeedStats.Clean_Temp)
      Dim tempStart As Long = TickCount()
      SetTitle("Cleaning Up Files", "Cleaning up mounts, work, and temporary directories...")
      SetDisp(MNGList.Delete)
      ToggleInputs(False)
      cmdClose.Enabled = False
      StopRun = False
      CleanMounts()
      SetStatus("Clearing Temp Directory...")
      WriteToOutput("Deleting """ & WorkDir & """...")
      SetProgressTime(SpeedStats.Clean_WORK)
      Dim workStart As Long = TickCount()
      Try
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Catch ex As Exception
      End Try
      SpeedStats.Clean_WORK = TickCount() - workStart
      SpeedStats.Clean_Temp = TickCount() - tempStart
      StopRun = True
      pctTitle.Image = Nothing
      mngDisp = Nothing
    End If
  End Sub
  Private Sub tmrAnimation_Tick(sender As System.Object, e As System.EventArgs) Handles tmrAnimation.Tick
    If mngDisp IsNot Nothing Then
      Dim bmpFrame As Bitmap = mngDisp.ToBitmap(FrameNumber)
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
          g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.Black, New Point(16, 8))
          g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.White, New Point(15, 7))
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
          g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.Black, New Point(16, 8))
          g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.White, New Point(15, 7))
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
        g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.Black, New Point(16, 8))
        g.DrawString(sTitleText, New Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular), Brushes.White, New Point(15, 7))
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
      mngDisp.Load(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng")
      FrameNumber = 0
      FrameCount = mngDisp.NumEmbeddedPNG - 1
      IO.File.Delete(My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "slips7ream_anim.mng")
    End If
  End Sub
  Private Sub RedoColumns()
    If Not lvMSU.Columns.Count = 0 Then
      Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 1
      If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
    End If
    If Not lvImages.Columns.Count = 0 Then
      Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width) - 1
      If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
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


        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        g.DrawString("SLIPS7REAM Output Consoles", SystemFonts.SmallCaptionFont, SystemBrushes.ActiveCaptionText, New RectangleF(3, 1, pctOutputTear.Width - 6, pctOutputTear.Height - 2), New StringFormat(StringFormatFlags.NoWrap Or StringFormatFlags.NoClip))

      End Using
      pctOutputTear.Image = bTitle.Clone
    End Using
  End Sub
  Private Delegate Sub ToggleInputsInvoker(Enabled As Boolean)
  Private Sub ToggleInputs(Enabled As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New ToggleInputsInvoker(AddressOf ToggleInputs), Enabled)
    Else
      If Enabled Then
        RunActivity = 0
        Me.Cursor = Cursors.Default
        tmrAnimation.Stop()
        FreshDraw()
      Else
        Me.Cursor = Cursors.AppStarting
        tmrAnimation.Start()
      End If
      pnlSlips7ream.SuspendLayout()
      lblWIM.Enabled = Enabled
      txtWIM.Enabled = Enabled
      cmdWIM.Enabled = Enabled
      chkSP.Enabled = Enabled
      txtSP.Enabled = IIf(Enabled, chkSP.Checked, Enabled)
      cmdSP.Enabled = IIf(Enabled, chkSP.Checked, Enabled)
      lblSP64.Enabled = IIf(Enabled, chkSP.Checked, Enabled)
      txtSP64.Enabled = IIf(Enabled, chkSP.Checked, Enabled)
      cmdSP64.Enabled = IIf(Enabled, chkSP.Checked, Enabled)
      lblMSU.Enabled = Enabled
      lvMSU.Enabled = Enabled
      lvMSU.HideSelection = Not Enabled
      cmdAddMSU.Enabled = Enabled
      cmdRemMSU.Enabled = IIf(Enabled, lvMSU.SelectedItems.Count > 0, Enabled)
      cmdClearMSU.Enabled = IIf(Enabled, lvMSU.Items.Count > 0, Enabled)
      If RunComplete Then
        cmdBegin.Text = "Rerun"
        cmdOpenFolder.Visible = True
      Else
        cmdBegin.Text = "&Begin"
        cmdOpenFolder.Visible = False
      End If
      cmdBegin.Visible = Enabled
      chkISO.Enabled = Enabled
      txtISO.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      cmdISO.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      lblISOFeatures.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      chkUnlock.Enabled = IIf(Enabled, chkISO.Checked And (chkUnlock.Tag Is Nothing), Enabled)
      chkUEFI.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      lblISOLabel.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      txtISOLabel.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      lblISOFS.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      cmbISOFormat.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
      cmbLimitType.Enabled = Enabled
      If chkISO.Checked Then
        If Not cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Add("Split ISO")
      Else
        If cmbLimitType.Items.Contains("Split ISO") Then cmbLimitType.Items.Remove("Split ISO")
      End If
      If cmbLimitType.SelectedIndex = -1 Then cmbLimitType.SelectedIndex = 0
      cmbLimit.Enabled = IIf(Enabled, cmbLimitType.SelectedIndex > 0, Enabled)
      chkMerge.Enabled = Enabled
      txtMerge.Enabled = IIf(Enabled, chkMerge.Checked, Enabled)
      cmdMerge.Enabled = IIf(Enabled, chkMerge.Checked, Enabled)
      lblImages.Enabled = Enabled
      lvImages.Enabled = Enabled
      If Enabled Then
        cmdClose.Text = "&Close"
        SetStatus("Idle")
      Else
        cmdClose.Text = "&Cancel"
      End If
      cmdConfig.Visible = Enabled
      cmdClose.Enabled = True
      If pbTotal.Visible = Enabled Then
        pbTotal.Visible = Not Enabled
        pbIndividual.Visible = Not Enabled
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
      pbTotal.Visible = Not Enabled
      pbIndividual.Visible = Not Enabled
      If Not pbTotal.Visible Then
        pbTotal.Value = 0
        timeTot = 0
        lblTotalTime.Text = "--"
        lblTotalTime.Visible = False
      End If
      If Not pbIndividual.Visible Then
        pbIndividual.Value = 0
        timeInd = 0
        lblIndividualTime.Text = "--"
        lblIndividualTime.Visible = False
      End If
      pnlSlips7ream.ResumeLayout(True)
      If pbTotal.Visible Then
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
      Else
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
      End If
    End If
  End Sub
  Private Sub pctTitle_DoubleClick(sender As Object, e As System.EventArgs) Handles pctTitle.DoubleClick
    frmTetris.Show()
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
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
      chkISO.Checked = True
      txtISO.Text = txtWIM.Text
    Else
      chkISO.Checked = False
      txtISO.Text = String.Empty
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
    cmdOpenFolder.Visible = False
    If tLister Is Nothing Then
      tLister = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf ParseImageList))
      tLister.Start("WIM")
    End If
  End Sub
  Private Sub cmdWIM_Click(sender As System.Object, e As System.EventArgs) Handles cmdWIM.Click
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "INSTALL.WIM Sources|INSTALL.WIM;*.ISO|INSTALL.WIM|INSTALL.WIM|Windows 7 ISO|*.ISO|All Files|*.*"
      cdlBrowse.Title = "Choose INSTALL.WIM Image..."
      cdlBrowse.ShowReadOnly = False
      If Not String.IsNullOrEmpty(txtWIM.Text) Then cdlBrowse.InitialDirectory = txtWIM.Text
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtWIM.Text = cdlBrowse.FileName
        If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
          chkISO.Checked = True
          txtISO.Text = txtWIM.Text
        Else
          chkISO.Checked = False
          txtISO.Text = String.Empty
        End If
      End If
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
      Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
      Dim ParseWorkWIM As String = ParseWork & "WIM" & IO.Path.DirectorySeparatorChar
      For Each lvItem As ListViewItem In lvImages.Items
        Dim propData As PackageInfoEx = lvItem.Tag(1)
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
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "Service Pack EXE|*.EXE|All Files|*.*"
      cdlBrowse.Title = "Choose Windows 7 Service Pack 1 EXE..."
      cdlBrowse.ShowReadOnly = False
      If Not String.IsNullOrEmpty(txtSP.Text) Then cdlBrowse.InitialDirectory = txtSP.Text
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtSP.Text = cdlBrowse.FileName
      End If
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
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "Service Pack EXE|*.EXE|All Files|*.*"
      cdlBrowse.Title = "Choose Windows 7 x64 Service Pack 1 EXE..."
      cdlBrowse.ShowReadOnly = False
      If Not String.IsNullOrEmpty(txtSP64.Text) Then cdlBrowse.InitialDirectory = txtSP64.Text
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtSP64.Text = cdlBrowse.FileName
      End If
    End Using
  End Sub
#End Region
#End Region
#Region "Updates"
  Private Sub lvMSU_DoubleClick(sender As Object, e As System.EventArgs) Handles lvMSU.DoubleClick
    If lvMSU.SelectedItems IsNot Nothing AndAlso lvMSU.SelectedItems.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.SelectedItems
        Dim msuData As New UpdateInfoEx(lvItem.Tag)
        If Not String.IsNullOrEmpty(msuData.Failure) Then If ExtractFailureAlert(msuData.Failure) Then Continue For
        If Not String.IsNullOrEmpty(msuData.DisplayName) Then
          For Each Form In OwnedForms
            If Form.Tag = msuData.DisplayName Then
              Form.Focus()
              Return
            End If
          Next
          Dim props As New frmUpdateProps
          props.Tag = msuData.DisplayName
          props.txtDisplayName.Text = msuData.DisplayName
          props.txtAppliesTo.Text = msuData.AppliesTo
          props.txtArchitecture.Text = msuData.Architecture
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
          props.ttLink.SetTooltip(props.lblKBLink, msuData.SupportLink)
          props.Show(Me)
        End If
      Next
    End If
  End Sub

#Region "Drag/Drop"
  Private Function GetFileCountFromDir(Path As String) As Integer
    Dim iCount As Integer = 0
    For Each sFile As String In IO.Directory.EnumerateFiles(Path)
      Select Case IO.Path.GetExtension(sFile).ToLower
        Case ".msu", ".cab", ".mlc", ".exe"
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
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
        SetProgress(0, 0)
        SetStatus("Reading Update Information...")
        Dim expectedTime As Long = 0
        For I As Integer = 0 To Data.Length - 1
          Dim sUpdate As String = Data(I)
          Dim uType = GetUpdateType(sUpdate)
          Dim lIncrease As Long = 0
          If sUpdate.ToLower.Contains("x64") Then
            Select Case uType
              Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", "x64")
              Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", "x64")
              Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", "x64")
              Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", "x64")
              Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", "x64")
            End Select
          ElseIf sUpdate.ToLower.Contains("x86") Then
            Select Case uType
              Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", "x86")
              Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", "x86")
              Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", "x86")
              Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", "x86")
              Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", "x86")
            End Select
          Else
            Dim iSize As Integer = Math.Ceiling(New IO.FileInfo(sUpdate).Length / 1024 / 1024)
            Dim sSize As String = iSize & "MB"
            Select Case uType
              Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", sSize)
              Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", sSize)
              Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", sSize)
              Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", sSize)
              Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", sSize)
            End Select
          End If
          If lIncrease > 0 Then expectedTime += lIncrease
        Next I
        SetTotalTime(expectedTime)
      End If
      Dim FailCollection As New Collections.Generic.List(Of String)
      lvMSU.SuspendLayout()
      Dim iProg As Integer = 0
      For Each Item In Data
        If FileCount > 2 Then
          iProg += 1
          SetTotal(iProg, FileCount)
          If taskBar IsNot Nothing Then taskBar.SetProgressValue(Me.Handle, pbTotal.Value, pbTotal.Maximum)
          Application.DoEvents()
          If StopRun Then
            SetProgress(0, 1)
            ToggleInputs(True)
            Return
          End If
        End If
        Dim lStart As Long = TickCount()
        Dim addRet As AddResult = AddToUpdates(Item)
        Dim uType = GetUpdateType(Item)
        If Item.ToLower.Contains("x64") Then
          Select Case uType
            Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", "x64") = TickCount() - lStart
            Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", "x64") = TickCount() - lStart
            Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", "x64") = TickCount() - lStart
            Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", "x64") = TickCount() - lStart
            Case UpdateType.LP : SpeedStats.Update_Parse("LP", "x64") = TickCount() - lStart
          End Select
        ElseIf Item.ToLower.Contains("x86") Then
          Select Case uType
            Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", "x86") = TickCount() - lStart
            Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", "x86") = TickCount() - lStart
            Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", "x86") = TickCount() - lStart
            Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", "x86") = TickCount() - lStart
            Case UpdateType.LP : SpeedStats.Update_Parse("LP", "x86") = TickCount() - lStart
          End Select
        Else
          Dim iSize As Integer = Math.Ceiling(New IO.FileInfo(Item).Length / 1024 / 1024)
          Dim sSize As String = iSize & "MB"
          Select Case uType
            Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", sSize) = TickCount() - lStart
            Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", sSize) = TickCount() - lStart
            Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", sSize) = TickCount() - lStart
            Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", sSize) = TickCount() - lStart
            Case UpdateType.LP : SpeedStats.Update_Parse("LP", sSize) = TickCount() - lStart
          End Select
        End If
        If Not addRet.Success Then FailCollection.Add(IO.Path.GetFileNameWithoutExtension(Item) & ": " & addRet.FailReason)
      Next
      lvMSU.ResumeLayout(True)
      If FileCount > 2 Then
        SetProgress(0, 1)
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
        ToggleInputs(True)
      End If
      RedoColumns()
      If FailCollection.Count > 0 Then
        If FailCollection.Count > 10 Then
          FailCollection(9) = vbNewLine & "...and " & FailCollection.Count - 10 & " other failures."
          FailCollection.RemoveRange(10, FailCollection.Count - 10)
        End If
        MsgDlg(Me, "Some files could not be added to the Update List." & vbNewLine & "Click View Details to see a complete list.", "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, TaskDialogIcon.WindowsUpdate, , Join(FailCollection.ToArray, vbNewLine))
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
            Case ".msu", ".cab", ".mlc", ".exe"
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
        If sourceRows(0) <> TargetRowIndex Then
          Dim targetI As Integer = TargetRowIndex
          For Each item As ListViewItem In lvMSU.SelectedItems
            Dim tmpItem = item.Clone
            item.Remove()
            lvMSU.Items.Insert(targetI, tmpItem)
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
    If LastClick > 0 And (TickCount() - LastClick) < 250 Then
      lvMSU_DoubleClick(lvMSU, New EventArgs)
    End If
    sourceRows.Clear()
    LastClick = TickCount()
  End Sub

  Private Sub lvMSU_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvMSU.SelectedIndexChanged
    cmdRemMSU.Enabled = lvMSU.SelectedItems.Count > 0
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
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "All Packages|*.MSU;*.CAB;*.MLC;*.EXE|Windows Updates|*.MSU;*.CAB|Language Packs|*.MLC;*.EXE|All Files|*.*"
      cdlBrowse.Title = "Add Windows Updates..."
      cdlBrowse.Multiselect = True
      cdlBrowse.ShowReadOnly = False
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        Dim FailCollection As New Collections.Generic.List(Of String)
        Dim FileCount As Integer = cdlBrowse.FileNames.Count
        If FileCount > 2 Then
          RunActivity = 3
          StopRun = False
          SetDisp(MNGList.Delete)
          SetTitle("Parsing Update Information", "Reading data from update files...")
          ToggleInputs(False)
          SetTotal(0, FileCount)
          If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
          SetProgress(0, 0)
          SetStatus("Reading Update Information...")
          Dim expectedTime As Long = 0
          For I As Integer = 0 To cdlBrowse.FileNames.Length - 1
            Dim sUpdate As String = cdlBrowse.FileNames(I)
            Dim uType = GetUpdateType(sUpdate)
            Dim lIncrease As Long = 0
            If sUpdate.ToLower.Contains("x64") Then
              Select Case uType
                Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", "x64")
                Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", "x64")
                Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", "x64")
                Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", "x64")
                Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", "x64")
              End Select
            ElseIf sUpdate.ToLower.Contains("x86") Then
              Select Case uType
                Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", "x86")
                Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", "x86")
                Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", "x86")
                Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", "x86")
                Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", "x86")
              End Select
            Else
              Dim iSize As Integer = Math.Ceiling(New IO.FileInfo(sUpdate).Length / 1024 / 1024)
              Dim sSize As String = iSize & "MB"
              Select Case uType
                Case UpdateType.MSU : lIncrease = SpeedStats.Update_Parse("MSU", sSize)
                Case UpdateType.CAB : lIncrease = SpeedStats.Update_Parse("CAB", sSize)
                Case UpdateType.EXE : lIncrease = SpeedStats.Update_Parse("EXE", sSize)
                Case UpdateType.LIP : lIncrease = SpeedStats.Update_Parse("LIP", sSize)
                Case UpdateType.LP : lIncrease = SpeedStats.Update_Parse("LP", sSize)
              End Select
            End If
            If lIncrease > 0 Then expectedTime += lIncrease
          Next I
          SetTotalTime(expectedTime)
        End If
        lvMSU.SuspendLayout()
        For I As Integer = 0 To cdlBrowse.FileNames.Length - 1
          Dim sUpdate As String = cdlBrowse.FileNames(I)
          If FileCount > 2 Then
            SetTotal(I + 1, FileCount)
            If taskBar IsNot Nothing Then taskBar.SetProgressValue(Me.Handle, pbTotal.Value, pbTotal.Maximum)
            Application.DoEvents()
            If StopRun Then
              SetProgress(0, 1)
              ToggleInputs(True)
              Return
            End If
          End If
          Dim lStart As Long = TickCount()
          Dim addRet As AddResult = AddToUpdates(sUpdate)
          Dim uType = GetUpdateType(sUpdate)
          If sUpdate.ToLower.Contains("x64") Then
            Select Case uType
              Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", "x64") = TickCount() - lStart
              Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", "x64") = TickCount() - lStart
              Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", "x64") = TickCount() - lStart
              Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", "x64") = TickCount() - lStart
              Case UpdateType.LP : SpeedStats.Update_Parse("LP", "x64") = TickCount() - lStart
            End Select
          ElseIf sUpdate.ToLower.Contains("x86") Then
            Select Case uType
              Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", "x86") = TickCount() - lStart
              Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", "x86") = TickCount() - lStart
              Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", "x86") = TickCount() - lStart
              Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", "x86") = TickCount() - lStart
              Case UpdateType.LP : SpeedStats.Update_Parse("LP", "x86") = TickCount() - lStart
            End Select
          Else
            Dim iSize As Integer = Math.Ceiling(New IO.FileInfo(sUpdate).Length / 1024 / 1024)
            Dim sSize As String = iSize & "MB"
            Select Case uType
              Case UpdateType.MSU : SpeedStats.Update_Parse("MSU", sSize) = TickCount() - lStart
              Case UpdateType.CAB : SpeedStats.Update_Parse("CAB", sSize) = TickCount() - lStart
              Case UpdateType.EXE : SpeedStats.Update_Parse("EXE", sSize) = TickCount() - lStart
              Case UpdateType.LIP : SpeedStats.Update_Parse("LIP", sSize) = TickCount() - lStart
              Case UpdateType.LP : SpeedStats.Update_Parse("LP", sSize) = TickCount() - lStart
            End Select
          End If

          If Not addRet.Success Then FailCollection.Add(IO.Path.GetFileNameWithoutExtension(sUpdate) & ": " & addRet.FailReason)
        Next
        lvMSU.ResumeLayout(True)
        If FileCount > 2 Then
          SetProgress(0, 1)
          If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
          ToggleInputs(True)
        End If
        RedoColumns()
        If FailCollection.Count > 0 Then
          If FailCollection.Count > 10 Then
            FailCollection(9) = vbNewLine & "...and " & FailCollection.Count - 10 & " other failures."
            FailCollection.RemoveRange(10, FailCollection.Count - 10)
          End If
          MsgDlg(Me, "Some files could not be added to the Update List." & vbNewLine & "Click View Details to see a complete list.", "Unable to add files to the Update List.", "Error Adding Updates", MessageBoxButtons.OK, TaskDialogIcon.WindowsUpdate, , Join(FailCollection.ToArray, vbNewLine))
        End If
      End If
    End Using
    SetStatus("Idle")
  End Sub
  Private Class AddResult
    Public Success As Boolean
    Public FailReason As String
    Public Sub New(b As Boolean, Optional s As String = "")
      Success = b
      If Not b Then
        If String.IsNullOrEmpty(s) Then s = "Unknown Failure"
        FailReason = s
      End If
    End Sub
  End Class

  Dim ReplaceAllOldUpdates As TriState = TriState.UseDefault
  Dim ReplaceAllNewUpdates As TriState = TriState.UseDefault
  Private Function AddToUpdates(sUpdate As String) As AddResult
    If IO.File.Exists(sUpdate) Then
      Dim msuData As New UpdateInfoEx(sUpdate)
      If Not String.IsNullOrEmpty(msuData.Failure) Then Return New AddResult(False, msuData.Failure)
      If String.IsNullOrEmpty(msuData.DisplayName) Then Return New AddResult(False, "Unable to parse information.")
      Dim msuType As String
      Select Case GetUpdateType(sUpdate)
        Case UpdateType.MSU : msuType = msuData.Architecture & " MSU"
        Case UpdateType.CAB : msuType = msuData.Architecture & " CAB"
        Case UpdateType.LP : msuType = msuData.Architecture & " MUI"
        Case UpdateType.LIP : msuType = msuData.Architecture & " LIP"
        Case UpdateType.EXE
          If msuData.DisplayName = "Windows Update Agent" Then
            msuType = msuData.Architecture & " CAB"
          Else
            msuType = msuData.Architecture & " MUI"
          End If
        Case Else
          If String.IsNullOrEmpty(IO.Path.GetExtension(sUpdate)) Then
            Return New AddResult(False, "Unknown Update Type.")
          Else
            Return New AddResult(False, "Unknown Update Type: """ & IO.Path.GetExtension(sUpdate).Substring(1).ToUpper & """.")
          End If
      End Select
      For Each item As ListViewItem In lvMSU.Items
        If item.SubItems(1).Text = msuType Then
          If item.Text = msuData.DisplayName Then
            Return New AddResult(False, "Update already added.")
          ElseIf msuData.DisplayName.Contains("-v") Or item.Text.Contains("-v") Then
            Dim AddingName As String = msuData.DisplayName
            Dim AddingVer As String = "1"
            If AddingName.Contains("-v") Then
              AddingVer = AddingName.Substring(AddingName.IndexOf("-v") + 2)
              AddingVer = AddingVer.Substring(0, AddingVer.IndexOf("-"))
              AddingName = AddingName.Substring(0, AddingName.IndexOf("-v"))
            ElseIf AddingName.Contains("-x") Then
              AddingName = AddingName.Substring(0, AddingName.IndexOf("-x"))
            End If
            Dim ItemName As String = item.Text
            Dim ItemVer As String = "1"
            If ItemName.Contains("-v") Then
              ItemVer = ItemName.Substring(ItemName.IndexOf("-v") + 2)
              ItemVer = ItemVer.Substring(0, ItemVer.IndexOf("-"))
              ItemName = ItemName.Substring(0, ItemName.IndexOf("-v"))
            ElseIf ItemName.Contains("-x") Then
              ItemName = ItemName.Substring(0, ItemName.IndexOf("-x"))
            End If
            If AddingName = ItemName Then
              Dim aVer, iVer As Integer
              If Not Integer.TryParse(AddingVer, aVer) Then aVer = 1
              If Not Integer.TryParse(ItemVer, iVer) Then iVer = 1
              Dim always As Boolean = False
              If aVer > iVer Then
                If ReplaceAllOldUpdates = TriState.True Then
                  item.Remove()
                ElseIf ReplaceAllOldUpdates = TriState.False Then
                  Return New AddResult(True)
                Else
                  If SelectionBox(Me, sUpdate, AddingVer, item.Tag, ItemVer, always) Then
                    If always Then ReplaceAllOldUpdates = TriState.True
                    item.Remove()
                  Else
                    If always Then ReplaceAllOldUpdates = TriState.False
                    Return New AddResult(True)
                  End If
                End If
              ElseIf aVer < iVer Then
                If ReplaceAllNewUpdates = TriState.True Then
                  item.Remove()
                ElseIf ReplaceAllNewUpdates = TriState.False Then
                  Return New AddResult(True)
                Else
                  If SelectionBox(Me, sUpdate, AddingVer, item.Tag, ItemVer, always) Then
                    If always Then ReplaceAllNewUpdates = TriState.True
                    item.Remove()
                  Else
                    If always Then ReplaceAllNewUpdates = TriState.False
                    Return New AddResult(True)
                  End If
                End If
              Else
                Return New AddResult(False, "Update already added.")
              End If
            End If
          End If
        End If
      Next
      If msuData.DisplayName.ToLower.Contains("-kb2533552-") Then Return New AddResult(False, "Update can't be integrated.")
      If msuData.DisplayName.ToLower.Contains("-kb947821-") Then Return New AddResult(False, "Update can't be integrated.")
      Dim lvItem As New ListViewItem(msuData.DisplayName)
      lvItem.Tag = sUpdate
      Dim bWhitelist As Boolean = msuData.Architecture = "x86" AndAlso CheckWhitelist(msuData.DisplayName)
      Dim ttItem As String = IIf(String.IsNullOrEmpty(msuData.KBArticle), msuData.DisplayName, "KB" & msuData.KBArticle)
      ttItem &= vbNewLine & msuData.AppliesTo & " " & msuData.Architecture & IIf(bWhitelist, " [Whitelisted for 64-bit]", "")
      If Not String.IsNullOrEmpty(msuData.BuildDate) Then ttItem &= vbNewLine & "Built: " & msuData.BuildDate
      ttItem &= vbNewLine & ShortenPath(msuData.Path)
      lvItem.BackColor = IIf(bWhitelist, SystemColors.GradientInactiveCaption, SystemColors.Window)
      If msuData.DisplayName.ToLower.Contains("-kb2647753-v2-") Then
        lvItem.ForeColor = Color.Red
        ttItem &= vbNewLine & "Version 2 may not integrate correctly. SLIPS7REAM suggests using Version 4 from the Microsoft Update Catalog."
      ElseIf msuData.DisplayName.ToLower.Contains("-kb2830477-") Then
        Dim Req1 As Boolean = False
        Dim Req2 As Boolean = False
        For Each searchItem As ListViewItem In lvMSU.Items
          If searchItem.Text.ToLower.Contains("kb2574819") Then Req1 = True
          If searchItem.Text.ToLower.Contains("kb2857650") Then Req2 = True
          If Req1 And Req1 Then Exit For
        Next
        If Not Req1 Or Not Req2 Then
          lvItem.ForeColor = Color.Orange
          ttItem &= vbNewLine & "Please make sure KB2574819 and KB2857650 are also integrated."
        End If
      ElseIf msuData.DisplayName.ToLower.Contains("-kb2574819-") Then
        Dim Req As Boolean = False
        For Each searchItem As ListViewItem In lvMSU.Items
          If searchItem.Text.ToLower.Contains("kb2857650") Then
            Req = True
            Exit For
          End If
        Next
        If Req Then
          For Each searchItem As ListViewItem In lvMSU.Items
            If searchItem.Text.ToLower.Contains("kb2830477") Then
              searchItem.ForeColor = lvMSU.ForeColor
              searchItem.ToolTipText = searchItem.ToolTipText.Substring(0, searchItem.ToolTipText.LastIndexOf(vbNewLine))
              Exit For
            End If
          Next
        End If
      ElseIf msuData.DisplayName.ToLower.Contains("-kb2857650-") Then
        Dim Req As Boolean = False
        For Each searchItem As ListViewItem In lvMSU.Items
          If searchItem.Text.ToLower.Contains("kb2574819") Then
            Req = True
            Exit For
          End If
        Next
        If Req Then
          For Each searchItem As ListViewItem In lvMSU.Items
            If searchItem.Text.ToLower.Contains("kb2830477") Then
              searchItem.ForeColor = lvMSU.ForeColor
              searchItem.ToolTipText = searchItem.ToolTipText.Substring(0, searchItem.ToolTipText.LastIndexOf(vbNewLine))
              Exit For
            End If
          Next
        End If
      End If
      lvItem.ToolTipText = ttItem
      Select Case GetUpdateType(sUpdate)
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
        Case UpdateType.LIP
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(msuData.Architecture & " LIP")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
        Case UpdateType.EXE
          If msuData.DisplayName = "Windows Update Agent" Then
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
      If String.IsNullOrEmpty(IO.Path.GetExtension(sUpdate)) Then
        Return New AddResult(False, "Unknown Update Type.")
      Else
        Return New AddResult(False, "Unknown Update Type: """ & IO.Path.GetExtension(sUpdate).Substring(1).ToUpper & """.")
      End If
    Else
      Return New AddResult(False, "File doesn't exist.")
    End If
  End Function
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
      SetStatus("Idle")
      RedoColumns()
    Else
      Beep()
    End If
  End Sub
  Private Sub cmdClearMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdClearMSU.Click
    If lvMSU.Items.Count > 0 Then
      If MsgDlg(Me, IIf(lvMSU.Items.Count > 2, "All " & lvMSU.Items.Count & " updates", "All updates") & " will be removed from the list.", "Do you want to clear the Update List?", "Remove All Updates", MessageBoxButtons.YesNo, TaskDialogIcon.Delete, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
        lvMSU.Items.Clear()
        SetStatus("Idle")
        RedoColumns()
      End If
    Else
      Beep()
    End If
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
    If IO.File.Exists(txtISO.Text) Then
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
      WriteToOutput("Extracting Comment from """ & txtISO.Text & """...")
      Dim sComment As String = ExtractComment(txtISO.Text)
      If Not String.IsNullOrEmpty(sComment) Then txtISOLabel.Text = sComment
      SetStatus("Idle")
    End If
  End Sub
  Private Sub cmdISO_Click(sender As System.Object, e As System.EventArgs) Handles cmdISO.Click
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "Windows 7 ISO|*.ISO|All Files|*.*"
      cdlBrowse.Title = "Choose ISO to Save Image To"
      cdlBrowse.ShowReadOnly = False
      If Not String.IsNullOrEmpty(txtISO.Text) Then cdlBrowse.InitialDirectory = txtISO.Text
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtISO.Text = cdlBrowse.FileName
      End If
    End Using
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
  Private Sub cmbISOFormat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbISOFormat.SelectedIndexChanged
    If Me.Tag Is Nothing Then
      mySettings.DefaultFS = cmbISOFormat.Text
    End If
  End Sub
#End Region
#Region "Merge"
  Private Sub chkMerge_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMerge.CheckedChanged
    txtMerge.Enabled = chkMerge.Checked
    cmdMerge.Enabled = chkMerge.Checked
    lvImages.ShowGroups = chkMerge.Checked
    If chkMerge.Checked Then
      txtMerge_TextChanged(txtMerge, New EventArgs)
    Else
      ClearImageList("MERGE")
    End If
    If lvImages.ShowGroups Then
      For Each lvItem As ListViewItem In lvImages.Items
        If lvItem.Tag(0) = "WIM" Then
          lvItem.Group = lvImages.Groups("WIM")
        ElseIf lvItem.Tag(0) = "MERGE" Then
          lvItem.Group = lvImages.Groups("MERGE")
        End If
      Next
    Else
      For Each lvItem As ListViewItem In lvImages.Items
        lvItem.Group = Nothing
      Next
    End If
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
    Using cdlBrowse As New OpenFileDialog
      cdlBrowse.Filter = "INSTALL.WIM Sources|INSTALL.WIM;*.ISO|INSTALL.WIM|INSTALL.WIM|Windows 7 ISO|*.ISO|All Files|*.*"
      cdlBrowse.Title = "Choose INSTALL.WIM Image to Merge"
      cdlBrowse.ShowReadOnly = False
      If Not String.IsNullOrEmpty(txtMerge.Text) Then cdlBrowse.InitialDirectory = txtMerge.Text
      If cdlBrowse.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        txtMerge.Text = cdlBrowse.FileName
      End If
    End Using
  End Sub
#End Region
#Region "Packages"
  Private Sub lvImages_DoubleClick(sender As Object, e As System.EventArgs) Handles lvImages.DoubleClick
    If lvImages.SelectedItems IsNot Nothing AndAlso lvImages.SelectedItems.Count > 0 Then
      Dim propData As PackageInfoEx
      Dim Path As String = Nothing
      Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
      Dim ParseWorkWIM As String = ParseWork & "WIM" & IO.Path.DirectorySeparatorChar
      Dim ParseWorkMerge As String = ParseWork & "Merge" & IO.Path.DirectorySeparatorChar
      lvImages.SelectedItems(0).Checked = Not lvImages.SelectedItems(0).Checked
      propData = lvImages.SelectedItems(0).Tag(1)
      Dim props As New frmPackageProps(propData)
      props.txtName.Text = lvImages.SelectedItems(0).SubItems(1).Text
      If props.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        lvImages.SelectedItems(0).SubItems(1).Text = props.txtName.Text
      End If
    End If
  End Sub
  Private Delegate Sub ClearImageListInvoker(ToClear As String)
  Private Sub ClearImageList(ToClear As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New ClearImageListInvoker(AddressOf ClearImageList), ToClear)
    Else
      If String.IsNullOrEmpty(ToClear) Then
        lvImages.Items.Clear()
      Else
        For Each lvItem As ListViewItem In lvImages.Items
          If lvItem.Tag(0) = ToClear Then lvItem.Remove()
        Next
      End If
      RedoColumns()
    End If
  End Sub
  Private Delegate Sub ImageListInvoker(lvItem As ListViewItem)
  Private Sub AddToImageList(lvItem As ListViewItem)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New ImageListInvoker(AddressOf AddToImageList), lvItem)
    Else
      lvImages.Items.Add(lvItem)
      If chkMerge.Checked Then
        If lvItem.Tag(0) = "WIM" Then
          lvItem.Group = lvImages.Groups("WIM")
        ElseIf lvItem.Tag(0) = "MERGE" Then
          lvItem.Group = lvImages.Groups("MERGE")
        End If
      End If
      chkSP_CheckedChanged(chkSP, New EventArgs)
    End If
  End Sub
  Private Sub ClearLister(ToClear As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New ClearImageListInvoker(AddressOf ClearLister), ToClear)
    Else
      If ToClear = "WIM" Then
        tLister = Nothing
      ElseIf ToClear = "MERGE" Then
        tLister2 = Nothing
      Else
        tLister = Nothing
        tLister2 = Nothing
      End If
    End If
  End Sub
  Private Delegate Sub ParseImageListInvoker(ToRun As String)
  Private Sub ParseImageList(ToRun As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New ParseImageListInvoker(AddressOf ParseImageList), ToRun)
    Else
      SetDisp(MNGList.Delete)
      SetTitle("Parsing WIM Packages", "Reading data from Windows Image package descriptor...")
      ToggleInputs(False)
      If ToRun = "WIM" Then
        ParseMainWIM()
      ElseIf ToRun = "MERGE" Then
        ParseMergeWIM()
      Else
        ParseMainWIM()
        ParseMergeWIM()
      End If
      ClearLister(ToRun)
      ToggleInputs(True)
      FreshDraw()
    End If
  End Sub
  Private Sub lvImages_ItemChecked(sender As Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvImages.ItemChecked
    If lblActivity.Text = "Idle" Or lblActivity.Text.Contains("Estimated Time") Then
      Dim runTime As Long = CalculateRunTime()
      If runTime > 0 Then
        lblActivity.Text = "Estimated Time: " & ConvertTime(runTime, True, True)
      End If
    End If
  End Sub
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
        cmbLimit.Items.AddRange({"700 MB (CD)", "4095 MB (32-Bit Limit)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)"})
        If chkUEFI.Checked Then
          cmbLimit.SelectedIndex = 1
        Else
          cmbLimit.SelectedIndex = 2
        End If
      Case 2
        cmbLimit.Enabled = True
        cmbLimit.Items.Clear()
        cmbLimit.Items.AddRange({"700 MB (CD)", "4700 MB (DVD)", "8500 MB (Dual-Layer DVD)"})
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
      cmbLimit.Text = iLimit & " MB"
      mySettings.SplitVal = cmbLimit.Text
    End If
  End Sub
#End Region
#Region "Control"
  Private Sub cmbPriority_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbPriority.SelectedIndexChanged
    If Me.Tag Is Nothing Then
      Select Case cmbPriority.SelectedIndex
        Case 0 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.RealTime
        Case 1 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        Case 2 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        Case 3 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Normal
        Case 4 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.BelowNormal
        Case 5 : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Idle
        Case Else : Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.Normal
      End Select
      mySettings.Priority = cmbPriority.Text
    End If
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
          MsgDlg(Me, "Unable to open the folder for """ & sPath & """!", "Unable to open folder.", "Folder was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , ex.Message)
        End Try
      Else
        MsgDlg(Me, "Unable to find the file """ & sPath & """!", "Unable to find completed Image.", "File was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
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
        If MsgDlg(Me, "Do you want to cancel the " & sProc & " proceess?", "SLIPS7REAM is busy " & sActivity & ".", "Stop " & sTitle & "?", MessageBoxButtons.YesNo, TaskDialogIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
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
      Return lblActivity.Text
    End If
  End Function
  Private Delegate Sub SetStatusInvoker(Message As String)
  Private Sub SetStatus(Message As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New SetStatusInvoker(AddressOf SetStatus), Message)
    Else
      lblActivity.Text = Message
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
      If Message = "Idle" Then
        Dim runTime As Long = CalculateRunTime()
        If runTime > 0 Then
          lblActivity.Text = "Estimated Time: " & ConvertTime(runTime, True, True)
        End If
      End If
      Application.DoEvents()
    End If
  End Sub
  Private Delegate Sub SetTitleInvoker(Title As String, Tooltip As String)
  Private Sub SetTitle(Title As String, Tooltip As String)
    If Me.InvokeRequired Then
      Me.Invoke(New SetTitleInvoker(AddressOf SetTitle), Title, Tooltip)
    Else
      sTitleText = Title
      If String.IsNullOrWhiteSpace(Tooltip) Then
        ttInfo.SetTooltip(pctTitle, Nothing)
      Else
        ttInfo.SetTooltip(pctTitle, Tooltip)
      End If
    End If
  End Sub
  Private Delegate Sub SetProgressInvoker(Value As Integer, Maximum As Integer)
  Public Sub SetProgress(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New SetProgressInvoker(AddressOf SetProgress), Value, Maximum)
    Else
      If Value = 0 And Maximum = 0 Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        pbIndividual.Value = 0
        pbIndividual.Maximum = 1
      ElseIf Value <= Maximum Then
        pbIndividual.Style = ProgressBarStyle.Continuous
        If pbIndividual.Value > Maximum Then
          pbIndividual.Value = Value
          pbIndividual.Maximum = Maximum
        Else
          pbIndividual.Maximum = Maximum
          pbIndividual.Value = Value
        End If
      Else
        pbIndividual.Style = ProgressBarStyle.Continuous
        pbIndividual.Value = 0
        pbIndividual.Maximum = Maximum
      End If
    End If
  End Sub
  Private startProg, startTot As Long
  Private Delegate Sub SetTimeInvoker(Time As Long)
  Private timeInd As Long
  Private timeDone As Long
  Public Sub SetProgressTime(Optional Time As Long = -1)
    If Me.InvokeRequired Then
      Me.Invoke(New SetTimeInvoker(AddressOf SetProgressTime), Time)
    Else
      If Time >= 0 Then
        startProg = TickCount()
        timeInd = Time
      End If
      Dim left As Long = timeInd - (TickCount() - startProg)
      If left < 1000 Then
        lblIndividualTime.Text = "finishing"
        timeInd = 0
        If lblTotalTime.Text = "finishing" Then tmrCountdown.Enabled = False
      Else
        lblIndividualTime.Text = ConvertTime(left, True, True)
        lblIndividualTime.Visible = True
        tmrCountdown.Enabled = True
      End If
    End If
  End Sub
  Public Sub SetTotal(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New SetProgressInvoker(AddressOf SetTotal), Value, Maximum)
    Else
      pbTotal.Maximum = Maximum
      If Value > pbTotal.Maximum Then
        pbTotal.Value = pbTotal.Maximum
        If taskBar IsNot Nothing Then taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NOPROGRESS)
      Else
        pbTotal.Value = Value
        If taskBar IsNot Nothing Then
          taskBar.SetProgressState(Me.Handle, TaskbarLib.TBPFLAG.TBPF_NORMAL)
          taskBar.SetProgressValue(Me.Handle, Value, Maximum)
        End If
      End If
    End If
  End Sub
  Private timeTot As Long
  Public Sub SetTotalTime(Optional Time As Long = -1)
    If Me.InvokeRequired Then
      Me.Invoke(New SetTimeInvoker(AddressOf SetTotalTime), Time)
    Else
      If Time >= 0 Then
        startTot = TickCount()
        timeTot = Time
      End If
      Dim left As Long = timeTot - (TickCount() - startTot)

      If left < 1000 Then
        lblTotalTime.Text = "finishing"
        timeTot = 0
        timeDone = TickCount()
        If lblIndividualTime.Text = "finishing" Then tmrCountdown.Enabled = False
      Else
        lblTotalTime.Text = ConvertTime(left, True, True)
        lblTotalTime.Visible = True
        tmrCountdown.Enabled = True
      End If
    End If
  End Sub
  Private Sub tmrCountdown_Tick(sender As System.Object, e As System.EventArgs) Handles tmrCountdown.Tick
    If timeInd >= 0 Then
      SetProgressTime()
    Else
      SetProgressTime(0)
    End If
    If timeTot >= 0 Then
      SetTotalTime()
    Else
      SetTotalTime(0)
    End If
  End Sub
  Private Sub expOutput_Closed(sender As Object, e As System.EventArgs) Handles expOutput.Closed
    ttInfo.SetTooltip(expOutput, "Show Output consoles.")
    ttInfo.SetTooltip(expOutput.pctExpander, "Show Output consoles.")
    If outputWindow Then
      txtOutput.Text = frmOutput.txtOutput.Text
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
      txtOutputError.Text = frmOutput.txtOutputError.Text
      If txtOutputError.Text.Length > 0 Then
        txtOutputError.SelectionStart = txtOutputError.Text.Length - 1
        txtOutputError.SelectionLength = 0
        txtOutputError.ScrollToCaret()
      End If
      frmOutput.Hide()
      outputWindow = False
    Else
      pnlSlips7ream.SuspendLayout()
      pctOutputTear.Visible = False
      txtOutput.Visible = False
      txtOutputError.Visible = False
      Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
      Me.Height -= HeightDifferentialA
      pnlSlips7ream.ResumeLayout(True)
    End If
  End Sub
  Private Sub expOutput_Opened(sender As System.Object, e As System.EventArgs) Handles expOutput.Opened
    ttInfo.SetTooltip(expOutput, "Hide Output consoles.")
    ttInfo.SetTooltip(expOutput.pctExpander, "Hide Output consoles.")
    If outputWindow Then
      frmOutput.Show(Me)
      frmOutput.Location = New Point(Me.Left, Me.Bottom)
      frmOutput.txtOutput.Text = txtOutput.Text
      If frmOutput.txtOutput.Text.Length > 0 Then
        frmOutput.txtOutput.SelectionStart = frmOutput.txtOutput.Text.Length - 1
        frmOutput.txtOutput.SelectionLength = 0
        frmOutput.txtOutput.ScrollToCaret()
      End If
      frmOutput.txtOutputError.Text = txtOutputError.Text
      If frmOutput.txtOutputError.Text.Length > 0 Then
        frmOutput.txtOutputError.SelectionStart = frmOutput.txtOutputError.Text.Length - 1
        frmOutput.txtOutputError.SelectionLength = 0
        frmOutput.txtOutputError.ScrollToCaret()
      End If
    Else
      pnlSlips7ream.SuspendLayout()
      Me.Height += HeightDifferentialA
      Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height + HeightDifferentialA)
      pctOutputTear.Visible = True
      txtOutput.Visible = True
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
      txtOutputError.Visible = True
      If txtOutputError.Text.Length > 0 Then
        txtOutputError.SelectionStart = txtOutputError.Text.Length - 1
        txtOutputError.SelectionLength = 0
        txtOutputError.ScrollToCaret()
      End If
      pnlSlips7ream.ResumeLayout(True)
      redrawCaption()
    End If
  End Sub
  Private Delegate Sub WriteToOutputCallBack(Message As String)
  Private Sub WriteToOutput(Message As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New WriteToOutputCallBack(AddressOf WriteToOutput), Message)
    Else
      If outputWindow Then
        frmOutput.txtOutput.AppendText(Message & vbNewLine)
      Else
        txtOutput.AppendText(Message & vbNewLine)
      End If
    End If
  End Sub
  Private Sub WriteToError(Message As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New WriteToOutputCallBack(AddressOf WriteToError), Message)
    Else
      If outputWindow Then
        frmOutput.txtOutputError.AppendText(Message & vbNewLine)
      Else
        txtOutputError.AppendText(Message & vbNewLine)
      End If
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
        txtOutputError.Visible = False
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
        frmOutput.txtOutputError.Text = txtOutputError.Text
        If frmOutput.txtOutputError.Text.Length > 0 Then
          frmOutput.txtOutputError.SelectionStart = frmOutput.txtOutputError.Text.Length - 1
          frmOutput.txtOutputError.SelectionLength = 0
          frmOutput.txtOutputError.ScrollToCaret()
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
    Else
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
      txtOutputError.Visible = True
      txtOutputError.Text = frmOutput.txtOutputError.Text
      If txtOutputError.Text.Length > 0 Then
        txtOutputError.SelectionStart = txtOutputError.Text.Length - 1
        txtOutputError.SelectionLength = 0
        txtOutputError.ScrollToCaret()
      End If
      pnlSlips7ream.ResumeLayout(True)
      frmOutput.Hide()
    End If
  End Sub
#End Region
#End Region
#Region "Command Calls"
  Private Function CleanMounts() As Boolean
    SetProgressTime(SpeedStats.Clean_MOUNT)
    Dim mountStart As Long = TickCount()
    Try
      Dim DISMInfo As String = RunWithReturn(DismPath, "/Get-MountedWimInfo /English", True)
      Dim mFindA As String = WorkDir.Substring(0, WorkDir.Length - 1).ToLower
      Dim mFindB As String = ShortenPath(mFindA).ToLower
      If Not DISMInfo.Contains("No mounted images found.") Then
        SetStatus("Cleaning Old DISM Mounts...")
        Dim sLines() As String = Split(DISMInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Dir : ") Then
            Dim tmpPath As String = line.Substring(line.IndexOf(":") + 2)
            If line.ToLower.Contains(mFindA) Or line.ToLower.Contains(mFindB) Then RunHidden(DismPath, "/Unmount-Wim /MountDir:" & ShortenPath(tmpPath) & " /discard /English")
          End If
        Next
      End If
      RunHidden(DismPath, "/cleanup-wim")
      Dim ImageXInfo As String = RunWithReturn(AIKDir & "imagex", "/unmount", True)
      If Not ImageXInfo.Contains("Number of Mounted Images: 0") Then
        SetStatus("Cleaning Old ImageX Mounts...")
        Dim sLines() As String = Split(ImageXInfo, vbNewLine)
        For Each line In sLines
          If line.Contains("Mount Path") Then
            Dim tmpPath As String = line.Substring(line.IndexOf("[") + 1)
            tmpPath = tmpPath.Substring(0, tmpPath.IndexOf("]"))
            If tmpPath.ToLower.Contains(mFindA) Or tmpPath.ToLower.Contains(mFindB) Then RunHidden(AIKDir & "imagex", "/unmount " & ShortenPath(tmpPath))
          End If
        Next
      End If
      RunHidden(AIKDir & "imagex", "/cleanup")
      WriteToOutput("Deleting """ & WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar & """...")
      SlowDeleteDirectory(WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar, FileIO.DeleteDirectoryOption.DeleteAllContents)
      If IO.Directory.Exists(WorkDir & "MOUNT" & IO.Path.DirectorySeparatorChar) Then Return False
      Return True
    Catch ex As Exception
      Return False
    Finally
      SpeedStats.Clean_MOUNT = TickCount() - mountStart
    End Try
  End Function
#Region "DISM"
  Private Function InitDISM(WIMFile As String, WIMIndex As Integer, MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(DismPath, "/Mount-Wim /WimFile:" & ShortenPath(WIMFile) & " /index:" & WIMIndex.ToString.Trim & " /MountDir:" & ShortenPath(MountPath) & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMPackages(WIMFile As String) As Integer
    Dim PackageList As String = RunWithReturn(DismPath, "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile) & " /English")
    Dim PackageRows() As String = Split(PackageList, vbNewLine)
    Dim Indexes As Integer = 0
    For Each row In PackageRows
      If row.StartsWith("Index : ") Then
        Indexes += 1
      End If
    Next
    Return Indexes
  End Function
  Private Function GetDISMPackageData(WIMFile As String, Index As Integer) As PackageInfoEx
    Dim PackageList As String = RunWithReturn(DismPath, "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile) & " /index:" & Index & " /English")
    Dim Info As New PackageInfoEx(PackageList)
    Return Info
  End Function
  Private Function AddToDism(MountPath As String, AddPath As String) As Boolean
    Dim sRet As String = RunWithReturn(DismPath, "/Image:" & ShortenPath(MountPath) & " /Add-Package /PackagePath:" & ShortenPath(AddPath) & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function SaveDISM(MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(DismPath, "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /commit" & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DiscardDISM(MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(DismPath, "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /discard /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
#End Region
#Region "IMAGEX"
  Private Function SplitWIM(SourceWIM As String, DestSWM As String, Size As Integer) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "imagex", "/split " & SourceWIM & " " & DestSWM & " " & Size)
    Return sRet.Contains("Successfully split")
  End Function
  Private Function ExportWIM(SourceWIM As String, SourceIndex As Integer, DestWIM As String, DestName As String) As Boolean
    ReturnProgress = True
    Dim sRet As String = RunWithReturn(AIKDir & "imagex", "/export """ & SourceWIM & """ " & SourceIndex & " """ & DestWIM & """ """ & DestName & """ /compress maximum")
    ReturnProgress = False
    Return sRet.Contains("Successfully exported image")
  End Function
  Private Function UpdateLang(SourcePath As String) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "intlcfg", "-genlangini -dist:" & ShortenPath(SourcePath) & " -image:" & ShortenPath(Mount) & " -f")
    If Not sRet.Contains("A new Lang.ini file has been generated") Then
      Return False
    End If
    Dim MountPath As String = WorkDir & "BOOT" & IO.Path.DirectorySeparatorChar
    If Not IO.Directory.Exists(MountPath) Then IO.Directory.CreateDirectory(MountPath)
    ReturnProgress = True
    sRet = RunWithReturn(AIKDir & "imagex", "/mountrw " & ShortenPath(SourcePath & "sources" & IO.Path.DirectorySeparatorChar & "boot.wim") & " 2 " & ShortenPath(MountPath))
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
    sRet = RunWithReturn(AIKDir & "imagex", "/unmount /commit " & ShortenPath(MountPath))
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
    Dim sRet As String
    If String.IsNullOrEmpty(BootOrderFile) Then
      sRet = RunWithReturn(AIKDir & "oscdimg", "-m " & FileSystem & " " & ShortenPath(FromDir.Substring(0, FromDir.Length - 1)) & " " & ShortenPath(DestISO) & " -l" & Label)
    Else
      sRet = RunWithReturn(AIKDir & "oscdimg", "-m " & FileSystem & " -yo" & ShortenPath(BootOrderFile) & " -b" & ShortenPath(FromDir & "boot" & IO.Path.DirectorySeparatorChar & "etfsboot.com") & " " & ShortenPath(FromDir.Substring(0, FromDir.Length - 1)) & " " & ShortenPath(DestISO) & " -l" & Label)
    End If
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
    Else
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
          MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Data Error"
          MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Unsupported Method"
          MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "File Not Found"
          MsgDlg(Me, "Unable to find files in " & IO.Path.GetFileName(Source) & "!", "The files were not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case Else
          MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
      End Select
    End If
  End Sub
  Private Sub AsyncExtractAllFiles(Obj As Object)
    Dim Source, Destination As String, cIndex As UInteger
    Source = Obj(0)
    Destination = Obj(1)
    cIndex = Obj(2)
    Dim sStatus As String = GetStatus()
    If sStatus.EndsWith("...") Then sStatus = sStatus.Substring(0, sStatus.Length - 3)
    If sStatus.Contains(" (File ") Then sStatus = sStatus.Substring(0, sStatus.IndexOf(" (File"))
    If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source))
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
    SetProgress(0, 1000)
    c_ExtractRet(cIndex) = "OK"
  End Sub
  Private Delegate Sub ExtractFilesInvoker(Source As String, Destination As String, Except As String)
  Private Sub ExtractFiles(Source As String, Destination As String, Except As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ExtractFilesInvoker(AddressOf ExtractFiles), Source, Destination, Except)
    Else
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
          MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Data Error"
          MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Unsupported Method"
          MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract files!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "File Not Found"
          MsgDlg(Me, "Unable to find files in " & IO.Path.GetFileName(Source) & "!", "The files were not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case Else
          MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
      End Select
    End If
  End Sub
  Private Sub AsyncExtractFiles(Obj As Object)
    Dim Source, Destination, Except As String, cIndex As UInteger
    Source = Obj(0)
    Destination = Obj(1)
    Except = Obj(2)
    cIndex = Obj(3)
    Dim sStatus As String = GetStatus()
    If sStatus.EndsWith("...") Then sStatus = sStatus.Substring(0, sStatus.Length - 3)
    If sStatus.Contains(" (File ") Then sStatus = sStatus.Substring(0, sStatus.IndexOf(" (File"))
    If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source))
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
    SetProgress(0, 1000)
    c_ExtractRet(cIndex) = "OK"
  End Sub
  Private Delegate Sub ExtractAFileInvoker(Source As String, Destination As String, File As String)
  Private Sub ExtractAFile(Source As String, Destination As String, File As String)
    If Me.InvokeRequired Then
      Me.Invoke(New ExtractAFileInvoker(AddressOf ExtractAFile), Source, Destination, File)
    Else
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
      Select Case sRet
        Case "OK"

        Case "CRC Error"
          MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Data Error"
          MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "Unsupported Method"
          MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to extract """ & File & """!", "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case "File Not Found"
          MsgDlg(Me, "Unable to find """ & File & """ in " & IO.Path.GetFileName(Source) & "!", "The file was not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        Case Else
          MsgDlg(Me, sRet, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
      End Select
    End If
  End Sub
  Private Sub AsyncExtractAFile(Obj As Object)
    Dim Source, Destination, Find As String
    Source = Obj(0)
    Destination = Obj(1)
    Find = Obj(2)
    Dim cIndex As UInteger = Obj(3)
    Dim bFound As Boolean = False
    If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar

    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source))
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
    SetProgress(0, 1000)
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
    Else
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
            MsgDlg(Me, "CRC Error in " & IO.Path.GetFileName(Source) & " while attempting to read the file!", "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
          Case "Data Error"
            MsgDlg(Me, "Data Error in " & IO.Path.GetFileName(Source) & " while attempting to read the file!", "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
          Case "Unsupported Method"
            MsgDlg(Me, "Unsupported Method in " & IO.Path.GetFileName(Source) & " while attempting to read the file!", "There was an error while extracting.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
          Case "File Not Found"
            MsgDlg(Me, "Unable to find any files in " & IO.Path.GetFileName(Source) & "!", "No files were found.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
          Case Else
            MsgDlg(Me, sRet, "There was an error while reading.", "File read error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
        End Select
        Return Nothing
      End If
    End If
  End Function
  Private Sub AsyncExtractFilesList(Obj As Object)
    Dim Source As String
    Source = Obj(0)
    Dim cIndex As UInteger = Obj(1)
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source))
    Try
      Extractor.Open()
    Catch ex As Exception
      Extractor.Dispose()
      Extractor = Nothing
      c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
      Return
    End Try
    Dim sList As New List(Of String)
    Dim eFiles() As Extraction.COM.IArchiveEntry = Extractor.ToArray
    For Each file As Extraction.COM.IArchiveEntry In eFiles
      sList.Add(file.Name)
    Next
    If sList.Count > 0 Then
      c_ExtractRet(cIndex) = Join(sList.ToArray, "|")
    Else
      c_ExtractRet(cIndex) = "File Not Found"
    End If
  End Sub
  Private Function ExtractComment(Source As String) As String
    Extractor = New Extraction.ArchiveFile(New IO.FileInfo(Source))
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
        SetProgress(e.Item.Index, Extractor.ItemCount)
        Application.DoEvents()
      End If
    End If
  End Sub
  Private Sub Extractor_ExtractProgress(sender As Object, e As Extraction.COM.ExtractProgressEventArgs) Handles Extractor.ExtractProgress
    If Extractor.ExtractionCount = 1 AndAlso e.Total > 1048576 * 64 Then
      If StopRun Then e.ContinueOperation = False
      Dim iMax As Integer = pbIndividual.Width
      Dim iVal As Integer = (e.Written / e.Total) * iMax
      If pbIndividual.Value = iVal AndAlso pbIndividual.Maximum = iMax Then Return
      SetProgress(iVal, iMax)
      Application.DoEvents()
    End If
  End Sub
  Private Function ExtractFailureAlert(Message As String) As Boolean
    If String.IsNullOrEmpty(Message) Then Return False
    If Message = "OK" Then
      Return False
    ElseIf Message.StartsWith("CRC Error") Then
      MsgDlg(Me, "CRC Error in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
    ElseIf Message.StartsWith("Data Error") Then
      MsgDlg(Me, "Data Error in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
    ElseIf Message.StartsWith("Unsupported Method") Then
      MsgDlg(Me, "Unsupported Method in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
    ElseIf Message.StartsWith("File Not Found") Then
      MsgDlg(Me, "Unable to find expected file in compressed file!" & vbNewLine & Message.Substring(Message.IndexOf("|") + 1), "The file was not found.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
    Else
      MsgDlg(Me, Message, "There was an error while extracting.", "File extraction error.", MessageBoxButtons.OK, TaskDialogIcon.Error)
    End If
    Return True
  End Function
#End Region
#Region "EXE2CAB"
  Private Function EXE2CAB(Source As String, Destination As String) As Boolean
    Dim ExeToCab As String = AIKDir & "exe2cab.exe"
    RunHidden(ExeToCab, """" & Source & """ -q """ & Destination & """")
    Return IO.File.Exists(Destination)
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
      Me.BeginInvoke(New RunWithReturnRetCallBack(AddressOf RunWithReturnRet), Index, Output)
    Else
      If Output.StartsWith("!" & vbNewLine) Then
        Output = Output.Substring(3)
      Else
        WriteToOutput(Output)
      End If
      c_RunWithReturnRet(Index) = Output
    End If
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
      Me.BeginInvoke(New RunWithReturnRetCallBack(AddressOf AsyncRunWithReturnRet), Index, Output)
    Else
      If Output Is Nothing Then
        c_RunWithReturnRet(Index) = "!" & vbNewLine & c_RunWithReturnAccumulation(Index)
        c_RunWithReturnAccumulation(Index) = Nothing
      Else
        If ReturnProgress Then
          If Output.Contains(" ] Exporting progress") Or Output.Contains(" ] Mounting progress") Or Output.Contains(" ] Committing Image progress") Or Output.Contains(" ] Unmounting progress") Or Output.Contains(" ] Mount cleanup progress") Then
            Dim ProgI As String = Output.Substring(2)
            ProgI = ProgI.Substring(0, ProgI.IndexOf("%"))
            SetProgress(Val(ProgI), 100)
          End If
        End If
        c_RunWithReturnAccumulation(Index) &= Output & vbNewLine
        WriteToOutput(Output)
      End If
    End If
  End Sub
  Private Sub AsyncRunWithReturnErrorRet(Index As Integer, Output As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New RunWithReturnRetCallBack(AddressOf AsyncRunWithReturnErrorRet), Index, Output)
    Else
      If Output IsNot Nothing And ReturnProgress Then
        If Output.Contains("% complete") Then
          Dim ProgI As String = Output.Substring(0, Output.IndexOf("%"))
          SetProgress(Val(ProgI), 100)
        End If
      End If
      WriteToError(Output)
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
      Me.BeginInvoke(New RunHiddenCallBack(AddressOf RunHiddenRet), Index)
    Else
      c_RunHiddenRet(Index) = True
    End If
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
  Private Function CalculateRunTime() As Long
    Dim lTotalTime As Long = 0
    Dim sWIMSize As String = CalculateCompatibleSize(txtWIM.Text)
    Dim sMergeSize As String = CalculateCompatibleSize(txtMerge.Text)
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
      If IO.File.Exists(txtWIM.Text) Then
        lTotalTime += SpeedStats.WIM_ExtractFromISO(sWIMSize)
        Using isoExtract As New Extraction.ArchiveFile(New IO.FileInfo(txtWIM.Text))
          Dim bOpen As Boolean = False
          Try
            isoExtract.Open()
            bOpen = True
          Catch ex As Exception
            bOpen = False
          End Try
          If bOpen Then
            Dim eFiles() As Extraction.COM.IArchiveEntry = isoExtract.ToArray
            For Each item As Extraction.COM.IArchiveEntry In eFiles
              Dim ext As String = IO.Path.GetExtension(item.Name).ToLower
              If item.Name.ToLower.Contains("install") And (ext = ".wim" Or ext = ".swm") Then
                Dim wimCopySize As String = Math.Round(item.Size / 1024 / 1024 / 1024, 1, MidpointRounding.ToEven) & "GB"
                lTotalTime += SpeedStats.WIM_MoveImage(wimCopySize)
              End If
            Next
          End If
        End Using
      End If
    End If
    If chkMerge.Checked Then
      If Not String.IsNullOrEmpty(txtMerge.Text) Then
        If IO.Path.GetExtension(txtMerge.Text).ToLower = ".iso" Then
          If IO.File.Exists(txtMerge.Text) Then
            lTotalTime += SpeedStats.WIM_ExtractFromISO(sMergeSize)
            Using isoExtract As New Extraction.ArchiveFile(New IO.FileInfo(txtMerge.Text))
              Dim bOpen As Boolean = False
              Try
                isoExtract.Open()
                bOpen = True
              Catch ex As Exception
                bOpen = False
              End Try
              If bOpen Then
                Dim eFiles() As Extraction.COM.IArchiveEntry = isoExtract.ToArray
                For Each item As Extraction.COM.IArchiveEntry In eFiles
                  Dim ext As String = IO.Path.GetExtension(item.Name).ToLower
                  If item.Name.ToLower.Contains("install") And (ext = ".wim" Or ext = ".swm") Then
                    Dim wimCopySize As String = Math.Round(item.Size / 1024 / 1024 / 1024, 1, MidpointRounding.ToEven) & "GB"
                    lTotalTime += SpeedStats.WIM_MoveImage(wimCopySize)
                  End If
                Next
              End If
            End Using
            If IO.Directory.Exists(Work & "Merge" & IO.Path.DirectorySeparatorChar) Then lTotalTime += SpeedStats.Clean_WORK
            lTotalTime += SpeedStats.Clean_WORK
          End If
        End If
      End If
    End If
    Dim imageCount32 As Integer = 0
    Dim imageCount64 As Integer = 0
    Dim imageCountAll As Integer = 0
    If lvImages.Items.Count > 0 Then
      For Each row As ListViewItem In lvImages.Items
        If row.Checked Then
          If row.SubItems(1).Text.Contains("x64") Then
            imageCount64 += 1
          Else
            imageCount32 += 1
          End If
        End If
      Next
      imageCountAll = imageCount32 + imageCount64
      If imageCountAll > 0 Then
        lTotalTime += SpeedStats.WIM_MergeImage("First")
        If imageCountAll > 1 Then lTotalTime += SpeedStats.WIM_MergeImage("Additional") * (imageCountAll - 1)
      End If
    End If

    If chkSP.Checked Then
      If Not String.IsNullOrEmpty(txtSP.Text) Then
        If Not String.IsNullOrEmpty(txtSP64.Text) Then
          lTotalTime += SpeedStats.SP_Extract("86")
          lTotalTime += SpeedStats.SP_Integrate("86") * imageCount32
          lTotalTime += SpeedStats.Clean_SP1("86")
          lTotalTime += SpeedStats.SP_Extract("64")
          lTotalTime += SpeedStats.SP_Integrate("64") * imageCount64
          lTotalTime += SpeedStats.Clean_SP1("64")

          If IO.Directory.Exists(Work & "Merge" & IO.Path.DirectorySeparatorChar) Then lTotalTime += SpeedStats.Clean_WORK
        Else
          Dim isx64 As Boolean = False
          For Each row As ListViewItem In lvImages.Items
            If row.SubItems(1).Text.Contains("x64") Then
              isx64 = True
              Exit For
            End If
          Next
          If isx64 Then
            lTotalTime += SpeedStats.SP_Extract("64")
            lTotalTime += SpeedStats.SP_Integrate("64") * imageCount64
            lTotalTime += SpeedStats.Clean_SP1("64")
          Else
            lTotalTime += SpeedStats.SP_Extract("86")
            lTotalTime += SpeedStats.SP_Integrate("86") * imageCount32
            lTotalTime += SpeedStats.Clean_SP1("86")
          End If
        End If
      End If
    End If

    If lvMSU.Items.Count > 0 Then
      Dim hasx86 As Boolean = False
      Dim hasx64 As Boolean = False
      For Each row As ListViewItem In lvMSU.Items
        Dim fileSize As String = CalculateCompatibleSize(row.Tag)
        If row.SubItems(1).Text.Contains("x86") Then
          hasx86 = True
          Select Case GetUpdateType(row.Tag)
            Case UpdateType.MSU : lTotalTime += SpeedStats.Update_Integrate("MSU", "86", fileSize) * imageCount32
            Case UpdateType.LIP : lTotalTime += SpeedStats.Update_Integrate("LIP", "86", fileSize) * imageCount32
            Case UpdateType.LP : lTotalTime += SpeedStats.Update_Integrate("LP", "86", fileSize) * imageCount32
            Case UpdateType.EXE : lTotalTime += SpeedStats.Update_Integrate("EXE", "86", fileSize) * imageCount32
            Case UpdateType.CAB : lTotalTime += SpeedStats.Update_Integrate("CAB", "86", fileSize) * imageCount32
          End Select
        ElseIf row.SubItems(1).Text.Contains("x64") Or row.SubItems(1).Text.Contains("amd64") Then
          hasx64 = True
          Select Case GetUpdateType(row.Tag)
            Case UpdateType.MSU : lTotalTime += SpeedStats.Update_Integrate("MSU", "64", fileSize) * imageCount64
            Case UpdateType.LIP : lTotalTime += SpeedStats.Update_Integrate("LIP", "64", fileSize) * imageCount64
            Case UpdateType.LP : lTotalTime += SpeedStats.Update_Integrate("LP", "64", fileSize) * imageCount64
            Case UpdateType.EXE : lTotalTime += SpeedStats.Update_Integrate("EXE", "64", fileSize) * imageCount64
            Case UpdateType.CAB : lTotalTime += SpeedStats.Update_Integrate("CAB", "64", fileSize) * imageCount64
          End Select
        End If
      Next
      If hasx86 Then
        lTotalTime += SpeedStats.WIM_MountImage("86") * imageCount32
        lTotalTime += SpeedStats.WIM_SaveImage("86") * imageCount32
      End If
      If hasx64 Then
        lTotalTime += SpeedStats.WIM_MountImage("64") * imageCount64
        lTotalTime += SpeedStats.WIM_SaveImage("64") * imageCount64
      End If
    End If

    If chkISO.Checked Then
      If Not String.IsNullOrEmpty(txtISO.Text) Then
        Dim sISOSize As String = CalculateCompatibleSize(txtISO.Text)
        lTotalTime += SpeedStats.NOWIM_ExtractFromISO(sISOSize)
        If imageCountAll > 0 Then
          For I As Integer = 1 To imageCountAll
            lTotalTime += SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional"))
          Next
        End If
        If cmbLimitType.SelectedIndex > 0 Then
          Dim splUEFI As Boolean = chkUEFI.Checked
          Dim limVal As String = cmbLimit.Text
          If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
          Dim splFileSize As Long = NumericVal(limVal)
          If IO.File.Exists(txtWIM.Text) Then
            Dim splWIMSize As Long = New IO.FileInfo(txtWIM.Text).Length
            If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then splWIMSize -= 351
            Dim splISOSize As Long = splWIMSize + 351
            Dim sEstWIMSize As String = CalculateCompatibleSizeVal(splWIMSize)
            Dim sEstWIMSizePlus As String = CalculateCompatibleSizeVal(splWIMSize + (1024 * 1024 * 1024))
            If cmbLimitType.SelectedIndex = 2 Then
              If splISOSize > splFileSize Then
                Dim ISOSplit As Long = splFileSize
                Dim WIMSplit As Long = ISOSplit - 351
                If splUEFI And (WIMSplit > 4095) Then WIMSplit = 4095
                Dim lSplitTime As Long = SpeedStats.WIM_SplitImage(sEstWIMSize, WIMSplit & "MB")
                If lSplitTime < 1 Then lSplitTime = SpeedStats.WIM_SplitImage(sEstWIMSizePlus, WIMSplit & "MB")
                lTotalTime += lSplitTime
                If Math.Floor(ISOSplit / WIMSplit) > 1 Then
                  lTotalTime += SpeedStats.ISO_Make * (Math.Ceiling(splWIMSize / WIMSplit) / (Math.Floor(ISOSplit / WIMSplit)))
                ElseIf Math.Floor(ISOSplit / WIMSplit) = 1 Then
                  lTotalTime += SpeedStats.ISO_Make * Math.Ceiling(splWIMSize / WIMSplit)
                Else
                  lTotalTime += SpeedStats.ISO_Make * Math.Ceiling(splWIMSize / WIMSplit)
                End If
              ElseIf splUEFI And splWIMSize > 4095 Then
                Dim lSplitTime As Long = SpeedStats.WIM_SplitImage(sEstWIMSize, "4095MB")
                If lSplitTime < 1 Then lSplitTime = SpeedStats.WIM_SplitImage(sEstWIMSizePlus, "4095MB")
                lTotalTime += lSplitTime
              End If
            Else
              If splWIMSize > splFileSize Then
                Dim lSplitTime As Long = SpeedStats.WIM_SplitImage(sEstWIMSize, splFileSize & "MB")
                If lSplitTime < 1 Then lSplitTime = SpeedStats.WIM_SplitImage(sEstWIMSizePlus, splFileSize & "MB")
                lTotalTime += lSplitTime
              End If
            End If
          End If
        End If
        lTotalTime += SpeedStats.ISO_Make
      End If
    Else
      If imageCountAll > 0 Then
        For I As Integer = 1 To imageCountAll
          lTotalTime += SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional"))
        Next
      End If
      If cmbLimitType.SelectedIndex > 0 Then
        Dim limVal As String = cmbLimit.Text
        If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
        Dim splFileSize As Long = NumericVal(limVal)
        If IO.File.Exists(txtWIM.Text) Then
          Dim splWIMSize As Long = New IO.FileInfo(txtWIM.Text).Length
          If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then splWIMSize -= 351
          If splWIMSize > splFileSize Then
            Dim sEstWIMSize As String = CalculateCompatibleSizeVal(splWIMSize)
            Dim sEstWIMSizePlus As String = CalculateCompatibleSizeVal(splWIMSize + (1024 * 1024 * 1024))
            Dim lSplitTime As Long = SpeedStats.WIM_SplitImage(sEstWIMSize, splFileSize & "MB")
            If lSplitTime < 1 Then lSplitTime = SpeedStats.WIM_SplitImage(sEstWIMSizePlus, splFileSize & "MB")
            lTotalTime += lSplitTime
          End If
        End If
      End If
    End If
    Return lTotalTime
  End Function
  Private Sub Slipstream()
    RunComplete = False
    StopRun = False
    LangChange = False
    timeDone = 0
    If txtISOLabel.Enabled And txtISOLabel.Text.Contains(" ") Then
      SetStatus("Spaces are not allowed in the ISO Label!")
      txtISOLabel.Text = Replace(txtISOLabel.Text, " ", "_")
      txtISOLabel.Focus()
      Return
    End If
    Dim workEstim, workStart, workSpeed As Long

    SetTitle("Preparing Images", "Cleaning up mounts and extracting WIM from ISO if necessary...")
    RunActivity = 1
    ToggleInputs(False)
    Dim WIMFile As String = Nothing
    If IO.Directory.Exists(WorkDir) Then
      SetProgress(0, 1)
      If Not CleanMounts() Then
        ToggleInputs(True)
        SetStatus("Active Mount Detected!")
        cmdConfig.Focus()
        Beep()
        MsgDlg(Me, "The selected Temp directory has an active mount that could not be removed. Please restart your computer or change your Temp directory before continuing.", "Active mount has been detected.", "Unable to Begin Slipstream Process", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked)
        Return
      End If
      SetStatus("Clearing Old Data...")
      Try
        WriteToOutput("Deleting """ & WorkDir & """...")
        workEstim = SpeedStats.Clean_WORK
        SetProgressTime(workEstim)
        workStart = TickCount()
        SlowDeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
        workSpeed = TickCount() - workStart
        SpeedStats.Clean_WORK = workSpeed
        Application.DoEvents()
      Catch ex As Exception
        Application.DoEvents()
      End Try
    End If
    Dim iTotalVal As Integer = 0
    Dim iTotalMax As Integer = 2
    If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then iTotalMax += 1
    If lvMSU.Items.Count > 0 Then iTotalMax += 1
    If Not String.IsNullOrEmpty(txtSP.Text) Then
      iTotalMax += 1
      If Not String.IsNullOrEmpty(txtSP64.Text) Then iTotalMax += 1
    End If
    If Not String.IsNullOrEmpty(txtISO.Text) Then iTotalMax += 3

    SetTotal(0, iTotalMax)
    SetProgress(0, 1)
    pbTotal.Style = ProgressBarStyle.Continuous

    SetDisp(MNGList.Copy)
    SetStatus("Checking WIM...")
    If String.IsNullOrEmpty(txtWIM.Text) OrElse Not IO.File.Exists(txtWIM.Text) Then
      ToggleInputs(True)
      SetStatus("Missing WIM File!")
      txtWIM.Focus()
      Beep()
      Return
    Else
      SetStatus("Calculating Time...")
      SetTotalTime(CalculateRunTime)
      If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
        SetProgress(0, 1)
        SetStatus("Extracting Image from ISO...")
        WriteToOutput("Extracting ""INSTALL.WIM"" from """ & txtWIM.Text & """ to """ & Work & """...")
        Dim wimSize As String = CalculateCompatibleSize(txtWIM.Text)
        workEstim = SpeedStats.WIM_ExtractFromISO(wimSize)
        SetProgressTime(workEstim)
        workStart = TickCount()
        ExtractAFile(txtWIM.Text, Work, "INSTALL.WIM")
        workSpeed = TickCount() - workStart
        SpeedStats.WIM_ExtractFromISO(wimSize) = workSpeed
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        WIMFile = Work & "INSTALL.WIM"
      Else
        WIMFile = txtWIM.Text
      End If
    End If
    If StopRun Then
      ToggleInputs(True)
      Return
    End If
    SetDisp(MNGList.Delete)
    SetTitle("Merging Image Packages", "Merging all WIM packages into single WIM...")

    SetStatus("Checking Merge...")
    Dim MergeFile As String = Nothing
    If chkMerge.Checked Then
      If String.IsNullOrEmpty(txtMerge.Text) OrElse Not IO.File.Exists(txtMerge.Text) Then
        ToggleInputs(True)
        SetStatus("Missing Merge File!")
        txtSP.Focus()
        Beep()
        Return
      Else
        MergeFile = txtMerge.Text
      End If
    Else
      MergeFile = Nothing
    End If

    Dim MergeWIM As String = Nothing
    Dim MergeWork As String = Work & "Merge" & IO.Path.DirectorySeparatorChar
    If Not String.IsNullOrEmpty(MergeFile) Then
      If IO.Directory.Exists(MergeWork) Then
        WriteToOutput("Deleting """ & MergeWork & """...")
        workEstim = SpeedStats.Clean_WORK
        SetProgressTime(workEstim)
        workStart = TickCount()
        Try
          SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
        End Try
        workSpeed = TickCount() - workStart
        SpeedStats.Clean_WORK = workSpeed
      End If
      IO.Directory.CreateDirectory(MergeWork)
      Dim MergeWorkExtract As String = MergeWork & "Extract" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(MergeWorkExtract) Then IO.Directory.CreateDirectory(MergeWorkExtract)
      If IO.Path.GetExtension(MergeFile).ToLower = ".iso" Then
        SetProgress(0, 1)
        SetStatus("Extracting Merge Image from ISO...")
        WriteToOutput("Extracting ""INSTALL.WIM"" from """ & MergeFile & """ to """ & MergeWorkExtract & """...")
        Dim wimSize As String = CalculateCompatibleSize(MergeFile)
        workEstim = SpeedStats.WIM_ExtractFromISO(wimSize)
        SetProgressTime(workEstim)
        workStart = TickCount()
        ExtractAFile(MergeFile, MergeWorkExtract, "INSTALL.WIM")
        workSpeed = TickCount() - workStart
        SpeedStats.WIM_ExtractFromISO(wimSize) = workSpeed
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

    SetProgress(0, 100)
    Dim NewWIM As String = Work & "newINSTALL.wim"
    SetStatus("Generating Image...")
    For Each lvRow As ListViewItem In lvImages.Items
      If lvRow.Checked Then
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
        SetStatus("Merging WIM """ & RowName & """...")
        SetProgress(0, 100)
        Dim sFirst As String = "First"
        If IO.File.Exists(NewWIM) Then sFirst = "Additional"
        workEstim = SpeedStats.WIM_MergeImage(sFirst)
        SetProgressTime(workEstim)
        workStart = TickCount()
        If ExportWIM(RowImage, RowIndex, NewWIM, RowName) Then
          workSpeed = TickCount() - workStart
          SpeedStats.WIM_MergeImage(sFirst) = workSpeed
          Continue For
        Else
          ToggleInputs(True)
          SetStatus("Failed to Merge WIM """ & RowName & """")
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
      workEstim = SpeedStats.Clean_WORK
      SetProgressTime(workEstim)
      workStart = TickCount()
      Try
        SlowDeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Catch ex As Exception
      End Try
      workSpeed = TickCount() - workStart
      SpeedStats.Clean_WORK = workSpeed
    End If
    SetProgress(0, 1)
    iTotalVal += 1
    SetTotal(iTotalVal, iTotalMax)
    SetStatus("Making Backup of Old WIM...")
    Dim BakWIM As String = WorkDir & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(WIMFile & "_BAK.WIM")
    WriteToOutput("Copying """ & WIMFile & """ to """ & BakWIM & """...")
    Dim wimCopySize As String = Math.Round(New IO.FileInfo(WIMFile).Length / 1024 / 1024 / 1024, 1, MidpointRounding.ToEven) & "GB"
    workEstim = SpeedStats.WIM_MoveImage(wimCopySize)
    SetProgressTime(workEstim)
    workStart = TickCount()
    If Not SlowCopyFile(WIMFile, BakWIM, True) Then
      ToggleInputs(True)
      SetStatus("Failed to back up Install WIM!")
      Return
    End If
    workSpeed = TickCount() - workStart
    If workSpeed > 500 Then SpeedStats.WIM_MoveImage(wimCopySize) = workSpeed
    SetStatus("Moving Generated WIM...")
    WriteToOutput("Copying """ & NewWIM & """ to """ & WIMFile & """...")
    wimCopySize = Math.Round(New IO.FileInfo(NewWIM).Length / 1024 / 1024 / 1024, 1, MidpointRounding.ToEven) & "GB"
    workEstim = SpeedStats.WIM_MoveImage(wimCopySize)
    SetProgressTime(workEstim)
    workStart = TickCount()
    If Not SlowCopyFile(NewWIM, WIMFile, True) Then
      SetStatus("Generated WIM Move Failed! Reverting to Old WIM...")
      WriteToOutput("Copying """ & BakWIM & """ to """ & WIMFile & """...")
      If Not SlowCopyFile(BakWIM, WIMFile, True) Then
        ToggleInputs(True)
        SetStatus("Generated WIM Move Failed! Original WIM Restore Failed!")
      Else
        ToggleInputs(True)
        SetStatus("Generated WIM Move Failed! Original WIM Restored.")
      End If
      Return
    End If
    workSpeed = TickCount() - workStart
    If workSpeed > 500 Then SpeedStats.WIM_MoveImage(wimCopySize) = workSpeed
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
        ToggleInputs(True)
        SetStatus("Missing Service Pack File!")
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
        ToggleInputs(True)
        SetStatus("Missing x64 Service Pack File!")
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
        ToggleInputs(True)
        SetStatus("Missing ISO File!")
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
    Dim UpdateFiles As New Collections.Generic.List(Of UpdateInfo)
    If lvMSU.Items.Count > 0 Then
      For Each lvItem As ListViewItem In lvMSU.Items
        UpdateFiles.Add(New UpdateInfo(lvItem.Tag, True))
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
        SetStatus("Integrating Service Pack...")
        If IntegrateSP(WIMFile, SPFile) Then
          iTotalVal += 1
          SetTotal(iTotalVal, iTotalMax)
          SetStatus("Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      Else
        SetStatus("Integrating x86 Service Pack...")
        If IntegrateSP(WIMFile, SPFile, "86") Then
          iTotalVal += 1
          SetTotal(iTotalVal, iTotalMax)
          SetStatus("x86 Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
        SetStatus("Integrating x64 Service Pack...")
        If IntegrateSP(WIMFile, SP64File, "64") Then
          iTotalVal += 1
          SetTotal(iTotalVal, iTotalMax)
          SetStatus("x64 Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      End If
    End If
    SetDisp(MNGList.Copy)
    SetTitle("Adding Windows Updates", "Integrating update data into WIM packages...")

    Dim NoMount As Boolean = False
    If UpdateFiles.Count > 0 Then
      SetStatus("Integrating Updates...")
      If IntegrateFiles(WIMFile, UpdateFiles.ToArray) Then
        iTotalVal += 1
        SetTotal(iTotalVal, iTotalMax)
        SetStatus("Updates Integrated!")
      Else
        Dim sMsg As String = lblActivity.Text
        ToggleInputs(True)
        lblActivity.Text = sMsg
        Return
      End If
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Return
      End If
    Else
      If chkUEFI.Checked Then
        SetStatus("Mounting Final WIM for UEFI boot data...")
        InitDISM(WIMFile, GetDISMPackages(WIMFile), Mount)
      Else
        NoMount = True
      End If
    End If
    SetDisp(MNGList.Move)
    If Not String.IsNullOrEmpty(ISOFile) Then
      SetTitle("Generating ISO", "Preparing WIMs and file structures, and writing ISO data...")
      Dim ISODir As String = Work & "ISO" & IO.Path.DirectorySeparatorChar
      Dim ISODDir As String = Work & "ISOD%n" & IO.Path.DirectorySeparatorChar

      If Not IO.Directory.Exists(ISODir) Then IO.Directory.CreateDirectory(ISODir)
      SetProgress(0, 1)
      SetStatus("Extracting ISO contents...")
      WriteToOutput("Extracting Setup Disc files from """ & ISOFile & """ to """ & ISODir & """...")
      Dim isoSize As String = CalculateCompatibleSize(ISOFile)
      workEstim = SpeedStats.NOWIM_ExtractFromISO(isoSize)
      SetProgressTime(workEstim)
      workStart = TickCount()
      ExtractFiles(ISOFile, ISODir, "install.wim")
      workSpeed = TickCount() - workStart
      SpeedStats.NOWIM_ExtractFromISO(isoSize) = workSpeed
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)

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
          Dim sMsg As String = lblActivity.Text
          If Not NoMount Then DiscardDISM(Mount)
          ToggleInputs(True)
          lblActivity.Text = sMsg
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
              MsgDlg(Me, "Unable to find the file """ & IO.Path.DirectorySeparatorChar & "Windows" & IO.Path.DirectorySeparatorChar & "Boot" & IO.Path.DirectorySeparatorChar & "EFI" & IO.Path.DirectorySeparatorChar & "bootmgfw.efi"" in Image!", "Unable to enable UEFI Boot.", "File was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
            End If
          Else
            chkUEFI.Checked = False
            SetStatus("UEFI Boot already enabled!")
          End If
        Else
          chkUEFI.Checked = False
          MsgDlg(Me, "Unable to find the folder """ & IO.Path.DirectorySeparatorChar & "efi" & IO.Path.DirectorySeparatorChar & "microsoft" & IO.Path.DirectorySeparatorChar & "boot" & IO.Path.DirectorySeparatorChar & """ in ISO!", "Unable to enable UEFI Boot.", "Folder was not found.", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
        End If
      End If
      If Not NoMount Then
        Dim hasx64 As Boolean = lvImages.Items(lvImages.Items.Count - 1).SubItems(1).Text.Contains("x64")
        workEstim = SpeedStats.WIM_SaveImage(IIf(hasx64, "64", "86"))
        SetProgressTime(workEstim)
        workStart = TickCount()
        SetStatus("Saving Final Image Package...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Save Final Image Package!")
          Return
        End If
        workSpeed = TickCount() - workStart
        SpeedStats.WIM_SaveImage(IIf(hasx64, "64", "86")) = workSpeed
      End If
      SetProgress(0, 1)
      SetStatus("Integrating and Compressing INSTALL.WIM...")
      Dim ISOWIMFile As String = ISODir & "sources" & IO.Path.DirectorySeparatorChar & "install.wim"
      If IO.File.Exists(ISOWIMFile) Then
        WriteToOutput("Deleting """ & ISOWIMFile & """...")
        IO.File.Delete(ISOWIMFile)
      End If
      Dim NewWIMPackageCount As Integer = GetDISMPackages(WIMFile)
      For I As Integer = 1 To NewWIMPackageCount
        Dim NewWIMPackageInfo = GetDISMPackageData(WIMFile, I)
        Dim RowIndex As String = NewWIMPackageInfo.Index
        Dim RowName As String = NewWIMPackageInfo.Name
        SetStatus("Integrating and Compressing INSTALL.WIM Package """ & RowName & """...")
        SetProgress(0, 100)
        workEstim = SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional"))
        SetProgressTime(workEstim)
        workStart = TickCount()
        If ExportWIM(WIMFile, RowIndex, ISOWIMFile, RowName) Then
          workSpeed = TickCount() - workStart
          SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional")) = workSpeed
          Continue For
        Else
          ToggleInputs(True)
          SetStatus("Failed to Integrate WIM Package """ & RowName & """")
          Return
        End If
        If StopRun Then
          ToggleInputs(True)
          Return
        End If
      Next
      SetProgress(0, 1)
      iTotalVal += 1
      SetTotal(iTotalVal, iTotalMax)

      If cmbLimitType.SelectedIndex > 0 Then
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
              workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB")
              SetProgressTime(workEstim)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              workStart = TickCount()
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                workSpeed = TickCount() - workStart
                SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB") = workSpeed
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
                      ToggleInputs(True)
                      SetStatus("Failed to move Install WIM #" & WIMNumber & "!")
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
                                ToggleInputs(True)
                                SetStatus("ISO creation cancelled by user!")
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
                        workEstim = SpeedStats.ISO_Make
                        SetProgressTime(workEstim)
                        workStart = TickCount()
                        MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                        workSpeed = TickCount() - workStart
                        SpeedStats.ISO_Make = workSpeed
                      Catch ex As Exception

                      End Try
                      DiscNumber += 1
                      WIMNumber = 0
                    End If
                  End If
                Next

              Else
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Return
              End If
            ElseIf Math.Floor(ISOSplit / WIMSplit) = 1 Then
              REM Split WIM to WIMSplit size, put 1 WIM on each ISO including first
              SetProgress(0, 0)
              workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB")
              SetProgressTime(workEstim)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              workStart = TickCount()
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                workSpeed = TickCount() - workStart
                SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB") = workSpeed
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
                      ToggleInputs(True)
                      SetStatus("Failed to move Install WIM #" & discNo & "!")
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
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
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
                      workEstim = SpeedStats.ISO_Make
                      SetProgressTime(workEstim)
                      workStart = TickCount()
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                      workSpeed = TickCount() - workStart
                      SpeedStats.ISO_Make = workSpeed
                    Catch ex As Exception

                    End Try
                  End If
                Next
              Else
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Return
              End If
            Else
              REM Split WIM to WIMSplit size, put no WIMs on first ISO, one per ISO afterward
              SetProgress(0, 0)
              workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB")
              SetProgressTime(workEstim)
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              workStart = TickCount()
              If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), WIMSplit) Then
                workSpeed = TickCount() - workStart
                SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), WIMSplit & "MB") = workSpeed
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
                      ToggleInputs(True)
                      SetStatus("Failed to move Primary Install WIM!")
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
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
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
                      workEstim = SpeedStats.ISO_Make
                      SetProgressTime(workEstim)
                      workStart = TickCount()
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                      workSpeed = TickCount() - workStart
                      SpeedStats.ISO_Make = workSpeed
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
                      ToggleInputs(True)
                      SetStatus("Failed to move Install WIM #" & discNo & "!")
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
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
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
                      workEstim = SpeedStats.ISO_Make
                      SetProgressTime(workEstim)
                      workStart = TickCount()
                      MakeISO(sIDir, isoLabel, Nothing, isoFormat, ISODFile)
                      workSpeed = TickCount() - workStart
                      SpeedStats.ISO_Make = workSpeed
                    Catch ex As Exception

                    End Try
                  End If
                Next
              Else
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Return
              End If
            End If
          ElseIf splUEFI And splWIMSize > 4095 Then
            REM Split WIM to 4095 MB size
            SetProgress(0, 0)
            workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), "4095MB")
            SetProgressTime(workEstim)
            SetStatus("Splitting WIM into 4095 MB Chunks...")
            workStart = TickCount()
            If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), 4095) Then
              workSpeed = TickCount() - workStart
              SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), "4095MB") = workSpeed
              If IO.File.Exists(ISOWIMFile) Then
                WriteToOutput("Deleting """ & ISOWIMFile & """...")
                IO.File.Delete(ISOWIMFile)
              End If
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
              Return
            End If
          End If
        Else
          If splWIMSize > splFileSize Then
            REM Split WIM to FileSize size
            SetProgress(0, 0)
            workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), splFileSize & "MB")
            SetProgressTime(workEstim)
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            workStart = TickCount()
            If SplitWIM(ISOWIMFile, IO.Path.ChangeExtension(ISOWIMFile, ".swm"), splFileSize) Then
              workSpeed = TickCount() - workStart
              SpeedStats.WIM_SplitImage(CalculateCompatibleSize(ISOWIMFile), splFileSize & "MB") = workSpeed
              If IO.File.Exists(ISOWIMFile) Then
                WriteToOutput("Deleting """ & ISOWIMFile & """...")
                IO.File.Delete(ISOWIMFile)
              End If
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
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
                  ToggleInputs(True)
                  SetStatus("ISO creation cancelled by user!")
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
        ToggleInputs(True)
        SetStatus("Failed to back up ISO!")
        Return
      End If
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
        workEstim = SpeedStats.ISO_Make
        SetProgressTime(workEstim)
        workStart = TickCount()
        Saved = MakeISO(ISODir, isoLabel, BootOrder, isoFormat, ISOFile)
        workSpeed = TickCount() - workStart
        SpeedStats.ISO_Make = workSpeed
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
          ToggleInputs(True)
          SetStatus("ISO Build and backup ISO restore failed!")
        End If
        Return
      End If
    Else
      SetTitle("Generating WIM", "Preparing WIMs and file structure...")
      If Not NoMount Then
        SetStatus("Saving Final Image Package...")
        Dim hasx64 As Boolean = lvImages.Items(lvImages.Items.Count - 1).SubItems(1).Text.Contains("x64")
        workEstim = SpeedStats.WIM_SaveImage(IIf(hasx64, "64", "86"))
        SetProgressTime(workEstim)
        workStart = TickCount()
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Save Final Image Package!")
          Return
        End If
        workSpeed = TickCount() - workStart
        SpeedStats.WIM_SaveImage(IIf(hasx64, "64", "86")) = workSpeed
      End If
      SetStatus("Compressing INSTALL.WIM...")
      Dim OldWIM As String = IO.Path.GetDirectoryName(WIMFile) & IO.Path.DirectorySeparatorChar & IO.Path.GetFileNameWithoutExtension(WIMFile & "_OLD.WIM")
      SetProgress(0, 1)
      WriteToOutput("Copying """ & WIMFile & """ to """ & OldWIM & """...")
      If Not SlowCopyFile(WIMFile, OldWIM, True) Then
        ToggleInputs(True)
        SetStatus("Failed to back up Install WIM!")
        Return
      End If
      Dim NewWIMPackageCount As Integer = GetDISMPackages(OldWIM)
      For I As Integer = 1 To NewWIMPackageCount
        Dim NewWIMPackageInfo As PackageInfoEx = GetDISMPackageData(OldWIM, I)
        Dim RowIndex As String = NewWIMPackageInfo.Index
        Dim RowName As String = NewWIMPackageInfo.Name
        SetProgress(0, 100)
        workEstim = SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional"))
        SetProgressTime(workEstim)
        workStart = TickCount()
        If ExportWIM(OldWIM, RowIndex, WIMFile, RowName) Then
          workSpeed = TickCount() - workStart
          SpeedStats.WIM_MergeAndCompressImage(IIf(I = 1, "First", "Additional")) = workSpeed
          Continue For
        Else
          ToggleInputs(True)
          SetStatus("Failed to Compress WIM """ & RowName & """")
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
            workEstim = SpeedStats.WIM_SplitImage(CalculateCompatibleSize(WIMFile), splFileSize & "MB")
            SetProgressTime(workEstim)
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            workStart = TickCount()
            If SplitWIM(WIMFile, IO.Path.ChangeExtension(WIMFile, ".swm"), splFileSize) Then
              workSpeed = TickCount() - workStart
              SpeedStats.WIM_SplitImage(CalculateCompatibleSize(WIMFile), splFileSize & "MB") = workSpeed
              WriteToOutput("Deleting """ & WIMFile & """...")
              IO.File.Delete(WIMFile)
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
              Return
            End If
          End If
        End If
      End If
    End If
    If timeDone > 0 Then
      Debug.Print("Ran for " & ConvertTime(Math.Abs(TickCount() - timeDone)) & " after the clock ran out")
    Else
      Debug.Print("Ran with " & lblTotalTime.Text & " remaining on the clock")
    End If
    SetProgress(0, 100)
    SetTotal(0, 100)
    ToggleInputs(True)
    SetStatus("Complete!")
    Select Case cmbCompletion.SelectedIndex
      Case 0
        REM done
        RunComplete = True
        ToggleInputs(True)
      Case 1
        REM play alert noise
        RunComplete = True
        ToggleInputs(True)
        If String.IsNullOrEmpty(mySettings.AlertNoisePath) Then
          My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
        ElseIf IO.File.Exists(mySettings.AlertNoisePath) Then
          Try
            My.Computer.Audio.Play(mySettings.AlertNoisePath, AudioPlayMode.Background)
          Catch ex As Exception
            mySettings.AlertNoisePath = String.Empty
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
          End Try
        Else
          mySettings.AlertNoisePath = String.Empty
          My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
        End If
      Case 2
        REM close
        RunComplete = True
        ToggleInputs(True)
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
        RunComplete = True
        CloseCleanup()
        ToggleInputs(True)
        Application.SetSuspendState(PowerState.Suspend, False, False)
        Me.Close()
    End Select
  End Sub
  Private Function IntegrateFiles(WIMPath As String, MSUPaths() As UpdateInfo) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount > 0 Then
      If MSUPaths.Length > 0 Then
        Dim DISM_32 As New Collections.Generic.List(Of PackageInfoEx)
        Dim DISM_64 As New Collections.Generic.List(Of PackageInfoEx)
        Dim MSU_32 As New Collections.Generic.List(Of UpdateInfoEx)
        Dim MSU_64 As New Collections.Generic.List(Of UpdateInfoEx)
        SetProgress(0, PackageCount)
        SetProgressTime(SpeedStats.Update_Parse("MSU", "x86"))
        For I As Integer = 1 To PackageCount
          SetProgress(I, PackageCount)
          SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
          If StopRun Then
            DiscardDISM(Mount)
            ToggleInputs(True)
            Return False
          End If
          Dim tmpDISM As PackageInfoEx = GetDISMPackageData(WIMPath, I)
          If tmpDISM.Architecture = "x86" Then
            DISM_32.Add(tmpDISM)
          ElseIf tmpDISM.Architecture = "x64" Then
            DISM_64.Add(tmpDISM)
          End If
        Next
        SetProgress(0, MSUPaths.Length)
        For I As Integer = 0 To MSUPaths.Length - 1
          If MSUPaths(I).IsFile Then
            SetProgress(I + 1, MSUPaths.Length)
            SetStatus("Loading Update """ & IO.Path.GetFileNameWithoutExtension(MSUPaths(I).Path) & """ Data...")
            If StopRun Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              Return False
            End If
            Dim tmpMSU As New UpdateInfoEx(MSUPaths(I).Path)
            If Not String.IsNullOrEmpty(tmpMSU.Failure) Then Continue For
            If String.IsNullOrEmpty(tmpMSU.Architecture) Then Continue For
            If DISM_32.Count > 0 And tmpMSU.Architecture = "x86" Then
              MSU_32.Add(tmpMSU)
            End If
            If DISM_64.Count > 0 Then
              If tmpMSU.Architecture = "amd64" Then
                MSU_64.Add(tmpMSU)
              Else
                If CheckWhitelist(tmpMSU.DisplayName) Then MSU_64.Add(tmpMSU)
              End If
            End If
          End If
        Next
        SetProgress(0, 0)
        Dim pbMax As Integer = 1
        Dim pbVal As Integer = 0
        For Each tmpDISM In DISM_32
          pbMax += MSU_32.Count + 1
        Next
        For Each tmpDISM In DISM_64
          pbMax += MSU_64.Count + 1
        Next
        SetProgress(0, pbMax)
        Dim workEstim, workStart, workSpeed As Long
        If MSU_32.Count > 0 Then
          For D As Integer = 0 To DISM_32.Count - 1
            Dim tmpDISM As PackageInfoEx = DISM_32(D)
            pbVal += 1
            SetProgress(pbVal, pbMax)
            SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
            workEstim = SpeedStats.WIM_MountImage("86")
            SetProgressTime(workEstim)
            workStart = TickCount()
            If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              SetStatus("Failed to Load Image Package """ & tmpDISM.Name & """!")
              Return False
            End If
            workSpeed = TickCount() - workStart
            SpeedStats.WIM_MountImage("86") = workSpeed
            If StopRun Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              Return False
            End If
            For I As Integer = 0 To MSU_32.Count - 1
              Dim tmpMSU = MSU_32(I)
              pbVal += 1
              SetProgress(pbVal, pbMax)
              Dim DisplayName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
              If Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
                DisplayName = "KB" & tmpMSU.KBArticle
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DisplayName) Then
                DisplayName = tmpMSU.DisplayName
              End If
              Dim fileSize As String = CalculateCompatibleSize(tmpMSU.Path)
              SetStatus((I + 1).ToString.Trim & "/" & MSU_32.Count.ToString & " - Integrating " & DisplayName & " into " & tmpDISM.Name & "...")
              Dim upType = GetUpdateType(tmpMSU.Path)
              workEstim = SpeedStats.Update_Integrate(TypeToString(upType), "86", fileSize)
              SetProgressTime(workEstim)
              workStart = TickCount()
              Select Case upType
                Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
                  If Not AddToDism(Mount, tmpMSU.Path) Then
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If upType = UpdateType.LP Then LangChange = True
                Case UpdateType.EXE
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
                    ToggleInputs(True)
                    SetStatus("Failed to extract " & DisplayName & " from EXE to CAB!")
                    Return False
                  End If
                  Dim cabList() As String = ExtractFilesList(tmpCAB)
                  If cabList.Contains("update.mum") Then
                    If Not AddToDism(Mount, tmpCAB) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True)
                      SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                      Return False
                    End If
                  ElseIf cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                    Dim useEXE As String = "WUA-Downlevel.exe"
                    If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                    ExtractAFile(tmpCAB, WorkDir, useEXE)
                    If Not IO.File.Exists(WorkDir & useEXE) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True)
                      SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If

                      If Not AddToDism(Mount, WorkDir & useCab1) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      If Not AddToDism(Mount, WorkDir & useCab2) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      If Not AddToDism(Mount, WorkDir & useCab3) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If

                    End If
                  Else
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                  End If

                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
                Case UpdateType.LIP
                  Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  WriteToOutput("Copying """ & tmpMSU.Path & """ to """ & tmpCAB & """...")
                  My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
                  If Not AddToDism(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
              End Select
              workSpeed = TickCount() - workStart
              SpeedStats.Update_Integrate(TypeToString(upType), "86", fileSize) = workSpeed
              If StopRun Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                Return False
              End If
            Next
            Dim DoSave As Boolean = True
            If DISM_64.Count = 0 Then
              If D = DISM_32.Count - 1 Then
                DoSave = False
              End If
            End If
            If DoSave Then
              SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
              workEstim = SpeedStats.WIM_SaveImage("86")
              SetProgressTime(workEstim)
              workStart = TickCount()
              If Not SaveDISM(Mount) Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                SetStatus("Failed to Save Image Package """ & tmpDISM.Name & """!")
                Return False
              End If
              workSpeed = TickCount() - workStart
              SpeedStats.WIM_SaveImage("86") = workSpeed
            End If
            If StopRun Then
              ToggleInputs(True)
              Return False
            End If
          Next
        End If
        If MSU_64.Count > 0 Then
          For D As Integer = 0 To DISM_64.Count - 1
            Dim tmpDISM As PackageInfoEx = DISM_64(D)
            pbVal += 1
            SetProgress(pbVal, pbMax)
            SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
            workEstim = SpeedStats.WIM_MountImage("64")
            SetProgressTime(workEstim)
            workStart = TickCount()
            If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              SetStatus("Failed to Load Image Package """ & tmpDISM.Name & """!")
              Return False
            End If
            workSpeed = TickCount() - workStart
            SpeedStats.WIM_MountImage("64") = workSpeed
            If StopRun Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              Return False
            End If
            For I As Integer = 0 To MSU_64.Count - 1
              Dim tmpMSU = MSU_64(I)
              pbVal += 1
              SetProgress(pbVal, pbMax)
              Dim DisplayName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
              If Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
                DisplayName = "KB" & tmpMSU.KBArticle
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DisplayName) Then
                DisplayName = tmpMSU.DisplayName
              End If
              Dim fileSize As String = CalculateCompatibleSize(tmpMSU.Path)
              SetStatus((I + 1).ToString.Trim & "/" & MSU_64.Count.ToString & " - Integrating " & DisplayName & " into " & tmpDISM.Name & "...")
              Dim upType = GetUpdateType(tmpMSU.Path)
              workEstim = SpeedStats.Update_Integrate(TypeToString(upType), "64", fileSize)
              SetProgressTime(workEstim)
              workStart = TickCount()
              Select Case upType
                Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
                  If Not AddToDism(Mount, tmpMSU.Path) Then
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If upType = UpdateType.LP Then LangChange = True
                Case UpdateType.EXE
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
                    ToggleInputs(True)
                    SetStatus("Failed to extract " & DisplayName & " from EXE to CAB!")
                    Return False
                  End If
                  Dim cabList() As String = ExtractFilesList(tmpCAB)
                  If cabList.Contains("update.mum") Then
                    If Not AddToDism(Mount, tmpCAB) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True)
                      SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                      Return False
                    End If
                  ElseIf cabList.Contains("WUA-Downlevel.exe") And cabList.Contains("WUA-Win7SP1.exe") Then
                    Dim useEXE As String = "WUA-Downlevel.exe"
                    If chkSP.Checked Or tmpDISM.SPLevel > 0 Then useEXE = "WUA-Win7SP1.exe"
                    ExtractAFile(tmpCAB, WorkDir, useEXE)
                    If Not IO.File.Exists(WorkDir & useEXE) Then
                      If IO.File.Exists(tmpCAB) Then
                        WriteToOutput("Deleting """ & tmpCAB & """...")
                        IO.File.Delete(tmpCAB)
                      End If
                      DiscardDISM(Mount)
                      ToggleInputs(True)
                      SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
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
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If

                      If Not AddToDism(Mount, WorkDir & useCab1) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab1 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      If Not AddToDism(Mount, WorkDir & useCab2) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab2 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If
                      If Not AddToDism(Mount, WorkDir & useCab3) Then
                        If IO.File.Exists(tmpCAB) Then
                          WriteToOutput("Deleting """ & tmpCAB & """...")
                          IO.File.Delete(tmpCAB)
                        End If
                        If IO.File.Exists(WorkDir & useEXE) Then
                          WriteToOutput("Deleting """ & WorkDir & useEXE & """...")
                          IO.File.Delete(WorkDir & useEXE)
                        End If
                        DiscardDISM(Mount)
                        ToggleInputs(True)
                        SetStatus("Failed to integrate " & DisplayName & " (" & useCab3 & ") into " & tmpDISM.Name & "!")
                        Return False
                      End If

                    End If
                  Else
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
                Case UpdateType.LIP
                  Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  WriteToOutput("Copying """ & tmpMSU.Path & """ to """ & tmpCAB & """...")
                  My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
                  If Not AddToDism(Mount, tmpCAB) Then
                    If IO.File.Exists(tmpCAB) Then
                      WriteToOutput("Deleting """ & tmpCAB & """...")
                      IO.File.Delete(tmpCAB)
                    End If
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate " & DisplayName & " into " & tmpDISM.Name & "!")
                    Return False
                  End If
                  If IO.File.Exists(tmpCAB) Then
                    WriteToOutput("Deleting """ & tmpCAB & """...")
                    IO.File.Delete(tmpCAB)
                  End If
                  LangChange = True
              End Select
              workSpeed = TickCount() - workStart
              SpeedStats.Update_Integrate(TypeToString(upType), "64", fileSize) = workSpeed
              If StopRun Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                Return False
              End If
            Next
            Dim DoSave As Boolean = True
            If D = DISM_64.Count - 1 Then
              DoSave = False
            End If
            If DoSave Then
              SetStatus("Saving Image Package """ & tmpDISM.Name & """...")
              workEstim = SpeedStats.WIM_SaveImage("64")
              SetProgressTime(workEstim)
              Dim saveStart As Long = TickCount()
              If Not SaveDISM(Mount) Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                SetStatus("Failed to Save Image Package """ & tmpDISM.Name & """!")
                Return False
              End If
              workSpeed = TickCount() - workStart
              SpeedStats.WIM_SaveImage("64") = workSpeed
            End If
            If StopRun Then
              ToggleInputs(True)
              Return False
            End If
          Next
        End If
      End If
      Return True
    Else
      ToggleInputs(True)
      SetStatus("No packages in WIM!")
      Return False
    End If
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
        Dim dismData As PackageInfoEx = GetDISMPackageData(WIMPath, I)
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
    Dim pbMax As Integer = (ActivePackages * 3) + 14
    SetProgress(0, 0)
    SetStatus("Extracting Service Pack...")
    Dim workEstim, workStart, workSpeed As Long
    workEstim = SpeedStats.SP_Extract(activeArch)
    SetProgressTime(workEstim)
    workStart = TickCount()
    RunHidden(SPPath, "/x:""" & Work & "SP1""")
    SetProgress(1, pbMax)
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
      ToggleInputs(True)
      SetStatus("No KB976932.cab to extract!")
      Return False
    End If
    SetProgress(2, pbMax)
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
      ToggleInputs(True)
      SetStatus("No NestedMPPcontent.cab to extract!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    SetProgress(3, pbMax)
    Dim Update As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "update.ses"
    If IO.File.Exists(Update) Then
      SetStatus("Preparing Service Pack (Modifying update.ses)...")
      UpdateSES(Update)
    Else
      ToggleInputs(True)
      SetStatus("No update.ses to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    SetProgress(4, pbMax)
    Update = Work & "SP1" & IO.Path.DirectorySeparatorChar & "update.mum"
    If IO.File.Exists(Update) Then
      SetStatus("Preparing Service Pack (Modifying update.mum)...")
      UpdateMUM(Update)
    Else
      ToggleInputs(True)
      SetStatus("No update.mum to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    SetProgress(5, pbMax)
    Dim Update86 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "Windows7SP1-KB976933~31bf3856ad364e35~x86~~6.1.1.17514.mum"
    Dim Update64 As String = Work & "SP1" & IO.Path.DirectorySeparatorChar & "Windows7SP1-KB976933~31bf3856ad364e35~amd64~~6.1.1.17514.mum"
    If IO.File.Exists(Update86) Then
      SetStatus("Preparing Service Pack (Modifying KB976933.mum)...")
      UpdateMUM(Update86)
    ElseIf IO.File.Exists(Update64) Then
      SetStatus("Preparing Service Pack (Modifying KB976933.mum)...")
      UpdateMUM(Update64)
    Else
      ToggleInputs(True)
      SetStatus("No KB976933.mum to modify!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    SetProgress(6, pbMax)
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
      Extract = Work & "SP1" & IO.Path.DirectorySeparatorChar & "KB976933-LangsCab" & I.ToString.Trim & ".cab"
      If IO.File.Exists(Extract) Then
        SetStatus("Preparing Service Pack (Extracting Language CAB " & (I + 1).ToString.Trim & " of 7)...")
        WriteToOutput("Extracting """ & Extract & """ to """ & Work & "SP1""...")
        ExtractAllFiles(Extract, Work & "SP1")
        WriteToOutput("Deleting """ & Extract & """...")
        IO.File.Delete(Extract)
      Else
        ToggleInputs(True)
        SetStatus("No KB976933-LangsCab" & I.ToString.Trim & ".cab to extract!")
        Return False
      End If
      SetProgress(7 + I, pbMax)
      If StopRun Then
        ToggleInputs(True)
        Return False
      End If
    Next
    workSpeed = TickCount() - workStart
    SpeedStats.SP_Extract(activeArch) = workSpeed
    SetProgress(14, pbMax)
    Dim iProg As Integer = 14
    If PackageCount > 0 Then
      workEstim = SpeedStats.SP_Integrate(activeArch)
      SetProgressTime(workEstim * PackageCount)
      Dim iRunCount As Integer = 0
      workStart = TickCount()
      For I As Integer = 1 To PackageCount
        Dim dismData As PackageInfoEx = GetDISMPackageData(WIMPath, I)
        Try
          If dismData.SPLevel > 0 Then Continue For
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not dismData.Architecture.Contains(Architecture) Then
            Continue For
          End If
        Catch ex As Exception

        End Try
        iRunCount += 1
        SetProgress(iProg, pbMax)
        SetStatus("Integrating Service Pack (Loading " & dismData.Name & ")...")
        If Not InitDISM(WIMPath, I, Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Load Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        iProg += 1
        SetProgress(iProg, pbMax)
        SetStatus("Integrating Service Pack into " & dismData.Name & "...")
        If Not AddToDism(Mount, Work & "SP1") Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Add Service Pack to Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          Return False
        End If
        iProg += 1
        SetProgress(iProg, pbMax)
        SetStatus("Integrating Service Pack (Saving " & dismData.Name & ")...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Save Image Package """ & dismData.Name & """!")
          Return False
        End If
        If StopRun Then
          ToggleInputs(True)
          Return False
        End If
        iProg += 1
        SetProgress(iProg, pbMax)
      Next
      workSpeed = (TickCount() - workStart) / iRunCount
      SpeedStats.SP_Integrate(activeArch) = workSpeed
    Else
      ToggleInputs(True)
      SetStatus("No packages in WIM!")
      Return False
    End If
    SetProgress(0, pbMax)
    SetStatus("Clearing Temp Files...")
    WriteToOutput("Deleting """ & Work & "SP1""...")
    workEstim = SpeedStats.Clean_SP1(activeArch)
    SetProgressTime(workEstim)
    workStart = TickCount()
    SlowDeleteDirectory(Work & "SP1", FileIO.DeleteDirectoryOption.DeleteAllContents)
    workSpeed = TickCount() - workStart
    SpeedStats.Clean_SP1(activeArch) = workSpeed
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
  Private Function CalculateCompatibleSize(fromFile As String) As String
    If String.IsNullOrEmpty(fromFile) Then Return Nothing
    If Not IO.File.Exists(fromFile) Then Return Nothing
    Return CalculateCompatibleSizeVal(New IO.FileInfo(fromFile).Length)
  End Function
  Private Function CalculateCompatibleSizeVal(fromSize As Long) As String
    If fromSize > 1024L * 1024L Then
      If fromSize > 1024L * 1024L * 1024L Then
        Return Math.Round(fromSize / 1024 / 1024 / 1024, 1, MidpointRounding.ToEven) & "GB"
      Else
        Return Math.Ceiling(fromSize / 1024 / 1024) & "MB"
      End If
    Else
      Return "1MB"
    End If
  End Function
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
    SetTotal(0, 3)
    Dim imgSize As String = CalculateCompatibleSize(sWIM)
    SetTotalTime(SpeedStats.WIM_ParseImage(imgSize))
    Dim parseStart As Long = TickCount()
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      SetTotal(1, 3)
      SetProgressTime(SpeedStats.WIM_ExtractFromISO(imgSize))
      Dim extractStart As Long = TickCount()
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      SpeedStats.WIM_ExtractFromISO(imgSize) = TickCount() - extractStart
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If IO.File.Exists(WIMFile) Then
      SetTotal(2, 3)
      SetStatus("Reading Image Packages...")
      ClearImageList("WIM")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      SetTotal(3, 3)
      SetStatus("Populating Image Package List...")
      For I As Integer = 1 To PackageCount
        SetProgress(I, PackageCount)
        SetProgressTime(0)
        Dim Package As PackageInfoEx = GetDISMPackageData(WIMFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
        If Package.Equals(New PackageInfoEx) Then Return
        If Package.Architecture = "x64" And Not Package.Name.Contains("64") Then Package.Name &= " x64"
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        lvItem.Tag = {"WIM", Package}
        Dim ttItem As String = Package.Desc & IIf(Package.SPLevel > 0, " Service Pack " & Package.SPLevel, "") & vbNewLine &
          "Version " & Package.Version & "." & Package.SPBuild & vbNewLine &
          Package.ProductType & " " & Package.Edition & " " & Package.Architecture & vbNewLine &
          Package.Files & " files, " & Package.Directories & " folders" & vbNewLine &
          Package.Modified & vbNewLine &
          ShortenPath(WIMFile)
        lvItem.ToolTipText = ttItem
        AddToImageList(lvItem)
      Next
      If tLister2 Is Nothing Then
        SetStatus("Idle")
      Else
        SetStatus("Extracting Merge Image from ISO...")
      End If
    Else
      SetStatus("Could not Extract Image from ISO!")
    End If
    SpeedStats.WIM_ParseImage(imgSize) = TickCount() - parseStart
    SetProgress(0, 10)
    SetTotal(0, 3)
  End Sub
  Private Sub ParseMergeWIM()
    Dim sMerge As String = txtMerge.Text
    Dim ParseWork As String = Work & "PARSE" & IO.Path.DirectorySeparatorChar
    Dim ParseWorkMerge As String = ParseWork & "Merge" & IO.Path.DirectorySeparatorChar
    IO.Directory.CreateDirectory(ParseWorkMerge)
    If String.IsNullOrEmpty(sMerge) Then Return
    Dim MergeFile As String = String.Empty
    SetTotal(0, 3)
    Dim imgSize As String = CalculateCompatibleSize(sMerge)
    SetTotalTime(SpeedStats.WIM_ParseImage(imgSize))
    Dim parseStart As Long = TickCount()
    If IO.Path.GetExtension(sMerge).ToLower = ".iso" Then
      SetStatus("Extracting Merge Image from ISO...")
      SetTotal(1, 3)
      SetProgressTime(SpeedStats.WIM_ExtractFromISO(imgSize))
      Dim extractStart As Long = TickCount()
      ExtractAFile(sMerge, ParseWorkMerge, "INSTALL.WIM")
      SpeedStats.WIM_ExtractFromISO(imgSize) = TickCount() - extractStart
      MergeFile = ParseWorkMerge & "INSTALL.WIM"
    Else
      MergeFile = sMerge
    End If
    If IO.File.Exists(MergeFile) Then
      SetStatus("Reading Merge Image Packages...")
      SetTotal(2, 3)
      ClearImageList("MERGE")
      Dim PackageCount As Integer = GetDISMPackages(MergeFile)
      SetStatus("Populating Merge Image Package List...")
      SetTotal(3, 3)
      For I As Integer = 1 To PackageCount
        SetProgress(I, PackageCount)
        SetProgressTime(0)
        Dim Package As PackageInfoEx = GetDISMPackageData(MergeFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
        If Package.Equals(New PackageInfoEx) Then Return
        If Package.Architecture = "x64" And Not Package.Name.Contains("64") Then Package.Name &= " x64"
        lvItem.Checked = True
        lvItem.SubItems.Add(Package.Name)
        lvItem.SubItems.Add(ByteSize(Package.Size))
        lvItem.Tag = {"MERGE", Package}
        Dim ttItem As String = Package.Desc & IIf(Package.SPLevel > 0, " Service Pack " & Package.SPLevel, "") & vbNewLine &
          "Version " & Package.Version & "." & Package.SPBuild & vbNewLine &
          Package.ProductType & " " & Package.Edition & " " & Package.Architecture & vbNewLine &
          Package.Files & " files, " & Package.Directories & " folders" & vbNewLine &
          Package.Modified & vbNewLine &
          ShortenPath(MergeFile)
        lvItem.ToolTipText = ttItem
        AddToImageList(lvItem)
      Next
      If tLister Is Nothing Then
        SetStatus("Idle")
      Else
        SetStatus("Extracting Image from ISO...")
      End If
    Else
      SetStatus("Could not Extract Merge Image from ISO!")
    End If
    SpeedStats.WIM_ParseImage(imgSize) = TickCount() - parseStart
    SetProgress(0, 10)
    SetTotal(0, 3)
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
#End Region
#Region "Update Check"
  Private Sub tmrUpdateCheck_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdateCheck.Tick
    If mySettings Is Nothing Then Return
    tmrUpdateCheck.Stop()
    If mySettings.LastUpdate.Year = 1970 Then mySettings.LastUpdate = Today
    If DateDiff(DateInterval.Day, mySettings.LastUpdate, Today) > 13 Then
      mySettings.LastUpdate = Today
      cUpdate = New clsUpdate
      Dim updateCaller As New MethodInvoker(AddressOf cUpdate.CheckVersion)
      updateCaller.BeginInvoke(Nothing, Nothing)
    End If
  End Sub
  Private Sub cUpdate_CheckingVersion(sender As Object, e As System.EventArgs) Handles cUpdate.CheckingVersion
    SetStatus("Checking for new Version...")
  End Sub
  Private Sub cUpdate_CheckProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdate.CheckProgressChanged
    SetStatus("Checking for new Version... (" & e.ProgressPercentage & "%)")
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
        If IO.File.Exists(WorkDir & "Setup.exe") Then
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
