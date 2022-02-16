#nullable enable

using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json.Serialization;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;

namespace Snittlistan.Web;

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
        config.MessageHandlers.Add(new CanceledTaskMessageHandler());
    }

    private class CanceledTaskMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            return cancellationToken.IsCancellationRequested
                ? new HttpResponseMessage(HttpStatusCode.InternalServerError)
                : response;
        }
    }
}
