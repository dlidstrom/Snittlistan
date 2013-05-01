using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class SearchTermsController : AbstractController
    {
        public JsonResult Teams(string q)
        {
            var options = DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(t => t.Team.StartsWith(q))
                .Distinct()
                .AsProjection<RosterSearchTerms.Result>()
                .OrderBy(t => t.Team)
                .Select(t => t.Team)
                .ToList();

            return Json(new { options }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Opponents(string q)
        {
            var options = DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(t => t.Opponent.StartsWith(q))
                .Distinct()
                .AsProjection<RosterSearchTerms.Result>()
                .OrderBy(t => t.Opponent)
                .Select(t => t.Opponent)
                .ToList();

            return Json(new { options }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Locations(string q)
        {
            var options = DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(t => t.Location.StartsWith(q))
                .Distinct()
                .AsProjection<RosterSearchTerms.Result>()
                .OrderBy(t => t.Location)
                .Select(t => t.Location)
                .ToList();

            return Json(new { options }, JsonRequestBehavior.AllowGet);
        }
    }
}
