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
		private List<Match> matches;

		public MatchController()
		{
			var match = new Match
			{
				Date = DateTime.ParseExact("2011-03-26", "yyyy-MM-dd", CultureInfo.InvariantCulture),
				Place = "Mälarhallen",
				Series = new List<Serie>
				{
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "M. Axelsson", 202, 1),
							new Game(1, "C. Liedholm", 218, 1),
							new Game(2, "K. Persson", 172, 0),
							new Game(2, "P. Sjöberg", 220, 0),
							new Game(3, "K. Jansson", 166, 0),
							new Game(3, "H. Norbeck", 194, 0),
							new Game(4, "L. Öberg", 222, 1),
							new Game(4, "T. Jensen", 204, 1)
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "L. Öberg", 182, 1),
							new Game(1, "T. Jensen", 211, 1),
							new Game(2, "K. Jansson", 208, 1),
							new Game(2, "H. Norbeck", 227, 1),
							new Game(3, "K. Persson", 194, 0),
							new Game(3, "P. Sjöberg", 195, 0),
							new Game(4, "M. Axelsson", 206, 0),
							new Game(4, "C. Liedholm", 150, 0)
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "K. Persson", 174, 0),
							new Game(1, "P. Sjöberg", 182, 0),
							new Game(2, "M. Axelsson", 214, 1),
							new Game(2, "C. Liedholm", 176, 1),
							new Game(3, "L. Öberg", 168, 0),
							new Game(3, "T. Jensen", 199, 0),
							new Game(4, "K. Jansson", 180, 0),
							new Game(4, "H. Norbeck", 212, 0),
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "K. Jansson", 189, 0),
							new Game(1, "H. Norbeck", 181, 0),
							new Game(2, "L. Öberg", 227, 0),
							new Game(2, "T. Jensen", 180, 0),
							new Game(3, "M. Axelsson", 223, 1),
							new Game(3, "C. Liedholm", 191, 1),
							new Game(4, "T. Gurell", 159, 0),
							new Game(4, "P. Sjöberg", 190, 0),
						}
					}
				}
			};

			matches = new Match[] { match }.ToList();
		}

		/// <summary>
		/// GET: /Match/
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
            return View(matches.MapTo<MatchViewModel>());
        }

		/// <summary>
		/// GET: /Match/Details/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        public ActionResult Details(string id)
        {
            return View(matches.Where(m => m.Id == id).Single());
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
        public ActionResult Edit(string id)
        {
            return View(matches.Where(m => m.Id == id).Single());
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
        public ActionResult Delete(string id)
        {
			return View(matches.Where(m => m.Id == id).Single());
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
