Imports System.ComponentModel
<DefaultEvent("Opened"), DefaultProperty("Open")>
Public Class Expander
  Private Const EXP_OPEN As Integer = &H10
  Private Const EXP_CLOSED As Integer = &H20
  Private Const EXP_NORM As Integer = &H1
  Private Const EXP_HOVER As Integer = &H2
  Private Const EXP_DOWN As Integer = &H4
  Private c_Open As Boolean
  Private c_Focus As Boolean
  Private c_Hover As Boolean
  Private c_Text As String
  <Description("Event occurs when the expander has been opened.")>
  Public Event Opened(sender As Object, e As EventArgs)
  <Description("Event occurs when the expander has been closed.")>
  Public Event Closed(sender As Object, e As EventArgs)
  <DefaultValue(False), Browsable(True), EditorBrowsable(True), Description("Indicates whether the component is in the open state.")>
  Public Property Open As Boolean
    Get
      Return c_Open
    End Get
    Set(value As Boolean)
      c_Open = value
      DrawExpander(IIf(c_Open, EXP_OPEN, EXP_CLOSED))
    End Set
  End Property
  <DefaultValue("Expander"), Browsable(True), EditorBrowsable(True), Description("The text associated with the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
  Public Overrides Property Text As String
    Get
      Return c_Text
    End Get
    Set(value As String)
      c_Text = value
      lblExpander.Text = c_Text
      If String.IsNullOrEmpty(lblExpander.Text) Then
        lblExpander.Padding = New Padding(0)
      Else
        lblExpander.Padding = New Padding(3, 0, 3, 0)
      End If
    End Set
  End Property
  Public Sub New()
    InitializeComponent()
    If Not String.IsNullOrEmpty(c_Text) Then
      lblExpander.Text = c_Text
    Else
      lblExpander.Text = Nothing
    End If
    If String.IsNullOrEmpty(lblExpander.Text) Then
      lblExpander.Padding = New Padding(0)
    Else
      lblExpander.Padding = New Padding(3, 0, 3, 0)
    End If
    DrawExpander(IIf(c_Open, EXP_OPEN, EXP_CLOSED) Or EXP_NORM)
  End Sub
  Public Sub PerformClick()
    c_Open = Not c_Open
    DrawExpander(IIf(c_Open, EXP_OPEN, EXP_CLOSED))
    If c_Open Then
      RaiseEvent Opened(Me, New EventArgs)
    Else
      RaiseEvent Closed(Me, New EventArgs)
    End If
  End Sub
  Private Sub pctExpander_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctExpander.MouseDown
    If Not e.Button = Windows.Forms.MouseButtons.Left Then Return
    DrawExpander(EXP_DOWN)
  End Sub
  Private Sub pctExpander_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctExpander.MouseUp
    If Not e.Button = Windows.Forms.MouseButtons.Left Then Return
    If c_Hover Or c_Focus Then
      DrawExpander(EXP_HOVER)
    Else
      DrawExpander(EXP_NORM)
    End If
    c_Open = Not c_Open
    DrawExpander(IIf(c_Open, EXP_OPEN, EXP_CLOSED))
    If c_Open Then
      RaiseEvent Opened(Me, New EventArgs)
    Else
      RaiseEvent Closed(Me, New EventArgs)
    End If
  End Sub
  Private Sub pctExpander_MouseEnter(sender As Object, e As System.EventArgs) Handles pctExpander.MouseEnter
    c_Hover = True
    DrawExpander(EXP_HOVER)
  End Sub
  Private Sub pctExpander_MouseLeave(sender As Object, e As System.EventArgs) Handles pctExpander.MouseLeave
    c_Hover = False
    If Not c_Focus Then DrawExpander(EXP_NORM)
  End Sub
  Private Sub DrawExpander(iFlags As Integer)
    Static Last10 As Integer
    Static Last1 As Integer
    Dim iItem As Integer = -1
    If (iFlags And EXP_NORM) Then
      iItem = 0
    ElseIf (iFlags And EXP_HOVER) Then
      iItem = 1
    ElseIf (iFlags And EXP_DOWN) Then
      iItem = 2
    End If
    If iItem = -1 Then
      If (Last1 And EXP_NORM) Then
        iItem = 0
      ElseIf (Last1 And EXP_HOVER) Then
        iItem = 1
      ElseIf (Last1 And EXP_DOWN) Then
        iItem = 2
      End If
    Else
      Last1 = (iFlags And &HF)
    End If
    If iItem = -1 Then iItem = 0
    Dim DrawOpenClosed As TriState = TriState.UseDefault
    If iFlags And EXP_OPEN Then
      DrawOpenClosed = TriState.True
    ElseIf iFlags And EXP_CLOSED Then
      DrawOpenClosed = TriState.False
    End If
    If DrawOpenClosed = TriState.UseDefault Then
      If Last10 And EXP_OPEN Then
        DrawOpenClosed = TriState.True
      ElseIf Last10 And EXP_CLOSED Then
        DrawOpenClosed = TriState.False
      End If
    Else
      Last10 = (iFlags And &HF0)
    End If
    If DrawOpenClosed = TriState.True Then
      iItem += 3
    Else
      iItem += 0
    End If
    Const Square As Integer = 19
    Using expanderChunk As New Bitmap(Square, Square)
      Using g As Graphics = Graphics.FromImage(expanderChunk)
        g.Clear(Color.Transparent)
        g.DrawImage(My.Resources.expander, New Rectangle(0, 0, Square, Square), New Rectangle(0, (iItem * Square), Square, Square), GraphicsUnit.Pixel)
      End Using
      pctExpander.Image = expanderChunk.Clone
    End Using
    Application.DoEvents()
  End Sub
  Private Sub Expander_GotFocus(sender As Object, e As System.EventArgs) Handles Me.GotFocus
    c_Focus = True
    DrawExpander(EXP_HOVER)
  End Sub
  Private Sub Expander_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Space Then
      DrawExpander(EXP_DOWN)
    End If
  End Sub
  Private Sub Expander_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Space Then
      If c_Hover Or c_Focus Then
        DrawExpander(EXP_HOVER)
      Else
        DrawExpander(EXP_NORM)
      End If
      c_Open = Not c_Open
      DrawExpander(IIf(c_Open, EXP_OPEN, EXP_CLOSED))
      If c_Open Then
        RaiseEvent Opened(Me, New EventArgs)
      Else
        RaiseEvent Closed(Me, New EventArgs)
      End If
    End If
  End Sub
  Private Sub Expander_LostFocus(sender As Object, e As System.EventArgs) Handles Me.LostFocus
    c_Focus = False
    If Not c_Hover Then DrawExpander(EXP_NORM)
  End Sub
  Private Sub Expander_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
    If e.Delta < 0 And Not c_Open Then
      c_Open = True
      DrawExpander(EXP_OPEN)
      RaiseEvent Opened(Me, New EventArgs)
    ElseIf e.Delta > 0 And c_Open Then
      c_Open = False
      DrawExpander(EXP_CLOSED)
      RaiseEvent Closed(Me, New EventArgs)
    End If
  End Sub
End Class
