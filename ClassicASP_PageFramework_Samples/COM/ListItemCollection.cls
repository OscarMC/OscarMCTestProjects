VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ListItemCollection"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Dim mobjXMLDocument As MSXML2.FreeThreadedDOMDocument30
Dim mobjItemNode     As IXMLDOMNode
Dim mMode As Integer ' 0 Single Selection, <>0 Multiple Selection


Public Property Get GetSelectedText() As String
    Dim strCritera As String
    Dim objNode As MSXML2.IXMLDOMNode
        
    strCritera = "//IC/I[@S='-1']"
    
    Set objNode = mobjXMLDocument.selectSingleNode(strCritera)
    
    If objNode Is Nothing Then
        GetSelectedText = ""
    Else
        GetSelectedText = objNode.Attributes.getNamedItem("T").nodeValue 'objNode.Text
    End If

End Property


Public Property Get GetSelectedValue() As String
    Dim strCritera As String
    Dim objNode As MSXML2.IXMLDOMNode
        
    strCritera = "//IC/I[@S='-1']"
    
    Set objNode = mobjXMLDocument.selectSingleNode(strCritera)
    
    If objNode Is Nothing Then
        GetSelectedValue = ""
    Else
        GetSelectedValue = objNode.Attributes.getNamedItem("V").nodeValue
    End If

End Property



Public Property Let Mode(ByVal value)
    mMode = value
End Property

Public Property Get Mode()
    Mode = mMode
End Property




Private Sub Class_Initialize()
    'Initializes the ItemCollection
    
    Set mobjXMLDocument = New MSXML2.FreeThreadedDOMDocument30
    
    mobjXMLDocument.loadXML "<IC><I V="""" S="""" T=""""/></IC>"
   
    Set mobjItemNode = mobjXMLDocument.firstChild.childNodes(0).cloneNode(False)
    
    mobjXMLDocument.firstChild.removeChild mobjXMLDocument.firstChild.childNodes(0)
    
    mMode = 0 'Single Select
    
End Sub


Public Sub SetState(ByRef strCollectionXML As Variant)
        mobjXMLDocument.loadXML strCollectionXML
End Sub
Public Function GetState()
    GetState = mobjXMLDocument.xml
End Function

Public Sub Clear()
    Set mobjXMLDocument = New MSXML2.FreeThreadedDOMDocument30
        
    mobjXMLDocument.loadXML "<IC><I V="""" S="""" T=""""/></IC>"
   
    Set mobjItemNode = mobjXMLDocument.firstChild.childNodes(0).cloneNode(False)
    
    mobjXMLDocument.firstChild.removeChild mobjXMLDocument.firstChild.childNodes(0)
        
End Sub

Public Sub Add(ByVal Text As String, ByVal value As Variant, ByVal selected As Boolean, Optional ByVal index As Integer = -1)
    Dim xmlNode As IXMLDOMNode
    Dim xmlRef As IXMLDOMNode
    
    If index >= 0 Then
        If mobjXMLDocument.firstChild.childNodes.length < index Then
            index = mobjXMLDocument.firstChild.childNodes.length
        End If
        
        Set xmlRef = mobjXMLDocument.firstChild.childNodes(index)
    Else
        Set xmlRef = Nothing
    End If
    
    If mMode = 0 And selected = True Then
        Me.SetAllSelected False
    End If
    
    'With mobjXMLDocument.firstChild.appendChild(mobjItemNode.cloneNode(False))
    With mobjXMLDocument.firstChild.insertBefore(mobjItemNode.cloneNode(False), xmlRef)
        '.Text = Text
        .Attributes.getNamedItem("T").nodeValue = Text
        .Attributes.getNamedItem("V").nodeValue = value
        .Attributes.getNamedItem("S").nodeValue = selected
    End With
End Sub

Public Property Get Count() As Integer
    Count = mobjXMLDocument.firstChild.childNodes.length
End Property
Public Sub Remove(ByVal index As Integer)
    mobjXMLDocument.firstChild.removeChild mobjXMLDocument.firstChild.childNodes(index)
