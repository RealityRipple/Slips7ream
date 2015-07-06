Imports System.Runtime.InteropServices
Public Class frmOutput
  <StructLayout(LayoutKind.Sequential)>
  Public Structure WINDOWPOS
    Public hwnd As IntPtr
    Public hwndInsertAfter As IntPtr
    Public x As Integer
    Public y As Integer
    Public cx As Integer
    Public cy As Integer
    Public flags As SWP
  End Structure
  <Flags()>
  Public Enum SWP As UInteger
    Normal = 0
    NoSize = &H1
    NoMove = &H2
    NoZOrder = &H4
    NoRedraw = &H8
    NoActivate = &H10
    FrameChanged = &H20
    ShowWindow = &H40
    HideWindow = &H80
    NoCopyBits = &H100
    NoOwnerZOrder = &H200
    NoSendChanging = &H400
    NoClientSize = &H800
    NoClientMove = &H1000
    DrawFrame = FrameChanged
    NoReposition = NoOwnerZOrder
    DeferErase = &H2000
    AsyncWindowPos = &H4000
    StateChanged = &H8000
  End Enum
  Private Const DockPad As Integer = 16
  Private bDock As DockStyle = DockStyle.None
  Private defSize As Size
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    Select Case m.Msg
      Case &H46
        Dim NewPosition As WINDOWPOS = CType(Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS)), WINDOWPOS)
        If NewPosition.y = 0 And NewPosition.cy = 0 And NewPosition.x = 0 And NewPosition.cx = 0 Then
          MyBase.WndProc(m)
          Return
        End If
        Dim mainBounds = frmMain.Bounds
        If Not ((NewPosition.flags And SWP.NoSize) = SWP.NoSize And (NewPosition.flags And SWP.NoMove) = SWP.NoMove) Then
          bDock = DockStyle.None
        End If
        'output window smaller than main window
        If NewPosition.x >= mainBounds.Left - DockPad And NewPosition.x + NewPosition.cx <= mainBounds.Right + DockPad Then
          'top dock
          If NewPosition.y + NewPosition.cy >= mainBounds.Top - DockPad And NewPosition.y + NewPosition.cy <= mainBounds.Top + DockPad Then
            NewPosition.y = mainBounds.Top - NewPosition.cy
            If Math.Abs(NewPosition.x - mainBounds.Left) < DockPad * 4 Then NewPosition.x = mainBounds.Left
            If Math.Abs(NewPosition.cx - mainBounds.Width) < DockPad * 4 Then
              NewPosition.cx = mainBounds.Width
              NewPosition.x = mainBounds.Right - NewPosition.cx
            ElseIf Math.Abs((NewPosition.x + NewPosition.cx) - mainBounds.Right) < DockPad * 4 Then
              NewPosition.x = mainBounds.Right - NewPosition.cx
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Top
            If defSize.IsEmpty Then defSize = Me.Size
          End If
          'bottom dock
          If NewPosition.y >= mainBounds.Bottom - DockPad And NewPosition.y <= mainBounds.Bottom + DockPad Then
            NewPosition.x = mainBounds.Left
            NewPosition.y = mainBounds.Bottom
            NewPosition.cx = mainBounds.Width
            NewPosition.cy = Me.MinimumSize.Height
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Bottom
            If defSize.IsEmpty Then defSize = Me.Size
          End If
        End If
        'main window smaller than output window
        If mainBounds.Left >= NewPosition.x - DockPad And mainBounds.Right <= NewPosition.x + NewPosition.cx + DockPad Then
          'top dock
          If mainBounds.Top >= NewPosition.y + NewPosition.cy - DockPad And mainBounds.Top <= NewPosition.y + NewPosition.cy + DockPad Then
            NewPosition.y = mainBounds.Top - NewPosition.cy
            If Math.Abs(mainBounds.Left - NewPosition.x) < DockPad * 4 Then NewPosition.x = mainBounds.Left
            If Math.Abs(mainBounds.Width - NewPosition.cx) < DockPad * 4 Then
              NewPosition.cx = mainBounds.Width
              NewPosition.x = mainBounds.Right - NewPosition.cx
            ElseIf Math.Abs(mainBounds.Right - (NewPosition.x + NewPosition.cx)) < DockPad * 4 Then
              NewPosition.x = mainBounds.Right - NewPosition.cx
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Top
            If defSize.IsEmpty Then defSize = Me.Size
          End If
          'bottom dock
          If mainBounds.Bottom >= NewPosition.y - DockPad And mainBounds.Bottom <= NewPosition.y + DockPad Then
            NewPosition.x = mainBounds.Left
            NewPosition.y = mainBounds.Bottom
            NewPosition.cx = mainBounds.Width
            NewPosition.cy = Me.MinimumSize.Height
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Bottom
            If defSize.IsEmpty Then defSize = Me.Size
          End If
        End If
        'output window smaller than main window
        If NewPosition.y >= mainBounds.Top - DockPad And NewPosition.y + NewPosition.cy <= mainBounds.Bottom + DockPad Then
          'Left Dock
          If NewPosition.x + NewPosition.cx >= mainBounds.Left - DockPad And NewPosition.x + NewPosition.cx <= mainBounds.Left + DockPad Then
            NewPosition.x = mainBounds.Left - NewPosition.cx
            If Math.Abs(NewPosition.y - mainBounds.Top) < DockPad * 4 Then NewPosition.y = mainBounds.Top
            If Math.Abs(NewPosition.cy - mainBounds.Height) < DockPad * 4 Then
              NewPosition.cy = mainBounds.Height
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            ElseIf Math.Abs((NewPosition.y + NewPosition.cy) - mainBounds.Bottom) < DockPad * 4 Then
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Left
            If defSize.IsEmpty Then defSize = Me.Size
          End If
          'Right Dock
          If NewPosition.x >= mainBounds.Right - DockPad And NewPosition.x <= mainBounds.Right + DockPad Then
            NewPosition.x = mainBounds.Right
            If Math.Abs(NewPosition.y - mainBounds.Top) < DockPad * 4 Then NewPosition.y = mainBounds.Top
            If Math.Abs(NewPosition.cy - mainBounds.Height) < DockPad * 4 Then
              NewPosition.cy = mainBounds.Height
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            ElseIf Math.Abs((NewPosition.y + NewPosition.cy) - mainBounds.Bottom) < DockPad * 4 Then
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Right
            If defSize.IsEmpty Then defSize = Me.Size
          End If
        End If
        'main window smaller than output window
        If mainBounds.Top >= NewPosition.y - DockPad And mainBounds.Bottom <= NewPosition.y + NewPosition.cy + DockPad Then
          'Left Dock
          If mainBounds.Left >= NewPosition.x + NewPosition.cx - DockPad And mainBounds.Left <= NewPosition.x + NewPosition.cx + DockPad Then
            NewPosition.x = mainBounds.Left - NewPosition.cx
            If Math.Abs(mainBounds.Top - NewPosition.y) < DockPad * 4 Then NewPosition.y = mainBounds.Top
            If Math.Abs(mainBounds.Height - NewPosition.cy) < DockPad * 4 Then
              NewPosition.cy = mainBounds.Height
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            ElseIf Math.Abs((NewPosition.y + NewPosition.cy) - mainBounds.Bottom) < DockPad * 4 Then
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Left
            If defSize.IsEmpty Then defSize = Me.Size
          End If
          'Right Dock
          If NewPosition.x >= mainBounds.Right - DockPad And NewPosition.x <= mainBounds.Right + DockPad Then
            NewPosition.x = mainBounds.Right
            If Math.Abs(NewPosition.y - mainBounds.Top) < DockPad * 4 Then NewPosition.y = mainBounds.Top
            If Math.Abs(NewPosition.cy - mainBounds.Height) < DockPad * 4 Then
              NewPosition.cy = mainBounds.Height
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            ElseIf Math.Abs(mainBounds.Bottom - (NewPosition.y + NewPosition.cy)) < DockPad * 4 Then
              NewPosition.y = mainBounds.Bottom - NewPosition.cy
            End If
            Runtime.InteropServices.Marshal.StructureToPtr(NewPosition, m.LParam, True)
            bDock = DockStyle.Right
            If defSize.IsEmpty Then defSize = Me.Size
          End If
        End If
    End Select
    MyBase.WndProc(m)
  End Sub
  Private Sub frmOutput_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.UserClosing Then
      e.Cancel = True
      frmMain.expOutput.PerformClick()
    End If
  End Sub
  Private Sub frmOutput_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    txtOutput.ContextMenu = mnuOutput
  End Sub
  Private Sub mnuOutput_Popup(sender As System.Object, e As System.EventArgs) Handles mnuOutput.Popup
    Dim mParent As ContextMenu = sender
    Dim txtSel As TextBox = mParent.SourceControl
    mnuSelectAll.Enabled = Not txtSel.TextLength = 0
    mnuClear.Enabled = Not txtSel.TextLength = 0
    mnuCopy.Enabled = Not txtSel.SelectedText.Length = 0
  End Sub
  Private Sub frmOutput_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
    Dim mainBounds = frmMain.Bounds
    If Me.Bounds.Left >= mainBounds.Left - DockPad And Me.Bounds.Right <= mainBounds.Right + DockPad Then
      If Me.Bounds.Top >= mainBounds.Bottom - DockPad And Me.Bounds.Top <= mainBounds.Bottom + DockPad Then
        frmMain.ReturnOutput()
      End If
    End If
  End Sub
  Private Sub tmrMove_Tick(sender As System.Object, e As System.EventArgs) Handles tmrMove.Tick
    RePosition()
  End Sub
  Public Sub DoResize()
    defSize = Me.Size
    frmOutput_ResizeEnd(Me, New EventArgs)
  End Sub
  Public Sub RePosition()
    Dim mainBounds = frmMain.Bounds
    Select Case bDock
      Case DockStyle.Top
        Me.Top = mainBounds.Top - Me.Height
        If Math.Abs(Me.Left - mainBounds.Left) < DockPad * 4 Then Me.Left = mainBounds.Left
        If Math.Abs(Me.Width - mainBounds.Width) < DockPad * 4 Then
          Me.Width = mainBounds.Width
          Me.Left = mainBounds.Right - Me.Width
        ElseIf Math.Abs(Me.Right - mainBounds.Right) < DockPad * 4 Then
          Me.Left = mainBounds.Right - Me.Width
        End If
      Case DockStyle.Bottom
        Me.Left = mainBounds.Left
        Me.Top = mainBounds.Bottom
        Me.Width = mainBounds.Width
        Me.Height = Me.MinimumSize.Height
      Case DockStyle.Left
        Me.Left = mainBounds.Left - Me.Width
        If Math.Abs(Me.Top - mainBounds.Top) < DockPad * 4 Then Me.Top = mainBounds.Top
        If Math.Abs(Me.Height - mainBounds.Height) < DockPad * 4 Then
          Me.Height = mainBounds.Height
          Me.Top = mainBounds.Bottom - Me.Height
        ElseIf Math.Abs(Me.Bottom - mainBounds.Bottom) < DockPad * 4 Then
          Me.Top = mainBounds.Bottom - Me.Height
        End If
      Case DockStyle.Right
        Me.Left = mainBounds.Right
        If Math.Abs(Me.Top - mainBounds.Top) < DockPad * 4 Then Me.Top = mainBounds.Top
        If Math.Abs(Me.Height - mainBounds.Height) < DockPad * 4 Then
          Me.Height = mainBounds.Height
          Me.Top = mainBounds.Bottom - Me.Height
        ElseIf Math.Abs(Me.Bottom - mainBounds.Bottom) < DockPad * 4 Then
          Me.Top = mainBounds.Bottom - Me.Height
        End If
      Case Else
        If Not defSize.IsEmpty Then
          Me.Size = defSize
          defSize = Size.Empty
        End If
    End Select
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
End Class
