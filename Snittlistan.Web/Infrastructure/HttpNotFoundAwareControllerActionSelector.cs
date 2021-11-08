namespace Snittlistan.Web.Infrastructure
{
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Snittlistan.Web.Areas.V2.Controllers.Api;

    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                HttpStatusCode code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound && code != HttpStatusCode.MethodNotAllowed)
                {
                    throw;
                }

                System.Web.Http.Routing.IHttpRouteData routeData = controllerContext.RouteData;
                routeData.Values["action"] = "Handle404";
                IHttpController httpController = new ApiErrorController();
                controllerContext.Controller = httpController;
                controllerContext.ControllerDescriptor = new HttpControllerDescriptor(controllerContext.Configuration, "Error", httpController.GetType());
                decriptor = base.SelectAction(controllerContext);
            }

            return decriptor;
        }
    }
}