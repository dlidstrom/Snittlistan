
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Areas.V2.Controllers;
[Authorize(Roles = WebsiteRoles.Uk.UkTasks)]
public class AuditLogViewerController : AbstractController
{
    public ActionResult Index(string id)
    {
        object model = DocumentSession.Load<object>(id);
        if (model is IAuditLogCapable capable)
        {
            FormattedAuditLog history = capable.GetFormattedAuditLog(DocumentSession);
            return View(history);
        }

        return View(FormattedAuditLog.Empty);
    }
}
