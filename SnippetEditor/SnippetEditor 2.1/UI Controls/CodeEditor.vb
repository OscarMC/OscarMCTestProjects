'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Imports VB = Microsoft.VisualBasic

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.DesignerCategory("")> _
Friend Class CodeEditor
   Inherits RichTextBox


#Region "public Properties"


   Private m_TabSize As Int32 = 4 ' default size

   <DefaultValue(4)> _
   Public Property TabSize() As Int32
      Get
         Return m_TabSize
      End Get
      Set(ByVal value As Int32)
         m_TabSize = value
         Me.SetDefaultTabStop(value)
      End Set
   End Property



   Public Overrides Property Text() As String
      Get
         ' If Me.IsHandleCreated Then
         'Return Me.GetText
         ' Else
         Return MyBase.Text
         ' End If
      End Get
      Set(ByVal value As String)
         MyBase.Text = value
         ClearUndo()
         If Me.Created Then Me.ColorAll()
      End Set
   End Property


   Public ReadOnly Property TextWithoutCR() As String
      Get
         Return MyBase.Text
      End Get
   End Property



   Private m_CodeLanguage As CodeColorisers.CodeLanguage

   <DefaultValue(GetType(CodeColorisers.CodeLanguage), "VB")> _
   Public Property CodeLanguage() As CodeColorisers.CodeLanguage
      Get
         Return m_CodeLanguage
      End Get
      Set(ByVal value As CodeColorisers.CodeLanguage)
			If value <> m_CodeLanguage Then
				m_CodeLanguage = value
				Me.ColorAll()
			End If
		End Set
   End Property


	'Public Property SubString(ByVal start As Int32, ByVal [end] As Int32) As String
	'   Get
	'      Return Me.ITextDocument.Range(start, [end]).Text
	'   End Get
	'   Set(ByVal value As String)
	'      Me.ITextDocument.Range(start, [end]).Text = value
	'   End Set
	'End Property


   Public Function GetRange(ByVal start As Int32, ByVal [end] As Int32) As ITextRange
      Return Me.ITextDocument.Range(start, [end])
   End Function

#End Region


#Region "Highlighting methods"

   Public Sub HighightRange(ByVal start As Int32, ByVal [end] As Int32, ByVal color As Drawing.Color)
      Dim rng As ITextRange = Me.ITextDocument.Range(start, [end])
      rng.Font.BackColor = ColorToRGB(color)
   End Sub


   Public Sub ClearAllHighLighting()
      Dim rng As ITextRange = Me.ITextDocument.Range(0, Me.Text.Length)
      rng.Font.BackColor = ColorToRGB(Me.BackColor)
   End Sub

#End Region


#Region "Replace overloads"


   Public Function ReplaceAll(ByVal valueToFind As String, ByVal newValue As String) As Int32
      Return ReplaceAndHighlightAll(valueToFind, newValue, Me.BackColor)
   End Function


	'Public Function ReplaceAndHighlightAll(ByVal valueToFind As String, ByVal newValue As String) As Int32
	'   Return ReplaceAndHighlightAll(valueToFind, newValue, Color.Yellow)
	'End Function

   Public Function ReplaceAndHighlightAll(ByVal valueToFind As String, ByVal newValue As String, ByVal bgcolor As Color) As Int32
      If valueToFind = Nothing Then Return 0
      If newValue Is Nothing Then newValue = ""

      Dim cnt As Int32
      Dim clr As Int32 = ColorToRGB(bgcolor)
      Dim rng As ITextRange = Me.ITextDocument.Range(0, 1)
      Dim idx As Int32 = 0
      Dim length As Int32 = valueToFind.Length
      Dim newLength As Int32 = newValue.Length
      Do
         idx = MyBase.Text.IndexOf(valueToFind, idx)

         If idx >= 0 Then
            rng.SetRange(idx, idx + length)
            rng.Text = newValue
            rng.Font.BackColor = clr
            idx += newLength
         Else
            Exit Do
         End If
         If idx > MyBase.Text.Length Then Exit Do
      Loop
      Return cnt
   End Function


   Public Function ReplaceWords(ByVal oldWord As String, ByVal newWord As String) As Int32

      If oldWord = Nothing Then Return 0
      If newWord Is Nothing Then newWord = ""

      Dim idx As Int32 = 0
      Dim count As Int32 = 0
      Do While idx >= 0
         idx = Me.Find(oldWord, idx, RichTextBoxFinds.MatchCase Or RichTextBoxFinds.NoHighlight Or RichTextBoxFinds.WholeWord)
         If idx < 0 Then Exit Do
         Me.GetRange(idx, idx + oldWord.Length).Text = newWord
         idx += oldWord.Length + 1
         count += 1
      Loop

      Return count

   End Function


   Public Function IsWord(ByVal value As String, ByVal position As Int32) As Boolean
		If value Is Nothing OrElse position < 1 Then Return False
      Dim rtn As Int32 = Me.Find(value, position - 1, position + value.Length, RichTextBoxFinds.MatchCase Or RichTextBoxFinds.NoHighlight Or RichTextBoxFinds.WholeWord)
      Return rtn >= 0
   End Function


