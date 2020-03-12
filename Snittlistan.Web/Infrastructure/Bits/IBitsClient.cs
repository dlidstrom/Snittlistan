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
            var matchResults = await client.GetMatchResults(matchId);
            var matchScores = await client.GetMatchScores(matchId);
            var headResultInfo = await client.GetHeadResultInfo(matchId);
            return new BitsMatchResult(matchResults, matchScores, headResultInfo);
        }
    }
}