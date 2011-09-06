using System;

namespace Snittlistan.Events
{
	public class DomainEventReset : IDisposable
	{
		public void Dispose()
		{
			DomainEvent.Reset();
		}
	}
}