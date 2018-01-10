using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Raven.Client;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Web.Areas.V1.Controllers
{
    /// <summary>
    /// Do not cache results from validation.
    /// </summary>
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : AbstractController
    {
        public JsonResult IsBitsMatchIdAvailable(int bitsMatchId)
        {
            var id = DocumentSession.Query<Match_ByBitsMatchId.Result, Match_ByBitsMatchId>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .ProjectFromIndexFieldsInto<Match_ByBitsMatchId.Result>()
                .SingleOrDefault(m => m.BitsMatchId == bitsMatchId);

            // true is valid
            return Json(id == null, JsonRequestBehavior.AllowGet);
        }
    }
}