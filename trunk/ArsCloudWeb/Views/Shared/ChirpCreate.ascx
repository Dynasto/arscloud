<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ArsCloudWeb.Models.ChirpModel>" %>
<%
	if(Request.IsAuthenticated)
	{
		using(Html.BeginForm("Create", "Chirp"))
		{
		%>
		<div class="chirp-form">
			<fieldset class="chirp-fields">
				<div class="chirp-textbox-label">
					<%: Html.LabelFor(m => m.Chirp)%>
				</div>
				<div class="chirp-textbox">
					<%: Html.TextAreaFor(m => m.Chirp, new { cols = "64", rows="4" })%>
					<%: Html.ValidationMessageFor(m => m.Chirp)%>
				</div>
				<div class="chirp-submit">
					<input class="chirp-submit-button" type="submit" value="Chirp!" />
				</div>
			</fieldset>
		</div>
		<%
		}
	}
%>
