Public Class frmUpdateProps
  Public KBLink As String
  Private Sub lblKBLink_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblKBLink.LinkClicked
    If String.IsNullOrEmpty(KBLink) Then
      lblKBLink.Link = False
    Else
      Process.Start(KBLink)
    End If
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
End Class
