'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">



Namespace SnippetRepresentation
   ''' <summary>
   ''' This class represents the CodeSnippet\Snippet element
   ''' </summary>
   ''' <remarks></remarks>
	Public Class Snippet

      Private _references As References
      Private _imports As [Imports]
      Private _declarations As Declarations
      Private _code As Code


      Public Sub New()
         Me.New(Nothing)
      End Sub

      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _references = New References
            _imports = New [Imports]
            _declarations = New Declarations
            _code = New Code
         Else
            _references = New References(el.<References>.FirstOrDefault)
            _imports = New [Imports](el.<Imports>.FirstOrDefault)
            _declarations = New Declarations(el.<Declarations>.FirstOrDefault)
            _code = New Code(el.<Code>.FirstOrDefault)
         End If
      End Sub



      ''' <remarks>optional</remarks>
      Public ReadOnly Property References() As References
         Get
            Return _references
         End Get
      End Property



      ''' <remarks>optional</remarks>
      Public ReadOnly Property [Imports]() As [Imports]
         Get
            Return _imports
         End Get
      End Property


	
      ''' <remarks>optional</remarks>
		Public ReadOnly Property Declarations() As Declarations
			Get
            Return _declarations
			End Get
		End Property



		''' <remarks></remarks>
		Public ReadOnly Property Code() As Code
			Get
            Return _code
			End Get
		End Property


	

#Region "equality methods"

      Public Overrides Function Equals(ByVal obj As Object) As Boolean
         Dim snip As Snippet = TryCast(obj, Snippet)
         If snip Is Nothing Then Return False

         Return _references.Equals(snip._references) AndAlso _
                _imports.Equals(snip._imports) AndAlso _
                _declarations.Equals(snip._declarations) AndAlso _
                _code.Equals(snip._code)

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region

   End Class

End Namespace

