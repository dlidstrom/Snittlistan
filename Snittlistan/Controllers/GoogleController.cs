namespace Snittlistan.Controllers
{
    using System.Web.Mvc;

    public class GoogleController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}
