Public Structure Update_Identity
  Public Name As String
  Public Token As String
  Public Architecture As String
  Public Language As String
  Public Version As String
  Public Sub New(Info As String)
    Dim pDatas() As String = Split(Info, "~")
    Name = pDatas(0)
    Token = pDatas(1)
    Architecture = pDatas(2)
    Language = pDatas(3)
    Version = pDatas(4)
    If String.IsNullOrEmpty(Architecture) Then Architecture = "neutral"
    If String.IsNullOrEmpty(Language) Then Language = "neutral"
  End Sub
End Structure
