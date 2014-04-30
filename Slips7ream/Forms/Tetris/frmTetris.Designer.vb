<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTetris
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTetris))
    Me.pnlInterface = New System.Windows.Forms.TableLayoutPanel()
    Me.lblLines = New System.Windows.Forms.Label()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlLevel = New System.Windows.Forms.GroupBox()
    Me.lblLevel = New System.Windows.Forms.Label()
    Me.pnlTopScore = New System.Windows.Forms.GroupBox()
    Me.lblTopScore = New System.Windows.Forms.Label()
    Me.pnlScore = New System.Windows.Forms.GroupBox()
    Me.lblScore = New System.Windows.Forms.Label()
    Me.pnlNext = New System.Windows.Forms.GroupBox()
    Me.pctNextPiece = New System.Windows.Forms.PictureBox()
    Me.cmdNew = New System.Windows.Forms.Button()
    Me.pctBoard = New System.Windows.Forms.PictureBox()
    Me.tmrDisplayUpdate = New System.Windows.Forms.Timer(Me.components)
    Me.tmrKeyUpdate = New System.Windows.Forms.Timer(Me.components)
    Me.tmrMove = New System.Windows.Forms.Timer(Me.components)
    Me.tmrRotate = New System.Windows.Forms.Timer(Me.components)
    Me.lblHelp = New System.Windows.Forms.Label()
    Me.pnlInterface.SuspendLayout()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.pnlLevel.SuspendLayout()
    Me.pnlTopScore.SuspendLayout()
    Me.pnlScore.SuspendLayout()
    Me.pnlNext.SuspendLayout()
    CType(Me.pctNextPiece, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pctBoard, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlInterface
    '
    Me.pnlInterface.ColumnCount = 2
    Me.pnlInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlInterface.Controls.Add(Me.lblLines, 0, 0)
    Me.pnlInterface.Controls.Add(Me.TableLayoutPanel1, 1, 1)
    Me.pnlInterface.Controls.Add(Me.pctBoard, 0, 1)
    Me.pnlInterface.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlInterface.Location = New System.Drawing.Point(0, 0)
    Me.pnlInterface.Name = "pnlInterface"
    Me.pnlInterface.RowCount = 2
    Me.pnlInterface.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlInterface.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlInterface.Size = New System.Drawing.Size(319, 357)
    Me.pnlInterface.TabIndex = 0
    '
    'lblLines
    '
    Me.lblLines.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblLines.AutoSize = True
    Me.pnlInterface.SetColumnSpan(Me.lblLines, 2)
    Me.lblLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLines.Location = New System.Drawing.Point(89, 0)
    Me.lblLines.Name = "lblLines"
    Me.lblLines.Size = New System.Drawing.Size(141, 29)
    Me.lblLines.TabIndex = 2
    Me.lblLines.Text = "Lines - 000"
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.ColumnCount = 1
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.pnlLevel, 0, 3)
    Me.TableLayoutPanel1.Controls.Add(Me.pnlTopScore, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.pnlScore, 0, 1)
    Me.TableLayoutPanel1.Controls.Add(Me.pnlNext, 0, 2)
    Me.TableLayoutPanel1.Controls.Add(Me.cmdNew, 0, 4)
    Me.TableLayoutPanel1.Controls.Add(Me.lblHelp, 0, 5)
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(171, 32)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 6
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(145, 320)
    Me.TableLayoutPanel1.TabIndex = 1
    '
    'pnlLevel
    '
    Me.pnlLevel.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlLevel.Controls.Add(Me.lblLevel)
    Me.pnlLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.pnlLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlLevel.ForeColor = System.Drawing.Color.Silver
    Me.pnlLevel.Location = New System.Drawing.Point(62, 203)
    Me.pnlLevel.Name = "pnlLevel"
    Me.pnlLevel.Size = New System.Drawing.Size(80, 45)
    Me.pnlLevel.TabIndex = 0
    Me.pnlLevel.TabStop = False
    Me.pnlLevel.Text = "Level"
    '
    'lblLevel
    '
    Me.lblLevel.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLevel.ForeColor = System.Drawing.Color.White
    Me.lblLevel.Location = New System.Drawing.Point(10, 18)
    Me.lblLevel.Name = "lblLevel"
    Me.lblLevel.Size = New System.Drawing.Size(60, 24)
    Me.lblLevel.TabIndex = 0
    Me.lblLevel.Text = "00"
    Me.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'pnlTopScore
    '
    Me.pnlTopScore.Controls.Add(Me.lblTopScore)
    Me.pnlTopScore.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnlTopScore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.pnlTopScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlTopScore.ForeColor = System.Drawing.Color.Silver
    Me.pnlTopScore.Location = New System.Drawing.Point(3, 3)
    Me.pnlTopScore.Name = "pnlTopScore"
    Me.pnlTopScore.Size = New System.Drawing.Size(139, 45)
    Me.pnlTopScore.TabIndex = 1
    Me.pnlTopScore.TabStop = False
    Me.pnlTopScore.Text = "Top Score"
    '
    'lblTopScore
    '
    Me.lblTopScore.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTopScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTopScore.ForeColor = System.Drawing.Color.IndianRed
    Me.lblTopScore.Location = New System.Drawing.Point(8, 18)
    Me.lblTopScore.Name = "lblTopScore"
    Me.lblTopScore.Size = New System.Drawing.Size(125, 24)
    Me.lblTopScore.TabIndex = 0
    Me.lblTopScore.Tag = ""
    Me.lblTopScore.Text = "0,000,000"
    Me.lblTopScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'pnlScore
    '
    Me.pnlScore.Controls.Add(Me.lblScore)
    Me.pnlScore.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnlScore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.pnlScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlScore.ForeColor = System.Drawing.Color.Silver
    Me.pnlScore.Location = New System.Drawing.Point(3, 54)
    Me.pnlScore.Name = "pnlScore"
    Me.pnlScore.Size = New System.Drawing.Size(139, 45)
    Me.pnlScore.TabIndex = 2
    Me.pnlScore.TabStop = False
    Me.pnlScore.Text = "Score"
    '
    'lblScore
    '
    Me.lblScore.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblScore.ForeColor = System.Drawing.Color.White
    Me.lblScore.Location = New System.Drawing.Point(8, 18)
    Me.lblScore.Name = "lblScore"
    Me.lblScore.Size = New System.Drawing.Size(125, 24)
    Me.lblScore.TabIndex = 0
    Me.lblScore.Text = "0,000,000"
    Me.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'pnlNext
    '
    Me.pnlNext.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pnlNext.Controls.Add(Me.pctNextPiece)
    Me.pnlNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.pnlNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlNext.ForeColor = System.Drawing.Color.Silver
    Me.pnlNext.Location = New System.Drawing.Point(34, 105)
    Me.pnlNext.Name = "pnlNext"
    Me.pnlNext.Size = New System.Drawing.Size(76, 92)
    Me.pnlNext.TabIndex = 3
    Me.pnlNext.TabStop = False
    Me.pnlNext.Text = "Next"
    '
    'pctNextPiece
    '
    Me.pctNextPiece.Location = New System.Drawing.Point(6, 23)
    Me.pctNextPiece.Name = "pctNextPiece"
    Me.pctNextPiece.Size = New System.Drawing.Size(64, 64)
    Me.pctNextPiece.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.pctNextPiece.TabIndex = 0
    Me.pctNextPiece.TabStop = False
    '
    'cmdNew
    '
    Me.cmdNew.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdNew.FlatAppearance.BorderColor = System.Drawing.Color.White
    Me.cmdNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
    Me.cmdNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
    Me.cmdNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdNew.Location = New System.Drawing.Point(22, 261)
    Me.cmdNew.Name = "cmdNew"
    Me.cmdNew.Size = New System.Drawing.Size(101, 35)
    Me.cmdNew.TabIndex = 4
    Me.cmdNew.Text = "New Game"
    Me.cmdNew.UseVisualStyleBackColor = True
    '
    'pctBoard
    '
    Me.pctBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctBoard.Location = New System.Drawing.Point(3, 32)
    Me.pctBoard.Name = "pctBoard"
    Me.pctBoard.Size = New System.Drawing.Size(162, 322)
    Me.pctBoard.TabIndex = 3
    Me.pctBoard.TabStop = False
    '
    'tmrDisplayUpdate
    '
    Me.tmrDisplayUpdate.Interval = 1000
    '
    'tmrKeyUpdate
    '
    Me.tmrKeyUpdate.Enabled = True
    Me.tmrKeyUpdate.Interval = 25
    '
    'tmrMove
    '
    Me.tmrMove.Enabled = True
    '
    'tmrRotate
    '
    Me.tmrRotate.Enabled = True
    '
    'lblHelp
    '
    Me.lblHelp.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblHelp.AutoSize = True
    Me.lblHelp.Location = New System.Drawing.Point(28, 307)
    Me.lblHelp.Name = "lblHelp"
    Me.lblHelp.Size = New System.Drawing.Size(88, 13)
    Me.lblHelp.TabIndex = 5
    Me.lblHelp.Text = "Press F1 for Help"
    '
    'frmTetris
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackColor = System.Drawing.Color.Black
    Me.ClientSize = New System.Drawing.Size(319, 357)
    Me.Controls.Add(Me.pnlInterface)
    Me.ForeColor = System.Drawing.Color.White
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmTetris"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "TETRIS"
    Me.pnlInterface.ResumeLayout(False)
    Me.pnlInterface.PerformLayout()
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.TableLayoutPanel1.PerformLayout()
    Me.pnlLevel.ResumeLayout(False)
    Me.pnlTopScore.ResumeLayout(False)
    Me.pnlScore.ResumeLayout(False)
    Me.pnlNext.ResumeLayout(False)
    CType(Me.pctNextPiece, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pctBoard, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlInterface As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlLevel As System.Windows.Forms.GroupBox
  Friend WithEvents pnlTopScore As System.Windows.Forms.GroupBox
  Friend WithEvents pnlScore As System.Windows.Forms.GroupBox
  Friend WithEvents pnlNext As System.Windows.Forms.GroupBox
  Friend WithEvents lblLines As System.Windows.Forms.Label
  Friend WithEvents lblLevel As System.Windows.Forms.Label
  Friend WithEvents lblTopScore As System.Windows.Forms.Label
  Friend WithEvents lblScore As System.Windows.Forms.Label
  Friend WithEvents tmrDisplayUpdate As System.Windows.Forms.Timer
  Friend WithEvents cmdNew As System.Windows.Forms.Button
  Friend WithEvents pctNextPiece As System.Windows.Forms.PictureBox
  Friend WithEvents pctBoard As System.Windows.Forms.PictureBox
  Friend WithEvents tmrKeyUpdate As System.Windows.Forms.Timer
  Friend WithEvents tmrMove As System.Windows.Forms.Timer
  Friend WithEvents tmrRotate As System.Windows.Forms.Timer
  Friend WithEvents lblHelp As System.Windows.Forms.Label

End Class
