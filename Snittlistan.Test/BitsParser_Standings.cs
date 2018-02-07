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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
                        {
                            Group = null,
                            Name = "Fredrikshof IF BK B",
                            Matches = 14,
                            Win = 10,
                            Draw = 0,
                            Loss = 4,
                            Total = "168 - 111",
                            Diff = 57,
                            Points = 20,
                            DividerSolid = true
                        },
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
                        {
                            Group = null,
                            Name = "DN/Expressen B",
                            Matches = 14,
                            Win = 10,
                            Draw = 0,
                            Loss = 4,
                            Total = "168 - 111",
                            Diff = 57,
                            Points = 20,
                            DividerSolid = true
                        },
                        new ParseMatchSchemeResult.StandingsItem
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
                        new ParseMatchSchemeResult.StandingsItem
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
                        }
                    });

                yield return new TestCaseData(
                    "Snittlistan.Test.BitsResult.VärtansIK-standings.html",
                    "http://bits.swebowl.se/Matches/Standings.aspx?DivisionId=9&amp;SeasonId=2017&amp;LeagueId=1&amp;LevelId=3");
            }
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void DirectLink(string resourceName, string expectedDirectLink, ParseMatchSchemeResult.StandingsItem[] expectedItems)
        {
            // Act
            ParseMatchSchemeResult matchScheme;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    matchScheme = BitsParser.ParseMatchScheme(content);
                }
            }

            // Assert
            Assert.That(matchScheme.DirectLink, Is.EqualTo(expectedDirectLink), "DirectLink");
            Assert.That(matchScheme.Items, Has.Length.EqualTo(expectedItems.Length));
            for (var i = 0; i < matchScheme.Items.Length; i++)
            {
                var item = matchScheme.Items[i];
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