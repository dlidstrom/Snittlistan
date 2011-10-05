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
			match = TestData.CreateMatch();
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
		public void PinscoreForTeam()
		{
			match.AwayTeam.Pins().ShouldBe(6216);
			match.AwayTeam.PinsFor(1).ShouldBe(1598);
			match.AwayTeam.PinsFor(2).ShouldBe(1573);
			match.AwayTeam.PinsFor(3).ShouldBe(1505);
			match.AwayTeam.PinsFor(4).ShouldBe(1540);
		}

		[Fact]
		public void LaneScores()
		{
			match.HomeTeam.Score.ShouldBe(13);
			match.AwayTeam.ScoreFor(1).ShouldBe(2);
			match.AwayTeam.ScoreFor(2).ShouldBe(2);
			match.AwayTeam.ScoreFor(3).ShouldBe(1);
			match.AwayTeam.ScoreFor(4).ShouldBe(1);
			match.AwayTeam.Score.ShouldBe(6);
		}

		[Fact]
		public void Teams()
		{
			match.HomeTeam.Name.ShouldBe("Sollentuna Bwk");
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
		}
	}
}