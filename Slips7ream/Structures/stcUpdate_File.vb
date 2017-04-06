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
  Public ReleaseType As String
  Public AllowedOffline As Boolean
  Private Shared Extract_ReturnList As New List(Of String)
  Public Sub New(Location As String)
    Path = Location
    Failure = Nothing
    AllowedOffline = True
    If IO.File.Exists(Location) Then
      Select Case GetUpdateType(Location)
        Case UpdateType.MSU
          Dim MSUPath As String = IO.Path.Combine(WorkDir, "UpdateMSU_Extract")
          If IO.Directory.Exists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(MSUPath)
          Try
            Dim exRet As String = Extract_File(Location, MSUPath, "pkgProperties.txt")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update Properties file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("Update MSU {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("Update Properties file {0}", exRet)
              Else
                Failure = String.Format("Update Properties file Error: {0}", exRet)
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
            exRet = Extract_File(Location, MSUPath, XMLFile)
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update XML file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("Update MSU {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("Update XML file {0}", exRet)
              Else
                Failure = String.Format("Update XML file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSUPath, XMLFile)) Then
              Failure = "Update XML file could not be extracted."
              Return
            End If
            Dim xMSU As XElement = XElement.Load(IO.Path.Combine(MSUPath, XMLFile))
            Dim xService As XElement = xMSU.Element("{urn:schemas-microsoft-com:unattend}servicing")
            Dim xPackage As XElement = xService.Element("{urn:schemas-microsoft-com:unattend}package")
            Dim xAssemblyIdentity As XElement = xPackage.Element("{urn:schemas-microsoft-com:unattend}assemblyIdentity")
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, xAssemblyIdentity.Attribute("processorArchitecture").Value, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            If Ident.Name = "Microsoft-Windows-InternetExplorer-LanguagePack" Then
              Dim ieVer As String = Ident.Version.Substring(0, Ident.Version.IndexOf("."))
              AppliesTo = String.Format("Internet Explorer {0}", ieVer)
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
            Dim CABFile As String = IO.Path.GetFileName(sFile).Replace("-pkgProperties.txt", ".cab")
            exRet = Extract_File(Location, MSUPath, CABFile)
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update CAB file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("Update MSU {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("Update CAB file {0}", exRet)
              Else
                Failure = String.Format("Update CAB file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSUPath, CABFile)) Then
              Failure = "Update CAB file could not be extracted."
              Return
            End If
            Dim MUMFile As String = "update.mum"
            exRet = Extract_File(IO.Path.Combine(MSUPath, CABFile), MSUPath, MUMFile)
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update MUM file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("Update CAB {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("Update MUM file {0}", exRet)
              Else
                Failure = String.Format("Update MUM file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSUPath, MUMFile)) Then
              Failure = "Update MUM file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(IO.Path.Combine(MSUPath, MUMFile))
            Dim xMUMPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            If xMUMPackage IsNot Nothing Then
              Dim xReleaseType As XAttribute = xMUMPackage.Attribute("releaseType")
              If xReleaseType IsNot Nothing Then
                ReleaseType = xReleaseType.Value.Trim
                Name = GetUpdateName(Ident, ReleaseType)
              Else
                Name = GetUpdateName(Ident, "Update")
              End If
              Dim xMUMPackageEx As XElement = xMUMPackage.Element("{urn:schemas-microsoft-com:asm.v3}packageExtended")
              If xMUMPackageEx IsNot Nothing Then
                Dim xMUMallowedOffline As XAttribute = xMUMPackageEx.Attribute("allowedOffline")
                If xMUMallowedOffline IsNot Nothing AndAlso xMUMallowedOffline.Value.Trim.ToLower = "false" Then AllowedOffline = False
              End If
            End If
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MSUPath) Then SlowDeleteDirectory(MSUPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          End Try
        Case UpdateType.CAB
          Dim CABPath As String = IO.Path.Combine(WorkDir, "UpdateCAB_Extract")
          If IO.Directory.Exists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(CABPath)
          Try
            Dim exRet As String = Extract_File(Location, CABPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "Update Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("Update CAB {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("Update Description file {0}", exRet)
              Else
                Failure = String.Format("Update Description file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(CABPath, "update.mum")) Then
              Failure = "Update Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(IO.Path.Combine(CABPath, "update.mum"))
            DisplayName = xMUM.Attribute("displayName").Value
            SupportLink = xMUM.Attribute("supportInformation").Value
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
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
            ReleaseType = xPackage.Attribute("releaseType").Value
            Name = GetUpdateName(Ident, ReleaseType)
            If KBArticle.StartsWith("KB") Then KBArticle = KBArticle.Substring(2)
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xParentAssemblyIdentity As XElement = xParent.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            AppliesTo = xParentAssemblyIdentity.Attribute("name").Value
            Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(CABPath, "update.mum")).CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(CABPath) Then SlowDeleteDirectory(CABPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          End Try
        Case UpdateType.LP
          Dim LPPath As String = IO.Path.Combine(WorkDir, "UpdateLP_Extract")
          If IO.Directory.Exists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(LPPath)
          Try
            Dim exRet As String = Extract_File(Location, LPPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("LP CAB {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("LP Description file {0}", exRet)
              Else
                Failure = String.Format("LP Description file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(LPPath, "update.mum")) Then
              Failure = "Update Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(IO.Path.Combine(LPPath, "update.mum"))
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = Nothing '"http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            ReleaseType = xPackage.Attribute("releaseType").Value
            Name = GetUpdateName(Ident, ReleaseType)
            KBArticle = Nothing
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
              KBArticle = "248313"
              SupportLink = String.Format("http://support.microsoft.com?kbid={0}", KBArticle)
            ElseIf xInfo.Attribute("LPTargetSPLevel").Value = "0" Then
              AppliesTo = "Windows 7"
              KBArticle = "972813"
              SupportLink = String.Format("http://support.microsoft.com?kbid={0}", KBArticle)
            Else
              AppliesTo = String.Format("Windows 7 SP{0}", xInfo.Attribute("LPTargetSPLevel").Value)
              KBArticle = Nothing
              SupportLink = Nothing
            End If
            Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(LPPath, "update.mum")).CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(LPPath) Then SlowDeleteDirectory(LPPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          End Try
        Case UpdateType.LIP
          Dim MLCPath As String = IO.Path.Combine(WorkDir, "UpdateMLC_Extract")
          If IO.Directory.Exists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(MLCPath)
          Try
            Dim exRet As String = Extract_File(Location, MLCPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("LIP MLC {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("LIP Description file {0}", exRet)
              Else
                Failure = String.Format("LIP Description file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MLCPath, "update.mum")) Then
              Failure = "LIP Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(IO.Path.Combine(MLCPath, "update.mum"))
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = "http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            ReleaseType = xPackage.Attribute("releaseType").Value
            Name = GetUpdateName(Ident, ReleaseType)
            KBArticle = Nothing
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
            Else
              AppliesTo = "Windows 7"
            End If
            Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(MLCPath, "update.mum")).CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MLCPath) Then SlowDeleteDirectory(MLCPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          End Try
        Case UpdateType.MSI
          Dim MSIPath As String = IO.Path.Combine(WorkDir, "UpdateMSI_Extract")
          If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(MSIPath)
          Try
            Dim exRet As String = Extract_File(Location, MSIPath, "LIP.cab")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP MSI CAB file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("LIP MSI {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("LIP MSI CAB file {0}", exRet)
              Else
                Failure = String.Format("LIP MSI CAB file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSIPath, "LIP.cab")) Then
              Failure = "LIP MSI CAB file could not be extracted."
              Return
            End If
            exRet = Extract_File(IO.Path.Combine(MSIPath, "LIP.cab"), MSIPath, "LIP.mlc")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP MLC file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("LIP CAB {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("LIP MLC file {0}", exRet)
              Else
                Failure = String.Format("LIP MLC file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSIPath, "LIP.mlc")) Then
              Failure = "LIP MLC file could not be extracted."
              Return
            End If
            exRet = Extract_File(IO.Path.Combine(MSIPath, "LIP.mlc"), MSIPath, "update.mum")
            If Not exRet = "OK" Then
              If exRet = "File Not Found" Then
                Failure = "LIP Description file not found."
              ElseIf exRet.StartsWith("Error Opening: ") Then
                Failure = String.Format("LIP MLC {0}", exRet)
              ElseIf exRet.StartsWith("Error Extracting: ") Then
                Failure = String.Format("LIP Description file {0}", exRet)
              Else
                Failure = String.Format("LIP Description file Error: {0}", exRet)
              End If
              Return
            End If
            If Not IO.File.Exists(IO.Path.Combine(MSIPath, "update.mum")) Then
              Failure = "LIP Description file could not be extracted."
              Return
            End If
            Dim xMUM As XElement = XElement.Load(IO.Path.Combine(MSIPath, "update.mum"))
            Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
            SupportLink = "http://windows.microsoft.com/en-us/windows/language-packs#lptabs=win7"
            Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
            Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
            Ident = New Update_Identity(Identity)
            If Not String.IsNullOrEmpty(Ident.Version) Then
              KBVersion = Ident.Version
            Else
              KBVersion = "0"
            End If
            Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
            DisplayName = xPackage.Attribute("identifier").Value
            ReleaseType = xPackage.Attribute("releaseType").Value
            Name = GetUpdateName(Ident, ReleaseType)
            KBArticle = Nothing
            Dim xParent As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}parent")
            Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
            If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
              AppliesTo = "Windows 7 SP1"
            Else
              AppliesTo = "Windows 7"
            End If
            Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(MSIPath, "update.mum")).CreationTime
            BuildDate = wDate.ToString("yyyy/MM/dd")
          Catch ex As Exception
            Failure = ex.Message
          Finally
            If IO.Directory.Exists(MSIPath) Then SlowDeleteDirectory(MSIPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          End Try
        Case UpdateType.EXE
          Dim EXEPath As String = IO.Path.Combine(WorkDir, "UpdateEXE_Extract")
          If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
          IO.Directory.CreateDirectory(EXEPath)
          Dim fInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Location)
          If fInfo.OriginalFilename = "mergedwusetup.exe" Then
            Dim exRet As String = Extract_File(Location, EXEPath, "WUA-Win7SP1.exe")
            If exRet = "OK" Then
              exRet = Extract_File(IO.Path.Combine(EXEPath, "WUA-Win7SP1.exe"), EXEPath, "WUClient-SelfUpdate-Core-TopLevel.cab")
              If exRet = "OK" Then
                exRet = Extract_File(IO.Path.Combine(EXEPath, "WUClient-SelfUpdate-Core-TopLevel.cab"), EXEPath, "update.mum")
                If exRet = "OK" Then
                  If IO.File.Exists(IO.Path.Combine(EXEPath, "update.mum")) Then
                    Dim xMUM As XElement = XElement.Load(IO.Path.Combine(EXEPath, "update.mum"))
                    DisplayName = xMUM.Attribute("displayName").Value
                    SupportLink = "http://support.microsoft.com?kbid=949104"
                    Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
                    Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
                    Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
                    Ident = New Update_Identity(Identity)
                    If Not String.IsNullOrEmpty(Ident.Version) Then
                      KBVersion = Ident.Version
                    Else
                      KBVersion = "0"
                    End If
                    KBArticle = "949104"
                    AppliesTo = "Windows 7"
                    Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(EXEPath, "update.mum")).CreationTime
                    BuildDate = wDate.ToString("yyyy/MM/dd")
                    Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
                    ReleaseType = xPackage.Attribute("releaseType").Value
                    Name = GetUpdateName(Ident, ReleaseType)
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
            Dim exRet As String = Extract_File(Location, EXEPath, "update.mum")
            If exRet = "OK" Then
              If IO.File.Exists(IO.Path.Combine(EXEPath, "update.mum")) Then
                Dim xMUM As XElement = XElement.Load(IO.Path.Combine(EXEPath, "update.mum"))
                Dim xAssemblyIdentity As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}assemblyIdentity")
                Architecture = xAssemblyIdentity.Attribute("processorArchitecture").Value
                Identity = MakeIdentity(xAssemblyIdentity.Attribute("name").Value, xAssemblyIdentity.Attribute("publicKeyToken").Value, Architecture, xAssemblyIdentity.Attribute("language").Value, xAssemblyIdentity.Attribute("version").Value)
                Ident = New Update_Identity(Identity)
                If Not String.IsNullOrEmpty(Ident.Version) Then
                  KBVersion = Ident.Version
                Else
                  KBVersion = "0"
                End If
                Dim xPackage As XElement = xMUM.Element("{urn:schemas-microsoft-com:asm.v3}package")
                DisplayName = xPackage.Attribute("identifier").Value
                ReleaseType = xPackage.Attribute("releaseType").Value
                Name = GetUpdateName(Ident, ReleaseType)
                KBArticle = Nothing
                Dim xInfo As XElement = xPackage.Element("{urn:schemas-microsoft-com:asm.v3}customInformation")
                If xInfo.Attribute("LPTargetSPLevel").Value = "1" Then
                  AppliesTo = "Windows 7 SP1"
                  KBArticle = "248313"
                  SupportLink = String.Format("http://support.microsoft.com?kbid={0}", KBArticle)
                ElseIf xInfo.Attribute("LPTargetSPLevel").Value = "0" Then
                  AppliesTo = "Windows 7"
                  KBArticle = "972813"
                  SupportLink = String.Format("http://support.microsoft.com?kbid={0}", KBArticle)
                Else
                  AppliesTo = String.Format("Windows 7 SP{0}", xInfo.Attribute("LPTargetSPLevel").Value)
                  KBArticle = Nothing
                  SupportLink = Nothing
                End If
                Dim wDate As Date = New IO.FileInfo(IO.Path.Combine(EXEPath, "update.mum")).CreationTime
                BuildDate = wDate.ToString("yyyy/MM/dd")
              Else
                Failure = "Description file not found"
              End If
            Else
              Failure = String.Format("Error extracting Description file: {0}", exRet)
            End If
          End If
          If IO.Directory.Exists(EXEPath) Then SlowDeleteDirectory(EXEPath, FileIO.DeleteDirectoryOption.DeleteAllContents, False)
        Case UpdateType.INF
          DriverData = frmMain.DISM_DriverINF_GetData(Path)
          Name = "DRIVER"
          DisplayName = Nothing
          AppliesTo = Nothing
          SupportLink = Nothing
          Architecture = Nothing
          KBArticle = Nothing
          KBVersion = Nothing
          BuildDate = Nothing
          Failure = Nothing
          ReleaseType = Nothing
        Case UpdateType.Other
          Name = Nothing
          DisplayName = Nothing
          AppliesTo = Nothing
          SupportLink = Nothing
          Architecture = Nothing
          KBArticle = Nothing
          KBVersion = Nothing
          BuildDate = Nothing
          ReleaseType = Nothing
          If String.IsNullOrEmpty(IO.Path.GetExtension(IO.Path.GetExtension(Path))) Then
            Failure = "No File Type"
          Else
            Dim sExt As String = IO.Path.GetExtension(Path).Substring(1).ToUpper
            Select Case sExt
              Case "CAT" : Failure = "Security Catalogs are included when needed automatically and do not need to be added."
              Case Else : Failure = String.Format("Unknown file type: {0}", sExt)
            End Select
          End If
      End Select
    End If
  End Sub
  Private Function Extract_File(Source As String, Destination As String, File As String) As String
    Dim tRunWithReturn As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf Extract_File_Task))
    Dim cIndex As Integer = Extract_ReturnList.Count
    Extract_ReturnList.Add(Nothing)
    tRunWithReturn.Start(CType({Source, Destination, File, cIndex}, Object()))
    Do While String.IsNullOrEmpty(Extract_ReturnList(cIndex))
      Application.DoEvents()
      Threading.Thread.Sleep(1)
    Loop
    Dim sRet As String = Extract_ReturnList(cIndex)
    Extract_ReturnList(cIndex) = Nothing
    Return sRet
  End Function
  Private Sub Extract_File_Task(Obj As Object)
    Dim Source, Destination, Find As String
    Source = CStr(CType(Obj, Object())(0))
    Destination = CStr(CType(Obj, Object())(1))
    Find = CStr(CType(Obj, Object())(2))
    Dim cIndex As Integer = CInt(CType(Obj, Object())(3))
    Dim bFound As Boolean = False
    Using Extract_Archive As New Extraction.ArchiveFile(New IO.FileInfo(Source), GetUpdateCompression(Source))
      Try
        Extract_Archive.Open()
      Catch ex As Exception
        Extract_ReturnList(cIndex) = String.Format("Error Opening: {0}", ex.Message)
        Return
      End Try
      Dim eFiles() As Extraction.COM.IArchiveEntry = Extract_Archive.ToArray
      For Each file As Extraction.COM.IArchiveEntry In eFiles
        If file.Name.ToLower.EndsWith(Find.ToLower) Then
          file.Destination = New IO.FileInfo(IO.Path.Combine(Destination, file.Name))
          bFound = True
          Exit For
        End If
      Next
      If bFound Then
        Try
          Extract_Archive.Extract()
        Catch ex As Exception
          Extract_ReturnList(cIndex) = String.Format("Error Extracting: {0}", ex.Message)
          Return
        End Try
        Extract_ReturnList(cIndex) = "OK"
      Else
        Extract_ReturnList(cIndex) = "File Not Found"
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
    Return String.Format("{0}~{1}~{2}~{3}~{4}", Name, Token, Architecture, Language, Version)
  End Function
  Friend Shared ReadOnly Property WorkDir As String
    Get
      Dim mySettings As New MySettings
      Dim tempDir As String = mySettings.TempDir
      If String.IsNullOrEmpty(tempDir) Then tempDir = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "Slips7ream")
      If Not IO.Directory.Exists(tempDir) Then IO.Directory.CreateDirectory(tempDir)
      Return tempDir
    End Get
  End Property
End Structure
