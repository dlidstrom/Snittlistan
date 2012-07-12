namespace Snittlistan
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Common.Logging;
    using Infrastructure;
    using Infrastructure.Attributes;
    using Infrastructure.AutoMapper;
    using Infrastructure.IoC;

    public class MvcApplication : HttpApplication
    {
#if DEBUG
        private const bool IsDebug = true;
#else
        private const bool IsDebug = false;
#endif
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private static IWindsorContainer container;

        public static IWindsorContainer Container
        {
            get { return container; }
        }

        public static bool IsDebugConfig { get { return IsDebug; } }

        protected void Application_Start()
        {
            Log.Info("Application Starting");
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize container and controller factory
            InitializeContainer();

            // register routes
            new RouteConfigurator(RouteTable.Routes).Configure();

            // add model binders
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            // configure AutoMapper
            AutoMapperConfiguration.Configure(container);
        }

        protected void Application_End()
        {
            Log.Info("Application Ending");
            container.Dispose();
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserTrackerLogAttribute());
            filters.Add(new RavenActionFilterAttribute());
        }

        private static void InitializeContainer()
        {
            if (container == null)
            {
                container = new WindsorContainer()
                    .Install(FromAssembly.This());
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}