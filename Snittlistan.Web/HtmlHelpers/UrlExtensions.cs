namespace Snittlistan.Web.HtmlHelpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public static class UrlExtensions
    {
        private static readonly MD5 HashAlgorithm = MD5.Create();

        public static string ContentCacheBreak(this UrlHelper url, string contentPath)
        {
            if (contentPath == null) throw new ArgumentNullException(nameof(contentPath));
            var path = HostingEnvironment.MapPath(contentPath);
            if (File.Exists(path) == false) throw new Exception($"{path} does not exist");
            var hashPart = string.Empty;
            if (path != null)
            {
                var bytes = File.ReadAllBytes(path);
                var hash = HashAlgorithm.ComputeHash(bytes);
                var hashBuilder = new StringBuilder();
                foreach (var b in hash)
                {
                    hashBuilder.Append($"{b:x2}");
                }

                hashPart = $"?{hashBuilder}";
            }

            return $"{url.Content(contentPath)}{hashPart}";
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