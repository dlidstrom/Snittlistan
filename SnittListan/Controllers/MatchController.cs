using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SnittListan.Infrastructure;
using SnittListan.Models;
using SnittListan.ViewModels;

namespace SnittListan.Controllers
{
    public class MatchController : Controller
    {
		private MatchRepository repo;

		public MatchController()
		{
			repo = new MatchRepository();
		}

		/// <summary>
		/// GET: /Match/
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
            return View(repo.LoadAll(0, 25).MapTo<MatchViewModel>());
        }

		/// <summary>
		/// GET: /Match/Details/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        public ActionResult Details(int id)
        {
			return View(repo.Query().Where(m => m.Id == id).Single().MapTo<MatchViewModel>());
        }

		/// <summary>
		/// GET: /Match/Create
		/// </summary>
		/// <returns></returns>
        public ActionResult Create()
        {
            return View();
        } 

		/// <summary>
		/// POST: /Match/Create
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
        [HttpPost]
        public ActionResult Create(Match match)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
 		/// <summary>
		/// GET: /Match/Edit/5
 		/// </summary>
 		/// <param name="id"></param>
 		/// <returns></returns>
        public ActionResult Edit(int id)
        {
            return View(repo.Query().Where(m => m.Id == id).Single());
        }

		/// <summary>
		/// POST: /Match/Edit/5
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Match match)
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

 		/// <summary>
		/// GET: /Match/Delete/5
 		/// </summary>
 		/// <param name="id"></param>
 		/// <returns></returns>
        public ActionResult Delete(int id)
        {
			return View(repo.Query().Where(m => m.Id == id).Single());
        }

		/// <summary>
		/// POST: /Match/Delete/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
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
