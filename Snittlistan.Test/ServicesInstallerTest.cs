using Castle.Windsor;
using Snittlistan.Web.Infrastructure.Installers;
using Snittlistan.Web.Services;
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
            var service = container.Resolve<IAuthenticationService>();
            Assert.NotNull(service);
        }
    }
}