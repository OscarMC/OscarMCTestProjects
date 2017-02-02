'-------------------------------------------------------------------------------
'<copyright file="MainForm.Designer.vb" company="Microsoft">
'   Copyright (c) Microsoft Corporation. All rights reserved.
'</copyright>
'
' THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
' KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
' IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
' PARTICULAR PURPOSE.
'-------------------------------------------------------------------------------
Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.VisualBasic


Partial Friend Class MainForm
	Inherits System.Windows.Forms.Form

	<System.Diagnostics.DebuggerNonUserCode()> _
	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

      Me.SnippetEditorControl1.SnippetFile = Me._Snippet
      Me.SnippetExplorer1.Snippet = Me._Snippet

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
   '<System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
      Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
      Me.SnippetExplorer1 = New SnippetEditor.SnippetExplorer
      Me.SplashPicture = New System.Windows.Forms.PictureBox
      Me.SnippetEditorControl1 = New SnippetEditor.SnippetEditorControl
      Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
      Me.tsb_ShowFolders = New System.Windows.Forms.ToolStripButton
      Me.tsb_Sync = New System.Windows.Forms.ToolStripButton
      Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
      Me.tsb_NewSnippet = New System.Windows.Forms.ToolStripButton
      Me.tsb_Save = New System.Windows.Forms.ToolStripSplitButton
      Me.tsb_SaveAs = New System.Windows.Forms.ToolStripMenuItem
      Me.tsb_Help = New System.Windows.Forms.ToolStripDropDownButton
      Me.TipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
      Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
      Me.tsb_Options = New System.Windows.Forms.ToolStripButton
      Me.tsb_ExportToVSI = New System.Windows.Forms.ToolStripButton
      Me.m_StatusStrip = New System.Windows.Forms.StatusStrip
      Me.m_StatusLabel = New System.Windows.Forms.ToolStripStatusLabel
      Me.SplitContainer1.Panel1.SuspendLayout()
      Me.SplitContainer1.Panel2.SuspendLayout()
      Me.SplitContainer1.SuspendLayout()
      CType(Me.SplashPicture, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.ToolStrip1.SuspendLayout()
      Me.m_StatusStrip.SuspendLayout()
      Me.SuspendLayout()
      '
      'SplitContainer1
      '
      Me.SplitContainer1.DataBindings.Add(New System.Windows.Forms.Binding("SplitterDistance", Global.SnippetEditor.My.MySettings.Default, "SplitterPosition", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      resources.ApplyResources(Me.SplitContainer1, "SplitContainer1")
      Me.SplitContainer1.Name = "SplitContainer1"
      '
      'SplitContainer1.Panel1
      '
      resources.ApplyResources(Me.SplitContainer1.Panel1, "SplitContainer1.Panel1")
      Me.SplitContainer1.Panel1.Controls.Add(Me.SnippetExplorer1)
      '
      'SplitContainer1.Panel2
      '
      resources.ApplyResources(Me.SplitContainer1.Panel2, "SplitContainer1.Panel2")
      Me.SplitContainer1.Panel2.Controls.Add(Me.SplashPicture)
      Me.SplitContainer1.Panel2.Controls.Add(Me.SnippetEditorControl1)
      Me.SplitContainer1.Panel2.Controls.Add(Me.ToolStrip1)
      Me.SplitContainer1.SplitterDistance = Global.SnippetEditor.My.MySettings.Default.SplitterPosition
      '
      'SnippetExplorer1
      '
      resources.ApplyResources(Me.SnippetExplorer1, "SnippetExplorer1")
      Me.SnippetExplorer1.Name = "SnippetExplorer1"
      Me.SnippetExplorer1.Snippet = Nothing
      '
      'SplashPicture
      '
      Me.SplashPicture.BackgroundImage = Global.SnippetEditor.My.Resources.Resources.message_in_a_bottle
      resources.ApplyResources(Me.SplashPicture, "SplashPicture")
      Me.SplashPicture.Name = "SplashPicture"
      Me.SplashPicture.TabStop = False
      '
      'SnippetEditorControl1
      '
      resources.ApplyResources(Me.SnippetEditorControl1, "SnippetEditorControl1")
      Me.SnippetEditorControl1.DataBindings.Add(New System.Windows.Forms.Binding("EditorFont", Global.SnippetEditor.My.MySettings.Default, "EditorFont", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.SnippetEditorControl1.DataBindings.Add(New System.Windows.Forms.Binding("TabSize", Global.SnippetEditor.My.MySettings.Default, "TabSize", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
      Me.SnippetEditorControl1.EditorFont = Global.SnippetEditor.My.MySettings.Default.EditorFont
      Me.SnippetEditorControl1.MinimumSize = New System.Drawing.Size(562, 607)
      Me.SnippetEditorControl1.Name = "SnippetEditorControl1"
      Me.SnippetEditorControl1.SnippetFile = Nothing
      Me.SnippetEditorControl1.TabSize = Global.SnippetEditor.My.MySettings.Default.TabSize
      '
      'ToolStrip1
      '
      Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_ShowFolders, Me.tsb_Sync, Me.ToolStripSeparator3, Me.tsb_NewSnippet, Me.tsb_Save, Me.tsb_Help, Me.tsb_Options, Me.tsb_ExportToVSI})
      resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
      Me.ToolStrip1.Name = "ToolStrip1"
      '
      'tsb_ShowFolders
      '
      Me.tsb_ShowFolders.Checked = True
      Me.tsb_ShowFolders.CheckOnClick = True
      Me.tsb_ShowFolders.CheckState = System.Windows.Forms.CheckState.Checked
      Me.tsb_ShowFolders.Image = Global.SnippetEditor.My.Resources.Resources.img_book_open
      resources.ApplyResources(Me.tsb_ShowFolders, "tsb_ShowFolders")
      Me.tsb_ShowFolders.Name = "tsb_ShowFolders"
      '
      'tsb_Sync
      '
      resources.ApplyResources(Me.tsb_Sync, "tsb_Sync")
      Me.tsb_Sync.Image = Global.SnippetEditor.My.Resources.Resources.img_SychronizeList
      Me.tsb_Sync.Name = "tsb_Sync"
      '
      'ToolStripSeparator3
      '
      Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
      resources.ApplyResources(Me.ToolStripSeparator3, "ToolStripSeparator3")
      '
      'tsb_NewSnippet
      '
      Me.tsb_NewSnippet.Image = Global.SnippetEditor.My.Resources.Resources.img_NewDocument
      resources.ApplyResources(Me.tsb_NewSnippet, "tsb_NewSnippet")
      Me.tsb_NewSnippet.Name = "tsb_NewSnippet"
      '
      'tsb_Save
      '
      Me.tsb_Save.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsb_SaveAs})
      resources.ApplyResources(Me.tsb_Save, "tsb_Save")
      Me.tsb_Save.Image = Global.SnippetEditor.My.Resources.Resources.img_Save
      Me.tsb_Save.Name = "tsb_Save"
      '
      'tsb_SaveAs
      '
      Me.tsb_SaveAs.Name = "tsb_SaveAs"
      resources.ApplyResources(Me.tsb_SaveAs, "tsb_SaveAs")
      '
      'tsb_Help
      '
      Me.tsb_Help.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
      Me.tsb_Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      Me.tsb_Help.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TipsToolStripMenuItem, Me.AboutToolStripMenuItem})
      Me.tsb_Help.Image = Global.SnippetEditor.My.Resources.Resources.img_Help
      resources.ApplyResources(Me.tsb_Help, "tsb_Help")
      Me.tsb_Help.Name = "tsb_Help"
      '
      'TipsToolStripMenuItem
      '
      Me.TipsToolStripMenuItem.Image = Global.SnippetEditor.My.Resources.Resources.img_Idea
      resources.ApplyResources(Me.TipsToolStripMenuItem, "TipsToolStripMenuItem")
      Me.TipsToolStripMenuItem.Name = "TipsToolStripMenuItem"
      '
      'AboutToolStripMenuItem
      '
      Me.AboutToolStripMenuItem.Image = Global.SnippetEditor.My.Resources.Resources.img_Help
      resources.ApplyResources(Me.AboutToolStripMenuItem, "AboutToolStripMenuItem")
      Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
      '
      'tsb_Options
      '
      Me.tsb_Options.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
      Me.tsb_Options.Image = Global.SnippetEditor.My.Resources.Resources.img_Options
      resources.ApplyResources(Me.tsb_Options, "tsb_Options")
      Me.tsb_Options.Name = "tsb_Options"
      '
      'tsb_ExportToVSI
      '
      resources.ApplyResources(Me.tsb_ExportToVSI, "tsb_ExportToVSI")
      Me.tsb_ExportToVSI.Image = Global.SnippetEditor.My.Resources.Resources.img_ToVSI
      Me.tsb_ExportToVSI.Name = "tsb_ExportToVSI"
      '
      'm_StatusStrip
      '
      Me.m_StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.m_StatusLabel})
      resources.ApplyResources(Me.m_StatusStrip, "m_StatusStrip")
      Me.m_StatusStrip.Name = "m_StatusStrip"
      '
      'm_StatusLabel
      '
      Me.m_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me.m_StatusLabel.IsLink = True
      Me.m_StatusLabel.Name = "m_StatusLabel"
      resources.ApplyResources(Me.m_StatusLabel, "m_StatusLabel")
      Me.m_StatusLabel.Spring = True
      '
      'MainForm
      '
      resources.ApplyResources(Me, "$this")
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.SplitContainer1)
      Me.Controls.Add(Me.m_StatusStrip)
      Me.Name = "MainForm"
      Me.SplitContainer1.Panel1.ResumeLayout(False)
      Me.SplitContainer1.Panel2.ResumeLayout(False)
      Me.SplitContainer1.Panel2.PerformLayout()
      Me.SplitContainer1.ResumeLayout(False)
      CType(Me.SplashPicture, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ToolStrip1.ResumeLayout(False)
      Me.ToolStrip1.PerformLayout()
      Me.m_StatusStrip.ResumeLayout(False)
      Me.m_StatusStrip.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents m_StatusStrip As System.Windows.Forms.StatusStrip
   Friend WithEvents m_StatusLabel As System.Windows.Forms.ToolStripStatusLabel
   Friend WithEvents SplashPicture As System.Windows.Forms.PictureBox
   Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
   Friend WithEvents tsb_ShowFolders As System.Windows.Forms.ToolStripButton
   Friend WithEvents tsb_Sync As System.Windows.Forms.ToolStripButton
   Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents tsb_NewSnippet As System.Windows.Forms.ToolStripButton
   Friend WithEvents tsb_Save As System.Windows.Forms.ToolStripSplitButton
   Friend WithEvents tsb_SaveAs As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents tsb_Help As System.Windows.Forms.ToolStripDropDownButton
   Friend WithEvents TipsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents tsb_Options As System.Windows.Forms.ToolStripButton
   Friend WithEvents tsb_ExportToVSI As System.Windows.Forms.ToolStripButton
   Friend WithEvents SnippetEditorControl1 As SnippetEditor.SnippetEditorControl
   Friend WithEvents SnippetExplorer1 As SnippetEditor.SnippetExplorer
   Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer

End Class
