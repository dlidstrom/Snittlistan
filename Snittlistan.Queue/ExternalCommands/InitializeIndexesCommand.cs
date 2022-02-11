#nullable enable

namespace Snittlistan.Queue.ExternalCommands;

public class InitializeIndexesCommand : CommandBase
{
    public InitializeIndexesCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }

    public string Password { get; }
}
