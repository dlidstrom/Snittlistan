using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Commands;
public class VerifyMatchesCommandHandler : CommandHandler<VerifyMatchesCommand>
{
    protected override Task<TaskBase> CreateMessage(VerifyMatchesCommand command)
    {
        return Task.FromResult((TaskBase)new VerifyMatchesTask(command.Force));
    }
}
