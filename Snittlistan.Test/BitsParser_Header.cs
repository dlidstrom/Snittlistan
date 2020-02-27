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
            HashSet<string> possibleTeams,
            DateTime date,
            string team,
            string opponent,
            int turn,
            string oilPatternName,
            string oilPatternUrl)
        {
            // Arrange
            var content = await BitsGateway.GetHeadInfo(bitsMatchId);

            // Act
            var header = BitsParser.ParseHeader(content);

            // Assert
            Assert.That(header.Date, Is.EqualTo(date));
            Assert.That(header.Team, Is.EqualTo(team));
            Assert.That(header.Opponent, Is.EqualTo(opponent));
            Assert.That(header.Turn, Is.EqualTo(turn));
            Assert.That(header.OilPattern.Name, Is.EqualTo(oilPatternName));
            Assert.That(header.OilPattern.Url, Is.EqualTo(oilPatternUrl));
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    3148486,
                    new HashSet<string> { "Värtans IK B" },
                    new DateTime(2018, 1, 21, 16, 20, 0),
                    "Värtans IK B",
                    "IK Makkabi",
                    13,
                    "High Street",
                    "http://bits.swebowl.se/OilPattern.aspx?OilPatternId=51");
                yield return new TestCaseData(
                    3152235,
                    new HashSet<string> { "Fredrikshof IF BK F" },
                    new DateTime(2017, 10, 28, 12, 20, 0),
                    "Fredrikshof IF BK F",
                    "BajenFans BF",
                    8,
                    "ABT#2",
                    "http://bits.swebowl.se/OilPattern.aspx?OilPatternId=61");
                yield return new TestCaseData(
                    3048477,
                    new HashSet<string> { "Fredrikshof IF F" },
                    new DateTime(2013, 4, 20),
                    "Fredrikshof IF F",
                    "BwK Ankaret F",
                    22,
                    "Ingen OljeProfil",
                    string.Empty);
            }
        }
    }
}