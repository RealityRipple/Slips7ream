﻿Public Class frmPackageProps
  Public Sub New()
    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
  End Sub
  Private WIMIdenifier As String
  Private UpdateData As List(Of Update_Integrated)
  Private FeatureData As List(Of Feature)
  Private DriverData As List(Of Driver)
  Public Class PackagePropertiesEventArgs
    Inherits EventArgs
    Public ImageIndex As Integer
    Public OldImageName As String
    Public NewImageName As String
    Public FeatureList() As Feature
    Public UpdateList() As Update_Integrated
    Public DriverList() As Driver
    Public Sub New(Index As Integer, OldName As String, NewName As String, Features() As Feature, Updates() As Update_Integrated, Drivers() As Driver)
      ImageIndex = Index
      OldImageName = OldName
      NewImageName = NewName
      FeatureList = Features
      UpdateList = Updates
      DriverList = Drivers
    End Sub
  End Class
  Public Event Response(sender As Object, e As PackagePropertiesEventArgs)
  Private bLoading As Boolean = False
  Private originalPackageName As String
  Private selFeature As TreeNode
  Private selUpdate As ListViewItem

  Public Sub New(WIMID As String, Data As ImagePackage, Features As List(Of Feature), Drivers As List(Of Driver))
    bLoading = True
    InitializeComponent()
    LoadImageLists()
    WIMIdenifier = WIMID
    UpdateData = Data.IntegratedUpdateList
    If UpdateData IsNot Nothing AndAlso UpdateData.Count = 0 Then UpdateData = Nothing
    FeatureData = Features
    If FeatureData IsNot Nothing AndAlso FeatureData.Count = 0 Then FeatureData = Nothing
    DriverData = Drivers
    If DriverData IsNot Nothing AndAlso DriverData.Count = 0 Then DriverData = Nothing
    txtIndex.Text = Data.Index
    Me.Text = Data.Name & " Image Package Properties"
    txtName.Text = Data.Name
    originalPackageName = Data.Name
    txtDesc.Text = Data.Desc
    txtSize.Text = ByteSize(Data.Size) & " (" & FormatNumber(Data.Size, 0, TriState.False, TriState.False, TriState.True) & " bytes)"
    txtArchitecture.Text = Data.Architecture
    txtHAL.Text = Data.HAL
    txtVersion.Text = Data.Version
    txtSPLevel.Text = Data.SPLevel
    txtSPBuild.Text = Data.SPBuild
    txtEdition.Text = Data.Edition
    txtInstallation.Text = Data.Installation
    txtProductType.Text = Data.ProductType
    txtProductSuite.Text = Data.ProductSuite
    txtSystemRoot.Text = Data.SystemRoot
    txtFiles.Text = FormatNumber(Data.Files, 0, TriState.False, TriState.False, TriState.True)
    txtDirectories.Text = FormatNumber(Data.Directories, 0, TriState.False, TriState.False, TriState.True)
    txtCreated.Text = Data.Created
    txtModified.Text = Data.Modified
    txtLanguages.Text = Join(Data.LangList, vbNewLine)
    lvUpdates.Items.Clear()
    If UpdateData IsNot Nothing Then
      expUpdates.Open = True
      DisplayUpdates()
    Else
      expUpdates.Open = False
    End If
    tvFeatures.Nodes.Clear()
    If FeatureData IsNot Nothing Then
      expFeatures.Open = True
      DisplayFeatures()
    Else
      expFeatures.Open = False
    End If
    lvDriverClass.Items.Clear()
    lvDriverProvider.Items.Clear()
    lvDriverINF.Items.Clear()
    If DriverData IsNot Nothing Then
      expDrivers.Open = True
      DisplayDrivers()
    Else
      expDrivers.Open = False
    End If
    PositionViews()
    bLoading = False
  End Sub

  Private Sub LoadImageLists()
    imlFeatures.Images.Add("unknown", My.Resources.update_folder)
    imlFeatures.Images.Add(".net_environment", My.Resources.feature_dotnet_environment)
    imlFeatures.Images.Add(".net_extensibility", My.Resources.feature_dotnet_extensibility)
    imlFeatures.Images.Add("administrative_tools", My.Resources.feature_administrative_tools)
    imlFeatures.Images.Add("application_development_features", My.Resources.feature_application_development_features)
    imlFeatures.Images.Add("asp", My.Resources.feature_asp)
    imlFeatures.Images.Add("asp.net", My.Resources.feature_asp_net)
    imlFeatures.Images.Add("basic_authentication", My.Resources.feature_basic_authentication)
    imlFeatures.Images.Add("cgi", My.Resources.feature_cgi)
    imlFeatures.Images.Add("chess", My.Resources.feature_chess)
    imlFeatures.Images.Add("client_certificate_mapping_authentication", My.Resources.feature_client_certificate_mapping_authentication)
    imlFeatures.Images.Add("client_for_nfs", My.Resources.feature_client_for_nfs)
    imlFeatures.Images.Add("common_http_features", My.Resources.feature_common_http_features)
    imlFeatures.Images.Add("configuration_apis", My.Resources.feature_configuration_apis)
    imlFeatures.Images.Add("content_compression", My.Resources.feature_content_compression)
    imlFeatures.Images.Add("custom_logging", My.Resources.feature_custom_logging)
    imlFeatures.Images.Add("default_document", My.Resources.feature_default_document)
    imlFeatures.Images.Add("digest_authentication", My.Resources.feature_digest_authentication)
    imlFeatures.Images.Add("directory_browsing", My.Resources.feature_directory_browsing)
    imlFeatures.Images.Add("freecell", My.Resources.feature_freecell)
    imlFeatures.Images.Add("ftp_extensibility", My.Resources.feature_ftp_extensibility)
    imlFeatures.Images.Add("ftp_server", My.Resources.feature_ftp_server)
    imlFeatures.Images.Add("ftp_service", My.Resources.feature_ftp_service)
    imlFeatures.Images.Add("games", My.Resources.feature_games)
    imlFeatures.Images.Add("health_and_diagnostics", My.Resources.feature_health_and_diagnostics)
    imlFeatures.Images.Add("hearts", My.Resources.feature_hearts)
    imlFeatures.Images.Add("help", My.Resources.feature_help)
    imlFeatures.Images.Add("http_errors", My.Resources.feature_http_errors)
    imlFeatures.Images.Add("http_logging", My.Resources.feature_http_logging)
    imlFeatures.Images.Add("http_redirection", My.Resources.feature_http_redirection)
    imlFeatures.Images.Add("iis_6", My.Resources.feature_iis_6)
    imlFeatures.Images.Add("iis", My.Resources.feature_iis)
    imlFeatures.Images.Add("iis_client_certificate_mapping_authentication", My.Resources.feature_iis_client_certificate_mapping_authentication)
    imlFeatures.Images.Add("internet_backgammon", My.Resources.feature_internet_backgammon)
    imlFeatures.Images.Add("internet_checkers", My.Resources.feature_internet_checkers)
    imlFeatures.Images.Add("internet_explorer_8", My.Resources.feature_internet_explorer_8)
    imlFeatures.Images.Add("internet_explorer", My.Resources.feature_internet_explorer)
    imlFeatures.Images.Add("internet_games", My.Resources.feature_internet_games)
    imlFeatures.Images.Add("internet_information_services", My.Resources.feature_internet_information_services)
    imlFeatures.Images.Add("internet_printing_client", My.Resources.feature_internet_printing_client)
    imlFeatures.Images.Add("internet_spades", My.Resources.feature_internet_spades)
    imlFeatures.Images.Add("ip_security", My.Resources.feature_ip_security)
    imlFeatures.Images.Add("isapi_extensions", My.Resources.feature_isapi_extensions)
    imlFeatures.Images.Add("isapi_filters", My.Resources.feature_isapi_filters)
    imlFeatures.Images.Add("logging_tools", My.Resources.feature_logging_tools)
    imlFeatures.Images.Add("lpd_print_service", My.Resources.feature_lpd_print_service)
    imlFeatures.Images.Add("lpr_port_monitor", My.Resources.feature_lpr_port_monitor)
    imlFeatures.Images.Add("mahjong_titans", My.Resources.feature_mahjong_titans)
    imlFeatures.Images.Add("media_features", My.Resources.feature_media_features)
    imlFeatures.Images.Add("microsoft_.net_framework", My.Resources.feature_microsoft__net_framework)
    imlFeatures.Images.Add("microsoft_message_queue_(msmq)_server", My.Resources.feature_microsoft_message_queue__msmq__server)
    imlFeatures.Images.Add("minesweeper", My.Resources.feature_minesweeper)
    imlFeatures.Images.Add("more_games", My.Resources.feature_more_games)
    imlFeatures.Images.Add("msmq_active_directory_domain_services_integration", My.Resources.feature_msmq_active_directory_domain_services_integration)
    imlFeatures.Images.Add("msmq_dcom_proxy", My.Resources.feature_msmq_dcom_proxy)
    imlFeatures.Images.Add("msmq_http_support", My.Resources.feature_msmq_http_support)
    imlFeatures.Images.Add("msmq_triggers", My.Resources.feature_msmq_triggers)
    imlFeatures.Images.Add("multicasting_support", My.Resources.feature_multicasting_support)
    imlFeatures.Images.Add("odbc_logging", My.Resources.feature_odbc_logging)
    imlFeatures.Images.Add("performance_features", My.Resources.feature_performance_features)
    imlFeatures.Images.Add("print_and_document_services", My.Resources.feature_print_and_document_services)
    imlFeatures.Images.Add("process_model", My.Resources.feature_process_model)
    imlFeatures.Images.Add("purble_place", My.Resources.feature_purble_place)
    imlFeatures.Images.Add("ras_connection_manager_administration_kit_(cmak)", My.Resources.feature_ras_connection_manager_administration_kit__cmak_)
    imlFeatures.Images.Add("remote_differential_compression", My.Resources.feature_remote_differential_compression)
    imlFeatures.Images.Add("request_filtering", My.Resources.feature_request_filtering)
    imlFeatures.Images.Add("request_monitor", My.Resources.feature_request_monitor)
    imlFeatures.Images.Add("rip_listener", My.Resources.feature_rip_listener)
    imlFeatures.Images.Add("scan_management", My.Resources.feature_scan_management)
    imlFeatures.Images.Add("search_tools", My.Resources.feature_search_tools)
    imlFeatures.Images.Add("security", My.Resources.feature_security)
    imlFeatures.Images.Add("server-side_includes", My.Resources.feature_server_side_includes)
    imlFeatures.Images.Add("services_for_nfs", My.Resources.feature_services_for_nfs)
    imlFeatures.Images.Add("simple_network_management_protocol_(snmp)", My.Resources.feature_simple_network_management_protocol__snmp_)
    imlFeatures.Images.Add("simple_tcpip_services", My.Resources.feature_simple_tcpip_services)
    imlFeatures.Images.Add("solitaire", My.Resources.feature_solitaire)
    imlFeatures.Images.Add("spider_solitaire", My.Resources.feature_spider_solitaire)
    imlFeatures.Images.Add("static_content", My.Resources.feature_static_content)
    imlFeatures.Images.Add("subsystem_for_unix-based_applications", My.Resources.feature_subsystem_for_unix_based_applications)
    imlFeatures.Images.Add("tablet_pc_components", My.Resources.feature_tablet_pc_components)
    imlFeatures.Images.Add("telnet", My.Resources.feature_telnet)
    imlFeatures.Images.Add("tftp_client", My.Resources.feature_tftp_client)
    imlFeatures.Images.Add("tracing", My.Resources.feature_tracing)
    imlFeatures.Images.Add("url_authorization", My.Resources.feature_url_authorization)
    imlFeatures.Images.Add("web_management_tools", My.Resources.feature_web_management_tools)
    imlFeatures.Images.Add("webdav_publishing", My.Resources.feature_webdav_publishing)
    imlFeatures.Images.Add("windows_authentication", My.Resources.feature_windows_authentication)
    imlFeatures.Images.Add("windows_communication_foundation_http_activation", My.Resources.feature_windows_communication_foundation_http_activation)
    imlFeatures.Images.Add("windows_communication_foundation_non-http_activation", My.Resources.feature_windows_communication_foundation_non_http_activation)
    imlFeatures.Images.Add("windows_dvd_maker", My.Resources.feature_windows_dvd_maker)
    imlFeatures.Images.Add("windows_fax_and_scan", My.Resources.feature_windows_fax_and_scan)
    imlFeatures.Images.Add("windows_gadget_platform", My.Resources.feature_windows_gadget_platform)
    imlFeatures.Images.Add("windows_media_center", My.Resources.feature_windows_media_center)
    imlFeatures.Images.Add("windows_media_player", My.Resources.feature_windows_media_player)
    imlFeatures.Images.Add("windows_process_activation_service", My.Resources.feature_windows_process_activation_service)
    imlFeatures.Images.Add("windows_tiff_ifilter", My.Resources.feature_windows_tiff_ifilter)
    imlFeatures.Images.Add("world_wide_web_services", My.Resources.feature_world_wide_web_services)
    imlFeatures.Images.Add("xml_paper_specification", My.Resources.feature_xps)

    imlUpdates.Images.Add("DID", My.Resources.u_a)
    imlUpdates.Images.Add("DO", My.Resources.u_p)
    imlUpdates.Images.Add("UNDO", My.Resources.u_u)
    imlUpdates.Images.Add("PROBLEM", My.Resources.u_i)
    imlUpdates.Images.Add("NO", My.Resources.u_n)
  End Sub

  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    Dim iIndex As Integer = Int(txtIndex.Text)
    Dim sName As String = txtName.Text
    If originalPackageName = sName Then sName = Nothing
    Dim Features As New List(Of Feature)
    If FeatureData IsNot Nothing AndAlso FeatureData.Count > 0 Then
      For Each Feature In FeatureData
        If (Feature.State = "Enabled" Or Feature.State = "Enable Pending") And Not Feature.Enable Then
          Features.Add(Feature)
        ElseIf Not (Feature.State = "Enabled" Or Feature.State = "Enable Pending") And Feature.Enable Then
          Features.Add(Feature)
        End If
      Next
    End If
    Dim Updates As New List(Of Update_Integrated)
    If UpdateData IsNot Nothing AndAlso UpdateData.Count > 0 Then
      For Each Item In UpdateData
        If Item.Remove Then Updates.Add(Item)
      Next
    End If
    Dim Drivers As New List(Of Driver)
    If DriverData IsNot Nothing AndAlso DriverData.Count > 0 Then
      For Each Item In DriverData
        If Item.Remove Then Drivers.Add(Item)
      Next
    End If
    RaiseEvent Response(Me, New PackagePropertiesEventArgs(iIndex, originalPackageName, sName, Features.ToArray, Updates.ToArray, Drivers.ToArray))
    Me.Close()
  End Sub
  Private Sub frmPackageProps_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    ResizeUpdates()
    ResizeDrivers()
  End Sub
  Private Sub ResizeUpdates()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ResizeUpdates))
      Return
    End If
    Dim tUpdates As New Threading.Timer(New Threading.TimerCallback(AddressOf AsyncResizeUpdates), Nothing, 200, System.Threading.Timeout.Infinite)
  End Sub
  Private Sub AsyncResizeUpdates()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf AsyncResizeUpdates))
      Return
    End If
    If bLoading Then Return
    If lvUpdates.Columns.Count = 0 Then Return
    If lvUpdates.ClientSize.Width = 0 Then Return
    If lvUpdates.Items.Count > 0 Then
      lvUpdates.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
    Else
      lvUpdates.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.None)
      lvUpdates.Columns(1).Width = 0
    End If
    Dim packageSize As Integer = lvUpdates.ClientSize.Width - lvUpdates.Columns(1).Width - 1
    If Not lvUpdates.Columns(0).Width = packageSize Then lvUpdates.Columns(0).Width = packageSize
  End Sub

  Private Sub ResizeDrivers()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ResizeDrivers))
      Return
    End If
    Dim tDrivers As New Threading.Timer(New Threading.TimerCallback(AddressOf AsyncResizeDrivers), Nothing, 200, System.Threading.Timeout.Infinite)
  End Sub
  Private Sub AsyncResizeDrivers()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf AsyncResizeDrivers))
      Return
    End If
    If bLoading Then Return
    If lvDriverClass.Columns.Count = 0 Then Return
    If lvDriverClass.ClientSize.Width = 0 Then Return
    lvDriverClass.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.None)
    lvDriverClass.Columns(0).Width = lvDriverClass.ClientSize.Width - 2
    If lvDriverProvider.Columns.Count = 0 Then Return
    If lvDriverProvider.ClientSize.Width = 0 Then Return
    lvDriverProvider.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.None)
    lvDriverProvider.Columns(0).Width = lvDriverProvider.ClientSize.Width - 2
    If lvDriverINF.Columns.Count = 0 Then Return
    If lvDriverINF.ClientSize.Width = 0 Then Return
    If lvDriverINF.Items.Count = 0 Then
      lvDriverINF.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.None)
      lvDriverINF.Columns(1).Width = 0
    Else
      lvDriverINF.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvDriverINF.Columns(1).Width > lvDriverINF.ClientSize.Width / 2 Then
        lvDriverINF.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.None)
        lvDriverINF.Columns(1).Width = 0
      End If
    End If
    lvDriverINF.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.None)
    lvDriverINF.Columns(0).Width = lvDriverINF.ClientSize.Width - lvDriverINF.Columns(1).Width - 2
  End Sub
  Private Sub frmPackageProps_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    AsyncResizeUpdates()
    AsyncResizeDrivers()
  End Sub

  Private Sub expFeatures_Closed(sender As Object, e As System.EventArgs) Handles expFeatures.Closed
    PositionViews()
  End Sub
  Private Sub expFeatures_Opened(sender As System.Object, e As System.EventArgs) Handles expFeatures.Opened
    PositionViews()
    DisplayFeatures()
  End Sub
  Private Sub expUpdates_Closed(sender As Object, e As System.EventArgs) Handles expUpdates.Closed
    PositionViews()
  End Sub
  Private Sub expUpdates_Opened(sender As System.Object, e As System.EventArgs) Handles expUpdates.Opened
    PositionViews()
    DisplayUpdates()
    ResizeUpdates()
  End Sub
  Private Sub expDrivers_Closed(sender As System.Object, e As System.EventArgs) Handles expDrivers.Closed
    PositionViews()
  End Sub
  Private Sub expDrivers_Opened(sender As System.Object, e As System.EventArgs) Handles expDrivers.Opened
    PositionViews()
    DisplayDrivers()
    ResizeDrivers()
  End Sub

  Private Sub ToggleUI(Enable As Boolean)
    txtName.ReadOnly = Not Enable
    'expFeatures.Enabled = Enable
    cmdLoadFeatures.Enabled = Enable
    tvFeatures.Enabled = Enable
    'expUpdates.Enabled = Enable
    cmdLoadUpdates.Enabled = Enable
    lvUpdates.ReadOnly = Not Enable
    'expDrivers.Enabled = Enable
    cmdLoadDrivers.Enabled = Enable
    lvDriverClass.ReadOnly = Not Enable
    lvDriverProvider.ReadOnly = Not Enable
    lvDriverINF.ReadOnly = Not Enable
    cmdSave.Enabled = Enable
    cmdClose.Enabled = Enable
  End Sub

  Private Sub PositionViews()
    pnlProps.SuspendLayout()
    Dim SomethingOpen As Boolean = False
    If expFeatures.Open Then
      SomethingOpen = True
      tvFeatures.Visible = True
      cmdLoadFeatures.Visible = FeatureData Is Nothing
      pnlLists.RowStyles(1) = New RowStyle(SizeType.Percent, 33)
    Else
      tvFeatures.Visible = False
      cmdLoadFeatures.Visible = False
      pnlLists.RowStyles(1) = New RowStyle(SizeType.AutoSize)
    End If
    If expUpdates.Open Then
      SomethingOpen = True
      lvUpdates.Visible = True
      cmdLoadUpdates.Visible = UpdateData Is Nothing
      pnlLists.RowStyles(4) = New RowStyle(SizeType.Percent, 33)
      ResizeUpdates()
    Else
      lvUpdates.Visible = False
      cmdLoadUpdates.Visible = False
      pnlLists.RowStyles(4) = New RowStyle(SizeType.AutoSize)
    End If
    If expDrivers.Open Then
      SomethingOpen = True
      pnlDrivers.Visible = True
      cmdLoadDrivers.Visible = DriverData Is Nothing
      pnlLists.RowStyles(7) = New RowStyle(SizeType.Percent, 33)
      ResizeDrivers()
    Else
      pnlDrivers.Visible = False
      cmdLoadDrivers.Visible = False
      pnlLists.RowStyles(7) = New RowStyle(SizeType.AutoSize)
    End If
    If SomethingOpen Then
      If Me.Width = 510 Then Me.Width = 635
      Me.MinimumSize = New Size(635, 560)
    Else
      Me.MinimumSize = New Size(510, 560)
      If Me.Width = 635 Then Me.Width = 510
    End If
    pnlProps.ResumeLayout()
  End Sub
  Private Sub DisplayFeatures()
    Dim ParentFeatures As New Collections.Specialized.NameValueCollection
    Dim featureRows() As String = Split(My.Resources.ParentFeatureList, vbNewLine)
    For Each row In featureRows
      If row.Contains("|") Then
        Dim RowSplit() As String = Split(row, "|", 2)
        ParentFeatures.Add(RowSplit(0), RowSplit(1))
      End If
    Next
    If tvFeatures.Nodes.Count = 0 Then
      If FeatureData IsNot Nothing Then
        tvFeatures.Enabled = True
        For Each pFeature As Feature In FeatureData
          Dim FeatureDisplayName As String = pFeature.DisplayName
          If String.IsNullOrEmpty(FeatureDisplayName) Then FeatureDisplayName = pFeature.FeatureName
          Dim tvItem As TreeNode = Nothing
          Dim shouldAdd As Boolean = False
          Dim searchNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find("tvn" & FeatureDisplayName.Replace(" ", "_"), True))
          If searchNode Is Nothing Then
            tvItem = New TreeNode(FeatureDisplayName)
            shouldAdd = True
          Else
            tvItem = searchNode
          End If
          tvItem.Name = "tvn" & FeatureDisplayName.Replace(" ", "_")
          tvItem.Tag = pFeature
          tvItem.Checked = (pFeature.State = "Enabled") Or (pFeature.State = "Enable Pending")
          Dim sDescription As String = pFeature.Desc
          If sDescription.Length > 80 Then
            If sDescription.LastIndexOf(" ", 80) > 0 Then
              Dim FrontHalf As String = sDescription.Substring(0, sDescription.LastIndexOf(" ", 80))
              Dim RearHalf As String = sDescription.Substring(sDescription.LastIndexOf(" ", 80) + 1)
              Do
                If RearHalf.Length > 80 Then
                  If RearHalf.LastIndexOf(" ", 80) > 0 Then
                    FrontHalf = FrontHalf & vbNewLine & RearHalf.Substring(0, RearHalf.LastIndexOf(" ", 80))
                    RearHalf = RearHalf.Substring(RearHalf.LastIndexOf(" ", 80) + 1)
                  Else
                    sDescription = FrontHalf & vbNewLine & RearHalf
                    Exit Do
                  End If
                Else
                  sDescription = FrontHalf & vbNewLine & RearHalf
                  Exit Do
                End If
              Loop
            End If
          End If
          If pFeature.CustomProperties IsNot Nothing AndAlso pFeature.CustomProperties.Length > 0 Then
            If pFeature.CustomProperties.Length = 1 AndAlso pFeature.CustomProperties(0).Contains("(No custom properties found)") Then
              tvItem.ToolTipText = FeatureDisplayName & vbNewLine &
                                   " " & sDescription & vbNewLine &
                                   " Internal Name: " & pFeature.FeatureName & vbNewLine &
                                   " State: " & pFeature.State & vbNewLine &
                                   " Restart Required: " & pFeature.RestartRequired
            Else
              Dim sCustomProps As String = Nothing
              If pFeature.CustomProperties.Length = 1 Then
                Dim sPropKey As String = pFeature.CustomProperties(0)
                Dim sPropVal As String = Nothing
                If sPropKey.Contains(" : ") Then
                  sPropVal = sPropKey.Substring(sPropKey.IndexOf(" : ") + 3)
                  sPropKey = sPropKey.Substring(0, sPropKey.IndexOf(" : "))
                End If
                If sPropKey = "SoftBlockLink" Then
                  If String.IsNullOrEmpty(sCustomProps) OrElse Not sCustomProps.Contains("Changes default settings.") Then sCustomProps = " Changes default settings." & vbNewLine & sCustomProps
                  sPropKey = "Link"
                End If
                sCustomProps &= " " & sPropKey & ": " & sPropVal
              Else
                For Each sProperty In pFeature.CustomProperties
                  Dim sPropKey As String = sProperty
                  Dim sPropVal As String = Nothing
                  If sPropKey.Contains(" : ") Then
                    sPropVal = sPropKey.Substring(sPropKey.IndexOf(" : ") + 3)
                    sPropKey = sPropKey.Substring(0, sPropKey.IndexOf(" : "))
                  End If
                  If sPropKey = "SoftBlockLink" Then
                    If String.IsNullOrEmpty(sCustomProps) OrElse Not sCustomProps.Contains("Changes default settings.") Then sCustomProps = " Changes default settings." & vbNewLine & sCustomProps
                    sPropKey = "Link"
                  End If
                  sCustomProps &= " " & sPropKey & ": " & sPropVal & vbNewLine
                Next
                Do While sCustomProps.EndsWith(vbNewLine)
                  sCustomProps = sCustomProps.Substring(0, sCustomProps.Length - 2)
                Loop
              End If
              tvItem.ToolTipText = FeatureDisplayName & vbNewLine &
                                   " " & sDescription & vbNewLine &
                                   " Internal Name: " & pFeature.FeatureName & vbNewLine &
                                   " State: " & pFeature.State & vbNewLine &
                                   " Restart Required: " & pFeature.RestartRequired & vbNewLine &
                                   sCustomProps
            End If
          Else
            tvItem.ToolTipText = FeatureDisplayName & vbNewLine &
                                 " " & sDescription & vbNewLine &
                                 " Internal Name: " & pFeature.FeatureName & vbNewLine &
                                 " State: " & pFeature.State & vbNewLine &
                                 " Restart Required: " & pFeature.RestartRequired
          End If
          Dim KeyID As String = Nothing
          For Each key In imlFeatures.Images.Keys
            If FeatureDisplayName.ToLower.Replace(" ", "_") = key Then
              KeyID = key
              Exit For
            End If
          Next
          If String.IsNullOrEmpty(KeyID) Then
            For Each key In imlFeatures.Images.Keys
              If FeatureDisplayName.ToLower.Replace(" ", "_").Contains(key) Then
                KeyID = key
                Exit For
              End If
            Next
          End If
          If shouldAdd Then
            If ParentFeatures.AllKeys.Contains(FeatureDisplayName) Then
              Dim parentNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find("tvn" & ParentFeatures(FeatureDisplayName).Replace(" ", "_"), True))
              If parentNode Is Nothing Then
                Dim newNode As TreeNode = tvFeatures.Nodes.Add(ParentFeatures(FeatureDisplayName))
                newNode.Name = "tvn" & ParentFeatures(FeatureDisplayName).Replace(" ", "_")
                newNode.Nodes.Add(tvItem)
                newNode.ToolTipText = "Node Group - Not a Feature"
                Dim ParentKeyID As String = Nothing
                For Each key In imlFeatures.Images.Keys
                  If ParentFeatures(FeatureDisplayName).ToLower.Replace(" ", "_") = key Then
                    ParentKeyID = key
                    Exit For
                  End If
                Next
                If String.IsNullOrEmpty(ParentKeyID) Then
                  For Each key In imlFeatures.Images.Keys
                    If ParentFeatures(FeatureDisplayName).ToLower.Replace(" ", "_").Contains(key) Then
                      ParentKeyID = key
                      Exit For
                    End If
                  Next
                End If
                If Not String.IsNullOrEmpty(ParentKeyID) Then
                  newNode.ImageKey = ParentKeyID
                Else
                  newNode.ImageKey = "Unknown"
                End If
                newNode.SelectedImageKey = newNode.ImageKey
              Else
                parentNode.Nodes.Add(tvItem)
              End If
            Else
              tvFeatures.Nodes.Add(tvItem)
            End If
          End If
          If Not String.IsNullOrEmpty(KeyID) Then
            tvItem.ImageKey = KeyID
          Else
            If tvItem.Parent IsNot Nothing AndAlso Not String.IsNullOrEmpty(tvItem.Parent.ImageKey) Then
              tvItem.ImageKey = tvItem.Parent.ImageKey
            Else
              tvItem.ImageKey = "Unknown"
            End If
          End If
          tvItem.SelectedImageKey = tvItem.ImageKey
        Next
        tvFeatures.Sort()
      Else
        tvFeatures.Enabled = False
      End If
    End If
  End Sub
  Private Sub DisplayUpdates()
    If lvUpdates.Items.Count > 0 Then
      lvUpdates.ReadOnly = False
      Return
    End If
    If UpdateData Is Nothing Then
      lvUpdates.ReadOnly = True
      Return
    End If
    lvUpdates.ReadOnly = False
    Dim sGroupList As New Collections.Generic.SortedDictionary(Of String, String)
    For Each pUpdate As Update_Integrated In UpdateData
      Dim sReleaseType As String = pUpdate.ReleaseType
      If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ReleaseType) Then sReleaseType = pUpdate.UpdateInfo.ReleaseType
      Dim sGroupName As String = "Unknown"
      If Not String.IsNullOrEmpty(sReleaseType) Then sGroupName = sReleaseType
      If sGroupName = "Security Update" Then
        If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then
          If Split(pUpdate.Ident.Version, ".", 4)(0) > 7 Then
            sGroupName &= " for Internet Explorer"
          Else
            sGroupName &= " for Windows"
          End If
        End If
      End If
      Dim sGroupKey As String = "lvgUnknown"
      If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = "lvg" & sGroupName.Replace(" ", "_")
      If Not sGroupList.ContainsKey(sGroupKey) Then sGroupList.Add(sGroupKey, sGroupName)
    Next
    If sGroupList.Count > 0 Then
      For Each sGroupEntry In sGroupList
        lvUpdates.Groups.Add(sGroupEntry.Key, sGroupEntry.Value)
      Next
    End If
    lvUpdates.Sorting = SortOrder.Ascending
    For Each pUpdate As Update_Integrated In UpdateData
      If String.IsNullOrEmpty(pUpdate.UpdateInfo.Identity) Then

        Dim pVer As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then
          pVer = pUpdate.Ident.Version
        Else
          pVer = "Unknown"
        End If
        If String.IsNullOrEmpty(pVer) Then pVer = "Unknown"
        Dim Versions() As Version = GetVersions(pVer)
        Dim vParentProd As Version = Versions(0)
        Dim vThisProd As Version = Versions(1)
        Dim sParentProd As String = ConvertOSVerToID(vParentProd)
        Dim sReleaseName As String = Nothing
        If pUpdate.ReleaseType = "Language Pack" AndAlso Not String.IsNullOrEmpty(pUpdate.Ident.Language) Then
          sReleaseName = pUpdate.Ident.Language & " Multilingual User Interface Pack"
        ElseIf pUpdate.ReleaseType = "Service Pack" Then
          sReleaseName = pUpdate.ReleaseType & " " & vThisProd.Major & " (Build " & vThisProd.Minor & ")"
        Else
          sReleaseName = pUpdate.ReleaseType
        End If
        Dim sName As String = pUpdate.Ident.Name
        Dim pName As String = Nothing
        If pUpdate.Ident.Name.StartsWith("Package_for_KB") Then
          Dim sArticle As String = pUpdate.Ident.Name.Substring(pUpdate.Ident.Name.IndexOf("KB"))
          pName = sReleaseName & " for " & sParentProd & " (" & sArticle & ")"
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-Client-LanguagePack-Package" Then
          pName = sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-CodecPack-Basic-Package" Then
          pName = "Basic Codec " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-Foundation-Package" Then
          pName = sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-IE-Troubleshooters-Package" Then
          pName = "Internet Explorer Troubleshooters " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-InternetExplorer-Optional-Package" Then
          pName = "Optional " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-InternetExplorer-Package-TopLevel" Then
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          Else
            pName = sParentProd
          End If
        ElseIf pUpdate.Ident.Name.StartsWith("Microsoft-Windows-LocalPack-") And pUpdate.Ident.Name.EndsWith("-Package") Then
          Dim sLocale As String = pUpdate.Ident.Name.Substring(pUpdate.Ident.Name.IndexOf("LocalPack-") + 10)
          sLocale = sLocale.Substring(0, sLocale.IndexOf("-Package"))
          pName = sLocale & " " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-PlatformUpdate-Win7-SRV08R2-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Platform Update for Windows 7 (" & sArticle & ")"
          Else
            pName = "Platform Update for Windows 7"
          End If
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-RDP-WinIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Remote App and Desktop Connections Update (" & sArticle & ")"
          Else
            pName = "Remote App and Desktop Connections Update"
          End If
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-RDP-BlueIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Remote Desktop Protocol Update (" & sArticle & ")"
          Else
            pName = "Remote Desktop Protocol Update"
          End If
        ElseIf sName.StartsWith("Update for Microsoft Windows (KB") Then
          Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
          sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
          pName = sReleaseName & " for " & sParentProd & " (" & sArticle & ")"
        Else
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          Else
            pName = sReleaseName & " for " & sParentProd
          End If
        End If
        If vParentProd.Major = 6 And vParentProd.Minor = 1 And vThisProd.Major = 7601 Then pName &= " (Service Pack 1)"
        Dim lvItem As New ListViewItem(pName)
        If pUpdate.Remove Then
          lvItem.Checked = False
        Else
          lvItem.Checked = True
        End If
        If vThisProd.Major = 0 And vThisProd.Minor = 0 Then
          lvItem.SubItems.Add(pVer)
        ElseIf vThisProd.Major < 7600 And vThisProd.Minor < 16385 Then
          lvItem.SubItems.Add(vThisProd.Major & "." & vThisProd.Minor)
        Else
          lvItem.SubItems.Add(pVer)
        End If

        Dim ttName As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Name) Then
          ttName = pUpdate.Ident.Name
          If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then ttName &= " v" & pUpdate.Ident.Version
        End If

        Dim ttState As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.State) Then ttState = " State: " & pUpdate.State

        Dim ttInstalled As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.InstallTime) Then ttInstalled = " Installed: " & pUpdate.InstallTime

        Dim ttArch As String = " " & pUpdate.Ident.Architecture
        If Not String.IsNullOrEmpty(pUpdate.ReleaseType) Then ttArch &= " " & pUpdate.ReleaseType

        Dim ttLang As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Language) AndAlso Not pUpdate.Ident.Language = "Neutral" Then ttLang = " Language: " & pUpdate.Ident.Language

        lvItem.ToolTipText = IIf(String.IsNullOrEmpty(ttName), "", ttName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttState), "", ttState & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttInstalled), "", ttInstalled & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttArch), "", ttArch & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttLang), "", ttLang & vbNewLine)
        Select Case pUpdate.State
          Case "Installed", "Staged", "Enabled" : lvItem.ImageKey = "DID"
          Case "Install Pending", "Enable Pending" : lvItem.ImageKey = "DO"
          Case "Uninstall Pending", "Disable Pending" : lvItem.ImageKey = "UNDO"
          Case "Superseded" : lvItem.ImageKey = "PROBLEM"
          Case Else : lvItem.ImageKey = "NO"
        End Select

        Dim sGroupName As String = "Unknown"
        If Not String.IsNullOrEmpty(pUpdate.ReleaseType) Then sGroupName = pUpdate.ReleaseType
        If sGroupName = "Security Update" Then
          If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then
            If Split(pUpdate.Ident.Version, ".", 4)(0) > 7 Then
              sGroupName &= " for Internet Explorer"
            Else
              sGroupName &= " for Windows"
            End If
          End If
        End If
        Dim sGroupKey As String = "lvgUnknown"
        If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = "lvg" & sGroupName.Replace(" ", "_")
        lvUpdates.Items.Add(lvItem)
        lvItem.Group = lvUpdates.Groups(sGroupKey)
      Else
        Dim pVer As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ProductVersion) Then
          pVer = pUpdate.UpdateInfo.ProductVersion
        ElseIf Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then
          pVer = pUpdate.Ident.Version
        Else
          pVer = "Unknown"
        End If
        If String.IsNullOrEmpty(pVer) Then pVer = "Unknown"
        Dim Versions() As Version = GetVersions(pVer)
        Dim vParentProd As Version = Versions(0)
        Dim vThisProd As Version = Versions(1)
        Dim sParentProd As String = ConvertOSVerToID(vParentProd)
        Dim sReleaseName As String = Nothing
        If pUpdate.UpdateInfo.ReleaseType = "Language Pack" AndAlso Not String.IsNullOrEmpty(pUpdate.Ident.Language) Then
          sReleaseName = pUpdate.Ident.Language & " Multilingual User Interface Pack"
        ElseIf pUpdate.UpdateInfo.ReleaseType = "Service Pack" Then
          sReleaseName = pUpdate.UpdateInfo.ReleaseType & " " & vThisProd.Major & " (Build " & vThisProd.Minor & ")"
        Else
          sReleaseName = pUpdate.UpdateInfo.ReleaseType
        End If
        Dim sName As String = pUpdate.UpdateInfo.Name
        If sName = "default" Then sName = Nothing
        Dim sDescription As String = pUpdate.UpdateInfo.Description
        If sDescription.StartsWith("Fix for KB") Then sDescription = Nothing


        Dim pName As String = Nothing
        If pUpdate.UpdateInfo.ProductName.StartsWith("Package_for_KB") Then
          Dim sArticle As String = pUpdate.UpdateInfo.ProductName.Substring(pUpdate.UpdateInfo.ProductName.IndexOf("KB"))
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          ElseIf Not String.IsNullOrEmpty(sDescription) Then
            pName = sDescription
          Else
            pName = sReleaseName & " for " & sParentProd & " (" & sArticle & ")"
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-Client-LanguagePack-Package" Then
          pName = sReleaseName & " for " & sParentProd
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-CodecPack-Basic-Package" Then
          pName = "Basic Codec " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-Foundation-Package" Then
          If pUpdate.UpdateInfo.FeatureList Is Nothing Then
            pName = sReleaseName & " for " & sParentProd
          Else
            Dim sFeatureName As String = Nothing
            For Each sFeature In pUpdate.UpdateInfo.FeatureList
              If sFeature.StartsWith("Feature Name :") Then
                sFeatureName = sFeature.Substring(sFeature.IndexOf(" :") + 2).Trim
                Exit For
              End If
            Next
            If String.IsNullOrEmpty(sFeatureName) Then
              pName = "Feature " & sReleaseName & " for " & sParentProd
            Else
              pName = sFeatureName & " Feature " & sReleaseName & " for " & sParentProd
            End If
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-IE-Troubleshooters-Package" Then
          pName = "Internet Explorer Troubleshooters " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-InternetExplorer-Optional-Package" Then
          pName = "Optional " & sReleaseName & " for " & sParentProd
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-InternetExplorer-Package-TopLevel" Then
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          Else
            pName = sParentProd
          End If
        ElseIf pUpdate.UpdateInfo.ProductName.StartsWith("Microsoft-Windows-LocalPack-") And pUpdate.UpdateInfo.ProductName.EndsWith("-Package") Then
          Dim sLocale As String = pUpdate.UpdateInfo.ProductName.Substring(pUpdate.UpdateInfo.ProductName.IndexOf("LocalPack-") + 10)
          sLocale = sLocale.Substring(0, sLocale.IndexOf("-Package"))
          Dim sFeatureName As String = Nothing
          For Each sFeature In pUpdate.UpdateInfo.FeatureList
            If sFeature.StartsWith("Feature Name :") Then
              sFeatureName = sFeature.Substring(sFeature.IndexOf(" :") + 2).Trim
              Exit For
            End If
          Next
          If String.IsNullOrEmpty(sFeatureName) Then
            pName = sLocale & " " & sReleaseName & " for " & sParentProd
          Else
            pName = sFeatureName & " " & sParentProd & " Feature " & sReleaseName
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-PlatformUpdate-Win7-SRV08R2-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Platform Update for Windows 7 (" & sArticle & ")"
          Else
            pName = "Platform Update for Windows 7"
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-RDP-WinIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Remote App and Desktop Connections Update (" & sArticle & ")"
          Else
            pName = "Remote App and Desktop Connections Update"
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-RDP-BlueIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = "Remote Desktop Protocol Update (" & sArticle & ")"
          Else
            pName = "Remote Desktop Protocol Update"
          End If
        ElseIf sName.StartsWith("Update for Microsoft Windows (KB") Then
          Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
          sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
          pName = sDescription & " " & sReleaseName & " for " & sParentProd & " (" & sArticle & ")"
        Else
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          ElseIf Not String.IsNullOrEmpty(sDescription) Then
            pName = sDescription
          Else
            pName = sReleaseName & " for " & sParentProd
          End If
        End If

        If Not pUpdate.UpdateInfo.CustomProperties Is Nothing Then
          Dim sSPLevel As String = Nothing
          For Each sProp In pUpdate.UpdateInfo.CustomProperties
            If sProp.StartsWith("LPTargetSPLevel :") Then
              sSPLevel = sProp.Substring(sProp.IndexOf(" :") + 2).Trim
              Exit For
            End If
          Next
          If Not String.IsNullOrEmpty(sSPLevel) Then
            If sSPLevel = "0" Then
              pName &= " (No Service Pack)"
            Else
              pName &= " (Service Pack " & sSPLevel & ")"
            End If
          End If
        End If

        Dim lvItem As New ListViewItem(pName)
        If pUpdate.Remove Then
          lvItem.Checked = False
        Else
          lvItem.Checked = True
        End If
        If vThisProd.Major = 0 And vThisProd.Minor = 0 Then
          lvItem.SubItems.Add(pVer)
        ElseIf vThisProd.Major < 7600 And vThisProd.Minor < 16385 Then
          lvItem.SubItems.Add(vThisProd.Major & "." & vThisProd.Minor)
        Else
          lvItem.SubItems.Add(pVer)
        End If

        Dim ttName As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ProductName) Then
          ttName = pUpdate.UpdateInfo.ProductName
          If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ProductVersion) Then ttName &= " v" & pUpdate.UpdateInfo.ProductVersion
        End If

        Dim ttDescr As String = pUpdate.UpdateInfo.Description

        Dim ttState As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.State) Then ttState = " State: " & pUpdate.UpdateInfo.State

        Dim ttCreation As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.CreationTime) Then ttCreation = " Created: " & pUpdate.UpdateInfo.CreationTime

        Dim ttInstalled As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.InstallTime) Then
          ttInstalled = " Installed: " & pUpdate.UpdateInfo.InstallTime
          If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.InstallClient) AndAlso Not pUpdate.UpdateInfo.InstallClient = "DISM Package Manager Provider" Then ttInstalled &= " by " & pUpdate.UpdateInfo.InstallClient
        End If

        Dim ttCompany As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Company) Then
          ttCompany = " " & pUpdate.UpdateInfo.Company
          If (Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Copyright)) AndAlso (Not pUpdate.UpdateInfo.Company.ToLower = pUpdate.UpdateInfo.Copyright.ToLower) Then ttCompany &= " (" & pUpdate.UpdateInfo.Copyright & ")"
        ElseIf Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Copyright) Then
          ttCompany = " " & pUpdate.UpdateInfo.Copyright
        End If

        Dim ttArch As String = " " & pUpdate.Ident.Architecture
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ReleaseType) Then ttArch &= " " & pUpdate.UpdateInfo.ReleaseType

        Dim ttLang As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Language) AndAlso Not pUpdate.Ident.Language = "Neutral" Then ttLang = " Language: " & pUpdate.Ident.Language

        Dim ttCustom As String = Nothing
        If Not pUpdate.UpdateInfo.CustomProperties Is Nothing Then
          For Each sProp In pUpdate.UpdateInfo.CustomProperties
            If sProp.Contains(" :") Then
              Dim sPropD() As String = Split(sProp, ":", 2)
              Dim sPropKey As String = sPropD(0).Trim
              Dim sPropVal As String = sPropD(1).Trim
              If sPropKey = "LPTargetSPLevel" Then
                sPropKey = "Target Service Pack"
              ElseIf sPropKey = "LPType" Then
                sPropKey = "Language Pack Type"
              End If
              ttCustom &= " " & sPropKey & ": " & sPropVal & vbNewLine
            End If
          Next
          If Not String.IsNullOrEmpty(ttCustom) Then
            Do While ttCustom.EndsWith(vbNewLine)
              ttCustom = ttCustom.Substring(0, ttCustom.Length - 2)
            Loop
          End If
        End If

        Dim ttFeature As String = Nothing
        If Not pUpdate.UpdateInfo.FeatureList Is Nothing Then
          For Each sFeat In pUpdate.UpdateInfo.FeatureList
            If sFeat.Contains(" :") Then
              Dim sFeatD() As String = Split(sFeat, ":", 2)
              Dim sFeatKey As String = sFeatD(0).Trim
              Dim sFeatVal As String = sFeatD(1).Trim
              If sFeatKey = "State" Then
                sFeatKey = "Feature State"
              End If
              ttFeature &= " " & sFeatKey & ": " & sFeatVal & vbNewLine
            End If
          Next
          If Not String.IsNullOrEmpty(ttFeature) Then
            Do While ttFeature.EndsWith(vbNewLine)
              ttFeature = ttFeature.Substring(0, ttFeature.Length - 2)
            Loop
          End If
        End If

        lvItem.ToolTipText = IIf(String.IsNullOrEmpty(ttName), "", ttName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttDescr), "", ttDescr & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttState), "", ttState & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttFeature), "", ttFeature & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttCreation), "", ttCreation & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttInstalled), "", ttInstalled & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttCompany), "", ttCompany & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttArch), "", ttArch & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttLang), "", ttLang & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttCustom), "", ttCustom & vbNewLine)
        Select Case pUpdate.UpdateInfo.State
          Case "Installed", "Staged", "Enabled" : lvItem.ImageKey = "DID"
          Case "Install Pending", "Enable Pending" : lvItem.ImageKey = "DO"
          Case "Uninstall Pending", "Disable Pending" : lvItem.ImageKey = "UNDO"
          Case "Superseded" : lvItem.ImageKey = "PROBLEM"
          Case Else : lvItem.ImageKey = "NO"
        End Select

        Dim sGroupName As String = "Unknown"
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ReleaseType) Then sGroupName = pUpdate.UpdateInfo.ReleaseType
        Dim sGroupKey As String = "lvgUnknown"
        If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = "lvg" & sGroupName.Replace(" ", "_")
        lvUpdates.Items.Add(lvItem)
        lvItem.Group = lvUpdates.Groups(sGroupKey)
      End If
    Next
    lvUpdates.Sort()
  End Sub
  Private Sub DisplayDrivers()
    If lvDriverClass.Items.Count > 0 Then
      lvDriverClass.ReadOnly = False
      Return
    End If
    If DriverData Is Nothing Then
      lvDriverClass.Items.Clear()
      lvDriverClass.ReadOnly = True
      lvDriverProvider.Items.Clear()
      lvDriverProvider.ReadOnly = True
      lvDriverINF.Items.Clear()
      lvDriverINF.ReadOnly = True
      Return
    End If
    lvDriverProvider.Items.Clear()
    lvDriverProvider.ReadOnly = True
    lvDriverINF.Items.Clear()
    lvDriverINF.ReadOnly = True

    Dim ClassList As New SortedList(Of String, Driver)

    For Each pDriver As Driver In DriverData
      Dim sGroupKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ClassGUID) Then sGroupKey = GetDriverClassName(pDriver.ClassGUID)
      If Not ClassList.ContainsKey(sGroupKey) Then
        Dim classDriver As New Driver
        classDriver.ClassName = pDriver.ClassName
        classDriver.ClassDescription = pDriver.ClassDescription
        classDriver.ClassGUID = pDriver.ClassGUID
        classDriver.BootCritical = pDriver.BootCritical
        ClassList.Add(sGroupKey, classDriver)
        imlDriverClass.Images.Add(sGroupKey, pDriver.ClassIcon)
      Else
        If Not ClassList(sGroupKey).BootCritical = pDriver.BootCritical Then
          Debug.Print(GetValidDriverDescription(pDriver) & " BootCritical Hidden!")
          Dim classDriver As New Driver
          classDriver.ClassName = ClassList(sGroupKey).ClassName
          classDriver.ClassDescription = ClassList(sGroupKey).ClassDescription
          classDriver.ClassGUID = ClassList(sGroupKey).ClassGUID
          classDriver.BootCritical = Nothing
          ClassList(sGroupKey) = classDriver
        End If
      End If
    Next

    For Each cDriver As KeyValuePair(Of String, Driver) In ClassList
      If lvDriverClass.Items.ContainsKey(cDriver.Key) Then Continue For
      Dim sGroupTitle As String = GetValidDriverDescription(cDriver.Value)
      Dim lvGroupItem As ListViewItem = lvDriverClass.Items.Add(cDriver.Key, sGroupTitle, cDriver.Key)
      lvGroupItem.ToolTipText = sGroupTitle & vbNewLine &
                                " Class Name: " & cDriver.Value.ClassName & vbNewLine &
                                " Class Description: " & cDriver.Value.ClassDescription & vbNewLine &
                                " Class GUID: " & cDriver.Value.ClassGUID
      If Not String.IsNullOrEmpty(cDriver.Value.BootCritical) Then lvGroupItem.ToolTipText &= vbNewLine & " Boot Critical: " & cDriver.Value.BootCritical
    Next
    lvDriverClass.ReadOnly = (lvDriverClass.Items.Count = 0)
    ResizeDrivers()
  End Sub

  Private Function GetValidDriverDescription(pDriver As Driver) As String
    If Not String.IsNullOrEmpty(pDriver.ClassDescription) AndAlso Not pDriver.ClassDescription = "Unknown device class" Then
      Return pDriver.ClassDescription
    ElseIf Not String.IsNullOrEmpty(pDriver.ClassName) Then
      Return pDriver.ClassName
    Else
      Return "Unknown device class"
    End If
  End Function

  Private Function GetDriverClassName(GUID As String) As String
    Return "lvi" & GUID.Replace("-", "").Replace("{", "").Replace("}", "").ToUpper
  End Function

  Private Sub lvDriverClass_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvDriverClass.SelectedIndexChanged
    If lvDriverClass.SelectedItems.Count = 0 Then
      lvDriverProvider.Items.Clear()
      lvDriverProvider.ReadOnly = True
      lvDriverINF.Items.Clear()
      lvDriverINF.ReadOnly = True
      Return
    End If
    lvDriverINF.Items.Clear()
    lvDriverINF.ReadOnly = True
    Dim sDriverClassName As String = lvDriverClass.SelectedItems(0).Name
    Dim CompanyList As New SortedList(Of String, String)
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      Dim sProviderKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = "lvi" & pDriver.ProviderName.Replace(" ", "_")
      If Not CompanyList.ContainsKey(sProviderKey) Then CompanyList.Add(sProviderKey, pDriver.ProviderName)
    Next
    For Each cDriver As KeyValuePair(Of String, String) In CompanyList
      If lvDriverProvider.Items.ContainsKey(cDriver.Key) Then Continue For
      Dim sProviderTitle As String = "Unknown"
      If Not String.IsNullOrEmpty(cDriver.Value) Then sProviderTitle = GetUpdateCompany(cDriver.Value, CompanyList.Values.ToArray)
      Dim nameExists As Boolean = False
      For Each item As ListViewItem In lvDriverProvider.Items
        If item.Text = sProviderTitle Then
          nameExists = True
          Exit For
        End If
      Next
      If Not nameExists Then
        imlDriverCompany.Images.Add(cDriver.Key, GetDriverCompanyIcon(sProviderTitle))
        lvDriverProvider.Items.Add(cDriver.Key, sProviderTitle, cDriver.Key)
      End If

    Next
    lvDriverProvider.ReadOnly = (lvDriverProvider.Items.Count = 0)
    ResizeDrivers()
    If lvDriverProvider.Items.Count > 0 Then lvDriverProvider.Items(0).Selected = True
  End Sub

  Private Sub lvDriverProvider_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvDriverProvider.SelectedIndexChanged
    If lvDriverClass.SelectedItems.Count = 0 Then
      lvDriverProvider.Items.Clear()
      lvDriverProvider.ReadOnly = True
      lvDriverINF.Items.Clear()
      lvDriverINF.ReadOnly = True
      Return
    End If
    If lvDriverProvider.SelectedItems.Count = 0 Then
      lvDriverINF.Items.Clear()
      lvDriverINF.ReadOnly = True
      Return
    End If

    Dim sDriverClassName As String = lvDriverClass.SelectedItems(0).Name
    Dim CompanyList As New SortedList(Of String, String)
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      Dim sProviderKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = "lvi" & pDriver.ProviderName.Replace(" ", "_")
      If Not CompanyList.ContainsKey(sProviderKey) Then CompanyList.Add(sProviderKey, pDriver.ProviderName)
    Next
    Dim sDriverCompanyName As String = lvDriverProvider.SelectedItems(0).Text
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      If Not GetUpdateCompany(pDriver.ProviderName, CompanyList.Values.ToArray) = sDriverCompanyName Then Continue For
      Dim sDeviceKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.PublishedName) Then sDeviceKey = "lvi" & pDriver.PublishedName.Replace(" ", "_")
      Dim sDeviceTitle As String = "Unknown"
      If Not String.IsNullOrEmpty(pDriver.PublishedName) Then sDeviceTitle = pDriver.PublishedName
      If Not lvDriverINF.Items.ContainsKey(sDeviceKey) Then
        imlDriverINF.Images.Add(sDeviceKey, pDriver.DriverIcon)
        Dim lvDeviceItem As ListViewItem = lvDriverINF.Items.Add(sDeviceKey, sDeviceTitle, sDeviceKey)
        lvDeviceItem.SubItems.Add(pDriver.Version)

        Dim ttPublishedName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.PublishedName) Then
          ttPublishedName = pDriver.PublishedName
          If Not String.IsNullOrEmpty(pDriver.Version) Then ttPublishedName &= " v" & pDriver.Version
        End If
        Dim ttOriginalFileName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.OriginalFileName) Then ttOriginalFileName = " Original File Name: " & pDriver.OriginalFileName
        Dim ttDriverStorePath As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.DriverStorePath) Then ttDriverStorePath = " Driver Store Path: " & pDriver.DriverStorePath
        Dim ttInbox As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.Inbox) Then ttInbox = " Inbox: " & pDriver.Inbox
        Dim ttClassName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ClassName) Then
          ttClassName = " Class Name: " & pDriver.ClassName
          If Not String.IsNullOrEmpty(pDriver.ClassDescription) Then ttClassName &= " (" & pDriver.ClassDescription & ")"
        ElseIf Not String.IsNullOrEmpty(pDriver.ClassDescription) Then
          ttClassName = " Class Description: " & pDriver.ClassDescription
        End If
        Dim ttClassGUID As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ClassGUID) Then ttClassGUID = " Class GUID: " & pDriver.ClassGUID
        Dim ttProviderName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ProviderName) Then ttProviderName = " Provider: " & pDriver.ProviderName
        Dim ttDate As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.Date) Then ttDate = " Date: " & pDriver.Date
        Dim ttArch As String = Nothing
        If pDriver.Architectures IsNot Nothing AndAlso pDriver.Architectures.Count > 0 Then ttArch = " Supported Architectures: " & Join(pDriver.Architectures.ToArray, ", ")
        Dim ttBootCritical As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.BootCritical) Then ttBootCritical = " Boot Critical: " & pDriver.BootCritical

        lvDeviceItem.ToolTipText = IIf(String.IsNullOrEmpty(ttPublishedName), "", ttPublishedName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttOriginalFileName), "", ttOriginalFileName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttDriverStorePath), "", ttDriverStorePath & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttInbox), "", ttInbox & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttClassName), "", ttClassName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttClassGUID), "", ttClassGUID & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttProviderName), "", ttProviderName & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttDate), "", ttDate & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttArch), "", ttArch & vbNewLine) &
                             IIf(String.IsNullOrEmpty(ttBootCritical), "", ttBootCritical & vbNewLine)
        lvDeviceItem.Checked = Not pDriver.Remove
      End If
    Next
    lvDriverINF.ReadOnly = (lvDriverINF.Items.Count = 0)
    ResizeDrivers()
    If lvDriverINF.Items.Count > 0 Then
      lvDriverINF.Items(0).Selected = True
    End If
  End Sub

  Private Sub lvDriverINF_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvDriverINF.MouseDoubleClick
    If lvDriverINF.SelectedItems.Count = 0 Then Return
    Dim sDriverClassName As String = lvDriverClass.SelectedItems(0).Name
    Dim CompanyList As New SortedList(Of String, String)
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      Dim sProviderKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = "lvi" & pDriver.ProviderName.Replace(" ", "_")
      If Not CompanyList.ContainsKey(sProviderKey) Then CompanyList.Add(sProviderKey, pDriver.ProviderName)
    Next
    Dim sDriverCompanyName As String = lvDriverProvider.SelectedItems(0).Text
    Dim sDriverINFName As String = lvDriverINF.SelectedItems(0).Text
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      If Not GetUpdateCompany(pDriver.ProviderName, CompanyList.Values.ToArray) = sDriverCompanyName Then Continue For
      If Not pDriver.PublishedName = sDriverINFName Then Continue For
      Dim driverProps As frmDriverProps
      driverProps = New frmDriverProps(pDriver)
      driverProps.Show(Me)
      Exit For
    Next
  End Sub

  Private Function GetVersions(VerString As String) As Version()
    If Not VerString.Contains(".") Then Return {New Version(0, 0), New Version(0, 0)}
    Dim ver() As String = Split(VerString, ".", 4)
    If ver.Length = 4 Then
      Return {New Version(ver(0), ver(1)), New Version(ver(2), ver(3))}
    ElseIf ver.Length = 3 Then
      Return {New Version(ver(0), ver(1)), New Version(ver(2), 0)}
    ElseIf ver.Length = 2 Then
      Return {New Version(ver(0), ver(1)), New Version(0, 0)}
    Else
      Return {New Version(ver(0), 0), New Version(0, 0)}
    End If
  End Function

  Private Function ConvertOSVerToID(OSVer As Version) As String
    If OSVer.Major = 6 Then
      If OSVer.Minor = 1 Then Return "Windows 7"
    ElseIf OSVer.Major = 7 Then
      If OSVer.Minor = 1 Then
        Return "Windows 8"
      ElseIf OSVer.Minor = 2 Then
        Return "Windows 8.1"
      End If
    ElseIf OSVer.Major > 7 And OSVer.Major < 12 Then
      Return "Internet Explorer " & OSVer.Major
    End If
    Return "Windows " & OSVer.Major & "." & OSVer.Minor
  End Function

  Private Sub cmdLoadFeatures_Click(sender As Object, e As EventArgs) Handles cmdLoadFeatures.Click
    ToggleUI(False)
    frmMain.LoadPackageFeatures(WIMIdenifier, txtIndex.Text)
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      If item.SubItems(1).Text = originalPackageName Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      If UBound(lvItem.Tag) > 1 AndAlso lvItem.Tag(2) IsNot Nothing Then
        FeatureData = lvItem.Tag(2)
        DisplayFeatures()
        PositionViews()
      Else
        MsgDlg(Me, "The Features list could not be loaded. See the Output Console for details.", "Error loading features.", "Feature List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad)
      End If
    End If
  End Sub

  Private Sub cmdLoadUpdates_Click(sender As Object, e As EventArgs) Handles cmdLoadUpdates.Click
    ToggleUI(False)
    frmMain.LoadPackageUpdates(WIMIdenifier, txtIndex.Text)
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      If item.SubItems(1).Text = originalPackageName Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      If UBound(lvItem.Tag) > 0 AndAlso lvItem.Tag(1) IsNot Nothing Then
        Dim Package As ImagePackage = lvItem.Tag(1)
        UpdateData = Package.IntegratedUpdateList
        DisplayUpdates()
        PositionViews()
      Else
        MsgDlg(Me, "The Integrated Updates list could not be loaded. See the Output Console for details.", "Error loading integrated updates.", "Update List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad)
      End If
    End If
  End Sub

  Private Sub cmdLoadDrivers_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoadDrivers.Click
    ToggleUI(False)
    frmMain.LoadPackageDrivers(WIMIdenifier, txtIndex.Text)
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      If item.SubItems(1).Text = originalPackageName Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      If UBound(lvItem.Tag) > 2 AndAlso lvItem.Tag(3) IsNot Nothing Then
        DriverData = lvItem.Tag(3)
        DisplayDrivers()
        PositionViews()
      Else
        MsgDlg(Me, "The Driver list could not be loaded. See the Output Console for details.", "Error loading drivers.", "Driver List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad)
      End If
    End If
  End Sub

  Private Sub tvFeatures_AfterCheck(sender As Object, e As System.Windows.Forms.TreeViewEventArgs) Handles tvFeatures.AfterCheck
    If e.Node.ToolTipText = "Node Group - Not a Feature" Then
      If e.Node.Checked Then e.Node.Checked = False
    Else
      Dim RequiredFeatures As New Collections.Generic.Dictionary(Of String, String())
      Dim featureRows() As String = Split(My.Resources.RequiredFeatureList, vbNewLine)
      For Each row In featureRows
        If row.Contains("|") Then
          Dim RowSplit() As String = Split(row, "|", 2)
          RequiredFeatures.Add(RowSplit(0), Split(RowSplit(1), ";"))
        End If
      Next
      If e.Node.Checked Then
        If RequiredFeatures.ContainsKey(e.Node.Text) Then
          For Each sRequirement As String In RequiredFeatures(e.Node.Text)
            Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find("tvn" & sRequirement.Replace(" ", "_"), True))
            If requiredNode Is Nothing Then
              MsgDlg(Me, "The feature " & e.Node.Text & " requires another feature, " & sRequirement & ", which could not be found!", "A required feature is missing from the feature list.", "Windows Features", MessageBoxButtons.OK, TaskDialogIcon.Information)
              e.Node.Checked = False
            Else
              If Not requiredNode.Checked Then requiredNode.Checked = True
            End If
          Next
        End If
        If e.Node.Parent IsNot Nothing Then
          If Not e.Node.Parent.Checked Then e.Node.Parent.Checked = True
        End If
      Else
        If e.Node.Nodes IsNot Nothing Then
          For Each childNode As TreeNode In e.Node.Nodes
            childNode.Checked = False
          Next
        End If
      End If
    End If
  End Sub

  Private Sub tvFeatures_BeforeCheck(sender As Object, e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvFeatures.BeforeCheck
    If e.Node.ToolTipText = "Node Group - Not a Feature" Then
      If Not e.Node.Checked Then e.Cancel = True
    Else
      Dim RequiredFeatures As New Collections.Generic.Dictionary(Of String, String())
      Dim featureRows() As String = Split(My.Resources.RequiredFeatureList, vbNewLine)
      For Each row In featureRows
        If row.Contains("|") Then
          Dim RowSplit() As String = Split(row, "|", 2)
          RequiredFeatures.Add(RowSplit(0), Split(RowSplit(1), ";"))
        End If
      Next
      If e.Node.Checked Then
        Dim RequiredFor As New List(Of String)
        Dim SearchList As List(Of String) = GetFullNodeList(e.Node)
        For Each Feature In RequiredFeatures
          For Each toFind In SearchList
            If Feature.Value.Contains(toFind) Then
              Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find("tvn" & Feature.Key.Replace(" ", "_"), True))
              Dim isRequired As Boolean = False
              If requiredNode IsNot Nothing Then
                If requiredNode.Checked Then isRequired = True
              End If
              If isRequired And Not RequiredFor.Contains(Feature.Key) Then RequiredFor.Add(Feature.Key)
            End If
          Next
        Next
        If Not e.Action = TreeViewAction.Unknown Then
          Dim sLearnMoreURL As String = Nothing
          If e.Node.ToolTipText.Contains("Link: ") Then sLearnMoreURL = e.Node.ToolTipText.Substring(e.Node.ToolTipText.IndexOf("Link: ") + 6)
          If String.IsNullOrEmpty(sLearnMoreURL) Then
            If RequiredFor.Count > 0 Then
              If MsgDlg(Me, Join(RequiredFor.ToArray, vbNewLine), "The following Windows features will also be turned off because they are dependent on " & e.Node.Text & ". Do you want to continue?", "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
                Return
              End If
            End If
          Else
            If RequiredFor.Count > 0 Then
              If MsgDlg(Me, Join(RequiredFor.ToArray, vbNewLine) & vbNewLine & "Other Windows features and programs on your computer might also be affected, including default settings." & vbNewLine & "<a href=""" & sLearnMoreURL & """>Go online to learn more</a>", "The following Windows features will also be turned off because they are dependent on " & e.Node.Text & ". Do you want to continue?", "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
                Return
              End If
            Else
              If MsgDlg(Me, "<a href=""" & sLearnMoreURL & """>Go online to learn more</a>", "Turning off " & e.Node.Text & " might affect other Windows features and programs installed on your computer, including default settings. Do you want to continue?", "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
              End If
            End If
          End If
        End If
        For Each Feature In RequiredFor
          Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find("tvn" & Feature.Replace(" ", "_"), True))
          If requiredNode IsNot Nothing Then
            If requiredNode.Checked Then requiredNode.Checked = False
          End If
        Next
      End If
    End If
  End Sub

  Private Function GetFullNodeList(fromNode As TreeNode) As List(Of String)
    Dim lNodes As New List(Of String)
    lNodes.Add(fromNode.Text)
    If fromNode.Nodes IsNot Nothing Then
      For Each node As TreeNode In fromNode.Nodes
        lNodes.AddRange(GetFullNodeList(node))
      Next
    End If
    Return lNodes
  End Function

  Private Function FilterFind(Of T)(Results() As T) As T
    If Results.Length = 0 Then
      Return Nothing
    ElseIf Results.Length = 1 Then
      Return Results(0)
    Else
      Return Results(0)
    End If
  End Function

  Private Sub tvFeatures_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles tvFeatures.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      selFeature = tvFeatures.HitTest(e.X, e.Y).Node
      If selFeature IsNot Nothing Then
        If selFeature.Nodes Is Nothing OrElse selFeature.Nodes.Count = 0 Then
          mnuFeatureExpand.Enabled = False
          mnuFeatureCollapse.Enabled = False
        Else
          mnuFeatureExpand.Enabled = True
          mnuFeatureCollapse.Enabled = True
        End If
        mnuFeatureEnabled.Checked = selFeature.Checked
        mnuFeatures.Show(tvFeatures, e.Location)
      End If
    End If
  End Sub

  Private Sub mnuFeatureEnabled_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureEnabled.Click
    If selFeature IsNot Nothing Then
      selFeature.Checked = Not selFeature.Checked
    End If
  End Sub

  Private Sub mnuFeatureExpand_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureExpand.Click
    If selFeature IsNot Nothing Then
      selFeature.ExpandAll()
    End If
  End Sub

  Private Sub mnuFeatureCollapse_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureCollapse.Click
    If selFeature IsNot Nothing Then
      selFeature.Collapse(False)
    End If
  End Sub

  Private Sub lvUpdates_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lvUpdates.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      selUpdate = lvUpdates.GetItemAt(e.X, e.Y)
      If selUpdate IsNot Nothing Then
        mnuUpdateInclude.Checked = selUpdate.Checked
        mnuUpdates.Show(lvUpdates, e.Location)
      End If
    End If
  End Sub

  Private Sub mnuUpdateInclude_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdateInclude.Click
    If selUpdate IsNot Nothing Then
      If selUpdate.Checked Then
        selUpdate.Checked = False
      Else
        selUpdate.Checked = True
      End If
    End If
  End Sub
End Class