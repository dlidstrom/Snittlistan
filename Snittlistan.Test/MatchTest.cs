using System;
using System.Collections.Generic;
using System.Globalization;
using MvcContrib.TestHelper;
using Snittlistan.Models;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchTest : DbTest
	{
		private readonly Match match;

		public MatchTest()
		{
			match = DbSeed.CreateMatch();
			Session.Store(match);
			Session.SaveChanges();
			match = Session.Load<Match>(match.Id);
		}

		[Fact]
		public void PinscoreForPlayer()
		{
			match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
		}

		[Fact]
		public void VerifyValues()
		{
			TestData.VerifyMatch(match);
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