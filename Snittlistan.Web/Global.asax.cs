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
#if DEBUG
        private const bool IsDebug = true;
#else
        private const bool IsDebug = false;
#endif
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static IWindsorContainer Container { get; private set; }

        public static bool IsDebugConfig { get { return IsDebug; } }

        protected void Application_Start()
        {
            Log.Info("Application Starting");

            RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize container and controller factory
            InitializeContainer();

            // register routes
            new RouteConfig(RouteTable.Routes).Configure();
            AreaRegistration.RegisterAllAreas();

            // add model binders
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            // configure AutoMapper
            AutoMapperConfiguration.Configure(Container);
        }

        protected void Application_End()
        {
            Log.Info("Application Ending");
            Container.Dispose();
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