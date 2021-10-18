namespace Snittlistan.Queue.Messages
{
    public class OneTimeKeyEvent
    {
        public OneTimeKeyEvent(string email, string oneTimePassword)
        {
            Subject = "Logga in till Snittlistan";
            Email = email;
            OneTimePassword = oneTimePassword;
        }

        public string Subject { get; }

        public string Email { get; }

        public string OneTimePassword { get; }
    }
}
