﻿Public Class frmMain
  Private StopRun As Boolean = False
  Private LangChange As Boolean = False
  Private RunComplete As Boolean = False
  Private tLister As Threading.Thread
  Private tLister2 As Threading.Thread
  Private Const HeightDifferentialA As Integer = 187
  Private Const HeightDifferentialB As Integer = 22
  Private WithEvents cUpdate As clsUpdate
  Private FrameNumber As UInteger
  Private FrameCount As UInteger
  Private mngDisp As MNG
  Private sTitleText As String
  Private Const FrameInterval As UInteger = 3
  Private mySettings As MySettings
  Private Enum MNGList
    Move
    Copy
    Delete
  End Enum
#Region "GUI"
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Me.Tag = "LOADING"
    mySettings = New MySettings
    Me.imlUpdates.Images.Add("MSU", My.Resources.wusa)
    Me.imlUpdates.Images.Add("DIR", My.Resources.folder)
    Me.imlUpdates.Images.Add("CAB", My.Resources.cab)
    Me.imlUpdates.Images.Add("MLC", My.Resources.mlc)
    ttInfo.SetTooltip(expOutput.pctExpander, "Show Output consoles.")
    If String.IsNullOrEmpty(MySettings.DefaultFS) Then
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
    cmbCompletion.Text = "Do Nothing"
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
    ToggleInputs(True)
    SetDisp(MNGList.Move)
    FreshDraw()
    Me.Tag = Nothing
  End Sub
  Private Sub frmMain_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
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
  End Sub
  Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    StopRun = True
    If My.Computer.FileSystem.DirectoryExists(WorkDir) Then
      SetTitle("Cleaning Up Files", "Cleaning up mounts, work, and temporary directories...")
      SetDisp(MNGList.Delete)
      ToggleInputs(False)
      cmdClose.Enabled = False
      StopRun = False
      CleanMounts()
      StopRun = True
      SetStatus("Clearing Temp Directory...")
      Try
        My.Computer.FileSystem.DeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Catch ex As Exception
      End Try
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
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng", My.Resources.move, False)
      Case MNGList.Copy
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng", My.Resources.copy, False)
      Case MNGList.Delete
        My.Computer.FileSystem.WriteAllBytes(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng", My.Resources.delete, False)
    End Select
    If My.Computer.FileSystem.FileExists(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng") Then
      mngDisp.Load(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng")
      FrameNumber = 0
      FrameCount = mngDisp.NumEmbeddedPNG - 1
      My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\slips7ream_anim.mng")
    End If
  End Sub
  Private Sub RedoColumns()
    Dim msuSize As Integer = lvMSU.ClientSize.Width - lvMSU.Columns(1).Width - 1
    If Not lvMSU.Columns(0).Width = msuSize Then lvMSU.Columns(0).Width = msuSize
    Dim imagesSize As Integer = lvImages.ClientSize.Width - (lvImages.Columns(0).Width + lvImages.Columns(2).Width) - 1
    If Not lvImages.Columns(1).Width = imagesSize Then lvImages.Columns(1).Width = imagesSize
    If cmdAddMSU.Enabled Then
      cmdRemMSU.Enabled = lvMSU.SelectedItems.Count > 0
      cmdClearMSU.Enabled = lvMSU.Items.Count > 0
    End If
  End Sub
  Private Sub ToggleInputs(Enabled As Boolean)
    If Enabled Then
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
      cmdBegin.Text = "Begin"
      cmdOpenFolder.Visible = False
    End If
    cmdBegin.Visible = Enabled
    chkISO.Enabled = Enabled
    txtISO.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
    cmdISO.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
    chkUnlock.Enabled = IIf(Enabled, chkISO.Checked, Enabled)
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
      cmdClose.Text = "Close"
      lblActivity.Text = "Idle"
    Else
      cmdClose.Text = "Cancel"
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
    pnlSlips7ream.ResumeLayout(True)
    If Not pbTotal.Visible Then pbTotal.Value = 0
    If Not pbIndividual.Visible Then pbIndividual.Value = 0
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
#Region "WIM"
  Private Sub txtWIM_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtWIM.TextChanged
    RunComplete = False
    cmdBegin.Text = "Begin"
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
    txtSP.Enabled = chkSP.Checked
    cmdSP.Enabled = chkSP.Checked
    If chkSP.Checked Then
      Dim has86 As Boolean = False
      Dim has64 As Boolean = False
      Dim ParseWork As String = Work & "PARSE\"
      Dim ParseWorkWIM As String = ParseWork & "WIM\"
      For Each lvItem As ListViewItem In lvImages.Items
        Dim propData As PackageInfoEx = lvItem.Tag(1)
        If propData.Architecture.Contains("64") Then has64 = True
        If propData.Architecture.Contains("86") Then has86 = True
        If has86 And has64 Then Exit For
      Next
      If has86 And has64 Then
        chkSP.Text = "x86 Service Pack:"
        lblSP64.Visible = True
        pnlSP64.Visible = True
        lblSP64.Enabled = True
        txtSP64.Enabled = True
        cmdSP64.Enabled = True
      Else
        chkSP.Text = "Service Pack:"
        lblSP64.Visible = False
        pnlSP64.Visible = False
        lblSP64.Enabled = False
        txtSP64.Enabled = False
        cmdSP64.Enabled = False
      End If
    Else
      chkSP.Text = "Service Pack:"
      lblSP64.Visible = False
      pnlSP64.Visible = False
      lblSP64.Enabled = False
      txtSP64.Enabled = False
      cmdSP64.Enabled = False
    End If
    RedoColumns()
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
        If Not String.IsNullOrEmpty(msuData.DisplayName) Then
          Dim props As New frmUpdateProps
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
              props.lblKBLink.Text = "Article " & msuData.KBArticle
              props.lblKBLink.Visible = True
            End If
          Else
            props.lblKBLink.Link = True
            props.lblKBLink.Tag = msuData.SupportLink
            If String.IsNullOrEmpty(msuData.KBArticle) Then
              props.lblKBLink.Text = "Details"
              props.lblKBLink.Visible = True
            Else
              props.lblKBLink.Text = "Article " & msuData.KBArticle
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
  Public Sub DragDropEvent(sender As Object, e As DragEventArgs)
    ReplaceAllOldUpdates = TriState.UseDefault
    ReplaceAllNewUpdates = TriState.UseDefault
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      Dim FileCount As Integer = Data.Length
      If FileCount > 9 Then
        SetDisp(MNGList.Delete)
        SetTitle("Parsing Update Information", "Reading data from update files...")
        ToggleInputs(False)
        pbTotal.Value = 0
        pbTotal.Maximum = FileCount
        pbIndividual.Style = ProgressBarStyle.Marquee
        SetStatus("Reading Update Information...")
      End If
      Dim FailCollection As New Collections.Generic.List(Of String)
      For Each Item In Data
        If FileCount > 9 Then pbTotal.Value += 1
        Application.DoEvents()
        Dim addRet = AddToUpdates(Item)
        If Not addRet.Success Then FailCollection.Add(IO.Path.GetFileNameWithoutExtension(Item) & ": " & addRet.FailReason)
      Next
      If FileCount > 9 Then
        pbIndividual.Style = ProgressBarStyle.Continuous
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
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Public Sub DragEnterEvent(sender As Object, e As DragEventArgs)
    e.Effect = DragDropEffects.All
  End Sub
  Public Sub DragOverEvent(sender As Object, e As DragEventArgs)
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      Dim hasUpdate As Boolean = False
      For Each file In Data
        Select Case IO.Path.GetExtension(file).ToLower
          Case ".msu", ".cab", ".mlc", ".exe"
            hasUpdate = True
            Exit For
        End Select
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

  Private LastClick As Integer = 0
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
    If LastClick > 0 And (Environment.TickCount - LastClick) < 250 Then
      lvMSU_DoubleClick(lvMSU, New EventArgs)
    End If
    sourceRows.Clear()
    LastClick = Environment.TickCount
  End Sub

  Private Sub lvMSU_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvMSU.SelectedIndexChanged
    cmdRemMSU.Enabled = lvMSU.SelectedItems.Count > 0
  End Sub
#End Region

  Private Sub lvMSU_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvMSU.KeyDown
    If e.KeyCode = Keys.Delete Then
      If lvMSU.SelectedItems.Count > 0 Then
        cmdRemMSU_Click(cmdRemMSU, New EventArgs)
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
        If FileCount > 9 Then
          SetDisp(MNGList.Delete)
          SetTitle("Parsing Update Information", "Reading data from update files...")
          ToggleInputs(False)
          pbTotal.Value = 0
          pbTotal.Maximum = FileCount
          pbIndividual.Style = ProgressBarStyle.Marquee
          SetStatus("Reading Update Information...")
        End If
        For Each sUpdate As String In cdlBrowse.FileNames
          If FileCount > 9 Then pbTotal.Value += 1
          Application.DoEvents()
          Dim addRet = AddToUpdates(sUpdate)
          If Not addRet.Success Then FailCollection.Add(IO.Path.GetFileNameWithoutExtension(sUpdate) & ": " & addRet.FailReason)
        Next
        If FileCount > 9 Then
          pbIndividual.Style = ProgressBarStyle.Continuous
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
    If My.Computer.FileSystem.FileExists(sUpdate) Then
      Dim msuData As New UpdateInfoEx(sUpdate)
      If String.IsNullOrEmpty(msuData.DisplayName) Then Return New AddResult(False, "Unable to parse information.")
      For Each item As ListViewItem In lvMSU.Items
        If item.SubItems(1).Text = msuData.Architecture & " " & IO.Path.GetExtension(sUpdate).Substring(1).ToUpper Then
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
          lvItem.ImageKey = "MLC"
          lvItem.SubItems.Add(msuData.Architecture & " MUI")
          lvMSU.Items.Add(lvItem)
          Return New AddResult(True)
      End Select
      Return New AddResult(False, "Unknown Update Type: """ & IO.Path.GetExtension(sUpdate).ToUpper & """.")
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
      RedoColumns()
    Else
      Beep()
    End If
  End Sub
  Private Sub cmdClearMSU_Click(sender As System.Object, e As System.EventArgs) Handles cmdClearMSU.Click
    If lvMSU.Items.Count > 0 Then
      If MsgDlg(Me, IIf(lvMSU.Items.Count > 2, "All " & lvMSU.Items.Count & " updates", "All updates") & " will be removed from the list.", "Do you want to clear the Update List?", "Remove All Updates", MessageBoxButtons.YesNo, TaskDialogIcon.Delete, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
        lvMSU.Items.Clear()
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
    cmdBegin.Text = "Begin"
    cmdOpenFolder.Visible = False
    txtISO.Enabled = chkISO.Checked
    cmdISO.Enabled = chkISO.Checked
    chkUEFI.Enabled = chkISO.Checked
    chkUnlock.Enabled = chkISO.Checked
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
  Private Sub txtISOLabel_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISOLabel.TextChanged
    If Me.Tag Is Nothing Then
      If Not String.IsNullOrEmpty(txtISOLabel.Text) Then
        mySettings.DefaultISOLabel = txtISOLabel.Text
      End If
    End If
  End Sub
  Private Sub txtISO_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtISO.TextChanged
    RunComplete = False
    cmdBegin.Text = "Begin"
    cmdOpenFolder.Visible = False
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
  Private Sub txtMerge_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMerge.TextChanged
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
      Dim ParseWork As String = Work & "PARSE\"
      Dim ParseWorkWIM As String = ParseWork & "WIM\"
      Dim ParseWorkMerge As String = ParseWork & "Merge\"
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
  Private Sub ParseImageList(ToRun As String)
    SetDisp(MNGList.Delete)
    SetTitle("Parsing WIM Packages", "Reading data from Windows Image package descriptor...")
    tmrAnimation.Start()
    If ToRun = "WIM" Then
      ParseMainWIM()
    ElseIf ToRun = "MERGE" Then
      ParseMergeWIM()
    Else
      ParseMainWIM()
      ParseMergeWIM()
    End If
    ClearLister(ToRun)
    tmrAnimation.Stop()
    FreshDraw()
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
            Exit Sub
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
          MsgDlg(Me, "Unable to open the folder for """ & sPath & """!", "Unable to open folder.", "Folder Not Found", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, , ex.Message)
        End Try
      Else
        MsgDlg(Me, "Unable to find the file """ & sPath & """!", "Unable to find completed Image.", "File Not Found", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
      End If
    End If
  End Sub
  Private Sub cmdConfig_Click(sender As System.Object, e As System.EventArgs) Handles cmdConfig.Click
    frmConfig.ShowDialog(Me)
    mySettings = New MySettings
  End Sub
  Private Sub cmdBegin_Click(sender As System.Object, e As System.EventArgs) Handles cmdBegin.Click
    RunComplete = False
    StopRun = False
    LangChange = False
    If txtISOLabel.Enabled And txtISOLabel.Text.Contains(" ") Then
      SetStatus("Spaces are not allowed in the ISO Label!")
      txtISOLabel.Text = Replace(txtISOLabel.Text, " ", "_")
      txtISOLabel.Focus()
      Exit Sub
    End If

    SetTitle("Preparing Images", "Cleaning up mounts and extracting WIM from ISO if necessary...")
    ToggleInputs(False)
    Dim WIMFile As String = Nothing
    If My.Computer.FileSystem.DirectoryExists(WorkDir) Then
      pbIndividual.Style = ProgressBarStyle.Marquee
      CleanMounts()
      SetStatus("Clearing Old Data...")
      Try
        My.Computer.FileSystem.DeleteDirectory(WorkDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Application.DoEvents()
      Catch ex As Exception
        Application.DoEvents()
      End Try
    End If
    pbTotal.Value = 0
    pbIndividual.Value = 0
    pbTotal.Maximum = 12
    pbIndividual.Maximum = 100
    pbTotal.Style = ProgressBarStyle.Continuous
    pbIndividual.Style = ProgressBarStyle.Continuous

    SetDisp(MNGList.Copy)
    SetStatus("Checking WIM...")
    If String.IsNullOrEmpty(txtWIM.Text) OrElse Not My.Computer.FileSystem.FileExists(txtWIM.Text) Then
      ToggleInputs(True)
      SetStatus("Missing WIM File!")
      txtWIM.Focus()
      Beep()
      Exit Sub
    Else
      If IO.Path.GetExtension(txtWIM.Text).ToLower = ".iso" Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        SetStatus("Extracting Image from ISO...")
        ExtractAFile(txtWIM.Text, Work, "INSTALL.WIM")
        pbIndividual.Style = ProgressBarStyle.Continuous
        pbTotal.Value += 1
        WIMFile = Work & "INSTALL.WIM"
      Else
        WIMFile = txtWIM.Text
      End If
    End If
    If StopRun Then
      ToggleInputs(True)
      Exit Sub
    End If
    SetDisp(MNGList.Delete)
    SetTitle("Merging Image Packages", "Merging all WIM packages into single WIM...")

    SetStatus("Checking Merge...")
    Dim MergeFile As String = Nothing
    If chkMerge.Checked Then
      If String.IsNullOrEmpty(txtMerge.Text) OrElse Not My.Computer.FileSystem.FileExists(txtMerge.Text) Then
        ToggleInputs(True)
        SetStatus("Missing Merge File!")
        txtSP.Focus()
        Beep()
        Exit Sub
      Else
        MergeFile = txtMerge.Text
      End If
    Else
      MergeFile = Nothing
    End If

    Dim MergeWIM As String = Nothing
    Dim MergeWork As String = Work & "Merge\"
    If Not String.IsNullOrEmpty(MergeFile) Then
      If My.Computer.FileSystem.DirectoryExists(MergeWork) Then My.Computer.FileSystem.DeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
      My.Computer.FileSystem.CreateDirectory(MergeWork)
      Dim MergeWorkExtract As String = MergeWork & "Extract\"
      If Not My.Computer.FileSystem.DirectoryExists(MergeWorkExtract) Then My.Computer.FileSystem.CreateDirectory(MergeWorkExtract)
      If IO.Path.GetExtension(MergeFile).ToLower = ".iso" Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        SetStatus("Extracting Merge Image from ISO...")
        ExtractAFile(MergeFile, MergeWorkExtract, "INSTALL.WIM")
        pbIndividual.Style = ProgressBarStyle.Continuous
        Application.DoEvents()
        MergeWIM = MergeWorkExtract & "INSTALL.WIM"
      Else
        MergeWIM = MergeFile
      End If
    End If
    If StopRun Then
      ToggleInputs(True)
      Exit Sub
    End If

    pbIndividual.Value = 0
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
        If ExportWIM(RowImage, RowIndex, NewWIM, RowName) Then
          Continue For
        Else
          ToggleInputs(True)
          SetStatus("Failed to Merge WIM """ & RowName & """")
          Exit Sub
        End If
      End If
      If StopRun Then
        ToggleInputs(True)
        Exit Sub
      End If
    Next
    If My.Computer.FileSystem.DirectoryExists(MergeWork) Then My.Computer.FileSystem.DeleteDirectory(MergeWork, FileIO.DeleteDirectoryOption.DeleteAllContents)

    pbIndividual.Value = 0
    pbIndividual.Style = ProgressBarStyle.Marquee
    pbTotal.Value += 1
    SetStatus("Making Backup of Old WIM...")
    My.Computer.FileSystem.MoveFile(WIMFile, WIMFile & ".del", True)
    Try
      SetStatus("Moving Generated WIM...")
      My.Computer.FileSystem.MoveFile(NewWIM, WIMFile, True)
    Catch ex As Exception
      SetStatus("Generated WIM Move Failed! Reverting to Old WIM...")
      My.Computer.FileSystem.MoveFile(WIMFile & ".del", WIMFile, True)
    End Try
    If My.Computer.FileSystem.FileExists(WIMFile & ".del") Then
      SetStatus("Cleaning Up Backup WIM...")
      My.Computer.FileSystem.DeleteFile(WIMFile & ".del")
    End If
    pbIndividual.Style = ProgressBarStyle.Continuous
    Application.DoEvents()
    If StopRun Then
      ToggleInputs(True)
      Exit Sub
    End If
    SetDisp(MNGList.Move)
    SetTitle("Preparing Files", "Checking updates and Service Packs to integrate...")

    SetStatus("Checking Service Pack...")
    Dim SPFile As String = Nothing
    If chkSP.Checked Then
      If String.IsNullOrEmpty(txtSP.Text) OrElse Not My.Computer.FileSystem.FileExists(txtSP.Text) Then
        ToggleInputs(True)
        SetStatus("Missing Service Pack File!")
        txtSP.Focus()
        Beep()
        Exit Sub
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
      ElseIf Not My.Computer.FileSystem.FileExists(txtSP64.Text) Then
        ToggleInputs(True)
        SetStatus("Missing x64 Service Pack File!")
        txtSP64.Focus()
        Beep()
        Exit Sub
      Else
        SP64File = txtSP64.Text
      End If
    Else
      SP64File = Nothing
    End If

    SetStatus("Checking ISO...")
    Dim ISOFile As String = Nothing
    If chkISO.Checked Then
      If String.IsNullOrEmpty(txtISO.Text) OrElse Not My.Computer.FileSystem.FileExists(txtISO.Text) Then
        ToggleInputs(True)
        SetStatus("Missing ISO File!")
        txtISO.Focus()
        Beep()
        Exit Sub
      Else
        ISOFile = txtISO.Text
      End If
    Else
      ISOFile = Nothing
    End If

    SetStatus("Collecting Update List...")
    Dim UpdateFiles As New Collections.Generic.List(Of UpdateInfo)
    If lvMSU.Items.Count > 0 Then
      pbIndividual.Value = 0
      pbIndividual.Maximum = lvMSU.Items.Count
      For Each lvItem As ListViewItem In lvMSU.Items
        pbIndividual.Value += 1
        Application.DoEvents()
        UpdateFiles.Add(New UpdateInfo(lvItem.Tag, True))
        If StopRun Then
          ToggleInputs(True)
          Exit Sub
        End If
      Next
      pbIndividual.Value = 0
      pbTotal.Value += 1
      Application.DoEvents()
    End If
    If StopRun Then
      ToggleInputs(True)
      Exit Sub
    End If
    SetDisp(MNGList.Copy)
    SetTitle("Adding Service Packs", "Integrating Service Pack data into WIM packages...")

    If Not String.IsNullOrEmpty(SPFile) Then
      If String.IsNullOrEmpty(SP64File) Then
        SetStatus("Integrating Service Pack...")
        If IntegrateSP(WIMFile, SPFile) Then
          pbTotal.Value += 1
          SetStatus("Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Exit Sub
        End If
        If StopRun Then
          ToggleInputs(True)
          Exit Sub
        End If
      Else
        SetStatus("Integrating x86 Service Pack...")
        If IntegrateSP(WIMFile, SPFile, "86") Then
          pbTotal.Value += 1
          SetStatus("x86 Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Exit Sub
        End If
        If StopRun Then
          ToggleInputs(True)
          Exit Sub
        End If
        SetStatus("Integrating x64 Service Pack...")
        If IntegrateSP(WIMFile, SP64File, "64") Then
          pbTotal.Value += 1
          SetStatus("x64 Service Pack Integrated!")
        Else
          Dim sMsg As String = lblActivity.Text
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Exit Sub
        End If
        If StopRun Then
          ToggleInputs(True)
          Exit Sub
        End If
      End If
    End If
    SetDisp(MNGList.Copy)
    SetTitle("Adding Windows Updates", "Integrating update data into WIM packages...")

    Dim NoMount As Boolean = False
    If UpdateFiles.Count > 0 Then
      SetStatus("Integrating Updates...")
      If IntegrateFiles(WIMFile, UpdateFiles.ToArray) Then
        pbTotal.Value += 1
        SetStatus("Updates Integrated!")
      Else
        Dim sMsg As String = lblActivity.Text
        ToggleInputs(True)
        lblActivity.Text = sMsg
        Exit Sub
      End If
      If StopRun Then
        DiscardDISM(Mount)
        ToggleInputs(True)
        Exit Sub
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
      Dim ISODir As String = Work & "ISO\"
      Dim ISODDir As String = Work & "ISOD%n\"

      If Not My.Computer.FileSystem.DirectoryExists(ISODir) Then My.Computer.FileSystem.CreateDirectory(ISODir)
      pbIndividual.Style = ProgressBarStyle.Marquee
      SetStatus("Extracting ISO contents...")
      ExtractFiles(ISOFile, ISODir, "install.wim")
      pbTotal.Value += 1

      If chkUnlock.Checked Then
        SetStatus("Unlocking All Editions...")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\ei.cfg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\ei.cfg")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\install_Windows 7 STARTER.clg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\install_Windows 7 STARTER.clg")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\install_Windows 7 HOMEBASIC.clg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\install_Windows 7 HOMEBASIC.clg")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\install_Windows 7 HOMEPREMIUM.clg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\install_Windows 7 HOMEPREMIUM.clg")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\install_Windows 7 PROFESSIONAL.clg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\install_Windows 7 PROFESSIONAL.clg")
        If My.Computer.FileSystem.FileExists(ISODir & "sources\install_Windows 7 ULTIMATE.clg") Then My.Computer.FileSystem.DeleteFile(ISODir & "sources\install_Windows 7 ULTIMATE.clg")
      End If
      If My.Computer.FileSystem.DirectoryExists(ISODir & "[BOOT]") Then
        Try
          My.Computer.FileSystem.DeleteDirectory(ISODir & "[BOOT]", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception
        End Try
      End If
      If LangChange Then
        pbIndividual.Style = ProgressBarStyle.Marquee
        SetStatus("Updating Language INIs...")
        Dim PackageCount As Integer = GetDISMPackages(ISODir & "sources\install.wim")
        pbIndividual.Style = ProgressBarStyle.Continuous
        If Not UpdateLang(ISODir, PackageCount) Then
          Dim sMsg As String = lblActivity.Text
          If Not NoMount Then DiscardDISM(Mount)
          ToggleInputs(True)
          lblActivity.Text = sMsg
          Exit Sub
        End If
        SetStatus("Updated Language INIs!")
        If StopRun Then
          If Not NoMount Then DiscardDISM(Mount)
          ToggleInputs(True)
          Exit Sub
        End If
      End If
      If chkUEFI.Checked Then
        SetStatus("Enabling UEFI Boot...")
        If My.Computer.FileSystem.DirectoryExists(ISODir & "\efi\microsoft\boot\") Then
          If Not My.Computer.FileSystem.DirectoryExists(ISODir & "\efi\boot\") Then
            My.Computer.FileSystem.CopyDirectory(ISODir & "\efi\microsoft\boot\", ISODir & "\efi\boot\", True)
            If My.Computer.FileSystem.FileExists(Mount & "\Windows\Boot\EFI\bootmgfw.efi") Then
              My.Computer.FileSystem.CopyFile(Mount & "\Windows\Boot\EFI\bootmgfw.efi", ISODir & "\efi\boot\bootx64.efi", True)
              SetStatus("UEFI Boot Enabled!")
            Else
              My.Computer.FileSystem.DeleteDirectory(ISODir & "\efi\boot\", FileIO.DeleteDirectoryOption.DeleteAllContents)
              chkUEFI.Checked = False
              MsgDlg(Me, "Unable to find the file ""\Windows\Boot\EFI\bootmgfw.efi"" in Image!", "Unable to enable UEFI Boot.", "File Not Found", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
            End If
          Else
            chkUEFI.Checked = False
            SetStatus("UEFI Boot already enabled!")
          End If
        Else
          chkUEFI.Checked = False
          MsgDlg(Me, "Unable to find the folder ""\efi\microsoft\boot\"" in ISO!", "Unable to enable UEFI Boot.", "Folder Not Found", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder)
        End If
      End If
      If Not NoMount Then
        SetStatus("Saving Final Image Package...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Save Final Image Package!")
          Exit Sub
        End If
      End If
      SetStatus("Integrating INSTALL.WIM...")
      My.Computer.FileSystem.CopyFile(WIMFile, ISODir & "sources\install.wim", True)
      pbIndividual.Style = ProgressBarStyle.Continuous
      pbTotal.Value += 1

      If cmbLimitType.SelectedIndex > 0 Then
        Dim splUEFI As Boolean = chkUEFI.Checked
        Dim WIMToSplit As String = ISODir & "sources\install.wim"
        Dim limVal As String = cmbLimit.Text
        If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
        Dim splFileSize As Long = NumericVal(limVal)
        Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(WIMToSplit).Length
        Dim splISOSize As Long = splWIMSize + 351
        If cmbLimitType.SelectedIndex = 2 Then
          If splISOSize > splFileSize Then
            Dim ISOSplit As Long = splFileSize
            Dim WIMSplit As Long = ISOSplit - 351
            If splUEFI And (WIMSplit > 4095) Then WIMSplit = 4095
            If Math.Floor(ISOSplit / WIMSplit) > 1 Then
              'Split WIM to WIMSplit size, put 1 WIM on first, and Math.Floor(ISOSplit / WIMSplit) per ISO afterward
              pbIndividual.Style = ProgressBarStyle.Marquee
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(WIMToSplit, IO.Path.ChangeExtension(WIMToSplit, ".swm"), WIMSplit) Then
                If My.Computer.FileSystem.FileExists(WIMToSplit) Then My.Computer.FileSystem.DeleteFile(WIMToSplit)
                pbIndividual.Style = ProgressBarStyle.Continuous
                SetStatus("Generating Data ISOs...")
                pbIndividual.Style = ProgressBarStyle.Continuous
                Dim DiscNumber As Integer = 1
                Dim WIMNumber As Integer = 0
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources\", FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                For I As Integer = 0 To FilesInOrder.Count - 1
                  Dim File As String = FilesInOrder(I)
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Continue For
                  ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                    WIMNumber += 1
                    Dim sIDir As String = ISODDir.Replace("%n", DiscNumber)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir) Then My.Computer.FileSystem.CreateDirectory(sIDir)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir & "sources\") Then My.Computer.FileSystem.CreateDirectory(sIDir & "sources\")
                    My.Computer.FileSystem.MoveFile(File, sIDir & "\sources\" & IO.Path.GetFileName(File))
                    If WIMNumber = Math.Floor(ISOSplit / WIMSplit) Or I = FilesInOrder.Count - 1 Then
                      ExtractFiles(Application.StartupPath & "\AR.zip", sIDir)
                      If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                        SetStatus("Looking for oversized files...")
                        Dim BigList = FindBigFiles(sIDir)
                        If BigList.Count > 0 Then
                          Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
                            Select Case fsAlert.ShowDialog(Me)
                              Case Windows.Forms.DialogResult.Yes
                                'UDF
                                cmbISOFormat.SelectedIndex = 3
                              Case Windows.Forms.DialogResult.OK
                                'UDF102
                                cmbISOFormat.SelectedIndex = 5
                              Case Windows.Forms.DialogResult.No
                                'Default
                              Case Windows.Forms.DialogResult.Cancel
                                'STOP
                                ToggleInputs(True)
                                SetStatus("ISO creation cancelled by user!")
                                Exit Sub
                            End Select
                          End Using
                        End If
                      End If
                      Dim DiscIDNo As String = DiscNumber
                      Dim DiscFiles() As String = My.Computer.FileSystem.GetFiles(sIDir & "\sources\", FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                      SortFiles(DiscFiles)
                      DiscIDNo = NumericVal(IO.Path.GetFileNameWithoutExtension(DiscFiles(0)))
                      Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & "\" & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & DiscIDNo & IO.Path.GetExtension(ISOFile)
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
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Exit Sub
              End If
            ElseIf Math.Floor(ISOSplit / WIMSplit) = 1 Then
              'Split WIM to WIMSplit size, put 1 WIM on each ISO including first
              pbIndividual.Style = ProgressBarStyle.Marquee
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(WIMToSplit, IO.Path.ChangeExtension(WIMToSplit, ".swm"), WIMSplit) Then
                If My.Computer.FileSystem.FileExists(WIMToSplit) Then My.Computer.FileSystem.DeleteFile(WIMToSplit)
                SetStatus("Generating Data ISOs...")
                pbIndividual.Style = ProgressBarStyle.Continuous
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources\", FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
                For Each File As String In FilesInOrder
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Continue For
                  ElseIf IO.Path.GetFileNameWithoutExtension(File).ToLower.Contains("install") Then
                    Dim discNo As String = IO.Path.GetFileNameWithoutExtension(File).Substring(7)
                    Dim sIDir As String = ISODDir.Replace("%n", discNo)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir) Then My.Computer.FileSystem.CreateDirectory(sIDir)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir & "sources\") Then My.Computer.FileSystem.CreateDirectory(sIDir & "sources\")
                    My.Computer.FileSystem.MoveFile(File, sIDir & "\sources\" & IO.Path.GetFileName(File))
                    ExtractFiles(Application.StartupPath & "\AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
                          Select Case fsAlert.ShowDialog(Me)
                            Case Windows.Forms.DialogResult.Yes
                              'UDF
                              cmbISOFormat.SelectedIndex = 3
                            Case Windows.Forms.DialogResult.OK
                              'UDF102
                              cmbISOFormat.SelectedIndex = 5
                            Case Windows.Forms.DialogResult.No
                              'Default
                            Case Windows.Forms.DialogResult.Cancel
                              'STOP
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
                              Exit Sub
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & "\" & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & discNo & IO.Path.GetExtension(ISOFile)
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
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Exit Sub
              End If
            Else
              'Split WIM to WIMSplit size, put no WIMs on first ISO, one per ISO afterward
              pbIndividual.Style = ProgressBarStyle.Marquee
              SetStatus("Splitting WIM into " & ByteSize(WIMSplit * 1024 * 1024) & " Chunks...")
              If SplitWIM(WIMToSplit, IO.Path.ChangeExtension(WIMToSplit, ".swm"), WIMSplit) Then
                If My.Computer.FileSystem.FileExists(WIMToSplit) Then My.Computer.FileSystem.DeleteFile(WIMToSplit)
                SetStatus("Generating Data ISOs...")
                pbIndividual.Style = ProgressBarStyle.Continuous
                Dim FilesInOrder() As String = My.Computer.FileSystem.GetFiles(ISODir & "sources\", FileIO.SearchOption.SearchTopLevelOnly, "*.swm").ToArray
                SortFiles(FilesInOrder)
                FilesInOrder.OrderBy(Function(path) Int32.Parse(IO.Path.GetFileNameWithoutExtension(path)))
                For Each File As String In FilesInOrder
                  If IO.Path.GetFileNameWithoutExtension(File).ToLower = "install" Then
                    Dim sIDir As String = ISODDir.Replace("%n", "1")
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir) Then My.Computer.FileSystem.CreateDirectory(sIDir)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir & "sources\") Then My.Computer.FileSystem.CreateDirectory(sIDir & "sources\")
                    My.Computer.FileSystem.MoveFile(File, sIDir & "\sources\install.swm")
                    ExtractFiles(Application.StartupPath & "\AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
                          Select Case fsAlert.ShowDialog(Me)
                            Case Windows.Forms.DialogResult.Yes
                              'UDF
                              cmbISOFormat.SelectedIndex = 3
                            Case Windows.Forms.DialogResult.OK
                              'UDF102
                              cmbISOFormat.SelectedIndex = 5
                            Case Windows.Forms.DialogResult.No
                              'Default
                            Case Windows.Forms.DialogResult.Cancel
                              'STOP
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
                              Exit Sub
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & "\" & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D1" & IO.Path.GetExtension(ISOFile)
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
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir) Then My.Computer.FileSystem.CreateDirectory(sIDir)
                    If Not My.Computer.FileSystem.DirectoryExists(sIDir & "sources\") Then My.Computer.FileSystem.CreateDirectory(sIDir & "sources\")
                    My.Computer.FileSystem.MoveFile(File, sIDir & "\sources\" & IO.Path.GetFileName(File))
                    ExtractFiles(Application.StartupPath & "\AR.zip", sIDir)
                    If (cmbISOFormat.SelectedIndex = 0) Or (cmbISOFormat.SelectedIndex = 1) Or (cmbISOFormat.SelectedIndex = 2) Or (cmbISOFormat.SelectedIndex = 4) Or (cmbISOFormat.SelectedIndex = 6) Then
                      SetStatus("Looking for oversized files...")
                      Dim BigList = FindBigFiles(sIDir)
                      If BigList.Count > 0 Then
                        Using fsAlert As New frmFS(BigList, sIDir, cmbISOFormat.SelectedIndex)
                          Select Case fsAlert.ShowDialog(Me)
                            Case Windows.Forms.DialogResult.Yes
                              'UDF
                              cmbISOFormat.SelectedIndex = 3
                            Case Windows.Forms.DialogResult.OK
                              'UDF102
                              cmbISOFormat.SelectedIndex = 5
                            Case Windows.Forms.DialogResult.No
                              'Default
                            Case Windows.Forms.DialogResult.Cancel
                              'STOP
                              ToggleInputs(True)
                              SetStatus("ISO creation cancelled by user!")
                              Exit Sub
                          End Select
                        End Using
                      End If
                    End If
                    Dim ISODFile As String = IO.Path.GetDirectoryName(ISOFile) & "\" & IO.Path.GetFileNameWithoutExtension(ISOFile) & "_D" & discNo & IO.Path.GetExtension(ISOFile)
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
                ToggleInputs(True)
                SetStatus("Failed to Split WIM!")
                Exit Sub
              End If
            End If
          ElseIf splUEFI And splWIMSize > 4095 Then
            'Split WIM to 4095 MB size
            pbIndividual.Style = ProgressBarStyle.Marquee
            SetStatus("Splitting WIM into 4095 MB Chunks...")
            If SplitWIM(WIMToSplit, IO.Path.ChangeExtension(WIMToSplit, ".swm"), 4095) Then
              If My.Computer.FileSystem.FileExists(WIMToSplit) Then My.Computer.FileSystem.DeleteFile(WIMToSplit)
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
              Exit Sub
            End If
          End If
        Else
          If splWIMSize > splFileSize Then
            'Split WIM to FileSize size
            pbIndividual.Style = ProgressBarStyle.Marquee
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            If SplitWIM(WIMToSplit, IO.Path.ChangeExtension(WIMToSplit, ".swm"), splFileSize) Then
              If My.Computer.FileSystem.FileExists(WIMToSplit) Then My.Computer.FileSystem.DeleteFile(WIMToSplit)
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
              Exit Sub
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
                  'UDF
                  cmbISOFormat.SelectedIndex = 3
                Case Windows.Forms.DialogResult.OK
                  'UDF102
                  cmbISOFormat.SelectedIndex = 5
                Case Windows.Forms.DialogResult.No
                  'Default
                Case Windows.Forms.DialogResult.Cancel
                  'STOP
                  ToggleInputs(True)
                  SetStatus("ISO creation cancelled by user!")
                  Exit Sub
              End Select
            End Using
          End If
        End If
      End If

      Dim BootOrder As String = Work & "bootorder.txt"
      My.Computer.FileSystem.WriteAllText(BootOrder, "boot\bcd" & vbNewLine &
                                          "boot\boot.sdi" & vbNewLine &
                                          "boot\bootfix.bin" & vbNewLine &
                                          "boot\bootsect.exe" & vbNewLine &
                                          "boot\etfsboot.com" & vbNewLine &
                                          "boot\memtest.efi" & vbNewLine &
                                          "boot\memtest.exe" & vbNewLine &
                                          "boot\en-us\bootsect.exe.mui" & vbNewLine &
                                          "boot\fonts\chs_boot.ttf" & vbNewLine &
                                          "boot\fonts\cht_boot.ttf" & vbNewLine &
                                          "boot\fonts\jpn_boot.ttf" & vbNewLine &
                                          "boot\fonts\kor_boot.ttf" & vbNewLine &
                                          "boot\fonts\wgl4_boot.ttf" & vbNewLine &
                                          "sources\boot.wim" & vbNewLine, False, System.Text.Encoding.GetEncoding(1252))
      pbIndividual.Style = ProgressBarStyle.Marquee
      SetStatus("Making Backup of Old ISO...")
      My.Computer.FileSystem.MoveFile(ISOFile, ISOFile & ".del", True)
      pbTotal.Value += 1
      pbIndividual.Style = ProgressBarStyle.Continuous
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
        Saved = MakeISO(ISODir, isoLabel, BootOrder, isoFormat, ISOFile)
      Catch ex As Exception
        Saved = False
      End Try
      pbTotal.Value += 1
      pbIndividual.Style = ProgressBarStyle.Continuous
      If Saved Then
        If My.Computer.FileSystem.FileExists(ISOFile & ".del") Then My.Computer.FileSystem.DeleteFile(ISOFile & ".del")
      Else
        SetStatus("Build Failed! Restoring Old ISO...")
        My.Computer.FileSystem.MoveFile(ISOFile & ".del", ISOFile, True)
      End If
    Else
      SetTitle("Generating WIM", "Preparing WIMs and file structure...")

      If Not NoMount Then
        SetStatus("Saving Final Image Package...")
        If Not SaveDISM(Mount) Then
          DiscardDISM(Mount)
          ToggleInputs(True)
          SetStatus("Failed to Save Final Image Package!")
          Exit Sub
        End If
      End If

      If cmbLimitType.SelectedIndex > 0 Then
        If My.Computer.FileSystem.FileExists(WIMFile) Then
          Dim limVal As String = cmbLimit.Text
          If limVal.Contains("(") Then limVal = limVal.Substring(0, limVal.IndexOf("("))
          Dim splFileSize As Long = NumericVal(limVal)
          Dim splWIMSize As Long = My.Computer.FileSystem.GetFileInfo(WIMFile).Length
          If splWIMSize > splFileSize Then
            'Split WIMs to FileSize size
            SetStatus("Splitting WIM into " & ByteSize(splFileSize * 1024 * 1024) & " Chunks...")
            If SplitWIM(WIMFile, IO.Path.ChangeExtension(WIMFile, ".swm"), splFileSize) Then
              My.Computer.FileSystem.DeleteFile(WIMFile)
            Else
              ToggleInputs(True)
              SetStatus("Failed to Split WIM!")
              Exit Sub
            End If
          End If
        End If
      End If
      If My.Computer.FileSystem.FileExists(ISOFile & ".del") Then My.Computer.FileSystem.DeleteFile(ISOFile & ".del")
    End If

    pbTotal.Value = 0
    pbIndividual.Value = 0
    pbTotal.Maximum = 100
    pbIndividual.Maximum = 100
    ToggleInputs(True)
    SetStatus("Complete!")
    Select Case cmbCompletion.SelectedIndex
      Case 0
        'done
        RunComplete = True
        ToggleInputs(True)
        'cmdOpenFolder.Visible = True
      Case 1
        'close
        Me.Close()
      Case 2
        'shut down
        Process.Start("shutdown", "/s /t 0 /d p:0:0 /f")
      Case 3
        'restart
        Process.Start("shutdown", "/r /t 0 /d p:0:0 /f")
      Case 4
        'sleep
        Application.SetSuspendState(PowerState.Suspend, False, False)
    End Select
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    If cmdClose.Text = "Close" Then
      Me.Close()
    Else
      StopRun = True
      cmdClose.Enabled = False
      Application.DoEvents()
    End If
  End Sub
#End Region
#Region "Status"
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
  Private Sub SetProgress(Value As Integer, Maximum As Integer)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New SetProgressInvoker(AddressOf SetProgress), Value, Maximum)
    Else
      pbIndividual.Maximum = Maximum
      pbIndividual.Value = Value
    End If
  End Sub
  Private Sub expOutput_Closed(sender As Object, e As System.EventArgs) Handles expOutput.Closed
    pnlSlips7ream.SuspendLayout()
    ttInfo.SetTooltip(expOutput, "Show Output consoles.")
    ttInfo.SetTooltip(expOutput.pctExpander, "Show Output consoles.")
    txtOutput.Visible = False
    txtOutputError.Visible = False
    Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height - HeightDifferentialA)
    Me.Height -= HeightDifferentialA
    pnlSlips7ream.ResumeLayout(True)
  End Sub
  Private Sub expOutput_Opened(sender As System.Object, e As System.EventArgs) Handles expOutput.Opened
    pnlSlips7ream.SuspendLayout()
    ttInfo.SetTooltip(expOutput, "Hide Output consoles.")
    ttInfo.SetTooltip(expOutput.pctExpander, "Hide Output consoles.")
    Me.Height += HeightDifferentialA
    Me.MinimumSize = New Size(Me.MinimumSize.Width, Me.MinimumSize.Height + HeightDifferentialA)
    txtOutput.Visible = True
    txtOutputError.Visible = True
    pnlSlips7ream.ResumeLayout(True)
  End Sub
  Private Delegate Sub WriteToOutputCallBack(Message As String)
  Private Sub WriteToOutput(Message As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New WriteToOutputCallBack(AddressOf WriteToOutput), Message)
    Else
      txtOutput.Text &= Message & vbNewLine
      If txtOutput.Text.Length > 0 Then
        txtOutput.SelectionStart = txtOutput.Text.Length - 1
        txtOutput.SelectionLength = 0
        txtOutput.ScrollToCaret()
      End If
    End If
  End Sub
  Private Sub WriteToError(Message As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New WriteToOutputCallBack(AddressOf WriteToError), Message)
    Else
      txtOutputError.Text &= Message & vbNewLine
      If txtOutputError.Text.Length > 0 Then
        txtOutputError.SelectionStart = txtOutputError.Text.Length - 1
        txtOutputError.SelectionLength = 0
        txtOutputError.ScrollToCaret()
      End If
    End If
  End Sub
#End Region
#End Region
#Region "Command Calls"
  Private Sub CleanMounts()
    Dim DISMInfo As String = RunWithReturn(AIKDir & "dism", "/Get-MountedWimInfo /English", True)
    Dim mFindA As String = WorkDir.Substring(0, WorkDir.Length - 1).ToLower
    Dim mFindB As String = ShortenPath(mFindA).ToLower
    If Not DISMInfo.Contains("No mounted images found.") Then
      SetStatus("Cleaning Old DISM Mounts...")
      Dim sLines() As String = Split(DISMInfo, vbNewLine)
      For Each line In sLines
        If line.Contains("Mount Dir : ") Then
          Dim tmpPath As String = line.Substring(line.IndexOf(":") + 2)
          If line.ToLower.Contains(mFindA) Or line.ToLower.Contains(mFindB) Then RunHidden(AIKDir & "dism", "/Unmount-Wim /MountDir:" & ShortenPath(tmpPath) & " /discard /English")
        End If
      Next
      RunHidden(AIKDir & "dism", "/cleanup-wim")
    End If
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
      RunHidden(AIKDir & "imagex", "/cleanup")
    End If
  End Sub
  Private Function MakeDataImages(sIDir As String, sDiscName As String, ISODest As String) As Boolean
    ExtractFiles(Application.StartupPath & "\AR.zip", sIDir)
    Try
      My.Computer.FileSystem.WriteAllBytes(ISODest, {0, 0}, False)
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
      Return MakeISO(sIDir, sDiscName, Nothing, isoFormat, ISODest)
    Catch ex As Exception
      Return False
    End Try
  End Function
#Region "DISM"
  Private Function InitDISM(WIMFile As String, WIMIndex As Integer, MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "dism", "/Mount-Wim /WimFile:" & ShortenPath(WIMFile) & " /index:" & WIMIndex.ToString.Trim & " /MountDir:" & ShortenPath(MountPath) & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function GetDISMPackages(WIMFile As String) As Integer
    Dim PackageList As String = RunWithReturn(AIKDir & "dism", "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile) & " /English")
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
    Dim PackageList As String = RunWithReturn(AIKDir & "dism", "/Get-WimInfo /WimFile:" & ShortenPath(WIMFile) & " /index:" & Index & " /English")
    Dim Info As New PackageInfoEx(PackageList)
    Return Info
  End Function
  Private Function AddToDism(MountPath As String, AddPath As String) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "dism", "/Image:" & ShortenPath(MountPath) & " /Add-Package /PackagePath:" & ShortenPath(AddPath) & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function SaveDISM(MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "dism", "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /commit" & " /English")
    Return sRet.Contains("The operation completed successfully.")
  End Function
  Private Function DiscardDISM(MountPath As String) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "dism", "/Unmount-Wim /MountDir:" & ShortenPath(MountPath) & " /discard" & " /English")
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
  Private Function UpdateLang(SourcePath As String, SourceIndex As Integer) As Boolean
    Dim sRet As String = RunWithReturn(AIKDir & "intlcfg", "-genlangini -dist:" & ShortenPath(SourcePath) & " -image:" & ShortenPath(Mount) & " -f") '-all:la-ng
    If Not sRet.Contains("A new Lang.ini file has been generated") Then
      Return False
    End If
    Dim MountPath As String = WorkDir & "BOOT\"
    If Not My.Computer.FileSystem.DirectoryExists(MountPath) Then My.Computer.FileSystem.CreateDirectory(MountPath)
    ReturnProgress = True
    sRet = RunWithReturn(AIKDir & "imagex", "/mountrw " & ShortenPath(SourcePath & "sources\boot.wim") & " 2 " & ShortenPath(MountPath))
    ReturnProgress = False
    If Not sRet.Contains("Successfully mounted image.") Then
      SetStatus("Failed to mount boot.wim!")
      Return False
    End If
    If My.Computer.FileSystem.FileExists(SourcePath & "sources\lang.ini") Then My.Computer.FileSystem.CopyFile(SourcePath & "sources\lang.ini", MountPath & "sources\lang.ini", True)
    ReturnProgress = True
    sRet = RunWithReturn(AIKDir & "imagex", "/unmount /commit " & ShortenPath(MountPath))
    ReturnProgress = False
    If Not sRet.Contains("Successfully unmounted image.") Then
      SetStatus("Failed to unmount boot.wim!")
      Return False
    End If
    My.Computer.FileSystem.DeleteDirectory(MountPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
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
      sRet = RunWithReturn(AIKDir & "oscdimg", "-m " & FileSystem & " -yo" & ShortenPath(BootOrderFile) & " -b" & ShortenPath(FromDir & "boot\etfsboot.com") & " " & ShortenPath(FromDir.Substring(0, FromDir.Length - 1)) & " " & ShortenPath(DestISO) & " -l" & Label)
    End If
    ReturnProgress = False
    Return sRet.Contains("Done.")
  End Function
#End Region
#Region "7-Zip"
  Private Sub ExtractFiles(Source As String, Destination As String)
    Dim SevenZ As String = AIKDir & "7z.exe"
    RunHidden(SevenZ, "x -y -o""" & Destination & """ """ & Source & """")
  End Sub
  Private Sub ExtractFiles(Source As String, Destination As String, Except As String)
    Dim SevenZ As String = AIKDir & "7z.exe"
    RunHidden(SevenZ, "x -y -o""" & Destination & """ """ & Source & """ -xr!" & Except)
  End Sub
  Private Sub ExtractAFile(Source As String, Destination As String, File As String)
    Dim SevenZ As String = AIKDir & "7z.exe"
    RunHidden(SevenZ, "e -y -o""" & Destination & """ """ & Source & """ *\" & File)
  End Sub
#End Region
#Region "EXE2CAB"
  Private Function EXE2CAB(Source As String, Destination As String) As Boolean
    Dim ExeToCab As String = AIKDir & "exe2cab.exe"
    RunHidden(ExeToCab, """" & Source & """ -q """ & Destination & """")
    Return My.Computer.FileSystem.FileExists(Destination)
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
          Else
            Debug.Print(Output)
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
        Else
          Debug.Print(Output)
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
        Exit Sub
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
        'ok
      ElseIf item.GetType = GetType(Boolean) AndAlso item = False Then
        'ok
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
  Private Function IntegrateFiles(WIMPath As String, MSUPaths() As UpdateInfo) As Boolean
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    If PackageCount > 0 Then
      If MSUPaths.Length > 0 Then
        Dim DISM_32 As New Collections.Generic.List(Of PackageInfoEx)
        Dim DISM_64 As New Collections.Generic.List(Of PackageInfoEx)
        Dim MSU_32 As New Collections.Generic.List(Of UpdateInfoEx)
        Dim MSU_64 As New Collections.Generic.List(Of UpdateInfoEx)
        pbIndividual.Value = 0
        pbIndividual.Maximum = PackageCount
        For I As Integer = 1 To PackageCount
          pbIndividual.Value += 1
          SetStatus("Loading Image Package #" & I.ToString.Trim & " Data...")
          Dim tmpDISM As PackageInfoEx = GetDISMPackageData(WIMPath, I)
          If tmpDISM.Architecture = "x86" Then
            DISM_32.Add(tmpDISM)
          ElseIf tmpDISM.Architecture = "x64" Then
            DISM_64.Add(tmpDISM)
          End If
        Next
        pbIndividual.Value = 0
        pbIndividual.Maximum = MSUPaths.Length
        For I As Integer = 0 To MSUPaths.Length - 1
          If MSUPaths(I).IsFile Then
            pbIndividual.Value += 1
            SetStatus("Loading Update """ & IO.Path.GetFileNameWithoutExtension(MSUPaths(I).Path) & """ Data...")
            Dim tmpMSU As New UpdateInfoEx(MSUPaths(I).Path)
            If String.IsNullOrEmpty(tmpMSU.Architecture) Then
              Continue For
            Else
              If tmpMSU.Architecture = "x86" Then
                MSU_32.Add(tmpMSU)
              End If
              If tmpMSU.Architecture = "amd64" Then
                MSU_64.Add(tmpMSU)
              Else
                If CheckWhitelist(tmpMSU.DisplayName) Then MSU_64.Add(tmpMSU)
              End If
            End If
          End If
        Next
        pbIndividual.Value = 0
        pbIndividual.Maximum = 1
        For Each tmpDISM In DISM_32
          pbIndividual.Maximum += MSU_32.Count + 1
        Next
        For Each tmpDISM In DISM_64
          pbIndividual.Maximum += MSU_64.Count + 1
        Next
        If MSU_32.Count > 0 Then
          For D As Integer = 0 To DISM_32.Count - 1
            Dim tmpDISM As PackageInfoEx = DISM_32(D)
            pbIndividual.Value += 1
            SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
            If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              SetStatus("Failed to Load Image Package """ & tmpDISM.Name & """!")
              Return False
            End If
            If StopRun Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              Return False
            End If
            For I As Integer = 0 To MSU_32.Count - 1
              Dim tmpMSU = MSU_32(I)
              pbIndividual.Value += 1
              Dim DisplayName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
              If Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
                DisplayName = "KB" & tmpMSU.KBArticle
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DisplayName) Then
                DisplayName = tmpMSU.DisplayName
              End If
              SetStatus((I + 1).ToString.Trim & "/" & MSU_32.Count.ToString & " - Integrating """ & DisplayName & """ into """ & tmpDISM.Name & """...")
              Select Case GetUpdateType(tmpMSU.Path)
                Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
                  If Not AddToDism(Mount, tmpMSU.Path) Then
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If GetUpdateType(tmpMSU.Path) = UpdateType.LP Then LangChange = True
                Case UpdateType.EXE
                  Dim tmpCAB As String = WorkDir & "lp.cab"
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to extract """ & DisplayName & """ from EXE to CAB!")
                    Return False
                  End If
                  If Not AddToDism(Mount, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  LangChange = True
                Case UpdateType.LIP
                  Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
                  If Not AddToDism(Mount, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  LangChange = True
              End Select
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
              If Not SaveDISM(Mount) Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                SetStatus("Failed to Save Image Package """ & tmpDISM.Name & """!")
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
            Dim tmpDISM As PackageInfoEx = DISM_64(D)
            pbIndividual.Value += 1
            SetStatus("Loading Image Package """ & tmpDISM.Name & """...")
            If Not InitDISM(WIMPath, tmpDISM.Index, Mount) Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              SetStatus("Failed to Load Image Package """ & tmpDISM.Name & """!")
              Return False
            End If
            If StopRun Then
              DiscardDISM(Mount)
              ToggleInputs(True)
              Return False
            End If
            For I As Integer = 0 To MSU_64.Count - 1
              Dim tmpMSU = MSU_64(I)
              pbIndividual.Value += 1
              Dim DisplayName As String = IO.Path.GetFileNameWithoutExtension(tmpMSU.Path)
              If Not String.IsNullOrEmpty(tmpMSU.KBArticle) Then
                DisplayName = "KB" & tmpMSU.KBArticle
              ElseIf Not String.IsNullOrEmpty(tmpMSU.DisplayName) Then
                DisplayName = tmpMSU.DisplayName
              End If
              SetStatus((I + 1).ToString.Trim & "/" & MSU_32.Count.ToString & " - Integrating """ & DisplayName & """ into """ & tmpDISM.Name & """...")
              Select Case GetUpdateType(tmpMSU.Path)
                Case UpdateType.MSU, UpdateType.CAB, UpdateType.LP
                  If Not AddToDism(Mount, tmpMSU.Path) Then
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If GetUpdateType(tmpMSU.Path) = UpdateType.LP Then LangChange = True
                Case UpdateType.EXE
                  Dim tmpCAB As String = WorkDir & "lp.cab"
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  If Not EXE2CAB(tmpMSU.Path, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to extract """ & DisplayName & """ from EXE to CAB!")
                    Return False
                  End If
                  If Not AddToDism(Mount, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  LangChange = True
                Case UpdateType.LIP
                  Dim tmpCAB As String = WorkDir & "tmp" & IO.Path.GetFileNameWithoutExtension(tmpMSU.Path) & ".cab"
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  My.Computer.FileSystem.CopyFile(tmpMSU.Path, tmpCAB)
                  If Not AddToDism(Mount, tmpCAB) Then
                    If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                    DiscardDISM(Mount)
                    ToggleInputs(True)
                    SetStatus("Failed to integrate """ & DisplayName & """ into """ & tmpDISM.Name & """!")
                    Return False
                  End If
                  If My.Computer.FileSystem.FileExists(tmpCAB) Then My.Computer.FileSystem.DeleteFile(tmpCAB)
                  LangChange = True
              End Select
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
              If Not SaveDISM(Mount) Then
                DiscardDISM(Mount)
                ToggleInputs(True)
                SetStatus("Failed to Save Image Package """ & tmpDISM.Name & """!")
                Return False
              End If
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
    pbIndividual.Value = 0
    Dim PackageCount As Integer = GetDISMPackages(WIMPath)
    pbIndividual.Maximum = PackageCount + 13
    SetStatus("Extracting Service Pack...")
    RunHidden(SPPath, "/x:""" & Work & "SP1""")
    pbIndividual.Value += 1
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    Dim Extract86 As String = Work & "SP1\windows6.1-KB976932-X86.cab"
    Dim Extract64 As String = Work & "SP1\windows6.1-KB976932-X64.cab"
    If My.Computer.FileSystem.FileExists(Extract86) Then
      SetStatus("Extracting KB976932.cab...")
      ExtractFiles(Extract86, Work & "SP1")
      My.Computer.FileSystem.DeleteFile(Extract86)
    ElseIf My.Computer.FileSystem.FileExists(Extract64) Then
      SetStatus("Extracting KB976932.cab...")
      ExtractFiles(Extract64, Work & "SP1")
      My.Computer.FileSystem.DeleteFile(Extract64)
    Else
      ToggleInputs(True)
      SetStatus("No KB976932.cab to extract!")
      Return False
    End If
    pbIndividual.Value += 1
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If

    Dim Extract As String = Work & "SP1\NestedMPPcontent.cab"
    If My.Computer.FileSystem.FileExists(Extract) Then
      SetStatus("Extracting NestedMPPcontent.cab...")
      ExtractFiles(Extract, Work & "SP1")
      My.Computer.FileSystem.DeleteFile(Extract)
    Else
      ToggleInputs(True)
      SetStatus("No NestedMPPcontent.cab to extract!")
      Return False
    End If
    If StopRun Then
      ToggleInputs(True)
      Return False
    End If
    pbIndividual.Value += 1
    Dim Update As String = Work & "SP1\update.ses"
    If My.Computer.FileSystem.FileExists(Update) Then
      SetStatus("Modifying update.ses...")
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
    pbIndividual.Value += 1
    Update = Work & "SP1\update.mum"
    If My.Computer.FileSystem.FileExists(Update) Then
      SetStatus("Modifying update.mum...")
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
    pbIndividual.Value += 1
    Dim Update86 As String = Work & "SP1\Windows7SP1-KB976933~31bf3856ad364e35~x86~~6.1.1.17514.mum"
    Dim Update64 As String = Work & "SP1\Windows7SP1-KB976933~31bf3856ad364e35~amd64~~6.1.1.17514.mum"
    If My.Computer.FileSystem.FileExists(Update86) Then
      SetStatus("Modifying KB976933.mum...")
      UpdateMUM(Update86)
    ElseIf My.Computer.FileSystem.FileExists(Update64) Then
      SetStatus("Modifying KB976933.mum...")
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
    pbIndividual.Value += 1
    Dim CABList As String = Work & "SP1\cabinet.cablist.ini"
    If My.Computer.FileSystem.FileExists(CABList) Then My.Computer.FileSystem.DeleteFile(CABList)
    CABList = Work & "SP1\old_cabinet.cablist.ini"
    If My.Computer.FileSystem.FileExists(CABList) Then My.Computer.FileSystem.DeleteFile(CABList)

    For I As Integer = 0 To 6
      Extract = Work & "SP1\KB976933-LangsCab" & I.ToString.Trim & ".cab"
      If My.Computer.FileSystem.FileExists(Extract) Then
        SetStatus("Extracting Language CAB " & (I + 1).ToString.Trim & " of 7...")
        ExtractFiles(Extract, Work & "SP1")
        My.Computer.FileSystem.DeleteFile(Extract)
      Else
        ToggleInputs(True)
        SetStatus("No KB976933-LangsCab" & I.ToString.Trim & ".cab to extract!")
        Return False
      End If
      pbIndividual.Value += 1
      If StopRun Then
        ToggleInputs(True)
        Return False
      End If
    Next
    If PackageCount > 0 Then
      For I As Integer = 1 To PackageCount
        Dim dismData As PackageInfoEx = GetDISMPackageData(WIMPath, I)
        Try
          If Not String.IsNullOrEmpty(Architecture) AndAlso Not GetDISMPackageData(WIMPath, I).Architecture.Contains(Architecture) Then
            Continue For
          End If
        Catch ex As Exception

        End Try
        SetStatus("Loading Image Package """ & dismData.Name & """...")
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
        pbIndividual.Value += 1
        SetStatus("Merging Service Pack into Image Package """ & dismData.Name & """...")
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
        SetStatus("Saving Image Package """ & dismData.Name & """...")
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
      Next
    Else
      ToggleInputs(True)
      SetStatus("No packages in WIM!")
      Return False
    End If
    pbIndividual.Value = 0
    pbIndividual.Style = ProgressBarStyle.Marquee
    SetStatus("Clearing Temp Files...")
    My.Computer.FileSystem.DeleteDirectory(Work & "SP1", FileIO.DeleteDirectoryOption.DeleteAllContents)
    pbIndividual.Style = ProgressBarStyle.Continuous
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
    If String.IsNullOrEmpty(sWIM) Then Exit Sub
    Dim ParseWork As String = Work & "PARSE\"
    Dim ParseWorkWIM As String = ParseWork & "WIM\"
    If My.Computer.FileSystem.FileExists(ParseWork) Then My.Computer.FileSystem.DeleteDirectory(ParseWork, FileIO.DeleteDirectoryOption.DeleteAllContents)
    My.Computer.FileSystem.CreateDirectory(ParseWork)
    My.Computer.FileSystem.CreateDirectory(ParseWorkWIM)
    Dim WIMFile As String = String.Empty
    If IO.Path.GetExtension(sWIM).ToLower = ".iso" Then
      SetStatus("Extracting Image from ISO...")
      ExtractAFile(sWIM, ParseWorkWIM, "INSTALL.WIM")
      WIMFile = ParseWorkWIM & "INSTALL.WIM"
    Else
      WIMFile = sWIM
    End If
    If My.Computer.FileSystem.FileExists(WIMFile) Then
      SetStatus("Reading Image Packages...")
      ClearImageList("WIM")
      Dim PackageCount As Integer = GetDISMPackages(WIMFile)
      For I As Integer = 1 To PackageCount
        Dim Package As PackageInfoEx = GetDISMPackageData(WIMFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
        If Package.Equals(New PackageInfoEx) Then Exit Sub
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
  End Sub
  Private Sub ParseMergeWIM()
    Dim sMerge As String = txtMerge.Text
    Dim ParseWork As String = Work & "PARSE\"
    Dim ParseWorkMerge As String = ParseWork & "Merge\"
    My.Computer.FileSystem.CreateDirectory(ParseWorkMerge)
    If String.IsNullOrEmpty(sMerge) Then Exit Sub
    Dim MergeFile As String = String.Empty
    If IO.Path.GetExtension(sMerge).ToLower = ".iso" Then
      SetStatus("Extracting Merge Image from ISO...")
      ExtractAFile(sMerge, ParseWorkMerge, "INSTALL.WIM")
      MergeFile = ParseWorkMerge & "INSTALL.WIM"
    Else
      MergeFile = sMerge
    End If
    If My.Computer.FileSystem.FileExists(MergeFile) Then
      SetStatus("Reading Merge Image Packages...")
      ClearImageList("MERGE")
      Dim PackageCount As Integer = GetDISMPackages(MergeFile)
      For I As Integer = 1 To PackageCount
        Dim Package As PackageInfoEx = GetDISMPackageData(MergeFile, I)
        Dim lvItem As New ListViewItem(Package.Index)
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
      If String.IsNullOrEmpty(tempDir) Then tempDir = My.Computer.FileSystem.SpecialDirectories.Temp & "\Slips7ream\"
      If Not My.Computer.FileSystem.DirectoryExists(tempDir) Then My.Computer.FileSystem.CreateDirectory(tempDir)
      Return tempDir
    End Get
  End Property
  Private ReadOnly Property Work As String
    Get
      Dim sDir As String = WorkDir & "WORK\"
      If Not My.Computer.FileSystem.DirectoryExists(sDir) Then My.Computer.FileSystem.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
  Private ReadOnly Property Mount As String
    Get
      Dim sDir As String = WorkDir & "MOUNT\"
      If Not My.Computer.FileSystem.DirectoryExists(sDir) Then My.Computer.FileSystem.CreateDirectory(sDir)
      Return sDir
    End Get
  End Property
#End Region
#Region "Update Check"
  Private Sub tmrUpdateCheck_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdateCheck.Tick
    tmrUpdateCheck.Stop()
    If mySettings.LastUpdate.Year = 1970 Then mySettings.LastUpdate = Today
    If DateDiff(DateInterval.Day, mySettings.LastUpdate, Today) > 13 Then
      mySettings.LastUpdate = Today
      cUpdate = New clsUpdate
      cUpdate.CheckVersion()
    End If
  End Sub
  Private Sub cUpdate_CheckingVersion(sender As Object, e As System.EventArgs) Handles cUpdate.CheckingVersion
    SetStatus("Checking for new Version...")
  End Sub
  Private Sub cUpdate_CheckProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdate.CheckProgressChanged
    SetStatus("Checking for new Version... (" & e.ProgressPercentage & "%)")
  End Sub
  Private Sub cUpdate_CheckResult(sender As Object, e As clsUpdate.CheckEventArgs) Handles cUpdate.CheckResult
    If e.Cancelled Then
      SetStatus("Update Check Cancelled!")
    ElseIf e.Error IsNot Nothing Then
      SetStatus("Update Check Failed: " & e.Error.Message & "!")
    Else

      If e.Result = clsUpdate.CheckEventArgs.ResultType.NewUpdate Then
        SetStatus("New Version Available!")
        If MsgDlg(Me, "Would you like to update now?", "SLIPS7REAM v" & e.Version & " is available!", "SLIPS7REAM Update", MessageBoxButtons.YesNo, TaskDialogIcon.InternetRJ45) = Windows.Forms.DialogResult.Yes Then
          cUpdate.DownloadUpdate(WorkDir & "Setup.exe")
        End If
      Else
        SetStatus("Idle")
      End If
    End If
  End Sub
  Private Sub cUpdate_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles cUpdate.DownloadingUpdate
    SetStatus("Downloading New Version - Waiting for Response...")
  End Sub
  Private Sub cUpdate_DownloadResult(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles cUpdate.DownloadResult
    If e.Cancelled Then
      SetStatus("Update Download Cancelled!")
    ElseIf e.Error IsNot Nothing Then
      SetStatus("Update Download Failed: " & e.Error.Message & "!")
    Else
      cUpdate.Dispose()
      SetStatus("Download Complete!")
      Application.DoEvents()
      If My.Computer.FileSystem.FileExists(WorkDir & "Setup.exe") Then
        Try
          Shell(WorkDir & "Setup.exe /silent", AppWinStyle.NormalFocus, False)
          Application.Exit()
        Catch ex As Exception
          MsgDlg(Me, "If you have User Account Control enabled, please allow the SLIPS7REAM Installer to run.", "There was an error starting the update.", "SLIPS7REAM Update Failure", MessageBoxButtons.OK, TaskDialogIcon.ShieldUAC, , ex.Message)
        End Try
      Else
        SetStatus("Update Failure!")
      End If
    End If
  End Sub
  Private Sub cUpdate_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles cUpdate.UpdateProgressChanged
    SetStatus("Downloading New Version - " & ByteSize(e.BytesReceived) & " of " & ByteSize(e.TotalBytesToReceive) & "... (" & e.ProgressPercentage & "%)")
  End Sub
#End Region
End Class