#End Region


#Region "coloring"

   Public Event ColoringCompleted As EventHandler

   Protected Sub OnColoringCompleted(ByVal ev As EventArgs)
      RaiseEvent ColoringCompleted(Me, ev)
   End Sub

   Public Sub ColorAll()
      If Not Me.Created Then Return

      Dim isReadOnly As Boolean
      If Me.ReadOnly Then
         isReadOnly = True
         Me.ReadOnly = False
      End If


      Me.ITextDocument.Freeze()

      Dim idx As Int32 = 0
      For Each line As String In Me.Lines
         ColorLine(idx, line)
         idx += line.Length + 1
      Next
      Me.ITextDocument.Unfreeze()


      If isReadOnly Then Me.ReadOnly = True
      OnColoringCompleted(EventArgs.Empty)
   End Sub




   Private Sub ColorCurrentLine(Optional ByVal offset As Int32 = 0)

      Dim href As New Runtime.InteropServices.HandleRef(Me, Me.Handle)

      Dim lineNumber As Int32
      Dim startIndex As Int32

      ' get the current line index
      lineNumber = NativeMethods.GetCurrentLineNumber(href) + offset
      startIndex = NativeMethods.GetLineCharIndex(href, lineNumber)
      Dim txt As String = NativeMethods.GetLine(href, lineNumber)

      If txt.Length > 0 Then
         ColorLine(startIndex, NativeMethods.GetCurrentLine(href))
      End If

      OnColoringCompleted(EventArgs.Empty)

   End Sub



   Public Sub ColorLine(ByVal offset As Int32, ByVal sourceLine As String)

      Dim endPos As Int32 = offset + sourceLine.Length
		Dim rg As ITextRange = Me.ITextDocument.Range(offset, endPos)

      If rg.Font.CanChange <> 0 Then
         rg.Font.ForeColor = &H0
         rg.Font.BackColor = ColorToRGB(Me.BackColor)
      End If

      For Each token As CodeColorisers.ColorToken In CodeColorisers.Coloriser.GetColoriser(Me.CodeLanguage).GetColorTokens(sourceLine, offset)
         rg.SetRange(token.Start, token.Start + token.Length)
         If Not token.ForeColor.IsEmpty AndAlso rg.Font.CanChange <> 0 Then rg.Font.ForeColor = ColorToRGB(token.ForeColor)
         If Not token.BackColor.IsEmpty AndAlso rg.Font.CanChange <> 0 Then rg.Font.BackColor = ColorToRGB(token.BackColor)
      Next

   End Sub



   Public Shared Function ColorToRGB(ByVal color As Color) As Int32
      Return (CInt(color.B) << 16) Or (CInt(color.G) << 8) Or (color.R)
   End Function


#End Region


#Region "Private Properties:: ITextDocument"

   Private m_Itextdocument As ITextDocument


   Private Property ITextDocument() As ITextDocument

      Get
         If m_Itextdocument Is Nothing Then
            m_Itextdocument = NativeMethods.GetOLEInterface(Me)
         End If
         Return m_Itextdocument
      End Get

      Set(ByVal value As ITextDocument)
         If value Is Nothing Then
            If Not m_Itextdocument Is Nothing Then
               Marshal.ReleaseComObject(m_Itextdocument)
            End If
         End If
         m_Itextdocument = value
      End Set

   End Property


#End Region


