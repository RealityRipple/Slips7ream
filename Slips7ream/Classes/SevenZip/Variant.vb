Imports System.Runtime.InteropServices
Imports Slips7ream.Extraction.COM
Namespace Extraction
  Friend Class [Variant]
    Implements IDisposable
    Private pv As New PropVariant()
    Friend Sub New(Archive As IInArchive, Id As ItemPropId)
      Archive.GetArchiveProperty(Id, pv)
    End Sub
    Friend Sub New(Archive As IInArchive, FileNumber As UInteger, Id As ItemPropId)
      Archive.GetProperty(FileNumber, Id, pv)
    End Sub
    Public ReadOnly Property VarType() As VarEnum
      Get
        Return pv.type
      End Get
    End Property
    Friend Function GetObject() As Object
      Select Case VarType
        Case VarEnum.VT_EMPTY
          Return Nothing
        Case VarEnum.VT_FILETIME
          Return DateTime.FromFileTime(pv.union.longValue)
        Case Else
          Dim PropHandle = GCHandle.Alloc(pv, GCHandleType.Pinned)
          Try
            Return Marshal.GetObjectForNativeVariant(PropHandle.AddrOfPinnedObject())
          Finally
            PropHandle.Free()
          End Try
      End Select
    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
      Select Case VarType
        Case VarEnum.VT_EMPTY
          Exit Select
        Case VarEnum.VT_NULL, VarEnum.VT_I2, VarEnum.VT_I4, VarEnum.VT_R4, VarEnum.VT_R8, VarEnum.VT_CY, _
         VarEnum.VT_DATE, VarEnum.VT_ERROR, VarEnum.VT_BOOL, VarEnum.VT_I1, VarEnum.VT_UI1, VarEnum.VT_UI2, _
         VarEnum.VT_UI4, VarEnum.VT_I8, VarEnum.VT_UI8, VarEnum.VT_INT, VarEnum.VT_UINT, VarEnum.VT_HRESULT, _
         VarEnum.VT_FILETIME
          pv.type = 0
          Exit Select
        Case Else
          SafeNativeMethods.PropVariantClear(pv)
          Exit Select
      End Select
    End Sub
    Public Function GetBool() As Boolean
      Dim o = GetObject()
      Return If(o Is Nothing, False, CType(o, [Boolean]))
    End Function
    Public Function GetString() As String
      Return TryCast(GetObject(), String)
    End Function
    Public Function GetUint() As UInteger
      Dim o = GetObject()
      Return If(o Is Nothing, 0, Convert.ToUInt32(o))
    End Function
    Public Function GetUlong() As ULong
      Dim o = GetObject()
      Return If(o Is Nothing, 0, Convert.ToUInt64(o))
    End Function
  End Class
End Namespace
