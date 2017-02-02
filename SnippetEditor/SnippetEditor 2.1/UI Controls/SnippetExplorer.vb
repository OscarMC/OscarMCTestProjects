'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Windows.Forms
Imports System.Diagnostics
Imports SnippetEditor.SnippetRepresentation
Imports VB = Microsoft.VisualBasic
Imports System.Collections.Generic
Imports SnippetEditor.Utility
Imports System.Linq
Imports System.Xml.Linq

Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">


Friend Class SnippetExplorer


#Region "private variables"

   Private m_Snippet As SnippetFile

#End Region


#Region "public properties"

   <System.ComponentModel.Browsable(False)> _
    Public Property Snippet() As SnippetFile
      Get
         Return m_Snippet
      End Get
      Set(ByVal value As SnippetFile)
         m_Snippet = value
      End Set
   End Property

#End Region


#Region "public event declarations"

   Public Event FolderSelected As EventHandler(Of PathEventArgs)
   Public Event FileSelected As EventHandler(Of PathEventArgs)
   Public Event OpenSnippet As EventHandler(Of PathEventArgs)


   Protected Overridable Sub OnOpenSnippet(ByVal path As String)
      RaiseEvent OpenSnippet(Me, New PathEventArgs(path))
   End Sub

   Protected Overridable Sub OnFileSelected(ByVal path As String)
      RaiseEvent FileSelected(Me, New PathEventArgs(path))
   End Sub

   Protected Overridable Sub OnFolderSelected(ByVal path As String)
      RaiseEvent FolderSelected(Me, New PathEventArgs(path))
   End Sub

#End Region


#Region "public methods"


   Public Sub LoadCustomFile(ByVal path As String)
      If Not m_loaded Then
         m_loaded = True
         FillTreeView()
      End If
      Dim nd As BaseFileNode = Me.FindNodeByPath(path)
      If nd Is Nothing Then
         'need to load an uncatalogued root node and add the path
         Dim root As LanguageNode = Nothing

         'check to see if we already have an UnCatalogued root node
         For Each langNode As LanguageNode In Me.m_SnippetTreeView.Nodes
            If langNode.Language Is Utility.Language.UnCatalogued Then
               root = langNode
               Exit For
            End If
         Next
         If root Is Nothing Then
            root = New LanguageNode(Utility.Language.UnCatalogued)
            m_SnippetTreeView.Nodes.Add(root)
         End If
         nd = New FileNode(path)
         root.Nodes.Add(nd)
      End If

      m_SnippetTreeView.SelectedNode = nd
      Me.OnOpenSnippet(path)
   End Sub


   Public Function SelectNodeByPath(ByVal path As String) As BaseFileNode
      Dim nd As BaseFileNode = Me.FindNodeByPath(path)
      m_SnippetTreeView.SelectedNode = nd
      Return nd
   End Function


#End Region


#Region "Snippet Explorer load"

   Private m_loaded As Boolean

   Private Sub SnippetExplorer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      If Not m_loaded AndAlso Not Me.DesignMode Then
         m_loaded = True
         Dim products = (From p In My.Settings.Products Where p.IsInstalled).ToList

         If products.Count > 0 Then
            If My.Settings.SelectedProduct Is Nothing OrElse _
             (From p In products Where p.Name = My.Settings.SelectedProduct).Count = 0 Then
               My.Settings.SelectedProduct = products(0).Name
            End If
         End If

         Me.ComboBox1.ValueMember = "Name"
         Me.ComboBox1.DisplayMember = "Name"
         MySettingsBindingSource.DataSource = My.Settings
         ComboBox1.DataSource = products

      End If
   End Sub

#End Region


