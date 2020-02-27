namespace Snittlistan.Test
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Snittlistan.Web.Infrastructure;

    public static class BitsGateway
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<string> GetHeadInfo(int id)
        {
            return await Try($"HeadInfo-{id}.json", async () =>
            {
                var apiKey = Environment.GetEnvironmentVariable("ApiKey");
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey={apiKey}&id={id}");
                request.Headers.Referrer = new Uri($"https://bits.swebowl.se/match-detail?matchid={id}");
                var result = await Client.SendAsync(request);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();
                return content;
            });
        }

        public static async Task<string> GetMatch(int bitsMatchId)
        {
            var bitsClient = new BitsClient();
            var content = await Try($"BitsMatch-{bitsMatchId}.html", () => Task.FromResult(bitsClient.DownloadMatchResult(bitsMatchId)));
            return content;
        }

        private static async Task<string> Try(string filename, Func<Task<string>> func)
        {
            string content;
            var currentDirectory = Directory.GetCurrentDirectory();
            var outputDirectory = Path.Combine(currentDirectory, "bits");
            if (Directory.Exists(outputDirectory) == false)
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var path = Path.Combine(outputDirectory, filename);
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception)
            {
                content = await func.Invoke();
                File.WriteAllText(path, content);
            }

            return content;
        }
    }
}