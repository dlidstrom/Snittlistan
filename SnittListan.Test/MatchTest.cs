using System.Linq;
using MvcContrib.TestHelper;
using SnittListan.Models;
using Xunit;

namespace SnittListan.Test
{
	public class MatchTest
	{
		private readonly Match match;

		public MatchTest()
		{
			match = new MatchRepository().LoadAll(0, 1).Single();
		}

		[Fact]
		public void PinscoreForPlayer()
		{
			match.PinscoreForPlayer("P. Sjöberg").ShouldBe(787);
		}

		[Fact]
		public void PinscoreForTeam()
		{
			match.PinScoreForTeam().ShouldBe(6216);
			match.PinscoreForTeam(1).ShouldBe(1598);
			match.PinscoreForTeam(2).ShouldBe(1573);
			match.PinscoreForTeam(3).ShouldBe(1505);
			match.PinscoreForTeam(4).ShouldBe(1540);
		}

		[Fact]
		public void LaneScoreForTeam()
		{
			match.LaneScoreForTeam().ShouldBe(6);
			match.LaneScoreForTeam(1).ShouldBe(2);
			match.LaneScoreForTeam(2).ShouldBe(2);
			match.LaneScoreForTeam(3).ShouldBe(1);
			match.LaneScoreForTeam(4).ShouldBe(1);
		}

		[Fact]
		public void SeriesPlayed()
		{
			match.NumberOfSeries.ShouldBe(4);
		}
	}
}
