'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Windows.Forms



Friend Class InputForm

	Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
		Me.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.Close()
	End Sub

	Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
		Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.Close()
	End Sub

	Public Overloads Function ShowDialog(ByVal title As String, ByVal prompt As String) As DialogResult
		Me.Text = title
		Me.lblPrompt.Text = prompt
		Me.txtInput.SelectAll()
		Me.txtInput.Focus()
		Return Me.ShowDialog
	End Function

	Public Overloads Function ShowDialog(ByVal title As String, ByVal prompt As String, ByVal usertext As String) As DialogResult
		Me.txtInput.Text = usertext
		Return Me.ShowDialog(title, prompt)
	End Function

End Class
