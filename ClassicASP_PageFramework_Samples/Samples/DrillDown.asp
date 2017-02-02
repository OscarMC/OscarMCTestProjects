<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_DataList.asp" -->
<!--#Include File = "..\Server_DataGrid.asp" -->
<!--#Include File = "DBWrapper.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Drill-Down Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">Drill-Down Example</Span>
<Span><BR>Can it be any easier?!. In this sample I'm using the always helpful Page.AutoResetScrollPosition = True to retain vertical scroll, critical when doing this kind of browsing...</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<HR>
	<%objDataList%>
	<HR>
	
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage	
	Dim objDataList
	Dim objOrders
	Dim objOrderDetails
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set objDataList = New_ServerDataList("objDataList")						
		Set objOrders = New_ServerDataGrid("objOrders")
		Set objOrderDetails = New_ServerDataGrid("objOrderDetails")
				
		objOrders.AllowPaging = True
		objOrderDetails.AllowPaging = True
		objOrders.AutoGenerateColumns = False
		
		'Blahh, use some standard templates. This is just an idea... they're ugly though!
		DataGrid_RedTemplate objOrders,True
		DataGrid_BlueTemplate objOrderDetails,True
		
		objOrders.Control.Style = "border-collapse:collapse;width:100%;font-size:8pt"
		objOrderDetails.Control.Style = "border-collapse:collapse;width:100%;font-size:8pt"
		
		objOrders.SelectedItemStyle  = "font-weight:bold"
		Page.AutoResetScrollPosition = True	
		
	End Function

	Public Function Page_Controls_Init()						

		Page.ViewState.Add "ID",0
		objDataList.BorderWidth = 1
		objDataList.RepeatColumns=1
		objDataList.Control.Style = "width:75%;border-collapse:collapse;"
		objDataList.ItemTemplate.Style = "color:green"
		objDataList.AlternatingItemTemplate.Style = "background-color:#DDDDDD"
		
		objDataList.HeaderTemplate.Style = "font-size:12pt;color:white;background-color:#6495ed"
		objDataList.FooterTemplate.Style = "font-size:12pt;color:white;background-color:#6495ed"
		
		objDataList.HeaderTemplate.FunctionName = "fncHeader"
		objDataList.FooterTemplate.FunctionName = "fncFooter"		
		objDataList.ItemTemplate.FunctionName  = "fncItemTemplate"
		objDataList.AlternatingItemTemplate.FunctionName  = "fncAlternateItemTemplate"
		objDataList.SelectedItemTemplate.FunctionName  = "fncSelectedItemTemplate"
				
	End Function
	
	Public Function Page_Load()
			Set objDataList.DataSource = GetRecordSet("Select  CustomerID,CompanyName,ContactName + '/' + ContactTitle As Contact, Address From Customers")
	End Function
	
	Public Function fncHeader()
		Response.Write "<B>Drill-Down sample. Click on a customer to see their orders jkhjk</B>"
	End Function
	
	Public Function fncFooter()
		Response.Write "--The Footer--"
	End Function

	Public Function fncItemTemplate(ds)
			Response.Write " <A " & Page.GetEventScript("HREF", "Page", "SelectRow", ds.AbsolutePosition,"") & ">" & ds(0).Value & "</A>"
			Response.Write "  -" & ds(2).Value
	End Function

	Public Function fncAlternateItemTemplate(ds)		
		Response.Write " <A " & Page.GetEventScript("HREF", "Page", "SelectRow", ds.AbsolutePosition,"") & ">" & ds(0).Value & "</A>"
		Response.Write "  -" & ds(2).Value
	End Function

	Public Function fncSelectedItemTemplate(ds)
		
		Response.Write "<B>" & dS(0).Value & "</B>"
		Response.Write "  -" & ds(2).Value
		Set objOrders.DataSource = GetRecordSet("SELECT OrderID,OrderDate,ShipName FROM Orders WHERE CustomerID='" & ds(0).Value & "'")
		
		If Page.ViewState.GetValue("ID")<> ds(0).Value Then
			objOrders.Pager.PageIndex = 0
			objOrders.SelectedItemIndex = -1
			objOrderDetails.SelectedItemIndex = -1
		End If		
		objOrders.GenerateColumns()
		objOrders.Columns(0).ColumnType = 3
		objOrders.Columns(0).DataValueField = "OrderID"
		objOrders.Columns(0).CellRenderFunctionName = "fncOrderCol0"
		Call objOrders.Render()
		Page.ViewState.Add "ID",ds(0).Value
		
	End Function
	
	Public Function Page_SelectRow(e)
		objDataList.SelectedItemIndex = CInt(e.Instance)
	End Function
	
	Public Function fncOrderCol0(ds)
		Response.Write " <A " & Page.GetEventScript("HREF", "Page", "SelectOrder", ds.AbsolutePosition,"") & ">" & ds(0).Value & "</A>"
		'Render the order details if current row is the selected row
		If objOrders.SelectedItemIndex = ds.AbsolutePosition Then
			Set objOrderDetails.DataSource = GetRecordSet("SELECT P.ProductName as Product , D.UnitPrice as Price, D.Quantity, D.Discount FROM [Order Details] D Join Products P On (D.ProductID=P.ProductID) WHERE OrderID=" & ds(0).Value & "")
			Call objOrderDetails.Render()
		End If
	End Function
	
	Public Function Page_SelectOrder(e)
		objOrders.SelectedItemIndex = CInt(e.Instance)
	End Function
	
%>
