#nullable enable

using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public interface ICommandHandler<TCommand>
{
    Task Handle(HandlerContext<TCommand> context);
}
