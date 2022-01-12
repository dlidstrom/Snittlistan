#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class GetPlayersFromBitsCommandHandler : CommandHandler<GetPlayersFromBitsCommand>
{
    protected override Task<TaskBase> CreateMessage(GetPlayersFromBitsCommand command)
    {
        return Task.FromResult((TaskBase)new GetPlayersFromBitsTask());
    }
}
