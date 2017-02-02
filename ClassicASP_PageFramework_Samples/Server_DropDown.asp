<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server DropDown

	Dim CSS_Name_ServerDropDown
		
	CSS_Name_ServerDropDown = ""
	
	Public Function New_ServerDropDown(name) 
		Set New_ServerDropDown = New ServerDropDown 
			New_ServerDropDown.Control.Name = name
	End Function
	
	 Class ServerDropDown
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Caption
		Dim Multiple
		Dim ReadOnly
		Dim Rows
		
		Dim DataValueField
		Dim DataTextField
		Dim DataSource
		Dim Items
		Dim AutoPostBack
				
		Dim CaptionCssClass
		Dim CaptionStyle
		
		Private mbolWasRendered
		Private mPropBag
		
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			
			Caption = ""			
			Rows  = 1
			Control.CssClass = CSS_Name_ServerDropDown
			Multiple = False
			Set DataSource =  Nothing
			DataValueField = ""
			DataTextField = ""
			AutoPostBack = False	
			Set Items = CreateObject("ASPFramework.ListItemCollection")				
			
			Set mPropBag = Nothing
			mbolWasRendered = False
			ReadOnly = False	
	   End Sub
	   Private Sub Class_Terminate()
			Set Items = Nothing
	   End Sub
	   
	   Public Property Get Text()
			Text = Items.GetSelectedText
	   End Property
	   Public Property Get Value()
			Value = Items.GetSelectedValue	
	   End Property
	   
	   	Public Function ReadProperties(bag)
	   			Dim x,mx

	   			mbolWasRendered = CBool(bag.ReadBoolean("R"))
	   			Set mPropBag = bag
	   			
	   			Caption =bag.Read("Caption")
	   			Multiple = CBool(bag.ReadBoolean("Multiple"))
	   			Rows = CInt(bag.ReadInt("Rows"))   			
	   			AutoPostBack = CBool(bag.ReadBoolean("PB"))
	   			CaptionCssClass = bag.Read("CSS")
	   			CaptionStyle = bag.Read("ST")
	   			ReadOnly = CBool(bag.ReadBoolean("RO"))
	   			
	   			If Multiple Then
	   				Items.Mode = 2
	   			End If
	   			Items.SetState bag.Read("Items")	   		
	   				
				If mbolWasRendered AND Control.Enabled Then 'YUCK!, If not enabled then IE doesn't post it!
					mx = Request.Form(Control.Name).Count			
					Items.SetAllSelected(False)
					For x = 1 to mx						  
						 Call Items.SetSelectedByValue(Request.Form(Control.Name).Item(x),True)
					Next
				End If

		End Function
		
		Public Sub Bind(pDataSource,pDataValueField,pDataTextField,CacheAs,bolAddBlank)
			Dim bolFirstTime
			bolFirstTime = False

			If CacheAs<>"" Then
				If Application(CacheAs)<>"" Then
					bolFirstTime = True

					SetFromCache(Application(CacheAs))
					Exit Sub
				End If
			End If

			Set Me.DataSource  = pDataSource
			Me.DataValueField  = pDataValueField
			Me.DataTextField   = pDataTextField
			Me.DataBind
			If bolAddBlank Then
				Items.Add "","",True,0
			End If
				
			On Error Resume Next
			If CacheAs<>"" And bolFirstTime Then
				Application.Lock
				Application(CacheAs) = Items.GetState()
				Application.UnLock
				Err.Clear
				End If 
		End Sub

		Public Function SetFromCache(cache)
	   			Dim x,mx

				Items.SetState cache
				'Items.get
	   			'Only if it was rendered in the previous request (data exists in the request)
				If mbolWasRendered Then
					Page.TraceImportantCall Me.Control, "Setting Items from Cache"
					mx = Request.Form(Control.Name).Count			
					Items.SetAllSelected(False)
					For x = 1 to mx						  
						 Call Items.SetSelectedByValue(Request.Form(Control.Name).Item(x),True)
					Next
				End If
		End Function
		
		Public Function WriteProperties(bag)

			Set mPropBag = bag
			bag.Write  "R",False
			
			bag.Write "Caption",Caption
			bag.Write "Multiple",Multiple
			bag.Write "Rows",Rows
			bag.Write "Items",Items.GetState()
			bag.Write "CSS",CaptionCssClass
	   		bag.Write "ST",CaptionStyle
	   		bag.Write  "PB",AutoPostBack
	   		bag.Write  "RO",ReadOnly
		End Function
	   
	   
	   	Public Function SetValueFromDataSource(value)
			Checked = CBool(value)			
			Items.SetAllSelected(False)
			If DataValueField <> "" Then
				Items.SetSelectedByValue value,True
			Else		
				Items.SetSelectedByText  value,True
			End If			
	    End Function

	   
	   Public Function HandleClientEvent(e)
		 	If AutoPostBack Then
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
			Else  
				Set fld1 = Nothing
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
			
			If Control.Visible and Page.IsPostBack Then
				mx = Request.Form(Control.Name).Count			
				Items.SetAllSelected(False)										
				For x = 1 to mx						  
					 Call Items.SetSelectedByValue(Request.Form(Control.Name).Item(x),True)
				Next
			End If
			Set fld1 = Nothing
			Set fld2 = Nothing
						
		End Sub

	   
	   Private Function RenderDropDown()
			Dim x,mx,varHTML,varInput
			Dim Selected,Value,Text
			Dim objOut 
			Dim evtName
						
			
			If AutoPostBack Then
				evtName = Page.GetEventScript("onchange",Control.Name,"ItemChange","this","")
			End If
			If Caption<>"" Then
				Response.Write "<SPAN "
				Response.Write IIf(CaptionCssClass<>""," Class='" & CaptionCssClass  + "' ","")
				Response.Write iif(CaptionStyle<>""," Style='" & CaptionStyle + "' ","")					
				Response.Write ">" & Caption & "</SPAN>"
			End If	
			
			If ReadOnly Then
				Response.Write  "<SPAN ID='" & Control.Name & "' NAME='" & Control.Name & "' " &_
					  	  IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
				          iif(Control.Style<>""," Style='" & Control.Style + "' ","") &_
				        ">" & vbNewLine												
				Response.Write Server.HTMLEncode (Items.GetSelectedText)
				Response.Write "</SPAN>"
				Exit Function
			End If

			
			mx = Items.Count-1
			Response.Write  "<SELECT ID='" & Control.Name & "' NAME='" & Control.Name & "' " &_
					  	  evtName &_ 
					  	  " TabIndex = " & Control.TabIndex &_
					  	  IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
				          iif(Multiple," Multiple Size=" & Rows & " "  ," ") &_
				          iif(Control.Style<>""," Style='" & Control.Style + "' ","") &_
				          IIf(Not Control.Enabled," Disabled ","") &_
				        ">" & vbNewLine								
			For x = 0 to mx			
				    Items.GetItemData x,Text,Value,Selected	
					Response.Write  "<OPTION " & IIf(Selected," selected","") & " value='"  & Server.HTMLEncode(Value)	 & "'>" &_
							   Text & "</OPTION>" & vbNewLine						
			Next 			
			Response.Write "</SELECT>" & vbNewLine
	   End Function

	   Public Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If

			 varStart = Now

			If Not mPropBag Is Nothing And Not ReadOnly Then
				mbolWasRendered = True
				mPropBag.Write "R",True
			End If
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 Render = RenderDropDown
					
		 	 Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>