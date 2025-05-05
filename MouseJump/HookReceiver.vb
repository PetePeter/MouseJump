Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class HookReceiver
    Private m_global_hook As GlobalHooker
    Private WithEvents m_global_mouse As MouseHookListener
    Private WithEvents m_global_keyboard As KeyboardHookListener
    Private m_is_started As Boolean = False
    Private m_lock As New Object()

    Public Event KeyDown(e As KeyEventArgs)
    Public Event KeyPress(e As KeyPressEventArgs)
    Public Event KeyUp(e As KeyEventArgs)
    Public Event MouseClick(e As MouseEventArgs)
    Public Event MouseDoubleClick(e As MouseEventArgs)
    Public Event MouseDown(e As MouseEventArgs)
    Public Event MouseDownExt(e As MouseEventExtArgs)
    Public Event MouseMove(e As MouseEventArgs)
    Public Event MouseMoveExt(e As MouseEventExtArgs)
    Public Event MouseUp(e As MouseEventArgs)
    Public Event MouseWheel(e As MouseEventArgs)

    Public Sub New()
        m_global_hook = New GlobalHooker()
        m_global_mouse = New MouseHookListener(m_global_hook)
        m_global_keyboard = New KeyboardHookListener(m_global_hook)
    End Sub

    ''' <summary>
    ''' Enables or disables the hooking systems.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Enabled As Boolean
        Set(value As Boolean)
            SyncLock m_lock
                m_global_mouse.Enabled = value
                m_global_keyboard.Enabled = value
                m_is_started = value
            End SyncLock
        End Set
        Get
            Return m_is_started
        End Get
    End Property


    Private Sub m_global_keyboard_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles m_global_keyboard.KeyDown
        RaiseEvent KeyDown(e)
    End Sub

    Private Sub m_global_keyboard_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles m_global_keyboard.KeyPress
        RaiseEvent KeyPress(e)
    End Sub

    Private Sub m_global_keyboard_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles m_global_keyboard.KeyUp
        RaiseEvent KeyUp(e)
    End Sub

    Private Sub m_global_mouse_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseClick
        RaiseEvent MouseClick(e)
    End Sub

    Private Sub m_global_mouse_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseDoubleClick
        RaiseEvent MouseDoubleClick(e)
    End Sub

    Private Sub m_global_mouse_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseDown
        RaiseEvent MouseDown(e)
    End Sub

    Private Sub m_global_mouse_MouseDownExt(sender As Object, e As MouseKeyboardActivityMonitor.MouseEventExtArgs) Handles m_global_mouse.MouseDownExt
        RaiseEvent MouseDownExt(e)
    End Sub

    Private Sub m_global_mouse_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseMove
        RaiseEvent MouseMove(e)
    End Sub

    Private Sub m_global_mouse_MouseMoveExt(sender As Object, e As MouseKeyboardActivityMonitor.MouseEventExtArgs) Handles m_global_mouse.MouseMoveExt
        RaiseEvent MouseMoveExt(e)
    End Sub

    Private Sub m_global_mouse_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseUp
        RaiseEvent MouseUp(e)
    End Sub

    Private Sub m_global_mouse_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles m_global_mouse.MouseWheel
        RaiseEvent MouseWheel(e)
    End Sub
End Class
