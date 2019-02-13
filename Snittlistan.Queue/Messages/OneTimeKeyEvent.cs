namespace Snittlistan.Queue.Messages
{
    public class OneTimeKeyEvent
    {
        public OneTimeKeyEvent(string email, string activationUri)
        {
            Email = email;
            ActivationUri = activationUri;
        }

        public string Email { get; }

        public string ActivationUri { get; }
    }
}