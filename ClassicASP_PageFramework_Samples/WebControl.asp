<%@ Language=VBScript%> 
<%Option Explicit%>
<!--#Include File = "StringBuilder.asp"-->

<%
'**********************************************************************************************************************
'CLASSIC ASP FRAMEWORK V0.1, (stable version)
'BY CHRISTIAN JAVIER CALDERON A. 
'EMAIL: CCALDERON@HOTMAIL.COM
'Please forward comments to CCALDERON@HOTMAIL.COM only on MONDAYS AND FRIDAYS, all other days I have every incoming email blocked, this is due the 
'huge amount of spam I get... it sucks!.  The other way to contact me is through my email address listed in www.asp.net, I'm user ccalderon
'Now, the rules:
'1) Use of this software is also done so at your own risk. The code is supplied as is without warranty or guarantee of any kind.
'2) This header MUST remain intact, you cannot remove it. You may add your credits if you make any modifications.
'3) It is expressly forbidden to sell this source code. You can use it in your proyects, but you cannot sell it as a framework.
'4) You are free to use the source code in your own code, but you may not claim that you created the code. 
'
'THE CREDITS (CODE THAT I USED):
'The ViewState Component (Base64 stuff): Contains cryptography software by David Ireland of "DI Management Services Pty Ltd <www.di-mgt.com.au>."
'
'This header alplies also to the following source code every file included in this framework.
'
'*********************************************************************************************************************

Response.buffer=true

Response.CacheControl = "no-cache"
Response.AddHeader "Pragma", "no-cache"
Response.ExpiresAbsolute = Now()- 1000
Response.Expires = 0
	

CONST VIEW_STATE_MODE_SERVER = 2
CONST VIEW_STATE_MODE_CLIENT = 1

Dim PAGE_CSS

Dim Page
Dim Security 'Set at the security module level, they'all should have the same interface...
Set Security = Nothing

Set Page = Nothing 'Yeps, yah have to do it before!
Set Page = New cPage



':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::MAIN PAGE CONTROLER
	Public Sub Main()

		Page.HandleServerEvent "Page_Authenticate_Request"
		Page.HandleServerEvent "Page_Authorize_Request"
		Page.HandleServerEvent "Page_Init"
		If Not Page.IsPostBack Then 'Just once...
			Page.HandleServerEvent "Page_Controls_Init"
		End If
		If Page.IsPostBack Then
			Page.HandleServerEvent "Page_LoadViewState"			
		End If
		Page.HandleServerEvent "Page_Load"			
		If Page.IsPostBack Then			
			Page.TraceCall Page.Control,"Handle PostBack -Start"
			Page.HandleClientEvent ""
			Page.TraceCall Page.Control,"Handle PostBack -End"
		End If
		Page.HandleServerEvent "Page_PreRender"
		Page.HandleServerEvent "Page_SaveViewState" 'Collects the viewstate

	End Sub

':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'Include your authentication handler here (e.g. Security_FormsAuthentication.asp)
'Add the # in fron of the include and you will see! :-)
%>
<!-- Include File = "Security_FormsAuthentication.asp"-->
<%

':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::EVENT
   Class ClientEvent
		Dim EventName
		Dim Source
		Dim Instance
		Dim CommandSource
		Dim ExtraMessage
		Dim TargetObject
		Public Property Get EventFnc()
			EventFnc = Source & "_" & EventName
		End Property
	End Class
	
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::PROPERTY BAG WRAPPER -- SORT OF :-)
	Class PropertyBag
		Dim Node
		
		Private Sub Class_Initialize()
			Set Node = Nothing
		End Sub
		
		Public Function Write(Name,Value)
			WriteProperty Name,Value,Node
		End Function

		Public Function Read(Name)		
			Read = ReadProperty(Name,Node)
		End Function

		Public Function ReadBoolean(Name)		
			ReadBoolean = CBool(ReadProperty(Name,Node) & "0")
		End Function

		Public Function ReadInt(Name)		
			ReadInt = ReadProperty(Name,Node)
			If ReadInt = "" Then
				ReadInt = 0
			Else
				ReadInt = CInt(ReadInt)
			End If
		End Function

		Public Function ReadDouble(Name)		
			ReadDouble = ReadProperty(Name,Node)
			If ReadDouble = "" Then
				ReadDouble = 0
			Else
				ReadDouble = CInt(ReadDouble)
			End If			
		End Function
		
		Public Function ReadDate(Name)		
			
		End Function


	End Class
	
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::CONTROLS COLLECTION
   Class ControlsCollection
		Private mintIndex
		Private mvarControlArray
		Public  GrowRate		
		Public  Owner      'Owner Web Control (has to be a WebControl)
						   'Me.Owner = WebControl, Me.Owner.Owner : WebControl Implementation. i.e. textbox
		
		Private Sub Class_Initialize()
			mintIndex = 0
			GrowRate = 5
			Set Owner = Nothing
		End Sub
		
		Public Property Get Count
			Count = mintIndex
		End Property
		
		Public Sub Clear()
			mintIndex = 0
		End Sub
		
		Private Sub ReSize()
			Dim x,mx			
			If mintIndex  = 0 Then			
				'If the control requesting the resize of the Page itself then augment the GrowRate. 
				'Most of the other controls don't need to do this!
				If Owner.Name = "Page" Then
					GrowRate = 50
				End If
				Redim mvarControlArray(GrowRate)
			Else
				Redim Preserve mvarControlArray(Ubound(mvarControlArray) + GrowRate)
			End If
			mx = Ubound(mvarControlArray)
			For x = mintIndex To mx
				Set mvarControlArray(x) = Nothing
			Next
		End Sub
		
		Public Function GetControl(index) Set GetControl = mvarControlArray(index) End Function
		
		Public Sub Add(control)
			Dim obj
			
			If typeName(control) <> "WebControl" Then
				Set obj = control.Control
			Else
				Set obj = control
			End If	
			
			If control Is Me.Owner Then
				Err.Raise   vbObjectError + 2,typename(control.Owner)& ":" & control.Name & ":Controls.Add","Cannot add a Control instance to its own collection."
				Exit Sub
			End If

			If Not control.Parent Is Nothing Then				
				Call control.Parent.Controls.Remove(control)
			End If

			If mintIndex = 0 Then				
				Call ReSize()
			Else
				If mintIndex > UBound(mvarControlArray) Then
				   Call Resize()
				End If
			End If		
			
			
			Set mvarControlArray(mintIndex) = control
			Call control.SetParent(Owner)
			mintIndex = mintIndex + 1
			
		End Sub
		
		Public Sub Remove(objRemove)
			Dim x,mx,obj
			mx = mintIndex - 1
			For x = 0 To mx
				Set obj = mvarControlArray(x)
				If Not obj Is Nothing Then
					If objRemove Is obj Then
						Set mvarControlArray(x) = Nothing
						If x = mintIndex Then						
							 mintIndex = mintIndex - 1 
						Else
							mintIndex = mintIndex - 1
							Set  mvarControlArray(x) = 	  mvarControlArray(mintIndex)
							Set mvarControlArray(mintIndex) = Nothing
						End If
						Exit Sub
					End If	
				End If
			Next
			
		End Sub 
		
		Public Function FindControl(name)
			Dim x,mx,obj

			Set FindControl = Nothing
			mx = mintIndex - 1
			For x = 0 To mx
				Set obj = mvarControlArray(x)
				If Not obj Is Nothing Then
					If obj.Name = name Then
						Set FindControl = obj
						Exit Function
					End If	
				Else
					Exit Function
				End If
			Next
		End Function				
	End Class
	
