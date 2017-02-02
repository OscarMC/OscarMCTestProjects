<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "UserControl.asp" -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">COMPOSITE USER CONTROLS</Span>
<Span style='font-size:8pt'><BR>This user control ("LoginControl") is made up of server text boxes and server link button...!
<br>You can look at the LoginControl control in usercontrol.asp to see how easy is to build one.
</Span>

<!--#Include File = "..\FormStart.asp"        -->
<table border=2>

<%	x = 0
	For r = 1 to 5
%>
		<tr valign=top>
		<%For c = 1 To 2%>
			<td><%ucLogin(X).Render()
				x = x + 1
			%></td>
		<%Next%>
		</tr>
	<%	
	Next
	%>
</table>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim  ucLogin
	Dim x,c,r

	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Redim ucLogin(10)
		For x = 0 To 10
			Set ucLogin(x) = New LoginControl
			ucLogin(x).Control.Name = "Login"	& x
		Next
	End Function

	Public Function Page_Controls_Init()
	End Function
	
	Public Function Login0_Login_OnClick()
		'Response.Write "Login..."
	End Function


%>