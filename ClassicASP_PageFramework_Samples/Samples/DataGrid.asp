<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_CheckBox.asp" -->
<!--#Include File = "..\Server_DataGrid.asp" -->
<!--#Include File = "DBWrapper.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>DatGrid Example</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<script language="JavaScript">
	function makeNegative(val) {
		var value = 0;
		if(parseFloat(val) != NaN)  {			
			 value = parseFloat(val);
			 return value>0?-value:value;
		}else return NaN //handle error
		
	}
//	alert(makeNegative(1));
//	alert(makeNegative(-1));
//	alert(makeNegative('a'));
	
</script>
<%
	Call Main()
%>
<BR>
<Span Class="Caption">DataGrid Example</Span>
<span><br>Check the code behing and the properties of the ServerDataGrid and the Pager (ServerDataPager). You can change ANYTHING in the look and feel and behavior of the datagrid...
<br>In the page I also commented out a query that returns 800+ rows. You can use it to test the render peformace. Is never good to render that many rows... check how fast it is when you
enable pagination vs not doing it...

<!--#Include File = "..\FormStart.asp"        -->
<%chkAllowPaging%> | <%chkPagerStyle%>
<HR>
<%objDataGrid%>	
<HR>
<%cmdShowDebug%>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim cmdShowDebug
	Dim objDataGrid
	Dim chkAllowPaging
	Dim chkPagerStyle

	
	Page.DebugEnabled =  False
	
	Public Function Page_Init()
		Set cmdShowDebug = New_ServerLinkButton("cmdShowDebug")
		Set objDataGrid = New ServerDataGrid
			objDataGrid.Control.Name = "objDataGrid"
		Set objDataGrid.DataSource = GetRecordSet("Select CustomerID,CompanyName,ContactName + '/' + ContactTitle As Contact, Address From Customers")
		'Set objDataGrid.DataSource = GetRecordSet("SELECT [OrderID], [CustomerID], [OrderDate], [ShipVia], [ShipName] FROM [Northwind].[dbo].[Orders]")


		objDataGrid.ItemStyle = "color:blue"
		objDataGrid.AlternatingItemStyle = "background-color:#DDDDDD"
		objDataGrid.SelectedItemStyle = "background-color:#AAAAAA;color:red"
		objDataGrid.Control.Style = "border-collapse:collapse;width:80%"
		objDataGrid.HeaderStyle = "font-weight:bold;color:white;background-color:#777777"
		objDataGrid.BorderWidth = 1

		objDataGrid.AutoGenerateColumns = False 'To avoid the control from doing this :-)
		objDataGrid.GenerateColumns() 'Do it and I will take over...
		objDataGrid.Columns(0).ColumnType = 3 'Templated Column
		objDataGrid.Columns(0).CellRenderFunctionName = "RenderColumn0"


		'DataGrid_BlueTemplate objDataGrid,True
		
		Set chkAllowPaging = New_ServerCheckBox("chkAllowPaging")
		Set chkPagerStyle  = New_ServerCheckBox("chkPagerStyle")
		'objDataGrid.ShowHeader=False
		Page.AutoResetScrollPosition = True
	End Function

	Public Function Page_Controls_Init()						
		cmdShowDebug.Text = "Post..."
		chkAllowPaging.Caption = "Allow Pagination"
		chkPagerStyle.Caption  = "Multi-Page Pager"
		chkAllowPaging.AutoPostBack=True
		chkPagerStyle.AutoPostBack=True
		objDataGrid.Pager.PagerSize = 5
		objDataGrid.Pager.CurrentPageStyle = "color:red;font-weight:bold"
		'objDataGrid.Pager.PrevText = "<img src='book01.gif' border=1>"
	End Function
	
	Public Function chkAllowPaging_Click()
		objDataGrid.AllowPaging = (chkAllowPaging.Checked)
	End Function
	
	Public Function chkPagerStyle_Click()
		objDataGrid.Pager.PagerType = IIF(chkPagerStyle.Checked,1,0)
	End Function	

	Public Function objDataGrid_ClickColumn0(e)
		objDataGrid.SelectedItemIndex = CInt(e.Instance)
	End Function
	Public Function RenderColumn0(ds)
		 Response.Write " <A style='color:green' " 
		 Response.Write Page.GetEventScript("HREF","objDataGrid","ClickColumn0",ds.AbsolutePosition,"chris")
		 Response.Write " >" & ds(0) & "</a>"		 
	End Function
		
	
%>