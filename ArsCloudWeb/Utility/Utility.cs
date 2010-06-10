using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Globalization;
using System.Reflection;

namespace ArsCloudWeb.Utility
{
	// from http://blogs.intesoft.net/post/2009/02/RenderSubAction-alternative-to-RenderAction-for-sub-controllers-using-asp-net-mvc.aspx
	public static class HtmlHelperExtensions
	{
		public static void RenderSubAction<TController>(this HtmlHelper helper, Expression<Action<TController>> action)
			where TController : Controller
		{
			RouteValueDictionary routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper
				.GetRouteValuesFromExpression(action);
			helper.RenderRoute(routeValuesFromExpression);
		}

		public static void RenderSubAction(this HtmlHelper helper, string actionName)
		{
			helper.RenderSubAction(actionName, null);
		}

		public static void RenderSubAction(this HtmlHelper helper, string actionName, string controllerName)
		{
			helper.RenderSubAction(actionName, controllerName, null);
		}

		public static void RenderSubAction(this HtmlHelper helper, string actionName, string controllerName,
				object routeValues)
		{
			helper.RenderSubAction(actionName, controllerName, new RouteValueDictionary(routeValues));
		}

		public static void RenderSubAction(this HtmlHelper helper, string actionName, string controllerName,
										RouteValueDictionary routeValues)
		{
			RouteValueDictionary dictionary = routeValues != null ? new RouteValueDictionary(routeValues)
				: new RouteValueDictionary();
			foreach(var pair in helper.ViewContext.RouteData.Values)
			{
				if(!dictionary.ContainsKey(pair.Key))
				{
					dictionary.Add(pair.Key, pair.Value);
				}
			}
			if(!string.IsNullOrEmpty(actionName))
			{
				dictionary["action"] = actionName;
			}
			if(!string.IsNullOrEmpty(controllerName))
			{
				dictionary["controller"] = controllerName;
			}
			helper.RenderRoute(dictionary);
		}

		public static void RenderRoute(this HtmlHelper helper, RouteValueDictionary routeValues)
		{
			var routeData = new RouteData();
			foreach(var pair in routeValues)
			{
				routeData.Values.Add(pair.Key, pair.Value);
			}
			HttpContextBase httpContext = new OverrideRequestHttpContextWrapper(HttpContext.Current);
			var context = new RequestContext(httpContext, routeData);
			bool validateRequest = helper.ViewContext.Controller.ValidateRequest;
			new RenderSubActionMvcHandler(context, validateRequest).ProcessRequestInternal(httpContext);
		}

		#region Nested type: RenderSubActionMvcHandler

		private class RenderSubActionMvcHandler : MvcHandler
		{
			private bool _validateRequest;
			public RenderSubActionMvcHandler(RequestContext context, bool validateRequest)
				: base(context)
			{
				_validateRequest = validateRequest;
			}

			protected override void AddVersionHeader(HttpContextBase httpContext) { }

			public void ProcessRequestInternal(HttpContextBase httpContext)
			{
				AddVersionHeader(httpContext);
				string requiredString = RequestContext.RouteData.GetRequiredString("controller");
				IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
				IController controller = controllerFactory.CreateController(RequestContext, requiredString);
				if(controller == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture,
						"The IControllerFactory '{0}' did not return a controller for a controller named '{1}'.",
						new object[] { controllerFactory.GetType(), requiredString }));
				}
				try
				{
					((ControllerBase)controller).ValidateRequest = _validateRequest;
					controller.Execute(RequestContext);
				}
				finally
				{
					controllerFactory.ReleaseController(controller);
				}
			}
		}

		private class OverrideHttpMethodHttpRequestWrapper : HttpRequestWrapper
		{
			public OverrideHttpMethodHttpRequestWrapper(HttpRequest httpRequest) : base(httpRequest) { }

			public override string HttpMethod
			{
				get { return "GET"; }
			}
		}

		private class OverrideRequestHttpContextWrapper : HttpContextWrapper
		{
			private readonly HttpContext _httpContext;
			public OverrideRequestHttpContextWrapper(HttpContext httpContext)
				: base(httpContext)
			{
				_httpContext = httpContext;
			}

			public override HttpRequestBase Request
			{
				get { return new OverrideHttpMethodHttpRequestWrapper(_httpContext.Request); }
			}
		}

		#endregion
	}
}
