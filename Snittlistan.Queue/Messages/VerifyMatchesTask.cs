namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchesTask : ITask
    {
        public VerifyMatchesTask(bool force)
        {
            Force = force;
        }

        public bool Force { get; }

        public BusinessKey BusinessKey => new(GetType(), string.Empty);
    }
}
