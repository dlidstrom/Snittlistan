namespace Snittlistan.Queue.Messages
{
    public class OneTimeKeyEvent
    {
        public OneTimeKeyEvent(string email, string oneTimePassword)
        {
            Email = email;
            OneTimePassword = oneTimePassword;
        }

        public string Email { get; }

        public string OneTimePassword { get; }
    }
}