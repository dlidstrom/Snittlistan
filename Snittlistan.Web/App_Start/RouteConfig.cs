namespace Snittlistan.Web.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        private readonly RouteCollection routes;

        public RouteConfig(RouteCollection routes)
        {
            this.routes = routes;
        }

        public void Configure()
        {
            this.routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            this.routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // robots.txt
            this.routes.IgnoreRoute("{file}.txt");
        }
    }
}