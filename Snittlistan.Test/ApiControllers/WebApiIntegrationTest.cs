using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core;
using Castle.Windsor;
using EventStoreLite.IoC.Castle;
using Raven.Client;
using Snittlistan.Web;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Infrastructure.IoC;

namespace Snittlistan.Test.ApiControllers
{
    public abstract class WebApiIntegrationTest : IDisposable
    {
        protected WebApiIntegrationTest()
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

            MvcApplication.Bootstrap(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));
        }

        protected HttpClient Client { get; private set; }

        private IWindsorContainer Container { get; set; }

        public void Dispose()
        {
            MvcApplication.Shutdown();
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            using (var session = Container.Resolve<IDocumentStore>().OpenSession())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        protected virtual void SetUp(Action<IWindsorContainer> action)
        {
            action.Invoke(Container);
        }

        private void WaitForIndexing()
        {
            var documentStore = Container.Resolve<IDocumentStore>();
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var s = documentStore.DatabaseCommands.GetStatistics().StaleIndexes;
                        if (s.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500);
                    }
                });
            indexingTask.Wait(15000);
        }
    }
}