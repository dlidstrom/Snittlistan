namespace Snittlistan.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V2.Domain;

    [TestFixture]
    public class BitsParser_Header
    {
        [TestCaseSource(nameof(TestCases))]
        public async Task TestParseHeader(
            int bitsMatchId,
            int homeTeamId,
            DateTime date,
            string team,
            string opponent,
            int turn,
            string oilPatternName,
            string oilPatternUrl,
            string location)
        {
            // Arrange
            Web.Infrastructure.Bits.Contracts.HeadInfo content = await BitsGateway.GetHeadInfo(bitsMatchId);

            // Act
            ParseHeaderResult header = BitsParser.ParseHeader(content, homeTeamId);

            // Assert
            Assert.That(header.Date, Is.EqualTo(date));
            Assert.That(header.Team, Is.EqualTo(team));
            Assert.That(header.Opponent, Is.EqualTo(opponent));
            Assert.That(header.Turn, Is.EqualTo(turn));
            Assert.That(header.OilPattern.Name, Is.EqualTo(oilPatternName));
            Assert.That(header.OilPattern.Url, Is.EqualTo(oilPatternUrl));
            Assert.That(header.Location, Is.EqualTo(location));
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    3148486,
                    4494,
                    new DateTime(2018, 1, 21, 16, 20, 0),
                    "Värtans IK B",
                    "IK Makkabi - B1",
                    13,
                    "High Street",
                    "https://bits.swebowl.se/MiscDisplay/Oilpattern/51",
                    "Stockholm - Brännkyrka");
                yield return new TestCaseData(
                    3152235,
                    51538,
                    new DateTime(2017, 10, 28, 12, 20, 0),
                    "Fredrikshof IF BK A",
                    "BajenFans BF A",
                    8,
                    "ABT#2",
                    "https://bits.swebowl.se/MiscDisplay/Oilpattern/61",
                    "Stockholm - Bowl-O-Rama");
                yield return new TestCaseData(
                    3048477,
                    1660,
                    new DateTime(2013, 4, 20),
                    "Fredrikshof IF F",
                    "BwK Ankaret F",
                    22,
                    "Ingen OljeProfil",
                    string.Empty,
                    "Stockholm - Lidingö");
            }
        }
    }
}