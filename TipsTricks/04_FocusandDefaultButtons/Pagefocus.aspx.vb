
Partial Class Pagefocus
    Inherits System.Web.UI.Page

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click

        If Not Panel1.Visible Then
            Panel1.Visible = True
            LinkButton1.Text = "Hide Panel"
            TextBox4.Focus()
        Else
            Panel1.Visible = False
            LinkButton1.Text = "Show Panel"
            Page.SetFocus(TextBox1)
        End If

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Label1.Text = "Hello " & TextBox4.Text
    End Sub
End Class
