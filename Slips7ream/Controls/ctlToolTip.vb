Imports System.ComponentModel
<ToolboxBitmap(GetType(System.Windows.Forms.ToolTip))>
Public Class ToolTip
  Inherits System.Windows.Forms.ToolTip
  Private b_Persistant As Boolean
  Private b_HideOnHover As Boolean
  Private ControlList As New List(Of Control)
  Public Sub New()
    MyBase.New()
    b_Persistant = False
    b_HideOnHover = True
  End Sub
  Public Sub New(cont As IContainer)
    MyBase.New(cont)
    b_Persistant = False
    b_HideOnHover = True
  End Sub
  <DefaultValue(False), Browsable(True), EditorBrowsable(True), Description("Toggles persistant tooltips which will show up on disabled items."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Property Persistant As Boolean
    Get
      Return b_Persistant
    End Get
    Set(value As Boolean)
      If Not b_Persistant = value Then
        b_Persistant = value
        For Each ctl In ControlList
          If Not b_Persistant Then
            AddHandler ctl.MouseLeave, New EventHandler(AddressOf ToolTip_MouseLeave)
          Else
            RemoveHandler ctl.MouseLeave, New EventHandler(AddressOf ToolTip_MouseLeave)
          End If
        Next
      End If
    End Set
  End Property
  <DefaultValue(True), Browsable(True), EditorBrowsable(True), Description("Hide the tooltip if the mouse happens to hover over it."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Property HideOnHover As Boolean
    Get
      Return b_HideOnHover
    End Get
    Set(value As Boolean)
      b_HideOnHover = value
    End Set
  End Property
  Public Shadows Sub SetToolTip(ctl As System.Windows.Forms.Control, caption As String)
    If ControlList.Contains(ctl) Then ControlList.Remove(ctl)
    RemoveHandler ctl.MouseEnter, New EventHandler(AddressOf ToolTip_MouseEnter)
    RemoveHandler ctl.MouseLeave, New EventHandler(AddressOf ToolTip_MouseLeave)
    MyBase.SetToolTip(ctl, caption)
    If Not String.IsNullOrEmpty(caption) Then
      If Not ControlList.Contains(ctl) Then ControlList.Add(ctl)
      AddHandler ctl.MouseEnter, New EventHandler(AddressOf ToolTip_MouseEnter)
      If Not b_Persistant Then AddHandler ctl.MouseLeave, New EventHandler(AddressOf ToolTip_MouseLeave)
    End If
  End Sub
  Private Sub ToolTip_MouseEnter(sender As Object, e As EventArgs)
    Me.Active = False
    Me.Active = True
  End Sub
  Private Sub ToolTip_MouseLeave(sender As Object, e As EventArgs)
    Me.Active = False
  End Sub
  Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
    Get
      Dim newCP As CreateParams = MyBase.CreateParams
      If Not b_HideOnHover Then newCP.ExStyle = newCP.ExStyle Or 32
      Return newCP
    End Get
  End Property
End Class
