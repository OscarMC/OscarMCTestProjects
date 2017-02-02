VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ViewState"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Option Compare Text

Dim mobjXMLDocument As MSXML2.FreeThreadedDOMDocument30
Dim mobjNode    As IXMLDOMNode


Public Sub GetDomObject(ByRef obj)
    
    Set obj = mobjXMLDocument
    
End Sub


Private Sub Class_Initialize()
    'Initializes the ItemCollection
    Call InitXML
End Sub

Private Sub InitXML()
    
    Set mobjXMLDocument = New MSXML2.FreeThreadedDOMDocument30
    mobjXMLDocument.loadXML "<VS><V N=""""/></VS>"
    Set mobjNode = mobjXMLDocument.firstChild.childNodes(0).cloneNode(False)
    
    mobjXMLDocument.firstChild.removeChild mobjXMLDocument.firstChild.childNodes(0)

End Sub
Public Sub Clear()
    Call InitXML
End Sub
Public Sub LoadViewState(ByRef strViewStateXML As Variant)
        mobjXMLDocument.loadXML strViewStateXML
End Sub
Public Sub LoadViewStateBase64(ByRef strViewStateXMLBase64 As Variant, Optional ByVal bolIsCompressed As Boolean = False)
            
        Dim objCrypto As ASPFramework.Crypto
        Dim strXML As String
        
        strXML = strViewStateXMLBase64
        
        If Len(strXML) = 0 Then
            Call InitXML
        Else
            Set objCrypto = New ASPFramework.Crypto
            strXML = objCrypto.DecodeStr64(strXML)
            
            If bolIsCompressed Then
                strXML = objCrypto.UncompressString(strXML)
            End If
            
            If Not mobjXMLDocument.loadXML(strXML) Then
                Call InitXML
            End If
            Set objCrypto = Nothing
        End If
        
        strViewStateXMLBase64 = strXML

End Sub
Public Function GetViewState()
        GetViewState = mobjXMLDocument.xml
End Function
Public Function GetViewStateBase64(Optional ByVal lngCompressIfLargerThan As Long = -1, Optional ByRef bolIsCompressed As Variant = False)
        Dim strXML As String
        Dim objCrypto As New ASPFramework.Crypto
        
        strXML = mobjXMLDocument.xml
                
        If lngCompressIfLargerThan > 0 Then
            If Len(strXML) > lngCompressIfLargerThan Then
                bolIsCompressed = True
                strXML = objCrypto.CompressString(strXML)
            End If
        End If
        
        GetViewStateBase64 = objCrypto.EncodeStr64(strXML)
        
        Set objCrypto = Nothing

End Function
Public Sub Add(ByVal Name As String, ByVal value As Variant)
    Dim xmlNode As IXMLDOMNode
    
    Set xmlNode = GetNodeByName(Name)
    
    If xmlNode Is Nothing Then
        Set xmlNode = mobjXMLDocument.firstChild.appendChild(mobjNode.cloneNode(False))
    End If
    
    With xmlNode
        .Text = value
        .Attributes.getNamedItem("N").nodeValue = Name
    End With
    
End Sub
Public Property Get Count() As Integer
    Count = mobjXMLDocument.firstChild.childNodes.length
End Property


Public Sub Remove(ByVal index As Integer)
    Call mobjXMLDocument.firstChild.removeChild(mobjXMLDocument.firstChild.childNodes(index))
End Sub

Public Sub RemoveByName(ByVal Name As String)
    Dim objNode As MSXML2.IXMLDOMNode
    Set objNode = GetNodeByName(Name)
    If Not objNode Is Nothing Then
        Call mobjXMLDocument.firstChild.removeChild(objNode)
    End If
End Sub
Public Function GetValueByIndex(ByVal index As Integer) As Variant
    GetValueByIndex = mobjXMLDocument.firstChild.childNodes(index).Text
End Function

Public Sub SetalueByIndex(ByVal index As Integer, ByVal value As Variant)
    mobjXMLDocument.firstChild.childNodes(index).Text = value
End Sub
Public Function GetValue(ByVal Name As String) As Variant
    Dim objNode As MSXML2.IXMLDOMNode
            
    Set objNode = GetNodeByName(Name)
    If Not objNode Is Nothing Then
        GetValue = objNode.Text
    End If
End Function

Public Sub SetValue(ByVal Name As String, ByVal value As Variant)
    Dim objNode As MSXML2.IXMLDOMNode
    
    Set objNode = GetNodeByName(Name)
    If Not objNode Is Nothing Then
         objNode.Text = value
    End If
        
End Sub

Private Function GetNodeByName(ByVal Name As String) As MSXML2.IXMLDOMNode
    Dim strCritera As String
    Dim objNode As MSXML2.IXMLDOMNode
    
    strCritera = "//VS/V[@N = '" & Name & "']"
    Set GetNodeByName = mobjXMLDocument.selectSingleNode(strCritera)
    
        

End Function

Public Function HasKey(ByVal Name As String) As Boolean
        HasKey = (Not GetNodeByName(Name) Is Nothing)
End Function


'Public Property Get NewEnum() As IUnknown
    'Set NewEnum = mCol.[_NewEnum]
'End Property

