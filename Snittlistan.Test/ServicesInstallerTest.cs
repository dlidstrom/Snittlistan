using Castle.Windsor;
using Snittlistan.Installers;
using Snittlistan.Services;
using Xunit;

namespace Snittlistan.Test
{
	public class ServicesInstallerTest
	{
		private readonly IWindsorContainer container;

		public ServicesInstallerTest()
		{
			container = new WindsorContainer().Install(new ServicesInstaller());
		}

		[Fact]
		public void InstallsFormsAuthenticationService()
		{
			Assert.NotNull(container.Resolve<IAuthenticationService>());
		}

		[Fact]
		public void InstallsEmailService()
		{
			var handlers = InstallerTestHelper.GetHandlersFor(typeof(IEmailService), container);
			Assert.Equal(1, handlers.Length);
		}
	}
}
