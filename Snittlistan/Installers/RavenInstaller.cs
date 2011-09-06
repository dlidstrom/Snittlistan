using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace Snittlistan.Installers
{
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
			var store = new EmbeddableDocumentStore
			{
				DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),
#if DEBUG
				UseEmbeddedHttpServer = true
#endif
			}.Initialize();

			store.Conventions.IdentityPartsSeparator = "-";
			return store;
		}
	}
}