Public Class frmTetrisHelp
  Private skipFocus As Boolean = False

  Private Sub frmTetrisHelp_GotFocus(sender As Object, e As System.EventArgs) Handles Me.GotFocus
    If skipFocus Then
      skipFocus = False
    Else
      Me.Hide()
    End If
  End Sub

  Private Sub frmTetrisHelp_VisibleChanged(sender As Object, e As System.EventArgs) Handles Me.VisibleChanged
    If Me.Visible Then
      skipFocus = True
    End If
  End Sub
End Class