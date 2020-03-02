namespace Snittlistan.Web.Infrastructure
{
    using System.Threading.Tasks;

    public interface IBitsClient
    {
        Task<string> GetHeadInfo(int matchId);

        Task<string> GetMatchResults(int matchId);

        Task<string> GetMatchScores(int matchId);
    }

    public static class BitsClientExtensions
    {
        public static async Task<(string, string)> GetResultsAndScores(this IBitsClient client, int matchId)
        {
            var matchResults = await client.GetMatchResults(matchId);
            var matchScores = await client.GetMatchScores(matchId);
            return (matchResults, matchScores);
        }
    }
}