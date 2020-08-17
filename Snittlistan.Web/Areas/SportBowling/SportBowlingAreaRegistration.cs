namespace Snittlistan.Web.Areas.SportBowling
{
    using System.Web.Mvc;
    using Snittlistan.Web.Infrastructure;

    public class SportBowlingAreaRegistration : AreaRegistration
    {
        public override string AreaName => "SportBowling";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new
                {
                    hostname = new HostnameRouteConstraint("sportbowling.se")
                });
        }
    }
}
