namespace Snittlistan.Test
{
    using Snittlistan.Web.Models;

    using Xunit;

    public class Match8x4Test : DbTest
    {
        private readonly Match8x4 match;

        public Match8x4Test()
        {
            match = DbSeed.Create8x4Match();
            Session.Store(match);
            Session.SaveChanges();
            match = Session.Load<Match8x4>(match.Id);
        }

        [Fact]
        public void PinscoreForPlayer()
        {
            Assert.Equal(787, match.AwayTeam.PinsForPlayer("Peter Sjöberg"));
        }

        [Fact]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Fact]
        public void LaneScores()
        {
            Assert.Equal(13, match.HomeTeam.Score);
        }

        [Fact]
        public void Teams()
        {
            Assert.Equal("Sollentuna Bwk", match.HomeTeam.Name);
            Assert.Equal("Fredrikshof IF", match.AwayTeam.Name);
        }
    }
}