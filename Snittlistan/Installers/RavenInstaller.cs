namespace Snittlistan.Installers
{
    using System;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using Raven.Client.MvcIntegration;
    using Snittlistan.Infrastructure.Indexes;

    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Component.For<IDocumentStore>().Instance(CreateDocumentStore()).LifestyleSingleton(),
                    Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifestylePerWebRequest());
        }

        private static IDocumentSession GetDocumentSession(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }

        private IDocumentStore CreateDocumentStore()
        {
            // run with server when debugging, and embedded in production
            IDocumentStore store = null;
            if (MvcApplication.IsDebug)
                store = new DocumentStore { ConnectionStringName = "RavenDB" };
            else
                store = new EmbeddableDocumentStore { DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() };

            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";

            // create indexes
            IndexCreator.CreateIndexes(store);
            RavenProfiler.InitializeFor(store);
            return store;
        }
    }
}