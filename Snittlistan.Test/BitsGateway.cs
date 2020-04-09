namespace Snittlistan.Test
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Web.Infrastructure;
    using Web.Infrastructure.Bits;
    using Web.Infrastructure.Bits.Contracts;

    public static class BitsGateway
    {
        private static readonly IBitsClient Client = new BitsClient(Environment.GetEnvironmentVariable("ApiKey"), new HttpClient());

        public static async Task<HeadInfo> GetHeadInfo(int matchId)
        {
            var headInfo = await Try(
                $"HeadInfo-{matchId}.json",
                () => Client.GetHeadInfo(matchId));
            return headInfo;
        }

        public static async Task<MatchResults> GetMatchResults(int matchId)
        {
            var matchResults = await Try(
                $"MatchResults-{matchId}.json",
                () => Client.GetMatchResults(matchId));
            return matchResults;
        }

        public static async Task<MatchScores> GetMatchScores(int matchId)
        {
            var matchScores = await Try(
                $"MatchScores-{matchId}.json",
                () => Client.GetMatchScores(matchId));
            return matchScores;
        }

        public static async Task<BitsMatchResult> GetBitsMatchResult(int matchId)
        {
            var matchResult = await Try(
                $"MatchResult-{matchId}.json",
                () => Client.GetBitsMatchResult(matchId));
            return matchResult;
        }

        private static async Task<TResult> Try<TResult>(string filename, Func<Task<TResult>> func)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var outputDirectory = Path.Combine(currentDirectory, "bits");
            if (Directory.Exists(outputDirectory) == false)
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var path = Path.Combine(outputDirectory, filename);
            try
            {
                var content = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<TResult>(content);
            }
            catch (Exception)
            {
                var result = await func.Invoke();
                File.WriteAllText(path, JsonConvert.SerializeObject(result));
                return result;
            }
        }
    }
}