Imports System.Runtime.InteropServices
Imports System.Text
Public Class clsExplorer
  <DllImport("shell32.dll", exactspelling:=True)>
  Private Shared Function SHOpenFolderAndSelectItems(pidlFolder As IntPtr, cidl As UInteger, <[In](), MarshalAs(UnmanagedType.LPArray)> apidl As IntPtr(), dwFlags As UInteger) As Integer
  End Function
  <DllImport("shell32.dll")>
  Private Shared Function ILCreateFromPath(<MarshalAs(UnmanagedType.LPTStr)> pszPath As String) As IntPtr
  End Function
  Public Shared Sub OpenFolderAndSelectFiles(folder As String, ParamArray filesToSelect() As String)
    Dim dir As IntPtr = ILCreateFromPath(folder)
    Dim filesToSelectIntPtrs = New IntPtr(filesToSelect.Length - 1) {}
    For I As Integer = 0 To filesToSelect.Length - 1
      filesToSelectIntPtrs(I) = ILCreateFromPath(filesToSelect(I))
    Next
    SHOpenFolderAndSelectItems(dir, CUInt(filesToSelect.Length), filesToSelectIntPtrs, 0)
    ReleaseComObject(dir)
    ReleaseComObject(filesToSelectIntPtrs)
  End Sub
  Private Shared Sub ReleaseComObject(ParamArray comObjs As Object())
    For Each obj As Object In comObjs
      If obj IsNot Nothing AndAlso Marshal.IsComObject(obj) Then
        Marshal.ReleaseComObject(obj)
      End If
    Next
  End Sub
End Class
