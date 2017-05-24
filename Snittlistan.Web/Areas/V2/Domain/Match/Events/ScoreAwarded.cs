using System.Collections.Generic;
using EventStoreLite;

// ReSharper disable once CheckNamespace
namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class ScoreAwarded : Event
    {
        public ScoreAwarded(Dictionary<string, int> playerIdToScore, int bitsMatchId)
        {
            PlayerIdToScore = playerIdToScore;
            BitsMatchId = bitsMatchId;
        }

        public Dictionary<string, int> PlayerIdToScore { get; private set; }
        public int BitsMatchId { get; private set; }
    }
}