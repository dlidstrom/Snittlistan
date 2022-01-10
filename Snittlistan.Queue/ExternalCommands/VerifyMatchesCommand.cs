#nullable enable

namespace Snittlistan.Queue.ExternalCommands;

public class VerifyMatchesCommand : CommandBase
{
    public VerifyMatchesCommand(bool force)
    {
        Force = force;
    }

    public bool Force { get; }
}
