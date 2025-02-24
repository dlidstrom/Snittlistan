#nullable enable

using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Newtonsoft.Json;
using Castle.Core.Logging;

namespace Snittlistan.Web.Infrastructure.Bits;

public class BitsClient(HttpClient client, MemoryCache memoryCache) : IBitsClient
{
    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task<HeadInfo> GetHeadInfo(int matchId)
    {
        HeadInfo result = await Get<HeadInfo>($"/api/v1/matchResult/GetHeadInfo?id={matchId}");
        return result;
    }

    public async Task<HeadResultInfo> GetHeadResultInfo(int matchId)
    {
        HeadResultInfo result = await Get<HeadResultInfo>($"/api/v1/matchResult/GetHeadResultInfo?id={matchId}");
        return result;
    }

    public async Task<MatchResults> GetMatchResults(int matchId)
    {
        MatchResults result = await Get<MatchResults>($"/api/v1/matchResult/GetMatchResults?matchId={matchId}&matchSchemeId=8M8BA");
        return result;
    }

    public async Task<MatchScores> GetMatchScores(int matchId)
    {
        MatchScores result = await Get<MatchScores>($"/api/v1/matchResult/GetMatchScores?matchId={matchId}&matchSchemeId=8M8BA");
        return result;
    }

    public async Task<TeamResult[]> GetTeam(int clubId, int seasonId)
    {
        TeamResult[] result = await Get<TeamResult[]>($"/api/v1/Team?clubId={clubId}&seasonId={seasonId}");
        return result;
    }

    public async Task<DivisionResult[]> GetDivisions(int teamId, int seasonId)
    {
        DivisionResult[] result = await Get<DivisionResult[]>($"/api/v1/Division?teamId={teamId}&seasonId={seasonId}");
        return result;
    }

    public async Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId)
    {
        MatchRound[] result = await Get<MatchRound[]>($"/api/v1/Match/?teamId={teamId}&divisionId={divisionId}&seasonId={seasonId}");
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
            $"/api/v1/player/GetAll");
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
        Stopwatch sw = Stopwatch.StartNew();
        while (sw.Elapsed.TotalSeconds < 60)
        {
            try
            {
                Logger.InfoFormat(
                    "Requesting {url}",
                    url);
                HttpRequestMessage request = new(method, url);
                action.Invoke(request);
                HttpResponseMessage result = await client.SendAsync(request);
                string content = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
            {
                // retry after delay
            }

            await Task.Delay(1000);
        }
    }
}
