using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Tasks;

#nullable enable

namespace Snittlistan.Web.Commands;

public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommand>
{
    protected override Task<TaskBase> CreateMessage(RegisterMatchesCommand command)
    {
        return Task.FromResult((TaskBase)new RegisterMatchesTaskHandler.RegisterMatchesTask());
    }
}
