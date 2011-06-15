using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace SnittListan.Installers
{
	public class ControllerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
					AllTypes
						.FromThisAssembly()
						.BasedOn<IController>()
						.Configure(c => c.LifeStyle.Transient.Named(c.Implementation.Name))
				);
		}
	}
}