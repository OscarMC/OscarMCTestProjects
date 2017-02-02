'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On



<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class SnippetExplorer
   Inherits System.Windows.Forms.UserControl

   'UserControl overrides dispose to clean up the component list.
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SnippetExplorer))
      Me.m_SnippetTreeView = New System.Windows.Forms.TreeView
      Me.m_FileContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
      Me.mnu_EditFile = New System.Windows.Forms.ToolStripMenuItem
      Me.mnu_DeleteFile = New System.Windows.Forms.ToolStripMenuItem
      Me.m_TreeViewImageList = New System.Windows.Forms.ImageList(Me.components)
      Me.m_FolderContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
      Me.mnu_NewFolder = New System.Windows.Forms.ToolStripMenuItem
      Me.mnu_NewSnippet = New System.Windows.Forms.ToolStripMenuItem
      Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
      Me.mnu_DeleteFolder = New System.Windows.Forms.ToolStripMenuItem
      Me.m_FilterToolStrip = New System.Windows.Forms.ToolStrip
      Me.m_FilterTextBox = New System.Windows.Forms.ToolStripTextBox
      Me.tsb_ApplyFilter = New System.Windows.Forms.ToolStripSplitButton
      Me._SearchCode = New System.Windows.Forms.ToolStripMenuItem
      Me._SearchDescription = New System.Windows.Forms.ToolStripMenuItem
      Me._SearchKeywords = New System.Windows.Forms.ToolStripMenuItem
      Me._SearchShortcut = New System.Windows.Forms.ToolStripMenuItem
      Me._SearchTitle = New System.Windows.Forms.ToolStripMenuItem
      Me.m_LanguageContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
      Me.mnu_AddPath = New System.Windows.Forms.ToolStripMenuItem
      Me.ComboBox1 = New System.Windows.Forms.ComboBox
      Me.MySettingsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
      Me.m_FileContextMenu.SuspendLayout()
      Me.m_FolderContextMenu.SuspendLayout()
      Me.m_FilterToolStrip.SuspendLayout()
      Me.m_LanguageContextMenu.SuspendLayout()
      CType(Me.MySettingsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'm_SnippetTreeView
      '
      Me.m_SnippetTreeView.AllowDrop = True
      Me.m_SnippetTreeView.ContextMenuStrip = Me.m_FileContextMenu
      resources.ApplyResources(Me.m_SnippetTreeView, "m_SnippetTreeView")
      Me.m_SnippetTreeView.FullRowSelect = True
      Me.m_SnippetTreeView.HideSelection = False
      Me.m_SnippetTreeView.ImageList = Me.m_TreeViewImageList
      Me.m_SnippetTreeView.Name = "m_SnippetTreeView"
      Me.m_SnippetTreeView.ShowNodeToolTips = True
      '
      'm_FileContextMenu
      '
      Me.m_FileContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnu_EditFile, Me.mnu_DeleteFile})
      Me.m_FileContextMenu.Name = "ContextMenuStrip1"
      resources.ApplyResources(Me.m_FileContextMenu, "m_FileContextMenu")
      '
      'mnu_EditFile
      '
      Me.mnu_EditFile.Image = Global.SnippetEditor.My.Resources.Resources.img_Snippet
      resources.ApplyResources(Me.mnu_EditFile, "mnu_EditFile")
      Me.mnu_EditFile.Name = "mnu_EditFile"
      '
      'mnu_DeleteFile
      '
      Me.mnu_DeleteFile.Image = Global.SnippetEditor.My.Resources.Resources.img_Delete
      resources.ApplyResources(Me.mnu_DeleteFile, "mnu_DeleteFile")
      Me.mnu_DeleteFile.Name = "mnu_DeleteFile"
      '
      'm_TreeViewImageList
      '
      Me.m_TreeViewImageList.ImageStream = CType(resources.GetObject("m_TreeViewImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
      Me.m_TreeViewImageList.TransparentColor = System.Drawing.Color.Fuchsia
      Me.m_TreeViewImageList.Images.SetKeyName(0, "VSFolder_closed.bmp")
      Me.m_TreeViewImageList.Images.SetKeyName(1, "snippet.bmp")
      Me.m_TreeViewImageList.Images.SetKeyName(2, "book_active_directory.bmp")
      Me.m_TreeViewImageList.Images.SetKeyName(3, "VSFolder_fav.bmp")
      '
      'm_FolderContextMenu
      '
      Me.m_FolderContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnu_NewFolder, Me.mnu_NewSnippet, Me.ToolStripSeparator1, Me.mnu_DeleteFolder})
      Me.m_FolderContextMenu.Name = "ContextMenuStrip2"
      resources.ApplyResources(Me.m_FolderContextMenu, "m_FolderContextMenu")
      '
      'mnu_NewFolder
      '
      Me.mnu_NewFolder.Image = Global.SnippetEditor.My.Resources.Resources.img_Folder_closed
      resources.ApplyResources(Me.mnu_NewFolder, "mnu_NewFolder")
      Me.mnu_NewFolder.Name = "mnu_NewFolder"
      '
      'mnu_NewSnippet
      '
      Me.mnu_NewSnippet.Image = Global.SnippetEditor.My.Resources.Resources.img_Snippet
      resources.ApplyResources(Me.mnu_NewSnippet, "mnu_NewSnippet")
      Me.mnu_NewSnippet.Name = "mnu_NewSnippet"
      '
      'ToolStripSeparator1
      '
      Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
      resources.ApplyResources(Me.ToolStripSeparator1, "ToolStripSeparator1")
      '
      'mnu_DeleteFolder
      '
      Me.mnu_DeleteFolder.Image = Global.SnippetEditor.My.Resources.Resources.img_Delete
      resources.ApplyResources(Me.mnu_DeleteFolder, "mnu_DeleteFolder")
      Me.mnu_DeleteFolder.Name = "mnu_DeleteFolder"
      '
      'm_FilterToolStrip
      '
      Me.m_FilterToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
      Me.m_FilterToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.m_FilterTextBox, Me.tsb_ApplyFilter})
      resources.ApplyResources(Me.m_FilterToolStrip, "m_FilterToolStrip")
      Me.m_FilterToolStrip.Name = "m_FilterToolStrip"
      '
      'm_FilterTextBox
      '
      Me.m_FilterTextBox.AcceptsReturn = True
      resources.ApplyResources(Me.m_FilterTextBox, "m_FilterTextBox")
      Me.m_FilterTextBox.Name = "m_FilterTextBox"
      Me.m_FilterTextBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
      Me.m_FilterTextBox.ShortcutsEnabled = False
      '
      'tsb_ApplyFilter
      '
      Me.tsb_ApplyFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
      Me.tsb_ApplyFilter.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me._SearchCode, Me._SearchDescription, Me._SearchKeywords, Me._SearchShortcut, Me._SearchTitle})
      Me.tsb_ApplyFilter.Image = Global.SnippetEditor.My.Resources.Resources.img_Zoom
      resources.ApplyResources(Me.tsb_ApplyFilter, "tsb_ApplyFilter")
      Me.tsb_ApplyFilter.Name = "tsb_ApplyFilter"
      '
      '_SearchCode
      '
      Me._SearchCode.Checked = True
      Me._SearchCode.CheckOnClick = True
      Me._SearchCode.CheckState = System.Windows.Forms.CheckState.Checked
      Me._SearchCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me._SearchCode.Name = "_SearchCode"
      resources.ApplyResources(Me._SearchCode, "_SearchCode")
      '
      '_SearchDescription
      '
      Me._SearchDescription.Checked = True
      Me._SearchDescription.CheckOnClick = True
      Me._SearchDescription.CheckState = System.Windows.Forms.CheckState.Checked
      Me._SearchDescription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me._SearchDescription.Name = "_SearchDescription"
      resources.ApplyResources(Me._SearchDescription, "_SearchDescription")
      '
      '_SearchKeywords
      '
      Me._SearchKeywords.Checked = True
      Me._SearchKeywords.CheckOnClick = True
      Me._SearchKeywords.CheckState = System.Windows.Forms.CheckState.Checked
      Me._SearchKeywords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me._SearchKeywords.Name = "_SearchKeywords"
      resources.ApplyResources(Me._SearchKeywords, "_SearchKeywords")
      '
      '_SearchShortcut
      '
      Me._SearchShortcut.Checked = True
      Me._SearchShortcut.CheckOnClick = True
      Me._SearchShortcut.CheckState = System.Windows.Forms.CheckState.Checked
      Me._SearchShortcut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me._SearchShortcut.Name = "_SearchShortcut"
      resources.ApplyResources(Me._SearchShortcut, "_SearchShortcut")
      '
      '_SearchTitle
      '
      Me._SearchTitle.Checked = True
      Me._SearchTitle.CheckOnClick = True
      Me._SearchTitle.CheckState = System.Windows.Forms.CheckState.Checked
      Me._SearchTitle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
      Me._SearchTitle.Name = "_SearchTitle"
      resources.ApplyResources(Me._SearchTitle, "_SearchTitle")
      '
      'm_LanguageContextMenu
      '
      Me.m_LanguageContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnu_AddPath})
      Me.m_LanguageContextMenu.Name = "m_LanguageContextMenu"
      resources.ApplyResources(Me.m_LanguageContextMenu, "m_LanguageContextMenu")
      '
      'mnu_AddPath
      '
      Me.mnu_AddPath.Image = Global.SnippetEditor.My.Resources.Resources.img_Folder_closed
      resources.ApplyResources(Me.mnu_AddPath, "mnu_AddPath")
      Me.mnu_AddPath.Name = "mnu_AddPath"
      '
      'ComboBox1
      '
      Me.ComboBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.MySettingsBindingSource, "SelectedProduct", True))
      Me.ComboBox1.DisplayMember = "Name"
      resources.ApplyResources(Me.ComboBox1, "ComboBox1")
      Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.ComboBox1.FormattingEnabled = True
      Me.ComboBox1.Name = "ComboBox1"
      Me.ComboBox1.ValueMember = "Name"
      '
      'MySettingsBindingSource
      '
      Me.MySettingsBindingSource.DataSource = GetType(SnippetEditor.My.MySettings)
      '
      'SnippetExplorer
      '
      resources.ApplyResources(Me, "$this")
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.m_SnippetTreeView)
      Me.Controls.Add(Me.m_FilterToolStrip)
      Me.Controls.Add(Me.ComboBox1)
      Me.Name = "SnippetExplorer"
      Me.m_FileContextMenu.ResumeLayout(False)
      Me.m_FolderContextMenu.ResumeLayout(False)
      Me.m_FilterToolStrip.ResumeLayout(False)
      Me.m_FilterToolStrip.PerformLayout()
      Me.m_LanguageContextMenu.ResumeLayout(False)
      CType(Me.MySettingsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents m_SnippetTreeView As System.Windows.Forms.TreeView
   Friend WithEvents m_TreeViewImageList As System.Windows.Forms.ImageList
   Friend WithEvents m_FileContextMenu As System.Windows.Forms.ContextMenuStrip
   Friend WithEvents mnu_EditFile As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents mnu_DeleteFile As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_FolderContextMenu As System.Windows.Forms.ContextMenuStrip
   Friend WithEvents mnu_NewFolder As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents mnu_NewSnippet As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents mnu_DeleteFolder As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents m_FilterToolStrip As System.Windows.Forms.ToolStrip
   Friend WithEvents m_FilterTextBox As System.Windows.Forms.ToolStripTextBox
   Friend WithEvents m_LanguageContextMenu As System.Windows.Forms.ContextMenuStrip
   Friend WithEvents mnu_AddPath As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents tsb_ApplyFilter As System.Windows.Forms.ToolStripSplitButton
   Friend WithEvents _SearchTitle As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents _SearchDescription As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents _SearchCode As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents _SearchShortcut As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
   Friend WithEvents _SearchKeywords As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents MySettingsBindingSource As System.Windows.Forms.BindingSource

End Class
