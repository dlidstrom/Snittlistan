namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Infrastructure.Indexes;

    public class SearchController : AbstractController
    {
        public SearchController(IDocumentSession session)
            : base(session)
        {
        }

        public JsonResult Teams(string q)
        {
            var options = this.Session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(t => t.Team.StartsWith(q))
                .OrderBy(t => t.Team)
                .AsProjection<RosterSearchTerms.Result>()
                .ToList()
                .Select(t => t.Team);

            return this.Json(new { options }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Opponents(string q)
        {
            var options = this.Session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(t => t.Opponent.StartsWith(q))
                .OrderBy(t => t.Opponent)
                .AsProjection<RosterSearchTerms.Result>()
                .ToList()
                .Select(t => t.Opponent);

            return this.Json(new { options }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Locations(string q)
        {
            var options = this.Session.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Where(t => t.Location.StartsWith(q))
                .OrderBy(t => t.Location)
                .AsProjection<RosterSearchTerms.Result>()
                .ToList()
                .Select(t => t.Location);

            return this.Json(new { options }, JsonRequestBehavior.AllowGet);
        }
    }
}
