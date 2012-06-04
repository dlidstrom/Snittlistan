namespace Snittlistan
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Snittlistan.Infrastructure;

    public class RouteConfigurator
    {
        private readonly RouteCollection routes;

        public RouteConfigurator(RouteCollection routes)
        {
            this.routes = routes;
        }

        public void Configure()
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // robots.txt
            routes.IgnoreRoute("{file}.txt");

            ConfigureAccount();
            ConfigureHome();
            ConfigureElmah();
            ConfigureHacker();

            // default route
            routes.MapRoute(
                name: "Default", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults

            NotFoundRoute();
            CatchAllRoute();
        }

        private void ConfigureAccount()
        {
            // ~/logon|register|verify
            routes.MapRouteLowerCase(
                name: "AccountController-Action",
                url: "{action}",
                defaults: new { controller = "Account" },
                constraints: new { action = "^LogOn$|^Register$|^Verify$" });
        }

        private void ConfigureHome()
        {
            // ~/about
            routes.MapRouteLowerCase(
                name: "HomeController-Action",
                url: "{action}",
                defaults: new { controller = "Home" },
                constraints: new { action = "About" });
        }

        private void ConfigureElmah()
        {
            routes.MapRouteLowerCase(
                name: "ElmahController-internal",
                url: "admin/elmah/{type}",
                defaults: new { controller = "Elmah", action = "Index", type = UrlParameter.Optional });
        }

        private void ConfigureHacker()
        {
            routes.MapRoute(
                name: "Hacker-Routes",
                url: "{*php}",
                defaults: new { controller = "Hacker", action = "Index" },
                constraints: new { php = @".*\.php.*|catalog|^s?cgi(\-bin)?.*|^scripts.*|^(aw)?stats.*|^shop.*" });
        }

        private void NotFoundRoute()
        {
            // To allow IIS to execute "/notfound" when requesting something which is disallowed,
            // such as /bin or /add_data.
            routes.MapRoute(
                name: "NotFound",
                url: "notfound",
                defaults: new { controller = "NotFound", action = "NotFound" });
        }

        private void CatchAllRoute()
        {
            routes.MapRoute(
                name: "NotFound-Catch-All",
                url: "{*any}",
                defaults: new { controller = "NotFound", action = "NotFound" });
        }
    }
}