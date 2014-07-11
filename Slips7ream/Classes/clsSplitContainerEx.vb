Imports System.ComponentModel
Imports System.Windows.Forms
<ToolboxBitmap(GetType(System.Windows.Forms.SplitContainer))>
Public Class SplitContainerEx
  Inherits SplitContainer
  Private c_DrawHandle As Boolean
  Private c_ResizeRect As Boolean
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
      Dim SmallerRect As New Rectangle(Me.SplitterRectangle.X + 1, Me.SplitterRectangle.Y, Me.SplitterRectangle.Width - 2, Me.SplitterRectangle.Height)
      e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), Me.SplitterRectangle)
      ControlPaint.DrawBorder3D(e.Graphics, SmallerRect, Border3DStyle.SunkenInner)
      'ControlPaint.DrawGrabHandle(e.Graphics, New Rectangle(Me.SplitterRectangle.X + 3, Me.SplitterRectangle.Y, Me.SplitterRectangle.Width - 6, Me.SplitterRectangle.Height), Me.Focused, Me.Enabled)
    End If

  End Sub

  Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
    If Not c_ResizeRect Then MyBase.IsSplitterFixed = True
    MyBase.OnMouseDown(e)
  End Sub
  Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
    MyBase.OnMouseMove(e)
    If Not c_ResizeRect And MyBase.IsSplitterFixed Then
      If e.Button = Windows.Forms.MouseButtons.Left Then
        If MyBase.Orientation = Windows.Forms.Orientation.Vertical Then
          If e.X > 0 And e.X < MyBase.Width Then
            MyBase.SplitterDistance = e.X
            MyBase.Refresh()
          End If
        ElseIf MyBase.Orientation = Windows.Forms.Orientation.Horizontal Then
          If e.Y > 0 And e.Y < MyBase.Height Then
            MyBase.SplitterDistance = e.Y
            MyBase.Refresh()
          End If
        End If
      Else
        MyBase.IsSplitterFixed = False
      End If
    End If
  End Sub
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    MyBase.OnMouseUp(e)
    If Not c_ResizeRect Then MyBase.IsSplitterFixed = False
  End Sub
End Class
