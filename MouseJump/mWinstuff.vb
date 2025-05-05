Imports System.Runtime.InteropServices

Module mWinstuff
    Private Structure structRect
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    <DllImport("user32.dll")> _
    Private Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Function GetWindowRect(handle As IntPtr, ByRef rect As structRect) As Boolean
    End Function


    ''' <summary>
    ''' Returns the centre point for the currently active window.
    ''' If there's no currently active window then the point it returns is
    ''' Point.Empty
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetActiveWindowCentre() As Point
        Dim handle As IntPtr = GetForegroundWindow()

        If (handle = IntPtr.Zero) Then
            Return Point.Empty
        Else
            Dim rect As New structRect
            If Not (GetWindowRect(handle, rect)) Then
                'some error, *shrug - ignore
                Return Point.Empty
            Else
                'Calculate the centre of the window
                Dim ret As New Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2)
                Return ret
            End If
        End If
    End Function

    Public Function GetActiveWindowRectangle() As Rectangle
        Dim handle As IntPtr = GetForegroundWindow()

        If (handle = IntPtr.Zero) Then
            Return Rectangle.Empty
        Else
            Dim rect As New structRect
            If Not (GetWindowRect(handle, rect)) Then
                'some error, *shrug - ignore
                Return Rectangle.Empty
            Else
                'Calculate the centre of the window
                Return New Rectangle(rect.Left, rect.Right, rect.Right - rect.Left, rect.Bottom - rect.Top)
            End If
        End If
    End Function
End Module
