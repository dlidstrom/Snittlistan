#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class GetRostersFromBitsCommandHandler : CommandHandler<GetRostersFromBitsCommand>
    {
        protected override Task<TaskBase> CreateMessage(GetRostersFromBitsCommand command)
        {
            return Task.FromResult((TaskBase)new GetRostersFromBitsTask());
        }
    }
}
