Imports System.IO

Public Class MNG
  Private Class MNGHeader
    ''' <summary>
    ''' The first 8 bytes of an MNG encoding
    ''' </summary>
    Shared expectedSignature As Byte() = {&H8A, &H4D, &H4E, &H47, &HD, &HA, _
     &H1A, &HA}
    ''' <summary>
    ''' The signature parsed from the input stream
    ''' </summary>
    Private signature As Byte()

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
      signature = Nothing
    End Sub

    ''' <summary>
    ''' Attempts to read an MNG Header chunk from the supplied stream.
    ''' </summary>
    ''' <param name="stream">The stream containing the MNG Header</param>
    Public Sub Read(stream As Stream)
      ' Stream must be readable
      If Not stream.CanRead Then
        Throw New ArgumentException("Stream is not readable")
      End If

      ' Read the signature
      Try
        signature = Utils.ReadStream(stream, 8)
      Catch generatedExceptionName As Exception
        ' Re-throw any exceptions
        Throw
      End Try

      ' Test signature for validity
      If signature.Length = expectedSignature.Length Then
        For i As Integer = 0 To expectedSignature.Length - 1
          ' Invalid signature
          If expectedSignature(i) <> signature(i) Then
            Throw New ApplicationException("MNG signature not found.")
          End If
        Next
      Else
        ' Invalid signature
        Throw New ApplicationException("MNG signature not found.")
      End If
    End Sub
  End Class

  Private Class Utils
    ''' <summary>
    ''' Attempts to read count bytes of data from the supplied stream.
    ''' </summary>
    ''' <param name="stream">The stream to read from</param>
    ''' <param name="count">The number of bytes to read</param>
    ''' <returns>A byte[] containing the data or null if an error occurred</returns>
    Public Shared Function ReadStream(stream As Stream, count As UInteger) As Byte()
      Dim bytes As Byte() = New Byte(count - 1) {}
      Try
        stream.Read(bytes, 0, CInt(count))
      Catch generatedExceptionName As IOException
        Throw
      End Try
      Return bytes
    End Function

    ''' <summary>
    ''' Attempts to parse an unsigned integer value from the array of bytes
    ''' provided.  The most significant byte of the unsigned integer is
    ''' parsed from the first element in the array.
    ''' </summary>
    ''' <param name="buffer">An array of bytes from which the value is to be extracted</param>
    ''' <param name="uintLengthInBytes">The number of bytes to parse (must be &lt;= sizeof(uint))</param>
    ''' <returns>The extracted unsigned integer returned in a uint</returns>
    Public Shared Function ParseUint(buffer As Byte(), uintLengthInBytes As Integer) As UInteger
      Dim offset As Integer = 0
      Return ParseUint(buffer, uintLengthInBytes, offset)
    End Function

    ''' <summary>
    ''' Attempts to parse an unsigned integer value from the array of bytes
    ''' provided.  The most significant byte of the unsigned integer is
    ''' parsed from the specified offset in the array.
    ''' </summary>
    ''' <param name="buffer">An array of bytes from which the value is to be extracted</param>
    ''' <param name="uintLengthInBytes">The number of bytes to parse (must be &lt;= sizeof(uint))</param>
    ''' <param name="offset">The offset in the array of bytes where parsing shall begin</param>
    ''' <returns>The extracted unsigned integer returned in a uint</returns>
    Public Shared Function ParseUint(buffer As Byte(), uintLengthInBytes As Integer, ByRef offset As Integer) As UInteger
      Dim value As UInteger = 0
      If uintLengthInBytes > 4 Then
        Throw New ArgumentException([String].Format("Function can only be used to parse up to {0} bytes from the buffer", 4))
      End If
      If buffer.Length - offset < uintLengthInBytes Then
        Throw New ArgumentException([String].Format("buffer is not long enough to extract {0} bytes at offset {1}", 4, offset))
      End If
      Dim i, j As Integer
      i = offset + uintLengthInBytes - 1
      j = 0
      While i >= offset
        value = value Or (CUInt(buffer(i)) << (8 * j))
        i -= 1
        j += 1
      End While
      offset += uintLengthInBytes
      Return value
    End Function
  End Class

  Private Class MENDChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "MEND"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class

  Private Class TERMChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "TERM"

    Private terminationAction As UInteger
    Private actionAfterTermination As UInteger
    Private delay As UInteger
    Private iterationMax As UInteger

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub

    ''' <summary>
    ''' Extracts various fields specific to this chunk from the MNG's
    ''' data field
    ''' </summary>
    ''' <param name="chunkData">An array of bytes representing the MNG's data field</param>
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      terminationAction = Utils.ParseUint(chunkData, 1, offset)
      ' If the data length is > 1 then read 9 more bytes
      If chunkData.Length > 1 Then
        actionAfterTermination = Utils.ParseUint(chunkData, 1, offset)
        delay = Utils.ParseUint(chunkData, 4, offset)
        iterationMax = Utils.ParseUint(chunkData, 4, offset)
      End If
    End Sub
  End Class

  Private Class BKGDChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "bKGD"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class

  Private Class BACKChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "BACK"

    Private redBackground As UInteger
    Private greenBackground As UInteger
    Private blueBackground As UInteger
    Private mandatoryBackground As UInteger
    Private backgroundImageId As UInteger
    Private backgroundTiling As UInteger

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub

    ''' <summary>
    ''' Extracts various fields specific to this chunk from the MNG's
    ''' data field
    ''' </summary>
    ''' <param name="chunkData">An array of bytes representing the MNG's data field</param>
    Protected Overrides Sub ParseData(chunkData As Byte())
      Dim offset As Integer = 0
      redBackground = Utils.ParseUint(chunkData, 2, offset)
      greenBackground = Utils.ParseUint(chunkData, 2, offset)
      blueBackground = Utils.ParseUint(chunkData, 2, offset)

      ' If the data length is > 6 then read 1 more byte
      If chunkData.Length > 6 Then
        mandatoryBackground = Utils.ParseUint(chunkData, 1, offset)
      End If
      ' If the data length is > 7 then read 2 more bytes
      If chunkData.Length > 7 Then
        backgroundImageId = Utils.ParseUint(chunkData, 2, offset)
      End If
      ' If the data length is > 9 then read 1 more byte
      If chunkData.Length > 9 Then
        backgroundTiling = Utils.ParseUint(chunkData, 1, offset)
      End If
    End Sub
  End Class

  Private Class IHDRChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "IHDR"

    Private width As UInteger
    Private height As UInteger
    Private bitDepth As UInteger
    Private colorType As UInteger
    Private compressionMethod As UInteger
    Private filterMethod As UInteger
    Private interlaceMethod As UInteger

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub

    ''' <summary>
    ''' Extracts various fields specific to this chunk from the MNG's
    ''' data field
    ''' </summary>
    ''' <param name="chunkData">An array of bytes representing the MNG's data field</param>
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
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "PLTE"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub

    Public Function IsEmpty() As Boolean
      Return ChunkLength = 0
    End Function

  End Class

  Private Class IENDChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "IEND"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class

  Private Class IDATChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "IDAT"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub
  End Class

  Private Class MHDRChunk
    Inherits MNGChunk
    ''' <summary>
    ''' The ASCII name of the MNG chunk
    ''' </summary>
    Public Const NAME As [String] = "MHDR"

    ''' <summary>
    ''' The MNG frame width in pixels
    ''' </summary>
    Private m_frameWidth As UInteger
    ''' <summary>
    ''' The MNG frame height in pixels
    ''' </summary>
    Private m_frameHeight As UInteger
    ''' <summary>
    ''' The MNG frame rate
    ''' </summary>
    Private ticksPerSecond As UInteger
    ''' <summary>
    ''' The MNG layer count
    ''' </summary>
    Private nominalLayerCount As UInteger
    ''' <summary>
    ''' The MNG frame count
    ''' </summary>
    Private nominalFrameCount As UInteger
    ''' <summary>
    ''' The MNG play time
    ''' </summary>
    Private nominalPlayTime As UInteger
    ''' <summary>
    ''' The MNG simplicity profile
    ''' </summary>
    Private simplicityProfile As UInteger

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="chunk">The MNG chunk containing the data for this specific chunk</param>
    Public Sub New(chunk As MNGChunk)
      MyBase.New(chunk, NAME)
    End Sub

    ''' <summary>
    ''' The MNG width in pixels
    ''' </summary>
    Public ReadOnly Property FrameWidth() As UInteger
      Get
        Return m_frameWidth
      End Get
    End Property

    ''' <summary>
    ''' The MNG height in pixels
    ''' </summary>
    Public ReadOnly Property FrameHeight() As UInteger
      Get
        Return m_frameHeight
      End Get
    End Property

    ''' <summary>
    ''' Extracts various fields specific to this chunk from the MNG's
    ''' data field
    ''' </summary>
    ''' <param name="chunkData">An array of bytes representing the MNG's data field</param>
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

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
      m_chunkLength = InlineAssignHelper(m_chunkType, InlineAssignHelper(m_chunkData, InlineAssignHelper(m_chunkCRC, Nothing)))
      [error] = "No Error"
    End Sub

    ''' <summary>
    ''' Constructor which takes an existing MNGChunk object and
    ''' verifies that its type matches that which is expected
    ''' </summary>
    ''' <param name="chunk">The MNGChunk to copy</param>
    ''' <param name="expectedType">The input MNGChunk expected type</param>
    Public Sub New(chunk As MNGChunk, expectedType As [String])
      ' Copy the existing chunks members
      m_chunkLength = chunk.m_chunkLength
      m_chunkType = chunk.m_chunkType
      m_chunkData = chunk.m_chunkData
      m_chunkCRC = chunk.m_chunkCRC

      ' Verify the chunk type is as expected
      If ChunkType <> expectedType Then
        Throw New ArgumentException([String].Format("Specified chunk type is not {0} as expected", expectedType))
      End If

      ' Parse the chunk's data
      ParseData(m_chunkData)
    End Sub

    ''' <summary>
    ''' Extracts various fields specific to this chunk from the MNG's
    ''' data field
    ''' </summary>
    ''' <param name="chunkData">An array of bytes representing the MNG's data field</param>
    Protected Overridable Sub ParseData(chunkData As Byte())
      ' Nothing specific to do here.  Derived classes can override this
      ' to do specific field parsing.
    End Sub

    ''' <summary>
    ''' Gets the array of bytes which make up the MNG chunk.  This includes:
    ''' o 4 bytes of the chunk's length
    ''' o 4 bytes of the chunk's type
    ''' o N bytes of the chunk's data
    ''' o 4 bytes of the chunk's CRC
    ''' </summary>
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

    ''' <summary>
    ''' Gets the array of bytes which make up the chunk's data field
    ''' </summary>
    Public ReadOnly Property ChunkData() As Byte()
      Get
        Return m_chunkData
      End Get
    End Property

    ''' <summary>
    ''' Gets chunk's type field as an string
    ''' </summary>
    Public ReadOnly Property ChunkType() As [String]
      Get
        Return New [String](New Char() {ChrW(m_chunkType(0)), ChrW(m_chunkType(1)), ChrW(m_chunkType(2)), ChrW(m_chunkType(3))})
      End Get
    End Property

    ''' <summary>
    ''' Gets the length field of the chunk
    ''' </summary>
    Public ReadOnly Property ChunkLength() As UInteger
      Get
        Return Utils.ParseUint(m_chunkLength, m_chunkLength.Length)
      End Get
    End Property

    ''' <summary>
    ''' Gets the CRC field of the chunk
    ''' </summary>
    Public ReadOnly Property ChunkCRC() As UInteger
      Get
        Return Utils.ParseUint(m_chunkCRC, m_chunkCRC.Length)
      End Get
    End Property

    ''' <summary>
    ''' Attempts to parse an MNGChunk for the specified stream
    ''' </summary>
    ''' <param name="stream">The stream containing the MNG Chunk</param>
    Public Sub Read(stream As Stream)
      If Not stream.CanRead Then
        Throw New ArgumentException("Stream is not readable")
      End If

      calculatedCRC = CRC.INITIAL_CRC

      Dim chunkStart As Long = stream.Position

      ' Read the data Length
      m_chunkLength = Utils.ReadStream(stream, 4)

      ' Read the chunk type
      m_chunkType = Utils.ReadStream(stream, 4)
      calculatedCRC = CRC.UpdateCRC(calculatedCRC, m_chunkType)

      ' Read the data
      m_chunkData = Utils.ReadStream(stream, ChunkLength)
      calculatedCRC = CRC.UpdateCRC(calculatedCRC, m_chunkData)

      ' Read the CRC
      m_chunkCRC = Utils.ReadStream(stream, 4)

      ' CRC is inverted
      calculatedCRC = Not calculatedCRC

      ' Verify the CRC
      If ChunkCRC <> calculatedCRC Then
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine([String].Format("MNG Chunk CRC Mismatch.  Chunk CRC = {0}, Calculated CRC = {1}.", ChunkCRC, calculatedCRC))
        sb.AppendLine([String].Format("This occurred while parsing the chunk at position {0} (0x{1:X8}) in the stream.", chunkStart, chunkStart))
        Throw New ApplicationException(sb.ToString())
      End If
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
      target = value
      Return value
    End Function
  End Class

  Private Class PNG
    ''' <summary>
    ''' The PNG file signature
    ''' </summary>
    Private Shared header As Byte() = New Byte() {&H89, &H50, &H4E, &H47, &HD, &HA, _
     &H1A, &HA}
    ''' <summary>
    ''' The PNG file's IHDR chunk
    ''' </summary>
    Private m_ihdr As IHDRChunk
    ''' <summary>
    ''' The PNG file's PLTE chunk (optional)
    ''' </summary>
    Private m_plte As PLTEChunk
    ''' <summary>
    ''' The PNG file's IDAT chunks
    ''' </summary>
    Private m_idats As List(Of IDATChunk)
    ''' <summary>
    ''' The PNG file's IEND chunk
    ''' </summary>
    Private m_iend As IENDChunk

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
      m_idats = New List(Of IDATChunk)()
    End Sub

    ''' <summary>
    ''' Converts the chunks making up the PNG into a single MemoryStream which
    ''' is suitable for writing to a PNG file or creating a Image object using
    ''' Bitmap.FromStream
    ''' </summary>
    ''' <returns>MemoryStream</returns>
    Public Function ToStream() As MemoryStream
      Dim ms As New MemoryStream()
      ms.Write(header, 0, header.Length)
      ms.Write(m_ihdr.Chunk, 0, m_ihdr.Chunk.Length)
      If m_plte IsNot Nothing Then
        ms.Write(m_plte.Chunk, 0, m_plte.Chunk.Length)
      End If
      For Each chunk As IDATChunk In m_idats
        ms.Write(chunk.Chunk, 0, chunk.Chunk.Length)
      Next
      ms.Write(m_iend.Chunk, 0, m_iend.Chunk.Length)
      Return ms
    End Function

    ''' <summary>
    ''' Gets or Sets the PNG's IHDR chunk
    ''' </summary>
    Public Property IHDR() As IHDRChunk
      Get
        Return m_ihdr
      End Get
      Set(value As IHDRChunk)
        m_ihdr = value
      End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the PNG's PLTE chunk
    ''' </summary>
    Public Property PLTE() As PLTEChunk
      Get
        Return m_plte
      End Get
      Set(value As PLTEChunk)
        m_plte = value
      End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the PNG's IEND chunk
    ''' </summary>
    Public Property IEND() As IENDChunk
      Get
        Return m_iend
      End Get
      Set(value As IENDChunk)
        m_iend = value
      End Set
    End Property

    ''' <summary>
    ''' Gets the list of IDAT chunk's making up the PNG
    ''' </summary>
    Public ReadOnly Property IDATS() As List(Of IDATChunk)
      Get
        Return m_idats
      End Get
    End Property

    ''' <summary>
    ''' Adds the assigned IDAT chunk to the end of the PNG's list of IDAT chunks
    ''' </summary>
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
      Dim c, n, k As UInteger
      For n = 0 To crcTable.Length - 1
        c = n
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
      Dim n As UInteger
      If crcTable Is Nothing Then
        MakeCRCTable()
      End If
      For n = 0 To bytes.Length - 1
        c = crcTable((c Xor bytes(n)) And &HFF) Xor (c >> 8)
      Next
      Return c
    End Function
    Public Shared Function Calculate(bytes As Byte()) As UInteger
      Return UpdateCRC(INITIAL_CRC, bytes)
    End Function
  End Class

  ''' <summary>
  ''' List of chunks in the MNG
  ''' </summary>
  Private chunks As List(Of MNGChunk)
  ''' <summary>
  ''' List of PNGs embedded in the MNG
  ''' </summary>
  Private pngs As List(Of PNG)
  ''' <summary>
  ''' The MNG's MHDRChunk
  ''' </summary>
  Private headerChunk As MHDRChunk

  ''' <summary>
  ''' Gets the number of embedded PNGs within the MNG
  ''' </summary>
  Public ReadOnly Property NumEmbeddedPNG() As Integer
    Get
      Return pngs.Count
    End Get
  End Property

  ''' <summary>
  ''' Creates a Bitmap object containing the embedded PNG at the specified
  ''' index in the MNG's list of embedded PNGs
  ''' </summary>
  ''' <param name="index">The embedded PNG index</param>
  ''' <returns>Bitmap</returns>
  Public Function ToBitmap(index As Integer) As Bitmap
    ' Verify the index
    If index > NumEmbeddedPNG Then
      Return Nothing
      'Throw New ArgumentException([String].Format("Embedded PNG index must be between 0 and {0}", NumEmbeddedPNG - 1))
    End If
    ' Create the bitmap
    Dim b As Bitmap = DirectCast(Bitmap.FromStream(pngs(index).ToStream()), Bitmap)
    Return b
  End Function

  ''' <summary>
  ''' Creates a string containing the names of all the chunks in the MNG
  ''' </summary>
  ''' <returns>String</returns>
  Public Overrides Function ToString() As String
    Dim sb As New System.Text.StringBuilder()
    For Each chunk As MNGChunk In chunks
      sb.AppendLine(chunk.ChunkType)
    Next
    Return sb.ToString()
  End Function

  ''' <summary>
  ''' Attempts to load an MNG from the specified file name
  ''' </summary>
  ''' <param name="filename">Name of the MNG file to load</param>
  Public Sub Load(filename As String)
    chunks = New List(Of MNGChunk)()
    pngs = New List(Of PNG)()

    ' Open the file for reading
    Dim stream As Stream = File.OpenRead(filename)

    ' Create a new header (should be 1 per file) and
    ' read it from the stream
    Dim header As New MNGHeader()
    Try
      header.Read(stream)
    Catch generatedExceptionName As Exception
      stream.Close()
      Throw
    End Try

    Dim chunk As MNGChunk
    Dim png As PNG = Nothing
    Dim globalPLTE As PLTEChunk = Nothing

    ' Read chunks from the stream until we reach the MEND chunk
    Do
      ' Read a generic Chunk
      chunk = New MNGChunk()
      Try
        chunk.Read(stream)
      Catch generatedExceptionName As Exception
        stream.Close()
        Throw
      End Try

      ' Take a look at the chunk type and decide what derived class to
      ' use to create a specific chunk
      Select Case chunk.ChunkType
        Case MHDRChunk.NAME
          If headerChunk IsNot Nothing Then
            Throw New ApplicationException([String].Format("Only one chunk of type {0} is allowed", chunk.ChunkType))
          End If
          chunk = InlineAssignHelper(headerChunk, New MHDRChunk(chunk))
          Exit Select
        Case MENDChunk.NAME
          chunk = New MENDChunk(chunk)
          Exit Select
        Case TERMChunk.NAME
          chunk = New TERMChunk(chunk)
          Exit Select
        Case BACKChunk.NAME
          chunk = New BACKChunk(chunk)
          Exit Select
        Case BKGDChunk.NAME
          chunk = New BKGDChunk(chunk)
          Exit Select
        Case PLTEChunk.NAME
          chunk = New PLTEChunk(chunk)
          ' We can get an PLTE chunk w/o having gotten
          ' an IHDR
          If png IsNot Nothing Then
            png.PLTE = TryCast(chunk, PLTEChunk)
            If png.PLTE.IsEmpty() Then
              If globalPLTE Is Nothing Then
                Throw New ApplicationException([String].Format("An empty PLTE chunk was found inside a IHDR/IEND pair but no 'global' PLTE chunk was found"))
              End If
              png.PLTE = globalPLTE
            End If
          Else
            globalPLTE = TryCast(chunk, PLTEChunk)
          End If
          Exit Select
        Case IHDRChunk.NAME
          chunk = New IHDRChunk(chunk)
          ' This is the beginning of a new embedded PNG
          png = New PNG()
          png.IHDR = TryCast(chunk, IHDRChunk)
          Exit Select
        Case IDATChunk.NAME
          chunk = New IDATChunk(chunk)
          ' We shouldn't get an IDAT chunk if we haven't yet
          ' gotten an IHDR chunk
          If png Is Nothing Then
            Throw New ArgumentNullException("png")
          End If
          png.IDAT = TryCast(chunk, IDATChunk)
          Exit Select
        Case IENDChunk.NAME
          chunk = New IENDChunk(chunk)
          ' We can get an IEND chunk w/o having gotten
          ' an IHDR
          If png IsNot Nothing Then
            ' However, if we've gotten an IHDR chunk then
            ' this is the end of the embedded PNG
            png.IEND = TryCast(chunk, IENDChunk)
            pngs.Add(png)
            png = Nothing
          End If
          Exit Select
        Case Else
          Exit Select
      End Select
      ' Add the chunk to our list of chunks
      chunks.Add(chunk)
    Loop While chunk.ChunkType <> MENDChunk.NAME
    stream.Close()
  End Sub
  Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
    target = value
    Return value
  End Function
End Class