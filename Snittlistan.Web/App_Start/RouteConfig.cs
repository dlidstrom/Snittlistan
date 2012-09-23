namespace Snittlistan.Web.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Snittlistan.Web.Infrastructure;

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

            this.ConfigureAccount();
            this.ConfigureReset();
            this.ConfigureHome();
            this.ConfigureElmah();
            this.ConfigureHacker();
            this.V2Route();
            this.ApiRoute();

            // default route
            this.routes.MapRoute(
                name: "Default", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults

            this.NotFoundRoute();
            this.CatchAllRoute();
        }

        private void ConfigureAccount()
        {
            // ~/logon|register|verify
            this.routes.MapRouteLowerCase(
                name: "AccountController-Action",
                url: "{action}",
                defaults: new { controller = "Account" },
                constraints: new { action = "^LogOn$|^Register$|^Verify$" });
        }

        private void ConfigureReset()
        {
            this.routes.MapRouteLowerCase(
                name: "WelcomeController-Reset",
                url: "{action}",
                defaults: new { controller = "Welcome" },
                constraints: new { action = "^reset$" });
        }

        private void ConfigureHome()
        {
            // ~/about
            this.routes.MapRouteLowerCase(
                name: "HomeController-Action",
                url: "{action}",
                defaults: new { controller = "Home" },
                constraints: new { action = "About" });
        }

        private void ConfigureElmah()
        {
            this.routes.MapRouteLowerCase(
                name: "ElmahController-internal",
                url: "admin/elmah/{type}",
                defaults: new { controller = "Elmah", action = "Index", type = UrlParameter.Optional });
        }

        private void ConfigureHacker()
        {
            this.routes.MapRoute(
                name: "Hacker-Routes",
                url: "{*php}",
                defaults: new { controller = "Hacker", action = "Index" },
                constraints: new { php = @".*\.php.*|catalog|^s?cgi(\-bin)?.*|^scripts.*|^(aw)?stats.*|^shop.*" });
        }

        private void V2Route()
        {
            this.routes.MapRoute("V2-index", "v2", new { controller = "App", action = "Index" });
            this.routes.MapRoute("V2-turns", "v2/turns", new { controller = "App", action = "Index" });
            this.routes.MapRoute("V2-players", "v2/results", new { controller = "App", action = "Results" });
            this.routes.MapRoute("V2-results", "v2/players", new { controller = "App", action = "Players" });
        }

        private void ApiRoute()
        {
            this.routes.MapRoute("SessionApi-route", "api/session", new { controller = "SessionApi", action = "Session" });
            this.routes.MapRoute("TurnsApi-route", "api/turns", new { controller = "TurnsApi", action = "Turns" });
        }

        private void NotFoundRoute()
        {
            // To allow IIS to execute "/notfound" when requesting something which is disallowed,
            // such as /bin or /add_data.
            this.routes.MapRoute(
                name: "NotFound",
                url: "notfound",
                defaults: new { controller = "NotFound", action = "NotFound" });
        }

        private void CatchAllRoute()
        {
            this.routes.MapRoute(
                name: "NotFound-Catch-All",
                url: "{*any}",
                defaults: new { controller = "NotFound", action = "NotFound" });
        }
    }
}