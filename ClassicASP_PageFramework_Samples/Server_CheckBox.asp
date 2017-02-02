<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server TextBox

	Dim CSS_Name_ServerCheckBox
	Dim CSS_Name_ServerCheckBox_Caption
	CSS_Name_ServerCheckBox = ""
	CSS_Name_ServerCheckBox_Caption = ""
	
	Public Function New_ServerCheckBox(name) 
		Set New_ServerCheckBox = New ServerCheckBox
			New_ServerCheckBox.Control.Name = name
	End Function
	
	 Class ServerCheckBox
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim Caption
		Dim ReadOnly
		Dim Checked
		Dim AutoPostBack
			
			
		Private mbolWasRendered
		Private mPropBag
			
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			
			
			Caption = ""
			ReadOnly = False
			Checked = False
		    AutoPostBack = False
			
			Control.CssClass = CSS_Name_ServerCheckBox
			
			Set mPropBag = Nothing
			mbolWasRendered = False

	    End Sub
	   	
	   	Public Function WriteProperties(bag)			

			 Set mPropBag = bag
			 bag.Write  "R",False
			 bag.Write  "PB",AutoPostBack
			 bag.Write "Caption",Caption
			 If Control.Visible Then										
			  	bag.Write "Checked", Checked
			End If
		End Function
		
		Public Function ReadProperties(bag)
	   		mbolWasRendered = CBool(bag.Read("R"))
	   		Set mPropBag = bag
			Caption   = bag.Read("Caption")
			AutoPostBack = CBool(bag.Read("PB"))
			If mbolWasRendered Then
			   Checked  = (Request.Form(Control.Name)<>"")
			Else
			   Checked = CBool( bag.Read("Checked"))
			End If
			
		End Function
	   
	   Public Function HandleClientEvent(e)
		 	If AutoPostBack Then
				HandleClientEvent = ExecuteEventFunction(e.EventFnc)
		 	End If	
	   End Function			
	   
	   Public Function SetValueFromDataSource(value)
			Checked = CBool(value)			
	   End Function
	   
	   Private Function RenderCheckBox()
			Dim style
			Dim evtName
	   		
	   		If AutoPostBack Then
	   			evtName = Page.GetEventScript("onclick",Control.Name,"Click","","")
	   		End If
	   		
			Response.Write "<INPUT TYPE='CheckBox' Id='" & Control.Name & "' Name='" & Control.Name & "' " &_
					          iif(Checked," Checked "," ") &_
							  " TabIndex = " & Control.TabIndex &_
					          " Value = '1' "  &_
					          IIf(Control.Style<>""," Control.Style='" & Style + "' ","") &_
						      IIf(Not Control.Enabled," Disabled ","") &_
						      IIf(Control.CssClass<>""," Class='" & Control.CssClass  + "' ","") &_
						      evtName  &_
					         ">&nbsp;"	& IIF(Caption<>"","<span class='" & CSS_Name_ServerCheckBox_Caption & "'>" &  Caption & "</span>","") 
	   End Function

	   Public Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.IsVisible = False Then
				Exit Function
			 End If

			 varStart = Now
 
 			 If Not mPropBag Is Nothing Then
				mbolWasRendered = True
				mPropBag.Write "R",True
 			 End If
			 
			 If Control.TabIndex = 0 Then  'If Not assigned, then autoassign
				Control.TabIndex = Page.GetNextTabIndex()
			 End If
			 
			 Render = RenderCheckBox()
					
		 	Page.TraceRender varStart,Now,Control.Name
		 	 
		End Function

	End Class

%>