namespace Snittlistan.Web.Areas.V2.Controllers
{
    using Indexes;
    using Raven.Abstractions;
    using Snittlistan.Web.Areas.V2.Commands;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
    public class MatchResultAdminController : AbstractController
    {
        public ActionResult Register(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            // only support for 4-player as of now
            ViewBag.rosterid = DocumentSession.CreateRosterSelectList(season.Value, pred: x => x.IsFourPlayer);
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public ActionResult Register_RosterSelected(string rosterId)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.IsFourPlayer)
                return RedirectToAction("RegisterMatch4Editor", new { rosterId });
            return RedirectToAction("Index", "MatchResult");
        }

        public ActionResult RegisterMatch4Editor(string rosterId)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (roster.MatchResultId != null)
                throw new HttpException(500, "Roster already registered");

            var availablePlayers = DocumentSession.Query<Player, PlayerSearch>()
                                                  .OrderBy(x => x.Name)
                                                  .Where(p => p.PlayerStatus == Player.Status.Active)
                                                  .ToList();
            SelectListItem[] playerListItems = availablePlayers.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToArray();

            var viewModel = new RegisterMatch4ViewModel(
                    DocumentSession.LoadRosterViewModel(roster),
                    playerListItems,
                    RegisterMatch4ViewModel.PostModel.ForCreate());
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("RegisterMatch4Editor")]
        public ActionResult RegisterMatchEditorStore(string rosterId, RegisterMatch4ViewModel viewModel)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");
            if (ModelState.IsValid == false)
            {
                var availablePlayers = DocumentSession.Query<Player, PlayerSearch>()
                                                      .OrderBy(x => x.Name)
                                                      .Where(p => p.PlayerStatus == Player.Status.Active)
                                                      .ToList();
                SelectListItem[] playerListItems = availablePlayers.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToArray();
                viewModel.RosterViewModel = DocumentSession.LoadRosterViewModel(roster);
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
            var series = new List<ResultSeries4ReadModel.Serie>();

            // keep track of who is the reserve
            int currentReserve = 4;
            int[] subs = new[] { 0, 1, 2, 3, 4 };
            for (int i = 0; i < 4; i++)
            {
                var serie = new ResultSeries4ReadModel.Serie();
                serie.Games.Clear();
                var games = new List<PlayerGames>();
                for (int j = 0; j < viewModel.Model.Players.Length; j++)
                {
                    if (games.Count == 4) break;

                    if (viewModel.Model.Players[j].Games[i].Pins.HasValue == false
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
                    Debug.Assert(games[pos].Games[i].Pins != null, $"games[{pos}].Games[{i}].Pins != null");
                    var game = new ResultSeries4ReadModel.Game
                    {
                        Player = games[pos].PlayerId,
                        Score = games[pos].Games[i].Score ? 1 : 0,
                        Pins = games[pos].Games[i].Pins.Value,
                    };
                    serie.Games.Add(game);
                }

                series.Add(serie);
            }

            roster.Players = viewModel.Model
                                      .Players
                                      .Where(x => x.PlayerId != null)
                                      .Select(x => x.PlayerId).ToList();
            Debug.Assert(viewModel.Model.TeamScore != null, "viewModel.Model.TeamScore != null");
            Debug.Assert(viewModel.Model.OpponentScore != null, "viewModel.Model.OpponentScore != null");
            var parse4Result = new Parse4Result(
                viewModel.Model.TeamScore.Value,
                viewModel.Model.OpponentScore.Value,
                roster.Turn,
                series.ToArray());
            ExecuteCommand(
                new RegisterMatch4Command(
                    roster,
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

            public RosterViewModel RosterViewModel { get; set; }

            public SelectListItem[] PlayerListItems { get; set; }

            public PostModel Model { get; set; }

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
                public string Commentary { get; set; }

                [MaxLength(1024)]
                [AllowHtml]
                public string CommentaryHtml { get; set; }

                [Display(Name = "Matchreferat")]
                public IHtmlString CommentaryDisplay { get; set; }

                [Required]
                [Range(0, 20)]
                [Display(Name = "Lagpoäng")]
                public int? TeamScore { get; set; }

                [Required]
                [Range(0, 20)]
                [Display(Name = "Motståndarpoäng")]
                public int? OpponentScore { get; set; }

                public PlayerGames[] Players { get; set; }

                public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
                {
                    if (TeamScore.GetValueOrDefault() + OpponentScore.GetValueOrDefault() > 20)
                    {
                        yield return new ValidationResult("Summan av lagpoängen kan inte överstiga 20.");
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (Players.Count(x => x.Games[i].Pins.HasValue) != 4)
                        {
                            yield return new ValidationResult($"Ange 4 resultat i serie {i + 1}");
                        }
                    }

                    if (Players[4].Games[0].Pins.HasValue)
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
            public RegisterMatchViewModel(SelectListItem[] playerListItems)
            {
                Series = new RegisterSerie[4];
                PlayerListItems = playerListItems;
            }

            public RegisterMatchViewModel(
                int teamScore,
                int opponentScore,
                RegisterSerie[] series,
                SelectListItem[] playerListItems)
            {
                TeamScore = teamScore;
                OpponentScore = opponentScore;
                Series = series;
                PlayerListItems = playerListItems;
            }

            [Range(0, 20)]
            public int TeamScore { get; }

            [Range(0, 20)]
            public int OpponentScore { get; }

            public RegisterSerie[] Series { get; }

            public SelectListItem[] PlayerListItems { get; }
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

            public string PlayerId { get; set; }

            public PlayerGame[] Games { get; set; }
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
            public string AggregateId { get; set; }

            [Required]
            public string RosterId { get; set; }

            [Range(0, 20), Required]
            public int? TeamScore { get; set; }

            [Range(0, 20), Required]
            public int? OpponentScore { get; set; }

            [Required]
            public int? BitsMatchId { get; set; }

            public RegisterSerie[] Series { get; set; }
        }
    }
}