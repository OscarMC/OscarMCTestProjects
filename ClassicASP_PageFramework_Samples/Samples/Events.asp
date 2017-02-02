<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Events Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->

<BR>
<Span Class="Caption">Events EXAMPLES<HR></Span>

<%
	Call Main()
%>	

<!--#Include File = "..\FormStart.asp"        -->
	<HR>
	<%cmdShowDebug%> | 
	<A <%=Page.GetEventScriptRedirect("HREF", "Page", "RedirectedPB", "1", "Hi","Events.asp")%>>Redirected PostBack</A>
	| <A <%=Page.GetEventScript("HREF", "Page", "ManualPostBack", "1", "Hi")%>>Manual Event...</A>
	
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>
<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...

	Dim cmdShowDebug
	'Page.DebugEnabled =  false
	Public Function Page_Authenticate_Request()
		Response.Write "* Page_Authenticate_Request<BR>"
		'If Not Security Is Nothing Then
		'	Response.Write "<span style='color:red;font-size:8pt'>Forms Authentication is on, this event is overriding the forms authentication... just if you are wondering :-)<BR></span>"
		'End If
	End Function
	
	Public Function  Page_Authorize_Request()
		Response.Write "* Page_Authorize_Request<BR>"
	End Function
	
	Public Function Page_Init()
		Response.Write "* Page_Init<BR>"		
		Set cmdShowDebug = New_ServerLinkButton("cmdShowDebug")
	End Function

	Public Function Page_Controls_Init()						
		Response.Write "* Page_Controls_Init<BR>"
		cmdShowDebug.Text = "Post..."
	End Function
	
	Public Function Page_LoadViewState()
		Response.Write "* Page_LoadViewState<BR>"
	End Function
	
	Public Function Page_Load()
		Response.Write "* Page_Load<BR>"
	End Function
		
	Public Function Page_PreRender()
		Response.Write "* Page_PreRender<BR>"
	End Function

	Public Function Page_SaveViewState()
		Response.Write "* Page_SaveViewState<BR>"
	End Function

	Public Function Page_Terminate()
		Response.Write "* Page_Terminate<BR>"
	End Function

	Public Function cmdShowDebug_OnClick()
		Page.DebugEnabled = True
	End Function
	
	Public Function Page_ManualPostBack(e)
		Response.Write "<HR><B>You clicked on a manual post... check the event data...</B><HR>"
	End Function
	
%>