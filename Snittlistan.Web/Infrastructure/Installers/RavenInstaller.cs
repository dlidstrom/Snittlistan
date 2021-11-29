#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using System.Reflection;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using NLog;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using Snittlistan.Web.Infrastructure.Database;

    public class RavenInstaller : IWindsorInstaller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly DocumentStoreMode mode;
        private readonly Tenant[] tenants;

        public RavenInstaller(Tenant[] tenants)
        {
            mode = MvcApplication.Mode switch
            {
                ApplicationMode.Debug => DocumentStoreMode.Server,
                ApplicationMode.Release => DocumentStoreMode.Server,
                ApplicationMode.Test => DocumentStoreMode.InMemory,
                _ => throw new ArgumentOutOfRangeException(),
            };
            this.tenants = tenants;
        }

        public RavenInstaller(Tenant[] tenants, DocumentStoreMode mode)
        {
            this.tenants = tenants;
            this.mode = mode;
        }

        public void Install(IWindsorContainer container, IConfigurationStore configurationStore)
        {
            if (mode == DocumentStoreMode.InMemory)
            {
                EmbeddableDocumentStore store = new()
                {
                    RunInMemory = true
                };
                store.Configuration.Storage.Voron.AllowOn32Bits = true;
                _ = container.Register(Component.For<IDocumentStore>().Instance(store.Initialize()));
#pragma warning disable 618
                store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
#pragma warning restore 618
            }
            else
            {
                foreach (Tenant tenant in tenants)
                {
                    IDocumentStore documentStore = InitializeStore(CreateDocumentStore(tenant));
                    string nameOfComponent = $"DocumentStore-{tenant.Hostname}";
                    Log.Info($"Registering document store named {nameOfComponent}");
                    ComponentRegistration<IDocumentStore> documentStoreComponent = Component.For<IDocumentStore>()
                        .Instance(documentStore)
                        .Named(nameOfComponent)
                        .LifestyleSingleton();
                    _ = container.Register(documentStoreComponent);
                }
            }

            if (mode == DocumentStoreMode.InMemory)
            {
                _ = container.Register(
                    Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifestyleScoped());
            }
            else
            {
                _ = container.Register(
                    Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifestylePerWebRequest());
            }
        }

        private static bool FindIdentityProperty(MemberInfo memberInfo)
        {
            Attribute attribute = Attribute.GetCustomAttribute(memberInfo, typeof(IdAttribute));
            return attribute != null || memberInfo.Name == "Id";
        }

        private IDocumentSession GetDocumentSession(IKernel kernel)
        {
            // document store is resolved depending on hostname
            IDocumentStore store = kernel.Resolve<IDocumentStore>();
            IDocumentSession documentSession;
            if (mode == DocumentStoreMode.InMemory)
            {
                documentSession = store.OpenSession();
            }
            else
            {
                documentSession = store.OpenSession();
            }

            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }

        private static IDocumentStore InitializeStore(IDocumentStore store)
        {
            _ = store.Initialize();
            store.Conventions.IdentityPartsSeparator = "-";
#pragma warning disable 618
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
#pragma warning restore 618
            store.Conventions.FindIdentityProperty = FindIdentityProperty;
            store.Conventions.MaxNumberOfRequestsPerSession = 1024;

            return store;
        }

        private IDocumentStore CreateDocumentStore(Tenant tenant)
        {
            IDocumentStore store;
            switch (mode)
            {
                case DocumentStoreMode.InMemory:
                    {
                        EmbeddableDocumentStore documentStore = new()
                        {
                            RunInMemory = true
                        };

                        store = documentStore;
                    }

                    break;

                case DocumentStoreMode.Server:
                    store = new DocumentStore
                    {
                        Url = "http://localhost:8080",
                        DefaultDatabase = tenant.DatabaseName
                    };
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return store;
        }
    }
}
