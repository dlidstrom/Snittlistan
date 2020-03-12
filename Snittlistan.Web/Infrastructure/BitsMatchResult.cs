namespace Snittlistan.Web.Infrastructure
{
    using Bits.Contracts;

    public class BitsMatchResult
    {
        public BitsMatchResult(MatchResults matchResults, MatchScores matchScores, HeadResultInfo headResultInfo, HeadInfo headInfo)
        {
            MatchResults = matchResults;
            MatchScores = matchScores;
            HeadResultInfo = headResultInfo;
            HeadInfo = headInfo;
        }

        public MatchResults MatchResults { get; }

        public MatchScores MatchScores { get; }

        public HeadResultInfo HeadResultInfo { get; }

        public HeadInfo HeadInfo { get; }
    }
}