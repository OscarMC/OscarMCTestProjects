<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server TextBox

	Dim CSS_Name_ServerTextBox
	CSS_Name_ServerTextBox = ""
	
	'Helper function.
	Public Function New_ServerTextBox(name) 
		Set New_ServerTextBox = New ServerTextBox
			New_ServerTextBox.Control.Name = name
	End Function

	Public Function New_ServerTextBoxEX(name,maxsize,width)	
		Set New_ServerTextBoxEX = New ServerTextBox
		New_ServerTextBoxEX.Control.Name = name		
		New_ServerTextBoxEX.Size=width
		New_ServerTextBoxEX.MaxLength = maxsize 
	End Function

	
	 Class ServerTextBox
		
		Private bolRendered 
		
		Dim Controls
		Dim Control
		Dim Text
		Dim Caption
		Dim Mode '1 Text, 2 Password, 3 TextArea, 4 Label
		Dim Size
		Dim ReadOnly
		Dim MaxLength
		Dim Cols
		Dim Rows
		Dim TextChanged
	
		Private Sub Class_Initialize()
			
			Set Control = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 			
		
			Text    = ""
			Caption = ""
			Mode    = 1
			ReadOnly = False
			Size=0			
			MaxLength = 0
			Cols = 40
			Rows = 5			
			Control.CssClass = CSS_Name_ServerTextBox		
			TextChanged=False
	   End Sub

	   	Public Function ReadProperties(bag)			
	   	
			Caption   = bag.Read("Caption")
			Mode	  = CInt(bag.Read("Mode"))
			Size	  = CInt(bag.Read("Size"))
			MaxLength = CInt(bag.Read("MaxLength"))
			ReadOnly = CBool(bag.Read("RO"))
			Rows = CInt(bag.Read("Rows"))
			Cols = CInt(bag.Read("Cols"))
			
			If  Request.Form.Key(Control.Name)<>"" Then
				Text  = Request.Form(Control.Name)
				TextChanged = (bag.Read("Text")<>Text)
				'If bag.Read("Text") <> Text then raise changed event...
			Else
			    Text  = bag.Read("Text")
			End If
			
		End Function
		
		Public Function WriteProperties(bag)
		
			bag.Write "Caption",Caption
			bag.Write "Mode",Mode
			bag.Write "MaxLength",MaxLength
			bag.Write "Size",Size							
			bag.Write "Text",Text
			bag.Write "RO",ReadOnly
			bag.Write "Rows",Rows
			bag.Write "Cols",Cols
			
		End Function
	   
	   Public Function HandleClientEvent(e)
		 	HandleClientEvent = False	
	   End Function					
	   
	   Public Function SetValueFromDataSource(value)
			Text = value
	   End Function
	   
	   Private Function RenderLabel()
	   	   		Response.Write  "<SPAN  Id='" & Control.Name & "' Name='" &_
			       Control.Name & "' Value=""" & Server.HTMLEncode(Text) & """ "  &_
			       IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
			       IIf(Control.Style<>""," Style='" & Control.Style + "' ","")  & ">"
			   Response.Write Server.HTMLEncode(Text)
				Response.Write "</SPAN>"			    
	   End Function
	   
	   Private Function RenderTextBox()									   		
	   		Response.Write  "<INPUT  Id='" & Control.Name & "' Name='" &_
			       Control.Name & "' Value=""" & Server.HTMLEncode(Text) & """ "  &_
			       " TabIndex = " & Control.TabIndex &_
			       IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
			       IIf(Control.Attributes<>""," " & Control.Attributes  + " ","") &_
			       IIf(Control.Style<>""," Style='" & Control.Style + "' ","") &_
			       IIf(Size>0," Size=" & Size,"") &_
			       IIf(MaxLength>0," maxlength=" & MaxLength,"") &_			       
			       IIf(ReadOnly," ReadOnly ","") &_
			       " TYPE='" & IIf(Mode=2,"PASSWORD","TEXT")  & "'"&_
			       IIf(Not Control.Enabled," Disabled ","") &_
			     ">"				
	   End Function

	   Private Function RenderTextArea()									   		
	   		Response.Write  "<TEXTAREA Id='" & Control.Name & "' Name='" & Control.Name & "' " &_
			       " TabIndex = " & Control.TabIndex &_
			       IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
			       IIf(Control.Attributes<>""," " & Control.Attributes  + " ","") &_
			       IIf(Control.Style<>""," Style='" & Control.Style + "' ","") &_
			       " Rows = " & Rows  &_
			       " Cols = " & Cols  &_
			       IIf(MaxLength>0," maxlength=" & MaxLength,"") &_			       
			       IIf(ReadOnly," ReadOnly ","") &_
			       " TYPE='" & IIf(Mode=2,"PASSWORD","TEXT")  & "'"&_
			       IIf(Not Control.Enabled," Disabled ","") &_
			     ">"				
			Response.Write Server.HTMLEncode(Text)
			Response.Write "</TEXTAREA>"
	   End Function

	
	   Public  Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.IsVisible = False Then
				Exit Function
			 End If

			 varStart = Now
			
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 If Caption<>"" Then
				Response.Write Caption & "&nbsp;"
			 End If

			 Select Case Mode
				Case 3:
					Render = RenderTextArea
				Case 4:
					Render = RenderLabel				
				Case Else
					Render = RenderTextBox
				
			 End Select
					
		 	 Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>