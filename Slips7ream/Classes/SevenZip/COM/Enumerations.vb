Namespace Extraction.COM
  Friend Enum ArchivePropId As UInteger
    Name = 0
    ClassID = 1
    Extension = 2
    AddExtension = 3
    Update = 4
    KeepName = 5
    StartSignature = 6
    FinishSignature = 7
    Associate = 8
  End Enum
  Friend Enum ExtractMode As Integer
    Extract = 0
    Test = 1
    Skip = 2
  End Enum
  Friend Enum ItemPropId As UInteger
    NoProperty = 0
    HandlerItemIndex = 2
    Path = 3
    Name = 4
    Extension = 5
    IsDir = 6
    Size = 7
    PackedSize = 8
    Attrib = 9
    CreationTime = 10
    AccessTime = 11
    ModificationTime = 12
    Solid = 13
    Commented = 14
    Encrypted = 15
    SplitBefore = 16
    SplitAfter = 17
    DictionarySize = 18
    CRC = 19
    Type = 20
    IsAnti = 21
    Method = 22
    HostOS = 23
    FileSystem = 24
    User = 25
    Group = 26
    Block = 27
    Comment = 28
    Position = 29
    Prefix = 30
    NumSubDirs = 31
    NumSubFiles = 32
    UnpackVer = 33
    Volume = 34
    IsVolume = 35
    Offset = 36
    Links = 37
    NumBlocks = 38
    NumVolumes = 39
    TimeType = 40
    Bit64 = 41
    BigEndian = 42
    Cpu = 43
    PhySize = 44
    HeadersSize = 45
    Checksum = 46
    Characts = 47
    Va = 48
    TotalSize = &H1100
    FreeSpace = &H1101
    ClusterSize = &H1102
    VolumeName = &H1103
    LocalName = &H1200
    Provider = &H1201
    UserDefined = &H10000
  End Enum
  Friend Enum OperationResult As Integer
    OK = 0
    UnSupportedMethod = 1
    DataError = 2
    CRCError = 3
  End Enum
  Public Enum ExtractionStage
    Done
    Extracting
  End Enum
End Namespace
