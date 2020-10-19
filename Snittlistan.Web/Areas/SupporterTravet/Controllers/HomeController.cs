namespace Snittlistan.Web.Areas.SupporterTravet.Controllers
{
    using System.Web.Mvc;
    using Web.Controllers;

    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
