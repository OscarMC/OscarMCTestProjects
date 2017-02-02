<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server ServerDataRepeater
	Public Function New_ServerDataRepeater(name) 
		Set New_ServerDataRepeater = New ServerDataRepeater
			New_ServerDataRepeater.Control.Name = name
	End Function
	
	 Class ServerDataRepeater
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim DataSource
		
		Dim Header
		Dim ItemTemplate
		Dim AlternateItemTemplate
		Dim Footer
		Dim SeparatorTemplate

		Private fncHeader
		Private fncItemTemplate
		Private fncAlternateItemTemplate
		Private fncFooter
		Private fncSeparatorTemplate
		
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me 			

			Header = ""
			ItemTemplate  = ""
			AlternateItemTemplate  = ""
			Footer = ""
			SeparatorTemplate = ""
			
			Set fncHeader = Nothing
			Set fncItemTemplate  = Nothing
			Set fncAlternateItemTemplate  = Nothing
			Set fncFooter = Nothing
			Set fncSeparatorTemplate = Nothing
			
	   End Sub
	   
	   	Public Function ReadProperties(bag)			
	   		Header = bag.Read("H")
	   		ItemTemplate = bag.Read("IT")
	   		AlternateItemTemplate = bag.Read("AI")
	   		Footer  = bag.Read("F")
	   		SeparatorTemplate = bag.Read("S")
		End Function
		
		Public Function WriteProperties(bag)
	   		bag.Write "H",Header
	   		bag.Write "IT",ItemTemplate
	   		bag.Write "AI",AlternateItemTemplate
	   		bag.Write "F",Footer
	   		bag.Write "S",SeparatorTemplate
		End Function
	   
	    Public Function HandleClientEvent(e)
			HandleClientEvent = ExecuteEventFunctionEX(e)
	    End Function			
	   
	    Public Default Function Render()	
				Dim alt
				Dim varStart	 
				
				varStart = Now

	    		If Header<>"" Then
	    			Set fncHeader = GetRef(Header)
	    		End If

	    		If ItemTemplate<>"" Then
	    			Set fncItemTemplate = GetRef(ItemTemplate)
	    		End If

	    		If AlternateItemTemplate<>"" Then
	    			Set fncAlternateItemTemplate = GetRef(AlternateItemTemplate)
	    		Else
	    			Set fncAlternateItemTemplate = fncItemTemplate
	    		End If

	    		If Footer<>"" Then
	    			Set fncFooter = GetRef(Footer)
	    		End If

	    		alt = 0
	    		If Not fncHeader Is Nothing Then fncHeader()
	    		
	    		While Not DataSource.EOF
	    			
	    			If alt = 0 Then
	    				If Not fncItemTemplate Is Nothing Then fncItemTemplate(DataSource)	
	    			Else
	    				If Not fncAlternateItemTemplate Is Nothing Then fncAlternateItemTemplate(DataSource)	
	    			End If	    			
	    			alt = 1 - alt	    			
	    			If Not fncSeparatorTemplate Is Nothing Then fncSeparatorTemplate()
	    				    			
	    			DataSource.MoveNext
	    		Wend
	    		
	    		If Not fncFooter Is Nothing Then fncFooter()
				
				Page.TraceRender varStart,Now,Control.Name
				
	    End Function

	End Class

%>