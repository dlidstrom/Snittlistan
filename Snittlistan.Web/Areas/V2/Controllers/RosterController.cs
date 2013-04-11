using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Services;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class RosterController : AbstractController
    {
        public RosterController(IAuthenticationService authenticationService)
        {
            if (authenticationService == null) throw new ArgumentNullException("authenticationService");
        }

        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = this.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var rosters = this.DocumentSession.Query<Roster, RosterSearchTerms>()
                .ToList();
            var q = from roster in rosters
                    orderby roster.Turn
                    group roster by roster.Turn into g
                    let lastDate = g.Max(x => x.Date)
                    where lastDate >= SystemTime.UtcNow.Date
                    select new TurnViewModel
                        {
                            Turn = g.Key,
                            StartDate = g.Min(x => x.Date),
                            EndDate = lastDate,
                            Rosters =
                                g.Select(x => x.MapTo<RosterViewModel>())
                                .SortRosters()
                                .ToList()
                        };
            var vm = new InitialDataViewModel
                {
                    SeasonStart = season.Value,
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
                    Season = this.DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year)
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
            this.DocumentSession.Store(roster);
            return this.RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            return this.View(roster.MapTo<CreateRosterViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string id, CreateRosterViewModel vm)
        {
            if (!this.ModelState.IsValid)
                return this.View(vm);

            var roster = this.DocumentSession.Load<Roster>(id);
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
            var roster = this.DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            return this.View(roster.MapTo<RosterViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var roster = this.DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            this.DocumentSession.Delete(roster);
            return this.RedirectToAction("Index");
        }

        public ActionResult View(int season, int? turn)
        {
            if (turn.HasValue == false)
            {
                // find out next turn
                var rosters = this.DocumentSession.Query<Roster, RosterSearchTerms>()
                    .Where(x => x.Season == season)
                    .Where(x => x.Date > SystemTime.UtcNow.Date)
                    .OrderBy(x => x.Date)
                    .Take(1)
                    .ToList();
                turn = rosters.Select(x => x.Turn).FirstOrDefault();
            }

            var rostersForTurn = this.DocumentSession.Query<Roster, RosterSearchTerms>()
                .Include<Roster>(roster => roster.Players)
                .Where(roster => roster.Turn == turn)
                .Where(roster => roster.Season == season)
                .ToArray();
            var list = rostersForTurn.Select(this.LoadRoster)
                .SortRosters()
                .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel
            {
                Id = turn.Value,
                Season = season,
                Rosters = list
            };
            return this.View(viewTurnViewModel);
        }

        [Authorize]
        public ActionResult EditPlayers(string id)
        {
            var roster = this.DocumentSession
                .Include<Roster>(r => r.Players)
                .Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var availablePlayers = this.DocumentSession.Query<Player, PlayerSearch>()
                .OrderBy(x => x.Name)
                .Where(p => p.IsSupporter == false);

            var vm = new EditRosterPlayersViewModel
            {
                Id = id,
                Roster = this.LoadRoster(roster),
                Preliminary = roster.Preliminary,
                AvailablePlayers = availablePlayers.MapTo<PlayerViewModel>().ToArray()
            };
            return this.View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPlayers(string id, RosterPlayersViewModel vm)
        {
            if (ModelState.IsValid == false) return EditPlayers(id);
            var roster = DocumentSession.Load<Roster>(id);
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
            roster.Preliminary = vm.Preliminary;
            if (vm.Reserve != null && this.DocumentSession.Load<Player>(vm.Reserve) != null)
                roster.Players.Add(vm.Reserve);
            return RedirectToAction("View", new { season = roster.Season, turn = roster.Turn });
        }

        private RosterViewModel LoadRoster(Roster roster)
        {
            var vm = roster.MapTo<RosterViewModel>();
            foreach (var player in roster.Players.Where(p => p != null).Select(playerId => this.DocumentSession.Load<Player>(playerId)))
            {
                vm.Players.Add(Tuple.Create(player.Id, player.Name));
            }

            return vm;
        }
    }
}
