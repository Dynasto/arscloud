﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IList<ArsCloudWeb.Data.Chirp>>" %>
<%@ Import Namespace="ArsCloudWeb.Data" %>

<div class="chirp-list">
<ul>
	<%
		foreach(Chirp c in (IList<Chirp>)ViewData["Chirps"])
		{
		%>
		<li>
			<div class="chirp-username"><%= Html.Encode(c.Username) %></div>
			<div class="chirp-text"><%= Html.Encode(c.Text) %></div>
			<div class="chirp-timestamp"><%= Html.Encode(c.Timestamp) %></div>
		</li>
		<%
		}
	%></ul>
</div>