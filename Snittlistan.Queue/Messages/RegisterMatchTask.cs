namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchTask
    {
        public RegisterMatchTask(string rosterId)
        {
            RosterId = rosterId;
        }

        public string RosterId { get; }
    }
}
