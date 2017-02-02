<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Button

	Dim CSS_Name_ServerLinkButton	
	CSS_Name_ServerLinkButton = ""
	
	Public Function New_ServerLinkButton(name) 
		Set New_ServerLinkButton = New ServerLinkButton
			New_ServerLinkButton.Control.Name = name
	End Function
	
	 Class ServerLinkButton
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Text
		Dim OnClick
		Dim CommandSource		
		Dim ImageURL
		Dim Target
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			OnClick = ""
			Text    = ""	
			CommandSource = ""
			ImageURL = ""
			Target = ""
			Control.CssClass = CSS_Name_ServerLinkButton		
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
			Text = bag.Read("T")
			CommandSource = bag.Read("C")			
			ImageURL =  bag.Read("U")
			Target = bag.Read("TG")
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "T",Text
			bag.Write "C",CommandSource
			bag.Write "U",ImageURL
			bag.Write "TG",Target
		End Function
	   
	   Public Function SetValueFromDataSource(value)		
				Text = value
	   End Function

	   Public Sub HandleClientEvent(e)
		 	If OnClick<>"" Then
				ExecuteEventFunction(OnClick)
			Else			
				ExecuteEventFunction(e.EventFnc)
		 	End If	
	   End Sub					
	   
	   Public Default Function Render()
			
			 Dim style
			 Dim varStart	 
			 Dim evtName
			 
			 If Control.Visible = False Then
				Exit Function
			 End If
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 varStart = Now
			 style = Control.Style

			 'If clientscript provided then
			 'if(clientscript) doPostBack


			'IMPORTANT!!!, CHECK THE TARGET:
			'If Target<>"" Themn Page.GetEventScriptRedirect("HREF",Control.Name,"OnClick",CommandSource,Target,Form

			'To support the ItemCommand
			Select Case TypeName(Control.Parent.Owner) 
				Case  "ServerDBTable" , "ServerDataRepeater", "ServerDataList" 
					If CommandSource = "" Then					
						evtName = Page.GetEventScript("href",Control.Name,"ItemCommand",0,Control.Parent.Owner.DataSource.AbsolutePosition)
					Else
						evtName = Page.GetEventScript("href",Control.Name,"ItemCommand",0,CommandSource)
					End If					
				Case Else	
					evtName = Page.GetEventScript("href",Control.Name,"OnClick",0,"")
			End Select
						 			 
			 Response.Write "<A ID='" & Control.Name & "'" &_				       
					   IIf(style<>""," Style='" & style  + "' ","") &_
					   IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
					  evtName &_
				     ">" 
			If ImageURL<>"" Then
				Response.Write "<IMG SRC='" & ImageURL & "' BORDER=0>"
			End If
			Response.Write Text & "</A>"  & vbNewLine
					
			Page.TraceRender varStart,Now,Control.Name
		End Function

	End Class

%>