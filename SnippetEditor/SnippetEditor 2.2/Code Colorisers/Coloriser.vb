'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System


Namespace CodeColorisers

   ''' <summary>
   ''' Provides Shared methods that provide the default code colorisers for different languages
   ''' </summary>
   ''' <remarks>
   ''' Theoretically this should be plug-in extensible, however there is no practical need
   '''  If needed, the defaults could be  determined by via a config file entry, where the language
   '''  name is a lookup in the config file, and the type in that entry implements IColorTokenProvider.
   '''  Should other languages get with the snippet program, and/or folks want to provide their own
   '''  code coloriser for a given language, then we should look at doing this
   ''' </remarks>

   Public NotInheritable Class Coloriser

      Private Sub New()
         ' all members of the class are shared
      End Sub

#Region "default colorisers"


      Private Shared m_VBColoriser As VBCodeColoriser

      Public Shared ReadOnly Property VBColoriser() As IColorTokenProvider
         Get
            If m_VBColoriser Is Nothing Then
               m_VBColoriser = New VBCodeColoriser
            End If
            Return m_VBColoriser
         End Get
      End Property



      Private Shared m_CSharpColoriser As CSharpCodeColoriser

      Public Shared ReadOnly Property CSharpColoriser() As IColorTokenProvider
         Get
            If m_CSharpColoriser Is Nothing Then
               m_CSharpColoriser = New CSharpCodeColoriser
            End If
            Return m_CSharpColoriser
         End Get
      End Property



      Private Shared m_JSharpColoriser As JSharpCodeColoriser

      Public Shared ReadOnly Property JSharpColoriser() As IColorTokenProvider
         Get
            If m_JSharpColoriser Is Nothing Then
               m_JSharpColoriser = New JSharpCodeColoriser
            End If
            Return m_JSharpColoriser
         End Get
      End Property





      Private Shared m_XmlColoriser As XMLCodeColoriser

      Public Shared ReadOnly Property XmlColoriser() As IColorTokenProvider
         Get
            If m_XmlColoriser Is Nothing Then
               m_XmlColoriser = New XmlCodeColoriser
            End If
            Return m_XmlColoriser
         End Get
      End Property




      Private Shared m_UnknownColoriser As UnknownCodeColoriser

      Public Shared ReadOnly Property UnknownColoriser() As IColorTokenProvider
         Get
            If m_UnknownColoriser Is Nothing Then
               m_UnknownColoriser = New UnknownCodeColoriser
            End If
            Return m_UnknownColoriser
         End Get
      End Property

#End Region



      Public Shared Function GetColoriser(ByVal language As String) As IColorTokenProvider

			Select Case language.ToLower
				Case "vb", "visual basic"
					Return VBColoriser

				Case "csharp"
					Return CSharpColoriser

				Case "jsharp"
					Return JSharpColoriser

				Case "xml"
					Return XmlColoriser

				Case Else
					Return UnknownColoriser

			End Select

      End Function




      Public Shared Function GetColoriser(ByVal language As CodeLanguage) As IColorTokenProvider

         Select Case language
            Case CodeLanguage.VB
               Return VBColoriser

            Case CodeLanguage.CSharp
               Return CSharpColoriser

            Case CodeLanguage.JSharp
               Return JSharpColoriser

            Case CodeLanguage.XML
               Return XmlColoriser

            Case CodeLanguage.Unknown
               Return UnknownColoriser

            Case Else
               Return UnknownColoriser

         End Select

      End Function



   End Class



End Namespace


