#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;
    using Snittlistan.Queue.Messages;

    public class InitializeIndexesCommandHandler : CommandHandler<InitializeIndexesCommand>
    {
        protected override Task<object> CreateMessage(InitializeIndexesCommand command)
        {
            return Task.FromResult((object)new InitializeIndexesTask(command.Email, command.Password));
        }
    }
}
