namespace Snittlistan.Queue.Messages
{
    /// <summary>
    /// Raised when a new user is created.
    /// </summary>
    public class NewUserCreatedTask
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
    }
}