Public Structure Update_File
  Public Path As String
  Public Identity As String
  Public Name As String
  Public DisplayName As String
  Public AppliesTo As String
  Public Architecture As String
  Public BuildDate As String
  Public KBArticle As String
  Public KBVersion As String
  Public SupportLink As String
  Public Failure As String
  Public Ident As Update_Identity
  Public DriverData As Driver
  Private Shared c_ExtractRet As New Collections.Generic.List(Of String)
  Public Sub New(Location As String)
    Path = Location
    Failure = Nothing
    If IO.File.Exists(Location) Then
      Select Case GetUpdateType(Location)
        Case UpdateType.MSU
          Dim MSUPath As String = WorkDir & "UpdateMSU_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(MSUPath)
          Try
            Dim exRet As String = ExtractAFile(Location, MSUPath, "pkgProperties.txt")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update Properties file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "Update MSU " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "Update Properties file " & exRet
              Else
                Failure = "Update Properties file Error: " & exRet
              End If
              Return
            End If
            Dim filePath() As String = My.Computer.FileSystem.GetFiles(MSUPath, FileIO.SearchOption.SearchTopLevelOnly).ToArray
            If filePath.Count = 0 Then
              Failure = "Update Properties file could not be extracted."
              Return
            End If
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
            Dim XMLFile As String = IO.Path.GetFileName(sFile).Replace("-pkgProperties.txt", ".xml")
            exRet = ExtractAFile(Location, MSUPath, XMLFile)
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update XML file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "Update MSU " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "Update XML file " & exRet
              Else
                Failure = "Update XML file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(MSUPath & XMLFile) Then
              Failure = "Update XML file could not be extracted."
              Return
            End If
            Dim xMSU As XElement = XElement.Load(MSUPath & XMLFile)
            Dim xService As XElement = xMSU.Element("{urn:schemas-microsoft-com:unattend}servicing")
            Dim xPackage As XElement = xService.Element("{urn:schemas-microsoft-com:unattend}package")
            Dim xAssemblyIdentity As XElement = xPackage.Element("{urn:schemas-microsoft-com:unattend}assemblyIdentity")
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, xAssemblyIdentity.Attribute("processorArchitecture").Value, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            Name = GetUpdateName(Ident)
            If Ident.Name = "Microsoft-Windows-InternetExplorer-LanguagePack" Then
              Dim ieVer As String = Ident.Version.Substring(0, Ident.Version.IndexOf("."))
              AppliesTo = "Internet Explorer " & ieVer
            End If
            If Not String.IsNullOrEmpty(Ident.Version) Then
              If Ident.Version.StartsWith("6.1.") Or Ident.Version.StartsWith("6.2.") Or Ident.Version.StartsWith("6.3.") Then
                KBVersion = Ident.Version.Substring(4)
              Else
                KBVersion = Ident.Version
              End If
            Else
              KBVersion = "0"
            End If
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End Try
        Case UpdateType.CAB
          Dim CABPath As String = WorkDir & "UpdateCAB_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(CABPath)
          Try
            Dim exRet As String = ExtractAFile(Location, CABPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "Update CAB " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "Update Description file " & exRet
              Else
                Failure = "Update Description file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(CABPath & "update.mum") Then
              Failure = "Update Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(CABPath & "update.mum")
            DisplayName = xMUM.Attribute("displayName").Value
            SupportLink = xMUM.Attribute("supportInformation").Value
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            Name = GetUpdateName(Ident)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              If Ident.Version.StartsWith("6.1.") Or Ident.Version.StartsWith("6.2.") Or Ident.Version.StartsWith("6.3.") Then
                KBVersion = Ident.Version.Substring(4)
              Else
                KBVersion = Ident.Version
              End If
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            KBArticle = xPackage.Attribute("identifier").Value
            If KBArticle.StartsWith("KB") Then KBArticle = KBArticle.Substring(2)
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            AppliesTo = xParentAssemblyIdentity.Attribute("name").Value
            Dim wDate As Date = New IO.FileInfo(CABPath & "update.mum").CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End Try
        Case UpdateType.LP
          Dim LPPath As String = WorkDir & "UpdateLP_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(LPPath)
          Try
            Dim exRet As String = ExtractAFile(Location, LPPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "LP CAB " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "LP Description file " & exRet
              Else
                Failure = "LP Description file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(LPPath & "update.mum") Then
              Failure = "Update Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(LPPath & "update.mum")
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = Nothing '"http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            Name = GetUpdateName(Ident)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            KBArticle = Nothing
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
              KBArticle = "248313"
              SupportLink = "http://support.microsoft.com?kbid=" & KBArticle
            ElseIf xInfo.Attribute("LPTargetSPLevel").Value = "0" Then
              AppliesTo = "Windows 7"
              KBArticle = "972813"
              SupportLink = "http://support.microsoft.com?kbid=" & KBArticle
            Else
              AppliesTo = "Windows 7 SP" & xInfo.Attribute("LPTargetSPLevel").Value
              KBArticle = Nothing
              SupportLink = Nothing
            End If
            Dim wDate As Date = New IO.FileInfo(LPPath & "update.mum").CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")

          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End Try
        Case UpdateType.LIP
          Dim MLCPath As String = WorkDir & "UpdateMLC_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(MLCPath)
          Try
            Dim exRet As String = ExtractAFile(Location, MLCPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "LIP MLC " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "LIP Description file " & exRet
              Else
                Failure = "LIP Description file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(MLCPath & "update.mum") Then
              Failure = "LIP Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(MLCPath & "update.mum")
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = "http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            Name = GetUpdateName(Ident)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            KBArticle = Nothing
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
            Else
              AppliesTo = "Windows 7"
            End If
            Dim wDate As Date = New IO.FileInfo(MLCPath & "update.mum").CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End Try
        Case UpdateType.MSI
          Dim MSIPath As String = WorkDir & "UpdateMSI_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(MSIPath)
          Try
            Dim exRet As String = ExtractAFile(Location, MSIPath, "LIP.cab")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP MSI CAB file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "LIP MSI " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "LIP MSI CAB file " & exRet
              Else
                Failure = "LIP MSI CAB file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(MSIPath & "LIP.cab") Then
              Failure = "LIP MSI CAB file could not be extracted."
              Return
            End If
            exRet = ExtractAFile(MSIPath & "LIP.cab", MSIPath, "LIP.mlc")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP MLC file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "LIP CAB " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "LIP MLC file " & exRet
              Else
                Failure = "LIP MLC file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(MSIPath & "LIP.mlc") Then
              Failure = "LIP MLC file could not be extracted."
              Return
            End If
            exRet = ExtractAFile(MSIPath & "LIP.mlc", MSIPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = "LIP MLC " & exRet
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = "LIP Description file " & exRet
              Else
                Failure = "LIP Description file Error: " & exRet
              End If
              Return
            End If
            If Not IO.File.Exists(MSIPath & "update.mum") Then
              Failure = "LIP Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(MSIPath & "update.mum")
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = "http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            Name = GetUpdateName(Ident)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            KBArticle = Nothing
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
            Else
              AppliesTo = "Windows 7"
            End If
            Dim wDate As Date = New IO.FileInfo(MSIPath & "update.mum").CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End Try
        Case UpdateType.EXE
          Dim EXEPath As String = WorkDir & "UpdateEXE_Extract" & IO.Path.DirectorySeparatorChar
          If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
          IO.Directory.CreateDirectory(EXEPath)
          Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Location)
          If fInfo.OriginalFilename = "mergedwusetup.exe" Then
            Dim exRet As String = ExtractAFile(Location, EXEPath, "WUA-Win7SP1.exe")
            If exRet = "OK" Then
              exRet = ExtractAFile(EXEPath & "WUA-Win7SP1.exe", EXEPath, "WUClient-SelfUpdate-Core-TopLevel.cab")
              If exRet = "OK" Then
                exRet = ExtractAFile(EXEPath & "WUClient-SelfUpdate-Core-TopLevel.cab", EXEPath, "update.mum")
                If exRet = "OK" Then
                  If IO.File.Exists(EXEPath & "update.mum") Then
                    Dim xMUM As XElement = XElement.Load(EXEPath & "update.mum")
                    DisplayName = xMUM.Attribute("displayName").Value
                    SupportLink = "http://support.microsoft.com?kbid=949104"
                    Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
                    Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
                    Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
                    Ident = New Update_Identity(Identity)
                    Name = GetUpdateName(Ident)
                    If Not String.IsNullOrEmpty(Ident.Version) Then
                      KBVersion = Ident.Version
                    Else
                      KBVersion = "0"
                    End If
                    KBArticle = "949104"
                    AppliesTo = "Windows 7"
                    Dim wDate As Date = New IO.FileInfo(EXEPath & "update.mum").CreationTime
                    BuildDate = wDate.ToString("yyyy/MM/dd")
                  Else
                    Failure = "Description File Not Found"
                  End If
                Else
                  Failure = exRet
                End If
              Else
                Failure = exRet
              End If
            Else
              Failure = exRet
            End If
          Else
            Dim exRet As String = ExtractAFile(Location, EXEPath, "update.mum")
            If exRet = "OK" Then
              If IO.File.Exists(EXEPath & "update.mum") Then
                Dim xMUM As XElement = XElement.Load(EXEPath & "update.mum")
                Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
                Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
                Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
                Ident = New Update_Identity(Identity)
                Name = GetUpdateName(Ident)
                If Not String.IsNullOrEmpty(Ident.Version) Then
                  KBVersion = Ident.Version
                Else
                  KBVersion = "0"
                End If
                Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
                DisplayName = xPackage.Attribute("identifier").Value
                KBArticle = Nothing
                Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
                If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
                  AppliesTo = "Windows 7 SP1"
                  KBArticle = "248313"
                  SupportLink = "http://support.microsoft.com?kbid=" & KBArticle
                ElseIf xInfo.Attribute("LPTargetSPLevel").Value = "0" Then
                  AppliesTo = "Windows 7"
                  KBArticle = "972813"
                  SupportLink = "http://support.microsoft.com?kbid=" & KBArticle
                Else
                  AppliesTo = "Windows 7 SP" & xInfo.Attribute("LPTargetSPLevel").Value
                  KBArticle = Nothing
                  SupportLink = Nothing
                End If
                Dim wDate As Date = New IO.FileInfo(EXEPath & "update.mum").CreationTime
                BuildDate = wDate.ToString("yyyy/MM/dd")
              Else
                Failure = "Description file not found"
              End If
            Else
              Failure = "Error extracting Description file: " & exRet
            End If
          End If
          If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Case UpdateType.INF
          DriverData = frmMain.GetDISMDriverItemData(Path)
          Name = "DRIVER"
          DisplayName = Nothing
          AppliesTo = Nothing
          SupportLink = Nothing
          Architecture = Nothing
          KBArticle = Nothing
          KBVersion = Nothing
          BuildDate = Nothing
          Failure = Nothing
        Case UpdateType.Other
          Name = Nothing
          DisplayName = Nothing
          AppliesTo = Nothing
          SupportLink = Nothing
          Architecture = Nothing
          KBArticle = Nothing
          KBVersion = Nothing
          BuildDate = Nothing
          Failure = "Unknown file type"
      End Select
    End If
  End Sub
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
    Using Extractor As New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
      Try
        Extractor.Open()
      Catch ex As Exception
        c_ExtractRet(cIndex) = "Error Opening: " & ex.Message
        Return
      End Try
      Dim eFiles() As Extraction.COM.IArchiveEntry = Extractor.ToArray
      For Each file As Extraction.COM.IArchiveEntry In eFiles
        If file.Name.ToLower.EndsWith(Find.ToLower) Then
          file.Destination = New IO.FileInfo(Destination & file.Name)
          bFound = True
          Exit For
        End If
      Next
      If bFound Then
        Try
          Extractor.Extract()
        Catch ex As Exception
          c_ExtractRet(cIndex) = "Error Extracting: " & ex.Message
          Return
        End Try
        c_ExtractRet(cIndex) = "OK"
      Else
        c_ExtractRet(cIndex) = "File Not Found"
      End If
    End Using
  End Sub
  Private Function GetMSUValue(Line As String) As String
    Dim sTmp As String = Line.Substring(Line.IndexOf("="c) + 1)
    Return sTmp.Substring(1, sTmp.Length - 2)
  End Function
  Private Function MakeIdentity(Name As String, Optional Token As String = "31bf3856ad364e35", Optional Architecture As String = "neutral", Optional Language As String = "neutral", Optional Version As String = Nothing) As String
    If Architecture = "neutral" Then Architecture = ""
    If Language = "neutral" Then Language = ""
    Return Name & "~" & Token & "~" & Architecture & "~" & Language & "~" & Version
  End Function
  Friend Shared ReadOnly Property WorkDir As String
    Get
      Dim mySettings As New MySettings
      Dim tempDir As String = mySettings.TempDir
      If String.IsNullOrEmpty(tempDir) Then tempDir = My.Computer.FileSystem.SpecialDirectories.Temp & IO.Path.DirectorySeparatorChar & "Slips7ream" & IO.Path.DirectorySeparatorChar
      If Not IO.Directory.Exists(tempDir) Then IO.Directory.CreateDirectory(tempDir)
      Return tempDir
    End Get
  End Property
End Structure
