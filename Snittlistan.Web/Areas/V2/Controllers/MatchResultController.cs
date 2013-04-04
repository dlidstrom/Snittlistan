using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Helpers;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class MatchResultController : AbstractController
    {
        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = this.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var vm = new MatchResultViewModel {
                                                  SeasonStart = season.Value,
                                                  Turns = DocumentSession.Query<ResultHeaderReadModel>()
                                                      .ToList()
                                                      .GroupBy(x => x.Turn)
                                                      .ToDictionary(x => x.Key, x => x.ToList())
                                              };
            return this.View(vm);
        }

        public ActionResult Register(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            CreateRosterSelectList(season.Value);
            return this.View(new RegisterResult());
        }

        [HttpPost, Authorize, ActionName("Register")]
        public ActionResult RegisterConfirmed(int? season, RegisterResult vm)
        {
            if (ModelState.IsValid == false) return this.Register(season);

            var roster = this.DocumentSession.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult(
                roster,
                vm.TeamScore,
                vm.OpponentScore,
                vm.BitsMatchId);
            EventStoreSession.Store(matchResult);

            return this.RedirectToAction(
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

            this.CreateRosterSelectList(matchResult.Season, matchResult.RosterId);

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
                registerResult.TeamScore,
                registerResult.OpponentScore,
                registerResult.BitsMatchId);

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (this.EventStoreSession.Load<MatchResult>(id) == null)
                throw new HttpException(404, "MatchResult not found");
            return this.View();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var matchResult = this.EventStoreSession.Load<MatchResult>(id);
            if (matchResult == null)
                throw new HttpException(404, "MatchResult not found");
            matchResult.Delete();

            return RedirectToAction("Index");
        }

        public ActionResult RegisterSerie(string aggregateId, string rosterId, int bitsMatchId)
        {
            var roster = this.DocumentSession
                .Include<Roster>(r => r.Players)
                .Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            var registerSerie = new RegisterSerie(
                new ResultSeriesReadModel.Serie(),
                roster.Players.Select(
                    x => new SelectListItem
                         {
                             Text = this.DocumentSession.Load<Player>(x)
                                 .Name,
                             Value = x
                         })
                    .ToList());
            return this.View(registerSerie);
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
                new
                {
                    id = bitsMatchId
                });
        }

        public ActionResult Details(int id)
        {
            var headerId = ResultHeaderReadModel.IdFromBitsMatchId(id);
            var headerReadModel = DocumentSession.Load<ResultHeaderReadModel>(headerId);
            if (headerReadModel == null) throw new HttpException(404, "Match result not found");

            var matchId = ResultSeriesReadModel.IdFromBitsMatchId(id);
            var resultReadModel = DocumentSession.Load<ResultSeriesReadModel>(matchId)
                ?? new ResultSeriesReadModel();

            return this.View(new ResultViewModel(headerReadModel, resultReadModel));
        }

        private void CreateRosterSelectList(int season, string rosterId = "")
        {
            ViewBag.rosterid = this.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(x => x.Season == season)
                .Where(x => x.Preliminary == false)
                .Where(x => x.PlayerCount >= 8)
                .OrderBy(x => x.Date)
                .AsProjection<RosterSearchTerms.Result>()
                .ToList()
                .Where(x => x.MatchResultId == null || string.IsNullOrEmpty(rosterId) == false)
                .Select(
                    x => new SelectListItem
                    {
                        Text = string.Format("{0}: {1} - {2} ({3})", x.Turn, x.Team, x.Opponent, x.Location),
                        Value = x.Id,
                        Selected = x.Id == rosterId
                    })
                .ToList();
        }
    }
}