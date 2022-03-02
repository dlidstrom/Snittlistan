#nullable enable

using System.Web.Mvc;

namespace Snittlistan.Web.Infrastructure.Attributes;

public class SetTempModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        filterContext.Controller.TempData["ModelState"]
            = filterContext.Controller.ViewData.ModelState;
    }
}
