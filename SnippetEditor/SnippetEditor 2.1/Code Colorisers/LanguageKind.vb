'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On




Namespace CodeColorisers

   ''' <summary>
   ''' public enum for known languages.
   ''' Used primarily with the code colorisers.
   ''' </summary>
   ''' <remarks>
   ''' Potentially subject to change in the future !  
   ''' But does provide a more efficient way of filtering than using strings.
   ''' </remarks>
   Public Enum CodeLanguage
      VB
      CSharp
      JSharp
      XML
      Unknown
   End Enum


End Namespace

