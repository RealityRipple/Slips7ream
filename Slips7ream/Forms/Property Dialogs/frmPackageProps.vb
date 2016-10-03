Public Class frmPackageProps
  Private imgID As String
  Private Group As WIMGroup
  Private UpdateData As List(Of Update_Integrated)
  Private FeatureData As List(Of Feature)
  Private DriverData As List(Of Driver)
  Public Class PackagePropertiesEventArgs
    Inherits EventArgs
    Public ImageID As String
    Public NewImageName As String
    Public NewImageDesc As String
    Public FeatureList() As Feature
    Public UpdateList() As Update_Integrated
    Public DriverList() As Driver
    Public Sub New(ID As String, NewName As String, NewDescription As String, Features() As Feature, Updates() As Update_Integrated, Drivers() As Driver)
      ImageID = ID
      NewImageName = NewName
      NewImageDesc = NewDescription
      FeatureList = Features
      UpdateList = Updates
      DriverList = Drivers
    End Sub
  End Class
  Public Event Response(sender As Object, e As PackagePropertiesEventArgs)
  Private bLoading As Boolean = False
  Private selFeature As TreeNode
  Private selUpdate As ListViewItem
  Private LoadingFeatures As Boolean = False
  Private LoadingUpdates As Boolean = False
#Region "GUI"
#Region "Window"
  Public Sub New(ImageGroup As WIMGroup, Data As ImagePackage, Features As List(Of Feature), Drivers As List(Of Driver))
    bLoading = True
    InitializeComponent()
    LoadImageLists()
    imgID = Data.ToString
    Group = ImageGroup
    UpdateData = Data.IntegratedUpdateList
    If UpdateData IsNot Nothing AndAlso UpdateData.Count = 0 Then UpdateData = Nothing
    FeatureData = Features
    If FeatureData IsNot Nothing AndAlso FeatureData.Count = 0 Then FeatureData = Nothing
    DriverData = Drivers
    If DriverData IsNot Nothing AndAlso DriverData.Count = 0 Then DriverData = Nothing
    txtIndex.Text = CStr(Data.Index)
    Me.Text = String.Format("{0} Image Package Properties", Data.Name)
    txtName.Text = Data.Name
    txtDesc.Text = Data.Desc
    txtSize.Text = String.Format("{1} ({0} bytes)", FormatNumber(Data.Size, 0, TriState.False, TriState.False, TriState.True), ByteSize(Data.Size))
    txtArchitecture.Text = Data.Architecture
    txtHAL.Text = Data.HAL
    txtVersion.Text = Data.Version
    txtSPLevel.Text = CStr(Data.SPLevel)
    txtSPBuild.Text = CStr(Data.SPBuild)
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
  Private Sub frmPackageProps_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    AsyncResizeUpdates(Nothing)
    AsyncResizeDrivers(Nothing)
    txtName.Focus()
  End Sub
  Private Sub frmPackageProps_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    AsyncResizeUpdates(Nothing)
    AsyncResizeDrivers(Nothing)
  End Sub
  Private Sub frmPackageProps_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
    AsyncResizeUpdates(Nothing)
    AsyncResizeDrivers(Nothing)
  End Sub
#Region "Buttons"
  Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
    Dim sName As String = txtName.Text
    Dim sDesc As String = txtDesc.Text
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
    RaiseEvent Response(Me, New PackagePropertiesEventArgs(imgID, sName, sDesc, Features.ToArray, Updates.ToArray, Drivers.ToArray))
    Me.Close()
  End Sub
  Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
    Me.Close()
  End Sub
#End Region
#End Region
#Region "Lists"
#Region "Features"
  Private Sub expFeatures_Opened(sender As System.Object, e As System.EventArgs) Handles expFeatures.Opened
    PositionViews()
    DisplayFeatures()
    ResizeUpdates()
    ResizeDrivers()
  End Sub
  Private Sub expFeatures_Closed(sender As Object, e As System.EventArgs) Handles expFeatures.Closed
    PositionViews()
  End Sub
  Private Sub tvFeatures_BeforeCheck(sender As Object, e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvFeatures.BeforeCheck
    If LoadingFeatures Then Return
    If e.Node.ToolTipText.StartsWith("Node Group") Then
      If Not e.Node.Checked Then e.Cancel = True
      Return
    End If
    Dim RequiredFeatures As New Dictionary(Of String, String())
    Dim featureRows() As String = Split(My.Resources.RequiredFeatureList, vbNewLine)
    For Each row In featureRows
      If row.Contains("|") Then
        Dim RowSplit() As String = Split(row, "|", 2)
        RequiredFeatures.Add(RowSplit(0), Split(RowSplit(1), ";"))
      End If
    Next
    If Not e.Node.Checked Then Return
    Dim RequiredFor As New List(Of String)
    Dim SearchList As List(Of String) = GetFullNodeList(e.Node)
    For Each Feature In RequiredFeatures
      For Each toFind In SearchList
        If Feature.Value.Contains(toFind) Then
          Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find(String.Format("tvn{0}", Feature.Key.Replace(" ", "_")), True))
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
          If MsgDlg(Me, Join(RequiredFor.ToArray, vbNewLine), String.Format("The following Windows features will also be turned off because they are dependent on {0}. Do you want to continue?", e.Node.Text), "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Warning, MessageBoxDefaultButton.Button1, , "Dependent Features") = Windows.Forms.DialogResult.No Then
            e.Cancel = True
            Return
          End If
        End If
      Else
        If RequiredFor.Count > 0 Then
          If MsgDlg(Me, String.Concat(Join(RequiredFor.ToArray, vbNewLine), vbNewLine, "Other Windows features and programs on your computer might also be affected, including default settings.", vbNewLine, String.Format("<a href=""{0}"">Go online to learn more</a>", sLearnMoreURL)), String.Format("The following Windows features will also be turned off because they are dependent on {0}. Do you want to continue?", e.Node.Text), "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Warning, MessageBoxDefaultButton.Button1, , "Dependent Features") = Windows.Forms.DialogResult.No Then
            e.Cancel = True
            Return
          End If
        Else
          If MsgDlg(Me, String.Format("<a href=""{0}"">Go online to learn more</a>", sLearnMoreURL), String.Format("Turning off {0} might affect other Windows features and programs installed on your computer, including default settings. Do you want to continue?", e.Node.Text), "Windows Features", MessageBoxButtons.YesNo, TaskDialogIcon.Information, MessageBoxDefaultButton.Button1, , "Dependent Features") = Windows.Forms.DialogResult.No Then
            e.Cancel = True
          End If
        End If
      End If
    End If
    For Each Feature In RequiredFor
      Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find(String.Format("tvn{0}", Feature.Replace(" ", "_")), True))
      If requiredNode IsNot Nothing Then
        If requiredNode.Checked Then requiredNode.Checked = False
      End If
    Next
  End Sub
  Private Sub tvFeatures_AfterCheck(sender As Object, e As System.Windows.Forms.TreeViewEventArgs) Handles tvFeatures.AfterCheck
    If LoadingFeatures Then Return
    If e.Node.ToolTipText.StartsWith("Node Group") Then
      If e.Node.Checked Then e.Node.Checked = False
      Return
    End If
    Dim RequiredFeatures As New Dictionary(Of String, String())
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
          Dim requiredNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find(String.Format("tvn{0}", sRequirement.Replace(" ", "_")), True))
          If requiredNode Is Nothing Then
            MsgDlg(Me, String.Format("The feature {0} requires another feature, {1}, which could not be found!", e.Node.Text, sRequirement), "A required feature is missing from the feature list.", "Windows Features", MessageBoxButtons.OK, TaskDialogIcon.Information, , , "Missing Feature")
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
    For I As Integer = 0 To FeatureData.Count - 1
      If FeatureData(I).FeatureName = CType(e.Node.Tag, Feature).FeatureName Then
        Dim newData As Feature = FeatureData(I)
        newData.Enable = e.Node.Checked
        FeatureData(I) = newData
        Exit For
      End If
    Next
  End Sub
  Private Sub tvFeatures_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles tvFeatures.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      selFeature = tvFeatures.HitTest(e.X, e.Y).Node
      If selFeature IsNot Nothing Then
        If selFeature.Nodes Is Nothing OrElse selFeature.Nodes.Count = 0 Then
          Dim collapsed As Boolean = False
          Dim expanded As Boolean = False
          If selFeature.Parent IsNot Nothing Then
            If selFeature.Parent.IsExpanded Then
              expanded = True
            Else
              collapsed = True
            End If
          End If
          mnuFeatureExpand.Enabled = collapsed
          mnuFeatureCollapse.Enabled = expanded
        Else
          Dim collapsed As Boolean = False
          Dim expanded As Boolean = False
          If selFeature.Parent IsNot Nothing Then
            If selFeature.Parent.IsExpanded Then
              expanded = True
            Else
              collapsed = True
            End If
          End If
          If selFeature.IsExpanded Then
            expanded = True
          Else
            collapsed = True
          End If
          For Each tvNode As TreeNode In selFeature.Nodes
            IterateNodes(tvNode, expanded, collapsed)
          Next
          mnuFeatureExpand.Enabled = collapsed
          mnuFeatureCollapse.Enabled = expanded
        End If
        Dim collapsedAll As Boolean = False
        Dim expandedAll As Boolean = False
        For Each tvNode As TreeNode In tvFeatures.Nodes
          IterateNodes(tvNode, expandedAll, collapsedAll)
        Next
        mnuFeatureExpandAll.Enabled = collapsedAll
        mnuFeatureCollapseAll.Enabled = expandedAll
        mnuFeatureEnabled.Checked = selFeature.Checked
        mnuFeatures.Show(tvFeatures, e.Location)
      End If
    End If
  End Sub
