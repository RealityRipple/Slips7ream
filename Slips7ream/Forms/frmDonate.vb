Public Class frmDonate
  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    Process.Start("http://realityripple.com/donate.php?itm=SLIPS7REAM")
    frmMain.Donate_Clicked()
    Me.Close()
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
End Class