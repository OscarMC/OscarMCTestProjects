'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System


''' <summary>
''' Extends EventArgs to include a Path property.
''' </summary>
''' <remarks>Used to indicate the path selected in the snippet explorer and allow main form to handle those events</remarks>
Friend Class PathEventArgs
   Inherits EventArgs

   Public Sub New(ByVal path As String)
      Me.m_Path = path
   End Sub

   Private m_Path As String

   Public ReadOnly Property Path() As String
      Get
         Return m_Path
      End Get
   End Property

End Class
