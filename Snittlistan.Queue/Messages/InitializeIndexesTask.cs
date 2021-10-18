namespace Snittlistan.Queue.Messages
{
    public class InitializeIndexesTask
    {
        public InitializeIndexesTask(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
