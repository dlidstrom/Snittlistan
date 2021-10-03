namespace Snittlistan.Queue.Messages
{
    using System;

    public class SendUpdateMailEvent
    {
        public SendUpdateMailEvent(string rosterId, string playerId, Guid correlationId)
        {
            RosterId = rosterId;
            PlayerId = playerId;
            CorrelationId = correlationId;
        }

        public string RosterId { get; }

        public string PlayerId { get; }

        public Guid CorrelationId { get; }
    }
}
