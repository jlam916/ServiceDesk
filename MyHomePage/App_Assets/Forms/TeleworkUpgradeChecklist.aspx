<%@ Page Title="Telework Upgrade Checklist" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="TeleworkUpgradeChecklist.aspx.cs" Inherits="App_Assets_Forms_TeleworkUpgradeChecklist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="sm1" />
    <style>
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .Image {
        }

        .Label {
            font-size: 14px;
            font-weight: bold;
        }

        .left {
            float: left;
        }

        .rigth {
            float: right;
        }

        .full {
            margin-left: 2%;
        }

        .half {
            width: 50%;
            margin-left: 2%;
        }

        #lblMsg {
            font-size: 15px;
        }
    </style>
    <script type='text/javascript'>
        function printForm() {
            document.getElementById('<%=PrintButton.ClientID %>').style.visibility = 'hidden';
            document.getElementById('<%=OKButton.ClientID %>').style.visibility = 'hidden';
            var printContent = document.getElementById('<%=PopUpPanel.ClientID %>');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Image runat="server" AlternateText="CalRecycleHeader" ImageUrl="~/App_Assets/Forms/CalRecycleLogo/HeaderTelework.jpg" CssClass="Image" Height="260px" /><br />

            <asp:Table runat="server" CssClass="full" Width="98%">
                <asp:TableRow>
                    <asp:TableCell Width="33%" VerticalAlign="Top">
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:Label Text="User Information" runat="server" CssClass="Label"></asp:Label>
                            <br />
                            <br />
                            <asp:Table runat="server" Width="100%">
                                <asp:TableRow Width="100%">
                                    <asp:TableCell Width="11%">First Name</asp:TableCell>
                                    <asp:TableCell Width="70%">
                                        <asp:TextBox ValidationGroup="UserInfo" runat="server" Width="50%" ID="txtFName"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV1" ForeColor="Red" ControlToValidate="txtFName" ValidationGroup="UserInfo" ErrorMessage=" Enter a first name." runat="server"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Width="100%">
                                    <asp:TableCell Width="10%">Last Name</asp:TableCell>
                                    <asp:TableCell Width="70%">
                                        <asp:TextBox ValidationGroup="UserInfo" runat="server" Width="50%" ID="txtLName"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV2" ForeColor="Red" ControlToValidate="txtLName" ValidationGroup="UserInfo" ErrorMessage=" Enter a last name." runat="server"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Width="100%">
                                    <asp:TableCell Width="10%">Ticket Number</asp:TableCell>
                                    <asp:TableCell Width="70%">
                                        <asp:TextBox ValidationGroup="UserInfo" runat="server" Width="50%" ID="TxtTNumber"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV3" ForeColor="Red" ControlToValidate="TxtTNumber" ValidationGroup="UserInfo" ErrorMessage=" Enter a ticket number." runat="server"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:TableCell>

                    <asp:TableCell Width="33%" VerticalAlign="Top">
                        <asp:Panel ID="Panel3" runat="server">
                            <asp:Label Text="Equipment Requested" runat="server" CssClass="Label"></asp:Label>
                            <asp:Table runat="server" Width="100%">
                                <asp:TableRow Width="50%">
                                    <asp:TableCell>
                                        <asp:CheckBoxList ID="CheckBoxList1" RepeatColumns="5" CellSpacing="13" ClientIDMode="Static" runat="server">
                                            <asp:ListItem Text="Computer"></asp:ListItem>
                                            <asp:ListItem Text="Mouse"></asp:ListItem>
                                            <asp:ListItem Text="Wifi Adapter"></asp:ListItem>
                                            <asp:ListItem Text="Monitor"></asp:ListItem>
                                            <asp:ListItem Text="Standard Keyboard"></asp:ListItem>
                                            <asp:ListItem Text="Surge Protector"></asp:ListItem>
                                            <asp:ListItem Text="Natural Ergo Keyboard"></asp:ListItem>
                                            <asp:ListItem Text="Power Cords (2)"></asp:ListItem>
                                            <asp:ListItem Text="Network Cables"></asp:ListItem>
                                            <asp:ListItem Text="Lync Headset"></asp:ListItem>
                                            <asp:ListItem Text="Audio Speakers"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Table runat="server" CssClass="full" Width="98%">
                <asp:TableRow runat="server" Width="100%">
                    <asp:TableCell Width="33%" VerticalAlign="Top">
                        <asp:Panel runat="server">
                            <asp:Button runat="server" CssClass="button" ID="btnReset" OnClick="btnReset_Click" Text="Reset" CausesValidation="false" ToolTip="Clear fields and check boxes" />
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Image runat="server" AlternateText="CalRecycleHeader" ImageUrl="~/App_Assets/Forms/CalRecycleLogo/FooterTelework.jpg" CssClass="Image" Height="68
        px" />
            <asp:Panel Style="text-align: center" runat="server">
                <asp:Button runat="server" CssClass="button" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="UserInfo" /><br />
            </asp:Panel>
            <asp:Image runat="server" AlternateText="CalRecycleHeader" ImageUrl="~/App_Assets/Forms/CalRecycleLogo/FooterTelework.jpg" CssClass="Image" Height="68
        px" />
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateSelectButton="true" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateDeleteButton="true" OnRowDeleting="GridView1_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                <SortedDescendingHeaderStyle BackColor="#820000" />
            </asp:GridView>
            <asp:Panel ID="PopUpPanel" runat="server" Width="75%" BorderColor="Black" BorderStyle="Inset" BackColor="WhiteSmoke">
                <asp:Image runat="server" AlternateText="CalRecycleHeader" ImageUrl="~/App_Assets/Forms/CalRecycleLogo/HeaderTelework.jpg" CssClass="Image" Height="260px" /><br />

                <asp:Table runat="server" CssClass="full" Width="98%">
                    <asp:TableRow>
                        <asp:TableCell Width="33%" VerticalAlign="Top">
                            <asp:Panel ID="Panel2" runat="server">
                                <asp:Label Text="User Information" runat="server" CssClass="Label"></asp:Label>
                                <br />
                                <br />
                                <asp:Table runat="server" Width="100%">
                                    <asp:TableRow Width="100%">
                                        <asp:TableCell Width="11%">First Name</asp:TableCell>
                                        <asp:TableCell Width="70%">
                                            <asp:TextBox runat="server" Width="50%" ID="txtFNamePopUp" Enabled="false"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow Width="100%">
                                        <asp:TableCell Width="10%">Last Name</asp:TableCell>
                                        <asp:TableCell Width="70%">
                                            <asp:TextBox runat="server" Width="50%" ID="txtLNamePopUp" Enabled="false"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow Width="100%">
                                        <asp:TableCell Width="10%">Ticket Number</asp:TableCell>
                                        <asp:TableCell Width="70%">
                                            <asp:TextBox runat="server" Width="50%" ID="txtTicketNoPopUp" Enabled="false"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:TableCell>

                        <asp:TableCell Width="33%" VerticalAlign="Top">
                            <asp:Panel ID="Panel4" runat="server">
                                <asp:Label Text="Equipment Requested" runat="server" CssClass="Label"></asp:Label>
                                <asp:Table runat="server" Width="100%">
                                    <asp:TableRow Width="50%">
                                        <asp:TableCell>
                                            <asp:CheckBoxList ID="CheckBoxList2" RepeatColumns="5" CellSpacing="13" ClientIDMode="Static" runat="server" Enabled="false">
                                                <asp:ListItem Text="Computer"></asp:ListItem>
                                                <asp:ListItem Text="Mouse"></asp:ListItem>
                                                <asp:ListItem Text="Wifi Adapter"></asp:ListItem>
                                                <asp:ListItem Text="Monitor"></asp:ListItem>
                                                <asp:ListItem Text="Standard Keyboard"></asp:ListItem>
                                                <asp:ListItem Text="Surge Protector"></asp:ListItem>
                                                <asp:ListItem Text="Natural Ergo Keyboard"></asp:ListItem>
                                                <asp:ListItem Text="Power Cords (2)"></asp:ListItem>
                                                <asp:ListItem Text="Network Cables"></asp:ListItem>
                                                <asp:ListItem Text="Lync Headset"></asp:ListItem>
                                                <asp:ListItem Text="Audio Speakers"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Button ID="PrintButton" runat="server" Style="float: right" Text="Print" OnClientClick="printForm()" />
                <asp:Button ID="OKButton" runat="server" Style="float: right" Text="Close" />
            </asp:Panel>

            <%-- This pop up needs TargetControlID, use hidden button and show using c# --%>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender runat="server" ID="mpe" TargetControlID="btnShowPopup" PopupControlID="PopUpPanel" OkControlID="OKButton" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>