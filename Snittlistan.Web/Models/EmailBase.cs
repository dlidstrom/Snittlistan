#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public abstract class EmailBase : Email
{
    protected EmailBase(string viewName)
        : base(viewName)
    {
    }

    public abstract EmailState State { get; }

    public string Bcc => State.Bcc;

    public string From => State.From;

    public string To => State.To;

    public string Subject => State.Subject;
}
