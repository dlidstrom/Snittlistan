using System.Linq;
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
			Session.Store(new MatchRepository().LoadAll(0, 1).Single());
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();
			match = Session.Query<Match>().Single();
		}

		[Fact]
		public void PinscoreForPlayer()
		{
			match.AwayTeam.PinscoreForPlayer("Peter Sjöberg").ShouldBe(787);
		}

		[Fact]
		public void PinscoreForTeam()
		{
			match.AwayTeam.Pins.ShouldBe(6216);
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