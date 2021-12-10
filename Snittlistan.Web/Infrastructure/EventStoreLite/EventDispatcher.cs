using Castle.Windsor;
using EventStoreLite.Infrastructure;

#nullable enable

namespace EventStoreLite;
/// <summary>
/// Used to dispatch events to event handlers.
/// </summary>
internal class EventDispatcher
{
    private readonly IWindsorContainer container;

    public EventDispatcher(IWindsorContainer container)
    {
        this.container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public void Dispatch(IDomainEvent e, string aggregateId)
    {
        Type type = typeof(IEventHandler<>).MakeGenericType(e.GetType());
        Array handlers = container.ResolveAll(type);
        foreach (object handler in handlers)
        {
            handler.AsDynamic()!.Handle(e, aggregateId);
        }
    }
}
