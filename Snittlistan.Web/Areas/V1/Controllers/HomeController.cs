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
                .OrderByDescending(s => s.AveragePins);

            var last20 = this.Session.Query<Pins_Last20.Result, Pins_Last20>()
                .ToDictionary(s => s.Player, s => s.Pins);

            var vm = stats.Select(s => new PlayerStatsViewModel(s, last20)).ToList();

            return this.View(vm);
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

            var q = this.Session.Query<Player_ByMatch.Result, Player_ByMatch>()
                .Where(r => r.Player == player)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.BitsMatchId);

            var results = this.Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .SingleOrDefault(r => r.Player == player);

            var last20 = this.Session.Query<Pins_Last20.Result, Pins_Last20>()
                .SingleOrDefault(r => r.Player == player);

            return this.View(new PlayerMatchesViewModel
            {
                Player = player,
                Stats = q.ToList(),
                Results = results ?? new Matches_PlayerStats.Result(),
                Last20 = last20 ?? new Pins_Last20.Result()
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