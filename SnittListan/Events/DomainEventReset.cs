using System;

namespace SnittListan.Events
{
	public class DomainEventReset : IDisposable
	{
		public void Dispose()
		{
			DomainEvent.Reset();
		}
	}
}