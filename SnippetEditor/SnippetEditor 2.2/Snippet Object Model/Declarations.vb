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
   ''' This class represents the CodeSnippet\Snippet\Declarations element
   ''' </summary>
   ''' <remarks>contains Object and Literal elements. Rather than have two lists,
   '''  an abstracted type in this object model, "Replacement", is used to represent both Object and Literal elements. 
   ''' </remarks>
	Public Class Declarations

	

      Private _Replacements As List(Of Replacement)

      Public Sub New()
         Me.New(Nothing)
      End Sub


      Public Sub New(ByVal el As XElement)
         If el Is Nothing Then
            _Replacements = New List(Of Replacement)
         Else
            _Replacements = (From item In el.Elements Select New Replacement(item)).ToList
         End If

      End Sub




      Public ReadOnly Property Replacements() As List(Of Replacement)
         Get
            Return _Replacements
         End Get
      End Property


#Region "equality methods"

      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim decs As Declarations = TryCast(obj, Declarations)
         If decs Is Nothing Then Return False

         If (Me._Replacements.Count <> decs._Replacements.Count) Then Return False
         For i As Int32 = 0 To Me._Replacements.Count - 1
            If (Not Me._Replacements.Item(i).Equals(decs._Replacements.Item(i))) Then
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

