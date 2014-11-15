using Snittlistan.Web.DomainEvents;

namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    public class MatchRegisteredEvent : IDomainEvent
    {
        public MatchRegisteredEvent(string rosterId, int score, int opponentScore)
        {
            RosterId = rosterId;
            Score = score;
            OpponentScore = opponentScore;
        }

        public string RosterId { get; private set; }

        public int Score { get; private set; }

        public int OpponentScore { get; private set; }
    }
}