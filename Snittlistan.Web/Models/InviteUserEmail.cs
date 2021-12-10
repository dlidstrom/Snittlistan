#nullable enable

namespace Snittlistan.Web.Models;

public class InviteUserEmail : EmailBase
{
    public InviteUserEmail(string to, string subject, string activationUri)
        : base("InviteUser", to, subject)
    {
        ActivationUri = activationUri;
    }

    public string ActivationUri { get; }
}
