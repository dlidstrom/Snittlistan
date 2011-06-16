using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Embedded;

namespace SnittListan.Installers
{
	public class RavenInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
					Component.For<IDocumentStore>().Instance(CreateDocumentStore()).LifeStyle.Singleton,
					Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSession).LifeStyle.PerWebRequest);
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
				DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
			}.Initialize();

			return store;
		}
	}
}