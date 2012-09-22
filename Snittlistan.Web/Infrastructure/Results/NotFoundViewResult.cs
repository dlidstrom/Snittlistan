namespace Snittlistan.Web.Infrastructure.Results
{
    using System.Web.Mvc;

    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult()
        {
            this.ViewName = "NotFound";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            var url = request.Url.OriginalString;
            this.ViewData["RequestedUrl"] = url;
            this.ViewData["ReferrerUrl"] = (request.UrlReferrer != null && request.UrlReferrer.OriginalString != url) ? request.UrlReferrer.OriginalString : null;
            response.StatusCode = 404;

            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;
            base.ExecuteResult(context);
        }
    }
}
