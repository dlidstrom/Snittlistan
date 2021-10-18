namespace Snittlistan.Queue.Messages
{
    using System;

    public class VerifyMatchTask
    {
        public VerifyMatchTask(int bitsMatchId, string rosterId, bool force, Guid correlationId)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
            Force = force;
            CorrelationId = correlationId;
        }

        public int BitsMatchId { get; }

        public string RosterId { get; }

        public bool Force { get; }

        public Guid CorrelationId { get; }

        public override string ToString()
        {
            return $"VerifyMatch RosterId={RosterId} BitsMatchid={BitsMatchId} Force={Force}";
        }
    }
}
