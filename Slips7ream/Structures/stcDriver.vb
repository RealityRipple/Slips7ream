Public Structure Driver
  Public PublishedName As String
  Public OriginalFileName As String
  Public DriverStorePath As String
  Public Remove As Boolean
  Public Inbox As String
  Public ClassName As String
  Public ClassDescription As String
  Public ClassGUID As String
  Public ClassIcon As Icon
  Public ProviderName As String
  Public ProviderIcon As Icon
  Public [Date] As String
  Public Version As String
  Public BootCritical As String
  Public DriverIcon As Icon
  Public Architectures As List(Of String)
  Public DriverHardware As Collections.Generic.Dictionary(Of String, List(Of Driver_Hardware))
  Public Sub New(RowData As String)
    If Not RowData.Contains("|") Then Return
    Dim DriverData() As String = Split(RowData, "|")
    If Not DriverData.Length = 7 Then Return
    PublishedName = Trim(DriverData(0))
    OriginalFileName = Trim(DriverData(1))
    Inbox = Trim(DriverData(2))
    ClassName = Trim(DriverData(3))
    ProviderName = Trim(DriverData(4))
    [Date] = Trim(DriverData(5))
    Version = Trim(DriverData(6))
    Remove = False
  End Sub
  Public Sub ReadExtraData(ExtraData As String, TrimStorePath As String, arch As ArchitectureList)
    Dim dataLines() As String = Split(ExtraData, vbNewLine)
    For I As Integer = 0 To dataLines.Length - 1
      Dim line As String = dataLines(I)
      If line.StartsWith("Published Name :") And String.IsNullOrEmpty(PublishedName) Then
        PublishedName = GetVal(line)
      ElseIf line.StartsWith("Driver Store Path :") Then
        DriverStorePath = GetVal(line)
        If Not String.IsNullOrEmpty(DriverStorePath) Then
          If IO.File.Exists(DriverStorePath) Then DriverIcon = GetDriverIcon(DriverStorePath, arch)
          If Not String.IsNullOrEmpty(ClassGUID) Then
            ClassIcon = GetDriverClassIcon(ClassGUID, DriverStorePath, arch)
            If DriverIcon Is Nothing Then DriverIcon = ClassIcon
          End If
          If DriverStorePath.StartsWith(TrimStorePath) Then DriverStorePath = DriverStorePath.Substring(TrimStorePath.Length)
        End If
      ElseIf line.StartsWith("Class Name :") And String.IsNullOrEmpty(ClassName) Then
        ClassName = GetVal(line)
      ElseIf line.StartsWith("Class Description :") Then
        ClassDescription = GetVal(line)
      ElseIf line.StartsWith("Class GUID :") Then
        ClassGUID = GetVal(line)
        If Not String.IsNullOrEmpty(ClassGUID) And Not String.IsNullOrEmpty(DriverStorePath) Then
          ClassIcon = GetDriverClassIcon(ClassGUID, IO.Path.Combine(TrimStorePath, DriverStorePath), arch)
          If DriverIcon Is Nothing Then DriverIcon = ClassIcon
        End If
      ElseIf line.StartsWith("Date :") And String.IsNullOrEmpty([Date]) Then
        [Date] = GetVal(line)
      ElseIf line.StartsWith("Version :") And String.IsNullOrEmpty(Version) Then
        Version = GetVal(line)
      ElseIf line.StartsWith("Boot Critical :") Then
        BootCritical = GetVal(line)
      ElseIf line.StartsWith("Drivers for architecture :") Then
        Dim sArch As String = GetVal(line)
        If Architectures Is Nothing Then
          Architectures = New List(Of String)
        End If
        If Not Architectures.Contains(sArch) Then Architectures.Add(sArch)
        Dim sDriverInfos As New List(Of String)
        Dim sDriverInfoChunk As String = Nothing
        Dim J As Integer = I + 1
        Do
          J += 1
          If dataLines.Length - 1 < J Then
            If Not String.IsNullOrEmpty(sDriverInfoChunk) Then
              sDriverInfos.Add(sDriverInfoChunk)
              sDriverInfoChunk = Nothing
            End If
            Exit Do
          End If
          Dim sLine As String = Trim(dataLines(J))
          If String.IsNullOrEmpty(sLine) Then
            If Not String.IsNullOrEmpty(sDriverInfoChunk) Then
              sDriverInfos.Add(sDriverInfoChunk)
              sDriverInfoChunk = Nothing
            End If
          ElseIf sLine.StartsWith("Drivers for architecture :") Or sLine.StartsWith("The operation completed successfully.") Then
            If Not String.IsNullOrEmpty(sDriverInfoChunk) Then
              sDriverInfos.Add(sDriverInfoChunk)
              sDriverInfoChunk = Nothing
            End If
            Exit Do
          Else
            sLine = sLine.Replace(vbTab, "")
            sDriverInfoChunk &= sLine & vbNewLine
          End If
        Loop
        Dim DriverList As New List(Of Driver_Hardware)
        For Each sDriverInfo In sDriverInfos
          Dim driverItem As New Driver_HardwareA(sDriverInfo)
          Dim appended As Boolean = False
          For Each driver As Driver_Hardware In DriverList
            If driverItem = driver Then
              If driver.HardwareIDs.ContainsKey(driverItem.HardwareID) Then
                If driverItem.CompatibleIDs IsNot Nothing AndAlso driverItem.CompatibleIDs.Count > 0 Then driver.HardwareIDs(driverItem.HardwareID).CompatibleIDs.AddRange(driverItem.CompatibleIDs)
                If driverItem.ExcludeIDs IsNot Nothing AndAlso driverItem.ExcludeIDs.Count > 0 Then driver.HardwareIDs(driverItem.HardwareID).ExcludeIDs.AddRange(driverItem.ExcludeIDs)
              Else
                driver.HardwareIDs.Add(driverItem.HardwareID, New Driver_Hardware_IDLists(driverItem.CompatibleIDs, driverItem.ExcludeIDs))
              End If
              appended = True
              Exit For
            End If
          Next
          If Not appended Then DriverList.Add(New Driver_Hardware(driverItem))
        Next
        If DriverHardware Is Nothing Then DriverHardware = New Collections.Generic.Dictionary(Of String, List(Of Driver_Hardware))
        DriverHardware.Add(sArch, DriverList)
      End If
    Next
    If DriverIcon Is Nothing Then DriverIcon = My.Resources.inf
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Return line.Substring(line.IndexOf(" : ") + 3)
    Else
      Return Nothing
    End If
  End Function
  Public Shared Operator =(info1 As Driver, info2 As Driver) As Boolean
    If Not info1.ClassGUID.ToLower = info2.ClassGUID.ToLower Then Return False
    Dim archMatches As New List(Of String)
    For Each arch1 As String In info1.Architectures
      For Each arch2 As String In info2.Architectures
        If arch1.ToLower = arch2.ToLower Then archMatches.Add(arch1.ToLower)
      Next
    Next
    If archMatches.Count = 0 Then Return False
    For Each Architecture In archMatches
      Dim hw1 As List(Of Driver_Hardware) = Nothing
      For Each hw In info1.DriverHardware
        If hw.Key.ToLower = Architecture Then
          hw1 = hw.Value
          Exit For
        End If
      Next
      Dim hw2 As List(Of Driver_Hardware) = Nothing
      For Each hw In info2.DriverHardware
        If hw.Key.ToLower = Architecture Then
          hw2 = hw.Value
          Exit For
        End If
      Next
      If hw1 Is Nothing Then Continue For
      If hw2 Is Nothing Then Continue For
      For Each hwVal1 In hw1
        For Each hwID1 In hwVal1.HardwareIDs
          For Each hwVal2 In hw2
            For Each hwID2 In hwVal2.HardwareIDs
              If hwID1.Key = hwID2.Key Then Return True
              If hwID1.Value.CompatibleIDs IsNot Nothing AndAlso hwID1.Value.CompatibleIDs.Count > 0 Then
                If hwID2.Value.CompatibleIDs IsNot Nothing AndAlso hwID2.Value.CompatibleIDs.Count > 0 Then
                  For Each hwCID1 In hwID1.Value.CompatibleIDs
                    For Each hwCID2 In hwID2.Value.CompatibleIDs
                      If hwCID1 = hwCID2 Then Return True
                    Next
                  Next
                End If
              End If
            Next
          Next
        Next
      Next
    Next
    Return False
    'Public Function IterativeContainsCheck(inArray1() As String, inArray2() As String) As Boolean
    '  For Each str1 In inArray1
    '    For Each str2 In inArray2
    '      If str1.ToLower = str2.ToLower Then Return True
    '    Next
    '  Next
    '  Return False
    'End Function
  End Operator
  Public Shared Operator <>(info1 As Driver, info2 As Driver) As Boolean
    Return Not info1 = info2
  End Operator
