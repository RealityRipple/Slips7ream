﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOutput
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
    Me.mnuOutput = New System.Windows.Forms.ContextMenu()
    Me.mnuCopy = New System.Windows.Forms.MenuItem()
    Me.mnuClear = New System.Windows.Forms.MenuItem()
    Me.mnuSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuSelectAll = New System.Windows.Forms.MenuItem()
    Me.tmrMove = New System.Windows.Forms.Timer(Me.components)
    Me.txtOutput = New System.Windows.Forms.TextBox()
    Me.SuspendLayout()
    '
    'mnuOutput
    '
    Me.mnuOutput.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCopy, Me.mnuClear, Me.mnuSpacer, Me.mnuSelectAll})
    '
    'mnuCopy
    '
    Me.mnuCopy.Index = 0
    Me.mnuCopy.Text = "&Copy"
    '
    'mnuClear
    '
    Me.mnuClear.Index = 1
    Me.mnuClear.Text = "C&lear"
    '
    'mnuSpacer
    '
    Me.mnuSpacer.Index = 2
    Me.mnuSpacer.Text = "-"
    '
    'mnuSelectAll
    '
    Me.mnuSelectAll.Index = 3
    Me.mnuSelectAll.Text = "Select &All"
    '
    'tmrMove
    '
    Me.tmrMove.Enabled = True
    Me.tmrMove.Interval = 50
    '
    'txtOutput
    '
    Me.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtOutput.Location = New System.Drawing.Point(0, 0)
    Me.txtOutput.Multiline = True
    Me.txtOutput.Name = "txtOutput"
    Me.txtOutput.ReadOnly = True
    Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtOutput.Size = New System.Drawing.Size(424, 91)
    Me.txtOutput.TabIndex = 3
    '
    'frmOutput
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(424, 91)
    Me.Controls.Add(Me.txtOutput)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
    Me.Icon = Global.Slips7ream.My.Resources.Resources.icon
    Me.MinimumSize = New System.Drawing.Size(200, 125)
    Me.Name = "frmOutput"
    Me.Text = "SLIPS7REAM Output Console"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents mnuOutput As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuClear As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
  Friend WithEvents tmrMove As System.Windows.Forms.Timer
  Friend WithEvents txtOutput As System.Windows.Forms.TextBox
End Class
