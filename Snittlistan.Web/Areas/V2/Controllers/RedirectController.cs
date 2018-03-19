using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V2.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Redirect()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult RedirectNewView(int? season, int? turn)
        {
            return RedirectToActionPermanent("View", "Roster", new
            {
                season,
                turn
            });
        }
    }
}