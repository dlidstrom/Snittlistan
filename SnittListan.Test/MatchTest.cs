using System.Linq;
using MvcContrib.TestHelper;
using SnittListan.Models;
using Xunit;

namespace SnittListan.Test
{
	public class MatchTest : DbTest
	{
		private readonly Match match;

		public MatchTest()
		{
			Session.Store(new MatchRepository().LoadAll(0, 1).Single());
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();
			match = Session.Query<Match>().Single();
		}

		[Fact]
		public void PinscoreForPlayer()
		{
			match.PinscoreForPlayer("Peter Sjöberg").ShouldBe(787);
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
		public void LaneScores()
		{
			match.LaneScoreForTeam().ShouldBe(6);
			match.LaneScoreForTeam(1).ShouldBe(2);
			match.LaneScoreForTeam(2).ShouldBe(2);
			match.LaneScoreForTeam(3).ShouldBe(1);
			match.LaneScoreForTeam(4).ShouldBe(1);
			match.OppTeamLaneScore.ShouldBe(13);
		}

		[Fact]
		public void SeriesPlayed()
		{
			match.NumberOfSeries.ShouldBe(4);
		}

		[Fact]
		public void Teams()
		{
			match.HomeTeam.ShouldBe("Sollentuna Bwk");
			match.OppTeam.ShouldBe("Fredrikshof IF");
		}
	}
}