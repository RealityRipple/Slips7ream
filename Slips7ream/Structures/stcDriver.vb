﻿Public Structure Driver
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
    If String.IsNullOrEmpty(info1.ClassGUID) Then Return False
    If String.IsNullOrEmpty(info2.ClassGUID) Then Return False
    If Not info1.ClassGUID.ToLower = info2.ClassGUID.ToLower Then Return False
    If Not info1.Version = info2.Version Then Return False
    If Not info1.Date = info2.Date Then Return False
    Dim archMatches As New List(Of String)
    For Each arch1 As String In info1.Architectures
      If StrComp(arch1, "ia64", CompareMethod.Text) = 0 Then Continue For
      For Each arch2 As String In info2.Architectures
        If StrComp(arch1, arch2, CompareMethod.Text) = 0 Then archMatches.Add(arch1)
      Next
    Next
    If archMatches.Count = 0 Then Return False
    For Each arch In archMatches
      Dim hw1 = info1.DriverHardware(arch)
      Dim hw2 = info2.DriverHardware(arch)
      If Not hw1.Count = hw2.Count Then Return False
      For I As Integer = 0 To hw1.Count - 1
        If Not hw1(I) = hw2(I) Then Return False
      Next
    Next
    Return True
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
  Public Shared Operator =(hw1 As Driver_Hardware, hw2 As Driver_Hardware)
    If Not StrComp(hw1.ServiceName, hw2.ServiceName, CompareMethod.Text) = 0 Then Return False
    If Not StrComp(hw1.Description, hw2.Description, CompareMethod.Text) = 0 Then Return False
    If Not StrComp(hw1.Manufacturer, hw2.Manufacturer, CompareMethod.Text) = 0 Then Return False
    If hw1.HardwareIDs Is Nothing And hw2.HardwareIDs Is Nothing Then Return True
    If hw1.HardwareIDs Is Nothing Then Return False
    If hw2.HardwareIDs Is Nothing Then Return False
    If Not hw1.HardwareIDs.Count = hw2.HardwareIDs.Count Then Return False
    For I As Integer = 0 To hw1.HardwareIDs.Count - 1
      Dim key1 As String = hw1.HardwareIDs.Keys.ToArray()(I)
      Dim key2 As String = hw2.HardwareIDs.Keys.ToArray()(I)
      If Not StrComp(key1, key2, CompareMethod.Text) = 0 Then Return False
      If Not hw1.HardwareIDs(key1).CompatibleIDs Is Nothing And Not hw2.HardwareIDs(key2).CompatibleIDs Is Nothing Then
        If Not hw1.HardwareIDs(key1).CompatibleIDs.Count = hw2.HardwareIDs(key2).CompatibleIDs.Count Then Return False
        For J As Integer = 0 To hw1.HardwareIDs(key1).CompatibleIDs.Count - 1
          If Not StrComp(hw1.HardwareIDs(key1).CompatibleIDs(J), hw2.HardwareIDs(key2).CompatibleIDs(J), CompareMethod.Text) = 0 Then Return False
        Next
      Else
        If Not hw1.HardwareIDs(key1).CompatibleIDs Is Nothing Then Return False
        If Not hw2.HardwareIDs(key2).CompatibleIDs Is Nothing Then Return False
      End If
      If Not hw1.HardwareIDs(key1).ExcludeIDs Is Nothing And Not hw2.HardwareIDs(key2).ExcludeIDs Is Nothing Then
        If Not hw1.HardwareIDs(key1).ExcludeIDs.Count = hw2.HardwareIDs(key2).ExcludeIDs.Count Then Return False
        For J As Integer = 0 To hw1.HardwareIDs(key1).ExcludeIDs.Count - 1
          If Not StrComp(hw1.HardwareIDs(key1).ExcludeIDs(J), hw2.HardwareIDs(key2).ExcludeIDs(J), CompareMethod.Text) = 0 Then Return False
        Next
      Else
        If Not hw1.HardwareIDs(key1).ExcludeIDs Is Nothing Then Return False
        If Not hw2.HardwareIDs(key2).ExcludeIDs Is Nothing Then Return False
      End If
    Next
    Return True
  End Operator
  Public Shared Operator <>(hw1 As Driver_Hardware, hw2 As Driver_Hardware)
    Return Not hw1 = hw2
  End Operator
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
