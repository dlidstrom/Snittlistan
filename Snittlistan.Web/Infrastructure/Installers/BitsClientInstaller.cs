using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Areas.V2.Controllers.Api;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class BitsClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBitsClient>().ImplementedBy<BitsClient>());
        }
    }
}