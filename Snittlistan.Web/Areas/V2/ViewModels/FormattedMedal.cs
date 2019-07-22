namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class FormattedMedal
    {
        public FormattedMedal(string description, string seasonSpan)
        {
            Description = description;
            SeasonSpan = seasonSpan;
        }

        public string Description { get; }

        public string SeasonSpan { get; }
    }
}