#Region "Context Menu"
  Private Sub mnuFeatureEnabled_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureEnabled.Click
    If selFeature IsNot Nothing Then
      selFeature.Checked = Not selFeature.Checked
    End If
  End Sub
  Private Sub mnuFeatureExpandAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureExpandAll.Click
    tvFeatures.ExpandAll()
  End Sub
  Private Sub mnuFeatureExpand_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureExpand.Click
    If selFeature IsNot Nothing Then
      selFeature.ExpandAll()
    End If
  End Sub
  Private Sub mnuFeatureCollapse_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureCollapse.Click
    If selFeature IsNot Nothing Then
      If selFeature.IsExpanded Then
        selFeature.Collapse(False)
      ElseIf selFeature.Parent IsNot Nothing AndAlso selFeature.Parent.IsExpanded Then
        selFeature.Parent.Collapse(False)
      End If
    End If
  End Sub
  Private Sub mnuFeatureCollapseAll_Click(sender As System.Object, e As System.EventArgs) Handles mnuFeatureCollapseAll.Click
    tvFeatures.CollapseAll()
  End Sub
#End Region
  Private Sub cmdLoadFeatures_Click(sender As Object, e As EventArgs) Handles cmdLoadFeatures.Click
    If Not frmMain.RunActivity = ActivityType.Nothing Then
      Dim Activity As ActivityRet = ActivityParser(frmMain.RunActivity)
      MsgDlg(Me, String.Format("Please wait until SLIPS7REAM finishes the {0} process.", Activity.Process), String.Format("SLIPS7REAM is busy {0}.", Activity.Activity), String.Format("Can't Load Windows Features - Busy {0}", Activity.Title), MessageBoxButtons.OK, TaskDialogIcon.Run, , , "Program Busy")
      Return
    End If
    ToggleUI(False)
    frmMain.LoadPackageFeatures(Group, CInt(txtIndex.Text))
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      Dim lvIndex2 As Integer = CInt(item.Tag)
      If frmMain.ImageDataList(lvIndex2).Package.ToString = imgID Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      If frmMain.ImageDataList(lvIndex).FeatureList IsNot Nothing Then
        FeatureData = frmMain.ImageDataList(lvIndex).FeatureList
        DisplayFeatures()
        PositionViews()
      Else
        MsgDlg(Me, "The Features list could not be loaded. See the Output Console for details.", "Error loading features.", "Feature List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad, , , "No Features")
      End If
    End If
  End Sub
#End Region
#Region "Updates"
  Private Sub expUpdates_Opened(sender As System.Object, e As System.EventArgs) Handles expUpdates.Opened
    PositionViews()
    DisplayUpdates()
    ResizeUpdates()
    ResizeDrivers()
  End Sub
  Private Sub expUpdates_Closed(sender As Object, e As System.EventArgs) Handles expUpdates.Closed
    PositionViews()
  End Sub
  Private Sub lvUpdates_ItemChecked(sender As Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvUpdates.ItemChecked
    If LoadingUpdates Then Return
    For I As Integer = 0 To UpdateData.Count - 1
      If UpdateData(I).Identity = CType(e.Item.Tag, Update_Integrated).Identity Then
        Dim newData As Update_Integrated = UpdateData(I)
        newData.Remove = Not e.Item.Checked
        UpdateData(I) = newData
        Exit For
      End If
    Next
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
  Private Sub cmdLoadUpdates_Click(sender As Object, e As EventArgs) Handles cmdLoadUpdates.Click
    If Not frmMain.RunActivity = ActivityType.Nothing Then
      Dim Activity As ActivityRet = ActivityParser(frmMain.RunActivity)
      MsgDlg(Me, String.Format("Please wait until SLIPS7REAM finishes the {0} process.", Activity.Process), String.Format("SLIPS7REAM is busy {0}.", Activity.Activity), String.Format("Can't Load Integrated Windows Updates - Busy {0}", Activity.Title), MessageBoxButtons.OK, TaskDialogIcon.Run, , , "Program Busy")
      Return
    End If
    ToggleUI(False)
    frmMain.LoadPackageUpdates(Group, CInt(txtIndex.Text))
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      Dim lvIndex2 As Integer = CInt(item.Tag)
      If frmMain.ImageDataList(lvIndex2).Package.ToString = imgID Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      If frmMain.ImageDataList(lvIndex).Package.IntegratedUpdateList IsNot Nothing Then
        UpdateData = frmMain.ImageDataList(lvIndex).Package.IntegratedUpdateList
        DisplayUpdates()
        PositionViews()
      Else
        MsgDlg(Me, "The Integrated Updates list could not be loaded. See the Output Console for details.", "Error loading integrated updates.", "Update List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad, , , "No Updates")
      End If
    End If
  End Sub
#End Region
#Region "Drivers"
  Private Sub expDrivers_Opened(sender As System.Object, e As System.EventArgs) Handles expDrivers.Opened
    PositionViews()
    DisplayDrivers()
    ResizeUpdates()
    ResizeDrivers()
  End Sub
  Private Sub expDrivers_Closed(sender As System.Object, e As System.EventArgs) Handles expDrivers.Closed
    PositionViews()
  End Sub
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
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = String.Format("lvi{0}", pDriver.ProviderName.Replace(" ", "_"))
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
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = String.Format("lvi{0}", pDriver.ProviderName.Replace(" ", "_"))
      If Not CompanyList.ContainsKey(sProviderKey) Then CompanyList.Add(sProviderKey, pDriver.ProviderName)
    Next
    Dim sDriverCompanyName As String = lvDriverProvider.SelectedItems(0).Text
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      If Not GetUpdateCompany(pDriver.ProviderName, CompanyList.Values.ToArray) = sDriverCompanyName Then Continue For
      Dim sDeviceKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.PublishedName) Then sDeviceKey = String.Format("lvi{0}", pDriver.PublishedName.Replace(" ", "_"))
      Dim sDeviceTitle As String = "Unknown"
      If Not String.IsNullOrEmpty(pDriver.PublishedName) Then sDeviceTitle = pDriver.PublishedName
      If Not lvDriverINF.Items.ContainsKey(sDeviceKey) Then
        imlDriverINF.Images.Add(sDeviceKey, pDriver.DriverIcon)
        Dim lvDeviceItem As ListViewItem = lvDriverINF.Items.Add(sDeviceKey, sDeviceTitle, sDeviceKey)
        lvDeviceItem.SubItems.Add(pDriver.Version)
        Dim ttPublishedName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.PublishedName) Then
          If String.IsNullOrEmpty(pDriver.Version) Then
            ttPublishedName = pDriver.PublishedName
          Else
            ttPublishedName = String.Format("{0} v{1}", pDriver.PublishedName, pDriver.Version)
          End If
        End If
        Dim ttOriginalFileName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.OriginalFileName) Then ttOriginalFileName = String.Concat(en, String.Format("Original File Name: {0}", pDriver.OriginalFileName))
        Dim ttDriverStorePath As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.DriverStorePath) Then ttDriverStorePath = String.Concat(en, String.Format("Driver Store Path: {0}", pDriver.DriverStorePath))
        Dim ttInBox As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.InBox) Then ttInBox = String.Concat(en, String.Format("In-Box: {0}", pDriver.InBox))
        Dim ttClassName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ClassName) Then
          If String.IsNullOrEmpty(pDriver.ClassDescription) Then
            ttClassName = String.Concat(en, String.Format("Class Name: {0}", pDriver.ClassName))
          Else
            ttClassName = String.Concat(en, String.Format("Class Name: {0} ({1})", pDriver.ClassName, pDriver.ClassDescription))
          End If
        ElseIf Not String.IsNullOrEmpty(pDriver.ClassDescription) Then
          ttClassName = String.Concat(en, String.Format("Class Description: {0}", pDriver.ClassDescription))
        End If
        Dim ttClassGUID As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ClassGUID) Then ttClassGUID = String.Concat(en, String.Format("Class GUID: {0}", pDriver.ClassGUID))
        Dim ttProviderName As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.ProviderName) Then ttProviderName = String.Concat(en, String.Format("Provider: {0}", pDriver.ProviderName))
        Dim ttDate As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.Date) Then ttDate = String.Concat(en, String.Format("Date: {0}", pDriver.Date))
        Dim ttArch As String = Nothing
        If pDriver.Architectures IsNot Nothing AndAlso pDriver.Architectures.Count > 0 Then ttArch = String.Concat(en, String.Format("Supported Architectures: {0}", Join(pDriver.Architectures.ToArray, ", ")))
        Dim ttBootCritical As String = Nothing
        If Not String.IsNullOrEmpty(pDriver.BootCritical) Then ttBootCritical = String.Concat(en, String.Format("Boot Critical: {0}", pDriver.BootCritical))
        Dim sDeviceTT As String = Nothing
        If Not String.IsNullOrEmpty(ttPublishedName) Then sDeviceTT = String.Concat(sDeviceTT, ttPublishedName, vbNewLine)
        If Not String.IsNullOrEmpty(ttOriginalFileName) Then sDeviceTT = String.Concat(sDeviceTT, ttOriginalFileName, vbNewLine)
        If Not String.IsNullOrEmpty(ttDriverStorePath) Then sDeviceTT = String.Concat(sDeviceTT, ttDriverStorePath, vbNewLine)
        If Not String.IsNullOrEmpty(ttInBox) Then sDeviceTT = String.Concat(sDeviceTT, ttInBox, vbNewLine)
        If Not String.IsNullOrEmpty(ttClassName) Then sDeviceTT = String.Concat(sDeviceTT, ttClassName, vbNewLine)
        If Not String.IsNullOrEmpty(ttClassGUID) Then sDeviceTT = String.Concat(sDeviceTT, ttClassGUID, vbNewLine)
        If Not String.IsNullOrEmpty(ttProviderName) Then sDeviceTT = String.Concat(sDeviceTT, ttProviderName, vbNewLine)
        If Not String.IsNullOrEmpty(ttDate) Then sDeviceTT = String.Concat(sDeviceTT, ttDate, vbNewLine)
        If Not String.IsNullOrEmpty(ttArch) Then sDeviceTT = String.Concat(sDeviceTT, ttArch, vbNewLine)
        If Not String.IsNullOrEmpty(ttBootCritical) Then sDeviceTT = String.Concat(sDeviceTT, ttBootCritical, vbNewLine)
        lvDeviceItem.ToolTipText = sDeviceTT.TrimEnd
        lvDeviceItem.Checked = Not pDriver.Remove
      End If
    Next
    lvDriverINF.ReadOnly = (lvDriverINF.Items.Count = 0)
    ResizeDrivers()
    If lvDriverINF.Items.Count > 0 Then
      lvDriverINF.Items(0).Selected = True
    End If
  End Sub
  Private Sub lvDriverINF_ItemChecked(sender As Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvDriverINF.ItemChecked
    If lvDriverINF.SelectedItems.Count = 0 Then Return
    Dim sDriverClassName As String = lvDriverClass.SelectedItems(0).Name
    Dim CompanyList As New SortedList(Of String, String)
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      Dim sProviderKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = String.Format("lvi{0}", pDriver.ProviderName.Replace(" ", "_"))
      If Not CompanyList.ContainsKey(sProviderKey) Then CompanyList.Add(sProviderKey, pDriver.ProviderName)
    Next
    Dim sDriverCompanyName As String = lvDriverProvider.SelectedItems(0).Text
    Dim sDriverINFName As String = lvDriverINF.SelectedItems(0).Text
    For I As Integer = 0 To DriverData.Count - 1
      If Not GetDriverClassName(DriverData(I).ClassGUID) = sDriverClassName Then Continue For
      If Not GetUpdateCompany(DriverData(I).ProviderName, CompanyList.Values.ToArray) = sDriverCompanyName Then Continue For
      If Not DriverData(I).PublishedName = sDriverINFName Then Continue For
      Dim newData As Driver = DriverData(I)
      newData.Remove = Not lvDriverINF.SelectedItems(0).Checked
      DriverData(I) = newData
      Exit For
    Next
  End Sub
  Private Sub lvDriverINF_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvDriverINF.MouseDoubleClick
    If lvDriverINF.SelectedItems.Count = 0 Then Return
    Dim sDriverClassName As String = lvDriverClass.SelectedItems(0).Name
    Dim CompanyList As New SortedList(Of String, String)
    For Each pDriver As Driver In DriverData
      If Not GetDriverClassName(pDriver.ClassGUID) = sDriverClassName Then Continue For
      Dim sProviderKey As String = "lviUnknown"
      If Not String.IsNullOrEmpty(pDriver.ProviderName) Then sProviderKey = String.Format("lvi{0}", pDriver.ProviderName.Replace(" ", "_"))
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
  Private Sub cmdLoadDrivers_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoadDrivers.Click
    If Not frmMain.RunActivity = ActivityType.Nothing Then
      Dim Activity As ActivityRet = ActivityParser(frmMain.RunActivity)
      MsgDlg(Me, String.Format("Please wait until SLIPS7REAM finishes the {0} process.", Activity.Process), String.Format("SLIPS7REAM is busy {0}.", Activity.Activity), String.Format("Can't Load Windows Drivers - Busy {0}", Activity.Title), MessageBoxButtons.OK, TaskDialogIcon.Run, , , "Program Busy")
      Return
    End If
    ToggleUI(False)
    frmMain.LoadPackageDrivers(Group, CInt(txtIndex.Text))
    If Me.Disposing Or Me.IsDisposed Then Return
    Dim lvItem As ListViewItem = Nothing
    For Each item As ListViewItem In frmMain.lvImages.Items
      Dim lvIndex2 As Integer = CInt(item.Tag)
      If frmMain.ImageDataList(lvIndex2).Package.ToString = imgID Then
        lvItem = item
        Exit For
      End If
    Next
    ToggleUI(True)
    If lvItem IsNot Nothing Then
      Dim lvIndex As Integer = CInt(lvItem.Tag)
      If frmMain.ImageDataList(lvIndex).DriverList IsNot Nothing Then
        DriverData = frmMain.ImageDataList(lvIndex).DriverList
        DisplayDrivers()
        PositionViews()
      Else
        MsgDlg(Me, "The Driver list could not be loaded. See the Output Console for details.", "Error loading drivers.", "Driver List Empty", MessageBoxButtons.OK, TaskDialogIcon.Bad, , , "No Drivers")
      End If
    End If
  End Sub
#End Region
#End Region
#End Region
#Region "Useful Functions"
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
  Private Sub ToggleUI(Enable As Boolean)
    If Me.Disposing Or Me.IsDisposed Then Return
    txtName.ReadOnly = Not Enable
    txtDesc.ReadOnly = Not Enable
    cmdLoadFeatures.Enabled = Enable
    tvFeatures.ReadOnly = Not Enable
    cmdLoadUpdates.Enabled = Enable
    lvUpdates.ReadOnly = Not Enable
    cmdLoadDrivers.Enabled = Enable
    lvDriverClass.ReadOnly = Not Enable
    lvDriverProvider.ReadOnly = Not Enable
    lvDriverINF.ReadOnly = Not Enable
    cmdOK.Enabled = Enable
    cmdCancel.Enabled = Enable
  End Sub
  Private Sub PositionViews()
    If Me.Disposing Or Me.IsDisposed Then Return
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
#Region "Features"
  Private Sub DisplayFeatures()
    If tvFeatures.Disposing Or tvFeatures.IsDisposed Then Return
    If tvFeatures.Nodes.Count > 0 Then Return
    If FeatureData Is Nothing Then
      tvFeatures.ReadOnly = True
      Return
    End If
    LoadingFeatures = True
    Dim ParentFeatures As New Collections.Specialized.NameValueCollection
    Dim featureRows() As String = Split(My.Resources.ParentFeatureList, vbNewLine)
    For Each row In featureRows
      If row.Contains("|") Then
        Dim RowSplit() As String = Split(row, "|", 2)
        ParentFeatures.Add(RowSplit(0), RowSplit(1))
      End If
    Next
    tvFeatures.ReadOnly = False
    For Each pFeature As Feature In FeatureData
      Dim FeatureDisplayName As String = pFeature.DisplayName
      If String.IsNullOrEmpty(FeatureDisplayName) Then FeatureDisplayName = pFeature.FeatureName
      Dim tvItem As TreeNode = Nothing
      Dim shouldAdd As Boolean = False
      Dim searchNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find(String.Format("tvn{0}", FeatureDisplayName.Replace(" ", "_")), True))
      If searchNode Is Nothing Then
        tvItem = New TreeNode(FeatureDisplayName)
        shouldAdd = True
      Else
        tvItem = searchNode
      End If
      tvItem.Name = String.Format("tvn{0}", FeatureDisplayName.Replace(" ", "_"))
      tvItem.Tag = pFeature
      tvItem.Checked = pFeature.Enable
      Dim sDescription As String = pFeature.Desc
      If sDescription.Length > 80 Then
        If sDescription.Substring(0, 80).LastIndexOf(" ") > 0 Then
          Dim FrontHalf As String = sDescription.Substring(0, 80)
          FrontHalf = FrontHalf.Substring(0, FrontHalf.LastIndexOf(" "))
          Dim RearHalf As String = sDescription.Substring(FrontHalf.Length + 1)
          Do
            If RearHalf.Length > 80 Then
              If RearHalf.Substring(0, 80).LastIndexOf(" ") > 0 Then
                Dim midHalf As String = RearHalf.Substring(0, 80)
                midHalf = midHalf.Substring(0, midHalf.LastIndexOf(" "))
                FrontHalf = String.Concat(FrontHalf, vbNewLine, midHalf)
                RearHalf = RearHalf.Substring(midHalf.Length + 1)
              Else
                sDescription = String.Concat(FrontHalf, vbNewLine, RearHalf)
                Exit Do
              End If
            Else
              sDescription = String.Format(FrontHalf, vbNewLine, RearHalf)
              Exit Do
            End If
          Loop
        End If
      End If
      If pFeature.CustomProperties IsNot Nothing AndAlso pFeature.CustomProperties.Length > 0 Then
        If pFeature.CustomProperties.Length = 1 AndAlso pFeature.CustomProperties(0).Contains("(No custom properties found)") Then
          tvItem.ToolTipText = String.Concat(FeatureDisplayName, vbNewLine,
                               en, Replace(sDescription, vbNewLine, String.Concat(vbNewLine, en)), vbNewLine,
                               en, String.Format("Internal Name: {0}", pFeature.FeatureName), vbNewLine,
                               en, String.Format("State: {0}", pFeature.State), vbNewLine,
                               en, String.Format("Restart Required: {0}", pFeature.RestartRequired))
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
              If String.IsNullOrEmpty(sCustomProps) OrElse Not sCustomProps.Contains("Changes default settings.") Then sCustomProps = String.Concat(en, "Changes default settings.", vbNewLine, sCustomProps)
              sPropKey = "Link"
            End If
            sCustomProps = String.Concat(sCustomProps, en, String.Format("{0}: {1}", sPropKey, sPropVal))
          Else
            For Each sProperty In pFeature.CustomProperties
              Dim sPropKey As String = sProperty
              Dim sPropVal As String = Nothing
              If sPropKey.Contains(" : ") Then
                sPropVal = sPropKey.Substring(sPropKey.IndexOf(" : ") + 3)
                sPropKey = sPropKey.Substring(0, sPropKey.IndexOf(" : "))
              End If
              If sPropKey = "SoftBlockLink" Then
                If String.IsNullOrEmpty(sCustomProps) OrElse Not sCustomProps.Contains("Changes default settings.") Then sCustomProps = String.Concat(en, "Changes default settings.", vbNewLine, sCustomProps)
                sPropKey = "Link"
              End If
              sCustomProps = String.Concat(sCustomProps, en, String.Format("{0}: {1}", sPropKey, sPropVal), vbNewLine)
            Next
            sCustomProps = sCustomProps.TrimEnd
          End If
          tvItem.ToolTipText = String.Concat(FeatureDisplayName, vbNewLine,
                               en, Replace(sDescription, vbNewLine, String.Concat(vbNewLine, en)), vbNewLine,
                               en, String.Format("Internal Name: {0}", pFeature.FeatureName), vbNewLine,
                               en, String.Format("State: {0}", pFeature.State), vbNewLine,
                               en, String.Format("Restart Required: {0}", pFeature.RestartRequired), vbNewLine,
                               sCustomProps)
        End If
      Else
        tvItem.ToolTipText = String.Concat(FeatureDisplayName, vbNewLine,
                             en, Replace(sDescription, vbNewLine, String.Concat(vbNewLine, en)), vbNewLine,
                             en, String.Format("Internal Name: {0}", pFeature.FeatureName), vbNewLine,
                             en, String.Format("State: {0}", pFeature.State), vbNewLine,
                             en, String.Format("Restart Required: {0}", pFeature.RestartRequired))
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
          Dim parentNode As TreeNode = FilterFind(Of TreeNode)(tvFeatures.Nodes.Find(String.Format("tvn{0}", ParentFeatures(FeatureDisplayName).Replace(" ", "_")), True))
          If parentNode Is Nothing Then
            Dim newNode As TreeNode = tvFeatures.Nodes.Add(ParentFeatures(FeatureDisplayName))
            newNode.Name = String.Format("tvn{0}", ParentFeatures(FeatureDisplayName).Replace(" ", "_"))
            newNode.Nodes.Add(tvItem)
            newNode.ToolTipText = String.Concat("Node Group", vbNewLine,
                                  en, "This is a group containing related features.", vbNewLine,
                                  en, "Node groups are not features and can not be enabled.")
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
            newNode.Expand()
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
    LoadingFeatures = False
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
  Private Sub IterateNodes(tvNode As TreeNode, ByRef expanded As Boolean, ByRef collapsed As Boolean)
    If tvNode.Nodes IsNot Nothing AndAlso tvNode.Nodes.Count > 0 Then
      If tvNode.IsExpanded Then
        expanded = True
      Else
        collapsed = True
      End If
      For Each tvSubNode As TreeNode In tvNode.Nodes
        IterateNodes(tvSubNode, expanded, collapsed)
      Next
    End If
  End Sub
