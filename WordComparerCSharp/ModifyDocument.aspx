<%@ Register TagPrefix="uc1" TagName="siteheader" Src="siteheader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="sitemenu" Src="sitemenu.ascx" %>
<%@ Page language="c#" Codebehind="ModifyDocument.aspx.cs" AutoEventWireup="True" Inherits="WordApplication.ModifyDocument" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Modify an existing document</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="StyleSheet1.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="ModifyDocument" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<!-- riga in alto -->
				<tr>
					<td><uc1:siteheader id="Siteheader1" runat="server"></uc1:siteheader></td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<!-- menu lato sx -->
								<td vAlign="top" width="192"><uc1:sitemenu id="Sitemenu1" runat="server"></uc1:sitemenu></td>
								<td vAlign="top" width="2" bgColor="graytext"><IMG src="Spacer.gif"></td>
								<td vAlign="top" width="10" bgColor="#ffffff"><IMG src="Spacer.gif"></td>
								<!-- inizio parte centrale -->
								<td vAlign="top">
									<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TBODY class="TITLEINFO">
											<tr>
												<td class="TITLEINFO" vAlign="top" align="middle">Change a new document appending a 
													string at the end of it
													<br>
													&nbsp;<br>
												</td>
											</tr>
											<tr>
												<td>
													<table cellSpacing="0" cellPadding="0" border="0">
														<tr>
															<td class="DATAINFO" vAlign="top">Name of the&nbsp;document
															</td>
															<td class="DATAINFO"><asp:textbox id="DocName" runat="server"></asp:textbox>(without 
																.doc and directory name )
																<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="DocName" Enabled="true" ErrorMessage="Insert the file name"></asp:requiredfieldvalidator><br>
																<font color="red">
																	<pre class="NOTE"> The working dir must be writable</pre>
																</font>
															</td>
														</tr>
														<tr>
															<td class="DATAINFO">Text
															</td>
															<td class="DATAINFO"><asp:textbox id="Text" runat="server" Height="102px" Width="352px" TextMode="MultiLine"></asp:textbox></td>
														</tr>
														<tr>
															<td align="middle" colSpan="2"><asp:button id="Button2" runat="server" Width="99px" Text="OK"></asp:button></td>
														</tr>
														<tr height="150">
															<td align="middle"><asp:hyperlink id="HyperLink1" runat="server" Target="_blank"></asp:hyperlink></td>
															<td align="middle"><asp:hyperlink id="Hyperlink2" runat="server" Target="_blank"></asp:hyperlink></td>
														</tr>
														<tr>
															<td align="middle" colSpan="2" class="DATAINFO"><asp:label id="StatusMessage" runat="server"></asp:label>
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
