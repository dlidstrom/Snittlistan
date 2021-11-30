#nullable enable

namespace Snittlistan.Queue.Messages;
public class UserInvitedTask : TaskBase
{
    public UserInvitedTask(string activationUri, string email)
        : base(new(typeof(UserInvitedTask), email))
    {
        ActivationUri = activationUri ?? throw new ArgumentNullException(nameof(activationUri));
        Email = email;
    }

    public string ActivationUri { get; }

    public string Email { get; }
}
