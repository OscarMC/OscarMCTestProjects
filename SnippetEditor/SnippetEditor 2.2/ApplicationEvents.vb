'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Diagnostics
Imports Microsoft.VisualBasic


Namespace My

   ' general TODO's can go here:

   'TODO: Check app for accessibility

   'TODO: check app on XP x64



   Partial Friend Class MyApplication


		Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

			' HACK: for testing set the Culture and UICulture
			'My.Application.ChangeCulture("nl-NL")
			'My.Application.ChangeUICulture("nl")

			If e.CommandLine.Count > 0 Then
				OpenExisitngSnippet(e.CommandLine.Item(0))
			End If
		End Sub



      Private Sub OpenExisitngSnippet(ByVal arg As String)
         Try
            'Remove quotes
            If (arg.StartsWith(ControlChars.Quote) AndAlso _
               arg.EndsWith(ControlChars.Quote) AndAlso arg.Length > 3) Then
               'Remove quotes
               arg = arg.Remove(0, 1)
               arg = arg.Remove(arg.Length - 1, 1)
               arg = arg.Trim
            End If

            My.Forms.MainForm.LoadCustomFile(arg)

         Catch ex As Exception

            'TODO:  Do we need better exception handling here ?
            MsgBox(ex.ToString)

         End Try
      End Sub


   End Class

End Namespace

