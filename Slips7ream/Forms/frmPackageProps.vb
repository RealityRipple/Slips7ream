Public Class frmPackageProps
  Public Sub New()
    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
  End Sub
  Friend Sub New(Data As PackageInfoEx)
    InitializeComponent()
    txtIndex.Text = Data.Index
    txtName.Text = Data.Name
    txtDesc.Text = Data.Desc
    txtSize.Text = ByteSize(Data.Size)
    txtArchitecture.Text = Data.Architecture
    txtHAL.Text = Data.HAL
    txtVersion.Text = Data.Version
    txtSPBuild.Text = Data.SPBuild
    txtSPLevel.Text = Data.SPLevel
    txtEdition.Text = Data.Edition
    txtInstallation.Text = Data.Installation
    txtProductType.Text = Data.ProductType
    txtProductSuite.Text = Data.ProductSuite
    txtSystemRoot.Text = Data.SystemRoot
    txtFiles.Text = Data.Files
    txtDirectories.Text = Data.Directories
    txtCreated.Text = Data.Created
    txtModified.Text = Data.Modified
    txtLanguages.Text = Join(Data.LangList, vbNewLine)
    If Data.PackageList IsNot Nothing AndAlso Data.PackageList.Count > 0 Then
      lblUpdates.Visible = True
      lvUpdates.Visible = True
      pnlProps.ColumnStyles(2).Width = 350
      lvUpdates.Items.Clear()
      For Each pUpdate As PackageItem In Data.PackageList
        Dim lvItem As New ListViewItem(pUpdate.Ident.Name)
        lvItem.SubItems.Add(pUpdate.Ident.Version)
        lvItem.ToolTipText = pUpdate.Ident.Name & vbNewLine &
                             "Type: " & pUpdate.ReleaseType & vbNewLine &
                             "Installed: " & pUpdate.InstallTime & vbNewLine &
                             "Architecture: " & pUpdate.Ident.Architecture & vbNewLine &
                             "Language: " & pUpdate.Ident.Language & vbNewLine &
                             "Installation State: " & pUpdate.State
        Select Case pUpdate.State
          Case "Installed", "Staged" : lvItem.ImageKey = "DID"
          Case "Install Pending" : lvItem.ImageKey = "DO"
          Case "Uninstall Pending", "Superseded" : lvItem.ImageKey = "UNDO"
          Case Else : lvItem.ImageKey = "NO"
        End Select
        lvUpdates.Items.Add(lvItem)
      Next
    Else
      lblUpdates.Visible = False
      lvUpdates.Visible = False
      pnlProps.ColumnStyles(2).Width = 0
      Me.Width = 280
    End If
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.DialogResult = Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    Me.DialogResult = Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub
  Private Sub frmPackageProps_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    If Not lvUpdates.Columns.Count = 0 Then
      lvUpdates.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
      Dim packageSize As Integer = lvUpdates.ClientSize.Width - lvUpdates.Columns(1).Width - 1
      If Not lvUpdates.Columns(0).Width = packageSize Then lvUpdates.Columns(0).Width = packageSize
    End If
  End Sub
  Private Sub frmPackageProps_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    If Not lvUpdates.Columns.Count = 0 Then
      lvUpdates.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)
      Dim packageSize As Integer = lvUpdates.ClientSize.Width - lvUpdates.Columns(1).Width - 1
      If Not lvUpdates.Columns(0).Width = packageSize Then lvUpdates.Columns(0).Width = packageSize
    End If
  End Sub
End Class
