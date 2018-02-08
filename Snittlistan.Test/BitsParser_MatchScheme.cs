using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test
{
    [TestFixture]
    public class BitsParser_MatchScheme
    {
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "Snittlistan.Test.BitsResult.FredrikshofIF-matchScheme.html",
                    new[]
                    {
                        new ParseMatchSchemeResult.MatchItem
                        {
                            Turn = 1,
                            Date = new DateTime(2017, 9, 3, 10, 0, 0),
                            BitsMatchId = 3138419,
                            Teams = "BK Runan B - KK Strike",
                            MatchResult = "14 - 5",
                            OilPatternName = "Ingen OljeProfil",
                            OilPatternId = 0,
                            Location = "Stockholm - Täby",
                            LocationUrl = "HallSchemeAdminList.aspx?HallId=814&amp;SeasonId=0&amp;RoundId=1"
                        },
                        new ParseMatchSchemeResult.MatchItem
                        {
                            Turn = 1,
                            Date = new DateTime(2017, 9, 3, 10, 0, 0),
                            BitsMatchId = 3138420,
                            Teams = "BK Scott - DN/Expressen B",
                            MatchResult = "14 - 6",
                            OilPatternName = "Ingen OljeProfil",
                            OilPatternId = 0,
                            Location = "Stockholm - Birka",
                            LocationUrl = "HallSchemeAdminList.aspx?HallId=774&amp;SeasonId=0&amp;RoundId=1"
                        },
                        new ParseMatchSchemeResult.MatchItem
                        {
                            Turn = 1,
                            Date = new DateTime(2017, 9, 3, 10, 0, 0),
                            BitsMatchId = 3138523,
                            Teams = "Fredrikshof IF BK B - Djurgårdens IF C",
                            MatchResult = "14 - 6",
                            OilPatternName = "Ingen OljeProfil",
                            OilPatternId = 0,
                            Location = "Stockholm - Birka",
                            LocationUrl = "HallSchemeAdminList.aspx?HallId=774&amp;SeasonId=0&amp;RoundId=1"
                        },
                    });
                yield return new TestCaseData(
                    "Snittlistan.Test.BitsResult.VärtansIK-matchScheme.html");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void ParsesMatchScheme(string resourceName, ParseMatchSchemeResult.MatchItem[] expectedItems)
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
            //Assert.That(matchScheme.Matches, Has.Length.EqualTo(expectedItems.Length));
            for (var i = 0; i < expectedItems.Length; i++)
            {
                var item = matchScheme.Matches[i];
                var expectedItem = expectedItems[i];
                Assert.That(item.Turn, Is.EqualTo(expectedItem.Turn), $"Item {i} Turn");
                Assert.That(item.Date, Is.EqualTo(expectedItem.Date), $"Item {i} Date");
                Assert.That(item.BitsMatchId, Is.EqualTo(expectedItem.BitsMatchId), $"Item {i} BitsMatchId");
                Assert.That(item.Teams, Is.EqualTo(expectedItem.Teams), $"Item {i} Teams");
                Assert.That(item.MatchResult, Is.EqualTo(expectedItem.MatchResult), $"Item {i} MatchResult");
                Assert.That(item.OilPatternName, Is.EqualTo(expectedItem.OilPatternName), $"Item {i} OilPatternName");
                Assert.That(item.OilPatternId, Is.EqualTo(expectedItem.OilPatternId), $"Item {i} OilPatternId");
                Assert.That(item.Location, Is.EqualTo(expectedItem.Location), $"Item {i} Location");
                Assert.That(item.LocationUrl, Is.EqualTo(expectedItem.LocationUrl), $"Item {i} LocationUrl");
            }
        }
    }
}