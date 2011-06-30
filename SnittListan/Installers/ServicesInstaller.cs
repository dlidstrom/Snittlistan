using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SnittListan.Services;

namespace SnittListan.Installers
{
	public class ServicesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				AllTypes
					.FromThisAssembly()
					.Where(Component.IsInSameNamespaceAs<IEmailService>())
					.WithService.DefaultInterface()
					.Configure(c => c.LifeStyle.Transient));
		}
	}
}