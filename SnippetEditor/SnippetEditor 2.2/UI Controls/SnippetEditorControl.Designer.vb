'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic


Partial Friend Class SnippetEditorControl
	Inherits System.Windows.Forms.UserControl

	<System.Diagnostics.DebuggerNonUserCode()> _
	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

	End Sub

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso components IsNot Nothing Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnippetEditorControl))
      Dim EditableLabel1 As System.Windows.Forms.Label
      Dim FunctionLabel1 As System.Windows.Forms.Label
      Dim labelReplacementKind As System.Windows.Forms.Label
      Dim labelScope As System.Windows.Forms.Label
      Dim labelLanguage As System.Windows.Forms.Label
      Dim labelDescription As System.Windows.Forms.Label
      Dim labelShortcut As System.Windows.Forms.Label
      Dim labelHelpURL As System.Windows.Forms.Label
      Dim labelAuthor As System.Windows.Forms.Label
      Dim labelTitle As System.Windows.Forms.Label
      Dim labeltype As System.Windows.Forms.Label
      Dim labelID As System.Windows.Forms.Label
      Dim labelDefaultsTo As System.Windows.Forms.Label
      Dim labelTooltip As System.Windows.Forms.Label
      Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
      Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
      Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
      Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
      Me.m_editorContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
      Me.m_menuItem_AddReplacement = New System.Windows.Forms.ToolStripMenuItem
      Me.m_menuitem_Cut = New System.Windows.Forms.ToolStripMenuItem
      Me.m_menuitem_Copy = New System.Windows.Forms.ToolStripMenuItem
      Me.m_menuitem_Paste = New System.Windows.Forms.ToolStripMenuItem
      Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
      Me.m_headerBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.ErrorProvider2 = New System.Windows.Forms.ErrorProvider(Me.components)
      Me.m_replacementsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.ExPanel4 = New SnippetEditor.ExpandablePanel
      Me.m_importsDataGridView = New System.Windows.Forms.DataGridView
      Me._Namespace = New System.Windows.Forms.DataGridViewTextBoxColumn
      Me.m_importsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.ExPanel3 = New SnippetEditor.ExpandablePanel
      Me.m_referencesDataGridView = New System.Windows.Forms.DataGridView
      Me.m_datagridviewcolumn_Assembly = New System.Windows.Forms.DataGridViewTextBoxColumn
      Me.m_datagridviewcolumn_Url = New System.Windows.Forms.DataGridViewTextBoxColumn
      Me.m_referencesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.ExPanel2 = New SnippetEditor.ExpandablePanel
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.EditableCheckBox1 = New System.Windows.Forms.CheckBox
      Me.FunctionTextBox1 = New System.Windows.Forms.TextBox
      Me.m_litvsobjComboBox = New System.Windows.Forms.ComboBox
      Me.m_defaultField = New System.Windows.Forms.TextBox
      Me.m_typeField = New System.Windows.Forms.TextBox
      Me.m_idField = New System.Windows.Forms.TextBox
      Me.m_tooltipField = New System.Windows.Forms.TextBox
      Me.replacementBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
      Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
      Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
      Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
      Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
      Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
      Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
      Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
      Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
      Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
      Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton
      Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton
      Me.Splitter1 = New System.Windows.Forms.Splitter
      Me.m_editorTextBox = New SnippetEditor.CodeEditor
      Me.m_codeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.m_kindComboBox = New System.Windows.Forms.ComboBox
      Me.m_languageComboBox = New System.Windows.Forms.ComboBox
      Me.m_shortcutTextBox = New System.Windows.Forms.TextBox
      Me.m_helpUrlTextBox = New System.Windows.Forms.TextBox
      Me.m_descriptionTextBox = New System.Windows.Forms.TextBox
      Me.m_authorTextBox = New System.Windows.Forms.TextBox
      Me.m_titleTextBox = New System.Windows.Forms.TextBox
      Me.ExPanel1 = New SnippetEditor.ExpandablePanel
      ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
      EditableLabel1 = New System.Windows.Forms.Label
      FunctionLabel1 = New System.Windows.Forms.Label
      labelReplacementKind = New System.Windows.Forms.Label
      labelScope = New System.Windows.Forms.Label
      labelLanguage = New System.Windows.Forms.Label
      labelDescription = New System.Windows.Forms.Label
      labelShortcut = New System.Windows.Forms.Label
      labelHelpURL = New System.Windows.Forms.Label
      labelAuthor = New System.Windows.Forms.Label
      labelTitle = New System.Windows.Forms.Label
      labeltype = New System.Windows.Forms.Label
      labelID = New System.Windows.Forms.Label
      labelDefaultsTo = New System.Windows.Forms.Label
      labelTooltip = New System.Windows.Forms.Label
      Me.m_editorContextMenuStrip.SuspendLayout()
      CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.m_headerBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.ErrorProvider2, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.m_replacementsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.ExPanel4.SuspendLayout()
      CType(Me.m_importsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.m_importsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.ExPanel3.SuspendLayout()
      CType(Me.m_referencesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.m_referencesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.ExPanel2.SuspendLayout()
      Me.Panel1.SuspendLayout()
      CType(Me.replacementBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.replacementBindingNavigator.SuspendLayout()
      CType(Me.m_codeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.ExPanel1.SuspendLayout()
      Me.SuspendLayout()
      '
      'ToolStripSeparator3
      '
      ToolStripSeparator3.Name = "ToolStripSeparator3"
      resources.ApplyResources(ToolStripSeparator3, "ToolStripSeparator3")
      '
      'EditableLabel1
      '
      resources.ApplyResources(EditableLabel1, "EditableLabel1")
      EditableLabel1.Name = "EditableLabel1"
      '
      'FunctionLabel1
      '
      resources.ApplyResources(FunctionLabel1, "FunctionLabel1")
      FunctionLabel1.Name = "FunctionLabel1"
      '
      'labelReplacementKind
      '
      resources.ApplyResources(labelReplacementKind, "labelReplacementKind")
      labelReplacementKind.Name = "labelReplacementKind"
      '
      'labelScope
      '
      resources.ApplyResources(labelScope, "labelScope")
      labelScope.Name = "labelScope"
      '
      'labelLanguage
      '
      resources.ApplyResources(labelLanguage, "labelLanguage")
      labelLanguage.Name = "labelLanguage"
      '
      'labelDescription
      '
      resources.ApplyResources(labelDescription, "labelDescription")
      labelDescription.Name = "labelDescription"
      '
      'labelShortcut
      '
      resources.ApplyResources(labelShortcut, "labelShortcut")
      labelShortcut.Name = "labelShortcut"
      '
      'labelHelpURL
      '
      resources.ApplyResources(labelHelpURL, "labelHelpURL")
      labelHelpURL.Name = "labelHelpURL"
      '
      'labelAuthor
      '
      resources.ApplyResources(labelAuthor, "labelAuthor")
      labelAuthor.Name = "labelAuthor"
      '
      'labelTitle
      '
      resources.ApplyResources(labelTitle, "labelTitle")
      labelTitle.Name = "labelTitle"
      '
      'labeltype
      '
      resources.ApplyResources(labeltype, "labeltype")
      labeltype.Name = "labeltype"
      '
      'labelID
      '
      resources.ApplyResources(labelID, "labelID")
      labelID.Name = "labelID"
      '
      'labelDefaultsTo
      '
      resources.ApplyResources(labelDefaultsTo, "labelDefaultsTo")
      labelDefaultsTo.Name = "labelDefaultsTo"
      '
      'labelTooltip
      '
      resources.ApplyResources(labelTooltip, "labelTooltip")
      labelTooltip.Name = "labelTooltip"
      '
      'm_editorContextMenuStrip
      '
      Me.m_editorContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.m_menuItem_AddReplacement, ToolStripSeparator3, Me.m_menuitem_Cut, Me.m_menuitem_Copy, Me.m_menuitem_Paste})
      Me.m_editorContextMenuStrip.Name = "ContextMenuStrip1"
      resources.ApplyResources(Me.m_editorContextMenuStrip, "m_editorContextMenuStrip")
      '
      'm_menuItem_AddReplacement
      '
      Me.m_menuItem_AddReplacement.Name = "m_menuItem_AddReplacement"
      resources.ApplyResources(Me.m_menuItem_AddReplacement, "m_menuItem_AddReplacement")
      '
      'm_menuitem_Cut
      '
      Me.m_menuitem_Cut.Name = "m_menuitem_Cut"
      resources.ApplyResources(Me.m_menuitem_Cut, "m_menuitem_Cut")
      '
      'm_menuitem_Copy
      '
      Me.m_menuitem_Copy.Name = "m_menuitem_Copy"
      resources.ApplyResources(Me.m_menuitem_Copy, "m_menuitem_Copy")
      '
      'm_menuitem_Paste
      '
      Me.m_menuitem_Paste.Name = "m_menuitem_Paste"
      resources.ApplyResources(Me.m_menuitem_Paste, "m_menuitem_Paste")
      '
      'ErrorProvider1
      '
      Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
      Me.ErrorProvider1.ContainerControl = Me
      Me.ErrorProvider1.DataSource = Me.m_headerBindingSource
      '
      'm_headerBindingSource
      '
      Me.m_headerBindingSource.DataSource = GetType(SnippetEditor.SnippetRepresentation.Header)
      '
      'ErrorProvider2
      '
      Me.ErrorProvider2.ContainerControl = Me
      Me.ErrorProvider2.DataSource = Me.m_replacementsBindingSource
      '
      'm_replacementsBindingSource
      '
      Me.m_replacementsBindingSource.AllowNew = True
      Me.m_replacementsBindingSource.DataSource = GetType(SnippetEditor.SnippetRepresentation.Replacement)
      '
      'ExPanel4
      '
      resources.ApplyResources(Me.ExPanel4, "ExPanel4")
      Me.ExPanel4.Collapsed = False
      Me.ExPanel4.Controls.Add(Me.m_importsDataGridView)
      Me.ExPanel4.ForeColor = System.Drawing.SystemColors.WindowText
      Me.ExPanel4.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
      Me.ExPanel4.MinimumSize = New System.Drawing.Size(600, 0)
      Me.ExPanel4.Name = "ExPanel4"
      Me.ExPanel4.TitleBackColor1 = System.Drawing.Color.Gray
      Me.ExPanel4.TitleBackColor2 = System.Drawing.Color.White
      Me.ExPanel4.TitleFont = New System.Drawing.Font("Verdana", 10.0!)
      Me.ExPanel4.TitleForeColor = System.Drawing.Color.White
      Me.ExPanel4.TitleHeight = 23
      '
      'm_importsDataGridView
      '
      Me.m_importsDataGridView.AutoGenerateColumns = False
      Me.m_importsDataGridView.BackgroundColor = System.Drawing.SystemColors.Control
      Me.m_importsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.m_importsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
      DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
      DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro
      DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(119, Byte), Integer))
      DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
      DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
      DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
      Me.m_importsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
      resources.ApplyResources(Me.m_importsDataGridView, "m_importsDataGridView")
      Me.m_importsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me._Namespace})
      Me.m_importsDataGridView.DataSource = Me.m_importsBindingSource
      DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
      DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
      DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
      DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window
      DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText
      DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
      Me.m_importsDataGridView.DefaultCellStyle = DataGridViewCellStyle2
      Me.m_importsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
      Me.m_importsDataGridView.Name = "m_importsDataGridView"
      Me.m_importsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
      Me.m_importsDataGridView.RowHeadersVisible = False
      Me.m_importsDataGridView.RowTemplate.Height = 20
      Me.m_importsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
      '
      '_Namespace
      '
      Me._Namespace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
      Me._Namespace.DataPropertyName = "Namespace"
      resources.ApplyResources(Me._Namespace, "_Namespace")
      Me._Namespace.Name = "_Namespace"
      '
      'm_importsBindingSource
      '
      Me.m_importsBindingSource.DataSource = GetType(SnippetEditor.SnippetRepresentation.Import)
      '
      'ExPanel3
      '
      resources.ApplyResources(Me.ExPanel3, "ExPanel3")
      Me.ExPanel3.Collapsed = False
      Me.ExPanel3.Controls.Add(Me.m_referencesDataGridView)
      Me.ExPanel3.ForeColor = System.Drawing.SystemColors.WindowText
      Me.ExPanel3.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
      Me.ExPanel3.MinimumSize = New System.Drawing.Size(600, 0)
      Me.ExPanel3.Name = "ExPanel3"
      Me.ExPanel3.TitleBackColor1 = System.Drawing.Color.Gray
      Me.ExPanel3.TitleBackColor2 = System.Drawing.Color.White
      Me.ExPanel3.TitleFont = New System.Drawing.Font("Verdana", 10.0!)
      Me.ExPanel3.TitleForeColor = System.Drawing.Color.White
      Me.ExPanel3.TitleHeight = 23
      '
      'm_referencesDataGridView
      '
      Me.m_referencesDataGridView.AutoGenerateColumns = False
      Me.m_referencesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
      Me.m_referencesDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
      Me.m_referencesDataGridView.BackgroundColor = System.Drawing.SystemColors.Control
      Me.m_referencesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
      DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
      DataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro
      DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(119, Byte), Integer))
      DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window
      DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(119, Byte), Integer))
      DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
      Me.m_referencesDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
      resources.ApplyResources(Me.m_referencesDataGridView, "m_referencesDataGridView")
      Me.m_referencesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.m_datagridviewcolumn_Assembly, Me.m_datagridviewcolumn_Url})
      Me.m_referencesDataGridView.DataSource = Me.m_referencesBindingSource
      DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
      DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
      DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
      DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window
      DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText
      DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
      Me.m_referencesDataGridView.DefaultCellStyle = DataGridViewCellStyle4
      Me.m_referencesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
      Me.m_referencesDataGridView.Name = "m_referencesDataGridView"
      Me.m_referencesDataGridView.RowHeadersVisible = False
      Me.m_referencesDataGridView.RowTemplate.Height = 20
      Me.m_referencesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
      '
      'm_datagridviewcolumn_Assembly
      '
      Me.m_datagridviewcolumn_Assembly.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
      Me.m_datagridviewcolumn_Assembly.DataPropertyName = "Assembly"
      resources.ApplyResources(Me.m_datagridviewcolumn_Assembly, "m_datagridviewcolumn_Assembly")
      Me.m_datagridviewcolumn_Assembly.Name = "m_datagridviewcolumn_Assembly"
      '
      'm_datagridviewcolumn_Url
      '
      Me.m_datagridviewcolumn_Url.DataPropertyName = "Url"
      resources.ApplyResources(Me.m_datagridviewcolumn_Url, "m_datagridviewcolumn_Url")
      Me.m_datagridviewcolumn_Url.Name = "m_datagridviewcolumn_Url"
      '
      'm_referencesBindingSource
      '
      Me.m_referencesBindingSource.DataSource = GetType(SnippetEditor.SnippetRepresentation.Reference)
      '
      'ExPanel2
      '
      resources.ApplyResources(Me.ExPanel2, "ExPanel2")
      Me.ExPanel2.Collapsed = False
      Me.ExPanel2.Controls.Add(Me.Panel1)
      Me.ExPanel2.Controls.Add(Me.Splitter1)
      Me.ExPanel2.Controls.Add(Me.m_editorTextBox)
      Me.ExPanel2.ForeColor = System.Drawing.Color.SteelBlue
      Me.ExPanel2.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
      Me.ExPanel2.MinimumSize = New System.Drawing.Size(600, 0)
      Me.ExPanel2.Name = "ExPanel2"
      Me.ExPanel2.TitleBackColor1 = System.Drawing.Color.Gray
      Me.ExPanel2.TitleBackColor2 = System.Drawing.Color.White
      Me.ExPanel2.TitleFont = New System.Drawing.Font("Verdana", 10.0!)
      Me.ExPanel2.TitleForeColor = System.Drawing.Color.White
      Me.ExPanel2.TitleHeight = 23
      '
      'Panel1
      '
      resources.ApplyResources(Me.Panel1, "Panel1")
      Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.Panel1.Controls.Add(EditableLabel1)
      Me.Panel1.Controls.Add(Me.EditableCheckBox1)
      Me.Panel1.Controls.Add(FunctionLabel1)
      Me.Panel1.Controls.Add(Me.FunctionTextBox1)
      Me.Panel1.Controls.Add(labelReplacementKind)
      Me.Panel1.Controls.Add(Me.m_litvsobjComboBox)
      Me.Panel1.Controls.Add(Me.m_defaultField)
      Me.Panel1.Controls.Add(Me.m_typeField)
      Me.Panel1.Controls.Add(Me.m_idField)
      Me.Panel1.Controls.Add(Me.m_tooltipField)
      Me.Panel1.Controls.Add(labeltype)
      Me.Panel1.Controls.Add(labelID)
      Me.Panel1.Controls.Add(labelDefaultsTo)
      Me.Panel1.Controls.Add(labelTooltip)
      Me.Panel1.Controls.Add(Me.replacementBindingNavigator)
      Me.Panel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(119, Byte), Integer))
      Me.Panel1.Name = "Panel1"
      '
      'EditableCheckBox1
      '
      resources.ApplyResources(Me.EditableCheckBox1, "EditableCheckBox1")
      Me.EditableCheckBox1.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.m_replacementsBindingSource, "Editable", True))
      Me.EditableCheckBox1.Name = "EditableCheckBox1"
      '
      'FunctionTextBox1
      '
      resources.ApplyResources(Me.FunctionTextBox1, "FunctionTextBox1")
      Me.FunctionTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.FunctionTextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "Function", True))
      Me.FunctionTextBox1.Name = "FunctionTextBox1"
      '
      'm_litvsobjComboBox
      '
      Me.m_litvsobjComboBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "ReplacementKind", True))
      Me.m_litvsobjComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      resources.ApplyResources(Me.m_litvsobjComboBox, "m_litvsobjComboBox")
      Me.m_litvsobjComboBox.FormattingEnabled = True
      Me.m_litvsobjComboBox.Items.AddRange(New Object() {resources.GetString("m_litvsobjComboBox.Items"), resources.GetString("m_litvsobjComboBox.Items1")})
      Me.m_litvsobjComboBox.Name = "m_litvsobjComboBox"
      '
      'm_defaultField
      '
      resources.ApplyResources(Me.m_defaultField, "m_defaultField")
      Me.m_defaultField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_defaultField.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "Default", True))
      Me.m_defaultField.Name = "m_defaultField"
      '
      'm_typeField
      '
      resources.ApplyResources(Me.m_typeField, "m_typeField")
      Me.m_typeField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_typeField.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "Type", True))
      Me.m_typeField.Name = "m_typeField"
      '
      'm_idField
      '
      Me.m_idField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_idField.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "Id", True))
      resources.ApplyResources(Me.m_idField, "m_idField")
      Me.m_idField.Name = "m_idField"
      '
      'm_tooltipField
      '
      resources.ApplyResources(Me.m_tooltipField, "m_tooltipField")
      Me.m_tooltipField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_tooltipField.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_replacementsBindingSource, "Tooltip", True))
      Me.m_tooltipField.Name = "m_tooltipField"
      '
      'replacementBindingNavigator
      '
      Me.replacementBindingNavigator.AddNewItem = Nothing
      Me.replacementBindingNavigator.BindingSource = Me.m_replacementsBindingSource
      Me.replacementBindingNavigator.CountItem = Me.BindingNavigatorCountItem
      Me.replacementBindingNavigator.DeleteItem = Nothing
      Me.replacementBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem})
      resources.ApplyResources(Me.replacementBindingNavigator, "replacementBindingNavigator")
      Me.replacementBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
      Me.replacementBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
      Me.replacementBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
      Me.replacementBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
      Me.replacementBindingNavigator.Name = "replacementBindingNavigator"
      Me.replacementBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
      '
      'BindingNavigatorCountItem
      '
      Me.BindingNavigatorCountItem.ForeColor = System.Drawing.SystemColors.WindowText
      Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
      resources.ApplyResources(Me.BindingNavigatorCountItem, "BindingNavigatorCountItem")
      '
      'BindingNavigatorMoveFirstItem
      '
      Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorMoveFirstItem, "BindingNavigatorMoveFirstItem")
      Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
      '
      'BindingNavigatorMovePreviousItem
      '
      Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorMovePreviousItem, "BindingNavigatorMovePreviousItem")
      Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
      '
      'BindingNavigatorSeparator
      '
      Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
      resources.ApplyResources(Me.BindingNavigatorSeparator, "BindingNavigatorSeparator")
      '
      'BindingNavigatorPositionItem
      '
      resources.ApplyResources(Me.BindingNavigatorPositionItem, "BindingNavigatorPositionItem")
      Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
      '
      'BindingNavigatorSeparator1
      '
      Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
      resources.ApplyResources(Me.BindingNavigatorSeparator1, "BindingNavigatorSeparator1")
      '
      'BindingNavigatorMoveNextItem
      '
      Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorMoveNextItem, "BindingNavigatorMoveNextItem")
      Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
      '
      'BindingNavigatorMoveLastItem
      '
      Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorMoveLastItem, "BindingNavigatorMoveLastItem")
      Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
      '
      'BindingNavigatorSeparator2
      '
      Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
      resources.ApplyResources(Me.BindingNavigatorSeparator2, "BindingNavigatorSeparator2")
      '
      'BindingNavigatorAddNewItem
      '
      Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorAddNewItem, "BindingNavigatorAddNewItem")
      Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
      '
      'BindingNavigatorDeleteItem
      '
      Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      resources.ApplyResources(Me.BindingNavigatorDeleteItem, "BindingNavigatorDeleteItem")
      Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
      '
      'Splitter1
      '
      Me.Splitter1.BackColor = System.Drawing.SystemColors.ControlDark
      resources.ApplyResources(Me.Splitter1, "Splitter1")
      Me.Splitter1.Name = "Splitter1"
      Me.Splitter1.TabStop = False
      '
      'm_editorTextBox
      '
      Me.m_editorTextBox.AcceptsTab = True
      Me.m_editorTextBox.AllowDrop = True
      Me.m_editorTextBox.ContextMenuStrip = Me.m_editorContextMenuStrip
      Me.m_editorTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_codeBindingSource, "Data", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.m_editorTextBox.DataBindings.Add(New System.Windows.Forms.Binding("CodeLanguage", Me.m_codeBindingSource, "Language", True, System.Windows.Forms.DataSourceUpdateMode.Never))
      Me.m_editorTextBox.DetectUrls = False
      resources.ApplyResources(Me.m_editorTextBox, "m_editorTextBox")
      Me.m_editorTextBox.Name = "m_editorTextBox"
      Me.m_editorTextBox.RichTextShortcutsEnabled = False
      '
      'm_codeBindingSource
      '
      Me.m_codeBindingSource.DataSource = GetType(SnippetEditor.SnippetRepresentation.Code)
      '
      'm_kindComboBox
      '
      resources.ApplyResources(Me.m_kindComboBox, "m_kindComboBox")
      Me.m_kindComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.m_kindComboBox.FormattingEnabled = True
      Me.m_kindComboBox.Items.AddRange(New Object() {resources.GetString("m_kindComboBox.Items"), resources.GetString("m_kindComboBox.Items1"), resources.GetString("m_kindComboBox.Items2"), resources.GetString("m_kindComboBox.Items3")})
      Me.m_kindComboBox.Name = "m_kindComboBox"
      '
      'm_languageComboBox
      '
      resources.ApplyResources(Me.m_languageComboBox, "m_languageComboBox")
      Me.m_languageComboBox.FormattingEnabled = True
      Me.m_languageComboBox.Items.AddRange(New Object() {resources.GetString("m_languageComboBox.Items"), resources.GetString("m_languageComboBox.Items1"), resources.GetString("m_languageComboBox.Items2"), resources.GetString("m_languageComboBox.Items3"), resources.GetString("m_languageComboBox.Items4"), resources.GetString("m_languageComboBox.Items5")})
      Me.m_languageComboBox.Name = "m_languageComboBox"
      '
      'm_shortcutTextBox
      '
      resources.ApplyResources(Me.m_shortcutTextBox, "m_shortcutTextBox")
      Me.m_shortcutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_shortcutTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_headerBindingSource, "Shortcut", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.m_shortcutTextBox.Name = "m_shortcutTextBox"
      '
      'm_helpUrlTextBox
      '
      resources.ApplyResources(Me.m_helpUrlTextBox, "m_helpUrlTextBox")
      Me.m_helpUrlTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_helpUrlTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_headerBindingSource, "HelpUrl", True))
      Me.m_helpUrlTextBox.Name = "m_helpUrlTextBox"
      '
      'm_descriptionTextBox
      '
      resources.ApplyResources(Me.m_descriptionTextBox, "m_descriptionTextBox")
      Me.m_descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_descriptionTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_headerBindingSource, "Description", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.m_descriptionTextBox.Name = "m_descriptionTextBox"
      '
      'm_authorTextBox
      '
      resources.ApplyResources(Me.m_authorTextBox, "m_authorTextBox")
      Me.m_authorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_authorTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_headerBindingSource, "Author", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.m_authorTextBox.Name = "m_authorTextBox"
      '
      'm_titleTextBox
      '
      resources.ApplyResources(Me.m_titleTextBox, "m_titleTextBox")
      Me.m_titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.m_titleTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.m_headerBindingSource, "Title", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, ""))
      Me.m_titleTextBox.Name = "m_titleTextBox"
      '
      'ExPanel1
      '
      resources.ApplyResources(Me.ExPanel1, "ExPanel1")
      Me.ExPanel1.Collapsed = False
      Me.ExPanel1.Controls.Add(labelScope)
      Me.ExPanel1.Controls.Add(Me.m_kindComboBox)
      Me.ExPanel1.Controls.Add(Me.m_titleTextBox)
      Me.ExPanel1.Controls.Add(Me.m_authorTextBox)
      Me.ExPanel1.Controls.Add(labelLanguage)
      Me.ExPanel1.Controls.Add(Me.m_languageComboBox)
      Me.ExPanel1.Controls.Add(Me.m_descriptionTextBox)
      Me.ExPanel1.Controls.Add(Me.m_helpUrlTextBox)
      Me.ExPanel1.Controls.Add(Me.m_shortcutTextBox)
      Me.ExPanel1.Controls.Add(labelAuthor)
      Me.ExPanel1.Controls.Add(labelDescription)
      Me.ExPanel1.Controls.Add(labelTitle)
      Me.ExPanel1.Controls.Add(labelShortcut)
      Me.ExPanel1.Controls.Add(labelHelpURL)
      Me.ExPanel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(119, Byte), Integer))
      Me.ExPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
      Me.ExPanel1.MinimumSize = New System.Drawing.Size(600, 0)
      Me.ExPanel1.Name = "ExPanel1"
      Me.ExPanel1.TitleBackColor1 = System.Drawing.Color.Gray
      Me.ExPanel1.TitleBackColor2 = System.Drawing.Color.White
      Me.ExPanel1.TitleFont = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.ExPanel1.TitleForeColor = System.Drawing.Color.White
      Me.ExPanel1.TitleHeight = 23
      '
      'SnippetEditorControl
      '
      resources.ApplyResources(Me, "$this")
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.ExPanel4)
      Me.Controls.Add(Me.ExPanel3)
      Me.Controls.Add(Me.ExPanel2)
      Me.Controls.Add(Me.ExPanel1)
      Me.MinimumSize = New System.Drawing.Size(650, 0)
      Me.Name = "SnippetEditorControl"
      Me.m_editorContextMenuStrip.ResumeLayout(False)
      CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.m_headerBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.ErrorProvider2, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.m_replacementsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ExPanel4.ResumeLayout(False)
      CType(Me.m_importsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.m_importsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ExPanel3.ResumeLayout(False)
      CType(Me.m_referencesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.m_referencesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ExPanel2.ResumeLayout(False)
      Me.ExPanel2.PerformLayout()
      Me.Panel1.ResumeLayout(False)
      Me.Panel1.PerformLayout()
      CType(Me.replacementBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
      Me.replacementBindingNavigator.ResumeLayout(False)
      Me.replacementBindingNavigator.PerformLayout()
      CType(Me.m_codeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ExPanel1.ResumeLayout(False)
      Me.ExPanel1.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents m_editorContextMenuStrip As System.Windows.Forms.ContextMenuStrip
   Friend WithEvents m_menuitem_Copy As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_menuitem_Cut As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_menuitem_Paste As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_menuItem_AddReplacement As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_kindComboBox As System.Windows.Forms.ComboBox
   Friend WithEvents m_languageComboBox As System.Windows.Forms.ComboBox
   Friend WithEvents m_shortcutTextBox As System.Windows.Forms.TextBox
   Friend WithEvents m_helpUrlTextBox As System.Windows.Forms.TextBox
   Friend WithEvents m_descriptionTextBox As System.Windows.Forms.TextBox
   Friend WithEvents m_authorTextBox As System.Windows.Forms.TextBox
   Friend WithEvents m_titleTextBox As System.Windows.Forms.TextBox
   Friend WithEvents m_referencesDataGridView As System.Windows.Forms.DataGridView
   Friend WithEvents m_importsDataGridView As System.Windows.Forms.DataGridView
   Friend WithEvents m_importsBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents m_editorTextBox As SnippetEditor.CodeEditor
   Friend WithEvents m_codeBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents m_headerBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents m_referencesBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents m_replacementsBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents EditableCheckBox1 As System.Windows.Forms.CheckBox
   Friend WithEvents FunctionTextBox1 As System.Windows.Forms.TextBox
   Friend WithEvents m_litvsobjComboBox As System.Windows.Forms.ComboBox
   Friend WithEvents m_defaultField As System.Windows.Forms.TextBox
   Friend WithEvents m_typeField As System.Windows.Forms.TextBox
   Friend WithEvents m_idField As System.Windows.Forms.TextBox
   Friend WithEvents m_tooltipField As System.Windows.Forms.TextBox
   Friend WithEvents replacementBindingNavigator As System.Windows.Forms.BindingNavigator
   Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
   Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
   Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents BindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
   Friend WithEvents m_datagridviewcolumn_Assembly As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents m_datagridviewcolumn_Url As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents _Namespace As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents ExPanel1 As ExpandablePanel
   Friend WithEvents ExPanel2 As ExpandablePanel
   Friend WithEvents ExPanel3 As ExpandablePanel
   Friend WithEvents ExPanel4 As ExpandablePanel
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
   Private WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
   Friend WithEvents ErrorProvider2 As System.Windows.Forms.ErrorProvider

End Class
