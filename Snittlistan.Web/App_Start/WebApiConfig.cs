#nullable enable

namespace Snittlistan.Web
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Newtonsoft.Json.Serialization;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            _ = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // camelCase by default
            JsonMediaTypeFormatter formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Services.Add(typeof(IExceptionLogger), new LoggingExceptionLogger());

            config.Filters.Add(new UnhandledExceptionFilter());
            config.Filters.Add(new ValidateModelAttribute());
            config.Formatters.Add(new ICalFormatter());
            config.MessageHandlers.Add(new OutlookAgentMessageHandler());
        }
    }
}
