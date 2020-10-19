namespace Snittlistan.Web.Areas.SupporterTravet
{
    using System.Web.Mvc;

    public class SupporterTravetAreaRegistration : AreaRegistration
    {
        public override string AreaName => "SupporterTravet";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // default route
            context.MapRoute(
                "SupporterTravet_default",
                "SupporterTravet/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
