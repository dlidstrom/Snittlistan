namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Abstractions;
    using Raven.Client;
    using Raven.Client.Linq;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;

    public class MatchResultController : AbstractController
    {
        public MatchResultController(IDocumentSession session)
            : base(session)
        {
        }

        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = this.Session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var results = Session.Query<MatchResultIndex.Result, MatchResultIndex>()
                .Where(r => r.Season == season)
                .AsProjection<MatchResultIndex.Result>()
                .ToList()
                .GroupBy(x => x.Turn)
                .Select(x => new ResultViewModel
                                 {
                                     Turn = x.Key,
                                     Results = x.Select(y => new TurnResultViewModel(y)).ToList()
                                 })
                .ToList();

            var vm = new MatchResultViewModel
            {
                SeasonStart = season.Value,
                Results = results
            };
            return this.View(vm);
        }

        public ActionResult Register(int? season)
        {
            if (season.HasValue == false)
                season = Session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var rosters = this.Session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(x => x.Season == season)
                .OrderBy(x => x.Date)
                .AsProjection<RosterSearchTerms.Result>()
                .ToList()
                .Where(x => x.MatchResultId == null)
                .Select(
                    x => new SelectListItem
                    {
                        Text = string.Format("{0}: {1} - {2} ({3})", x.Turn, x.Team, x.Opponent, x.Location),
                        Value = x.Id
                    })
                .ToList();
            return this.View(rosters);
        }

        [HttpPost, Authorize, ActionName("Register")]
        public ActionResult RegisterConfirmed(int? season, RegisterMatchResult vm)
        {
            if (ModelState.IsValid == false) return this.Register(season);

            var roster = this.Session.Load<Roster>(vm.RosterId);
            if (roster == null)
                throw new HttpException(404, "Roster not found");

            var matchResult = new MatchResult(
                vm.RosterId,
                vm.TeamScore,
                vm.OpponentScore,
                vm.BitsMatchId);
            Session.Store(matchResult);
            roster.MatchResultId = Session.Advanced.GetDocumentId(matchResult);
            return this.RedirectToAction("Index");
        }
    }
}