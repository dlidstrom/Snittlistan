namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Web.Http.Filters;
    using Snittlistan.Web.Controllers;

    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ControllerContext.Controller is not AbstractApiController controller
                || actionExecutedContext.Exception != null)
            {
                return;
            }

            controller.SaveChanges();
        }
    }
}
