'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.ComponentModel
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">




Namespace SnippetRepresentation

   ''' <summary>
   ''' represents the CodeSnippets\CodeSnippet\Header element
   ''' </summary>
   ''' <remarks>Specifies general information about the IntelliSense Code Snippet.</remarks>

   Public Class Header
      Implements IDataErrorInfo



      Private _title As String
      Private _author As String
      Private _description As String
      Private _helpUrl As String
      Private _shortcut As String
      Private _keywords As Keywords
      Private _snippettypes As SnippetTypes




      Public Sub New()
         Me.New(Nothing)
      End Sub


      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _keywords = New Keywords
            _snippettypes = New SnippetTypes
         Else
            With el
               _title = .<Title>.Value
               _author = .<Author>.Value
               _description = .<Description>.Value
               _helpUrl = .<HelpUrl>.Value
               _shortcut = .<Shortcut>.Value
               _keywords = New Keywords(.<Keywords>.FirstOrDefault)
               _snippettypes = New SnippetTypes(.<SnippetTypes>.FirstOrDefault)
            End With
         End If
      End Sub



      ''' <remarks>required</remarks>
      Public Property Title() As String
         Get
            Return _title
         End Get
         Set(ByVal value As String)
            _title = value
         End Set
      End Property



      ''' <remarks>optional</remarks>
      Public Property Author() As String
         Get
            Return _author
         End Get
         Set(ByVal value As String)
            _author = value
         End Set
      End Property



      ''' <remarks>optional</remarks>
      Public Property Description() As String
         Get
            Return _description
         End Get
         Set(ByVal value As String)
            _description = value
         End Set
      End Property



      ''' <remarks>optional</remarks>
      Public Property HelpUrl() As String
         Get
            Return _helpUrl
         End Get
         Set(ByVal value As String)
            _helpUrl = value
         End Set
      End Property



      ''' <remarks>optional</remarks>
      Public Property Shortcut() As String
         Get
            Return _shortcut
         End Get
         Set(ByVal value As String)
            _shortcut = value
         End Set
      End Property



      ''' <remarks>optional, currently not used</remarks>
      Public ReadOnly Property Keywords() As Keywords
         Get
            Return _keywords
         End Get
      End Property



      ''' <remarks>optional</remarks>
      Public ReadOnly Property SnippetTypes() As SnippetTypes
         Get
            Return _snippettypes
         End Get
      End Property






      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim hdr As Header = TryCast(obj, Header)
         If hdr Is Nothing Then Return False

         Return (_title = hdr._title) AndAlso _
                (_author = hdr._author) AndAlso _
                (_description = hdr._description) AndAlso _
                (_helpUrl = hdr._helpUrl) AndAlso _
                (_shortcut = hdr._shortcut) AndAlso _
                (_keywords.Equals(hdr._keywords)) AndAlso _
                (_snippettypes.Equals(hdr._snippettypes))

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

      Public ReadOnly Property [Error]() As String Implements System.ComponentModel.IDataErrorInfo.Error
         Get
            Return If(_title = Nothing, "Title required", Nothing)
         End Get
      End Property

      Default Public ReadOnly Property Item(ByVal columnName As String) As String Implements System.ComponentModel.IDataErrorInfo.Item
         Get
            Select Case columnName
               Case "Title"
                  Return If(_title = Nothing, "Title required", Nothing)
               Case Else
                  Return Nothing
            End Select
         End Get
      End Property
   End Class

End Namespace
