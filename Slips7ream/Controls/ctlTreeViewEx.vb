Imports System.ComponentModel
Public Class TreeViewEx
  Inherits TreeView
  Private c_ReadOnly As Boolean
  Private c_TooltipTitles As Boolean
  Private WithEvents ttTV As New Slips7ream.ToolTip
  Public Property [ReadOnly] As Boolean
    Get
      Return c_ReadOnly
    End Get
    Set(value As Boolean)
      c_ReadOnly = value
      MyBase.HideSelection = value
      If value Then
        MyBase.SelectedNode = Nothing
        MyBase.BackColor = SystemColors.ButtonFace
      Else
        MyBase.BackColor = SystemColors.Window
      End If
      For Each lvItem As TreeNode In Me.Nodes
        lvItem.BackColor = MyBase.BackColor
      Next
    End Set
  End Property
  <DefaultValue(False), Browsable(True), EditorBrowsable(True), Description("Turn the first line of the Tooltip Text for each TreeNode into a bold title."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Property TooltipTitles As Boolean
    Get
      Return c_TooltipTitles
    End Get
    Set(value As Boolean)
      c_TooltipTitles = value
    End Set
  End Property
  Protected Overrides Sub OnBeforeCheck(e As System.Windows.Forms.TreeViewCancelEventArgs)
    If c_ReadOnly Then
      e.Cancel = True
    End If
    MyBase.OnBeforeCheck(e)
  End Sub
  Protected Overrides Sub OnAfterCheck(e As System.Windows.Forms.TreeViewEventArgs)
    MyBase.OnAfterCheck(e)
  End Sub
  Protected Overrides Sub OnBeforeSelect(e As System.Windows.Forms.TreeViewCancelEventArgs)
    If c_ReadOnly Then
      e.Cancel = True
    End If
    MyBase.OnBeforeSelect(e)
  End Sub
  Protected Overrides Sub OnAfterSelect(e As System.Windows.Forms.TreeViewEventArgs)
    If c_ReadOnly Then
      Return
    End If
    MyBase.OnAfterSelect(e)
  End Sub
  Protected Overrides Sub OnDragEnter(drgevent As System.Windows.Forms.DragEventArgs)
    If Not c_ReadOnly Then MyBase.OnDragEnter(drgevent)
  End Sub
  Protected Overrides Sub OnDragDrop(drgevent As System.Windows.Forms.DragEventArgs)
    If Not c_ReadOnly Then MyBase.OnDragDrop(drgevent)
  End Sub
  Protected Overrides Sub OnDragOver(drgevent As System.Windows.Forms.DragEventArgs)
    If Not c_ReadOnly Then MyBase.OnDragOver(drgevent)
  End Sub
  Protected Overrides Sub OnKeyDown(e As System.Windows.Forms.KeyEventArgs)
    If Not c_ReadOnly Then MyBase.OnKeyDown(e)
  End Sub
  Protected Overrides Sub OnKeyPress(e As System.Windows.Forms.KeyPressEventArgs)
    If Not c_ReadOnly Then MyBase.OnKeyPress(e)
  End Sub
  Protected Overrides Sub OnKeyUp(e As System.Windows.Forms.KeyEventArgs)
    If Not c_ReadOnly Then MyBase.OnKeyUp(e)
  End Sub
  Protected Overrides Sub OnDoubleClick(e As System.EventArgs)
    If Not c_ReadOnly Then MyBase.OnDoubleClick(e)
  End Sub
  Protected Overrides Sub OnMouseDoubleClick(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ReadOnly Then MyBase.OnMouseDoubleClick(e)
  End Sub
  Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ReadOnly Then MyBase.OnMouseDown(e)
  End Sub
  Protected Overrides Sub OnMouseClick(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ReadOnly Then MyBase.OnMouseClick(e)
  End Sub
  Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
    MyBase.OnMouseMove(e)
    If c_TooltipTitles Then
      If Me.Nodes.Count = 0 Then Return
      Dim lvHTInfo As TreeViewHitTestInfo = Me.HitTest(e.X, e.Y)
      If lvHTInfo.Node Is Nothing Then
        ttTV.ToolTipTitle = Nothing
        ttTV.RemoveAll()
        ttTV.SetToolTip(Me, Nothing)
        Return
      End If
      Dim sToolTip As String = lvHTInfo.Node.ToolTipText
      Dim sTTTitle As String = Nothing
      Dim sTTText As String = sToolTip
      If Not String.IsNullOrEmpty(sToolTip) Then
        If sToolTip.Contains(vbNewLine) Then
          Dim sTTSplit() As String = Split(sToolTip, vbNewLine, 2)
          sTTTitle = sTTSplit(0)
          sTTText = sTTSplit(1)
        End If
      End If
      If ttTV.GetToolTip(Me) = sTTText Then Return
      ttTV.RemoveAll()
      ttTV = New Slips7ream.ToolTip()
      ttTV.AutoPopDelay = 30000
      ttTV.InitialDelay = 500
      ttTV.ReshowDelay = 100
      ttTV.Persistant = False
      ttTV.ToolTipTitle = sTTTitle
      ttTV.SetToolTip(Me, Nothing)
      ttTV.Show(sTTText, Me, e.X + 16, e.Y + 24, Integer.MaxValue)
    End If
  End Sub
  Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
    MyBase.OnMouseLeave(e)
    If c_TooltipTitles Then
      ttTV.ToolTipTitle = Nothing
      ttTV.RemoveAll()
      ttTV.SetToolTip(Me, Nothing)
    End If
  End Sub
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ReadOnly Then MyBase.OnMouseUp(e)
  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    If m.Msg = &H203 And CheckBoxes Then
      Dim X As Integer = m.LParam.ToInt32 And &HFFFFI
      Dim Y As Integer = (m.LParam.ToInt32 >> 16) And &HFFFFI
      Dim htInfo As TreeViewHitTestInfo = HitTest(X, Y)
      If htInfo.Node IsNot Nothing AndAlso htInfo.Location = TreeViewHitTestLocations.StateImage Then
        OnBeforeCheck(New TreeViewCancelEventArgs(htInfo.Node, False, TreeViewAction.ByMouse))
        htInfo.Node.Checked = Not htInfo.Node.Checked
        OnAfterCheck(New TreeViewEventArgs(htInfo.Node, TreeViewAction.ByMouse))
        m.Result = IntPtr.Zero
        Return
      End If
    End If
    MyBase.WndProc(m)
  End Sub
End Class
