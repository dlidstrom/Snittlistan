using Castle.Core;
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
        private readonly LifestyleType lifestyleType;

        public EventStoreSessionInstaller(LifestyleType lifestyleType)
        {
            this.lifestyleType = lifestyleType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IEventStoreSession>()
                    .UsingFactoryMethod(CreateEventStoreSession)
                    .LifeStyle.Is(lifestyleType));
        }

        private static IEventStoreSession CreateEventStoreSession(IKernel kernel)
        {
            return kernel.Resolve<EventStore>()
                .OpenSession(kernel.Resolve<IDocumentStore>(), kernel.Resolve<IDocumentSession>());
        }
    }
}