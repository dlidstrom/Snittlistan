namespace Snittlistan.Web.HtmlHelpers
{
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Imports.Newtonsoft.Json;

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
    }
}