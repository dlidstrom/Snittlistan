#nullable enable

using Postal;
using System.Configuration;

namespace Snittlistan.Web.Models;

public abstract class EmailState
{
    protected EmailState(
        string from,
        string to,
        string bcc,
        string subject)
    {
        From = from;
        To = to;
        Bcc = bcc;
        Subject = subject;
    }

    public static string OwnerEmail { get; } = ConfigurationManager.AppSettings["OwnerEmail"];

    public static string BccEmail { get; } = ConfigurationManager.AppSettings["BccEmail"];

    public string Bcc { get; }

    public string From { get; }

    public string To { get; }

    public string Subject { get; }

    public abstract Email CreateEmail();
}
