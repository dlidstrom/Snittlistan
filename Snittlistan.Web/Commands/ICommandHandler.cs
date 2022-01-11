#nullable enable

namespace Snittlistan.Web.Commands;

public interface ICommandHandler<TCommand>
    where TCommand : CommandBase
{
    Task Handle(CommandContext<TCommand> context);
}
