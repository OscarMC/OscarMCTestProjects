<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Button

	Dim CSS_Name_ServerImageButton
	
	CSS_Name_ServerImageButton = ""
	Public Function New_ServerImageButton(name) 
		Set New_ServerImageButton = New ServerImageButton
			New_ServerImageButton.Control.Name = name
	End Function

	 Class ServerImageButton
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Image
		Dim RollOverImage
		Dim OnClick
		Dim CommandArgument		
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			OnClick  = ""
			Image    = ""	
			RollOverImage = ""
			CommandArgument = ""
			Control.CssClass = CSS_Name_ServerImageButton		
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
			Image = bag.Read("Image")
			RollOverImage = bag.Read("RollOverImage")
			CommandArgument = bag.Read("CommandArgument")			
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "Image",Image
			bag.Write "RollOverImage",RollOverImage
			bag.Write "CommandArgument",CommandArgument
		End Function

	    Public Function SetValueFromDataSource(value)
			Image = value
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
			 Dim RollOverScript
			 Dim evtName
			 
			 If Control.Visible = False Then
				Exit Function
			 End If

			 varStart = Now
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 If RollOverImage <> "" Then			 
				RollOverScript = " onmouseover = 'this.src =" & chr(34) & RollOverImage & chr(34) & "' " & _
							     " onmouseout  = 'this.src =" & chr(34) & Image & chr(34) & "' "
			End If							    
			
			'To support the ItemCommand
			Select Case TypeName(Control.Parent.Owner) 
				Case  "ServerDBTable" , "ServerDataRepeater", "ServerDataList" 
					If CommandSource = "" Then					
						evtName = Page.GetEventScript("onClick",Control.Name,"ItemCommand",0,Control.Parent.Owner.DataSource.AbsolutePosition)
					Else
						evtName = Page.GetEventScript("onClick",Control.Name,"ItemCommand",0,CommandSource)
					End If					
				Case Else	
					evtName = Page.GetEventScript("onclick",Control.Name,"OnClick",0,"")
			End Select
			Response.Write "<A HREF='#' " & evtName  & " >"		 
			Response.Write "<IMG border=0 Id='" & Control.Name & "' " &_				       
					   IIf(Control.Style<>""," Style='" & Control.Style  + "' ","") &_
					   IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
					   " TabIndex = " & Control.TabIndex &_
					   " Src = '" &  Image & "'" &_
					   RollOverScript &_					   
				     ">" & vbNewLine
			Response.Write "</A>"		

			Page.TraceRender varStart,Now,Control.Name
		End Function

	End Class

%>