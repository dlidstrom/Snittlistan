using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using SnittListan.Handlers;

namespace SnittListan.Events
{
	public static class DomainEvent
	{
		static DomainEvent()
		{
			RaiseAction = DefaultRaiseAction;
		}

		public static Func<IWindsorContainer> ReturnContainer { get; set; }
		public static Action<IDomainEvent> RaiseAction { get; set; }

		public static void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
		{
			RaiseAction(@event);
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

		private static void DefaultRaiseAction<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
		{
			var container = ReturnContainer();
			var handlers = container.ResolveAll<IHandle<TEvent>>();

			foreach (var handle in handlers)
			{
				handle.Handle(@event);
				container.Release(handle);
			}
		}
	}
}