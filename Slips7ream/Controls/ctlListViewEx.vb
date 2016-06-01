Imports System.ComponentModel
Imports System.Windows.Forms
<ToolboxBitmap(GetType(System.Windows.Forms.ListView))>
Public Class ListViewEx
  Inherits ListView
  Private c_ReadOnly As Boolean
  Private c_FullRowTooltip As Boolean
  Private c_TooltipTitles As Boolean
  Private c_HeaderStyle As ColumnHeaderStyle = ColumnHeaderStyle.None
  Private components As System.ComponentModel.IContainer
  Private WithEvents ttLV As New Slips7ream.ToolTip
  Public Property [ReadOnly] As Boolean
    Get
      Return c_ReadOnly
    End Get
    Set(value As Boolean)
      If Not c_ReadOnly = value Then
        c_ReadOnly = value
        MyBase.HideSelection = value
        If value Then
          If Not MyBase.HeaderStyle = ColumnHeaderStyle.None And c_HeaderStyle = ColumnHeaderStyle.None Then c_HeaderStyle = MyBase.HeaderStyle
          MyBase.HeaderStyle = ColumnHeaderStyle.Nonclickable
          MyBase.SelectedItems.Clear()
          MyBase.BackColor = SystemColors.ButtonFace
        Else
          If Not MyBase.HeaderStyle = ColumnHeaderStyle.None And Not c_HeaderStyle = ColumnHeaderStyle.None Then
            MyBase.HeaderStyle = c_HeaderStyle
            c_HeaderStyle = ColumnHeaderStyle.None
          End If
          MyBase.BackColor = SystemColors.Window
          End If
          For Each lvItem As ListViewItem In Me.Items
            lvItem.BackColor = MyBase.BackColor
          Next
      End If
    End Set
  End Property
  <DefaultValue(False), Browsable(True), EditorBrowsable(True), Description("Show Tooltip Text for each ListViewItem across its entire row, not just in the first column."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Property FullRowTooltip As Boolean
    Get
      Return c_FullRowTooltip
    End Get
    Set(value As Boolean)
      c_FullRowTooltip = value
    End Set
  End Property
  <DefaultValue(False), Browsable(True), EditorBrowsable(True), Description("Turn the first line of the Tooltip Text for each ListViewItem into a bold title."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Property TooltipTitles As Boolean
    Get
      Return c_TooltipTitles
    End Get
    Set(value As Boolean)
      c_TooltipTitles = value
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
  Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
    MyBase.OnMouseMove(e)
    If c_FullRowTooltip Then
      If Me.Items.Count = 0 Then Return
      Dim lvHTInfo As ListViewHitTestInfo = Me.HitTest(e.X, e.Y)
      If lvHTInfo.Item Is Nothing Or lvHTInfo.SubItem Is Nothing Then
        ttLV.ToolTipTitle = Nothing
        ttLV.SetToolTip(Me, Nothing)
        Return
      End If
      Dim sToolTip As String = lvHTInfo.Item.ToolTipText
      Dim sTTTitle As String = Nothing
      Dim sTTText As String = sToolTip
      If Not String.IsNullOrEmpty(sToolTip) And c_TooltipTitles Then
        If sToolTip.Contains(vbNewLine) Then
          Dim sTTSplit() As String = Split(sToolTip, vbNewLine, 2)
          sTTTitle = sTTSplit(0)
          sTTText = sTTSplit(1)
        End If
      End If
      If ttLV.GetToolTip(Me) = sTTText Then Return
      ttLV = New Slips7ream.ToolTip()
      ttLV.AutoPopDelay = 30000
      ttLV.InitialDelay = 500
      ttLV.ReshowDelay = 100
      ttLV.Persistant = False
      ttLV.ToolTipTitle = sTTTitle
      ttLV.SetToolTip(Me, sTTText)
    ElseIf c_TooltipTitles Then
      If Me.Items.Count = 0 Then Return
      Dim lvHTInfo As ListViewHitTestInfo = Me.HitTest(e.X, e.Y)
      If lvHTInfo.Item Is Nothing OrElse lvHTInfo.Location = ListViewHitTestLocations.None OrElse lvHTInfo.SubItem.Bounds.Left > 0 Then
        ttLV.ToolTipTitle = Nothing
        ttLV.SetToolTip(Me, Nothing)
        Return
      End If
      Dim sToolTip As String = lvHTInfo.Item.ToolTipText
      Dim sTTTitle As String = Nothing
      Dim sTTText As String = sToolTip
      If Not String.IsNullOrEmpty(sToolTip) And c_TooltipTitles Then
        If sToolTip.Contains(vbNewLine) Then
          Dim sTTSplit() As String = Split(sToolTip, vbNewLine, 2)
          sTTTitle = sTTSplit(0)
          sTTText = sTTSplit(1)
        End If
      End If
      If ttLV.GetToolTip(Me) = sTTText Then Return
      ttLV = New Slips7ream.ToolTip()
      ttLV.AutoPopDelay = 30000
      ttLV.InitialDelay = 500
      ttLV.ReshowDelay = 100
      ttLV.Persistant = False
      ttLV.ToolTipTitle = sTTTitle
      ttLV.SetToolTip(Me, sTTText)
    End If
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
        ElseIf htInfo.Location = ListViewHitTestLocations.Image Then
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
