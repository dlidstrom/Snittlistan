#nullable enable

using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Models;

public class MatchRegisteredEmail : EmailBase
{
    private readonly MatchRegisteredEmail_State _state;

    public MatchRegisteredEmail(
        string team,
        string opponent,
        int score,
        int opponentScore,
        ResultSeriesReadModel resultSeriesReadModel,
        ResultHeaderReadModel resultHeaderReadModel)
        : base("MatchRegistered")
    {
        _state = new(
            EmailState.OwnerEmail,
            EmailState.OwnerEmail,
            "Match har registrerats",
            team,
            opponent,
            score,
            opponentScore,
            resultSeriesReadModel,
            resultHeaderReadModel);
    }

    public string Team => _state.Team;

    public string Opponent => _state.Opponent;

    public int Score => _state.Score;

    public int OpponentScore => _state.OpponentScore;

    public ResultSeriesReadModel ResultSeriesReadModel => _state.ResultSeriesReadModel;

    public ResultHeaderReadModel ResultHeaderReadModel => _state.ResultHeaderReadModel;

    public override EmailState State => _state;
}
