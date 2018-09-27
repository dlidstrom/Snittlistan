using EventStoreLite;
using Raven.Imports.Newtonsoft.Json;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class ResultHeaderReadModel : IReadModel
    {
        public ResultHeaderReadModel(
            Roster roster,
            string aggregateId,
            int teamScore,
            int opponentScore)
        {
            Id = IdFromBitsMatchId(roster.BitsMatchId, roster.Id);
            SetValues(roster, aggregateId, teamScore, opponentScore);
            MatchCommentary = string.Empty;
            BodyText = new string[0];
        }

        [JsonConstructor]
        private ResultHeaderReadModel()
        {
            MatchCommentary = string.Empty;
            BodyText = new string[0];
        }

        public string AggregateId { get; private set; }

        /// <summary>
        /// The BITS match id.
        /// </summary>
        public string Id { get; private set; }

        public int OpponentScore { get; private set; }

        public string RosterId { get; private set; }

        public int Season { get; private set; }

        public int TeamScore { get; private set; }

        public string MatchCommentary { get; private set; }

        public string[] BodyText { get; private set; }

        public static string IdFromBitsMatchId(int bitsMatchId, string rosterId)
        {
            if (bitsMatchId != 0)
                return "ResultHeader-" + bitsMatchId;
            return $"ResultHeader-R{rosterId.Substring(8)}";
        }

        public void SetValues(Roster roster, string aggregateId, int teamScore, int opponentScore)
        {
            Season = roster.Season;
            AggregateId = aggregateId;
            RosterId = roster.Id;
            TeamScore = teamScore;
            OpponentScore = opponentScore;
        }

        public void SetMatchCommentary(string matchCommentary, string[] bodyText)
        {
            MatchCommentary = matchCommentary;
            BodyText = bodyText;
        }
    }
}