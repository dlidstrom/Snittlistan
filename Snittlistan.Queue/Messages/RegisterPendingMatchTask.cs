#nullable enable

namespace Snittlistan.Queue.Messages;

public class RegisterPendingMatchTask : TaskBase
{
    public RegisterPendingMatchTask(string rosterId, int bitsMatchId)
        : base(new(typeof(RegisterPendingMatchTask).FullName, $"{rosterId}/{bitsMatchId}"))
    {
        RosterId = rosterId;
        BitsMatchId = bitsMatchId;
    }

    public string RosterId { get; }

    public int BitsMatchId { get; }
}
