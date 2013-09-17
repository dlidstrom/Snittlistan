using System;
using System.IO;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Web.Infrastructure.Installers
{
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
            switch (MvcApplication.Mode)
            {
                case ApplicationMode.Debug:
                    mode = DocumentStoreMode.Server;
                    break;

                case ApplicationMode.Release:
                    mode = DocumentStoreMode.Embeddable;
                    break;

                case ApplicationMode.Test:
                    mode = DocumentStoreMode.InMemory;
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the RavenInstaller class.
        /// </summary>
        /// <param name="mode">Indicates which mode Raven will be run in.</param>
        public RavenInstaller(DocumentStoreMode mode)
        {
            this.mode = mode;
        }

        public static IDocumentStore InitializeStore(IDocumentStore store, DocumentStoreMode mode)
        {
            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Conventions.FindTypeTagName = type => type == typeof(Match8x4) ? "Matches" : DocumentConvention.DefaultTypeTagName(type);
            store.Conventions.FindIdentityProperty = FindIdentityProperty;
            store.Conventions.MaxNumberOfRequestsPerSession = 1024;

            // create indexes
            IndexCreator.CreateIndexes(store);
            return store;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().Instance(InitializeStore(CreateDocumentStore(), mode)).LifestyleSingleton(),
                Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifestylePerWebRequest());
        }

        private static bool FindIdentityProperty(PropertyInfo property)
        {
            var attribute = Attribute.GetCustomAttribute(property, typeof(IdAttribute));
            return attribute != null || property.Name == "Id";
        }

        private static IDocumentSession GetDocumentSession(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            var documentSession = store.OpenSession();
            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }

        private static void Configure(EmbeddableDocumentStore documentStore)
        {
            // limit memory usage when running embedded mode
            documentStore.Configuration.MemoryCacheLimitMegabytes = 256;

            //documentStore.Configuration.MemoryCacheLimitPercentage = 10;
            //documentStore.Configuration.Settings["Raven/Esent/CacheSizeMax"] = "256";
            //documentStore.Configuration.Settings["Raven/Esent/MaxVerPages"] = "512";
        }

        private IDocumentStore CreateDocumentStore()
        {
            IDocumentStore store;
            switch (mode)
            {
                case DocumentStoreMode.InMemory:
                    {
                        var documentStore = new EmbeddableDocumentStore
                        {
                            RunInMemory = true
                        };

                        Configure(documentStore);
                        store = documentStore;
                    }

                    break;

                case DocumentStoreMode.Server:
                    store = new DocumentStore { ConnectionStringName = "RavenDB" };
                    break;

                default:
                    {
                        var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                        var dataDirectory = Path.Combine(path, "Database");
                        var documentStore = new EmbeddableDocumentStore
                        {
                            DataDirectory = dataDirectory
                        };

                        Configure(documentStore);
                        store = documentStore;
                    }
                    break;
            }

            return store;
        }
    }
}