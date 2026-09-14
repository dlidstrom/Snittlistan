#nullable enable

using Snittlistan.Web.Infrastructure.Bits;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;

public class BitsClientInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(Component.For<IBitsClient>().ImplementedBy<BitsDbClient>());
    }
}
