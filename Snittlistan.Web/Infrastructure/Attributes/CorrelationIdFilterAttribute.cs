using System.Diagnostics;
using System.Web.Mvc;

namespace Snittlistan.Web;
public class CorrelationIdFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (Trace.CorrelationManager.ActivityId == default)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }

        filterContext.HttpContext.Items["CorrelationId"]
            = Trace.CorrelationManager.ActivityId;
    }
}
