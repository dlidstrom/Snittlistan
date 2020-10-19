namespace Snittlistan.Web.Areas.V2
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    public class V2AreaRegistration : AreaRegistration
    {
        public override string AreaName => "V2";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Redirects1",
                url: "Home/Player/{*rest}",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects2",
                url: "Match/Details4x4/{*rest}",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects3",
                url: "Match/Details8x4/{*rest}",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects4",
                url: "Match",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects5",
                url: "register",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects6",
                url: "Account/Register",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects7",
                url: "Roster/{season}",
                defaults: new
                {
                    controller = "Redirect",
                    action = "RedirectNewView"
                },
                constraints: new
                {
                    season = @"\d+"
                });
            context.MapRoute(
                name: "Redirects8",
                url: "Roster/{season}/{turn}",
                defaults: new
                {
                    controller = "Redirect",
                    action = "RedirectNewView"
                },
                constraints: new
                {
                    season = @"\d+",
                    turn = @"\d+"
                });
            context.MapRoute(
                name: "Redirects9",
                url: "MatchResultAdmin/RegisterSerie",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects10",
                url: "App/LogOn2",
                defaults: new { controller = "Redirect", action = "Redirect" });
            string[] urls = new[]
            {
                "admin",
                "old-site",
                "tmp",
                "dev",
                "demo",
                "backup",
                "home",
                "site",
                "main",
                "blog",
                "old",
                "test",
                "wordpress",
                "new",
                "wp",
            };
            for (int i = 0; i < urls.Length; i++)
            {
                string u = urls[i];
                context.MapRoute(
                name: $"Redirects{i + 11}",
                url: u,
                defaults: new { controller = "Redirect", action = "Redirect" });
            }

            context.MapRoute(
                name: "SearchTerms-Route",
                url: "Search/{action}",
                defaults: new { controller = "SearchTerms" });

            // register match result TODO is this needed now?
            context.MapRoute(
                "RegisterMatchResult",
                "MatchResult/Register/{season}",
                new { controller = "MatchResult", action = "Register" });

            context.MapRoute(
                "RegisterMatchEditor",
                "MatchResultAdmin/RegisterMatch4Editor/{rosterId}",
                new
                {
                    controller = "MatchResultAdmin",
                    action = "RegisterMatch4Editor"
                });

            RosterRoutes(context);

            // default route
            IEnumerable<string> controllerNames =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(Controller).IsAssignableFrom(t) && t.Namespace == "Snittlistan.Web.Areas.V2.Controllers"
                select t.Name.Replace("Controller", string.Empty);
            context.MapRoute(
                "V2_default",
                "{controller}/{action}/{id}",
                new { controller = "Roster", action = "Index", id = UrlParameter.Optional },
                new { controller = $"^({string.Join("|", controllerNames)})$" });
        }

        private static void RosterRoutes(AreaRegistrationContext context)
        {
            // create roster
            context.MapRoute(
                "CreateRoster",
                "Roster/Create",
                new
                {
                    controller = "Roster",
                    action = "Create"
                });

            // edit roster
            context.MapRoute(
                "EditRoster",
                "Roster/Edit/{id}",
                new
                {
                    controller = "Roster",
                    action = "Edit"
                });

            // delete roster
            context.MapRoute(
                "DeleteRoster",
                "Roster/Delete/{id}",
                new
                {
                    controller = "Roster",
                    action = "Delete"
                });

            // edit players
            context.MapRoute(
                "EditPlayers",
                "Roster/Players/{id}",
                new
                {
                    controller = "Roster",
                    action = "EditPlayers"
                });

            // view roster
            context.MapRoute(
                "ViewRoster",
                "RosterView/{season}/{turn}",
                new
                {
                    controller = "Roster",
                    action = "View",
                    season = UrlParameter.Optional,
                    turn = UrlParameter.Optional
                });
        }
    }
}
