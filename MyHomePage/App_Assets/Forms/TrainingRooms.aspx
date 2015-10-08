<%@ Page Title="Training Rooms" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeFile="TrainingRooms.aspx.cs" Inherits="App_Assets_Forms_TrainingRooms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="Server">
	<link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Content/bootstrap.min.css")%>" />
	<style>
		.well {
			margin: 10px;
			background-color: grey;
		}

		.col-md-4, .col-lg-8 {
			text-align: center;
			font-family: Georgia, 'Times New Roman', Times, serif;
		}
	</style>
	<asp:UpdatePanel ID="SouthPanel" runat="server">
		<ContentTemplate>
			<ajaxToolkit:ToolkitScriptManager runat="server" ID="sm1" />
			<div class="row">
				<div class="col-md-2"></div>
				<div class="col-md-4">
					<div class="well">
						<asp:Label runat="server" CssClass="h1">Training Room 8 North</asp:Label>
						<hr />
						<div class="btn-group-vertical" role="group" aria-label="...">
							<asp:Button ID="arbNorth" CssClass="btn btn-default north" runat="server" Text="ARB" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="recycleNorth" CssClass="btn btn-default north" runat="server" Text="CalRecycle" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="dprNorth" CssClass="btn btn-default north" runat="server" Text="DPR" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="dtscNorth" CssClass="btn btn-default north" runat="server" Text="DTSC" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="oehhaNorth" CssClass="btn btn-default north" runat="server" Text="OEHHA" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="swrcbNorth" CssClass="btn btn-default north" runat="server" Text="SWRCB" OnClick="setNorth_Click" data-toggle="modal" data-target="#loadModal" />
						</div>
						<div>
							<hr />
							<asp:Label runat="server" CssClass="h3">Current Reservations </asp:Label>
							<asp:LinkButton runat="server" ID="linkNorth" Text="Go" CssClass="btn btn-default"></asp:LinkButton>
						</div>
					</div>
				</div>
				<div class="col-md-4">
					<div class="well">
						<asp:Label runat="server" CssClass="h1">Training Room 8 South</asp:Label>
						<hr />
						<div class="btn-group-vertical" role="group" aria-label="...">
							<asp:Button ID="arbSouth" CssClass="btn btn-default south" runat="server" Text="ARB" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="recycleSouth" CssClass="btn btn-default south" runat="server" Text="CalRecycle" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="dprSouth" CssClass="btn btn-default south" runat="server" Text="DPR" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="dtscSouth" CssClass="btn btn-default south" runat="server" Text="DTSC" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="oehhaSouth" CssClass="btn btn-default south" runat="server" Text="OEHHA" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
							<asp:Button ID="swrcbSouth" CssClass="btn btn-default south" runat="server" Text="SWRCB" OnClick="setSouth_Click" data-toggle="modal" data-target="#loadModal" />
						</div>
						<div>
							<hr />
							<asp:Label runat="server" CssClass="h3">Current Reservations </asp:Label>
							<asp:LinkButton runat="server" ID="linkSouth" Text="Go" CssClass="btn btn-default"></asp:LinkButton>
						</div>
					</div>
				</div>
				<div class="col-md-2"></div>
			</div>
			<div class="row">
				<div class="col-lg-2"></div>
				<div class="col-lg-8">
					<div class="well">
						<asp:Label runat="server" CssClass="h4 text-center"><span class="label label-success">Green</span> indicates currently connected BDO.</asp:Label>
					</div>
				</div>
				<div class="col-lg-2"></div>
			</div>

			<!-- Modal -->
			<div class="modal fade" id="loadModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
				<div class="modal-dialog modal-sm">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
							<h4 class="modal-title" id="myModalLabel">Switching</h4>
						</div>
						<div class="modal-body">
							Switching networks...
						</div>
					</div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<script type="text/javascript">
		function getActiveID(room) {
			var bdo;
			switch (room) {
				case "south":
					bdo = "<%=activeConnectionSouth%>";
					return bdo;
					break;
				case "north":
					bdo = "<%=activeConnectionNorth%>";
				return bdo;
				break;
			 }
		}
	</script>
	<script src='<%=ResolveUrl("~/Scripts/bootstrap.js")%>'></script>
	<script src='<%=ResolveUrl("~/Scripts/trainingrooms.js")%>'></script>
</asp:Content>