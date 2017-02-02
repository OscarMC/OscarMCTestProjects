'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">




Namespace SnippetRepresentation
   ''' <summary>
   ''' represents the CodeSnippets\CodeSnippet element. 
   ''' </summary>
   ''' <remarks>Each .snippet file can contain multiple CodeSnippet's. 
   '''</remarks>
	Public Class CodeSnippet



      Private _format As String
      Private _header As Header
      Private _snippet As Snippet


      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _format = "1.0.0"
            _header = New Header
            _snippet = New Snippet
         Else
            _format = el.@Format
            _header = New Header(el.<Header>.FirstOrDefault)
            _snippet = New Snippet(el.<Snippet>.FirstOrDefault)
         End If
      End Sub

      Public Sub New()
         Me.New(Nothing)
      End Sub


      ''' <summary>
      ''' format attribute of the CodeSnippet
      ''' </summary>
      ''' <value></value>
      ''' <remarks>required. 
      ''' Specifies the schema version of the code snippet. 
      ''' The Format attribute must be a string in the syntax of x.x.x, where 
      ''' each "x" represents a numerical value of the version number. Visual Studio 
      ''' will ignore code snippets with Format attributes that it does not understand.
      ''' </remarks>
      Public Property Format() As String
         Get
            Return _format
         End Get
         Set(ByVal value As String)
            _format = value
         End Set
      End Property


      ''' <summary>
      ''' the Header element of the CodeSnippet
      ''' </summary>
      ''' <value></value>
      ''' <remarks>required. 
      ''' Specifies general information about the IntelliSense Code Snippet.
      ''' </remarks>
      Public ReadOnly Property Header() As Header
         Get
            Return _header
         End Get
      End Property


      ''' <summary>
      ''' the Snippet element of the CodeSnippet
      ''' </summary>
      ''' <value></value>
      ''' <remarks>required. 
      ''' Specifies the references, imports, declarations, and code for the code snippet.
      ''' </remarks>
      Public ReadOnly Property Snippet() As Snippet
         Get
            Return _snippet
         End Get
      End Property


     
#Region "equality methods"

      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim cs As CodeSnippet = TryCast(obj, CodeSnippet)
         If cs Is Nothing Then Return False

         Return _format = cs._format AndAlso _
           _header.Equals(cs._header) AndAlso _
           _snippet.Equals(cs._snippet)

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region

   End Class
End Namespace


