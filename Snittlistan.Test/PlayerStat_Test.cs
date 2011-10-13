using System;
using System.Linq;
using MvcContrib.TestHelper;
using Snittlistan.Infrastructure.Indexes;
using Snittlistan.Models;
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
			WaitForNonStaleResults<Match>();
			Matches_PlayerStats.Results stats = Session.Query<Matches_PlayerStats.Results, Matches_PlayerStats>()
				.Single(s => s.Player == "Mikael Axelsson");

			// Assert
			stats.Count.ShouldBe(4);
			stats.TotalPins.ShouldBe(845);
			stats.Max.ShouldBe(223);
			stats.Min.ShouldBe(202);
		}
	}
}
