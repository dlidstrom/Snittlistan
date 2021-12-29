#nullable enable

namespace Snittlistan.Queue.Messages;

public class GetPlayersFromBitsTask : TaskBase
{
    public GetPlayersFromBitsTask()
        : base(new(typeof(GetPlayersFromBitsTask).FullName, string.Empty))
    {
    }
}
