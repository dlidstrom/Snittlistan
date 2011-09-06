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
		private MatchRepository repo;

		public MatchController(IDocumentSession session)
			: base(session)
		{
			repo = new MatchRepository();
		}

		/// <summary>
		/// GET: /Match/
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View(Session.Query<Match>().MapTo<MatchViewModel>());
		}

		/// <summary>
		/// GET: /Match/Details/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Details(int id)
		{
			var match = Session.Load<Match>(id).MapTo<MatchViewModel>();
			return View(match);
		}

		/// <summary>
		/// GET: /Match/Create
		/// </summary>
		/// <returns></returns>
		public ActionResult Create()
		{
			return View(new MatchInfoViewModel());
		}

		/// <summary>
		/// POST: /Match/Create
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Create(MatchInfoViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var match = new Match(
				model.Place,
				model.Date,
				new Team(model.HomeTeam, model.HomeTeamScore),
				new Team(model.AwayTeam, model.AwayTeamScore),
				model.BitsMatchId);
			Session.Store(match);

			return RedirectToAction("Details", new { id = match.Id });
		}

		public ActionResult AddGame(int matchId)
		{
			return View(new GameViewModel { MatchId = matchId });
		}

		/// <summary>
		/// GET: /Match/Edit/5
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			return View(Session.Load<Match>(id).MapTo<MatchViewModel>());
		}

		/// <summary>
		/// POST: /Match/Edit/5
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(MatchViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var match = new Match(
				model.Info.Place,
				model.Info.Date,
				new Team(model.Info.HomeTeam, model.Info.HomeTeamScore),
				new Team(model.Info.AwayTeam, model.Info.AwayTeamScore),
				model.Info.BitsMatchId);
			match.Id = model.Id;
			Session.Store(match);

			return RedirectToAction("Details", new { id = match.Id });
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
