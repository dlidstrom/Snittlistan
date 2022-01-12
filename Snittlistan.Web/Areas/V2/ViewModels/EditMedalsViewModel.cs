
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class EditMedalsViewModel
{
    public EditMedalsViewModel(
        string name,
        EliteMedals.EliteMedal.EliteMedalValue existingMedal,
        int capturedSeason,
        int currentSeason)
    {
        Name = name;
        ExistingMedal = existingMedal;
        CapturedSeason = capturedSeason;
        CurrentSeason = currentSeason;
    }

    public string Name { get; }

    public EliteMedals.EliteMedal.EliteMedalValue ExistingMedal { get; }

    public int CapturedSeason { get; }

    public int CurrentSeason { get; }
}
