namespace Snittlistan
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Common.Logging;
    using Raven.Client;
    using Snittlistan.Helpers;
    using Snittlistan.Helpers.Attributes;
    using Snittlistan.Infrastructure;
    using Snittlistan.Infrastructure.AutoMapper;
    using Snittlistan.IoC;
    using Snittlistan.Models;

    public class MvcApplication : HttpApplication
    {
#if DEBUG
        private static readonly bool isDebug = true;
#else
        private static readonly bool isDebug = false;
#endif
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();
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
            logger.Info("Application Starting");
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

            if (IsDebug)
            {
                // always release through container, even though
                // store is a singleton
                var store = Container.Resolve<IDocumentStore>();
                using (var session = store.OpenSession())
                {
                    if (!session.BitsIdExists(3003231))
                    {
                        session.Store(DbSeed.CreateMatch());
                        session.SaveChanges();
                    }
                }

                Container.Release(store);
            }
        }

        protected void Application_End()
        {
            logger.Info("Application Ending");
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