'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports System.Diagnostics



' description :  MainForm.  As the name implies this is the application's main form.
'                           The two main controls on this form are a SnippetExplorer which provides the tree-view navigation
'                           and the SnippetEditorControl which does all the work in editing a snippet
'                           The form itself orchestrates messages from the SnippetExplorer to the SnippetEditorControl,
'                           handles the tool strip button events, including displaying the options dialogue.

Friend Class MainForm


#Region "private fields"

   Private _titleSuffix As String = " - " + My.Resources.Snippet_Editor

   Private _SnippetPath As String

   Private _Snippet As New SnippetFile


#End Region


#Region "snippet explorer event handlers"


   Private Sub SnippetExplorer1_FileSelected(ByVal sender As Object, ByVal e As PathEventArgs) Handles SnippetExplorer1.FileSelected
      Me.m_StatusLabel.Text = e.Path
   End Sub


   Private Sub SnippetExplorer1_FolderSelected(ByVal sender As Object, ByVal e As PathEventArgs) Handles SnippetExplorer1.FolderSelected
      Me.m_StatusLabel.Text = e.Path
   End Sub


   Private Sub SnippetExplorer1_OpenSnippet(ByVal sender As Object, ByVal e As PathEventArgs) Handles SnippetExplorer1.OpenSnippet
      OpenSnippet(e.Path)
   End Sub



   Private Sub OpenSnippet(ByVal path As String)
      EnableToolStripButtons()

      With Me.SnippetEditorControl1
         If Me._SnippetPath <> Nothing AndAlso IO.File.Exists(Me._SnippetPath) Then
            If .SnippetFile.SnippetHasChanged Then
               If MsgBox("save changes to " & My.Computer.FileSystem.GetName(.SnippetFile.Filename) & " ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                  .SaveSnippet()
               End If
            End If
         End If
         _SnippetPath = path
         If path = Nothing Then
            .NewSnippet()
         Else
            .OpenSnippet(path)
         End If
      End With
   End Sub


   Private Sub SnippetEditorControl1_SnippetOpened(ByVal sender As Object, ByVal e As PathEventArgs) Handles SnippetEditorControl1.SnippetOpened
      Me.Text = My.Computer.FileSystem.GetName(e.Path) & _titleSuffix
   End Sub


#End Region



#Region "tool strip event handlers"

   Private Sub tsb_Save_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsb_Save.ButtonClick
      If SnippetEditorControl1.SnippetFile.Filename = Nothing Then
         SaveAs()
         Return
      End If
      If SnippetEditorControl1.SaveSnippet() Then
         MessageDialogue.DisplayMessage(My.Resources.Save_Snippet_Success, Me)
         Dim nd As SnippetExplorer.BaseFileNode = Me.SnippetExplorer1.SelectNodeByPath(_SnippetPath)
         If nd IsNot Nothing Then nd.SetText()
      Else
         MessageDialogue.DisplayMessage(My.Resources.Save_Snippet_Error, Me, , 6000, DialogueStyle.Failure)
      End If
   End Sub


   Private Sub tsb_SaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_SaveAs.Click
      SaveAs()
   End Sub


   Private Sub tsb_Sync_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsb_Sync.Click
      If _SnippetPath <> Nothing Then
         Me.SnippetExplorer1.SelectNodeByPath(_SnippetPath)
      End If
   End Sub


   Private Sub tsb_ExportToVSI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_ExportToVSI.Click
      Select Case ExportToVSI()
         Case ActionResult.Success
            MessageDialogue.DisplayMessage(My.Resources.Export_Snippet_Success, Me)

         Case ActionResult.Cancelled
            MessageDialogue.DisplayMessage(My.Resources.Export_Snippet_Cancelled, Me, , , DialogueStyle.Information)

         Case ActionResult.Fail
            MessageDialogue.DisplayMessage(My.Resources.Export_Snippet_Error, Me, , 6000, DialogueStyle.Failure)

      End Select

   End Sub


   Private Sub tsb_Options_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_Options.Click
      If My.Forms.OptionsForm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
         For Each tsb As ToolStripItem In Me.ToolStrip1.Items
            tsb.DisplayStyle = My.Settings.Toolbar_DisplayStyle
         Next
      End If
   End Sub



   Private Sub StatusLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles m_StatusLabel.Click
      System.Diagnostics.Process.Start("explorer", "/select," & m_StatusLabel.Text)
   End Sub


   Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
      My.Forms.AboutForm.ShowDialog(Me)
   End Sub


   Private Sub TipsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TipsToolStripMenuItem.Click
      My.Forms.TipsForm.ShowDialog(Me)
   End Sub


   Private Sub tsb_NewSnippet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_NewSnippet.Click
      OpenSnippet(Nothing)
   End Sub


   Private Sub tsb_ShowFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsb_ShowFolders.Click
      Me.SplitContainer1.Panel1Collapsed = Not Me.SplitContainer1.Panel1Collapsed
   End Sub

#End Region



#Region "save and export methods"

   Public Sub SaveAs()
      Select Case SaveSnippetAs_Internal()
         Case ActionResult.Success
            MessageDialogue.DisplayMessage(My.Resources.Save_Snippet_Success, Me)
            If Me._Snippet IsNot Nothing Then
               Me.LoadCustomFile(Me._Snippet.Filename)
            End If

         Case ActionResult.Cancelled
            MessageDialogue.DisplayMessage(My.Resources.Save_Snippet_Cancelled, Me, , 3000, DialogueStyle.Information)

         Case ActionResult.Fail
            MessageDialogue.DisplayMessage(My.Resources.Save_Snippet_Error, Me, , 6000, DialogueStyle.Failure)

      End Select
   End Sub




   ''' <summary>
   ''' This method tries to "save as" the current snippet 
   ''' </summary>
   Private Function SaveSnippetAs_Internal() As ActionResult
      Validate()
      Dim sfd As SaveFileDialog = New SaveFileDialog()
      With sfd
         .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & My.Resources.MyDocs_Snippets_Path
         .Filter = My.Resources.OpenSaveTypeFilter
         .FilterIndex = 1
         .Title = My.Resources.Save_Snippet_As
         .AddExtension = True
         .CheckPathExists = True
         .DefaultExt = My.Resources.Snippet_Extension
         .OverwritePrompt = True
         .ValidateNames = True

         If SnippetEditorControl1.SnippetFile.Filename = Nothing Then
            .FileName = .InitialDirectory + "*." & .DefaultExt
         Else
            .FileName = SnippetEditorControl1.SnippetFile.Filename
         End If
      End With


      Select Case sfd.ShowDialog()
         Case Windows.Forms.DialogResult.OK
            If (SnippetEditorControl1.SnippetFile.ToFile(sfd.FileName)) Then
               Return ActionResult.Success
            Else
               Return ActionResult.Fail
            End If
         Case Windows.Forms.DialogResult.Cancel
            Return ActionResult.Cancelled
         Case Else
            Return ActionResult.Fail
      End Select


   End Function



   Private Function ExportToVSI() As ActionResult

      Dim sfd As SaveFileDialog = New SaveFileDialog()
      With sfd
         .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
         .Filter = My.Resources.Export_SaveTypeFilter
         .FilterIndex = 1
         .Title = My.Resources.Export_Title
         .AddExtension = True
         .CheckPathExists = True
         .DefaultExt = "vsi"
         .OverwritePrompt = True
         .ValidateNames = True

         If SnippetEditorControl1.SnippetFile.Filename = Nothing Then
            .FileName = .InitialDirectory + "*." & .DefaultExt
         Else
            .FileName = IO.Path.GetFileNameWithoutExtension(SnippetEditorControl1.SnippetFile.Filename) & "." & .DefaultExt
         End If
      End With

      Select Case sfd.ShowDialog()

         Case Windows.Forms.DialogResult.OK
            If VsiWriter.SaveAsVsi(sfd.FileName, Me._SnippetPath) Then
               Return ActionResult.Success
            Else
               Return ActionResult.Fail
            End If

         Case Windows.Forms.DialogResult.Cancel
            Return ActionResult.Cancelled
         Case Else
            Return ActionResult.Fail
      End Select


   End Function


#End Region



#Region "utility methods"

   ' enables the save and refresh buttons once a snippet has been loaded
   ' called from the snippet explorer double clicked event
   Private Sub EnableToolStripButtons()
      Me.tsb_Save.Enabled = True
      Me.tsb_Sync.Enabled = True
      Me.tsb_ExportToVSI.Enabled = True
      Me.SplashPicture.Visible = False
   End Sub



#End Region



#Region "startup"

   'called from MyApplication_Startup in ApplicationEvents.vb if there are command line arguments
   Public Sub LoadCustomFile(ByVal path As String)
      Me.SnippetExplorer1.LoadCustomFile(path)
   End Sub

   Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
      If Me.WindowState = FormWindowState.Normal Then
         My.Settings.Form_Width = Me.Width
         My.Settings.Form_Height = Me.Height
      Else
         My.Settings.Form_Width = Me.RestoreBounds.Width
         My.Settings.Form_Height = Me.RestoreBounds.Height
      End If
      My.Settings.Save()
   End Sub

   Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Me.Size = New Drawing.Size(My.Settings.Form_Width, My.Settings.Form_Height)
      Me.SplitContainer1.SplitterDistance = My.Settings.SplitterPosition
      Me.Activate()
   End Sub



#End Region


End Class




