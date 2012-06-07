namespace Snittlistan.Test
{
    using Models;
    using MvcContrib.TestHelper;
    using Xunit;

    public class Match4x4Test : DbTest
    {
        private readonly Match4x4 match;

        public Match4x4Test()
        {
            match = DbSeed.Create4x4Match();
            Session.Store(match);
            Session.SaveChanges();
            match = Session.Load<Match4x4>(match.Id);
        }

        [Fact]
        public void PinscoreForPlayer()
        {
            match.HomeTeam.PinsForPlayer("Lars Norbeck").ShouldBe(717);
        }

        [Fact]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.HomeTeam);
        }

        [Fact]
        public void LaneScores()
        {
            match.HomeTeam.Score.ShouldBe(6);
        }

        [Fact]
        public void Teams()
        {
            match.HomeTeam.Name.ShouldBe("Fredrikshof C");
            match.AwayTeam.Name.ShouldBe("Librex");
        }
    }
}