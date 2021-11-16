#nullable enable

namespace Snittlistan.Test.ApiControllers
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
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
                new DatabaseContextInstaller(() => new(inMemoryContext, inMemoryContext)),
                EventStoreInstaller.FromAssembly(typeof(MvcApplication).Assembly, DocumentStoreMode.InMemory),
                new EventStoreSessionInstaller(LifestyleType.Scoped));
            _ = Container.Register(Component.For<IMsmqTransaction>().Instance(Mock.Of<IMsmqTransaction>()));
            await OnSetUp(Container);

            MvcApplication.Bootstrap(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));
            OnlyLocalAllowedAttribute.SkipValidation = true;

            Task.Run(async () => await Act()).Wait();
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

        protected void Transact(Action<IDocumentSession> action)
        {
            WaitForIndexing();

            using (IDocumentSession session = Container.Resolve<IDocumentStore>().OpenSession())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        protected virtual Task OnSetUp(IWindsorContainer container)
        {
            return Task.CompletedTask;
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
