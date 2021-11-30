#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class VerifyMatchesCommandHandler : CommandHandler<VerifyMatchesCommand>
    {
        protected override Task<TaskBase> CreateMessage(VerifyMatchesCommand command)
        {
            return Task.FromResult((TaskBase)new VerifyMatchesTask(command.Force));
        }
    }
}
