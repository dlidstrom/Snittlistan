namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using ViewModels;
    using Web.Controllers;

    public class ActivityDetailsController : AbstractController
    {
        public ActionResult Index(string id)
        {
            var activity = DocumentSession.Load<Activity>(id);
            if (activity==null) throw new HttpException(404, "Not found");
            return View(new ActivityViewModel(activity));
        }
    }
}