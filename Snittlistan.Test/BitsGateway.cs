namespace Snittlistan.Test
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Snittlistan.Web.Infrastructure;

    public static class BitsGateway
    {
        private static readonly IBitsClient Client = new BitsClient(Environment.GetEnvironmentVariable("ApiKey"));

        public static async Task<string> GetHeadInfo(int matchId)
        {
            return await Try($"HeadInfo-{matchId}.json", async () => await Client.GetHeadInfo(matchId));
        }

        public static async Task<string> GetMatchResults(int matchId)
        {
            var matchResults = await Try(
                $"MatchResults-{matchId}.json",
                () => Client.GetMatchResults(matchId));
            return matchResults;
        }

        public static async Task<string> GetMatchScores(int matchId)
        {
            var matchScores = await Try(
                $"MatchScores-{matchId}.json",
                () => Client.GetMatchScores(matchId));
            return matchScores;
        }

        public static async Task<(string, string)> GetResultsAndScores(int matchId)
        {
            var matchResults = await GetMatchResults(matchId);
            var matchScores = await GetMatchScores(matchId);
            return (matchResults, matchScores);
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