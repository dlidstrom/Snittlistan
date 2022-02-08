#nullable enable

namespace Snittlistan.Web.Models;

public class InviteUserEmail : EmailBase
{
    private readonly InviteUserEmail_State _state;

    public InviteUserEmail(string to, string subject, string activationUri)
        : base("InviteUser")
    {
        _state = new(
            to,
            subject,
            activationUri);
    }

    public string ActivationUri => _state.ActivationUri;

    public override EmailState State => _state;
}
