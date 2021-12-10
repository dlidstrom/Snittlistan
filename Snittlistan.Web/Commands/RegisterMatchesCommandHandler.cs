using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Commands;
public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommand>
{
    protected override Task<TaskBase> CreateMessage(RegisterMatchesCommand command)
    {
        return Task.FromResult((TaskBase)new RegisterMatchesTask());
    }
}
