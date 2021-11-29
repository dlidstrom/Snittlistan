#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;
    using NLog;

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
}
