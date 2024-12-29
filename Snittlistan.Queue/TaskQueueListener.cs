#nullable enable

using System.Net.Http;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Queue;

public class TaskQueueListener : MessageQueueListenerBase
{
    private readonly HttpClient client = new(new LoggingHandler())
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
        MessageEnvelope envelope = contents.FromJson<MessageEnvelope>();
        TaskRequest request = new(envelope);
        HttpResponseMessage result = await client.PostAsJsonAsync(
            $"{urlScheme}://{envelope.Hostname}:{port}/api/task",
            request);
        _ = result.EnsureSuccessStatusCode();
    }
}
