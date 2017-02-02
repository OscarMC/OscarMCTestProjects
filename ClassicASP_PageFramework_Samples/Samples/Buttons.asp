<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_Button.asp" -->
<!--#Include File = "..\Server_ImageButton.asp" -->
<!--#Include File = "..\Server_Label.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Server Buttons Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="/PageFrameworkV2/Samples/Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">BUTTONS EXAMPLE</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%lblMessage%><HR>
	<%cmdLinkButton%> | <%cmdButton%> | <%cmdImageButton%>
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage
	Dim cmdLinkButton
	Dim cmdButton
	Dim cmdImageButton
	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set lblMessage = New_ServerLabel("lblMessage")
		Set cmdLinkButton = New_ServerLinkButton("cmdLinkButton")		
		Set cmdButton = New_ServerButton("cmdButton")		
		Set cmdImageButton = New_ServerImageButton("cmdImageButton")		
	End Function

	Public Function Page_Controls_Init()						
		cmdButton.Text = "Normal Button"
		cmdLinkButton.Text = "Link Button"
		'cmdLinkButton.ImageURL = "book01.gif"

		cmdImageButton.Image = "../book.gif"
		cmdImageButton.RollOverImage = "../clear_all.gif"
		lblMessage.Control.Style = "border:1px solid blue;background-color:#AAAAAA"
	End Function

	Public Function cmdButton_OnClick()
		lblMessage.Text = "You Clicked a normal button"
	End Function

	Public Function cmdLinkButton_OnClick()
		lblMessage.Text = "You Clicked  Link Button"
	End Function
	
	Public Function cmdImageButton_OnClick()
		lblMessage.Text = "You Clicked on a Image Button"
	End Function


%>