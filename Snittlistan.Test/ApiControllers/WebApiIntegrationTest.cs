using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core;
using Castle.Windsor;
using EventStoreLite.IoC.Castle;
using NUnit.Framework;
using Raven.Client;
using Snittlistan.Web;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Infrastructure.IoC;

namespace Snittlistan.Test.ApiControllers
{
    public abstract class WebApiIntegrationTest
    {
        protected HttpClient Client { get; private set; }

        private IWindsorContainer Container { get; set; }

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            Container = new WindsorContainer();
            Container.Install(
                new ControllerInstaller(),
                new ApiControllerInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller(DocumentStoreMode.InMemory),
                new HandlersInstaller(),
                EventStoreInstaller.FromAssembly(typeof(MvcApplication).Assembly),
                new EventStoreSessionInstaller(LifestyleType.Scoped));
            OnSetUp(Container);

            MvcApplication.Bootstrap(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));

            Act();
        }

        [TearDown]
        public void TearDown()
        {
            MvcApplication.Shutdown();
        }

        protected virtual void Act()
        {
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            WaitForIndexing();

            using (var session = Container.Resolve<IDocumentStore>().OpenSession())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        protected virtual void OnSetUp(IWindsorContainer container)
        {
        }

        private void WaitForIndexing()
        {
            var documentStore = Container.Resolve<IDocumentStore>();
            const int Timeout = 15000;
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    while (sw.Elapsed.TotalMilliseconds < Timeout)
                    {
                        var s = documentStore.DatabaseCommands.GetStatistics()
                                             .StaleIndexes;
                        if (s.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500);
                    }
                });
            indexingTask.Wait(Timeout);
        }
    }
}