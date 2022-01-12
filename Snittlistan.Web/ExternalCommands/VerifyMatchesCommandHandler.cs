#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class VerifyMatchesCommandHandler
    : CommandHandler<VerifyMatchesCommand, VerifyMatchesTask>
{
    protected override VerifyMatchesTask CreateMessage(VerifyMatchesCommand command)
    {
        return new VerifyMatchesTask(command.Force);
    }
}
