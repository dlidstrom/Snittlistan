namespace Snittlistan.Web.Areas.V2.ViewModels;

public class ViewTurnViewModel
{
    public ViewTurnViewModel(
        int turn,
        int season,
        RosterViewModel[] rosterViewModels,
        bool withAbsence,
        string headerImagePath)
    {
        Turn = turn;
        Season = season;
        RosterViewModels = rosterViewModels;
        WithAbsence = withAbsence;
        HeaderImagePath = headerImagePath;
    }

    public int Turn { get; }

    public int Season { get; }

    public RosterViewModel[] RosterViewModels { get; }

    public bool WithAbsence { get; }

    public string HeaderImagePath { get; }
}
