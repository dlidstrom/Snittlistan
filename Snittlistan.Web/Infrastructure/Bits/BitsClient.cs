namespace Snittlistan.Web.Infrastructure.Bits
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Newtonsoft.Json;
    using NLog;

    public class BitsClient : IBitsClient
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
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

        public async Task<PlayerResult> GetPlayers(int clubId)
        {
            var result = await Post<PlayerResult>(
                new
                {
                    ClubId = clubId,
                    TakeOnlyActive = true,
                    take = "250",
                    skip = 0,
                    page = 1,
                    pageSize = "250",
                    sort = new object[0]
                },
                $"https://api.swebowl.se/api/v1/player/GetAll?APIKey={apiKey}");
            return result;
        }

        private async Task<TResult> Get<TResult>(string url)
        {
            var result = await Request(HttpMethod.Get, url, _ => { });
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        private async Task<TResult> Post<TResult>(object body, string url)
        {
            var result = await Request(
                HttpMethod.Post,
                url,
                x => x.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        private async Task<string> Request(HttpMethod method, string url, Action<HttpRequestMessage> action)
        {
            Logger.Info("{0}", url);
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