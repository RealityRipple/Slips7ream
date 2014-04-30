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
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.DialogResult = Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    Me.DialogResult = Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub
End Class