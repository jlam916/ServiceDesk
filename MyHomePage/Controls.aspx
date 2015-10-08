<%@ Page Title="Command Center" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="Controls.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="sm1" />
    <style>
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            Computer Name:
            <asp:TextBox ID="ComputerTextBox" runat="server" Height="16px"></asp:TextBox>
            <ajaxToolkit:AutoCompleteExtender ID="autoCompleteExtender" TargetControlID="ComputerTextBox" runat="server" ServiceMethod="GetCompletionList"
                UseContextKey="True" CompletionSetCount="10" MinimumPrefixLength="5">
            </ajaxToolkit:AutoCompleteExtender>
            <asp:Image ID="pingImage" runat="server" Visible="false" />&nbsp;&nbsp;
            <asp:Button ID="SearchBtn" runat="server" Text="Search" OnClick="SearchBtn_Click" Width="100px" />
            <asp:Panel GroupingText="QuickFix" runat="server">
                <asp:Button ID="MapDrivesBtn" runat="server" OnClick="Button1_Click1" Text="Remap Drives" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="GPupdateBtn" runat="server" Text="GPUpdate" Width="100px" OnClick="GPupdateBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="MailProfileBtn" runat="server" Text="Mail Profile" Width="100px" OnClick="MailProfileBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="NetworkBtn" runat="server" Text="Network" Width="100px" OnClick="NetworkBtn_Click" />&nbsp;&nbsp;&nbsp;
            </asp:Panel>
            <asp:Panel GroupingText="Computer" runat="server">
                <asp:Button ID="RenameBtn" runat="server" Text="?" OnClick="RenameBtn_Click" Width="100px" Enabled="False" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="RestartBtn" runat="server" Text="Restart" OnClick="RestartBtn_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ShutdownBtn" runat="server" Text="Shutdown" Width="100px" OnClick="ShutdownBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="NewBtn" runat="server" Text="New" Width="100px" OnClick="NewBtn_Click" />
            </asp:Panel>
            <asp:Panel GroupingText="Printer" runat="server">
                <asp:Button ID="AddPrinterBtn" runat="server" Text="Add" Width="100px" OnClick="AddPrinterBtn_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="DeletePrinterBtn" runat="server" Text="Delete" Width="100px" OnClick="DeletePrinterBtn_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="LevelsBtn" runat="server" Text="Levels" Width="100px" OnClick="LevelsBtn_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="PollBtn" runat="server" Text="Poll" Width="100px" OnClick="PollBtn_Click" Enabled="False" />
                <asp:Label runat="server" Text="*Poll has been replaced by an automated web service" Font-Italic="true" ForeColor="Red" />
            </asp:Panel>
            <asp:Panel GroupingText="Update" runat="server">
                <asp:Button ID="TrainingSBtn" runat="server" Text="8TR South" Width="100px" OnClick="TrainingSBtn_Click" />&nbsp;&nbsp;&nbsp;
           <asp:Button ID="TrainingNBtn" runat="server" Text="8TR North" Width="100px" OnClick="TrainingNBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="WebconfBtn" runat="server" Text="WebConfs" Width="100px" OnClick="WebconfBtn_Click" />
            </asp:Panel>
            <asp:Panel GroupingText="Audit" runat="server">
                <asp:Button ID="ServicesBtn" runat="server" Text="Services" OnClick="servicesBtn_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ProgramsBtn" runat="server" Text="Programs" OnClick="ProgramsBtn_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="computerInfoBtn" runat="server" Text="Computer" OnClick="computerInfoBtn_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="monitorInfoBtn" runat="server" OnClick="monitorInfoBtn_Click" Text="Monitor" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="printerInfoBtn" runat="server" OnClick="printerInfoBtn_Click" Text="Printer" Width="100px" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="IPConfigBtn" Text="IPConfig" runat="server" Width="100px" OnClick="IPConfigBtn_Click" />
            </asp:Panel>
            <asp:Panel GroupingText="View" runat="server">
                <asp:Button ID="EventViewerBtn" Text="Event Viewer" runat="server" Width="100px" OnClick="EventViewerBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="CDriveBtn" Text="C: Drive" runat="server" Width="100px" OnClick="CDriveBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="RemoteAssistBtn" Text="Remote Assist" runat="server" Width="100px" OnClick="RemoteAssistBtn_Click" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ScreenshotBtn" Text="Screenshot" runat="server" Width="100px" OnClick="ScreenshotBtn_Click" Enabled="false" Visible="false" />
            </asp:Panel>
            <asp:Panel GroupingText="Install" runat="server">
                <asp:Button ID="JavaUpdateBtn" runat="server" Text="Java" Width="100px" OnClick="JavaUpdateBtn_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="JavaDoriisBtn" runat="server" Text="DORIIS Java" Width="100px" OnClick="JavaDoriisBtn_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ReflectionsInstall" Text="Reflections" runat="server" Width="100px" OnClick="ReflectionsInstall_Click" />
            </asp:Panel>
            <asp:Panel ID="PopUpPanel" runat="server" Width="500px" BorderColor="Black" BorderStyle="Inset">
                <asp:Label ID="lblComputerDetails" runat="server" Text="Audit Details" BackColor="lightblue" Width="100%" Style="text-align: center" />
                <asp:DetailsView ID="dvAuditDetails" runat="server" DefaultMode="ReadOnly" Width="100%" BackColor="WhiteSmoke"></asp:DetailsView>
                <asp:GridView ID="dvAuditGrid" runat="server" Width="100%" BackColor="WhiteSmoke" AllowPaging="True" OnPageIndexChanging="dvAuditGrid_PageIndexChanging" PageSize="20"></asp:GridView>
                <asp:Button ID="OKButton" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>
            <asp:Panel ID="programPanel" runat="server" BorderColor="Black" BorderStyle="Inset">
                <asp:UpdatePanel ID="programUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="Label1" runat="server" Text="Installed Programs" BackColor="lightblue" Width="100%" Style="text-align: center" />
                        <asp:GridView ID="programsGridView" runat="server" Width="100%" BackColor="WhiteSmoke" AllowPaging="True"
                            OnPageIndexChanging="programsGridView_PageIndexChanging" AutoGenerateColumns="false" PageSize="20"
                            SelectedRowStyle-BackColor="#33CCFF" AutoGenerateSelectButton="True" OnSelectedIndexChanged="programsGridView_SelectedIndexChanged"
                            AllowSorting="true" OnSorting="programsGridView_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Program Name" HeaderText="Program Name" />
                                <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                <asp:BoundField DataField="Installed Date" HeaderText="Installed Date" />
                                <asp:BoundField DataField="Uninstall String" HeaderText="Uninstall String" />
                                <asp:BoundField DataField="GUID" HeaderText="GUID" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="uninstallButton" runat="server" Style="float: right" Text="Uninstall" OnClick="uninstallBtn_Click" Enabled="false" />
                <asp:Button ID="OKButtonPrograms" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>
            <asp:Panel ID="printerPanel" runat="server" BorderColor="Black" BorderStyle="Inset">
                <asp:Label ID="Label2" runat="server" Text="Add a Printer" BackColor="lightblue" Width="100%" Style="text-align: center" />
                <asp:Label ID="lable" runat="server" Text="Select floor #: "></asp:Label>
                <asp:DropDownList ID="floorList" runat="server">
                    <asp:ListItem>08-</asp:ListItem>
                    <asp:ListItem>09-</asp:ListItem>
                    <asp:ListItem>10-</asp:ListItem>
                    <asp:ListItem>13-</asp:ListItem>
                    <asp:ListItem>19-</asp:ListItem>
                    <asp:ListItem>23-</asp:ListItem>
                    <asp:ListItem>24-</asp:ListItem>
                    <asp:ListItem>25-</asp:ListItem>
                    <asp:ListItem>15K-</asp:ListItem>
                    <asp:ListItem>17K-</asp:ListItem>
                    <asp:ListItem>19K-</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="EnterButton" runat="server" Text="Enter" OnClick="getPrinters_Click" />
                <br />
                <asp:Label ID="printerDropDownLabel" runat="server" Text="Select printer: " Visible="false"></asp:Label>
                <asp:DropDownList ID="printerDropDownList" runat="server" Visible="false"></asp:DropDownList>
                <asp:Button ID="InstallPrinterBtn" runat="server" Text="Install" Visible="false" OnClick="InstallPrinterBtn_Click" />
                <br />
                <asp:Button ID="OKButtonPrinter" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>
            <asp:Panel ID="deletePrinterPanel" runat="server" BorderColor="Black" BorderStyle="Inset">
                <asp:Label ID="Label3" runat="server" Text="Delete a Printer" BackColor="lightblue" Width="100%" Style="text-align: center" />
                <asp:GridView ID="printerGridView" runat="server" Width="100%" BackColor="WhiteSmoke" AutoGenerateColumns="false"
                    SelectedRowStyle-BackColor="#33CCFF" AutoGenerateSelectButton="True" OnSelectedIndexChanged="printerGridView_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Printer Name" HeaderText="Printer Name" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="OKDeleteButtonPrinter" runat="server" Style="float: right" Text="Close" />
                <asp:Button ID="uninstallPrinterBtn" runat="server" Style="float: right" Text="Uninstall" OnClick="uninstallPrinterBtn_Click" />
            </asp:Panel>
            <asp:Panel ID="PopUpPrinterPanel" runat="server" BorderColor="Black" BorderStyle="Inset" BackColor="WhiteSmoke" Width="50%">
                <asp:Label ID="Label4" runat="server" Text="Printer List" BackColor="lightblue" Width="100%" Style="text-align: center" />
                <asp:Panel ID="CenterPanel" runat="server">
                    <asp:RadioButtonList ID="rblPrinters" RepeatColumns="5" RepeatLayout="Table" runat="server" align="center"></asp:RadioButtonList>
                </asp:Panel>
                <asp:Button ID="btnLevel" runat="server" Style="float: right" Text="Levels" OnClick="btnLevel_Click" />
                <asp:Button ID="Button1" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>

            <asp:Panel ID="LevelsPanel" runat="server" Width="500px" BorderColor="Black" BorderStyle="Inset" BackColor="WhiteSmoke">
                <asp:Label ID="lblPrinter" runat="server" BackColor="lightblue" Width="100%" Style="text-align: center; font: bold" />
                <asp:Panel GroupingText="Printer Information" runat="server">
                    <asp:Label ID="lblModel" runat="server" Font-Bold="true" Font-Underline="true" /><br />
                    <asp:Table runat="server" Width="90%" Style="margin-left: 5%">
                        <asp:TableHeaderRow BackColor="#ffcc99" Font-Bold="true" runat="server" HorizontalAlign="Center">
                            <asp:TableCell ColumnSpan="2">
                                Toner Model Information
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow ID="trCyan" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblCyanPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblCyanPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trMagenta" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblMagentaPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblMagentaPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trYellow" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblYellowPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblYellowPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trBlack" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblBlackPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblBlackPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trFuser" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblFuserPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblFuserPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trTransfer" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblTransferPartKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblTransferPartValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trCyanDrum" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblCyanPartDrumKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblCyanPartDrumValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trMagentaDrum" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblMagentaPartDrumKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblMagentaPartDrumValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trYellowDrum" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblYellowPartDrumKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblYellowPartDrumValue" runat="server" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="trBlackDrum" Visible="false">
                            <asp:TableCell>
                                <asp:Label ID="lblBlackPartDrumKey" runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="lblBlackPartDrumValue" runat="server" />
                            </asp:TableCell></asp:TableRow></asp:Table><asp:HyperLink ID="hlWebServer" runat="server" />
                </asp:Panel>
                <asp:Panel GroupingText="Levels Information" runat="server">
                    <asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrCyan" Visible="false">
                            <asp:TableCell Width="12%">
                            Cyan:
                            </asp:TableCell><asp:TableCell ID="tblcCyan" BackColor="Cyan">
                                <asp:Label runat="server" ID="lblCyan" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcCyanFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrMagenta" Visible="false">
                            <asp:TableCell Width="12%">
                            Magenta:
                            </asp:TableCell><asp:TableCell ID="tblcMagenta" BackColor="Magenta">
                                <asp:Label runat="server" ID="lblMagenta" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcMagentaFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrYellow" Visible="false">
                            <asp:TableCell Width="12%">
                            Yellow:
                            </asp:TableCell><asp:TableCell ID="tblcYellow" BackColor="Yellow">
                                <asp:Label runat="server" ID="lblYellow" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcYellowFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrBlack" Visible="false">
                            <asp:TableCell Width="12%">
                            Black:
                            </asp:TableCell><asp:TableCell ID="tblcBlack" BackColor="Black">
                                <asp:Label runat="server" ID="lblBlack" Style="text-align: center" Width="100%" ForeColor="White"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcBlackFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrFuser" Visible="false">
                            <asp:TableCell Width="12%">
                                Fuser:
                            </asp:TableCell><asp:TableCell ID="tblcFuser" BackColor="MediumPurple">
                                <asp:Label runat="server" ID="lblFuser" Style="text-align: center" Width="100%" ForeColor="Black"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcFuserFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrTransfer" Visible="false">
                            <asp:TableCell Width="12%">
                                Transfer:
                            </asp:TableCell><asp:TableCell ID="tblcTransfer" BackColor="Purple">
                                <asp:Label runat="server" ID="lblTransfer" Style="text-align: center" Width="100%" ForeColor="Black"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcTransferFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrCyanDrum" Visible="false">
                            <asp:TableCell Width="20%">
                            Cyan Drum:
                            </asp:TableCell><asp:TableCell ID="tblcCyanDrum" BackColor="Cyan">
                                <asp:Label runat="server" ID="lblCyanDrum" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcCyanDrumFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrMagentaDrum" Visible="false">
                            <asp:TableCell Width="20%">
                            Magenta Drum:
                            </asp:TableCell><asp:TableCell ID="tblcMagentaDrum" BackColor="Magenta">
                                <asp:Label runat="server" ID="lblMagentaDrum" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcMagentaDrumFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrYellowDrum" Visible="false">
                            <asp:TableCell Width="20%">
                            Yellow Drum:
                            </asp:TableCell><asp:TableCell ID="tblcYellowDrum" BackColor="Yellow">
                                <asp:Label runat="server" ID="lblYellowDrum" Style="text-align: center" Width="100%"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcYellowDrumFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table><asp:Table runat="server" Width="100%">
                        <asp:TableRow Width="100%" ID="tblrBlackDrum" Visible="false">
                            <asp:TableCell Width="20%">
                            Black Drum:
                            </asp:TableCell><asp:TableCell ID="tblcBlackDrum" BackColor="Black">
                                <asp:Label runat="server" ID="lblBlackDrum" Style="text-align: center" Width="100%" ForeColor="White"></asp:Label>
                            </asp:TableCell><asp:TableCell ID="tblcBlackDrumFiller" BackColor="White"></asp:TableCell></asp:TableRow></asp:Table></asp:Panel><asp:Button ID="OKTonerBtn" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>
            <asp:Panel ID="ReflectionPanel" runat="server" BorderColor="Black" BorderStyle="Inset" BackColor="WhiteSmoke" Width="500px">
                <asp:Label runat="server" Text="Reflection Install" BackColor="lightblue" Width="100%" Style="text-align: center" />
                <asp:Panel runat="server" HorizontalAlign="Center">
                    <br />
                    <asp:Label runat="server" Text="Enter up to 3 Device Names:" /><br />
                    <asp:TextBox runat="server" ID="device1" Text="SWTN"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox runat="server" ID="device2" Text="SWTN" />
                    &nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox runat="server" ID="device3" Text="SWTN" />
                    &nbsp;&nbsp;&nbsp;&nbsp; </asp:Panel><asp:Button ID="btnReflectionInstall" runat="server" Style="float: right" Text="Install" OnClick="btnReflectionInstall_Click" />
                <asp:Button ID="ReflectionOKButton" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>

            <%-- This pop up needs TargetControlID, use hidden button and show using c# --%>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <asp:Button ID="btn1" runat="server" Style="display: none" />
            <asp:Button ID="btn2" runat="server" Style="display: none" />
            <asp:Button ID="btn3" runat="server" Style="display: none" />
            <asp:Button ID="btnShowPrinters" runat="server" Style="display: none" />
            <asp:Button ID="btnLevels" runat="server" Style="display: none" />
            <asp:Button ID="btnReflection" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpe" TargetControlID="btnShowPopup" PopupControlID="PopUpPanel" OkControlID="OKButton" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePrograms" TargetControlID="btn1" PopupControlID="programPanel" OkControlID="OKButtonPrograms" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeAddPrinter" TargetControlID="btn2" PopupControlID="printerPanel" OkControlID="OKButtonPrinter" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <%-- for Add printer popup --%>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeDeletePrinter" TargetControlID="btn3" PopupControlID="deletePrinterPanel" OkControlID="OKDeleteButtonPrinter" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <%-- for Delete printer popup --%>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpePrinters" TargetControlID="btnShowPrinters" PopupControlID="PopUpPrinterPanel" OkControlID="OKButton" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeLevels" TargetControlID="btnLevels" PopupControlID="LevelsPanel" OkControlID="OKTonerBtn" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeReflection" TargetControlID="btnReflection" PopupControlID="ReflectionPanel" OkControlID="ReflectionOKButton" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>