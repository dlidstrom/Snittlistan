namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;

    public class OilPatternInformation : IEqualityComparer<OilPatternInformation>
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

        public bool Equals(OilPatternInformation x, OilPatternInformation y)
        {
            return Tuple.Create(x.Name, x.Url) == Tuple.Create(y.Name, y.Url);
        }

        public int GetHashCode(OilPatternInformation obj)
        {
            return Tuple.Create(Name, Url).GetHashCode();
        }

        public string Name { get; private set; }

        public string Url { get; private set; }
    }
}
