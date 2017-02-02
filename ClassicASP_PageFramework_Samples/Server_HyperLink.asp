<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server HyperLink

	Public Function New_ServerHyperLink(name) 
		Set New_ServerHyperLink = New ServerHyperLink
			New_ServerHyperLink.Control.Name = name
	End Function
	
	 Class ServerHyperLink
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Text
		Dim NavigateUrl
		Dim Target
		Dim ImageURL

		Private Sub Class_Initialize()			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			NavigateUrl = ""
			Target = ""
			Text    = ""	
			ImageURL = ""
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
			Text = bag.Read("T")
			Target = bag.Read("TR")
			NavigateURL = bag.Read("N")
			ImageURL =  bag.Read("U")
		End Function
		
		Public Function WriteProperties(bag)
			bag.Write "T",Text
			bag.Write "TR",Target
			bag.Write "N",NavigateURL
			bag.Write "U",ImageURL
		End Function
	   
	   Public Function SetValueFromDataSource(value)		
				Text = value
				NavigateURL = Text
	   End Function

	   Public Sub HandleClientEvent(e)
	   End Sub					
	   
	   Public Default Function Render()
			
			 Dim varStart	 
			 Dim evtName
			 
			 If Control.Visible = False Then
				Exit Function
			 End If
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 varStart = Now

			 Response.Write "<A ID='" & Control.Name & "'" &_				       
					   "HREF='" & NavigateURL & "'" &_
					   IIf(Target<>""," Target='" & Target  + "' ","") &_
					   IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
					   IIf(Control.Style<>""," Style='" & Control.Style  + "' ","") &_					   
					   " TabIndex = " & Control.TabIndex &_
				     ">" 
			If ImageURL<>"" Then
				Response.Write "<IMG SRC='" & ImageURL & "' BORDER=0>"
			End If
			Response.Write Text & "</A>"  & vbNewLine
					
			Page.TraceRender varStart,Now,Control.Name
		End Function

	End Class

%>