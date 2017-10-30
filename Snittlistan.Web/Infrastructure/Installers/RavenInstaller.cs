using System;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Snittlistan.Web.Areas.V1.Models;

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
                    mode = DocumentStoreMode.Server;
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

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var tenantConfigurations = container.ResolveAll<TenantConfiguration>();
            foreach (var tenantConfiguration in tenantConfigurations)
            {
                var documentStore = InitializeStore(CreateDocumentStore(tenantConfiguration.ConnectionStringName));
                var documentStoreComponent = Component.For<IDocumentStore>()
                                                      .Instance(documentStore)
                                                      .Named($"DocumentStore-{tenantConfiguration.Name}")
                                                      .LifestyleSingleton();
                container.Register(documentStoreComponent);
            }

            container.Register(
                Component.For<IDocumentSession>()
                         .UsingFactoryMethod(GetDocumentSession)
                         .LifeStyle.Is(mode == DocumentStoreMode.InMemory ? LifestyleType.Scoped : LifestyleType.PerWebRequest));
        }

        private static bool FindIdentityProperty(PropertyInfo property)
        {
            var attribute = Attribute.GetCustomAttribute(property, typeof(IdAttribute));
            return attribute != null || property.Name == "Id";
        }

        private static IDocumentSession GetDocumentSession(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            var tenantConfiguration = kernel.Resolve<TenantConfiguration>();
            var documentSession = store.OpenSession(tenantConfiguration.Database);
            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }

        private static IDocumentStore InitializeStore(IDocumentStore store)
        {
            store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            store.Conventions.FindTypeTagName = type => type == typeof(Match8x4) ? "Matches" : DocumentConvention.DefaultTypeTagName(type);
            store.Conventions.FindIdentityProperty = FindIdentityProperty;
            store.Conventions.MaxNumberOfRequestsPerSession = 1024;

            return store;
        }

        private IDocumentStore CreateDocumentStore(string connectionStringName)
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

                        store = documentStore;
                    }

                    break;

                case DocumentStoreMode.Server:
                    store = new DocumentStore { ConnectionStringName = connectionStringName };
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return store;
        }
    }
}