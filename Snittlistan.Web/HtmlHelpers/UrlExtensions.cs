using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;

#nullable enable

namespace Snittlistan.Web.HtmlHelpers;
public static class UrlExtensions
{
    public static string ContentCacheBreak(this UrlHelper url, string contentPath)
    {
        if (contentPath == null)
        {
            throw new ArgumentNullException(nameof(contentPath));
        }

        string path = HostingEnvironment.MapPath(contentPath);
        if (File.Exists(path) == false)
        {
            throw new Exception($"{path} does not exist");
        }

        string hashPart = string.Empty;
        if (path != null)
        {
            byte[] bytes = File.ReadAllBytes(path);
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(bytes);
            StringBuilder hashBuilder = new();
            foreach (byte b in hash)
            {
                _ = hashBuilder.Append($"{b:x2}");
            }

            hashPart = $"?{hashBuilder}";
        }

        return $"{url.Content(contentPath)}{hashPart}";
    }

    public static string GetWebcalUrl(this UrlHelper helper)
    {
        Uri uri = helper.RequestContext.HttpContext.Request.Url;
        Debug.Assert(uri != null, "uri != null");
        string url = $"webcal://{uri!.Host}/api/calendar";
        return url;
    }
}
