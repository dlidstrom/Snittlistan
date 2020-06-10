namespace Snittlistan.Queue.Messages
{
    public class InitializeIndexesMessage
    {
        public InitializeIndexesMessage(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
