#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class RegisterMatchesCommandHandler
    : CommandHandler<RegisterMatchesCommand, RegisterMatchesTask>
{
    protected override RegisterMatchesTask CreateMessage(RegisterMatchesCommand command)
    {
        return new RegisterMatchesTask();
    }
}
