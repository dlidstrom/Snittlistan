namespace Snittlistan.Queue.Messages
{
    public class InitializeIndexesTask : ITask
    {
        public InitializeIndexesTask(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }

        public string Password { get; }

        public BusinessKey BusinessKey => new(GetType(), string.Empty);
    }
}
