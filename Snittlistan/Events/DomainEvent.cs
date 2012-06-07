namespace Snittlistan.Events
{
    using System;
    using Castle.Windsor;
    using Handlers;

    /// <summary>
    /// Used to raise events within the domain model.
    /// </summary>
    public static class DomainEvent
    {
        /// <summary>
        /// Initializes static members of the DomainEvent class.
        /// </summary>
        static DomainEvent()
        {
            ReturnContainer = DefaultReturnContainer;
        }

        /// <summary>
        /// Gets or sets a function that supplies the container.
        /// Can be used to set a container for tests.
        /// </summary>
        public static Func<IWindsorContainer> ReturnContainer { get; set; }

        /// <summary>
        /// Gets or sets an action used to raise events.
        /// Used for testing purposes.
        /// </summary>
        public static Action<IDomainEvent> RaiseAction { get; set; }

        /// <summary>
        /// Raises a domain event. Default action is to resolve all handlers
        /// and let them handle the event.
        /// </summary>
        /// <typeparam name="TEvent">Type of event.</typeparam>
        /// <param name="event">Event to raise.</param>
        public static void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            if (RaiseAction != null)
            {
                RaiseAction(@event);
                return;
            }

            var container = ReturnContainer();
            var handlers = container.ResolveAll<IHandle<TEvent>>();

            foreach (var handle in handlers)
            {
                handle.Handle(@event);
                container.Release(handle);
            }
        }

        /// <summary>
        /// Used for testing purposes.
        /// </summary>
        /// <param name="raiseAction">Custom raise action.</param>
        /// <returns>Domain reset event to go back to default behaviour.</returns>
        public static DomainEventReset TestWith(Action<IDomainEvent> raiseAction)
        {
            RaiseAction = raiseAction;

            return new DomainEventReset();
        }

        /// <summary>
        /// Resets the raise action to default behaviour.
        /// </summary>
        public static void Reset()
        {
            RaiseAction = null;
        }

        /// <summary>
        /// Used for testing purposes. Use within a using-scope:
        /// <code>
        /// using (DomainEvent.Disable())
        /// { ... }
        /// </code>
        /// No events will be raised within the using-scope.
        /// </summary>
        /// <returns>Event reset to default behaviour.</returns>
        public static DomainEventReset Disable()
        {
            RaiseAction = e => { };

            return new DomainEventReset();
        }

        /// <summary>
        /// Returns the default container.
        /// </summary>
        /// <returns>The global container.</returns>
        private static IWindsorContainer DefaultReturnContainer()
        {
            return MvcApplication.Container;
        }
    }
}