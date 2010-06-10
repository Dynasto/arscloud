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
		[HttpGet]
		public ActionResult Create()
		{
			return View("ChirpCreate");
		}

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

		//[HttpGet]
		//public ActionResult List(string s)
		//{
		//    switch(s)
		//    {
		//    case "all":
		//        return PartialView("ChirpList", ChirpManager.FindAll());
		//    case "mine":
		//        return PartialView("ChirpList", ChirpManager.FindMine(User.Identity.Name));
		//    }
		//    return new EmptyResult();
		//}
		[HttpGet]
		public ActionResult List()
		{
			ViewData["Chirps"] = TempData["Chirps"] ?? ChirpManager.FindAll();
			return PartialView("ChirpList");
		}

		[HttpGet]
		public ActionResult ListAll()
		{
			TempData["Chirps"] = ChirpManager.FindAll();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize]
		public ActionResult ListMine()
		{
			TempData["Chirps"] = ChirpManager.FindMine(User.Identity.Name);
			return RedirectToAction("Index", "Home");
		}
	}
}
