using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Services;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                    .FromThisAssembly()
                    .Where(Component.IsInSameNamespaceAs<IEmailService>())
                    .WithServiceDefaultInterfaces()
                    .LifestyleTransient());
        }
    }
}