<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CrossPagePostback_vb.aspx.vb" Inherits="_01_CrossPagePostback_CrossPagePostback_vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <h4 align="right">
            Search:&nbsp;<asp:TextBox ID="SearchTerm" Runat="server"></asp:TextBox>
            <asp:Button ID="Button2" Runat="server" Text="Search" PostBackUrl="search.aspx" />
        </h4>
        <h1>
            <hr />
        </h1>
        <h1>
            <font color="#ff0033">Cross Page Posting (Page1.aspx)</font></h1>
        <p>
            <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                <tr valign="top">
                    <td width="100">
                        Enter Name:</td>
                    <td width="100">
                        <asp:TextBox ID="TextBox1" Runat="server"></asp:TextBox>
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="100">
                        Meeting Date:
                    </td>
                    <td width="100">
                        <asp:Calendar ID="Calendar1" Runat="server" Font-Names="Verdana" Font-Size="8pt"
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
                        <asp:Button ID="Button1" Runat="server" Text="Postback to Same Page" />
                        <br />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="Label1" Runat="server" Font-Size="Large"></asp:Label>
            <br />
            <br />
            &nbsp;&nbsp;<br />
            <br />
        </p>
    </form>
</body>
</html>
