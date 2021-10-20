namespace Snittlistan.Web.Infrastructure.Logging
{
    using Npgsql.Logging;

    public class NLogLoggingProvider : INpgsqlLoggingProvider
    {
        public NpgsqlLogger CreateLogger(string name)
        {
            return new NLogLogger(name);
        }
    }
}
