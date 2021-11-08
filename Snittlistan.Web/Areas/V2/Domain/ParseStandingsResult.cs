#nullable enable

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class ParseStandingsResult
    {
        public ParseStandingsResult(string directLink, StandingsItem[] items)
        {
            DirectLink = directLink;
            Items = items;
        }

        public string DirectLink { get; }

        public StandingsItem[] Items { get; }

        public class StandingsItem
        {
            public string? Group { get; set; }
            public string? Name { get; set; }
            public int Matches { get; set; }
            public int Win { get; set; }
            public int Draw { get; set; }
            public int Loss { get; set; }
            public string? Total { get; set; }
            public int Diff { get; set; }
            public int Points { get; set; }
            public bool DividerSolid { get; set; }

            public override string ToString()
            {
                return $"{nameof(Group)}: {Group}, {nameof(Name)}: {Name}, {nameof(Matches)}: {Matches}, {nameof(Win)}: {Win}, {nameof(Draw)}: {Draw}, {nameof(Loss)}: {Loss}, {nameof(Total)}: {Total}, {nameof(Diff)}: {Diff}, {nameof(Points)}: {Points}, {nameof(DividerSolid)}: {DividerSolid}";
            }
        }
    }
}