#End Region
#Region "Updates"
  Private Sub DisplayUpdates()
    If lvUpdates.Disposing Or lvUpdates.IsDisposed Then Return
    If lvUpdates.Items.Count > 0 Then Return
    If UpdateData Is Nothing Then
      lvUpdates.ReadOnly = True
      Return
    End If
    LoadingUpdates = True
    lvUpdates.ReadOnly = False
    Dim sGroupList As New SortedDictionary(Of String, String)
    For Each pUpdate As Update_Integrated In UpdateData
      Dim sGroupName As String = ConvertIDToGroup(pUpdate)
      Dim sGroupKey As String = "lvgUnknown"
      If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = String.Format("lvg{0}", sGroupName.Replace(" ", "_"))
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
          sReleaseName = String.Format("{0} Multilingual User Interface Pack", pUpdate.Ident.Language)
        ElseIf pUpdate.ReleaseType = "Service Pack" Then
          sReleaseName = String.Format("{0} {1} (Build {2})", pUpdate.ReleaseType, vThisProd.Major, vThisProd.Minor)
        Else
          sReleaseName = pUpdate.ReleaseType
        End If
        Dim sName As String = pUpdate.Ident.Name
        Dim pName As String = Nothing
        If pUpdate.Ident.Name.StartsWith("Package_for_KB") Then
          Dim sArticle As String = pUpdate.Ident.Name.Substring(pUpdate.Ident.Name.IndexOf("KB"))
          pName = String.Format("{0} for {1} ({2})", sReleaseName, sParentProd, sArticle)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-Client-LanguagePack-Package" Then
          pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-CodecPack-Basic-Package" Then
          pName = String.Format("Basic Codec {0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-Foundation-Package" Then
          pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-IE-Troubleshooters-Package" Then
          pName = String.Format("Internet Explorer Troubleshooters {0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-InternetExplorer-Optional-Package" Then
          pName = String.Format("Optional {0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-InternetExplorer-Package-TopLevel" Then
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          Else
            pName = sParentProd
          End If
        ElseIf pUpdate.Ident.Name.StartsWith("Microsoft-Windows-LocalPack-") And pUpdate.Ident.Name.EndsWith("-Package") Then
          Dim sLocale As String = pUpdate.Ident.Name.Substring(pUpdate.Ident.Name.IndexOf("LocalPack-") + 10)
          sLocale = sLocale.Substring(0, sLocale.IndexOf("-Package"))
          pName = String.Format("{0} {1} for {2}", sLocale, sReleaseName, sParentProd)
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-PlatformUpdate-Win7-SRV08R2-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Platform Update for Windows 7 ({0})", sArticle)
          Else
            pName = "Platform Update for Windows 7"
          End If
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-RDP-WinIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Remote App and Desktop Connections Update ({0})", sArticle)
          Else
            pName = "Remote App and Desktop Connections Update"
          End If
        ElseIf pUpdate.Ident.Name = "Microsoft-Windows-RDP-BlueIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Remote Desktop Protocol Update ({0})", sArticle)
          Else
            pName = "Remote Desktop Protocol Update"
          End If
        ElseIf sName.StartsWith("Update for Microsoft Windows (KB") Then
          Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
          sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
          pName = String.Format("{0} for {1} ({2})", sReleaseName, sParentProd, sArticle)
        Else
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          Else
            pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
          End If
        End If
        If vParentProd.Major = 6 And vParentProd.Minor = 1 And vThisProd.Major = 7601 Then pName = String.Concat(pName, " (Service Pack 1)")
        Dim lvItem As New ListViewItem(pName)
        If pUpdate.Remove Then
          lvItem.Checked = False
        Else
          lvItem.Checked = True
        End If
        If vThisProd.Major = 0 And vThisProd.Minor = 0 Then
          lvItem.SubItems.Add(pVer)
        ElseIf vThisProd.Major < 7600 And vThisProd.Minor < 16385 Then
          lvItem.SubItems.Add(String.Format("{0}.{1}", vThisProd.Major, vThisProd.Minor))
        Else
          lvItem.SubItems.Add(pVer)
        End If
        Dim ttName As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Name) Then
          ttName = pUpdate.Ident.Name
          If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then ttName = String.Format("{0} v{1}", ttName, pUpdate.Ident.Version)
        End If
        Dim ttState As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.State) Then ttState = String.Concat(en, String.Format("State: {0}", pUpdate.State))
        Dim ttInstalled As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.InstallTime) Then ttInstalled = String.Concat(en, String.Format("Installed: {0}", pUpdate.InstallTime))
        Dim ttArch As String = String.Concat(en, pUpdate.Ident.Architecture)
        If Not String.IsNullOrEmpty(pUpdate.ReleaseType) Then ttArch = String.Concat(ttArch, " ", pUpdate.ReleaseType)
        Dim ttLang As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Language) AndAlso Not pUpdate.Ident.Language = "Neutral" Then ttLang = String.Concat(en, String.Format("Language: {0}", pUpdate.Ident.Language))
        Dim sUpdateTT As String = Nothing
        If Not String.IsNullOrEmpty(ttName) Then sUpdateTT = String.Concat(sUpdateTT, ttName, vbNewLine)
        If Not String.IsNullOrEmpty(ttState) Then sUpdateTT = String.Concat(sUpdateTT, ttState, vbNewLine)
        If Not String.IsNullOrEmpty(ttInstalled) Then sUpdateTT = String.Concat(sUpdateTT, ttInstalled, vbNewLine)
        If Not String.IsNullOrEmpty(ttArch) Then sUpdateTT = String.Concat(sUpdateTT, ttArch, vbNewLine)
        If Not String.IsNullOrEmpty(ttLang) Then sUpdateTT = String.Concat(sUpdateTT, ttLang, vbNewLine)
        lvItem.ToolTipText = sUpdateTT.TrimEnd
        Select Case pUpdate.State
          Case "Installed", "Staged", "Enabled" : lvItem.ImageKey = "DID"
          Case "Install Pending", "Enable Pending" : lvItem.ImageKey = "DO"
          Case "Uninstall Pending", "Disable Pending" : lvItem.ImageKey = "UNDO"
          Case "Superseded" : lvItem.ImageKey = "PROBLEM"
          Case Else : lvItem.ImageKey = "NO"
        End Select
        lvItem.Tag = pUpdate
        Dim sGroupName As String = ConvertIDToGroup(pUpdate)
        Dim sGroupKey As String = "lvgUnknown"
        If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = String.Format("lvg{0}", sGroupName.Replace(" ", "_"))
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
          sReleaseName = String.Format("{0} Multilingual User Interface Pack", pUpdate.Ident.Language)
        ElseIf pUpdate.UpdateInfo.ReleaseType = "Service Pack" Then
          sReleaseName = String.Format("{0} {1} (Build {2})", pUpdate.UpdateInfo.ReleaseType, vThisProd.Major, vThisProd.Minor)
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
            pName = String.Format("{0} for {1} ({2})", sReleaseName, sParentProd, sArticle)
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-Client-LanguagePack-Package" Then
          pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-CodecPack-Basic-Package" Then
          pName = String.Format("Basic Codec {0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-Foundation-Package" Then
          If pUpdate.UpdateInfo.FeatureList Is Nothing Then
            pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
          Else
            Dim sFeatureName As String = Nothing
            For Each sFeature In pUpdate.UpdateInfo.FeatureList
              If sFeature.StartsWith("Feature Name :") Then
                sFeatureName = sFeature.Substring(sFeature.IndexOf(" :") + 2).Trim
                Exit For
              End If
            Next
            If String.IsNullOrEmpty(sFeatureName) Then
              pName = String.Format("Feature {0} for {1}", sReleaseName, sParentProd)
            Else
              pName = String.Format("{0} Feature {1} for {2}", sFeatureName, sReleaseName, sParentProd)
            End If
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-IE-Troubleshooters-Package" Then
          pName = String.Format("Internet Explorer Troubleshooters {0} for {1}", sReleaseName, sParentProd)
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-InternetExplorer-Optional-Package" Then
          pName = String.Format("Optional {0} for {1}", sReleaseName, sParentProd)
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
            pName = String.Format("{0} {1} for {2}", sLocale, sReleaseName, sParentProd)
          Else
            pName = String.Format("{0} {1} Feature {2}", sFeatureName, sParentProd, sReleaseName)
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-PlatformUpdate-Win7-SRV08R2-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Platform Update for Windows 7 ({0})", sArticle)
          Else
            pName = "Platform Update for Windows 7"
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-RDP-WinIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Remote App and Desktop Connections Update ({0})", sArticle)
          Else
            pName = "Remote App and Desktop Connections Update"
          End If
        ElseIf pUpdate.UpdateInfo.ProductName = "Microsoft-Windows-RDP-BlueIP-Package-TopLevel" Then
          If sName.StartsWith("Update for Microsoft Windows (KB") Then
            Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
            sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
            pName = String.Format("Remote Desktop Protocol Update ({0})", sArticle)
          Else
            pName = "Remote Desktop Protocol Update"
          End If
        ElseIf sName.StartsWith("Update for Microsoft Windows (KB") Then
          Dim sArticle As String = sName.Substring(sName.IndexOf("KB"))
          sArticle = sArticle.Substring(0, sArticle.LastIndexOf(")"))
          pName = String.Format("{0} {1} for {2} ({3})", sDescription, sReleaseName, sParentProd, sArticle)
        Else
          If Not String.IsNullOrEmpty(sName) Then
            pName = sName
          ElseIf Not String.IsNullOrEmpty(sDescription) Then
            pName = sDescription
          Else
            pName = String.Format("{0} for {1}", sReleaseName, sParentProd)
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
              pName = String.Concat(pName, " (No Service Pack)")
            Else
              pName = String.Concat(pName, String.Format(" (Service Pack {0})", sSPLevel))
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
          lvItem.SubItems.Add(String.Format("{0}.{1}", vThisProd.Major, vThisProd.Minor))
        Else
          lvItem.SubItems.Add(pVer)
        End If
        Dim ttName As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ProductName) Then
          ttName = pUpdate.UpdateInfo.ProductName
          If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ProductVersion) Then ttName = String.Format("{0} v{1}", ttName, pUpdate.UpdateInfo.ProductVersion)
        End If
        Dim ttDescr As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Description) Then ttDescr = String.Concat(en, pUpdate.UpdateInfo.Description)
        Dim ttState As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.State) Then ttState = String.Concat(en, String.Format("State: {0}", pUpdate.UpdateInfo.State))
        Dim ttCreation As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.CreationTime) Then ttCreation = String.Concat(en, String.Format("Created: {0}", pUpdate.UpdateInfo.CreationTime))
        Dim ttInstalled As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.InstallTime) Then
          If String.IsNullOrEmpty(pUpdate.UpdateInfo.InstallClient) OrElse pUpdate.UpdateInfo.InstallClient = "DISM Package Manager Provider" Then
            ttInstalled = String.Concat(en, String.Format("Installed: {0}", pUpdate.UpdateInfo.InstallTime))
          Else
            ttInstalled = String.Concat(en, String.Format("Installed: {0} by {1}", pUpdate.UpdateInfo.InstallTime, pUpdate.UpdateInfo.InstallClient))
          End If
        End If
        Dim ttCompany As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Company) Then
          If String.IsNullOrEmpty(pUpdate.UpdateInfo.Copyright) OrElse pUpdate.UpdateInfo.Company.ToLower = pUpdate.UpdateInfo.Copyright.ToLower Then
            ttCompany = String.Concat(en, pUpdate.UpdateInfo.Company)
          Else
            ttCompany = String.Concat(en, String.Format("{0} ({1})", pUpdate.UpdateInfo.Company, pUpdate.UpdateInfo.Copyright))
          End If
        ElseIf Not String.IsNullOrEmpty(pUpdate.UpdateInfo.Copyright) Then
          ttCompany = String.Concat(en, pUpdate.UpdateInfo.Copyright)
        End If
        Dim ttArch As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Architecture) Then
          If String.IsNullOrEmpty(pUpdate.UpdateInfo.ReleaseType) Then
            ttArch = String.Concat(en, pUpdate.Ident.Architecture)
          Else
            ttArch = String.Concat(en, String.Format("{0} {1}", pUpdate.Ident.Architecture, pUpdate.UpdateInfo.ReleaseType))
          End If
        End If
        Dim ttLang As String = Nothing
        If Not String.IsNullOrEmpty(pUpdate.Ident.Language) AndAlso Not pUpdate.Ident.Language = "Neutral" Then ttLang = String.Concat(en, String.Format("Language: {0}", pUpdate.Ident.Language))
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
              ttCustom = String.Concat(ttCustom, en, String.Format("{0}: {1}", sPropKey, sPropVal), vbNewLine)
            End If
          Next
          If Not String.IsNullOrEmpty(ttCustom) Then ttCustom = ttCustom.TrimEnd
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
              ttFeature = String.Concat(ttFeature, en, String.Format("{0}: {1}", sFeatKey, sFeatVal), vbNewLine)
            End If
          Next
          If Not String.IsNullOrEmpty(ttFeature) Then ttFeature = ttFeature.TrimEnd
        End If
        Dim sFeatureTT As String = Nothing
        If Not String.IsNullOrEmpty(ttName) Then sFeatureTT = String.Concat(sFeatureTT, ttName, vbNewLine)
        If Not String.IsNullOrEmpty(ttDescr) Then sFeatureTT = String.Concat(sFeatureTT, ttDescr, vbNewLine)
        If Not String.IsNullOrEmpty(ttState) Then sFeatureTT = String.Concat(sFeatureTT, ttState, vbNewLine)
        If Not String.IsNullOrEmpty(ttFeature) Then sFeatureTT = String.Concat(sFeatureTT, ttFeature, vbNewLine)
        If Not String.IsNullOrEmpty(ttCreation) Then sFeatureTT = String.Concat(sFeatureTT, ttCreation, vbNewLine)
        If Not String.IsNullOrEmpty(ttInstalled) Then sFeatureTT = String.Concat(sFeatureTT, ttInstalled, vbNewLine)
        If Not String.IsNullOrEmpty(ttCompany) Then sFeatureTT = String.Concat(sFeatureTT, ttCompany, vbNewLine)
        If Not String.IsNullOrEmpty(ttArch) Then sFeatureTT = String.Concat(sFeatureTT, ttArch, vbNewLine)
        If Not String.IsNullOrEmpty(ttLang) Then sFeatureTT = String.Concat(sFeatureTT, ttLang, vbNewLine)
        If Not String.IsNullOrEmpty(ttCustom) Then sFeatureTT = String.Concat(sFeatureTT, ttCustom, vbNewLine)
        lvItem.ToolTipText = sFeatureTT.TrimEnd
        Select Case pUpdate.UpdateInfo.State
          Case "Installed", "Staged", "Enabled" : lvItem.ImageKey = "DID"
          Case "Install Pending", "Enable Pending" : lvItem.ImageKey = "DO"
          Case "Uninstall Pending", "Disable Pending" : lvItem.ImageKey = "UNDO"
          Case "Superseded" : lvItem.ImageKey = "PROBLEM"
          Case Else : lvItem.ImageKey = "NO"
        End Select
        lvItem.Tag = pUpdate
        Dim sGroupName As String = ConvertIDToGroup(pUpdate)
        Dim sGroupKey As String = "lvgUnknown"
        If Not String.IsNullOrEmpty(sGroupName) Then sGroupKey = String.Concat("lvg{0}", sGroupName.Replace(" ", "_"))
        lvUpdates.Items.Add(lvItem)
        lvItem.Group = lvUpdates.Groups(sGroupKey)
      End If
    Next
    lvUpdates.Sort()
    LoadingUpdates = False
  End Sub
  Private Function GetVersions(VerString As String) As Version()
    If Not VerString.Contains(".") Then Return {New Version(0, 0), New Version(0, 0)}
    Dim ver() As String = Split(VerString, ".", 4)
    If ver.Length = 4 Then
      Return {New Version(CInt(ver(0)), CInt(ver(1))), New Version(CInt(ver(2)), CInt(ver(3)))}
    ElseIf ver.Length = 3 Then
      Return {New Version(CInt(ver(0)), CInt(ver(1))), New Version(CInt(ver(2)), 0)}
    ElseIf ver.Length = 2 Then
      Return {New Version(CInt(ver(0)), CInt(ver(1))), New Version(0, 0)}
    Else
      Return {New Version(CInt(ver(0)), 0), New Version(0, 0)}
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
      Return String.Format("Internet Explorer {0}", OSVer.Major)
    End If
    Return String.Format("Windows {0}.{1}", OSVer.Major, OSVer.Minor)
  End Function
  Private Function ConvertIDToGroup(pUpdate As Update_Integrated) As String
    Dim sReleaseType As String = pUpdate.ReleaseType
    If Not String.IsNullOrEmpty(pUpdate.UpdateInfo.ReleaseType) Then sReleaseType = pUpdate.UpdateInfo.ReleaseType
    Dim sGroupName As String = "Unknown"
    If Not String.IsNullOrEmpty(sReleaseType) Then sGroupName = sReleaseType
    If sGroupName = "Security Update" Then
      If Not String.IsNullOrEmpty(pUpdate.Ident.Version) Then
        Try
          Dim vVal As New Version(pUpdate.Ident.Version)
          'TODO: Check the vVal version value for various updates to verify validity universally
          Stop
          If vVal.Major > 7 And vVal.Major < 12 Then
            sGroupName = String.Concat(sGroupName, " for Internet Explorer")
          Else
            sGroupName = String.Concat(sGroupName, " for Windows")
          End If
        Catch ex As Exception
          Debug.Print("VERSION: " & pUpdate.Ident.Version)
          Debug.Print(ex.Message)
          'TODO: Something doesn't like being a version!
          Stop
          If pUpdate.Ident.Version.Contains(".") Then
            Dim sMajor As String = pUpdate.Ident.Version.Substring(0, pUpdate.Ident.Version.IndexOf("."c))
            If Val(sMajor) > 7 And Val(sMajor) < 12 Then
              sGroupName = String.Concat(sGroupName, " for Internet Explorer")
            Else
              sGroupName = String.Concat(sGroupName, " for Windows")
            End If
          Else
            sGroupName = String.Concat(sGroupName, " for Windows")
          End If
        End Try
      End If
    End If
    Return sGroupName
  End Function
