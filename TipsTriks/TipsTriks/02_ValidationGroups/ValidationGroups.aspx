<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ValidationGroups.aspx.vb"
    Inherits="ValidationGroups" %>

<html>
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="Form1" runat="server">
    
        <h4 align="right">
            Search: <asp:TextBox ID="SearchTerm" runat="server"></asp:TextBox>
            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Search Empty!"
                ControlToValidate="SearchTerm" Display="Dynamic" ValidationGroup="search">
            </asp:RequiredFieldValidator>
            
            <asp:Button ID="Button2" runat="server" Text="Search" PostBackUrl="search.aspx" ValidationGroup="search" />
        </h4>
        
        <hr />
        
        <h1><font color="#ff0033">Validation Groups (Page1.aspx)</font></h1>
        
        <p>
            <table id="Table1" cellspacing="1" cellpadding="1" border="0" style="width: 592px;
                height: 294px">
                <tr valign="top">
                
                    <td width="100">Enter Name:</td>
                
                    <td style="width: 392px">
                    
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name not entered!"
                            ControlToValidate="TextBox1" Font-Bold="True">
                        </asp:RequiredFieldValidator>
                        
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="100">
                        Meeting Date:
                    </td>
                    <td style="width: 392px">
                        <asp:Calendar ID="Calendar1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                            ForeColor="Black" Height="180px" DayNameFormat="FirstLetter" Width="200px" BorderColor="#999999"
                            CellPadding="4" BackColor="White">
                            <WeekendDayStyle BackColor="#FFFFCC"></WeekendDayStyle>
                            <TodayDayStyle ForeColor="Black" BackColor="#CCCCCC"></TodayDayStyle>
                            <SelectedDayStyle ForeColor="White" BackColor="#666666" Font-Bold="True"></SelectedDayStyle>
                            <DayHeaderStyle BackColor="#CCCCCC" Font-Size="7pt" Font-Bold="True"></DayHeaderStyle>
                            <TitleStyle BorderColor="Black" BackColor="#999999" Font-Bold="True"></TitleStyle>
                            <SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
                            <NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
                            <OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
                        </asp:Calendar>
                        
                        <br />
                        <asp:Button ID="Button1" runat="server" Text="Postback to Same Page" OnClick="Button1_Click" />
                        <br />
                        
                    </td>
                </tr>
            </table>
            
            <br />
            
            <asp:Label ID="Label1" runat="server" Font-Size="Large"></asp:Label>
            
        </p>
    </form>
</body>
</html>
