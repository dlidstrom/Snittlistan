﻿#nullable enable

using System.IO;
using System.Net.Http;
using System.Runtime.Caching;
using Newtonsoft.Json;
using NUnit.Framework;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Infrastructure.Bits.Contracts;

namespace Snittlistan.Test;

public static class BitsGateway
{
    private static readonly IBitsClient Client = new BitsClient(
        new HttpClient()
        {
          BaseAddress = new Uri(Environment.GetEnvironmentVariable("GatewayUrl"))
        },
        MemoryCache.Default);

    public static async Task<HeadInfo> GetHeadInfo(int matchId)
    {
        HeadInfo headInfo = await Try(
            $"HeadInfo-{matchId}.json",
            () => Client.GetHeadInfo(matchId));
        return headInfo;
    }

    public static async Task<MatchResults> GetMatchResults(int matchId)
    {
        MatchResults matchResults = await Try(
            $"MatchResults-{matchId}.json",
            () => Client.GetMatchResults(matchId));
        return matchResults;
    }

    public static async Task<MatchScores> GetMatchScores(int matchId)
    {
        MatchScores matchScores = await Try(
            $"MatchScores-{matchId}.json",
            () => Client.GetMatchScores(matchId));
        return matchScores;
    }

    public static async Task<BitsMatchResult> GetBitsMatchResult(int matchId)
    {
        BitsMatchResult matchResult = await Try(
            $"MatchResult-{matchId}.json",
            () => Client.GetBitsMatchResult(matchId));
        return matchResult;
    }

    public static async Task<HeadResultInfo> GetHeadResultInfo(int matchId)
    {
        HeadResultInfo matchResult = await Try(
            $"HeadResultInfo-{matchId}.json",
            () => Client.GetHeadResultInfo(matchId));
        return matchResult;
    }

    public static async Task<TeamResult[]> GetTeam(int clubId, int seasonId)
    {
        TeamResult[] teamResult = await Try(
            $"Team-{clubId}-{seasonId}.json",
            () => Client.GetTeam(clubId, seasonId));
        return teamResult;
    }

    public static async Task<DivisionResult[]> GetDivisions(int teamId, int seasonId)
    {
        DivisionResult[] divisionResult = await Try(
            $"Division-{teamId}-{seasonId}.json",
            () => Client.GetDivisions(teamId, seasonId));
        return divisionResult;
    }

    public static async Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId)
    {
        MatchRound[] matchRounds = await Try(
            $"MatchRound-{teamId}-{divisionId}-{seasonId}.json",
            () => Client.GetMatchRounds(teamId, divisionId, seasonId));
        return matchRounds;
    }

    public static async Task<PlayerResult> GetPlayers(int clubId)
    {
        PlayerResult playerResult = await Try(
            $"PlayerResult-{clubId}.json",
            () => Client.GetPlayers(clubId));
        return playerResult;
    }

    private static async Task<TResult> Try<TResult>(string filename, Func<Task<TResult>> func)
    {
        string currentDirectory = TestContext.CurrentContext.TestDirectory;
        while (Directory.Exists(Path.Combine(currentDirectory, "bits")) == false)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(currentDirectory);
            if (directoryInfo.Exists == false)
            {
                throw new Exception($"Unable to find bits directory, started search in {TestContext.CurrentContext.TestDirectory}");
            }

            currentDirectory = directoryInfo.FullName;
        }

        string outputDirectory = Path.Combine(currentDirectory, "bits");
        if (Directory.Exists(outputDirectory) == false)
        {
            _ = Directory.CreateDirectory(outputDirectory);
        }

        string path = Path.Combine(outputDirectory, filename);
        try
        {
            string content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<TResult>(content)!;
        }
        catch (Exception)
        {
            TResult result = await func.Invoke();
            File.WriteAllText(path, JsonConvert.SerializeObject(result));
            return result;
        }
    }
}
