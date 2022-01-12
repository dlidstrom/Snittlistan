#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class MatchRegisteredTaskHandler
    : TaskHandler<MatchRegisteredTask, MatchRegisteredCommandHandler.Command>
{
    protected override MatchRegisteredCommandHandler.Command CreateCommand(MatchRegisteredTask payload)
    {
        return new(
            payload.RosterId,
            payload.BitsMatchId,
            payload.Score,
            payload.OpponentScore);
    }
}
