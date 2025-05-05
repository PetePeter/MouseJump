Imports System.Windows.Forms

''' <summary>
''' The event processor receives events from a hook receiver and does stuff with them.
''' </summary>
''' <remarks></remarks>
Public Class EventProcessor
    Private WithEvents m_hook As HookReceiver

    Private m_delay_max As Integer = 300
    Private m_timestamp_last As Date = Date.MinValue
    Private m_key_last As Keys = Keys.None

    Public Event DoPop()
    Public Event DoPush()
    Public Event DoView()
    Public Event DoSwap()
    public Event DoCentreActiveWindow()
    Public Event DoWalk(direction As Integer)

    ''' <summary>
    ''' Keys to operate.
    ''' 
    ''' Shift to push
    ''' Ctrl to pop
    ''' Alt to swap
    ''' </summary>
    ''' <remarks></remarks>
    Private m_keys_all As Keys() = {Keys.LControlKey, Keys.RControlKey, Keys.LMenu, Keys.RMenu, Keys.LShiftKey, Keys.RShiftKey, Keys.CapsLock}
    Private m_keys_push As Keys() = {Keys.LShiftKey, Keys.RShiftKey}
    Private m_keys_pop As Keys() = {Keys.LControlKey, Keys.RControlKey}
    Private m_keys_swap As Keys() = {}
    Private m_keys_centre As Keys() = {Keys.LMenu, Keys.RMenu}
    Private m_keys_walk As Keys() = {Keys.CapsLock}

    Public Sub New(p_hook As HookReceiver)
        m_hook = p_hook
    End Sub

    ''' <summary>
    ''' Returns true if the last key press was within the time window chosen by m_delay_max
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function check_time(k As Keys) As Boolean
        Dim t As Date = Now
        Dim res As Boolean

        res = t.Subtract(m_timestamp_last).TotalMilliseconds < m_delay_max
        res = res AndAlso (k = m_key_last)

        m_timestamp_last = t
        m_key_last = k
        Return res
    End Function

    ''' <summary>
    ''' On the second up it wil do something.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub m_hook_KeyUp(e As System.Windows.Forms.KeyEventArgs) Handles m_hook.KeyUp
        If (check_time(e.KeyCode)) Then
            If (m_keys_pop.Contains(e.KeyCode)) Then
                RaiseEvent DoPop()
            ElseIf (m_keys_push.Contains(e.KeyCode)) Then
                RaiseEvent DoPush()
            ElseIf (m_keys_swap.Contains(e.KeyCode)) Then
                RaiseEvent DoSwap()
            ElseIf (m_keys_walk.Contains(e.KeyCode)) Then
                RaiseEvent DoWalk(1)
            ElseIf (m_keys_centre.Contains(e.KeyCode)) Then
                RaiseEvent DoCentreActiveWindow()
            End If
        End If
    End Sub



    Private Sub m_hook_MouseDoubleClick(e As System.Windows.Forms.MouseEventArgs) Handles m_hook.MouseDoubleClick
        If (e.Button = MouseButtons.Middle) Then
            RaiseEvent DoView()
        End If
    End Sub

    Private Sub m_hook_MouseWheel(e As System.Windows.Forms.MouseEventArgs) Handles m_hook.MouseWheel
        If (My.Computer.Keyboard.AltKeyDown) Then
            Dim moveby As Integer = 0
            If (e.Delta > 0) Then
                moveby = 1
            ElseIf (e.Delta < 0) Then
                moveby = -1
            End If
            RaiseEvent DoWalk(moveby)
        End If
    End Sub
End Class
