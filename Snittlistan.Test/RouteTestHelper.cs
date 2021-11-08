namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;
    using Moq;
    using NUnit.Framework;

    public static class RouteTestHelper
    {
        public static void Maps(this RouteCollection routes, string httpVerb, string url, object expectations)
        {
            RouteData routeData = RetrieveRouteData(routes, httpVerb, url);
            Assert.NotNull(routeData);

            foreach (PropertyValue property in GetProperties(expectations))
            {
                bool equal = string.Equals(
                    property.Value.ToString(),
                    routeData.Values[property.Name].ToString(),
                    StringComparison.OrdinalIgnoreCase);
                string message = $"Expected '{property.Value}', not '{routeData.Values[property.Name]}' for '{property.Name}'.";
                Assert.True(equal, message);
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
            public string Name
            {
                get;
                set;
            }

            public object Value
            {
                get;
                set;
            }
        }
    }
}
