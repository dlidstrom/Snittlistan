namespace Snittlistan.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using Infrastructure.Indexes;
    using Raven.Client;
    using Raven.Client.Linq;

    /// <summary>
    /// Do not cache results from validation.
    /// </summary>
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : AbstractController
    {
        public ValidationController(IDocumentSession session)
            : base(session)
        {
        }

        public JsonResult IsBitsMatchIdAvailable(int bitsMatchId)
        {
            var id = Session.Query<Match_ByBitsMatchId.Result, Match_ByBitsMatchId>()
                .AsProjection<Match_ByBitsMatchId.Result>()
                .SingleOrDefault(m => m.BitsMatchId == bitsMatchId);

            // true is valid
            return Json(id == null, JsonRequestBehavior.AllowGet);
        }
    }
}