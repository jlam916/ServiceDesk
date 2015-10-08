<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="PollService.aspx.cs" Inherits="PollService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <asp:Label ID="lblDate" runat="server"></asp:Label>
    <asp:GridView runat="server" ID="gridView" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Center" />
    </asp:GridView>
</asp:Content>