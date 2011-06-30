using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Common.Logging;
using SnittListan.Helpers.Attributes;
using SnittListan.IoC;

namespace SnittListan
{
	public class MvcApplication : HttpApplication
	{
		private static readonly ILog logger = LogManager.GetCurrentClassLogger();
		private static IWindsorContainer container;

		public static IWindsorContainer Container
		{
			get { return container; }
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ElmahHandleErrorAttribute());
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("admin/elmah.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			// initialize container and controller factory
			InitializeContainer();
		}

		protected void Application_End()
		{
			container.Dispose();
		}

		private void InitializeContainer()
		{
			logger.Info("Initializing container");
			if (container == null)
			{
				container = new WindsorContainer()
					.Install(FromAssembly.This());
			}

			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}
	}
}