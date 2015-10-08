<%@ Page Title="Telework Config" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="TeleworkConfigurations.aspx.cs" Inherits="App_Assets_Configurations_TeleworkConfigurations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">

    <script type="text/javascript" src="../../JavaScript.js"></script>
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

    <div id="Div1" class="OuterDiv" runat="server">
        <h1>Complete Pre-Configuration Steps Before Continuing Checklist</h1>
        <h2>
            <asp:CheckBox ID="cbPreConfig" Text=" Pre-Configuration" runat="server" ClientIDMode="Static" onclick="hideDiv(cbPreConfig, PreConfigDiv)" />
        </h2>
        <div class="InnerDiv" id="PreConfigDiv">
            <ol>
                <li>Gather the required components and/or component packages.</li>
                <li><strong>BARSCAN</strong> the computer to the user <strong>BEFORE</strong> taking it from the Hardware Configuration Room </li>
                <li>Label the top front of the telework computer with person's userID
             <ul>
                 <li>USERID: <strong>FirstIntialLastname</strong> (up to 8 characters)</li>
             </ul>
                </li>
            </ol>
        </div>
    </div>
    <div id="Div2" class="OuterDiv" runat="server">
        <h1>Begin Configuration on Telework Machine</h1>
        <h2>
            <asp:CheckBox ID="cbBIOS" Text=" Applying the Image" runat="server" ClientIDMode="Static" onclick="hideDiv(cbBIOS, BIOSDiv)" />
        </h2>
        <div class="InnerDiv" id="BIOSDiv">
            <ol>
                <li>Insert latest Windows 8.1 USB Drive </li>
                <li>Boot from USB</li>
                <li>Apply the Win 8.1 Custom Image </li>
                <li>After the image is applied, press "any key" to continue and then remove the USB Drive</li>
            </ol>
        </div>
    </div>

    <div id="Div6" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbName" Text=" Naming the Computer and Create User Profile" runat="server" ClientIDMode="Static" onclick="hideDiv(cbName, NameDiv)" />
        </h2>
        <div class="InnerDiv" id="NameDiv">
            <ol>

                <li>Name the computer to: HOME-USERID</li>
                <li>Create User profile
           <ul>
               <li>Go to the Windows 8 Start Menu by clicking on the Windows Key.
               </li>
               <li>Begin typing <strong>"Account"</strong> and select <strong>"Add, delete, and manage other user accounts"</strong> when it populates in the search.
               </li>
               <li>Click on <strong>"Add an Account"</strong> and select <strong>"Sign in without Microsoft account..."</strong>
               </li>
               <li>Choose <strong>"Local Account"</strong>
               </li>
               <li>User Name:  UserID
               </li>
               <li>Password: Recycle1
               </li>
               <li>Password hint: Call ITS Help Center @ 916-341-6141
               </li>
           </ul>
                    <li>Add Admins rights to User Profile
           <ul>
               <li>Select the User's Profile and click <strong>"Edit"</strong>
               </li>
               <li>Choose <strong>"Administrator"</strong>from the Account type drop down menu
               </li>
           </ul>
                    </li>
            </ol>
        </div>
    </div>

    <div id="Div3" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbWinUpdate" Text=" Update Windows     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbWinUpdate, WinUpdateDiv)" />
        </h2>
        <div class="InnerDiv" id="WinUpdateDiv">
            <ol class="listIndent">
                <li>Go to the Windows 8 Start Menu by clicking on the Windows Key.</li>
                <li>Begin typing <strong>"Windows Update"</strong> and select the icon when it populates in the search.</li>
                <li>Select check for updates and install any available updates. A reboot may be necessary.</li>
            </ol>
        </div>
    </div>

    <div id="Div4" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbJavaUpdate" Text=" Update Java     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbJavaUpdate, JavaUpdateDiv)" />
        </h2>
        <div class="InnerDiv" id="JavaUpdateDiv">
            <ol class="listIndent">
                <li>Update to "Java 8 Update 25"</li>
                <ul>
                    <li><a href="../../Downloads/Java8_25.exe" title="Java" style="color: #3399FF" type="application/octed-stream">Click Here</a> to update Java</li>
                </ul>
            </ol>
        </div>
    </div>
    <div id="Div5" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbSound" Text=" Change Internet Explorer Homepage     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbSound, SoundDiv)" />
        </h2>
        <div class="InnerDiv" id="SoundDiv">
            <ol class="listIndent">
                <li>Open Internet Explorer and change the default homepage to: <strong>https://remote.calrecycle.ca.gov</strong></li>
            </ol>
        </div>
    </div>
    <div id="Div9" class="OuterDiv" runat="server">
        <h2>
            <asp:CheckBox ID="cbTaskbar" Text=" Modify the Taskbar     " runat="server" ClientIDMode="Static" onclick="hideDiv(cbTaskbar, TaskbarDiv)" />
        </h2>
        <div class="InnerDiv" id="TaskbarDiv">
            <ol class="listIndent">
                <li>Unpin the Windows Store icon from the taskbar.</li>
                <li>Open the start menu and right-click on Lync; choose "Pin to Taskbar."</li>
                <ul>
                    <li><strong>If customer will be using Lync, open and make sure it is set to start at Startup.</strong></li>
                </ul>
            </ol>
        </div>
    </div>

    <div class="OuterDiv">
        <h2>
            <asp:CheckBox ID="cbSoftware" Text=" Special Software" runat="server" ClientIDMode="Static" onclick="hideDiv(cbSoftware, SoftwareDiv)" />
        </h2>
        <div class="InnerDiv" id="SoftwareDiv">
            <ol class="listIndent">
                <li>Install any Special Software</li>
            </ol>
        </div>
    </div>
    <div id="Div7" class="OuterDiv" runat="server">
        <h1>Customer Follow-up</h1>
        <div class="OuterDiv">
            <h2>
                <asp:CheckBox ID="cbPrinters" Text=" Contact User for Telework Machine Pickup" runat="server" ClientIDMode="Static" onclick="hideDiv(cbPrinters, PrintersDiv)" />
            </h2>
            <div class="InnerDiv" id="PrintersDiv">
                <ol class="listIndent">
                    <li>Notify the User that the PC is ready for pickup</li>
                    <li>Ensure that the user understand how to setup the equipment at home
                    </li>
                    <li>Give User the password to login
                    </li>
                    <li>If necessary, help user deliver equipment to their car
                    </li>
                </ol>
            </div>
        </div>
    </div>
</asp:Content>