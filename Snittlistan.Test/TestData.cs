namespace Snittlistan.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using MvcContrib.TestHelper;
    using Snittlistan.Models;

    public class TestData
    {
        private static Dictionary<int, int[]> result8x4 = new Dictionary<int, int[]>
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

        private static int[] result4x4 = new int[]
        {
            160, 154, 169, 140, 141, 114, 163, 127, 128, 165, 231, 165, 132, 165, 154, 162
        };

        public static void VerifyTeam(Team8x4 team)
        {
            team.Name.ShouldBe("Fredrikshof IF");
            team.PinsForPlayer("Mikael Axelsson").ShouldBe(845);
            team.PinsForPlayer("Hans Norbeck").ShouldBe(814);
            team.PinsForPlayer("Lars Öberg").ShouldBe(799);
            team.PinsForPlayer("Torbjörn Jensen").ShouldBe(794);
            team.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
            team.PinsForPlayer("Kjell Jansson").ShouldBe(743);
            team.PinsForPlayer("Christer Liedholm").ShouldBe(735);
            team.PinsForPlayer("Kjell Persson").ShouldBe(540);
            team.PinsForPlayer("Thomas Gurell").ShouldBe(159);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int result1 = result8x4[(i * 4) + j][0];
                    int result2 = result8x4[(i * 4) + j][1];

                    team.Series.ElementAt(i).Tables.ElementAt(j).Game1.Pins.ShouldBe(result1);
                    team.Series.ElementAt(i).Tables.ElementAt(j).Game2.Pins.ShouldBe(result2);
                }
            }

            team.Series.Count().ShouldBe(4);
            team.Pins().ShouldBe(6216);
            team.PinsFor(1).ShouldBe(1598);
            team.PinsFor(2).ShouldBe(1573);
            team.PinsFor(3).ShouldBe(1505);
            team.PinsFor(4).ShouldBe(1540);
            team.ScoreFor(1).ShouldBe(2);
            team.ScoreFor(2).ShouldBe(2);
            team.ScoreFor(3).ShouldBe(1);
            team.ScoreFor(4).ShouldBe(1);
            team.Score.ShouldBe(6);
        }

        public static void VerifyTeam(Team4x4 team)
        {
            team.Name.ShouldBe("Fredrikshof C");
            team.PinsForPlayer("Tomas Gustavsson").ShouldBe(561);
            team.PinsForPlayer("Markus Norbeck").ShouldBe(598);
            team.PinsForPlayer("Lars Norbeck").ShouldBe(717);
            team.PinsForPlayer("Matz Classon").ShouldBe(594);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int result = result4x4[(i * 4) + j];

                    team.Series.ElementAt(i).Games.ElementAt(j).Pins.ShouldBe(result);
                }
            }

            team.Series.Count().ShouldBe(4);
            team.Pins().ShouldBe(2470);
            team.PinsFor(1).ShouldBe(623);
            team.PinsFor(2).ShouldBe(545);
            team.PinsFor(3).ShouldBe(689);
            team.PinsFor(4).ShouldBe(613);
            team.ScoreFor(1).ShouldBe(1);
            team.ScoreFor(2).ShouldBe(1);
            team.ScoreFor(3).ShouldBe(2);
            team.ScoreFor(4).ShouldBe(2);
            team.Score.ShouldBe(6);
        }
    }
}