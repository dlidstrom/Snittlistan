#nullable enable

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;

public class CompositionRootInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(Component.For<CompositionRoot>().LifestyleTransient());
    }
}
