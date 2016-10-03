Public Structure Feature
  Public FeatureName As String
  Public Enable As Boolean
  Public State As String
  Public DisplayName As String
  Public Desc As String
  Public RestartRequired As String
  Public CustomProperties() As String
  Public Sub New(Info As String)
    Enable = True
    Dim infoLines() As String = Split(Info, vbNewLine)
    For I As Integer = 0 To infoLines.Length - 1
      Dim line As String = infoLines(I)
      If line.StartsWith("Feature Name :") Then
        FeatureName = GetVal(line)
      ElseIf line.StartsWith("Display Name :") Then
        DisplayName = GetVal(line)
      ElseIf line.StartsWith("Description :") Then
        Desc = GetVal(line)
      ElseIf line.StartsWith("Restart Required :") Then
        RestartRequired = GetVal(line)
      ElseIf line.StartsWith("State :") Then
        State = GetVal(line)
        Enable = ((State = "Enabled") Or (State = "Enable Pending"))
      ElseIf line.StartsWith("Custom Properties:") Then
        Dim sProp As String = String.Empty
        Dim sProps As New Collections.Generic.List(Of String)
        Dim J As Integer = I + 1
        Do
          J += 1
          If infoLines.Length - 1 < J Then Exit Do
          sProp = infoLines(J).Trim
          sProp = sProp.Replace(vbTab, "")
          If Not String.IsNullOrEmpty(sProp) Then sProps.Add(sProp)
        Loop Until String.IsNullOrEmpty(sProp)
        CustomProperties = sProps.ToArray
      End If
    Next
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Return line.Substring(line.IndexOf(" : ") + 3)
    Else
      Return Nothing
    End If
  End Function
End Structure
