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

            this.routes.MapRoute(
                name: "Hacker-Routes",
                url: "{*php}",
                defaults: new { controller = "Hacker", action = "Index" },
                constraints: new { php = @".*\.php.*|catalog|^s?cgi(\-bin)?.*|^scripts.*|^(aw)?stats.*|^shop.*" });
        }
    }
}