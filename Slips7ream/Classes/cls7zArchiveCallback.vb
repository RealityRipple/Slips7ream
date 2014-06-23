Public Class ArchiveCallback
  Implements IArchiveExtractCallback
  Private Files As New Collections.Generic.Dictionary(Of UInteger, String)
  Private FileStream As OutStreamWrapper
  Private lExtractVal As ULong
  Private lExtractTotal As ULong
  Private lCurrentIndex As UInteger
  Public Event DisplayProgress(index As UInteger, items As Long, Value As ULong, Total As ULong)
  Public Event DisplayResult(index As UInteger, items As Long, result As OperationResult)
  Public Sub New(fileNumber As UInteger, fileName As String)
    Files.Add(fileNumber, fileName)
  End Sub
  Public Sub New(fileNumbers() As UInteger, fileNames() As String, destDir As String)
    For I As Integer = 0 To fileNumbers.LongLength - 1
      Files.Add(fileNumbers(I), destDir & fileNames(I))
    Next
  End Sub
#Region "IArchiveExtractCallback Members"
  Public Sub SetTotal(total As ULong) Implements IArchiveExtractCallback.SetTotal
    lExtractTotal = total
  End Sub
  Public Sub SetCompleted(ByRef completeValue As ULong) Implements IArchiveExtractCallback.SetCompleted
    lExtractVal = completeValue
    RaiseEvent DisplayProgress(lCurrentIndex, Files.LongCount, lExtractVal, lExtractTotal)
  End Sub
  Public Function GetStream(index As UInteger, ByRef outStream As ISequentialOutStream, askExtractMode As AskMode) As Integer Implements IArchiveExtractCallback.GetStream
    If Files.ContainsKey(index) AndAlso (askExtractMode = AskMode.kExtract) Then
      lCurrentIndex = index
      Dim FileName As String = Files(index)
      Dim FileDir As String = IO.Path.GetDirectoryName(FileName)
      If Not String.IsNullOrEmpty(FileDir) Then
        IO.Directory.CreateDirectory(FileDir)
      End If
      FileStream = New OutStreamWrapper(IO.File.Create(FileName))
      outStream = FileStream
    Else
      outStream = Nothing
    End If
    Return 0
  End Function
  Public Sub PrepareOperation(askExtractMode As AskMode) Implements IArchiveExtractCallback.PrepareOperation
  End Sub
  Public Sub SetOperationResult(e As OperationResult) Implements IArchiveExtractCallback.SetOperationResult
    RaiseEvent DisplayResult(lCurrentIndex, Files.LongCount, e)
    If FileStream IsNot Nothing Then FileStream.Dispose()
  End Sub
#End Region
End Class

Public Class ArchiveOpenCallback
  Implements IArchiveOpenCallback
  Public Sub SetCompleted(files As System.IntPtr, bytes As System.IntPtr) Implements IArchiveOpenCallback.SetCompleted
  End Sub
  Public Sub SetTotal(files As System.IntPtr, bytes As System.IntPtr) Implements IArchiveOpenCallback.SetTotal
  End Sub
End Class
