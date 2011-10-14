using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Snittlistan.Infrastructure.Indexes;

namespace Snittlistan.Controllers
{
	public class HomeController : AbstractController
	{
		public HomeController(IDocumentSession session)
			: base(session)
		{ }

		public ActionResult Index()
		{
			var stats = Session.Query<Matches_PlayerStats.Results, Matches_PlayerStats>()
				.OrderByDescending(s => s.Average)
				.ToList();

			return View(stats);
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