':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::WEB CONTROL
   Class WebControl
	
		Private mobjParent     'Parent Web Control
		
		Dim EnableViewState 
		Dim Owner          'Web Control Owner, such as text box
		Dim Name
		Dim Controls        'To expose the web controls collection
		Dim Enabled
		Dim Visible    
		
		Dim Style		
		Dim Attributes
		Dim CssClass	
		Dim TabIndex
		Dim ToolTip
		Dim DataTextField
		Dim IsViewStateRestored

		'To make the name of the webcontrols somewhat compatible with ASP.NET you could do something like:
		'Change the Name variable to -> private mName. The thing is that because XML is used to persist viewstate
		'u cannot use weird characters such as ":" or "$" to build the name hierarchy...
		'You could also add a ClientID property and if = "" then = Name. Controls will then need to use it when rendering
		'the tags...
		'Public Property Let Name(ByVal v)
		'	mName = v
		'End Property
		'Public Property Get Name() Name=mName End Property
		'Public Property Get UniqueID()   
		'	Dim n	
		'	If mName = "Page" Then
		'		UniqueID = ""
		'	Else
		'		If Parent Is Nothing Then
		'			UniqueID = mName
		'		Else
		'			n = Parent.Name
		'			UniqueID = IIF(n<>"", "__" & mName , mName)					
		'		End If		
		'	End If
		'End Property				

		Private Sub Class_Initialize()
			
			Set Controls        = New ControlsCollection
			Set Controls.Owner  = Me
			Set Owner           = Page 'ARU U SURE??? Nothing
			Set mobjParent      = Nothing
			
			Style   = ""
			Enabled = True
			Visible = True
			TabIndex = 0
			DataTextField = ""
			IsViewStateRestored = False
			
			'When The PAGE object is created (and it inherits from WebControl) Page will be Nothing so it will
			'not add the page to its collection :-P nice... For all others it will do	
			If Not Page Is Nothing Then
				EnableViewState = Page.Control.EnableViewState
				Page.Controls.Add Me
			End If						
			
		End Sub

		Public Property Get IsVisible()   
			If Visible Then 'Are u really visible?
				If Parent Is Nothing Then 
					IsVisible = True
				Else
					IsVisible = Parent.IsVisible 'This will cascade up in the hierarchy :-)
				End If		
			Else
				IsVisible = False 'If you are not I believe u :-P
			End If
		End Property
								
		Public Property Get HasControls() HasControls = (Controls.Count>0) End Property

		Public Function ReadProperties(vs)
			Dim wc
			Dim bag
			IsViewStateRestored = True
			
			Set wc = GetSection("WC",vs)			
			
			'On Error Resume Next
			
			Visible = CBool(ReadProperty("V",wc) & "0")
			Enabled = CBool(ReadProperty("E",wc) & "0")
			EnableViewState = Cbool(ReadProperty("EV",wc) & "0" )
			TabIndex = CInt("0" & ReadProperty("TI",wc))
			CssClass = ReadProperty("CSS",wc)
			Style    = ReadProperty("ST",wc)
			Attributes    = ReadProperty("AT",wc)
						
			Set bag = New PropertyBag
			Set bag.Node = vs						
			Owner.ReadProperties(bag)
			Set bag = Nothing
			
		End Function
		
		Public Function WriteProperties(vs)
			Dim varState
			Dim wc
			Dim bag
						
			If vs Is Nothing Then
				Exit Function
			End If
			
			Set wc = GetSection("WC",vs)
			
			WriteProperty "V",Visible,wc
			WriteProperty "E",Enabled,wc
			WriteProperty "EV",EnableViewState,wc
			WriteProperty "TI",TabIndex,wc
			WriteProperty "CSS",CssClass,wc
			WriteProperty "ST",Style,wc
			WriteProperty "AT",Attributes,wc
			
			Set bag = New PropertyBag
			Set bag.Node = vs
			
			Owner.WriteProperties(bag)
			vs.AppendChild wc
			Set bag = Nothing
				
		End Function

		Public Sub SetParent(value)
			Dim obj
			Set obj = value
			If typeName(value) <> "WebControl" Then
				Set obj = value.Control
			End If
			
			If Me Is value Then
				Exit Sub
			End If			
			Set mobjParent = value
		End Sub
		
		Public Property Get Parent()	  Set Parent = mobjParent End Property		

		Public Property Set Parent(value) 
			Dim obj
			
			If typeName(obj) <> "WebControl" Then
				Set obj = value.Control
			Else
				Set obj = value	
			End If

			If Me.Name = "Page" Then
				Err.Raise   vbObjectError + 1 ,typename(me) & ":" & Me.Name & "." & Parent,"Cannot Realign the Page's parent!"	
			End If
						
			'Test For Alignment...
			Dim objTest
			Set objTest = obj
			While Not objTest is Nothing
				If Me Is objTest Then
					Response.Write "<BR>Recursive Realignment Detected!!!<BR>"
					Page.TraceImportantCall Me,"WARNING: Recursive Realignment Detected for:" & Me.Name & " is child of " & obj.Name
				End If
				Set objTest = objTest.Parent
			Wend
						
			If mobjParent Is Value Then
				Page.TraceImportantCall Me,"WARNING: Parent Is Value:"  & Me.Name & " - " & mobjParent.Name
			End If
			obj.Controls.Add Me
			
		End Property

		Public Sub HandleServerEvent(EventName)
			If Not Owner Is Nothing Then
				Owner.HandleServerEvent(EventName)
			End If
		End Sub
		
		Public Function HandleClientEvent(EventName)
				Owner.HandleClientEvent(EventName)								
		End Function
		
		Public Function GetControlsMap(node)
			Dim x,mx
			Dim strResults
			mx = Controls.Count  - 1
			If node = 0 Then 
				strResults =  "<B>" & Name & "</B><BR>"
				node = node + 1
			End If
			For x=0 To mx
				strResults =  strResults &  string(node*2,"-") & "> " &  Controls.GetControl(x).Name & "<BR>"
				strResults =  strResults & Controls.GetControl(x).GetControlsMap(node + 1)				
			Next
			GetControlsMap = strResults
		End Function
	
	
		Public Function FindControl(ControlName)
			Dim X,MX,TheControl
			If Me.Name = ControlName Then
				Set FindControl = Me
				Exit Function
			End If
	
			'Check root collection first before navigating down the hierarchy
			Set FindControl = Controls.FindControl(ControlName)
			If Not FindControl Is Nothing Then
				Exit Function
			End If
			
			'Nothing yet?, then lets go down by recursive calling this fnc.
			MX = Controls.Count
			For X = 0 To MX
				Set TheControl = Controls.GetControl(X)
				If TheControl Is Nothing Then
					Set TheControl = Controls.GetControl(X).FindControl(ControlName)
				Else
					Exit For
				End If
			Next					
		End Function
		
	End Class

