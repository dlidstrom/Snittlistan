namespace Snittlistan.Queue.Messages
{
    public class RegisterMatchMessage
    {
        public RegisterMatchMessage(string rosterId)
        {
            RosterId = rosterId;
        }

        public string RosterId { get; }
    }
}
