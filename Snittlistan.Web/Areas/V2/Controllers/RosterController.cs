namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Abstractions;
    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Infrastructure.Indexes;
    using Snittlistan.Web.Services;

    public class RosterController : AbstractController
    {
        public RosterController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (authenticationService == null) throw new ArgumentNullException("authenticationService");
        }

        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = this.Session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var rosters = this.Session.Query<Roster, RosterSearchTerms>()
                .Where(r => r.Season == season && r.Date >= SystemTime.UtcNow.Date)
                .ToList();
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
                                .SortRosters()
                                .ToList()
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
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new CreateRosterViewModel
                {
                    Season = this.Session.LatestSeasonOrDefault(DateTime.Now.Year)
                };
            return this.View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateRosterViewModel vm)
        {
            if (!this.ModelState.IsValid) return this.View(vm);

            var time = new TimeSpan(
                int.Parse(vm.Time.Substring(0, 2)),
                int.Parse(vm.Time.Substring(3)),
                0);
            var roster = new Roster(
                vm.Season,
                vm.Turn,
                vm.Team,
                vm.Location,
                vm.Opponent,
                vm.Date.Add(time));
            this.Session.Store(roster);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var roster = Session.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            return this.View(roster.MapTo<CreateRosterViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, CreateRosterViewModel vm)
        {
            if (!this.ModelState.IsValid)
                return this.View(vm);

            var roster = this.Session.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");

            roster.Location = vm.Location;
            roster.Opponent = vm.Opponent;
            roster.Season = vm.Season;
            roster.Team = vm.Team;
            roster.Turn = vm.Turn;
            var time = new TimeSpan(
                int.Parse(vm.Time.Substring(0, 2)),
                int.Parse(vm.Time.Substring(3)),
                0);
            roster.Date = vm.Date.Add(time);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var roster = this.Session.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            return this.View(roster.MapTo<RosterViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var roster = this.Session.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            this.Session.Delete(roster);
            return this.RedirectToAction("Index");
        }

        public ActionResult View(int season, int turn)
        {
            var rosters = this.Session.Query<Roster, RosterSearchTerms>()
                .Include<Roster>(roster => roster.Players)
                .Where(roster => roster.Turn == turn)
                .Where(roster => roster.Season == season)
                .ToArray();
            var list = rosters.Select(this.LoadRoster)
                .SortRosters()
                .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel
            {
                Id = turn,
                Season = season,
                Rosters = list
            };
            return this.View(viewTurnViewModel);
        }

        [Authorize]
        public ActionResult EditPlayers(int id)
        {
            var roster = this.Session
                .Include<Roster>(r => r.Players)
                .Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var availablePlayers = this.Session.Query<Player, PlayerSearch>()
                .Where(p => p.IsSupporter == false);

            var vm = new EditRosterPlayersViewModel
            {
                Id = id,
                Roster = this.LoadRoster(roster),
                AvailablePlayers = availablePlayers.MapTo<PlayerViewModel>().ToArray()
            };
            return this.View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPlayers(int id, RosterPlayersViewModel vm)
        {
            if (ModelState.IsValid == false) return EditPlayers(id);
            var roster = Session.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            roster.Players = new List<string>
                {
                    vm.Table1Player1,
                    vm.Table1Player2,
                    vm.Table2Player1,
                    vm.Table2Player2,
                    vm.Table3Player1,
                    vm.Table3Player2,
                    vm.Table4Player1,
                    vm.Table4Player2
                };
            if (vm.Reserve != null && this.Session.Load<Player>(vm.Reserve) != null)
                roster.Players.Add(vm.Reserve);
            return RedirectToAction("View", new { season = roster.Season, turn = roster.Turn });
        }

        private RosterViewModel LoadRoster(Roster roster)
        {
            var vm = roster.MapTo<RosterViewModel>();
            foreach (var player in roster.Players.Where(p => p != null).Select(playerId => this.Session.Load<Player>(playerId)))
            {
                vm.Players.Add(Tuple.Create(player.Id, player.Name));
            }

            return vm;
        }
    }
}
