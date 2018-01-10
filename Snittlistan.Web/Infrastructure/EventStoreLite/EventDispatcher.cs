using System;
using Castle.Windsor;
using EventStoreLite.Infrastructure;

// ReSharper disable once CheckNamespace
namespace EventStoreLite
{
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
            var type = typeof(IEventHandler<>).MakeGenericType(e.GetType());
            var handlers = container.ResolveAll(type);
            foreach (var handler in handlers)
            {
                handler.AsDynamic().Handle(e, aggregateId);
            }
        }
    }
}