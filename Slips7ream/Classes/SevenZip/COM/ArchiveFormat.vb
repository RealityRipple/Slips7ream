Namespace Extraction.COM
  Public Class ArchiveFormat
    Public Enum KnownSevenZipFormat
      SevenZip
      Arj
      BZip2
      Cab
      Chm
      Compound
      Cpio
      Deb
      GZip
      Iso
      Lzh
      Lzma
      Nsis
      Rar
      Rpm
      Split
      Tar
      Wim
      Lzw
      Udf
      Xar
      Mub
      Hfs
      Dmg
      XZ
      Mslz
      PE
      Elf
      Swf
      Vhd
      Z
      Zip
      Unknown
    End Enum
    Private Shared FFormatClassMap As Dictionary(Of KnownSevenZipFormat, Guid)
    Private Shared ReadOnly Property FormatClassMap() As Dictionary(Of KnownSevenZipFormat, Guid)
      Get
        If FFormatClassMap Is Nothing Then
          FFormatClassMap = New Dictionary(Of KnownSevenZipFormat, Guid)()
          FFormatClassMap.Add(KnownSevenZipFormat.SevenZip, New Guid("23170f69-40c1-278a-1000-000110070000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Arj, New Guid("23170f69-40c1-278a-1000-000110040000"))
          FFormatClassMap.Add(KnownSevenZipFormat.BZip2, New Guid("23170f69-40c1-278a-1000-000110020000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Cab, New Guid("23170f69-40c1-278a-1000-000110080000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Chm, New Guid("23170f69-40c1-278a-1000-000110e90000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Compound, New Guid("23170f69-40c1-278a-1000-000110e50000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Cpio, New Guid("23170f69-40c1-278a-1000-000110ed0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Deb, New Guid("23170f69-40c1-278a-1000-000110ec0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.GZip, New Guid("23170f69-40c1-278a-1000-000110ef0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Iso, New Guid("23170f69-40c1-278a-1000-000110e70000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Lzh, New Guid("23170f69-40c1-278a-1000-000110060000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Lzma, New Guid("23170f69-40c1-278a-1000-0001100a0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Nsis, New Guid("23170f69-40c1-278a-1000-000110090000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Rar, New Guid("23170f69-40c1-278a-1000-000110030000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Rpm, New Guid("23170f69-40c1-278a-1000-000110eb0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Split, New Guid("23170f69-40c1-278a-1000-000110ea0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Tar, New Guid("23170f69-40c1-278a-1000-000110ee0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Wim, New Guid("23170f69-40c1-278a-1000-000110e60000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Lzw, New Guid("23170f69-40c1-278a-1000-000110050000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Zip, New Guid("23170f69-40c1-278a-1000-000110010000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Udf, New Guid("23170f69-40c1-278a-1000-000110E00000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Xar, New Guid("23170f69-40c1-278a-1000-000110E10000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Mub, New Guid("23170f69-40c1-278a-1000-000110E20000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Hfs, New Guid("23170f69-40c1-278a-1000-000110E30000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Dmg, New Guid("23170f69-40c1-278a-1000-000110E40000"))
          FFormatClassMap.Add(KnownSevenZipFormat.XZ, New Guid("23170f69-40c1-278a-1000-0001100C0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Mslz, New Guid("23170f69-40c1-278a-1000-000110D50000"))
          FFormatClassMap.Add(KnownSevenZipFormat.PE, New Guid("23170f69-40c1-278a-1000-000110DD0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Elf, New Guid("23170f69-40c1-278a-1000-000110DE0000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Swf, New Guid("23170f69-40c1-278a-1000-000110D70000"))
          FFormatClassMap.Add(KnownSevenZipFormat.Vhd, New Guid("23170f69-40c1-278a-1000-000110DC0000"))
        End If
        Return FFormatClassMap
      End Get
    End Property
    Public Shared Function GetClassIdFromKnownFormat(format As KnownSevenZipFormat) As Guid
      Dim Result As Guid
      If FormatClassMap.TryGetValue(format, Result) Then Return Result
      Return Guid.Empty
    End Function
  End Class
End Namespace
