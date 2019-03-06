namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Raven.Abstractions;
    using Raven.Client;
    using Rotativa;
    using Rotativa.Options;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Models;

    public class RosterController : AbstractController
    {
        private readonly IBitsClient bitsClient;

        public RosterController(IBitsClient bitsClient)
        {
            this.bitsClient = bitsClient;
        }

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
                    select new InitialDataViewModel.TurnViewModel(
                        seasonStart: season.Value,
                        turn: g.Key,
                        startDate: g.Min(x => x.Date),
                        endDate: lastDate,
                        rosters: g.Select(x => new RosterViewModel(
                                       x,
                                       Tuple.Create(string.Empty, string.Empty, false),
                                       new List<Tuple<string, string, bool>>()))
                                   .SortRosters()
                                   .ToArray());
            var turns = q.ToArray();
            IEnumerable<InitialDataViewModel.ScheduledItem> activities =
                DocumentSession.Query<Activity, ActivityIndex>()
                               .Where(x => x.Season == season.Value)
                               .ToArray()
                               .Where(x => selectAll || x.Date >= SystemTime.UtcNow.Date)
                               .Select(x => new InitialDataViewModel.ScheduledActivityItem(x));
            var isFiltered = rosters.Count != turns.Sum(x => x.Rosters.Length);
            var vm = new InitialDataViewModel(turns.Concat(activities).OrderBy(x => x.Date).ToArray(), season.Value, isFiltered);

            if (turns.Length <= 0) return View("Unscheduled", vm);
            return View(vm);
        }

        public ActionResult Results()
        {
            return RedirectToAction("Index");
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult CreateBits()
        {
            return View();
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
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
            var content = bitsClient.DownloadMatchResult(vm.BitsMatchId);
            var header = BitsParser.ParseHeader(content, websiteConfig.GetTeamNames());
            ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
            return View(
                "Create", new CreateRosterViewModel
                {
                    Season = season,
                    Turn = header.Turn,
                    BitsMatchId = vm.BitsMatchId,
                    Team = header.Team,
                    IsFourPlayer = false,
                    Opponent = header.Opponent,
                    Date = header.Date,
                    Location = header.Location,
                    OilPatternName = header.OilPattern.Name,
                    OilPatternUrl = header.OilPattern.Url
                });
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult Create()
        {
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
            var vm = new CreateRosterViewModel
                {
                    Season = DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year)
                };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
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
                vm.IsFourPlayer,
                new OilPatternInformation(vm.OilPatternName, vm.OilPatternUrl));
            DocumentSession.Store(roster);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult Edit(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.MatchResultId != null)
                throw new HttpException(400, "Can not modify registered rosters");
            var websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
            return View(new CreateRosterViewModel(roster));
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
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
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.MatchResultId != null)
                throw new HttpException(400, "Can not modify registered rosters");

            roster.Location = vm.Location;
            roster.Opponent = vm.Opponent;
            roster.Season = vm.Season;
            roster.Team = vm.Team.Split(';')[0];
            roster.TeamLevel = vm.Team.Split(';')[1];
            roster.Turn = vm.Turn;
            roster.BitsMatchId = vm.BitsMatchId;
            roster.IsFourPlayer = vm.IsFourPlayer;
            roster.Date = vm.Date;
            if (vm.BitsMatchId == 0)
            {
                roster.OilPattern = new OilPatternInformation(vm.OilPatternName, string.Empty);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult Delete(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.MatchResultId != null)
                throw new HttpException(400, "Can not delete registered rosters");
            return View(new RosterViewModel(
                roster,
                Tuple.Create(string.Empty, string.Empty, false),
                new List<Tuple<string, string, bool>>()));
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.MatchResultId != null)
                throw new HttpException(400, "Can not delete registered rosters");
            DocumentSession.Delete(roster);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult View(int? season, int? turn, bool? print)
        {
            if (season.HasValue == false)
            {
                // find out current season
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            }

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
                                                .Where(roster => roster.Turn == turn && roster.Season == season)
                                                .ToArray();
            var rosterViewModels = rostersForTurn.Select(DocumentSession.LoadRosterViewModel)
                                                 .SortRosters()
                                                 .ToArray();

            if (rosterViewModels.Length <= 0)
            {
                var vm = new InitialDataViewModel(
                    new InitialDataViewModel.ScheduledItem[0],
                    season.Value,
                    true);
                return View("Unscheduled", vm);
            }

            var viewTurnViewModel = new ViewTurnViewModel(
                turn.Value,
                season.Value,
                rosterViewModels,
                true,
                TenantConfiguration.AppleTouchIcon);

            if (print.GetValueOrDefault())
            {
                return View("Print", viewTurnViewModel);
            }

            return View(viewTurnViewModel);
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        [HttpPost]
        public ActionResult Print(
            int season,
            int turn,
            bool pdf,
            bool excludePast,
            bool withAbsence,
            bool excludePreliminary)
        {
            var rostersForTurn = DocumentSession.Query<Roster, RosterSearchTerms>()
                                                .Include(roster => roster.Players)
                                                .Where(roster => roster.Turn == turn && roster.Season == season)
                                                .ToArray()
                                                .Where(roster => (roster.Preliminary == false || excludePreliminary == false)
                                                                 && (roster.Date.Date >= SystemTime.UtcNow || excludePast == false))
                                                .ToArray();
            var rosterViewModels = rostersForTurn.Select(DocumentSession.LoadRosterViewModel)
                                                 .SortRosters()
                                                 .ToArray();

            var viewTurnViewModel = new ViewTurnViewModel(
                turn,
                season,
                rosterViewModels,
                withAbsence,
                TenantConfiguration.AppleTouchIcon);

            if (pdf)
            {
                var filename = $"Uttagningar-{season}-{turn}.pdf";
                var customSwitchesBuilder = new StringBuilder();
                customSwitchesBuilder.Append($"--footer-left \"Filnamn: {filename}\"");
                customSwitchesBuilder.Append(" --footer-right \"Sida [page] av [toPage]\"");
                customSwitchesBuilder.Append(" --footer-font-size \"8\"");
                customSwitchesBuilder.Append(" --footer-line");
                customSwitchesBuilder.Append(" --footer-spacing \"3\"");
                customSwitchesBuilder.Append($" --header-left \"{TenantConfiguration.FullTeamName}\"");
                customSwitchesBuilder.Append($" --header-center \"Omgång {turn}\"");
                customSwitchesBuilder.Append($" --header-right \"{DateTime.Now.Date.ToShortDateString()}\"");
                customSwitchesBuilder.Append(" --header-line");
                var customSwitches = customSwitchesBuilder.ToString();
                return new ViewAsPdf(viewTurnViewModel)
                {
                    PageSize = Size.A4,
                    FileName = filename,
                    CustomSwitches = customSwitches
                };
            }

            return View(viewTurnViewModel);
        }

        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult EditPlayers(string rosterId)
        {
            var roster = DocumentSession
                .Include<Roster>(r => r.Players)
                .Load<Roster>(rosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var availablePlayers = DocumentSession.Query<Player, PlayerSearch>()
                .OrderBy(x => x.Name)
                .Where(p => p.PlayerStatus == Player.Status.Active)
                .ToList();

            var vm = new EditRosterPlayersViewModel
            {
                RosterViewModel = DocumentSession.LoadRosterViewModel(roster),
                AvailablePlayers = availablePlayers.Select(x => new PlayerViewModel(x, WebsiteRoles.UserGroup().ToDict())).ToArray()
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
        public ActionResult EditPlayers(string rosterId, RosterPlayersViewModel vm)
        {
            if (ModelState.IsValid == false) return EditPlayers(rosterId);
            var roster = DocumentSession.Load<Roster>(rosterId);
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
                .ProjectFromIndexFieldsInto<AbsenceIndex.Result>()
                .ToArray()
                .ToLookup(x => x.Player)
                .ToDictionary(x => x.Key, x => x.ToList());

            var players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray();
            var rostersForPlayers = new Dictionary<string, List<RosterViewModel>>();
            foreach (var roster in rosters)
            {
                var rosterViewModel = new RosterViewModel(
                    roster,
                    Tuple.Create(string.Empty, string.Empty, false),
                    new List<Tuple<string, string, bool>>());
                foreach (var player in roster.Players)
                {
                    if (rostersForPlayers.TryGetValue(player, out var rosterViewModels) == false)
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
                if (resultsForPlayer.TryGetValue(player.Id, out var resultForPlayer)
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

                var activity = new PlayerStatusViewModel(player, playerForm, from, to);

                if (rostersForPlayers.ContainsKey(player.Id))
                {
                    var rostersForPlayer = rostersForPlayers[player.Id];
                    activity.Teams.AddRange(rostersForPlayer);
                }

                if (absences.TryGetValue(player.Id, out var playerAbsences))
                {
                    activity.AddAbsences(playerAbsences.OrderBy(x => x.From));
                }

                activities.Add(activity);
            }

            var vm = activities.OrderByDescending(x => x, new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm))
                               .ToArray();
            return PartialView(vm);
        }

        public ActionResult OilPattern(string id)
        {
            var roster = DocumentSession.Load<Roster>(id);
            ViewBag.Url = roster.OilPattern.Url;
            return View("_BitsIframe");
        }

        public class InitialDataViewModel
        {
            public InitialDataViewModel(
                ScheduledItem[] scheduledItems,
                int seasonStart,
                bool isFiltered)
            {
                ScheduledItems = scheduledItems;
                SeasonStart = seasonStart;
                IsFiltered = isFiltered;
            }

            public ScheduledItem[] ScheduledItems { get; }

            public int SeasonStart { get; }

            public bool IsFiltered { get; }

            public abstract class ScheduledItem
            {
                public abstract MvcHtmlString Render(HtmlHelper<InitialDataViewModel> helper);
                public abstract DateTime Date { get; }
            }

            public class TurnViewModel : ScheduledItem
            {
                public TurnViewModel(
                    int seasonStart,
                    int turn,
                    DateTime startDate,
                    DateTime endDate,
                    RosterViewModel[] rosters)
                {
                    SeasonStart = seasonStart;
                    Turn = turn;
                    StartDate = startDate;
                    EndDate = endDate;
                    Rosters = rosters;
                }

                public int SeasonStart { get; }

                public int Turn { get; }

                public DateTime StartDate { get; }

                public DateTime EndDate { get; }

                public RosterViewModel[] Rosters { get; }

                public override DateTime Date => StartDate;

                public override MvcHtmlString Render(HtmlHelper<InitialDataViewModel> helper)
                {
                    return helper.Partial("_TurnViewModel", this);
                }
            }

            public class ScheduledActivityItem : ScheduledItem
            {
                public ScheduledActivityItem(Activity activity)
                {
                    ViewModel = new ActivityViewModel(activity);
                }

                public ActivityViewModel ViewModel { get; }

                public override MvcHtmlString Render(HtmlHelper<InitialDataViewModel> helper)
                {
                    return helper.DisplayFor(x => ViewModel, new { showViewMoreLink = true });
                }

                public override DateTime Date => ViewModel.ActivityDate;
            }
        }
    }
}