using System.Web.Http.Filters;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Infrastructure.Attributes
{
    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(actionExecutedContext.ActionContext.ControllerContext.Controller is AbstractApiController controller) || actionExecutedContext.Exception != null)
                return;

            controller.SaveChanges();
        }
    }
}