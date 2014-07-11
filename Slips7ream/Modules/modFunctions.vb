﻿Imports Microsoft.WindowsAPICodePack.Dialogs
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
    Public Failure As String
    Public Sub New(Location As String)
      Path = Location
      Failure = Nothing
      If My.Computer.FileSystem.FileExists(Location) Then
        Select Case GetUpdateType(Location)
          Case UpdateType.MSU
            Dim MSUPath As String = WorkDir & "UpdateMSU_Extract" & IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(MSUPath)
            Dim exRet As String = ExtractAFile(Location, MSUPath, "pkgProperties.txt")
            If Not exRet = "OK" Then
              Failure = exRet
              Exit Sub
            End If
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
                  If KBArticle.StartsWith("KB") Then KBArticle = KBArticle.Substring(2)
                ElseIf sLine.StartsWith("Support Link=") Then
                  SupportLink = GetMSUValue(sLine)
                End If
              Next
            Else
              Failure = "File Not Found"
            End If
            If My.Computer.FileSystem.DirectoryExists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.CAB
            Dim CABPath As String = WorkDir & "UpdateCAB_Extract" & IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(CABPath)
            Dim exRet As String = ExtractAFile(Location, CABPath, "update.mum")
            If Not exRet = "OK" Then
              Failure = exRet
              Exit Sub
            End If
            If My.Computer.FileSystem.FileExists(CABPath & "update.mum") Then
              Dim xMUM As XElement = XElement.Load(CABPath & "update.mum")
              DisplayName = xMUM.Attribute("displayName")
              SupportLink = xMUM.Attribute("supportInformation")
              Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              Architecture = xAssemblyIdentity.Attribute("processorArchitecture")
              Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
              KBArticle = xPackage.Attribute("identifier")
              If KBArticle.StartsWith("KB") Then KBArticle = KBArticle.Substring(2)
              Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
              Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
              AppliesTo = xParentAssemblyIdentity.Attribute("name")
              BuildDate = Nothing
            Else
              Failure = "File Not Found"
            End If
            If My.Computer.FileSystem.DirectoryExists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.LP
            Dim LPPath As String = WorkDir & "UpdateLP_Extract" & IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(LPPath)
            Dim exRet As String = ExtractAFile(Location, LPPath, "update.mum")
            If Not exRet = "OK" Then
              Failure = exRet
              Exit Sub
            End If
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
              Failure = "File Not Found"
            End If
            If My.Computer.FileSystem.DirectoryExists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.LIP
            Dim MLCPath As String = WorkDir & "UpdateMLC_Extract" & IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(MLCPath)
            Dim exRet As String = ExtractAFile(Location, MLCPath, "update.mum")
            If Not exRet = "OK" Then
              Failure = exRet
              Exit Sub
            End If
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
              Failure = "File Not Found"
            End If
            If My.Computer.FileSystem.DirectoryExists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.EXE
            Dim EXEPath As String = WorkDir & "UpdateEXE_Extract" & IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(EXEPath)
            Dim exRet As String = ExtractAFile(Location, EXEPath, "update.mum")
            If Not exRet = "OK" Then
              Failure = exRet
              Exit Sub
            End If
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
              Failure = "File Not Found"
            End If
            If My.Computer.FileSystem.DirectoryExists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          Case UpdateType.Other
            DisplayName = Nothing
            AppliesTo = Nothing
            SupportLink = Nothing
            Architecture = Nothing
            KBArticle = Nothing
            BuildDate = Nothing
            Failure = "Unknown File Type"
        End Select
      End If
    End Sub
    Private Extractor As Extraction.ArchiveFile
    Private Shared c_ExtractRet As New Collections.Generic.List(Of String)

    Private Delegate Sub ExtractAFileInvoker(Source As String, Destination As String, File As String)
    Private Function ExtractAFile(Source As String, Destination As String, File As String) As String
      Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf AsyncExtractAFile))
      Dim cIndex As Integer = c_ExtractRet.Count
      c_ExtractRet.Add(Nothing)
      tRunWithReturn.Start({Source, Destination, File, cIndex})
      Do While String.IsNullOrEmpty(c_ExtractRet(cIndex))
        Application.DoEvents()
        Threading.Thread.Sleep(1)
      Loop
      Dim sRet As String = c_ExtractRet(cIndex)
      c_ExtractRet(cIndex) = Nothing
      Return sRet
    End Function
    Private Sub AsyncExtractAFile(Obj As Object)
      Dim Source, Destination, Find As String
      Source = Obj(0)
      Destination = Obj(1)
      Find = Obj(2)
      Dim cIndex As UInteger = Obj(3)
      Dim bFound As Boolean = False
      If Not Destination.EndsWith(IO.Path.DirectorySeparatorChar) Then Destination &= IO.Path.DirectorySeparatorChar

      Using Extractor As New Extraction.ArchiveFile(New IO.FileInfo(Source))
        Try
          Extractor.Open()
        Catch ex As Exception
          c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
        End Try
        For Each File In Extractor
          If File.Name.ToLower.EndsWith(Find.ToLower) Then
            File.Destination = New IO.FileInfo(Destination & File.Name)
            bFound = True
            Exit For
          End If
        Next
        Try
          Extractor.Extract()
        Catch ex As Exception
          c_ExtractRet(cIndex) = "Error Extracting: " & ex.Message
        End Try
      End Using
      If bFound Then
        c_ExtractRet(cIndex) = "OK"
      Else
        c_ExtractRet(cIndex) = "File Not Found"
      End If
    End Sub

    Private Function GetMSUValue(Line As String) As String
      Dim sTmp As String = Line.Substring(Line.IndexOf("="c) + 1)
      Return sTmp.Substring(1, sTmp.Length - 2)
    End Function
    Private ReadOnly Property WorkDir As String
      Get
        Dim mySettings As New MySettings
        Dim tempDir As String = mySettings.TempDir
        If String.IsNullOrEmpty(tempDir) Then tempDir = My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "Slips7ream" & IO.Path.DirectorySeparatorChar
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
      Dim toolsDir As String = Application.StartupPath & IO.Path.DirectorySeparatorChar & "tools" & IO.Path.DirectorySeparatorChar
      If Environment.Is64BitProcess Then
        toolsDir &= "amd64" & IO.Path.DirectorySeparatorChar
      Else
        toolsDir &= "x86" & IO.Path.DirectorySeparatorChar
      End If
      Return toolsDir
    End Get
  End Property
  Public ReadOnly Property DismPath As String
    Get
      Dim localDir As String = AIKDir
      localDir &= "Dism.exe"
      Dim systemDir As String = Environment.SystemDirectory & IO.Path.DirectorySeparatorChar & "Dism.exe"
      If IO.File.Exists(systemDir) Then
        Dim sysInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(systemDir)
        Dim locInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(localDir)
        If clsUpdate.CompareVersions(sysInfo.ProductVersion, locInfo.ProductVersion) Then
          Return systemDir
        End If
      End If
      Return localDir
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
  Public Sub SlowDeleteDirectory(Directory As String, OnDirectoryNotEmpty As FileIO.DeleteDirectoryOption)
    If Not IO.Directory.Exists(Directory) Then Exit Sub
    If OnDirectoryNotEmpty = FileIO.DeleteDirectoryOption.ThrowIfDirectoryNonEmpty Then
      If IO.Directory.GetFileSystemEntries(Directory).Count > 0 Then Exit Sub
      IO.Directory.Delete(Directory, False)
    Else
      Dim sDirs() As String = IO.Directory.GetDirectories(Directory)
      Dim sFiles() As String = IO.Directory.GetFiles(Directory)
      For I As Integer = 0 To sDirs.Count - 1
        frmMain.SetTotal(I, sDirs.Count - 1)
        SlowDeleteDirectory(sDirs(I), OnDirectoryNotEmpty)
        If I Mod 25 = 0 Then Application.DoEvents()
      Next
      For I As Integer = 0 To sFiles.Count - 1
        If sFiles.Count > 1 Then frmMain.SetProgress(I, sFiles.Count - 1)
        Try
          IO.File.Delete(sFiles(I))
        Catch ex As Exception
        End Try
        If I Mod 25 = 0 Then Application.DoEvents()
      Next
      Try
        IO.Directory.Delete(Directory)
      Catch ex As Exception
      End Try
    End If
  End Sub
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
#Region "Task Dialogs"
  Public Function SelectionBox(owner As Form, newPath As String, newVer As Integer, oldPath As String, oldVer As Integer, ByRef Always As Boolean) As Boolean
    If TaskDialog.IsPlatformSupported Then
      Dim newData As New UpdateInfoEx(newPath)
      Dim oldData As New UpdateInfoEx(oldPath)
      If newVer > oldVer Then
        Using dlgUpdate As New TaskDialog
          dlgUpdate.Cancelable = False
          dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
          dlgUpdate.Caption = "Replace Older Version?"
          dlgUpdate.InstructionText = "There is already an older version of KB" & oldData.KBArticle & " in the Update List."
          dlgUpdate.StandardButtons = TaskDialogStandardButtons.None
          dlgUpdate.Text = "Click the version you want to keep"
          dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
          dlgUpdate.FooterCheckBoxChecked = False
          dlgUpdate.FooterCheckBoxText = "&Do this for all new versions"
          Dim em As String = ChrW(&H2003) & ChrW(&H2003)
          Dim sYes As String
          Dim newFInfo As New IO.FileInfo(newPath)
          If String.IsNullOrEmpty(newData.Failure) Then
            sYes = "Replace the update with this new version:" & vbNewLine &
              em & newData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newData.BuildDate
          Else
            sYes = "Replace the update with this new version:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(newPath) & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdYes As New CommandLink(
            "cmdNew",
            "Use Newer Version " & newVer,
            sYes)
          cmdYes.Default = True
          AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdYes)
          Dim sNo As String
          Dim oldFInfo As New IO.FileInfo(oldPath)
          If String.IsNullOrEmpty(oldData.Failure) Then
            sNo = "This update will not be replaced:" & vbNewLine &
              em & oldData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldData.BuildDate
          Else
            sNo = "This update will not be replaced:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(oldPath) & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdNo As New CommandLink(
            "cmdOld",
            "Use Older Version " & oldVer,
            sNo)
          AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdNo)
          dlgUpdate.OwnerWindowHandle = owner.Handle
          AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
          Dim ret As TaskDialogResult = dlgUpdate.Show()
          Always = dlgUpdate.FooterCheckBoxChecked
          If ret = TaskDialogResult.Yes Then Return True
          Return False
        End Using
      Else
        Using dlgUpdate As New TaskDialog
          dlgUpdate.Cancelable = False
          dlgUpdate.StartupLocation = TaskDialogStartupLocation.CenterOwner
          dlgUpdate.Caption = "Replace Newer Version?"
          dlgUpdate.InstructionText = "There is already a newer version of KB" & newData.KBArticle & " in the Update List."
          dlgUpdate.StandardButtons = TaskDialogStandardButtons.None
          dlgUpdate.Text = "Click the version you want to keep"
          dlgUpdate.Icon = TaskDialogIcon.WindowsUpdate
          dlgUpdate.FooterCheckBoxChecked = False
          dlgUpdate.FooterCheckBoxText = "&Do this for all old versions"
          Dim em As String = ChrW(&H2003) & ChrW(&H2003)
          Dim sYes As String
          Dim newFInfo As New IO.FileInfo(newPath)
          If String.IsNullOrEmpty(newData.Failure) Then
            sYes = "Replace the update with this old version:" & vbNewLine &
              em & newData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newData.BuildDate
          Else
            sYes = "Replace the update with this old version:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(newPath) & vbNewLine &
              em & "Size: " & ByteSize(newFInfo.Length) & vbNewLine &
              em & "Built: " & newFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdYes As New CommandLink(
            "cmdNew",
            "Use Older Version " & newVer,
            sYes)
          AddHandler cmdYes.Click, AddressOf SelectionDialogCommandLink_Click
          Dim sNo As String
          Dim oldFInfo As New IO.FileInfo(oldPath)
          If String.IsNullOrEmpty(oldData.Failure) Then
            sNo = "This update will not be replaced:" & vbNewLine &
              em & oldData.DisplayName & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldData.BuildDate
          Else
            sNo = "This update will not be replaced:" & vbNewLine &
              em & IO.Path.GetFileNameWithoutExtension(oldPath) & vbNewLine &
              em & "Size: " & ByteSize(oldFInfo.Length) & vbNewLine &
              em & "Built: " & oldFInfo.LastWriteTime.ToShortDateString
          End If
          Dim cmdNo As New CommandLink(
            "cmdOld",
            "Use Newer Version " & oldVer,
            sNo)
          cmdNo.Default = True
          AddHandler cmdNo.Click, AddressOf SelectionDialogCommandLink_Click
          dlgUpdate.Controls.Add(cmdNo)
          dlgUpdate.Controls.Add(cmdYes)
          dlgUpdate.OwnerWindowHandle = owner.Handle
          AddHandler dlgUpdate.Opened, AddressOf RefreshDlg
          Dim ret As TaskDialogResult
          Try
            ret = dlgUpdate.Show
          Catch ex As Exception
            Return SelectionBoxLegacy(owner, newPath, newVer, oldPath, oldVer, Always)
          End Try
          Always = dlgUpdate.FooterCheckBoxChecked
          If ret = TaskDialogResult.Yes Then Return True
          Return False
        End Using
      End If
    Else
      Return SelectionBoxLegacy(owner, newPath, newVer, oldPath, oldVer, Always)
    End If
  End Function
  Private Function SelectionBoxLegacy(owner As Form, newPath As String, newVer As Integer, oldPath As String, oldVer As Integer, ByRef Always As Boolean) As Boolean
    Dim oldData As New UpdateInfoEx(oldPath)
    Dim UpdateName As String
    If String.IsNullOrEmpty(oldData.Failure) Then
      UpdateName = oldData.DisplayName
    Else
      Dim newData As New UpdateInfoEx(newPath)
      If String.IsNullOrEmpty(newData.Failure) Then
        UpdateName = newData.DisplayName
      Else
        UpdateName = IO.Path.GetFileNameWithoutExtension(oldPath)
      End If
    End If
    If newVer > oldVer Then
      Return MessageBox.Show(owner, "There is already an older version of this update in the list." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newVer & "?", "Replace Older Version?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes
    Else
      Return MessageBox.Show(owner, "There is already a newer version of this update in the list." & vbNewLine & "Do you want to replace " & UpdateName & " with version " & newVer & "?", "Replace Newer Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End If
  End Function
  Private Sub SelectionDialogButton_Click(sender As TaskDialogButton, e As EventArgs)
    Select Case sender.Name
      Case "cmdYes" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Yes)
      Case "cmdNo" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.No)
      Case "cmdCancel" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Cancel)
      Case "cmdClose" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Close)
      Case "cmdAbort" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.CustomButtonClicked)
      Case "cmdOK" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Ok)
      Case "cmdRetry" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Retry)
    End Select
  End Sub
  Private Sub SelectionDialogCommandLink_Click(sender As TaskDialogCommandLink, e As EventArgs)
    Select Case sender.Name
      Case "cmdNew" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Yes)
      Case "cmdOld" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.No)
    End Select
  End Sub

  Private Class CommandLink
    Inherits TaskDialogCommandLink
    Public Sub New(name As String, text As String)
      MyBase.New(name, text)
    End Sub
    Public Sub New(name As String, text As String, instruction As String)
      MyBase.New(name, text, instruction)
    End Sub
    Public Overrides Function ToString() As String
      Dim str As String
      Dim noLabel As Boolean = String.IsNullOrEmpty(Me.Text)
      Dim noInstruction As Boolean = String.IsNullOrEmpty(Me.Instruction)
      If noLabel And noInstruction Then
        str = String.Empty
      ElseIf Not noLabel And noInstruction Then
        str = Me.Text
      ElseIf noLabel And Not noInstruction Then
        str = Me.Instruction
      Else
        str = Me.Text & vbLf & Me.Instruction
      End If
      Return str
    End Function
  End Class

  Public Enum TaskDialogIcon
    None = 0
    Space = &H1
    File = &H2
    OpenFolder = &H3
    OpenFolder2 = &H4
    ClosedFolder = &H5
    FolderOpening = &H6
    SearchCatalog = &H8
    CatalogFolder = &H9
    CatalogFolderOpening = &HA
    Games = &HE
    EXE = &HF
    SearchFolder = &H12
    AlignedTextFile = &H13
    Envelope = &H14
    PictureViewer = &H15
    MusicSheet = &H16
    VideoClip = &H17
    DefaultPrograms = &H18
    Internet = &H19
    PrinterFolder = &H1A
    ControlPanel = &H1B
    FloppyDrive35 = &H1C
    FloppyDrive525 = &H1D
    CDDrive = &H1E
    NetworkDriveDisconnected = &H1F
    HardDrive = &H20
    NetworkDrive = &H21
    MemoryChip = &H22
    RemovableDrive = &H23
    WindowsDrive = &H24
    DVDDrive = &H25
    DVDR = &H26
    DVDRAM = &H27
    DVDROM = &H28
    DVDRW = &H29
    ZipDrive = &H2A
    TapeDrive = &H2B
    BluDrive = &H2C
    PrinterInternet = &H2D
    Camcorder = &H2E
    Phone = &H2F
    PrinterDefaultFloppy = &H30
    PrinterDefault = &H31
    PrinterDefaultNetwork = &H32
    Printer = &H33
    PrinterFloppy = &H34
    PrinterNetwork = &H35
    TrashFull = &H36
    TrashEmpty = &H37
    DVD = &H38
    Camera = &H39
    CardReader = &H3A
    Padlock = &H3B
    SDCard = &H3C
    CD = &H3D
    CDR = &H3E
    CDROM = &H3F
    CDRW = &H40
    JewelCase = &H41
    MP3Player = &H42
    DLL = &H43
    Batch = &H44
    INI = &H45
    GIF = &H46
    BMP = &H47
    JPEG = &H48
    InternetFolder = &H49
    InternetFolderOpen = &H4A
    UnknownDrive = &H4B
    Fax = &H4C
    Fonts = &H4D
    ShieldUAC = &H4E
    UserAccount = &H4F
    StartMenu = &H50
    Information = &H51
    Key = &H52
    PNG = &H53
    Warning = &H54
    CDMusic = &H55
    Accessibility = &H56
    WindowsUpdate = &H57
    UserAccount2 = &H58
    Delete = &H59
    RichText = &H5A
    WhiteFloppyDrive = &H5B
    SilverScreen = &H5C
    PDA = &H5D
    TextSelection = &H5E
    Scanner = &H5F
    ExternalChip = &H60
    Disabled = &H61
    [Error] = &H62
    Question = &H63
    Run = &H64
    Sleep = &H65
    BlankTextFile = &H66
    Projector = &H67
    ShieldQuestion = &H68
    ShieldError = &H69
    ShieldOK = &H6A
    ShieldWarning = &H6B
    MusicFolder = &H6C
    Computer = &H6D
    Desktop = &H6E
    Defrag = &H6F
    DocumentsFolder = &H70
    PicturesFolder = &H71
    Options = &H72
    FavoritesFolder = &H73
    TaskDialog = &H74
    Recent = &H75
    InternetNetwork = &H78
    UserFolder = &H7B
    FontBigA = &H7C
    FontTTCert = &H7D
    FontTCCert = &H7E
    FontOCert = &H7F
    FontLittleA = &H80
    Fonts2 = &H81
    GuestUser = &H82
    MusicFile = &H83
    PictureFile = &H84
    VideoFile = &H85
    MediaFile = &H86
    DVDMusic = &H87
    DVDVideo = &H88
    CDMusic2 = &H89
    DVDVideoClip = &H8A
    HDDVDVideoClip = &H8B
    BluRayVideoClip = &H8C
    VCDVideoClip = &H8D
    SVCDVideoClip = &H8E
    NetworkFolder = &H8F
    InternetTime = &H90
    SearchComputer = &H91
    CDUnknown = &H92
    ComputerTransfer = &H93
    LibraryStack = &H94
    SystemProperties = &H95
    ResourceMonitor = &H96
    Personalize = &H97
    Network = &H98
    CDBurn = &H9B
    [Default] = &H9D
    FontTT = &H9E
    FontTC = &H9F
    FontO = &HA0
    WindowsUpdate2 = &HA1
    FolderFull = &HA2
    Shortcut = &HA3
    [Shared] = &HA4
    Preferences = &HA5
    FolderPreferences = &HA6
    ZipDriveDisconnected = &HA7
    WindowsPhotoFile = &HA8
    Download = &HA9
    Bad = &HAA
    MoveToNetwork = &HAB
    DVDPlusR = &HAC
    DVDPlusRW = &HAD
    ZipFile = &HAE
    CompressedFile = &HAF
    Sound = &HB0
    Search = &HB1
    UserAccountsFolder = &HB2
    InternetRJ45 = &HB3
    CDMusicPlus = &HB4
    ContactsFolder = &HB5
    Keyboard = &HB6
    DesktopFolder = &HB7
    DownloadsFolder = &HB8
    LinksFolder = &HB9
    GamesFolder = &HBA
    VideoFolder = &HBD
    GreenFolder = &HBE
    EmptyBox = &HBF
    BorderedBox = &HC0
    VideoStandard = &HC1
    VideoWide = &HC2
    ShieldUpdateTime = &HC3
    PrinterSound = &HC4
    PersonalizeComputer = &HC5
    Library = &H3E9
    LibraryDocuments = &H3EA
    LibraryPictures = &H3EB
    LibraryMusic = &H3EC
    LibraryVideos = &H3ED
    TV = &H3F0
    Users = &H3F2
    Homegroup = &H3F5
    Library2 = &H3F6
    FavoritesFolder2 = &H3FC
    ComputerDialog = &H3FD
    ComputerTasks = &H3FE
    Libraries = &H3FF
    Favorites = &H400
    SearchesFolder = &H401
    Album = &H402
    No = &H403
    User = &H405
    DriveUnlocked = &H406
    DriveLocked = &H407
    DriveUnlockedError = &H408
    DriveUnlockedWindows = &H409
    DriveUnlcokedErrorWindows = &H40A
    DriveUnlockedRemovable = &H40B
    DriveLockedRemovable = &H40C
    DriveUnlockedErrorRemovable = &H40D
    TaskDialog2 = &HFF9C
    ShieldUAC2 = &HFFF7
    ShieldOK2 = &HFFF8
    ShieldError2 = &HFFF9
    ShieldWarning2 = &HFFFA
    ShieldUAC3 = &HFFFB
    ShieldUAC4 = &HFFFC
    Information2 = &HFFFD
    Error2 = &HFFFE
    Warning2 = &HFFFF
  End Enum
  Public Function MsgDlg(owner As Form, Text As String, Optional Title As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As TaskDialogIcon = TaskDialogIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing) As DialogResult
    If TaskDialog.IsPlatformSupported Then
      Using dlgMessage As New TaskDialog
        dlgMessage.Cancelable = True
        dlgMessage.Caption = "SLIPS7REAM - " & Caption
        dlgMessage.InstructionText = Title
        dlgMessage.Text = Text
        dlgMessage.Icon = Icon
        Select Case Buttons
          Case MessageBoxButtons.OK
            Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdOK.Default = True
            Else
              cmdOK.Default = False
            End If
            AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdOK)
          Case MessageBoxButtons.YesNo
            Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
            Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdYes.Default = True
              cmdNo.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdYes.Default = False
              cmdNo.Default = True
            Else
              cmdYes.Default = False
              cmdNo.Default = False
            End If
            AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdYes)
            dlgMessage.Controls.Add(cmdNo)
          Case MessageBoxButtons.YesNoCancel
            Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
            Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdYes.Default = True
              cmdNo.Default = False
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdYes.Default = False
              cmdNo.Default = True
              cmdCancel.Default = False
            Else
              cmdYes.Default = False
              cmdNo.Default = False
              cmdCancel.Default = True
            End If
            AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdYes)
            dlgMessage.Controls.Add(cmdNo)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.OKCancel
            Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdOK.Default = True
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdOK.Default = False
              cmdCancel.Default = True
            Else
              cmdOK.Default = False
              cmdCancel.Default = False
            End If
            AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdOK)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.RetryCancel
            Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdRetry.Default = True
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdRetry.Default = False
              cmdCancel.Default = True
            Else
              cmdRetry.Default = False
              cmdCancel.Default = False
            End If
            AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdRetry)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.AbortRetryIgnore
            Dim cmdAbort As New TaskDialogButton("cmdNo", "&Abort")
            Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
            Dim cmdIgnore As New TaskDialogButton("cmdClose", "&Ignore")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdAbort.Default = True
              cmdRetry.Default = False
              cmdIgnore.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdAbort.Default = False
              cmdRetry.Default = True
              cmdIgnore.Default = False
            Else
              cmdAbort.Default = False
              cmdRetry.Default = False
              cmdIgnore.Default = True
            End If
            AddHandler cmdAbort.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdIgnore.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdAbort)
            dlgMessage.Controls.Add(cmdRetry)
            dlgMessage.Controls.Add(cmdIgnore)
        End Select
        If Not String.IsNullOrEmpty(Details) Then
          dlgMessage.DetailsExpanded = False
          dlgMessage.DetailsCollapsedLabel = "View Details"
          dlgMessage.DetailsExpandedLabel = "Hide Details"
          dlgMessage.DetailsExpandedText = Details
          dlgMessage.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandContent
        End If
        If owner IsNot Nothing Then dlgMessage.OwnerWindowHandle = owner.Handle
        AddHandler dlgMessage.Opened, AddressOf RefreshDlg
        Dim ret As TaskDialogResult
        Try
          ret = dlgMessage.Show()
        Catch ex As Exception
          Return MsgDlgLegacy(owner, Text, Title, Caption, Buttons, Icon, DefaultButton, Details)
        End Try
        Select Case ret
          Case TaskDialogResult.Yes : Return DialogResult.Yes
          Case TaskDialogResult.No : Return IIf(Buttons = MessageBoxButtons.AbortRetryIgnore, DialogResult.Abort, DialogResult.No)
          Case TaskDialogResult.Ok : Return DialogResult.OK
          Case TaskDialogResult.Cancel : Return DialogResult.Cancel
          Case TaskDialogResult.Close : Return DialogResult.Ignore
          Case TaskDialogResult.Retry : Return DialogResult.Retry
        End Select
        Return DialogResult.None
      End Using
    Else
      Return MsgDlgLegacy(owner, Text, Title, Caption, Buttons, Icon, DefaultButton, Details)
    End If
  End Function
  Private Sub RefreshDlg(sender As Object, e As EventArgs)
    Dim dlg As TaskDialog = sender
    dlg.Icon = dlg.Icon
    dlg.InstructionText = dlg.InstructionText
  End Sub
  Private Function MsgDlgLegacy(owner As Form, Text As String, Optional Title As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As TaskDialogIcon = TaskDialogIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing) As DialogResult
    Dim Content As String
    If String.IsNullOrEmpty(Title) And String.IsNullOrEmpty(Text) Then
      Content = String.Empty
    ElseIf String.IsNullOrEmpty(Title) Then
      Content = Text
    ElseIf String.IsNullOrEmpty(Text) Then
      Content = Title
    Else
      Content = Title & vbNewLine & vbNewLine & Text
    End If
    Dim msgIcon As MessageBoxIcon = MessageBoxIcon.None
    Select Case Icon
      Case TaskDialogIcon.None : msgIcon = MessageBoxIcon.None
      Case TaskDialogIcon.Error, TaskDialogIcon.Error2, TaskDialogIcon.ShieldError, TaskDialogIcon.ShieldError2 : msgIcon = MessageBoxIcon.Error
      Case TaskDialogIcon.Question, TaskDialogIcon.ShieldQuestion : msgIcon = MessageBoxIcon.Question
      Case TaskDialogIcon.Warning, TaskDialogIcon.Warning2, TaskDialogIcon.ShieldWarning, TaskDialogIcon.ShieldWarning2 : msgIcon = MessageBoxIcon.Warning
      Case TaskDialogIcon.Information, TaskDialogIcon.Information2 : msgIcon = MessageBoxIcon.Information
      Case Else
        Select Case Buttons
          Case MessageBoxButtons.YesNo, MessageBoxButtons.YesNoCancel : msgIcon = MessageBoxIcon.Question
          Case MessageBoxButtons.OKCancel : msgIcon = MessageBoxIcon.Information
          Case MessageBoxButtons.RetryCancel : msgIcon = MessageBoxIcon.Error
          Case MessageBoxButtons.AbortRetryIgnore : msgIcon = MessageBoxIcon.Warning
        End Select
    End Select
    Return MessageBox.Show(owner, Content, Caption, Buttons, msgIcon, DefaultButton)
  End Function
#End Region
End Module
