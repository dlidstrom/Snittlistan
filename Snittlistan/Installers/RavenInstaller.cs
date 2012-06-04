namespace Snittlistan.Installers
{
    using System;
    using System.IO;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Infrastructure.Indexes;
    using Models;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using Raven.Client.MvcIntegration;

    public class RavenInstaller : IWindsorInstaller
    {
        public static IDocumentStore InitializeStore(IDocumentStore store)
        {
            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Conventions.FindTypeTagName = type => type == typeof(Match8x4) ? "Matches" : DocumentConvention.DefaultTypeTagName(type);

            // create indexes
            IndexCreator.CreateIndexes(store);
            RavenProfiler.InitializeFor(store);
            return store;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().Instance(InitializeStore(CreateDocumentStore())).LifestyleSingleton(),
                Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifestylePerWebRequest());
        }

        private static IDocumentSession GetDocumentSession(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }

        private static IDocumentStore CreateDocumentStore()
        {
            // run with server when debugging, and embedded in production
            IDocumentStore store = null;
            if (MvcApplication.IsDebug)
                store = new DocumentStore { ConnectionStringName = "RavenDB" };
            else
            {
                store = new EmbeddableDocumentStore
                {
                    DataDirectory = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Database")
                };
            }

            return store;
        }
    }
}