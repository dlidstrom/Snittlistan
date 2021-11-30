using System.Web.Http.ExceptionHandling;
using NLog;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public class LoggingExceptionLogger : IExceptionLogger
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public static EventHandler<Exception> ExceptionHandler = (sender, exception) => Logger.Error(exception);

    public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
    {
        ExceptionHandler.Invoke(this, context.Exception);
        return Task.FromResult(0);
    }
}
