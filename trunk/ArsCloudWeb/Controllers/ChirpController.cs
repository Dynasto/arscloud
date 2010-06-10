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
        //
        // GET: /Chirp/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Chirp/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // POST: /Chirp/Create
		[HttpPost]
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

        //
        // GET: /Chirp/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Chirp/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Chirp/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Chirp/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
