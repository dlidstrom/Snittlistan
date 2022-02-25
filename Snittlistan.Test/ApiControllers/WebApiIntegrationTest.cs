#nullable enable

using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using Castle.Core;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EventStoreLite.IoC;
using Moq;
using NUnit.Framework;
using Postal;
using Raven.Client;
using Snittlistan.Queue;
using Snittlistan.Test.ApiControllers.Infrastructure;
using Snittlistan.Web;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Infrastructure.IoC;

namespace Snittlistan.Test.ApiControllers;

public abstract class WebApiIntegrationTest
{
    protected HttpClient Client { get; private set; } = null!;

    protected Databases Databases { get; private set; } = null!;

    protected IWindsorContainer Container { get; set; } = null!;

    protected Tenant CurrentTenant { get; set; } = null!;

    [SetUp]
    public async Task SetUp()
    {
        HttpConfiguration configuration = new();
        Container = new WindsorContainer();
        InMemoryContext inMemoryContext = new();
        Databases = new(inMemoryContext, inMemoryContext);
        CurrentTenant = new("TEST", "favicon", "touchicon", "touchiconsize", "title", 51538, "Hofvet");
        _ = Container
            .AddFacility<LoggingFacility>(x => x.LogUsing<TestLoggerFactory>())
            .Install(
                new ControllerInstaller(),
                new ApiControllerInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller(new[] { CurrentTenant }, DocumentStoreMode.InMemory),
                new TaskHandlerInstaller(),
                new CompositionRootInstaller(),
                CommandHandlerInstaller.Scoped(),
                new DatabaseContextInstaller(() => Databases, LifestyleType.Scoped),
                EventStoreInstaller.FromAssembly(new[] { CurrentTenant }, typeof(MvcApplication).Assembly, DocumentStoreMode.InMemory),
                new EventStoreSessionInstaller(LifestyleType.Scoped));
        _ = Container.Register(Component.For<IMsmqTransaction>().Instance(Mock.Of<IMsmqTransaction>()));
        _ = Container.Register(Component.For<IEmailService>().Instance(Mock.Of<IEmailService>()));
        HttpRequestBase requestMock =
            Mock.Of<HttpRequestBase>(x => x.ServerVariables == new NameValueCollection() { { "SERVER_NAME", "TEST" } });
        HttpContextBase httpContextMock =
            Mock.Of<HttpContextBase>(x =>
                x.Request == requestMock
                && x.Items == new Dictionary<object, object>()
                && x.Cache == new Cache());
        _ = inMemoryContext.Tenants.Add(CurrentTenant);
        CurrentHttpContext.Instance = () => httpContextMock;
        await OnSetUp(Container);

        MvcApplication.Bootstrap(Container, configuration, () => new(inMemoryContext, inMemoryContext));
        Client = new HttpClient(new HttpServer(configuration));
        OnlyLocalAllowedAttribute.SkipValidation = true;

        LoggingExceptionLogger.ExceptionHandler += ExceptionHandler;
        await Act();
    }

    [TearDown]
    public void TearDown()
    {
        MvcApplication.Shutdown();
    }

    protected virtual Task Act()
    {
        return Task.CompletedTask;
    }

    protected async Task Transact(Func<IDocumentSession, Task> action)
    {
        WaitForIndexing();

        using (IDocumentSession session = Container.Resolve<IDocumentStore>().OpenSession())
        {
            await action.Invoke(session);
            session.SaveChanges();
        }

        WaitForIndexing();
    }

    protected virtual Task OnSetUp(IWindsorContainer container)
    {
        return Task.CompletedTask;
    }

    private static void ExceptionHandler(object sender, Exception exception)
    {
        Assert.Fail(exception.Demystify().ToString());
    }

    private void WaitForIndexing()
    {
        IDocumentStore documentStore = Container.Resolve<IDocumentStore>();
        const int Timeout = 15000;
        Task indexingTask = Task.Factory.StartNew(
            () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalMilliseconds < Timeout)
                {
                    string[] s = documentStore.DatabaseCommands.GetStatistics()
                                         .StaleIndexes;
                    if (s.Length == 0)
                    {
                        break;
                    }

                    Task.Delay(500).Wait();
                }
            });
        _ = indexingTask.Wait(Timeout);
    }
}
