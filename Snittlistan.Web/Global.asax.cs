using System;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EventStoreLite.IoC;
using NLog;
using Raven.Client;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.AutoMapper;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Infrastructure.Installers;
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

        public static ApplicationMode Mode => applicationMode;

        public static void Bootstrap(IWindsorContainer container, HttpConfiguration configuration)
        {
            Container = container;
            applicationMode = ApplicationMode.Test;
            Bootstrap(configuration);
            IndexCreator.CreateIndexes(container.Resolve<IDocumentStore>());
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
                var tenantConfigurations = new[]
                {
                    new TenantConfiguration(
                        "test.localhost",
                        "Snittlistan",
                        "Snittlistan-hofvet",
                        "hofvet.ico",
                        "hofvet.png",
                        "76x76",
                        "Hofvet"),
                    new TenantConfiguration(
                        "snittlistan.se",
                        "Snittlistan",
                        "Snittlistan-hofvet",
                        "hofvet.ico",
                        "hofvet.png",
                        "76x76",
                        "Hofvet"),
                    new TenantConfiguration(
                        "vartansik.snittlistan.se",
                        "Snittlistan-vartansik",
                        "Snittlistan-vartansik",
                        "vartansik.ico",
                        "vartansik.png",
                        "180x180",
                        "Värtans IK")
                };
                foreach (var tenantConfiguration in tenantConfigurations)
                {
                    Container.Register(
                        Component.For<TenantConfiguration>()
                                 .Instance(tenantConfiguration)
                                 .Named(tenantConfiguration.Name));
                }

                Container.Kernel.AddHandlerSelector(new HostBasedComponentSelector());
                Container.Install(
                    new ApiControllerInstaller(),
                    new AutoMapperInstaller(),
                    new BackgroundTaskHandlersInstaller(),
                    new BitsClientInstaller(),
                    new ControllerInstaller(),
                    new EventMigratorInstaller(),
                    new EventStoreSessionInstaller(),
                    new HandlersInstaller(),
                    new RavenInstaller(),
                    new ServicesInstaller(),
                    new TaskRunnerInstaller(),
                    EventStoreInstaller.FromAssembly(Assembly.GetExecutingAssembly(), DocumentStoreMode.Server));
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