Imports System.ComponentModel
<ToolboxBitmap(GetType(System.Windows.Forms.ToolStripSeparator))>
Public Class LineBreak
  Public Sub New()
    InitializeComponent()
    Me.TabStop = False
  End Sub
  Private Sub LineBreak_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
    Using g As Graphics = Graphics.FromHwnd(Me.Handle)
      g.DrawLine(New Pen(SystemColors.ButtonShadow, 1), Me.Padding.Left, Me.Padding.Top, Me.DisplayRectangle.Width - (Me.Padding.Right + Me.Padding.Left), Me.Padding.Top)
      g.DrawLine(New Pen(SystemColors.ButtonHighlight, 1), Me.Padding.Left, Me.Padding.Top + 1, Me.DisplayRectangle.Width - (Me.Padding.Right + Me.Padding.Left), Me.Padding.Top + 1)
    End Using
  End Sub
End Class
