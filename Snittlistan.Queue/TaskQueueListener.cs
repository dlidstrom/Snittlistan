#nullable enable

namespace Snittlistan.Queue
{
    using System;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Snittlistan.Queue.Messages;

    public class TaskQueueListener : MessageQueueListenerBase
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly HttpClient client = new(new LoggingHandler(new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(600)
        };

        public TaskQueueListener(MessageQueueProcessorSettings settings)
            : base(settings)
        {
        }

        protected override void DoHandle(string contents)
        {
            MessageEnvelope? envelope = JsonConvert.DeserializeObject<MessageEnvelope>(contents, SerializerSettings);
            if (envelope == null)
            {
                throw new Exception("deserialiation failed");
            }

            TaskRequest request = new(envelope);

            HttpResponseMessage result = client.PostAsJsonAsync(
                envelope.Uri,
                request).Result;
            _ = result.EnsureSuccessStatusCode();
        }
    }
}
