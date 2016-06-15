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
  Public Shared Operator =(info1 As Update_Identity, info2 As Update_Identity) As Boolean
    If info1.IsEmpty And info2.IsEmpty Then Return True
    If info1.IsEmpty Then Return False
    If info2.IsEmpty Then Return False
    If Not info1.Name.ToLower = info2.Name.ToLower Then Return False
    If Not CompareArchitectures(info1.Architecture, info2.Architecture, False) Then Return False
    If Not (info1.Language.ToLower = info2.Language.ToLower) And Not (info1.Language.ToLower = "neutral" And String.IsNullOrEmpty(info2.Language)) And Not (String.IsNullOrEmpty(info1.Language) And info2.Language.ToLower = "neutral") Then Return False
    Return True
  End Operator
  Public Shared Operator <>(info1 As Update_Identity, info2 As Update_Identity) As Boolean
    Return Not info1 = info2
  End Operator
  Public ReadOnly Property IsEmpty As Boolean
    Get
      If String.IsNullOrEmpty(Name) And String.IsNullOrEmpty(Token) And String.IsNullOrEmpty(Version) Then
        If String.IsNullOrEmpty(Architecture) OrElse Architecture = "neutral" Then
          If String.IsNullOrEmpty(Language) OrElse Version = "neutral" Then Return True
        End If
      End If
      Return False
    End Get
  End Property
End Structure
