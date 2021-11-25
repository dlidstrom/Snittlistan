#nullable enable

namespace Snittlistan.Test.ApiControllers
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Castle.Core;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using EventStoreLite.IoC;
    using Moq;
    using NUnit.Framework;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Test.ApiControllers.Infrastructure;
    using Snittlistan.Web;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.Infrastructure.Installers;
    using Snittlistan.Web.Infrastructure.IoC;

    public abstract class WebApiIntegrationTest
    {
        protected HttpClient Client { get; private set; } = null!;

        protected Databases Databases { get; private set; } = null!;

        private IWindsorContainer Container { get; set; } = null!;

        [SetUp]
        public async Task SetUp()
        {
            HttpConfiguration configuration = new();
            Container = new WindsorContainer();
            InMemoryContext inMemoryContext = new();
            _ = Container.Install(
                new ControllerInstaller(),
                new ApiControllerInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller(DocumentStoreMode.InMemory),
                new TaskHandlerInstaller(),
                new DatabaseContextInstaller(() => new(inMemoryContext, inMemoryContext), LifestyleType.Scoped),
                EventStoreInstaller.FromAssembly(typeof(MvcApplication).Assembly, DocumentStoreMode.InMemory),
                new EventStoreSessionInstaller(LifestyleType.Scoped));
            _ = Container.Register(Component.For<IMsmqTransaction>().Instance(Mock.Of<IMsmqTransaction>()));
            HttpRequestBase requestMock =
                Mock.Of<HttpRequestBase>(x => x.ServerVariables == new NameValueCollection() { { "SERVER_NAME", "TEST" } });
            HttpContextBase httpContextMock =
                Mock.Of<HttpContextBase>(x => x.Request == requestMock);
            _ = inMemoryContext.Tenants.Add(new("TEST", "favicon", "touchicon", "touchiconsize", "title", 51538));
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
}
