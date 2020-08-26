namespace Snittlistan.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class BitsClientTest
    {
        [Test]
        public async Task ParsesTeam()
        {
            // Act
            Web.Infrastructure.Bits.Contracts.TeamResult[] team = await BitsGateway.GetTeam(51538, 2019);

            // Assert
            Assert.That(team, Has.Length.EqualTo(3));
            Assert.That(team[0].TeamId, Is.EqualTo(185185));
            Assert.That(team[0].TeamName, Is.EqualTo("Fredrikshof IF BK"));
            Assert.That(team[0].TeamAlias, Is.EqualTo("Fredrikshof IF BK A"));
            Assert.That(team[1].TeamId, Is.EqualTo(185186));
            Assert.That(team[1].TeamName, Is.EqualTo("Fredrikshof IF BK B"));
            Assert.That(team[1].TeamAlias, Is.EqualTo("Fredrikshof IF BK B"));
            Assert.That(team[2].TeamId, Is.EqualTo(185187));
            Assert.That(team[2].TeamName, Is.EqualTo("Fredrikshof IF BK F"));
            Assert.That(team[2].TeamAlias, Is.EqualTo("Fredrikshof IF BK F"));
        }

        [Test]
        public async Task ParsesDivisions()
        {
            // Act
            Web.Infrastructure.Bits.Contracts.DivisionResult[] divisions = await BitsGateway.GetDivisions(185185, 2019);

            // Assert
            Assert.That(divisions, Has.Length.EqualTo(1));
            Assert.That(divisions[0].DivisionId, Is.EqualTo(8));
            Assert.That(divisions[0].DivisionName, Is.EqualTo("Div 1 Södra Svealand"));
        }

        [Test]
        public async Task ParsesMatchRounds()
        {
            // Act
            Web.Infrastructure.Bits.Contracts.MatchRound[] matches = await BitsGateway.GetMatchRounds(185567, 684, 2019);

            // Assert
            Assert.That(matches, Has.Length.EqualTo(15));
            Assert.That(matches[0].MatchRoundId, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(Players))]
        public async Task ParsesPlayers(int clubId, int length, string licNbr)
        {
            // Act
            Web.Infrastructure.Bits.Contracts.PlayerResult players = await BitsGateway.GetPlayers(clubId);

            // Assert
            Assert.That(players.Data, Has.Length.EqualTo(length));
            Assert.That(players.Data[0].LicNbr, Is.EqualTo(licNbr));
        }

        private static IEnumerable<TestCaseData> Players
        {
            get
            {
                // may need updating, perhaps skip length entirely?
                yield return new TestCaseData(51538, 40, "M010982KRI01");
                yield return new TestCaseData(4494, 27, "K081082CAR01");
            }
        }
    }
}
