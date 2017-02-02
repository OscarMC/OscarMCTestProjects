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
   ''' represents the CodeSnippet\References\Reference element
   ''' </summary>
   ''' <remarks>Although the Assembly property is required, 
   '''  don't enforce this as Reference items with an empty Assembly element are stripped out when saved.
   ''' </remarks>
	Public Class Reference

      Private _assembly As String
      Private _url As String


      Public Sub New()
         'Me.New(Nothing)
      End Sub

      Public Sub New(ByVal el As XElement)
         If el IsNot Nothing Then
            _assembly = el.<Assembly>.Value
            _url = el.<Url>.Value
         End If
      End Sub


      Public Sub New(ByVal assembly As String, ByVal url As String)
         _assembly = assembly
         _url = url
      End Sub


      ''' <remarks>required</remarks>
      Public Property Assembly() As String
         Get
            Return _assembly
         End Get
         Set(ByVal value As String)
            _assembly = value
         End Set
      End Property



      ''' <remarks>optional</remarks>
      Public Property Url() As String
         Get
            Return _url
         End Get
         Set(ByVal value As String)
            _url = value
         End Set
      End Property



#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim ref As Reference = TryCast(obj, Reference)
         If ref Is Nothing Then Return False

         Return _assembly = ref._assembly AndAlso _
                _url = ref._url

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region

   End Class

End Namespace

