namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Web.Mvc;

    public class RedirectController : Controller
    {
        public ActionResult Redirect()
        {
            return RedirectToAction("Index", "Roster");
        }

        public ActionResult RedirectNewView(int? season, int? turn)
        {
            return RedirectToAction("View", "Roster", new
            {
                season,
                turn
            });
        }
    }
}
