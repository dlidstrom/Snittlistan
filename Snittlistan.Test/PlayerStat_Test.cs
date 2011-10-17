using System.Linq;
using System.Threading;
using MvcContrib.TestHelper;
using Snittlistan.Infrastructure.Indexes;
using Xunit;

namespace Snittlistan.Test
{
	public class PlayerStat_Test : DbTest
	{
		[Fact]
		public void VerifyIndex()
		{
			// Arrange
			var match = DbSeed.CreateMatch();
			Session.Store(match);
			Session.SaveChanges();

			// Act
			var stats = Session.Query<Matches_PlayerStats.Results, Matches_PlayerStats>()
				.Customize(c => c.WaitForNonStaleResults())
				.SingleOrDefault(s => s.Player == "Mikael Axelsson");

			// Assert
			stats.ShouldNotBeNull("Failed to read from index");
			stats.Series.ShouldBe(4);
			stats.Pins.ShouldBe(845.0);
			stats.Max.ShouldBe(223);
			stats.Min.ShouldBe(202);
			stats.Strikes.ShouldBe(5);
			stats.Misses.ShouldBe(2);
			stats.OnePinMisses.ShouldBe(1);
			stats.Splits.ShouldBe(2);
			stats.Average.ShouldBe(211.25);
		}
	}
}
