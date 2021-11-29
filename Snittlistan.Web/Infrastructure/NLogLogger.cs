#nullable enable

namespace Snittlistan.Web.Infrastructure
{
    using System;
    using NLog;
    using Npgsql.Logging;

    public class NLogLogger : NpgsqlLogger
    {
        private readonly Logger _log;

        public NLogLogger(string name)
        {
            _log = LogManager.GetLogger(name);
        }

        public override bool IsEnabled(NpgsqlLogLevel level)
        {
            return _log.IsEnabled(ToNLogLogLevel(level));
        }

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception? exception = null)
        {
            LogEventInfo ev = new(ToNLogLogLevel(level), _log.Name, msg);
            if (exception != null)
            {
                ev.Exception = exception;
            }

            if (connectorId != 0)
            {
                ev.Properties["ConnectorId"] = connectorId;
            }

            _log.Log(ev);
        }

        private static LogLevel ToNLogLogLevel(NpgsqlLogLevel level)
        {
            return level switch
            {
                NpgsqlLogLevel.Trace => LogLevel.Trace,
                NpgsqlLogLevel.Debug => LogLevel.Debug,
                NpgsqlLogLevel.Info => LogLevel.Info,
                NpgsqlLogLevel.Warn => LogLevel.Warn,
                NpgsqlLogLevel.Error => LogLevel.Error,
                NpgsqlLogLevel.Fatal => LogLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException("level"),
            };
        }
    }
}
