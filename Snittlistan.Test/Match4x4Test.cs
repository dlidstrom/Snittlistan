namespace Snittlistan.Test
{
    using NUnit.Framework;
    using Snittlistan.Web.Areas.V1.Models;

    [TestFixture]
    public class Match4x4Test : DbTest
    {
        private Match4x4 match;

        protected override void OnSetUp()
        {
            match = DbSeed.Create4x4Match();
            Session.Store(match);
            Session.SaveChanges();
            match = Session.Load<Match4x4>(match.Id);
        }

        [Test]
        public void PinscoreForPlayer()
        {
            Assert.That(match.HomeTeam.PinsForPlayer("Lars Norbeck"), Is.EqualTo(717));
        }

        [Test]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.HomeTeam);
        }

        [Test]
        public void LaneScores()
        {
            Assert.That(match.HomeTeam.Score, Is.EqualTo(6));
        }

        [Test]
        public void Teams()
        {
            Assert.That(match.HomeTeam.Name, Is.EqualTo("Fredrikshof C"));
            Assert.That(match.AwayTeam.Name, Is.EqualTo("Librex"));
        }
    }
}