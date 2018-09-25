namespace Snittlistan.Web.Areas.V2.Domain
{
    public class OilPatternInformation
    {
        public static readonly OilPatternInformation Empty =
            new OilPatternInformation(string.Empty, string.Empty);

        public OilPatternInformation(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; private set; }

        public string Url { get; private set; }
    }
}