End Sub
Public Function GetValue(ByVal index As Integer) As Variant
    GetValue = mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("V").nodeValue
End Function
Public Function GetText(ByVal index As Integer) As Variant
    GetText = mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("T").nodeValue
    'GetText = mobjXMLDocument.firstChild.childNodes(index).Text
End Function
Public Function IsSelected(ByVal index As Integer) As Boolean
    IsSelected = mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("S").nodeValue
End Function

Public Sub GetItemData(ByVal index As Integer, ByRef Text As Variant, ByRef value As Variant, ByRef selected As Variant)
    Dim objNode As MSXML2.IXMLDOMNode
    Set objNode = mobjXMLDocument.firstChild.childNodes(index)
    selected = objNode.Attributes.getNamedItem("S").nodeValue
    'Text = objNode.Text
    Text = objNode.Attributes.getNamedItem("T").nodeValue
    value = objNode.Attributes.getNamedItem("V").nodeValue
End Sub

Public Function IsSelectedByText(ByVal Text As String) As Boolean
    Dim objNode As MSXML2.IXMLDOMNode
    
    Set objNode = GetItemByText(Text)
    
    If Not objNode Is Nothing Then
        IsSelectedByText = objNode.Attributes.getNamedItem("S").nodeValue
    End If
    
End Function
Public Function IsSelectedByValue(ByVal value As String) As Boolean
    Dim objNode As MSXML2.IXMLDOMNode
    
    Set objNode = GetItemByValue(value)
    
    If Not objNode Is Nothing Then
        IsSelectedByValue = objNode.Attributes.getNamedItem("S").nodeValue
    End If

End Function


Public Function SetValue(ByVal index As Integer, ByVal value As Variant)
        mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("V").nodeValue = value
End Function
Public Function SetText(ByVal index As Integer, ByVal value As Variant)
    'mobjXMLDocument.firstChild.childNodes(index).Text = value
    mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("T").nodeValue = value
End Function
Public Sub SetSelected(ByVal index As Integer, ByVal bolSelected As Boolean)
    
    If mMode = 0 And bolSelected = True Then
        Me.SetAllSelected False
    End If
        
    mobjXMLDocument.firstChild.childNodes(index).Attributes.getNamedItem("S").nodeValue = bolSelected
End Sub
Public Sub SetSelectedByText(ByVal Text As Variant, ByVal newvalue As Boolean)
    
    Dim objNode As MSXML2.IXMLDOMNode
            
    Set objNode = GetItemByText(Text)
    
    If mMode = 0 Then
        Me.SetAllSelected False
    End If
    
    If Not objNode Is Nothing Then
        objNode.Attributes.getNamedItem("S").nodeValue = newvalue
    End If
End Sub
Public Sub SetSelectedByValue(ByVal value As Variant, ByVal newvalue As Boolean)
    Dim objNode As MSXML2.IXMLDOMNode
    
    Set objNode = GetItemByValue(value)
    
    If mMode = 0 Then
        Me.SetAllSelected False
    End If
    
    If Not objNode Is Nothing Then
        objNode.Attributes.getNamedItem("S").nodeValue = newvalue
    End If

End Sub

Private Function GetItemByValue(ByRef value As Variant) As MSXML2.IXMLDOMNode
    Dim strCritera As String
    Dim objNode As MSXML2.IXMLDOMNode
        
    strCritera = "//IC/I[@V='" & value & "']"
    
    Set GetItemByValue = mobjXMLDocument.selectSingleNode(strCritera)
        
End Function

Private Function GetItemByText(ByRef Text As Variant) As MSXML2.IXMLDOMNode
    Dim strCritera As String
    Dim objNode As MSXML2.IXMLDOMNode
        
    strCritera = "//IC/I[@T='" & Text & "']"
    
    Set GetItemByText = mobjXMLDocument.selectSingleNode(strCritera)
        
End Function

Public Sub SetAllSelected(ByVal bolSelected As Boolean)
    
    Dim node As MSXML2.IXMLDOMNode

    For Each node In mobjXMLDocument.firstChild.childNodes
        node.Attributes.getNamedItem("S").nodeValue = bolSelected
    Next

End Sub

