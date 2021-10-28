namespace Snittlistan.Web
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

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
}
