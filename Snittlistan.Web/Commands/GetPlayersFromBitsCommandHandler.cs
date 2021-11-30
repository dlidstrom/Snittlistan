#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class GetPlayersFromBitsCommandHandler : CommandHandler<GetPlayersFromBitsCommand>
    {
        protected override Task<TaskBase> CreateMessage(GetPlayersFromBitsCommand command)
        {
            return Task.FromResult((TaskBase)new GetPlayersFromBitsTask());
        }
    }
}