':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::PAGE CONTROL
	 Class cPage
	
		Dim Controls
		Dim Control
			
		Private mobjRenderTrace
		Private mdteStartRender
		Private mdteEndRender
		Private mvarRenderTrace
		Private mvarRenderTraceRow
		Private mobjStackTrace
		Private mvarStackTraceRow
		Private mvarPostBackObjTypeName	
		Private mobjClientScripts
		Private mobjErrorTrace
		Private mobjValidations
		
		Dim CompressFactor
		Dim IsCompressed
				
		Dim IsRedirectedPostBack		
		Dim IsPostBack		
		Dim Action
		Dim ActionSource
		Dim ActionSourceInstance
		Dim ActionXMessage
				
		Dim DebugEnabled		
		Dim DebugLevel
		Dim ViewStateMode
		Dim ViewState
		Dim PageFormAction 'Don't touch it unless you want to post to another page!
		Dim AutoResetFocus
		Dim AutoResetScrollPosition
		Dim OnSubmitStatement
		
		
		Private mintTabIndex
						
		Private Sub Class_Initialize()
			
			mdteStartRender = Now
			
			Set Control = New WebControl	
			Set Controls = Control.Controls
			Set Control.Owner = Me 'To implement "Inheritance" or use an aggreational pattern
			
			Set mobjStackTrace  = New StringBuilder
			Set mobjRenderTrace = New StringBuilder
			Set mobjErrorTrace  = New StringBuilder
			Set mobjValidations = Nothing
			Set mobjClientScripts = Nothing
						
			AutoResetScrollPosition = False
			AutoResetFocus = False
			DebugEnabled = True
			DebugLevel = 3' 3) Display everything. 2) Display Controls Tree, Stack Trace, Forms Collection, etc. 1) For Postback data only
			
			mvarPostBackObjTypeName = ""						
			Control.EnableViewState = True 
			mintTabIndex = 1
			CompressFactor = -1																		

			mvarRenderTraceRow = 0			
			mvarStackTraceRow  = 0
			Control.Name = "Page"						
			
			ViewstateMode = VIEW_STATE_MODE_CLIENT
			Set ViewState = CreateObject("ASPFramework.ViewState")
			
			IsRedirectedPostBack = (Request("__ISREDIRECTEDPOSTBACK") <> "")						
			IsPostBack           = (Request("__ISPOSTBACK")= "True") And Not IsRedirectedPostBack
			If IsPostBack Then
				Action		         = Request("__ACTION")
				ActionSource         = Request("__SOURCE")
				ActionSourceInstance = Request("__INSTANCE")			
				ActionXMessage       = Request("__EXTRAMSG")					
				IsCompressed = CBool(IIF(Request("__VIEWSTATECOMPRESSED")="","False",Request("__VIEWSTATECOMPRESSED")))
				
				If Session("VS")<> "" Then
					ViewstateMode = VIEW_STATE_MODE_SERVER
					Call ViewState.LoadViewStateBase64(Session("VS"),IsCompressed)
				Else				
					ViewstateMode = VIEW_STATE_MODE_CLIENT
					Call ViewState.LoadViewStateBase64(Request("__VIEWSTATE"),IsCompressed)
				End If								
			Else			
				Session("VS") = ""
				IsPostBack = False
				IsCompressed = False
				Dim obj,node 'Create the page node the first time so it is available right away
				ViewState.GetDomObject obj
				Set node = GetSection("PAGE",obj.ChildNodes(0))
				Call Page_WriteViewState(Me.Control,node)
				Call obj.ChildNodes(0).AppendChild(Node)				
			End If
			
		End Sub
		
		Private Sub Class_Terminate()
			mdteEndRender = Now
			HandleServerEvent("Page_Terminate")
			
			If DebugEnabled Then
				
				If IsPostBack Or IsRedirectedPostBack Then
					OpenDebugSection "PostBack Event Datta"
						WriteToDebugSection "IsPostBack",IsPostBack
						WriteToDebugSection "Is RedirectedPostBack",IsRedirectedPostBack
						WriteToDebugSection "Event",Request("__ACTION")
						WriteToDebugSection "Object",Request("__SOURCE")
						WriteToDebugSection "Instance",Request("__INSTANCE")
						WriteToDebugSection "Extra Message",Request("__EXTRAMSG")
					CloseDebugSection
				End If
				
				PrintIISCollectionFor Request.Form,"Form Collection"
				PrintIISCollectionFor Request.Cookies,"Cookies Collection"
				PrintIISCollectionFor Request.QueryString,"Query String"
				PrintIISCollectionFor Application.Contents,"Application Contents"				
				PrintIISCollectionFor Session.Contents,"Session Contents"
				OpenDebugSection "Session Information"
					WriteToDebugSection "ID",Session.SessionID
					WriteToDebugSection "Timeout",Session.Timeout
					WriteToDebugSection "CodePage",Session.CodePage
				CloseDebugSection

				OpenDebugSection "Response Information"
					WriteToDebugSection "Content Type",Response.ContentType
					WriteToDebugSection "Cache Control",Response.CacheControl
					WriteToDebugSection "Status",Response.Status
				CloseDebugSection
					
				Response.Write("<HR><BR><Span style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro;width:100%'>Errors Trace</span><BR><TABLE width='100%' cellpadding=0 cellspacing=0 style='font-family:verdana'><TR style='background-color:black;color:white;font-family:verdana'><TD>Control</TD><TD>Msg</TD><TD>Severity</TD></TR>")
				Response.Write(mobjErrorTrace.ToString())
				Response.Write("</TABLE><HR>")		
				
				If Page.DebugLevel > 1 Then
					'Prints the Render Trace
					Response.Write("<BR><Span style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro;width:100%'>Trace Output</span><BR><TABLE width='100%' cellpadding=0 cellspacing=0 style='font-family:verdana;font-size:8pt'><TR style='background-color:black;color:white;font-family:verdana'><TD>Control</TD><TD>From</TD><TD>To</TD></TD><TD>Total</TD></TR>")
					Response.Write( "<TR style='font-weight:bold'><TD>Page</TD><TD>" & mdteStartRender & "</TD><TD>" &  mdteEndRender & "</TD><TD>" & DateDiff("s",mdteStartRender,mdteEndRender )  & "</TD></TR>" )								
					Response.Write(mobjRenderTrace.ToString())
					Response.Write("</TABLE><HR>")		

					'Prints the stack trace
					Response.Write("<BR><Span style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro;width:100%'>Calls Trace</span><BR><TABLE width='100%' cellpadding=0 cellspacing=0 style='font-family:verdana;font-size:8pt'><TR style='background-color:black;color:white;font-family:verdana'><TD>Control</TD><TD>Msg</TD><TD>Time</TD></TR>")
					Response.Write(mobjStackTrace.ToString()) 'mvarStackTrace)
					Response.Write("</TABLE><HR>")
				
					'Prints the Controls Tree
					Response.Write "<BR><Span style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro;width:100%'>Controls Tree</span><BR><span style='font-size:8pt'>" & Page.Control.GetControlsMap(0) & "</span><HR>"
				End If

				If Page.DebugLevel>2 Then
					PrintIISCollectionFor Request.ServerVariables,"Server Variables"
					
					OpenDebugSection "View State Information"
						WriteToDebugSection "Size",Len(Page.ViewState.GetViewState)
						WriteToDebugSection "Is Compressed",Page.IsCompressed
						WriteToDebugSection "Compress Factor",Page.CompressFactor & " (in bytes)"
						If Page.IsCompressed Then
							WriteToDebugSection "Compressed Size",Len(Page.ViewState.GetViewStateBase64(Page.CompressFactor))
						End If
					CloseDebugSection									
					Response.Write "<BR><textarea rows=20  style='width:100%' >" & Page.ViewState.GetViewState & "</textarea>"
				End If
								
			End If			

			Set ViewState = Nothing
			
			'To keep the focus in the same control after a postback, if any, if possible... nice!			
			If IsPostBack Then
				If AutoResetFocus Then
					Response.Write(vbNewLine &  "<SCRIPT LANGUAGE='JavaScript'>")
					Response.Write "__AutoResetFocus('" & ActionSource & "','" & ActionSourceInstance & "');"
					Response.Write "</SCRIPT>"
				End If
				If AutoResetScrollPosition Then
					Response.Write(vbNewLine &  "<SCRIPT LANGUAGE='JavaScript'>")
					Response.Write " document.body.scrollTop = " & Request.Form("__SCROLLTOP")
					Response.Write "</SCRIPT>"
				End If
			End If						
			Call RenderClientScripts()
			If Page.OnSubmitStatement<>"" Then
				Response.Write(vbNewLine &  "<SCRIPT LANGUAGE='JavaScript'>")
				Response.Write(vbNewLine &  "function __onSubmit() {")
					Response.Write "return " & Page.OnSubmitStatement
				Response.Write(vbNewLine &  "}")
				Response.Write(vbNewLine &  "document.frmForm.onSubmit=__onSubmit();")				
				Response.Write(vbNewLine &  "</SCRIPT>")
			End If
		End Sub

		Private Sub OpenDebugSection(SectionName)
				Response.Write "<BR><TABLE width=100% border=0 cellspacing=0>"
				Response.Write "<TR style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro'><TD  colspan=2>" & SectionName & "</TD></TR>"
				Response.Write "<TR style='font-size:8pt;font-family:verdana;background-color:Black;color:white'><TD>Property</TD><TD>Value</TD></TR>"
		End Sub
		Private Sub CloseDebugSection()
				Response.Write "</TABLE>"
		End Sub
		Private Sub WriteToDebugSection(propName,propValue)
				Response.Write "<TR><TD width=25% nowrap style='color:navy;font-size:8pt;font-weight:bold'>"
				Response.Write propName
				Response.Write "</TD><TD style='font-size:8pt;'>" & propValue & "</TD></TR>"
		End Sub

		Private Function PrintIISCollectionFor(obj,title)
				Dim x,varKey,tName
				If obj.Count = 0  Then
					Exit Function
				End If
				Response.Write "<BR><Span style='font-size:10pt;font-family:verdana;font-weight:bold;background-color:Gainsboro;width:100%'>" & title & "</span><BR>"
				Response.Write "<TABLE width='100%' cellpadding=2 cellspacing=0 style='font-family:verdana'><TR style='background-color:black;color:white;font-family:verdana;font-size:10pt'><TD>Item</TD><TD>Value</TD></TR>"
				For x = 1 To obj.Count
					varKey = obj.Key(x)
					If varKey<> "__VIEWSTATE"  Then
						Response.Write "<TR><TD valign=top style='font-size:8pt;color:navy;font-weight:bold'width=25% nowrap  >"  &  varKey & "</TD>"
						
						If IsObject(obj.Item(varKey) ) Then
							tName = TypeName(obj.Item(varKey) ) 
							If tName = "IStringList" Then
								tName = obj.Item(varKey)
							End If
							Response.Write "<TD valign=top style='font-size:8pt'>"  & tName  & "</TD></TR>"
						Else
							Response.Write "<TD valign=top style='font-size:8pt'>"  & Server.HTMLEncode( obj.Item(varKey) ) & "</TD></TR>"
						End If
					End If
				Next
				Response.Write("</TABLE><HR>")		
		End Function


		Public Function RegisterValidation(object,vtype,msg,def)			
			'Format: object, friendly name, format, required, def)
			If  mobjValidations Is Nothing Then
				Set mobjValidations = New StringBuilder
			End If
			mobjValidations.Append "__ValidateObject('" & object.Control.Name & "'," & vtype & ",'" & msg & "','" & def & "');" & vbNewLine
		End Function
		
		Public Sub RegisterClientScript(ScriptName,ScriptText)
			If mobjClientScripts Is Nothing Then
				Set mobjClientScripts = CreateObject("ASPFramework.ViewState")
			End If
			mobjClientScripts.Add ScriptName,ScriptText			
		End Sub
		
		Private Function RenderClientScripts()
			Dim x,mx

			If Not 	mobjClientScripts Is Nothing Then
				mx  = mobjClientScripts.Count - 1				
				For x = 0 To mx
					Response.Write mobjClientScripts.GetValueByIndex(x) 
					Response.Write vbNewLine
				Next				
				Set mobjClientScripts = Nothing				
			End If
			
			If Not mobjValidations Is Nothing Then
				Response.Write "<script language='javascript'>" & vbNewLine
				Response.Write mobjValidations.ToString()
				Response.Write vbNewLine & "</script>" & vbNewLine
			End If
					
		End Function
		
		Public Function GetNextTabIndex()
			GetNextTabIndex = mintTabIndex
			mintTabIndex = mintTabIndex + 1
		End Function
		
		Public Function GetEventScript(ClientTrigger, ObjectName, EventName, ByVal Instance, ExtraMessage)
			If Instance <> "this" Then
				Instance = "'" & Instance & "'"
			End If
			GetEventScript = " " & ClientTrigger & " = ""javascript:doPostBack('" & EventName  & "','" & ObjectName & "'," & Instance & ",'" & ExtraMessage  & "')"" "
		End Function
		
		Public Function GetEventScriptRedirect(ClientTrigger, ObjectName, EventName, ByVal Instance, ExtraMessage,Action)
			If Instance <> "this" Then
				Instance = "'" & Instance & "'"
			End If
			GetEventScriptRedirect = " " & ClientTrigger & " = ""javascript:doPostBack('" & EventName  & "','" & ObjectName & "'," & Instance & ",'" & ExtraMessage  & "','" & Action & "')"" "
		End Function

		Public Function GetEventScriptNewWindow(ClientTrigger, ObjectName, EventName, ByVal Instance, ExtraMessage,Action,TargetWindow)
			If Instance <> "this" Then
				Instance = "'" & Instance & "'"
			End If
			GetEventScriptNewWindow = " " & ClientTrigger & " = ""javascript:doPostBack('" & EventName  & "','" & ObjectName & "'," & Instance & ",'" & ExtraMessage  & "','" & Action & "','" & TargetWindow & "')"" "
		End Function
		
		Public Sub TraceRender(FromStart,ToEnd,varControl)
			Dim varValue
			
			If DebugEnabled Then
				If varControl = ActionSource Then
					varValue = "<span style='color:red;font-weight:bold'>(" & varControl & ")</span>"
				Else
					varValue = varControl
				End If
				mobjRenderTrace.Append  "<TR" & iif(mvarRenderTraceRow=0," style='background-color:lightblue'","") & "><TD>"
				mobjRenderTrace.Append varValue
				mobjRenderTrace.Append "</TD><TD>"
				mobjRenderTrace.Append FromStart
				mobjRenderTrace.Append "</TD><TD>"
				mobjRenderTrace.Append ToEnd
				mobjRenderTrace.Append "</TD><TD>"
				mobjRenderTrace.Append DateDiff("s",FromStart,ToEnd )
				mobjRenderTrace.Append  "</TD></TR>" & vbNewLine								
				mvarRenderTraceRow = 1 - mvarRenderTraceRow 
			 End If
		End Sub						
		
		Public Sub TraceCall(obj,msg)
			If DebugEnabled Then
				mobjStackTrace.Append "<TR " &  iif(mvarStackTraceRow=0," style='background-color:lightblue'","")  & "><TD>"
				mobjStackTrace.Append obj.Name
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append msg
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append Now
				mobjStackTrace.Append "</TD></TR>"				
				mvarStackTraceRow = 1 - mvarStackTraceRow
			End If
		End Sub

		Public Sub TraceMsg(method,msg)
			If DebugEnabled Then
				mobjStackTrace.Append "<TR " &  iif(mvarStackTraceRow=0," style='background-color:lightblue'","")  & "><TD>"
				mobjStackTrace.Append method
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append msg
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append Now
				mobjStackTrace.Append "</TD></TR>"				
				mvarStackTraceRow = 1 - mvarStackTraceRow
			End If
		End Sub


		Public Sub TraceImportantCall(obj,msg)
			If DebugEnabled Then
				mobjStackTrace.Append "<TR " &  iif(mvarStackTraceRow=0," style='background-color:lightblue;color:red;font-weight:bold'"," style='color:red;font-weight:bold' ")  & "><TD>"				
				mobjStackTrace.Append obj.Name
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append msg
				mobjStackTrace.Append "</TD><TD>"
				mobjStackTrace.Append Now
				mobjStackTrace.Append "</TD></TR>"				
				mvarStackTraceRow = 1 - mvarStackTraceRow
			End If
		End Sub

		
		Public Sub TraceError(Control,Message,Severity)		
				If Err.number > 0 Then
					mobjErrorTrace.Append "<TR><TD>" & IIF(Not Control Is Nothing,Control.Name,"") & "</TD><TD>" & err.Number & ":" & err.Description & "<BR>" & Message & "</TD><TD>" & Severity & "</TD></TR>" & vbNewLine
					Err.Clear
				End If
				
				On Error Goto 0 'Reset Error
		End Sub
		
		Public Function ReadProperties(bag)
			'Page.TraceCall Page.Control, "Page->Getting state" 
		End Function
		
		Public Function WriteProperties(bag)
			'Page.TraceCall Page.Control, "Page->Setting state" 
		End Function
		
		Private Function Page_WriteViewState(obj,node)
			Dim x,mx
			Dim objChildNode	
			Dim objControl
			mx = obj.Controls.Count  - 1			
			
			If obj.EnableViewState Then
				obj.WriteProperties(node)
			End If
			
			For x=0 To mx
				Set objControl   = obj.Controls.GetControl(x)
				Set objChildNode = GetSection(objControl.Name,node)
				Call Page_WriteViewState(objControl,objChildNode)
				node.AppendChild objChildNode
			Next		
				
			Set obj = Nothing
			Set objControl = Nothing
			Set objChildNode = Nothing
			
		End Function

		Private Function Page_ReadViewState(obj,node)
			Dim x,mx
			Dim objControl
			Dim objChildNode
			
			If obj.EnableViewState And Not obj.IsViewStateRestored Then
				obj.ReadProperties Node
			End If
			
			mx = obj.Controls.Count  - 1
			For x = 0 To mx 
				Set objControl   = obj.Controls.GetControl(x)
				Set objChildNode = GetSection(objControl.Name,node)
				Call Page_ReadViewState(objControl,objChildNode)
			Next
						
			Set obj = Nothing
			Set objControl = Nothing
			Set objChildNode = Nothing
		End Function


		Public Function DataBind(ds, wc)
			
			Dim x,mx
			Dim objControl
			
			If ds Is Nothing Then
				Exit Function
			End If

			If wc is Nothing Then
				Set wc=Page.Control
			End If
						
			If wc.DataTextField <> "" Then
				Execute("wc.SetValueFromDataSource(ds(wc.DataField))")
			End If
			
			mx = wc.Controls.Count  - 1
			
			For x=0 To mx
				Set objControl   = wc.Controls.GetControl(x)				
				If objControl.DataTextField <> "" Then
					On Error Resume Next
					Execute(objControl.Name & ".SetValueFromDataSource(ds(objControl.DataTextField))")
					If Err.number >0  Then
						Page.TraceError  objControl, "Bind Error",1
					End If
				End If
				'Call DataBind(ds,objControl)
			Next			
			Set wc = Nothing
			Set objControl = Nothing
		End Function
				

		Private Function PropagateEvent(Wc,EventName)
			
			Dim x,mx
			Dim objControl

			If Wc is Nothing Then
				Set Wc=Page.Control
			End If						
			
			'Page.TraceCall Wc, "_" & EventName
			
			On Error Resume Next			
			Execute("WC.Owner." & EventName & "()")			
			mx = Wc.Controls.Count  - 1			
			
			For x=0 To mx
				Set objControl   = Wc.Controls.GetControl(x)				
				Execute("objControl.Owner." & EventName & "()")
				Call PropagateEvent(objControl,EventName)
			Next
						
			Set Wc = Nothing
			Set objControl = Nothing
		End Function

		Public Sub HandleServerEvent(EventName)
			Dim obj,node			
			
			Page.TraceCall Page.Control, EventName	 & " - Start"
			
			Select Case EventName
				Case "Page_LoadViewState"				
					 ViewState.GetDomObject obj						
					 Set node = GetSection("PAGE",obj.ChildNodes(0))
					 Call Page_ReadViewState(Me.Control,node) 'Set View State Before postback						
				Case "Page_SaveViewState"					
					ViewState.GetDomObject obj
					Set node = GetSection("PAGE",obj.ChildNodes(0))
					Call Page_WriteViewState(Me.Control,node)
					Call obj.ChildNodes(0).AppendChild(Node)
				Case "Page_Load"
					Call PropagateEvent(Me.Control,"OnLoad")
			End Select			

			ExecuteEventFunction EventName			
			
			Select Case EventName
				Case "Page_Init"					
					Call PropagateEvent(Me.Control,"OnInit")
			End Select
			
			Page.TraceCall Page.Control, EventName	 & " - End"			
			Set obj = Nothing
			Set node = Nothing
		End Sub

		Public Function GetPostBackEvent()
				Set GetPostBackEvent = New ClientEvent
				GetPostBackEvent.EventName = Request("__ACTION")
				GetPostBackEvent.Source    = Request("__SOURCE")
				GetPostBackEvent.Instance  = Request("__INSTANCE")			
				GetPostBackEvent.ExtraMessage	= Request("__EXTRAMSG")
		End Function
		
		Public Function HandleClientEvent(e)		
				Dim obj 
				
				Set e = New ClientEvent
				e.EventName = Page.Action
				e.Source    = Page.ActionSource
				e.Instance  = Page.ActionSourceInstance
				e.ExtraMessage	= Page.ActionXMessage			
				
				HandleClientEvent = False
				
				If Control.Name = e.Source Then
					Page.TraceImportantCall Page.Control, "<B>Page Event (" & e.EventName & ")</B>"
					HandleClientEvent = ExecuteEventFunctionEX(e)					
					Exit Function
				End If				
				
				Set obj = Control.FindControl(e.Source)
				
				Set e.TargetObject  = obj
				
				If Not obj Is Nothing Then
					mvarPostBackObjTypeName = TypeName(obj.Owner)				
					Page.TraceImportantCall obj, "Handled Event " & e.EventName & ":" & obj.Owner.HandleClientEvent(e)
					HandleClientEvent = True
				Else
					Page.TraceCall Page.Control,"<span style='color:red;font-weight:bold'>Postback Without Action</span>"	
					Page.TraceImportantCall Page.Control, e.Source & ":" & e.EventName
				End If			
				
		End Function
		
		Public Function ShowMessage(strTitle,strMessage,x,y,w,h)
				Dim strDivMsg
				strDivMsg = "<DIV id='__msg' class='thedummyclass' style='border:groove medium #0033cc;font-size:10pt;position: absolute; left: " & x & ";width:" & w & ";top:" & y & ";height:" & h & ";font-family: verdana;background-color: #ffffcc;' onmouseover='___msg=this'>"				
				strDivMsg = strDivMsg & "<table width='100%'><tr><td style='font-weight:bold;font-family:verdana;color:blue'>" & strTitle & "</td><td align=right><a href='javascript:AssingObjectStyle(___msg,""visibility"",""hidden"")'>close</a></td></tr></table><HR>"
				strDivMsg= strDivMsg & strMessage
				strDivMsg = strDivMsg & "</DIV>"								
				Page.RegisterClientScript "showmessage",strDivMsg
		End Function
	End Class


