﻿#nullable enable

global using System.Data.Entity;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.Services.Logging.NLogIntegration;
using Castle.Windsor;
using EventStoreLite.IoC;
using NLog;
using Npgsql.Logging;
using Raven.Client;
using Snittlistan.Queue;
using Snittlistan.Web.Areas.V2;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Infrastructure.IoC;
using Snittlistan.Web.Models;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Snittlistan.Web;

public class MvcApplication : HttpApplication
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static IWindsorContainer Container { get; private set; } = null!;

    public static IWindsorContainer ChildContainer { get; private set; } = null!;

    public static ApplicationMode Mode { get; private set; } =
#if DEBUG
 ApplicationMode.Debug;
#else
            ApplicationMode.Release;
#endif

    public static void Bootstrap(
        IWindsorContainer container,
        HttpConfiguration configuration,
        Func<Databases> databasesFactory)
    {
        Container = container;
        Mode = ApplicationMode.Test;
        Bootstrap(configuration, databasesFactory);
        IndexCreator.CreateIndexes(container.Resolve<IDocumentStore>());
    }

    public static void Shutdown()
    {
        ModelBinders.Binders.Clear();
        RouteTable.Routes.Clear();
        if (ChildContainer != null)
        {
            Container!.RemoveChildContainer(ChildContainer);
            ChildContainer.Dispose();
        }

        Container?.Dispose();
    }

    public static string GetAssemblyVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version version = assembly.GetName().Version;
        return version.ToString();
    }

    protected void Application_Start()
    {
        Bootstrap(GlobalConfiguration.Configuration, DatabasesFactory);
        MvcHandler.DisableMvcResponseHeader = true;

        static Databases DatabasesFactory()
        {
            return new(new SnittlistanContext(), new BitsContext());
        }
    }

    protected void Application_End()
    {
        Log.Info("Application Ending");
        Shutdown();
    }

    protected void Application_BeginRequest()
    {
        Guid correlationId = Guid.NewGuid();
        Trace.CorrelationManager.ActivityId = correlationId;
        if (Context.IsDebuggingEnabled || Context.Request.IsLocal)
        {
            return;
        }

        if (Context.Request.IsSecureConnection == false
            && Context.Request.Url.ToString().Contains("localhost:") == false)
        {
            Response.Clear();
            Response.Status = "301 Moved Permanently";
            try
            {
              Response.AddHeader("Location", Context.Request.Url.ToString().Insert(4, "s"));
            }
            catch (Exception ex)
            {
              throw new Exception($"Failed to redirect to {Context.Request.Url}", ex);
            }

            Response.End();
        }
    }

    protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
    {
        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie == null)
        {
            return;
        }

        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        if (authTicket == null)
        {
            return;
        }

        IDocumentSession session = Container!.Resolve<IDocumentSession>();

        // try the player first
        Player player = session.Load<Player>(authTicket.Name);
        if (player != null)
        {
            IEnumerable<string> defaultRoles = WebsiteRoles.PlayerGroup().Select(x => x.Name);
            string[] roles = new HashSet<string>(defaultRoles.Concat(player.Roles)).ToArray();
            HttpContext.Current.User =
                new CustomPrincipal(
                    player.Id,
                    player.Name,
                    player.Email,
                    player.UniqueId,
                    roles);
            return;
        }

        // try the user now
        User user = session.FindUserByEmail(authTicket.Name);
        if (user != null)
        {
            if (user.Id == Models.User.AdminId)
            {
                HttpContext.Current.User =
                    new CustomPrincipal(
                        null,
                        user.Email,
                        user.Email,
                        user.UniqueId,
                        WebsiteRoles.AdminGroup().Select(x => x.Name).ToArray());
            }
            else
            {
                HttpContext.Current.User =
                    new CustomPrincipal(
                        null,
                        user.Email,
                        user.Email,
                        user.UniqueId,
                        WebsiteRoles.UserGroup().Select(x => x.Name).ToArray());
            }

            return;
        }

        Log.Error($"Unable to load profile information using {authTicket.Name}, signing user out");
        FormsAuthentication.SignOut();
    }

    private static void Bootstrap(HttpConfiguration configuration, Func<Databases> databasesFactory)
    {
        RegisterGlobalFilters(GlobalFilters.Filters);

        NpgsqlLogManager.Provider = new NLogLoggingProvider();
        NpgsqlLogManager.IsParameterLoggingEnabled = true;

        Log.Info("Application Starting");

        // initialize container and controller factory
        InitializeContainer(configuration, databasesFactory);

        // register routes
        new RouteConfig(RouteTable.Routes).Configure();
        WebApiConfig.Register(configuration);
        if (Mode != ApplicationMode.Test)
        {
            AreaRegistration.RegisterAllAreas();
        }

        // add model binders
        ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

        WebsiteRoles.Initialize();
    }

    private static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new ElmahHandleErrorAttribute());
        filters.Add(new HandleErrorAttribute());
        filters.Add(new UserTrackerLogAttribute());
    }

    private static void InitializeContainer(
        HttpConfiguration configuration,
        Func<Databases> databasesFactory)
    {
        if (Container == null)
        {
            Container = new WindsorContainer();

            Databases databases = databasesFactory.Invoke();
            Tenant[] tenants = databases.Snittlistan.Tenants.ToArray();

            foreach (Tenant tenant in tenants)
            {
                _ = Container.Register(
                    Component.For<Tenant>()
                             .Instance(tenant)
                             .Named(tenant.Hostname));
            }

            Container.Kernel.AddHandlerSelector(new HostBasedComponentSelector());
            string gatewayUrl = Environment.GetEnvironmentVariable("GatewayUrl");
            Log.Info($"gatewayUrl: {gatewayUrl}");
            HttpClient httpClient = new(
                new RateHandler(rate: 1.0, per: 1.0, maxTries: 60,
                    new LoggingHandler()))
            {
              BaseAddress = new Uri(gatewayUrl)
            };
            _ = Container
                .AddFacility<LoggingFacility>(f => f.LogUsing<NLogFactory>())
                .Install(
                    new ApiControllerInstaller(),
                    new BitsClientInstaller(httpClient),
                    CommandHandlerInstaller.PerWebRequest(),
                    new ControllerInstaller(),
                    new DatabaseContextInstaller(databasesFactory),
                    new EmailServiceInstaller(HostingEnvironment.MapPath("~/Views/Emails")),
                    new EventMigratorInstaller(),
                    new EventStoreSessionInstaller(),
                    new MsmqInstaller(ConfigurationManager.AppSettings["TaskQueue"]),
                    new RavenInstaller(tenants),
                    new ServicesInstaller(),
                    new TaskHandlerInstaller(),
                    new CompositionRootInstaller(),
                    EventStoreInstaller.FromAssembly(
                        tenants,
                        Assembly.GetExecutingAssembly(),
                        DocumentStoreMode.Server));
        }

        if (ChildContainer == null)
        {
            ChildContainer = new WindsorContainer().Register(
                Component.For<IDocumentSession>().UsingFactoryMethod(kernel =>
                {
                    IDocumentSession documentSession = kernel.Resolve<IDocumentStore>()
                        .OpenSession();
                    documentSession.Advanced.UseOptimisticConcurrency = true;
                    return documentSession;
                }).LifestyleTransient());
            Container.AddChildContainer(ChildContainer);
        }

        DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
        configuration.DependencyResolver =
            new WindsorHttpDependencyResolver(Container.Kernel);
        configuration.Services.Replace(
            typeof(IHttpControllerSelector),
            new HttpNotFoundAwareDefaultHttpControllerSelector(configuration));
        configuration.Services.Replace(
            typeof(IHttpActionSelector),
            new HttpNotFoundAwareControllerActionSelector());
    }
}
