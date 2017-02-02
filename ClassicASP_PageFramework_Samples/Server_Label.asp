<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Label

	Dim CSS_Name_ServerLabel
	
	CSS_Name_ServerLabel = ""
	Public Function New_ServerLabel(name) 
		Set New_ServerLabel = New ServerLabel
			New_ServerLabel.Control.Name = name
	End Function
	
	 Class ServerLabel
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Text
				
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			Text    = ""	
			Control.CssClass = CSS_Name_ServerLabel			
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
			Text = bag.Read("Text")
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "Text",Text
		End Function
	   
	   Public Function HandleClientEvent(e)
	   End Function			
	   
	   Public Function SetValueFromDataSource(value)
			Text = value
	   End Function
	   
	   Public Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If
			 
			 varStart = Now

			 Response.Write  "<SPAN ID='" & Control.Name & "' "
			 Response.Write IIf(Control.Style<>""," Style='" & Control.Style  + "' ","") 
			 Response.Write  IIF(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") 
			 Response.Write ">"
			 Response.Write  Text
			 Response.Write  "</SPAN>"
					
			Page.TraceRender varStart,Now,Control.Name
		
		End Function

	End Class

%>