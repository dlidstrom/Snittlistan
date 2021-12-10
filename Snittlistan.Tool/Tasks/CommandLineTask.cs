using System.Net.Http;
using Newtonsoft.Json;
using Snittlistan.Queue;
using Snittlistan.Queue.Commands;

#nullable enable

namespace Snittlistan.Tool.Tasks;
public abstract class CommandLineTask : ICommandLineTask
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    private readonly HttpClient client = new(new LoggingHandler(new HttpClientHandler()))
    {
        Timeout = TimeSpan.FromSeconds(600)
    };

    public HttpConnectionSettings HttpConnectionSettings { get; set; } = null!;

    public abstract string HelpText { get; }

    public abstract Task Run(string[] args);

    protected async Task ExecuteCommand(CommandBase command)
    {
        CommandRequest request = new(command);
        HttpResponseMessage result = await client.PostAsJsonAsync(
            $"{HttpConnectionSettings.UrlScheme}://localhost:{HttpConnectionSettings.Port}/api/command",
            request);
        _ = result.EnsureSuccessStatusCode();
    }

    private class CommandRequest
    {
        public CommandRequest(object command)
        {
            CommandJson = JsonConvert.SerializeObject(command, SerializerSettings);
        }

        public string CommandJson { get; }
    }
}
