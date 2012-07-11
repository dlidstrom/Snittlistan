namespace Snittlistan.Infrastructure.Installers
{
    using System;
    using System.IO;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Indexes;
    using Models;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    public class RavenInstaller : IWindsorInstaller
    {
        private readonly DocumentStoreMode mode;

        /// <summary>
        /// Initializes a new instance of the RavenInstaller class.
        /// Raven mode is determined depending on debug or release:
        /// run with server when debugging, and embedded in production.
        /// </summary>
        public RavenInstaller()
        {
            mode = MvcApplication.IsDebugConfig ? DocumentStoreMode.Server : DocumentStoreMode.Embeddable;
        }

        /// <summary>
        /// Initializes a new instance of the RavenInstaller class.
        /// </summary>
        /// <param name="mode">Indicates which mode Raven will be run in.</param>
        public RavenInstaller(DocumentStoreMode mode)
        {
            this.mode = mode;
        }

        public static IDocumentStore InitializeStore(IDocumentStore store)
        {
            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Conventions.FindTypeTagName = type => type == typeof(Match8x4) ? "Matches" : DocumentConvention.DefaultTypeTagName(type);

            // create indexes
            IndexCreator.CreateIndexes(store);
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

        private IDocumentStore CreateDocumentStore()
        {
            IDocumentStore store;
            switch (mode)
            {
                case DocumentStoreMode.InMemory:
                    store = new EmbeddableDocumentStore { RunInMemory = true };
                    break;
                case DocumentStoreMode.Server:
                    store = new DocumentStore { ConnectionStringName = "RavenDB" };
                    break;
                default:
                    store = new EmbeddableDocumentStore
                                {
                                    DataDirectory = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Database")
                                };
                    break;
            }

            return store;
        }
    }
}