#Region "overrides"


   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      Me.ITextDocument = Nothing
      MyBase.Dispose(disposing)
   End Sub


   Protected Overrides Sub CreateHandle()
      Me.ITextDocument = Nothing
      MyBase.CreateHandle()
   End Sub


   

   Protected Overrides Sub DestroyHandle()
      Me.ITextDocument = Nothing
      MyBase.DestroyHandle()
   End Sub



   Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)
      MyBase.OnKeyUp(e)
      If e.Handled Then Return

      If e.Modifiers = Keys.Control Then
         Select Case e.KeyCode
            Case Keys.X
               ColorAll()
            Case Keys.Enter, Keys.LineFeed
               ColorCurrentLine(-1)
               ColorCurrentLine()
         End Select
         Return
      End If


      Select Case e.KeyCode

         Case Keys.Delete, Keys.Back, Keys.Tab, Keys.Space
            ColorCurrentLine()

         Case Keys.Return
            ColorCurrentLine(-1)
            ColorCurrentLine()

         Case Keys.D0 To Keys.Divide, Keys.OemSemicolon To Keys.OemBackslash
            ColorCurrentLine()

      End Select

   End Sub


   <SecurityPermission(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)> _
   Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
      Select Case keyData
         Case (Keys.V Or Keys.Control) 'paste
            MyClass.Paste()
            Return True
         Case (Keys.Z Or Keys.Control) ' undo
            MyClass.Undo()
            Return True
         Case (Keys.Y Or Keys.Control) ' redo
            MyClass.Redo()
            Return True
         Case (Keys.L Or Keys.Control), (Keys.R Or Keys.Control), (Keys.E Or Keys.Control) 'alignment
            Return True
         Case Else
            Return MyBase.ProcessCmdKey(msg, keyData)
      End Select
   End Function


   Public Shadows Sub Paste()
      MyBase.Paste(DataFormats.GetFormat(DataFormats.Text))
      Me.ColorAll()
   End Sub

#End Region


#Region "undo and redo tracking"
   ' unfortuantely we need to track this manually as the formatting interferes with the RTF undo/redo tracking
   ' note: we cannot use the TOM.ITextDocument.BeginEditcollection and EndEditCollection as these are not supported with the .NET RTF control


   Private m_Undo As New Stack(Of TextChangeInfo)
   Private m_Redo As New Stack(Of TextChangeInfo)
   Private m_LastText As String '= ""
   Private m_Undoing As Boolean '= False
   Private m_CurrentRange As NativeMethods.CharRange
   Private m_PreviousRange As NativeMethods.CharRange

   Private Structure TextChangeInfo
      Public Text As String
      Public Range As NativeMethods.CharRange

      Public Sub New(ByVal newText As String, ByVal newRange As NativeMethods.CharRange)
         Me.Range = newRange
         Me.Text = newText
      End Sub

   End Structure


   Public Shadows Sub Undo()
      If m_Undo.Count = 0 Then Return
      m_Undoing = True
      Dim oldInfo As New TextChangeInfo(Me.Text, Me.m_CurrentRange)
      Dim changeinfo As TextChangeInfo = m_Undo.Pop
      With changeinfo
         MyBase.Text = .Text
         m_LastText = .Text
         SetSelection(.Range)
         If Me.Created Then Me.ColorAll()
      End With
      m_Redo.Push(oldInfo)
      m_Undoing = False
   End Sub


   Public Shadows Sub Redo()
      If m_Redo.Count = 0 Then Return
      m_Undoing = True
      Dim changeinfo As TextChangeInfo = m_Redo.Pop
      With changeinfo
         MyBase.Text = .Text
         If Me.Created Then Me.ColorAll()
         SetSelection(.Range)
      End With
      m_Undo.Push(changeinfo)
      m_Undoing = False
   End Sub


   Public Shadows Sub ClearUndo()
      Me.m_Undo.Clear()
      Me.m_Redo.Clear()
      Me.m_CurrentRange = Nothing
      Me.m_PreviousRange = Nothing
      m_LastText = Me.Text
   End Sub


   Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
      Dim newText As String = Me.Text
      If Not m_Undoing Then
         If Not String.Equals(newText, m_LastText, StringComparison.Ordinal) Then
            m_Undo.Push(New TextChangeInfo(m_LastText, Me.m_PreviousRange))
            m_LastText = newText
         End If
      End If
      MyBase.OnTextChanged(e)
   End Sub


   Protected Overrides Sub OnSelectionChanged(ByVal e As System.EventArgs)
      MyBase.OnSelectionChanged(e)
      Dim range As NativeMethods.CharRange = GetSelection()
      Me.m_PreviousRange = Me.m_CurrentRange
      Me.m_CurrentRange = range
   End Sub

