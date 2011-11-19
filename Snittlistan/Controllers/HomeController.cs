namespace Snittlistan.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Raven.Client;
    using Raven.Client.Linq;
    using Snittlistan.Infrastructure.Indexes;
    using Snittlistan.Models;
    using Snittlistan.ViewModels;

    /// <summary>
    /// Manages the start page.
    /// </summary>
    public class HomeController : AbstractController
    {
        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        /// <param name="session">Document session.</param>
        public HomeController(IDocumentSession session)
            : base(session)
        { }

        /// <summary>
        /// GET: /Home/Index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .OrderByDescending(s => s.Average)
                .ToList();

            var last20 = Session.Query<Pins_Last20.Result, Pins_Last20>()
                .ToDictionary(s => s.Player, s => s.Pins);

            var vm = stats.Select(s => new PlayerStatsViewModel
            {
                Player = s.Player,
                Series = s.Series,
                AverageScore = s.Score / s.Series,
                AveragePins = s.Pins / s.Series,
                Max = s.Max,
                AverageStrikes = s.Strikes / s.Series,
                AverageMisses = s.Misses / s.Series,
                AverageOnePinMisses = s.OnePinMisses / s.Series,
                AverageSplits = s.Splits / s.Series,
                CoveredAll = s.CoveredAll,
                AverageLast20 = last20[s.Player]
            }).ToList();

            return View(vm);
        }

        /// <summary>
        /// GET: /Home/Player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public ActionResult Player(string player)
        {
            if (string.IsNullOrWhiteSpace(player))
                return HttpNotFound();

            var q = Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Where(r => r.Player == player)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.BitsMatchId);

            return View(new PlayerMatchesViewModel { Player = player, Stats = q.ToList() });
        }

        /// <summary>
        /// GET: /Home/About.
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
    }
}