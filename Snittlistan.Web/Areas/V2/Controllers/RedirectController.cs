namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Web.Mvc;

    public class RedirectController : Controller
    {
        public ActionResult Redirect()
        {
            return this.RedirectToActionPermanent("Index", "App");
        }
    }
}