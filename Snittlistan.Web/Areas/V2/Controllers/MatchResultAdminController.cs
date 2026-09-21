#nullable enable

using Snittlistan.Web.Areas.V2.Indexes;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.Areas.V2.Controllers;

[Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
public class MatchResultAdminController : AbstractController
{
    public ActionResult Register(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        ViewBag.rosterid = CompositionRoot.DocumentSession.CreateRosterSelectList(season.Value);
        return View();
    }

    [HttpPost]
    [ActionName("Register")]
    public ActionResult Register_RosterSelected(string rosterId)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.IsFourPlayer)
        {
            return RedirectToAction("RegisterMatch4Editor", new { rosterId });
        }

        return RedirectToAction("RegisterMatchEditor", new { rosterId });
    }

    public ActionResult RegisterMatch4Editor(string rosterId)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(500, "Roster already registered");
        }

        List<Player> availablePlayers = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .OrderBy(x => x.Name)
            .Where(p => p.PlayerStatus == Player.Status.Active)
            .ToList();
        SelectListItem[] playerListItems = availablePlayers.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        }).ToArray();

        RegisterMatch4ViewModel viewModel = new(
            CompositionRoot.DocumentSession.LoadRosterViewModel(roster),
            playerListItems,
            RegisterMatch4ViewModel.PostModel.ForCreate());
        return View(viewModel);
    }

    [HttpPost]
    [ActionName("RegisterMatch4Editor")]
    public async Task<ActionResult> RegisterMatchEditorStore(string rosterId, RegisterMatch4ViewModel viewModel)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (ModelState.IsValid == false)
        {
            List<Player> availablePlayers = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
                .OrderBy(x => x.Name)
                .Where(p => p.PlayerStatus == Player.Status.Active)
                .ToList();
            SelectListItem[] playerListItems = availablePlayers.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToArray();
            viewModel.RosterViewModel = CompositionRoot.DocumentSession.LoadRosterViewModel(roster);
            viewModel.PlayerListItems = playerListItems;
            return View(
                "RegisterMatch4Editor",
                viewModel);
        }

        int[,] movement = new[,]
        {
                { 1, 2, 3, 4 },
                { 3, 4, 1, 2 },
                { 4, 3, 2, 1 },
                { 2, 1, 4, 3 }
            };
        List<ResultSeries4ReadModel.Serie> series = new();

        // keep track of who is the reserve
        int currentReserve = 4;
        int[] subs = new[] { 0, 1, 2, 3, 4 };
        for (int i = 0; i < 4; i++)
        {
            ResultSeries4ReadModel.Serie serie = new();
            serie.Games.Clear();
            List<PlayerGames> games = new();
            for (int j = 0; j < viewModel.Model!.Players!.Length; j++)
            {
                if (games.Count == 4)
                {
                    break;
                }

                if (viewModel.Model.Players[j].Games![i].Pins.HasValue == false
                    && currentReserve != j)
                {
                    int temp = subs[j];
                    subs[Array.IndexOf(subs, j)] = currentReserve;
                    subs[4] = temp;
                    currentReserve = j;
                }
            }

            games.AddRange(subs.Take(4).Select(sub => viewModel.Model.Players[sub]));

            int leftPos = movement[i, 0] - 1;
            int centerLeftPos = movement[i, 1] - 1;
            int centerRightPos = movement[i, 2] - 1;
            int rightPos = movement[i, 3] - 1;
            foreach (int pos in new[] { leftPos, centerLeftPos, centerRightPos, rightPos })
            {
                Debug.Assert(games[pos].Games![i].Pins != null, $"games[{pos}].Games[{i}].Pins != null");
                ResultSeries4ReadModel.Game game = new()
                {
                    Player = games[pos].PlayerId!,
                    Score = games[pos].Games![i].Score ? 1 : 0,
                    Pins = games[pos].Games![i].Pins!.Value,
                };
                serie.Games.Add(game);
            }

            series.Add(serie);
        }

        roster.SetPlayers(
            viewModel.Model!
                .Players
                .Where(x => x.PlayerId != null)
                .Select(x => x.PlayerId!).ToList());
        Debug.Assert(viewModel.Model.TeamScore != null, "viewModel.Model.TeamScore != null");
        Debug.Assert(viewModel.Model.OpponentScore != null, "viewModel.Model.OpponentScore != null");
        Parse4Result parse4Result = new(
            viewModel.Model.TeamScore!.Value,
            viewModel.Model.OpponentScore!.Value,
            roster.Turn,
            series.ToArray());
        await ExecuteCommand(
            new RegisterMatch4CommandHandler.Command(
                roster.Id!,
                parse4Result,
                viewModel.Model.Commentary,
                viewModel.Model.CommentaryHtml));

        return RedirectToAction(
            "Details",
            "MatchResult",
            new
            {
                Id = roster.BitsMatchId,
                RosterId = roster.Id
            });
    }

    public ActionResult RegisterMatchEditor(string rosterId)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (roster.MatchResultId != null)
        {
            throw new HttpException(500, "Roster already registered");
        }

        SelectListItem[] playerListItems = LoadActivePlayerListItems();

        RegisterMatchViewModel viewModel = new(
            CompositionRoot.DocumentSession.LoadRosterViewModel(roster),
            playerListItems,
            RegisterMatchViewModel.PostModel.ForCreate(roster.AcceptedPlayers));
        return View(viewModel);
    }

    [HttpPost]
    [ActionName("RegisterMatchEditor")]
    public async Task<ActionResult> RegisterMatchEditorManualStore(string rosterId, RegisterMatchViewModel viewModel)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(rosterId);
        if (roster == null)
        {
            throw new HttpException(404, "Roster not found");
        }

        if (ModelState.IsValid == false)
        {
            viewModel.RosterViewModel = CompositionRoot.DocumentSession.LoadRosterViewModel(roster);
            viewModel.PlayerListItems = LoadActivePlayerListItems();
            return View(
                "RegisterMatchEditor",
                viewModel);
        }

        RegisterMatchViewModel.PostModel model = viewModel.Model!;
        MatchSerie[] matchSeries = new MatchSerie[4];
        HashSet<string> usedPlayerIds = new();
        for (int i = 0; i < 4; i++)
        {
            MatchTable[] matchTables = new MatchTable[4];
            for (int t = 0; t < 4; t++)
            {
                RegisterMatchViewModel.PlayerRow p1 = model.Players![t * 2];
                RegisterMatchViewModel.PlayerRow p2 = model.Players[(t * 2) + 1];
                MatchGame game1 = new(p1.PlayerId!, p1.Games![i].Pins!.Value, 0, 0);
                MatchGame game2 = new(p2.PlayerId!, p2.Games![i].Pins!.Value, 0, 0);
                matchTables[t] = new MatchTable(t + 1, game1, game2, model.Wins![t].Won![i] ? 1 : 0);
                usedPlayerIds.Add(p1.PlayerId!);
                usedPlayerIds.Add(p2.PlayerId!);
            }

            matchSeries[i] = new MatchSerie(i + 1, matchTables);
        }

        roster.SetPlayers(usedPlayerIds.ToList());

        await ExecuteCommand(
            new RegisterMatchManualCommandHandler.Command(
                roster.Id!,
                model.TeamScore!.Value,
                model.OpponentScore!.Value,
                matchSeries,
                model.Commentary!,
                model.CommentaryHtml!));

        return RedirectToAction(
            "Details",
            "MatchResult",
            new
            {
                Id = roster.BitsMatchId,
                RosterId = roster.Id
            });
    }

    private SelectListItem[] LoadActivePlayerListItems()
    {
        List<Player> availablePlayers = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .OrderBy(x => x.Name)
            .Where(p => p.PlayerStatus == Player.Status.Active)
            .ToList();
        return availablePlayers.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        }).ToArray();
    }

    public class RegisterMatch4ViewModel
    {
        public RegisterMatch4ViewModel()
        {
        }

        public RegisterMatch4ViewModel(
            RosterViewModel rosterViewModel,
            SelectListItem[] playerListItems,
            PostModel postModel)
        {
            RosterViewModel = rosterViewModel;
            PlayerListItems = playerListItems;
            Model = postModel;
        }

        public RosterViewModel? RosterViewModel { get; set; }

        public SelectListItem[]? PlayerListItems { get; set; }

        public PostModel? Model { get; set; }

        public class PostModel : IValidatableObject
        {
            public PostModel()
            {
            }

            public PostModel(PlayerGames[] players)
            {
                Players = players;
            }

            [MaxLength(1024)]
            public string? Commentary { get; set; }

            [MaxLength(1024)]
            [AllowHtml]
            public string? CommentaryHtml { get; set; }

            [Display(Name = "Matchreferat")]
            public IHtmlString? CommentaryDisplay { get; set; }

            [Required]
            [Range(0, 20)]
            [Display(Name = "Lagpoäng")]
            public int? TeamScore { get; set; }

            [Required]
            [Range(0, 20)]
            [Display(Name = "Motståndarpoäng")]
            public int? OpponentScore { get; set; }

            public PlayerGames[]? Players { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (TeamScore.GetValueOrDefault() + OpponentScore.GetValueOrDefault() > 20)
                {
                    yield return new ValidationResult("Summan av lagpoängen kan inte överstiga 20.");
                }

                for (int i = 0; i < 4; i++)
                {
                    if (Players.Count(x => x.Games![i].Pins.HasValue) != 4)
                    {
                        yield return new ValidationResult($"Ange 4 resultat i serie {i + 1}");
                    }
                }

                if (Players![4].Games![0].Pins.HasValue)
                {
                    yield return new ValidationResult("Reserven spelar inte i första serien.");
                }

                if (Players.Any(x => x.Games.Any(y => y.Pins.HasValue == false && y.Score)))
                {
                    yield return new ValidationResult("Reserven kan inte vinna en serie.");
                }

                if (Players.Any(x => x.PlayerId == null && x.Games.Any(y => y.Pins.HasValue)))
                {
                    yield return new ValidationResult("Ange spelaren som ska ha resultat.");
                }
            }

            public static PostModel ForCreate()
            {
                return new PostModel(
                    new[]
                    {
                            new PlayerGames(new PlayerGame[4]),
                            new PlayerGames(new PlayerGame[4]),
                            new PlayerGames(new PlayerGame[4]),
                            new PlayerGames(new PlayerGame[4]),
                            new PlayerGames(new PlayerGame[4])
                    });
            }
        }
    }

    public class RegisterMatchViewModel
    {
        public RegisterMatchViewModel()
        {
        }

        public RegisterMatchViewModel(
            RosterViewModel rosterViewModel,
            SelectListItem[] playerListItems,
            PostModel postModel)
        {
            RosterViewModel = rosterViewModel;
            PlayerListItems = playerListItems;
            Model = postModel;
        }

        public RosterViewModel? RosterViewModel { get; set; }

        public SelectListItem[]? PlayerListItems { get; set; }

        public PostModel? Model { get; set; }

        public class PostModel : IValidatableObject
        {
            public PostModel()
            {
            }

            public PostModel(PlayerRow[] players, TableWin[] wins)
            {
                Players = players;
                Wins = wins;
            }

            [MaxLength(1024)]
            [Required]
            public string? Commentary { get; set; }

            [MaxLength(1024)]
            [AllowHtml]
            [Required]
            public string? CommentaryHtml { get; set; }

            [Display(Name = "Matchreferat")]
            public IHtmlString? CommentaryDisplay { get; set; }

            [Required]
            [Range(0, 20)]
            [Display(Name = "Lagpoäng")]
            public int? TeamScore { get; set; }

            [Required]
            [Range(0, 20)]
            [Display(Name = "Motståndarpoäng")]
            public int? OpponentScore { get; set; }

            /// <summary>
            /// Eight rows, paired two-by-two into the four tables (rows 0-1 = table 1, etc.),
            /// with the same player playing all four series at their table.
            /// </summary>
            public PlayerRow[]? Players { get; set; }

            /// <summary>One entry per table (four in total), holding which series that table won.</summary>
            public TableWin[]? Wins { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (TeamScore.GetValueOrDefault() + OpponentScore.GetValueOrDefault() > 20)
                {
                    yield return new ValidationResult("Summan av lagpoängen kan inte överstiga 20.");
                }

                for (int i = 0; i < Players!.Length; i++)
                {
                    if (string.IsNullOrEmpty(Players[i].PlayerId))
                    {
                        yield return new ValidationResult($"Välj spelare på rad {i + 1}.");
                    }

                    for (int s = 0; s < 4; s++)
                    {
                        if (Players[i].Games![s].Pins.HasValue == false)
                        {
                            yield return new ValidationResult($"Ange resultat för rad {i + 1} i serie {s + 1}.");
                        }
                    }
                }

                for (int t = 0; t < 4; t++)
                {
                    if (string.IsNullOrEmpty(Players[t * 2].PlayerId) == false
                        && Players[t * 2].PlayerId == Players[(t * 2) + 1].PlayerId)
                    {
                        yield return new ValidationResult($"Samma spelare kan inte spela mot sig själv, bord {t + 1}.");
                    }
                }
            }

            public static PostModel ForCreate(IEnumerable<string> acceptedPlayerIds)
            {
                string[] playerIds = acceptedPlayerIds.Take(8).ToArray();
                PlayerRow[] players = Enumerable.Range(0, 8)
                    .Select(i => new PlayerRow
                    {
                        PlayerId = i < playerIds.Length ? playerIds[i] : null,
                        Games = Enumerable.Range(0, 4).Select(_ => new PinsCell()).ToArray()
                    })
                    .ToArray();
                TableWin[] wins = Enumerable.Range(0, 4)
                    .Select(_ => new TableWin { Won = new bool[4] })
                    .ToArray();
                return new PostModel(players, wins);
            }
        }

        public class PlayerRow
        {
            public string? PlayerId { get; set; }

            public PinsCell[]? Games { get; set; }
        }

        public class PinsCell
        {
            [Range(0, 300)]
            public int? Pins { get; set; }
        }

        public class TableWin
        {
            public bool[]? Won { get; set; }
        }
    }

    public class PlayerGame
    {
        public bool Score { get; set; }

        [Range(0, 300)]
        public int? Pins { get; set; }
    }

    public class PlayerGames
    {
        public PlayerGames()
        {
        }

        public PlayerGames(PlayerGame[] games)
        {
            Games = games;
        }

        public string? PlayerId { get; set; }

        public PlayerGame[]? Games { get; set; }
    }

    public class RegisterResult
    {
        public RegisterResult()
        {
            Series = new RegisterSerie[4];
        }

        public RegisterResult(ResultHeaderReadModel matchResult)
        {
            AggregateId = matchResult.AggregateId;
            TeamScore = matchResult.TeamScore;
            OpponentScore = matchResult.OpponentScore;
        }

        [HiddenInput]
        public string? AggregateId { get; set; }

        [Required]
        public string? RosterId { get; set; }

        [Range(0, 20), Required]
        public int? TeamScore { get; set; }

        [Range(0, 20), Required]
        public int? OpponentScore { get; set; }

        [Required]
        public int? BitsMatchId { get; set; }

        public RegisterSerie[]? Series { get; set; }
    }
}
