#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class InviteUserEmail_State : EmailState
{
    public InviteUserEmail_State(string to, string subject, string activationUri)
        : base(OwnerEmail, to, OwnerEmail, subject)
    {
        ActivationUri = activationUri;
    }

    public string ActivationUri { get; }

    public override Email CreateEmail()
    {
        return new InviteUserEmail(
            To,
            Subject,
            ActivationUri);
    }
}
