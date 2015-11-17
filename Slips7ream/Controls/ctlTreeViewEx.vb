Public Class TreeViewEx
  Inherits TreeView
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    If m.Msg = &H203 And CheckBoxes Then
      Dim X As Integer = m.LParam.ToInt32 And &HFFFFI
      Dim Y As Integer = (m.LParam.ToInt32 >> 16) And &HFFFFI
      Dim htInfo As TreeViewHitTestInfo = HitTest(X, Y)
      If htInfo.Node IsNot Nothing AndAlso htInfo.Location = TreeViewHitTestLocations.StateImage Then
        OnBeforeCheck(New TreeViewCancelEventArgs(htInfo.Node, False, TreeViewAction.ByMouse))
        htInfo.Node.Checked = Not htInfo.Node.Checked
        OnAfterCheck(New TreeViewEventArgs(htInfo.Node, TreeViewAction.ByMouse))
        m.Result = IntPtr.Zero
        Return
      End If
    End If
    MyBase.WndProc(m)
  End Sub
End Class
