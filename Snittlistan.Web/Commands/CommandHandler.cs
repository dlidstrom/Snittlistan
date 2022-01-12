#nullable enable

using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : CommandBase
{
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public abstract Task Handle(HandlerContext<TCommand> context);
}
