#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Snittlistan.Queue;

    public class MsmqInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _ = container.Register(
                Component.For<IMsmqTransaction>()
                         .UsingFactoryMethod(k => MsmqGateway.AutoCommitScope())
                         .LifestylePerWebRequest());
        }
    }
}
