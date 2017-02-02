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
''' "About" form
''' </summary>
''' <remarks>showns when the user clicks on "About" from the help menu. 
''' When the user mouses over the animation in the centre, the text expands.</remarks>
Public Class AboutForm



   Private Sub tbnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbnClose.Click
      Me.Close()
   End Sub

   Private Sub tbnClose_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnClose.MouseEnter
      Me.tbnClose.BackgroundImage = My.Resources.img_Close
   End Sub

   Private Sub tbnClose_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnClose.MouseLeave
      Me.tbnClose.BackgroundImage = My.Resources.img_Close_trans
   End Sub


   Private Sub AboutForm_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
      Me.wbMessage.Hide()
   End Sub

   Private Sub PictureBox1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseEnter
      GrowAndShowControl(Me.wbMessage)
   End Sub

   Private Sub AboutForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      lblVersion.Text = My.Application.Info.Version.ToString
      If Me.wbMessage.DocumentText <> My.Resources.About_HTML Then
         Me.wbMessage.DocumentText = My.Resources.About_HTML
      End If
   End Sub



   Private Sub GrowAndShowControl(ByVal cntl As Control)
      Dim rect = cntl.Bounds
      Dim x, y, w, h As Int32
      Dim frames As Int32 = 50
      For i As Int32 = 1 To frames
         x = rect.X + (((frames - i) * rect.Width) \ (2 * frames))
         y = rect.Y + (((frames - i) * rect.Height) \ (2 * frames))
         w = (i * rect.Width) \ frames
         h = (i * rect.Height) \ frames
         cntl.Bounds = New Rectangle(x, y, w, h)
         If Not cntl.Visible Then cntl.Visible = True
         cntl.Refresh()
         Threading.Thread.Sleep(1)
      Next
   End Sub


End Class
