<!--#Include File = "..\Server_TextBox.asp" -->
<!--#Include File = "..\Server_LinkButton.asp" -->
<!--#Include File = "..\Server_Label.asp" -->
<%
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::User Control Sample

	 Class LoginControl
		
		Dim Controls
		Dim Control
		Private txtLoginName
		Private txtPassword
		Private cmdLogin
			
		Private Sub Class_Initialize()
			
			Set Control = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 			
		
			Set txtLoginName = New ServerTextBox
			Set txtPassword  = New ServerTextBox
			Set cmdLogin     = New ServerLinkButton
	   End Sub

		Public Sub OnInit()
			txtLoginName.Control.Name = Control.Name  & "_txtLogin"
			txtPassword.Control.Name = Control.Name  & "_txtPassword"
			cmdLogin.Control.Name = Control.Name  & "_Login"			
			cmdLogin.Text = "Login"
			txtPassword.Mode  = 2
		End Sub
		
	   	Public Function ReadProperties(bag)				   	
		End Function
		
		Public Function WriteProperties(bag)			
		End Function
	   
	   Public Function HandleClientEvent(e)
		 	HandleClientEvent = False	
	   End Function					
	   
	   
	
	   Public  Default Function Render()
			
			 Dim varStart	 
			 
			 If Control.Visible = False Then
				Exit Function
			 End If
			 varStart = Now			
			%>
				<Table border=0 bordercolor='black' cellspacing=0>
					<TR><TD>Login</TD><TD><%txtLoginName%></TD></TR>
					<TR><TD>Password</TD><TD><%txtPassword%></TD></TR>
					<TR><TD colspan=2 align=right><%cmdLogin%></TD></TR>
				</Table>
			<%

		 	 Page.TraceRender varStart,Now,Control.Name		 	 
		End Function

	End Class

%>