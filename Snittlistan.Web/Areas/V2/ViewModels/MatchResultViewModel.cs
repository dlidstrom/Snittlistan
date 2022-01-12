namespace Snittlistan.Web.Areas.V2.ViewModels;
public class MatchResultViewModel
{
    public MatchResultViewModel()
    {
        Turns = new Dictionary<int, List<ResultHeaderViewModel>>();
    }

    public int SeasonStart { get; set; }

    public Dictionary<int, List<ResultHeaderViewModel>> Turns { get; set; }
}
