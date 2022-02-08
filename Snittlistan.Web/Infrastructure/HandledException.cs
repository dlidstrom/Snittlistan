#nullable enable

namespace Snittlistan.Web.Infrastructure;

public class HandledException : Exception
{
    public HandledException(string message)
        : base(message)
    {
    }
}
