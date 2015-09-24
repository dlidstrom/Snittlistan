using System;
using System.Web.Mvc;

namespace Snittlistan.Web.HtmlHelpers
{
    public static class UrlExtensions
    {
        public static string ContentCacheBreak(this UrlHelper url, string contentPath)
        {
            if (contentPath == null) throw new ArgumentNullException("contentPath");
            var path = url.Content(contentPath);
            path += "?" + MvcApplication.GetAssemblyVersion();
            return path;
        }
    }
}