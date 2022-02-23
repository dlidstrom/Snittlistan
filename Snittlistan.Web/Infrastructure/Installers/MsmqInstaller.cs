
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Queue;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers;
public class MsmqInstaller : IWindsorInstaller
{
    private readonly string path;

    public MsmqInstaller(string path)
    {
        this.path = path;
    }

    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            Component.For<IMsmqTransaction>()
                     .UsingFactoryMethod(k => MsmqGateway.Create())
                     .LifestylePerWebRequest());
    }
}
