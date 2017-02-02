'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports SnippetEditor.SnippetRepresentation
Imports Microsoft.VisualBasic

Imports <xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">


''' <summary>
''' Stotres file information, the CodeSnippet contents and provides support methods such as FromFile and ToFile
''' </summary>
''' <remarks></remarks>
Friend Class SnippetFile

   Private _codeSnippet As CodeSnippet
   Private _filename As String = ""


   ''' <summary>
   ''' the filename (with complete path) of the opened snippet
   ''' </summary>
   Public Property Filename() As String
      Get
         Return _filename
      End Get
      Set(ByVal value As String)
         _filename = value
      End Set
   End Property


   ''' <summary>
   ''' the CodeSnippet contained in this SnippetFile
   ''' </summary>
   Public Property CodeSnippet() As CodeSnippet
      Get
         Return _codeSnippet
      End Get
      Set(ByVal value As CodeSnippet)
         _codeSnippet = value
      End Set
   End Property



   ''' <summary>
   ''' Saves a snippet to a file
   ''' </summary>
   ''' <param name="filename">The file to save the snippet to</param>
   ''' <returns>True is success</returns>
   Public Function ToFile(ByVal filename As String) As Boolean



      Dim xcdCode As New XCData(_codeSnippet.Snippet.Code.Data)

      Dim doc As XDocument = <?xml version="1.0" encoding="utf-8"?>
                             <CodeSnippets>
                                <CodeSnippet Format=<%= _codeSnippet.Format %>>
                                   <Header>
                                      <Title><%= _codeSnippet.Header.Title %></Title>
                                      <Author><%= _codeSnippet.Header.Author %></Author>
                                      <Description><%= _codeSnippet.Header.Description %></Description>
                                      <HelpUrl><%= _codeSnippet.Header.HelpUrl %></HelpUrl>
                                      <SnippetTypes>
                                         <%= From item In _codeSnippet.Header.SnippetTypes.List Select <SnippetType><%= item %></SnippetType> %>
                                      </SnippetTypes>
                                      <Keywords>
                                         <%= From item In _codeSnippet.Header.Keywords.List Select <Keyword><%= item %></Keyword> %>
                                      </Keywords>
                                      <Shortcut><%= _codeSnippet.Header.Shortcut %></Shortcut>
                                   </Header>
                                   <Snippet>
                                      <References>
                                         <%= From item In _codeSnippet.Snippet.References.List Select _
                                            <Reference><Assembly><%= item.Assembly %></Assembly><Url><%= item.Url %></Url></Reference> %>
                                      </References>
                                      <Imports>
                                         <%= From item In _codeSnippet.Snippet.[Imports].List Select <Import><Namespace><%= item.Namespace %></Namespace></Import> %>
                                      </Imports>
                                      <Declarations>
                                         <%= From item In _codeSnippet.Snippet.Declarations.Replacements _
                                            Where item.ReplacementKind = ReplacementKind.Literal Select _
                                            <Literal Editable=<%= item.Editable %>>
                                               <ID><%= item.Id %></ID>
                                               <Type><%= item.Type %></Type>
                                               <ToolTip><%= item.Tooltip %></ToolTip>
                                               <Default><%= item.Default %></Default>
                                               <Function><%= item.Function %></Function>
                                            </Literal> %>
                                         <%= From item In _codeSnippet.Snippet.Declarations.Replacements _
                                            Where item.ReplacementKind = ReplacementKind.Object Select _
                                            <Object Editable=<%= item.Editable %>>
                                               <ID><%= item.Id %></ID>
                                               <Type><%= item.Type %></Type>
                                               <ToolTip><%= item.Tooltip %></ToolTip>
                                               <Default><%= item.Default %></Default>
                                               <Function><%= item.Function %></Function>
                                            </Object> %>
                                      </Declarations>
                                      <Code Language=<%= _codeSnippet.Snippet.Code.Language %>
                                         Kind=<%= _codeSnippet.Snippet.Code.Kind %>
                                         Delimiter=<%= _codeSnippet.Snippet.Code.Delimiter %>
                                         ><%= xcdCode %></Code>
                                   </Snippet>
                                </CodeSnippet>
                             </CodeSnippets>


      doc.Save(filename)
      _filename = filename

      Return True
   End Function


   ''' <summary>
   ''' loads a snippet from a file
   ''' </summary>
   ''' <param name="filename">file to load the snippet from</param>
   Public Sub LoadFile(ByVal filename As String)
      Dim cs As CodeSnippet = FileToCodeSnippet(filename)
      If cs IsNot Nothing Then
         _codeSnippet = cs
         _filename = filename
      End If
   End Sub


   ''' <summary>
   ''' load a file and returns a codesnippet
   ''' </summary>
   Private Function FileToCodeSnippet(ByVal filename As String) As CodeSnippet
      Dim cs As CodeSnippet = Nothing
      Try
         Dim doc = XDocument.Load(filename)
         Dim el = doc...<CodeSnippet>.FirstOrDefault
         If el Is Nothing Then Return Nothing
         cs = New CodeSnippet(el)
      Catch ex As Exception
         'TODO: should remove MsgBox on errors or have a UI switch.
         MsgBox(ex.Message, MsgBoxStyle.Critical, My.Resources.Error_Loading_Xml_File)
      End Try
      Return cs
   End Function


   ''' <summary>
   ''' determines if the snippet has changed since the last save
   ''' </summary>
   Public Function SnippetHasChanged() As Boolean

      If (_codeSnippet Is Nothing) Then
         Return False
      End If

      Dim cs As CodeSnippet

      If _filename = "" Then
         cs = New CodeSnippet
         Return Not cs.Equals(_codeSnippet)

      Else 'The snippet has been saved before
         cs = FileToCodeSnippet(_filename)
         If cs IsNot Nothing Then
            Return Not cs.Equals(_codeSnippet)
         Else
            Return True
         End If
      End If
   End Function


   ''' <summary>
   ''' creates a new CodeSnippet
   ''' </summary>
   Public Sub CreateNewSnippet()
      _codeSnippet = New CodeSnippet()
      _filename = ""
   End Sub

End Class
