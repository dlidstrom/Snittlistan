using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Snittlistan.Infrastructure;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Controllers
{
	public class MatchController : AbstractController
	{
		public MatchController(IDocumentSession session)
			: base(session)
		{
		}

		/// <summary>
		/// GET: /Match/
		/// </summary>
		/// <returns></returns>
		public ViewResult Index()
		{
			var matches = Session.Query<Match>()
				.ToList()
				.Select(m => new MatchViewModel
				{
					Match = m.MapTo<MatchViewModel.MatchDetails>(),
					HomeTeam = m.HomeTeam.MapTo<MatchViewModel.Team>(),
					AwayTeam = m.AwayTeam.MapTo<MatchViewModel.Team>()
				});
			return View(matches);
		}

		/// <summary>
		/// GET: /Match/Details/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Details(int id)
		{
			var match = Session.Load<Match>(id);
			if (match == null)
				return HttpNotFound();

			var vm = new MatchViewModel
			{
				Match = match.MapTo<MatchViewModel.MatchDetails>(),
				HomeTeam = match.HomeTeam.MapTo<MatchViewModel.Team>(),
				AwayTeam = match.AwayTeam.MapTo<MatchViewModel.Team>()
			};

			return View(vm);
		}

		/// <summary>
		/// GET: /Match/Register
		/// </summary>
		/// <returns></returns>
		public ViewResult Register()
		{
			return View(new RegisterMatchViewModel());
		}

		/// <summary>
		/// POST: /Match/Register
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Register(RegisterMatchViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var match = new Match(
				model.Place,
				model.Date,
				model.BitsMatchId,
				new Team(model.HomeTeamName, model.HomeTeamScore),
				new Team(model.AwayTeamName, model.AwayTeamScore));
			Session.Store(match);

			return RedirectToAction("Details", new { id = match.Id });
		}

		public ActionResult AddGame(int matchId)
		{
			return View();
		}

		/// <summary>
		/// GET: /Match/Edit/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			var match = Session.Load<Match>(id);
			var vm = new MatchViewModel
			{
				Match = match.MapTo<MatchViewModel.MatchDetails>(),
				HomeTeam = match.HomeTeam.MapTo<MatchViewModel.Team>(),
				AwayTeam = match.AwayTeam.MapTo<MatchViewModel.Team>()
			};

			return View(vm);
		}

		/// <summary>
		/// POST: /Match/Edit/5
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditDetails(MatchViewModel.MatchDetails model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var match = Session.Load<Match>(model.Id);
			match.Place = model.Place;
			match.Date = model.Date;
			match.BitsMatchId = model.BitsMatchId;

			return RedirectToAction("Details", new { id = match.Id });
		}

		/// <summary>
		/// GET: /Match/Delete/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Delete(int id)
		{
			var match = Session.Load<Match>(id);
			return View(match.MapTo<MatchViewModel.MatchDetails>());
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
