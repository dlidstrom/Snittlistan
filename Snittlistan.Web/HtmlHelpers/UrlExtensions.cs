namespace Snittlistan.Web.HtmlHelpers
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

    public static class UrlExtensions
    {
        public static string ContentCacheBreak(this UrlHelper url, string contentPath)
        {
            if (contentPath == null) throw new ArgumentNullException(nameof(contentPath));
            var path = url.Content(contentPath);
            path += "?" + MvcApplication.GetAssemblyVersion();
            return path;
        }

        public static string GetWebcalUrl(this UrlHelper helper)
        {
            var uri = helper.RequestContext.HttpContext.Request.Url;
            Debug.Assert(uri != null, "uri != null");
            var url = $"webcal://{uri.Host}/api/calendar";
            return url;
        }
    }
}