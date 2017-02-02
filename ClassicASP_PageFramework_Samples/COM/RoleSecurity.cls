VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RoleSecurity"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Dim msxml

Private Sub Init()

    
    
    
    
    If Not msxml.Load("C:\TEST.XML") Then
        Stop

    End If


End Sub

Private Sub Class_Initialize()
    Set msxml = New MSXML2.FreeThreadedDOMDocument30
    
    
    
    msxml.setProperty "SelectionLanguage", "XPath"
    Call Init
    
End Sub


Public Function CanAccess(ByVal ScriptName, ByVal UserName, ByVal Roles) As Boolean
    
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

Public Function CanAccessPage(ByVal FolderPath, ByVal PageName, ByVal UserName, ByVal RolesFilter) As Boolean
    Dim node As IXMLDOMNode
    Dim nodes As IXMLDOMNodeList
    Dim attRole As IXMLDOMNode
    Dim attUser As IXMLDOMNode
    Dim XpathQuery
    
    CanAccessPage = CanAccessFolder(FolderPath, UserName, RolesFilter)
    If Not CanAccessPage Then
        Exit Function
    End If
    
    XpathQuery = "//FOLDER[@path='{f}']/PAGE[@name='{p}']/allow[{u} @users='*' or @users='?' " & RolesFilter & "] | " & _
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
Public Function CanAccessFolder(ByVal FolderPath, ByVal UserName, ByVal RolesFilter) As Boolean
    Dim node As IXMLDOMNode
    Dim nodes As IXMLDOMNodeList
    Dim attRole As IXMLDOMNode
    Dim attUser As IXMLDOMNode
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

