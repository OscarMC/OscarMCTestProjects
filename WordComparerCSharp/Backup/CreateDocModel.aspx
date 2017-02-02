<%@ Page language="c#" Codebehind="CreateDocModel.aspx.cs" AutoEventWireup="false" Inherits="WordApplication.CreateDocModel" %>
<%@ Register TagPrefix="uc1" TagName="sitemenu" Src="sitemenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="siteheader" Src="siteheader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Create a new word document based on a template</title>
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
												<td class="TITLEINFO" vAlign="top" align="middle">Build a new document based on an 
													existing template
													<br>
													&nbsp;<br>
												</td>
											</tr>
											<tr>
												<td>
													<table cellSpacing="0" cellPadding="0" border="0">
														<tr>
															<td class="DATAINFO" vAlign="top" colspan="2">Template's Name : model1.dot (this 
																model has 3 bookmarks (Title, name, address)<br>
																&nbsp;<br>
															</td>
														</tr>
														<tr>
															<td class="DATAINFO" vAlign="top">Name of the new document
															</td>
															<td class="DATAINFO"><asp:textbox id="DocName" runat="server"></asp:textbox>(without 
																.doc)
																<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Insert the file name" Enabled="true" ControlToValidate="DocName"></asp:RequiredFieldValidator><br>
																<font color="red">
																	<pre class="NOTE"> The working dir must be writable</pre>
																</font>
															</td>
														</tr>
														<tr>
															<td class="DATAINFO">Title
															</td>
															<td class="DATAINFO"><asp:textbox id="TextTitle" TextMode="MultiLine" runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td>Name
															</td>
															<td><asp:textbox id="TextName" TextMode="MultiLine" runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td class="DATAINFO">Address
															</td>
															<td class="DATAINFO"><asp:textbox id="TextAddress" TextMode="MultiLine" runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td align="middle" colSpan="2"><asp:button id="Button2" runat="server" Text="OK" Width="99px"></asp:button></td>
														</tr>
														<tr height="150">
															<td align="middle"><asp:hyperlink id="HyperLink1" runat="server" Target="_blank"></asp:hyperlink></td>
															<td align="middle"><asp:hyperlink id="Hyperlink2" runat="server" Target="_blank"></asp:hyperlink></td>
														</tr>
														<tr>
															<td class="DATAINFO" align="middle" colspan="2"><asp:Label id="StatusMessage" runat="server"></asp:Label></td>
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
