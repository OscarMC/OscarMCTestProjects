<!--#Include File = "../WebControl.asp"        -->
<!--#Include File = "../Server_LinkButton.asp" -->
<!--#Include File = "../Server_CheckBox.asp" -->
<!--#Include File = "../Server_CheckBoxList.asp" -->
<!--#Include File = "../Server_Label.asp"    -->
<!--#Include File = "DBWrapper.asp"    -->

<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<TITLE>CheckBox and CheckBoxList Example</TITLE>
<!--LINK rel="stylesheet" type="text/css" href="/Samples/Samples.css"-->
<STYLE>
BODY
{
    FONT-FAMILY: Verdana
}
.Caption
{
    FONT-WEIGHT: bolder;
    FONT-SIZE: 12pt
}
TD
{
    FONT-SIZE: 8pt
}
A
{
    FONT-SIZE: 8pt;
    COLOR: black
}
.InputCaption
{
    FONT-WEIGHT: bolder;
    FONT-SIZE: 8pt
}
SPAN
{
    FONT-SIZE: 8pt
}
.LinkCss
{
    FONT-SIZE: 8pt;
    COLOR: fuchsia
}
.LinkCss:hover
{
    FONT-SIZE: 8pt;
    COLOR: red
}
OPTION
{
    BORDER-RIGHT: red 1px solid;
    BORDER-TOP: red 1px solid;
    BORDER-LEFT: red 1px solid;
    BORDER-BOTTOM: red 1px solid
}
</STYLE>
</HEAD>
<BODY>
<!--#Include File = "Home.asp"        -->
<%
	Call Main()
%>	
<Span Class="Caption">CHECKBOX EXAMPLES</Span>

<!--#Include File = "..\FormStart.asp"        -->
	<%lblMessage%><HR>
	<%chkHideShow%> | <%chkTableLayOut%> | <%chkHorizontalDirection%> | <%chkShowGrid%><HR>
	<%chkCheckBoxList%>
	<HR>
	<%cmdAdd%> | <%cmdRemove%> | <%cmdAddColumnOrRow%> | <%cmdRemoveColumnOrRow%>
	<script language="javascript">
		//alert(document.frmForm.chkCheckBoxList.length)
		//alert(document.frmForm.chkCheckBoxList[0].value)
	</script>
<!--#Include File = "..\FormEnd.asp"        -->

</BODY>
</HTML>

