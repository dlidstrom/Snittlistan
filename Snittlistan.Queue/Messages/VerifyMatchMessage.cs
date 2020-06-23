namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchMessage
    {
        public VerifyMatchMessage(int bitsMatchId, string rosterId, bool force)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
            Force = force;
        }

        public int BitsMatchId { get; }

        public string RosterId { get; }

        public bool Force { get; }

        public override string ToString()
        {
            return $"VerifyMatch RosterId={RosterId} BitsMatchid={BitsMatchId} Force={Force}";
        }
    }
}
