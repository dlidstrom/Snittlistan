#nullable enable

namespace Snittlistan.Queue.Messages;

public class VerifyMatchTask : TaskBase
{
    public VerifyMatchTask(int bitsMatchId, string rosterId, bool force)
        : base(new(typeof(VerifyMatchTask).FullName, $"{rosterId}/{bitsMatchId}"))
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
