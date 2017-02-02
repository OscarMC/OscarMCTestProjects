'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Xml
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports SnippetEditor.SnippetRepresentation
Imports System.ComponentModel




Friend Class SnippetEditorControl
   'Implements INotifyPropertyChanged



#Region "private fields"


   Private _snippetFile As SnippetFile

   Private ReadOnly CrLf As String = Environment.NewLine

#End Region

   ' expose public properties Font and TabSize to allow binding 
#Region "Public Properties"


   Private _font As Font = Me.Font
   Private _TabSize As Int32 = 4 ' default size


   Public Property EditorFont() As Font
      Get
         Return _font
      End Get
      Set(ByVal value As Drawing.Font)
         _font = value
         UpDateFont()
         UpdateTabSize(_TabSize)
      End Set
   End Property


   ' updates the font to the text editor controls
   Private Sub UpDateFont()
      m_editorTextBox.Font = _font
      ColorAll()
   End Sub


   Public Property TabSize() As Int32
      Get
         Return _TabSize
      End Get
      Set(ByVal value As Int32)
         _TabSize = value
         UpdateTabSize(value)
      End Set
   End Property


   Private Sub UpdateTabSize(ByVal value As Int32)
      Me.m_editorTextBox.TabSize = value
      ColorAll()
   End Sub


   Private Sub ColorAll()
      Me.m_editorTextBox.ColorAll()
   End Sub

   Public Property SnippetFile() As SnippetFile
      Get
         Return Me._snippetFile
      End Get
      Set(ByVal value As SnippetFile)
         Me._snippetFile = value
      End Set
   End Property

   Private Property CodeWindowHeight() As Int32
      Get
         Return m_editorTextBox.Height
      End Get
      Set(ByVal value As Int32)
         m_editorTextBox.Height = value
      End Set
   End Property

   Private Property Collapse_Properties() As Boolean
      Get
         Return Me.ExPanel1.Collapsed
      End Get
      Set(ByVal value As Boolean)
         Me.ExPanel1.Collapsed = value
      End Set
   End Property

   Private Property Collapse_Code() As Boolean
      Get
         Return Me.ExPanel2.Collapsed
      End Get
      Set(ByVal value As Boolean)
         Me.ExPanel2.Collapsed = value
      End Set
   End Property

   Private Property Collapse_References() As Boolean
      Get
         Return Me.ExPanel3.Collapsed
      End Get
      Set(ByVal value As Boolean)
         Me.ExPanel3.Collapsed = value
      End Set
   End Property

   Private Property Collapse_Imports() As Boolean
      Get
         Return Me.ExPanel4.Collapsed
      End Get
      Set(ByVal value As Boolean)
         Me.ExPanel4.Collapsed = value
      End Set
   End Property


#End Region


   'This region contains event handlers for general events and events of the MainForm
#Region "SnippetEditorControl events"



   ''' <summary>
   ''' Event handler for the loading of the form. Used to do initialisaton operations
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub SnippetEditorControl_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

      'Bind event handlers
      m_languageComboBox.SelectedIndex = 0 'Default to VB
      AddHandler m_languageComboBox.SelectedIndexChanged, AddressOf Me.UserChangedLanguage

      m_kindComboBox.SelectedIndex = 3 'Default kind to unspecified
      AddHandler m_kindComboBox.SelectedIndexChanged, AddressOf Me.UserChangedKind

      BindUIToSnippet()

      With My.Settings
         Collapse_Code = .CodePanel_Collapsed
         Collapse_Imports = .ImportsPanel_Collapsed
         Collapse_Properties = .PropertiesPanel_Collapsed
         Collapse_References = .ReferencesPanel_Collapsed
         CodeWindowHeight = .CodeWindowHeight
      End With

   End Sub




   Private Sub SnippetEditorControl_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
      With My.Settings
         .CodePanel_Collapsed = Collapse_Code
         .ImportsPanel_Collapsed = Collapse_Imports
         .PropertiesPanel_Collapsed = Collapse_Properties
         .ReferencesPanel_Collapsed = Collapse_References
         .CodeWindowHeight = CodeWindowHeight
      End With
   End Sub


#End Region


   'This region contains methods related to menu items such as New, Open, Save, Save As...
