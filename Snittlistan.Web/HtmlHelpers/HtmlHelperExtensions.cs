#nullable enable

namespace Snittlistan.Web.HtmlHelpers
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public static class HtmlHelperExtensions
    {
        private static readonly JsonSerializerSettings defaultSettings = new() { Formatting = Formatting.Indented };

        public static ApplicationMode ApplicationMode(this HtmlHelper _)
        {
            return MvcApplication.Mode;
        }

        public static string ToJson<T>(this T obj, JsonSerializerSettings? settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? defaultSettings);
        }

        public static HtmlString ActionMenuLink(
            this HtmlHelper helper,
            string text,
            string icon,
            string action,
            string url)
        {
            System.Web.Routing.RouteValueDictionary routeData = helper.ViewContext.RouteData.Values;
            object currentAction = routeData["action"];

            string liClass = string.Empty;
            if (string.Equals(action, currentAction as string, StringComparison.OrdinalIgnoreCase))
            {
                liClass = "active";
            }

            TagBuilder tag = new("li");
            tag.MergeAttribute("class", liClass);
            TagBuilder anchor = new("a");
            anchor.MergeAttribute("href", url);
            TagBuilder li = new("i");
            if (string.IsNullOrWhiteSpace(icon) == false)
            {
                li.MergeAttribute("class", icon);
            }

            anchor.InnerHtml = li + text;
            tag.InnerHtml = anchor.ToString();
            return new HtmlString(tag.ToString());
        }

        public static HtmlString FormatDateSpan(this HtmlHelper helper, DateTime from, DateTime to)
        {
            StringBuilder builder = new();
            if (from.Date == to.Date)
            {
                _ = builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.ToString("d MMM"));
            }
            else if (from.Month == to.Month)
            {
                _ = builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.Day);
                _ = builder.Append("<text>&minus;</text>");
                _ = builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    to.ToString("s"),
                    to.ToString("d MMM"));
            }
            else
            {
                _ = builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    from.ToString("s"),
                    from.ToString("d MMM"));
                _ = builder.AppendFormat(
                    "<text>&minus;</text>");
                _ = builder.AppendFormat(
                    @"<time datetime=""{0}"">{1}</time>",
                    to.ToString("s"),
                    to.ToString("d MMM"));
            }

            return new HtmlString(builder.ToString());
        }

        public static HtmlString GetAssemblyVersion(this HtmlHelper helper)
        {
            string version = MvcApplication.GetAssemblyVersion();
            return new HtmlString(version);
        }
    }
}
