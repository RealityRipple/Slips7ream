Module modFunctions
  Private Declare Auto Function GetShortPathName Lib "kernel32.dll" (ByVal lpszLongPath As String, ByVal lpszShortPath As String, ByVal cchBuffer As Int32) As Int32
  Public Structure UpdateInfo
    Public IsFile As Boolean
    Public Path As String
    Public Sub New(Location As String, File As Boolean)
      IsFile = File
      Path = Location
    End Sub
  End Structure
  Public Structure UpdateInfoEx
    Public Path As String
    Public DisplayName As String
    Public AppliesTo As String
    Public Architecture As String
    Public BuildDate As String
    Public KBArticle As String
    Public SupportLink As String
    Public Sub New(Location As String)
      Path = Location
      If My.Computer.FileSystem.FileExists(Location) Then
        Select Case GetUpdateType(Location)
          Case UpdateType.MSU
            Dim MSUPath As String = WorkDir & "UpdateMSU_Extract\"
            If My.Computer.FileSystem.DirectoryExists(MSUPath) Then My.Computer.FileSystem.DeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(MSUPath)
            ExtractAFile(Location, MSUPath, "*pkgProperties.txt")
            Dim filePath() As String = My.Computer.FileSystem.GetFiles(MSUPath, FileIO.SearchOption.SearchTopLevelOnly).ToArray
            If filePath.Count >= 1 Then
              Dim sFile As String = filePath(0)
              DisplayName = IO.Path.GetFileNameWithoutExtension(sFile)
              DisplayName = DisplayName.Substring(0, DisplayName.Length - 14)
              Dim sMSU As String = My.Computer.FileSystem.ReadAllText(sFile)
              Dim sData() As String = Split(sMSU, vbNewLine)
              For Each sLine As String In sData
                If sLine.StartsWith("Applies to=") Then
                  AppliesTo = GetMSUValue(sLine)
                ElseIf sLine.StartsWith("Processor Architecture=") Then
                  Architecture = GetMSUValue(sLine)
                ElseIf sLine.StartsWith("Build Date=") Then
                  BuildDate = GetMSUValue(sLine)
                ElseIf sLine.StartsWith("KB Article Number=") Then
                  KBArticle = GetMSUValue(sLine)
                ElseIf sLine.StartsWith("Support Link=") Then
                  SupportLink = GetMSUValue(sLine)
                End If
              Next
            Else
              'can't read
            End If
            If My.Computer.FileSystem.DirectoryExists(MSUPath) Then My.Computer.FileSystem.DeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.CAB
            Dim CABPath As String = WorkDir & "UpdateCAB_Extract\"
            If My.Computer.FileSystem.DirectoryExists(CABPath) Then My.Computer.FileSystem.DeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(CABPath)
            ExtractAFile(Location, CABPath, "update.mum")
            If My.Computer.FileSystem.FileExists(CABPath & "update.mum") Then
              Dim xMUM As XElement = XElement.Load(CABPath & "update.mum")
              DisplayName = xMUM.Attribute("displayName")
              SupportLink = xMUM.Attribute("supportInformation")
              Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              Architecture = xAssemblyIdentity.Attribute("processorArchitecture")
              Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
              KBArticle = xPackage.Attribute("identifier")
              Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
              Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              AppliesTo = xParentAssemblyIdentity.Attribute("name")
              BuildDate = Nothing
            Else
              'can't read
            End If
            If My.Computer.FileSystem.DirectoryExists(CABPath) Then My.Computer.FileSystem.DeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.LP
            Dim LPPath As String = WorkDir & "UpdateLP_Extract\"
            If My.Computer.FileSystem.DirectoryExists(LPPath) Then My.Computer.FileSystem.DeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(LPPath)
            ExtractAFile(Location, LPPath, "update.mum")
            If My.Computer.FileSystem.FileExists(LPPath & "update.mum") Then
              Dim xMUM As XElement = XElement.Load(LPPath & "update.mum")
              Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              DisplayName = xAssemblyIdentity.Attribute("language").Value & " Multilingual User Interface Pack"
              SupportLink = Nothing '"http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
              Architecture = xAssemblyIdentity.Attribute("processorArchitecture")
              Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
              KBArticle = Nothing
              Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
              Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              AppliesTo = xParentAssemblyIdentity.Attribute("name")
              BuildDate = Nothing
            Else
              'can't read
            End If
            If My.Computer.FileSystem.DirectoryExists(LPPath) Then My.Computer.FileSystem.DeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.LIP
            Dim MLCPath As String = WorkDir & "UpdateMLC_Extract\"
            If My.Computer.FileSystem.DirectoryExists(MLCPath) Then My.Computer.FileSystem.DeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(MLCPath)
            ExtractAFile(Location, MLCPath, "update.mum")
            If My.Computer.FileSystem.FileExists(MLCPath & "update.mum") Then
              Dim xMUM As XElement = XElement.Load(MLCPath & "update.mum")
              Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              DisplayName = xAssemblyIdentity.Attribute("language").Value & " Language Interface Pack"
              SupportLink = Nothing '"http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
              Architecture = xAssemblyIdentity.Attribute("processorArchitecture")
              Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
              KBArticle = Nothing
              Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
              Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              AppliesTo = xParentAssemblyIdentity.Attribute("name")
              BuildDate = Nothing
            Else
              'can't read
            End If
            If My.Computer.FileSystem.DirectoryExists(MLCPath) Then My.Computer.FileSystem.DeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.EXE
            Dim EXEPath As String = WorkDir & "UpdateEXE_Extract\"
            If My.Computer.FileSystem.DirectoryExists(EXEPath) Then My.Computer.FileSystem.DeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(EXEPath)
            ExtractAFile(Location, EXEPath, "update.mum")
            If My.Computer.FileSystem.FileExists(EXEPath & "update.mum") Then
              Dim xMUM As XElement = XElement.Load(EXEPath & "update.mum")
              Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              DisplayName = xAssemblyIdentity.Attribute("language").Value & " Multilingual User Interface Pack"
              SupportLink = Nothing
              Architecture = xAssemblyIdentity.Attribute("processorArchitecture")
              Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
              KBArticle = Nothing
              Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
              Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              AppliesTo = xParentAssemblyIdentity.Attribute("name")
              BuildDate = Nothing
            Else
              'can't read
            End If
            If My.Computer.FileSystem.DirectoryExists(EXEPath) Then My.Computer.FileSystem.DeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.Other
            DisplayName = Nothing
            AppliesTo = Nothing
            SupportLink = Nothing
            Architecture = Nothing
            KBArticle = Nothing
            BuildDate = Nothing
        End Select
      End If
    End Sub
    Private Sub ExtractAFile(Source As String, Destination As String, File As String)
      Dim SevenZ As String = AIKDir & "7z.exe"
      RunHidden(SevenZ, "e -y -o""" & Destination & """ """ & Source & """ " & File)
    End Sub
    Private Function GetMSUValue(Line As String) As String
      Dim sTmp As String = Line.Substring(Line.IndexOf("="c) + 1)
      Return sTmp.Substring(1, sTmp.Length - 2)
    End Function
    Private Sub RunHidden(Filename As String, Arguments As String)
      Dim mySettings As New MySettings
      Dim PkgList As New Process
      PkgList.StartInfo.FileName = Filename
      PkgList.StartInfo.Arguments = Arguments
      PkgList.StartInfo.UseShellExecute = True
      PkgList.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
      PkgList.Start()
      If mySettings.Timeout > 0 Then
        PkgList.WaitForExit(mySettings.Timeout)
      Else
        PkgList.WaitForExit()
      End If
    End Sub
    Private ReadOnly Property WorkDir As String
      Get
        Dim mySettings As New MySettings
        Dim tempDir As String = mySettings.TempDir
        If String.IsNullOrEmpty(tempDir) Then tempDir = My.Computer.FileSystem.SpecialDirectories.Temp & "\Slips7ream\"
        If Not My.Computer.FileSystem.DirectoryExists(tempDir) Then My.Computer.FileSystem.CreateDirectory(tempDir)
        Return tempDir
      End Get
    End Property
  End Structure
  Public Structure PackageInfo
    Public Index As Integer
    Public Name As String
    Public Desc As String
    Public Size As Long
    Public Sub New(i As Integer, n As String, d As String, s As Long)
      Index = i
      Name = n
      Desc = d
      Size = s
    End Sub
  End Structure
  Public Structure PackageInfoEx
    Public Index As Integer
    Public Name As String
    Public Desc As String
    Public Size As Long
    Public Architecture As String
    Public HAL As String
    Public Version As String
    Public SPBuild As Integer
    Public SPLevel As Integer
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
    Public Sub New(Info As String)
      Dim infoLines() As String = Split(Info, vbNewLine)
      For I As Integer = 0 To infoLines.Length - 1
        Dim line As String = infoLines(I)
        If line.StartsWith("Index : ") Then
          Index = GetVal(line)
        ElseIf line.StartsWith("Name : ") Then
          Name = GetVal(line)
        ElseIf line.StartsWith("Description : ") Then
          Desc = GetVal(line)
        ElseIf line.StartsWith("Size : ") Then
          Dim sTmp As String = GetVal(line)
          Size = NumericVal(sTmp.Substring(0, sTmp.Length - 6))
        ElseIf line.StartsWith("Architecture : ") Then
          Architecture = GetVal(line)
        ElseIf line.StartsWith("Hal : ") Then
          HAL = GetVal(line)
        ElseIf line.StartsWith("Version : ") Then
          Version = GetVal(line)
        ElseIf line.StartsWith("ServicePack Build : ") Then
          SPBuild = GetVal(line)
        ElseIf line.StartsWith("ServicePack Level : ") Then
          SPLevel = GetVal(line)
        ElseIf line.StartsWith("Edition : ") Then
          Edition = GetVal(line)
        ElseIf line.StartsWith("Installation : ") Then
          Installation = GetVal(line)
        ElseIf line.StartsWith("ProductType : ") Then
          ProductType = GetVal(line)
        ElseIf line.StartsWith("ProductSuite : ") Then
          ProductSuite = GetVal(line)
        ElseIf line.StartsWith("System Root : ") Then
          SystemRoot = GetVal(line)
        ElseIf line.StartsWith("Directories : ") Then
          Directories = GetVal(line)
        ElseIf line.StartsWith("Files : ") Then
          Files = GetVal(line)
        ElseIf line.StartsWith("Created : ") Then
          Created = GetVal(line)
        ElseIf line.StartsWith("Modified : ") Then
          Modified = GetVal(line)
        ElseIf line.StartsWith("Languages :") Then
          Dim sLang As String = String.Empty
          Dim sLangs As New Collections.Generic.List(Of String)
          Dim J As Integer = I
          Do
            J += 1
            sLang = Trim(infoLines(J))
            sLang = sLang.Replace(vbTab, "")
            If Not String.IsNullOrEmpty(sLang) Then sLangs.Add(sLang)
          Loop Until String.IsNullOrEmpty(sLang)
          LangList = sLangs.ToArray
        End If
      Next
    End Sub
    Private Function GetVal(line As String) As String
      Return line.Substring(line.IndexOf(" : ") + 3)
    End Function
  End Structure
  Public Function ByteSize(InBytes As UInt64) As String
    If InBytes > 1024 Then
      If InBytes / 1024 > 1024 Then
        If InBytes / 1024 / 1024 > 1024 Then
          If InBytes / 1024 / 1024 / 1024 > 1024 Then
            Return Format((InBytes) / 1024 / 1024 / 1024 / 1024, "0.0#") & " TB"
          Else
            Return Format((InBytes) / 1024 / 1024 / 1024, "0.0#") & " GB"
          End If
        Else
          Return Format((InBytes) / 1024 / 1024, "0.0#") & " MB"
        End If
      Else
        Return Format((InBytes) / 1024, "0.0#") & " KB"
      End If
    Else
      Return (InBytes) & " B"
    End If
  End Function
  Public ReadOnly Property AIKDir As String
    Get
      Dim tempDir As String = Application.StartupPath & "\tools\"
      If Environment.Is64BitProcess Then
        tempDir &= "amd64\"
      Else
        tempDir &= "x86\"
      End If
      Return tempDir
    End Get
  End Property
  Public Function ShortenPath(Path As String) As String
    Dim count As Integer = GetShortPathName(Path, Nothing, 0)
    Dim strShortPath As String = Space(count)
    Dim ret As Integer = GetShortPathName(Path, strShortPath, count)
    If ret > 0 Then
      Return strShortPath.Substring(0, ret)
    Else
      Return Path
    End If
  End Function
  Public Function NumericVal(value As String) As Long
    Dim sSize As String = value
    For J As Integer = sSize.Length - 1 To 0 Step -1
      If Not IsNumeric(sSize(J)) Then sSize = sSize.Substring(0, J) & sSize.Substring(J + 1)
    Next
    If String.IsNullOrEmpty(sSize) Then Return 0
    Dim lRet As Long
    If Long.TryParse(sSize, lRet) Then Return lRet
    Return 0
  End Function
  Public Enum UpdateType
    MSU
    CAB
    LIP
    LP
    EXE
    Other
  End Enum
  Public Function GetUpdateType(Path As String) As UpdateType
    If IO.Path.GetFileName(Path).ToLower = "lp.cab" Then Return UpdateType.LP
    If IO.Path.GetExtension(Path).ToLower = ".cab" Then Return UpdateType.CAB
    If IO.Path.GetExtension(Path).ToLower = ".mlc" Then Return UpdateType.LIP
    If IO.Path.GetExtension(Path).ToLower = ".exe" Then Return UpdateType.EXE
    If IO.Path.GetExtension(Path).ToLower = ".msu" Then Return UpdateType.MSU
    Return UpdateType.Other
  End Function
  Public Sub SortFiles(ByRef FileList() As String)
    Dim FilesOutOfOrder() As String = FileList
    For a As Integer = 1 To FilesOutOfOrder.Count - 1
      For b As Integer = 0 To FilesOutOfOrder.Count - 2
        If IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b + 1)).ToLower = "install" OrElse NumericVal(IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b + 1))) < NumericVal(IO.Path.GetFileNameWithoutExtension(FilesOutOfOrder(b))) Then
          Dim tmp As String = FilesOutOfOrder(b)
          FilesOutOfOrder(b) = FilesOutOfOrder(b + 1)
          FilesOutOfOrder(b + 1) = tmp
        End If
      Next
    Next
    FileList = FilesOutOfOrder
  End Sub
End Module
