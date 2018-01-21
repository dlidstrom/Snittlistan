using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Queue;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class MsmqInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMsmqTransaction>()
                         .UsingFactoryMethod(k => MsmqGateway.AutoCommitScope())
                         .LifestylePerWebRequest());
        }
    }
}