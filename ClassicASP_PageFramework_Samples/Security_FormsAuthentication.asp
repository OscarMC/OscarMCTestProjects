<%

Set Security = New FormsAuthentication

'FORMS AUTHENTICATION...	
	Class FormsAuthentication
		
		Private mIsAuthenticated
		Private mUserName
		Private mRoles
		'Dim mRoles
		Private mFormsCookieName
		Private mobjCookie
	
		Private mScriptName
		
		Dim RequireSSL
		Dim SlidingExpiration
		Dim LoginURL
	
		Private objSecurityXML
		
		Private Sub Class_Initialize()			
			SlidingExpiration = False
			RequireSSL  = False
			mFormsCookieName = "FATOKEN"			
			Call Init()			

			'Get the name of the script-- first time only!
			If Not Page.IsPostBack Then
				mScriptName = Request.ServerVariables("SCRIPT_NAME")
				Page.ViewState.Add "__FOSN",mScriptName
			Else
				mScriptName = Page.ViewState.GetValue("__FOSN")
			End If
			
			LoginURL = "/PageFrameworkV2/Samples/Login.asp"			
		End Sub
		
		Public Property Get ScriptName
			ScriptName = mScriptName
		End Property
		
		Private Sub Init()
			Dim FaToken						
			'Encrypt/Decrypt?
			Set mobjCookie = Request.Cookies(mFormsCookieName)
			FaToken = GetCookieValue("FATOKEN")			
			If FaToken = "" Then
				mIsAuthenticated = False
				mUserName = ""
			Else
				mIsAuthenticated = True
				mUserName = GetCookieValue("FAUSER")
				mRoles    = Split(GetCookieValue("FAROLES"),"$$")
			End If
		End Sub
		
		Public Property Let FormsCookieName(ByVal v)
			mFormsCookieName = v
			Call Init()
		End Property

		Public Property Get FormsCookieName
			FormsCookieName = mFormsCookieName
		End Property
		
		Private Function GetCookieValue(key)			
			If Not mobjCookie Is Nothing Then
				GetCookieValue = mobjCookie(key)
			Else
				GetCookieValue = ""	
			End If
		End Function
		
		'This will make the user be authenticated
		Public Sub SetAuthCookie(ByVal userName,ByVal roles, ByVal createPersistentCookie)			
			'EncodeKey(UserName,Session.SessionID)
			'EncodeKey(join(roles,"$$"),Session.SessionID)			
			Response.Cookies(mFormsCookieName)("FATOKEN")="XZU"
			Response.Cookies(mFormsCookieName)("FAUSER")=userName
			Response.Cookies(mFormsCookieName)("FAROLES")=join(roles,"$$")
			mUserName = userName
			mRoles = roles
		End Sub
		
		'Authenticates from a user store... (change to anything u want...)
		'Call SetAuthCookie from here...
		Public Function Authenticate(user,password)
		End Function
		
		Public Property Get IsAuthenticated
			IsAuthenticated = (mUserName<>"")
		End Property
		
		Public Property Get UserName
			UserName = mUserName
		End Property
		
		Public Property Get Roles
			If  isArray(mRoles) Then
				Roles = mRoles
			Else
				Roles = Array("")
			End If
		End Property

		Public Function IsInRole(rol)
		End Function

		Public Function IsInRoles(roles)
		End Function		
		
		Public Function SignOut()
			Response.Cookies(mFormsCookieName).Expires = Now-1
			mUserName = ""
			Redim mRoles(0)
		End Function
		
		Public Sub RedirectFromLoginPage(defaultURL)
			If GetRedirectURL<>"" Then
				Response.Redirect GetRedirectURL
				Response.End
			Else
				Response.Redirect defaultURL
			End If
			Response.End				
		End Sub
		
		Public Function  GetRedirectURL()
			GetRedirectURL = Request.QueryString("returnURL")
		End Function

	End Class

	Private Function Page_Authenticate_Request()
			
		'ExecuteEventFunction "Page_Init_Security"
		'that event which should be defined (if needed) in the asp page should do something like
		'Page.Users = Array(Array("allow","?","ChrisCalderon"),Array("deny","*"))
		'Page.Roles = Array(Array("allow","Admin","ChristianRole"))
		'If CanAccessPage() Then Else .. else redirect no access...
		
		Dim rs 
		Set rs = New RoleSecurity		
		Response.Write "CAN ACCESS?:" &  rs.CanAccess(Security.ScriptName,Security.userName,Security.roles )
		Set rs = Nothing
		'If Not Security.IsAuthenticated And Instr(Security.ScriptName,"Login.asp")=0 Then
		'	Response.Redirect Security.LoginURL &  "?returnURL=" & Server.URLEncode( Security.ScriptName )
		'End If				
	End Function


	Function EncodeKey(myStr, inKey)
	  
	  If (inKey = "") Then
	    Response.Write("Error: 1")
	    Exit Function
	  End If
	  
	  If (myStr = "") Then
	    Response.Write("Error: 2")
	    Exit Function
	  End If
	  
	  Dim i, tmpNum, tmpChr, tmpAsc, newAsc, retStr
	  
	  tmpNum = 1
	  retStr = ""
	  
	  For i = 1 To Len(myStr)
	    tmpChr = Mid(myStr, i, 1)
	    tmpAsc = Asc(tmpChr)
	    newAsc = tmpAsc - Asc(Mid(inKey, tmpNum, 1))
	    If (newAsc <= 0) Then newAsc = 255 + newAsc
	    retStr = retStr & Chr(newAsc)
	    tmpNum = tmpNum + 1
	    If (tmpNum > Len(inKey)) Then tmpNum = 1
	    
	  Next
	  
	  EncodeKey = retStr
	  
	End Function



	Function DecodeKey(myStr, inKey)
	  
	  If (inKey = "") Then
	    Response.Write("Error: 3")
	    Exit Function
	  End If
	  
	  If (myStr = "") Then
	    Response.Write("Error: 4")
	    Exit Function
	  End If
	  
	  Dim i, tmpNum, tmpChr, tmpAsc, newAsc, retStr
	  
	  tmpNum = 1
	  retStr = ""
	  
	  For i = 1 To Len(myStr)
	    tmpChr = Mid(myStr, i, 1)
	    tmpAsc = Asc(tmpChr)
	    newAsc = tmpAsc + Asc(Mid(inKey, tmpNum, 1))
	    If (newAsc > 255) Then newAsc = newAsc - 255
	    retStr = retStr & Chr(newAsc)
	    tmpNum = tmpNum + 1
	    If (tmpNum > Len(inKey)) Then tmpNum = 1
	    
	  Next
	  
	  DecodeKey = retStr
	  
	End Function




