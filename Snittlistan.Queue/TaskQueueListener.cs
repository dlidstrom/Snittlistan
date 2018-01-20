using System;
using System.Net.Http;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue
{
    public class TaskQueueListener : MessageQueueListenerBase
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly HttpClient client = new HttpClient(new LoggingHandler(new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(600)
        };

        public TaskQueueListener(MessageQueueProcessorSettings settings)
            : base(settings)
        {
        }

        protected override void DoHandle(string contents)
        {
            var envelope = JsonConvert.DeserializeObject<MessageEnvelope>(contents, serializerSettings);
            var request = new TaskRequest(envelope);
            var result = client.PostAsJsonAsync(
                envelope.Uri,
                request).Result;
            result.EnsureSuccessStatusCode();
        }
    }
}