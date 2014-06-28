Imports System.Runtime.InteropServices
Namespace Extraction.COM
  <StructLayout(LayoutKind.Explicit, Size:=16)>
  Friend Structure PropVariant
    <FieldOffset(0)>
    <MarshalAs(UnmanagedType.U4)>
    Public type As VarEnum
    <FieldOffset(8)>
    Friend union As PropVariantUnion
  End Structure
  <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable"), StructLayout(LayoutKind.Explicit)> _
  Friend Structure PropVariantUnion
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    <FieldOffset(0)> _
    Public byteValue As Byte
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    <FieldOffset(0)> _
    Public pointerValue As IntPtr
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    <FieldOffset(0)> _
    Public bstrValue As IntPtr
    <FieldOffset(0)> _
    Public longValue As Long
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    <FieldOffset(0)> _
    Public filetime As System.Runtime.InteropServices.ComTypes.FILETIME
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    <FieldOffset(0), MarshalAs(UnmanagedType.U8)> _
    Public ui8Value As ULong
  End Structure
End Namespace