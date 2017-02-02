<%@ Register TagPrefix="uc1" TagName="sitemenu" Src="sitemenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="siteheader" Src="siteheader.ascx" %>
<%@ Page language="c#" Codebehind="WordClass.aspx.cs" AutoEventWireup="false" Inherits="WordApplication.WordClass" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>The class CCWordApp</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="StyleSheet1.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="CreateDocModel" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<!-- riga in alto -->
				<tr>
					<td><uc1:siteheader id="Siteheader2" runat="server"></uc1:siteheader></td>
				</tr>
				<tr>
					<td>
						<table class="DATAINFO" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<!-- menu lato sx -->
								<td vAlign="top" width="192"><uc1:sitemenu id="Sitemenu1" runat="server"></uc1:sitemenu></td>
								<td vAlign="top" width="2" bgColor="graytext"><IMG src="Spacer.gif"></td>
								<td vAlign="top" width="10" bgcolor="#ffffff"><IMG src="Spacer.gif"></td>
								<!-- inizio parte centrale -->
								<td vAlign="top">
									<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TBODY class="TITLEINFO">
											<tr>
												<td class="TITLEINFO" vAlign="top" align="middle">CCWord Class<br>
													&nbsp;
												</td>
											</tr>
											<tr>
												<td>
													<table cellSpacing="0" cellPadding="0" border="0">
														<tr>
															<td class="DATAINFO" colspan="3"><FONT color="red">Private Members</FONT>
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">private Word.ApplicationClass WordApplic
															</td>
															<td class="DATAINFO">it's a reference to the COM object of Microsoft Word 
																Application
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">private Word.Document oDoc
															</td>
															<td class="DATAINFO">it's a reference to the document in use
															</td>
														</tr>
														<tr>
															<td class="DATAINFO" colspan="3"><FONT color="red">Public functio</FONT>
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">CCWordApp ()
															</td>
															<td class="DATAINFO">
																Activate the interface with the COM object of Microsoft Word
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">Open ()
															</td>
															<td class="DATAINFO">Open a new document
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">Open (string strFileName)
															</td>
															<td class="DATAINFO">Open an existing file or open a new file based on a template
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																Quit&nbsp;()
															</td>
															<td class="DATAINFO">
																Deactivate the interface with the COM object of Microsoft Word
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">Save ()
															</td>
															<td class="DATAINFO">
																Save the document
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">SaveAs(string strFileName)
															</td>
															<td class="DATAINFO">
																Save the document with a new name
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">CCWordApp SaveAsHtml(string strFileName)
															</td>
															<td class="DATAINFO">
																Save the document with a new name as HTML document
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">InsertText( string strText)
															</td>
															<td class="DATAINFO">
																Insert text
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">InsertLineBreak( )
															</td>
															<td class="DATAINFO">
																Insert Line Break
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">InsertLineBreak( int nline)
															</td>
															<td class="DATAINFO">
																Insert multiple Line Break
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" valign="top" style="WIDTH: 271px">SetAlignment(string strType 
																)
															</td>
															<td class="DATAINFO">
																Set the paragraph alignement.<br>
																Possible values of strType :<br>
																"Center", "Right", "Left", "Justfy".
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" valign="top" style="WIDTH: 271px">SetFont( string strType )
															</td>
															<td class="DATAINFO">
																Set the font style.<br>
																Possible values of strType :<br>
																"Bold","Italic,"Underlined".
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																SetFontName( string strType )
															</td>
															<td class="DATAINFO">
																Set the font name.
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																SetFontSize( int nSize )
															</td>
															<td class="DATAINFO">
																Set the font dimension.
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																InsertPagebreak()
															</td>
															<td class="DATAINFO">
																Insert a page break.
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																GotoBookMark(string strBookMarkName)
															</td>
															<td class="DATAINFO">
																Go to a specific bookmark.
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																GoToTheEnd( )
															</td>
															<td class="DATAINFO">
																Go to the end of document.
															</td>
														</tr>
														<tr>
															<td width="90"></td>
															<td class="DATAINFO" style="WIDTH: 271px">
																GoToTheBeginning( )
															</td>
															<td class="DATAINFO">
																Go to the&nbsp;beginning of document.
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</TBODY>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			</TD></TR></TABLE></form>
	</body>
</HTML>
