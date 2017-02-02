
Partial Class _01_CrossPagePostback_CrossPagePostback_vb
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Label1.Text = "Hi " & TextBox1.Text & " you selected: " & Calendar1.SelectedDate
    End Sub

End Class
