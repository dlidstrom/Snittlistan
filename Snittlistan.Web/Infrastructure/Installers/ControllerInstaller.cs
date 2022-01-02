
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers;
public class ControllerInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            Classes
                .FromThisAssembly()
                .BasedOn<IController>()
                .If(t => t.Name.EndsWith("Controller"))
                .Configure(c => c.Named(c.Implementation.Name)).LifestyleTransient());
    }
}
