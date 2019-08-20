namespace Snittlistan.Web.Areas.V2.Controllers.SupporterTravet
{
    public static class SupporterTravetModels
    {
        public class Header
        {
            public Header(string id)
            {
                Id = id;
            }

            public string Id { get; }
        }

        public class Turn
        {
            public Turn(string index, Race[] races)
            {
                Index = index;
                Races = races;
            }

            public string Index { get; }
            public Race[] Races { get; }
        }

        public class Race
        {
            public Race(string raceIndex, RaceResult[] results)
            {
                RaceIndex = raceIndex;
                Results = results;
            }

            public string RaceIndex { get; }
            public RaceResult[] Results { get; }
        }

        public class RaceResult
        {
            public RaceResult(int horse, int odds)
            {
                Horse = horse;
                Odds = odds;
            }

            public int Horse { get; }
            public int Odds { get; }
        }
    }
}