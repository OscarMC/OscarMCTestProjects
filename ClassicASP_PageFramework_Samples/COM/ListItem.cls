VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ListItem"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Private mvarText As String
Private mvarValue As Variant
Private mbolSelected As Variant


Public Sub LoadData(ByVal Text As Variant, ByVal value As Variant, ByVal IsSelected As Boolean)
    mvarText = Text
    mvarValue = value
    mbolSelected = IsSelected
End Sub

Public Property Get value()
        value = mvarValue
End Property

Public Property Let value(ByVal value As Variant)
        mvarValue = value
End Property

Public Property Get Text()
    Text = mvarText
End Property

Public Property Let Text(ByVal value As Variant)
    mvarText = value
End Property


Public Property Get Selected()
    Selected = mbolSelected
End Property

Public Property Let Selected(ByVal value As Boolean)
    mbolSelected = value
End Property


