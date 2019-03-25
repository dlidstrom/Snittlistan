namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Web.Mvc;
    using Snittlistan.Web.Controllers;

    public class MatchController : AbstractController
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult Details8x4()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult Details4x4()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult Create()
        {
            return RedirectToActionPermanent("Index");
        }

        public ActionResult Edit()
        {
            return RedirectToActionPermanent("Index");
        }

        public ActionResult LegacyRedirect()
        {
            return RedirectToActionPermanent("Index");
        }
    }
}