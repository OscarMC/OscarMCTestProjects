<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server CheckBox	
	
	Public Function New_ServerCheckBoxList(name) 
		Set New_ServerCheckBoxList = New ServerCheckBoxList
			New_ServerCheckBoxList.Control.Name = name
	End Function
	
	 Class ServerCheckBoxList
		
		Dim Controls
		Dim Control
		Dim ReadOnly
		
		Dim DataValueField
		Dim DataTextField
		Dim DataSource
		Dim Items
		Dim AutoPostBack

		Dim RepeatLayout 'Table (def)/Flow
		Dim RepeatDirection 'Vertical (def)/Horizontal)
		Dim RepeatColumns  '0 def
		
		Dim TableCss
		Dim TableStyle
		Dim BorderWidth
		Dim BorderColor
		Dim GridLines  '1 hor, 2 ver 3 both
		Dim CellSpacing 
		Dim CellPadding

		
		'Render State
		Private mbolWasRendered
		Private mPropBag
					
		Private Sub Class_Initialize()
			
			Set Control = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 			
			
			ReadOnly = False
		
			Set DataSource =  Nothing
			DataValueField = ""
			DataTextField = ""
			AutoPostBack = False	
			

			Set Items = CreateObject("ASPFramework.ListItemCollection")				
			Items.Mode = 2
			Set mPropBag = Nothing
			mbolWasRendered = False

			TableStyle = ""	
			TableCss   = ""		
			BorderWidth = 0
			BorderColor = "black"
			GridLines    = 3
			CellSpacing = 0
			CellPadding = 2
			RepeatLayout   = 1
			RepeatDirection =  2
			RepeatColumns  = 1
								
	   End Sub

	   Private Sub Class_Terminate()
			Set Items = Nothing
	   End Sub
	   
	   	Public Function ReadProperties(bag)
			Dim x,mx
			Dim frmElement
	   		mbolWasRendered = CBool(bag.Read("R"))
	   		Set mPropBag = bag

	   		RepeatLayOut = Cint(bag.Read("RL"))
	   		RepeatDirection = Cint(bag.Read("RD"))
	   		RepeatColumns = Cint(bag.Read("RC"))
	   		BorderWidth = CInt(bag.Read("W"))
	   		BorderColor = bag.Read("BC")
	   		GridLines   = bag.Read("GL")
	   		TableStyle   = bag.Read("BS")
	   		TableCss     = bag.Read("BCS")
	   		AutoPostBack     = CBool(bag.Read("AP"))
	   			   		
	   		Items.SetState bag.Read("Items")	   				   		

			'Do this only if the control was sent in the postback
			If mbolWasRendered AND Control.Enabled Then 'YUCK!, If not enabled then IE doesn't post it!
				mx = Request.Form(Control.Name).Count							
				Items.SetAllSelected(False)				
				Set frmElement = Request.Form(Control.Name)
				For x = 1 to mx						  
					 Call Items.SetSelectedByValue(frmElement.Item(x),True)
				Next
			End If
		End Function
		
		Public Function WriteProperties(bag)

			Set mPropBag = bag
			bag.Write  "R",False

			bag.Write "Items",Items.GetState()

	   		bag.Write "RL",RepeatLayOut 
	   		bag.Write "RD",RepeatDirection 
	   		bag.Write "RC",RepeatColumns 			
			bag.Write "W" ,BorderWidth
	   		bag.Write "BC",BorderColor
	   		bag.Write "GL",GridLines 

	   		bag.Write "BCS",TableCss
	   		bag.Write "GS",TableStyle
	   		bag.Write "AP",AutoPostBack  

		End Function
	   
	   Public Function HandleClientEvent(e)
		 	If AutoPostBack Then
				'HandleClientEvent = ExecuteEventFunction(e.EventFnc)
				HandleClientEvent = ExecuteEventFunctionEX(e)
				
		 	End If	
	   End Function
	   
		Public Sub DataBind()
			
			Dim objRs,x,mx	
			Dim fld1,fld2
			Dim use1,use2
			
			Items.Clear()
			
			If DataSource Is Nothing Then				
				Exit Sub
			End If		
					
			Set objRs = DataSource				
			objRs.MoveFirst
			If DataTextField<>""  Then 
				Set fld1 = objRs(DataTextField) 
				use1 = True
				Else  Set fld1 = Nothing
				use1 = False
			End If
			If DataValueField<>"" Then 
				Set fld2 = objRs(DataValueField) 
				use2 = True
			Else 
				Set fld2 = Nothing
				use2 = false
			End If
			x=0
			While Not objRs.EOF							
				Items.Add IIF(use1, fld1,""),  IIF(use2, fld2,x),False
				objRs.MoveNext
				x = x + 1		
			Wend
			
			If mbolWasRendered Then
				mx = Request.Form(Control.Name).Count			
				Items.SetAllSelected(False)										
				For x = 1 to mx						  
					 Call Items.SetSelectedByValue(Request.Form(Control.Name).Item(x),True)
				Next
			End If
			Set fld1 = Nothing
			Set fld2 = Nothing
		End Sub

		Private Function  RenderByColumn								
			Dim x,mx,i,alt
			Dim Rows,r
			Dim Selected,Value,Text
			Dim Enabled, ControlName,Style,Css
			Dim sCaption
			
			Enabled = IIf(Not Control.Enabled," Disabled ","") 
			Style = IIf(Control.Style<>""," Style='" & Control.Style + "' ","")
			Css = IIf(Control.CssClass<>""," Class='" & Control.CssClass + "' ","")
			ControlName = Control.Name
			sCaption = ">&nbsp;<span " & Style & Css & ">"
									
			mx  = Items.Count
			rows = Int(mx/RepeatColumns)
			
			If mx mod RepeatColumns = 0 Then
				Rows = Rows -1
			End If

			If RepeatLayout =1 Then
				Response.Write  vbNewLine &  "<TABLE CellSpacing=" & CellSpacing & " CellPadding=" & CellPadding
				Response.Write IIF(TableCss<>""," Class='" & TableCss & "' ","")
				Response.Write IIF(TableStyle<>""," Style='" & TableStyle & "' ","")
				If BorderWidth > 0 Then
					Response.Write " Border=" & BorderWidth & " BorderColor='" & BorderColor & "'"
				End If
				Response.Write ">" & vbNewLine				
			End If
			
			i = 0						
			For r = 0 To Rows
				If RepeatLayout = 1 Then 					
					Response.Write "<TR>"
				End If								
				For x = 1 To RepeatColumns					
					If i<mx Then				
						Items.GetItemData i,Text,Value,Selected	
						If RepeatLayOut = 1 Then 
							Response.Write "<TD>"
						End If
						
						Response.Write "<INPUT TYPE='CheckBox' Id='" & ControlName & "' Name='" & ControlName & "' " &_
						   IIf(Selected," Checked "," ") & " Value = """ + Server.HTMLEncode(Value) + """ " &  Enabled
						   If AutoPostBack Then
				   				Response.Write Page.GetEventScript("onclick",ControlName,"Click",i,"")	   		
						   End If
						   Response.Write sCaption & Text & "</span>"
						   If RepeatLayOut = 1 Then 
								Response.Write "</TD>"
						   End If
						   i = i + 1
					End If
				Next				
				If RepeatLayout = 1 Then 
					Response.Write "</TR>" & vbNewLine
				End If
			Next
			If RepeatLayout =1 Then				
				Response.Write  "</TABLE>" & vbNewLine
			End If
												
		End Function
	   
		Private Function  RenderByRow				
			Dim x,mx
			Dim Cols,c
			Dim Rows,r,Row,Pos
			Dim Selected,Value,Text
			Dim Enabled, Style, Css,sCaption,ControlName
			Enabled = IIf(Not Control.Enabled," Disabled ","") 
			Style = IIf(Control.Style<>""," Style='" & Control.Style + "' ","")
			Css = IIf(Control.CssClass<>""," Class='" & Control.CssClass + "' ","")
			ControlName = Control.Name
			sCaption = ">&nbsp;<span " & Style & Css & ">"
			
			mx = Items.Count
			Cols = RepeatColumns -1
			Rows = Int(mx/RepeatColumns) - 1
			
			If RepeatLayout =1 Then
				Response.Write  vbNewLine &  "<TABLE CellSpacing=" & CellSpacing & " CellPadding=" & CellPadding
				Response.Write " ID='" &  Control.Name & "' "
				Response.Write IIF(TableCss<>""," Class='" & TableCss & "' ","")
				Response.Write IIF(TableStyle<>""," Style='" & TableStyle & "' ","")
				If BorderWidth > 0 Then
					Response.Write " Border=" & BorderWidth & " BorderColor='" & BorderColor & "'"
				End If
				Response.Write ">" & vbNewLine				
			End If
			Row = 0
			For x = 1 To 2 
				For r = 0 To Rows
					If RepeatLayout = 1 Then 					
						Response.Write "<TR>"
					End If								
					For c = 0 To Cols
						Pos = Row + (Rows * c + r + c)
						If Pos<mx Then
							Items.GetItemData Pos,Text,Value,Selected	
							
							If RepeatLayOut = 1 Then 
								Response.Write "<TD>"
							End If
							
							Response.Write "<INPUT TYPE='CheckBox' Id='" & ControlName & "' Name='" & ControlName & "' " &_
							   IIf(Selected," Checked "," ") & " Value = """ + Server.HTMLEncode(Value) + """ " & Enabled
							   If AutoPostBack Then
				   					Response.Write Page.GetEventScript("onclick",ControlName,"Click",Pos,"")	   		
							   End If
							   Response.Write sCaption & Text & "</span>"
							   If RepeatLayOut = 1 Then 
									Response.Write "</TD>"
							   End If
						End If						
					Next
					
					If RepeatLayout = 1 Then 
						Response.Write "</TR>" & vbNewLine
					End If
				Next
				Cols = mx mod RepeatColumns
				Rows = 0
				If Cols = 0 Then 'Last Column!
					Exit For
				Else
					Row = mx-Cols
					Cols = Cols - 1
				End If
			Next
			
			If RepeatLayout =1 Then
				Response.Write  "</TABLE>" & vbNewLine
			End If
				
		End Function

	   Public Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.IsVisible = False Then
				Exit Function
			 End If			
			 
			 varStart = Now

			 If Not mPropBag Is Nothing Then
				mbolWasRendered = True
				mPropBag.Write "R",True
			 End If
			 
			 RepeatColumns = CInt(RepeatColumns)
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 If RepeatDirection=1 Then
				Render = RenderByColumn()
			 Else
				Render = RenderByRow()
			 End If
					
		 	 Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>