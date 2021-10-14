#nullable enable

namespace Snittlistan.Web.Models
{
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class MatchRegisteredEmail : EmailBase
    {
        public MatchRegisteredEmail(
            string team,
            string opponent,
            int score,
            int opponentScore,
            ResultSeriesReadModel resultSeriesReadModel,
            ResultHeaderReadModel resultHeaderReadModel)
            : base("MatchRegistered", OwnerEmail, "Match har registrerats")
        {
            Team = team;
            Opponent = opponent;
            Score = score;
            OpponentScore = opponentScore;
            ResultSeriesReadModel = resultSeriesReadModel;
            ResultHeaderReadModel = resultHeaderReadModel;
        }

        public string Team { get; }
        public string Opponent { get; }
        public int Score { get; }
        public int OpponentScore { get; }
        public ResultSeriesReadModel ResultSeriesReadModel { get; }
        public ResultHeaderReadModel ResultHeaderReadModel { get; }
    }
}
