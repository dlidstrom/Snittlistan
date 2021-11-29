#nullable enable

namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class OnlyLocalAllowedAttribute : ActionFilterAttribute
    {
        public static bool SkipValidation;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipValidation == false && actionContext.Request.IsLocal() == false)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}
