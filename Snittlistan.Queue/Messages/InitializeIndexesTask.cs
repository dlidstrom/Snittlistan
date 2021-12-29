#nullable enable

namespace Snittlistan.Queue.Messages;

public class InitializeIndexesTask : TaskBase
{
    public InitializeIndexesTask(string email, string password)
        : base(new(typeof(InitializeIndexesTask).FullName, string.Empty))
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }

    public string Password { get; }
}
