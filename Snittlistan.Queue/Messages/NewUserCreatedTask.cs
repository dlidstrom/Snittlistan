#nullable enable

namespace Snittlistan.Queue.Messages
{
    public class NewUserCreatedTask : ITask
    {
        public NewUserCreatedTask(string email, string activationKey, string userId)
        {
            Email = email;
            ActivationKey = activationKey;
            UserId = userId;
        }

        public string Email { get; }

        public string ActivationKey { get; }

        public string UserId { get; }

        public BusinessKey BusinessKey => new(GetType(), $"{Email}/{UserId}");
    }
}
