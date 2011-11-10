namespace Snittlistan.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using MvcContrib;
    using Raven.Client;
    using Raven.Client.Linq;
    using Snittlistan.Helpers;
    using Snittlistan.Infrastructure.AutoMapper;
    using Snittlistan.Infrastructure.Indexes;
    using Snittlistan.Models;
    using Snittlistan.ViewModels;

    public class MatchController : AbstractController
    {
        /// <summary>
        /// Initializes a new instance of the MatchController class.
        /// </summary>
        /// <param name="session">Session instance.</param>
        public MatchController(IDocumentSession session)
            : base(session)
        {
        }

        /// <summary>
        /// GET: /Match/.
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            var matches = Session.Query<Match, Match_ByDate>()
                .OrderByDescending(m => m.Date)
                .ToList()
                .Select(match => new MatchViewModel
                {
                    Match = match.MapTo<MatchViewModel.MatchDetails>(),
                    HomeTeam = match.HomeTeam.MapTo<TeamDetailsViewModel>(),
                    AwayTeam = match.AwayTeam.MapTo<TeamDetailsViewModel>()
                });
            return View(matches);
        }

        /// <summary>
        /// GET: /Match/Details/5.
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
                HomeTeam = match.HomeTeam.MapTo<TeamDetailsViewModel>(),
                AwayTeam = match.AwayTeam.MapTo<TeamDetailsViewModel>()
            };

            return View(vm);
        }

        /// <summary>
        /// GET: /Match/Register.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ViewResult Register()
        {
            return View(new RegisterMatchViewModel());
        }

        /// <summary>
        /// POST: /Match/Register.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult Register(RegisterMatchViewModel model)
        {
            if (Session.BitsIdExists(model.BitsMatchId))
                ModelState.AddModelError("BitsMatchId", "Matchen redan registrerad");

            if (!ModelState.IsValid)
                return View(model);

            var match = new Match(
                model.Location,
                model.Date,
                model.BitsMatchId,
                model.HomeTeam.MapTo<HomeTeamFactory>().CreateTeam(),
                model.AwayTeam.MapTo<AwayTeamFactory>().CreateTeam());
            Session.Store(match);

            return RedirectToAction("Details", new { id = match.Id });
        }

        /// <summary>
        /// GET: /Match/Edit/5.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditDetails(int id)
        {
            var match = Session.Load<Match>(id);
            if (match == null)
                return HttpNotFound();

            return View(match.MapTo<MatchViewModel.MatchDetails>());
        }

        /// <summary>
        /// POST: /Match/Edit/5.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult EditDetails(MatchViewModel.MatchDetails model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var match = Session.Load<Match>(model.Id);
            if (match == null)
                return HttpNotFound();

            match.Location = model.Location;
            match.Date = model.Date;
            match.BitsMatchId = model.BitsMatchId;

            return RedirectToAction("Details", new { id = match.Id });
        }

        /// <summary>
        /// GET: /Match/EditTeam/5.
        /// </summary>
        /// <param name="id">Match identifier.</param>
        /// <param name="isHomeTeam">True if home team; false if away team.</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditTeam(int id, bool isHomeTeam)
        {
            var match = Session.Load<Match>(id);
            if (match == null)
                return HttpNotFound();

            var teamViewModel = isHomeTeam
                ? match.HomeTeam.MapTo<TeamViewModel>()
                : match.AwayTeam.MapTo<TeamViewModel>();
            var vm = new EditTeamViewModel
            {
                Id = id,
                IsHomeTeam = isHomeTeam,
                Team = teamViewModel
            };

            return View(vm);
        }

        /// <summary>
        /// POST: /Match/EditTeam/5.
        /// </summary>
        /// <param name="vm">Team view model.</param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult EditTeam(EditTeamViewModel vm)
        {
            var match = Session.Load<Match>(vm.Id);
            if (match == null)
                return HttpNotFound();

            if (vm.IsHomeTeam)
                match.HomeTeam = vm.Team.MapTo<HomeTeamFactory>().CreateTeam();
            else
                match.AwayTeam = vm.Team.MapTo<AwayTeamFactory>().CreateTeam();

            return RedirectToAction("Details", new { id = vm.Id });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var match = Session.Load<Match>(id);
            if (match == null)
                return HttpNotFound();

            return View(match.MapTo<MatchViewModel.MatchDetails>());
        }

        [Authorize, HttpPost]
        public ActionResult Delete(MatchViewModel.MatchDetails vm)
        {
            var match = Session.Load<Match>(vm.Id);
            if (match == null)
                return HttpNotFound();

            Session.Delete(match);

            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Create()
        {
            return RedirectToActionPermanent("Index");
        }

        public ActionResult Edit()
        {
            return RedirectToActionPermanent("Index");
        }
    }
}