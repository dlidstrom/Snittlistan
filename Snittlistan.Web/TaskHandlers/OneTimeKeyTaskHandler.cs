#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class OneTimeKeyTaskHandler
    : TaskHandler<OneTimeKeyTask, OneTimeKeyCommandHandler.Command>
{
    protected override OneTimeKeyCommandHandler.Command CreateCommand(OneTimeKeyTask payload)
    {
        return new(payload.Subject, payload.Email, payload.OneTimePassword);
    }
}