End Structure

Public Structure Driver_Hardware
  Public ServiceName As String
  Public Description As String
  Public Architecture As String
  Public Manufacturer As String
  Public HardwareIDs As Dictionary(Of String, Driver_Hardware_IDLists)
  Public Sub New(DriverData As Driver_HardwareA)
    ServiceName = DriverData.ServiceName
    Description = DriverData.Description
    Architecture = DriverData.Architecture
    Manufacturer = DriverData.Manufacturer
    HardwareIDs = New Dictionary(Of String, Driver_Hardware_IDLists)
    HardwareIDs.Add(DriverData.HardwareID, New Driver_Hardware_IDLists(DriverData.CompatibleIDs, DriverData.ExcludeIDs))
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Return line.Substring(line.IndexOf(" : ") + 3)
    Else
      Return Nothing
    End If
  End Function
End Structure

Public Structure Driver_Hardware_IDLists
  Public CompatibleIDs As List(Of String)
  Public ExcludeIDs As List(Of String)
  Public Sub New(CompatibleList As List(Of String), ExcludeList As List(Of String))
    CompatibleIDs = CompatibleList
    ExcludeIDs = ExcludeList
  End Sub
End Structure

Public Structure Driver_HardwareA
  Public ServiceName As String
  Public Description As String
  Public Architecture As String
  Public Manufacturer As String
  Public HardwareID As String
  Public CompatibleIDs As List(Of String)
  Public ExcludeIDs As List(Of String)
  Public Sub New(DriverData As String)
    Dim infoLines() As String = Split(DriverData, vbNewLine)
    For I As Integer = 0 To infoLines.Length - 1
      Dim line As String = infoLines(I)
      If line.StartsWith("Manufacturer :") Then
        Manufacturer = GetVal(line)
      ElseIf line.StartsWith("Description :") Then
        Description = GetVal(line)
      ElseIf line.StartsWith("Architecture :") Then
        Architecture = GetVal(line)
      ElseIf line.StartsWith("Hardware ID :") Then
        HardwareID = GetVal(line)
      ElseIf line.StartsWith("Service Name :") Then
        ServiceName = GetVal(line)
      ElseIf line.StartsWith("Compatible IDs :") Then
        Dim IDs As String = GetVal(line)
        If Not String.IsNullOrEmpty(IDs) Then
          If CompatibleIDs Is Nothing Then CompatibleIDs = New List(Of String)
          CompatibleIDs.Add(IDs)
        End If
      ElseIf line.StartsWith("Exclude IDs :") Then
        Dim IDs As String = GetVal(line)
        If Not String.IsNullOrEmpty(IDs) Then
          If ExcludeIDs Is Nothing Then ExcludeIDs = New List(Of String)
          ExcludeIDs.Add(IDs)
        End If
      End If
    Next
  End Sub
  Private Function GetVal(line As String) As String
    If line.Contains(" : ") Then
      Return line.Substring(line.IndexOf(" : ") + 3)
    Else
      Return Nothing
    End If
  End Function
  Public Shared Operator =(info1 As Driver_HardwareA, info2 As Driver_Hardware) As Boolean
    If Not info1.Manufacturer = info2.Manufacturer Then Return False
    If Not info1.Architecture = info2.Architecture Then Return False
    If Not info1.Description = info2.Description Then Return False
    If Not info1.ServiceName = info2.ServiceName Then Return False
    Return True
  End Operator
  Public Shared Operator <>(info1 As Driver_HardwareA, info2 As Driver_Hardware) As Boolean
    Return Not info1 = info2
  End Operator
End Structure
