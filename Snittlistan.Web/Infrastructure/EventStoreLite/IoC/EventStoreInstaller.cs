using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Installers;

#nullable enable

namespace EventStoreLite.IoC;
/// <summary>
/// Installs the event store into a Castle Windsor container.
/// </summary>
public class EventStoreInstaller : IWindsorInstaller
{
    private readonly IEnumerable<Type> handlerTypes;
    private readonly DocumentStoreMode mode;
    private readonly Tenant[] tenants;

    private EventStoreInstaller(
        Tenant[] tenants,
        IEnumerable<Type> handlerTypes,
        DocumentStoreMode mode)
    {
        this.tenants = tenants;
        this.handlerTypes = handlerTypes;
        this.mode = mode;
    }

    /// <summary>
    /// Installs event handlers from the specified assembly.
    /// </summary>
    /// <param name="assembly">Assembly with event handlers.</param>
    /// <param name="mode">In memory or server mode.</param>
    /// <returns>Event store installer for Castle Windsor.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static EventStoreInstaller FromAssembly(Tenant[] tenants, Assembly assembly, DocumentStoreMode mode)
    {
        return assembly == null
            ? throw new ArgumentNullException(nameof(assembly))
            : new EventStoreInstaller(tenants, assembly.GetTypes(), mode);
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
            foreach (Tenant tenant in tenants)
            {
                IDocumentStore documentStore = container.Resolve<IDocumentStore>($"DocumentStore-{tenant.Hostname}");
                _ = container.Register(
                    Component.For<EventStore>()
                             .UsingFactoryMethod(x => CreateEventStore(documentStore, container))
                             .Named($"EventStore-{tenant.Hostname}")
                             .LifestyleSingleton());
            }
        }

        foreach (Type type in handlerTypes.Where(x => x.IsClass && x.IsAbstract == false))
        {
            RegisterEventTypes(container, type);
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
        _ = eventStore.SetReadModelTypes(handlerTypes);
        _ = eventStore.Initialize(documentStore);
        return eventStore;
    }
}
