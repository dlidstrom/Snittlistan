using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using EventStoreLite.IoC.Castle;
using NLog;
using Raven.Client;
using Snittlistan.Web.App_Start;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.IoC;

namespace Snittlistan.Web
{
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

        public static IWindsorContainer ChildContainer { get; private set; }

        public static ApplicationMode Mode { get { return applicationMode; } }

        public static void Bootstrap(IWindsorContainer container, HttpConfiguration configuration)
        {
            Container = container;
            applicationMode = ApplicationMode.Test;
            Bootstrap(configuration);
        }

        public static void Shutdown()
        {
            ModelBinders.Binders.Clear();
            RouteTable.Routes.Clear();
            if (ChildContainer != null)
            {
                Container.RemoveChildContainer(ChildContainer);
                ChildContainer.Dispose();
            }

            if (Container != null)
                Container.Dispose();
        }

        protected void Application_Start()
        {
            Log.Info("Application Starting");
            Bootstrap(GlobalConfiguration.Configuration);
        }

        protected void Application_End()
        {
            Log.Info("Application Ending");
            Shutdown();
        }

        private static void Bootstrap(HttpConfiguration configuration)
        {
            RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize container and controller factory
            InitializeContainer(configuration);

            // register routes
            new RouteConfig(RouteTable.Routes).Configure();
            WebApiConfig.Register(configuration);
            if (Mode != ApplicationMode.Test)
                AreaRegistration.RegisterAllAreas();

            // add model binders
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            // configure AutoMapper
            AutoMapperConfiguration.Configure(Container);
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserTrackerLogAttribute());
        }

        private static void InitializeContainer(HttpConfiguration configuration)
        {
            if (Container == null)
            {
                Container = new WindsorContainer().Install(
                    FromAssembly.This(), EventStoreInstaller.FromAssembly(Assembly.GetExecutingAssembly()));
            }

            if (ChildContainer == null)
            {
                ChildContainer = new WindsorContainer().Register(
                    Component.For<IDocumentSession>().UsingFactoryMethod(kernel =>
                        {
                            var documentSession = kernel.Resolve<IDocumentStore>()
                                .OpenSession();
                            documentSession.Advanced.UseOptimisticConcurrency = true;
                            return documentSession;
                        }).LifestyleTransient());
                Container.AddChildContainer(ChildContainer);
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
            configuration.DependencyResolver =
                new WindsorHttpDependencyResolver(Container.Kernel);
        }
    }
}