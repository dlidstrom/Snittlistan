using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Models;

namespace Snittlistan.Test
{
    [TestFixture]
    public class Match8x4Test : DbTest
    {
        private Match8x4 match;

        protected override void OnSetUp()
        {
            match = DbSeed.Create8x4Match();
            Session.Store(match);
            Session.SaveChanges();
            match = Session.Load<Match8x4>(match.Id);
        }

        [Test]
        public void PinscoreForPlayer()
        {
            Assert.That(match.AwayTeam.PinsForPlayer("Peter Sjöberg"), Is.EqualTo(787));
        }

        [Test]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Test]
        public void LaneScores()
        {
            Assert.That(match.HomeTeam.Score, Is.EqualTo(13));
        }

        [Test]
        public void Teams()
        {
            Assert.That(match.HomeTeam.Name, Is.EqualTo("Sollentuna Bwk"));
            Assert.That(match.AwayTeam.Name, Is.EqualTo("Fredrikshof IF"));
        }
    }
}