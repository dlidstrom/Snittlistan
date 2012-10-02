namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Abstractions;
    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.Services;
    using Snittlistan.Web.ViewModels;
    using Snittlistan.Web.ViewModels.Account;

    public class AppController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        public AppController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            if (authenticationService == null) throw new ArgumentNullException("authenticationService");
            this.authenticationService = authenticationService;
        }

        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = Session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var level = new Dictionary<char, int>
                {
                    { 'a', 1 },
                    { 'f', 2 },
                    { 'b', 3 },
                    { 'c', 4 }
                };
            var rosters = Session.Query<Roster>().Where(r => r.Season == season).ToList();
            var q = from roster in rosters
                    orderby roster.Turn
                    group roster by roster.Turn into g
                    select new TurnViewModel
                        {
                            Turn = g.Key,
                            StartDate = g.Min(x => x.Date),
                            EndDate = g.Max(x => x.Date),
                            Rosters =
                                g.Select(x => x.MapTo<RosterViewModel>())
                                 .OrderBy(r => level[r.TeamLevel])
                                 .ThenBy(r => r.Date).ToList()
                        };
            var vm = new InitialDataViewModel
                {
                    SeasonStart = season.Value,
                    SeasonEnd = season.Value + 1,
                    Turns = q.ToList()
                };
            return this.View(vm);
        }

        public ActionResult Results()
        {
            return RedirectToAction("Index");
        }

        public ActionResult Players()
        {
            var players = new[]
                {
                    new PlayerViewModel
                        {
                            Index = 1,
                            Name = "Daniel Lidström"
                        },
                    new PlayerViewModel
                        {
                            Index = 2,
                            Name = "Daniel Solvander"
                        }
                };

            return this.View(players);
        }

        public ActionResult LogOn2()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult LogOn2(LogOnViewModel vm, string returnUrl)
        {
            // find the user in question
            var user = this.Session.FindUserByEmail(vm.Email);

            if (user == null)
            {
                this.ModelState.AddModelError("Email", "Användaren existerar inte.");
            }
            else if (!user.ValidatePassword(vm.Password))
            {
                this.ModelState.AddModelError("Password", "Lösenordet stämmer inte!");
            }
            else if (user.IsActive == false)
            {
                this.ModelState.AddModelError("Inactive", "Användaren har inte aktiverats");
            }

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid)
                return this.View(vm);

            Debug.Assert(user != null, "user != null");
            this.authenticationService.SetAuthCookie(user.Email, vm.RememberMe);

            if (this.Url.IsLocalUrl(returnUrl)
                && returnUrl.Length > 1
                && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//")
                && !returnUrl.StartsWith("/\\"))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult LogOff2()
        {
            this.authenticationService.SignOut();
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult CreateRoster()
        {
            var vm = new RosterViewModel
                {
                    Season = Session.LatestSeasonOrDefault(DateTime.Now.Year)
                };
            return this.View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateRoster(RosterViewModel vm)
        {
            if (!ModelState.IsValid) return this.View(vm);

            var dt = new DateTime(
                vm.Date.Year,
                vm.Date.Month,
                vm.Date.Day,
                int.Parse(vm.Time.Substring(0, 2)),
                int.Parse(vm.Time.Substring(3)),
                0);
            var roster = new Roster(
                vm.Season,
                vm.Turn,
                vm.Team,
                vm.Location,
                vm.Opponent,
                dt);
            Session.Store(roster);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteRoster(int id)
        {
            var roster = Session.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            return this.View(roster.MapTo<RosterViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("DeleteRoster")]
        public ActionResult DeleteRosterConfirmed(int id)
        {
            var roster = Session.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            Session.Delete(roster);
            return RedirectToAction("Index");
        }
    }
}
