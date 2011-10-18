namespace Snittlistan.Test
{
	using System.Collections.Generic;
	using System.Linq;
	using MvcContrib.TestHelper;
	using Snittlistan.Models;

	public class TestData
	{
		private static Dictionary<int, int[]> result = new Dictionary<int, int[]>
		{
			{ 0, new int[] { 202, 218 } },
			{ 1, new int[] { 172, 220 } },
			{ 2, new int[] { 166, 194 } },
			{ 3, new int[] { 222, 204 } },
			{ 4, new int[] { 182, 211 } },
			{ 5, new int[] { 208, 227 } },
			{ 6, new int[] { 194, 195 } },
			{ 7, new int[] { 206, 150 } },
			{ 8, new int[] { 174, 182 } },
			{ 9, new int[] { 214, 176 } },
			{ 10, new int[] { 168, 199 } },
			{ 11, new int[] { 180, 212 } },
			{ 12, new int[] { 189, 181 } },
			{ 13, new int[] { 227, 180 } },
			{ 14, new int[] { 223, 191 } },
			{ 15, new int[] { 159, 190 } }
		};

		public static void VerifyMatch(Match match)
		{
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
			match.AwayTeam.PinsForPlayer("Mikael Axelsson").ShouldBe(845);
			match.AwayTeam.PinsForPlayer("Hans Norbeck").ShouldBe(814);
			match.AwayTeam.PinsForPlayer("Lars Öberg").ShouldBe(799);
			match.AwayTeam.PinsForPlayer("Torbjörn Jensen").ShouldBe(794);
			match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
			match.AwayTeam.PinsForPlayer("Kjell Jansson").ShouldBe(743);
			match.AwayTeam.PinsForPlayer("Christer Liedholm").ShouldBe(735);
			match.AwayTeam.PinsForPlayer("Kjell Persson").ShouldBe(540);
			match.AwayTeam.PinsForPlayer("Thomas Gurell").ShouldBe(159);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int result1 = result[(i * 4) + j][0];
					int result2 = result[(i * 4) + j][1];

					match.AwayTeam.Series.ElementAt(i).Tables.ElementAt(j).Game1.Pins.ShouldBe(result1);
					match.AwayTeam.Series.ElementAt(i).Tables.ElementAt(j).Game2.Pins.ShouldBe(result2);
				}
			}

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
