namespace Snittlistan.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using Raven.Client;
    using Raven.Client.Linq;

    using Snittlistan.Web.Infrastructure.Indexes;

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
            var id = this.Session.Query<Match_ByBitsMatchId.Result, Match_ByBitsMatchId>()
                .AsProjection<Match_ByBitsMatchId.Result>()
                .SingleOrDefault(m => m.BitsMatchId == bitsMatchId);

            // true is valid
            return this.Json(id == null, JsonRequestBehavior.AllowGet);
        }
    }
}