Class RoleSecurity
	Private msxml
	
	Private Sub Class_Initialize()	
		Dim FileName
		'If Application.Contents.Item("Security")  Then
		'	Application.Lock
				Set Application("Security") = Server.CreateObject("MSXML2.FreeThreadedDOMDocument")
				Application("Security").setProperty "SelectionLanguage", "XPath"				
				FileName = Server.MapPath("/PageFrameworkV2/TEST.XML")
				Application("Security").Load(Cstr(FileName))				 
		'	Application().UnLock
		'End If	
		Set msxml = Application("Security")
	End Sub

Public Function CanAccess(ByVal ScriptName, ByVal UserName, ByVal Roles)
    
    Dim Script
    Dim Folder
    Dim varData
    Dim RolesFilter
    Dim role
    Dim XpathQuery
    
    CanAccess = True
    
    ScriptName = Replace(ScriptName, "\", "/")
    varData = Split(ScriptName, "/")
    
    Folder = varData(0)
    Script = varData(UBound(varData))
    For Each role In Roles
        RolesFilter = RolesFilter & IIf(RolesFilter = "", "", "or") & " contains(@roles,'[" & role & "]') "
    Next
    
    If RolesFilter <> "" Then
        RolesFilter = " or " & RolesFilter
    End If
    
    If UserName <> "" Then
        UserName = "[" & UserName & "]"
    End If
    
    CanAccess = CanAccessPage(Folder, Script, UserName, RolesFilter)
    
    
End Function

Public Function CanAccessPage(ByVal FolderPath, ByVal PageName, ByVal UserName, ByVal RolesFilter) 
    Dim node 'As IXMLDOMNode
    Dim nodes 'As IXMLDOMNodeList
    Dim attRole' As IXMLDOMNode
    Dim attUser 'As IXMLDOMNode
    Dim XpathQuery
    
    CanAccessPage = CanAccessFolder(FolderPath, UserName, RolesFilter)
    If Not CanAccessPage Then
        Exit Function
    End If
    
    XpathQuery = "//FOLDER[@path='{f}']/PAGE[@name='{p}']/allow[{u} @users='*' or @users='?' " & RolesFilter & "] | " &_
                 "//FOLDER[@path='{f}']/PAGE[@name='{p}']/deny[ {u} @users='*' or @users='?'  " & RolesFilter & "] "
    
    XpathQuery = Replace(XpathQuery, "{f}", FolderPath, 1, 2)
    XpathQuery = Replace(XpathQuery, "{p}", PageName, 1, 2)
    
    If UserName <> "" Then
        UserName = "contains(@users,'" & UserName & "') or"
        XpathQuery = Replace(XpathQuery, "{u}", UserName, 1, 2)
    Else
        XpathQuery = Replace(XpathQuery, "{u}", "", 1, 2)
    End If
                
    Set nodes = msxml.selectNodes(XpathQuery)
    
    'First rule in the config.wins... (unless '?')
    CanAccessPage = True
    For Each node In nodes
        Set attRole = node.Attributes.getNamedItem("roles")
        Set attUser = node.Attributes.getNamedItem("users")
        
        Select Case node.nodeName
            
            Case "deny"
                    If Not attUser Is Nothing Then
                        If attUser.Text = "?" Then 'Deny anonymous?, lets see
                            If UserName = "" Then 'Anonymous?, kick out!
                                CanAccessPage = False
                                Exit For
                            End If
                        Else
                            CanAccessPage = False
                            Exit For
                        End If
                    End If
                    
                    If Not attRole Is Nothing Then
                        CanAccessPage = False
                        Exit For
                    End If
                    
            Case "allow"
                If Not attUser Is Nothing Then
                    Select Case attUser.Text
                        Case "?"
                            If UserName <> "" Then
                                CanAccessPage = True
                                Exit For
                            End If
                        Case Else
                            CanAccessPage = True
                            Exit For
                        End Select
                    End If
                    
                    If Not attRole Is Nothing Then
                        CanAccessPage = True
                        Exit For
                    End If
            End Select
    Next
End Function


Public Function CanAccessFolder(ByVal FolderPath, ByVal UserName, ByVal RolesFilter)
    Dim node 'As IXMLDOMNode
    Dim nodes 'As IXMLDOMNodeList
    Dim attRole' As IXMLDOMNode
    Dim attUser 'As IXMLDOMNode
    Dim XpathQuery
    
    XpathQuery = "//FOLDER[@path='{f}']/allow[{u} @users='*' " & RolesFilter & "] | " & _
                 "//FOLDER[@path='{f}']/deny[ {u} @users='*'" & RolesFilter & "] "
    
    XpathQuery = Replace(XpathQuery, "{f}", FolderPath, 1, 2)
    
    If UserName <> "" Then
        UserName = "contains(@users,'" & UserName & "') or @users='?' or"
        XpathQuery = Replace(XpathQuery, "{u}", UserName, 1, 2)
    Else
        XpathQuery = Replace(XpathQuery, "{u}", "", 1, 2)
    End If
    
            
    Set nodes = msxml.selectNodes(XpathQuery)
    
    'First rule in the config.wins... (unless '?')
    CanAccessFolder = True
    
    For Each node In nodes
        Set attRole = node.Attributes.getNamedItem("roles")
        Set attUser = node.Attributes.getNamedItem("users")
        Select Case node.nodeName            
            Case "deny"
                    If Not attUser Is Nothing Then
                        If attUser.Text = "?" Then 'Deny anonymous?, lets see
                            If UserName = "" Then 'Anonymous?, kick out!
                                CanAccessFolder = False
                                Exit For
                            End If
                        Else
                            CanAccessFolder = False
                            Exit For
                        End If
                    End If

                    If Not attRole Is Nothing Then
                        CanAccessFolder = False
                        Exit For
                    End If
                    
            Case "allow"
                CanAccessFolder = True
                Exit For
            End Select                    
    Next

End Function
End Class

%>



