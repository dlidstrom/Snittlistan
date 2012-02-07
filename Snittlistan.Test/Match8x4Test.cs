namespace Snittlistan.Test
{
    using MvcContrib.TestHelper;
    using Snittlistan.Models;
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
            match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
        }

        [Fact]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.AwayTeam);
        }

        [Fact]
        public void LaneScores()
        {
            match.HomeTeam.Score.ShouldBe(13);
        }

        [Fact]
        public void Teams()
        {
            match.HomeTeam.Name.ShouldBe("Sollentuna Bwk");
            match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
        }
    }
}