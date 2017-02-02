'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Drawing




Imports Microsoft.VisualBasic
Namespace CodeColorisers

   ''' <summary>
   ''' Structure that contains color formatting information for use by code colorisers
   ''' Required by the IColorTokenProvider interface's GetColortokens method. 
   ''' </summary>
   ''' <remarks></remarks>

   Public Structure ColorToken

      Public Start As Int32
      Public Length As Int32
      Public ForeColor As Color
      Public BackColor As Color

      Sub New(ByVal start As Int32, ByVal length As Int32, ByVal foreColor As Color, ByVal backColor As Color)
         Me.Start = start
         Me.Length = length
         Me.ForeColor = foreColor
         Me.BackColor = backColor
      End Sub

      Public Overrides Function Equals(ByVal obj As Object) As Boolean
         Return Me.Equals(CType(obj, ColorToken))
      End Function

      Public Overloads Function Equals(ByVal token As ColorToken) As Boolean
         Return (Me.Start = token.Start) AndAlso _
                (Me.Length = token.Length) AndAlso _
                (Me.ForeColor = token.ForeColor) AndAlso _
                (Me.BackColor = token.BackColor)
      End Function

      Public Shared Operator <>(ByVal left As ColorToken, ByVal right As ColorToken) As Boolean
         Return Not left.Equals(right)
      End Operator

      Public Shared Operator =(ByVal left As ColorToken, ByVal right As ColorToken) As Boolean
         Return left.Equals(right)
      End Operator

      Public Overrides Function GetHashCode() As Integer
         Return Me.Start Xor Me.Length Xor Me.ForeColor.GetHashCode Xor Me.BackColor.GetHashCode
      End Function

   End Structure


End Namespace