#Region "Resize Updates"
  Private Sub ResizeUpdates()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ResizeUpdates))
      Return
    End If
    Dim tUpdates As New Threading.Timer(New Threading.TimerCallback(AddressOf AsyncResizeUpdates), Nothing, 200, System.Threading.Timeout.Infinite)
  End Sub
  Private Sub AsyncResizeUpdates(state As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New Threading.TimerCallback(AddressOf AsyncResizeUpdates), state)
      Return
    End If
    If bLoading Then Return
    If lvUpdates.Columns.Count = 0 Then Return
    If lvUpdates.ClientSize.Width = 0 Then Return
    If lvUpdates.Items.Count > 0 Then
      lvUpdates.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
      If lvUpdates.Columns(1).Width < 90 Then lvUpdates.Columns(1).Width = 90
    Else
      lvUpdates.Columns(1).Width = 0
    End If
    Dim packageSize As Integer = lvUpdates.ClientSize.Width - lvUpdates.Columns(1).Width - 2
    If Not lvUpdates.Columns(0).Width = packageSize Then lvUpdates.Columns(0).Width = packageSize
  End Sub
#End Region
#End Region
#Region "Drivers"
  Private Sub DisplayDrivers()
    If lvDriverClass.Disposing Or lvDriverClass.IsDisposed Then Return
    If lvDriverClass.Items.Count > 0 Then Return
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
      If String.IsNullOrEmpty(cDriver.Value.BootCritical) Then
        lvGroupItem.ToolTipText = String.Concat(sGroupTitle, vbNewLine,
                                  en, String.Format("Class Name: {0}", cDriver.Value.ClassName), vbNewLine,
                                  en, String.Format("Class Description: {0}", cDriver.Value.ClassDescription), vbNewLine,
                                  en, String.Format("Class GUID: {0}", cDriver.Value.ClassGUID))
      Else
        lvGroupItem.ToolTipText = String.Concat(sGroupTitle, vbNewLine,
                                  en, String.Format("Class Name: {0}", cDriver.Value.ClassName), vbNewLine,
                                  en, String.Format("Class Description: {0}", cDriver.Value.ClassDescription), vbNewLine,
                                  en, String.Format("Class GUID: {0}", cDriver.Value.ClassGUID), vbNewLine,
                                  en, String.Format("Boot Critical: {0}", cDriver.Value.BootCritical))
      End If
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
    Return String.Format("lvi{0}", GUID.Replace("-", "").Replace("{", "").Replace("}", "").ToUpper)
  End Function
