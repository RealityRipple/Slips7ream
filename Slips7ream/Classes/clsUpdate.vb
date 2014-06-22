Public Class clsUpdate
  Implements IDisposable
  Private Const VersionURL As String = "http://update.realityripple.com/Slips7ream/ver"
  Class ProgressEventArgs
    Inherits EventArgs
    Public BytesReceived As Long
    Public ProgressPercentage As Integer
    Public TotalBytesToReceive As Long
    Friend Sub New(bReceived As Long, bToReceive As Long, iPercentage As Integer)
      BytesReceived = bReceived
      TotalBytesToReceive = bToReceive
      ProgressPercentage = iPercentage
    End Sub
  End Class
  Class CheckEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs
    Enum ResultType
      NoUpdate
      NewUpdate
    End Enum
    Public Result As ResultType
    Public Version As String
    Friend Sub New(rtResult As ResultType, sVersion As String, ex As Exception, bCancelled As Boolean, state As Object)
      MyBase.New(ex, bCancelled, state)
      Version = sVersion
      Result = rtResult
    End Sub
  End Class
  Public Event CheckingVersion(sender As Object, e As EventArgs)
  Public Event CheckProgressChanged(sender As Object, e As ProgressEventArgs)
  Public Event CheckResult(sender As Object, e As CheckEventArgs)
  Public Event DownloadingUpdate(sender As Object, e As EventArgs)
  Public Event UpdateProgressChanged(sender As Object, e As ProgressEventArgs)
  Public Event DownloadResult(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
  Private WithEvents wsVer As New Net.WebClient
  Private DownloadURL As String
#Region "IDisposable Support"
  Private disposedValue As Boolean ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        If wsVer IsNot Nothing Then
          If wsVer.IsBusy Then
            wsVer.CancelAsync()
            'If wsVer Is Nothing Then Exit Sub
            'Do While wsVer.IsBusy
            '  Application.DoEvents()
            '  Threading.Thread.Sleep(10)
            '  If wsVer Is Nothing Then Exit Sub
            'Loop
          End If
          wsVer.Dispose()
          wsVer = Nothing
        End If
        ' TODO: dispose managed state (managed objects).
      End If

      ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
      ' TODO: set large fields to null.
    End If
    Me.disposedValue = True
  End Sub

  ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
  'Protected Overrides Sub Finalize()
  '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
  '    Dispose(False)
  '    MyBase.Finalize()
  'End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region
  Public Sub CheckVersion()
    wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
    wsVer.DownloadStringAsync(New Uri(VersionURL), "INFO")
    RaiseEvent CheckingVersion(Me, New EventArgs)
  End Sub
  Public Shared Function QuickCheckVersion() As CheckEventArgs.ResultType
    Dim sVerStr As String
    Using wsCheck As New Net.WebClient
      wsCheck.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
      Try
        sVerStr = wsCheck.DownloadString(New Uri(VersionURL))
      Catch ex As Exception
        Return CheckEventArgs.ResultType.NoUpdate
      End Try
    End Using

    Dim sVU() As String = sVerStr.Split("|"c)
    If CompareVersions(sVU(0)) Then
      Return CheckEventArgs.ResultType.NewUpdate
    End If

    Return CheckEventArgs.ResultType.NoUpdate
  End Function
  Public Sub DownloadUpdate(toLocation As String)
    If DownloadURL IsNot Nothing Then
      wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
      wsVer.DownloadFileAsync(New Uri(DownloadURL), toLocation, "FILE")
      RaiseEvent DownloadingUpdate(Me, New EventArgs)
    Else
      RaiseEvent DownloadResult(Me, New System.ComponentModel.AsyncCompletedEventArgs(New Exception("Version Check was not run."), True, Nothing))
    End If
  End Sub
  Private Sub wsVer_DownloadProgressChanged(sender As Object, e As System.Net.DownloadProgressChangedEventArgs) Handles wsVer.DownloadProgressChanged
    If e.UserState = "INFO" Then
      RaiseEvent CheckProgressChanged(sender, New ProgressEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage))
    ElseIf e.UserState = "FILE" Then
      RaiseEvent UpdateProgressChanged(sender, New ProgressEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage))
    End If
  End Sub
  Private Sub wsVer_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsVer.DownloadStringCompleted
    Dim rRet As CheckEventArgs.ResultType = CheckEventArgs.ResultType.NoUpdate
    Dim sVer As String = Nothing
    DownloadURL = Nothing
    If e.Error Is Nothing Then
      Try
        Dim sVerStr As String = e.Result
        Dim sVU() As String = sVerStr.Split("|"c)
          If CompareVersions(sVU(0)) Then
            rRet = CheckEventArgs.ResultType.NewUpdate
            DownloadURL = sVU(1)
            sVer = sVU(0)
          End If
      Catch ex As Exception
        RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, sVer, New Exception("Version Parsing Error", ex), e.Cancelled, e.UserState))
      End Try
    End If
    RaiseEvent CheckResult(sender, New CheckEventArgs(rRet, sVer, e.Error, e.Cancelled, e.UserState))
  End Sub
  Private Sub wsVer_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wsVer.DownloadFileCompleted
    RaiseEvent DownloadResult(sender, e)
  End Sub
  Public Shared Function CompareVersions(sRemote As String, Optional sLocal As String = Nothing) As Boolean
    If String.IsNullOrEmpty(sLocal) Then sLocal = Application.ProductVersion
    Dim LocalVer(3) As String
    If sLocal.Contains(".") Then
      For I As Integer = 1 To 3
        LocalVer(0) = sLocal.Split(".")(0)
        If sLocal.Split(".").Count > I Then
          Dim sTmp As String = sLocal.Split(".")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          LocalVer(I) = sTmp
        Else
          LocalVer(I) = "0000"
        End If
      Next
    ElseIf sLocal.Contains(",") Then
      LocalVer(0) = sLocal.Split(".")(0)
      For I As Integer = 1 To 3
        If sLocal.Split(",").Count > I Then
          Dim sTmp As String = sLocal.Split(",")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          LocalVer(I) = sTmp
        Else
          LocalVer(I) = "0000"
        End If
      Next
    End If
    Dim RemoteVer(3) As String
    If sRemote.Contains(".") Then
      RemoteVer(0) = sRemote.Split(".")(0)
      For I As Integer = 1 To 3
        If sRemote.Split(".").Count > I Then
          Dim sTmp As String = sRemote.Split(".")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          RemoteVer(I) = sTmp
        Else
          RemoteVer(I) = "0000"
        End If
      Next
    ElseIf sRemote.Contains(",") Then
      RemoteVer(0) = sRemote.Split(",")(0)
      For I As Integer = 1 To 3
        If sRemote.Split(",").Count > I Then
          Dim sTmp As String = sRemote.Split(",")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          RemoteVer(I) = sTmp
        Else
          RemoteVer(I) = "0000"
        End If
      Next
    End If
    Dim bUpdate As Boolean = False
    If Val(LocalVer(0)) > Val(RemoteVer(0)) Then
      'Local's OK
    ElseIf Val(LocalVer(0)) = Val(RemoteVer(0)) Then
      If Val(LocalVer(1)) > Val(RemoteVer(1)) Then
        'Local's OK
      ElseIf Val(LocalVer(1)) = Val(RemoteVer(1)) Then
        If Val(LocalVer(2)) > Val(RemoteVer(2)) Then
          'Local's OK
        ElseIf Val(LocalVer(2)) = Val(RemoteVer(2)) Then
          If Val(LocalVer(3)) >= Val(RemoteVer(3)) Then
            'Local's OK
          Else
            bUpdate = True
          End If
        Else
          bUpdate = True
        End If
      Else
        bUpdate = True
      End If
    Else
      bUpdate = True
    End If
    Return bUpdate
  End Function
End Class