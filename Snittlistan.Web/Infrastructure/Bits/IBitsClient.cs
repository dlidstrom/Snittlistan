namespace Snittlistan.Web.Infrastructure.Bits
{
    using System.Threading.Tasks;
    using Contracts;

    public interface IBitsClient
    {
        Task<HeadInfo> GetHeadInfo(int matchId);

        Task<HeadResultInfo> GetHeadResultInfo(int matchId);

        Task<MatchResults> GetMatchResults(int matchId);

        Task<MatchScores> GetMatchScores(int matchId);
    }

    public static class BitsClientExtensions
    {
        public static async Task<BitsMatchResult> GetBitsMatchResult(this IBitsClient client, int matchId)
        {
            var matchResultsTask = client.GetMatchResults(matchId);
            var matchScoresTask = client.GetMatchScores(matchId);
            var headResultInfoTask = client.GetHeadResultInfo(matchId);
            var headInfoTask = client.GetHeadInfo(matchId);

            await Task.WhenAll(matchResultsTask, matchScoresTask, headResultInfoTask, headInfoTask);

            var matchResults = await matchResultsTask;
            var matchScores = await matchScoresTask;
            var headResultInfo = await headResultInfoTask;
            var headInfo = await headInfoTask;
            return new BitsMatchResult(matchResults, matchScores, headResultInfo, headInfo);
        }
    }
}