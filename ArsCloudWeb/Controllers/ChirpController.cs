using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArsCloudWeb.Models;
using ArsCloudWeb.Data;

namespace ArsCloudWeb.Controllers
{
	public class ChirpController : Controller
	{
		// POST: /Chirp/Create
		[HttpPost]
		[Authorize]
		public ActionResult Create(ChirpModel cm)
		{
			try
			{
				ChirpManager.Add(User.Identity.Name, cm.Chirp);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return View();
			}
		}
	}
}
