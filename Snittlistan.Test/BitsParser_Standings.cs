using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test
{
    [TestFixture]
    public class BitsParser_Standings
    {
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "Snittlistan.Test.BitsResult.FredrikshofIF-standings.html",
                    "http://bits.swebowl.se/Matches/Standings.aspx?DivisionId=84&amp;SeasonId=2017&amp;LeagueId=10&amp;LevelId=8",
                    new []
                    {
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "BajenFans BF B",
                            Matches = 14,
                            Win = 10,
                            Draw = 0,
                            Loss = 4,
                            Total = "168 - 111",
                            Diff = 57,
                            Points = 20,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "Fredrikshof IF BK B",
                            Matches = 14,
                            Win = 9,
                            Draw = 1,
                            Loss = 4,
                            Total = "162 - 116",
                            Diff = 46,
                            Points = 19,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "Matteus-Pojkarna BK B",
                            Matches = 13,
                            Win = 8,
                            Draw = 2,
                            Loss = 3,
                            Total = "142 - 117",
                            Diff = 25,
                            Points = 18,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "Djurgårdens IF C",
                            Matches = 14,
                            Win = 8,
                            Draw = 1,
                            Loss = 5,
                            Total = "160 - 117",
                            Diff = 43,
                            Points = 17,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "BK Runan B",
                            Matches = 13,
                            Win = 6,
                            Draw = 1,
                            Loss = 6,
                            Total = "127 - 129",
                            Diff = -2,
                            Points = 13,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "KK Strike",
                            Matches = 14,
                            Win = 6,
                            Draw = 1,
                            Loss = 7,
                            Total = "126 - 151",
                            Diff = -25,
                            Points = 13,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "BK Scott",
                            Matches = 12,
                            Win = 5,
                            Draw = 1,
                            Loss = 6,
                            Total = "113 - 126",
                            Diff = -13,
                            Points = 11,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "AIK DB",
                            Matches = 14,
                            Win = 4,
                            Draw = 3,
                            Loss = 7,
                            Total = "122 - 158",
                            Diff = -36,
                            Points = 11,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "DN/Expressen B",
                            Matches = 13,
                            Win = 4,
                            Draw = 2,
                            Loss = 7,
                            Total = "120 - 137",
                            Diff = -17,
                            Points = 10,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "Jakobsbergs BS",
                            Matches = 14,
                            Win = 4,
                            Draw = 2,
                            Loss = 8,
                            Total = "125 - 154",
                            Diff = -29,
                            Points = 10,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = null,
                            Name = "BK Mercur",
                            Matches = 13,
                            Win = 3,
                            Draw = 0,
                            Loss = 10,
                            Total = "105 - 154",
                            Diff = -49,
                            Points = 6,
                            DividerSolid = false
                        }
                    });

                yield return new TestCaseData(
                    "Snittlistan.Test.BitsResult.VärtansIK-standings.html",
                    "http://bits.swebowl.se/Matches/Standings.aspx?DivisionId=9&amp;SeasonId=2017&amp;LeagueId=1&amp;LevelId=3",
                    new[]
                    {
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Sandvikens AIK",
                            Matches = 14,
                            Win = 10,
                            Draw = 3,
                            Loss = 1,
                            Total = "179 - 100",
                            Diff = 79,
                            Points = 23,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Uppsala BC 90",
                            Matches = 14,
                            Win = 9,
                            Draw = 0,
                            Loss = 5,
                            Total = "165 - 111",
                            Diff = 54,
                            Points = 18,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Turebergs IF",
                            Matches = 15,
                            Win = 7,
                            Draw = 1,
                            Loss = 7,
                            Total = "135 - 163",
                            Diff = -28,
                            Points = 15,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Värtans IK",
                            Matches = 14,
                            Win = 5,
                            Draw = 2,
                            Loss = 7,
                            Total = "126 - 153",
                            Diff = -27,
                            Points = 12,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Sollentuna BwK",
                            Matches = 15,
                            Win = 4,
                            Draw = 0,
                            Loss = 11,
                            Total = "134 - 163",
                            Diff = -29,
                            Points = 8,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp A",
                            Name = "Bålsta BC",
                            Matches = 14,
                            Win = 4,
                            Draw = 0,
                            Loss = 10,
                            Total = "112 - 167",
                            Diff = -55,
                            Points = 8,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "Gävle KK",
                            Matches = 14,
                            Win = 9,
                            Draw = 2,
                            Loss = 3,
                            Total = "175 - 104",
                            Diff = 71,
                            Points = 20,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "Domnarvets BS",
                            Matches = 15,
                            Win = 8,
                            Draw = 1,
                            Loss = 6,
                            Total = "146 - 152",
                            Diff = -6,
                            Points = 17,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "BK Klossen",
                            Matches = 14,
                            Win = 8,
                            Draw = 0,
                            Loss = 6,
                            Total = "160 - 117",
                            Diff = 43,
                            Points = 16,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "BK Rättvik",
                            Matches = 14,
                            Win = 7,
                            Draw = 0,
                            Loss = 7,
                            Total = "151 - 129",
                            Diff = 22,
                            Points = 14,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "BK Enjoy",
                            Matches = 14,
                            Win = 3,
                            Draw = 1,
                            Loss = 10,
                            Total = "104 - 175",
                            Diff = -71,
                            Points = 7,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp B",
                            Name = "Falu BK",
                            Matches = 13,
                            Win = 0,
                            Draw = 0,
                            Loss = 13,
                            Total = "60 - 200",
                            Diff = -140,
                            Points = 0,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "BK Taifun",
                            Matches = 15,
                            Win = 10,
                            Draw = 2,
                            Loss = 3,
                            Total = "173 - 125",
                            Diff = 48,
                            Points = 22,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "BK Trol",
                            Matches = 14,
                            Win = 8,
                            Draw = 0,
                            Loss = 6,
                            Total = "157 - 121",
                            Diff = 36,
                            Points = 16,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "Hammarby IF",
                            Matches = 14,
                            Win = 7,
                            Draw = 2,
                            Loss = 5,
                            Total = "150 - 127",
                            Diff = 23,
                            Points = 16,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "Wåxnäs BC",
                            Matches = 15,
                            Win = 7,
                            Draw = 2,
                            Loss = 6,
                            Total = "146 - 152",
                            Diff = -6,
                            Points = 16,
                            DividerSolid = false
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "BK Glam F1",
                            Matches = 13,
                            Win = 7,
                            Draw = 1,
                            Loss = 5,
                            Total = "134 - 124",
                            Diff = 10,
                            Points = 15,
                            DividerSolid = true
                        },
                        new ParseStandingsResult.StandingsItem
                        {
                            Group = "Grupp C",
                            Name = "BK Kasi",
                            Matches = 13,
                            Win = 4,
                            Draw = 3,
                            Loss = 6,
                            Total = "117 - 141",
                            Diff = -24,
                            Points = 11,
                            DividerSolid = false
                        }
                    });
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void ParsesStandings(string resourceName, string expectedDirectLink, ParseStandingsResult.StandingsItem[] expectedItems)
        {
            // Act
            ParseStandingsResult standings;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    standings = BitsParser.ParseStandings(content);
                }
            }

            // Assert
            Assert.That(standings.DirectLink, Is.EqualTo(expectedDirectLink), "DirectLink");
            Assert.That(standings.Items, Has.Length.EqualTo(expectedItems.Length));
            for (var i = 0; i < standings.Items.Length; i++)
            {
                var item = standings.Items[i];
                var expectedItem = expectedItems[i];
                Assert.That(item.Group, Is.EqualTo(expectedItem.Group), $"Item {i} Group");
                Assert.That(item.Name, Is.EqualTo(expectedItem.Name), $"Item {i} Name");
                Assert.That(item.Matches, Is.EqualTo(expectedItem.Matches), $"Item {i} Matches");
                Assert.That(item.Win, Is.EqualTo(expectedItem.Win), $"Item {i} Win");
                Assert.That(item.Draw, Is.EqualTo(expectedItem.Draw), $"Item {i} Draw");
                Assert.That(item.Loss, Is.EqualTo(expectedItem.Loss), $"Item {i} Loss");
                Assert.That(item.Total, Is.EqualTo(expectedItem.Total), $"Item {i} Total");
                Assert.That(item.Diff, Is.EqualTo(expectedItem.Diff), $"Item {i} Diff");
                Assert.That(item.Points, Is.EqualTo(expectedItem.Points), $"Item {i} Points");
                Assert.That(item.DividerSolid, Is.EqualTo(expectedItem.DividerSolid), $"Item {i} DividerSolid");
            }
        }
    }
}