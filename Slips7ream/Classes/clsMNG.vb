Imports System.IO
Public Class MNG
  Private Class MNGHeader
    Shared expectedSignature As Byte() = {&H8A, &H4D, &H4E, &H47, &HD, &HA, &H1A, &HA}
    Private signature As Byte()
    Public Sub New()
      signature = Nothing
    End Sub
    Public Function Read(stream As Stream) As Boolean
      If Not stream.CanRead Then Return False
      Try
        signature = Utils.ReadStream(stream, 8)
      Catch generatedExceptionName As Exception
        Return False
      End Try
      If Not signature.Length = expectedSignature.Length Then Return False
      For I As Integer = 0 To expectedSignature.Length - 1
        If Not expectedSignature(I) = signature(I) Then Return False
      Next
      Return True
    End Function
  End Class
  Private Class Utils
    Public Shared Function ReadStream(stream As Stream, count As UInteger) As Byte()
      If count > Integer.MaxValue Then Return Nothing
      Dim bytes As Byte() = New Byte(CInt(count - 1)) {}
      Try
        stream.Read(bytes, 0, CInt(count))
      Catch generatedExceptionName As IOException
        Throw
      End Try
      Return bytes
    End Function
    Public Shared Function ParseUint(buffer As Byte(), uintLengthInBytes As Integer) As UInteger
      Dim offset As Integer = 0
      Return ParseUint(buffer, uintLengthInBytes, offset)
    End Function
    Public Shared Function ParseUint(buffer As Byte(), uintLengthInBytes As Integer, ByRef offset As Integer) As UInteger
      Dim value As UInteger = 0
      If uintLengthInBytes > 4 Then Return 0
      If buffer.Length - offset < uintLengthInBytes Then Return 0
      Dim i, j As Integer
      i = offset + uintLengthInBytes - 1
      j = 0
      Do While i >= offset
        value = value Or (CUInt(buffer(i)) << (8 * j))
        i -= 1
        j += 1
      Loop
      offset += uintLengthInBytes
      Return value
    End Function
  End Class
  Private Class MENDChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "MEND"
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class
  Private Class TERMChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "TERM"
    Private terminationAction As UInteger
    Private actionAfterTermination As UInteger
    Private delay As UInteger
    Private iterationMax As UInteger
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      terminationAction = Utils.ParseUint(chunkData, 1, offset)
      If chunkData.Length > 1 Then
        actionAfterTermination = Utils.ParseUint(chunkData, 1, offset)
        delay = Utils.ParseUint(chunkData, 4, offset)
        iterationMax = Utils.ParseUint(chunkData, 4, offset)
      End If
    End Sub
  End Class
  Private Class BKGDChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "bKGD"
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class
  Private Class BACKChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "BACK"
    Private redBackground As UInteger
    Private greenBackground As UInteger
    Private blueBackground As UInteger
    Private mandatoryBackground As UInteger
    Private backgroundImageId As UInteger
    Private backgroundTiling As UInteger
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      redBackground = Utils.ParseUint(chunkData, 2, offset)
      greenBackground = Utils.ParseUint(chunkData, 2, offset)
      blueBackground = Utils.ParseUint(chunkData, 2, offset)
      If chunkData.Length > 6 Then mandatoryBackground = Utils.ParseUint(chunkData, 1, offset)
      If chunkData.Length > 7 Then backgroundImageId = Utils.ParseUint(chunkData, 2, offset)
      If chunkData.Length > 9 Then backgroundTiling = Utils.ParseUint(chunkData, 1, offset)
    End Sub
  End Class
  Private Class IHDRChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "IHDR"
    Private width As UInteger
    Private height As UInteger
    Private bitDepth As UInteger
    Private colorType As UInteger
    Private compressionMethod As UInteger
    Private filterMethod As UInteger
    Private interlaceMethod As UInteger
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      width = Utils.ParseUint(chunkData, 4, offset)
      height = Utils.ParseUint(chunkData, 4, offset)
      bitDepth = Utils.ParseUint(chunkData, 1, offset)
      colorType = Utils.ParseUint(chunkData, 1, offset)
      compressionMethod = Utils.ParseUint(chunkData, 1, offset)
      filterMethod = Utils.ParseUint(chunkData, 1, offset)
      interlaceMethod = Utils.ParseUint(chunkData, 1, offset)
    End Sub
  End Class
  Private Class PLTEChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "PLTE"
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
    Public Function IsEmpty() As Boolean
      Return ChunkLength = 0
    End Function
  End Class
  Private Class IENDChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "IEND"
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class
  Private Class IDATChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "IDAT"
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class
  Private Class MHDRChunk
    Inherits MNGChunk
    Public Const NAME As [String] = "MHDR"
    Private m_frameWidth As UInteger
    Private m_frameHeight As UInteger
    Private ticksPerSecond As UInteger
    Private nominalLayerCount As UInteger
    Private nominalFrameCount As UInteger
    Private nominalPlayTime As UInteger
    Private simplicityProfile As UInteger
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
    Public ReadOnly Property FrameWidth() As UInteger
      Get
        Return m_frameWidth
      End Get
    End Property
    Public ReadOnly Property FrameHeight() As UInteger
      Get
        Return m_frameHeight
      End Get
    End Property
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      m_frameWidth = Utils.ParseUint(chunkData, 4, offset)
      m_frameHeight = Utils.ParseUint(chunkData, 4, offset)
      ticksPerSecond = Utils.ParseUint(chunkData, 4, offset)
      nominalLayerCount = Utils.ParseUint(chunkData, 4, offset)
      nominalFrameCount = Utils.ParseUint(chunkData, 4, offset)
      nominalPlayTime = Utils.ParseUint(chunkData, 4, offset)
      simplicityProfile = Utils.ParseUint(chunkData, 4, offset)
    End Sub
  End Class
  Private Class MNGChunk
    Protected [error] As [String]
    Protected m_chunkLength As Byte()
    Protected m_chunkType As Byte()
    Protected m_chunkData As Byte()
    Protected m_chunkCRC As Byte()
    Protected calculatedCRC As UInteger
    Public Sub New()
      m_chunkLength = InlineAssignHelper(m_chunkType, InlineAssignHelper(m_chunkData, InlineAssignHelper(m_chunkCRC, Nothing)))
      [error] = "No Error"
    End Sub
    Public Sub New(chunk As MNGChunk, expectedType As [String])
      m_chunkLength = chunk.m_chunkLength
      m_chunkType = chunk.m_chunkType
      m_chunkData = chunk.m_chunkData
      m_chunkCRC = chunk.m_chunkCRC
      If Not ChunkType = expectedType Then
        [error] = String.Format("Chunk Type ({0}) did not match expected type ({1})", ChunkType, expectedType)
        Return
      End If
      ParseData(m_chunkData)
    End Sub
    Protected Overridable Sub ParseData(chunkData As Byte())
    End Sub
    Public ReadOnly Property Chunk() As Byte()
      Get
        Dim ba As Byte() = New Byte(m_chunkLength.Length + m_chunkType.Length + m_chunkData.Length + (m_chunkCRC.Length - 1)) {}
        m_chunkLength.CopyTo(ba, 0)
        m_chunkType.CopyTo(ba, m_chunkLength.Length)
        m_chunkData.CopyTo(ba, m_chunkLength.Length + m_chunkType.Length)
        m_chunkCRC.CopyTo(ba, m_chunkLength.Length + m_chunkType.Length + m_chunkData.Length)
        Return ba
      End Get
    End Property
    Public ReadOnly Property ChunkData() As Byte()
      Get
        Return m_chunkData
      End Get
    End Property
    Public ReadOnly Property ChunkType() As [String]
      Get
        Return New [String](New Char() {ChrW(m_chunkType(0)), ChrW(m_chunkType(1)), ChrW(m_chunkType(2)), ChrW(m_chunkType(3))})
      End Get
    End Property
    Public ReadOnly Property ChunkLength() As UInteger
      Get
        Return Utils.ParseUint(m_chunkLength, m_chunkLength.Length)
      End Get
    End Property
    Public ReadOnly Property ChunkCRC() As UInteger
      Get
        Return Utils.ParseUint(m_chunkCRC, m_chunkCRC.Length)
      End Get
    End Property
    Public Function Read(stream As Stream) As Boolean
      If Not stream.CanRead Then Return False
      calculatedCRC = CRC.INITIAL_CRC
      Dim chunkStart As Long = stream.Position
      m_chunkLength = Utils.ReadStream(stream, 4)
      m_chunkType = Utils.ReadStream(stream, 4)
      calculatedCRC = CRC.UpdateCRC(calculatedCRC, m_chunkType)
      m_chunkData = Utils.ReadStream(stream, ChunkLength)
      calculatedCRC = CRC.UpdateCRC(calculatedCRC, m_chunkData)
      m_chunkCRC = Utils.ReadStream(stream, 4)
      calculatedCRC = Not calculatedCRC
      If Not ChunkCRC = calculatedCRC Then Return False
      Return True
    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
      target = value
      Return value
    End Function
  End Class
  Private Class PNG
    Private Shared header As Byte() = New Byte() {&H89, &H50, &H4E, &H47, &HD, &HA, &H1A, &HA}
    Private m_ihdr As IHDRChunk
    Private m_plte As PLTEChunk
    Private m_idats As List(Of IDATChunk)
    Private m_iend As IENDChunk
    Public Sub New()
      m_idats = New List(Of IDATChunk)()
    End Sub
    Public Function ToStream() As MemoryStream
      Dim ms As New MemoryStream()
      ms.Write(header, 0, header.Length)
      ms.Write(m_ihdr.Chunk, 0, m_ihdr.Chunk.Length)
      If m_plte IsNot Nothing Then ms.Write(m_plte.Chunk, 0, m_plte.Chunk.Length)
      For Each chunk As IDATChunk In m_idats
        ms.Write(chunk.Chunk, 0, chunk.Chunk.Length)
      Next
      ms.Write(m_iend.Chunk, 0, m_iend.Chunk.Length)
      Return ms
    End Function
    Public Property IHDR() As IHDRChunk
      Get
        Return m_ihdr
      End Get
      Set(value As IHDRChunk)
        m_ihdr = value
      End Set
    End Property
    Public Property PLTE() As PLTEChunk
      Get
        Return m_plte
      End Get
      Set(value As PLTEChunk)
        m_plte = value
      End Set
    End Property
    Public Property IEND() As IENDChunk
      Get
        Return m_iend
      End Get
      Set(value As IENDChunk)
        m_iend = value
      End Set
    End Property
    Public ReadOnly Property IDATS() As List(Of IDATChunk)
      Get
        Return m_idats
      End Get
    End Property
    Public WriteOnly Property IDAT() As IDATChunk
      Set(value As IDATChunk)
        m_idats.Add(value)
      End Set
    End Property
  End Class
  Private Class CRC
    Private Shared crcTable As UInteger()
    Public Const INITIAL_CRC As UInteger = &HFFFFFFFFUI
    Private Shared Sub MakeCRCTable()
      crcTable = New UInteger(255) {}
      Dim c, k As UInteger
      For n As Integer = 0 To crcTable.Length - 1
        c = CUInt(n)
        For k = 0 To 7
          If (c And 1) = 0 Then
            c = c >> 1
          Else
            c = &HEDB88320UI Xor (c >> 1)
          End If
        Next
        crcTable(n) = c
      Next
    End Sub
    Public Shared Function UpdateCRC(crc As UInteger, bytes As Byte()) As UInteger
      If bytes.Length = 0 Then Return crc
      Dim c As UInteger = crc
      If crcTable Is Nothing Then MakeCRCTable()
      For n As Integer = 0 To bytes.Length - 1
        c = crcTable(CByte((c Xor bytes(n)) And &HFF)) Xor (c >> 8)
      Next
      Return c
    End Function
    Public Shared Function Calculate(bytes As Byte()) As UInteger
      Return UpdateCRC(INITIAL_CRC, bytes)
    End Function
  End Class
  Private chunks As List(Of MNGChunk)
  Private pngs As List(Of PNG)
  Private headerChunk As MHDRChunk
  Public ReadOnly Property NumEmbeddedPNG() As Integer
    Get
      Return pngs.Count
    End Get
  End Property
  Public Function ToBitmap(index As Integer) As Bitmap
    If index > NumEmbeddedPNG Then Return Nothing
    Try
      Dim b As Bitmap = DirectCast(Bitmap.FromStream(pngs(index).ToStream()), Bitmap)
      Return b
    Catch ex As Exception
      Return Nothing
    End Try
  End Function
  Public Overrides Function ToString() As String
    Dim sb As New System.Text.StringBuilder()
    For Each chunk As MNGChunk In chunks
      sb.AppendLine(chunk.ChunkType)
    Next
    Return sb.ToString()
  End Function
  Public Function Load(filename As String) As Boolean
    chunks = New List(Of MNGChunk)()
    pngs = New List(Of PNG)()
    Dim stream As Stream = File.OpenRead(filename)
    Dim header As New MNGHeader()
    If Not header.Read(stream) Then
      stream.Close()
      Return False
    End If
    Dim chunk As MNGChunk
    Dim png As PNG = Nothing
    Dim globalPLTE As PLTEChunk = Nothing
    Do
      chunk = New MNGChunk()
      If Not chunk.Read(stream) Then
        stream.Close()
        Return False
      End If
      Select Case chunk.ChunkType
        Case MHDRChunk.NAME
          If headerChunk IsNot Nothing Then Return False
          chunk = InlineAssignHelper(headerChunk, New MHDRChunk(chunk))
        Case MENDChunk.NAME
          chunk = New MENDChunk(chunk)
        Case TERMChunk.NAME
          chunk = New TERMChunk(chunk)
        Case BACKChunk.NAME
          chunk = New BACKChunk(chunk)
        Case BKGDChunk.NAME
          chunk = New BKGDChunk(chunk)
        Case PLTEChunk.NAME
          chunk = New PLTEChunk(chunk)
          If png IsNot Nothing Then
            globalPLTE = TryCast(chunk, PLTEChunk)
          Else
            png.PLTE = TryCast(chunk, PLTEChunk)
            If png.PLTE.IsEmpty() Then
              If globalPLTE Is Nothing Then Return False
              png.PLTE = globalPLTE
            End If
          End If
        Case IHDRChunk.NAME
          chunk = New IHDRChunk(chunk)
          png = New PNG()
          png.IHDR = TryCast(chunk, IHDRChunk)
        Case IDATChunk.NAME
          chunk = New IDATChunk(chunk)
          If png Is Nothing Then Return False
          png.IDAT = TryCast(chunk, IDATChunk)
        Case IENDChunk.NAME
          chunk = New IENDChunk(chunk)
          If png IsNot Nothing Then
            png.IEND = TryCast(chunk, IENDChunk)
            pngs.Add(png)
            png = Nothing
          End If
      End Select
      chunks.Add(chunk)
    Loop Until chunk.ChunkType = MENDChunk.NAME
    stream.Close()
    Return True
  End Function
  Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
    target = value
    Return value
  End Function
End Class