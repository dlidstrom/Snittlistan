using System;
using System.Linq;
using System.Net;
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

            return this.RedirectToAction("RegisterSerie", new { vm.AggregateId, vm.RosterId, vm.BitsMatchId });
        }

        public ActionResult RegisterSerie(string aggregateId, string rosterId, int bitsMatchId)
        {
            var roster = this.DocumentSession.Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            var registerSerie = new RegisterSerie(
                aggregateId, rosterId, bitsMatchId, roster.Players, new ResultReadModel.Serie());
            return this.View(registerSerie);
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
        public ActionResult EditResult(int id, RegisterResult registerMatchResult)
        {
            if (registerMatchResult == null) throw new ArgumentNullException("registerMatchResult");
            if (!ModelState.IsValid) return EditResult(id);

            var aggregate = EventStoreSession.Load<MatchResult>(registerMatchResult.AggregateId);
            if (aggregate == null) throw new HttpException(404, "Match result not found");

            aggregate.Update(
                DocumentSession.Load<Roster>(registerMatchResult.RosterId),
                registerMatchResult.TeamScore,
                registerMatchResult.OpponentScore,
                registerMatchResult.BitsMatchId);

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

        public ActionResult Details(int id)
        {
            var headerId = ResultHeaderReadModel.IdFromBitsMatchId(id);
            var headerReadModel = DocumentSession.Load<ResultHeaderReadModel>(headerId);
            if (headerReadModel == null) throw new HttpException(404, "Match result not found");

            var gamesReadModel = DocumentSession.Load<ResultReadModel>(ResultReadModel.IdFromBitsMatchId(id))
                ?? new ResultReadModel();

            return this.View(new ResultViewModel(headerReadModel, gamesReadModel));
        }

        private void CreateRosterSelectList(int season, string rosterId = "")
        {
            ViewBag.rosterid = this.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(x => x.Season == season)
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