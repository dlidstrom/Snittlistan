
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;
public class ServicesInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Classes
                .FromThisAssembly()
                .Where(Component.IsInNamespace("Snittlistan.Web.Services"))
                .WithServiceDefaultInterfaces()
                .LifestyleTransient());
    }
}
