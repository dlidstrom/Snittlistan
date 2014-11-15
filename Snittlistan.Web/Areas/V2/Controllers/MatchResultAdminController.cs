using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class MatchResultAdminController : AdminController
    {
        private readonly IBitsClient bitsClient;

        public MatchResultAdminController(IBitsClient bitsClient)
        {
            this.bitsClient = bitsClient;
        }

        public ActionResult Register(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            ViewBag.rosterid = DocumentSession.CreateRosterSelectList(season.Value);
            return View(new RegisterResult());
        }

        [HttpPost, ActionName("Register")]
        public ActionResult RegisterConfirmed(int? season, RegisterResult vm)
        {
            if (ModelState.IsValid == false) return Register(season);

            var roster = DocumentSession.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult(
                roster,
                vm.TeamScore.GetValueOrDefault(),
                vm.OpponentScore.GetValueOrDefault(),
                vm.BitsMatchId.GetValueOrDefault());
            EventStoreSession.Store(matchResult);

            return RedirectToAction(
                "RegisterSerie",
                new
                {
                    aggregateId = matchResult.Id,
                    vm.RosterId,
                    vm.BitsMatchId
                });
        }

        public ActionResult EditResult(int id)
        {
            var matchId = ResultHeaderReadModel.IdFromBitsMatchId(id);
            var matchResult = DocumentSession.Load<ResultHeaderReadModel>(matchId);
            if (matchResult == null) throw new HttpException(404, "Match result not found");

            ViewBag.rosterid = DocumentSession.CreateRosterSelectList(matchResult.Season, matchResult.RosterId);

            ViewBag.Title = "Redigera matchresultat";
            return View("Register", new RegisterResult(matchResult));
        }

        [HttpPost]
        public ActionResult EditResult(int id, RegisterResult registerResult)
        {
            if (registerResult == null) throw new ArgumentNullException("registerResult");
            if (!ModelState.IsValid) return EditResult(id);

            var matchResult = EventStoreSession.Load<MatchResult>(registerResult.AggregateId);
            if (matchResult == null) throw new HttpException(404, "Match result not found");

            matchResult.Update(
                DocumentSession.Load<Roster>(registerResult.RosterId),
                registerResult.TeamScore.GetValueOrDefault(),
                registerResult.OpponentScore.GetValueOrDefault(),
                registerResult.BitsMatchId.GetValueOrDefault());

            return RedirectToAction("Index", "MatchResult");
        }

        public ActionResult Delete(string id)
        {
            if (EventStoreSession.Load<MatchResult>(id) == null)
                throw new HttpException(404, "MatchResult not found");
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var matchResult = EventStoreSession.Load<MatchResult>(id);
            if (matchResult == null)
                throw new HttpException(404, "MatchResult not found");
            matchResult.Delete();

            return RedirectToAction("Index", "MatchResult");
        }

        public ActionResult RegisterSerie(string aggregateId, string rosterId, int bitsMatchId)
        {
            var roster = DocumentSession
                .Include<Roster>(r => r.Players)
                .Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            var registerSerie = new RegisterSerie(
                new ResultSeriesReadModel.Serie(),
                roster.Players.Select(
                    x => new SelectListItem
                    {
                        Text = DocumentSession.Load<Player>(x).Name,
                        Value = x
                    })
                    .ToList());
            return View(registerSerie);
        }

        [HttpPost]
        public ActionResult RegisterSerie(
            string aggregateId, int bitsMatchId, ResultSeriesReadModel.Serie serie)
        {
            var matchResult = EventStoreSession.Load<MatchResult>(aggregateId);
            if (matchResult == null) throw new HttpException(404, "Match result not found");
            var tables = new List<MatchTable>();
            for (var i = 0; i < 4; i++)
            {
                var game1 = new MatchGame(
                    serie.Tables[i].Game1.Player,
                    serie.Tables[i].Game1.Pins,
                    serie.Tables[i].Game1.Strikes,
                    serie.Tables[i].Game1.Spares);
                var game2 = new MatchGame(
                    serie.Tables[i].Game2.Player,
                    serie.Tables[i].Game2.Pins,
                    serie.Tables[i].Game2.Strikes,
                    serie.Tables[i].Game2.Spares);
                tables.Add(new MatchTable(game1, game2, serie.Tables[i].Score));
            }

            matchResult.RegisterSerie(new MatchSerie(tables));
            return RedirectToAction(
                "Details",
                "MatchResult",
                new
                {
                    id = bitsMatchId
                });
        }

        public ActionResult RegisterBits(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            ViewBag.rosterid = DocumentSession.CreateRosterSelectList(season.Value);
            return View(new RegisterBitsVerifyModel { Season = season.Value });
        }

        [Authorize]
        public ActionResult RegisterBitsVerify(RegisterBitsVerifyModel model)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.rosterid = DocumentSession.CreateRosterSelectList(model.Season);
                return View("RegisterBits", model);
            }

            var roster = DocumentSession.Include<Roster>(r => r.Players)
                .Load<Roster>(model.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var players = roster.Players
                                .Select(x => DocumentSession.Load<Player>(x))
                                .ToArray();
            var parser = new BitsParser(players);
            var content = bitsClient.DownloadMatchResult(roster.BitsMatchId);
            if (roster.IsFourPlayer)
            {
                var parse4Result = Parse4(model, parser, content, roster, players);
                return parse4Result;
            }

            var parseResult = Parse(model, parser, content, roster, players);
            return parseResult;
        }

        [HttpPost, ActionName("RegisterBitsVerify")]
        public ActionResult RegisterBitsVerifyConfirmed(RegisterBitsResult vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var roster = DocumentSession.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult(
                roster,
                vm.TeamScore,
                vm.OpponentScore,
                vm.BitsMatchId);
            foreach (var serie in new[] { vm.Serie1, vm.Serie2, vm.Serie3, vm.Serie4 }.Where(x => x != null))
            {
                var tables = new List<MatchTable>();
                for (var i = 0; i < 4; i++)
                {
                    var game1 = new MatchGame(
                        serie.Tables[i].Game1.Player,
                        serie.Tables[i].Game1.Pins,
                        serie.Tables[i].Game1.Strikes,
                        serie.Tables[i].Game1.Spares);
                    var game2 = new MatchGame(
                        serie.Tables[i].Game2.Player,
                        serie.Tables[i].Game2.Pins,
                        serie.Tables[i].Game2.Strikes,
                        serie.Tables[i].Game2.Spares);
                    tables.Add(new MatchTable(game1, game2, serie.Tables[i].Score));
                }

                matchResult.RegisterSerie(new MatchSerie(tables));
            }

            EventStoreSession.Store(matchResult);

            return RedirectToAction("Index", "MatchResult");
        }

        [HttpPost, ActionName("RegisterBitsVerify4")]
        public ActionResult RegisterBitsVerifyConfirmed4(RegisterBitsResult4 vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var roster = DocumentSession.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult4(
                roster,
                vm.TeamScore,
                vm.OpponentScore,
                vm.BitsMatchId);
            foreach (var serie in new[] { vm.Serie1, vm.Serie2, vm.Serie3, vm.Serie4 }.Where(x => x != null))
            {
                var games = new List<MatchGame4>();
                for (var i = 0; i < 4; i++)
                {
                    var game = new MatchGame4(
                        serie.Games[i].Player,
                        serie.Games[i].Score,
                        serie.Games[i].Pins);
                    games.Add(game);
                }

                matchResult.RegisterSerie(new MatchSerie4(games));
            }

            EventStoreSession.Store(matchResult);

            return RedirectToAction("Index", "MatchResult");
        }

        private ActionResult Parse(
            RegisterBitsVerifyModel model,
            BitsParser parser,
            string content,
            Roster roster,
            Player[] players)
        {
            var result = parser.Parse(content, roster.Team);
            var vm = new RegisterBitsResult
            {
                BitsMatchId = roster.BitsMatchId,
                TeamScore = result.TeamScore,
                OpponentScore = result.OpponentScore,
                RosterId = model.RosterId,
                Serie1 = result.Series.ElementAtOrDefault(0),
                Serie2 = result.Series.ElementAtOrDefault(1),
                Serie3 = result.Series.ElementAtOrDefault(2),
                Serie4 = result.Series.ElementAtOrDefault(3)
            };
            ViewBag.players = players.Select(x => new PlayerViewModel(x))
                .ToArray();
            return View("RegisterBitsVerify", vm);
        }

        private ActionResult Parse4(
            RegisterBitsVerifyModel model,
            BitsParser parser,
            string content,
            Roster roster,
            IEnumerable<Player> players)
        {
            Parse4Result result = parser.Parse4(content, roster.Team);
            var vm = new RegisterBitsResult4
            {
                BitsMatchId = roster.BitsMatchId,
                TeamScore = result.TeamScore,
                OpponentScore = result.OpponentScore,
                RosterId = model.RosterId,
                Serie1 = result.Series.ElementAtOrDefault(0),
                Serie2 = result.Series.ElementAtOrDefault(1),
                Serie3 = result.Series.ElementAtOrDefault(2),
                Serie4 = result.Series.ElementAtOrDefault(3)
            };
            ViewBag.players = players.Select(x => new PlayerViewModel(x))
                .ToArray();
            return View("RegisterBitsVerify4", vm);
        }
    }
}