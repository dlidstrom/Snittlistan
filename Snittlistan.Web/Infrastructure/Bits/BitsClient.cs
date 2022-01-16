#nullable enable

using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Newtonsoft.Json;
using Castle.Core.Logging;

namespace Snittlistan.Web.Infrastructure.Bits;

public class BitsClient : IBitsClient
{
    private readonly string apiKey;
    private readonly HttpClient client;
    private readonly MemoryCache memoryCache;

    public BitsClient(string apiKey, HttpClient client, MemoryCache memoryCache)
    {
        this.apiKey = apiKey;
        this.client = client;
        this.memoryCache = memoryCache;
    }

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task<HeadInfo> GetHeadInfo(int matchId)
    {
        HeadInfo result = await Get<HeadInfo>($"https://api.swebowl.se/api/v1/matchResult/GetHeadInfo?APIKey={apiKey}&id={matchId}");
        return result;
    }

    public async Task<HeadResultInfo> GetHeadResultInfo(int matchId)
    {
        HeadResultInfo result = await Get<HeadResultInfo>($"https://api.swebowl.se/api/v1/matchResult/GetHeadResultInfo?APIKey={apiKey}&id={matchId}");
        return result;
    }

    public async Task<MatchResults> GetMatchResults(int matchId)
    {
        MatchResults result = await Get<MatchResults>($"https://api.swebowl.se/api/v1/matchResult/GetMatchResults?APIKey={apiKey}&matchId={matchId}&matchSchemeId=8M8BA");
        return result;
    }

    public async Task<MatchScores> GetMatchScores(int matchId)
    {
        MatchScores result = await Get<MatchScores>($"https://api.swebowl.se/api/v1/matchResult/GetMatchScores?APIKey={apiKey}&matchId={matchId}&matchSchemeId=8M8BA");
        return result;
    }

    public async Task<TeamResult[]> GetTeam(int clubId, int seasonId)
    {
        TeamResult[] result = await Get<TeamResult[]>($"https://api.swebowl.se/api/v1/Team?APIKey={apiKey}&clubId={clubId}&seasonId={seasonId}");
        return result;
    }

    public async Task<DivisionResult[]> GetDivisions(int teamId, int seasonId)
    {
        DivisionResult[] result = await Get<DivisionResult[]>($"https://api.swebowl.se/api/v1/Division?APIKey={apiKey}&teamId={teamId}&seasonId={seasonId}");
        return result;
    }

    public async Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId)
    {
        MatchRound[] result = await Get<MatchRound[]>($"https://api.swebowl.se/api/v1/Match/?APIKey={apiKey}&teamId={teamId}&divisionId={divisionId}&seasonId={seasonId}");
        return result;
    }

    public async Task<PlayerResult> GetPlayers(int clubId)
    {
        PlayerResult result = await Post<PlayerResult>(
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

    private async Task<TResult> Get<TResult>(string url) where TResult : class
    {
        if (memoryCache.Get(url) is TResult item)
        {
            Logger.InfoFormat(
                "Found {url} in cache",
                url);
            return item;
        }

        string response = await Request(HttpMethod.Get, url, _ => { });
        TResult? result = JsonConvert.DeserializeObject<TResult>(response);
        if (result == null)
        {
            Exception exception = new("failed to deserialize response");
            exception.Data["response"] = response;
            throw exception;
        }

        memoryCache.Set(
            url,
            result,
            new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1),
                RemovedCallback = x => Logger.InfoFormat(
                    "{key} evicted due to {reason}",
                    x.CacheItem.Key,
                    x.RemovedReason)
            });
        return result;
    }

    private async Task<TResult> Post<TResult>(object body, string url)
    {
        string cacheKey = JsonConvert.SerializeObject(new
        {
            body,
            url
        });
        if (memoryCache.Get(cacheKey) is TResult item)
        {
            Logger.InfoFormat(
                "Found {key} in cache",
                cacheKey);
            return item;
        }

        string response = await Request(
            HttpMethod.Post,
            url,
            x => x.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        TResult? result = JsonConvert.DeserializeObject<TResult>(response);
        if (result == null)
        {
            Exception exception = new("failed to deserialize response");
            exception.Data["response"] = response;
            throw exception;
        }

        memoryCache.Set(
            url,
            result,
            new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1),
                RemovedCallback = x => Logger.InfoFormat(
                    "{key} evicted due to {reason}",
                    x.CacheItem.Key,
                    x.RemovedReason)
            });
        return result;
    }

    private async Task<string> Request(HttpMethod method, string url, Action<HttpRequestMessage> action)
    {
        Logger.InfoFormat(
            "Requesting {url}",
            url);
        HttpRequestMessage request = new(method, url);
        request.Headers.Referrer = new Uri("https://bits.swebowl.se");
        action.Invoke(request);
        HttpResponseMessage result = await client.SendAsync(request);
        string content = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        return content;
    }
}
