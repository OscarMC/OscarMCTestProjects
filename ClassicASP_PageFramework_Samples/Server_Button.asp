<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Button

	Dim CSS_Name_ServerButton	
	CSS_Name_ServerButton = ""
	
		'Helper function.
	Public Function New_ServerButton(name) 
		Set New_ServerButton = New ServerButton
			New_ServerButton.Control.Name = name
	End Function

	 Class ServerButton
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Text
		Dim OnClick
		Dim CommandSource
				
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			OnClick = ""
			Text    = ""	
			Control.CssClass = CSS_Name_ServerButton
			
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
			Text = bag.Read("Text")
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "Text",Text
		End Function
	   
	   Public Function HandleClientEvent(e)
		 	If OnClick<>"" Then
				HandleClientEvent = ExecuteEventFunction(OnClick)
			Else			
				HandleClientEvent = ExecuteEventFunction(e.EventFnc)
		 	End If	
	   End Function			
	   
	   Public Default Function Render()
			
			 Dim varStart	 
			 Dim evtName
			 
			 If Control.Visible = False Then
				Exit Function
			End If
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			'To support the ItemCommand
			Select Case TypeName(Control.Parent.Owner) 
				Case  "ServerDBTable" , "ServerDataRepeater", "ServerDataList" 
					If CommandSource = "" Then					
						evtName = Page.GetEventScript("onclick",Control.Name,"ItemCommand",0,Control.Parent.Owner.DataSource.AbsolutePosition)
					Else
						evtName = Page.GetEventScript("onclick",Control.Name,"ItemCommand",0,CommandSource)
					End If					
				Case Else	
					evtName = Page.GetEventScript("onclick",Control.Name,"OnClick",0,"")
			End Select
			 
			 varStart = Now
			 
			 Response.Write "<INPUT TYPE='Button' ID='" & Control.Name & "' NAME='"           &_
				       Control.Name & "' value=""" & Server.HTMLEncode(Text) & """ "  &_
					   IIf(Control.Style<>""," style='" & Control.Style  + "' ","") &_
					   IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
					   " TABINDEX = " & Control.TabIndex &_
				       IIf(Not Control.Enabled," disabled ","") &_
					   evtName  &_
				     ">" & vbNewLine
					
			Page.TraceRender varStart,Now,Control.Name
		End Function

	End Class

%>