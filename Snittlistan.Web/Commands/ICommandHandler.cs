#nullable enable

namespace Snittlistan.Web.Commands
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Commands;

    public interface ICommandHandler<TCommand> where TCommand : CommandBase
    {
        Task Handle(TCommand command);
    }
}
