<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ArsCloud.Web.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Chirp</h2>
	<% Html.RenderAction("Create", "Chirp"); %>
	<% Html.RenderAction("List", "Chirp"); %>
	<div class="chirp-menu">
	<% if(Request.IsAuthenticated) { %>
		<%: Html.ActionLink("All Chirps", "ListAll", "Chirp")%><br />
		<%: Html.ActionLink("My Chirps", "ListMine", "Chirp")%><br />
	<% } %>
	</div>
</asp:Content>
