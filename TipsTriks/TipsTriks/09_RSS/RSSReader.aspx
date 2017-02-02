<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RSSReader.aspx.vb" Inherits="_09_RSS_RSSReader" %>
<%@ Register Assembly="RssToolkit" Namespace="RssToolkit" TagPrefix="RssToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <h1>Recent ScottGu Posts</h1>

    <asp:DataList ID="MyFeeds" DataSourceID="MyRssFeed" runat="server">
    
        <ItemTemplate>
        
            <a href='<%# Eval("link") %>'>
                <%# Eval("title") %>
            </a>
        
        </ItemTemplate>
    
    </asp:DataList>
    
    <RssToolkit:RssDataSource ID="MyRssFeed"  
                              Url="http://weblogs.asp.net/scottgu/rss.aspx" 
                              MaxItems="10" 
                              runat="server" />
    
    
    <div style="margin-top:50px">
        <RssToolkit:RssHyperLink ID="RssLink" 
                                 NavigateUrl="~/09_RSS/RssFeed.ashx" 
                                 Text="Click here to subscribe to our products" runat="server" />
    </div>
    
    </div>
    </form>
</body>
</html>
