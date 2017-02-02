'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports VB = Microsoft.VisualBasic
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">



Namespace SnippetRepresentation



   ''' <summary>
   ''' represents the CodeSnippets\CodeSnippet\Snippet\Declarations\Literal element 
   '''            and CodeSnippets\CodeSnippet\Snippet\Declarations\Objectl element
   ''' </summary>
   ''' <remarks></remarks>
   Public Class Replacement
      Implements IDataErrorInfo



      Private _id As String
      Private _type As String
      Private _tooltip As String
      Private _default As String
      Private _declarationkind As ReplacementKind
      Private _editable As Boolean
      Private _Function As String


      Public Const LiteralTag As String = "Literal"
      Public Const ObjectTag As String = "Object"


      Public Sub New()
         Me.New("")
      End Sub

      Public Sub New(ByVal id As String)
         _id = id
         _editable = True
      End Sub

      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _editable = True
         Else
            _id = el.<ID>.Value
            _type = el.<Type>.Value
            _tooltip = el.<ToolTip>.Value
            _default = el.<Default>.Value
            _editable = If(el.@Editable Is Nothing, True, CBool(el.@Editable))
            _Function = el.<Function>.Value

            Select Case el.Name.LocalName
               Case LiteralTag
                  _declarationkind = SnippetEditor.ReplacementKind.Literal
               Case ObjectTag
                  _declarationkind = SnippetEditor.ReplacementKind.Object
            End Select
         End If
      End Sub




      ''' <summary>
      ''' represents the id element of the Literal
      ''' </summary>
      ''' <remarks>required</remarks>
      Public Property Id() As String
         Get
            Return _id
         End Get
         Set(ByVal value As String)
            _id = value
         End Set
      End Property


      ''' <summary>
      ''' represents the type element of the Literal
      ''' </summary>
      ''' <remarks>required if replacement kind is Object</remarks>
      Public Property Type() As String
         Get
            Return _type
         End Get
         Set(ByVal value As String)
            _type = value
         End Set
      End Property


      ''' <summary>
      ''' represents the tooltip element of the literal
      ''' </summary>
      ''' <remarks>optional</remarks>
      Public Property Tooltip() As String
         Get
            Return _tooltip
         End Get
         Set(ByVal value As String)
            _tooltip = value
         End Set
      End Property


      ''' <summary>
      ''' represents the default value of the literal
      ''' </summary>
      ''' <remarks>optional</remarks>
      Public Property [Default]() As String
         Get
            Return _default
         End Get
         Set(ByVal value As String)
            _default = value
         End Set
      End Property



      ''' <summary>
      ''' Specifies if the replacment is a literal or object
      ''' </summary>
      ''' <remarks>used so as we can represent literals and objects using the same class
      ''' </remarks>
      Public Property ReplacementKind() As ReplacementKind
         Get
            Return _declarationkind
         End Get
         Set(ByVal value As ReplacementKind)
            _declarationkind = value
         End Set
      End Property




      ''' <summary>
      ''' Specifies whether or not you can edit the literal after the code snippet is inserted. The default value of this attribute is true.
      ''' </summary>
      ''' <remarks>optional</remarks>
      Public Property Editable() As Boolean
         Get
            Return _editable
         End Get
         Set(ByVal value As Boolean)
            _editable = value
         End Set
      End Property


      ''' <summary>
      ''' Specifies a function to execute when the literal or object receives focus in Visual Studio.
      ''' </summary>
      ''' <remarks>optional</remarks>
      Public Property [Function]() As String
         Get
            Return _Function
         End Get
         Set(ByVal value As String)
            _Function = value
         End Set
      End Property



#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean
         Dim rep As Replacement = TryCast(obj, Replacement)
         If rep Is Nothing Then Return False

         Return _id = rep._id AndAlso _
           _type = rep._type AndAlso _
           _tooltip = rep._tooltip AndAlso _
           _default = rep._default AndAlso _
           _declarationkind = rep._declarationkind AndAlso _
           _editable = rep._editable AndAlso _
           _Function = rep._Function

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region


#Region "IDataErrorInfo"

      Public ReadOnly Property [Error]() As String Implements System.ComponentModel.IDataErrorInfo.Error
         Get
            Return If(_id = Nothing, " ID is required" & VB.vbCrLf, Nothing) & _
                   If(_declarationkind = SnippetEditor.ReplacementKind.Object AndAlso _type = Nothing, " Type is required for Objects", Nothing)
         End Get
      End Property


      Default Public ReadOnly Property Item(ByVal columnName As String) As String Implements System.ComponentModel.IDataErrorInfo.Item
         Get
            Select Case columnName
               Case "Id"
                  Return If(_id = Nothing, " ID is required", Nothing)
               Case "Type"
                  Return If(_declarationkind = SnippetEditor.ReplacementKind.Object AndAlso _type = Nothing, " Type is required for Objects", Nothing)
               Case Else
                  Return Nothing
            End Select
         End Get
      End Property

#End Region

   End Class

End Namespace

