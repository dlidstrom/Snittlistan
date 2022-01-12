#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class GetRostersFromBitsTaskHandler
    : TaskHandler<GetRostersFromBitsTask, GetRostersFromBitsCommandHandler.Command>
{
    protected override GetRostersFromBitsCommandHandler.Command CreateCommand(GetRostersFromBitsTask payload)
    {
        return new();
    }
}
