<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="ADTools.aspx.cs" Inherits="App_Assets_Forms_ADTools" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="sm1" />
    <script src="http://code.jquery.com/jquery-latest.min.js"></script>
    <script type="text/javascript" src="JS/ADTools.js"></script>

    <style>
    </style>

    <asp:Image runat="server" AlternateText="CalRecycleHeader" ImageUrl="~/App_Assets/Forms/CalRecycleLogo/HeaderAD.jpg" CssClass="Image" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="ChoicePanel" runat="server" CssClass="ChoicePanel">
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button CssClass="btnUser" runat="server" ID="btnUser" ClientIDMode="Static" Text="Manage Users" OnClick="btnUser_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button CssClass="btnComputer" runat="server" ID="btnComputer" ClientIDMode="Static" Text="Manage Computers" OnClick="btnComputer_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Panel ID="UserPanel" runat="server" CssClass="UserPanel">
                User Name:
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

                <fieldset>
                    <legend>Details</legend>
                </fieldset>
                <fieldset>
                    <legend>Actions</legend>
                </fieldset>
            </asp:Panel>
            <asp:Panel ID="ComputerPanel" runat="server" CssClass="ComputerPanel">
                Computer Name:
            <asp:TextBox ID="ComputerTextBox" runat="server" Height="16px"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender ID="autoCompleteExtender" TargetControlID="ComputerTextBox" runat="server" ServiceMethod="GetCompletionList"
                    UseContextKey="True" CompletionSetCount="10" MinimumPrefixLength="3">
                </ajaxToolkit:AutoCompleteExtender>
                <asp:Button ID="fillBtn" runat="server" Text="Fill" OnClick="fillBtn_Click" />
                <fieldset>
                    <legend>Details</legend>
                    <asp:Table runat="server" BackColor="WhiteSmoke" GridLines="Both" Width="100%">
                        <asp:TableHeaderRow BackColor="lightblue" BorderStyle="Solid" BorderColor="Black">
                            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Last Logon</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Enabled</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Distinguished Name</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblName" ClientIDMode="Static" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblLastLogon" ClientIDMode="Static" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblEnabled" ClientIDMode="Static" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDistinguishedName" ClientIDMode="Static" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </fieldset>
                <fieldset>
                    <legend>Actions</legend>
                    <asp:Button CssClass="button" runat="server" Text="Delete" ID="DeleteComputerBtn" OnClick="DeleteComputerBtn_Click" />
                </fieldset>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>