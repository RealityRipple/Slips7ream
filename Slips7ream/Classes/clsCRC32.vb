Public Class CRC32
  Private crc32Table(256) As UInteger
  Public Sub New(Optional seed As UInteger = &H69726F4AUI)
    Dim dwPolynomial As UInteger = seed
    For I As UInteger = 0 To 255
      Dim dwCrc As UInteger = I
      For J As Integer = 8 To 1 Step -1
        If (dwCrc And 1) = 1 Then
          dwCrc = ((dwCrc And &HFFFFFFFEUI) \ 2UI) And &H7FFFFFFFUI
          dwCrc = dwCrc Xor dwPolynomial
        Else
          dwCrc = ((dwCrc And &HFFFFFFFEUI) \ 2UI) And &H7FFFFFFFUI
        End If
      Next J
      crc32Table(CInt(I)) = dwCrc
    Next I
  End Sub
  Public Function GetByteArrayCrc32(buffer() As Byte) As UInteger
    Dim iLookup As UInteger
    Dim crc32Result As UInteger = &HFFFFFFFFUI
    For I As Integer = LBound(buffer) To UBound(buffer)
      iLookup = CUInt((crc32Result And &HFF) Xor buffer(I))
      crc32Result = CUInt(((crc32Result And &HFFFFFF00UI) \ &H100) And &HFFFFFFUI)
      crc32Result = crc32Result Xor crc32Table(CInt(iLookup))
    Next I
    Return Not (crc32Result)
  End Function
End Class
