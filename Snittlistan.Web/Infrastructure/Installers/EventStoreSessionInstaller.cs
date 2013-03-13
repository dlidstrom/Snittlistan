using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EventStoreLite;
using Raven.Client;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class EventStoreSessionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IEventStoreSession>()
                    .UsingFactoryMethod(CreateEventStoreSession)
                    .LifestylePerWebRequest());
        }

        private static IEventStoreSession CreateEventStoreSession(IKernel kernel)
        {
            return kernel.Resolve<EventStore>()
                .OpenSession(kernel.Resolve<IDocumentStore>(), kernel.Resolve<IDocumentSession>());
        }
    }
}