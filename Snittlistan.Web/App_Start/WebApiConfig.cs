using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Snittlistan.Web.Infrastructure.Attributes;

namespace Snittlistan.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // camelCase by default
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}