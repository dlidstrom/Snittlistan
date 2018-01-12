 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
 using Snittlistan.Web.Areas.V2.Commands;
 using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    [Authorize]
    public class MatchResultAdminController : AbstractController
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
            if (registerResult == null) throw new ArgumentNullException(nameof(registerResult));
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

        public ActionResult RegisterSerie(string aggregateId, string rosterId, int bitsMatchId)
        {
            var roster = DocumentSession.Include<Roster>(r => r.Players)
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
            string aggregateId,
            int bitsMatchId,
            ResultSeriesReadModel.Serie serie)
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
                tables.Add(new MatchTable(i + 1, game1, game2, serie.Tables[i].Score));
            }

            matchResult.RegisterSerie(tables.ToArray());
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

        [HttpPost]
        public ActionResult RegisterBits(RegisterBitsVerifyModel model)
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
                var parse4Result = parser.Parse4(content, roster.Team);
                ExecuteCommand(new RegisterMatch4Command(roster, parse4Result));
            }
            else
            {
                var parseResult = parser.Parse(content, roster.Team);
                ExecuteCommand(new RegisterMatchCommand(roster, parseResult));
            }

            return RedirectToAction("Index", "MatchResult");
        }
    }
}