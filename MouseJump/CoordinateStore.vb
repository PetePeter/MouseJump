''' <summary>
''' Stores and retrieves mouse coordinates.
''' </summary>
''' <remarks></remarks>
Public Class CoordinateStore
    Private m_store As New Stack(Of Point)

    ''' <summary>
    ''' Adds a coordinate to the end of the list
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Public Sub Push(p As Point)
        'we don't want to have duplicate points. 
        'having duplicate points is bad.
        'if we find one with a duplicate point we simply 'move it to the top'.
        If (m_store.Contains(p)) Then
            RemovePoint(p)
        End If
        m_store.Push(p)
    End Sub

    ''' <summary>
    ''' Expensive method!
    ''' Converts stack to list, removes item, and converts list back to a stack.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Public Sub RemovePoint(p As Point)
        Dim lst As New List(Of Point)

        While m_store.Count > 0
            lst.Add(m_store.Pop)
        End While

        lst.Remove(p)
        lst.Reverse()

        For Each item As Point In lst
            m_store.Push(item)
        Next
    End Sub

    ''' <summary>
    ''' Returns the head of the stack and then shoves it at the end.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Walk(direction As Integer) As Point
        If (m_store.Count = 0) Then
            Return Point.Empty
        End If

        If (direction = 1) Then
            'move forward :/

            Dim to_ret As Point = m_store.Pop()

            Dim lst As New List(Of Point)

            While m_store.Count > 0
                lst.Add(m_store.Pop)
            End While

            lst.Add(to_ret)
            lst.Reverse()

            For Each item As Point In lst
                m_store.Push(item)
            Next

            Return to_ret
        ElseIf (direction = -1) Then

            'move backwards
            Dim lst As New List(Of Point)
            Dim to_ret As Point

            While m_store.Count > 0
                lst.Add(m_store.Pop)
            End While

            to_ret = lst(lst.Count - 1)
            lst.RemoveAt(lst.Count - 1)

            lst.Reverse()

            For Each item As Point In lst
                m_store.Push(item)
            Next
            m_store.Push(to_ret)

            Return to_ret
        End If

    End Function

    ''' <summary>
    ''' Rets the number of elms on the stack.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count As Integer
        Get
            Return m_store.Count
        End Get
    End Property

    ''' <summary>
    ''' A swap will pop the last point stored, push the current and set current to be the last.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Public Function Swap(p As Point) As Point
        If (m_store.Count = 0) Then
            Return Point.Empty
        End If

        Dim ret As Point
        ret = m_store.Pop()
        m_store.Push(p)
        Return ret
    End Function

    Public Function Pop() As Point
        Dim ret As Point

        If (m_store.Count = 0) Then
            Return Point.Empty
        End If

        ret = m_store.Pop

        Return ret
    End Function

    Public Function ToList() As List(Of Point)
        Return New List(Of Point)(m_store)
    End Function
End Class
