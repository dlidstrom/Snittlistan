#nullable enable

namespace Snittlistan.Queue.Messages
{
    using System;

    public class SendUpdateMailTask : ITask
    {
        public SendUpdateMailTask(string rosterId, string playerId, Guid correlationId)
        {
            RosterId = rosterId;
            PlayerId = playerId;
            CorrelationId = correlationId;
        }

        public string RosterId { get; }

        public string PlayerId { get; }

        public Guid CorrelationId { get; }

        public BusinessKey BusinessKey => new(GetType(), $"{RosterId}/{PlayerId}");
    }
}
