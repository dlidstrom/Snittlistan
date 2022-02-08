#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Snittlistan.Web.Infrastructure.Bits;
using Raven.Abstractions;
using Raven.Client;
using Rotativa;
using Rotativa.Options;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class RosterController : AbstractController
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    private readonly IBitsClient bitsClient;

    public RosterController(IBitsClient bitsClient)
    {
        this.bitsClient = bitsClient;
    }

    public ActionResult Index(int? season)
    {
        bool selectAll = true;
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            selectAll = false;
        }

        List<Roster>? rosters = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .Where(r => r.Season == season)
            .ToList();
        IEnumerable<InitialDataViewModel.TurnViewModel> q =
            from roster in rosters
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
                               new RosterViewModel.PlayerItem(string.Empty, string.Empty, false),
                               new List<RosterViewModel.PlayerItem>()))
                           .SortRosters()
                           .ToArray());
        InitialDataViewModel.TurnViewModel[] turns = q.ToArray();
        IEnumerable<InitialDataViewModel.ScheduledItem> activities =
            CompositionRoot.DocumentSession.Query<Activity, ActivityIndex>()
                           .Where(x => x.Season == season.Value)
                           .ToArray()
                           .Where(x => selectAll || x.Date >= SystemTime.UtcNow.Date.AddDays(-3))
                           .Select(x => new InitialDataViewModel.ScheduledActivityItem(
                               x.Id!,
                               x.Title,
                               x.Date,
                               x.MessageHtml,
                               string.IsNullOrEmpty(x.AuthorId) == false ? CompositionRoot.DocumentSession.Load<Player>(x.AuthorId)?.Name ?? string.Empty : string.Empty));
        bool isFiltered = rosters.Count != turns.Sum(x => x.Rosters.Length);
        InitialDataViewModel vm = new(turns.Concat(activities).OrderBy(x => x.Date).ToArray(), season.Value, isFiltered);

        return turns.Length <= 0 ? View("Unscheduled", vm) : View(vm);
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
    public async Task<ActionResult> CreateBitsVerify(VerifyBitsViewModel vm)
    {
        if (CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .SingleOrDefault(x => x.BitsMatchId == vm.BitsMatchId) != null)
        {
            ModelState.AddModelError("BitsMatchId", "Matchen redan upplagd");
        }

        if (ModelState.IsValid == false)
        {
            return View("CreateBits");
        }

        int season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year);
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Infrastructure.Bits.Contracts.HeadInfo content = await bitsClient.GetHeadInfo(vm.BitsMatchId);
        ParseHeaderResult header = BitsParser.ParseHeader(content, websiteConfig.ClubId);
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
                Date = header.Date.ToString(DateTimeFormat),
                Location = header.Location,
                OilPatternName = header.OilPattern.Name,
                OilPatternUrl = header.OilPattern.Url
            });
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Create()
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
        CreateRosterViewModel vm = new()
        {
            Season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year)
        };
        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Create(CreateRosterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Roster roster = new(
            vm.Season,
            vm.Turn,
            vm.BitsMatchId,
            vm.Team.Split(';')[0],
            vm.Team.Split(';')[1],
            vm.Location,
            vm.Opponent,
            ParseDate(vm.Date),
            vm.IsFourPlayer,
            new OilPatternInformation(vm.OilPatternName!, vm.OilPatternUrl!));
        CompositionRoot.DocumentSession.Store(roster);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Edit(string id)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(400, "Can not modify registered rosters");
        }

        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
        return View(new CreateRosterViewModel(roster));
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    [HttpPost]
    public ActionResult Edit(string id, CreateRosterViewModel vm)
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        if (!ModelState.IsValid)
        {
            ViewBag.TeamNamesAndLevels = websiteConfig.TeamNamesAndLevels;
            return View(vm);
        }

        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(400, "Can not modify registered rosters");
        }

        roster.Location = vm.Location;
        roster.Opponent = vm.Opponent;
        roster.Season = vm.Season;
        roster.Team = vm.Team.Split(';')[0];
        roster.TeamLevel = vm.Team.Split(';')[1];
        roster.Turn = vm.Turn;
        roster.BitsMatchId = vm.BitsMatchId;
        roster.IsFourPlayer = vm.IsFourPlayer;
        roster.Date = ParseDate(vm.Date);
        if (vm.BitsMatchId == 0)
        {
            roster.OilPattern = new OilPatternInformation(vm.OilPatternName!, string.Empty);
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public ActionResult Delete(string id)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(400, "Can not delete registered rosters");
        }

        return View(new RosterViewModel(
            roster,
            new RosterViewModel.PlayerItem(string.Empty, string.Empty, false),
            new List<RosterViewModel.PlayerItem>()));
    }

    [HttpPost]
    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    [ActionName("Delete")]
    public ActionResult DeleteConfirmed(string id)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(400, "Can not delete registered rosters");
        }

        CompositionRoot.DocumentSession.Delete(roster);
        return RedirectToAction("Index");
    }

    [Authorize]
    public async Task<ActionResult> View(int? season, int? turn, bool? print)
    {
        if (season.HasValue == false)
        {
            // find out current season
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        if (turn.HasValue == false)
        {
            // find out next turn
            List<Roster> rosters = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
                .Where(x => x.Season == season)
                .Where(x => x.Date > SystemTime.UtcNow.Date)
                .OrderBy(x => x.Date)
                .Take(1)
                .ToList();
            turn = rosters.Select(x => x.Turn).FirstOrDefault();
        }

        Roster[] rostersForTurn = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .Include(roster => roster.Players)
            .Where(roster => roster.Turn == turn && roster.Season == season)
            .ToArray();
        RosterViewModel[] rosterViewModels = rostersForTurn.Select(CompositionRoot.DocumentSession.LoadRosterViewModel)
            .SortRosters()
            .ToArray();

        if (rosterViewModels.Length <= 0)
        {
            InitialDataViewModel vm = new(
                new InitialDataViewModel.ScheduledItem[0],
                season.Value,
                true);
            return View("Unscheduled", vm);
        }

        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        ViewTurnViewModel viewTurnViewModel = new(
            turn.Value,
            season.Value,
            rosterViewModels,
            true,
            tenant.AppleTouchIcon);

        if (print.GetValueOrDefault())
        {
            return View("Print", viewTurnViewModel);
        }

        return View(viewTurnViewModel);
    }

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    [HttpPost]
    public async Task<ActionResult> Print(
        int season,
        int turn,
        bool pdf,
        bool excludePast,
        bool withAbsence,
        bool excludePreliminary)
    {
        Roster[] rostersForTurn = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .Include(roster => roster.Players)
            .Where(roster => roster.Turn == turn && roster.Season == season)
            .ToArray()
            .Where(roster => (roster.Preliminary == false || excludePreliminary == false)
                                && (roster.Date.Date >= SystemTime.UtcNow || excludePast == false))
            .ToArray();
        RosterViewModel[] rosterViewModels = rostersForTurn.Select(CompositionRoot.DocumentSession.LoadRosterViewModel)
            .SortRosters()
            .ToArray();

        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        ViewTurnViewModel viewTurnViewModel = new(
            turn,
            season,
            rosterViewModels,
            withAbsence,
            tenant.AppleTouchIcon);

        if (pdf)
        {
            string filename = $"Uttagningar-{season}-{turn}.pdf";
            StringBuilder customSwitchesBuilder = new();
            string customSwitches = customSwitchesBuilder
                .Append($"--footer-left \"Filnamn: {filename}\"")
                .Append(" --footer-right \"Sida [page] av [toPage]\"")
                .Append(" --footer-font-size \"8\"")
                .Append(" --footer-line")
                .Append(" --footer-spacing \"3\"")
                .Append($" --header-left \"{tenant.TeamFullName}\"")
                .Append($" --header-center \"Omgång {turn}\"")
                .Append($" --header-right \"{DateTime.Now.Date.ToShortDateString()}\"")
                .Append(" --header-line")
                .ToString();
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
        Roster roster = CompositionRoot.DocumentSession
            .Include<Roster>(r => r.Players)
            .Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        List<Player> availablePlayers = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .OrderBy(x => x.Name)
            .Where(p => p.PlayerStatus == Player.Status.Active)
            .ToList();

        EditRosterPlayersViewModel vm = new()
        {
            RosterViewModel = CompositionRoot.DocumentSession.LoadRosterViewModel(roster),
            AvailablePlayers = availablePlayers.Select(x => new PlayerViewModel(x, WebsiteRoles.UserGroup().ToDict())).ToArray()
        };
        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public async Task<ActionResult> EditPlayers(string rosterId, RosterPlayersViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return EditPlayers(rosterId);
        }

        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        Roster.Update update = new(
            Roster.ChangeType.EditPlayers,
            User.CustomIdentity.PlayerId ?? User.CustomIdentity.Email)
        {
            Preliminary = vm.Preliminary
        };
        List<string>? players = null;
        if (roster.IsFourPlayer)
        {
            players = new List<string>
                {
                    vm.Player1!,
                    vm.Player2!,
                    vm.Player3!,
                    vm.Player4!
                };
        }
        else
        {
            players = new List<string>
                {
                    vm.Table1Player1!,
                    vm.Table1Player2!,
                    vm.Table2Player1!,
                    vm.Table2Player2!,
                    vm.Table3Player1!,
                    vm.Table3Player2!,
                    vm.Table4Player1!,
                    vm.Table4Player2!
                };
        }

        if (vm.Reserve1 != null && CompositionRoot.DocumentSession.Load<Player>(vm.Reserve1) != null)
        {
            players.Add(vm.Reserve1);
            if (vm.Reserve2 != null && CompositionRoot.DocumentSession.Load<Player>(vm.Reserve2) != null)
            {
                players.Add(vm.Reserve2);
            }
        }

        // case where reserve1 is unselected while reserve2 is selected
        if (vm.Reserve2 != null
            && players.Find(x => x == vm.Reserve2) == null
            && CompositionRoot.DocumentSession.Load<Player>(vm.Reserve2) != null)
        {
            players.Add(vm.Reserve2);
        }

        update.Players = players;

        if (vm.TeamLeader != null && CompositionRoot.DocumentSession.Load<Player>(vm.TeamLeader) != null)
        {
            update.TeamLeader = vm.TeamLeader;
        }
        else
        {
            update.TeamLeader = new Some<string?>(null);
        }

        roster.UpdateWith(CompositionRoot.CorrelationId, update);

        TenantFeatures? features = await CompositionRoot.GetFeatures();
        if (vm.SendUpdateMail && (features?.RosterMailEnabled ?? false))
        {
            if (roster.Preliminary)
            {
                Logger.Warn("Roster is preliminary, not sending requested update mail");
            }
            else if (roster.Date < SystemTime.UtcNow.ToLocalTime())
            {
                Logger.Warn("Roster date has passed, not sending requested update mail");
            }
            else
            {
                string uriString = Url.Action(
                    "View",
                    "Roster",
                    new { roster.Season, roster.Turn });
                string portPart =
                    Request.Url.Port == 80
                    ? string.Empty
                    : $":{Request.Url.Port}";
                Uri rosterLink = new(
                    $"{Request.Url.Scheme}://{Request.Url.Host}{portPart}{uriString}");
                await ExecuteCommand(
                    new CreateRosterMailCommandHandler.Command(rosterId, rosterLink));
            }
        }

        return RedirectToAction("View", new { season = roster.Season, turn = roster.Turn });
    }

    [ChildActionOnly]
    public ActionResult PlayerStatus(int turn, int season)
    {
        Roster[] rosters = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .Where(x => x.Turn == turn && x.Season == season)
            .ToArray();
        DateTime from = rosters.Select(x => x.Date.Date)
            .Min();
        DateTime to = rosters.Select(x => x.Date.Date)
            .Max();

        /*
         *    x   y
         * 1         1
         * 2    2
         *      3   3
         *     4 4
         */
        Dictionary<string, List<AbsenceIndex.Result>> absences = CompositionRoot.DocumentSession.Query<AbsenceIndex.Result, AbsenceIndex>()
            .Where(x => (x.From <= from && to <= x.To)
                || (x.From <= from && from <= x.To)
                || (x.From <= to && to <= x.To)
                || (from <= x.From && x.To <= to))
            .ProjectFromIndexFieldsInto<AbsenceIndex.Result>()
            .ToArray()
            .ToLookup(x => x.Player!)
            .ToDictionary(x => x.Key, x => x.ToList());

        Player[] players = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .ToArray();
        Dictionary<string, List<RosterViewModel>> rostersForPlayers = new();
        foreach (Roster roster in rosters)
        {
            RosterViewModel rosterViewModel = new(
                roster,
                new RosterViewModel.PlayerItem(string.Empty, string.Empty, false),
                new List<RosterViewModel.PlayerItem>());
            foreach (string player in roster.Players)
            {
                if (rostersForPlayers.TryGetValue(player, out List<RosterViewModel> rosterViewModels) == false)
                {
                    rosterViewModels = new List<RosterViewModel>();
                    rostersForPlayers.Add(player, rosterViewModels);
                }

                rosterViewModels.Add(rosterViewModel);
            }
        }

        Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer = CompositionRoot.DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
            .Where(x => x.Season == season)
            .ToArray()
            .ToDictionary(x => x.PlayerId);

        List<PlayerStatusViewModel> activities = new();
        foreach (Player player in players)
        {
            PlayerFormViewModel playerForm;
            if (resultsForPlayer.TryGetValue(player.Id, out ResultForPlayerIndex.Result resultForPlayer)
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

            PlayerStatusViewModel activity = new(player, playerForm, from, to);
            if (rostersForPlayers.ContainsKey(player.Id))
            {
                List<RosterViewModel> rostersForPlayer = rostersForPlayers[player.Id];
                activity.Teams.AddRange(rostersForPlayer);
            }

            if (absences.TryGetValue(player.Id, out List<AbsenceIndex.Result> playerAbsences))
            {
                activity.AddAbsences(playerAbsences.OrderBy(x => x.From));
            }

            activities.Add(activity);
        }

        PlayerStatusViewModel[] vm = activities.OrderByDescending(x => x, new PlayerStatusViewModel.Comparer(CompareMode.PlayerForm))
                           .ToArray();
        return PartialView(vm);
    }

    public ActionResult OilPattern(string id)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        ViewBag.Url = roster.OilPattern.Url;
        return View("_BitsIframe");
    }

    private static DateTime ParseDate(string date)
    {
        return DateTime.ParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture);
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
            public ScheduledActivityItem(
                string id,
                string title,
                DateTime date,
                string messageHtml,
                string author)
            {
                ViewModel = new ActivityViewModel(id, title, date, messageHtml, author);
            }

            public ActivityViewModel ViewModel { get; }

            public override MvcHtmlString Render(HtmlHelper<InitialDataViewModel> helper)
            {
                return helper.DisplayFor(x => ViewModel, new { showViewMoreLink = true });
            }

            public override DateTime Date => ViewModel.ActivityDate;
        }
    }

    public class CreateRosterViewModel
    {
        public CreateRosterViewModel()
        {
            Team = string.Empty;
            Location = string.Empty;
            Opponent = string.Empty;
            Date = SystemTime.UtcNow.ToLocalTime().Date.AddHours(10).ToString(DateTimeFormat);
        }

        public CreateRosterViewModel(Roster roster)
        {
            Season = roster.Season;
            Turn = roster.Turn;
            BitsMatchId = roster.BitsMatchId;
            Team = roster.Team;
            Location = roster.Location;
            Opponent = roster.Opponent;
            Date = roster.Date.ToString(DateTimeFormat);
            IsFourPlayer = roster.IsFourPlayer;
            OilPatternName = roster.OilPattern.Name;
            OilPatternUrl = roster.OilPattern.Url;
        }

        [Required]
        public int Season { get; set; }

        [Required]
        public int Turn { get; set; }

        [Required]
        public int BitsMatchId { get; set; }

        [Required]
        public string Team { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? Opponent { get; set; }

        [Required]
        [Display(Name = "Datum:")]
        public string Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public string? OilPatternName { get; set; }

        public string? OilPatternUrl { get; set; }
    }
}
