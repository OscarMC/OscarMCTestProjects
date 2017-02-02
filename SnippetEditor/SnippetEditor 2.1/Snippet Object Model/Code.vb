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
Imports System.ComponentModel



Namespace SnippetRepresentation

   ''' <summary>
   ''' This class represents the CodeSnippets\CodeSnippet\Snippet\Code element
   ''' </summary>
   ''' <remarks></remarks>
   Public Class Code
      Implements IDataErrorInfo



      Private _codeData As String = ""
      Private _language As String = "VB"
      Private _delimiter As String = "$"
      Private _kind As String = ""


      Public Sub New()
         Me.New(Nothing)
      End Sub

      Public Sub New(ByVal el As XElement)
         If el IsNot Nothing Then
            Data = el.Value
            Language = el.@Language
            Kind = el.@Kind
            _delimiter = If(el.@Delimiter, "$")
         End If
      End Sub


      ''' <summary>
      ''' the snippet's code 
      ''' </summary>
      ''' <remarks>required</remarks>
      Public Property Data() As String
         Get
            Return _codeData
         End Get
         Set(ByVal value As String)
            _codeData = value
         End Set
      End Property


      ''' <summary>
      ''' code language: should be one of the following : 'VB', 'CSharp', 'XML' or 'JSharp'
      ''' </summary>
      ''' <remarks>required</remarks>
      Public Property Language() As String
         Get
            Return _language
         End Get
         Set(ByVal value As String)
            If value Is Nothing Then value = "VB"
            _language = value
         End Set
      End Property


      ''' <summary>
      ''' delimiter used for literals and objects within the code
      ''' </summary>
      ''' <remarks>optional. defaults to $</remarks>
      Public Property Delimiter() As String
         Get
            Return _delimiter
         End Get
         Set(ByVal value As String)
            If value = Nothing Then value = "$"
            _delimiter = value
         End Set
      End Property


      ''' <summary>
      '''  scope of the snippet: type declaration, method declaration, method body, page (for asp.net) or comment.
      ''' </summary>
      ''' <remarks>optional</remarks>
      Public Property Kind() As String
         Get
            Return _kind
         End Get
         Set(ByVal value As String)
            If value Is Nothing Then value = ""
            _kind = value
         End Set
      End Property



#Region "equality methods"


      Public Overrides Function Equals(ByVal obj As Object) As Boolean

         Dim rep As Code = TryCast(obj, Code)
         If rep Is Nothing Then Return False

         Return String.Equals(_language, rep._language, StringComparison.OrdinalIgnoreCase) AndAlso _
                _delimiter = rep._delimiter AndAlso _
                _kind = rep._kind AndAlso _
                _codeData = rep._codeData

      End Function


      Public Overrides Function GetHashCode() As Integer
         Return MyBase.GetHashCode()
      End Function

#End Region


#Region "IDataErrorInfo"

      Public ReadOnly Property [Error]() As String Implements System.ComponentModel.IDataErrorInfo.Error
         Get
            If Language = Nothing Then
               Return "you need to specify a language"
            Else
               Return Nothing
            End If
         End Get
      End Property

      Default Public ReadOnly Property Item(ByVal columnName As String) As String Implements System.ComponentModel.IDataErrorInfo.Item
         Get
            Select Case columnName
               Case "Language"
                  If Language = Nothing Then Return "you need to specify a language"
               Case Else
                  Return Nothing
            End Select
            Return Nothing
         End Get
      End Property

#End Region

   End Class

End Namespace

