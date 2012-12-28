namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Areas.V1.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.Indexes;

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
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            var stats = this.Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .ToList()
                .OrderByDescending(s => s.AveragePins)
                .ToList();

            return this.View(stats);
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

            var stats = this.Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Where(r => r.Player == player)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.BitsMatchId)
                .ToList();

            var results = this.Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .SingleOrDefault(r => r.Player == player);

            return this.View(new PlayerMatchesViewModel
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
            return this.View();
        }
    }
}