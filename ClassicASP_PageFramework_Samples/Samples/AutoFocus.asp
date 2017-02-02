<!--#Include File = "..\WebControl.asp"        -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_CheckBox.asp" -->
<!--#Include File = "..\Server_CheckBoxList.asp" -->
<!--#Include File = "..\Server_DropDown.asp" -->
<!--#Include File = "..\Server_Label.asp"    -->
<!--#Include File = "DBWrapper.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>Auto Focus</TITLE>
<LINK rel="stylesheet" type="text/css" href="Samples.css">
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">Page.AutoResetFocus and AutoPostBack Example</Span>
<Span style="font-size:8pt"><BR>This sample shows how the page is posted when a item is clicked or selected in a dropdown or checkbox and also how the focus is restored to the same control (originator of the postback) once the page is rendered back.
<BR>There is also a neat feature: <B>Page.AutoResetScrollPosition</B> that when set to true will keep the vertical scroll betwen posts...
</Span>


<!--#Include File = "..\FormStart.asp"        -->
	<%lblMessage%><HR>
	<table width=70%>
		<tr>
			<td align=left><%chkCheckBoxList%></td>
			<td align=left valign=top><%cbDropDown%></td>
		</tr>
	</table>
	<HR>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage
	
	Dim chkCheckBoxList
	Dim cbDropDown	
	Page.DebugEnabled = False
	
	Public Function Page_Init()
		Set lblMessage = New_ServerLabel("lblMessage")
		Set chkCheckBoxList = New_ServerCheckBoxList("chkCheckBoxList")		
		Set cbDropDown =  New_ServerDropDown("cbDropDown")
		Page.AutoResetFocus = True
	End Function

	Public Function Page_Controls_Init()						
		lblMessage.Control.Style = "border:1px solid blue;background-color:#EEEEEE;width:100%;font-size:8pt"		
		lblMessage.Text = "This is an Example"
		chkCheckBoxList.DataTextField = "TerritoryDescription"
		chkCheckBoxList.DataValueField = "TerritoryID"
		Set chkCheckBoxList.DataSource = GetRecordset("SELECT TerritoryID,TerritoryDescription FROM Territories ORDER BY 2")		
		chkCheckBoxList.DataBind() 'Loads the items collection (that will stay in the viewstate)...
		Set chkCheckBoxList.DataSource = Nothing 'Clear
		chkCheckBoxList.RepeatColumns=4
		chkCheckBoxList.AutoPostBack = True
		cbDropDown.AutoPostBack=True
		cbDropDown.Multiple = True
		cbDropDown.Rows = 5
	End Function
	
	Public Function Page_PreRender()
		Dim x,mx
		Dim msg 
		Set msg = New StringBuilder
		msg.Append "<B>RepeatColumns:  </B>" & chkCheckBoxList.RepeatColumns &  "<BR>"
		msg.Append "<B>RepeatLayOut:  </B>" & chkCheckBoxList.RepeatLayOut &  "<BR>"
		msg.Append "<B>RepeatDirection:  </B>" & chkCheckBoxList.RepeatDirection  &  "<BR>"
		msg.Append "<HR>"
		mx = chkCheckBoxList.Items.Count -1		
		lblMessage.Text  = msg.ToString()
	End Function

	Public Function cbDropDown_ItemChange(e)
		Dim t,v,s
		cbDropDown.Items.GetItemData cInt(e.Instance),t,v,s
		If CBool(s) Then
			chkCheckBoxList.Items.Add t,v,False
			cbDropDown.Items.Remove CInt(e.Instance)
		End If
	End Function

	
	Public Function chkCheckBoxList_Click(e)
		Dim t,v,s
		chkCheckBoxList.Items.GetItemData e.Instance,t,v,s		
		If CBool(s) Then
			chkCheckBoxList.Items.Remove e.Instance
			cbDropDown.Items.Add t,v,False			
		End If
	End Function

	
%>