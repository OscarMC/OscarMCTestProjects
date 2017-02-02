<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::Server Button

	 Dim ucontrolCount
	 'ucontrolCount = 0	
	 Class UserControl
		Private mobjWebControl				
		Dim Controls
		Dim Control
		Dim txtUserName
		Dim txtPassword
		Dim cmdSubmit
		
		Private Sub Class_Initialize()
			
			Set mobjWebControl = New WebControl	
			Set Controls = mobjWebControl.Controls
			Set Control = mobjWebControl
			Set mobjWebControl.Owner = Me
			
			Set txtUserName = New ServerTextBox
			Set txtPassword  = New ServerTextBox
			Set cmdSubmit   = New ServerButton
			
			Control.Name = "LF" & ucontrolCount
			txtUserName.Control.Name = "LFLogin" & ucontrolCount
			txtPassword.Control.Name = "LFPassword" & ucontrolCount
			txtPassword.Mode = 2
			cmdSubmit.Control.Name   = "LFcmdSubmit" & ucontrolCount
			cmdSubmit.Text = "Login"
			ucontrolCount = ucontrolCount + 1
			
			Set txtUserName.Control.Parent = Me
			Set txtPassword.Control.Parent = Me
			Set cmdSubmit.Control.Parent   = Me
			
	   End Sub
	   
	   	Public Function ReadProperties(vs)
		End Function
		
		Public Function WriteProperties(vs)						
		End Function
	   
	   Public Sub HandleClientEvent(e)
	   End Sub					
	   
	   Public Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If
			 						 
			 varStart = Now
			 Response.Write  "<DIV Style = 'background-color:white;width:100%;border black 1px solid'>" 
			 Response.Write  "User Name:" 
			 txtUserName.Render 
			 Response.Write "<BR>Password:" 
			 txtPassword 
			 Response.Write  "<BR>" 
			 cmdSubmit.Render
			 Response.Write "<BR></DIV>"
			 Page.TraceRender varStart,Now,Control.Name
			 
		End Function

	End Class

%>