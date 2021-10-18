namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchTask : ITask
    {
        public RegisterMatchTask(string rosterId, int bitsMatchId)
        {
            RosterId = rosterId;
            BitsMatchId = bitsMatchId;
        }

        public string RosterId { get; }

        public int BitsMatchId { get; }

        public BusinessKey BusinessKey => new(GetType(), $"{RosterId}/{BitsMatchId}");
    }
}
