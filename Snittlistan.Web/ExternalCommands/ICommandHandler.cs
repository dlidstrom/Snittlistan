#nullable enable

using Snittlistan.Queue.ExternalCommands;

namespace Snittlistan.Web.ExternalCommands;

public interface ICommandHandler<TCommand>
    where TCommand : CommandBase
{
    Task Handle(TCommand command);
}
