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

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class RosterController : AbstractController
    {
        public ActionResult Index(int? season)
        {
            var selectAll = true;
            if (season.HasValue == false)
            {
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
                selectAll = false;
            }

            var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                .Where(r => r.Season == season)
                .ToList();
            var q = from roster in rosters
                    orderby roster.Turn
                    group roster by roster.Turn into g
                    let lastDate = g.Max(x => x.Date)
                    where selectAll || lastDate >= SystemTime.UtcNow.Date
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
            var turns = q.ToList();
            if (turns.Count <= 0) return View("Unscheduled");

            var vm = new InitialDataViewModel
            {
                SeasonStart = season.Value,
                Turns = turns
            };
            return View(vm);
        }

        public ActionResult Results()
        {
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new CreateRosterViewModel
                {
                    Season = DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year)
                };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateRosterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

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
                vm.Date.Add(time),
                vm.IsFourPlayer);
            DocumentSession.Store(roster);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            return View(roster.MapTo<CreateRosterViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string id, CreateRosterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");

            roster.Location = vm.Location;
            roster.Opponent = vm.Opponent;
            roster.Season = vm.Season;
            roster.Team = vm.Team;
            roster.Turn = vm.Turn;
            roster.IsFourPlayer = vm.IsFourPlayer;
            var time = new TimeSpan(
                int.Parse(vm.Time.Substring(0, 2)),
                int.Parse(vm.Time.Substring(3)),
                0);
            roster.Date = vm.Date.Add(time);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            return View(roster.MapTo<RosterViewModel>());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            DocumentSession.Delete(roster);
            return RedirectToAction("Index");
        }

        public ActionResult View(int season, int? turn)
        {
            if (turn.HasValue == false)
            {
                // find out next turn
                var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                    .Where(x => x.Season == season)
                    .Where(x => x.Date > SystemTime.UtcNow.Date)
                    .OrderBy(x => x.Date)
                    .Take(1)
                    .ToList();
                turn = rosters.Select(x => x.Turn).FirstOrDefault();
            }

            var rostersForTurn = DocumentSession.Query<Roster, RosterSearchTerms>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Include(roster => roster.Players)
                .Where(roster => roster.Turn == turn)
                .Where(roster => roster.Season == season)
                .ToArray();
            var list = rostersForTurn.Select(LoadRoster)
                .SortRosters()
                .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel
            {
                Id = turn.Value,
                Season = season,
                Rosters = list
            };
            return View(viewTurnViewModel);
        }

        [Authorize]
        public ActionResult EditPlayers(string id)
        {
            var roster = DocumentSession
                .Include<Roster>(r => r.Players)
                .Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var availablePlayers = DocumentSession.Query<Player, PlayerSearch>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .OrderBy(x => x.Name)
                .Where(p => p.IsSupporter == false);

            var vm = new EditRosterPlayersViewModel
            {
                Id = id,
                Roster = LoadRoster(roster),
                Preliminary = roster.Preliminary,
                AvailablePlayers = availablePlayers.MapTo<PlayerViewModel>().ToArray()
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPlayers(string id, RosterPlayersViewModel vm)
        {
            if (ModelState.IsValid == false) return EditPlayers(id);
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            if (roster.IsFourPlayer)
            {
                roster.Players = new List<string>
                {
                    vm.Player1,
                    vm.Player2,
                    vm.Player3,
                    vm.Player4
                };
            }
            else
            {
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
            }
            roster.Preliminary = vm.Preliminary;
            if (vm.Reserve != null && DocumentSession.Load<Player>(vm.Reserve) != null)
                roster.Players.Add(vm.Reserve);
            return RedirectToAction("View", new { season = roster.Season, turn = roster.Turn });
        }

        private RosterViewModel LoadRoster(Roster roster)
        {
            var vm = roster.MapTo<RosterViewModel>();
            foreach (var player in roster.Players.Where(p => p != null).Select(playerId => DocumentSession.Load<Player>(playerId)))
            {
                vm.Players.Add(Tuple.Create(player.Id, player.Name));
            }

            return vm;
        }
    }
}