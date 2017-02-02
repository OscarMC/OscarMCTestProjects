<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClientClick.aspx.vb" Inherits="ClientClick" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Client Click Event</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <h3>Client Click Event</h3>
      
      <asp:Button ID="Button1" 
                  OnClientClick='javascript:alert("clicked!")' 
                  OnClick="Button1_Click"
                  Text="Click Me!" 
                  runat="server" />
      
      <br />
      <br />
      
      <asp:Label ID="Label1" runat="server" Font-Size="16pt"></asp:Label>
      
    </div>
    </form>
</body>
</html>
