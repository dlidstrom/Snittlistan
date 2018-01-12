namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchMessage
    {
        public VerifyMatchMessage(int bitsMatchId, string rosterId)
        {
            BitsMatchId = bitsMatchId;
            RosterId = rosterId;
        }

        public int BitsMatchId { get; }

        public string RosterId { get; }

        public override string ToString()
        {
            return $"VerifyMatch RosterId={RosterId} BitsMatchid={BitsMatchId}";
        }
    }
}