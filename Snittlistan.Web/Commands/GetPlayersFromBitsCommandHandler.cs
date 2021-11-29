#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class GetPlayersFromBitsCommandHandler : CommandHandler<GetPlayersFromBitsCommand>
    {
        protected override Task<object> CreateMessage(GetPlayersFromBitsCommand command)
        {
            return Task.FromResult((object)new GetPlayersFromBitsTask());
        }
    }
}
