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
            Activity activity = DocumentSession.Load<Activity>(id);
            if (activity == null) throw new HttpException(404, "Not found");
            ViewData["showComments"] = true;
            Player player = null;
            if (activity.AuthorId != null)
            {
                player = DocumentSession.Load<Player>(activity.AuthorId);
            }

            var activityViewModel =
                new ActivityViewModel(
                    activity.Id,
                    activity.Title,
                    activity.Date,
                    activity.MessageHtml,
                    player?.Name ?? string.Empty);
            return View(activityViewModel);
        }
    }
}