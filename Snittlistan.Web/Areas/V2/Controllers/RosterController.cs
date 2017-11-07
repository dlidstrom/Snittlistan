using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Snittlistan.Web.Models;

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
                                         .Where(r => r.Season == season)
                                         .ToList();
            var q = from roster in rosters
                    orderby roster.Turn
                    group roster by roster.Turn
                    into g
                    let lastDate = g.Max(x => x.Date)
                    where selectAll || lastDate >= SystemTime.UtcNow.Date
                    select new TurnViewModel
                    {
                        Turn = g.Key,
                        StartDate = g.Min(x => x.Date),
                        EndDate = lastDate,
                        Rosters = g.Select(x => x.MapTo<RosterViewModel>())
                                   .SortRosters()
                                   .ToList()
                    };
            var turns = q.ToList();
            if (turns.Count <= 0) return View("Unscheduled");

            var isFiltered = rosters.Count != turns.Sum(x => x.Rosters.Count);
            var vm = new InitialDataViewModel
            {
                SeasonStart = season.Value,
                Turns = turns,
                IsFiltered = isFiltered
            };
            return View(vm);
        }

        public ActionResult Results()
        {
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult CreateBits()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateBitsVerify(VerifyBitsViewModel vm)
        {
            if (DocumentSession.Query<Roster, RosterSearchTerms>()
                               .SingleOrDefault(x => x.BitsMatchId == vm.BitsMatchId) != null)
            {
                ModelState.AddModelError("BitsMatchId", "Matchen redan upplagd");
            }

            if (ModelState.IsValid == false)
                return View("CreateBits");

            var season = DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year);
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);

            using (var client = new WebClient())
            {
                var address = $"http://bits.swebowl.se/Matches/MatchFact.aspx?MatchId={vm.BitsMatchId}";
                var content = client.DownloadString(address);
                var header = BitsParser.ParseHeader(content, websiteConfig.GetTeamNames());
                ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
                return View(
                    "Create", new CreateRosterViewModel
                    {
                        Season = season,
                        Turn = 1,
                        BitsMatchId = vm.BitsMatchId,
                        Team = header.Team,
                        IsFourPlayer = false,
                        Opponent = header.Opponent,
                        Date = header.Date,
                        Location = header.Location
                    });
            }
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

            var roster = new Roster(
                vm.Season,
                vm.Turn,
                vm.BitsMatchId,
                vm.Team.Split(';')[0],
                vm.Team.Split(';')[1],
                vm.Location,
                vm.Opponent,
                vm.Date,
                vm.IsFourPlayer);
            DocumentSession.Store(roster);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
            return View(roster.MapTo<CreateRosterViewModel>());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string id, CreateRosterViewModel vm)
        {
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (!ModelState.IsValid)
            {
                ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
                return View(vm);
            }

            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null) throw new HttpException(404, "Roster not found");

            roster.Location = vm.Location;
            roster.Opponent = vm.Opponent;
            roster.Season = vm.Season;
            roster.Team = vm.Team.Split(';')[0];
            roster.TeamLevel = vm.Team.Split(';')[1];
            roster.Turn = vm.Turn;
            roster.BitsMatchId = vm.BitsMatchId;
            roster.IsFourPlayer = vm.IsFourPlayer;
            roster.Date = vm.Date;

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
                                             .Where(x => x.Season == season)
                                             .Where(x => x.Date > SystemTime.UtcNow.Date)
                                             .OrderBy(x => x.Date)
                                             .Take(1)
                                             .ToList();
                turn = rosters.Select(x => x.Turn).FirstOrDefault();
            }

            var rostersForTurn = DocumentSession.Query<Roster, RosterSearchTerms>()
                                                .Include(roster => roster.Players)
                                                .Where(roster => roster.Turn == turn)
                                                .Where(roster => roster.Season == season)
                                                .ToArray();
            var list = rostersForTurn.Select(LoadRoster)
                                     .SortRosters()
                                     .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel
            {
                Turn = turn.Value,
                Season = season,
                Rosters = list
            };
            return View(viewTurnViewModel);
        }

        [Authorize]
        public ActionResult Print(int season, int turn)
        {
            var rostersForTurn = DocumentSession.Query<Roster, RosterSearchTerms>()
                                                .Include(roster => roster.Players)
                                                .Where(roster => roster.Turn == turn)
                                                .Where(roster => roster.Season == season)
                                                .ToArray();
            var list = rostersForTurn.Select(LoadRoster)
                                     .SortRosters()
                                     .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel
            {
                Turn = turn,
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
                .OrderBy(x => x.Name)
                .Where(p => p.PlayerStatus == Player.Status.Active)
                .ToList();

            var vm = new EditRosterPlayersViewModel
            {
                Id = id,
                Roster = LoadRoster(roster),
                Preliminary = roster.Preliminary,
                AvailablePlayers = availablePlayers.Select(x => new PlayerViewModel(x)).ToArray()
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
            if (vm.Reserve1 != null && DocumentSession.Load<Player>(vm.Reserve1) != null)
            {
                roster.Players.Add(vm.Reserve1);
                if (vm.Reserve2 != null && DocumentSession.Load<Player>(vm.Reserve2) != null)
                {
                    roster.Players.Add(vm.Reserve2);
                }
            }

            // case where reserve1 is unselected while reserve2 is selected
            if (vm.Reserve2 != null
                && roster.Players.Find(x => x == vm.Reserve2) == null
                && DocumentSession.Load<Player>(vm.Reserve2) != null)
            {
                roster.Players.Add(vm.Reserve2);
            }

            if (vm.TeamLeader != null && DocumentSession.Load<Player>(vm.TeamLeader) != null)
            {
                roster.TeamLeader = vm.TeamLeader;
            }
            else
            {
                roster.TeamLeader = null;
            }

            return RedirectToAction("View", new { season = roster.Season, turn = roster.Turn });
        }

        [ChildActionOnly]
        public ActionResult PlayerStatus(int turn, int season)
        {
            var rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
                .Where(x => x.Turn == turn && x.Season == season)
                .ToArray();
            var from = rosters.Select(x => x.Date.Date)
                .Min();
            var to = rosters.Select(x => x.Date.Date)
                .Max();

            /*
             *    x   y
             * 1         1
             * 2    2
             *      3   3
             *     4 4
             */
            var absences = DocumentSession.Query<AbsenceIndex.Result, AbsenceIndex>()
                .Where(x => x.From <= from && to <= x.To
                    || x.From <= from && from <= x.To
                    || x.From <= to && to <= x.To
                    || from <= x.From && x.To <= to)
                .OrderBy(p => p.To)
                .ThenBy(p => p.PlayerName)
                .AsProjection<AbsenceIndex.Result>()
                .ToArray()
                .ToLookup(x => x.Player)
                .ToDictionary(x => x.Key, x => x.ToList());

            var players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray();
            var rostersForPlayers = new Dictionary<string, List<RosterViewModel>>();
            foreach (var roster in rosters)
            {
                var rosterViewModel = roster.MapTo<RosterViewModel>();
                foreach (var player in roster.Players)
                {
                    List<RosterViewModel> rosterViewModels;
                    if (rostersForPlayers.TryGetValue(player, out rosterViewModels) == false)
                    {
                        rosterViewModels = new List<RosterViewModel>();
                        rostersForPlayers.Add(player, rosterViewModels);
                    }

                    rosterViewModels.Add(rosterViewModel);
                }
            }

            var resultsForPlayer = DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                                                  .Where(x => x.Season == season)
                                                  .ToArray()
                                                  .ToDictionary(x => x.PlayerId);

            var activities = new List<PlayerStatusViewModel>();
            foreach (var player in players)
            {
                PlayerFormViewModel playerForm;
                ResultForPlayerIndex.Result resultForPlayer;
                if (resultsForPlayer.TryGetValue(player.Id, out resultForPlayer)
                    && resultForPlayer.TotalSeries > 0)
                {
                    playerForm = new PlayerFormViewModel(player.Name)
                    {
                        TotalSeries = resultForPlayer.TotalSeries,
                        SeasonAverage = (double)resultForPlayer.TotalPins / Math.Max(1, resultForPlayer.TotalSeries),
                        Last5Average = (double)resultForPlayer.Last5TotalPins / Math.Max(1, resultForPlayer.Last5TotalSeries),
                        HasResult = true
                    };
                }
                else if (player.PlayerStatus == Player.Status.Active)
                {
                    playerForm = new PlayerFormViewModel(player.Name);
                }
                else
                {
                    continue;
                }

                var activity = new PlayerStatusViewModel(player.Name, playerForm);

                if (rostersForPlayers.ContainsKey(player.Id))
                {
                    var rostersForPlayer = rostersForPlayers[player.Id];
                    activity.Teams.AddRange(rostersForPlayer);
                }

                List<AbsenceIndex.Result> playerAbsences;
                if (absences.TryGetValue(player.Id, out playerAbsences))
                {
                    activity.Absences.AddRange(playerAbsences.OrderBy(x => x.From));
                }

                activities.Add(activity);
            }

            var activitiesWithNoAbsence = activities.Where(x => x.Absences.Count == 0);
            var activitiesWithAbsence = activities.Where(x => x.Absences.Count > 0);
            var vm = activitiesWithNoAbsence.OrderByDescending(x => x, new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm))
                .Concat(activitiesWithAbsence.OrderBy(x => x.Absences.Min(y => y.To)).ThenBy(x => x.Name));
            return PartialView(vm);
        }

        private RosterViewModel LoadRoster(Roster roster)
        {
            var vm = roster.MapTo<RosterViewModel>();
            foreach (var player in roster.Players.Where(p => p != null).Select(playerId => DocumentSession.Load<Player>(playerId)))
            {
                vm.Players.Add(Tuple.Create(player.Id, player.Name));
            }

            if (roster.TeamLeader != null)
            {
                var teamLeader = DocumentSession.Load<Player>(roster.TeamLeader);
                vm.TeamLeader = Tuple.Create(teamLeader.Id, teamLeader.Name);
            }

            return vm;
        }
    }
}