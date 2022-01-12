#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class GetRostersFromBitsCommandHandler : CommandHandler<GetRostersFromBitsCommand>
{
    protected override Task<TaskBase> CreateMessage(GetRostersFromBitsCommand command)
    {
        return Task.FromResult((TaskBase)new GetRostersFromBitsTask());
    }
}
