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

        public static OilPatternInformation Create(string matchOilPatternName, int matchOilPatternId)
        {
            return new OilPatternInformation(
                matchOilPatternName,
                matchOilPatternId != 0 ? $"https://bits.swebowl.se/MiscDisplay/Oilpattern/{matchOilPatternId}" : string.Empty);
        }

        public string Name { get; private set; }

        public string Url { get; private set; }
    }
}