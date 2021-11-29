#nullable enable

namespace Snittlistan.Queue.Commands
{
    public class VerifyMatchesCommand : CommandBase
    {
        public VerifyMatchesCommand(bool force)
        {
            Force = force;
        }

        public bool Force { get; }
    }
}
