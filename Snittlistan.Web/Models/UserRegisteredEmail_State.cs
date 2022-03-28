#nullable enable

using Postal;

namespace Snittlistan.Web.Models;

public class UserRegisteredEmail_State : EmailState
{
    public UserRegisteredEmail_State(
        string to,
        string subject,
        string id,
        string activationKey)
        : base(OwnerEmail, to, BccEmail, subject)
    {
        Id = id;
        ActivationKey = activationKey;
    }

    public string Id { get; }

    public string ActivationKey { get; }

    public override Email CreateEmail()
    {
        return new UserRegisteredEmail(
            To,
            Subject,
            Id,
            ActivationKey);
    }
}
