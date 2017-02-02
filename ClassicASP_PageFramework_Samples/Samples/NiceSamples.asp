<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_DataList.asp" -->
<!--#Include File = "..\Server_Label.asp"    -->
<!--#Include File = "DBWrapper.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<meta http-equiv="Page-Enter" content="blendtrans(duration=2.0)">

<TITLE>Nice..</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
<Style>

	.NewsLink { color:black;text-decoration:none }
	.NewsLink:hover { color:black;text-decoration:underline }
</Style>
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->

<script language="JavaScript">
	function onOver(obj,color) {
		obj.style.backgroundColor = color;
	}
	function onOut(obj,color) {
		obj.style.backgroundColor = color;
	}
</script>

<%
	Call Main()
%>	
<Span Class="Caption">Nice...</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%objDataList%>
	<HR>	
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim objDataList
	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set objDataList = New_ServerDataList("objDataList")		
	End Function

	Public Function Page_Controls_Init()						

		objDataList.RepeatColumns = 1 'Default
		
		objDataList.Control.Style = "border-collapse:collapse;border: #AAAAAA 1px solid"
		objDataList.ItemTemplate.Style = "background-color:#F6D7CE;width:100%;cursor:hand;padding:5px"
		objDataList.AlternatingItemTemplate.Style = "background-color:#F6D7CE;width:100%;cursor:hand;padding:5px"
				
		objDataList.BorderWidth = 1
		objDataList.CellSpacing = 0
		objDataList.BorderColor = "#EDB5A6"
		objDataList.CellPadding = 0

		
		objDataList.HeaderTemplate.Style = "border:red 1px solid"		
		objDataList.HeaderTemplate.FunctionName = "fncHeader"
		objDataList.ItemTemplate.FunctionName  = "fncItemTemplate"
		objDataList.AlternatingItemTemplate.FunctionName  = "fncAlternateItemTemplate"
		
		objDataList.ItemTemplate.ExtraAttributes = 	"onmouseover=""onOver(this,'#EDB5A6');"" onmouseout=""onOut(this,'#F6D7CE');"""
		objDataList.AlternatingItemTemplate.ExtraAttributes = 	"onmouseover=""onOver(this,'#EDB5A6');"" onmouseout=""onOut(this,'#F6D7CE');"""
		
		
	End Function
	
	Public Function Page_PreRender()
		Set objDataList.DataSource = GetRecordSet("Select  top 10 getdate() as dt, CompanyName From Customers")
	End Function

	Public Function fncHeader()
		Response.Write "<img src='images/header.gif'>"
	End Function
	

	Public Function fncItemTemplate(ds)
		Response.Write "<span style='color:red;font-weight:bold'>" & FormatDateTime(ds(0).Value,vbLongTime) & "</span>&nbsp;<a class='NewsLink' href='http://www.eud.com'>" & ds(1).Value  & "</a>"
	End Function

	Public Function fncAlternateItemTemplate(ds)				
		Response.Write "<span style='color:red;font-weight:bold'>" & FormatDateTime(ds(0).Value,vbLongTime) & "</span>&nbsp;<a class='NewsLink' href='http://www.eud.com'>" & ds(1).Value  & "</a>"
	End Function


	
%>

