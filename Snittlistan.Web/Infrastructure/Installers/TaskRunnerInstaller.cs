using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class TaskRunnerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<TaskRunner>()
                         .LifestyleSingleton()
                         .DependsOn(Dependency.OnAppSettingsValue("taskRunnerPollingInterval", "TaskRunnerPollingInterval"))
                         .Start());
        }
    }
}