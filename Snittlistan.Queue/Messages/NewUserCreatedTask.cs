#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class NewUserCreatedTask : TaskBase
    {
        public NewUserCreatedTask(string email, string activationKey, string userId)
            : base(new(typeof(NewUserCreatedTask), $"{email}/{userId}"))
        {
            Email = email;
            ActivationKey = activationKey;
            UserId = userId;
        }

        public string Email { get; }

        public string ActivationKey { get; }

        public string UserId { get; }
    }
}
