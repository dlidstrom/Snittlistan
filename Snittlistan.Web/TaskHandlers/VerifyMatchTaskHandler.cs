#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class VerifyMatchTaskHandler
    : TaskHandler<VerifyMatchTask, VerifyMatchCommandHandler.Command>
{
    protected override VerifyMatchCommandHandler.Command CreateCommand(VerifyMatchTask payload)
    {
        return new(payload.BitsMatchId, payload.RosterId, payload.Force);
    }
}
