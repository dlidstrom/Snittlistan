#nullable enable

using System.Net.Http;
using Newtonsoft.Json;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue;

public class TaskQueueListener : MessageQueueListenerBase
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };

    private readonly HttpClient client = new(new LoggingHandler(new HttpClientHandler()))
    {
        Timeout = TimeSpan.FromSeconds(600)
    };
    private readonly string urlScheme;
    private readonly int port;

    public TaskQueueListener(
        MessageQueueProcessorSettings settings,
        string urlScheme,
        int port)
        : base(settings)
    {
        this.urlScheme = urlScheme;
        this.port = port;
    }

    protected override async Task DoHandle(string contents)
    {
        MessageEnvelope? envelope = JsonConvert.DeserializeObject<MessageEnvelope>(contents, SerializerSettings);
        if (envelope == null)
        {
            throw new Exception("deserialization failed");
        }

        TaskRequest request = new(envelope);
        HttpResponseMessage result = await client.PostAsJsonAsync(
            $"{urlScheme}://{envelope.Hostname}:{port}/api/task",
            request);
        _ = result.EnsureSuccessStatusCode();
    }
}
