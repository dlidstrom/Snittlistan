namespace Snittlistan.Test
{
    using System.Linq;
    using System.Threading;
    using MvcContrib.TestHelper;
    using Snittlistan.Infrastructure.Indexes;
    using Xunit;

    public class PlayerStat_Test : DbTest
    {
        [Fact]
        public void VerifyIndex8x4()
        {
            // Arrange
            Session.Store(DbSeed.Create8x4Match());
            Session.SaveChanges();

            // Act
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResults())
                .SingleOrDefault(s => s.Player == "Mikael Axelsson");

            // Assert
            stats.ShouldNotBeNull("Failed to read from index");
            stats.Series.ShouldBe(4.0);
            stats.Score.ShouldBe(3.0);
            stats.Pins.ShouldBe(845.0);
            stats.Max.ShouldBe(223);
            stats.Strikes.ShouldBe(5.0);
            stats.Misses.ShouldBe(2.0);
            stats.OnePinMisses.ShouldBe(1.0);
            stats.Splits.ShouldBe(2.0);
            stats.AveragePins.ShouldBe(211.25);
            stats.CoveredAll.ShouldBe(1);
        }

        [Fact]
        public void VerifyIndex4x4()
        {
            // Arrange
            Session.Store(DbSeed.Create4x4Match());
            Session.SaveChanges();

            // Act
            var stats = Session.Query<Matches_PlayerStats.Result, Matches_PlayerStats>()
                .Customize(c => c.WaitForNonStaleResults())
                .SingleOrDefault(s => s.Player == "Lars Norbeck");

            // Assert
            stats.ShouldNotBeNull("Failed to read from index");
            stats.Series.ShouldBe(4.0);
            stats.Score.ShouldBe(4.0);
            stats.Pins.ShouldBe(717.0);
            stats.Max.ShouldBe(231);
            stats.AveragePins.ShouldBe(179.25);
        }
    }
}