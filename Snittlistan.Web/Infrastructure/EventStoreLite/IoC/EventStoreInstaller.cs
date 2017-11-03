using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Installers;

// ReSharper disable once CheckNamespace
namespace EventStoreLite.IoC
{
    /// <summary>
    /// Installs the event store into a Castle Windsor container.
    /// </summary>
    public class EventStoreInstaller : IWindsorInstaller
    {
        private readonly IEnumerable<IEventHandler> handlers;
        private readonly IEnumerable<Type> handlerTypes;
        private readonly DocumentStoreMode mode;

        private EventStoreInstaller(IEnumerable<Type> handlerTypes, DocumentStoreMode mode)
        {
            if (handlerTypes == null) throw new ArgumentNullException(nameof(handlerTypes));
            this.handlerTypes = handlerTypes;
            this.mode = mode;
        }

        private EventStoreInstaller(IEnumerable<IEventHandler> handlers)
        {
            if (handlers == null) throw new ArgumentNullException(nameof(handlers));
            this.handlers = handlers;
        }

        /// <summary>
        /// Installs event handlers from the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly with event handlers.</param>
        /// <param name="mode">In memory or server mode.</param>
        /// <returns>Event store installer for Castle Windsor.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static EventStoreInstaller FromAssembly(Assembly assembly, DocumentStoreMode mode)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return new EventStoreInstaller(assembly.GetTypes(), mode);
        }

        /// <summary>
        /// Installs the specified event handler types.
        /// </summary>
        /// <param name="handlerTypes">Event handler types.</param>
        /// <returns>Event store installer for Castle Windsor.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static EventStoreInstaller FromHandlerTypes(IEnumerable<Type> handlerTypes)
        {
            if (handlerTypes == null) throw new ArgumentNullException(nameof(handlerTypes));
            return new EventStoreInstaller(handlerTypes, DocumentStoreMode.Server);
        }

        /// <summary>
        /// Installs the specified event handler instances.
        /// </summary>
        /// <param name="handlers">Event handler instances.</param>
        /// <returns>Event store installer for Castle Windsor.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static EventStoreInstaller FromHandlerInstances(IEnumerable<IEventHandler> handlers)
        {
            if (handlers == null) throw new ArgumentNullException(nameof(handlers));
            return new EventStoreInstaller(handlers);
        }

        /// <summary>
        /// Installs the event store and the handler types or instances to the specified container.
        /// </summary>
        /// <param name="container">Container instance.</param>
        /// <param name="store">Configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (mode == DocumentStoreMode.InMemory)
            {
                var documentStore = container.Resolve<IDocumentStore>();
                container.Register(Component.For<EventStore>().UsingFactoryMethod(x => CreateEventStore(documentStore, container)));
            }
            else
            {
                var tenantConfigurations = container.ResolveAll<TenantConfiguration>();
                foreach (var tenantConfiguration in tenantConfigurations)
                {
                    var documentStore = container.Resolve<IDocumentStore>($"DocumentStore-{tenantConfiguration.Name}");
                    container.Register(
                        Component.For<EventStore>()
                                 .UsingFactoryMethod(x => CreateEventStore(documentStore, container))
                                 .Named($"EventStore-{tenantConfiguration.Name}")
                                 .LifestyleSingleton());
                }
            }

            if (handlerTypes != null)
            {
                foreach (var type in handlerTypes.Where(x => x.IsClass && x.IsAbstract == false))
                {
                    RegisterEventTypes(container, type);
                }
            }

            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    RegisterEventTypes(container, handler.GetType(), handler);
                }
            }
        }

        private static void RegisterEventTypes(IWindsorContainer container, Type type, object instance = null)
        {
            var interfaces = type.GetInterfaces();
            foreach (var i in interfaces.Where(x => x.IsGenericType))
            {
                var genericTypeDefinition = i.GetGenericTypeDefinition();
                if (!typeof(IEventHandler<>).IsAssignableFrom(genericTypeDefinition)) continue;
                var genericArguments = string.Join(
                    ", ", i.GetGenericArguments().Select(x => x.ToString()));
                var registration =
                    Component.For(i)
                             .Named($"{type.FullName}<{genericArguments}>");
                if (instance != null) registration.Instance(instance);
                else
                {
                    registration.ImplementedBy(type).LifestyleTransient();
                }

                container.Register(registration);
            }
        }

        private EventStore CreateEventStore(IDocumentStore documentStore, IWindsorContainer container)
        {
            var eventStore = new EventStore(container);
            if (handlerTypes != null)
            {
                eventStore.SetReadModelTypes(handlerTypes);
            }
            else
            {
                eventStore.SetReadModelTypes(handlers.Select(x => x.GetType()));
            }

            eventStore.Initialize(documentStore);
            return eventStore;
        }
    }
}