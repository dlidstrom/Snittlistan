using System;
using Castle.Windsor;
using SnittListan.Handlers;

namespace SnittListan.Events
{
	public static class DomainEvent
	{
		static DomainEvent()
		{
			ReturnContainer = DefaultReturnContainer;
		}

		public static Func<IWindsorContainer> ReturnContainer { get; set; }
		public static Action<IDomainEvent> RaiseAction { get; set; }

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

		public static DomainEventReset TestWith(Action<IDomainEvent> raiseAction)
		{
			RaiseAction = raiseAction;

			return new DomainEventReset();
		}

		public static void Reset()
		{
			RaiseAction = e => { };
		}

		public static DomainEventReset Disable()
		{
			Reset();

			return new DomainEventReset();
		}

		private static IWindsorContainer DefaultReturnContainer()
		{
			return MvcApplication.Container;
		}
	}
}