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
			IndexCreator.CreateIndexes(Store);

			// Act
			var match = DbSeed.CreateMatch();
			Session.Store(match);
			Session.SaveChanges();
			var stats = Session.Query<Matches_PlayerStats.Results, Matches_PlayerStats>()
				.Customize(c => c.WaitForNonStaleResults())
				.SingleOrDefault(s => s.Player == "Mikael Axelsson");

			// Assert
			stats.ShouldNotBeNull("Failed to read from index");
			stats.Count.ShouldBe(4);
			stats.TotalPins.ShouldBe(845);
			stats.Max.ShouldBe(223);
			stats.Min.ShouldBe(202);
		}
	}
}
