using System.Linq;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V1.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Web.Areas.V1.Controllers
{
    /// <summary>
    /// Manages the start page.
    /// </summary>
    public class HomeController : AbstractController
    {
        /// <summary>
        /// GET: /Home/Index.
        /// </summary>
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            var stats = DocumentSession.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .ToList()
                .OrderByDescending(s => s.AveragePins)
                .ToList();

            return View(stats);
        }

        /// <summary>
        /// GET: /Home/Player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public ViewResult Player(string player)
        {
            if (string.IsNullOrWhiteSpace(player))
                throw new HttpException(404, "Player not found");

            var stats = DocumentSession.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(r => r.Player == player)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.BitsMatchId)
                .ToList();

            var results = DocumentSession.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .SingleOrDefault(r => r.Player == player);

            return View(new PlayerMatchesViewModel
            {
                Player = player,
                Stats = stats,
                Results = results ?? new Matches_PlayerStats.Result()
            });
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