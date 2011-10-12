using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Snittlistan.Infrastructure.Indexes;

namespace Snittlistan.Installers
{
	public class RavenInstaller : IWindsorInstaller
	{
#if DEBUG
		private bool runningInDebug = true;
#else
		private static bool runningInDebug = false;
#endif

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
			if (runningInDebug)
				store = new DocumentStore { ConnectionStringName = "RavenDB" };
			else
				store = new EmbeddableDocumentStore { DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() };

			store.Initialize();
			store.Conventions.IdentityPartsSeparator = "-";
			IndexCreation.CreateIndexes(typeof(Matches_PlayerStats).Assembly, store);
			return store;
		}
	}
}