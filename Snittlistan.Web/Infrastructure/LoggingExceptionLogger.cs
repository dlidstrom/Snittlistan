#nullable enable

using System.Web.Http.ExceptionHandling;
using AsyncFriendlyStackTrace;
using NLog;

namespace Snittlistan.Web.Infrastructure;

public class LoggingExceptionLogger : IExceptionLogger
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public static EventHandler<Exception> ExceptionHandler =
        (sender, exception) => Logger.Error(exception.ToAsyncString());

    public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
    {
        ExceptionHandler.Invoke(this, context.Exception);
        return Task.FromResult(0);
    }
}
