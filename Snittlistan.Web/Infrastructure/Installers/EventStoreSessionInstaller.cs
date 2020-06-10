namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using EventStoreLite;
    using Raven.Client;

    public class EventStoreSessionInstaller : IWindsorInstaller
    {
        private readonly Func<ComponentRegistration<IEventStoreSession>, ComponentRegistration<IEventStoreSession>> func;

        public EventStoreSessionInstaller()
        {
            func = x => x.LifestylePerWebRequest();
        }

        public EventStoreSessionInstaller(LifestyleType lifestyleType)
        {
            func = x => x.LifeStyle.Is(lifestyleType);
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                func.Invoke(
                    Component.For<IEventStoreSession>()
                        .UsingFactoryMethod(CreateEventStoreSession)));
        }

        private static IEventStoreSession CreateEventStoreSession(IKernel kernel)
        {
            return kernel.Resolve<EventStore>()
                .OpenSession(kernel.Resolve<IDocumentStore>(), kernel.Resolve<IDocumentSession>());
        }
    }
}
