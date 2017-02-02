<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_Image.asp" -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Image Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">Image EXAMPLE</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%cmdOK%>
	<%imgBook%>
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim cmdOK
	Dim imgBook
	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set cmdOK = New_ServerLinkButton("cmdGo")		
		Set imgBook = New_ServerImage("imgBook")		
	End Function

	Public Function Page_Controls_Init()						
		cmdOK.Text = "Uh?"
		imgBook.ImageSrc = "Book01.Gif"
	End Function

%>