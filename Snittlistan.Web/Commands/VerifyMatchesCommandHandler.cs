#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class VerifyMatchesCommandHandler : CommandHandler<VerifyMatchesCommand>
    {
        protected override Task<object> CreateMessage(VerifyMatchesCommand command)
        {
            return Task.FromResult((object)new VerifyMatchesTask(command.Force));
        }
    }
}
