<%@ Page Title="New Employee" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="NewEmployee.aspx.cs" Inherits="NewEmployee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="sm1" />
    <script type="text/javascript" src="JavaScript.js"></script>
    <h3 style="color: gold">
        <asp:Literal ID="litUser" runat="server"></asp:Literal>
    </h3>
    
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="TableCell1" runat="server">
                <asp:Panel ID="Panel1" runat="server" CssClass="button" Width="80px" onclick="collapse()">Collapse</asp:Panel>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Panel ID="Panel2" runat="server" CssClass="button" Width="80px" onclick="expand()">Expand</asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Pre-Configuration !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div1" class="OuterDiv" runat="server">
        <h1>Complete Pre-Configuration Steps Before Continuing Checklist</h1>
        <h2>
            <asp:CheckBox ID="cbPreConfig" Text=" Pre-Configuration     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbPreConfig, PreConfigDiv)" />
        </h2>
        <div class="InnerDiv" id="PreConfigDiv">
            <ol class="listIndent">
                <li>Enter Computer Name using our Standard naming Convention: <strong>W8-USERID</strong> (all <span class="highlight">UPPERCASE!</span>,
                    first initial, last name up to eight characters)</li>
                <li>The computer will autologon as IMBAdmin where you can join the domain <strong>ITServices</strong></li>
                <li>Join The Domain</li>
                <ol class="listIndent" type="a">
                    <li>Open Windows Explorer and browse to <strong>C:\Windows\OEM</strong>
                        <asp:HyperLink ID="hlOEM" runat="server" Target="_blank" CssClass="decoration">Click Here</asp:HyperLink></li>
                    <li><strong>Right-Click</strong> on joinDomain-ITSERVICES-W8.bat and click <strong>Run As Administrator</strong></li>
                    <li>The System will REBOOT</li>
                </ol>
            </ol>
        </div>
    </div>
    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! BIOS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div2" class="OuterDiv" runat="server">
        <h1>Begin Configuration on New Machine</h1>
        <h2>
            <asp:CheckBox ID="cbBIOS" Text=" BIOS Updates         " runat="server" ClientIDMode="Static" onclick="hideDiv(cbBIOS, BIOSDiv)" />
            <img src="../Images/BIOS.jpg" alt="BIOS Update" />
        </h2>
        <div class="InnerDiv" id="BIOSDiv">
            <ol class="listIndent">
                <li>Log into the machine as the new user</li>
                <li>Update BIOS</li>
                <ol class="listIndent" type="a">
                    <li>Click on <strong>Update BIOS</strong> link.
                        <asp:HyperLink ID="hlBIOS" runat="server" CssClass="decoration">Click Here</asp:HyperLink></li>
                    <li>Right-click the <strong>1-Optiplex-7010-BIOS-Update-A16.exe</strong>, select <strong>Run As Administrator</strong></li>
                    <li>Click <strong>OK</strong>, to verify <strong>Update</strong></li>
                    <li>Click <strong>OK</strong>, at the <strong>"Do You Wish to Proceed.."</strong> message</li>
                    <li><strong>BIOS upgrade will Run and Reboot Machine</strong></li>
                </ol>
                <li>Update Firmware
                    <asp:HyperLink ID="hlFirmware" runat="server" Target="_blank" CssClass="decoration">run</asp:HyperLink></li>
                <ol class="listIndent" type="a">
                    <li>Click on the <strong>BIOS-Update File Explorer Window</strong></li>
                    <li>Double click <strong>2-BIOS-Settings-OP7010-Basic.exe</strong></li>
                    <li>Close <strong>BIOS-Update File Explorer Window</strong></li>
                </ol>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Windows Updates !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div3" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbWinUpdate" Text=" Update Windows     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbWinUpdate, WinUpdateDiv)" />
            <img src="../Images/WinUpdate.GIF" alt="Windows Updates" />
        </h2>
        <div class="InnerDiv" id="WinUpdateDiv">
            <ol class="listIndent">
                <li>Go to the Windows 8 Start Menu by clicking on the Windows Key.</li>
                <li>Begin typing <strong>"Windows Update"</strong> and select the icon when it populates in the search.</li>
                <li>Select check for updates and install any available updates. A reboot may be necessary.</li>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Graphics Drivers !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div4" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbGraphics" Text=" Confirm Graphics Drivers       " runat="server" ClientIDMode="Static" onclick="hideDiv(cbGraphics, GraphicsDiv)" />
            <img src="../Images/DeviceMgr.GIF" alt="Confirm Graphics Drivers" />
        </h2>
        <div class="InnerDiv" id="GraphicsDiv">
            <ol class="listIndent">
                <li>Go to the Windows 8 Start Menu by clicking on the Windows Key.</li>
                <li>Begin typing <strong>"Device Manager"</strong> and select the icon when it populates in the search.</li>
                <li>Perform the following tasks <strong>ONLY</strong> if the specific adapter has a little yellow tringle w/ exclamation.</li>
                <ol type="a" class="listIndent">
                    <li>Right click on the display adapter and select <strong>Uninstall</strong>.</li>
                    <li>Select the <strong>"Delete the driver software for this device"></strong> checkbox.</li>
                    <li>Reboot.</li>
                    <li>After reboot, go back to <strong>Device Manager.</strong></li>
                    <li>Expand <strong>"Display Adapters"</strong> if its not already expanded.</li>
                    <li>Right click and select <strong>"Update Device Drivers..."</strong></li>
                    <li>Select <strong>"Search automatically for updated driver software"</strong></li>
                    <li>Reboot.</li>
                </ol>
                <li>Right click on the Desktop, select <strong>"Screen Resolution"</strong> and verify it is set to <strong>"(Recommended)"</strong></li>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Sound !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div5" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbSound" Text=" Confirm Sound     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbSound, SoundDiv)" />
            <img src="../Images/Sound.jpg" alt="Sound" />
        </h2>
        <div class="InnerDiv" id="SoundDiv">
            <ol class="listIndent">
                <li>Right click speaker icon on the far right of the Taskbar and select <strong>Sounds</strong></li>
                <li>Select <strong>"Windows Default"</strong> from Sound Schemes. If a <strong>"Save previous scheme"</strong> window pops up, select No — and then select <strong>Apply.</strong></li>
                <li>Select a sound from the <strong>"Program Events:"</strong> list and press <strong>"Test"</strong> to verify you can hear system sounds.</li>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Lync, Word & Outlook !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div id="Div6" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbMS" Text=" Configure Lync, Word & Outlook" runat="server" ClientIDMode="Static" onclick="hideDiv(cbMS, MSDiv)" />
            <img src="../Images/Office.bmp" alt="MS Office" />
        </h2>
        <div class="InnerDiv" id="MSDiv">
            <ol class="listIndent">
                <li>Open <asp:HyperLink ID="hlLync" runat="server" CssClass="decoration">Lync</asp:HyperLink></li>
                <li>Open <asp:HyperLink ID="hlWord" runat="server" CssClass="decoration">Word</asp:HyperLink></li>
                <ol class="listIndent" type="a">
                    <li>Close the wizard by clicking on the <strong>"X"</strong> in the upper right corner of the wizard.</li>
                    <li>Double click on <strong>"Blank Document"</strong></li>
                    <li>Close the <strong>Document Recovery Window</strong> on the left panel by clicking the <strong>Close</strong> button in the bottom right corner.</li>
                    <li>Close Word.</li>
                </ol>
                <li>Open <asp:HyperLink ID="hlOutlook" runat="server" CssClass="decoration">Outlook</asp:HyperLink></li>
                <li>Go to File &rarr; options</li>
                <ol class="listIndent" type="a">
                    <li>Under <strong>Personalize your copy of Microsoft Office</strong>, remove <strong>"@Calrecycle"</strong> from the <strong>User name</strong> field.</li>
                    <li>Enter <strong>User Intitials.</strong></li>
                </ol>
                <li>Add any Shared Mailboxes listed on the checklist.</li>
                <ol class="listIndent" type="a">
                    <li>Click File &rarr; Account Settings &rarr; Account Settings &rarr; Double click on email &rarr; More Settings &rarr; Advanced Tab &rarr; Enter mailbox name.</li>
                </ol>
                <li>Close Outlook.</li>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Printers !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div class="OuterDiv">
        <h2>
            <asp:CheckBox ID="cbPrinters" Text=" Install Printers" runat="server" ClientIDMode="Static" onclick="hideDiv(cbPrinters, PrintersDiv)" />
            <img src="../Images/Printers.GIF" alt="Printers" />
        </h2>
        <div class="InnerDiv" id="PrintersDiv">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" style="margin-left: 1.9cm;">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddFloor" />
                </Triggers>
                <ContentTemplate>
                    Floor: &nbsp;<asp:DropDownList runat="server" AutoPostBack="true" ID="ddFloor" OnSelectedIndexChanged="ddFloor_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                    Printer: &nbsp;<asp:DropDownList runat="server" ID="ddPrinter"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnInstallPrinter" OnClick="btnInstallPrinter_Click" Text="Install" BackColor="#33CC33" BorderStyle="Double" Width="67px"/>
                </ContentTemplate>
                </asp:UpdatePanel>
            
           <!-- <ol class="listIndent">
                <li>Note the printers that need to be added.</li>
                <li>Go to the Windows 8 Start Menu by clicking on the Windows Key.</li>
                <li>Begin typing <strong>"Devices and Printers"</strong> and select the icon when it populates in the search.</li>
                <li>Click <strong>Add a Printer</strong></li>
                <li>Click <strong>The printer that I want isn't listed</strong></li>
                <li>Click on the <strong>Find a printer in the directory, based on location or feature</strong> radio button &rarr; Next.</li>
                <li>Type the floor number in the Name text box &rarr; Click <strong>Find Now</strong></li>
                <li>Double click on the printer to be added from the search results. Repeat steps for additional printers.</li>
            </ol> -->
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Special Software !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div class="OuterDiv">
        <h2>
            <asp:CheckBox ID="cbSoftware" Text=" Special Software" runat="server" ClientIDMode="Static" onclick="hideDiv(cbSoftware, SoftwareDiv)" />
        </h2>
        <div class="InnerDiv" id="SoftwareDiv">
            <ol class="listIndent">
                <li>Special software that can be installed via
                    <asp:HyperLink ID="hlSoftware" runat="server" Target="_blank" CssClass="decoration">S:\Ciwmb-Infotech\Software2</asp:HyperLink>&nbsp;
                    such as VanWrite, Reflections, SnagIt and Dymo printer drivers. Acrobat Pro and Visio are elevated to Paras in NSS for SCCM installation.</li>
                <%-- Links for Special Software instructions? --%>
            </ol>
        </div>
    </div>

    <%--
        !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Clean Up !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    --%>
    <div class="OuterDiv">
        <h2>
            <asp:CheckBox ID="cbClean" Text=" Clean up Desktop Shortcuts & Related Tasks & Misc." runat="server" ClientIDMode="Static" onclick="hideDiv(cbClean, CleanDiv)" />
        </h2>
        <div class="InnerDiv" id="CleanDiv">
            <ol class="listIndent">
                <li>If there are any blank Desktop icons, right click and delete. (<strong>Deskton.ini</strong> should be deleted if shown)</li>
                <li>Empty the Recycle Bin.</li>
                <li>Verify P: and S: drives. <strong>If not call Brenda at (916) 869-7027 OR Lynelle at (916) 341-6162 or cell at (916) 873-4557</strong></li>
                <li>Update System Center Endpoint Protection Scan</li>
                <li>Shut down machine.</li>
                <li>Update Barscan inventory.</li>
                <li>Leave Lync, Citrix, and Microsoft Office 2013 Brochures on desk.</li>
            </ol>
        </div>
    </div>
</asp:Content>