#Region "Resize Drivers"
  Private Sub ResizeDrivers()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf ResizeDrivers))
      Return
    End If
    Dim tDrivers As New Threading.Timer(New Threading.TimerCallback(AddressOf AsyncResizeDrivers), Nothing, 200, System.Threading.Timeout.Infinite)
  End Sub
  Private Sub AsyncResizeDrivers(state As Object)
    If Me.InvokeRequired Then
      Me.Invoke(New Threading.TimerCallback(AddressOf AsyncResizeDrivers), state)
      Return
    End If
    If bLoading Then Return
    If Not lvDriverClass.Columns.Count = 0 Then
      If Not lvDriverClass.ClientSize.Width = 0 Then
        lvDriverClass.Columns(0).Width = lvDriverClass.ClientSize.Width - 2
      End If
    End If
    If Not lvDriverProvider.Columns.Count = 0 Then
      If Not lvDriverProvider.ClientSize.Width = 0 Then
        lvDriverProvider.Columns(0).Width = lvDriverProvider.ClientSize.Width - 2
      End If
    End If
    If Not lvDriverINF.Columns.Count = 0 Then
      If Not lvDriverINF.ClientSize.Width = 0 Then
        If lvDriverINF.Items.Count = 0 Then
          lvDriverINF.Columns(1).Width = 0
        Else
          lvDriverINF.Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
          If lvDriverINF.Columns(1).Width > lvDriverINF.ClientSize.Width / 2 Then lvDriverINF.Columns(1).Width = 0
        End If
        lvDriverINF.Columns(0).Width = lvDriverINF.ClientSize.Width - lvDriverINF.Columns(1).Width - 2
      End If
    End If
  End Sub
#End Region
#End Region
#End Region
End Class
