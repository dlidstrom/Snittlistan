
using Npgsql.Logging;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public class NLogLoggingProvider : INpgsqlLoggingProvider
{
    public NpgsqlLogger CreateLogger(string name)
    {
        return new NLogLogger(name);
    }
}
