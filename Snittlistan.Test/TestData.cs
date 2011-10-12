namespace Snittlistan.Test
{
	using System.Linq;
	using MvcContrib.TestHelper;
	using Snittlistan.Models;

	public class TestData
	{
		public static void VerifyMatch(Match match)
		{
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
			match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
			match.AwayTeam.Series.Count().ShouldBe(4);
			match.AwayTeam.Pins().ShouldBe(6216);
			match.AwayTeam.PinsFor(1).ShouldBe(1598);
			match.AwayTeam.PinsFor(2).ShouldBe(1573);
			match.AwayTeam.PinsFor(3).ShouldBe(1505);
			match.AwayTeam.PinsFor(4).ShouldBe(1540);
			match.AwayTeam.ScoreFor(1).ShouldBe(2);
			match.AwayTeam.ScoreFor(2).ShouldBe(2);
			match.AwayTeam.ScoreFor(3).ShouldBe(1);
			match.AwayTeam.ScoreFor(4).ShouldBe(1);
			match.AwayTeam.Score.ShouldBe(6);
		}
	}
}
