#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class InitializeIndexesCommandHandler : CommandHandler<InitializeIndexesCommand>
    {
        protected override Task<TaskBase> CreateMessage(InitializeIndexesCommand command)
        {
            return Task.FromResult((TaskBase)new InitializeIndexesTask(command.Email, command.Password));
        }
    }
}