#End Region


#Region "win API"


   Private Sub SetDefaultTabStop(ByVal tabSize As Int32)
      NativeMethods.SetDefaultTabStop(tabSize, Me)
   End Sub

   Private Function GetText() As String
      Return NativeMethods.GetText(Me)
   End Function

   Private Function GetSelection() As NativeMethods.CharRange
      Return NativeMethods.GetSelection(Me)
   End Function

   Private Sub SetSelection(ByVal range As NativeMethods.CharRange)
      NativeMethods.SetSelection(range, Me)
   End Sub

   Private Class NativeMethods

      Private Sub New()
         ' class is Shared
      End Sub

      Friend Shared Sub SetDefaultTabStop(ByVal tabSize As Int32, ByVal editor As CodeEditor)
         SendMessageW(New HandleRef(editor, editor.Handle), &HCB, New IntPtr(1), tabSize * 4)
      End Sub

      Friend Shared Function GetText(ByVal editor As CodeEditor) As String
         Dim href As New HandleRef(editor, editor.Handle)
         Dim length As Int32 = SendMessageW(href, WM_GETTEXTLENGTH, IntPtr.Zero, 0).ToInt32 + 2
         Dim sb As New System.Text.StringBuilder(length + 2)
         SendMessageW(href, WM_GETTEXT, New IntPtr(length), sb)
         Return sb.ToString
      End Function

      Friend Shared Function GetSelection(ByVal editor As CodeEditor) As NativeMethods.CharRange
         Dim range As CharRange
         SendMessageW(New HandleRef(editor, editor.Handle), EM_EXGETSEL, IntPtr.Zero, range)
         Return range
      End Function

      Friend Shared Sub SetSelection(ByVal range As NativeMethods.CharRange, ByVal editor As CodeEditor)
         SendMessageW(New HandleRef(editor, editor.Handle), EM_EXSETSEL, IntPtr.Zero, range)
      End Sub

      Friend Shared Function GetOLEInterface(ByVal editor As CodeEditor) As ITextDocument
         Dim iDoc As ITextDocument = Nothing
         SendMessageW(New HandleRef(editor, editor.Handle), EM_GETOLEINTERFACE, IntPtr.Zero, iDoc)
         Return iDoc
      End Function


		'Friend Shared Function GetCurrentLine(ByVal editor As CodeEditor) As String
		'   Return GetCurrentLine(New HandleRef(editor, editor.Handle))
		'End Function

      Friend Shared Function GetCurrentLine(ByVal href As HandleRef) As String
         Return GetLine(href, GetCurrentLineNumber(href))
      End Function

		'Friend Shared Function GetLine(ByVal editor As CodeEditor, ByVal lineNumber As Int32) As String
		'   Return GetLine(New HandleRef(editor, editor.Handle), lineNumber)
		'End Function

      Friend Shared Function GetLine(ByVal href As HandleRef, ByVal lineNumber As Int32) As String
         Dim lineLength As Int32 = GetLineLength(href, lineNumber)

         If lineLength <= 0 Then Return ""

         Dim sb As New StringBuilder(lineLength + 2)
         sb.Append(VB.ChrW(lineLength))
         Dim rtn As Int32 = SendMessageW(href, EM_GETLINE, New IntPtr(lineNumber), sb).ToInt32

         Return sb.ToString(0, rtn)

      End Function



		'Friend Shared Function GetLineLength(ByVal editor As CodeEditor, ByVal lineNumber As Int32) As Int32
		'   Return GetLineLength(New HandleRef(editor, editor.Handle), lineNumber)
		'End Function

      Private Shared Function GetLineLength(ByVal href As HandleRef, ByVal lineNumber As Int32) As Int32
         Return SendMessageW(href, EM_LINELENGTH, New IntPtr(GetLineCharIndex(href, lineNumber)), 0).ToInt32
      End Function


		'Friend Shared Function GetCurrentLineNumber(ByVal editor As CodeEditor) As Int32
		'   Return GetCurrentLineNumber(New HandleRef(editor, editor.Handle))
		'End Function

      Friend Shared Function GetCurrentLineNumber(ByVal href As HandleRef) As Int32
         Return SendMessageW(href, EM_LINEFROMCHAR, New IntPtr(-1), 0).ToInt32
      End Function


		'Friend Shared Function GetLineCharIndex(ByVal editor As CodeEditor, ByVal lineNumber As Int32) As Int32
		'   Return GetLineCharIndex(New HandleRef(editor, editor.Handle), lineNumber)
		'End Function

      Friend Shared Function GetLineCharIndex(ByVal href As HandleRef, ByVal lineNumber As Int32) As Int32
         Return SendMessageW(href, EM_LINEINDEX, New IntPtr(lineNumber), 0).ToInt32
      End Function


      'Friend Shared Function SetAdvanacedTypography(ByVal href As HandleRef) As Int32
      '   Return SendMessageW(href, EM_SETTYPOGRAPHYOPTIONS, New IntPtr(TO_ADVANCEDTYPOGRAPHY), TO_ADVANCEDTYPOGRAPHY).ToInt32
      'End Function


      Private Declare Unicode Function SendMessageW Lib "user32" (ByVal hWnd As HandleRef, ByVal msg As Int32, ByVal wParam As IntPtr, ByRef lparam As ITextDocument) As IntPtr
      Private Declare Unicode Function SendMessageW Lib "user32" (ByVal hWnd As HandleRef, ByVal msg As Int32, ByVal wParam As IntPtr, ByVal lparam As StringBuilder) As IntPtr
      Private Declare Unicode Function SendMessageW Lib "user32" (ByVal hWnd As HandleRef, ByVal msg As Int32, ByVal wParam As IntPtr, ByRef lparam As Int32) As IntPtr
      Private Declare Unicode Function SendMessageW Lib "user32" (ByVal hWnd As HandleRef, ByVal msg As Int32, ByVal wParam As IntPtr, ByRef lparam As CharRange) As IntPtr


      Private Const WM_USER As Int32 = &H400
      Private Const EM_EXGETSEL As Int32 = (WM_USER + 52)
      Private Const EM_EXSETSEL As Int32 = (WM_USER + 55)
      Private Const EM_GETOLEINTERFACE As Int32 = (WM_USER + 60)
      Private Const EM_GETLINE As Int32 = &HC4
      Private Const EM_LINEFROMCHAR As Int32 = &HC9
      Private Const EM_LINELENGTH As Int32 = &HC1
      Private Const EM_LINEINDEX As Int32 = &HBB
      Private Const EM_SETTABSTOPS As Int32 = &HCB
      Private Const WM_GETTEXT As Int32 = &HD
      Private Const WM_GETTEXTLENGTH As Int32 = &HE
      Private Const TO_ADVANCEDTYPOGRAPHY As Int32 = 1
      Private Const EM_SETTYPOGRAPHYOPTIONS As Int32 = (WM_USER + 202)
   


      Friend Structure CharRange
         Public Min As Int32
         Public Max As Int32
      End Structure


   End Class


