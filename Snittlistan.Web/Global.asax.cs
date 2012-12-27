namespace Snittlistan.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Castle.Windsor;
    using Castle.Windsor.Installer;

    using NLog;

    using Snittlistan.Web.App_Start;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Infrastructure.IoC;

    public class MvcApplication : HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static ApplicationMode applicationMode =
#if DEBUG
            ApplicationMode.Debug;
#else
            ApplicationMode.Release;
#endif

        public static IWindsorContainer Container { get; private set; }

        public static ApplicationMode Mode { get { return applicationMode; } }

        public static void Bootstrap(ApplicationMode mode)
        {
            applicationMode = mode;
            Bootstrap();
        }

        public static void Bootstrap()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize container and controller factory
            InitializeContainer();

            // register routes
            new RouteConfig(RouteTable.Routes).Configure();
            if (Mode != ApplicationMode.Test)
                AreaRegistration.RegisterAllAreas();

            // add model binders
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            // configure AutoMapper
            AutoMapperConfiguration.Configure(Container);
        }

        public static void Shutdown()
        {
            ModelBinders.Binders.Clear();
            RouteTable.Routes.Clear();
            Container.Dispose();
        }

        protected void Application_Start()
        {
            Log.Info("Application Starting");

            Bootstrap();
        }

        protected void Application_End()
        {
            Log.Info("Application Ending");
            Shutdown();
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
            if (Container == null)
            {
                Container = new WindsorContainer()
                    .Install(FromAssembly.This());
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
        }
    }
}