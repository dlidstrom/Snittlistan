namespace Snittlistan.Test
{
    using Castle.Windsor;
    using NUnit.Framework;
    using Snittlistan.Web.Infrastructure.Installers;
    using Snittlistan.Web.Services;

    [TestFixture]
    public class ServicesInstallerTest
    {
        private readonly IWindsorContainer container;

        public ServicesInstallerTest()
        {
            container = new WindsorContainer().Install(new ServicesInstaller());
        }

        [Test]
        public void InstallsFormsAuthenticationService()
        {
            IAuthenticationService service = container.Resolve<IAuthenticationService>();
            Assert.NotNull(service);
        }
    }
}