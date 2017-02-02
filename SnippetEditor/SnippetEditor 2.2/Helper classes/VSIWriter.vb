'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Linq
Imports System.Xml.Linq
Imports Microsoft.VisualBasic
Imports SnippetEditor.SnippetRepresentation

Imports <xmlns:sn="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
Imports <xmlns="http://schemas.microsoft.com/developer/vscontent/2005">


'
''' <summary>
'''  class to write snippet(s) to a VSI file.
''' </summary>
''' <remarks>requires reference to Microsoft.VisualStudio.Zip</remarks>
Friend NotInheritable Class VsiWriter

	Private Shared ReadOnly CrLf As String = Environment.NewLine

	Private Sub New()
		' only shared methods
	End Sub


	Public Shared Function SaveAsVsi(ByVal vsiPath As String, ByVal snippetPath As String) As Boolean

		Dim tempPath As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\VbsnippetEditor"

		Dim snippetFileName As String = IO.Path.GetFileName(snippetPath)
		Dim filename As String = IO.Path.GetFileNameWithoutExtension(snippetPath)
		Dim newSnippetPath As String = IO.Path.Combine(tempPath, snippetFileName)
		Dim vscontentPath As String = IO.Path.Combine(tempPath, filename & ".vscontent")


		My.Computer.FileSystem.CreateDirectory(tempPath)

      My.Computer.FileSystem.CopyFile(snippetPath, newSnippetPath, True)
		WriteVSContentFile(vscontentPath, newSnippetPath)

		Dim myzip As New Microsoft.VisualStudio.Zip.ZipFileCompressor(vsiPath, tempPath, New String() {filename & ".vscontent", filename & ".snippet"}, True, False)

		My.Computer.FileSystem.DeleteDirectory(tempPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
		Return True

	End Function



	Private Shared Function WriteVSContentFile(ByVal path As String, ByVal snippetPath As String) As Boolean
		Return WriteVSContentFile(path, New String() {snippetPath})
	End Function


	Private Shared Function WriteVSContentFile(ByVal path As String, ByVal snippetPaths() As String) As Boolean
		Dim sOut As String = SnippetsToVSContent(snippetPaths)
		IO.File.WriteAllText(path, sOut)
		Return True
	End Function


	Private Shared Function SnippetsToVSContent(ByVal snippetPaths() As String) As String
      Return <VSContent>
                <%= From path As String In snippetPaths _
                   Let doc = XDocument.Load(path) _
                   Select <Content>
                             <FileName><%= IO.Path.GetFileName(path) %></FileName>
                             <DisplayName><%= doc...<sn:CodeSnippet>.<sn:Header>.<sn:Title>.Value %></DisplayName>
                             <FileContentType>Code Snippet</FileContentType>
                             <ContentVersion>1.0</ContentVersion>
                             <Attributes>
                                <Attribute name="lang" value=<%= doc...<sn:CodeSnippet>.<sn:Snippet>.<sn:Code>.@Language.ToLowerInvariant %>/>
                             </Attributes>
                          </Content> %>
             </VSContent>.ToString(SaveOptions.None)

   End Function




End Class
