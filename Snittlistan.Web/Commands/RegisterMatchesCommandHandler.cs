#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class RegisterMatchesCommandHandler : CommandHandler<RegisterMatchesCommand>
    {
        protected override Task<TaskBase> CreateMessage(RegisterMatchesCommand command)
        {
            return Task.FromResult((TaskBase)new RegisterMatchesTask());
        }
    }
}
