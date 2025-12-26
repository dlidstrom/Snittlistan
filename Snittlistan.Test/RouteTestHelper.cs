using System.ComponentModel;
using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

#nullable enable

namespace Snittlistan.Test;
public static class RouteTestHelper
{
    public static void Maps(this RouteCollection routes, string httpVerb, string url, object expectations)
    {
        RouteData routeData = RetrieveRouteData(routes, httpVerb, url);
        Assert.That(routeData, Is.Not.Null);

        foreach (PropertyValue property in GetProperties(expectations))
        {
            bool equal = string.Equals(
                property.Value!.ToString(),
                routeData.Values[property.Name].ToString(),
                StringComparison.OrdinalIgnoreCase);
            string message = $"Expected '{property.Value}', not '{routeData.Values[property.Name]}' for '{property.Name}'.";
            Assert.That(equal, message, Is.True);
        }
    }

    public static void DoNotMap(this RouteCollection routes, string httpVerb, string url)
    {
        Assert.That(RetrieveRouteData(routes, httpVerb, url), Is.Null);
    }

    private static RouteData RetrieveRouteData(RouteCollection routes, string httpVerb, string url)
    {
        Mock<HttpContextBase> httpContext = new();
        _ = httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
        _ = httpContext.Setup(c => c.Request.HttpMethod).Returns(httpVerb);

        return routes.GetRouteData(httpContext.Object);
    }

    private static IEnumerable<PropertyValue> GetProperties(object o)
    {
        return from prop in TypeDescriptor.GetProperties(o).Cast<PropertyDescriptor>()
               let val = prop.GetValue(o)
               where val != null
               select new PropertyValue { Name = prop.Name, Value = val };
    }

    private sealed class PropertyValue
    {
        public string? Name
        {
            get;
            set;
        }

        public object? Value
        {
            get;
            set;
        }
    }
}