':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
'::HELPER FUNCTIONS

	Public Function GetSection(SectionName,Node)
		Dim obj
			
		Set obj = Node.SelectSingleNode(SectionName)
		If  obj Is Nothing Then
			Set GetSection = Node.ownerDocument.CreateElement(SectionName)
		Else
			Set GetSection = obj
		End If
	End Function

	Public Function WriteProperty(Name,Value,Node)
		Dim att
		Set att = Node.OwnerDocument.CreateAttribute(Name)
		att.Text = Value
		Node.attributes.setNamedItem att
	End Function

	Public Function ReadProperty(Name,Node)		
		Dim obj
		Set obj =  Node.attributes.GetNamedItem(Name)		
		If obj Is Nothing  Then
			ReadProperty =""
		Else
			ReadProperty = obj.Text
		End If
	End Function
	
	Public Function ExecuteEventFunction(EventName)
		Dim fnc
		ExecuteEventFunction = False
		On Error Resume Next
		Set fnc = GetRef(EventName)		
		If Err.number = 0 Then
			On error Goto 0
			ExecuteEventFunction = True			
			Call fnc()
		Else
			Err.Clear
			On error Goto 0						
		End If
	End Function

	Public Function ExecuteEventFunctionEX(e)
		Dim fnc
		ExecuteEventFunctionEX = False
		On Error Resume Next
		Set fnc = GetRef(e.EventFnc)		
		If Err.number = 0 Then
			On error Goto 0
			ExecuteEventFunctionEX = True			
			Call fnc(e)
		Else
			On error Goto 0						
		End If
	End Function

	Public Function ExecuteEventFunctionArg(EventName,e)
		Dim fnc
		ExecuteEventFunctionArg = False
		On Error Resume Next
		Set fnc = GetRef(EventName)		
		If Err.number = 0 Then
			On error Goto 0
			ExecuteEventFunctionArg = True			
			Call fnc(e)
		Else
			On error Goto 0						
		End If
	End Function

	
	Public Function IIf(expression, truecondition,falsecondition)
		If CBool(expression) Then
			IIf = truecondition
		Else
			IIf = falsecondition
		End If
	End Function
	
