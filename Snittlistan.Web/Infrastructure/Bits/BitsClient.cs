namespace Snittlistan.Web.Infrastructure.Bits
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Contracts;
    using Newtonsoft.Json;

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
            var result = await Get<HeadInfo>($"https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey={apiKey}&id={matchId}");
            return result;
        }

        public async Task<HeadResultInfo> GetHeadResultInfo(int matchId)
        {
            var result = await Get<HeadResultInfo>($"https://api.swebowl.se/api/v1/matchResult/GetHeadResultInfo?APIKey={apiKey}&id={matchId}");
            return result;
        }

        public async Task<MatchResults> GetMatchResults(int matchId)
        {
            var result = await Get< MatchResults>($"https://api.swebowl.se/api/v1/matchResult/GetMatchResults?APIKey={apiKey}&matchId={matchId}&matchSchemeId=8M8BA");
            return result;
        }

        public async Task<MatchScores> GetMatchScores(int matchId)
        {
            var result = await Get<MatchScores>($"https://api.swebowl.se/api/v1/matchResult/GetMatchScores?APIKey={apiKey}&matchId={matchId}&matchSchemeId=8M8BA");
            return result;
        }

        public async Task<TeamResult[]> GetTeam(int clubId, int seasonId)
        {
            var result = await Get<TeamResult[]>($"https://api.swebowl.se/api/v1/Team?APIKey={apiKey}&clubId={clubId}&seasonId={seasonId}");
            return result;
        }

        public async Task<DivisionResult[]> GetDivisions(int teamId, int seasonId)
        {
            var result = await Get<DivisionResult[]>($"https://api.swebowl.se/api/v1/Division?APIKey={apiKey}&teamId={teamId}&seasonId={seasonId}");
            return result;
        }

        public async Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId)
        {
            var result = await Get<MatchRound[]>($"https://api.swebowl.se/api/v1/Match/?APIKey={apiKey}&teamId={teamId}&divisionId={divisionId}&seasonId={seasonId}");
            return result;
        }

        private async Task<TResult> Get<TResult>(string url)
        {
            var result = await Request(HttpMethod.Get, url, _ => { });
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        private async Task<string> Post(object body, string url)
        {
            var result = await Request(HttpMethod.Post, url, x => x.Content = new StringContent(JsonConvert.SerializeObject(body)));
            return result;
        }

        private async Task<string> Request(HttpMethod method, string url, Action<HttpRequestMessage> action)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Referrer = new Uri("https://bits.swebowl.se");
            action.Invoke(request);
            var result = await client.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }
    }
}