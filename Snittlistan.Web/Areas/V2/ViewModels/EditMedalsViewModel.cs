using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class EditMedalsViewModel
    {
        public EditMedalsViewModel(string name, EliteMedals.EliteMedal.EliteMedalValue existingMedal, int capturedSeason)
        {
            Name = name;
            ExistingMedal = existingMedal;
            CapturedSeason = capturedSeason;
        }

        public string Name { get; private set; }

        public EliteMedals.EliteMedal.EliteMedalValue ExistingMedal { get; private set; }

        public int CapturedSeason { get; private set; }
    }
}