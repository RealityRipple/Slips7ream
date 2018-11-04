Public Structure Update_Integrated
  Public [Type] As UpdateFileType
  Public Parent As ImagePackage
  Public Remove As Boolean
  Public Identity As String
  Public State As String
  Public ReleaseType As String
  Public InstallTime As String
  Public Ident As Update_Identity
  Public UpdateInfo As Update_Integrated_Info
  Public Sub New(owner As ImagePackage, Absent As Boolean)
    Parent = owner
    If Absent Then
      Type = UpdateFileType.Absent
    Else
      Type = UpdateFileType.Empty
    End If
    Remove = False
    Identity = Nothing
    Ident = Nothing
    State = Nothing
    ReleaseType = Nothing
    InstallTime = Nothing
    UpdateInfo = Nothing
  End Sub
  Public Sub New(owner As ImagePackage, Info As String)
    Parent = owner
    If Not String.IsNullOrEmpty(Info) AndAlso Info.Contains(vbNewLine) Then
      Dim infoLines() As String = Split(Info, vbNewLine)
      For I As Integer = 0 To infoLines.Length - 1
        Dim line As String = infoLines(I)
        If line.StartsWith("Package Identity :") Then
          Identity = GetVal(line)
          Ident = New Update_Identity(Identity)
        ElseIf line.StartsWith("State :") Then
          State = GetVal(line)
        ElseIf line.StartsWith("Release Type :") Then
          ReleaseType = GetVal(line)
        ElseIf line.StartsWith("Install Time :") Then
          InstallTime = GetVal(line)
        End If
      Next
      Type = UpdateFileType.Update
    Else
      Type = UpdateFileType.Failure
      Identity = Nothing
      Ident = Nothing
      State = Nothing
      ReleaseType = Nothing
      InstallTime = Nothing
    End If
    Remove = False
    UpdateInfo = Nothing
  End Sub
  Public Sub ParseInfo(Info As String)
    UpdateInfo = New Update_Integrated_Info(Info)
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Return line.Substring(line.IndexOf(" : ") + 3)
    Else
      Return Nothing
    End If
  End Function
End Structure

Public Structure Update_Integrated_Info
  Public Identity As String
  Public Applicable As String
  Public Copyright As String
  Public Company As String
  Public CreationTime As String
  Public Description As String
  Public InstallClient As String
  Public InstallPackageName As String
  Public InstallTime As String
  Public LastUpdateTime As String
  Public [Name] As String
  Public ProductName As String
  Public ProductVersion As String
  Public ReleaseType As String
  Public RestartRequired As String
  Public SupportInformation As String
  Public State As String
  Public CustomProperties() As String
  Public FeatureList() As String
  Public Sub New(Info As String)
    Dim infoLines() As String = Split(Info, vbNewLine)
    For I As Integer = 0 To infoLines.Length - 1
      Dim line As String = infoLines(I)
      If line.StartsWith("Package Identity :") Then
        Identity = GetVal(line)
      ElseIf line.StartsWith("Applicable :") Then
        Applicable = GetVal(line)
      ElseIf line.StartsWith("Copyright :") Then
        Copyright = GetVal(line)
      ElseIf line.StartsWith("Company :") Then
        Company = GetVal(line)
      ElseIf line.StartsWith("Creation Time :") Then
        CreationTime = GetVal(line)
      ElseIf line.StartsWith("Description :") Then
        Description = GetVal(line)
      ElseIf line.StartsWith("Install Client :") Then
        InstallClient = GetVal(line)
      ElseIf line.StartsWith("Install Package Name :") Then
        InstallPackageName = GetVal(line)
      ElseIf line.StartsWith("Install Time :") Then
        InstallTime = GetVal(line)
      ElseIf line.StartsWith("Last Update Time :") Then
        LastUpdateTime = GetVal(line)
      ElseIf line.StartsWith("Name :") Then
        [Name] = GetVal(line)
      ElseIf line.StartsWith("Product Name :") Then
        ProductName = GetVal(line)
      ElseIf line.StartsWith("Product Version :") Then
        ProductVersion = GetVal(line)
      ElseIf line.StartsWith("Release Type :") Then
        ReleaseType = GetVal(line)
      ElseIf line.StartsWith("Restart Required :") Then
        RestartRequired = GetVal(line)
      ElseIf line.StartsWith("Support Information :") Then
        SupportInformation = GetVal(line)
      ElseIf line.StartsWith("State :") Then
        State = GetVal(line)
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
        If sProps.Count = 1 AndAlso sProps(0) = "(No custom properties found)" Then
          CustomProperties = Nothing
        Else
          CustomProperties = sProps.ToArray
        End If
      ElseIf line.StartsWith("Features listing for package :") Then
        Dim sFeat As String = String.Empty
        Dim sFeats As New Collections.Generic.List(Of String)
        Dim J As Integer = I + 1
        Do
          J += 1
          If infoLines.Length - 1 < J Then Exit Do
          sFeat = infoLines(J).Trim
          sFeat = sFeat.Replace(vbTab, "")
          If Not String.IsNullOrEmpty(sFeat) Then sFeats.Add(sFeat)
        Loop Until String.IsNullOrEmpty(sFeat)
        If sFeats.Count = 1 And sFeats(0) = "(No features found for this package)" Then
          FeatureList = Nothing
        Else
          FeatureList = sFeats.ToArray
        End If
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
