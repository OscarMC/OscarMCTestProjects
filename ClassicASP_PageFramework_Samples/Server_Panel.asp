<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Panel

	Dim CSS_Name_ServerPanel
	
	CSS_Name_ServerPanel = ""
	
	 Class ServerPanel
		Private mobjWebControl				
		Private varRenderStart
		
		Dim Controls
		Dim Control
		Dim Text
		
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			Text    = ""	
			Control.CssClass = CSS_Name_ServerPanel		
	   End Sub
	   
	   	Public Function ReadProperties(bag)
			 Text = bag.Read("Text")
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "Text",Text
		End Function
	   
	   Public Sub HandleClientEvent(EventName)
			'Uh???
		 	'If OnClick<>"" Then
			'	ExecuteEventFunction(OnClick)
			'Else			
			'	ExecuteEventFunction(Control.Name & "_OnClick")
		 	'End If	
	   End Sub					
	   
	   Public Function OpenRenderTag()
			
			 Dim style
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				OpenRenderTag = False
				Exit Function
			End If
			 			 
			 varRenderStart = Now
			 style = Control.Style
			 
			 style = Control.Style.ToString()			 	
			 Response.Write vbNewLine & "<DIV  ID='" & Control.Name  & "'" &_				       
					   IIf(Style<>""," Style='" & Style  + "' ","") &_
					   IIf(Control.CssClass<>"","Class='" & Control.CssClass  + "' ","") &_
				       IIf(Not Control.Enabled," Disabled ","") &_
				     ">" & vbNewLine
			OpenRenderTag = True					
		End Function
		
		Public Function CloseRenderTag()
			If Control.Visible Then
				Response.Write "</DIV>"  '<!--End" & Control.Name & "-->"
				Page.TraceRender varRenderStart,Now,Control.Name
			End If
		End Function

	End Class

%>