'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' Simple splash screen.
''' </summary>
''' <remarks>If splash screen does not automatically close it's usually a sign of something wrong in the settings bindings elsewhere.
''' </remarks>
Public Class SplashForm



   Private Sub tbnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbnClose.Click
      Me.Close()
   End Sub

   Private Sub tbnClose_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnClose.MouseEnter
      Me.tbnClose.BackgroundImage = My.Resources.img_Close
   End Sub

   Private Sub tbnClose_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnClose.MouseLeave
      Me.tbnClose.BackgroundImage = My.Resources.img_Close_trans
   End Sub


End Class
