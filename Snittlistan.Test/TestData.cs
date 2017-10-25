using System.Collections.Generic;
using System.Linq;
using Snittlistan.Web.Areas.V1.Models;
using Xunit;

namespace Snittlistan.Test
{
    public static class TestData
    {
        private static readonly Dictionary<int, int[]> Result8X4 = new Dictionary<int, int[]>
        {
            { 0, new[] { 202, 218 } },
            { 1, new[] { 172, 220 } },
            { 2, new[] { 166, 194 } },
            { 3, new[] { 222, 204 } },
            { 4, new[] { 182, 211 } },
            { 5, new[] { 208, 227 } },
            { 6, new[] { 194, 195 } },
            { 7, new[] { 206, 150 } },
            { 8, new[] { 174, 182 } },
            { 9, new[] { 214, 176 } },
            { 10, new[] { 168, 199 } },
            { 11, new[] { 180, 212 } },
            { 12, new[] { 189, 181 } },
            { 13, new[] { 227, 180 } },
            { 14, new[] { 223, 191 } },
            { 15, new[] { 159, 190 } }
        };

        private static readonly int[] Result4X4 = {
            160, 154, 169, 140, 141, 114, 163, 127, 128, 165, 231, 165, 132, 165, 154, 162
        };

        public static void VerifyTeam(Team8x4 team)
        {
            Assert.Equal("Fredrikshof IF", team.Name);
            Assert.Equal(845, team.PinsForPlayer("Mikael Axelsson"));
            Assert.Equal(814, team.PinsForPlayer("Hans Norbeck"));
            Assert.Equal(799, team.PinsForPlayer("Lars Öberg"));
            Assert.Equal(794, team.PinsForPlayer("Torbjörn Jensen"));
            Assert.Equal(787, team.PinsForPlayer("Peter Sjöberg"));
            Assert.Equal(743, team.PinsForPlayer("Kjell Jansson"));
            Assert.Equal(735, team.PinsForPlayer("Christer Liedholm"));
            Assert.Equal(540, team.PinsForPlayer("Kjell Persson"));
            Assert.Equal(159, team.PinsForPlayer("Thomas Gurell"));
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int result1 = Result8X4[(i * 4) + j][0];
                    int result2 = Result8X4[(i * 4) + j][1];

                    Assert.Equal(result1, team.Series.ElementAt(i).Tables.ElementAt(j).Game1.Pins);
                    Assert.Equal(result2, team.Series.ElementAt(i).Tables.ElementAt(j).Game2.Pins);
                }
            }

            Assert.Equal(4, team.Series.Count());
            Assert.Equal(6216, team.Pins());
            Assert.Equal(1598, team.PinsFor(1));
            Assert.Equal(1573, team.PinsFor(2));
            Assert.Equal(1505, team.PinsFor(3));
            Assert.Equal(1540, team.PinsFor(4));
            Assert.Equal(2, team.ScoreFor(1));
            Assert.Equal(2, team.ScoreFor(2));
            Assert.Equal(1, team.ScoreFor(3));
            Assert.Equal(1, team.ScoreFor(4));
            Assert.Equal(6, team.Score);
        }

        public static void VerifyTeam(Team4x4 team)
        {
            Assert.Equal("Fredrikshof C", team.Name);
            Assert.Equal(561, team.PinsForPlayer("Tomas Gustavsson"));
            Assert.Equal(598, team.PinsForPlayer("Markus Norbeck"));
            Assert.Equal(717, team.PinsForPlayer("Lars Norbeck"));
            Assert.Equal(594, team.PinsForPlayer("Matz Classon"));
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int result = Result4X4[(i * 4) + j];

                    Assert.Equal(result, team.Series.ElementAt(i).Games.ElementAt(j).Pins);
                }
            }

            Assert.Equal(4, team.Series.Count());
            Assert.Equal(2470, team.Pins());
            Assert.Equal(623, team.PinsFor(1));
            Assert.Equal(545, team.PinsFor(2));
            Assert.Equal(689, team.PinsFor(3));
            Assert.Equal(613, team.PinsFor(4));
            Assert.Equal(1, team.ScoreFor(1));
            Assert.Equal(1, team.ScoreFor(2));
            Assert.Equal(2, team.ScoreFor(3));
            Assert.Equal(2, team.ScoreFor(4));
            Assert.Equal(6, team.Score);
        }
    }
}