#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Tasks;

namespace Snittlistan.Web.ExternalCommands;

public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommand>
{
    protected override Task<TaskBase> CreateMessage(RegisterMatchesCommand command)
    {
        return Task.FromResult((TaskBase)new RegisterMatchesTaskHandler.RegisterMatchesTask());
    }
}
