namespace Snittlistan.Test
{
    using Castle.Windsor;
    using Infrastructure.Installers;
    using MvcContrib.TestHelper;
    using Services;
    using Xunit;

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
            container.Resolve<IAuthenticationService>().ShouldNotBeNull("Expected IAuthenticationService in container");
        }

        [Fact]
        public void InstallsEmailService()
        {
            var handlers = InstallerTestHelper.GetHandlersFor(typeof(IEmailService), container);
            handlers.Length.ShouldBe(1);
        }
    }
}