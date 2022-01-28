using Snittlistan.Web.Infrastructure.Bits.Contracts;

namespace Snittlistan.Web.Infrastructure.Bits;
public interface IBitsClient
{
    Task<HeadInfo> GetHeadInfo(int matchId);

    Task<HeadResultInfo> GetHeadResultInfo(int matchId);

    Task<MatchResults> GetMatchResults(int matchId);

    Task<MatchScores> GetMatchScores(int matchId);

    Task<TeamResult[]> GetTeam(int clubId, int seasonId);

    Task<DivisionResult[]> GetDivisions(int teamId, int seasonId);

    Task<MatchRound[]> GetMatchRounds(int teamId, int divisionId, int seasonId);

    Task<PlayerResult> GetPlayers(int clubId);
}

public static class BitsClientExtensions
{
    public static async Task<BitsMatchResult> GetBitsMatchResult(this IBitsClient client, int matchId)
    {
        Task<MatchResults> matchResultsTask = client.GetMatchResults(matchId);
        Task<MatchScores> matchScoresTask = client.GetMatchScores(matchId);
        Task<HeadResultInfo> headResultInfoTask = client.GetHeadResultInfo(matchId);
        Task<HeadInfo> headInfoTask = client.GetHeadInfo(matchId);

        await Task.WhenAll(matchResultsTask, matchScoresTask, headResultInfoTask, headInfoTask);

        MatchResults matchResults = await matchResultsTask;
        MatchScores matchScores = await matchScoresTask;
        HeadResultInfo headResultInfo = await headResultInfoTask;
        HeadInfo headInfo = await headInfoTask;
        return new BitsMatchResult(matchResults, matchScores, headResultInfo, headInfo);
    }
}