#Region "internal methods to populate the treeview and language list"


   Public Sub FillTreeView()
      If Me.DesignMode Then Return
      Me.m_SnippetTreeView.Nodes.Clear()

      Dim prod As Product = TryCast(ComboBox1.SelectedItem, Product)

      If prod Is Nothing Then Return

      For Each lang As Utility.Language In prod.Languages
         If lang.IsInstalled Then FillLanguage(lang)
      Next

      Me.m_SnippetTreeView.Sort()
   End Sub



   Private Sub FillLanguage(ByVal lang As Utility.Language)
      Dim nd As New LanguageNode(lang)
      With nd.Nodes
         Dim paths() As String = lang.GetExpandedPaths 'SnippetPathsManager.GetExpandedPaths(lang)
         Array.Sort(paths)
         For Each path As String In paths
            If path.Length > 2 AndAlso My.Computer.FileSystem.DirectoryExists(path) Then
               Dim fldr As New FolderNode(path, True)
               fldr.ImageIndex = 3
               fldr.SelectedImageIndex = 3
               .Add(fldr)
            End If
         Next
      End With
      Me.m_SnippetTreeView.Nodes.Add(nd)
   End Sub


#End Region


#Region "command bar and context menu handlers"


   Private Sub mnu_DeleteFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_DeleteFile.Click

      If VB.MsgBox(My.Resources.Msg_Delete_File_Prompt, _
           VB.MsgBoxStyle.OkCancel, _
           My.Resources.Msg_Delete_File_Title) = VB.MsgBoxResult.Ok Then

         Dim nd As FileNode = CType(Me.m_SnippetTreeView.SelectedNode, FileNode)
         My.Computer.FileSystem.DeleteFile(nd.Path)
         nd.Remove()

      End If

   End Sub


   Private Sub mnu_DeleteFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_DeleteFolder.Click

      'TODO: probably should check to see if open snippet is in this folder !!
      Dim fldr As FolderNode = CType(Me.m_SnippetTreeView.SelectedNode, FolderNode)

      If Me.mnu_DeleteFolder.Text = My.Resources.Mnu_Delete Then

         ' prompt to delete folder, if yes then actually delete the folder
         If VB.MsgBox(My.Resources.Msg_Delete_Folder_Prompt & fldr.Path, _
            VB.MsgBoxStyle.YesNo, _
            My.Resources.Msg_Delete_Folder_Title) = VB.MsgBoxResult.Yes Then

            My.Computer.FileSystem.DeleteDirectory(fldr.Path, VB.FileIO.DeleteDirectoryOption.DeleteAllContents)
            fldr.Remove()

         End If


      Else  ' must be "Remove"
         'prompt to remove folder. If removed save SnippetPathManager path for the language

         If VB.MsgBox(My.Resources.Msg_Remove_Folder_Prompt & fldr.Path, _
            VB.MsgBoxStyle.YesNo, _
            My.Resources.Msg_Remove_Folder_Title) = VB.MsgBoxResult.Yes Then

            Dim path As String = fldr.Key
            If Not path.EndsWith("\") Then path &= "\"
            Dim lang As Utility.Language = CType(fldr.Parent, LanguageNode).Language
            Dim oldPaths() As String = lang.GetExpandedPaths
            Dim sb As New Text.StringBuilder

            For i As Int32 = 0 To oldPaths.Length - 1
               If Not oldPaths(i).EndsWith("\") Then oldPaths(i) &= "\"
               If oldPaths(i).Length > 2 AndAlso oldPaths(i) <> path Then
                  sb.Append(oldPaths(i))
                  sb.Append(";")
               End If
            Next

            lang.SnippetsPath = sb.ToString
            fldr.Remove()

         End If

      End If

   End Sub


   Private Sub mnu_EditFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_EditFile.Click
      OnOpenSnippet(CType(Me.m_SnippetTreeView.SelectedNode, FileNode).Path)
   End Sub


   Private Sub mnu_NewFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_NewFolder.Click

      Dim nd As FolderNode = CType(Me.m_SnippetTreeView.SelectedNode, FolderNode)

      With My.Forms.InputForm
         If .ShowDialog(My.Resources.Msg_Add_New_Folder_Title, _
              My.Resources.Msg_Add_New_Folder_Prompt, _
              My.Resources.Msg_Add_New_Folder_Default) _
               = DialogResult.OK Then

            Try
               Dim fldrpath As String = IO.Path.Combine(nd.Path, .txtInput.Text)
               If Not My.Computer.FileSystem.DirectoryExists(fldrpath) Then
                  My.Computer.FileSystem.CreateDirectory(fldrpath)
                  Dim fldr As New FolderNode(fldrpath)
                  nd.Nodes.Add(fldr)
                  Me.m_SnippetTreeView.SelectedNode = fldr
               End If
            Catch ex As Exception
               ' catch all
            End Try
         End If
      End With
   End Sub


   Private Sub mnu_NewSnippet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnu_NewSnippet.Click
      'prompt for name, create snippet then raise the open event

      Dim nd As FolderNode = CType(Me.m_SnippetTreeView.SelectedNode, FolderNode)

      With My.Forms.InputForm
         If .ShowDialog(My.Resources.Msg_Add_New_Snippet_Title, _
              My.Resources.Msg_Add_New_Snippet_Prompt, _
              GetNextFileName(nd.Path, My.Resources.Msg_Add_New_Snippet_Default, My.Resources.Snippet_Extension)) _
               = DialogResult.OK Then
            Dim sc As New SnippetFile
            sc.CreateNewSnippet()
            sc.Filename = IO.Path.Combine(nd.Path, EnsureExtension(GetNextFileName(nd.Path, .txtInput.Text, My.Resources.Snippet_Extension), My.Resources.Snippet_Extension))
            sc.CodeSnippet.Header.Title = IO.Path.GetFileNameWithoutExtension(sc.Filename)
            sc.ToFile(sc.Filename)
            Dim fn As New FileNode(sc.Filename)
            nd.Nodes.Add(fn)
            Me.m_SnippetTreeView.SelectedNode = fn
         End If
      End With
   End Sub


   Private Sub mnu_AddPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnu_AddPath.Click
      'browse for folder then add to SnippetPAthManager
      Dim nd As LanguageNode = CType(Me.m_SnippetTreeView.SelectedNode, LanguageNode)
      Dim fldrBrowser As New System.Windows.Forms.FolderBrowserDialog

      With fldrBrowser
         .Description = My.Resources.Msg_Add_Existing_Folder_Prompt
         .ShowNewFolderButton = False
         If .ShowDialog <> DialogResult.Cancel Then
            Dim paths As String = nd.Language.SnippetsPath
            If Not paths.TrimEnd.EndsWith(";"c) Then paths &= ";"
            paths &= .SelectedPath & "\;"
            nd.Language.SnippetsPath = paths
            nd.Nodes.Add(New FolderNode(.SelectedPath, True))
         End If
      End With
   End Sub


   Private Shared Function GetNextFileName(ByVal path As String, ByVal filename As String, ByVal extension As String) As String
      If Not path.EndsWith("\") Then path &= "\"
      If My.Computer.FileSystem.FileExists(path & filename & "." & extension) Then
         Dim i As Int32 = 1
         While My.Computer.FileSystem.FileExists(path & filename & i & "." & extension)
            i += 1
         End While
         Return filename & i
      Else
         Return filename
      End If
   End Function


   Private Shared Function EnsureExtension(ByVal filename As String, ByVal extension As String) As String
      If filename.TrimEnd.EndsWith("." & extension, StringComparison.OrdinalIgnoreCase) Then
         Return filename
      Else
         Return filename & "." & extension
      End If
   End Function

#End Region


#Region "treeview event handlers"


   Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles m_SnippetTreeView.AfterSelect

      If TypeOf e.Node Is FileNode Then
         Me.OnFileSelected(CType(e.Node, BaseFileNode).Path)
      ElseIf TypeOf e.Node Is FolderNode Then
         Me.OnFolderSelected(CType(e.Node, BaseFileNode).Path)
      End If

      'set the context menus
      If TypeOf e.Node Is FileNode Then
         Me.m_SnippetTreeView.ContextMenuStrip = Me.m_FileContextMenu
      ElseIf TypeOf e.Node Is FolderNode Then
         Me.m_SnippetTreeView.ContextMenuStrip = Me.m_FolderContextMenu
         If TypeOf e.Node.Parent Is LanguageNode Then
            Me.mnu_DeleteFolder.Text = My.Resources.Mnu_Remove
         Else
            Me.mnu_DeleteFolder.Text = My.Resources.Mnu_Delete
         End If
      ElseIf TypeOf e.Node Is LanguageNode Then
         If CType(e.Node, LanguageNode).Language Is Utility.Language.UnCatalogued Then
            Me.m_SnippetTreeView.ContextMenuStrip = Nothing
         Else
            Me.m_SnippetTreeView.ContextMenuStrip = Me.m_LanguageContextMenu
         End If
      End If
   End Sub


   Private Sub m_SnippetTreeView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_SnippetTreeView.DragDrop

      With Me.m_SnippetTreeView

         Dim pt As Drawing.Point = .PointToClient(New Drawing.Point(e.X, e.Y))
         Dim nd As TreeNode = .GetNodeAt(pt)

         If TypeOf nd Is FileNode Then nd = nd.Parent

         If Not TypeOf nd Is FolderNode Then Return 'early exit

         .SelectedNode = nd

         Dim fNode As FileNode = CType(e.Data.GetData(GetType(FileNode)), FileNode)

         Dim filepath As String = Nothing


         If Not fNode Is Nothing Then
            If fNode.Parent Is nd Then Return 'exit out as file is dropped onto same path
            filepath = fNode.Path

         Else

            Dim obj() As String = CType(e.Data.GetData("FileDrop"), String())
            If Not obj Is Nothing Then
               If obj.Length > 0 Then

                  filepath = obj(0)
                  'TODO: should we handle multipe file drop here ?  could cause conflicts with paths/filenames
               End If
            End If


         End If

         Dim strNewPath As String = My.Computer.FileSystem.CombinePath(CType(nd, FolderNode).Path, My.Computer.FileSystem.GetName(filepath))


         Try
            If e.Effect = DragDropEffects.Move Then

               My.Computer.FileSystem.MoveFile(filepath, strNewPath, VB.FileIO.UIOption.AllDialogs, VB.FileIO.UICancelOption.ThrowException)
               If Not fNode Is Nothing Then fNode.Remove()

            ElseIf e.Effect = DragDropEffects.Copy Then

               My.Computer.FileSystem.CopyFile(filepath, strNewPath, VB.FileIO.UIOption.AllDialogs, VB.FileIO.UICancelOption.ThrowException)
            End If


            For Each fNode In nd.Nodes
               If fNode.Path = strNewPath Then
                  .SelectedNode = fNode
                  Return
               End If
            Next

            fNode = New FileNode(strNewPath)
            nd.Nodes.Add(fNode)
            .SelectedNode = fNode

         Catch ex As Exception
            ' just catch the exception
         End Try


      End With
   End Sub


   Private Sub m_SnippetTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_SnippetTreeView.DragEnter
      Select Case e.KeyState
         Case 1
            e.Effect = DragDropEffects.Move
         Case 2, 9
            e.Effect = DragDropEffects.Copy
         Case Else
            e.Effect = DragDropEffects.Move
      End Select
   End Sub


   Private Sub m_SnippetTreeView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_SnippetTreeView.DragOver

      ' static vars to track node hovering
      Static lastOver As Int64 = 0
      Static lastNode As TreeNode = Nothing

      With Me.m_SnippetTreeView
         Dim pt As Drawing.Point = .PointToClient(New Drawing.Point(e.X, e.Y))
         Dim nd As TreeNode = .GetNodeAt(pt)

         If TypeOf nd Is FolderNode Then
            .SelectedNode = nd
         End If

         ' expand node if it is hovered over for 1 second
         If nd Is lastNode Then
            If (nd IsNot Nothing) AndAlso (DateTime.Now.Ticks - lastOver > 10000000) Then
               nd.Expand()
            End If
         Else
            lastNode = nd
            lastOver = DateTime.Now.Ticks
         End If


         'scroll the treeview if needed
         If pt.X < 16 Then
            NativeMethods.ScrollLineLeft(Me)
         ElseIf pt.X > .Width - 16 Then
            NativeMethods.ScrollLineRight(Me)
         End If

         If pt.Y < 16 Then
            NativeMethods.ScrollLineUp(Me)
         ElseIf pt.Y > .Height - 16 Then
            NativeMethods.ScrollLineDown(Me)
         End If
      End With

      Select Case e.KeyState
         Case 1
         Case 2, 9
            e.Effect = DragDropEffects.Copy
         Case Else
            e.Effect = DragDropEffects.Move
      End Select


   End Sub


   Private Sub m_SnippetTreeView_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles m_SnippetTreeView.ItemDrag
      If TypeOf e.Item Is FileNode Then
         If e.Button = Windows.Forms.MouseButtons.Left Then
            DoDragDrop(e.Item, DragDropEffects.Move)
         ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            DoDragDrop(e.Item, DragDropEffects.Copy)
         End If
      End If
   End Sub


   Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles m_SnippetTreeView.NodeMouseClick
      If e.Button = Windows.Forms.MouseButtons.Right Then m_SnippetTreeView.SelectedNode = e.Node

   End Sub


   Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles m_SnippetTreeView.NodeMouseDoubleClick
      If TypeOf e.Node Is FileNode Then
         Me.OnOpenSnippet(CType(e.Node, BaseFileNode).Path)
      End If
   End Sub


#End Region



#Region "win api for scrolling the treeview"

   Private Class NativeMethods
      Private Sub New()
         ' shared class
      End Sub

      Private Declare Function SendMessage Lib "user32" Alias "SendMessageW" (ByVal hwnd As Runtime.InteropServices.HandleRef, ByVal wMsg As Int32, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
      Private Const WM_HSCROLL As Int32 = &H114
      Private Const WM_VSCROLL As Int32 = &H115
      Private Const SB_LINEUP As Int32 = 0
      Private Const SB_LINEDOWN As Int32 = 1
      Private Const SB_LINELEFT As Int32 = 0
      Private Const SB_LINERIGHT As Int32 = 1

      Friend Shared Sub ScrollLineLeft(ByVal tv As SnippetExplorer)
         SendMessage(New Runtime.InteropServices.HandleRef(tv, tv.Handle), WM_HSCROLL, New IntPtr(SB_LINELEFT), IntPtr.Zero)
      End Sub

      Friend Shared Sub ScrollLineRight(ByVal tv As SnippetExplorer)
         SendMessage(New Runtime.InteropServices.HandleRef(tv, tv.Handle), WM_HSCROLL, New IntPtr(SB_LINERIGHT), IntPtr.Zero)
      End Sub

      Friend Shared Sub ScrollLineUp(ByVal tv As SnippetExplorer)
         SendMessage(New Runtime.InteropServices.HandleRef(tv, tv.Handle), WM_VSCROLL, New IntPtr(SB_LINEUP), IntPtr.Zero)
      End Sub

      Friend Shared Sub ScrollLineDown(ByVal tv As SnippetExplorer)
         SendMessage(New Runtime.InteropServices.HandleRef(tv, tv.Handle), WM_VSCROLL, New IntPtr(SB_LINEDOWN), IntPtr.Zero)
      End Sub


   End Class

#End Region




#Region "internal node find methods"

   Private Function FindNodeByPath(ByVal path As String) As BaseFileNode
      For Each rootnode As LanguageNode In Me.m_SnippetTreeView.Nodes
         If rootnode IsNot Nothing Then
            For Each nd As BaseFileNode In rootnode.Nodes
               If nd.Path = path Then Return nd
               Dim tempNode As BaseFileNode = FindNodeByPath(nd, path)
               If tempNode IsNot Nothing Then Return tempNode
            Next
         End If
      Next
      Return Nothing
   End Function

   Private Function FindNodeByPath(ByVal node As BaseFileNode, ByVal path As String) As BaseFileNode
      For Each nd As BaseFileNode In node.Nodes
         If nd.Path = path Then Return nd
         Dim tempNode As BaseFileNode = FindNodeByPath(nd, path)
         If tempNode IsNot Nothing Then Return tempNode
      Next
      Return Nothing
   End Function

#End Region


#Region "search handlers"

   <Flags()> _
   Private Enum SearchFlags
      None = 0
      Code = 1
      Description = 2
      Keywords = 4
      Shortcut = 8
      Title = 16
   End Enum


   Private Sub tsb_ApplyFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsb_ApplyFilter.Click
      FillTreeView()
      Dim search As String = m_FilterTextBox.Text.Trim
      If search.Length > 0 Then
         Dim flags As SearchFlags = If(_SearchCode.Checked, SearchFlags.Code, SearchFlags.None) Or _
                                    If(_SearchDescription.Checked, SearchFlags.Description, SearchFlags.None) Or _
                                    If(_SearchKeywords.Checked, SearchFlags.Keywords, SearchFlags.None) Or _
                                    If(_SearchShortcut.Checked, SearchFlags.Shortcut, SearchFlags.None) Or _
                                    If(_SearchTitle.Checked, SearchFlags.Title, SearchFlags.None)

         ApplySearch(search, flags)
         Me.m_SnippetTreeView.ExpandAll()
      End If
   End Sub



   Private Sub ApplySearch(ByVal search As String, ByVal flags As SearchFlags)
      Dim noMatches As List(Of BaseFileNode)
      For Each root As LanguageNode In Me.m_SnippetTreeView.Nodes

         noMatches = New List(Of BaseFileNode)
         For Each children As BaseFileNode In root.Nodes
            ApplyRecursiveSearch(children, search, flags, noMatches)
         Next

         For Each nd As BaseFileNode In noMatches
            nd.Remove()
         Next

         'check and remove empty folders
         Dim noMatches2 As New List(Of BaseFileNode)
         noMatches.Clear()
         For Each nd As BaseFileNode In root.Nodes
            HasSnippets(nd, noMatches)
         Next

         For Each nd As BaseFileNode In noMatches
            nd.Remove()
         Next
      Next

   End Sub


   Private Sub ApplyRecursiveSearch(ByVal node As BaseFileNode, ByVal search As String, ByVal flags As SearchFlags, ByVal noMatches As List(Of BaseFileNode))

      If TypeOf node Is FileNode Then
         If Not SearchContents(node.Path, search, flags) Then
            noMatches.Add(node)
         End If
      ElseIf TypeOf node Is FolderNode Then
         For Each nd As BaseFileNode In node.Nodes
            ApplyRecursiveSearch(nd, search, flags, noMatches)
         Next
      End If
   End Sub


   Private Shared Function SearchContents(ByVal path As String, ByVal search As String, ByVal flags As SearchFlags) As Boolean
      ' do least expensive string searches first
      Dim doc = XDocument.Load(path)

      If (flags And SearchFlags.Shortcut) = SearchFlags.Shortcut Then
         If If(doc...<CodeSnippet>.<Header>.<Shortcut>.Value, "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
      End If

      If (flags And SearchFlags.Title) = SearchFlags.Title Then
         If doc...<CodeSnippet>.<Header>.<Title>.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
      End If

      If (flags And SearchFlags.Description) = SearchFlags.Description Then
         If doc...<CodeSnippet>.<Header>.<Description>.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
      End If

      If (flags And SearchFlags.Keywords) = SearchFlags.Keywords Then
         For Each kwrd In doc...<CodeSnippet>.<Header>.<Keywords>.<Keyword>
            If kwrd.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
         Next
      End If

      If (flags And SearchFlags.Code) = SearchFlags.Code Then
         If doc...<CodeSnippet>.<Snippet>.<Code>.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 Then Return True
      End If

      Return False
   End Function


   Private Function HasSnippets(ByVal fldr As BaseFileNode, ByVal nomatches As List(Of BaseFileNode)) As Boolean

      If fldr.Nodes.Count = 0 Then
         If TypeOf fldr Is FolderNode Then nomatches.Add(fldr)
         Return False
      End If

      ' recurse through folder nodes
      Dim retVal As Boolean
      For Each nd As BaseFileNode In fldr.Nodes
         If TypeOf nd Is FolderNode Then
            If HasSnippets(nd, nomatches) Then retVal = True
         Else
            retVal = True
         End If
      Next

      If retVal = False Then nomatches.Add(fldr)
      Return retVal

   End Function


   Private Sub m_FilterTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles m_FilterTextBox.KeyUp
      If e.KeyCode = Keys.Enter Then
         e.Handled = True
         tsb_ApplyFilter.PerformClick()
      End If
   End Sub

#End Region


#Region "File and Folder treenode classes"


	Friend Class FolderNode
		Inherits BaseFileNode


		Sub New(ByVal key As String, Optional ByVal recurse As Boolean = False)
			MyBase.New(key)
			Me.ImageIndex = 0	' folder
			Me.SelectedImageIndex = 0
         If Not Me.Key.EndsWith("\") Then Me.Key &= "\"
			If recurse Then AddFilesAndfolders()
		End Sub


		Protected Friend Overrides Sub SetText()
         Dim temp As String = Path
			If temp.EndsWith("\") Then temp = temp.Substring(0, temp.Length - 1)
			Me.Text = My.Computer.FileSystem.GetName(temp)
		End Sub


		Private Sub AddFilesAndfolders()
			With Me.Nodes
				' add sub folders
				Dim paths() As String = System.IO.Directory.GetDirectories(Me.Path)
				Array.Sort(paths)
            For Each strPath As String In paths
               Dim tempPath As String = My.Computer.FileSystem.GetName(strPath)
               If Not tempPath.EndsWith("\") Then tempPath &= "\"
               .Add(New FolderNode(Me.Key & tempPath, True))
            Next
				'add files
				Dim files As List(Of String) = New List(Of String)(My.Computer.FileSystem.GetFiles(Path, _
								VB.FileIO.SearchOption.SearchTopLevelOnly, _
								New String() {"*.snippet"}))
				files.Sort()
				For Each strFile As String In files
					.Add(New FileNode(Me.Key & My.Computer.FileSystem.GetName(strFile)))
				Next
			End With
		End Sub


	End Class


	Friend Class FileNode
		Inherits BaseFileNode


		Sub New(ByVal key As String)
			MyBase.New(key)
			Me.ImageIndex = 1	' file
			Me.SelectedImageIndex = 1
		End Sub


		Protected Friend Overrides Sub SetText()
			Select Case My.Settings.NodeDisplayStyle

				Case NodeDisplayStyle.FileName
					Dim temp As String = My.Computer.FileSystem.GetName(Me.Path)
					temp = temp.Substring(0, temp.LastIndexOf("."c))
					Me.Text = temp

				Case NodeDisplayStyle.Title
					Using rdr As Xml.XmlReader = Xml.XmlReader.Create(Me.Path)
						rdr.ReadToFollowing("Title")
						rdr.Read()
						Me.Text = rdr.ReadContentAsString.Trim
					End Using
			End Select
		End Sub

	End Class


	Friend Class LanguageNode
		Inherits System.Windows.Forms.TreeNode

		Public Sub New(ByVal lang As Utility.Language)
			MyBase.New()
			If lang IsNot Nothing Then
				Me.Text = lang.LocalisedName
			End If
			Me.ImageIndex = 2
			Me.SelectedImageIndex = 2
			Me.m_Language = lang
		End Sub


		Private m_Language As Utility.Language

		Public Property Language() As Utility.Language
			Get
				Return m_Language
			End Get
			Set(ByVal value As Utility.Language)
				m_Language = value
			End Set
		End Property

	End Class




	Friend Class BaseFileNode
		Inherits System.Windows.Forms.TreeNode

		Private m_Path As String 'actual physcial path
		Private m_Key As String	'path as stored in registry or expansionsxml.xml.  We store the key to make it easy to find in expansionsxml.xml

		Sub New(ByVal key As String)
			MyBase.New()
			m_Key = key
			m_Path = m_Key
			SetText()
		End Sub


		Public Property Key() As String
			Get
				Return m_Key
			End Get
			Set(ByVal value As String)
				m_Key = value
			End Set
		End Property


		Public Property Path() As String
			Get
				Return m_Path
			End Get
			Set(ByVal value As String)
				m_Path = value
			End Set
		End Property

		Protected Friend Overridable Sub SetText()
			Me.Text = My.Computer.FileSystem.GetName(Me.Path)
		End Sub

	End Class


#End Region


	Public Enum NodeDisplayStyle
		FileName
		Title
	End Enum



   Private Sub m_FilterToolStrip_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_FilterToolStrip.Resize
      m_FilterTextBox.Width = m_FilterToolStrip.Width - tsb_ApplyFilter.Width - 10
   End Sub


   Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
      FillTreeView()
   End Sub



End Class
