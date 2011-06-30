using System.Linq;
using Castle.Core;
using Castle.Windsor;
using SnittListan.Events;
using SnittListan.Handlers;
using SnittListan.Installers;
using Xunit;
using Castle.MicroKernel.Registration;
using SnittListan.Services;
using Moq;

namespace SnittListan.Test
{
	public class HandlersInstallerTest
	{
		private readonly IWindsorContainer container;

		public HandlersInstallerTest()
		{
			container = new WindsorContainer().Install(new HandlersInstaller());
		}

		[Fact]
		public void InstallsHandlerForNewUserCreatedEvent()
		{
			var handler = InstallerTestHelper.GetHandlersFor(typeof(IHandle<NewUserCreatedEvent>), container);
			Assert.Equal(1, handler.Length);
		}

		[Fact]
		public void HandlersAreTransient()
		{
			var nonTransientControllers = InstallerTestHelper.GetHandlersFor(typeof(IHandle<>), container)
				.Where(c => c.ComponentModel.LifestyleType != LifestyleType.Transient)
				.ToArray();
			Assert.Empty(nonTransientControllers);
		}
	}
}
