using Newtonsoft.Json;

namespace Snittlistan.Queue.Messages
{
    public class TaskRequest
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public TaskRequest(MessageEnvelope envelope)
        {
            TaskJson = JsonConvert.SerializeObject(envelope.Payload, serializerSettings);
        }

        public string TaskJson { get; }
    }
}