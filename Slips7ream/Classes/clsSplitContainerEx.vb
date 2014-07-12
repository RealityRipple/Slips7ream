Imports System.ComponentModel
Imports System.Windows.Forms
<ToolboxBitmap(GetType(System.Windows.Forms.SplitContainer))>
Public Class SplitContainerEx
  Inherits SplitContainer
  Private c_DrawHandle As Boolean
  Private c_ResizeRect As Boolean
  Private c_Focus As Boolean
  <Description("Determines whether a handle is drawn on the splitter between panels for users to grab."), DefaultValue(False)>
  Public Property DrawGrabHandle As Boolean
    Get
      Return c_DrawHandle
    End Get
    Set(value As Boolean)
      c_DrawHandle = value
      Me.Invalidate()
    End Set
  End Property
  <Description("Determines whether the splitContainer resizes instantly or after the resize is complete."), DefaultValue(True)>
  Public Property ResizeRectangle As Boolean
    Get
      Return c_ResizeRect
    End Get
    Set(value As Boolean)
      c_ResizeRect = value
      Me.Invalidate()
    End Set
  End Property
  Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
    MyBase.OnPaint(e)
    If c_DrawHandle Then
      Dim SmallerRect As New Rectangle(Me.SplitterRectangle.X + 3, Me.SplitterRectangle.Y, Me.SplitterRectangle.Width - 6, Me.SplitterRectangle.Height)
      e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), Me.SplitterRectangle)
      If Me.Enabled Then
        If Me.Focused Or c_Focus Then
          ControlPaint.DrawBorder3D(e.Graphics, SmallerRect, Border3DStyle.RaisedOuter)
        Else
          ControlPaint.DrawBorder3D(e.Graphics, SmallerRect, Border3DStyle.RaisedInner)
        End If
      Else
        ControlPaint.DrawBorder3D(e.Graphics, SmallerRect, Border3DStyle.Etched)
      End If
    End If
  End Sub
  Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ResizeRect And e.Button = Windows.Forms.MouseButtons.Left Then MyBase.IsSplitterFixed = True : c_Focus = True : MyBase.Invalidate(MyBase.SplitterRectangle)
    MyBase.OnMouseDown(e)
  End Sub
  Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
    MyBase.OnMouseMove(e)
    If Not c_ResizeRect And MyBase.IsSplitterFixed Then
      If e.Button = Windows.Forms.MouseButtons.Left Then
        If MyBase.Orientation = Windows.Forms.Orientation.Vertical Then
          If e.X > 0 And e.X < MyBase.Width Then
            MyBase.SplitterDistance = e.X
            MyBase.Invalidate(MyBase.SplitterRectangle)
          End If
        Else
          If e.Y > 0 And e.Y < MyBase.Height Then
            MyBase.SplitterDistance = e.Y
            MyBase.Invalidate(MyBase.SplitterRectangle)
          End If
        End If
      Else
        MyBase.IsSplitterFixed = False
      End If
    End If
    If MyBase.SplitterRectangle.Contains(e.Location) Then
      If MyBase.Orientation = Windows.Forms.Orientation.Vertical Then
        MyBase.Cursor = Cursors.VSplit
      Else
        MyBase.Cursor = Cursors.HSplit
      End If
    End If
  End Sub
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ResizeRect And e.Button = Windows.Forms.MouseButtons.Left Then MyBase.IsSplitterFixed = False : c_Focus = False : MyBase.Invalidate(MyBase.SplitterRectangle)
    MyBase.OnMouseUp(e)
  End Sub
  Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
    MyBase.OnMouseLeave(e)
    MyBase.Cursor = ParentForm.Cursor
  End Sub
  Protected Overrides Sub OnParentCursorChanged(e As System.EventArgs)
    MyBase.OnParentCursorChanged(e)
    If MyBase.Cursor = Cursors.VSplit Or MyBase.Cursor = Cursors.HSplit Then Exit Sub
    MyBase.Cursor = ParentForm.Cursor
  End Sub
  Protected Overrides Sub OnKeyDown(e As System.Windows.Forms.KeyEventArgs)
    If Not c_ResizeRect Then MyBase.IsSplitterFixed = True
    MyBase.OnKeyDown(e)
    If Not c_ResizeRect And MyBase.IsSplitterFixed Then
      If MyBase.Orientation = Windows.Forms.Orientation.Vertical Then
        If e.KeyCode = Keys.Left Then
          MyBase.SplitterDistance -= MyBase.SplitterIncrement
          MyBase.Invalidate(MyBase.SplitterRectangle)
        ElseIf e.KeyCode = Keys.Right Then
          MyBase.SplitterDistance += MyBase.SplitterIncrement
          MyBase.Invalidate(MyBase.SplitterRectangle)
        End If
      Else
        If e.KeyCode = Keys.Up Then
          MyBase.SplitterDistance -= MyBase.SplitterIncrement
          MyBase.Invalidate(MyBase.SplitterRectangle)
        ElseIf e.KeyCode = Keys.Down Then
          MyBase.SplitterDistance += MyBase.SplitterIncrement
          MyBase.Invalidate(MyBase.SplitterRectangle)
        End If
      End If
    End If
  End Sub
  Protected Overrides Sub OnKeyUp(e As System.Windows.Forms.KeyEventArgs)
    If Not c_ResizeRect And MyBase.IsSplitterFixed Then MyBase.IsSplitterFixed = False
    MyBase.OnKeyUp(e)
    If Not c_ResizeRect Then MyBase.Invalidate(MyBase.SplitterRectangle)
  End Sub
  Protected Overrides Sub OnLostFocus(e As System.EventArgs)
    MyBase.OnLostFocus(e)
    If Not c_ResizeRect And MyBase.IsSplitterFixed Then MyBase.IsSplitterFixed = False : c_Focus = False
  End Sub
  Protected Overrides Sub OnResize(e As System.EventArgs)
    MyBase.OnResize(e)
    If Not c_ResizeRect Then MyBase.Invalidate(MyBase.SplitterRectangle)
  End Sub
End Class
