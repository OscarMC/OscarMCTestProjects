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
   ''' represents the CodeSnippets\CodeSnippet\Header\Keywords element
   ''' </summary>
   ''' <remarks>Groups individual Keyword elements. The code snippet keywords are used by Visual Studio
   '''  and represent a standard way for online content providers to add custom keywords.</remarks>
	Public Class Keywords


      Private _Keywords As List(Of String)

      Public Sub New()
         Me.New(Nothing)
      End Sub

      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _Keywords = New List(Of String)
         Else
            _Keywords = (From item In el.<Keyword> Select item.Value).ToList
         End If
      End Sub


      Public ReadOnly Property List() As List(Of String)
         Get
            Return _Keywords
         End Get
      End Property


#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim keys As Keywords = TryCast(obj, Keywords)
         If keys Is Nothing Then Return False

         If (Me.List.Count <> keys.List.Count) Then Return False
         For i As Int32 = 0 To Me.List.Count - 1
            If Not Me.List.Item(i) = keys.List.Item(i) Then
               Return False
            End If
         Next

         Return True

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region

   End Class
End Namespace

