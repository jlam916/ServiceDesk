<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reservations.aspx.cs" Inherits="App_Assets_Forms_Reservations" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Reservations</title>
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveUrl("~/Content/bootstrap.min.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveUrl("~/Content/chosen.css") %>" />
        <link rel="stylesheet" type="text/css" href="<%=Page.ResolveUrl("~/Content/jquery.timepicker.css") %>" />
    </asp:PlaceHolder>
    <style type="text/css">
        input[type=radio] {
            margin: 8px;
            margin-bottom: 15px;
        }

        .rooms {
            width: 200px;
        }

        input[type=checkbox] {
            margin: 5px;
            margin-bottom: 15px;
        }

        .vertical-center {
            display: flex;
            align-items: center;
        }

        .focus-error {
            border-color: rgba(255, 8, 0, 0.8);
            outline: 0;
            outline: thin dotted \9;
            -moz-box-shadow: 0 0 8px rgba(255, 8, 0, 0.8);
            box-shadow: 0 0 8px rgba(255, 8, 0, 0.6) !important;
        }

        .contact {
            margin: -10px;
            padding: -10px;
        }

        #bottom, 
        #top {
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
        }

        #top {
            z-index: 10;
        }
        .btn-danger {}
        .btn-primary {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="page-header">
                <img src="CalRecycleLogo/Header.jpg" width="100%" height="150px" />
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Contact Information</h4>
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="email" ErrorMessage="Please provide an email address."
                                Display="Dynamic" SetFocusOnError="true" Text="Please provide an email address." />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Missing full email address."
                                ControlToValidate="lname" Display="Dynamic" CssClass="label label-danger" />
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="ticket" ErrorMessage="Please provide ticket number."
                                Display="Dynamic" SetFocusOnError="true" Text="Please provide ticket number." />
                            <asp:RegularExpressionValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="ticket" ErrorMessage="Invalid ticket number."
                                Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{5}" />
                            <asp:RegularExpressionValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="email" ErrorMessage="Invalid email address."
                                Display="Dynamic" SetFocusOnError="true" ValidationExpression="[a-zA-Z]*[.][a-zA-Z]*" />
                        </div>
                        <div class="panel-body" style="height: 210px">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2">Email</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="email" placeholder="First.Last"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="autoCompleteExtender" TargetControlID="email" runat="server" ServiceMethod="GetCompletionList"
                                                UseContextKey="True" CompletionSetCount="10" MinimumPrefixLength="1">
                                            </asp:AutoCompleteExtender>
                                            <span class="input-group-addon">@CalRecycle.ca.gov</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2">First</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="fname" placeholder="John"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2">Last</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="lname" placeholder="Doe"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2">Ticket</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="ticket" placeholder="12345" MaxLength="5"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Meeting Information</h4>
                            <asp:RegularExpressionValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="calBtn" ErrorMessage="Please provide meeting date."
                                Display="Dynamic" SetFocusOnError="true"
                                ValidationExpression="[0-9]*/[0-9]*/[0-9]{4}" />
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="ticket" ErrorMessage="Please provide start time."
                                Display="Dynamic" SetFocusOnError="true" />
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="ticket" ErrorMessage="Please provide end time."
                                Display="Dynamic" SetFocusOnError="true" />
                        </div>
                        <div class="panel-body vertical-center" style="height: 210px">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-3">Date</label>
                                    <div class="col-sm-9">
                                        <asp:ToolkitScriptManager ID="tsm" runat="server" />
                                        <asp:TextBox CssClass="btn btn-default pull-right" ID="calBtn" runat="server" Style="width: 150px">Click Here</asp:TextBox>
                                        <asp:CalendarExtender ID="calExt" TargetControlID="calBtn" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5">
                                        Start (Actual Begin Time)</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="startTime" CssClass="time btn btn-default pull-right" AutoCompleteType="Disabled" Style="width: 150px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4">End</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="endTime" CssClass="time btn btn-default pull-right disabled" AutoCompleteType="Disabled" Style="width: 150px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Presentation Package</h4>
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="presRadio" ErrorMessage="Please select a pres package."
                                Display="Dynamic" SetFocusOnError="true" />
                        </div>
                        <div class="panel-body" style="height: 210px">
                                <label class="h6">Presentation Package Needed?</label>
                                <br />
                                <br />
                                <div class="input-group">
                                    <asp:RadioButtonList runat="server" ID="presRadio" ClientIDMode="Static">
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:Label runat="server" ID="lblSelectedPres" ForeColor="#33cc33" Visible="false"></asp:Label>
                            </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Location Information</h4>
                            <asp:CustomValidator runat="server" ID="cvRoomList" CssClass="label label-danger"
                                ClientValidationFunction="ValidateRoomList" Display="Dynamic"
                                ErrorMessage="Please select a room." />
                        </div>
                        <div class="panel-body vertical-center" style="height: 200px">
                            <div id="form_field" class="form-horizontal">
                                <div id="roomSelect" class="form-group">
                                    <label class="col-sm-4">Building</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="building" CssClass="rooms">
                                            <asp:ListItem Value="">Select one...</asp:ListItem>
                                            <asp:ListItem Value="epa" Text="CalEPA Building" ></asp:ListItem>
                                            <asp:ListItem Value="801k" Text="801 K Building"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4">Room</label>
                                    <div class="col-sm-8">
                                        <span id="epaRoomSpan">
                                            <asp:DropDownList runat="server" ID="epaRooms" CssClass="rooms" disabled="disabled">
                                                <asp:ListItem Value="" Text="..." ></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span id="kRoomSpan">
                                            <asp:DropDownList runat="server" ID="kRooms" CssClass="rooms">
                                                <asp:ListItem Value="" Text="..."></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4">Training</label>
                                    <div class="col-sm-8">
                                        <span id="epaTrainSpan">
                                            <asp:DropDownList runat="server" ID="epaTraining" CssClass="rooms" disabled="disabled">
                                                <asp:ListItem Value="" Text="..."></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span id="kTrainSpan">
                                            <asp:DropDownList runat="server" ID="kTraining" CssClass="rooms">
                                                <asp:ListItem Value="" Text="..."></asp:ListItem>
                                                <asp:ListItem Value="1919" Text="1919"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4">Conference</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList runat="server" ID="epaConf" CssClass="rooms" disabled="disabled">
                                            <asp:ListItem Value="" Text="..."></asp:ListItem>
                                            <asp:ListItem Value="byron" Text="Byron Sher"></asp:ListItem>
                                            <asp:ListItem Value="coastal" Text="Coastal"></asp:ListItem>
                                            <asp:ListItem Value="klamath" Text="Klamath"></asp:ListItem>
                                            <asp:ListItem Value="sierra" Text="Sierra"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Equipment Requested</h4>
                            <asp:CustomValidator runat="server" ID="cvEquipList" CssClass="label label-danger"
                                ClientValidationFunction="ValidateEquipList" Display="Dynamic"
                                ErrorMessage="Please select at least 1 item." />
                        </div>
                        <div class="panel-body vertical-center" style="height: 200px">
                            <asp:CheckBoxList runat="server" RepeatColumns="2" CellPadding="5" CellSpacing="5" RepeatLayout="Table" ID="equipment">
                                <asp:ListItem Value="laptop">&nbsp;Laptop</asp:ListItem>
                                <asp:ListItem Value="projector">&nbsp;Projector</asp:ListItem>
                                <asp:ListItem Value="projscreen">&nbsp;Proj. Screen</asp:ListItem>
                                <asp:ListItem Value="speakers">&nbsp;Speakers</asp:ListItem>
                                <asp:ListItem Value="confphone">&nbsp;Conference Phone</asp:ListItem>
                                <asp:ListItem Value="other">&nbsp;Other(See Comment)</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4 class="panel-title">Connection Type</h4>
                            <asp:RequiredFieldValidator runat="server" CssClass="label label-danger"
                                ControlToValidate="networkReq" ErrorMessage="Please select a connection preference."
                                Display="Dynamic" SetFocusOnError="true" />
                        </div>
                        <div class="panel-body" style="height: 200px">
                            <label class="h6">Is CalRecycle Network Access Required?</label>
                            <br />
                            <br />
                            <div class="input-group">
                                <asp:RadioButtonList runat="server" ID="networkReq">
                                    <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="no" Text="No (WiFi will be used)"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label runat="server" ID="networkWarn" Text="CalRecycle Network Access Not Available in Selected Room." Style="color: red"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8 col-md-offset-2 text-center">
                    <div id="bottom">
                    <asp:Button runat="server" ID="resetBtn" Text="Reset" CssClass="btn btn-danger" OnClick="resetBtn_Click" CausesValidation="false" Height="26px" Width="61px" />
                    <asp:Button runat="server" ID="submitBtn" Text="Submit" CssClass="btn btn-primary" OnClick="submitBtn_Click" CausesValidation="true" Height="26px" Width="61px" /></div>
                    <div id="top">
                        <div id="successAlert" class="alert alert-success fade in"  hidden="hidden">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <strong>Success!</strong> Meeting scheduled successfully.
                        </div>
                        <!-- added this part for if presentation packages are all booked. -->
                        <div id="infoAlert" class="alert alert-info fade in"  hidden="hidden">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <strong>Note!</strong> There are no Presentation Packages Avaliable at this Time. Please Choose a Different Time or Date.  
                        </div>
                        <div id="dangerAlert" class="alert alert-danger fade in"  hidden="hidden">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <strong>Failure!</strong> uh oh... hot dog!
                        </div>
                    </div>
                </div>
            </div>
            <br /><br /><hr />
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label>Comments (Shown to requestor)</label>
                        <asp:TextBox runat="server" Rows="3" Columns="500" CssClass="form-control" ID="commentBox" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery-2.1.3.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Scripts/bootstrap.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery.timepicker.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/App_Assets/Forms/JS/chosen.jquery.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/App_Assets/Forms/JS/Reservation.js")%>"></script>
<script type="text/javascript">
    // Custom validators for non-standard form items
    // Must be placed in aspx or validator does not see method
    $(document).ready(function () {
        $('#<%= presRadio.ClientID %> input').change(function () {
            if ($(this).attr('id') != "presRadio_5") {
                $('#equipment_0').attr('checked', 'true');
                $('#equipment_3').attr('checked', 'true');
                $('#equipment_1').attr('checked', 'true');
            }
        })

        $('#commentBox').keyup(function () {
            $('#equipment_5').attr('checked', 'true');
        })
    });

    function ValidateEquipList(source, args) {
        var chkListModules = document.getElementById('<%= equipment.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }

    function ValidateRoomList(source, args) {
        var roomSelect = [$('#epaRooms').val(), $('#kRooms').val(), $('#epaTraining').val(), $('#kTraining').val(), $('#epaConf').val()]

        for (var i = 0; i < roomSelect.length; i++) {
            if (roomSelect[i] !== "") {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }
</script>
</html>