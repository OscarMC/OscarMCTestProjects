'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On


''' <summary>
''' Provides the result value of an actions such as save or export.
''' </summary>
''' <remarks>Used by SaveAs and ExportToVSI etc.</remarks>
Friend Enum ActionResult
   Success = 0
	Fail = 1
	Cancelled = 2
End Enum
