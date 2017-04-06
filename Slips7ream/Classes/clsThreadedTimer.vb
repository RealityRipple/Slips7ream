Public Class ThreadedTimer
  Implements IDisposable
  Public Event Tick(sener As Object, e As EventArgs)
  Public Interval As ULong
  Private c_Parent As Control
  Private tX As Threading.Thread
  Public Sub New(parent As Control, timeout As ULong)
    c_Parent = parent
    Interval = timeout
  End Sub
  Public Sub Start()
    tX = New Threading.Thread(AddressOf Thread_Tick)
    tX.Start()
  End Sub
  Public Sub [Stop]()
    tX = Nothing
  End Sub
  Public ReadOnly Property IsActive As Boolean
    Get
      Return tX IsNot Nothing
    End Get
  End Property
  Private Sub Thread_Tick()
    Dim lastFlick As ULong = 0
    Do
      If lastFlick = 0 Then
        lastFlick = FlickCount()
        Continue Do
      End If
      Dim thisFlick As ULong = FlickCount() - lastFlick
      Dim checkInt As ULong = Interval
      If checkInt <= 5 Then
        checkInt = 1
      ElseIf checkInt <= 8 Then
        checkInt = 5
      ElseIf checkInt <= 1000 Then
        checkInt -= 3UL
      Else
        checkInt -= 7UL
      End If
      Threading.Thread.Sleep(1)
      If thisFlick >= checkInt Then
        RaiseTickA()
        lastFlick = FlickCount()
      End If
    Loop Until tX Is Nothing
  End Sub
  Private Sub RaiseTickA()
    If tX Is Nothing Then Return
    If c_Parent IsNot Nothing Then
      If c_Parent.Disposing Or c_Parent.IsDisposed Then Return
      c_Parent.BeginInvoke(New MethodInvoker(AddressOf RaiseTickB))
      Return
    End If
    RaiseTickB()
  End Sub
  Private Sub RaiseTickB()
    If tX Is Nothing Then Return
    If c_Parent IsNot Nothing Then
      If c_Parent.InvokeRequired Then
        If c_Parent.Disposing Or c_Parent.IsDisposed Then Return
        c_Parent.Invoke(New MethodInvoker(AddressOf RaiseTickB))
        Return
      End If
    End If
    RaiseEvent Tick(Me, New EventArgs)
  End Sub
  Public Shared Function FlickCount() As ULong
    Return CULng(CDbl(Stopwatch.GetTimestamp) / CDbl(Stopwatch.Frequency) * 100000)
  End Function
#Region "IDisposable Support"
  Private disposedValue As Boolean 
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        tX = Nothing
      End If
    End If
    Me.disposedValue = True
  End Sub
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region
End Class
