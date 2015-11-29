Public Class frmUpdateProps
  Private Sub lblKBLink_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblKBLink.LinkClicked
    Process.Start(lblKBLink.Tag)
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
End Class
