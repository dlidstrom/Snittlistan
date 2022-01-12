#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class VerifyMatchesCommandHandler : CommandHandler<VerifyMatchesCommand>
{
    protected override Task<TaskBase> CreateMessage(VerifyMatchesCommand command)
    {
        return Task.FromResult((TaskBase)new VerifyMatchesTask(command.Force));
    }
}
