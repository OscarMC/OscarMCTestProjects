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
   ''' represents the CodeSnippet\Snippet\Imports\Import element
   ''' </summary>
   ''' <remarks></remarks>
	Public Class Import
	

      Private _namespace As String


      Public Sub New()
      End Sub

      Public Sub New(ByVal namespacevalue As String)
         _namespace = namespacevalue
      End Sub

      Public Sub New(ByVal el As XElement)
         If el IsNot Nothing Then
            _namespace = el.<Namespace>.Value
         End If
      End Sub


      ''' <remarks>required</remarks>
      Public Property [Namespace]() As String
         Get
            Return _namespace
         End Get
         Set(ByVal value As String)
            _namespace = value
         End Set
      End Property



#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean
         Dim other As Import = TryCast(obj, Import)
         If other Is Nothing Then Return False
         Return _namespace = other._namespace
      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region


   End Class

End Namespace


