using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EventStoreLite;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class EventMigratorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn<IEventMigrator>()
                    .WithServiceFromInterface(typeof(IEventMigrator))
                    .LifestyleTransient());
        }
    }
}