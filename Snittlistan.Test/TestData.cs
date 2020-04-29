using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Snittlistan.Web.Areas.V1.Models;

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
            Assert.That(team.Name, Is.EqualTo("Fredrikshof IF"));
            Assert.That(team.PinsForPlayer("Mikael Axelsson"), Is.EqualTo(845));
            Assert.That(team.PinsForPlayer("Hans Norbeck"), Is.EqualTo(814));
            Assert.That(team.PinsForPlayer("Lars Öberg"), Is.EqualTo(799));
            Assert.That(team.PinsForPlayer("Torbjörn Jensen"), Is.EqualTo(794));
            Assert.That(team.PinsForPlayer("Peter Sjöberg"), Is.EqualTo(787));
            Assert.That(team.PinsForPlayer("Kjell Jansson"), Is.EqualTo(743));
            Assert.That(team.PinsForPlayer("Christer Liedholm"), Is.EqualTo(735));
            Assert.That(team.PinsForPlayer("Kjell Persson"), Is.EqualTo(540));
            Assert.That(team.PinsForPlayer("Thomas Gurell"), Is.EqualTo(159));
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    int result1 = Result8X4[(i * 4) + j][0];
                    int result2 = Result8X4[(i * 4) + j][1];

                    Assert.That(team.Series.ElementAt(i).Tables.ElementAt(j).Game1.Pins, Is.EqualTo(result1));
                    Assert.That(team.Series.ElementAt(i).Tables.ElementAt(j).Game2.Pins, Is.EqualTo(result2));
                }
            }

            Assert.That(team.Series.Count(), Is.EqualTo(4));
            Assert.That(team.Pins(), Is.EqualTo(6216));
            Assert.That(team.PinsFor(1), Is.EqualTo(1598));
            Assert.That(team.PinsFor(2), Is.EqualTo(1573));
            Assert.That(team.PinsFor(3), Is.EqualTo(1505));
            Assert.That(team.PinsFor(4), Is.EqualTo(1540));
            Assert.That(team.ScoreFor(1), Is.EqualTo(2));
            Assert.That(team.ScoreFor(2), Is.EqualTo(2));
            Assert.That(team.ScoreFor(3), Is.EqualTo(1));
            Assert.That(team.ScoreFor(4), Is.EqualTo(1));
            Assert.That(team.Score, Is.EqualTo(6));
        }

        public static void VerifyTeam(Team4x4 team)
        {
            Assert.That(team.Name, Is.EqualTo("Fredrikshof C"));
            Assert.That(team.PinsForPlayer("Tomas Vikbro"), Is.EqualTo(561));
            Assert.That(team.PinsForPlayer("Markus Norbeck"), Is.EqualTo(598));
            Assert.That(team.PinsForPlayer("Lars Norbeck"), Is.EqualTo(717));
            Assert.That(team.PinsForPlayer("Matz Classon"), Is.EqualTo(594));
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    int result = Result4X4[(i * 4) + j];

                    Assert.That(team.Series.ElementAt(i).Games.ElementAt(j).Pins, Is.EqualTo(result));
                }
            }

            Assert.That(team.Series.Count(), Is.EqualTo(4));
            Assert.That(team.Pins(), Is.EqualTo(2470));
            Assert.That(team.PinsFor(1), Is.EqualTo(623));
            Assert.That(team.PinsFor(2), Is.EqualTo(545));
            Assert.That(team.PinsFor(3), Is.EqualTo(689));
            Assert.That(team.PinsFor(4), Is.EqualTo(613));
            Assert.That(team.ScoreFor(1), Is.EqualTo(1));
            Assert.That(team.ScoreFor(2), Is.EqualTo(1));
            Assert.That(team.ScoreFor(3), Is.EqualTo(2));
            Assert.That(team.ScoreFor(4), Is.EqualTo(2));
            Assert.That(team.Score, Is.EqualTo(6));
        }
    }
}