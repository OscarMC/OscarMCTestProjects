'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Windows.Forms
Imports System.Drawing

' description:  This form is used to display messages such as successful file saves
' the form area is painted in a gradient fill and opacity is gradually decreased
' then the form is automatically self closed






Friend Class MessageDialogue


	Shared Sub DisplayMessage(ByVal msg As String, _
										ByVal owner As Windows.Forms.Form, _
										Optional ByVal anchor As AnchorStyles = AnchorStyles.Top Or AnchorStyles.Right, _
										Optional ByVal ticksToDisplay As Int32 = 4000, _
										Optional ByVal dialogStyle As DialogueStyle = DialogueStyle.Success)

		Dim frm As New MessageDialogue
		frm.m_msg = msg
		'
		If owner IsNot Nothing Then
			Dim x, y, dx, dy As Int32

			Dim pt As Drawing.Point
			pt = owner.PointToScreen(Drawing.Point.Empty)
			dx = owner.ClientSize.Width
			dy = owner.ClientSize.Height


			If (anchor And AnchorStyles.Left) = AnchorStyles.Left Then
				x = pt.X
			Else 'assume right
				x = pt.X + dx - frm.Width
			End If
			If (anchor And AnchorStyles.Top) = AnchorStyles.Top Then
				y = pt.Y
			Else 'assume bottom
				y = pt.Y + dy - frm.Height
			End If

			frm.StartPosition = FormStartPosition.Manual
			frm.Left = x
			frm.Top = y
		End If
		frm.m_dlgStyle = dialogStyle
		frm.Timer1.Interval = 50
		frm.Timer1.Enabled = True
		frm.m_maxcount = ticksToDisplay \ frm.Timer1.Interval
		frm.m_decreaseOpacity = 1 / (1.1 ^ frm.m_maxcount)
		frm.Show()
		If owner IsNot Nothing Then owner.Focus()
	End Sub


	Private m_msg As String
	Private m_maxcount As Int32 = 100
	Private m_count As Int32 '= 0
	Private m_decreaseOpacity As Double
	Private m_br As Drawing2D.LinearGradientBrush
	Private m_dlgStyle As DialogueStyle



	Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
		MyBase.OnPaint(e)
		If m_br Is Nothing Then
			Select Case m_dlgStyle
				Case DialogueStyle.Success
					m_br = New Drawing2D.LinearGradientBrush(e.ClipRectangle, Color.SteelBlue, Color.Lavender, Drawing2D.LinearGradientMode.Vertical)

				Case DialogueStyle.Failure
					m_br = New Drawing2D.LinearGradientBrush(e.ClipRectangle, Color.Purple, Color.Fuchsia, Drawing2D.LinearGradientMode.Vertical)

				Case DialogueStyle.Information
					m_br = New Drawing2D.LinearGradientBrush(e.ClipRectangle, Color.Gold, Color.Khaki, Drawing2D.LinearGradientMode.Vertical)
			End Select

		End If
		e.Graphics.FillRectangle(m_br, e.ClipRectangle)
		e.Graphics.DrawString(Me.m_msg, Me.Font, Brushes.White, e.ClipRectangle)
	End Sub

	Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
		m_count += 1
		If m_count > m_maxcount Then
			Me.Close()
		Else
			m_decreaseOpacity *= 1.1
			Me.Opacity -= m_decreaseOpacity
			Me.Refresh()
		End If

	End Sub




	Private Sub MessageDialogue_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		If Not m_br Is Nothing Then
			m_br.Dispose()
			m_br = Nothing
		End If
	End Sub


End Class

Public Enum DialogueStyle
   Success = 0
   Failure = 1
   Information = 2
End Enum