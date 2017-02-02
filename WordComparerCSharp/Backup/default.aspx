<%@ Register TagPrefix="Header" TagName="siteheader" Src="siteheader.ascx" %>
<%@ Register TagPrefix="Menu" TagName="sitemenu" Src="sitemenu.ascx" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="WordApplication.introduction" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>introduction</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="StyleSheet1.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="introduction" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<!-- riga in alto -->
				<tr>
					<td><HEADER:SITEHEADER id="Siteheader1" runat="server"></HEADER:SITEHEADER></td>
				</tr>
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<!-- menu lato sx -->
								<td vAlign="top" width="192"><MENU:SITEMENU id="Sitemenu1" runat="server"></MENU:SITEMENU></td>
								<td vAlign="top" width="2" bgColor="graytext"><IMG src="Spacer.gif"></td>
								<td vAlign="top" width="10" bgColor="white"><IMG src="Spacer.gif"></td>
								<!-- inizio parte centrale -->
								<td vAlign="top">
									<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TBODY>
											<tr>
												<td align="middle" colSpan="2" height="40">Working with Microsoft Word and ASP.NET<br>
												</td>
											</tr>
											<tr>
												<td vAlign="center" colSpan="2" width="700" class="DATAINFO"><b>Please check the 
														Web.config file and change the name of the directory if necessary.</b>
													<br>
													&nbsp;
												</td>
											</tr>
											<tr>
												<td vAlign="center" colSpan="2" width="700" class="DATAINFO">The First Step in 
													manipulating Word in .NET is that you'll need to add a COM reference to your 
													project by right clicking in the solution explorer onReferences-&gt;Add 
													Reference.&nbsp;<br>
													Click on the COM tab and look for the Microsoft Word 10.0 Object Library.&nbsp; 
													Click Select and OK.<br>
													To try to understand how to do operations in Microsoft Word,&nbsp; turn on the 
													Macro Recorder in Word and let Word write the code .&nbsp; (It'is VB code, but 
													it helps to understand how to implement your function).&nbsp; To start the 
													macro recorder in Word, go to&nbsp; <B>Tools-&gt;Macro-&gt;Record New Macro</B> 
													inside Microsoft Word.<br>
													Now anything you type, open, delete, or format, will get recorded in VB, so you 
													have a clue how to write your Word Interoperability code.<br>
												</td>
											</tr>
											<tr>
												<td vAlign="center" colSpan="2" height="40" class="DATAINFO">&nbsp;&nbsp;Here you 
													can find information :
												</td>
											</tr>
											<tr class="DATAINFO">
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink2" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://support.microsoft.com/default.aspx?scid=http://support.microsoft.com:80/support/kb/articles/q222/1/01.asp&amp;NoWebContent=1">HOWTO: Find and Use Office Object Model Documentation</asp:hyperlink><br>
												</td>
											</tr>
											<tr class="DATAINFO">
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink4" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnofftalk/html/office10032002.asp">A Primer to the Office XP Primary Interop Assemblies </asp:hyperlink><br>
												</td>
											</tr>
											<tr class="DATAINFO">
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink3" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnoxpta/html/odc_piaissues.asp">Office XP Primary Interop Assemblies Known Issues </asp:hyperlink><br>
												</td>
											</tr>
											<tr class="DATAINFO">
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink5" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://support.microsoft.com/default.aspx?scid=http://support.microsoft.com:80/support/kb/articles/Q301/6/56.ASP&amp;NoWebContent=1">HOWTO: Automate Microsoft Word to Perform a Mail Merge from Visual Basic .NET</asp:hyperlink><br>
												</td>
											</tr>
											<tr>
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink6" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/off2000/html/wotocObjectModelApplication.asp"> Microsoft Word Objects</asp:hyperlink><br>
												</td>
											</tr>
											<tr>
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink1" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://support.microsoft.com/default.aspx?scid=http://support.microsoft.com:80/support/kb/articles/q302/4/60.asp&amp;NoWebContent=1"> OFFXP: Microsoft Office XP Automation Help File Available</asp:hyperlink><br>
												</td>
											</tr>
											<tr>
												<td class="DATAINFO">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:hyperlink id="Hyperlink7" runat="server" Target="_blank" CssClass="LINK" NavigateUrl="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnofftalk/html/office11072002.asp">Converting Microsoft Office VBA Macros to Visual Basic .NET and C#	</asp:hyperlink><br>
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
