<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ArsCloudWeb.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Chirp</h2>
	<% Html.RenderPartial("ChirpCreate"); %>
	<dl>
	<%
		foreach(Chirp c in (ViewData["AllChirps"] as IList<Chirp>))
		{
		%>
		<dt><%= c.Username %></dt>
		<dd><%= c.Text %></dd>
		<%
		}
	%></dl>
</asp:Content>
