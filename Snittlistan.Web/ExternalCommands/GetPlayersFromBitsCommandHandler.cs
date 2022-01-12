#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class GetPlayersFromBitsCommandHandler
    : CommandHandler<GetPlayersFromBitsCommand, GetPlayersFromBitsTask>
{
    protected override GetPlayersFromBitsTask CreateMessage(GetPlayersFromBitsCommand command)
    {
        return new GetPlayersFromBitsTask();
    }
}
