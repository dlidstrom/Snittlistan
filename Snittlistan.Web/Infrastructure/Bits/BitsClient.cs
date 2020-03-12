namespace Snittlistan.Web.Infrastructure.Bits
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Contracts;

    public class BitsClient : IBitsClient
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        public BitsClient(string apiKey, HttpClient client)
        {
            this.apiKey = apiKey;
            this.client = client;
        }

        public async Task<HeadInfo> GetHeadInfo(int matchId)
        {
            var result = await Send($"https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey={apiKey}&id={matchId}", matchId);
            var headInfo = HeadInfo.FromJson(result);
            return headInfo;
        }

        public async Task<HeadResultInfo> GetHeadResultInfo(int matchId)
        {
            var result = await Send($"https://api.swebowl.se/api/v1/matchResult/GetHeadResultInfo?APIKey={apiKey}&id={matchId}", matchId);
            var headResultInfo = HeadResultInfo.FromJson(result);
            return headResultInfo;
        }

        public async Task<MatchResults> GetMatchResults(int matchId)
        {
            var result = await Send($"https://api.swebowl.se/api/v1/matchResult/GetMatchResults?APIKey={apiKey}&id={matchId}&matchSchemeId=8M8BA", matchId);
            var matchResults = MatchResults.FromJson(result);
            return matchResults;
        }

        public async Task<MatchScores> GetMatchScores(int matchId)
        {
            var result = await Send($"https://api.swebowl.se/api/v1/matchResult/GetMatchScores?APIKey={apiKey}&id={matchId}&matchSchemeId=8M8BA", matchId);
            var matchScores = MatchScores.FromJson(result);
            return matchScores;
        }

        private async Task<string> Send(string url, int matchId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Referrer = new Uri($"https://bits.swebowl.se/match-detail?matchid={matchId}");
            var result = await client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }
    }
}