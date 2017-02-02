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
   ''' Defualt code coloriser for unknown languages
   ''' </summary>
   ''' <remarks>
   ''' known languages can derive from this although it is preferable they implement IColorTokenProvider directly.
   ''' </remarks>
	Friend Class UnknownCodeColoriser
		Implements IColorTokenProvider


      Public Overridable Function GetColorTokens(ByVal sourceCode As String, Optional ByVal offset As Integer = 0) As IList(Of ColorToken) Implements IColorTokenProvider.GetColorTokens
         ' return an empty list to make it easy for callee enumeration code 
         Return New List(Of ColorToken)
      End Function
	End Class



End Namespace
