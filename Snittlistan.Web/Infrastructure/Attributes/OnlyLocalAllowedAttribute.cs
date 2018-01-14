using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Snittlistan.Web.Infrastructure.Attributes
{
    public class OnlyLocalAllowedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.IsLocal() == false)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}