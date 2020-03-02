namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class BitsClient : IBitsClient
    {
        private readonly string apiKey;
        private static readonly HttpClient Client = new HttpClient();

        public BitsClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<string> GetHeadInfo(int matchId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey={apiKey}&id={matchId}");
            request.Headers.Referrer = new Uri($"https://bits.swebowl.se/match-detail?matchid={matchId}");
            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetMatchResults(int matchId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.swebowl.se/api/v1/matchResult/GetMatchResults?APIKey={apiKey}&id={matchId}&matchSchemeId=8M8BA");
            request.Headers.Referrer = new Uri($"https://bits.swebowl.se/match-detail?matchid={matchId}");
            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetMatchScores(int matchId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.swebowl.se/api/v1/matchResult/GetMatchScores?APIKey={apiKey}&id={matchId}&matchSchemeId=8M8BA");
            request.Headers.Referrer = new Uri($"https://bits.swebowl.se/match-detail?matchid={matchId}");
            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }
    }
}