Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Threading

Public Class frmMain
  Private m_hook As New HookReceiver()
  Private WithEvents m_event_proc As EventProcessor
  Private m_store As New CoordinateStore()

  Private Const c_time_pop As Integer = 50
  Private Const c_time_mousedown As Integer = 500 'down for at least a second.
  Private Const c_amt_inflate As Integer = 5
  ''' <summary>
  ''' Used to iterate through the items that are displayed.
  ''' </summary>
  ''' <remarks></remarks>
  Private m_display_ring As New List(Of PictureBox)
  Private m_display_ring_current As Integer = 0
  Private m_cancel_request_id As Integer = 0
  Private m_cancel_request_id_max As Integer = 0
  Private m_cancel_middle_click As Boolean = False
  Private m_lock As New Object()

  Public Sub New()
    InitializeComponent()
    gui_notify.Icon = My.Resources.IconNotify
    Me.DoubleBuffered = True

    m_event_proc = New EventProcessor(m_hook)
  End Sub

  ''' <summary>
  ''' On double click show the main form.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub gui_notify_DoubleClick(sender As Object, e As System.EventArgs) Handles gui_notify.DoubleClick
    Me.Visible = Not (Me.Visible)
  End Sub

  ''' <summary>
  ''' Called to centre the mouse in the current active window.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub m_event_proc_DoCentreActiveWindow() Handles m_event_proc.DoCentreActiveWindow
    Dim centre As Point = GetActiveWindowCentre()

    If (centre = Point.Empty) Then
      'it couldn't get the centre for whatever reason.
      Return
    Else
      'set the centre yay.
      Cursor.Position = centre
    End If
  End Sub

  Private Sub m_event_proc_DoPop() Handles m_event_proc.DoPop
    If (TryChangeOrWarning()) Then
      Dim ret As Point = m_store.Pop()
      If (ret <> Point.Empty) Then
        Cursor.Position = ret
        Me.BeginInvoke(
             Sub()
               do_balloon("Cur loc popped")
             End Sub)
      Else
        Me.BeginInvoke(
             Sub()
               do_balloon("Stack empty")
             End Sub)
      End If
    End If
  End Sub

  Private Sub m_event_proc_DoPush() Handles m_event_proc.DoPush
    If (TryChangeOrWarning()) Then
      m_store.Push(Cursor.Position)
      Me.BeginInvoke(
              Sub()
                do_balloon("Cur loc saved")
              End Sub)
    End If
  End Sub

  Private Sub m_event_proc_DoSwap() Handles m_event_proc.DoSwap
    If (TryChangeOrWarning()) Then
      Dim pos As Point = m_store.Swap(Cursor.Position)
      If (pos <> Point.Empty) Then
        Cursor.Position = pos
        Me.BeginInvoke(
            Sub()
              do_balloon("Cur loc swapped")
            End Sub)
      Else
        Me.BeginInvoke(
            Sub()
              do_balloon("Stack empty")
            End Sub)
      End If
    End If
  End Sub

  Private Sub do_balloon(msg As String)
    gui_notify.ShowBalloonTip(c_time_pop, "", msg, ToolTipIcon.None)
  End Sub

  Private Sub gui_mnu_exit_Click(sender As Object, e As System.EventArgs) Handles gui_mnu_exit.Click
    gui_mnu_enable.Checked = False
    Application.Exit()
  End Sub

  Private Sub gui_mnu_enable_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles gui_mnu_enable.CheckedChanged
    m_hook.Enabled = gui_mnu_enable.Checked
  End Sub

  Private Sub m_event_proc_DoView() Handles m_event_proc.DoView
    Me.BeginInvoke(
        Sub()
          do_view()
        End Sub)
  End Sub


  Private Sub do_view()
    If (Me.Visible) Then
      Me.Visible = False
    Else
      sizeme()
      placeme()

      Me.TopMost = True
      Me.Visible = True
      Me.Activate()
    End If
  End Sub


