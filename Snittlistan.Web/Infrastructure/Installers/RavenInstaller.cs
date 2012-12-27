namespace Snittlistan.Web.Infrastructure.Installers
{
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
                    this.mode = DocumentStoreMode.Server;
                    break;
                case ApplicationMode.Release:
                    this.mode = DocumentStoreMode.Embeddable;
                    break;
                case ApplicationMode.Test:
                    this.mode = DocumentStoreMode.InMemory;
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

        public static IDocumentStore InitializeStore(IDocumentStore store)
        {
            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Conventions.FindTypeTagName = type => type == typeof(Match8x4) ? "Matches" : DocumentConvention.DefaultTypeTagName(type);
            store.Conventions.FindIdentityProperty = FindIdentityProperty;

            // create indexes
            IndexCreator.CreateIndexes(store);
            return store;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().Instance(InitializeStore(this.CreateDocumentStore())).LifestyleSingleton(),
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
            return store.OpenSession();
        }

        private IDocumentStore CreateDocumentStore()
        {
            IDocumentStore store;
            switch (this.mode)
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