#Region "Snippet new, open, save..."


   ''' <summary>
   ''' This method tries to create a new snippet
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub NewSnippet()
      _snippetFile.CreateNewSnippet()
      BindUIToSnippet()
      RaiseEvent SnippetOpened(Me, New PathEventArgs(Nothing))
   End Sub


   Public Sub OpenSnippet(ByVal strPath As String)
      _snippetFile.LoadFile(strPath)
      BindUIToSnippet()
      RaiseEvent SnippetOpened(Me, New PathEventArgs(strPath))
   End Sub

   Public Event SnippetOpened As EventHandler(Of PathEventArgs)





   ''' <summary>
   ''' This method tries to save the current snippet
   ''' </summary>
   ''' <returns>True if it succeeded, false if it failed</returns>
   ''' <remarks></remarks>
   Public Function SaveSnippet() As Boolean
      Validate()
      If (_snippetFile.ToFile(_snippetFile.Filename)) Then
         Return True
      Else
         Return False
      End If
   End Function





#End Region


   'This region contains BindUIToSnippet() along with associated methods
#Region "General binding methods"

   ''' <summary>
   ''' This method binds all UI fields to the snippet data
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub BindUIToSnippet()
      If Me._snippetFile Is Nothing Then Return
      If _snippetFile.CodeSnippet Is Nothing Then Return
      'Bind Header
      m_headerBindingSource.DataSource = _snippetFile.CodeSnippet.Header

      'Bind Imports
      m_importsBindingSource.DataSource = _snippetFile.CodeSnippet.Snippet.Imports.List

      'Bind References
      m_referencesBindingSource.DataSource = _snippetFile.CodeSnippet.Snippet.References.List

      'Bind Replacements
      m_replacementsBindingSource.DataSource = _snippetFile.CodeSnippet.Snippet.Declarations.Replacements
      'm_replacementBindingNavigator.BindingSource = m_replacementsBindingSource

      'Bind Code
      m_codeBindingSource.DataSource = _snippetFile.CodeSnippet.Snippet.Code

      'Update fields that are not binded through binding sources
      UpdateLanguage()
      UpdateKind()
      SelectReplacement(0)
      PrettyEditor()

   End Sub


#End Region ' General bindings methods


   'This region contains methods associated with the snippet properties
#Region "Snippet properties"

   'This region contains methods associated with the CodeSnippet\Snippet\Code Language attribute
#Region "Language"

   ''' <summary>
   ''' This method serves as an event handler for checked events on radio buttons specifying the Language
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub UserChangedLanguage(ByVal sender As Object, ByVal e As EventArgs)
      With _snippetFile.CodeSnippet.Snippet.Code
         'HTML
         'JScript
         'Visual Basic
         'Visual C#
         'Visual J#
         'Xml
         Select Case m_languageComboBox.SelectedIndex
            Case 0
               .Language = "html"
            Case 1
               .Language = "jscript"
            Case 2 'vb
               .Language = Language.VisualBasic
            Case 3 'c#
               .Language = Language.CSharp
            Case 4 'j#
               .Language = Language.JSharp
            Case 5 'xml
               .Language = Language.XML
            Case Else
               .Language = m_languageComboBox.Text
         End Select
      End With
   End Sub


   ''' <summary>
   ''' sets the state of the Language combobox
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub UpdateLanguage()
      With _snippetFile.CodeSnippet.Snippet.Code
         'HTML
         'JScript
         'Visual Basic
         'Visual C#
         'Visual J#
         'Xml
         Select Case .Language.ToLowerInvariant
            Case "html"
               m_languageComboBox.SelectedIndex = 0

            Case "jscript"
               m_languageComboBox.SelectedIndex = 1

            Case Language.VisualBasic.ToLowerInvariant
               m_languageComboBox.SelectedIndex = 2

            Case Language.CSharp.ToLowerInvariant
               m_languageComboBox.SelectedIndex = 3

            Case Language.JSharp.ToLowerInvariant
               m_languageComboBox.SelectedIndex = 4

            Case Language.XML.ToLowerInvariant
               m_languageComboBox.SelectedIndex = 5

            Case Else
               
               m_languageComboBox.Text = .Language
         End Select
      End With
   End Sub

#End Region


   'This region contains methods associated with the CodeSnippet\Snippet\Code Kind attribute
#Region "Kind"

   ''' <summary>
   ''' This method serves as an event handler for checked events on radio buttons specifying the Kind of the snippet
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub UserChangedKind(ByVal sender As Object, ByVal e As EventArgs)
      With _snippetFile.CodeSnippet.Snippet.Code
         Select Case m_kindComboBox.SelectedIndex
            Case 0
               .Kind = Kind.TypeDeclaration

            Case 1
               .Kind = Kind.MethodDeclaration

            Case 2
               .Kind = Kind.MethodBody

            Case 3
               .Kind = Kind.Unspecified

         End Select
      End With
   End Sub ' UserChangedKind


   ''' <summary>
   ''' This method sets the state of the Kind radio buttons according to the snippet data
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub UpdateKind()

      Select Case _snippetFile.CodeSnippet.Snippet.Code.Kind

         Case Kind.TypeDeclaration
            m_kindComboBox.SelectedIndex = 0

         Case Kind.MethodDeclaration
            m_kindComboBox.SelectedIndex = 1

         Case Kind.MethodBody
            m_kindComboBox.SelectedIndex = 2

         Case Else
            'The kind is unspecified or unknown : default to Unspecified
            m_kindComboBox.SelectedIndex = 3
      End Select

   End Sub
#End Region

#End Region ' Snippet properties


   'This region contains methods associated with the replacements pane
   'and with the CodeSnippet\Snippet\Declarations element
#Region "Replacements"


#Region "Event handlers"

   Private Sub BindingNavigatorDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingNavigatorDeleteItem.Click
      DeleteCurrentReplacement()
   End Sub


   Private Sub BindingNavigatorAddNewItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorAddNewItem.Click
      AddSelectedTextAsReplacement()
   End Sub


   Private Sub m_replacementsBindingSource_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles m_replacementsBindingSource.ListChanged
      EnableOrDisableReplacementControls()
   End Sub


   Private Sub m_replacementsBindingSource_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_replacementsBindingSource.PositionChanged
      HighLightReplacements(m_editorTextBox)
   End Sub


   Private Sub m_LitvsobjComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_litvsobjComboBox.SelectedIndexChanged
      If Not Me.m_codeBindingSource.IsBindingSuspended Then
         Dim rep As Replacement = CType(Me.m_replacementsBindingSource.Current, Replacement)
         If rep IsNot Nothing Then
            rep.ReplacementKind = CType(Me.m_litvsobjComboBox.SelectedIndex, ReplacementKind)
         End If
      End If
   End Sub

   Private Sub m_idField_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_idField.Validated
      HighLightReplacements(m_editorTextBox)
   End Sub


   Private Sub m_idField_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_idField.Validating
      Dim newID As String = m_idField.Text
      Dim rep As Replacement = GetCurrentReplacement()

      If rep Is Nothing Then Return

      newID = newID.Trim
      If newID = Nothing Then
         e.Cancel = True
         Return
      End If

      ChangeReplacmentText(rep, newID)
   End Sub


#End Region




   ''' <summary>
   ''' finds an exisitng replacement by it's ID.  If none exisits the function return Nothing
   ''' </summary>
   ''' <param name="id">The id of the replacement to select</param>
   ''' <remarks></remarks>
   Private Function GetReplacement(ByVal id As String) As Replacement
      For Each rep As Replacement In m_replacementsBindingSource
         If id = rep.Id Then
            Return rep
         End If
      Next
      Return Nothing
   End Function


   ''' <summary>
   ''' This method selects the replacement at a specific position (zero based)
   ''' </summary>
   ''' <param name="position">Position of the replacement to select</param>
   ''' <remarks></remarks>
   Private Sub SelectReplacement(ByVal position As Integer)

      If (position >= 0) AndAlso (position < m_replacementsBindingSource.Count) Then
         m_replacementsBindingSource.Position = position
         HighLightReplacements(Me.m_editorTextBox)
      End If

      EnableOrDisableReplacementControls()

   End Sub


   Private Sub EnableOrDisableReplacementControls()
      Dim enable As Boolean = (m_replacementsBindingSource.Count > 0)
      For Each cntl As Control In Me.Panel1.Controls
         cntl.Enabled = enable
      Next
      replacementBindingNavigator.Enabled = True
   End Sub


   ''' <summary>
   ''' deletes the current replacement
   ''' </summary>
   Private Sub DeleteCurrentReplacement()
      If m_replacementsBindingSource.Count = 0 Then Return

      Dim rep As Replacement = TryCast(m_replacementsBindingSource.Current, Replacement)
      If rep Is Nothing Then Return

      Dim sId As String = rep.Id
      Dim sDefault As String = rep.Default
      If sDefault = Nothing Then sDefault = sId
      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter

      Me.m_editorTextBox.ReplaceAll(delim + sId + delim, sDefault)

      Me.m_replacementsBindingSource.Remove(rep)
   End Sub



   ''' <summary>
   ''' returns the currently selected replacement or nothing if no replacement is selected
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function GetCurrentReplacement() As Replacement
      If (m_replacementsBindingSource.Count = 0) Then
         Return Nothing
      Else
         Return CType(m_replacementsBindingSource.Current, Replacement)
      End If
   End Function


   ''' <summary>
   ''' changes each occurence of the replacment in the code with the new replament name. 
   ''' </summary>
   ''' <param name="currentReplacement">the current replacement</param>
   ''' <param name="newID">the new replacement ID</param>
   ''' <remarks></remarks>
   Private Sub ChangeReplacmentText(ByVal currentReplacement As Replacement, ByVal newID As String)

      If _snippetFile Is Nothing OrElse _snippetFile.CodeSnippet Is Nothing Then Return 'early exit

      Dim rtb As CodeEditor = Me.m_editorTextBox
      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter

      Dim words As New List(Of String)
      Dim strId As String


      For Each rep As Replacement In _snippetFile.CodeSnippet.Snippet.Declarations.Replacements
         strId = rep.Id
         If strId = Nothing Then Continue For
         words.Add(strId & delim)
      Next

      newID = delim & newID & delim

      Dim j, idx, pos, offset As Int32
      Dim txt As String = rtb.TextWithoutCR
      Dim txtLength As Int32 = txt.Length
      Dim wrdLength As Int32 = 0
      Dim rng As ITextRange
      Dim repId As String = currentReplacement.Id & delim


      idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)

      While idx >= 0
         j = 0
         idx += 1
         For Each word As String In words
            wrdLength = word.Length
            If String.CompareOrdinal(txt, idx, word, 0, wrdLength) = 0 Then
               If word <> repId Then Exit For
               pos = idx + offset - 1
               rng = rtb.GetRange(pos, pos + wrdLength + 1)
               rng.Text = newID
               offset += newID.Length - wrdLength - 1
               idx += word.Length
               Exit For
            End If
            j += 1
         Next

         If idx >= txtLength Then Exit While

         idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)
      End While

   End Sub



   ''' <summary>
   ''' adds the selected text of the editor text box as a Literal
   ''' </summary>
   ''' <remarks>
   ''' workflow:
   ''' - if the selected text matches an exisitng replacment, then ensure the text is properly delimited then slect exisitng the replacement. Take care to check for surrounding replacement delimiters when adding delimiters
   ''' -if it doesn't match an existing replacement then create a new replacment and replace all instances of that word using whole word only search and replace.
   ''' -
   ''' </remarks>
   Private Function AddSelectedTextAsReplacement() As Replacement

      Dim rep As Replacement = Nothing
      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter
      Dim selText As String = m_editorTextBox.SelectedText.Trim
      Dim id As String = selText
      Dim isNew As Boolean

      If id = Nothing OrElse id.Contains(ControlChars.Lf) Then
         id = GenerateUniqueID()
         isNew = True
      Else
         If id.StartsWith(delim) Then id = id.Substring(1)
         If id.EndsWith(delim) Then id = id.Substring(0, id.Length - 1)
      End If

      If Not isNew Then rep = GetReplacement(id)

      isNew = (rep Is Nothing)

      If isNew Then
         ' new replacemnt. Add it, replace the selection then do a whole word search and replace
         rep = AddNewReplacement(id, ReplacementKind.Literal)
         Dim rng As ITextRange = Nothing
         With m_editorTextBox
            Dim pos As Int32 = .SelectionStart
            .SelectedText = id
            If Not .IsWord(id, pos) Then rng = .GetRange(pos, pos + id.Length)
            .ReplaceWords(id, delim & id & delim)
            If rng IsNot Nothing Then rng.Text = delim & id & delim
         End With

      Else
         'need to mark up selected text to make it a replacement
         If selText.StartsWith(delim) Then
            m_editorTextBox.SelectedText = delim & id & delim
         Else
            With m_editorTextBox
               'if the selection's backgorund color is highlights it must already be a selection, so we don't mark it up. Otherwise add delimeters.
               If .GetRange(.SelectionStart, .SelectionStart + .SelectionLength).Font.BackColor = CodeEditor.ColorToRGB(.BackColor) Then
                  m_editorTextBox.SelectedText = delim & id & delim
               End If
            End With

         End If
      End If

      'update replacement tab and highlighting
      SelectReplacement(m_replacementsBindingSource.IndexOf(rep))

      Return rep
   End Function



   ''' <summary>
   ''' generates a unique ID for the replacements
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function GenerateUniqueID() As String
      Dim counter As Integer = 1
      Dim id As String = My.Resources.NewLiteral
      Dim uniqueID As String = id
      Dim bRenameRequired As Boolean = True


      ' Check in the declarations list
      While bRenameRequired = True
         bRenameRequired = False
         For i As Integer = 0 To m_replacementsBindingSource.Count - 1
            If CType(m_replacementsBindingSource.Item(i), Replacement).Id = uniqueID Then
               bRenameRequired = True
               uniqueID = id & counter
               counter += 1
               Exit For
            End If
         Next
      End While

      Return uniqueID
   End Function


   Private Function AddNewReplacement(ByVal repName As String, ByVal repType As ReplacementKind) As Replacement
      Dim rep As New Replacement(repName)
      rep.ReplacementKind = repType
      rep.Default = repName
      m_replacementsBindingSource.Add(rep)
      Return rep
   End Function


#End Region


   'This region contains methods associated with the code editor
   'and with the CodeSnippet\Snippet\Code\Data element
#Region "Code Data"

#Region "Event handlers"

   Private Sub MenuItem_Cut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles m_menuitem_Cut.Click
      m_editorTextBox.Cut()
   End Sub

   Private Sub MenuItem_Copy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles m_menuitem_Copy.Click
      m_editorTextBox.Copy()
   End Sub

   Private Sub MenuItem_Paste_Click(ByVal sender As Object, ByVal e As EventArgs) Handles m_menuitem_Paste.Click
      m_editorTextBox.Paste()
   End Sub


   Private Sub MenuItem_AddReplacement_Click(ByVal sender As Object, ByVal e As EventArgs) Handles m_menuItem_AddReplacement.Click
      AddSelectedTextAsReplacement()
   End Sub



   Private Sub m_editorTextBox_ColoringCompleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_editorTextBox.ColoringCompleted
      HighLightReplacements(Me.m_editorTextBox)
   End Sub

#End Region ' Event Handlers


   Public Sub RefreshEditor()
      PrettyEditor()
   End Sub




   ''' <summary>
   ''' This method highlights replacements, colorizes the code and underlines the current replacement in the code editor text box
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub PrettyEditor()
      m_editorTextBox.ClearAllHighLighting()
      HighLightReplacements(m_editorTextBox)
   End Sub '


   Private Function AddDelims(ByVal value As String) As String
      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter
      Return delim + value + delim
   End Function




   ''' <summary>
   ''' searches for ids of replacements in the specified rich text box and highlights them accordingly
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub HighLightReplacements(ByVal rtb As CodeEditor)
      If _snippetFile Is Nothing OrElse _snippetFile.CodeSnippet Is Nothing Then Return 'early exit
      Dim current As Replacement = Me.GetCurrentReplacement
      Dim words As New List(Of String)
      Dim colors As New List(Of Drawing.Color)
      Dim strId As String

      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter

      For Each rep As Replacement In _snippetFile.CodeSnippet.Snippet.Declarations.Replacements
         strId = rep.Id
         If strId = Nothing Then Continue For

         words.Add(strId & delim)
         If rep Is current Then
            colors.Add(Color.Coral)
         Else
            colors.Add(Color.Yellow)
         End If
      Next


      Dim idx As Int32 = 0
      Dim j As Int32 = 0
      Dim txt As String = rtb.TextWithoutCR
      Dim txtLength As Int32 = txt.Length

      idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)

      While idx >= 0
         j = 0
         idx += 1
         For Each word As String In words
            If String.CompareOrdinal(txt, idx, word, 0, word.Length) = 0 Then
               rtb.HighightRange(idx - 1, idx + word.Length, colors(j))
               idx += word.Length
               Exit For
            End If
            j += 1
         Next

         If idx >= txtLength Then Exit While
         idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)
      End While

   End Sub



   ''' <summary>
   '''substitutes replacement ids with their default value in a RichTextBox.
   ''' </summary>
   ''' <param name="rtb"></param>
   ''' <remarks></remarks>
   Private Sub HighlightReplacementsAndSubstituteWithDefaults(ByVal rtb As CodeEditor)

      If _snippetFile Is Nothing OrElse _snippetFile.CodeSnippet Is Nothing Then Return 'early exit

      Dim delim As String = _snippetFile.CodeSnippet.Snippet.Code.Delimiter

      Dim words As New List(Of String)
      Dim defaults As New List(Of String)
      Dim strId As String
      Dim newValue As String


      For Each rep As Replacement In _snippetFile.CodeSnippet.Snippet.Declarations.Replacements
         strId = rep.Id
         If strId = Nothing Then Continue For
         words.Add(strId & delim)
         newValue = rep.Default
         If newValue = Nothing Then newValue = " "
         defaults.Add(newValue)
      Next



      Dim j, idx, pos, offset As Int32
      Dim txt As String = rtb.TextWithoutCR
      Dim txtLength As Int32 = txt.Length
      Dim wrdLength As Int32 = 0
      Dim rng As ITextRange
      Dim highlightColor As Int32 = CodeEditor.ColorToRGB(Color.MediumAquamarine)

      idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)

      While idx >= 0
         j = 0
         idx += 1
         For Each word As String In words
            wrdLength = word.Length
            If String.CompareOrdinal(txt, idx, word, 0, wrdLength) = 0 Then
               pos = idx + offset - 1
               rng = rtb.GetRange(pos, pos + wrdLength + 1)
               rng.Text = defaults(j)
               rng.Font.BackColor = highlightColor
               offset += defaults(j).Length - wrdLength - 1
               idx += word.Length
               Exit For
            End If
            j += 1
         Next

         If idx >= txtLength Then Exit While

         idx = txt.IndexOf(delim, idx, StringComparison.Ordinal)
      End While

   End Sub


