namespace Snittlistan.Queue.Messages
{
    public class VerifyMatchesMessage
    {
        public VerifyMatchesMessage(bool force)
        {
            Force = force;
        }

        public bool Force { get; }
    }
}
