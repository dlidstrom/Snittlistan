namespace Snittlistan.Web.Events
{
    using System;

    /// <summary>
    /// Used by DomainEvent for testing purposes.
    /// </summary>
    public class DomainEventReset : IDisposable
    {
        /// <summary>
        /// Resets the DomainEvent class to default behaviour.
        /// </summary>
        public void Dispose()
        {
            DomainEvent.Reset();
        }
    }
}