#End Region


End Class


#Region "COM Interfaces for RichEdit control's TextObjectModel "

<ComImport(), TypeLibType(CType(192, Short)), Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Name")> _
Public Interface ITextDocument
   ' Methods
   <DispId(0)> _
    ReadOnly Property Name() As <MarshalAs(UnmanagedType.BStr)> String
   <DispId(1)> _
   ReadOnly Property Selection() As <MarshalAs(UnmanagedType.Interface)> ITextSelection
   <DispId(2)> _
   ReadOnly Property StoryCount() As Integer
   <DispId(3)> _
   ReadOnly Property StoryRanges() As <MarshalAs(UnmanagedType.Interface)> Object
   <DispId(4)> _
   Property Saved() As Integer
   <DispId(5)> _
   Property DefaultTabStop() As Single

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(6)> _
    Sub [New]()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(7)> _
   Sub Open(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Flags As Integer, <[In]()> ByVal CodePage As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(8)> _
   Sub Save(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Flags As Integer, <[In]()> ByVal CodePage As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(9)> _
   Function Freeze() As Integer

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(10)> _
   Function Unfreeze() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(11)> _
    Sub BeginEditCollection()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(12)> _
   Sub EndEditCollection()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(13)> _
   Function Undo(<[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(14)> _
   Function Redo(<[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(15)> _
   Function Range(<[In]()> ByVal cp1 As Integer, <[In]()> ByVal cp2 As Integer) As <MarshalAs(UnmanagedType.Interface)> ITextRange
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(16)> _
   Function RangeFromPoint(<[In]()> ByVal x As Integer, <[In]()> ByVal y As Integer) As <MarshalAs(UnmanagedType.Interface)> ITextRange
   ' Properties


End Interface

<ComImport(), TypeLibType(CType(192, Short)), Guid("8CC497C2-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Text")> _
Public Interface ITextRange

   <DispId(0)> _
   Property [Text]() As <MarshalAs(UnmanagedType.BStr)> String


   <DispId(513)> _
   Property [Char]() As Integer
   <DispId(514)> _
   ReadOnly Property Duplicate() As <MarshalAs(UnmanagedType.Interface)> ITextRange
   <DispId(515)> _
   Property FormattedText() As <MarshalAs(UnmanagedType.Interface)> ITextRange
   <DispId(516)> _
    Property Start() As Integer
   <DispId(517)> _
   Property [End]() As Integer
   <DispId(518)> _
   Property Font() As <MarshalAs(UnmanagedType.Interface)> ITextFont
   <DispId(519)> _
   Property Para() As <MarshalAs(UnmanagedType.Interface)> ITextPara
   <DispId(520)> _
    ReadOnly Property StoryLength() As Integer
   <DispId(521)> _
   ReadOnly Property StoryType() As Integer


   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(528)> _
    Sub Collapse(<[In]()> ByVal bStart As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(529)> _
     Function Expand(<[In]()> ByVal Unit As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(530)> _
    Function GetIndex(<[In]()> ByVal Unit As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(531)> _
    Sub SetIndex(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Index As Integer, <[In]()> ByVal Extend As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(532)> _
   Sub SetRange(<[In]()> ByVal cpActive As Integer, <[In]()> ByVal cpOther As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(533)> _
    Function InRange(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(534)> _
   Function InStory(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(535)> _
   Function IsEqual(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(536)> _
    Sub [Select]()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(537)> _
   Function StartOf(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer


   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(544)> _
     Function EndOf(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(545)> _
    Function Move(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(546)> _
   Function MoveStart(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(547)> _
   Function MoveEnd(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(548)> _
    Function MoveWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(549)> _
     Function MoveStartWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(550)> _
    Function MoveEndWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(551)> _
   Function MoveUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(552)> _
    Function MoveStartUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(553)> _
    Function MoveEndUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer


   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(560)> _
     Function FindText(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(561)> _
    Function FindTextStart(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(562)> _
    Function FindTextEnd(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(563)> _
    Function Delete(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(564)> _
     Sub Cut(<Out(), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(565)> _
    Sub Copy(<Out(), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(566)> _
   Sub Paste(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Format As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(567)> _
    Function CanPaste(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Format As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(568)> _
    Function CanEdit() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(569)> _
   Sub ChangeCase(<[In]()> ByVal Type As Integer)


   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(576)> _
     Sub GetPoint(<[In]()> ByVal Type As Integer, <Out()> ByRef px As Integer, <Out()> ByRef py As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(577)> _
    Sub SetPoint(<[In]()> ByVal x As Integer, <[In]()> ByVal y As Integer, <[In]()> ByVal Type As Integer, <[In]()> ByVal Extend As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(578)> _
     Sub ScrollIntoView(<[In]()> ByVal Value As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(579)> _
    Function GetEmbeddedObject() As <MarshalAs(UnmanagedType.IUnknown)> Object

End Interface

<ComImport(), Guid("8CC497C1-A1DF-11CE-8098-00AA0047BE5D"), TypeLibType(CType(192, Short)), DefaultMember("Text")> _
Public Interface ITextSelection
   'Inherits ITextRange
   <DispId(0)> _
   Property [Text]() As <MarshalAs(UnmanagedType.BStr)> String

   <DispId(257)> _
   Property Flags() As Integer
   <DispId(258)> _
   ReadOnly Property Type() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(259)> _
   Function MoveLeft(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(260)> _
   Function MoveRight(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(261)> _
   Function MoveUp(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(262)> _
     Function MoveDown(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(263)> _
    Function HomeKey(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(264)> _
   Function EndKey(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(265)> _
   Sub TypeText(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String)


   <DispId(513)> _
   Property [Char]() As Integer
   <DispId(514)> _
   ReadOnly Property Duplicate() As <MarshalAs(UnmanagedType.Interface)> ITextRange
   <DispId(515)> _
   Property FormattedText() As <MarshalAs(UnmanagedType.Interface)> ITextRange
   <DispId(516)> _
   Property Start() As Integer
   <DispId(517)> _
   Property [End]() As Integer
   <DispId(518)> _
   Property Font() As <MarshalAs(UnmanagedType.Interface)> ITextFont
   <DispId(519)> _
   Property Para() As <MarshalAs(UnmanagedType.Interface)> ITextPara
   <DispId(520)> _
   ReadOnly Property StoryLength() As Integer
   <DispId(521)> _
   ReadOnly Property StoryType() As Integer



   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(528)> _
   Sub Collapse(<[In]()> ByVal bStart As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(529)> _
    Function Expand(<[In]()> ByVal Unit As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(530)> _
    Function GetIndex(<[In]()> ByVal Unit As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(531)> _
     Sub SetIndex(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Index As Integer, <[In]()> ByVal Extend As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(532)> _
   Sub SetRange(<[In]()> ByVal cpActive As Integer, <[In]()> ByVal cpOther As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(533)> _
   Function InRange(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(534)> _
   Function InStory(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(535)> _
   Function IsEqual(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pRange As ITextRange) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(536)> _
   Sub [Select]()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(537)> _
   Function StartOf(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer


   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(544)> _
   Function EndOf(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Extend As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(545)> _
    Function Move(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(546)> _
    Function MoveStart(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(547)> _
    Function MoveEnd(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(548)> _
    Function MoveWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(549)> _
     Function MoveStartWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(550)> _
    Function MoveEndWhile(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(551)> _
   Function MoveUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(552)> _
    Function MoveStartUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(553)> _
    Function MoveEndUntil(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef Cset As Object, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(560)> _
     Function FindText(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(561)> _
     Function FindTextStart(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(562)> _
    Function FindTextEnd(<[In](), MarshalAs(UnmanagedType.BStr)> ByVal bstr As String, <[In]()> ByVal cch As Integer, <[In]()> ByVal Flags As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(563)> _
   Function Delete(<[In]()> ByVal Unit As Integer, <[In]()> ByVal Count As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(564)> _
   Sub Cut(<Out(), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(565)> _
   Sub Copy(<Out(), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(566)> _
     Sub Paste(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Format As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(567)> _
   Function CanPaste(<[In](), MarshalAs(UnmanagedType.Struct)> ByRef pVar As Object, <[In]()> ByVal Format As Integer) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(568)> _
   Function CanEdit() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(569)> _
   Sub ChangeCase(<[In]()> ByVal Type As Integer)

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(576)> _
    Sub GetPoint(<[In]()> ByVal Type As Integer, <Out()> ByRef px As Integer, <Out()> ByRef py As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(577)> _
    Sub SetPoint(<[In]()> ByVal x As Integer, <[In]()> ByVal y As Integer, <[In]()> ByVal Type As Integer, <[In]()> ByVal Extend As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(578)> _
   Sub ScrollIntoView(<[In]()> ByVal Value As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(579)> _
    Function GetEmbeddedObject() As <MarshalAs(UnmanagedType.IUnknown)> Object



End Interface

'<ComImport(), Guid("8CC497C5-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Item"), TypeLibType(CType(192, Short))> _
'Public Interface ITextStoryRanges
'	'Implements IEnumerable

'' Methods
'	<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), TypeLibFunc(CType(1, Short)), DispId(-4)> _
'	Function GetEnumerator() As <MarshalAs(UnmanagedType.CustomMarshaler, MarshalType:="", MarshalTypeRef:=GetType(Runtime.InteropServices.custommarshallers.EnumeratorToEnumVariantMarshaler), MarshalCookie:="")> Collections.IEnumerator
'	<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(0)> _
'	Function Item(<[In]()> ByVal Index As Integer) As <MarshalAs(UnmanagedType.Interface)> ITextRange

'	' Properties
'<DispId(2)> _
'ReadOnly Property Count() As Integer
'End Interface

<ComImport(), Guid("8CC497C3-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Duplicate"), TypeLibType(CType(192, Short))> _
Public Interface ITextFont
   <DispId(0)> _
    Property Duplicate() As <MarshalAs(UnmanagedType.Interface)> ITextFont
   ' Methods
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(769)> _
   Function CanChange() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(770)> _
   Function IsEqual(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pFont As ITextFont) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(771)> _
   Sub Reset(<[In]()> ByVal Value As Integer)

   ' Properties
   <DispId(772)> _
   Property Style() As Integer
   <DispId(773)> _
   Property AllCaps() As Integer
   <DispId(774)> _
   Property Animation() As Integer
   <DispId(775)> _
   Property BackColor() As Integer
   <DispId(776)> _
   Property Bold() As Integer
   <DispId(777)> _
    Property Emboss() As Integer

   <DispId(784)> _
    Property ForeColor() As Integer
   <DispId(785)> _
   Property Hidden() As Integer
   <DispId(786)> _
    Property Engrave() As Integer
   <DispId(787)> _
   Property Italic() As Integer
   <DispId(788)> _
   Property Kerning() As Single
   <DispId(789)> _
   Property LanguageID() As Integer
   <DispId(790)> _
   Property Name() As <MarshalAs(UnmanagedType.BStr)> String
   <DispId(791)> _
   Property Outline() As Integer
   <DispId(792)> _
   Property Position() As Single
   <DispId(793)> _
   Property [Protected]() As Integer

   <DispId(800)> _
   Property Shadow() As Integer
   <DispId(801)> _
   Property Size() As Single
   <DispId(802)> _
   Property SmallCaps() As Integer
   <DispId(803)> _
   Property Spacing() As Single
   <DispId(804)> _
   Property StrikeThrough() As Integer
   <DispId(805)> _
   Property Subscript() As Integer
   <DispId(806)> _
   Property Superscript() As Integer
   <DispId(807)> _
   Property Underline() As Integer
   <DispId(808)> _
   Property Weight() As Integer
End Interface

<ComImport(), Guid("8CC497C4-A1DF-11CE-8098-00AA0047BE5D"), TypeLibType(CType(192, Short)), DefaultMember("Duplicate")> _
Public Interface ITextPara

   <DispId(0)> _
    Property Duplicate() As <MarshalAs(UnmanagedType.Interface)> ITextPara

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1025)> _
    Function CanChange() As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1026)> _
   Function IsEqual(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pPara As ITextPara) As Integer
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1027)> _
   Sub Reset(<[In]()> ByVal Value As Integer)
   <DispId(1028)> _
    Property Style() As Integer
   <DispId(1029)> _
    Property Alignment() As Integer
   <DispId(1031)> _
   ReadOnly Property FirstLineIndent() As Single
   <DispId(1030)> _
   Property Hyphenation() As Integer
   <DispId(1032)> _
   Property KeepTogether() As Integer
   <DispId(1033)> _
   Property KeepWithNext() As Integer

   <DispId(1040)> _
   ReadOnly Property LeftIndent() As Single
   <DispId(1041)> _
   ReadOnly Property LineSpacing() As Single
   <DispId(1042)> _
   ReadOnly Property LineSpacingRule() As Integer
   <DispId(1043)> _
   Property ListAlignment() As Integer
   <DispId(1044)> _
   Property ListLevelIndex() As Integer
   <DispId(1045)> _
   Property ListStart() As Integer
   <DispId(1046)> _
   Property ListTab() As Single
   <DispId(1047)> _
   Property ListType() As Integer
   <DispId(1048)> _
   Property NoLineNumber() As Integer
   <DispId(1049)> _
   Property PageBreakBefore() As Integer

   <DispId(1056)> _
   Property RightIndent() As Single

   <DispId(1059)> _
   Property SpaceAfter() As Single
   <DispId(1060)> _
   Property SpaceBefore() As Single
   <DispId(1061)> _
   Property WidowControl() As Integer
   <DispId(1062)> _
   ReadOnly Property TabCount() As Integer

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1057)> _
    Sub SetIndents(<[In]()> ByVal StartIndent As Single, <[In]()> ByVal LeftIndent As Single, <[In]()> ByVal RightIndent As Single)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1058)> _
   Sub SetLineSpacing(<[In]()> ByVal LineSpacingRule As Integer, <[In]()> ByVal LineSpacing As Single)




   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1063)> _
   Sub AddTab(<[In]()> ByVal tbPos As Single, <[In]()> ByVal tbAlign As Integer, <[In]()> ByVal tbLeader As Integer)
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1064)> _
   Sub ClearAllTabs()
   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1065)> _
   Sub DeleteTab(<[In]()> ByVal tbPos As Single)

   <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), DispId(1072)> _
   Sub GetTab(<[In]()> ByVal iTab As Integer, <Out()> ByRef ptbPos As Single, <Out()> ByRef ptbAlign As Integer, <Out()> ByRef ptbLeader As Integer)

End Interface


#End Region



























