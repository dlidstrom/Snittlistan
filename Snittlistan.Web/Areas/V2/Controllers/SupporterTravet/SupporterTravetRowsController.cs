namespace Snittlistan.Web.Areas.V2.Controllers.SupporterTravet
{
    using System.Web.Mvc;
    using Snittlistan.Web.Controllers;

    public class SupporterTravetRowsController : AbstractController
    {
        public ActionResult Index()
        {
            return View(new SupporterTravetModels.Header("sp-123"));
        }
    }
}
