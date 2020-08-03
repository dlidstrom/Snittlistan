namespace Snittlistan.Queue.Messages
{
    using Newtonsoft.Json;

    public class TaskRequest
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public TaskRequest(MessageEnvelope envelope)
        {
            TaskJson = JsonConvert.SerializeObject(envelope.Payload, SerializerSettings);
        }

        public string TaskJson { get; }
    }
}