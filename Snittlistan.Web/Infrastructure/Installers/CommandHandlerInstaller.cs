#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Snittlistan.Web.Commands;

    public class CommandHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _ = container.Register(
                Classes.FromThisAssembly()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithServiceBase()
                    .LifestyleScoped());
        }
    }
}
