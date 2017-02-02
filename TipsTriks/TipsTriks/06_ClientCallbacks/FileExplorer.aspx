<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileExplorer.aspx.cs" Inherits="FileExplorer" %>

<html>
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <h2>Populating TreeView Nodes On-Demand</h2>

        <asp:TreeView Id="MyTree" 
                      PathSeparator = "|"
                      OnTreeNodePopulate="PopulateNode"
                      ExpandDepth="0"
                      runat="server" ImageSet="XPFileExplorer" NodeIndent="15">
        
            <SelectedNodeStyle BackColor="#B5B5B5"></SelectedNodeStyle>
            <NodeStyle VerticalPadding="2" Font-Names="Tahoma" Font-Size="8pt" HorizontalPadding="2" ForeColor="#000000"></NodeStyle>
            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA"></HoverNodeStyle>

            <Nodes>
                <asp:TreeNode Text="Demos" PopulateOnDemand="True" Value="Demos" />
            </Nodes>

        </asp:TreeView>    
    
    </div>
    </form>
</body>
</html>

