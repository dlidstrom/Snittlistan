namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Web.Mvc;
    using Snittlistan.Web.Controllers;

    public class MatchController : AbstractController
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }

        public ActionResult Details8x4()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }

        public ActionResult Details4x4()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }

        public ActionResult Create()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }

        public ActionResult Edit()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }

        public ActionResult LegacyRedirect()
        {
            return RedirectToActionPermanent("Index", "Roster", new { area = "V2" });
        }
    }
}