
Partial Class search
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim searchbox As TextBox
        searchbox = PreviousPage.FindControl("SearchTerm")

        Label1.Text = searchbox.Text

    End Sub

End Class
