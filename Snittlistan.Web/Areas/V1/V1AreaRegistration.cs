
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V1;
public class V1AreaRegistration : AreaRegistration
{
    public override string AreaName => "V1";

    public override void RegisterArea(AreaRegistrationContext context)
    {
        // ~/about
        context.MapRoute(
            name: "HomeController-Action",
            url: "v1/{action}",
            defaults: new { controller = "Home" },
            constraints: new { action = "About" });

        // ~/Home/About which is also matched by rule in V2, which results in no view being found
        context.MapRoute(
            name: "HomeController-About",
            url: "Home/About",
            defaults: new { controller = "Home", action = "About" });

        // ~/logon|register|verify
        context.MapRoute(
            name: "AccountController-Action",
            url: "v1/{action}",
            defaults: new { controller = "Account" },
            constraints: new { action = "^LogOn$|^Register$|^Verify$" });

        // ~/Account/LogOn which is also matched by rule in V2, which results in no view being found
        context.MapRoute(
            name: "AccountController-LogOn",
            url: "Account/LogOn",
            defaults: new { controller = "Account", action = "LogOn" });

        // legacy route
        context.MapRoute(
            name: "LegacyMatch-route",
            url: "Match/Details/{id}",
            defaults: new
            {
                controller = "Match",
                action = "LegacyRedirect",
                id = UrlParameter.Optional
            });

        context.MapRoute(
            name: "WelcomeController-Reset",
            url: "v1/{action}",
            defaults: new { controller = "Welcome" },
            constraints: new { action = "^reset$" });

        context.MapRoute(
            name: "ElmahController-internal",
            url: "v1/admin/elmah/{type}",
            defaults: new { controller = "Elmah", action = "Index", type = UrlParameter.Optional });

        context.MapRoute(
            "V1_roster",
            "v1/{*rest}",
            defaults: new
            {
                controller = "Match",
                action = "LegacyRedirect"
            });

        context.MapRoute(
            "V1_default",
            "v1/{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Match",
                action = "LegacyRedirect",
                id = UrlParameter.Optional
            });
    }
}
