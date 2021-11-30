#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchesTask : TaskBase
    {
        public VerifyMatchesTask(bool force)
            : base(new(typeof(VerifyMatchesTask), string.Empty))
        {
            Force = force;
        }

        public bool Force { get; }
    }
}