%>

<Script language="JavaScript">

	var _validationArray = new Array()
	
	function doSubmit() {		
		alert('should not execute!');
		return false
	}	

/*
	function __DoSubmit() {				
		//if(document.forms[0].processingForm) {
				//alert('hello, no more submits please!')
		//		return false;
		//}
		
		document.forms[0].processingForm = true;
		document.forms[0].originalSubmit();
		document.forms[0].submit = null;
	}
		
	function __OneSubmitOnly() {
		document.forms[0].originalSubmit = document.forms[0].submit;
		document.forms[0].submit = __DoSubmit;		
		document.forms[0].processingForm = false;
		return true;
	}
	function DisableAllLinks() {
		var col = document.getElementsByTagName("A");
		var colmx = col.length;
		for(x = 0;x<2;x++) {
			//alert(col[x].attributes.getNamedItem("href").nodeValue);
			col[x].attributes.getNamedItem("href").nodeValue = "#";													
		}
		return
	}
	*/
	function doPostBack(action,object,instance,xmsg,frmaction,frameName) {
				
		var frm = document.frmForm;
				
		if(_validationArray.length>0)
			if(!validate()) return;
		if(typeof(instance)=='object') {				
				if(instance.type == 'select-one' || instance.type == 'select-multiple')
					frm.__INSTANCE.value   = instance.selectedIndex;	
		}
		else {
			frm.__INSTANCE.value   = instance?instance:'0';	
		}
	
		frm.__ISPOSTBACK.value = "True";
		frm.__ACTION.value     = action;
		frm.__SOURCE.value     = object;
		frm.__EXTRAMSG.value   = xmsg;
		
		//To Restore Scroll Position
		if(!document.layers)
			frm.__SCROLLTOP.value = document.body.scrollTop;
		else
			frm.__SCROLLTOP.value = 0;
		
		//Netscape 4.7 sucks
		if(document.layers && !frmaction) {
			frm.action  = window.location;		
		}
		
		if(frmaction) { 
			frm.action = frmaction;
			frm.__ISREDIRECTEDPOSTBACK.value = 1
		}
		
		//Opens in another frame?
		if(frameName)
			frm.target = frameName;
		frm.submit();	
		return;
	}
	
	
	function __Validation(objectName,validationType,msg,def) {
		this.objName = objectName
		this.obj = eval("document.frmForm." + objectName)
		this.type = validationType
		this.def  = def
		this.msg = msg
		_validationArray.push(this)
	}

	function __ValidateObject(objectName,validationType,msg,def) {
		var dummy =  new __Validation(objectName,validationType,msg,def)
		
	}	
	function validate() {	
		var msg = "";		
		var validationOK = true
		for(x=0;x<_validationArray.length;x++) {
			var obj = _validationArray[x].obj;
			var vobj = _validationArray[x];
			var objtype = ""
			var hasValue = true			
			
			if(!obj.type) {				
					if(obj.length)
						objtype = 'radio'
					else 
						objtype = obj.type
			}else objtype = obj.type
			
			//if obj.value == vobj.def
			//if obj.CausesValidation
								
			switch(objtype) {
				case 'text':
					hasValue =(obj.value!='')
					break;
				case 'checkbox':
					hasValue = (obj.checked)
					break;
				case 'radio':
					hasValue = (__validatOptionList(obj))
					break;
				case 'select-one':
					hasValue = (obj.selectedIndex>0)
					break;
				case 'select-multiple':
					hasValue = (obj.selectedIndex>0)
					break;
				default:
			}		
			if(vobj.type == 1) {
				if(!hasValue) {
					validationOK = false
					msg = msg + vobj.msg + '\n'
				}
			}
									
		}//for
				
		if(!validationOK)
			alert(msg)
		return validationOK		
	}//fnc	
	
	function __validatOptionList(obj) {
		for(i=0;i<obj.length;i++) {
			if(obj[i].checked)
				return true
		}
		return false
	}
	
	function __getValue(o) {
		var x;		
		var obj;
		var value = "";
		
		if(typeof(o) == 'string')
			obj = eval("document.frmForm." + o);
		else
			obj = o;
											
		return obj.value;
		
	}
		
	function AssingObjectStyle(obj,stylename,value) {						
		var r = eval("obj.style." + stylename + "='" + value + "'")
	}

	function __AutoResetFocus(objectName,index) {
		var obj;		
		obj = eval("document.frmForm." + objectName)	
		if(index && !obj.type)
			obj = eval ("document.frmForm." + objectName + "[" + index + "]")
		if(obj!=null ) 		
		  obj.focus()		  		
	}
	
	
</Script>