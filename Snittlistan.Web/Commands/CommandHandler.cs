#nullable enable

using NLog;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
{
    protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public CompositionRoot CompositionRoot { get; set; } = null!;

    public abstract Task Handle(HandlerContext<TCommand> context);

    protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
    {
        return query.Execute(CompositionRoot.DocumentSession);
    }
}
