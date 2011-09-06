using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Events;
using Snittlistan.Handlers;

namespace Snittlistan.Installers
{
	public class HandlersInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				AllTypes.FromThisAssembly()
				.BasedOn(typeof(IHandle<>))
				.If(Component.IsInSameNamespaceAs<SendRegistrationEmailHandler>())
				.Configure(c => c.LifestyleTransient()));
		}
	}
}