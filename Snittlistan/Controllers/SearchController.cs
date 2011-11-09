namespace Snittlistan.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Raven.Client;
    using Snittlistan.Infrastructure.Indexes;

    public class SearchController : AbstractController
    {
        public SearchController(IDocumentSession session)
            : base(session)
        {
        }

        public JsonResult PlayersQuickSearch(string term)
        {
            var results = Session.Query<Players.Result, Players>()
                .Where(p => p.Player.StartsWith(term))
                .ToList()
                .Select(p => new { label = p.Player });
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}