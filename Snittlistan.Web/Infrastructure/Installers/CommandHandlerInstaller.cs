#nullable enable

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;

public class CommandHandlerInstaller : IWindsorInstaller
{
    private readonly Func<BasedOnDescriptor, BasedOnDescriptor> func;

    private CommandHandlerInstaller(Func<BasedOnDescriptor, BasedOnDescriptor> func)
    {
        this.func = func;
    }

    public static IWindsorInstaller PerWebRequest()
    {
        return new CommandHandlerInstaller(x => x.LifestylePerWebRequest());
    }

    public static IWindsorInstaller Scoped()
    {
        return new CommandHandlerInstaller(x => x.LifestyleScoped());
    }

    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            func.Invoke(
                Classes.FromThisAssembly()
                    .BasedOn(typeof(ExternalCommands.ICommandHandler<>))
                    .WithServiceBase()));

        _ = container.Register(
            func.Invoke(
                Classes.FromThisAssembly()
                    .BasedOn(typeof(Commands.ICommandHandler<>))
                    .WithServiceBase()
                    .LifestyleScoped()));
    }
}
