using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Raven.Imports.Newtonsoft.Json;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static ApplicationMode ApplicationMode(this HtmlHelper helper)
        {
            return MvcApplication.Mode;
        }

        public static HtmlString ToJson<T>(this HtmlHelper helper, T obj)
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };
            var builder = new StringBuilder();
            serializer.Serialize(new StringWriter(builder), obj);
            return new HtmlString(builder.ToString());
        }

        public static HtmlString ActionMenuLink(
            this HtmlHelper helper,
            string text,
            string icon,
            string action,
            string url)
        {
            var routeData = helper.ViewContext.RouteData.Values;
            var currentAction = routeData["action"];

            var liClass = string.Empty;
            if (string.Equals(action, currentAction as string, StringComparison.OrdinalIgnoreCase))
            {
                liClass = "active";
            }

            var tag = new TagBuilder("li");
            tag.MergeAttribute("class", liClass);
            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("href", url);
            var li = new TagBuilder("i");
            if (string.IsNullOrWhiteSpace(icon) == false)
                li.MergeAttribute("class", icon);
            anchor.InnerHtml = li + text;
            tag.InnerHtml = anchor.ToString();
            return new HtmlString(tag.ToString());
        }

        public static HtmlString FormatDateSpan(this HtmlHelper helper, DateTime from, DateTime to)
        {
            var builder = new StringBuilder();
            if (from.Date == to.Date)
            {
                builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.ToString("d MMM"));
            }
            else if (from.Month == to.Month)
            {
                builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.Day);
                builder.Append("<text>&minus;</text>");
                builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    to.ToString("s"),
                    to.ToString("d MMM"));
            }
            else
            {
                builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.ToString("d MMM"));
                builder.AppendFormat(
                    "<text>&minus;</text>");
                builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    to.ToString("s"),
                    to.ToString("d MMM"));
            }

            return new HtmlString(builder.ToString());
        }

        public static HtmlString GetAssemblyVersion(this HtmlHelper helper)
        {
            var version = MvcApplication.GetAssemblyVersion();
            return new HtmlString(version);
        }

        public static WebsiteConfig GetWebsiteConfig(this HtmlHelper helper)
        {
            var documentSession = DependencyResolver.Current.GetService<IDocumentSession>();
            return documentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        }

        public static TenantConfiguration GetTenantConfiguration(this HtmlHelper helper)
        {
            var tenantConfiguration = DependencyResolver.Current.GetService<TenantConfiguration>();
            return tenantConfiguration;
        }
    }
}