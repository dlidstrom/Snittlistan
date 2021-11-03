namespace Snittlistan.Queue.Messages
{
    using System;

    public class VerifyMatchTask : ITask
    {
        public VerifyMatchTask(int bitsMatchId, string rosterId, bool force)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
            Force = force;
        }

        public int BitsMatchId { get; }

        public string RosterId { get; }

        public bool Force { get; }

        public BusinessKey BusinessKey => new(GetType(), $"{RosterId}/{BitsMatchId}");

        public override string ToString()
        {
            return $"VerifyMatch RosterId={RosterId} BitsMatchid={BitsMatchId} Force={Force}";
        }
    }
}
