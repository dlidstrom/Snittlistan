#nullable enable

namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Net;
    using System.Web.Mvc;

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
}
