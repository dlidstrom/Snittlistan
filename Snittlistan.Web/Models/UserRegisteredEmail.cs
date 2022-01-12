#nullable enable

namespace Snittlistan.Web.Models;

public class UserRegisteredEmail : EmailBase
{
    public UserRegisteredEmail(
        string to,
        string subject,
        string id,
        string activationKey)
        : base("UserRegistered", to, subject)
    {
        Id = id;
        ActivationKey = activationKey;
    }

    public string Id { get; }

    public string ActivationKey { get; }
}
