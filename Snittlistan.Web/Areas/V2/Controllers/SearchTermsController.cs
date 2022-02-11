#nullable enable

using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class SearchTermsController : AbstractController
{
    public JsonResult Teams(string q)
    {
        List<string> options = CompositionRoot.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
            .Where(t => t.Team.StartsWith(q))
            .Distinct()
            .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
            .OrderBy(t => t.Team)
            .Select(t => t.Team)
            .ToList();

        return Json(new { options }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult Opponents(string q)
    {
        List<string> options = CompositionRoot.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
            .Where(t => t.Opponent.StartsWith(q))
            .Distinct()
            .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
            .OrderBy(t => t.Opponent)
            .Select(t => t.Opponent)
            .ToList();

        return Json(new { options }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult Locations(string q)
    {
        List<string> options = CompositionRoot.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
            .Where(t => t.Location.StartsWith(q))
            .Distinct()
            .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
            .OrderBy(t => t.Location)
            .Select(t => t.Location)
            .ToList();

        return Json(new { options }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult OilPattern(string q)
    {
        List<string> options = CompositionRoot.DocumentSession.Query<RosterSearchTerms.Result, RosterSearchTerms>()
            .Where(t => t.OilPatternName.StartsWith(q))
            .Distinct()
            .ProjectFromIndexFieldsInto<RosterSearchTerms.Result>()
            .OrderBy(t => t.OilPatternName)
            .Select(t => t.OilPatternName)
            .ToList();

        return Json(new { options }, JsonRequestBehavior.AllowGet);
    }
}
