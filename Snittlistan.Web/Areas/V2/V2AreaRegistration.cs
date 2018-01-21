using System.Web.Http;
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V2
{
    public class V2AreaRegistration : AreaRegistration
    {
        public override string AreaName => "V2";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("SessionApi-route", "api/session", new { controller = "SessionApi", action = "Session" });
            context.MapRoute("TurnsApi-route", "api/turns", new { controller = "TurnsApi", action = "Turns" });

            // root routes
            context.MapRoute(
                "V2_root",
                "{action}/{id}",
                new { controller = "App", id = RouteParameter.Optional },
                new { action = "^players$|^results$" });

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
                name: "RegisterRedirect",
                url: "register",
                defaults: new { controller = "Redirect", action = "Redirect" });
            context.MapRoute(
                name: "Redirects5",
                url: "Account/Register",
                defaults: new { controller = "Redirect", action = "Redirect" });

            context.MapRoute(
                name: "SearchTerms-Route",
                url: "Search/{action}",
                defaults: new { controller = "SearchTerms" });

            // register match result
            context.MapRoute(
                "RegisterMatchResult",
                "MatchResult/Register/{season}",
                new { controller = "MatchResult", action = "Register" });

            RosterRoutes(context);

            // default route
            context.MapRoute(
                "V2_default",
                "{controller}/{action}/{id}",
                new { controller = "Roster", action = "Index", id = RouteParameter.Optional });
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
                "Roster/{season}/{turn}",
                new
                {
                    controller = "Roster",
                    action = "View",
                    turn = UrlParameter.Optional
                },
                new
                {
                    season = @"\d+"
                });
        }
    }
}
