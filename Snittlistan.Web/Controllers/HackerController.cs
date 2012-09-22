namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    public class HackerController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}