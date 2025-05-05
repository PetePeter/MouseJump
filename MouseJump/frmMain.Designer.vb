<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.gui_notify = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gui_menu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.gui_mnu_enable = New System.Windows.Forms.ToolStripMenuItem()
        Me.gui_mnu_lock = New System.Windows.Forms.ToolStripMenuItem()
        Me.gui_mnu_exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.gui_menu.SuspendLayout()
        Me.SuspendLayout()
        '
        'gui_notify
        '
        Me.gui_notify.ContextMenuStrip = Me.gui_menu
        Me.gui_notify.Text = "AMethodOfPointSelectionAndApparatus"
        Me.gui_notify.Visible = True
        '
        'gui_menu
        '
        Me.gui_menu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gui_mnu_enable, Me.gui_mnu_lock, Me.gui_mnu_exit})
        Me.gui_menu.Name = "gui_menu"
        Me.gui_menu.Size = New System.Drawing.Size(110, 70)
        '
        'gui_mnu_enable
        '
        Me.gui_mnu_enable.Checked = True
        Me.gui_mnu_enable.CheckOnClick = True
        Me.gui_mnu_enable.CheckState = System.Windows.Forms.CheckState.Checked
        Me.gui_mnu_enable.Name = "gui_mnu_enable"
        Me.gui_mnu_enable.Size = New System.Drawing.Size(109, 22)
        Me.gui_mnu_enable.Text = "&Enable"
        Me.gui_mnu_enable.ToolTipText = "Enable listening to keyboard/mouse events"
        '
        'gui_mnu_lock
        '
        Me.gui_mnu_lock.CheckOnClick = True
        Me.gui_mnu_lock.Name = "gui_mnu_lock"
        Me.gui_mnu_lock.Size = New System.Drawing.Size(109, 22)
        Me.gui_mnu_lock.Text = "&Lock"
        Me.gui_mnu_lock.ToolTipText = "If checked, prevents points from being added/moved. You can still jump to points " & _
    "but you can't change them."
        '
        'gui_mnu_exit
        '
        Me.gui_mnu_exit.Name = "gui_mnu_exit"
        Me.gui_mnu_exit.Size = New System.Drawing.Size(109, 22)
        Me.gui_mnu_exit.Text = "E&xit"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.ControlBox = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMain"
        Me.Opacity = 0.7R
        Me.ShowInTaskbar = False
        Me.Text = "BookMouse"
        Me.gui_menu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gui_notify As System.Windows.Forms.NotifyIcon
    Friend WithEvents gui_menu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents gui_mnu_enable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gui_mnu_exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gui_mnu_lock As System.Windows.Forms.ToolStripMenuItem

End Class
