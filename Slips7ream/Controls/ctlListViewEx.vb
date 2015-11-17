Imports System.ComponentModel
Imports System.Windows.Forms
<ToolboxBitmap(GetType(System.Windows.Forms.ListView))>
Public Class ListViewEx
  Inherits ListView
  Private c_ReadOnly As Boolean
  Public Property [ReadOnly] As Boolean
    Get
      Return c_ReadOnly
    End Get
    Set(value As Boolean)
      c_ReadOnly = value
      MyBase.HideSelection = value
      If value Then
        MyBase.BackColor = SystemColors.ButtonFace
      Else
        MyBase.BackColor = SystemColors.Window
      End If
      For Each lvItem As ListViewItem In Me.Items
        lvItem.BackColor = MyBase.BackColor
      Next
    End Set
  End Property
  Protected Overrides Sub OnItemCheck(e As System.Windows.Forms.ItemCheckEventArgs)
    If c_ReadOnly Then
      e.NewValue = e.CurrentValue
    End If
    MyBase.OnItemCheck(e)
  End Sub
  Protected Overrides Sub OnSelectedIndexChanged(e As System.EventArgs)
    If c_ReadOnly Then
      MyBase.SelectedItems.Clear()
    Else
      MyBase.OnSelectedIndexChanged(e)
    End If
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
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ReadOnly Then MyBase.OnMouseUp(e)
  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    If Not c_ReadOnly And m.Msg = &H203 And CheckBoxes Then
      Dim X As Integer = m.LParam.ToInt32 And &HFFFFI
      Dim Y As Integer = (m.LParam.ToInt32 >> 16) And &HFFFFI
      Dim htInfo As ListViewHitTestInfo = HitTest(X, Y)
      If htInfo.Item IsNot Nothing Then
        If htInfo.Location = ListViewHitTestLocations.Label Then
          OnMouseDoubleClick(New MouseEventArgs(Windows.Forms.MouseButtons.Left, 2, X, Y, 0))
          m.Result = IntPtr.Zero
          Return
        ElseIf htInfo.Location = ListViewHitTestLocations.StateImage Then
          OnItemCheck(New ItemCheckEventArgs(htInfo.Item.Index, Not htInfo.Item.Checked, htInfo.Item.Checked))
          htInfo.Item.Checked = Not htInfo.Item.Checked
          OnItemChecked(New ItemCheckedEventArgs(htInfo.Item))
          m.Result = IntPtr.Zero
          Return
        End If
      End If
    End If
    MyBase.WndProc(m)
  End Sub
End Class
