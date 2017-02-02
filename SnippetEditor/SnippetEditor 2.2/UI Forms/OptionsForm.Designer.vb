'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On




<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class OptionsForm
   Inherits System.Windows.Forms.Form

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
      Dim lblTabSize As System.Windows.Forms.Label
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsForm))
      Dim lblAssociate As System.Windows.Forms.Label
      Dim lblToolbarDisplayStyle As System.Windows.Forms.Label
      Dim lblEditorfont As System.Windows.Forms.Label
      Dim lblNodeDisplayStyle As System.Windows.Forms.Label
      Me.TabControl1 = New System.Windows.Forms.TabControl
      Me.SettingsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.tab_Appearance = New System.Windows.Forms.TabPage
      Me.NodeDisplayStyleComboBox = New System.Windows.Forms.ComboBox
      Me.Toolbar_DisplayStyleComboBox = New System.Windows.Forms.ComboBox
      Me.tab_TextEditor = New System.Windows.Forms.TabPage
      Me.lblEditorfontDisplay = New System.Windows.Forms.Label
      Me.numTabSize = New System.Windows.Forms.NumericUpDown
      Me.btnPickFont = New System.Windows.Forms.Button
      Me.tab_FileAssociations = New System.Windows.Forms.TabPage
      Me.chk_VSSnippet = New System.Windows.Forms.CheckBox
      Me.chk_VBSnippet = New System.Windows.Forms.CheckBox
      Me.chk_Snippet = New System.Windows.Forms.CheckBox
      Me.btn_Okay = New System.Windows.Forms.Button
      Me.btn_Cancel = New System.Windows.Forms.Button
      lblTabSize = New System.Windows.Forms.Label
      lblAssociate = New System.Windows.Forms.Label
      lblToolbarDisplayStyle = New System.Windows.Forms.Label
      lblEditorfont = New System.Windows.Forms.Label
      lblNodeDisplayStyle = New System.Windows.Forms.Label
      Me.TabControl1.SuspendLayout()
      CType(Me.SettingsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.tab_Appearance.SuspendLayout()
      Me.tab_TextEditor.SuspendLayout()
      CType(Me.numTabSize, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.tab_FileAssociations.SuspendLayout()
      Me.SuspendLayout()
      '
      'lblTabSize
      '
      resources.ApplyResources(lblTabSize, "lblTabSize")
      lblTabSize.Name = "lblTabSize"
      '
      'lblAssociate
      '
      resources.ApplyResources(lblAssociate, "lblAssociate")
      lblAssociate.Name = "lblAssociate"
      '
      'lblToolbarDisplayStyle
      '
      resources.ApplyResources(lblToolbarDisplayStyle, "lblToolbarDisplayStyle")
      lblToolbarDisplayStyle.Name = "lblToolbarDisplayStyle"
      '
      'lblEditorfont
      '
      resources.ApplyResources(lblEditorfont, "lblEditorfont")
      lblEditorfont.Name = "lblEditorfont"
      '
      'lblNodeDisplayStyle
      '
      resources.ApplyResources(lblNodeDisplayStyle, "lblNodeDisplayStyle")
      lblNodeDisplayStyle.Name = "lblNodeDisplayStyle"
      '
      'TabControl1
      '
      Me.TabControl1.Controls.Add(Me.tab_Appearance)
      Me.TabControl1.Controls.Add(Me.tab_TextEditor)
      Me.TabControl1.Controls.Add(Me.tab_FileAssociations)
      resources.ApplyResources(Me.TabControl1, "TabControl1")
      Me.TabControl1.Name = "TabControl1"
      Me.TabControl1.SelectedIndex = 0
      '
      'SettingsBindingSource
      '
      Me.SettingsBindingSource.DataSource = GetType(SnippetEditor.My.MySettings)
      '
      'tab_Appearance
      '
      Me.tab_Appearance.Controls.Add(lblNodeDisplayStyle)
      Me.tab_Appearance.Controls.Add(Me.NodeDisplayStyleComboBox)
      Me.tab_Appearance.Controls.Add(lblToolbarDisplayStyle)
      Me.tab_Appearance.Controls.Add(Me.Toolbar_DisplayStyleComboBox)
      resources.ApplyResources(Me.tab_Appearance, "tab_Appearance")
      Me.tab_Appearance.Name = "tab_Appearance"
      Me.tab_Appearance.UseVisualStyleBackColor = True
      '
      'NodeDisplayStyleComboBox
      '
      Me.NodeDisplayStyleComboBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.SettingsBindingSource, "NodeDisplayStyle", True))
      Me.NodeDisplayStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.NodeDisplayStyleComboBox.FormattingEnabled = True
      Me.NodeDisplayStyleComboBox.Items.AddRange(New Object() {resources.GetString("NodeDisplayStyleComboBox.Items"), resources.GetString("NodeDisplayStyleComboBox.Items1")})
      resources.ApplyResources(Me.NodeDisplayStyleComboBox, "NodeDisplayStyleComboBox")
      Me.NodeDisplayStyleComboBox.Name = "NodeDisplayStyleComboBox"
      '
      'Toolbar_DisplayStyleComboBox
      '
      Me.Toolbar_DisplayStyleComboBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.SettingsBindingSource, "Toolbar_DisplayStyle", True))
      Me.Toolbar_DisplayStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.Toolbar_DisplayStyleComboBox.FormattingEnabled = True
      Me.Toolbar_DisplayStyleComboBox.Items.AddRange(New Object() {resources.GetString("Toolbar_DisplayStyleComboBox.Items"), resources.GetString("Toolbar_DisplayStyleComboBox.Items1"), resources.GetString("Toolbar_DisplayStyleComboBox.Items2")})
      resources.ApplyResources(Me.Toolbar_DisplayStyleComboBox, "Toolbar_DisplayStyleComboBox")
      Me.Toolbar_DisplayStyleComboBox.Name = "Toolbar_DisplayStyleComboBox"
      '
      'tab_TextEditor
      '
      Me.tab_TextEditor.Controls.Add(lblEditorfont)
      Me.tab_TextEditor.Controls.Add(Me.lblEditorfontDisplay)
      Me.tab_TextEditor.Controls.Add(lblTabSize)
      Me.tab_TextEditor.Controls.Add(Me.numTabSize)
      Me.tab_TextEditor.Controls.Add(Me.btnPickFont)
      resources.ApplyResources(Me.tab_TextEditor, "tab_TextEditor")
      Me.tab_TextEditor.Name = "tab_TextEditor"
      Me.tab_TextEditor.UseVisualStyleBackColor = True
      '
      'lblEditorfontDisplay
      '
      Me.lblEditorfontDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
      Me.lblEditorfontDisplay.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.SettingsBindingSource, "EditorFont", True))
      Me.lblEditorfontDisplay.DataBindings.Add(New System.Windows.Forms.Binding("Font", Me.SettingsBindingSource, "EditorFont", True))
      resources.ApplyResources(Me.lblEditorfontDisplay, "lblEditorfontDisplay")
      Me.lblEditorfontDisplay.Name = "lblEditorfontDisplay"
      '
      'numTabSize
      '
      Me.numTabSize.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.SettingsBindingSource, "TabSize", True))
      resources.ApplyResources(Me.numTabSize, "numTabSize")
      Me.numTabSize.Maximum = New Decimal(New Integer() {40, 0, 0, 0})
      Me.numTabSize.Name = "numTabSize"
      Me.numTabSize.Value = New Decimal(New Integer() {4, 0, 0, 0})
      '
      'btnPickFont
      '
      resources.ApplyResources(Me.btnPickFont, "btnPickFont")
      Me.btnPickFont.Name = "btnPickFont"
      '
      'tab_FileAssociations
      '
      Me.tab_FileAssociations.Controls.Add(Me.chk_VSSnippet)
      Me.tab_FileAssociations.Controls.Add(Me.chk_VBSnippet)
      Me.tab_FileAssociations.Controls.Add(Me.chk_Snippet)
      Me.tab_FileAssociations.Controls.Add(lblAssociate)
      resources.ApplyResources(Me.tab_FileAssociations, "tab_FileAssociations")
      Me.tab_FileAssociations.Name = "tab_FileAssociations"
      Me.tab_FileAssociations.UseVisualStyleBackColor = True
      '
      'chk_VSSnippet
      '
      resources.ApplyResources(Me.chk_VSSnippet, "chk_VSSnippet")
      Me.chk_VSSnippet.Name = "chk_VSSnippet"
      '
      'chk_VBSnippet
      '
      resources.ApplyResources(Me.chk_VBSnippet, "chk_VBSnippet")
      Me.chk_VBSnippet.Name = "chk_VBSnippet"
      '
      'chk_Snippet
      '
      resources.ApplyResources(Me.chk_Snippet, "chk_Snippet")
      Me.chk_Snippet.Name = "chk_Snippet"
      '
      'btn_Okay
      '
      resources.ApplyResources(Me.btn_Okay, "btn_Okay")
      Me.btn_Okay.Name = "btn_Okay"
      '
      'btn_Cancel
      '
      resources.ApplyResources(Me.btn_Cancel, "btn_Cancel")
      Me.btn_Cancel.Name = "btn_Cancel"
      '
      'OptionsForm
      '
      resources.ApplyResources(Me, "$this")
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.btn_Cancel)
      Me.Controls.Add(Me.btn_Okay)
      Me.Controls.Add(Me.TabControl1)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "OptionsForm"
      Me.ShowIcon = False
      Me.ShowInTaskbar = False
      Me.TabControl1.ResumeLayout(False)
      CType(Me.SettingsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.tab_Appearance.ResumeLayout(False)
      Me.tab_Appearance.PerformLayout()
      Me.tab_TextEditor.ResumeLayout(False)
      Me.tab_TextEditor.PerformLayout()
      CType(Me.numTabSize, System.ComponentModel.ISupportInitialize).EndInit()
      Me.tab_FileAssociations.ResumeLayout(False)
      Me.tab_FileAssociations.PerformLayout()
      Me.ResumeLayout(False)

   End Sub
   Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
   Friend WithEvents tab_TextEditor As System.Windows.Forms.TabPage
   Friend WithEvents tab_FileAssociations As System.Windows.Forms.TabPage
   Friend WithEvents btn_Okay As System.Windows.Forms.Button
   Friend WithEvents btn_Cancel As System.Windows.Forms.Button
   Friend WithEvents btnPickFont As System.Windows.Forms.Button
   Friend WithEvents numTabSize As System.Windows.Forms.NumericUpDown
   Friend WithEvents chk_VSSnippet As System.Windows.Forms.CheckBox
   Friend WithEvents chk_VBSnippet As System.Windows.Forms.CheckBox
   Friend WithEvents chk_Snippet As System.Windows.Forms.CheckBox
   Friend WithEvents SettingsBindingSource As System.Windows.Forms.BindingSource
   Friend WithEvents tab_Appearance As System.Windows.Forms.TabPage
   Friend WithEvents Toolbar_DisplayStyleComboBox As System.Windows.Forms.ComboBox
   Friend WithEvents lblEditorfontDisplay As System.Windows.Forms.Label
   Friend WithEvents NodeDisplayStyleComboBox As System.Windows.Forms.ComboBox

End Class
