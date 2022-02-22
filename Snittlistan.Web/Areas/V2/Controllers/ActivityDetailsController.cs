#nullable enable

using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class ActivityDetailsController : AbstractController
{
    public async Task<ActionResult> Index(string id)
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

        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        ActivityViewModel activityViewModel =
            new(
                activity.Id!,
                activity.Title,
                activity.Date,
                activity.MessageHtml,
                player?.Name ?? string.Empty,
                tenant.AppleTouchIcon,
                tenant.TeamFullName);
        return View(activityViewModel);
    }
}
