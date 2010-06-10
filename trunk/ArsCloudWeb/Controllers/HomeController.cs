using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArsCloudWeb.Data;

namespace ArsCloudWeb.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			//ViewData["Message"] = "Welcome to ASP.NET MVC!";
			ViewData["AllChirps"] = ChirpManager.FindAll();
			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
