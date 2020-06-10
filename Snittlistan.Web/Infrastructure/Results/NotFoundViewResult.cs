namespace Snittlistan.Web.Infrastructure.Results
{
    using System.Web.Mvc;

    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult()
        {
            ViewName = "NotFound";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            System.Web.HttpResponseBase response = context.HttpContext.Response;
            System.Web.HttpRequestBase request = context.HttpContext.Request;
            string url = request.Url.OriginalString;
            ViewData["RequestedUrl"] = url;
            ViewData["ReferrerUrl"] = (request.UrlReferrer != null && request.UrlReferrer.OriginalString != url) ? request.UrlReferrer.OriginalString : null;
            response.StatusCode = 404;

            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;
            base.ExecuteResult(context);
        }
    }
}