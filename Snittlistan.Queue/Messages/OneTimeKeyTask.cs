#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class OneTimeKeyTask : ITask
    {
        public OneTimeKeyTask(string email, string oneTimePassword)
        {
            Subject = "Logga in till Snittlistan";
            Email = email;
            OneTimePassword = oneTimePassword;
        }

        public string Subject { get; }

        public string Email { get; }

        public string OneTimePassword { get; }

        public BusinessKey BusinessKey => new(GetType(), $"{Email}");
    }
}
