<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_Label.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Forms Authentication Sample</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">Please Login</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%lblMessage%><HR>
	<%cmdLinkButton%>
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage
	Dim cmdLinkButton
	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set lblMessage = New_ServerLabel("lblMessage")
		Set cmdLinkButton = New_ServerLinkButton("cmdLinkButton")		
	End Function

	Public Function Page_Controls_Init()						
		cmdLinkButton.Text = "Login"
		lblMessage.Text = "Click on the login button to  access the page requested..."
		lblMessage.Control.Style = "font-size:14pt;color:red"
	End Function

	Public Function cmdLinkButton_OnClick()
		'Do your authentication and then set the ticket... I'm "auto" adding the ticket...
		Security.SetAuthCookie "Christian",Array("Admin","Owner"),False
		Security.RedirectFromLoginPage ""
	End Function
	

%>