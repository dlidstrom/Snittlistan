using System;
using Newtonsoft.Json;

namespace Snittlistan.Queue.Messages
{
    public class MessageEnvelope
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public MessageEnvelope(object payload, Uri uri)
        {
            Payload = JsonConvert.SerializeObject(payload, serializerSettings);
            Uri = uri;
        }

        public string Payload { get; }
        public Uri Uri { get; }

        public override string ToString()
        {
            return $"{Uri}: {JsonConvert.DeserializeObject(Payload)}";
        }
    }
}