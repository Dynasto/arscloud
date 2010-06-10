<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IList<ArsCloud.Azure.Chirp>>" %>
<%@ Import Namespace="ArsCloud.Azure" %>

<div class="chirp-list">
	<table>
	<%
		foreach(Chirp c in (IList<Chirp>)ViewData["Chirps"])
		{
		%>
		<tr>
			<td class="chirp-avatar">
			<%
			if(AvatarManager.HasUri(c.Username, 16))
			{
				%><img src="<%= AvatarManager.GetUri(c.Username, 16) %>" width="16" height="16" alt="<%= Html.Encode(c.Username) %>" /><%
			}
			%>
			</td>
			<td class="chirp-username"><%= HttpUtility.HtmlEncode(c.Username)%></td>
			<td class="chirp-text"><%= HttpUtility.HtmlEncode(c.Text)%></td>
			<td class="chirp-timestamp"><%= HttpUtility.HtmlEncode(c.Timestamp)%></td>
		</tr>
		<%
		}
	%>
	</table>
</div>
