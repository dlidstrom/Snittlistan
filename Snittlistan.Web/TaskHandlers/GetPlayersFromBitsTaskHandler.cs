#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class GetPlayersFromBitsTaskHandler
    : TaskHandler<GetPlayersFromBitsTask, GetPlayersFromBitsCommandHandler.Command>
{
    protected override GetPlayersFromBitsCommandHandler.Command CreateCommand(GetPlayersFromBitsTask payload)
    {
        return new();
    }
}
