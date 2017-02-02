<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_Label.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Server Label Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">LABEL EXAMPLE</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%lblMessage%><BR>
	<%lblFancyLabel%><HR>
	<%cmdGo%> | <%cmdIncrement%>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage
	Dim lblFancyLabel
	Dim cmdGo
	Dim cmdIncrement
	Dim intCounter
	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set lblMessage = New_ServerLabel("lblMessage")
		Set lblFancyLabel = New_ServerLabel("lblFancyLabel")
		
		Set cmdGo = New_ServerLinkButton("cmdGo")		
		Set cmdIncrement  = New_ServerLinkButton("cmdIncrement")		
	End Function

	Public Function Page_Controls_Init()						
		intCounter = 0
		cmdGo.Text = "Do nothing..."
		cmdIncrement.Text = "Increment"
		lblFancyLabel.Control.Style = "border:1px solid blue;background-color:#AAAAAA"
		Page.ViewState.Add "Count",intCounter		
	End Function
	
	Public Function Page_Load()
		If Page.IsPostBack Then			
			intCounter = CInt(Page.ViewState.GetValue("Count"))
		End If
	End Function
	
	Public Function cmdIncrement_OnClick()
		lblMessage.Text = "Hello there " & intCounter
		lblFancyLabel.Text = lblMessage.Text
		intCounter = intCounter + 1
		Page.ViewState.Add "Count",intCounter
	End Function


%>