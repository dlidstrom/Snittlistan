#nullable enable

using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public interface ICommandHandler<TCommand>
    where TCommand : CommandBase
{
    Task Handle(HandlerContext<TCommand> context);
}