#End Region





#Region "validation"




   Private Sub ReferencesDataGridView_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_referencesDataGridView.Validating
      'remove empty rows from gridview looping through from the end to the start.
      ' skip the very end row as it is uncommitted. (hence Count -2 is the row we start with)
      With m_referencesDataGridView.Rows
         For i As Int32 = .Count - 2 To 0 Step -1
            Dim cell As DataGridViewCell = .Item(i).Cells(0)
            If cell.Value Is Nothing OrElse cell.Value.ToString.Trim.Length = 0 Then
               .RemoveAt(i)
            End If
         Next
      End With
   End Sub

   Private Sub ImportsDataGridView_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_importsDataGridView.Validating
      'remove empty rows from gridview looping through from the end to the start.
      ' skip the very end row as it is uncommitted. (hence Count -2 is the row we start with)
      With m_importsDataGridView.Rows
         For i As Int32 = .Count - 2 To 0 Step -1
            Dim cell As DataGridViewCell = .Item(i).Cells(0)
            If cell.Value Is Nothing OrElse cell.Value.ToString.Trim.Length = 0 Then
               .RemoveAt(i)
            End If
         Next
      End With
   End Sub


   Private Sub m_referencesDataGridView_RowsChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_referencesDataGridView.RowsAdded, m_referencesDataGridView.RowsRemoved
      With m_referencesDataGridView
         Dim count As Int32 = .Rows.Count
         .Height = (count + 2) * .RowTemplate.Height
         ExPanel3.Text = If(count > 1, "References (" & count - 1 & ")", "References")
      End With
   End Sub

   Private Sub m_importsDataGridView_RowsChanged(ByVal sender As Object, ByVal e As EventArgs) Handles m_importsDataGridView.RowsAdded, m_importsDataGridView.RowsRemoved
      With m_importsDataGridView
         Dim count As Int32 = .Rows.Count
         .Height = (count + 2) * .RowTemplate.Height
         ExPanel4.Text = If(count > 1, "Imports (" & count - 1 & ")", "Imports")
      End With
   End Sub

#End Region



End Class








