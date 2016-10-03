Public Structure ImagePackage
  Private UniqueID As String
  Public Index As Integer
  Public Name As String
  Public Desc As String
  Public Size As Long
  Public Architecture As String
  Public HAL As String
  Public Version As String
  Public SPLevel As Integer
  Public SPBuild As Integer
  Public Edition As String
  Public Installation As String
  Public ProductType As String
  Public ProductSuite As String
  Public SystemRoot As String
  Public Directories As Integer
  Public Files As Integer
  Public Created As String
  Public Modified As String
  Public LangList() As String
  Public IntegratedUpdateList As List(Of Update_Integrated)
  Public Sub New(Info As String)
    Dim infoLines() As String = Split(Info, vbNewLine)
    For I As Integer = 0 To infoLines.Length - 1
      Dim line As String = infoLines(I)
      If line.StartsWith("Index :") Then
        Index = CInt(GetVal(line))
      ElseIf line.StartsWith("Name :") Then
        Name = GetVal(line)
      ElseIf line.StartsWith("Description :") Then
        Desc = GetVal(line)
      ElseIf line.StartsWith("Size :") Then
        Dim sTmp As String = GetVal(line)
        Size = NumericVal(sTmp.Substring(0, sTmp.Length - 6))
      ElseIf line.StartsWith("Architecture :") Then
        Architecture = GetVal(line)
      ElseIf line.StartsWith("Hal :") Then
        HAL = GetVal(line)
      ElseIf line.StartsWith("Version :") Then
        Version = GetVal(line)
      ElseIf line.StartsWith("ServicePack Level :") Then
        SPLevel = CInt(GetVal(line))
      ElseIf line.StartsWith("ServicePack Build :") Then
        SPBuild = CInt(GetVal(line))
      ElseIf line.StartsWith("Edition :") Then
        Edition = GetVal(line)
      ElseIf line.StartsWith("Installation :") Then
        Installation = GetVal(line)
      ElseIf line.StartsWith("ProductType :") Then
        ProductType = GetVal(line)
      ElseIf line.StartsWith("ProductSuite :") Then
        ProductSuite = GetVal(line)
      ElseIf line.StartsWith("System Root :") Then
        SystemRoot = GetVal(line)
      ElseIf line.StartsWith("Directories :") Then
        Directories = CInt(GetVal(line))
      ElseIf line.StartsWith("Files :") Then
        Files = CInt(GetVal(line))
      ElseIf line.StartsWith("Created :") Then
        Created = GetVal(line)
      ElseIf line.StartsWith("Modified :") Then
        Modified = GetVal(line)
      ElseIf line.StartsWith("Languages :") Then
        Dim sLang As String = String.Empty
        Dim sLangs As New Collections.Generic.List(Of String)
        Dim J As Integer = I
        Do
          J += 1
          If infoLines.Length - 1 < J Then Exit Do
          sLang = infoLines(J).Trim
          sLang = sLang.Replace(vbTab, "")
          If Not String.IsNullOrEmpty(sLang) Then sLangs.Add(sLang)
        Loop Until String.IsNullOrEmpty(sLang)
        LangList = sLangs.ToArray
      End If
    Next
    UniqueID = BitConverter.ToString(New Security.Cryptography.MD5Cng().ComputeHash(System.Text.Encoding.GetEncoding(1252).GetBytes(String.Concat(Index, Size, Architecture, HAL, Version, SPLevel, SPBuild, Edition, Installation, ProductType, ProductSuite, SystemRoot, Directories, Files, Created, Modified, Join(LangList, ""))))).Replace("-", "")
  End Sub
  Public Sub PopulateUpdateList(List As List(Of Update_Integrated))
    IntegratedUpdateList = List
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Dim sRet As String = line.Substring(line.IndexOf(" : ") + 3)
      If sRet = "<undefined>" Then Return Nothing
      Return sRet
    Else
      Return Nothing
    End If
  End Function
  Public Overrides Function ToString() As String
    Return UniqueID
  End Function
  Public Shared Operator =(info1 As ImagePackage, info2 As ImagePackage) As Boolean
    If Not info1.Index = info2.Index Then Return False
    If Not info1.Desc = info2.Desc Then Return False
    If Not info1.Size = info2.Size Then Return False
    If Not CompareArchitectures(info1.Architecture, info2.Architecture, False) Then Return False
    If Not info1.HAL = info2.HAL Then Return False
    If Not info1.Version = info2.Version Then Return False
    If Not info1.SPLevel = info2.SPLevel Then Return False
    If Not info1.SPBuild = info2.SPBuild Then Return False
    If Not info1.Edition = info2.Edition Then Return False
    If Not info1.Installation = info2.Installation Then Return False
    If Not info1.ProductType = info2.ProductType Then Return False
    If Not info1.ProductSuite = info2.ProductSuite Then Return False
    If Not info1.Files = info2.Files Then Return False
    If Not info1.Directories = info2.Directories Then Return False
    If Not info1.Modified = info2.Modified Then Return False
    If info1.LangList Is Nothing Then
      If Not info2.LangList Is Nothing Then Return False
    ElseIf info2.LangList Is Nothing Then
      Return False
    End If
    If (Not info1.LangList Is Nothing) AndAlso (Not info2.LangList Is Nothing) Then
      If Not info1.LangList.Length = info2.LangList.Length Then Return False
      For I As Integer = 0 To info1.LangList.Length - 1
        If Not info1.LangList(I) = info2.LangList(I) Then Return False
      Next
    End If
    Return True
  End Operator
  Public Shared Operator <>(info1 As ImagePackage, info2 As ImagePackage) As Boolean
    Return Not info1 = info2
  End Operator
  Public ReadOnly Property IsEmpty As Boolean
    Get
      If Index > 0 Then Return False
      If Not String.IsNullOrEmpty(Desc) Then Return False
      If Size > 0 Then Return False
      If Not String.IsNullOrEmpty(Architecture) Then Return False
      If Not String.IsNullOrEmpty(HAL) Then Return False
      If Not String.IsNullOrEmpty(Version) Then Return False
      If SPLevel > 0 Then Return False
      If SPBuild > 0 Then Return False
      If Not String.IsNullOrEmpty(Edition) Then Return False
      If Not String.IsNullOrEmpty(Installation) Then Return False
      If Not String.IsNullOrEmpty(ProductType) Then Return False
      If Not String.IsNullOrEmpty(ProductSuite) Then Return False
      If Not String.IsNullOrEmpty(SystemRoot) Then Return False
      If Directories > 0 Then Return False
      If Files > 0 Then Return False
      If Not String.IsNullOrEmpty(Created) Then Return False
      If Not String.IsNullOrEmpty(Modified) Then Return False
      Return True
    End Get
  End Property
End Structure
