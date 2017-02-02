<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_TextBox.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY >
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">Basic Performance Test</Span>
<Span style='font-size:8pt'><BR>
This shows how well the Framework works rendering 100 controls.  It shows how viewstate and is persisted w/o problems and how everything is restored back after postbacks. 
<BR>You can improve the download performance by changing Page.CompressFactor to 1000 bytes (so it runs for sure).
In this scenario the viewstate is about 14 kb, compressed is < 1kb ... good for mobile devices or modem connections...<br>
You can also turn on the Page.DebugEnabled=True to see everything that happens behind the scenes..<Br>
Also, you can change the Page.Control.EnableViewState = False to see how much faster is the rendering without having to deal with the viewstate (click here to try it  <a href='PerformanceTest.asp?V=0'>[no viewstate]</a> )
<BR>You can always enable the viewstate per control...

</span>
</Span>

<!--#Include File = "..\FormStart.asp"        -->
<%cmdPost%><HR>
<table border=1 width="100%">
<% x = 0
	For R = 1 to 10%>
	<tr valign=top>
	<%for C = 1 To 10%>
		<td><%=X%></td><td>
			<%	txtArray(X).Render()
				X = X + 1
			%>
		</td>
	<%Next%>	
	</tr>
<%Next%>
</table>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim txtArray
	Dim cmdPost
	Dim x
	Dim r,c
	
	Public Function Page_Init()
		Page.DebugEnabled = False
		'Page.CompressFactor = 100
		Redim txtArray(100)
		If Request.QueryString("V")<>"" Then
			Page.Control.EnableViewState = False
		End If
		For x = 0 To Ubound(txtArray)
			Set txtArray(X) = New_ServerTextBox("txt" & X)
			txtArray(X).Size=10			
		Next		
		Set cmdPost = New_ServerLinkButton("cmdPost")
		cmdPost.Control.EnableViewState = True 'just this control...
	End Function

	Public Function Page_Controls_Init()
		cmdPost.Text = "Post"
	End Function

	Public Function cmdPost_OnClick()
	
	End Function

	
%>