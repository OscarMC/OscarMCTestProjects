Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class SampleWebService
     Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function CalculateValue(ByVal value As Integer) As String
        Return "Value is: " & value
    End Function

End Class
