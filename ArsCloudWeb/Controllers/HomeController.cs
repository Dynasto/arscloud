using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArsCloud.Web.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewData["ListMode"] = "all";
			return View();
		}

		public ActionResult IndexMine()
		{
			ViewData["ListMode"] = "mine";
			return View("Index");
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
