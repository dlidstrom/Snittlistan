using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Raven.Imports.Newtonsoft.Json;

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
    }
}