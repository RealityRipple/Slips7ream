Imports System.ComponentModel
Imports System.Windows.Forms
<ToolboxBitmap(GetType(System.Windows.Forms.SplitContainer))>
Public Class SplitContainerEx
  Inherits SplitContainer
  Private c_DrawHandle As Boolean
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
  Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
    MyBase.OnPaint(e)
    If c_DrawHandle Then ControlPaint.DrawGrabHandle(e.Graphics, New Rectangle(Me.SplitterRectangle.X + 3, Me.SplitterRectangle.Y, Me.SplitterRectangle.Width - 6, Me.SplitterRectangle.Height), Me.Focused, Me.Enabled)
  End Sub
End Class
