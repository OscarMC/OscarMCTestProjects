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
   ''' This class represents the CodeSnippet\Snippet\References element
   ''' </summary>
   ''' <remarks>references are optional</remarks>
	Public Class References

      Private _References As List(Of Reference)

      Public Sub New()
         Me.New(Nothing)
      End Sub

      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _References = New List(Of Reference)
         Else
            _References = (From item In el.<Reference> Select New Reference(item)).ToList
         End If
      End Sub


      Public ReadOnly Property List() As List(Of Reference)
         Get
            Return _References
         End Get
      End Property



#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim ref As References = TryCast(obj, References)
         If ref Is Nothing Then Return False

         If (Me.List.Count <> ref.List.Count) Then Return False
         For i As Int32 = 0 To Me.List.Count - 1
            If (Not Me.List.Item(i).Equals(ref.List.Item(i))) Then
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

