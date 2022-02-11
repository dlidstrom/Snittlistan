#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class VerifyMatchesTaskHandler
    : TaskHandler<VerifyMatchesTask, VerifyMatchesCommandHandler.Command>
{
    protected override VerifyMatchesCommandHandler.Command CreateCommand(VerifyMatchesTask payload)
    {
        return new(payload.Force);
    }
}
