Public Class frmTetris

  <Runtime.InteropServices.DllImport("user32")>
  Private Shared Function GetAsyncKeyState(ByVal vKey As Int32) As UShort
  End Function

  Private Const INTERVAL_START As Integer = 50
  Private Const INTERVAL_BREAK As Integer = 225
  Private Const INTERVAL_REPEAT As Integer = 30

  Private Enum Motions
    None = &H0
    Left = &H1
    Right = &H2
    Down = &H8
    RotateL = &H10
    RotateR = &H20
  End Enum

  Private Enum GameShapes
    O
    S
    Z
    L
    J
    I
    T
  End Enum

#Region "Sound"

  Private Class MCIPlayer
    Implements IDisposable
    Private sAlias As String
    <Runtime.InteropServices.DllImport("winmm")>
    Private Shared Function mciSendString(ByVal strCommand As String, ByVal strReturn As System.Text.StringBuilder, ByVal iReturnLength As Integer, ByVal hwndCallback As IntPtr) As Long
    End Function
    Public Sub New(sFileName As String)
      If Status() <> "" Then
        [Stop]()
        Close()
      End If
      sAlias = IO.Path.GetFileNameWithoutExtension(sFileName)
      Dim sCommand As String = "open """ & sFileName & """ type mpegvideo alias " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Close()
      Dim sCommand As String = "close " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
      sAlias = String.Empty
    End Sub
    Public Sub CloseAll()
      Dim sCommand As String = "close all wait"
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Pause()
      Dim sCommand As String = "pause " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Play(Optional Repeat As Boolean = False)
      Dim sCommand As String = "play " & sAlias & IIf(Repeat, " repeat", String.Empty)
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Restart(Optional Repeat As Boolean = False)
      Dim sCommand As String = "play " & sAlias & " from 0"
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Resume]()
      Dim sCommand As String = "play " & sAlias & " from " & CLng(Status())
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Stop]()
      Dim sCommand As String = "stop " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Function Status() As String
      Dim sBuffer As New System.Text.StringBuilder(128)
      mciSendString("status " & sAlias & " mode", sBuffer, sBuffer.Capacity, IntPtr.Zero)
      Return sBuffer.ToString()
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          [Stop]()
          Close()
        End If
      End If
      Me.disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
#End Region
  End Class

  Private Class EffectPlayer
    Implements IDisposable

    Private effect1 As MCIPlayer
    Private effect2 As MCIPlayer
    Private effval As Boolean = True
    Private sPath1 As String
    Private sPath2 As String

    Public Sub New(stream As IO.UnmanagedMemoryStream, id As String)
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(AppData)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(AppData))
      If Not My.Computer.FileSystem.DirectoryExists(AppData) Then My.Computer.FileSystem.CreateDirectory(AppData)
      sPath1 = AppData & "\" & id & "1.wav"
      sPath2 = AppData & "\" & id & "2.wav"
      If Not My.Computer.FileSystem.FileExists(sPath1) Or Not My.Computer.FileSystem.FileExists(sPath2) Then
        Dim streamData(stream.Length - 1) As Byte
        stream.Read(streamData, 0, stream.Length)
        My.Computer.FileSystem.WriteAllBytes(sPath1, streamData, False)
        My.Computer.FileSystem.WriteAllBytes(sPath2, streamData, False)
      End If
      effect1 = New MCIPlayer(sPath1)
      effect2 = New MCIPlayer(sPath2)
    End Sub

    Private ReadOnly Property AppData As String
      Get
        Static sTmp As String
        If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName)
        If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName)
        If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris") Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris")
        If String.IsNullOrEmpty(sTmp) Then
          sTmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris"
        End If
        Return sTmp
      End Get
    End Property

    Public Sub Play()
      If effval Then
        effect1.Restart()
        effval = False
      Else
        effect2.Restart()
        effval = True
      End If
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          effect1.Close()
          effect1.Dispose()
          effect1 = Nothing

          effect2.Close()
          effect2.Dispose()
          effect2 = Nothing

          My.Computer.FileSystem.DeleteFile(sPath1)
          My.Computer.FileSystem.DeleteFile(sPath2)
        End If
      End If
      Me.disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
#End Region
  End Class

  Private Sub LoadAllSounds()
    If move_effect Is Nothing Then move_effect = New EffectPlayer(My.Resources.tetris_move, "move")
    If rotate_effect Is Nothing Then rotate_effect = New EffectPlayer(My.Resources.tetris_rotate, "rotate")
    If down_effect Is Nothing Then down_effect = New EffectPlayer(My.Resources.tetris_down, "down")
    If row_effect Is Nothing Then row_effect = New EffectPlayer(My.Resources.tetris_row, "row")
    If four_effect Is Nothing Then four_effect = New EffectPlayer(My.Resources.tetris_four, "four")
    If level_effect Is Nothing Then level_effect = New EffectPlayer(My.Resources.tetris_level, "level")
    If gameover_effect Is Nothing Then gameover_effect = New EffectPlayer(My.Resources.tetris_gameover, "gameover")
  End Sub

  Private move_effect As EffectPlayer
  Private Sub Play_Move()
    If SoundEffectsEnabled Then
      If move_effect Is Nothing Then move_effect = New EffectPlayer(My.Resources.tetris_move, "move")
      move_effect.Play()
    End If
  End Sub

  Private rotate_effect As EffectPlayer
  Private Sub Play_Rotate()
    If SoundEffectsEnabled Then
      If rotate_effect Is Nothing Then rotate_effect = New EffectPlayer(My.Resources.tetris_rotate, "rotate")
      rotate_effect.Play()
    End If
  End Sub

  Private down_effect As EffectPlayer
  Private Sub Play_Down()
    If SoundEffectsEnabled Then
      If down_effect Is Nothing Then down_effect = New EffectPlayer(My.Resources.tetris_down, "down")
      down_effect.Play()
    End If
  End Sub

  Private row_effect As EffectPlayer
  Private Sub Play_Row()
    If SoundEffectsEnabled Then
      If row_effect Is Nothing Then row_effect = New EffectPlayer(My.Resources.tetris_row, "row")
      row_effect.Play()
    End If
  End Sub

  Private four_effect As EffectPlayer
  Private Sub Play_Four()
    If SoundEffectsEnabled Then
      If four_effect Is Nothing Then four_effect = New EffectPlayer(My.Resources.tetris_four, "four")
      four_effect.Play()
    End If
  End Sub

  Private level_effect As EffectPlayer
  Private Sub Play_Level()
    If SoundEffectsEnabled Then
      If level_effect Is Nothing Then level_effect = New EffectPlayer(My.Resources.tetris_level, "level")
      level_effect.Play()
    End If
  End Sub

  Private gameover_effect As EffectPlayer
  Private Sub Play_GameOver()
    If SoundEffectsEnabled Then
      If gameover_effect Is Nothing Then gameover_effect = New EffectPlayer(My.Resources.tetris_gameover, "gameover")
      gameover_effect.Play()
    End If
  End Sub
#Region "Song"
  Private song As MCIPlayer
  Private ReadOnly Property IsSongPlaying As Boolean
    Get
      Return song IsNot Nothing
    End Get
  End Property
  Private Sub ToggleSong()
    If song Is Nothing Then
      PlaySong()
    Else
      StopSong()
    End If
  End Sub
  Private Sub PlaySong()
    If song Is Nothing Then
      My.Computer.FileSystem.WriteAllBytes(AppData & "\shoot_em.ogg", My.Resources.shoot_em, False)
      song = New MCIPlayer(AppData & "\shoot_em.ogg")
      song.Play(True)
    End If
  End Sub
  Private Sub StopSong()
    If song IsNot Nothing Then
      song.Stop()
      song.Close()
      song.Dispose()
      song = Nothing
    End If
    If My.Computer.FileSystem.FileExists(AppData & "\shoot_em.ogg") Then
      Try
        My.Computer.FileSystem.DeleteFile(AppData & "\shoot_em.ogg")
      Catch ex As Exception
      End Try
    End If
  End Sub
#End Region

  Private ReadOnly Property AppData As String
    Get
      Static sTmp As String
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName)
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName)
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris") Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris")
      If String.IsNullOrEmpty(sTmp) Then
        sTmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\Tetris"
      End If
      Return sTmp
    End Get
  End Property
#End Region

#Region "Level Colors"

  Private Enum ColorStyle
    NoBlock
    ShapeOBlock
    ShapeSBlock
    ShapeZBlock
    ShapeLBlock
    ShapeJBlock
    ShapeIBlock
    ShapeTBlock
    RowBlock
    RowFade1
    RowFade2
    RowFade3
    RowFade4
    RowFade5
    RowFade6
    RowFade7
    RowFade8
    RowFade9
    RowFade10
  End Enum

  Private MustInherit Class LevelColors
    Public MustOverride ReadOnly Property Primary As Color
    Public MustOverride ReadOnly Property Secondary As Color
    Public MustOverride ReadOnly Property Tertiary As Color

    Public ReadOnly Property BGColor(Style As ColorStyle) As Color
      Get
        Select Case Style
          Case ColorStyle.RowBlock : Return Color.FromArgb(223, 223, 223)
          Case ColorStyle.RowFade1 : Return Color.FromArgb(230, 223, 223, 223)
          Case ColorStyle.RowFade2 : Return Color.FromArgb(205, 223, 223, 223)
          Case ColorStyle.RowFade3 : Return Color.FromArgb(180, 223, 223, 223)
          Case ColorStyle.RowFade4 : Return Color.FromArgb(155, 223, 223, 223)
          Case ColorStyle.RowFade5 : Return Color.FromArgb(130, 223, 223, 223)
          Case ColorStyle.RowFade6 : Return Color.FromArgb(105, 223, 223, 223)
          Case ColorStyle.RowFade7 : Return Color.FromArgb(80, 223, 223, 223)
          Case ColorStyle.RowFade8 : Return Color.FromArgb(55, 223, 223, 223)
          Case ColorStyle.RowFade9 : Return Color.FromArgb(30, 223, 223, 223)
          Case ColorStyle.RowFade10 : Return Color.FromArgb(5, 223, 223, 223)
          Case ColorStyle.ShapeIBlock, ColorStyle.ShapeOBlock : Return Primary
          Case ColorStyle.ShapeJBlock, ColorStyle.ShapeSBlock, ColorStyle.ShapeTBlock : Return Secondary
          Case ColorStyle.ShapeLBlock, ColorStyle.ShapeZBlock : Return Tertiary
          Case ColorStyle.NoBlock : Return Color.Black
        End Select
      End Get
    End Property
  End Class

  Private Class Level1Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(13, 153, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(29, 229, 253)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(26, 255, 203)
      End Get
    End Property
  End Class

  Private Class Level8Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(29, 229, 253)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(26, 255, 203)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(40, 255, 150)
      End Get
    End Property
  End Class

  Private Class Level4Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(26, 255, 203)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(40, 255, 150)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(8, 255, 97)
      End Get
    End Property
  End Class

  Private Class Level12Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(40, 255, 150)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(8, 255, 97)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(70, 255, 7)
      End Get
    End Property
  End Class

  Private Class Level2Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(8, 255, 97)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(70, 255, 7)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(179, 255, 8)
      End Get
    End Property
  End Class

  Private Class Level13Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(70, 255, 7)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(179, 255, 8)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(244, 243, 9)
      End Get
    End Property
  End Class

  Private Class Level5Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(179, 255, 8)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(244, 243, 9)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 164, 3)
      End Get
    End Property
  End Class

  Private Class Level16Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(244, 243, 9)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 164, 3)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 53, 17)
      End Get
    End Property
  End Class

  Private Class Level6Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 164, 3)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 53, 17)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 8, 106)
      End Get
    End Property
  End Class

  Private Class Level14Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 53, 17)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 8, 106)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 6, 163)
      End Get
    End Property
  End Class

  Private Class Level3Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 8, 106)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 6, 163)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 15, 213)
      End Get
    End Property
  End Class

  Private Class Level7Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 6, 163)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 15, 213)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(227, 0, 255)
      End Get
    End Property
  End Class

  Private Class Level15Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(255, 15, 213)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(227, 0, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(93, 5, 255)
      End Get
    End Property
  End Class

  Private Class Level10Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(227, 0, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(93, 5, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(36, 89, 255)
      End Get
    End Property
  End Class

  Private Class Level9Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(93, 5, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(36, 89, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(13, 153, 255)
      End Get
    End Property
  End Class

  Private Class Level11Colors
    Inherits LevelColors
    Public Overrides ReadOnly Property Primary As System.Drawing.Color
      Get
        Return Color.FromArgb(36, 89, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Secondary As System.Drawing.Color
      Get
        Return Color.FromArgb(13, 153, 255)
      End Get
    End Property
    Public Overrides ReadOnly Property Tertiary As System.Drawing.Color
      Get
        Return Color.FromArgb(29, 229, 253)
      End Get
    End Property
  End Class

#End Region

  Private BaseBoard()() As ColorStyle
  Private ActivePiece()() As ColorStyle

  Private SoundEffectsEnabled As Boolean
  Private Busy As Boolean = False
  Private Paused As Boolean = False
  Private FadeIn As Boolean = True

  Private musicToggle As Boolean = False
  Private soundToggle As Boolean = False
  Private helpToggle As Boolean = False
  Private pauseToggle As Boolean = False

  Private pauseLoop As Byte = 10

  Private statScore As ULong
  Private statTopScore As ULong
  Private statLevel As UShort
  Private statLines As UInteger
  Private statNext As GameShapes
  Private statColors As LevelColors
  Private WithEvents wsScore As Net.WebClient

#Region "Form Events"
  Private Sub frmTetris_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    NewGame()
    UpdateHighScore()
    If My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains("RealityRipple Software") Then
      If My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software").GetSubKeyNames.Contains("Tetris") Then
        lblHelp.Visible = False
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software\Tetris").GetValue("SFX", "N") = "Y" Then
          SoundEffectsEnabled = True
          LoadAllSounds()
        End If
        If My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software\Tetris").GetValue("BGM", "N") = "Y" Then
          PlaySong()
        End If
      End If
    End If
  End Sub

  Private Sub frmTetris_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If move_effect IsNot Nothing Then move_effect.Dispose()
    If rotate_effect IsNot Nothing Then rotate_effect.Dispose()
    If down_effect IsNot Nothing Then down_effect.Dispose()
    If row_effect IsNot Nothing Then row_effect.Dispose()
    If four_effect IsNot Nothing Then four_effect.Dispose()
    If level_effect IsNot Nothing Then level_effect.Dispose()
    If gameover_effect IsNot Nothing Then gameover_effect.Dispose()
    If IsSongPlaying Then StopSong()
  End Sub

  Private Sub frmTetris_Move(sender As Object, e As System.EventArgs) Handles Me.Move
    If frmTetrisHelp.Visible Then
      frmTetrisHelp.Left = Me.Left
      frmTetrisHelp.Top = Me.Top + Me.Height + 5
    End If
  End Sub

  Private Sub frmTetris_GotFocus(sender As Object, e As System.EventArgs) Handles Me.GotFocus
    If tmrDisplayUpdate.Enabled Then RedrawGame()
  End Sub

  Private Sub frmTetris_LostFocus(sender As Object, e As System.EventArgs) Handles Me.LostFocus
    If tmrDisplayUpdate.Enabled Then
      pauseLoop = 10
      FadeIn = True
      RedrawGame()
    End If
  End Sub
#End Region

#Region "Timers"
  Private Sub tmrDisplayUpdate_Tick(sender As System.Object, e As System.EventArgs) Handles tmrDisplayUpdate.Tick
    If Me.ContainsFocus Then
      If Not Paused And Not Busy Then
        If MovePiece(True, TriState.UseDefault, True) Then RedrawGame()
      ElseIf Paused Then
        RedrawGame()
      End If
    Else
      RedrawGame()
    End If
  End Sub

  Private Sub tmrKeyUpdate_Tick(sender As System.Object, e As System.EventArgs) Handles tmrKeyUpdate.Tick
    If Me.ContainsFocus Then
      If tmrDisplayUpdate.Enabled Then
        If GetAsyncKeyState(Convert.ToInt32(Keys.Left)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Left) = Motions.Left) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Left
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.A)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Left) = Motions.Left) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Left
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        Else
          tmrMove.Tag = tmrMove.Tag And Not Motions.Left
        End If

        If GetAsyncKeyState(Convert.ToInt32(Keys.Right)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Right) = Motions.Right) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Right
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.D)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Right) = Motions.Right) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Right
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        Else
          tmrMove.Tag = tmrMove.Tag And Not Motions.Right
        End If

        If GetAsyncKeyState(Convert.ToInt32(Keys.Down)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Down) = Motions.Down) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Down
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.S)) > 0 Then
          If Not ((tmrMove.Tag And Motions.Down) = Motions.Down) Then
            tmrMove.Interval = INTERVAL_START
            tmrMove.Tag = tmrMove.Tag Or Motions.Down
            tmrMove_Tick(Nothing, EventArgs.Empty)
          End If
        Else
          tmrMove.Tag = tmrMove.Tag And Not Motions.Down
        End If

        If GetAsyncKeyState(Convert.ToInt32(Keys.Up)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateL) = Motions.RotateL) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateL
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.W)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateL) = Motions.RotateL) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateL
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.OemCloseBrackets)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateL) = Motions.RotateL) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateL
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.E)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateL) = Motions.RotateL) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateL
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        Else
          tmrRotate.Tag = tmrRotate.Tag And Not Motions.RotateL
        End If

        If GetAsyncKeyState(Convert.ToInt32(Keys.OemOpenBrackets)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateR) = Motions.RotateR) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateR
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        ElseIf GetAsyncKeyState(Convert.ToInt32(Keys.Q)) > 0 Then
          If Not ((tmrRotate.Tag And Motions.RotateR) = Motions.RotateR) Then
            tmrRotate.Interval = INTERVAL_START
            tmrRotate.Tag = tmrRotate.Tag Or Motions.RotateR
            tmrRotate_Tick(Nothing, EventArgs.Empty)
          End If
        Else
          tmrRotate.Tag = tmrRotate.Tag And Not Motions.RotateR
        End If

        If GetAsyncKeyState(Convert.ToInt32(Keys.P)) > 0 Then
          If Not pauseToggle Then
            pauseLoop = 10
            FadeIn = True
            Paused = Not Paused
            RedrawGame()
            pauseToggle = True
          End If
        Else
          pauseToggle = False
        End If
      End If

      If GetAsyncKeyState(Convert.ToInt32(Keys.M)) > 0 Then
        If Not musicToggle Then
          If IsSongPlaying Then
            StopSong()
          Else
            PlaySong()
          End If
          If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains("RealityRipple Software") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey("RealityRipple Software")
          If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software").GetSubKeyNames.Contains("Tetris") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software", True).CreateSubKey("Tetris")
          My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software\Tetris", True).SetValue("BGM", IIf(IsSongPlaying, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
          musicToggle = True
        End If
      Else
        musicToggle = False
      End If

      If GetAsyncKeyState(Convert.ToInt32(Keys.N)) > 0 Then
        If Not soundToggle Then
          If SoundEffectsEnabled Then
            SoundEffectsEnabled = False
          Else
            LoadAllSounds()
            SoundEffectsEnabled = True
          End If
          If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software").GetSubKeyNames.Contains("RealityRipple Software") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software", True).CreateSubKey("RealityRipple Software")
          If Not My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software").GetSubKeyNames.Contains("Tetris") Then My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software", True).CreateSubKey("Tetris")
          My.Computer.Registry.CurrentUser.OpenSubKey("Software\RealityRipple Software\Tetris", True).SetValue("SFX", IIf(SoundEffectsEnabled, "Y", "N"), Microsoft.Win32.RegistryValueKind.String)
          soundToggle = True
        End If
      Else
        soundToggle = False
      End If

      If GetAsyncKeyState(Convert.ToInt32(Keys.F1)) > 0 Then
        If Not helpToggle Then
          If frmTetrisHelp.Visible Then
            frmTetrisHelp.Hide()
          Else
            frmTetrisHelp.Show()
            frmTetrisHelp.Left = Me.Left
            frmTetrisHelp.Top = Me.Top + Me.Height + 5
            Me.Focus()
          End If
          helpToggle = True
        End If
      Else
        helpToggle = False
      End If


    End If
  End Sub

  Private Sub tmrMove_Tick(sender As System.Object, e As System.EventArgs) Handles tmrMove.Tick
    If tmrDisplayUpdate.Enabled And Me.ContainsFocus And Not Busy And Not Paused Then
      Dim motion As Motions = tmrMove.Tag
      If motion = Motions.None Then
        tmrMove.Interval = INTERVAL_START
      Else
        If tmrMove.Interval = INTERVAL_START Then
          tmrMove.Interval = INTERVAL_BREAK
        ElseIf tmrMove.Interval = INTERVAL_BREAK Then
          tmrMove.Interval = INTERVAL_REPEAT
        End If
        If (motion And Motions.Left) = Motions.Left Then
          If MovePiece(False, TriState.False) Then RedrawGame()
        End If
        If (motion And Motions.Right) = Motions.Right Then
          If MovePiece(False, TriState.True) Then RedrawGame()
        End If
        If (motion And Motions.Down) = Motions.Down Then
          If MovePiece(True, TriState.UseDefault) Then RedrawGame()
        End If
      End If
    End If
  End Sub

  Private Sub tmrRotate_Tick(sender As System.Object, e As System.EventArgs) Handles tmrRotate.Tick
    If tmrDisplayUpdate.Enabled And Me.ContainsFocus And Not Busy And Not Paused Then
      Dim motion As Motions = tmrRotate.Tag
      If motion = Motions.None Then
        tmrRotate.Interval = INTERVAL_START
      Else
        If tmrRotate.Interval = INTERVAL_START Then
          tmrRotate.Interval = INTERVAL_BREAK
        ElseIf tmrRotate.Interval = INTERVAL_BREAK Then
          tmrRotate.Interval = INTERVAL_REPEAT
        End If
        If (motion And Motions.RotateL) = Motions.RotateL Then
          RotatePiece(True)
          RedrawGame()
        End If
        If (motion And Motions.RotateR) = Motions.RotateR Then
          RotatePiece(False)
          RedrawGame()
        End If
      End If
    End If
  End Sub
#End Region

  Private Sub NewGame()
    Randomize()
    ReDim BaseBoard(20)
    ReDim ActivePiece(20)
    For I As Integer = 0 To 20
      ReDim BaseBoard(I)(9)
      ReDim ActivePiece(I)(9)
      For J As Integer = 0 To 9
        BaseBoard(I)(J) = ColorStyle.NoBlock
        ActivePiece(I)(J) = ColorStyle.NoBlock
      Next
    Next
    statScore = 0
    statLevel = 1
    statColors = New Level1Colors
    statLines = 0
    tmrDisplayUpdate.Interval = 1000
    StartNextPiece()
  End Sub

  Private Sub cmdNew_Click(sender As System.Object, e As System.EventArgs) Handles cmdNew.Click
    cmdNew.Visible = False
    Me.Focus()
    NewGame()
    tmrDisplayUpdate.Enabled = True
    RedrawGame()
  End Sub

  Private Function MovePiece(Down As Boolean, RoL As TriState, Optional silent As Boolean = False) As Boolean
    If Down Then
      Dim MoveDown As Boolean = True
      For x As Integer = 0 To 9
        If Not ActivePiece(20)(x) = ColorStyle.NoBlock Then
          MoveDown = False
          Exit For
        End If
      Next
      For y As Integer = 0 To 19
        For x As Integer = 0 To 9
          If Not ActivePiece(y)(x) = ColorStyle.NoBlock And Not BaseBoard(y + 1)(x) = ColorStyle.NoBlock Then
            MoveDown = False
            Exit For
          End If
        Next
      Next
      If MoveDown Then
        For x As Integer = 0 To 9
          For y As Integer = 19 To 0 Step -1
            ActivePiece(y + 1)(x) = ActivePiece(y)(x)
          Next
          ActivePiece(0)(x) = ColorStyle.NoBlock
        Next
        If Not silent Then Play_Move()
      Else
        If Not AddToBase() Then Return False
      End If
    End If
    If RoL = TriState.True Then
      Dim MoveRight As Boolean = True
      For y As Integer = 0 To 20
        If Not ActivePiece(y)(9) = ColorStyle.NoBlock Then
          MoveRight = False
          Exit For
        End If
      Next
      For y As Integer = 0 To 20
        For x As Integer = 8 To 0 Step -1
          If Not ActivePiece(y)(x) = ColorStyle.NoBlock And Not BaseBoard(y)(x + 1) = ColorStyle.NoBlock Then
            MoveRight = False
            Exit For
          End If
        Next
      Next
      If MoveRight Then
        For y As Integer = 0 To 20
          For x As Integer = 8 To 0 Step -1
            ActivePiece(y)(x + 1) = ActivePiece(y)(x)
          Next
          ActivePiece(y)(0) = ColorStyle.NoBlock
        Next
        Play_Move()
      End If
    ElseIf RoL = TriState.False Then
      Dim MoveLeft As Boolean = True
      For y As Integer = 0 To 20
        If Not ActivePiece(y)(0) = ColorStyle.NoBlock Then
          MoveLeft = False
          Exit For
        End If
      Next
      For y As Integer = 0 To 20
        For x As Integer = 1 To 9
          If Not ActivePiece(y)(x) = ColorStyle.NoBlock And Not BaseBoard(y)(x - 1) = ColorStyle.NoBlock Then
            MoveLeft = False
            Exit For
          End If
        Next
      Next
      If MoveLeft Then
        For y As Integer = 0 To 20
          For x As Integer = 1 To 9
            ActivePiece(y)(x - 1) = ActivePiece(y)(x)
          Next
          ActivePiece(y)(9) = ColorStyle.NoBlock
        Next
        Play_Move()
      End If
    End If
    Return True
  End Function

  Private Sub RotatePiece(Clockwise As Boolean)
    Dim topX, topY As Integer
    topX = 9
    topY = 20
    For y As Integer = 0 To 20
      For x As Integer = 0 To 9
        If Not ActivePiece(y)(x) = ColorStyle.NoBlock Then
          If topX > x Then topX = x
          If topY > y Then topY = y
        End If
      Next
    Next
    Dim NewBlock()() As ColorStyle
    ReDim NewBlock(3)
    For I As Integer = 0 To 3
      ReDim NewBlock(I)(3)
    Next
    If Clockwise Then
      For x As Integer = topX To topX + 3
        For y As Integer = topY To topY + 3
          Dim y2 As Integer = y - topY
          Dim x2 As Integer = x - topX
          If x < 10 And y < 21 Then
            If Not ActivePiece(y)(x) = ColorStyle.NoBlock Then
              NewBlock(x2)(4 - y2 - 1) = ActivePiece(y)(x)
            End If
          End If
        Next
      Next
    Else
      For x As Integer = topX To topX + 3
        For y As Integer = topY To topY + 3
          Dim y2 As Integer = y - topY
          Dim x2 As Integer = x - topX
          If x < 10 And y < 21 Then
            If Not ActivePiece(y)(x) = ColorStyle.NoBlock Then
              NewBlock(4 - x2 - 1)(y2) = ActivePiece(y)(x)
            End If
          End If
        Next
      Next
    End If
    Dim BlockCount As Integer = 0

    Dim topX2, topY2 As Integer
    topX2 = 3
    topY2 = 3
    For y As Integer = 0 To 3
      For x As Integer = 0 To 3
        If Not NewBlock(y)(x) = ColorStyle.NoBlock Then
          If topX2 > x Then topX2 = x
          If topY2 > y Then topY2 = y
        End If
      Next
    Next

    Dim NewActive()() As ColorStyle
    ReDim NewActive(20)
    For I As Integer = 0 To 20
      ReDim NewActive(I)(9)
    Next
    For x As Integer = topX To topX + 3
      For y As Integer = topY To topY + 3
        Dim y2 As Integer = y - topY
        Dim x2 As Integer = x - topX
        If Not NewBlock(y2)(x2) = ColorStyle.NoBlock Then
          If x - topX2 > 9 Then Continue For
          If y - topY2 > 20 Then Continue For
          If y < topY2 Then Continue For
          If BaseBoard(y - topY2)(x - topX2) = ColorStyle.NoBlock Then
            BlockCount += 1
            NewActive(y - topY2)(x - topX2) = NewBlock(y2)(x2)
          End If
        End If
      Next
    Next
    If BlockCount = 4 Then
      ActivePiece = NewActive
      Play_Rotate()
    End If
  End Sub

  Private Function AddToBase() As Boolean
    For y As Integer = 1 To 20
      For x As Integer = 0 To 9
        If Not ActivePiece(y)(x) = ColorStyle.NoBlock Then
          BaseBoard(y)(x) = ActivePiece(y)(x)
          ActivePiece(y)(x) = ColorStyle.NoBlock
        End If
      Next
    Next
    Play_Down()
    tmrMove.Interval = INTERVAL_BREAK
    tmrRotate.Interval = INTERVAL_BREAK
    If CheckRows() Then
      StartNextPiece()
      Return True
    Else
      play_Gameover()
      tmrDisplayUpdate.Enabled = False
      RedrawGame()
      cmdNew.Visible = True
      SendHighScore(statScore)
      Return False
    End If
  End Function

  Private Function CheckRows() As Boolean
    Dim doneRows(20) As Boolean
    For y As Integer = 20 To 1 Step -1
      Dim FullRow As Boolean = True
      For x As Integer = 0 To 9
        If BaseBoard(y)(x) = ColorStyle.NoBlock Then
          FullRow = False
          Exit For
        End If
      Next
      doneRows(y) = FullRow
    Next
    Dim DoneCount As Integer = 0
    For I As Integer = 1 To 20
      If doneRows(I) Then DoneCount += 1
    Next
    If DoneCount > 0 Then
      Busy = True
      For I As Integer = 1 To 20
        If doneRows(I) Then
          For x As Integer = 0 To 9
            BaseBoard(I)(x) = ColorStyle.RowBlock
          Next
        End If
      Next
      RedrawBoard()
      Application.DoEvents()
      If SoundEffectsEnabled Then
        If DoneCount = 4 Then
          Play_Four()
        Else
          Play_Row()
        End If
      End If
      For A As Integer = 1 To 10
        Dim BlockStyle As ColorStyle = ColorStyle.RowBlock
        Select Case A
          Case 1 : BlockStyle = ColorStyle.RowFade1
          Case 2 : BlockStyle = ColorStyle.RowFade2
          Case 3 : BlockStyle = ColorStyle.RowFade3
          Case 4 : BlockStyle = ColorStyle.RowFade4
          Case 5 : BlockStyle = ColorStyle.RowFade5
          Case 6 : BlockStyle = ColorStyle.RowFade6
          Case 7 : BlockStyle = ColorStyle.RowFade7
          Case 8 : BlockStyle = ColorStyle.RowFade8
          Case 9 : BlockStyle = ColorStyle.RowFade9
          Case 10 : BlockStyle = ColorStyle.RowFade10
        End Select
        Threading.Thread.Sleep(50)
        For I As Integer = 1 To 20
          If doneRows(I) Then
            For x As Integer = 0 To 9
              BaseBoard(I)(x) = BlockStyle
            Next
          End If
        Next
        RedrawBoard()
        Application.DoEvents()
      Next
      If DoneCount = 1 Then
        statScore += 10
      ElseIf DoneCount = 2 Then
        statScore += 25
      ElseIf DoneCount = 3 Then
        statScore += 50
      Else
        statScore += 200
      End If
      statLines += DoneCount
      For I As Integer = 1 To 20
        If doneRows(I) Then
          For x As Integer = 0 To 9
            For y As Integer = I - 1 To 0 Step -1
              BaseBoard(y + 1)(x) = BaseBoard(y)(x)
            Next
            BaseBoard(0)(x) = ColorStyle.NoBlock
          Next
        End If
      Next
      Dim lastLevel As UShort = statLevel
      statLevel = ((statLines - 1) \ 10) + 1
      If Not statLevel = lastLevel Then
        Dim lSpeed As Integer = 1000
        Select Case statLevel
          Case Is < 1 : lSpeed = 1000
          Case 1 : lSpeed = 1000
          Case 2 : lSpeed = 800
          Case 3 : lSpeed = 600
          Case 4 : lSpeed = 500
          Case 5 : lSpeed = 350
          Case 6 : lSpeed = 250
          Case 7 : lSpeed = 225
          Case 8 : lSpeed = 200
          Case 9 : lSpeed = 175
          Case 10 : lSpeed = 150
          Case 11 : lSpeed = 130
          Case 12 : lSpeed = 120
          Case 13 : lSpeed = 110
          Case 14 : lSpeed = 100
          Case 15 : lSpeed = 90
          Case 16 : lSpeed = 80
          Case 17 : lSpeed = 70
          Case 18 : lSpeed = 60
          Case 19 : lSpeed = 50
          Case Else : lSpeed = 45
        End Select
        tmrDisplayUpdate.Interval = lSpeed
        Select Case statLevel Mod 16
          Case 1 : statColors = New Level1Colors
          Case 2 : statColors = New Level2Colors
          Case 3 : statColors = New Level3Colors
          Case 4 : statColors = New Level4Colors
          Case 5 : statColors = New Level5Colors
          Case 6 : statColors = New Level6Colors
          Case 7 : statColors = New Level7Colors
          Case 8 : statColors = New Level8Colors
          Case 9 : statColors = New Level9Colors
          Case 10 : statColors = New Level10Colors
          Case 11 : statColors = New Level11Colors
          Case 12 : statColors = New Level12Colors
          Case 13 : statColors = New Level13Colors
          Case 14 : statColors = New Level14Colors
          Case 15 : statColors = New Level15Colors
          Case 0 : statColors = New Level16Colors
        End Select
        statScore += 500
        If DoneCount = 4 Then statScore += 250
        RedrawGame()
        pctNextPiece.Image = Nothing
        Application.DoEvents()
        If SoundEffectsEnabled Then Play_Level()
        Threading.Thread.Sleep(500)
      Else
        pctNextPiece.Image = DrawDemoPiece(statNext)
      End If
      Busy = False
    End If
    For X As Integer = 0 To 9
      If Not BaseBoard(1)(X) = ColorStyle.NoBlock Then Return False
    Next
    Return True
  End Function

  Private Sub StartNextPiece()
    Select Case statNext
      Case GameShapes.I
        ActivePiece(1)(3) = ColorStyle.ShapeIBlock
        ActivePiece(1)(4) = ColorStyle.ShapeIBlock
        ActivePiece(1)(5) = ColorStyle.ShapeIBlock
        ActivePiece(1)(6) = ColorStyle.ShapeIBlock
      Case GameShapes.J
        ActivePiece(0)(4) = ColorStyle.ShapeJBlock
        ActivePiece(1)(4) = ColorStyle.ShapeJBlock
        ActivePiece(1)(5) = ColorStyle.ShapeJBlock
        ActivePiece(1)(6) = ColorStyle.ShapeJBlock
      Case GameShapes.L
        ActivePiece(1)(3) = ColorStyle.ShapeLBlock
        ActivePiece(1)(4) = ColorStyle.ShapeLBlock
        ActivePiece(1)(5) = ColorStyle.ShapeLBlock
        ActivePiece(0)(5) = ColorStyle.ShapeLBlock
      Case GameShapes.S
        ActivePiece(0)(5) = ColorStyle.ShapeSBlock
        ActivePiece(0)(6) = ColorStyle.ShapeSBlock
        ActivePiece(1)(4) = ColorStyle.ShapeSBlock
        ActivePiece(1)(5) = ColorStyle.ShapeSBlock
      Case GameShapes.Z
        ActivePiece(0)(4) = ColorStyle.ShapeZBlock
        ActivePiece(0)(5) = ColorStyle.ShapeZBlock
        ActivePiece(1)(5) = ColorStyle.ShapeZBlock
        ActivePiece(1)(6) = ColorStyle.ShapeZBlock
      Case GameShapes.T
        ActivePiece(0)(5) = ColorStyle.ShapeTBlock
        ActivePiece(1)(4) = ColorStyle.ShapeTBlock
        ActivePiece(1)(5) = ColorStyle.ShapeTBlock
        ActivePiece(1)(6) = ColorStyle.ShapeTBlock
      Case GameShapes.O
        ActivePiece(0)(4) = ColorStyle.ShapeOBlock
        ActivePiece(0)(5) = ColorStyle.ShapeOBlock
        ActivePiece(1)(4) = ColorStyle.ShapeOBlock
        ActivePiece(1)(5) = ColorStyle.ShapeOBlock
    End Select

    Dim iNew As Integer = Math.Floor(Rnd() * 7)
    Select Case iNew
      Case 0 : statNext = GameShapes.I
      Case 1 : statNext = GameShapes.J
      Case 2 : statNext = GameShapes.L
      Case 3 : statNext = GameShapes.S
      Case 4 : statNext = GameShapes.Z
      Case 5 : statNext = GameShapes.O
      Case 6 : statNext = GameShapes.T
    End Select
  End Sub

#Region "Drawing"
  Private Sub RedrawGame()
    RedrawStats()
    RedrawBoard()
  End Sub

  Private Sub RedrawBoard()
    Using bmpBoard As New Bitmap(160, 320)
      Using g As Graphics = Graphics.FromImage(bmpBoard)
        g.Clear(Color.Black)
        If Paused Or Not Me.ContainsFocus Then
          Dim blockType As ColorStyle = ColorStyle.RowBlock
          Select Case pauseLoop
            Case 10 : blockType = ColorStyle.RowFade10
            Case 9 : blockType = ColorStyle.RowFade9
            Case 8 : blockType = ColorStyle.RowFade8
            Case 7 : blockType = ColorStyle.RowFade7
            Case 6 : blockType = ColorStyle.RowFade6
            Case 5 : blockType = ColorStyle.RowFade5
            Case 4 : blockType = ColorStyle.RowFade4
            Case 3 : blockType = ColorStyle.RowFade3
            Case 2 : blockType = ColorStyle.RowFade2
            Case 1 : blockType = ColorStyle.RowFade1
          End Select

          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 1, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 2, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 1, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 2, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 1, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 1, 8 * 20, 8, 8))

          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 5, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 4, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 6, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 4, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 5, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 6, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 4, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 6, 8 * 20, 8, 8))

          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 8, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 10, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 8, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 10, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 8, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 10, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 8, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 9, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 10, 8 * 20, 8, 8))

          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 12, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 13, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 14, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 12, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 13, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 14, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 12, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 13, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 14, 8 * 20, 8, 8))

          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 16, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 17, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 18, 8 * 17, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 16, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 17, 8 * 18, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 16, 8 * 19, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 16, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 17, 8 * 20, 8, 8))
          g.DrawImage(MakeBlock(blockType), New Rectangle(8 * 18, 8 * 20, 8, 8))

          If FadeIn Then
            pauseLoop -= 1
            If pauseLoop = 0 Then
              FadeIn = False
              pauseLoop = 1
            End If
          Else
            pauseLoop += 1
            If pauseLoop = 11 Then
              FadeIn = True
              pauseLoop = 10
            End If
          End If
        Else
          For y As Integer = 1 To 20
            For x As Integer = 0 To 9
              If Not BaseBoard(y)(x) = ColorStyle.NoBlock Then
                g.DrawImage(MakeBlock(BaseBoard(y)(x)), New Point(16 * x, 16 * (y - 1)))
              ElseIf Not ActivePiece(y)(x) = ColorStyle.NoBlock Then
                g.DrawImage(MakeBlock(ActivePiece(y)(x)), New Point(16 * x, 16 * (y - 1)))
              End If
            Next
          Next
        End If
      End Using
      pctBoard.Image = bmpBoard.Clone
    End Using
  End Sub

  Private statLast As GameShapes
  Private Sub RedrawStats()
    If tmrDisplayUpdate.Enabled Then
      lblLines.Text = "Lines - " & Format(statLines, "000")
      lblLevel.Text = Format(statLevel, "00")
      If Paused Or Not Me.ContainsFocus Then
        pctNextPiece.Image = Nothing
      ElseIf pctNextPiece.Image Is Nothing OrElse Not statNext = statLast Then
        pctNextPiece.Image = DrawDemoPiece(statNext)
        statLast = statNext
      End If
    Else
      lblLines.Text = "GAME OVER - Level " & statLevel
      lblLevel.Text = "00"
      pctNextPiece.Image = Nothing
    End If
    lblScore.Text = Format(statScore, "0,000,000")
  End Sub

  Private Function DrawDemoPiece(shape As GameShapes) As Image
    Select Case shape
      Case GameShapes.I
        Using bmpDemo As New Bitmap(64, 16)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeIBlock), New Point(16 * 0, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeIBlock), New Point(16 * 1, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeIBlock), New Point(16 * 2, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeIBlock), New Point(16 * 3, 16 * 0))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.J
        Using bmpDemo As New Bitmap(48, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeJBlock), New Point(16 * 0, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeJBlock), New Point(16 * 0, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeJBlock), New Point(16 * 1, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeJBlock), New Point(16 * 2, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.L
        Using bmpDemo As New Bitmap(48, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeLBlock), New Point(16 * 2, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeLBlock), New Point(16 * 0, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeLBlock), New Point(16 * 1, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeLBlock), New Point(16 * 2, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.S
        Using bmpDemo As New Bitmap(48, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeSBlock), New Point(16 * 1, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeSBlock), New Point(16 * 2, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeSBlock), New Point(16 * 0, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeSBlock), New Point(16 * 1, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.Z
        Using bmpDemo As New Bitmap(48, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeZBlock), New Point(16 * 0, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeZBlock), New Point(16 * 1, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeZBlock), New Point(16 * 1, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeZBlock), New Point(16 * 2, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.T
        Using bmpDemo As New Bitmap(48, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeTBlock), New Point(16 * 1, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeTBlock), New Point(16 * 0, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeTBlock), New Point(16 * 1, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeTBlock), New Point(16 * 2, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case GameShapes.O
        Using bmpDemo As New Bitmap(32, 32)
          Using g As Graphics = Graphics.FromImage(bmpDemo)
            g.DrawImage(MakeBlock(ColorStyle.ShapeOBlock), New Point(16 * 0, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeOBlock), New Point(16 * 0, 16 * 1))
            g.DrawImage(MakeBlock(ColorStyle.ShapeOBlock), New Point(16 * 1, 16 * 0))
            g.DrawImage(MakeBlock(ColorStyle.ShapeOBlock), New Point(16 * 1, 16 * 1))
          End Using
          Return bmpDemo.Clone
        End Using
      Case Else
        Return Nothing
    End Select
  End Function

  Private Function MakeBlock(base As ColorStyle) As Bitmap
    Using bmpBlock As New Bitmap(16, 16)
      Using g As Graphics = Graphics.FromImage(bmpBlock)
        g.Clear(Color.Black)
        g.DrawRectangle(New Pen(statColors.BGColor(base)), New Rectangle(-1, -1, 16, 16))
        g.DrawRectangle(New Pen(Color.FromArgb(128, Color.Black)), New Rectangle(-1, -1, 16, 16))
        g.DrawRectangle(New Pen(statColors.BGColor(base)), New Rectangle(0, 0, 16, 16))
        g.DrawRectangle(New Pen(Color.FromArgb(128, Color.White)), New Rectangle(0, 0, 16, 16))
        g.FillRectangle(New SolidBrush(statColors.BGColor(base)), New Rectangle(2, 2, 13, 13))
      End Using
      Return bmpBlock.Clone
    End Using
  End Function
#End Region

#Region "High Score"
  Private Sub UpdateHighScore()
    lblTopScore.Text = "Updating"
    lblTopScore.ForeColor = Color.DarkGoldenrod
    wsScore = New Net.WebClient
    wsScore.DownloadStringAsync(New Uri("http://tetris.realityripple.com/tetris.php"), False)
  End Sub

  Private Sub SendHighScore(Score As ULong)
    If Score > statTopScore Then
      lblTopScore.Text = "Submitting"
      lblTopScore.ForeColor = Color.DarkGoldenrod
      Dim sMD5 As String = Nothing
      Using md5 As New Security.Cryptography.MD5Cng
        sMD5 = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.GetEncoding("latin1").GetBytes("tEtrIs" & Score & "sIrtEt"))).ToLower.Replace("-", "")
      End Using
      wsScore = New Net.WebClient
      wsScore.DownloadStringAsync(New Uri("http://tetris.realityripple.com/tetris.php?score=" & Score & "&verify=" & sMD5), True)
    End If
  End Sub

  Private Sub wsScore_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsScore.DownloadStringCompleted
    If e.Error IsNot Nothing Then
      lblTopScore.Text = "Load Error"
      lblTopScore.ForeColor = Color.Red
    ElseIf e.Cancelled Then
      lblTopScore.Text = "Cancelled"
      lblTopScore.ForeColor = Color.Red
    Else
      If String.IsNullOrEmpty(e.Result) Then
        lblTopScore.Text = "0,000,000"
        lblTopScore.ForeColor = Color.IndianRed
      Else
        ULong.TryParse(e.Result, statTopScore)
        If statTopScore = statScore And statTopScore > 0 Then
          lblTopScore.ForeColor = Color.Red
        Else
          lblTopScore.ForeColor = Color.IndianRed
        End If
        lblTopScore.Text = Format(statTopScore, "0,000,000")
      End If
    End If
  End Sub

  Private Sub lblTopScore_Click(sender As System.Object, e As System.EventArgs) Handles lblTopScore.Click
    UpdateHighScore()
  End Sub
#End Region
End Class
