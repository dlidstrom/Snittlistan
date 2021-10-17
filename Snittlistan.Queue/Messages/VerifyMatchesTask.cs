namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchesTask
    {
        public VerifyMatchesTask(bool force)
        {
            Force = force;
        }

        public bool Force { get; }
    }
}
