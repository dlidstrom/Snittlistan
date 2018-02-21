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
        [TestCaseSource(typeof(MatchSchemeData), nameof(MatchSchemeData.TestCases))]
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
            Assert.That(matchScheme.Matches, Has.Length.EqualTo(expectedItems.Length));
            for (var i = 0; i < expectedItems.Length; i++)
            {
                var item = matchScheme.Matches[i];
                var expectedItem = expectedItems[i];
                Assert.That(item.Turn, Is.EqualTo(expectedItem.Turn), $"Item {item.RowFromHtml} {item.Teams} Turn");
                Assert.That(item.Date, Is.EqualTo(expectedItem.Date), $"Item {item.RowFromHtml}: {item.Teams} Date");
                Assert.That(item.DateChanged, Is.EqualTo(expectedItem.DateChanged), $"Item {item.RowFromHtml}: {item.Teams} DateChanged");
                Assert.That(item.BitsMatchId, Is.EqualTo(expectedItem.BitsMatchId), $"Item {item.RowFromHtml}: {item.Teams} BitsMatchId");
                Assert.That(item.Teams, Is.EqualTo(expectedItem.Teams), $"Item {item.RowFromHtml}: {item.Teams} Teams");
                Assert.That(item.MatchResult, Is.EqualTo(expectedItem.MatchResult), $"Item {item.RowFromHtml}: {item.Teams} MatchResult");
                Assert.That(item.OilPatternName, Is.EqualTo(expectedItem.OilPatternName), $"Item {item.RowFromHtml}: {item.Teams} OilPatternName");
                Assert.That(item.OilPatternId, Is.EqualTo(expectedItem.OilPatternId), $"Item {item.RowFromHtml}: {item.Teams} OilPatternId");
                Assert.That(item.Location, Is.EqualTo(expectedItem.Location), $"Item {item.RowFromHtml}: {item.Teams} Location");
                Assert.That(item.LocationUrl, Is.EqualTo(expectedItem.LocationUrl), $"Item {item.RowFromHtml}: {item.Teams} LocationUrl");
            }
        }
    }
}
