#nullable enable

using System.Web.Mvc;
using Raven.Client.Linq;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V1.Controllers;

public class SearchController : AbstractController
{
    public JsonResult TeamsQuickSearch(string term)
    {
        if (term.Length < 3)
        {
            return Json(Array.Empty<string>(), JsonRequestBehavior.AllowGet);
        }

        var query =
            from team in CompositionRoot.Databases.Bits.Teams
            where team.TeamAlias.StartsWith(term)
            orderby team.TeamAlias
            select new
            {
                label = team.TeamAlias
            };

        return Json(query, JsonRequestBehavior.AllowGet);
    }

    public JsonResult LocationsQuickSearch(string term)
    {
        if (term.Length < 3)
        {
            return Json(Array.Empty<string>(), JsonRequestBehavior.AllowGet);
        }

        var query =
            from hall in CompositionRoot.Databases.Bits.Hallar
            where hall.HallName.StartsWith(term)
            orderby hall.HallName
            select new
            {
                label = hall.HallName
            };

        return Json(term, JsonRequestBehavior.AllowGet);
    }
}