#Region "Display stuff"

  Private Sub frmMain_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
    m_cancel_request_id_max = 0
    m_cancel_request_id = 0
  End Sub

  Private Sub frmMain_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    m_hook.Enabled = False
  End Sub
  Private Sub frmDisplay_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    hideme()
  End Sub

  Private Sub do_add_new_thread(id As Integer)
    Thread.Sleep(c_time_mousedown)
    Dim match As Boolean

    SyncLock m_lock
      match = (id = m_cancel_request_id)
    End SyncLock

    If match Then
      Me.BeginInvoke(New Action(AddressOf do_add_new))
    End If
  End Sub

  Private Sub do_mouse_up(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
    m_cancel_request_id = 0
  End Sub
  Private Sub do_mouse_down(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

    If (e.Button = Windows.Forms.MouseButtons.Middle) Then
      m_cancel_middle_click = False

      Dim id As Integer
      m_cancel_request_id_max += 1
      id = m_cancel_request_id_max
      SyncLock m_lock
        m_cancel_request_id = id
      End SyncLock
      Dim t As New Thread(AddressOf do_add_new_thread)
      t.Start(id)
    End If
  End Sub


  ''' <summary>
  ''' Adds a new point at the current location, redraws gui, does not dismiss
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub do_add_new()
    If (TryChangeOrWarning()) Then
      m_cancel_middle_click = True

      m_store.Push(Cursor.Position)
      placeme()
    End If
  End Sub

  Private Sub do_click(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
    If (m_display_ring.Count = 0) Then
      Return
    End If

    If (e.Button = Windows.Forms.MouseButtons.Left) Then
      'left click removes the item from the stack and puts it on the top, and jumps to it.
      'this does not change the stack order.
      Dim item As PictureBox = m_display_ring(m_display_ring_current)
      Dim pt As Point = CType(item.Tag, Point)
      Cursor.Position = pt

      'push will throw up to the top a point that isn't already at the top.
      m_store.Push(pt)

      'As a minimum, instead of place me it will have to update which one is marked as the nearest.
      placeme()
      do_view()
    ElseIf (e.Button = Windows.Forms.MouseButtons.Right) Then
      If (TryChangeOrWarning()) Then
        'if we right click on the item that is selected then we only remove and not 
        'jump to nor dismiss.
        Dim item As PictureBox = m_display_ring(m_display_ring_current)
        Dim pt As Point = CType(item.Tag, Point)

        m_store.RemovePoint(pt)


        If sender IsNot item Then
          'if the sender was not the selected item then we jump to it and close.
          'a kind of pop.
          Cursor.Position = pt
          do_view()
        Else
          'if the sender was the selected item then we simply refresh as the user wanted only to 
          'remove it but neither jump nor dismiss.
          placeme()
        End If
      End If
    ElseIf e.Button = Windows.Forms.MouseButtons.Middle AndAlso (Not (m_cancel_middle_click)) Then
      'If they press the middle button then we push their current location in the stack.
      'and jump to the selected one.
      If (TryChangeOrWarning()) Then
        m_store.Push(Cursor.Position)
        Dim item As PictureBox = m_display_ring(m_display_ring_current)
        Cursor.Position = CType(item.Tag, Point)
        do_view()
      End If
    End If
  End Sub

  Private Sub do_wheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
    If (m_display_ring.Count = 0) Then
      Return
    End If

    'the wheel will move up and down through the ring of points stored.
    Dim moveby As Integer
    If (e.Delta > 0) Then
      moveby = 1
    ElseIf (e.Delta < 0) Then
      moveby = -1
    End If

    Dim old_item As PictureBox = m_display_ring(m_display_ring_current)

    m_display_ring_current += moveby
    While (m_display_ring_current < 0)
      m_display_ring_current += m_display_ring.Count
    End While
    While (m_display_ring_current >= m_display_ring.Count)
      m_display_ring_current -= m_display_ring.Count
    End While


    Dim new_item As PictureBox = m_display_ring(m_display_ring_current)

    If (new_item IsNot old_item) Then
      old_item.BackgroundImage = My.Resources.PointNormal
      new_item.BackgroundImage = My.Resources.PointCurrent
    End If
  End Sub

  Private Sub frmDisplay_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
    hideme()
  End Sub

  ''' <summary>
  ''' Gets the point from the store after converting to form space.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function get_store_pts() As List(Of Tuple(Of Point, Point))
    Dim ret As New List(Of Tuple(Of Point, Point))

    For Each pt As Point In m_store.ToList
      Dim conv_pt = Me.PointToClient(pt)
      'pt.X -= Me.Left
      'pt.Y -= Me.Top

      ret.Add(New Tuple(Of Point, Point)(pt, conv_pt))
    Next

    Return ret
  End Function

  ''' <summary>
  ''' Called to place the cursor marks on the screen.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub placeme()
    'Clear screen and prep to add new.
    Me.SuspendLayout()
    For Each c As Control In Me.Controls
      RemoveHandler c.MouseClick, AddressOf do_click
      RemoveHandler c.MouseWheel, AddressOf do_wheel
      RemoveHandler c.MouseUp, AddressOf do_mouse_up
      RemoveHandler c.MouseDown, AddressOf do_mouse_down
    Next
    Me.Controls.Clear()

    m_display_ring.Clear()

    If (m_store.Count > 0) Then
      Dim pts As List(Of Tuple(Of Point, Point)) = get_store_pts()

      'The current is the top of the once-stack.
      m_display_ring_current = 0

      For ix As Integer = 0 To pts.Count - 1
        Dim c As New PictureBox
        Dim pt As Point = pts(ix).Item1
        Dim conv_pt As Point = pts(ix).Item2

        c.Tag = pt
        AddHandler c.MouseClick, AddressOf do_click
        AddHandler c.MouseWheel, AddressOf do_wheel
        AddHandler c.MouseUp, AddressOf do_mouse_up
        AddHandler c.MouseDown, AddressOf do_mouse_down

        c.BackColor = Color.Transparent
        If (ix = m_display_ring_current) Then
          c.BackgroundImage = My.Resources.PointCurrent
        Else
          c.BackgroundImage = My.Resources.PointNormal
        End If

        m_display_ring.Add(c)

        'Size and centre the point.
        c.Width = c.BackgroundImage.Width
        c.Height = c.BackgroundImage.Height
        c.Left = conv_pt.X - (c.Width / 2)
        c.Top = conv_pt.Y - (c.Height / 2)

        Me.Controls.Add(c)
      Next
    End If

    Me.ResumeLayout()

  End Sub



  ''' <summary>
  ''' Finds the index of the pt in pts that is closest to cur.
  ''' </summary>
  ''' <param name="pts"></param>
  ''' <param name="cur"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function find_closest_pt(pts As List(Of Tuple(Of Point, Point)), cur As Point) As Integer
    Dim ret As Integer = 0
    Dim last_distance = Integer.MaxValue
    Dim ix As Integer = 0

    For Each pt As Tuple(Of Point, Point) In pts
      Dim diff As Point = Point.Subtract(pt.Item1, cur)
      Dim distance As Integer

      distance = diff.X * diff.X + diff.Y * diff.Y
      If (distance < last_distance) Then
        ret = ix
        last_distance = distance
      End If

      ix += 1
    Next

    Return ret
  End Function

  Private Sub sizeme()
    'If (False) Then
    Dim rect As New Rectangle()

    For Each s As Screen In Screen.AllScreens
      rect = Rectangle.Union(s.WorkingArea, rect)
    Next

    Me.Left = rect.Left
    Me.Top = rect.Top
    Me.Width = rect.Width
    Me.Height = rect.Height
    'End If
  End Sub

  Private Sub frmDisplay_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Me.Visible = False
  End Sub

  Sub hideme()
    Me.Visible = False
  End Sub
#End Region

  ''' <summary>
  ''' Do walk is invoked to jump the mouse between stored positions.
  ''' 
  ''' If 1 stored position then walk jumps to it and conserves the point.
  ''' If >1 stored position then it will 'walk' thorugh the stack.
  ''' - Removes head and puts it at the end.
  ''' - It's a slow operation but meh.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub m_event_proc_DoWalk(direction As Integer) Handles m_event_proc.DoWalk
    Dim p As Point
    p = m_store.Walk(direction)
    If (p = Point.Empty) Then
      'If no point in stack then do nothing
      Return
    End If
    Cursor.Position = p
  End Sub

  ''' <summary>
  ''' Retunrs True if it's okay to change the point.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function TryChangeOrWarning() As Boolean
    If (IsLocked) Then
      Me.BeginInvoke(
          Sub()
            gui_notify.ShowBalloonTip(c_time_pop, "", "It's locked", ToolTipIcon.Warning)
          End Sub
      )
      Me.BeginInvoke(
          Sub()
            Dim c As Color = Me.BackColor
            Me.BackColor = Color.Red
            Me.Update()
            Thread.Sleep(c_time_pop)
            Me.BackColor = c
            Me.Update()
          End Sub)
      Return False
    Else
      Return True
    End If
  End Function

  ''' <summary>
  ''' Gets whether it's locked.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property IsLocked As Boolean
    Get
      Return gui_mnu_lock.Checked
    End Get
  End Property
End Class
