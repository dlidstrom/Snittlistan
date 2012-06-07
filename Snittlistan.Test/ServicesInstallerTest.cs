namespace Snittlistan.Test
{
    using Castle.Windsor;
    using Installers;
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
            Assert.NotNull(container.Resolve<IAuthenticationService>());
        }

        [Fact]
        public void InstallsEmailService()
        {
            var handlers = InstallerTestHelper.GetHandlersFor(typeof(IEmailService), container);
            handlers.Length.ShouldBe(1);
        }
    }
}