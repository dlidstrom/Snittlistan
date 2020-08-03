namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Raven.Client.Linq;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.Indexes;

    public class SearchController : AbstractController
    {
        public JsonResult PlayersQuickSearch(string term)
        {
            var results = DocumentSession.Query<PlayersIndex.Result, PlayersIndex>()
                                         .Where(p => p.Player.StartsWith(term))
                                         .OrderBy(p => p.Player)
                                         .ToList()
                                         .Select(p => new { label = p.Player });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TeamsQuickSearch(string term)
        {
            var results = DocumentSession.Query<Teams.Result, Teams>()
                                         .Where(t => t.Team.StartsWith(term))
                                         .OrderBy(t => t.Team)
                                         .ToList()
                                         .Select(t => new { label = t.Team });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LocationsQuickSearch(string term)
        {
            var results = DocumentSession.Query<Locations.Result, Locations>()
                                         .Where(t => t.Location.StartsWith(term))
                                         .OrderBy(t => t.Location)
                                         .ToList()
                                         .Select(t => new { label = t.Location });

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}