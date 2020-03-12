namespace Snittlistan.Web.Infrastructure
{
    using Bits.Contracts;

    public class BitsMatchResult
    {
        public BitsMatchResult(MatchResults matchResults, MatchScores matchScores, HeadResultInfo headResultInfo)
        {
            MatchResults = matchResults;
            MatchScores = matchScores;
            HeadResultInfo = headResultInfo;
        }

        public MatchResults MatchResults { get; }

        public MatchScores MatchScores { get; }

        public HeadResultInfo HeadResultInfo { get; }
    }
}