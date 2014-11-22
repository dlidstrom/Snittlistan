using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class BackgroundTaskHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(IBackgroundTaskHandler<>)).WithServiceAllInterfaces().LifestyleTransient());
        }
    }
}