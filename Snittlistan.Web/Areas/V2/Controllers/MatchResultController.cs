using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
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
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var headerReadModels = DocumentSession.Query<ResultHeaderReadModel, ResultHeaderIndex>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(x => x.Season == season)
                .ToList();
            var vm = new MatchResultViewModel
            {
                SeasonStart = season.Value,
                Turns = headerReadModels
                    .GroupBy(x => x.Turn)
                    .ToDictionary(x => x.Key, x => x.ToList())
            };
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var headerId = ResultHeaderReadModel.IdFromBitsMatchId(id);
            var headerReadModel = DocumentSession.Load<ResultHeaderReadModel>(headerId);
            if (headerReadModel == null) throw new HttpException(404, "Match result not found");

            if (headerReadModel.IsFourPlayer)
            {
                var matchId = ResultSeries4ReadModel.IdFromBitsMatchId(id);
                var resultReadModel = DocumentSession.Load<ResultSeries4ReadModel>(matchId)
                    ?? new ResultSeries4ReadModel();

                return View("Details4", new Result4ViewModel(headerReadModel, resultReadModel));
            }
            else
            {
                var matchId = ResultSeriesReadModel.IdFromBitsMatchId(id);
                var resultReadModel = DocumentSession.Load<ResultSeriesReadModel>(matchId)
                    ?? new ResultSeriesReadModel();

                return View(new ResultViewModel(headerReadModel, resultReadModel));
            }
        }

        public ActionResult Turns(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var vm = DocumentSession.Query<TeamOfWeek>()
                .Where(x => x.Season == season.Value)
                .OrderByDescending(x => x.Turn)
                .ToList();
            return View(new TeamOfWeekViewModel(season.Value, vm));
        }
    }
}