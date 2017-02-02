
Partial Class _06_DatabindExpressions_DatabindExpressions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim adapter As New NorthwindTableAdapters.ProductsTableAdapter

        DataList1.DataSource = adapter.GetProductByCategoryID(1)
        DataList1.DataBind()

    End Sub

    Public Function FormatPrice(ByVal value As Double) As String

        If (value > 15) Then
            Return "<span class='expensive'>$" & value & "</span>"
        Else
            Return "$" & value
        End If

    End Function


End Class
