<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArsCloudWeb.Models.ChirpModel>" %>
<%
	if(Request.IsAuthenticated)
	{
		using(Html.BeginForm("Create", "Chirp"))
		{
		%>
		<div>
			<fieldset>
				<div class="editor-label">
					<%: Html.LabelFor(m => m.Chirp)%>
				</div>
				<div class="editor-field">
					<%: Html.TextAreaFor(m => m.Chirp)%>
					<%: Html.ValidationMessageFor(m => m.Chirp)%>
				</div>
				<input type="submit" value="Chirp!" />
			</fieldset>
		</div>
		<%
		}
	}
%>
