<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ArsCloudWeb.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Chirp</h2>
	<% Html.RenderPartial("ChirpCreate"); %>
	<ul>
	<%
		foreach(Chirp c in (ViewData["AllChirps"] as IList<Chirp>))
		{
		%>
		<li>
		<div class="chirp-username"><%= c.Username %></div>
		<div class="chirp-text"><%= c.Text %></div>
		<div class="chirp-timestamp"><%= c.Timestamp %></div>
		</li>
		<%
		}
	%></ul>
</asp:Content>