<%  'This would normaly go in a another page, but for the sake of simplicity and to minimize the number of pages
    'I'm including code behind stuff here...
	Dim lblMessage
	Dim cmdAdd
	Dim cmdRemove
	Dim cmdAddColumnOrRow
	Dim cmdRemoveColumnOrRow
	
	Dim chkHideShow
	Dim chkTableLayOut
	Dim chkHorizontalDirection
	Dim chkCheckBoxList
	Dim chkShowGrid
	
	Page.DebugEnabled = False

	Public Function Page_Init()

		Set lblMessage = New_ServerLabel("lblMessage")
		Set cmdAdd = New_ServerLinkButton("cmdAdd")
		Set cmdRemove = New_ServerLinkButton("cmdRemove")				
		Set cmdAddColumnOrRow = New_ServerLinkButton("cmdAddColumnOrRow")
		Set cmdRemoveColumnOrRow = New_ServerLinkButton("cmdRemoveColumnOrRow")				
		
		Set chkHideShow = New_ServerCheckBox("chkHideShow")
		Set chkHorizontalDirection  = New_ServerCheckBox("chkHorizontalDirection")
		Set chkTableLayOut = New_ServerCheckBox("chkTableLayOut")
		Set chkCheckBoxList = New_ServerCheckBoxList("chkCheckBoxList")
		Set chkShowGrid  = New_ServerCheckBox("chkShowGrid")
		
	End Function

	Public Function Page_Controls_Init()						
		cmdAdd.Text = "Add"
		cmdRemove.Text = "Remove"

		cmdAddColumnOrRow.Text = "Add Column"
		cmdRemoveColumnOrRow.Text = "Remove Column"

		lblMessage.Control.Style = "border:1px solid blue;background-color:#EEEEEE;width:100%;font-size:8pt"		
		lblMessage.Text = "This is an Example"
		chkHideShow.Caption = "Hide/Show List"
		chkHideShow.AutoPostBack = True
		
		chkTableLayOut.Caption = "Table Layout"
		chkTableLayOut.Checked = True
		chkTableLayOut.AutoPostBack = True
		
		chkHorizontalDirection.Caption = "Horizontal Flow"
		chkHorizontalDirection.AutoPostBack=True
		
		chkShowGrid.Caption = "Show Grid"
		chkShowGrid.AutoPostBack = True
		
		chkCheckBoxList.DataTextField = "TerritoryDescription"
		chkCheckBoxList.DataValueField = "TerritoryID"
		Set chkCheckBoxList.DataSource = GetRecordset("SELECT TerritoryID,TerritoryDescription FROM Territories ORDER BY 2")		
		chkCheckBoxList.DataBind() 'Loads the items collection (that will stay in the viewstate)...
		Set chkCheckBoxList.DataSource = Nothing 'Clear
		chkCheckBoxList.RepeatColumns=4
	End Function
	
	Public Function Page_PreRender()
		Dim x,mx
		Dim msg 
		Set msg = New StringBuilder
		msg.Append "chkHideShow Is checked? " & chkHideShow.Checked & "<BR>"
		msg.Append "<B>RepeatColumns:  </B>" & chkCheckBoxList.RepeatColumns &  "<BR>"
		msg.Append "<B>RepeatLayOut:  </B>" & chkCheckBoxList.RepeatLayOut &  "<BR>"
		msg.Append "<B>RepeatDirection:  </B>" & chkCheckBoxList.RepeatDirection  &  "<BR>"
		msg.Append "<HR>"
		mx = chkCheckBoxList.Items.Count -1
		
		For x = 0 To mx
			If chkCheckBoxList.Items.IsSelected(x) Then
				msg.Append chkCheckBoxList.Items.GetText(x) & " Is Selected, Value:" &  chkCheckBoxList.Items.GetValue(x) & "<BR>"
			End If
		Next
		lblMessage.Text  = msg.ToString()
	End Function

	Public Function chkHideShow_Click()
		chkCheckBoxList.Control.Visible = Not chkCheckBoxList.Control.Visible
	End Function
	
	Public Function chkTableLayOut_Click()
		If  chkTableLayOut.Checked   Then
			chkCheckBoxList.RepeatLayout = 1			
		Else
			chkCheckBoxList.RepeatLayout = 2
		End If		
	End Function

	Public Function chkShowGrid_Click()
		If  chkShowGrid.Checked   Then
			chkCheckBoxList.BorderWidth = 1
		Else
			chkCheckBoxList.BorderWidth=0
		End If		
	End Function

	Public Function chkHorizontalDirection_Click()
		If  chkHorizontalDirection.Checked   Then
			chkCheckBoxList.RepeatDirection=1
		Else
			chkCheckBoxList.RepeatDirection = 2
		End If		
		
	End Function

	Public Function cmdAdd_OnClick()
		chkCheckBoxList.Items.Add chkCheckBoxList.Items.Count,chkCheckBoxList.Items.Count,False
	End Function

	Public Function cmdRemove_OnClick()
		chkCheckBoxList.Items.Remove chkCheckBoxList.Items.Count-1
	End Function
	

	Public Function cmdAddColumnOrRow_OnClick()
			chkCheckBoxList.RepeatColumns = chkCheckBoxList.RepeatColumns  + 1
	End Function

	Public Function cmdRemoveColumnOrRow_OnClick()
		If chkCheckBoxList.RepeatColumns - 1 >0 Then
			chkCheckBoxList.RepeatColumns = chkCheckBoxList.RepeatColumns -1
		End If
	End Function

	
%>