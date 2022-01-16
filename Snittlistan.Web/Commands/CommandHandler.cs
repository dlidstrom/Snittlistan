#nullable enable

using Castle.Core.Logging;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
{
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public abstract Task Handle(HandlerContext<TCommand> context);

    protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
    {
        return query.Execute(CompositionRoot.DocumentSession);
    }
}
