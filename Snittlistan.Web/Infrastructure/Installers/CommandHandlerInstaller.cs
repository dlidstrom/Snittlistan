#nullable enable

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;

public class CommandHandlerInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            Classes.FromThisAssembly()
                .BasedOn(typeof(ExternalCommands.ICommandHandler<>))
                .WithServiceBase()
                .LifestyleScoped());

        _ = container.Register(
            Classes.FromThisAssembly()
                .BasedOn(typeof(Commands.ICommandHandler<>))
                .WithServiceBase()
                .LifestyleScoped());
    }
}
