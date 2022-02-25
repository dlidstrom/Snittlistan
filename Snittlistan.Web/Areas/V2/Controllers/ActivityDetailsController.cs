#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using System.Web;
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class ActivityDetailsController : AbstractController
{
    public ActionResult Index(string id)
    {
        Activity activity = CompositionRoot.DocumentSession.Load<Activity>(id);
        if (activity == null)
        {
            throw new HttpException(404, "Not found");
        }

        ViewData["showComments"] = true;
        Player? player = null;
        if (activity.AuthorId != null)
        {
            player = CompositionRoot.DocumentSession.Load<Player>(activity.AuthorId);
        }

        ActivityViewModel activityViewModel =
            new(
                activity.Id!,
                activity.Title,
                activity.Date,
                activity.MessageHtml,
                player?.Name ?? string.Empty,
                CompositionRoot.CurrentTenant.AppleTouchIcon,
                CompositionRoot.CurrentTenant.TeamFullName);
        return View(activityViewModel);
    }
}
