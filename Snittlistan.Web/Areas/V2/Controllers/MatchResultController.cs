﻿namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using Helpers;
    using HtmlHelpers;
    using Indexes;
    using Infrastructure.Attributes;
    using Raven.Abstractions;
    using ReadModels;
    using ViewModels;
    using Web.Controllers;

    public class MatchResultController : AbstractController
    {
        public ActionResult Index(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            ResultHeaderReadModel[] headerReadModels = DocumentSession.Query<ResultHeaderReadModel, ResultHeaderIndex>()
                                                  .Where(x => x.Season == season)
                                                  .ToArray();
            var rosters = DocumentSession.Load<Roster>(headerReadModels.Select(x => x.RosterId))
                                         .ToDictionary(x => x.Id);

            IEnumerable<ResultHeaderViewModel> headerViewModels = headerReadModels.Select(x => new ResultHeaderViewModel(x, rosters[x.RosterId]));
            var vm = new MatchResultViewModel
            {
                SeasonStart = season.Value,
                Turns = headerViewModels.GroupBy(x => x.Turn)
                                        .ToDictionary(x => x.Key, x => x.ToList())
            };
            return View(vm);
        }

        public ActionResult Details(int id, string rosterId)
        {
            string headerId = ResultHeaderReadModel.IdFromBitsMatchId(id, rosterId);
            ResultHeaderReadModel headerReadModel = DocumentSession.Load<ResultHeaderReadModel>(headerId);
            if (headerReadModel == null) throw new HttpException(404, "Match result not found");

            Roster roster = DocumentSession.Load<Roster>(headerReadModel.RosterId);
            var headerViewModel = new ResultHeaderViewModel(headerReadModel, roster);
            if (roster.IsFourPlayer)
            {
                string matchId = ResultSeries4ReadModel.IdFromBitsMatchId(id, rosterId);
                ResultSeries4ReadModel resultReadModel = DocumentSession.Load<ResultSeries4ReadModel>(matchId)
                    ?? new ResultSeries4ReadModel();

                return View("Details4", new Result4ViewModel(headerViewModel, resultReadModel));
            }
            else
            {
                string matchId = ResultSeriesReadModel.IdFromBitsMatchId(id, rosterId);
                ResultSeriesReadModel resultReadModel = DocumentSession.Load<ResultSeriesReadModel>(matchId)
                    ?? new ResultSeriesReadModel();

                return View(new ResultViewModel(headerViewModel, resultReadModel));
            }
        }

        public ActionResult Turns(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            TeamOfWeek[] weeks = DocumentSession.Query<TeamOfWeek, TeamOfWeekIndex>()
                                       .Where(x => x.Season == season.Value)
                                       .ToArray();
            var rostersDictionary = DocumentSession.Load<Roster>(weeks.Select(x => x.RosterId)).ToDictionary(x => x.Id);
            var viewModel = new TeamOfWeekViewModel(season.Value, weeks, rostersDictionary);
            return View(viewModel);
        }

        public ActionResult Form(int? season)
        {
            if (season.HasValue == false)
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);

            Player[] players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray();

            ResultForPlayerIndex.Result[] results = DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                .Where(x => x.Season == season.Value)
                .ToArray();
            var seasonAverages = results.ToDictionary(x => x.PlayerId);

            var response = new List<PlayerFormViewModel>();
            foreach (Player player in players)
            {
                string name = player.Name;
                if (seasonAverages.TryGetValue(player.Id, out ResultForPlayerIndex.Result result)
                    && result.TotalSeries > 0)
                {
                    var playerForm = new PlayerFormViewModel(name)
                    {
                        TotalSeries = result.TotalSeries,
                        TotalScore = result.TotalScore,
                        ScoreAverage = (double)result.TotalScore / Math.Max(1, result.TotalSeries),
                        SeasonAverage = (double)result.TotalPins / Math.Max(1, result.TotalSeries),
                        Last5Average = (double)result.Last5TotalPins / Math.Max(1, result.Last5TotalSeries),
                        HasResult = true
                    };
                    response.Add(playerForm);
                }
                else if (player.PlayerStatus == Player.Status.Active)
                {
                    response.Add(new PlayerFormViewModel(name));
                }
            }

            var viewModel = new FormViewModel(
                season.Value,
                response.OrderByDescending(x => x.SeasonAverage).ThenBy(x => x.Name).ToArray());
            return View(viewModel);
        }

        [RestoreModelStateFromTempData]
        public ActionResult EliteMedals(int? season)
        {
            if (season.HasValue == false)
            {
                season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            }

            var playersDict = DocumentSession.Query<Player, PlayerSearch>()
                .Where(p => p.PlayerStatus == Player.Status.Active)
                .ToDictionary(x => x.Id);
            EliteMedals eliteMedals = DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
            if (eliteMedals == null)
            {
                eliteMedals = new EliteMedals();
                DocumentSession.Store(eliteMedals);
            }

            SeasonResults seasonResults = DocumentSession.Load<SeasonResults>(SeasonResults.GetId(season.Value));
            if (seasonResults == null)
            {
                seasonResults = new SeasonResults(season.Value);
                DocumentSession.Store(seasonResults);
            }

            var viewModel = new EliteMedalsViewModel(season.Value, playersDict, eliteMedals, seasonResults);
            return View(viewModel);
        }

        [Authorize(Roles = WebsiteRoles.EliteMedals.EditMedals)]
        public ActionResult EditMedals(int id)
        {
            Player player = DocumentSession.Load<Player>(id);
            EliteMedals eliteMedals = DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
            EliteMedals.EliteMedal eliteMedal = eliteMedals.GetExistingMedal(player.Id);
            var viewModel = new EditMedalsViewModel(
                player.Name,
                eliteMedal.Value,
                eliteMedal.CapturedSeason.GetValueOrDefault(),
                DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year));
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = WebsiteRoles.EliteMedals.EditMedals)]
        [ActionName("EditMedals")]
        public ActionResult EditMedalsPost(int id, EditMedalsPostModel postModel)
        {
            if (ModelState.IsValid == false)
            {
                return EditMedals(id);
            }

            EliteMedals eliteMedals = DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
            Debug.Assert(postModel.EliteMedal != null, "postModel.EliteMedal != null");
            eliteMedals.AwardMedal("players-" + id, postModel.EliteMedal.Value, postModel.CapturedSeason);
            return RedirectToAction("EliteMedals");
        }

        public ActionResult BitsMatchResult(string id)
        {
            Roster roster = DocumentSession.Load<Roster>(id);
            ViewBag.Url = CustomHtmlHelpers.GenerateBitsUrl(roster.BitsMatchId);
            return View();
        }
    }
}