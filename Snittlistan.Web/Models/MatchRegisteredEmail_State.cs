#nullable enable

using Postal;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Models;

public class MatchRegisteredEmail_State : EmailState
{
    public MatchRegisteredEmail_State(
        string from,
        string to,
        string subject,
        string team,
        string opponent,
        int score,
        int opponentScore,
        ResultSeriesReadModel resultSeriesReadModel,
        ResultHeaderReadModel resultHeaderReadModel,
        Uri userProfileLink)
        : base(from, to, BccEmail, subject, userProfileLink)
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

    public override Email CreateEmail()
    {
        return new MatchRegisteredEmail(
            Team,
            Opponent,
            Score,
            OpponentScore,
            ResultSeriesReadModel,
            ResultHeaderReadModel);
    }
}
