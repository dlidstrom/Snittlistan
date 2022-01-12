using Snittlistan.Web.Areas.V2.Domain;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class RosterHeaderViewModel
{
    public RosterHeaderViewModel()
    {
    }

    public RosterHeaderViewModel(
        string? rosterId,
        string? team,
        string? teamLevel,
        string? location,
        string? opponent,
        DateTime date,
        OilPatternInformation oilPattern,
        string? matchResultId,
        bool matchTimeChanged)
    {
        RosterId = rosterId;
        Team = team;
        TeamLevel = teamLevel;
        Location = location;
        Opponent = opponent;
        Date = date;
        OilPattern = oilPattern;
        MatchResultId = matchResultId;
        MatchTimeChanged = matchTimeChanged;
    }

    public string? RosterId { get; set; }

    public string? Team { get; set; }

    public string? TeamLevel { get; set; }

    public string? Location { get; set; }

    public string? Opponent { get; set; }

    public DateTime Date { get; set; }

    public OilPatternInformation? OilPattern { get; set; }

    public string? MatchResultId { get; set; }

    public bool MatchTimeChanged { get; set; }
}
