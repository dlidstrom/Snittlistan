#nullable enable

// ReSharper disable once CheckNamespace
namespace EventStoreLite.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Raven.Client;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure.Installers;

    /// <summary>
    /// Installs the event store into a Castle Windsor container.
    /// </summary>
    public class EventStoreInstaller : IWindsorInstaller
    {
        private readonly IEnumerable<IEventHandler>? handlers;
        private readonly IEnumerable<Type>? handlerTypes;
        private readonly DocumentStoreMode mode;

        private EventStoreInstaller(IEnumerable<Type> handlerTypes, DocumentStoreMode mode)
        {
            this.handlerTypes = handlerTypes ?? throw new ArgumentNullException(nameof(handlerTypes));
            this.mode = mode;
        }

        private EventStoreInstaller(IEnumerable<IEventHandler> handlers)
        {
            this.handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
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
            return assembly == null
                ? throw new ArgumentNullException(nameof(assembly))
                : new EventStoreInstaller(assembly.GetTypes(), mode);
        }

        /// <summary>
        /// Installs the specified event handler types.
        /// </summary>
        /// <param name="handlerTypes">Event handler types.</param>
        /// <returns>Event store installer for Castle Windsor.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static EventStoreInstaller FromHandlerTypes(IEnumerable<Type> handlerTypes)
        {
            return handlerTypes == null
                ? throw new ArgumentNullException(nameof(handlerTypes))
                : new EventStoreInstaller(handlerTypes, DocumentStoreMode.Server);
        }

        /// <summary>
        /// Installs the specified event handler instances.
        /// </summary>
        /// <param name="handlers">Event handler instances.</param>
        /// <returns>Event store installer for Castle Windsor.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static EventStoreInstaller FromHandlerInstances(IEnumerable<IEventHandler> handlers)
        {
            return handlers == null
                ? throw new ArgumentNullException(nameof(handlers))
                : new EventStoreInstaller(handlers);
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
                IDocumentStore documentStore = container.Resolve<IDocumentStore>();
                _ = container.Register(Component.For<EventStore>().UsingFactoryMethod(x => CreateEventStore(documentStore, container)));
            }
            else
            {
                TenantConfiguration[] tenantConfigurations = container.ResolveAll<TenantConfiguration>();
                foreach (TenantConfiguration tenantConfiguration in tenantConfigurations)
                {
                    IDocumentStore documentStore = container.Resolve<IDocumentStore>($"DocumentStore-{tenantConfiguration.Hostname}");
                    _ = container.Register(
                        Component.For<EventStore>()
                                 .UsingFactoryMethod(x => CreateEventStore(documentStore, container))
                                 .Named($"EventStore-{tenantConfiguration.Hostname}")
                                 .LifestyleSingleton());
                }
            }

            if (handlerTypes != null)
            {
                foreach (Type type in handlerTypes.Where(x => x.IsClass && x.IsAbstract == false))
                {
                    RegisterEventTypes(container, type);
                }
            }

            if (handlers != null)
            {
                foreach (IEventHandler handler in handlers)
                {
                    RegisterEventTypes(container, handler.GetType(), handler);
                }
            }
        }

        private static void RegisterEventTypes(IWindsorContainer container, Type type, object? instance = null)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type i in interfaces.Where(x => x.IsGenericType))
            {
                Type genericTypeDefinition = i.GetGenericTypeDefinition();
                if (!typeof(IEventHandler<>).IsAssignableFrom(genericTypeDefinition))
                {
                    continue;
                }

                string genericArguments = string.Join(
                    ", ", i.GetGenericArguments().Select(x => x.ToString()));
                ComponentRegistration<object> registration =
                    Component.For(i)
                             .Named($"{type.FullName}<{genericArguments}>");
                if (instance != null)
                {
                    _ = registration.Instance(instance);
                }
                else
                {
                    _ = registration.ImplementedBy(type).LifestyleTransient();
                }

                _ = container.Register(registration);
            }
        }

        private EventStore CreateEventStore(IDocumentStore documentStore, IWindsorContainer container)
        {
            EventStore eventStore = new(container);
            if (handlerTypes != null)
            {
                _ = eventStore.SetReadModelTypes(handlerTypes);
            }
            else
            {
                _ = eventStore.SetReadModelTypes(handlers.Select(x => x.GetType()));
            }

            _ = eventStore.Initialize(documentStore);
            return eventStore;
        }
    }
}
