#nullable enable

namespace Snittlistan.Web.Models;

public class UserRegisteredEmail : EmailBase
{
    private readonly UserRegisteredEmail_State _state;

    public UserRegisteredEmail(
        string to,
        string subject,
        string id,
        string activationKey)
        : base("UserRegistered")
    {
        _state = new(
            to,
            subject,
            id,
            activationKey);
    }

    public string Id => _state.Id;

    public string ActivationKey => _state.ActivationKey;

    public override EmailState State => _state;
}
