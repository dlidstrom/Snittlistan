using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;
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

        public ActionResult Form(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            var players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray();

            var results = DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                .Where(x => x.Season == season.Value)
                .ToArray();
            var seasonAverages = results.ToDictionary(x => x.PlayerId);

            var response = new List<PlayerForm>();
            foreach (var player in players)
            {
                var name = player.Name;
                ResultForPlayerIndex.Result result;
                if (seasonAverages.TryGetValue(player.Id, out result)
                    && result.TotalSeries > 0)
                {
                    var playerForm = new PlayerForm
                    {
                        Name = name,
                        TotalSeries = result.TotalSeries,
                        SeasonAverage = (double)result.TotalPins / Math.Max(1, result.TotalSeries),
                        Last5Average = (double)result.Last5TotalPins / Math.Max(1, result.Last5TotalSeries),
                        HasResult = true
                    };
                    response.Add(playerForm);
                }
                else if (player.PlayerStatus == Player.Status.Active)
                {
                    response.Add(new PlayerForm
                    {
                        Name = name
                    });
                }
            }

            return View(response.OrderByDescending(x => x.SeasonAverage).ThenBy(x => x.Name));
        }

        public class PlayerForm
        {
            public string Name { get; set; }

            public double Last5Average { get; set; }

            public double SeasonAverage { get; set; }

            public bool HasResult { get; set; }

            public int TotalSeries { get; set; }

            public string FormattedSeasonAverage()
            {
                if (HasResult == false) return string.Empty;
                return SeasonAverage.ToString("0.0");
            }

            public string FormattedLast5Average()
            {
                if (HasResult == false) return string.Empty;
                return Last5Average.ToString("0.0");
            }

            public string FormattedDiff()
            {
                if (HasResult == false) return string.Empty;
                var diff = Last5Average - SeasonAverage;
                return diff.ToString("+0.0;-0.0;0");
            }

            public string Class()
            {
                if (HasResult == false) return string.Empty;
                var diff = Last5Average - SeasonAverage;
                string klass;
                if (diff <= -10)
                {
                    klass = "form-minus-10";
                }
                else if (diff <= -6)
                {
                    klass = "form-minus-6";
                }
                else if (diff <= -4)
                {
                    klass = "form-minus-4";
                }
                else if (diff <= -2)
                {
                    klass = "form-minus-2";
                }
                else if (diff >= 10)
                {
                    klass = "form-plus-10";
                }
                else if (diff >= 6)
                {
                    klass = "form-plus-6";
                }
                else if (diff >= 4)
                {
                    klass = "form-plus-4";
                }
                else if (diff >= 2)
                {
                    klass = "form-plus-2";
                }
                else
                {
                    klass = "form-0";
                }

                return klass;
            }
        }
    }
}