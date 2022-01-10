#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class InitializeIndexesCommandHandler : CommandHandler<InitializeIndexesCommand>
{
    protected override Task<TaskBase> CreateMessage(InitializeIndexesCommand command)
    {
        return Task.FromResult((TaskBase)new InitializeIndexesTask(command.Email, command.Password));
    }
}
