namespace Snittlistan.Test
{
	using System;
	using System.Collections.Generic;
	using MvcContrib.TestHelper;
	using Snittlistan.Models;

	public class TestData
	{
		public static Match CreateMatch()
		{
			var match = new Match(
				location: "Sollentuna Bowlinghall",
				date: new DateTime(2011, 03, 26),
				bitsMatchId: 3003231,
				homeTeam: new Team("Sollentuna Bwk", 13),
				awayTeam: new Team("Fredrikshof IF", 6));
			var series = new List<Serie>
				{
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 1,
								Game1 = new Game("Mikael Axelsson", 202),
								Game2 = new Game("Christer Liedholm", 218)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Persson", 172),
								Game2 = new Game("Peter Sjöberg", 220),
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Jansson", 166),
								Game2 = new Game("Hans Norbeck", 194)
							},
							new Table
							{
								Score = 1,
								Game1 = new Game("Lars Öberg", 222),
								Game2 = new Game("Torbjörn Jensen", 204)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 1,
								Game1 = new Game("Lars Öberg", 182),
								Game2 = new Game("Torbjörn Jensen", 211)
							},
							new Table
							{
								Score = 1,
								Game1 = new Game("Kjell Jansson", 208),
								Game2 = new Game("Hans Norbeck", 227)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Persson", 194),
								Game2 = new Game("Peter Sjöberg", 195)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Mikael Axelsson", 206),
								Game2 = new Game("Christer Liedholm", 150)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Persson", 174),
								Game2 = new Game("Peter Sjöberg", 182)
							},
							new Table
							{
								Score = 1,
								Game1 = new Game("Mikael Axelsson", 214),
								Game2 = new Game("Christer Liedholm", 176)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Lars Öberg", 168),
								Game2 = new Game("Torbjörn Jensen", 199)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Jansson", 180),
								Game2 = new Game("Hans Norbeck", 212)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 0,
								Game1 = new Game("Kjell Jansson", 189),
								Game2 = new Game("Hans Norbeck", 181)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Lars Öberg", 227),
								Game2 = new Game("Torbjörn Jensen", 180)
							},
							new Table
							{
								Score = 1,
								Game1 = new Game("Mikael Axelsson", 223),
								Game2 = new Game("Christer Liedholm", 191)
							},
							new Table
							{
								Score = 0,
								Game1 = new Game("Thomas Gurell", 159),
								Game2 = new Game("Peter Sjöberg", 190)
							}
						}
					}
				};
			match.AwayTeam.Series = series;

			return match;
		}

		public static void VerifyMatch(Match match)
		{
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
			match.AwayTeam.PinsForPlayer("Peter Sjöberg").ShouldBe(787);
			match.AwayTeam.Series.Count.ShouldBe(4);
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
