'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System


Public Class TipsForm

   Private Sub TipsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      'TODO: add to the tips html document.
      If Me.WebBrowser1.DocumentText <> My.Resources.Welcome_HTML Then
         Me.WebBrowser1.DocumentText = My.Resources.Welcome_HTML
      End If
   End Sub
End Class