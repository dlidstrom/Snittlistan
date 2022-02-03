#nullable enable

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.TaskHandlers;

namespace Snittlistan.Web.Infrastructure.Installers;

public class TaskHandlerInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            Classes.FromThisAssembly()
                .BasedOn(typeof(ITaskHandler<>))
                .WithServiceBase()
                .LifestyleScoped());
    }
}
