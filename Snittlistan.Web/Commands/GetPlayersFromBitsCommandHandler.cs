using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Commands;

public class GetPlayersFromBitsCommandHandler : CommandHandler<GetPlayersFromBitsCommand>
{
    protected override Task<TaskBase> CreateMessage(GetPlayersFromBitsCommand command)
    {
        return Task.FromResult((TaskBase)new GetPlayersFromBitsTask());
    }
}
