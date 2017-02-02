<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server DateTextBox

	Dim CSS_Name_ServerDateTextBox
	CSS_Name_ServerDateTextBox = ""
	
	Public Function New_ServerDateTextBox(name) 
		Set New_ServerDateTextBox = New ServerDateTextBox
			New_ServerDateTextBox.Control.Name = name
	End Function

	 Class ServerDateTextBox
		
		Private bolRendered 
		
		Dim Controls
		Dim Control
		Dim Text
		Dim Caption
		Dim Size
		Dim ReadOnly
		Dim MaxLength
		Dim TextChanged
		
		Private Sub Class_Initialize()
			
			Set Control = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 			
		
			Text    = ""
			Caption = ""
			ReadOnly = False
			Size=10		
			Control.CssClass = CSS_Name_ServerDateTextBox		
			TextChanged = False
			Page.RegisterClientScript "Calendar","<script language='JavaScript' src='calendar/calendar2.js'></script>"

	   End Sub

	   	Public Function ReadProperties(bag)			
	   	
			Caption   = bag.Read("Caption")
			Size	  = CInt(bag.Read("Size"))
			'Am I in the request? (for text boxes is quite easy...)
			
			If  Request.Form.Key(Control.Name)<>"" Then
				Text  = Request.Form(Control.Name)
				TextChanged = (bag.Read("Text")<>Text)
			Else
			    Text  = bag.Read("Text")
			End If
			
			
		End Function
		
		Public Function WriteProperties(bag)
		
			bag.Write "Caption",Caption
			bag.Write "Size",Size							
			bag.Write "Text",Text
			
		End Function
	   
	   Public Function HandleClientEvent(e)
		 	HandleClientEvent = False	
	   End Function					
	   
	   Public Function SetValueFromDataSource(value)
			Text = value
	   End Function
	   
	   Private Function RenderDateTextBox()
			Dim style
			style = Control.Style

			If Caption<>"" Then
				Response.Write Caption & "&nbsp;"
			End If
									   		
	   		Response.Write  "<INPUT  Id='" & Control.Name & "' Name='" &_
			       Control.Name & "' Value=""" & Server.HTMLEncode(Text) & """ "  &_
			       " TabIndex = " & Control.TabIndex &_
			       IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
			       IIf(style<>""," Style='" & style + "' ","") &_
			       IIf(Size>0," Size=" & Size,"") &_
			       " maxlength = 10 " &_			       
			       IIf(ReadOnly," ReadOnly ","") &_
			       " TYPE='TEXT' "  &_
			       IIf(Not Control.Enabled," Disabled ","") &_
			     ">"
			Response.Write "&nbsp;<A href='javascript:" & Control.Name & ".popup();'><IMG alt='Click Here to Pick up the date' src='calendar/cal.gif' border=0></A>"

	   End Function

	
	   Public  Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If

			 varStart = Now
			
			 Page.RegisterClientScript "Calendar" & Control.Name ,  "<script language='JavaScript'>" &  Control.Name &  " = new calendar2(document.frmForm.elements['" & Control.Name & "']);</script>"			
			
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			Render = RenderDateTextBox
					
		 	Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>