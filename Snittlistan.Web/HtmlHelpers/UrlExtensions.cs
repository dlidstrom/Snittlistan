namespace Snittlistan.Web.HtmlHelpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public static class UrlExtensions
    {
        public static string ContentCacheBreak(this UrlHelper url, string contentPath)
        {
            if (contentPath == null) throw new ArgumentNullException(nameof(contentPath));
            string path = HostingEnvironment.MapPath(contentPath);
            if (File.Exists(path) == false) throw new Exception($"{path} does not exist");
            string hashPart = string.Empty;
            if (path != null)
            {
                byte[] bytes = File.ReadAllBytes(path);
                using (var md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(bytes);
                    var hashBuilder = new StringBuilder();
                    foreach (byte b in hash.Take(4))
                    {
                        hashBuilder.Append($"{b:x2}");
                    }

                    hashPart = $"?{hashBuilder}";
                }
            }

            return $"{url.Content(contentPath)}{hashPart}";
        }

        public static string GetWebcalUrl(this UrlHelper helper)
        {
            Uri uri = helper.RequestContext.HttpContext.Request.Url;
            Debug.Assert(uri != null, "uri != null");
            string url = $"webcal://{uri.Host}/api/calendar";
            return url;
        }
    }
}
