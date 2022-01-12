#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;

namespace Snittlistan.Web.TaskHandlers;

public class EmailTaskHandler
    : TaskHandler<EmailTask, SendEmailCommandHandler.Command>
{
    protected override SendEmailCommandHandler.Command CreateCommand(EmailTask payload)
    {
        return new(payload.To, payload.Subject, payload.Content);
    }
}
