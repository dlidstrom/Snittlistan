namespace Snittlistan.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Results;

    [Authorize]
    public class ElmahController : Controller
    {
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }
    }
}