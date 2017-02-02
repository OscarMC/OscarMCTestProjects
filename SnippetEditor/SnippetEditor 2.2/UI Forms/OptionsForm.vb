'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System


' description:
'
'  options form.  Allows user to change the default font, tab size, and file associations
'                 font changes are stored in the users settings (config file)
'                 file associations are only written to registry as the user asks
'                 the registry association is saved and restored should they decide to disassociate
'                 (see also FileAssociation.vb in the Helper classes folder)
'

Friend Class OptionsForm


#Region "okay and cancel button handlers, load and save"

	Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
		My.Settings.Reload()
		Me.DialogResult = Windows.Forms.DialogResult.Cancel
		Me.Close()
	End Sub


	Private Sub btn_Okay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Okay.Click
		SaveSettings()
		Me.DialogResult = Windows.Forms.DialogResult.OK
		Me.Close()
	End Sub

	Private Sub SaveSettings()
		SaveFileAssociations()
      My.Settings.Save()
   End Sub


	Private Sub OptionsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		DisplayFileAssociations()
		Me.SettingsBindingSource.DataSource = My.Settings
	End Sub


#End Region


#Region "font tab "

	Private Sub btnPickFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPickFont.Click
		Dim fd As New Windows.Forms.FontDialog
		With fd
			.Font = My.Settings.EditorFont
			.ShowEffects = False
			.ShowColor = False
			.ShowApply = False
			If .ShowDialog = Windows.Forms.DialogResult.OK Then
				My.Settings.EditorFont = .Font
			End If
		End With
	End Sub


#End Region


#Region "file association teb"

	Private m_const_snippet As String = ".snippet"
	Private m_const_vbsnippet As String = ".vbsnippet"
	Private m_const_vssnippet As String = ".vssnippet"


	Private Sub DisplayFileAssociations()
		chk_Snippet.Checked = FileAssociation.IsAssociated(m_const_snippet)
		chk_VBSnippet.Checked = FileAssociation.IsAssociated(m_const_vbsnippet)
		chk_VSSnippet.Checked = FileAssociation.IsAssociated(m_const_vssnippet)
	End Sub

	Private Sub SaveFileAssociations()
		FileAssociation.Associate(m_const_snippet, Not chk_Snippet.Checked)
		FileAssociation.Associate(m_const_vbsnippet, Not chk_VBSnippet.Checked)
		FileAssociation.Associate(m_const_vssnippet, Not chk_VSSnippet.Checked)
	End Sub

#End Region




End Class
