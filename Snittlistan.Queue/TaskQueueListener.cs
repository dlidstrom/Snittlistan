using System;
using System.Net.Http;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue
{
    public class TaskQueueListener : MessageQueueListenerBase
    {
        private readonly HttpClient client = new HttpClient(new LoggingHandler(new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(600)
        };

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public TaskQueueListener(MessageQueueProcessorSettings settings)
            : base(settings)
        {
        }

        protected override void DoHandle(string contents)
        {
            var envelope = JsonConvert.DeserializeObject<MessageEnvelope>(contents, serializerSettings);
            var payload = JsonConvert.DeserializeObject<string>(envelope.Payload);
            var result = client.PostAsJsonAsync(envelope.Uri, new TaskRequest(payload)).Result;
            result.EnsureSuccessStatusCode();
        }
    }
}