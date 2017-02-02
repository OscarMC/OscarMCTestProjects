'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Collections.Generic



Namespace CodeColorisers

   ''' <summary>
   ''' Interface that all code colorisers need to implement.
   ''' Provides the basis of providing code colorign information to the application via the
   ''' IColorTokenProvider.GetColorTokens method
   ''' </summary>
   ''' <remarks></remarks>
   Public Interface IColorTokenProvider

      Function GetColorTokens(ByVal sourceCode As String, Optional ByVal offset As Int32 = 0) As IList(Of ColorToken)

   End Interface



End Namespace
