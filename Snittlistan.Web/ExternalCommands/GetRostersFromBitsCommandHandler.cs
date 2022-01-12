#nullable enable

using Snittlistan.Queue.ExternalCommands;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Web.ExternalCommands;

public class GetRostersFromBitsCommandHandler : CommandHandler<GetRostersFromBitsCommand, GetRostersFromBitsTask>
{
    protected override GetRostersFromBitsTask CreateMessage(GetRostersFromBitsCommand command)
    {
        return new GetRostersFromBitsTask();
    }
}
