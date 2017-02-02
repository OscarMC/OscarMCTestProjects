<!--#Include File = "Server_DataPager.asp" -->
<%

	'TEMPLATES
	
	Public Sub DataGrid_BlueTemplate(obj,ShowBorder)
		obj.AlternatingItemStyle = "background-color:#DDDDDD;font-size:8pt"
		obj.ItemStyle = "font-size:8pt"
		obj.Control.Style = "border-collapse:collapse"
		obj.HeaderStyle = "font-weight:bold;color:white;background-color:navy;font-size:10pt"
		obj.BorderWidth = IIF(ShowBorder,1,0)
	End Sub

	Public Sub DataGrid_RedTemplate(obj,ShowBorder)
		obj.AlternatingItemStyle = "background-color:#faebd7;font-size:8pt"
		obj.ItemStyle = "font-size:8pt"
		obj.Control.Style = "border-collapse:collapse"
		obj.HeaderStyle = "font-weight:bold;color:white;background-color:#c71585;font-size:10pt"
		obj.BorderWidth = IIF(ShowBorder,1,0)
	End Sub


	Class DataGridColumn
	
		Dim ColumnType   '1 Bound, 2 Format, 3 Template
		Dim CellRenderFunctionName
		Dim HorizonalAlign
		Dim VerticalAlign
		Dim ColumnWidth
		Dim DataTextField
		Dim DataValueField
		Dim HeaderText
		
		Dim ReadyOnly
		Dim EditControlType  'CheckBox, DropDown, CheckBox
		
		Dim DataFormatString
		Dim DataSource
		Dim DataGridOwner
		
		Private fDataTextField
		Private fDataValueField
		Private fncCellRenderFunction
		
		Private mRenderStart
		Private mRenderEnd
		Private mAltRenderStart		
	
		Private Sub Class_Initialize()
			ColumnType = 1			
			Set DataSource = Nothing
			Set fncCellRenderFunction = Nothing
			Set fDataTextField  = Nothing
			Set fDataValueField = Nothing
			Set DataGridOwner   = Nothing
			
			ReadyOnly  = True
		
		End Sub
	
		Private Function GetTDTag(Style,CssClass)
			GetTDTag = "<TD " &_				
				IIF(HorizonalAlign   <>""," align='" & HorizonalAlign & "' ", "") &_
				IIF(VerticalAlign   <>""," valign='" & VerticalAlign & "' ", "") &_
				IIF(Style   <>""," Style='" & Style & "' ", "") &_
				IIF(ColumnWidth   <>""," Width='" & ColumnWidth & "' ", "") &_				
				IIF(CssClass<>""," Class='" & CssClass & "' ", "") &_
			">"
		End Function
		
		Private Sub Init()
			mRenderStart = GetTDTag(DataGridOwner.ItemStyle,DataGridOwner.ItemCssClass)				
			Set fDataTextField = DataSource.Fields(DataTextField)
			If  DataValueField <> "" Then
				Set fDataValueField = DataSource.Fields(DataValueField)
			End If
				
			If CellRenderFunctionName <> "" Then
				Set fncCellRenderFunction = GetRef(CellRenderFunctionName)
			End If				
			mRenderEnd = "</TD>"
		End Sub
		
		Public Sub Render(Mode) 'mode = 1 normal, 2 = selected, 3 = edit
			If mRenderStart = "" Then
				Call Init()
			End If
			
			Select Case Mode
				Case 1
					Response.Write mRenderStart
				Case 0
					If mAltRenderStart = "" Then
						mAltRenderStart =GetTDTag(DataGridOwner.AlternatingItemStyle,DataGridOwner.AlternatingItemCssClass)				
					End If
					Response.Write mAltRenderStart 
				Case 2
					Response.Write GetTDTag(DataGridOwner.SelectedItemStyle,DataGridOwner.SelectedItemCssClass)
				Case 3
					Response.Write GetTDTag(DataGridOwner.EditItemStyle,DataGridOwner.EditItemCssClass)
				Case Else
					Response.Write mRenderStart
			End Select
						
			Select Case ColumnType
				Case 1:
					Response.Write Server.HTMLEncode( "" & fDataTextField.Value )
				Case 2:
					Response.Write Replace(Replace( Replace(DataFormatString,"{R}",DataSource.AbsolutePosition) ,"{0}",fDataTextField.Value), "{1}",fDataValueField)
				Case 3:
					Call fncCellRenderFunction(DataSource)
			End Select
			
			Response.Write mRenderEnd
		End Sub
		
		Public Sub RenderHeader()
			Response.Write GetTDTag(DataGridOwner.HeaderStyle,DataGridOwner.HeaderCssClass)
			Response.Write HeaderText
			Response.Write "</TD>"
		End Sub	
		
	End Class

	Public Function New_ServerDataGrid(name) 
		Set New_ServerDataGrid = New ServerDataGrid
			New_ServerDataGrid.Control.Name = name
	End Function

	 Class ServerDataGrid
		
		Dim Control
		Dim Controls
		Dim Columns
		Dim DataSource
		
		Dim ShowHeader					
		Dim HorizonalAlign
		Dim CellPadding
		Dim CellSpacing
		Dim BorderWidth
		Dim BorderColor
		Dim BackImageURL			
		Dim TableStyle
		Dim TableCssClass
			
		Dim AllowCustomPaging
		Dim AllowPaging
		Dim AutoGenerateColumns
			
		Dim HeaderStyle
		Dim FooterStyle
		
		Dim ItemStyle
		Dim AlternatingItemStyle
		Dim EditItemStyle
		Dim SelectedItemStyle
		
		Dim HeaderCssClass			
		Dim FooterCssClass
		Dim ItemCssClass
		Dim AlternatingItemCssClass
		Dim EditItemCssClass
		Dim SelectedItemCssClass	

		Dim EditItemIndex
		Dim SelectedItemIndex
						
		Dim HeaderFunctionName
		Dim FooterFunctionName
		
		Dim Pager
		Dim PagerHorizontalAlign
		Dim PagerShowOnTop
		
		Private Sub Class_Initialize()

			 Set Control =  New WebControl
			 Set Controls = Control.Controls
			 Set Control.Owner = Me			 			 			 			 
					 
			 Set DataSource = Nothing
				
			 ShowHeader = True
			 HorizonalAlign = ""
			 CellPadding = 2
			 CellSpacing = 0
			 BorderWidth = 0
			 BorderColor = ""
			 BackImageURL = ""
				
			 TableStyle = ""
			 TableCssClass = ""
				
			 AllowCustomPaging = False
			 AllowPaging       = False
			 AutoGenerateColumns = True
				
			 HeaderStyle = ""
			 EditItemStyle = ""
			 SelectedItemStyle = ""
			 HeaderCssClass = ""
			 EditItemCssClass = ""
			 SelectedItemCssClass = ""	
 
			 EditItemIndex = -1
			 SelectedItemIndex = -1
								
			 HeaderFunctionName = ""
			 FooterFunctionName = ""			 
			 
			 Set Pager = New ServerDataPager
			 PagerHorizontalAlign = "right"
			 PagerShowOnTop = True

		End Sub

		Public Sub WriteProperties(bag)
			bag.Write "SH",ShowHeader
			bag.Write "HA",HorizonalAlign
			bag.Write "CP",CellPadding
			bag.Write "CS",CellSpacing
			bag.Write "BW",BorderWidth
			bag.Write "BC",BorderColor
			bag.Write "BIURL",BackImageURL
			bag.Write "TS",TableStyle
			bag.Write "TCC",TableCssClass
			bag.Write "ACP",AllowCustomPaging
			bag.Write "AP",AllowPaging
			bag.Write "AGC",AutoGenerateColumns
			bag.Write "HS",HeaderStyle
			bag.Write "FS",FooterStyle
			bag.Write "IS",ItemStyle
			bag.Write "AIS",AlternatingItemStyle
			bag.Write "EIS",EditItemStyle
			bag.Write "SIS",SelectedItemStyle
			bag.Write "HCC",HeaderCssClass
			bag.Write "FCC",FooterCssClass
			bag.Write "ICC",ItemCssClass
			bag.Write "AICC",AlternatingItemCssClass
			bag.Write "EICC",EditItemCssClass
			bag.Write "SICC",SelectedItemCssClass
			bag.Write "EII",EditItemIndex
			bag.Write "SII",SelectedItemIndex
			bag.Write "HFN",HeaderFunctionName
			bag.Write "FFN",FooterFunctionName
			bag.Write "PHA",PagerHorizontalAlign
			bag.Write "PSOT",PagerShowOnTop
		End Sub
	
		Public Sub ReadProperties(bag)
			ShowHeader= CBool(bag.Read("SH"))
			HorizonalAlign= bag.Read("HA")
			CellPadding= bag.Read("CP")
			CellSpacing= bag.Read("CS")
			BorderWidth= bag.Read("BW")
			BorderColor= bag.Read("BC")
			BackImageURL= bag.Read("BIURL")
			TableStyle= bag.Read("TS")
			TableCssClass= bag.Read("TCC")
			AllowCustomPaging= CBool(bag.Read("ACP"))
			AllowPaging= CBool(bag.Read("AP"))
			AutoGenerateColumns= CBool(bag.Read("AGC"))
			HeaderStyle= bag.Read("HS")
			FooterStyle= bag.Read("FS")
			ItemStyle= bag.Read("IS")
			AlternatingItemStyle= bag.Read("AIS")
			EditItemStyle= bag.Read("EIS")
			SelectedItemStyle= bag.Read("SIS")
			HeaderCssClass= bag.Read("HCC")
			FooterCssClass= bag.Read("FCC")
			ItemCssClass= bag.Read("ICC")
			AlternatingItemCssClass= bag.Read("AICC")
			EditItemCssClass= bag.Read("EICC")
			SelectedItemCssClass= bag.Read("SICC")
			EditItemIndex= CInt(bag.Read("EII"))
			SelectedItemIndex= CInt(bag.Read("SII"))
			HeaderFunctionName= bag.Read("HFN")
			FooterFunctionName= bag.Read("FFN")
			PagerHorizontalAlign= bag.Read("PHA")
			PagerShowOnTop= CBool(bag.Read("PSOT"))
		End Sub
	
		Public Sub OnInit()			
			  Pager.Control.Name = Control.Name & "_Pager"
			  'Set Pager.Control.Parent = Me  'If you want to make it 100% dependant... no need though...
			  Set Pager.PagerOwner = Me
		End Sub

		Public Function HandleClientEvent(e)
			e.Source = Me.Control.Name
			Select Case e.EventName
				Case "PageIndexChange"					
					HandleClientEvent = ExecuteEventFunctionEX(e)
					Pager.PageIndex = Pager.PageIndex + CInt(e.Instance)
				Case "GotoPageIndex"					
					HandleClientEvent = ExecuteEventFunctionEX(e)
					Pager.PageIndex = CInt(e.Instance)
				Case Else
					HandleClientEvent = ExecuteEventFunctionEX(e)
			End Select				
	    End Function					
	
		'Renders the pager and return the max. number of rows to browse for
		Private Function RenderPager()								
			RenderPager = Pager.PageSize							
			If 	AllowCustomPaging Then
				If Pager.VirtualItemCount = 0 Then
					Pager.VirtualItemCount = DataSource.RecordCount
				End If
			Else
				Pager.VirtualItemCount = DataSource.RecordCount
				If DataSource.RecordCount >0 Then
					DataSource.AbsolutePosition = (Pager.PageIndex * Pager.PageSize) + 1				
				End If
			End If			
			Pager.Render						
		End Function
			
		Public Sub GenerateColumns()
			Dim fld
			Dim x
			
			If IsArray(Columns) Then
				Erase Columns
			End If
			
			Redim Columns(DataSource.Fields.Count-1)

			x=0
			For Each fld in DataSource.Fields
				Set Columns(x) = New DataGridColumn
				With Columns(x)
					Set .DataGridOwner = Me
					 Set .DataSource = DataSource
					.DataTextField = fld.Name
					.HeaderText    = fld.Name										
				End With			
				x=x+1
			Next
		End Sub
				
		Public Default Function Render()
			Dim col
			Dim varStart
			Dim alt
			Dim mode
			Dim maxRows
			Dim row
			
			varStart = Now
			
			If Control.Visible = False Then
				Exit Function
			End If
				
			If DataSource Is Nothing Then
				Pager.VirtualItemCount = 0
				Pager.PageIndex = 0
				'Pager.Pages = 0
				Exit Function
			End If
			
			If AutoGenerateColumns Then				
				GenerateColumns()
			End If
						
			alt = 1
			Response.Write  vbNewLine &  "<TABLE CellSpacing=" & CellSpacing & " CellPadding=" & CellPadding
			Response.Write " ID='" &  Control.Name & "' "
			Response.Write IIF(Control.CssClass<>""," Class='" & Control.CssClass & "' ","")
			Response.Write IIF(Control.Style<>""," Style='" & Control.Style & "' ","")
			Response.Write IIF(HorizonalAlign<>""," align=" & HorizonalAlign ,"")
			If BorderWidth > 0 Then
				Response.Write " Border=" & BorderWidth & " BorderColor='" & BorderColor & "'"
			End If
			Response.Write ">" & vbNewLine

			If AllowPaging Then				
				Response.Write "<TR><TD ColSpan=" & Ubound(Columns) + 1 & " "
				Response.Write " align='" & PagerHorizontalAlign & "' >"
				maxRows   =  RenderPager() 				
				Response.Write "</TD></TR>"
			Else
				maxRows = DataSource.RecordCount
			End If

			
			If ShowHeader Then
				Response.Write "<TR>"
				For Each col In Columns					
					col.RenderHeader()					
				Next
				Response.Write "</TR>"
			End If
			
			row = 0
		
			While Not DataSource.EOF And row<maxRows
				Response.Write "<TR>"
				mode = alt
				
				If DataSource.AbsolutePosition = EditItemIndex Then
					mode = 3
				ElseIf DataSource.AbsolutePosition = SelectedItemIndex Then
					mode = 2
				End If
				
				For Each col In Columns
					col.Render(mode)
				Next
				Response.Write "</TR>" & vbNewLine
				alt = 1 - alt
				DataSource.MoveNext
				row = row + 1
			Wend	
			Response.Write "</TABLE>" & vbNewLine
			
			Page.TraceRender varStart,Now,Control.Name
			
		End Function
	
	End Class
%>