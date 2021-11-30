
using System.Net;
using System.Web.Mvc;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Attributes;
public class ValidateModelAttributeMvc : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.Controller.ViewData.ModelState.IsValid)
        {
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
