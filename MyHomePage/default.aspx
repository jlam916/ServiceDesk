<%@ Page Title="MyHomePage" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="MyHomePage._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <asp:Panel ID="SoftwarePanel" runat="server" GroupingText="Software">
        <asp:Image ImageUrl="~/Icons/clean.ico" Width="20px" runat="server" />
        <asp:HyperLink ID="CleanerHL" runat="server" NavigateUrl="~/apps/Cleaner/Cleaner.htm" ForeColor="#3399FF" ToolTip="Cleans non domain images.">Cleaner</asp:HyperLink>
        <br />
        <asp:Image ImageUrl="~/Icons/Explorer.ico" Width="20px" runat="server" />
        <a href="http://W8-RKOEN/apps/FileExplorer.exe" id="ExplorerHL" title="Export permissions to excel" style="color: #3399FF" type="application/octed-stream">Advanced File Explorer</a>
        <br />
        <asp:Image ImageUrl="~/Icons/icon.ico" runat="server" Width="20px" />
        <a href="http://W8-rkoen/apps/PowerTiles.exe" id="hlTiles" title="Create Windows 8 Power Tiles" style="color: #3399FF" type="application/octed-stream">Windows 8 Tiles</a>
        <br />
        <asp:Image ImageUrl="~/Icons/remast.ico" runat="server" Width="20px" />
        <a href="http://w8-rfreeman/RemoteAssist/setup.exe" id="remoteAssist" title="Reese's Remote Assistance" style="color: #3399FF" type="application/octed/stream">Remote Assist</a>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="HelpDocsPanel" GroupingText="Help Docs" runat="server">
        Instructions for new employees
        <asp:HyperLink ID="NewEmployeeHL" NavigateUrl="/docs/W7 ITSERVICES DOMAIN Configuration Instructions.pdf" ToolTip="W7 ITSERVICES DOMAIN Configuration Instructions" ForeColor="#3399FF" runat="server">here</asp:HyperLink>.
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="PlacesPanel" GroupingText="Places" runat="server">
        <asp:HyperLink ID="NewEmployeeChecklistHL" runat="server" NavigateUrl="~/NewEmployee.aspx" ForeColor="#3399FF">New Employee Checklist</asp:HyperLink><br />
        <asp:HyperLink ID="TeleworkEmployeeChecklistHL" runat="server" NavigateUrl="~/App_Assets/Configurations/TeleworkConfigurations.aspx" ForeColor="#3399FF">Telework Configuration Checklist</asp:HyperLink><br />
        <asp:HyperLink ID="hlReservation" runat="server" NavigateUrl="~/App_Assets/Forms/Reservations.aspx" ForeColor="#3399FF">Reservations</asp:HyperLink><br />
        <asp:HyperLink ID="TeleworkUpgradeChecklistHL" runat="server" NavigateUrl="~/App_Assets/Forms/TeleworkUpgradeChecklist.aspx" ForeColor="#3399FF">Telework Upgrade Checklist</asp:HyperLink><br />
        <asp:HyperLink ID="TrainingRooms" runat="server" NavigateUrl="~/App_Assets/Forms/TrainingRooms.aspx" ForeColor="#3399FF">Training Room Connections</asp:HyperLink>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="UtilsPanel" GroupingText="Utilities" runat="server">
        <a href="utils/VZ_Updater.exe" title="VZAccess Manager" style="color: #3399FF" type="application/octed-stream">Verizon Access Manager</a>
        <br />
        <a href="utils/CCleaner.exe" title="CCleaner" style="color: #3399FF" type="application/octed-stream">CCleaner</a>
    </asp:Panel>
</asp:Content>