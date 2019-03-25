using System.Web.Mvc;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V1.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult Player(string player)
        {
            return RedirectToActionPermanent("Index", "Roster");
        }

        public ActionResult About()
        {
            return RedirectToActionPermanent("Index", "Roster");
        }
    }
}