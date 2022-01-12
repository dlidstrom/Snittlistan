#nullable enable

namespace Snittlistan.Queue.Messages;

public class GetRostersFromBitsTask : TaskBase
{
    public GetRostersFromBitsTask()
        : base(new(typeof(GetRostersFromBitsTask).FullName, string.Empty))
    {
    }
}
