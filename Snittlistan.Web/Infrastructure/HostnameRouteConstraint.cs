namespace Snittlistan.Web.Infrastructure
{
    using System.Web;
    using System.Web.Routing;

    public class HostnameRouteConstraint : IRouteConstraint
    {
        private readonly string hostname;

        public HostnameRouteConstraint(string hostname)
        {
            this.hostname = hostname;
        }

        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return httpContext.Request.Url.Host.EndsWith(hostname);
        }
    }
}
