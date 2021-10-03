namespace Snittlistan.Queue.Messages
{
    using System;

    public class InitiateUpdateMailEvent
    {
        public InitiateUpdateMailEvent(string rosterId, Guid correlationId)
        {
            RosterId = rosterId;
            CorrelationId = correlationId;
        }

        public string RosterId { get; }

        public Guid CorrelationId { get; }
    }
}
