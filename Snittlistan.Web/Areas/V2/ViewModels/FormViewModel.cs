namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class FormViewModel
    {
        public FormViewModel(int season, PlayerFormViewModel[] playerForms)
        {
            Season = season;
            PlayerForms = playerForms;
        }

        public int Season { get; }

        public PlayerFormViewModel[] PlayerForms { get; }
    }
}