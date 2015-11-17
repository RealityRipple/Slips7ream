<ToolboxBitmap(GetType(System.Windows.Forms.ToolTip))>
Public Class ToolTip
  Inherits System.Windows.Forms.ToolTip
  Private b_Persistant As Boolean
  Private ControlList As New Collections.Generic.List(Of Control)
  Public Sub New()
    MyBase.New()
  End Sub
  Public Sub New(cont As System.ComponentModel.IContainer)
    MyBase.New(cont)
  End Sub
  <System.ComponentModel.DefaultValue(False), System.ComponentModel.Description("Toggles persistant tooltips which will show up on disabled items.")>
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
  Public Shadows Sub SetTooltip(ctl As System.Windows.Forms.Control, caption As String)
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
End Class