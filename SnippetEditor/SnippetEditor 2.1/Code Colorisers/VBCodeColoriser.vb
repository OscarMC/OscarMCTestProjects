'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions



Namespace CodeColorisers


   ''' <summary>
   ''' Code coloriser for the best language, VB.NET !!!
   ''' </summary>
   ''' <remarks>
   ''' uses simple regex.
   '''  TODO: colors for comments, strings and keywords are hard coded. It might be nice to put that into a config file
   '''  TODO: this needs to be updated to include LINQ.
   ''' </remarks>
	Friend Class VBCodeColoriser
		Implements IColorTokenProvider



      Private keywords As String = _
          "(?:\u0023)Region|(?:\u0023)End\s+Region|" & _
           "(?:\u0023)If|(?:\u0023)Else|(?:\u0023)ElseIf|(?:\u0023)End|(?:\u0023)Const|" & _
         "Custom\s+Event|" & _
          "Option\s+Strict\s+Off|Option\s+Explicit\s+Off|Option\s+Strict|Option\s+Explicit|Option\s+Compare\s+Binary|Option\s+Compare\s+Text|" & _
          "AddHandler|AddressOf|Alias|And|" & _
          "AndAlso|As|Boolean|ByRef|" & _
          "Byte|ByVal|Call|Case|" & _
          "Catch|CBool|CByte|CChar|" & _
          "CDate|CDbl|CDec|Char|" & _
          "CInt|Class|CLng|CObj|" & _
          "Const|Continue|CSByte|CShort|" & _
          "CSng|CStr|CType|CUInt|" & _
          "CULng|CUShort|Date|Decimal|" & _
          "Declare|Default|Delegate|Dim|" & _
          "DirectCast|Do|Double|Each|" & _
          "Else|ElseIf|End|EndIf|" & _
          "Enum|Erase|Error|Event|" & _
          "Exit|False|Finally|For|" & _
          "Friend|From|Function|Get|GetType|" & _
          "Global|GoSub|GoTo|Handles|" & _
          "If|Implements|Imports|In|" & _
          "Inherits|Integer|Interface|Is|" & _
          "IsNot|Let|Lib|Like|" & _
          "Long|Loop|Me|Mod|" & _
          "Module|MustInherit|MustOverride|MyBase|" & _
          "MyClass|Namespace|Narrowing|New|" & _
          "Next|Not|Nothing|NotInheritable|" & _
          "NotOverridable|Object|Of|On|" & _
          "Operator|Option|Optional|Or|" & _
          "OrElse|Overloads|Overridable|Overrides|" & _
          "ParamArray|Partial|Private|Property|" & _
          "Protected|Public|RaiseEvent|ReadOnly|" & _
          "ReDim|REM|RemoveHandler|Resume|" & _
          "Return|SByte|Select|Set|" & _
          "Shadows|Shared|Short|Single|" & _
          "Static|Step|Stop|String|" & _
          "Structure|Sub|SyncLock|Then|" & _
          "Throw|To|True|Try|" & _
          "TryCast|TypeOf|UInteger|ULong|" & _
          "UShort|Using|Variant|Wend|" & _
          "When|Where|While|Widening|With|" & _
          "WithEvents|WriteOnly|Xor"






      Public Function GetColorTokens(ByVal sourceCode As String, Optional ByVal offset As Integer = 0) As IList(Of ColorToken) Implements IColorTokenProvider.GetColorTokens

         Dim tokens As New List(Of ColorToken)

         ' early exit 
         If sourceCode Is Nothing OrElse sourceCode.Length = 0 Then Return tokens


         Dim endPos As Int32 = offset + sourceCode.Length
         Dim keywordRegEx As New Regex("(?<=\s|{|}|\(|\)|,|\=|^)(" & keywords & ")\b", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
         Dim quoteRegEx As New Regex("("".*?"")", RegexOptions.CultureInvariant)


         If (sourceCode.IndexOf("""") > -1) Then

            Dim subStrings() As String = quoteRegEx.Split(sourceCode)

            For Each subString As String In subStrings

               If subString.StartsWith("""") Then
                  tokens.Add(New ColorToken(offset, subString.Length, Color.DarkRed, Color.Empty))

               Else

                  For Each mtch As Match In keywordRegEx.Matches(subString)
                     tokens.Add(New ColorToken(offset + mtch.Index, mtch.Length, Color.Blue, Color.Empty))
                  Next

                  Dim idx As Int32 = subString.IndexOf("'"c)

                  If idx > -1 Then
                     tokens.Add(New ColorToken(offset + idx, endPos - offset - idx, Color.DarkGreen, Color.Empty))

                     Return tokens ' early exit
                  End If

               End If

               offset += subString.Length

            Next

         Else

            For Each mtch As Match In keywordRegEx.Matches(sourceCode)
               tokens.Add(New ColorToken(offset + mtch.Index, mtch.Length, Color.Blue, Color.Empty))
            Next

            Dim idx As Int32 = sourceCode.IndexOf("'"c)

            If idx > -1 Then
               tokens.Add(New ColorToken(offset + idx, endPos - offset - idx, Color.DarkGreen, Color.Empty))
            End If
         End If

         Return tokens

      End Function



	End Class




End Namespace
