#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class SendEmailTaskHandler
    : TaskHandler<SendEmailTask, SendEmailCommandHandler.Command>
{
    protected override SendEmailCommandHandler.Command CreateCommand(SendEmailTask payload)
    {
        return new(payload.To, payload.Subject, payload.Content, payload.RatePerSeconds);
    }
}
