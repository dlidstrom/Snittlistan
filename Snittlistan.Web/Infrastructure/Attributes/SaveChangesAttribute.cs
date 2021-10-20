namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using Snittlistan.Web.Controllers;

    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.ActionContext.ControllerContext.Controller is not AbstractApiController controller
                || actionExecutedContext.Exception != null)
            {
                return;
            }

            await controller.SaveChangesAsync();
        }
    }
}
