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
        if (TrySpreadTableWins(model.Players!, out MatchSerie[]? matchSeries, out string? spreadError) == false)
        {
            ModelState.AddModelError(string.Empty, spreadError!);
            viewModel.RosterViewModel = CompositionRoot.DocumentSession.LoadRosterViewModel(roster);
            viewModel.PlayerListItems = LoadActivePlayerListItems();
            return View(
                "RegisterMatchEditor",
                viewModel);
        }

        HashSet<string> usedPlayerIds = model.Players!
            .Where(p => p.Games!.Any(g => g.Pins.HasValue))
            .Select(p => p.PlayerId!)
            .ToHashSet();

        roster.SetPlayers(usedPlayerIds.ToList());

        await ExecuteCommand(
            new RegisterMatchManualCommandHandler.Command(
                roster.Id!,
                model.TeamScore!.Value,
                model.OpponentScore!.Value,
                matchSeries!,
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

    /// <summary>
    /// Splits each series into four tables of two, deciding the pairing and win/loss per table so
    /// that each player ends up with exactly the number of table wins they were given, ignoring who
    /// actually played alongside whom. A table's win is always shared by both its players, so within
    /// a series, players are freely re-paired every round: winners grouped together, losers grouped
    /// together. That is always enough to realize any per-player win counts, since credit is only
    /// ever handed out in pairs; the loop below prioritizes players who have no slack left (their
    /// remaining win target equals their remaining series) each round.
    /// </summary>
    private static bool TrySpreadTableWins(
        RegisterMatchViewModel.PlayerRow[] players,
        out MatchSerie[]? matchSeries,
        out string? error)
    {
        int[] remainingNeed = players.Select(p => p.TableWins.GetValueOrDefault()).ToArray();
        int[][] activeSeries = players
            .Select(p => Enumerable.Range(0, 4).Where(s => p.Games![s].Pins.HasValue).ToArray())
            .ToArray();

        for (int i = 0; i < players.Length; i++)
        {
            if (remainingNeed[i] > activeSeries[i].Length)
            {
                matchSeries = null;
                error = $"Rad {i + 1} kan inte ha fler bordpoäng ({remainingNeed[i]}) än antal spelade serier ({activeSeries[i].Length}).";
                return false;
            }
        }

        MatchSerie[] series = new MatchSerie[4];
        for (int s = 0; s < 4; s++)
        {
            List<int> active = Enumerable.Range(0, players.Length)
                .Where(i => players[i].Games![s].Pins.HasValue)
                .ToList();
            if (active.Count != 8)
            {
                matchSeries = null;
                error = $"Serie {s + 1} måste ha resultat för exakt 8 spelare (har {active.Count}).";
                return false;
            }

            List<int> remainingRoundsFromHere = active
                .Select(i => activeSeries[i].Count(round => round >= s))
                .ToList();

            List<int> winners = new();
            List<int> slack = new();
            for (int k = 0; k < active.Count; k++)
            {
                int i = active[k];
                if (remainingNeed[i] <= 0)
                {
                    continue;
                }

                if (remainingNeed[i] == remainingRoundsFromHere[k])
                {
                    winners.Add(i);
                }
                else
                {
                    slack.Add(i);
                }
            }

            if (winners.Count % 2 == 1)
            {
                if (slack.Count > 0)
                {
                    winners.Add(slack[0]);
                }
                else
                {
                    matchSeries = null;
                    error = "Kunde inte fördela bordpoängen jämnt över serierna. Justera antalet bordpoäng för någon spelare.";
                    return false;
                }
            }

            foreach (int w in winners)
            {
                remainingNeed[w]--;
            }

            List<int> losers = active.Except(winners).ToList();
            MatchTable[] matchTables = new MatchTable[4];
            int table = 0;
            for (int k = 0; k + 1 < winners.Count; k += 2)
            {
                matchTables[table] = BuildTable(table + 1, players, winners[k], winners[k + 1], s, score: 1);
                table++;
            }

            for (int k = 0; k + 1 < losers.Count; k += 2)
            {
                matchTables[table] = BuildTable(table + 1, players, losers[k], losers[k + 1], s, score: 0);
                table++;
            }

            series[s] = new MatchSerie(s + 1, matchTables);
        }

        if (remainingNeed.Any(need => need != 0))
        {
            matchSeries = null;
            error = "Kunde inte fördela bordpoängen jämnt över serierna. Justera antalet bordpoäng för någon spelare.";
            return false;
        }

        matchSeries = series;
        error = null;
        return true;
    }

    private static MatchTable BuildTable(
        int tableNumber,
        RegisterMatchViewModel.PlayerRow[] players,
        int index1,
        int index2,
        int serie,
        int score)
    {
        RegisterMatchViewModel.PlayerRow p1 = players[index1];
        RegisterMatchViewModel.PlayerRow p2 = players[index2];
        MatchGame game1 = new(p1.PlayerId!, p1.Games![serie].Pins!.Value, 0, 0);
        MatchGame game2 = new(p2.PlayerId!, p2.Games![serie].Pins!.Value, 0, 0);
        return new MatchTable(tableNumber, game1, game2, score);
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

            public PostModel(PlayerRow[] players)
            {
                Players = players;
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
            /// Up to nine rows: the eight regular players plus an optional ninth reserve who may
            /// sub in for exactly one of them in a given series. Which 8 played a given series is
            /// derived from which rows have a result entered for it; table pairing within a series
            /// is computed from everyone's <see cref="PlayerRow.TableWins"/> - who partners with
            /// whom doesn't matter.
            /// </summary>
            public PlayerRow[]? Players { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (TeamScore.GetValueOrDefault() + OpponentScore.GetValueOrDefault() > 20)
                {
                    yield return new ValidationResult("Summan av lagpoängen kan inte överstiga 20.");
                }

                int totalPlayedSeries = 0;
                for (int i = 0; i < Players!.Length; i++)
                {
                    int playedSeries = Players[i].Games!.Count(g => g.Pins.HasValue);
                    totalPlayedSeries += playedSeries;
                    if (playedSeries > 0 && string.IsNullOrEmpty(Players[i].PlayerId))
                    {
                        yield return new ValidationResult($"Välj spelare på rad {i + 1}.");
                    }

                    if (playedSeries > 0 && Players[i].TableWins.GetValueOrDefault() > playedSeries)
                    {
                        yield return new ValidationResult(
                            $"Rad {i + 1} kan inte ha fler bordpoäng än antal spelade serier ({playedSeries}).");
                    }
                }

                List<string> chosenPlayerIds = Players
                    .Where(p => string.IsNullOrEmpty(p.PlayerId) == false)
                    .Select(p => p.PlayerId!)
                    .ToList();
                if (chosenPlayerIds.Distinct().Count() != chosenPlayerIds.Count)
                {
                    yield return new ValidationResult("Samma spelare kan inte finnas på flera rader.");
                }

                for (int s = 0; s < 4; s++)
                {
                    int playedCount = Players.Count(p => p.Games![s].Pins.HasValue);
                    if (playedCount != 8)
                    {
                        yield return new ValidationResult($"Serie {s + 1} måste ha resultat för exakt 8 spelare (har {playedCount}).");
                    }
                }

                int totalTableWins = Players.Sum(p => p.TableWins.GetValueOrDefault());
                if (totalTableWins % 2 != 0)
                {
                    yield return new ValidationResult(
                        "Summan av allas bordpoäng måste vara jämn (varje bord som vinner ger poäng till två spelare).");
                }
            }

            public static PostModel ForCreate(IEnumerable<string> acceptedPlayerIds)
            {
                string[] playerIds = acceptedPlayerIds.Take(9).ToArray();
                PlayerRow[] players = Enumerable.Range(0, 9)
                    .Select(i => new PlayerRow
                    {
                        PlayerId = i < playerIds.Length ? playerIds[i] : null,
                        Games = Enumerable.Range(0, 4).Select(_ => new PinsCell()).ToArray()
                    })
                    .ToArray();
                return new PostModel(players);
            }
        }

        public class PlayerRow
        {
            public string? PlayerId { get; set; }

            public PinsCell[]? Games { get; set; }

            [Range(0, 4)]
            [Display(Name = "Bordpoäng")]
            public int? TableWins { get; set; }
        }

        public class PinsCell
        {
            [Range(0, 300)]
            public int? Pins { get; set; }
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
