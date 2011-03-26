namespace SnittListan
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// The application itself.
	/// </summary>
	/// <remarks>For instructions on enabling IIS6 or IIS7 classic mode, 
	/// visit http://go.microsoft.com/?LinkId=9394801</remarks>
	public class MvcApplication : HttpApplication
	{
		/// <summary>
		/// Registers global filters.
		/// </summary>
		/// <param name="filters">Add filters to this collection.</param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		/// <summary>
		/// Registers routes.
		/// </summary>
		/// <param name="routes">Routes collection.</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new
				{
					// Parameter defaults
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				});
		}

		/// <summary>
		/// Run when the application starts. Perform any
		/// necessary registration within this method.
		/// </summary>
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}