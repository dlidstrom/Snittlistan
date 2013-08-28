using Snittlistan.Web.Areas.V1.Models;
using Xunit;

namespace Snittlistan.Test
{
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
            Assert.Equal(717, match.HomeTeam.PinsForPlayer("Lars Norbeck"));
        }

        [Fact]
        public void VerifyValues()
        {
            TestData.VerifyTeam(match.HomeTeam);
        }

        [Fact]
        public void LaneScores()
        {
            Assert.Equal(6, match.HomeTeam.Score);
        }

        [Fact]
        public void Teams()
        {
            Assert.Equal("Fredrikshof C", match.HomeTeam.Name);
            Assert.Equal("Librex", match.AwayTeam.Name);
        }
    }
}