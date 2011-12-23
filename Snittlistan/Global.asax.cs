namespace Snittlistan
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Common.Logging;
    using Snittlistan.Infrastructure;
    using Snittlistan.Infrastructure.Attributes;
    using Snittlistan.Infrastructure.AutoMapper;
    using Snittlistan.IoC;

    public class MvcApplication : HttpApplication
    {
#if DEBUG
        private static readonly bool isDebug = true;
#else
        private static readonly bool isDebug = false;
#endif
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static IWindsorContainer container;

        public static IWindsorContainer Container
        {
            get { return container; }
        }

        public static bool IsDebug { get { return isDebug; } }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserTrackerLogAttribute());
            filters.Add(new RavenActionFilterAttribute());
        }

        protected void Application_Start()
        {
            log.Info("Application Starting");
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
            log.Info("Application Ending");
            container.Dispose();
        }

        private void InitializeContainer()
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