using System.Web.Mvc;
using System.Web.Routing;
using SnittListan.Helpers;

namespace SnittListan
{
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
			routes.IgnoreRoute("admin/elmah.axd/{*pathInfo}");

			ConfigureAccount();
			ConfigureHome();

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}

		private void ConfigureAccount()
		{
			routes.MapRouteLowerCase(
				"AccountController-Action",
				"{action}",
				new { controller = "Account" },
				new { action = "LogOn|Register" });
		}

		private void ConfigureHome()
		{
			routes.MapRouteLowerCase(
				"HomeController-Action",
				"{action}",
				new { controller = "Home" },
				new { action = "About" });
		}
	}
}