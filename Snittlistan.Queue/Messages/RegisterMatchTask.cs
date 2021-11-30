#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchTask : TaskBase
    {
        public RegisterMatchTask(string rosterId, int bitsMatchId)
            : base(new(typeof(RegisterMatchTask), $"{rosterId}/{bitsMatchId}"))
        {
            RosterId = rosterId;
            BitsMatchId = bitsMatchId;
        }

        public string RosterId { get; }

        public int BitsMatchId { get; }
    }
}
