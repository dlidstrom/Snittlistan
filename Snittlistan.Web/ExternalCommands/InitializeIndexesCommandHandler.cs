#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class InitializeIndexesCommandHandler
    : CommandHandler<InitializeIndexesCommand, InitializeIndexesTask>
{
    protected override InitializeIndexesTask CreateMessage(InitializeIndexesCommand command)
    {
        return new InitializeIndexesTask(command.Email, command.Password);
    }
}
