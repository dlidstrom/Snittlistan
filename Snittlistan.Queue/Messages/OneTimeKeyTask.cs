#nullable enable

namespace Snittlistan.Queue.Messages;

public class OneTimeKeyTask : TaskBase
{
    public OneTimeKeyTask(string email, string oneTimePassword)
        : base(new(typeof(OneTimeKeyTask), email))
    {
        Subject = "Logga in till Snittlistan";
        Email = email;
        OneTimePassword = oneTimePassword;
    }

    public string Subject { get; }

    public string Email { get; }

    public string OneTimePassword { get; }
}
