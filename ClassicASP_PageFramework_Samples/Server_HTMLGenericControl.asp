<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server HTMLGenericControl

	Public Function New_ServerHTMLGenericControl(name,tag) 
		Set New_ServerHTMLGenericControl = New ServerHTMLGenericControl
			New_ServerHTMLGenericControl.Tag = tag
			New_ServerHTMLGenericControl.Control.Name = name
	End Function

	 Class ServerHTMLGenericControl
		Dim Controls
		Dim Control
		Private Text
		Dim Tag			

		Private Sub Class_Initialize()			
			Set Control  = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 			
			Tag = ""
			Text    = ""
	   End Sub

	   	Public Function ReadProperties(bag)			
			Text = bag.Read("Text")
			Tag	 = bag.Read("Tag")
		End Function
			
		Public Function WriteProperties(bag)		
			bag.Write "Text",Text
			bag.Write "Tag",Tag
		End Function
	   
	   Public Function HandleClientEvent(e)		 	
	   End Function					
	
	   Public Property Let InnerHTML(val) Text = val End Property
	   Public Property Get InnerHTML()    InnerHTML = Text End Property	   
	   
	   Private Function RenderControl()
			Response.Write "<" & Tag 	
	   		Response.Write  " Id='" & Control.Name & "' "
			Response.Write    IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") 
			Response.Write    IIf(Control.Style<>""," Style='" & Control.Style + "' ","")
			Response.Write  ">"				
			Response.Write Text
			Response.Write "</" & Tag & ">"
	   End Function

	
	   Public  Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If

			 varStart = Now
			 
			 Call RenderControl()
					
		 	 Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>