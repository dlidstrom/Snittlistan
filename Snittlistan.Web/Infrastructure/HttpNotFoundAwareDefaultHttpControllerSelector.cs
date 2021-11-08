namespace Snittlistan.Web.Infrastructure
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    public class HttpNotFoundAwareDefaultHttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpNotFoundAwareDefaultHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor decriptor;
            try
            {
                decriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                HttpStatusCode code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound)
                {
                    throw;
                }

                System.Collections.Generic.IDictionary<string, object> routeValues = request.GetRouteData().Values;
                routeValues["controller"] = "ApiError";
                routeValues["action"] = "Handle404";
                decriptor = base.SelectController(request);
            }

            return decriptor;
        }
    }
}