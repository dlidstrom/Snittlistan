using Snittlistan.Queue.Commands;
using Snittlistan.Queue.Messages;

#nullable enable

namespace Snittlistan.Web.Commands;
public class GetRostersFromBitsCommandHandler : CommandHandler<GetRostersFromBitsCommand>
{
    protected override Task<TaskBase> CreateMessage(GetRostersFromBitsCommand command)
    {
        return Task.FromResult((TaskBase)new GetRostersFromBitsTask());
    }
}
