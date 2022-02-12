#nullable enable

using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.HtmlHelpers;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class MatchResultController : AbstractController
{
    public ActionResult Index(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        ResultHeaderReadModel[] headerReadModels = CompositionRoot.DocumentSession.Query<ResultHeaderReadModel, ResultHeaderIndex>()
            .Where(x => x.Season == season)
            .ToArray();
        Dictionary<string?, Roster> rosters = CompositionRoot.DocumentSession.Load<Roster>(headerReadModels.Select(x => x.RosterId))
            .ToDictionary(x => x.Id);

        IEnumerable<ResultHeaderViewModel> headerViewModels = headerReadModels.Select(x => new ResultHeaderViewModel(x, rosters[x.RosterId]));
        MatchResultViewModel vm = new()
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
        ResultHeaderReadModel headerReadModel = CompositionRoot.DocumentSession.Load<ResultHeaderReadModel>(headerId);
        if (headerReadModel == null)
        {
            throw new HttpException(404, "Match result not found");
        }

        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(headerReadModel.RosterId);
        ResultHeaderViewModel headerViewModel = new(headerReadModel, roster);
        if (roster.IsFourPlayer)
        {
            string matchId = ResultSeries4ReadModel.IdFromBitsMatchId(id, rosterId);
            ResultSeries4ReadModel resultReadModel = CompositionRoot.DocumentSession.Load<ResultSeries4ReadModel>(matchId)
                ?? new ResultSeries4ReadModel();

            return View("Details4", new Result4ViewModel(headerViewModel, resultReadModel));
        }
        else
        {
            string matchId = ResultSeriesReadModel.IdFromBitsMatchId(id, rosterId);
            ResultSeriesReadModel resultReadModel = CompositionRoot.DocumentSession.Load<ResultSeriesReadModel>(matchId)
                ?? new ResultSeriesReadModel();

            return View(new ResultViewModel(headerViewModel, resultReadModel));
        }
    }

    public ActionResult Turns(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        TeamOfWeek[] weeks = CompositionRoot.DocumentSession.Query<TeamOfWeek, TeamOfWeekIndex>()
            .Where(x => x.Season == season.Value)
            .ToArray();
        Dictionary<string, Roster> rostersDictionary = CompositionRoot.DocumentSession.Load<Roster>(weeks.Select(x => x.RosterId)).ToDictionary(x => x.Id!);
        TeamOfWeekViewModel viewModel = new(season.Value, weeks, rostersDictionary);
        return View(viewModel);
    }

    public ActionResult Form(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        Player[] players = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .ToArray();

        ResultForPlayerIndex.Result[] results = CompositionRoot.DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
            .Where(x => x.Season == season.Value)
            .ToArray();
        Dictionary<string, ResultForPlayerIndex.Result> seasonAverages = results.ToDictionary(x => x.PlayerId);

        List<PlayerFormViewModel> response = new();
        foreach (Player player in players)
        {
            string name = player.Name;
            if (seasonAverages.TryGetValue(player.Id, out ResultForPlayerIndex.Result result)
                && result.TotalSeries > 0)
            {
                PlayerFormViewModel playerForm = new(name)
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

        FormViewModel viewModel = new(
            season.Value,
            response.OrderByDescending(x => x.SeasonAverage).ThenBy(x => x.Name).ToArray());
        return View(viewModel);
    }

    [RestoreModelStateFromTempData]
    public ActionResult EliteMedals(int? season)
    {
        if (season.HasValue == false)
        {
            season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        }

        Dictionary<string, Player> playersDict = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .Where(p => p.PlayerStatus == Player.Status.Active)
            .ToDictionary(x => x.Id);
        EliteMedals eliteMedals = CompositionRoot.DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
        if (eliteMedals == null)
        {
            eliteMedals = new EliteMedals();
            CompositionRoot.DocumentSession.Store(eliteMedals);
        }

        SeasonResults seasonResults = CompositionRoot.DocumentSession.Load<SeasonResults>(SeasonResults.GetId(season.Value));
        if (seasonResults == null)
        {
            seasonResults = new SeasonResults(season.Value);
            CompositionRoot.DocumentSession.Store(seasonResults);
        }

        EliteMedalsViewModel viewModel = new(season.Value, playersDict, eliteMedals, seasonResults);
        return View(viewModel);
    }

    [Authorize(Roles = WebsiteRoles.EliteMedals.EditMedals)]
    public ActionResult EditMedals(int id)
    {
        Player player = CompositionRoot.DocumentSession.Load<Player>(id);
        EliteMedals eliteMedals = CompositionRoot.DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
        EliteMedals.EliteMedal eliteMedal = eliteMedals.GetExistingMedal(player.Id);
        EditMedalsViewModel viewModel = new(
            player.Name,
            eliteMedal.Value,
            eliteMedal.CapturedSeason.GetValueOrDefault(),
            CompositionRoot.DocumentSession.LatestSeasonOrDefault(DateTime.Now.Year));
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

        EliteMedals eliteMedals = CompositionRoot.DocumentSession.Load<EliteMedals>(Domain.EliteMedals.TheId);
        Debug.Assert(postModel.EliteMedal != null, "postModel.EliteMedal != null");
        eliteMedals.AwardMedal("players-" + id, postModel.EliteMedal!.Value, postModel.CapturedSeason);
        return RedirectToAction("EliteMedals");
    }

    public ActionResult BitsMatchResult(string id)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(id);
        ViewBag.Url = CustomHtmlHelpers.GenerateBitsUrl(roster.BitsMatchId);
        return View();
    }
}
