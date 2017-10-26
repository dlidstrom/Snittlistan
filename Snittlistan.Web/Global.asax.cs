using System;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using EventStoreLite;
using EventStoreLite.IoC.Castle;
using NLog;
using Raven.Client;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Infrastructure.IoC;
using Snittlistan.Web.Models;

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

            Container?.Dispose();
        }

        public static string GetAssemblyVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version.ToString();
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

        protected void Application_BeginRequest()
        {
            if (Context.IsDebuggingEnabled)
            {
                return;
            }

            if (Context.Request.IsSecureConnection == false
                && Context.Request.Url.ToString().Contains("localhost:") == false)
            {
                Response.Clear();
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", Context.Request.Url.ToString().Insert(4, "s"));
                Response.End();
            }
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

            Emails.Initialize(HostingEnvironment.MapPath("~/Views/Emails"));
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
                Container = new WindsorContainer();
                Container.AddFacility<StartableFacility>();
                Container.Register(
                    Component.For<TenantConfiguration>()
                             .Instance(new TenantConfiguration("Snittlistan"))
                             .Named("test.localhost"));
                Container.Register(
                    Component.For<TenantConfiguration>()
                             .Instance(new TenantConfiguration("Snittlistan"))
                             .Named("snittlistan.se"));
                Container.Register(
                    Component.For<TenantConfiguration>()
                             .Instance(new TenantConfiguration("Snittlistan-vartansik"))
                             .Named("vartansik.snittlistan.se"));
                Container.Kernel.AddHandlerSelector(new HostBasedComponentSelector());
                Container.Install(
                    FromAssembly.This(),
                    EventStoreInstaller.FromAssembly(Assembly.GetExecutingAssembly()));
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

            // create indexes
            var store = Container.Resolve<IDocumentStore>();
            if (ShouldInitializeIndexes(store, GetAssemblyVersion()))
            {
                Log.Info("Creating indexes");
                var eventStore = Container.Resolve<EventStore>();
                IndexCreator.ResetIndexes(store, eventStore);
            }
            else
            {
                Log.Info("Skipping creation of indexes");
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
            configuration.DependencyResolver =
                new WindsorHttpDependencyResolver(Container.Kernel);
        }

        private static bool ShouldInitializeIndexes(IDocumentStore documentStore, string version)
        {
            using (var session = documentStore.OpenSession())
            {
                var config = session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                if (config == null)
                {
                    Log.Info("Creating website config");
                    config = new WebsiteConfig();
                    session.Store(config);
                }

                Log.Info("Current version: {0}, IndexCreatedVersion: {1}", version, config.IndexCreatedVersion);
                var newVersion = true;
                try
                {
                    var oldMajorMinor = config.IndexCreatedVersion.Substring(0, config.IndexCreatedVersion.IndexOf('.', 1 + config.IndexCreatedVersion.IndexOf('.')));
                    var newMajorMinor = version.Substring(0, version.IndexOf('.', 1 + version.IndexOf('.')));
                    newVersion = oldMajorMinor != newMajorMinor;
                }
                catch
                {
                }

                config.SetIndexCreatedVersion(version);
                session.SaveChanges();

                return newVersion;
            }
        }
    }
}