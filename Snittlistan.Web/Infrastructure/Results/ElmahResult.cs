#nullable enable

namespace Snittlistan.Web.Infrastructure.Results
{
    using System.Web;
    using System.Web.Mvc;
    using Elmah;

    public class ElmahResult : ActionResult
    {
        private readonly string resourceType;

        public ElmahResult(string resourceType)
        {
            this.resourceType = resourceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            UrlHelper url = new(HttpContext.Current.Request.RequestContext);
            ErrorLogPageFactory factory = new();
            if (!string.IsNullOrEmpty(resourceType))
            {
                string pathInfo = "." + resourceType;
                string action = url.Action("Index", "Elmah", new { type = (string?)null });
                HttpContext.Current.RewritePath(action, pathInfo, HttpContext.Current.Request.QueryString.ToString());
            }

            IHttpHandler handler = factory.GetHandler(HttpContext.Current, null, null, null);

            handler.ProcessRequest(HttpContext.Current);
        }
    }
}
