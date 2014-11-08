using System.Web.Http.Filters;
using Snittlistan.Web.Controllers;

namespace Snittlistan.Web.Infrastructure.Attributes
{
    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as AbstractApiController;
            if (controller == null || actionExecutedContext.Exception != null)
                return;

            controller.SaveChanges();
        }
    }
}